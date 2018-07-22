#include "config.h"
#include "worldserv.h"
#include "player.h"
#include "employee.h"
#include "EmployeeQuestTable.h"
#include "EmployeeQuestSystem.h"

inline float CalcEmployeeQuestSuccessRate(float skLevel, float star, float color, float rate, float monsterSize){
	float r= 0.F;
	if(rate > 0.F){
		r = (skLevel + star + color * color * 0.2F) * (rate / monsterSize);
	}else{
		r = color * color / (monsterSize * 5 );
	}
	return r;
}

PlayerEmployeeQuestTable EmployeeQuestSystem::tables_; 	
PlayerEmployeeQuestList	EmployeeQuestSystem::list_;


inline int64 GetSecond(ACE_Date_Time const& dt){
	tm curTime;
	curTime.tm_year = dt.year();
	curTime.tm_mon = dt.month();
	curTime.tm_mday = dt.day();
	curTime.tm_hour = dt.hour();
	curTime.tm_min = dt.minute();
	curTime.tm_sec = dt.second();
	return ACE_OS::mktime(&curTime);
} 

float EmployeeQuestSystem::CalcSuccessRate(int32 playerId, SGE_EmployeeQuestInst const& questInst){
	Player* player = Player::getPlayerByInstId(playerId);
	if(NULL == player)
		return 0.F;
	EmployeeQuest const* quest = EmployeeQuest::getQuest(questInst.questId_);
	if(NULL == quest)
		return 0.F;
	
	struct EmployeeContext{
		int32 skId_;
		int32 skLevel_;
		int32 star_;
		int32 color_;
	};
	std::vector<EmployeeContext> contexts;
	for(size_t i=0; i<questInst.usedEmployees_.size(); ++i){
		Employee* employee = player->findEmployee(questInst.usedEmployees_[i]);
		if(NULL == employee){
			ACE_DEBUG((LM_ERROR,"EmployeeQuestSystem::CalcSuccessRate [[ player(%d) not haved employee(%d) at employee quest(%d)\n",playerId,questInst.usedEmployees_[i],questInst.questId_));
			continue;
		}
		EmployeeContext c;
		c.star_ = employee->star_;
		c.color_ = employee->color_;
		for(size_t j=0; j<employee->skills_.size(); ++j){
			if(employee->skills_[j] == NULL)
				continue;
			c.skId_ = employee->skills_[j]->skId_;
			c.skLevel_ = employee->skills_[j]->skLevel_;
			contexts.push_back(c);
		}	
	}

	if(contexts.empty()){
		ACE_DEBUG((LM_ERROR,"EmployeeQuestSystem::CalcSuccessRate [[ player(%d) empty employee at employee quest(%d)\n",playerId,questInst.questId_));
		return 0.F;
	}
	
	float maxRate = 0.F;
	
	for(size_t i=0; i<quest->monsters_.size(); ++i){
		EmployeeMonster const* monster = EmployeeMonster::find(quest->monsters_[i]);
		if(NULL == monster){
			continue;
		}
		
		for(size_t k=0; k<monster->skills_.size(); ++k){
			int32 usedIndex = -1;
			float usedRate = 0.F;
			for(size_t j=0; j<contexts.size(); ++j){
				int32 successRate = EmployeeSkill::getRestrain(contexts[j].skId_,monster->skills_[k]);
				float rate = CalcEmployeeQuestSuccessRate(contexts[j].skLevel_,contexts[j].star_,contexts[j].color_,successRate,quest->monsters_.size());
				if(rate > usedRate){
					usedRate = rate;
					usedIndex = j;
				}
			}
			
			if(usedIndex != -1){
				contexts.erase(contexts.begin() + usedIndex);
			}
			maxRate += usedRate;
			
		}
	}
	
	return maxRate + quest->successRate_;
}

