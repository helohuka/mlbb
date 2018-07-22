#include "pvpJJC.h"
#include "player.h"
#include "team.h"
#include "battle.h"
#include "worldserv.h"
#include "PVPrunkTable.h"
#include "DropTable.h"
#include "Activity.h"
#include "GameEvent.h"

std::vector<Team*>	PvpJJC::teamstore_;
std::vector<Player*> PvpJJC::playerstore_;

Team*
PvpJJC::findParticipantTeam(U32 teamId)
{
	for (size_t i = 0; i < teamstore_.size(); ++i)
	{
		if(teamstore_[i]->teamId_ == teamId)
			return teamstore_[i];
	}

	return NULL;
}

bool
PvpJJC::addParticipant(Team* p)
{
	/*if(findParticipantTeam(p->teamId_))
		return false;

	teamstore_.push_back(p);*/

	return true;
}

bool
PvpJJC::delParticipant(U32 teamId)
{
	for (size_t i = 0; i < teamstore_.size(); ++i)
	{
		if(teamstore_[i]->teamId_ == teamId)
		{
			teamstore_[i]->teamJJCTimes_ = 0;
			teamstore_.erase(teamstore_.begin() + i);
			return true;
		}
	}

	return false;
}

//---

void
PvpJJC::tick(float dt)
{
	/*for (size_t i = 0; i < teamstore_.size(); ++i)
	{
		checkEnemy(teamstore_[i],dt);
	}

	for (size_t i = 0; i < playerstore_.size(); ++i)
	{
		checkPlayerEnemy(playerstore_[i],dt);
	}*/
	Warriorchoose::instance()->tick(dt);
}

U32
PvpJJC::calcMeanSection(Team* p)
{
	U32 sectAll = 0;
	U32	sect = 0;

	if(p == NULL || p->teamMembers_.size() <= 0)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("PVPJJC calcMeanSection Team Error\n")));
		return 1000;
	}

	for (size_t i = 0; i < p->teamMembers_.size(); ++i)
	{
		if(p->teamMembers_[i] == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("PVPJJC calcMeanSection p->teamMembers_[i] is null..[%d]\n"),i));
			continue;
		}
		sectAll += p->teamMembers_[i]->pvpInfo_.section_;
	}

	sect = sectAll/p->teamMembers_.size();

	return sect;
}

void
PvpJJC::startMatching(Team* p)
{
	if(p == NULL)
		return;
	for (int32 i = 0; i < p->getMemberSize(); ++i)
	{
		if(p->teamMembers_[i]->getProp(PT_Level) < Global::get<int>(C_JJCOpenlevel))
		{
			Player* player = p->getLeader();
			if(player == NULL)
				return;
			CALL_CLIENT(player,errorno(EN_NoSubSyste));
			return;
		}
		if(p->teamMembers_[i]->isLeavingTeam_)
		{
			Player* player = p->getLeader();
			if(player == NULL)
				return;
			CALL_CLIENT(player,errorno(EN_TeamMemberLeaving));
			return;
		}
	}

	addParticipant(p);
	p->startMatchingOK();
}

void
PvpJJC::stopMatching(Team* p)
{
	if(p == NULL)
		return;
	p->stopMatchingOK(p->teamJJCTimes_);
	delParticipant(p->teamId_);
}

