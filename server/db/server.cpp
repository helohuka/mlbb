#include "config.h"
#include "ComEnv.h"
#include "ComGlobal.h"
#include "ComScriptEvn.h"

#include "handler.h"
#include "server.h"
#include "routine.h"

std::string dbAddr_;		///<mysql	地址
std::string dbName_;		///<mysql	数据库名
std::string dbUser_;		///<mysql	数据库用户名
std::string dbPwd_;			///<mysql	密码

int 
Server::init (int argc, ACE_TCHAR *argv[])
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init DB serv... \n")));
	
	///读取LUA
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin load lua...\n")));

	ScriptEnv::init();

#include "ComScriptRegster.h"
#include "ComScriptApi.h"
	std::string err;
	if( !ScriptEnv::loadFile("env.lua", err))
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("load env.lua failed:%s\n"), err.c_str()));
		SRV_ASSERT(0);
	}
	ACE_Connector<WorldHandler, ACE_SOCK_CONNECTOR > connector;
	ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenDB));
	WorldHandler *p = WorldHandler::instance();
	connector.reactor(reactor());
	if(connector.connect(p,addr) == -1)
	{
		SRV_ASSERT(0);
	}

	dbAddr_  = Env::get<const char*>(V_MysqlHost);
	dbName_	 = Env::get<const char*>(V_DatabaseName);
	dbUser_	 = Env::get<const char*>(V_MysqlUser);
	dbPwd_	 = Env::get<const char*>(V_MysqlPassword);
	SQLTask::sinit();

	initData();

	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, ACE_Time_Value(0), t);

	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin accept world server...\n")));
	return 0;
}

int 
Server::fini (void)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini DB serv... ")));

	// 退出计时器.
	this->reactor()->cancel_timer(this);

	SQLTask::sfini();
	return 0;
}

int 
Server::handle_timeout (const ACE_Time_Value &current_time, const void *act)
{
	SQLTask::doback();
	if(!contactCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync player infos \n"));
		
		enum{ INFO_SIZE = 200 };
		std::vector<SGE_ContactInfoExt> info;
		for(int i=0; i<INFO_SIZE && !contactCache_.empty(); ++i)
		{
			info.push_back(contactCache_.back());
			contactCache_.pop_back();
		}
		WorldHandler::instance()->syncContactInfo(info);
	}
	if(maxGuid_ != 0){
		WorldHandler::instance()->syncGlobalGuid(maxGuid_);
		maxGuid_ = 0;
	}
	
	static U64 oldTime = current_time.get_msec();
	U64 currTime = current_time.get_msec() ;
	float delta =  (currTime - oldTime) / 1000.F ;
	oldTime = currTime;

	{ ///监测内存
		enum{
			CHECK_INTERVAL = 300
		};

		static float check_interval = CHECK_INTERVAL;
		check_interval -= delta;

		if (check_interval < 0){
			ACE_DEBUG((LM_INFO,"Baby Cache Length = %d , Index Length = %d\n",playerBabyCache_.size(),idBabyCache_.size()));
			ACE_DEBUG((LM_INFO,"Employee Cache Length = %d Index Length = %d\n",playerEmployeeCache_.size(),idEmployeeCache_.size()));
			check_interval = CHECK_INTERVAL;
		}

	}



	initRank();
	SQLTask::ping();
	return 0;
}