SGE_PlayerEmployeeQuest EmployeeQuestSystem::NewPlayerEmployeeQuest(int32 playerId){

	SGE_PlayerEmployeeQuest quest;
	quest.quests_.resize(EQC_Max);
	quest.playerId_ = playerId;
	
	ACE_Date_Time dt;
	dt.update(ACE_Time_Value(WorldServ::instance()->curTime_));
	dt.second(0);
	dt.microsec(0);
	
	{
		std::vector<int32> whiteHours;
		whiteHours.push_back(1);
		whiteHours.push_back(9);
		whiteHours.push_back(17);
		int32 whiteHour = 17;
		for(size_t i=0; i<whiteHours.size(); ++i){
			if(dt.hour() > whiteHours[i]){
				whiteHour = whiteHours[i];
			}
		}
		dt.hour(whiteHour);
		
		std::vector<SGE_EmployeeQuestInst> newQuests;
		Refresh(EQC_White,quest.quests_[EQC_White].value_,newQuests);
		for(size_t i=0; i<newQuests.size() ; ++i){
			newQuests[i].refreshTime_ = GetSecond(dt);
		}
		quest.quests_[EQC_White].value_ = newQuests;
	}

	{
		dt.hour(0);
		std::vector<int32> blueWeeks;
		blueWeeks.push_back(0);
		blueWeeks.push_back(2);
		blueWeeks.push_back(4);
		blueWeeks.push_back(6);
		int32 blueWeek = 0;
		for(size_t i=0; i<blueWeeks.size(); ++i){
			if(dt.weekday() > blueWeeks[i]){
				blueWeek = blueWeeks[i];
			}
		}
		int64 sec = GetSecond(dt);
		blueWeek = dt.weekday()-blueWeek;
		sec -= blueWeek * ONE_DAY_SEC;
		
		
		std::vector<SGE_EmployeeQuestInst> newQuests;
		Refresh(EQC_Blue,quest.quests_[EQC_Blue].value_,newQuests);
		for(size_t i=0; i<newQuests.size() ; ++i){
			newQuests[i].refreshTime_ = sec;
		}
		quest.quests_[EQC_Blue].value_ = newQuests;
	}
	dt.update();
	dt.second(0);
	dt.microsec(0);
	{
		int64 sec = GetSecond(dt);
		int32 wday = dt.weekday() == 0 ? 7 : dt.weekday() -1;
		sec -= wday * ONE_DAY_SEC;
		
		std::vector<SGE_EmployeeQuestInst> newQuests;
		Refresh(EQC_Purple,quest.quests_[EQC_Purple].value_,newQuests);
		for(size_t i=0; i<newQuests.size() ; ++i){
			newQuests[i].refreshTime_ = sec;
		}
		quest.quests_[EQC_Purple].value_ = newQuests;
	}
	return quest;
}

void EmployeeQuestSystem::Refresh(EmployeeQuestColor color, std::vector<SGE_EmployeeQuestInst> const & oldQuests, std::vector<SGE_EmployeeQuestInst> &newQuests){
	static const int SizeLimit[] = {5,1,1};

	std::vector<int32> currQuests = GetQuestIds(oldQuests);
	std::vector<int32> loseQuests;
	UtlMath::subtract(EmployeeQuest::getColors(color),currQuests,loseQuests);
	if(loseQuests.empty())
		return;

	std::random_shuffle(loseQuests.begin(),loseQuests.end());

	if(loseQuests.size() > SizeLimit[color] - currQuests.size()){
		loseQuests.erase(loseQuests.begin() + SizeLimit[color] - currQuests.size() , loseQuests.end());
	}

	for(size_t i=0; i<loseQuests.size(); ++i){
		EmployeeQuest const * pdata = EmployeeQuest::getQuest(loseQuests[i]);
		SGE_EmployeeQuestInst inst;
		inst.questId_ = pdata->id_;
		inst.timeout_ = pdata->timeRequier_;
		inst.refreshTime_ = WorldServ::instance()->curTime_;
		newQuests.push_back(inst);
	}
}


std::vector<int32> EmployeeQuestSystem::GetQuestIds(std::vector<SGE_EmployeeQuestInst> const & quests){
	std::vector<int32> r;
	for(size_t i=0; i<quests.size(); ++i){
		r.push_back(quests[i].questId_);
	}
	return r;
}

bool EmployeeQuestSystem::HasUsedEmployees(int32 playerId, std::vector<int32> const& employees){
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);
	for(size_t i=0; i<employees.size(); ++i){
		if(std::find(questInst->usedEmployees_.begin(),questInst->usedEmployees_.end(),employees[i]) != questInst->usedEmployees_.end()){
			return true; //已经使用过
		}
	}
	return false;
}