void
PvpJJC::checkEnemy(Team* p, float dt)
{
	if(p == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("checkEnemy Team is Null !!! \n")));
		SRV_ASSERT(p);
	}

	p->teamJJCTimes_ += dt;
	
	S32 sect = calcMeanSection(p);

	if(p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime1))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			if(teamstore_[i]->getMemberSize() == p->getMemberSize() && calcMeanSection(teamstore_[i]) == sect)
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);

				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime1) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime2))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;

			if(teamstore_[i]->getMemberSize() == p->getMemberSize() && (sect -1 <= calcMeanSection(teamstore_[i]) && calcMeanSection(teamstore_[i]) <= sect + 1))
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);

				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime2) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime3))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			if( (p->getMemberSize() - 1 <= teamstore_[i]->getMemberSize() && teamstore_[i]->getMemberSize() <= p->getMemberSize()+1)  && (sect -1 <= calcMeanSection(teamstore_[i]) && calcMeanSection(teamstore_[i]) <= sect + 1) )
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);

				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime3) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime4))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			if( (p->getMemberSize() - 1 <= teamstore_[i]->getMemberSize() && teamstore_[i]->getMemberSize() <= p->getMemberSize()+1)  && (sect -2 <= calcMeanSection(teamstore_[i]) && calcMeanSection(teamstore_[i]) <= sect + 2) )
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);

				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime4) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime5))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			if( (p->getMemberSize() - 2 <= teamstore_[i]->getMemberSize() && teamstore_[i]->getMemberSize() <= p->getMemberSize()+2)  && (sect -2 <= calcMeanSection(teamstore_[i]) && calcMeanSection(teamstore_[i]) <= sect + 2) )
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);

				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime5) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime6))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if( sect -3 <= playerstore_[i]->pvpInfo_.section_ && playerstore_[i]->pvpInfo_.section_ <= sect + 3 )
			{
				syncEnemyMsg(playerstore_[i],p);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delSingleParticipant(playerstore_[i]->getGUID());

				return;
			}
		}
	}
	else
	{
		stopMatching(p);
		return;
	}
}

void
PvpJJC::exitTeamPvpJJC(Team* p)
{
	if(p == NULL)
		return;
	p->exitPvpJJCOk();
	delParticipant(p->teamId_);
}

//------单人------
Player*
PvpJJC::findParticipantPlayer(U32 instId)
{
	for (size_t i = 0; i < playerstore_.size(); ++i)
	{
		if(playerstore_[i]->playerId_ == instId)
			return playerstore_[i];
	}

	return NULL;
}

bool
PvpJJC::addSingleParticipant(Player* p)
{
	/*if(findParticipantPlayer(p->getGUID()))
		return false;
	p->rivalWaitTimes_ = 0;
	playerstore_.push_back(p);*/
	return true;
}

bool
PvpJJC::delSingleParticipant(U32 instId)
{
	for (size_t i = 0; i < playerstore_.size(); ++i)
	{
		if(playerstore_[i]->getGUID() == instId)
		{
			playerstore_[i]->rivalWaitTimes_ = 0;
			playerstore_.erase(playerstore_.begin() + i);
			return true;
		}
	}

	return false;
}

void
PvpJJC::startMatching(Player* p)
{
	if(p == NULL)
		return;
	if(p->getProp(PT_Level) < Global::get<int>(C_JJCOpenlevel))
		return;
	addSingleParticipant(p);

	CALL_CLIENT(p,startMatchingOK());
}

void
PvpJJC::stopMatching(Player* p)
{
	if(p == NULL)
		return;
	
	CALL_CLIENT(p,stopMatchingOK(p->rivalWaitTimes_));

	delSingleParticipant(p->getGUID());
}