void
Server::initData()
{
	InitSQLTables *pInitSQLTables = NULL;
	Routine::create(pInitSQLTables);
	SQLTask::sglob(pInitSQLTables);

	QueryPlayerContact* pQueryPlayerContact = NULL;
	Routine::create(pQueryPlayerContact);
	SQLTask::sglob(pQueryPlayerContact);
	
	QueryBabyCache *pQueryBabyCache = NULL;
	Routine::create(pQueryBabyCache);
	SQLTask::sglob(pQueryBabyCache);

	QueryEmployeeCache *pQueryEmployeeCache = NULL;
	Routine::create(pQueryEmployeeCache);
	SQLTask::sglob(pQueryEmployeeCache);
	//帮派
	FetchGuild* pGuild = NULL;
	Routine::create(pGuild);
	SQLTask::sglob(pGuild);

	FetchGuildMember* pGuildMembers = NULL;
	Routine::create(pGuildMembers);
	SQLTask::sglob(pGuildMembers);

	//运营活动
	FetchActivity * fa = NULL;
	Routine::create(fa);
	SQLTask::sglob(fa);

///@排行榜
///@{
	QueryEndlessAllDate *pqueryEndlessAllDate = NULL;
	Routine::create(pqueryEndlessAllDate);
	SQLTask::sglob(pqueryEndlessAllDate);

	QueryBabyByFF* babyrank = NULL;
	Routine::create(babyrank);
	SQLTask::sglob(babyrank);

	QueryEmployeeByFF* employeeFFrank = NULL;
	Routine::create(employeeFFrank);
	SQLTask::sglob(employeeFFrank);

	QueryPlayerByLevel* playerlevelrank = NULL;
	Routine::create(playerlevelrank);
	SQLTask::sglob(playerlevelrank);
	
	QueryPlayerByFF* playerffrank = NULL;
	Routine::create(playerffrank);
	SQLTask::sglob(playerffrank);
///}
	//伙伴任务
	FetchEmployeeQuest* quest = NULL;
	Routine::create(quest);
	SQLTask::sglob(quest);
}

void Server::initRank(){
	if(!babyFFrankCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync Baby FF Rank infos \n"));
		enum{ INFO_SIZE = 200 };
		std::vector<COM_BabyRankData> info;
		for(int i=0; i<INFO_SIZE && !babyFFrankCache_.empty(); ++i)
		{
			info.push_back(babyFFrankCache_.back());
			babyFFrankCache_.pop_back();
		}
		WorldHandler::instance()->queryBabyByFFOK(info);
	}

	if(!employeeFFrankCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync Employee FF Rank infos \n"));
		enum{ INFO_SIZE = 200 };
		std::vector<COM_EmployeeRankData> info;
		for(int i=0; i<INFO_SIZE && !employeeFFrankCache_.empty(); ++i)
		{
			info.push_back(employeeFFrankCache_.back());
			employeeFFrankCache_.pop_back();
		}
		WorldHandler::instance()->queryEmployeeByFFOK(info);
	}

	if(!playerFFrankCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync Player FF Rank infos \n"));
		enum{ INFO_SIZE = 200 };
		std::vector<COM_ContactInfo> info;
		for(int i=0; i<INFO_SIZE && !playerFFrankCache_.empty(); ++i)
		{
			info.push_back(playerFFrankCache_.back());
			playerFFrankCache_.pop_back();
		}
		WorldHandler::instance()->queryPlayerByFFOK(info);
	}

	if(!playerlevelrankCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync Player Level Rank infos \n"));
		enum{ INFO_SIZE = 200 };
		std::vector<COM_ContactInfo> info;
		for(int i=0; i<INFO_SIZE && !playerlevelrankCache_.empty(); ++i)
		{
			info.push_back(playerlevelrankCache_.back());
			playerlevelrankCache_.pop_back();
		}
		WorldHandler::instance()->queryPlayerByLevelOK(info);
	}

	if(!employeeQuestCache_.empty())
	{
		ACE_DEBUG((LM_INFO,"Sync Employee Quest infos \n"));
		enum{ INFO_SIZE = 200 };
		std::vector<SGE_PlayerEmployeeQuest> info;
		for(int i=0; i<INFO_SIZE && !employeeQuestCache_.empty(); ++i)
		{
			info.push_back(employeeQuestCache_.back());
			employeeQuestCache_.pop_back();
		}
		WorldHandler::instance()->syncEmployeeQuest(info);
	}
}

