#include "config.h"
#include "battle.h"
#include "player.h"
#include "itemtable.h"
#include "Scene.h"
#include "monster.h"
#include "team.h"
#include "Robot.h"
#include "scenetable.h"
#include "employee.h"
#include "sceneplayer.h"
#include "challengeTable.h"
#include "BattleData.h"
#include "Activity.h"
#include "Activities.h"
#include "PeriodEvent.h"
#include "GameRuler.h"
#include "worldserv.h"
void Battle::createPlayerUnit(Player* player,GroupType gt){
	{
		BattlePosition pos = findPosition(player,gt);
		pos = player->getProp(PT_Front) == 0 ? pos : (BattlePosition)(pos + 5);
		entities_.push_back(player);
		player->initBattleStatus(id_,gt,pos);
	}

	Baby *pBaby0 = player->getBattleBaby();
	if(pBaby0)
	{
		pBaby0->initBattleStatus(id_, player->battleForce_, calcFriendPosition(pBaby0->getOwner()));
		entities_.push_back(pBaby0);
	}
	
	
}
void Battle::createPlayerEmployees(Player*player){
	int n = 5;
	Team* t = player->isTeamLeader();
	if(t){
		n = n - t->teamMembers_.size() + t->getTeamLeaveNum();  //伙伴上阵数 5-队伍size+leaveNum;
	}
	std::vector<U32>& emps = player->getCurrentBattleEmployees();
	for (size_t i = 0; (i < emps.size()) && (n != 0); ++i,--n)
	{
		if(emps[i] == 0)
			continue;
		Employee* pEmp = player->findEmployee(emps[i]);
		if(pEmp == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Battle::create(Player* pPlayer, Robot* pRobot) BattleEmployee is NULL EmployeeInstId[%d]\n"),emps[i]));
			continue;
		}
		pEmp->initBattleStatus(id_, player->battleForce_, findPosition(pEmp,player->battleForce_),(sneakattack_ == SAT_SurpriseAttack));
		entities_.push_back(pEmp);
	}
	
}
void Battle::createMonsterUnits(std::vector<std::string> & monsters,std::vector<Monster*> &units){

	for (size_t i=0; i<monsters.size(); ++i)
	{
		if(monsters[i].empty() || monsters[i] == "")
			continue;
		Monster* pMonster = NEW_MEM(Monster,monsters[i]);
		SRV_ASSERT(pMonster);
		pMonster->initBattleStatus(id_,  (GroupType)((i <= BP_Down9) ? GT_Down : GT_Up ),(BattlePosition)i, (sneakattack_ == SAT_SurpriseAttack));
		units.push_back(pMonster);
	}
	killmonsters_.clear();
}

void Battle::create(Player* player,ZoneInfo* zone)
{	
	opentime_ = WorldServ::instance()->curTime_;
	if(!player)
		return ;
	if(!zone)
		return;
	if(player->isBattle())
		return;
	player->setBattleActive(false);
	battleRobot_ = NULL;
	battleType_ = BT_PVE;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	roundCount_ = 0;
	sneakattack_ = calcSneakAttack(player);
	
	createPlayerUnit(player,GT_Down);
	createPlayerEmployees(player);

	U32 roll = UtlMath::randNM(zone->rollMin_, zone->rollMax_);

	for (size_t i=0; i<roll; ++i)
	{
		U32 index = UtlMath::randNM(0, zone->monsterClasses_.size()-1);

		Monster* pMonster = NEW_MEM(Monster,zone->monsterClasses_[index]);
		pMonster->initBattleStatus(id_,GT_Up,findMonsterPosition(),(sneakattack_ == SAT_SneakAttack));
		entities_.push_back(pMonster);
		monsters_.push_back(pMonster);
	}
	
	checkSameGuid();
	regenPosTable();
	initAI();
	startBattle();
}

void Battle::create(Team* pTeam, ZoneInfo* pZone)
{
	opentime_ = WorldServ::instance()->curTime_;
	if(!pTeam)
		return;
	if(!pZone)
		return;
	if(pTeam->isBattle()){
		return;
	}
	
	battleRobot_ = NULL;
	battleType_ = BT_PVE;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	roundCount_ = 0;
	pTeam->createBttleUnit(this,GT_Down);
	teams_.push_back(pTeam);
	U32 roll = UtlMath::randNM(pZone->rollMin_, pZone->rollMax_);
	for (size_t i=0; i<roll; ++i){
		U32 index = UtlMath::randNM(0, pZone->monsterClasses_.size()-1);
		Monster* pMonster = NEW_MEM(Monster,pZone->monsterClasses_[index]);
		pMonster->initBattleStatus(id_,GT_Up,findMonsterPosition());
		entities_.push_back(pMonster);
		monsters_.push_back(pMonster);
	}

	checkSameGuid();
	regenPosTable();
	initAI();
	
	startBattle();
}