void
PvpJJC::checkPlayerEnemy(Player* p, float dt)
{
	if(p == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("checkEnemy Team is Null !!! \n")));
		SRV_ASSERT(p);
	}

	p->rivalWaitTimes_ += dt;

	S32 sect = p->pvpInfo_.section_;

	if(p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime1))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if(playerstore_[i]->getGUID() == p->getGUID())
				continue;
			if(playerstore_[i]->pvpInfo_.section_ == sect)
			{
				syncEnemyMsg(p,playerstore_[i]);

				Player* ptmp = playerstore_[i];
				delSingleParticipant(p->getGUID());
				delSingleParticipant(ptmp->getGUID());

				return;
			}
		}
	}
	else if(p->rivalWaitTimes_ > Global::get<float>(C_PVPJJCMeanTime1) && p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime2))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if(playerstore_[i]->getGUID() == p->getGUID())
				continue;

			if(sect -1 <= playerstore_[i]->pvpInfo_.section_ && playerstore_[i]->pvpInfo_.section_ <= sect + 1)
			{
				syncEnemyMsg(p,playerstore_[i]);

				Player* ptmp = playerstore_[i];
				delSingleParticipant(p->getGUID());
				delSingleParticipant(ptmp->getGUID());

				return;
			}
		}
	}
	else if(p->rivalWaitTimes_ > Global::get<float>(C_PVPJJCMeanTime2) && p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime3))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if(playerstore_[i]->getGUID() == p->getGUID())
				continue;
			if( (sect -1 <= playerstore_[i]->pvpInfo_.section_ && playerstore_[i]->pvpInfo_.section_ <= sect + 1) )
			{
				syncEnemyMsg(p,playerstore_[i]);

				Player* ptmp = playerstore_[i];
				delSingleParticipant(p->getGUID());
				delSingleParticipant(ptmp->getGUID());

				return;
			}
		}
	}
	else if(p->rivalWaitTimes_ > Global::get<float>(C_PVPJJCMeanTime3) && p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime4))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if(playerstore_[i]->getGUID() == p->getGUID())
				continue;
			if( (sect -2 <=  playerstore_[i]->pvpInfo_.section_ &&  playerstore_[i]->pvpInfo_.section_ <= sect + 2) )
			{
				syncEnemyMsg(p,playerstore_[i]);

				Player* ptmp = playerstore_[i];
				delSingleParticipant(p->getGUID());
				delSingleParticipant(ptmp->getGUID());

				return;
			}
		}
	}
	else if(p->rivalWaitTimes_ > Global::get<float>(C_PVPJJCMeanTime4) && p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime5))
	{
		for (size_t i = 0; i < playerstore_.size(); ++i)
		{
			if(playerstore_[i]->getGUID() == p->getGUID())
				continue;
			if( (sect -2 <= playerstore_[i]->pvpInfo_.section_ && playerstore_[i]->pvpInfo_.section_ <= sect + 2) )
			{
				syncEnemyMsg(p,playerstore_[i]);

				Player* ptmp = playerstore_[i];
				delSingleParticipant(p->getGUID());
				delSingleParticipant(ptmp->getGUID());

				return;
			}
		}
	}
	else if(p->rivalWaitTimes_ > Global::get<float>(C_PVPJJCMeanTime5) && p->rivalWaitTimes_ <= Global::get<float>(C_PVPJJCMeanTime6))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			U16 teamSize = teamstore_[i]->getMemberSize();
			U16 teamScetion = calcMeanSection(teamstore_[i]);
			S8 minScetion = p->pvpInfo_.section_ -3;
			S8 maxScetion = p->pvpInfo_.section_ + 3;
			if( (teamSize<=3 && (minScetion <= teamScetion && teamScetion <= maxScetion) ))
			{
				syncEnemyMsg(p,teamstore_[i]);

				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delSingleParticipant(p->getGUID());
				break;
			}
		}
	}
	else
	{
		stopMatching(p);
		return;
	}
}

void
PvpJJC::exitSingleJJC(U32 instId)
{
	Player* p = Player::getPlayerByInstId(instId);
	if(p == NULL)
		return;
	CALL_CLIENT(p,exitPvpJJCOk());
	delSingleParticipant(instId);
}

void
PvpJJC::rankrewardbyday(std::string& sendName,std::string& title, std::string& content)
{
	std::vector< std::pair< std::vector<std::string>, int> > mapping;
	mapping.resize(16);
	for(size_t i = 0; i < WorldServ::instance()->contactInfoCache_.size(); ++i)
	{
		COM_ContactInfo* p = WorldServ::instance()->contactInfoCache_[i];
		if(p == NULL)
			continue;
		if(p->level_ < Global::get<S32>(C_PVPJJCOpenlevel))
			continue;
		if(p->section_ < 1 || p->section_ > 15)
			continue;
		
		PvpRunkTable::PvpRunkDate const* pCore = PvpRunkTable::getPvpRunkById(p->section_);
		if(pCore == NULL)
		{
			ACE_DEBUG((LM_ERROR, "pvpJJC::rankrewardbyday() PvpRunkTable can not find reward by section_[%d]\n",p->section_));
			continue;
		}
		mapping[p->section_].first.push_back(p->name_);
		mapping[p->section_].second = pCore->dropDay_;
	}
	for(size_t i=0; i<mapping.size(); ++i){
		if(mapping[i].first.empty())
			continue;
		WorldServ::instance()->sendMailByDrop(sendName,mapping[i].first,title,content,mapping[i].second);
	}
}