bool EmployeeQuestSystem::TryAcceptEmployeeQuest(int32 playerId, int32 questId, std::vector<int32> const& employees, COM_EmployeeQuestInst &out){
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);

	for(size_t i=0; i<employees.size(); ++i){
		if(std::find(questInst->usedEmployees_.begin(),questInst->usedEmployees_.end(),employees[i]) != questInst->usedEmployees_.end()){
			return false; //已经使用过
		}
	}

	for(size_t i=0; i<questInst->quests_.size(); ++i){
		for(size_t j=0; j<questInst->quests_[i].value_.size(); ++j){
			if(questInst->quests_[i].value_[j].questId_ == questId)
			{
				EmployeeQuest const* p = EmployeeQuest::getQuest(questId);
				if(!p)
					return false;
				if(p->employeeRequier_ > employees.size())
					return false;
				questInst->quests_[i].value_[j].status_ = EQS_Running;
				questInst->quests_[i].value_[j].doTime_ = WorldServ::instance()->curTime_;
				questInst->quests_[i].value_[j].timeout_ = p->timeRequier_;
				questInst->quests_[i].value_[j].usedEmployees_ = employees;
				questInst->usedEmployees_.insert(questInst->usedEmployees_.end(),employees.begin(),employees.end());
				out = *(COM_EmployeeQuestInst*)&questInst->quests_[i].value_[j];
				updatePlayerEmployeeQuest(playerId);
				return true;
			}
		}
	}
	return false;
}

bool EmployeeQuestSystem::IsEmployeeQuestComplate(int32 playerId, int32 questId){
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);
	for(size_t i=0; i<questInst->quests_.size(); ++i){
		for(size_t j=0; j<questInst->quests_[i].value_.size(); ++j){
			if(questInst->quests_[i].value_[j].questId_ == questId){
				return questInst->quests_[i].value_[j].status_ == EQS_Complate;
			}
		}
	}
	return false;
}

bool EmployeeQuestSystem::RemoveComplateQuest(int32 playerId, int32 questId){
	Player *player = Player::getPlayerByInstId(playerId);
	if(!player)
		return false;
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);
	for(size_t i=0; i<questInst->quests_.size(); ++i){
		for(size_t j=0; j<questInst->quests_[i].value_.size(); ++j){
			if((questInst->quests_[i].value_[j].questId_ == questId) && (questInst->quests_[i].value_[j].status_ > EQS_Running )){
				
				EmployeeQuest const* quest = EmployeeQuest::getQuest(questId);
				SRV_ASSERT(quest);
				///删除使用的佣兵
				for(size_t k=0; k<questInst->quests_[i].value_[j].usedEmployees_.size(); ++k){
					std::vector<int32>::iterator itr = std::find(questInst->usedEmployees_.begin(),questInst->usedEmployees_.end(),questInst->quests_[i].value_[j].usedEmployees_[k]);
					if(itr!=questInst->usedEmployees_.end()){
						questInst->usedEmployees_.erase(itr);
					}
				}
				
				

				float rate = CalcSuccessRate(playerId,questInst->quests_[i].value_[j]);
				
				bool success = false;//questInst->quests_[i].value_[j].status_ == EQS_Success; ///返回任务成功失败结果
				
				///三个奖励
				for(size_t k=0; k<quest->rewards_.size(); ++k){
					/*if(rate >= rand){
						player->addBagItemByItemId(quest->rewards_[k],1,false,291);
						success = true;
					}else {
						break;
					}
					rate -= 100.F;*/
					if(rate >= (100.F* (k+1) )){
						player->addBagItemByItemId(quest->rewards_[k],1,false,291);
						success = true;
					}else{
						rate = rate - 100.F* k;
						float rand = UtlMath::frandNM(0.F,100.F);
						rate+=5;
						if(rate >= rand){
							player->addBagItemByItemId(quest->rewards_[k],1,false,291);
							success = true;
						}
						break;
					}
				}
				//ACE_DEBUG((LM_DEBUG,ACE_TEXT("EMLPOYEEQUEST RATE[%f] \n"),rate));
				questInst->quests_[i].value_.erase(questInst->quests_[i].value_.begin() + j);
				updatePlayerEmployeeQuest(playerId);
				return success;
			}
		}
	}
	return false;
}