void Battle::create(Player* p0, Player* p1, BattleType tp)
{
	opentime_ = WorldServ::instance()->curTime_;
	if (!p0)
		return;
	if (!p1)
		return;
	if (p0->playerId_ == p1->playerId_)
		return;
	if(p0->isBattle() || p1->isBattle())
		return;
	battleType_ = tp;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	roundCount_ = 0;
	battleRobot_ = NULL;
	
	createPlayerUnit(p0,GT_Down);
	createPlayerUnit(p1,GT_Up);


	if(tp == BT_PK1 || tp == BT_PK2)
	{
		createPlayerEmployees(p0);
		createPlayerEmployees(p1);
	}

	checkSameGuid();
	regenPosTable();
	initAI();
	startBattle();
}

void Battle::create(Player* pPlayer, Robot* pRobot)
{
	opentime_ = WorldServ::instance()->curTime_;
	if(pPlayer == NULL || pRobot == NULL)
		return;
	if(pPlayer->isBattle() || pRobot->isBattle())
	{
		CALL_CLIENT(pPlayer, errorno(EN_Battle));
		return;
	}

	battleType_ = BT_PVR;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	battleRobot_ = pRobot;
	actionIndex_ = 0;
	roundCount_ = 0;
		
	createPlayerUnit(pPlayer,GT_Down);
	createPlayerEmployees(pPlayer);

	pRobot->initBattleStatus(id_,GT_Up,BP_Up2);
	entities_.push_back(pRobot);
	Baby *pBaby1 = NULL;
	if (!pRobot->babies_.empty())
	{
		pBaby1 = pRobot->babies_[0];
		if(pBaby1)
		{
			pBaby1->initBattleStatus(id_,GT_Up,BP_Up7);
			entities_.push_back(pBaby1);

			for (size_t i = 0; i < pBaby1->skills_.size(); ++i)
			{
				pBaby1->aiSkills_.push_back(pBaby1->skills_[i]->skId_);
			}
		}
	}

	for (size_t i = 0; i < pRobot->employees_.size(); ++i)
	{
		Employee* pEmp = pRobot->employees_[i];
		if (pEmp == NULL)
			continue;

		BattlePosition bpos = findPosition(pEmp,GT_Up);
		
		if(bpos == BP_None)
			break;
		
		pEmp->initBattleStatus(id_,GT_Up,bpos);
		entities_.push_back(pEmp);
	}

	checkSameGuid();
	regenPosTable();
	initAI();
	startBattle();
}

void Battle::create(Player* player,S32 battleTableId)
{
	opentime_ = WorldServ::instance()->curTime_;
	if(!player)
		return ;
	if(player->isBattle())
		return;
	const BattleData* pbd = BattleData::getBattleDataById(battleTableId);
	if(NULL == pbd)
		return;
	//if(pbd->battleType_ == BT_PET){
	//	return;
	//}
	//if(pbd->battleType_ == BT_PVH )
	//{
	//	ErrorNo en = GameRuler::CanHundredBattle(player,pbd->battleId_);
	//	if(en!=EN_None){
	//		if(en!=EN_Max){
	//			CALL_CLIENT(player,errorno(en));
	//		}
	//		return;
	//	}
	//}
	
	battleType_ = pbd->battleType_;
	battleState_ = BS_Used; 
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	upWave_ = downWave_ = 0;
	roundCount_ = 0;
	battleDataId_ = battleTableId;
	battleRobot_ = NULL;
	createPlayerUnit(player);
	createPlayerEmployees(player);
	{
		std::vector<std::string> monsterstr;
		bool has = pbd->getUpMonsters(upWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());
		}
	}

	{
		std::vector<std::string> monsterstr;
		bool has = pbd->getDownMonsters(downWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());
		}
	}
	checkSameGuid();
	regenPosTable();
	initAI();
	startBattle();
}