void Server::addBabyInst(COM_BabyInst &inst){
	COM_BabyInst *p = new COM_BabyInst(inst);
	idBabyCache_[p->instId_] = p;
	playerBabyCache_[p->ownerName_].push_back(p);
}
void Server::delBabyInst(U32 instId){
	std::map<U32,COM_BabyInst*>::iterator mItr = idBabyCache_.find(instId);
	if(mItr == idBabyCache_.end())
		return;
	COM_BabyInst*p = mItr->second;
	idBabyCache_.erase(mItr);
	if(NULL == p)
		return;
	std::vector<COM_BabyInst*> &li = playerBabyCache_[p->ownerName_];
	std::vector<COM_BabyInst*>::iterator itr = std::find(li.begin(),li.end(),p);
	if(itr != li.end()){
		li.erase(itr);
	}
	delete p;
}
void Server::updateBabyInst(COM_BabyInst &inst){
	std::map<U32,COM_BabyInst*>::iterator mItr = idBabyCache_.find(inst.instId_);
	if(mItr == idBabyCache_.end())
		return;
	COM_BabyInst*p = mItr->second;
	if(NULL == p)
		return;
	*p = inst;
}

bool Server::getBabyInst(U32 instId, COM_BabyInst &inst){
	std::map<U32,COM_BabyInst*>::iterator mItr = idBabyCache_.find(inst.instId_);
	if(mItr == idBabyCache_.end())
		return false;
	COM_BabyInst*p = mItr->second;
	if(NULL == p)
		return false;
	 inst = *p;
	return true;
}

void Server::getPlayerBabyList(std::string& playerName, std::vector<COM_BabyInst>& out){
	if(playerBabyCache_.find(playerName) == playerBabyCache_.end())
		return;
	std::vector<COM_BabyInst*> &li = playerBabyCache_[playerName];
	for(size_t i=0; i<li.size(); ++i){
		out.push_back(*li[i]);
	}
}

void Server::addEmployeeInst(COM_EmployeeInst &inst){
	COM_EmployeeInst *p = new COM_EmployeeInst(inst);
	idEmployeeCache_[p->instId_] = p;
	playerEmployeeCache_[p->ownerName_].push_back(p);
}
void Server::delEmployeeInst(U32 instId){
	std::map<U32,COM_EmployeeInst*>::iterator mItr = idEmployeeCache_.find(instId);
	if(mItr == idEmployeeCache_.end())
		return;
	COM_EmployeeInst*p = mItr->second;
	idEmployeeCache_.erase(mItr);
	if(NULL == p)
		return;
	std::vector<COM_EmployeeInst*> &li = playerEmployeeCache_[p->ownerName_];
	std::vector<COM_EmployeeInst*>::iterator itr = std::find(li.begin(),li.end(),p);
	if(itr != li.end()){
		li.erase(itr);
	}
	delete p;
}
void Server::updateEmployeeInst(COM_EmployeeInst &inst){
	std::map<U32,COM_EmployeeInst*>::iterator mItr = idEmployeeCache_.find(inst.instId_);
	if(mItr == idEmployeeCache_.end())
		return;
	COM_EmployeeInst*p = mItr->second;
	if(NULL == p)
		return;
	*p = inst;
}

bool Server::getEmployeeInst(U32 instId, COM_EmployeeInst &inst){
	std::map<U32,COM_EmployeeInst*>::iterator mItr = idEmployeeCache_.find(inst.instId_);
	if(mItr == idEmployeeCache_.end())
		return false;
	COM_EmployeeInst*p = mItr->second;
	if(NULL == p)
		return false;
	 inst = *p;
	return true;
}

void Server::getPlayerEmployeeList(std::string& playerName, std::vector<COM_EmployeeInst>& out){
	if(playerEmployeeCache_.find(playerName) == playerEmployeeCache_.end())
		return;
	std::vector<COM_EmployeeInst*> &li = playerEmployeeCache_[playerName];
	for(size_t i=0; i<li.size(); ++i){
		out.push_back(*li[i]);
	}
}