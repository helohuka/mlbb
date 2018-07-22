#include "config.h"
#include "battle.h"
#include "player.h"
#include "itemtable.h"
#include "Scene.h"
#include "monster.h"
#include "employee.h"
#include "Robot.h"
#include "GameEvent.h"
#include "robotTable.h"
#include "pvpJJC.h"


void 
Battle::checkCleaner(COM_BattleOverClearing& ceaing){
	/*std::stringstream fn;
	fn << logdir_ << "/" << opentime_ <<"(" << id_ << "_" << battleDataId_ << ")" << (int)battleType_ << ".json";
	std::ofstream fs;
	fs.open(fn.str().c_str(),std::ios::app);
	fs << "---------------------------------OVERCLEARING----------------------------" << std::endl;
	fs << "-------------------------------------------------------------------------" << std::endl;
	fs << "-------------------------------------------------------------------------" << std::endl;

	ceaing.serializeJson(fs);
	fs.flush();
	fs.close();*/
}

void
Battle::checkInit(COM_InitBattle&battle)
{
	/*bool hasPlayer = false;
	for(size_t i=0; i<entities_.size(); ++i)
	{
		if(entities_[i]->asPlayer())
			hasPlayer = true;
	}
	if(!hasPlayer){
		ACE_DEBUG((LM_ERROR,"Check init can not find player %d %d\n",id_,battleType_));
		for(size_t i=0; i<entities_.size(); ++i)
			entities_[i]->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild)); 
		
		cleanBattle();
		return;
	}
	std::stringstream fn;
	fn << logdir_ << "/" << opentime_ <<"(" << id_ << "_" << battleDataId_ << ")" << (int)battleType_ << ".json";
	std::ofstream fs;
	fs.open(fn.str().c_str(),std::ios::app);
	fs << "-------------------------------------------------------------------------" << std::endl;
	fs << "-------------------------------------------------------------------------" << std::endl;
	fs << "-------------------------------------INIT--------------------------------" << std::endl;
	battle.serializeJson(fs);
	fs.flush();
	fs.close();*/

	
}	


void 
Battle::checkReport()
{
	/*std::stringstream fn;
	fn << logdir_ << "/" << opentime_ <<"(" << id_ << "_" << battleDataId_ << ")" << (int)battleType_ << ".json";
	std::ofstream fs;
	fs.open(fn.str().c_str(),std::ios::app);
	fs << "-----------------------------------REPORT------------------------------" << std::endl;
	roundReport_.serializeJson(fs);
	fs.flush();
	fs.close();*/
}

void 
Battle::checkEntity()
{
	//return; /// any used 
	//ACE_DEBUG((LM_DEBUG,"{{{{{{{{{{{{POSITION{{{{{{{{{{\n"));
	//for(size_t i=0; i<entities_.size(); ++i)
	//{
	//	ACE_DEBUG((LM_DEBUG,"  entity(%d) : position(%d) \n" ,entities_[i]->getGUID(),(S32)entities_[i]->battlePosition_));

	//	GroupType gt = GT_None;
	//	BattlePosition bp = (BattlePosition)(S32)entities_[i]->battlePosition_;
	//	if( bp >= BP_Down0 && bp <= BP_Down9 )
	//		gt = GT_Down;
	//	else if( bp >= BP_Up0 && bp <= BP_Up9 )
	//		gt = GT_Up;

	//	SRV_ASSERT(gt != GT_None);
	//	//SRV_ASSERT(gt == entities_[i]->battleForce_);
	//}
	//ACE_DEBUG((LM_DEBUG,"}}}}}}}}}}POSITION}}}}}}}}}}\n"));


}

void
Battle::checkOrder()
{
	/*std::stringstream fn;
	fn << logdir_ << "/" << opentime_ <<"(" << id_ << "_" << battleDataId_ << ")" << (int)battleType_ << ".json";
	std::ofstream fs;
	fs.open(fn.str().c_str(),std::ios::app);
	fs << "-------------------------------------ORDER--------------------------------" << std::endl;
	RAList& ral = actions();
	for(size_t i=0; i<ral.size(); ++i){
		ral[i].serializeJson(fs);
	}
	fs.flush();
	fs.close();*/
}

void
Battle::checkSameGuid(){
	std::vector<U32> sames;
	for(size_t i=0;i<entities_.size() -1; ++i){
		for (size_t j=i + 1;j<entities_.size(); ++j){
			if(entities_[i]->getGUID() == entities_[j]->getGUID()){
				sames.push_back(entities_[j]->getGUID());
				ACE_DEBUG((LM_INFO,"Battle check id same %d %d\n",id_,entities_[j]->getGUID()));
			}
		}
	}
	
	if(sames.empty())
		return;

	for(size_t i=0;i<entities_.size(); ++i){
		if(std::find(sames.begin(),sames.end(),entities_[i]->getGUID()) != sames.end()){
			entities_[i]->cleanBattleStatus();
			entities_.erase(entities_.begin() + i--);
		}	
	}
	
}

void 
Battle::logJoinBattle(){
	std::stringstream ss;
	for(size_t i=0; i<entities_.size(); ++i){
		if(entities_[i]->asPlayer()){
			ss << "Player[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << "] ";
		}
		else if(entities_[i]->asBaby()){
			ss << "Baby[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << "] ";
		}
		else if(entities_[i]->asEmployee()){
			ss << "Employee[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << "] ";
		}
		else if(entities_[i]->asMonster()){
			ss << "Monster[" << entities_[i]->getNameC() << "] ";
		}
		else if(entities_[i]->asRobot()){
			ss << "Robot[" << entities_[i]->getGUID() << "] ";
		}
	}
	ACE_DEBUG((LM_INFO,"Join battle %s\n",ss.str().c_str()));
}
void 
Battle::logExitBattle(){
	std::stringstream ss;
	for(size_t i=0; i<entities_.size(); ++i){
		if(entities_[i]->asPlayer()){
			ss << "Player[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << ":" << entities_[i]->getProp(PT_HpCurr) <<"] ";
		}
		else if(entities_[i]->asBaby()){
			ss << "Baby[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << ":" << entities_[i]->getProp(PT_HpCurr) <<"] ";
		}
		else if(entities_[i]->asEmployee()){
			ss << "Employee[" << entities_[i]->getNameC() << ":" << entities_[i]->getGUID() << ":" << entities_[i]->getProp(PT_HpCurr) <<"] ";
		}
		else if(entities_[i]->asMonster()){
			ss << "Monster[" << entities_[i]->getNameC() <<  ":" << entities_[i]->getProp(PT_HpCurr) <<"] ";
		}
		else if(entities_[i]->asRobot()){
			ss << "Robot[" << entities_[i]->getGUID() << ":" << entities_[i]->getProp(PT_HpCurr) <<"] ";
		}
	}
	ACE_DEBUG((LM_INFO,"Exit battle %s\n",ss.str().c_str()));
}