void Battle::create(Team* pTeam,int battleTableId)
{
	opentime_ = WorldServ::instance()->curTime_;
	if(!pTeam)
		return;
	if(pTeam->isBattle())
		return;
	const BattleData* pbd = BattleData::getBattleDataById(battleTableId);
	if(NULL == pbd)
		return;
	//if(pbd->battleType_ == BT_PVH && !pTeam->canHundredBattle(pbd->battleId_) )
	//	return;
	//else if(pbd->battleType_ == BT_PET && (!DayliActivity::status_[ACT_Pet] || !pTeam->canPetActivity(pbd->battleId_)))
	//	return ;
	battleType_ = pbd->battleType_;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	upWave_ = downWave_ = 0;
	roundCount_ = 0;
	battleDataId_ =  battleTableId;
	battleType_;
	battleRobot_ = NULL;
	pTeam->createBttleUnit(this,GT_Down);
	teams_.push_back(pTeam);
	{
		std::vector<std::string> monsterstr;
		bool has = pbd->getUpMonsters(upWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());
		}
	}

	{
		std::vector<std::string> monsterstr;
		bool has = pbd->getDownMonsters(downWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());
		}
	}
	checkSameGuid();
	regenPosTable();
	initAI();
	startBattle();
}

void
Battle::create(Team* pTeamDown,Team* pTeamUp, BattleType tp)
{
	if(!pTeamDown)
		return;
	if(!pTeamUp)
		return;
	
	if(pTeamDown->isBattle() || pTeamUp->isBattle())
		return;
	opentime_ = WorldServ::instance()->curTime_;
	
	battleRobot_ = NULL;
	battleType_ = tp;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	roundCount_ = 0;
	actionIndex_ = 0;
	pTeamDown->createBttleUnit(this,GT_Down,(battleType_ == BT_PK1 || battleType_ == BT_PK2));
	pTeamUp->createBttleUnit(this,GT_Up,(battleType_ == BT_PK1 || battleType_ == BT_PK2));	
	teams_.push_back(pTeamUp);
	teams_.push_back(pTeamDown);
	if(battleType_ == BT_Guild)
	{
		for(size_t i=0;i<pTeamDown->teamMembers_.size();++i){
			COM_GuildMember* pMember = pTeamDown->teamMembers_[i]->myGuildMember();
			SRV_ASSERT(pMember);
			pTeamDown->teamMembers_[i]->updateMyGuildMember();
		}
		for(size_t i=0;i<pTeamUp->teamMembers_.size();++i){
			COM_GuildMember* pMember = pTeamUp->teamMembers_[i]->myGuildMember();
			SRV_ASSERT(pMember);
			pTeamUp->teamMembers_[i]->updateMyGuildMember();
		}
	}
	checkSameGuid();
	regenPosTable();
	startBattle();
}

void
Battle::create(Player* player,Team* pTeam,BattleType tp)
{
	if(player == NULL || pTeam == NULL)
		return;
	if(pTeam->isBattle())
		return;
	if(!pTeam->isSameGuild())
		return;
	battleRobot_ = NULL;
	battleType_ = tp;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	upWave_ = downWave_ = 0;
	roundCount_ = 0;

	pTeam->createBttleUnit(this,GT_Down,battleType_ != BT_Guild);
	teams_.push_back(pTeam);
	createPlayerUnit(player,GT_Up);
	checkSameGuid();
	startBattle();

	if(battleType_ == BT_Guild)
	{
		player->updateMyGuildMember();
		for(size_t i=0;i<pTeam->teamMembers_.size();++i){ ///居然有不是帮派的人
			pTeam->teamMembers_[i]->updateMyGuildMember();
		}
	}
}

void Battle::create(Player* player, Monster* monster){
	opentime_ = WorldServ::instance()->curTime_;
	if(!player)
		return ;
	if(!monster)
		return;
	if(player->isBattle())
		return;
	player->setBattleActive(false);
	battleRobot_ = NULL;
	battleType_ = BT_Guild;
	battleState_ = BS_Used;
	battleWinner_ = GT_None;
	actionIndex_ = 0;
	roundCount_ = 0;
	
	Team* pTeam = player->myTeam();
	if(pTeam)
		pTeam->createBttleUnit(this,GT_Down,false);
	else 
		createPlayerUnit(player,GT_Down);

	monster->initBattleStatus(id_,GT_Up,findMonsterPosition(),(sneakattack_ == SAT_SneakAttack));
	entities_.push_back(monster);
	monsters_.push_back(monster);

	checkSameGuid();
	startBattle();
}