void
PvpJJC::rankrewardbysenson(std::string& sendName,std::string& title, std::string& content)
{
	std::vector< std::pair< std::vector<std::string>, int> > mapping;
	mapping.resize(16);
	
	for(size_t i = 0; i < WorldServ::instance()->contactInfoCache_.size(); ++i)
	{
		COM_ContactInfo* p = WorldServ::instance()->contactInfoCache_[i];
		if(p == NULL)
			continue;
		if(p->level_ < Global::get<S32>(C_PVPJJCOpenlevel))
			continue;
		if(p->section_ < 1 || p->section_ > 15)
			continue;
		PvpRunkTable::PvpRunkDate const* pCore = PvpRunkTable::getPvpRunkById(p->section_);
		if(pCore == NULL)
		{
			ACE_DEBUG((LM_ERROR, "pvpJJC::rankrewardbyday() PvpRunkTable can not find reward by section_[%d]\n",p->section_));
			continue;
		}

		mapping[p->section_].first.push_back(p->name_);
		mapping[p->section_].second = pCore->dropQuarter_;
	}

	for(size_t i=0; i<mapping.size(); ++i){
		if(mapping[i].first.empty())
			continue;
		WorldServ::instance()->sendMailByDrop(sendName,mapping[i].first,title,content,mapping[i].second);
	}
}

// ------------------------------------- [6/18/2016 lwh]
void
PvpJJC::syncEnemyMsg(Team* myTeam, Team* enemyTeam)
{
	std::vector<COM_SimpleInformation> enemyInfos;
	std::vector<COM_SimpleInformation> mySelfInfos;
	for (size_t k = 0; k < enemyTeam->getMemberSize(); ++k)
	{
		COM_SimpleInformation enemyInfo;
		enemyTeam->teamMembers_[k]->getSimpleInfo(enemyInfo);
		enemyInfos.push_back(enemyInfo);
	}

	for (size_t k = 0; k < myTeam->getMemberSize(); ++k)
	{
		COM_SimpleInformation mySelfInfo;
		myTeam->teamMembers_[k]->getSimpleInfo(mySelfInfo);
		mySelfInfos.push_back(mySelfInfo);
	}

	myTeam->syncEnemyPvpJJCTeamInfo(enemyInfos,enemyTeam->teamId_);
	enemyTeam->syncEnemyPvpJJCTeamInfo(mySelfInfos,myTeam->teamId_);
}

void
PvpJJC::syncEnemyMsg(Player* my, Player* enemy)
{
	if(my == NULL || enemy == NULL)
		return;

	COM_SimpleInformation myInfo;
	my->getSimpleInfo(myInfo);
	CALL_CLIENT(enemy,syncEnemyPvpJJCPlayerInfo(myInfo));

	COM_SimpleInformation enemyInfo;
	enemy->getSimpleInfo(enemyInfo);
	CALL_CLIENT(my,syncEnemyPvpJJCPlayerInfo(enemyInfo));
}

void
PvpJJC::syncEnemyMsg(Player* my, Team* enemyTeam)
{
	if(my == NULL || enemyTeam == NULL)
		return;
	COM_SimpleInformation myInfo;
	my->getSimpleInfo(myInfo);
	enemyTeam->syncEnemyPvpJJCPlayerInfo(myInfo);
	std::vector<COM_SimpleInformation> enemyInfos;
	for (size_t k = 0; k < enemyTeam->getMemberSize(); ++k)
	{
		COM_SimpleInformation enemyInfo;
		enemyTeam->teamMembers_[k]->getSimpleInfo(enemyInfo);
		enemyInfos.push_back(enemyInfo);
	}
	CALL_CLIENT(my,syncEnemyPvpJJCTeamInfo(enemyInfos,enemyTeam->teamId_));
}