bool EmployeeQuestSystem::IsHasEmployeeQuest(int32 playerId, int32 questId){
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);
	for(size_t i=0; i<questInst->quests_.size(); ++i){
		for(size_t j=0; j<questInst->quests_[i].value_.size(); ++j){
			if(questInst->quests_[i].value_[j].questId_ == questId)
				return true;
		}
	}
	return false;
}

std::vector<COM_EmployeeQuestInst> EmployeeQuestSystem::GetQuestList(int32 playerId){
	std::vector<COM_EmployeeQuestInst> r;
	SGE_PlayerEmployeeQuest * questInst = GetSelfEmployeeQuestInst(playerId);
	if(questInst){
		for(size_t i=0; i<questInst->quests_.size(); ++i){
			for(size_t j=0; j<questInst->quests_[i].value_.size(); ++j){
				r.push_back(*(COM_EmployeeQuestInst*)&questInst->quests_[i].value_[j]);
			}
		}
	}
	else
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("PLAYER[%d] GET EMPLOYEE QUEST LIST IS NULL\n"),playerId));
	return r;
}

SGE_PlayerEmployeeQuest *EmployeeQuestSystem::GetSelfEmployeeQuestInst(int32 playerId){
	if(tables_[playerId] == NULL){
		SGE_PlayerEmployeeQuest newInst = NewPlayerEmployeeQuest(playerId);
		Insert(newInst);
	}
	return tables_[playerId];
}
SGE_PlayerEmployeeQuest *EmployeeQuestSystem::GetEmployeeQuestInst(int32 playerId){
	return tables_[playerId];
}

void EmployeeQuestSystem::Insert(SGE_PlayerEmployeeQuest const& quests){
	list_.push_back(new SGE_PlayerEmployeeQuest(quests));
	tables_[quests.playerId_] = list_.back();
}

void EmployeeQuestSystem::Remove(int32 playerId){
	for(size_t i=0; i<list_.size(); ++i){
		if(list_[i]->playerId_ == playerId){
			delete list_[i];
			list_.erase(list_.begin() + i--);
			tables_[playerId] = NULL;
			DBHandler::instance()->delEmployeeQuest(playerId);
			return;
		}
	}
}

void EmployeeQuestSystem::Refresh(EmployeeQuestColor color){
	for(size_t i=0; i<list_.size(); ++i){
		if(list_[i]->playerId_ == 0){
			ACE_DEBUG((LM_ERROR,"Employee quest player id is 0\n"));
			list_.erase(list_.begin() + i--);
			continue;
		}
		if(list_[i]->quests_.empty()){
			ACE_DEBUG((LM_ERROR,"Employee quest empty player id is %d\n",list_[i]->playerId_));
			list_[i]->quests_.resize(EQC_Max);
		}
		std::vector<SGE_EmployeeQuestInst> newQuests;
		Refresh(color,list_[i]->quests_[color].value_,newQuests);
		if(!newQuests.empty()){
			list_[i]->quests_[color].value_.insert(list_[i]->quests_[color].value_.end(),newQuests.begin(),newQuests.end());
		}
	}
}

void EmployeeQuestSystem::Check(EmployeeQuestColor color){
	static const int32 Timeout[] = {ONE_DAY_SEC * 1, ONE_DAY_SEC * 3 , ONE_DAY_SEC * 7};
	int32 curTime= WorldServ::instance()->curTime_;
	for(size_t i=0; i<list_.size(); ++i){
		std::vector<SGE_EmployeeQuestInst> & curQuests = list_[i]->quests_[color].value_;
		std::vector<int32> delQuests;
		for(size_t j=0; j<curQuests.size(); ++j){
			if(curQuests[j].status_ == EQS_None){ ///正在做的&已经完成的不做检测
				int32 checkTime = curTime - curQuests[j].refreshTime_;
				if(checkTime > Timeout[color]){
					delQuests.push_back(curQuests[j].questId_);
					curQuests.erase(curQuests.begin() + j--);
				}
			}
		}
	}
}

