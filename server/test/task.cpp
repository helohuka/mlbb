#include "action.h"
#include "config.h"
#include "task.h"
#include "robotTable.h"

CaseTask::CaseTask(std::vector<TestCase*> cases):cases_(cases){
}

CaseTask::~CaseTask(){
	for(int i=0; i<cases_.size(); ++i)
	{
		if(cases_[i])
			delete cases_[i];
	}
	cases_.clear();
}

int CaseTask::start(){
	activate(THR_NEW_LWP | THR_JOINABLE | THR_INHERIT_SCHED , 1 );
	return 0;
}

int CaseTask::stop(){
	ACE_Message_Block* mb = new ACE_Message_Block();
	putq( mb );
	return 0;
}

int CaseTask::svc(void){
	enum{
		FPS_INTERVAL = 500
	};

	lastTime_	= ACE_OS::gettimeofday().get_msec();
	do 
	{
		uint64 interval = ACE_OS::gettimeofday().get_msec() - lastTime_;
		if(interval > FPS_INTERVAL){
			lastTime_ += interval; 
			ACE_Time_Value tv(0);
			ACE_Message_Block *mb = NULL;
			if(-1!=getq(mb,&tv) && mb->length() == 0){
				mb->release();
				break;
			}
			for(size_t i=0; i<cases_.size(); ++i){
				cases_[i]->update(interval * 0.001F);
				cases_[i]->updateAction(interval * 0.001F);
				checkCaseTask(cases_[i]);
			}
		}
		
		ACE_OS::sleep(1);
	} while (true);
	return 0;
}

void CaseTaskFactory::init(){
	int n = Env::get<int>(V_TestTaskSize);
	int m = Env::get<int>(V_TestCaseSize);
	int indx = UtlMath::rand();
	for( int j = 0; j < n ; ++j){
		std::vector<TestCase*> cases;
		for(int i=0; i<m; ++i)
			cases.push_back(new TestCase(indx++));
		tasks_.push_back(new CaseTask(cases));
		cases.clear();
	}
}

void CaseTaskFactory::fini(){
	for(size_t i=0; i<tasks_.size(); ++i){
		tasks_[i]->stop();
		delete tasks_[i];
	}
	tasks_.clear();
}

void CaseTaskFactory::run(){
	for(size_t i=0; i<tasks_.size(); ++i){
		tasks_[i]->start();
	}
}

void CaseTaskFactory::initRobot(){
	int indx = UtlMath::rand();
	std::vector<TestCase*> cases;

	for (size_t i = 0; i < RobotActionTable::actiondata_.size(); ++i)
	{
		RobotActionTable::RobotActionData* pRobotData = RobotActionTable::actiondata_[i];
		if(pRobotData == NULL)
			continue;
		TestCase* ptest = new TestCase(indx++,pRobotData->userName_,pRobotData->robotName_);
		ptest->setCaseAction(pRobotData->actionType_);
		if(pRobotData->actionType_ == RAT_TeamMove)
			Action::makeCreateTeamActions(ptest);
		Action::makeConnectActions(ptest);
		cases.push_back(ptest);
	}
	
	tasks_.push_back(new CaseTask(cases));
	cases.clear();
}

void CaseTask::checkCaseTask(TestCase* pcase){
	if(pcase == NULL)
		return;
	if(!pcase->actions_.empty()){
		return;
	}
	pcase->actions_.clear();
	RobotActionTable::RobotActionData const* pdata = RobotActionTable::getActionData(pcase->username_);
	if(pdata == NULL)
		return;
	pcase->setCaseAction(pdata->actionType_);
}