void
PvpJJC::pvpjjcBattleGo(Player* player,U32 id)
{
	if(player == NULL)
		return;
	Team* pteam = player->isTeamLeader();
	if(pteam != NULL)
	{
		Player* pother = Player::getPlayerByInstId(id);
		Team* potherTeam = NULL;
		if(pother == NULL)
			potherTeam = TeamLobby::instance()->getTeam(id);
		if(pother == NULL && potherTeam == NULL)
			return;
		if(pother != NULL)
			createjjcBattle(pteam,pother);
		else if(potherTeam != NULL)
			createjjcBattle(pteam,potherTeam);

	}
	else
	{
		Player* pother = Player::getPlayerByInstId(id);
		Team* potherTeam = NULL;
		if(pother == NULL)
			potherTeam = TeamLobby::instance()->getTeam(id);
		if(pother == NULL && potherTeam == NULL)
			return;
		if(pother != NULL)
			createjjcBattle(player,pother);
		else if(potherTeam != NULL)
			createjjcBattle(potherTeam,player);
	}
}

void
PvpJJC::createjjcBattle(Team* myteam,Team* enemyteam)
{
	if(myteam == NULL || enemyteam == NULL)
		return;
	if(myteam->isBattle() || enemyteam->isBattle())
		return;
	Battle* pBattle = Battle::getOneFree();
	pBattle->create(myteam,enemyteam,BT_PVP);
}

void
PvpJJC::createjjcBattle(Player* my, Player* enemy)
{
	if(my == NULL || enemy == NULL)
		return;
	if(my->isBattle() || enemy->isBattle())
		return;
	Battle* pBattle = Battle::getOneFree();
	pBattle->create(my,enemy);
}

void
PvpJJC::createjjcBattle(Team* pteam,Player* player)
{
	if(pteam == NULL || player == NULL)
		return;
	if(!pteam->isBattle())
		return;
	if(player->isBattle())
		return;
	Battle* pBattle = Battle::getOneFree();
	pBattle->create(player,pteam,BT_PVP);
}

//-----------------------------------
void Warriorchoose::start(U32 teamid){
	Team* pteam = TeamLobby::instance()->getTeam(teamid);
	if(pteam == NULL)
		return;
	if(DayliActivity::status_[ACT_Warrior] == false){
		if(!pteam->getLeader())
			return;
		CALL_CLIENT(pteam->getLeader(),errorno(EN_ActivityNoTime));
		return;
	}
	if(pteam->teamisWarrior_)
		return;
	U32 teamSize = pteam->getMemberSize();
	if(teamSize < 3)
		return;
	for (size_t i = 0; i < teamSize; ++i)
	{
		if(pteam->teamMembers_[i]->getProp(PT_Level) < Global::get<int>(C_PVPJJCOpenlevel)){
			return;
		}
		if(pteam->teamMembers_[i]->isLeavingTeam_){
			return;
		}
	}
	pteam->teamisWarrior_ = true;
	teamstore_.push_back(pteam);
	pteam->warriorStartOK();
}

void Warriorchoose::stop(U32 teamid){
	for (size_t i = 0; i < teamstore_.size(); ++i){
		if(teamstore_[i] == NULL)
			continue;
		if(teamstore_[i]->teamId_ == teamid){
			teamstore_[i]->teamisWarrior_ = false;
			teamstore_[i]->teamJJCTimes_ = 0;
			teamstore_[i]->warriorStopOK();
			teamstore_.erase(teamstore_.begin() + i);
			break;
		}
	}
}

void Warriorchoose::close(){
	for (size_t i = 0; i < teamstore_.size(); ++i){
		if(teamstore_[i] == NULL)
			continue;
		teamstore_[i]->teamisWarrior_ = false;
		teamstore_[i]->teamJJCTimes_ = 0;
	}
	teamstore_.clear();
}

void Warriorchoose::tick(float dt){
	if(DayliActivity::status_[ACT_Warrior] == false)
		return;
	for (size_t i = 0; i < teamstore_.size(); ++i){
		if(teamstore_[i] == NULL)
			continue;
		if(teamstore_[i]->teamisWarrior_ == false)
			continue;
		checkEnemy(teamstore_[i],dt);
	}
}

void Warriorchoose::teamWarriorstop(Team* pteam){
	pteam->teamisWarrior_ = false;
	stop(pteam->teamId_);
}

bool Warriorchoose::isWarriorchoose(U32 teamid){
	for (size_t i = 0; i < teamstore_.size(); ++i){
		if(teamstore_[i] == NULL)
			continue;
		if(teamstore_[i]->teamId_ == teamid)
			return true;
	}
	return false;
}