void EmployeeQuestSystem::TickRuning(int32 tickTime){
	for(size_t i=0; i<list_.size(); ++i){
		for(size_t j=0; j<list_[i]->quests_.size();++j){
			std::vector<SGE_EmployeeQuestInst> & curQuests = list_[i]->quests_[j].value_;
			for(size_t k=0; k<curQuests.size(); ++k){
				if(curQuests[k].status_ == EQS_Running){
					curQuests[k].timeout_ -= tickTime;
					if(curQuests[k].timeout_ <= 0){
						curQuests[k].status_ = EQS_Complate;
						updatePlayerEmployeeQuest(list_[i]->playerId_);
					}
				}
			}
		}
	}
}

void EmployeeQuestSystem::PeriodRefresh(U32 playerId,SGE_EmployeeQuestInst & curQuest){
	if(WorldServ::instance()->curTime_ >= curQuest.refreshTime_){
		EmployeeQuest const* quest = EmployeeQuest::getQuest(curQuest.questId_);
		SRV_ASSERT(quest);
		curQuest.refreshTime_ = GetRefreshTime(quest->color_);
		updatePlayerEmployeeQuest(playerId);
	}
}

int64 EmployeeQuestSystem::GetRefreshTime(EmployeeQuestColor color){
	int64 nexttime = 0;

	ACE_Date_Time dt;
	dt.update(ACE_Time_Value(WorldServ::instance()->curTime_));
	dt.second(0);
	dt.microsec(0);

	switch (color)
	{
	case EQC_White:
		{
			std::vector<int32> whiteHours;
			whiteHours.push_back(1);
			whiteHours.push_back(9);
			whiteHours.push_back(17);
			int32 whiteHour = 17;
			for(size_t i=0; i<whiteHours.size(); ++i){
				if(dt.hour() >= whiteHours[i]){
					whiteHour = whiteHours[i];
				}
			}
			dt.hour(whiteHour);
			nexttime = GetSecond(dt);
		}
		break;
	case EQC_Blue:
		{
			std::vector<int32> blueWeeks;
			blueWeeks.push_back(0);
			blueWeeks.push_back(2);
			blueWeeks.push_back(4);
			blueWeeks.push_back(6);
			int32 blueWeek = 0;
			for(size_t i=0; i<blueWeeks.size(); ++i){
				if(dt.weekday() > blueWeeks[i]){
					blueWeek = blueWeeks[i];
				}
			}
			int64 sec = GetSecond(dt);
			blueWeek = dt.weekday()-blueWeek;
			sec -= blueWeek * ONE_DAY_SEC;

			nexttime = sec;
		}	
		break;
	case EQC_Purple:
		{
			int64 sec = GetSecond(dt);
			int32 wday = dt.weekday() == 0 ? 7 : dt.weekday() -1;
			sec -= wday * ONE_DAY_SEC;

			nexttime = sec;
		}
		break;
	default:
		break;
	}
	return nexttime;
}

void EmployeeQuestSystem::initEmployeeQuest(std::vector<SGE_PlayerEmployeeQuest> info){
	for(size_t i=0; i<info.size(); ++i)
	{
		for (size_t j=0;j< info[i].quests_.size();++j)
		{
			std::vector<SGE_EmployeeQuestInst> & curQuests =  info[i].quests_[j].value_;
			for(size_t k=0; k<curQuests.size(); ++k){
				if(curQuests[k].status_ == EQS_Running){
					int32 curTime= WorldServ::instance()->curTime_;
					curQuests[k].timeout_ -= (curTime - curQuests[k].doTime_);
				}
			}
		}
		list_.push_back(new SGE_PlayerEmployeeQuest( info[i]));	
		tables_[info[i].playerId_] = list_.back();
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("PLAYER[%d] FETCH EMPLOYEE QUEST SIZE[%d] \n"),info[i].playerId_,info[i].quests_.size()));
	}
}

void EmployeeQuestSystem::updatePlayerEmployeeQuest(U32 playerID){
	SGE_PlayerEmployeeQuest * questInst = GetEmployeeQuestInst(playerID);
	if(questInst == NULL)
		return;
	DBHandler::instance()->insertEmployeeQuest(playerID,*questInst);
}