U32 Warriorchoose::calclevelsum(Team* pteam){
	U32 sum = 0;
	for (size_t i=0;i<pteam->getMemberSize();++i)
	{
		if(pteam->teamMembers_[i] == NULL)
			break;
		sum += pteam->teamMembers_[i]->getProp(PT_Level);
	}
	return sum;
}

void Warriorchoose::checkEnemy(Team* p, float dt){
	if(p == NULL){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("checkEnemy Team is Null !!! \n")));
		SRV_ASSERT(p);
	}

	p->teamJJCTimes_ += dt;
	S32 myTeamLevelSum = calclevelsum(p);

	if(p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime1))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i] == NULL)
				continue;
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			S32 otherTeamLevelSum = calclevelsum(teamstore_[i]);
			if(abs(myTeamLevelSum - otherTeamLevelSum) <= 10)
			{
				syncEnemyMsg(p,teamstore_[i]);
				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);
				return;
			}
		}
	}
	else if(p->teamJJCTimes_ > Global::get<float>(C_PVPJJCMeanTime1) && p->teamJJCTimes_ <= Global::get<float>(C_PVPJJCMeanTime2))
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i] == NULL)
				continue;
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			S32 otherTeamLevelSum = calclevelsum(teamstore_[i]);
			if(abs(myTeamLevelSum - otherTeamLevelSum) <= 15)
			{
				syncEnemyMsg(p,teamstore_[i]);
				Team* tmp = teamstore_[i];
				delParticipant(p->teamId_);
				delParticipant(tmp->teamId_);
				return;
			}
		}
	}
	else
	{
		for (size_t i = 0; i < teamstore_.size(); ++i)
		{
			if(teamstore_[i] == NULL)
				continue;
			if(teamstore_[i]->teamId_ == p->teamId_)
				continue;
			syncEnemyMsg(p,teamstore_[i]);
			Team* tmp = teamstore_[i];
			delParticipant(p->teamId_);
			delParticipant(tmp->teamId_);
			return;
		}
	}
}

void
Warriorchoose::syncEnemyMsg(Team* myTeam, Team* enemyTeam)
{
	std::vector<COM_SimpleInformation> enemyInfos;
	std::vector<COM_SimpleInformation> mySelfInfos;
	for (size_t k = 0; k < enemyTeam->getMemberSize(); ++k)
	{
		COM_SimpleInformation enemyInfo;
		enemyTeam->teamMembers_[k]->getSimpleInfo(enemyInfo);
		enemyInfos.push_back(enemyInfo);
	}

	for (size_t k = 0; k < myTeam->getMemberSize(); ++k)
	{
		COM_SimpleInformation mySelfInfo;
		myTeam->teamMembers_[k]->getSimpleInfo(mySelfInfo);
		mySelfInfos.push_back(mySelfInfo);
	}

	myTeam->syncWarriorEnemyTeamInfo(enemyInfos,enemyTeam->teamId_);
	enemyTeam->syncWarriorEnemyTeamInfo(mySelfInfos,myTeam->teamId_);
}

void Warriorchoose::delParticipant(U32 teamid){
	for (size_t i = 0; i < teamstore_.size(); ++i)
	{
		if(teamstore_[i]->teamId_ == teamid)
		{
			teamstore_[i]->teamisWarrior_ = false;
			teamstore_[i]->teamJJCTimes_ = 0;
			teamstore_.erase(teamstore_.begin() + i);
			return;
		}
	}
}

U32 Warriorchoose::sendtrophy(Player* player,bool iswin){
	U32 dropid = 0;
	if(!iswin){
		dropid = Global::get<int>(C_WarriorTrophylow);
	}
	else{
		if(player->warriortrophyNum_<Global::get<int>(C_WarriorTrophyMax)){
			dropid = Global::get<int>(C_WarriorTrophyhigh);
			player->warriortrophyNum_++;
		}
		else
			dropid = Global::get<int>(C_WarriorTrophymiddle);
	}
	player->giveDrop(dropid);

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = iswin;
	GameEvent::procGameEvent(GET_JJC,params,1,player->getHandleId());

	return dropid; 
}