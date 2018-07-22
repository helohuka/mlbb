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
#include "BattleData.h"
#include "team.h"
#include "worldserv.h"
#include "sceneplayer.h"

//const char* Battle::logdir_ = "battlelog";
std::vector<Battle*> Battle::battleCache_;

Battle::Battle(){
	battleState_	= BS_None;
	sneakattack_	= SAT_None;
	roundCount_		= 0;
	guildSceneId_ = 0;
	guildNpcId_ = 0;
	battleRobot_ = NULL;
}

void 
Battle::init(){
	/*std::string logdir  = logdir_;
	if (!logdir.empty()){
		ACE_OS::mkdir(logdir.c_str());
	}*/
	battleCache_.resize(BattleCacheSize);
	for(S32 i=0; i<BattleCacheSize; ++i){
		battleCache_[i] = NEW_MEM(Battle);
		SRV_ASSERT(battleCache_[i]);
		battleCache_[i]->id_ = i + 1;
	}
}

void
Battle::fini(){
	for (size_t i=0; i<battleCache_.size(); ++i){
		if(battleCache_[i])
			DEL_MEM(battleCache_[i]);
	}
	battleCache_.clear();
}

Battle*
Battle::find(S32 id){
	id = id -1;
	if(id<0 && id>battleCache_.size())
		return NULL;

	return battleCache_[id];
}

Battle*
Battle::getOneFree(){
	for (size_t i = 0; i < battleCache_.size(); ++i){
		if(battleCache_[i]->battleState_ == BS_None){
			battleCache_[i]->cleanBattle();
			return battleCache_[i];
		}
	}

	return NULL;
}

void 
Battle::updateBattle(float dt){
	for(size_t i=0; i<battleCache_.size(); ++i)
		battleCache_[i]->update(dt);
}

void 
Battle::update(float dt){
	if(battleState_ != BS_Used)
		return ;
	
	waitCloseTimeout_ -= dt;
	if(waitCloseTimeout_ <=0){
		allLose();
		battleOver();
		ACE_DEBUG((LM_DEBUG,"Battle all lose %d %d\n",id_,battleDataId_));
		logExitBattle();
		return;
	}

	if(hasPlayerPushOrder_){
		waitOrderTimeout_ -= dt;
		if(waitOrderTimeout_ <=0.F){
			for (size_t i=0; i<entities_.size(); ++i){
				if(checkState(entities_[i]))
					continue;
				if(!entities_[i]->getBattleActive())
					update(entities_[i]);
			}
		}
	}
	
	for (size_t i=0; i<entities_.size(); ++i){
		if(entities_[i]->isDeadth()){
			continue;
		}
		else if(entities_[i]->asBaby()){
			InnerPlayer* p = entities_[i]->asBaby()->getOwner();
			if(NULL==p)
				continue;
			else if(p->isDeadth())
				continue;
		}
		if(checkState(entities_[i]))
			continue;
		if(entities_[i]->isBattleAtkTimeout()){
			update(entities_[i]);
		}
		if(!entities_[i]->getBattleActive())
			return;
	}
	
	execRound(dt);
}

bool
Battle::execRound(float dt){
	++roundCount_;
	activeRound_ = true;
	prepareOrder(); ///处理无效order 删除掉
	orderSort();
	checkUnite();
	checkOrder();
	deleteStateByTrun();
	cleanHuwei();
	regenPosTable();
	//--
	RAList &ral = actions();
	for (actionIndex_=0; actionIndex_<ral.size(); ++actionIndex_){		
		if(!checkCurrentAction())
			ral.erase(ral.begin() + actionIndex_--);
		else if(!execOneOrder())
		{
			ral.erase(ral.begin() + actionIndex_--);
		}
		else if (battleWinner_ != GT_None){

			RAList::iterator tmp =  ral.begin() + actionIndex_ + 1;
			if(tmp != ral.end())
			{
				ral.erase(tmp,ral.end());
				updateUnite(); ///更新合击数
			}
			break;
		}
	}
	postState(); ///最后处理未操作玩家状态
	lookupStateByTrun();
	calcWinner(); ///最后的处理 状态可能掉血
	checkEntity();
	checkReport();
	
	addWaveMonster();
	syncRoundReport();
	///发送战报
	
	cleanRound();

	if (battleWinner_ != GT_None){
		calcBattleReward();
		battleOver();
	}
	else{
		for (size_t i=0; i<entities_.size(); ++i){
			if(entities_[i]->asPlayer() || entities_[i]->asBaby())
				continue;
			update(entities_[i]);
		}
	}
	
	return true;
}

void Battle::startBattle(){
	COM_InitBattle r;
	r.bt_ = battleType_;
	r.battleId_ = battleDataId_;
	r.sneakAttack_ = sneakattack_;
	r.roundCount_ = roundCount_;
	for ( size_t i=0; i<entities_.size(); ++i){
		COM_BattleEntityInformation info;
		entities_[i]->getBattleEntityInformation(info);
		r.actors_.push_back(info);
	}

	for(size_t i=0; i<entities_.size(); ++i){
		Player *p = entities_[i]->asPlayer();
		if(p){
			CALL_CLIENT(p,enterBattleOk(r));
			p->scenePlayer_->joinBattle();
		}

	}
	for(size_t i=0; i<teams_.size(); ++i){
		teams_[i]->lock();
	}

	logJoinBattle();
}

void
Battle::syncRoundReport(){
	for(size_t i=0; i<entities_.size(); ++i){
		if( entities_[i]->asPlayer() /*&& entities_[i]->getBattleActive()*/ ){
			CALL_CLIENT(entities_[i],syncOneTurnAction(roundReport_));
		}
		
	}
	//ACE_DEBUG((LM_DEBUG,"Battle::syncRoundReport();\n"));
}

void
Battle::cleanRound(){
	for(size_t i=0; i<entities_.size(); ++i){
		entities_[i]->setBattleActive(false);
	}
	waitCloseTimeout_ = WAIT_BATTLE_CLOSE_TIMEOUT;
	waitOrderTimeout_ = WAIT_BATTLE_ORDER_TIMEOUT;
	waitCalcOrderTimeout_ = WAIT_BATTLE_CALC_ORDER_TIMEOUT * roundReport_.actionResults_.size();
	roundReport_.actionResults_.clear();
	roundReport_.stateIds_.clear();
	roundReport_.targets_.clear();
	roundReport_.waveEntities_.clear();
	cleanUnitAtk();
	activeRound_ = false;
	hasPlayerPushOrder_ = false;
	
	//ACE_DEBUG((LM_DEBUG,"Battle::cleanRound();\n"));
}

bool 
Battle::execOneOrder(){
	//被飞掉 可能会直接destory
	COM_ReportAction *pra = getCurrentAction();

	Entity *caster = findEntityById(pra->casterId_);
	if(NULL == caster)
		return true;

	updateUnite();

	if(isInvalid(caster)){ ///如果是玩家的话需要计算输赢	
		if(caster->asPlayer()){
			calcWinner();
		}
		return true;
	}
	if(checkState(caster))
		return true;
	if(!caster->updateState(posTable_))
		return true;
	if(pra->itemId_)
		battleItem();
	else if(pra->weaponInstId_)
		battleWeapon();
	else if(pra->skill_){
		checkBattleSkill();
		battleSkill();
	}else {
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Battle order do nothing\n")));
	}
	calcWinner();
	return true;
}

void 
Battle::battleOver(){

	logExitBattle();

	for(size_t i=0; i<entities_.size(); ++i){
		//ACE_DEBUG((LM_INFO,"Battle over [%d]\n",entities_[i]->getGUID()));
		entities_[i]->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild)); 
		if(entities_[i]->asPlayer() ){
			entities_[i]->asPlayer()->calcEquipmentDurability();
			std::vector<S32>	babys;
			for (size_t j = 0; j < entities_[i]->asPlayer()->babies_.size(); ++j)
			{
				Baby* pbaby = entities_[i]->asPlayer()->babies_[j];
				if(pbaby == NULL)
					continue;
				babys.push_back(pbaby->getGUID());
			}
			GEParam params[3];
			params[0].type_ = GEP_INT;
			params[0].value_.i = battleDataId_;
			params[1].type_ = GEP_INT;
			params[1].value_.i = battleType_;
			params[2].type_ = GEP_HANDLE_ARRAY;
			params[2].value_.hArray = &babys;
			GameEvent::procGameEvent(GET_BattleOver,params,3,entities_[i]->asPlayer()->getHandleId());
		}
	}

	for(size_t i=0; i<deadthBaby_.size(); ++i){
		if(deadthBaby_[i])
			deadthBaby_[i]->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)); 
	}

	for(size_t i=0; i<teams_.size(); ++i){
		if(teams_[i])
			teams_[i]->unlock();
	}
	
	cleanBattle();
}

SneakAttackType
Battle::calcSneakAttack(Player *player){
	S32 monsterRate = Global::get<S32>(C_MonsterSneakAttack);
	S32 playerRate = player->getProp(PT_SneakAttack);
	S32 rate = UtlMath::randN(100);
	return (sneakattack_ = CALC_SNEAK_ATTACK(monsterRate,playerRate,rate));
}


void 
Battle::calcWinner(){
	bool hasUpOne = false;
	bool hasDownOne = false;
	for (size_t i = 0; i < entities_.size(); ++i){
		if(entities_[i]->asBaby())continue;
		if (entities_[i]->battleForce_ == GT_Up){
			if (isInvalid(entities_[i]))continue;
			hasUpOne = true;
		}
		if(entities_[i]->battleForce_ == GT_Down){
			if( isInvalid(entities_[i]) )continue;
			hasDownOne = true; /// BUG
		}
	}
	
	if(!hasUpOne) 
		battleWinner_ = GT_Down;
	else if(!hasDownOne) 
		battleWinner_ = GT_Up;
}

void Battle::addWaveMonster(){
	if(battleWinner_ == GT_Up){
		++downWave_;
		const BattleData* pbd = BattleData::getBattleDataById(battleDataId_);
		if(NULL == pbd)
			return;

		std::vector<std::string> monsterstr;
		bool has = pbd->getDownMonsters(downWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());

			for(size_t i=0;i<createMonsters.size(); ++i){
				COM_BattleEntityInformation info;
				createMonsters[i]->getBattleEntityInformation(info);
				roundReport_.waveEntities_.push_back(info);
			}
			battleWinner_ = GT_None;
		}
	}
	else if(battleWinner_ == GT_Down){
		++upWave_;
		const BattleData* pbd = BattleData::getBattleDataById(battleDataId_);
		if(NULL == pbd)
			return;

		std::vector<std::string> monsterstr;
		bool has = pbd->getUpMonsters(upWave_,monsterstr);
		if(has)
		{
			std::vector<Monster*> createMonsters;
			createMonsterUnits(monsterstr,createMonsters);
			monsters_.insert(monsters_.end(),createMonsters.begin(),createMonsters.end());
			entities_.insert(entities_.end(),createMonsters.begin(),createMonsters.end());

			for(size_t i=0;i<createMonsters.size(); ++i){
				COM_BattleEntityInformation info;
				createMonsters[i]->getBattleEntityInformation(info);
				roundReport_.waveEntities_.push_back(info);
			}
			battleWinner_ = GT_None;
		}
	}
}

bool 
Battle::calcBabyLoyal(COM_Order& order){
	Entity* caser = findEntityById(order.casterId_);
	if(NULL == caser) return false;
	Baby* baby = caser->asBaby();
	if(NULL == baby) return false;
	Player* player = baby->getOwner()->asPlayer();
	if(NULL == player) return false;
	
	float loyal = baby->getProp(PT_Glamour);

	S32 rand = UtlMath::randN(Global::get<int>(C_BabyLoyalMax));
	loyal += Global::get<int>(C_BabyLoyalVar);
	if(rand > loyal){
		enum{
			SK_0,
			SK_1,
			SK_2,
		};

		S32 rand2 = UtlMath::randN(3);
		if(rand2 == SK_0){
			order.skill_ = Global::get<int>(C_AttackSkillId);
			order.target_ = (int32)(caser->battleForce_ == GT_Up ? BP_Down1 : BP_Up1);
		}
		else if(rand2 == SK_1){
			order.skill_ = Global::get<int>(C_DefenseSkillId);
			order.target_ = caser->battlePosition_;
		}
		else if(rand2 == SK_2){
			order.skill_ = Global::get<int>(C_NothingSkillId);
		}
		order.isNo_ = true;
	}

	return false;
}

void
Battle::battleItem(){
	
	COM_ReportAction &ra = actions()[actionIndex_];
	Entity* pEnt = findEntityById(ra.casterId_);
	if(NULL == pEnt)
	{
		ACE_DEBUG((LM_ERROR,"Why has invalid caster %d \n",ra.casterId_));
		return;
	}
	Player* pPla = pEnt->asPlayer();
	if(NULL == pPla)
	{
		ACE_DEBUG((LM_ERROR,"Only player can use item in battle \n"));
		return;
	}
	COM_Item* pItem = pPla->getBagItemMinStackByItemId(ra.itemId_);
	if(NULL == pItem){
		ACE_DEBUG((LM_ERROR,"Battle use item can not find %d %d \n",ra.casterId_, ra.itemId_));
		return;
	}

	ItemTable::ItemData const* pData = ItemTable::getItemById(pItem->itemId_);
	
	if( NULL == pData){
		ACE_DEBUG((LM_ERROR,"Battle use item is error  %d %d \n",ra.casterId_, ra.itemId_));
		return;
	}

	if( pData->skillId_ == 0){
		ACE_DEBUG((LM_ERROR,"Battle use item dnot has skill id  %d %d \n",ra.casterId_, ra.itemId_));
		return;
	}

	ra.status_   = OS_ActiveOk;

	cleanOderParam();
	setOrderParam(OPT_BabyId,ra.babyId_);

	pPla->castSkill(pData->skillId_,(BattlePosition)ra.target_,id_,posTable_,orderParam_);

	pPla->delBagItemByItemId(pData->id_);
}

void 
Battle::battleWeapon(){
	COM_ReportAction &ra = actions()[actionIndex_];
	Entity* pEnt = findEntityById(ra.casterId_);
	if(NULL == pEnt)
	{
		ACE_DEBUG((LM_ERROR,"Why has invalid caster %d \n",ra.casterId_));
		return;
	}
	Player* player = pEnt->asPlayer();
	if(NULL == player)
	{
		ACE_DEBUG((LM_ERROR,"Only player can swap weapon in battle \n"));
		return;
	}

	ra.status_ = OS_Weapon;

	bool isEquiped = player->getEquipment(ra.weaponInstId_)!=NULL;
	
	if(!isEquiped){
		COM_Item* bagItem =  player->getBagItemByInstId(ra.weaponInstId_);
		
		if(NULL == bagItem){
			ACE_DEBUG((LM_INFO,"Not find weapon %d~!!!!!\n",ra.weaponInstId_));
			return;
		}
		const ItemTable::ItemData* itemData =  ItemTable::getItemById(bagItem->itemId_);
		if(NULL == itemData){
			ACE_DEBUG((LM_INFO,"Not find itemdata %d ~!!!!!\n",bagItem->itemId_));
			return;
		}

		if ( ( itemData->slot_ != ES_SingleHand )
			&& ( itemData->slot_ != ES_DoubleHand ) ){
			ACE_DEBUG((LM_INFO,"Can not change  itemData->kind_ != ES_SingleHand %d~!!!!!\n",bagItem->itemId_));
			return;
		}
		
		ra.itemId_ = itemData->id_;
		player->wearEquipment(player->getGUID(),ra.weaponInstId_);
		///穿装备和更换装备
	}
	else{
		player->delEquipment(ra.casterId_,ra.weaponInstId_);
		///托装备
	}
}

void
Battle::battleSkill(){
	COM_ReportAction &ra = actions()[actionIndex_];
	Entity* pEnt = findEntityById(ra.casterId_);
	if(NULL == pEnt)
	{
		ACE_DEBUG((LM_ERROR,"Why has invalid caster %d \n",ra.casterId_));
		return;
	}
	Skill* pSk = pEnt->getSkillById(ra.skill_);
	if(!pSk){
		ACE_DEBUG((LM_ERROR,"Can not find skill(%d) in caster skills !\n",ra.skill_));
		return;
	}
	SkillTable::Core const * pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	//SRV_ASSERT(pCore);
	if (pCore){
		if(pCore->skType_ == SKT_Active || pCore->skType_ == SKT_DefaultActive || pCore->skType_ == SKT_DefaultSecActive){
			Entity* pTarget = findEntityByPos(ra.target_);

			if(pEnt->isDeadth())
				return;

			BattlePosition huweiPos = getHuwei((BattlePosition)ra.target_);

			if(ra.uniteNum_ > 1){///合击不出护卫
				huweiPos = BP_None;
				ra.huweiPosition_ = 0;
			}
			else if(huweiPos != BP_None){ ///刘兆楠要拧一下
				if(huweiPos != pEnt->battlePosition_)
					ra.huweiPosition_ = ra.target_;
				else
					huweiPos = BP_None; //打的人是自己要护卫的人
			}
			
			ra.status_ = OS_ActiveOk;

			cleanOderParam();
			setOrderParam(OPT_BabyId,ra.babyId_);
			setOrderParam(OPT_Unite,ra.uniteNum_);
			setOrderParam(OPT_Huwei,(S32)huweiPos);
			setOrderParam(OPT_IsNo,ra.isNo_);
			if(false == pEnt->castSkill(ra.skill_,(BattlePosition)ra.target_,id_,posTable_,orderParam_)){
				if(ra.skill_ != 1){
					ACE_DEBUG((LM_DEBUG,ACE_TEXT("ERROR battleSkill casterID==[%d]--targetPos[%d]--castSkillid==[%d]\n"),ra.casterId_,ra.target_,ra.skill_));
					//return;
				}
			}
			
		}
	
	}
}

void Battle::calcPlayerKillEvent(Player* caster, Entity* target){
	if(!target->isDeadth())
		return;
	GEParam params[5];
	params[0].type_ = GEP_INT;
	params[0].value_.i = id_;
	params[1].type_ = GEP_INT;
	params[1].value_.i = (S32)target->getProp(PT_Level);
	params[2].type_ = GEP_INT;
	params[3].type_ = GEP_INT;
	params[4].type_ = GEP_INT;

	params[4].value_.i = (target->getForce() == caster->getForce()) ? 1 : 0;

	if(target->asMonster()){
	
		MonsterTable::MonsterData const* p = MonsterTable::getMonsterById(target->asMonster()->monsterId_);
		if(p == NULL)
			return;
		if(p->monsterType_ == 2)
			params[3].value_.i = ET_Boss;
		else
			params[3].value_.i = ET_Monster;
		params[2].value_.i = target->asMonster()->monsterId_;
	}
	else if(target->asEmployee()){
		params[3].value_.i = ET_Emplyee;
		params[2].value_.i = target->asEmployee()->getProp(PT_TableId);
	}
	else if(target->asBaby()){
		params[3].value_.i = ET_Baby;
		params[2].value_.i = target->getGUID();
	}
	else if(target->asPlayer()){
		params[3].value_.i = ET_Player;
		params[2].value_.i = target->getGUID();
	}	

	GameEvent::procGameEvent(GET_Kill,params,5,caster->getHandleId());
}

void Battle::getInitData(COM_InitBattle& r,U32 playerid){
	Entity* pE = findEntityById(playerid);
	if(NULL == pE)
		return;
	Player* pp = pE->asPlayer();
	S32 ppoc = orderCounter(pp);
	if(ppoc == 0){
		r.opType_ = OT_P1;
	}
	else if(ppoc == 1){
		pE = pp->getBattleBaby();
		if(NULL == pE)
			r.opType_ = OT_P2;
		else if(orderCounter(pE) == 0)
			r.opType_ = OT_B;
	}

	r.bt_ = battleType_;
	r.battleId_ = battleDataId_;
	r.sneakAttack_ = sneakattack_;
	r.roundCount_ = roundCount_;
	for ( size_t i=0; i<entities_.size(); ++i){
		COM_BattleEntityInformation info;
		entities_[i]->getBattleEntityInformation(info);
		r.actors_.push_back(info);
	}
}

S32 Battle::totalAtkVal(S32 unitId, S32 unitNum, S32 val){
	if(unitId == 0 || unitNum <= 1)
		return val;
	for(size_t i=0; i<unitAtk_.size(); ++i){
		if(unitAtk_[i].first == unitId){
			unitAtk_[i].second += val;
			return unitAtk_[i].second;
		}
	}
	
	std::pair<S32,S32> pa(unitId,val);

	unitAtk_.push_back(pa);
	return val;
}


void Battle::cleanBattle(){
	
	cleanRound();
	
	Scene* s = SceneManager::instance()->getScene(guildSceneId_);
	if(s){
		s->delBattleNpc(guildNpcId_);
	}
	
	for (size_t i=0; i<monsters_.size(); ++i){
		if(monsters_[i])
			DEL_MEM(monsters_[i]);
	}

	monsters_.clear();
	killmonsters_.clear();
	entities_.clear();
	deadthBaby_.clear();
	teams_.clear();
	battleState_ = BS_None;
	roundCount_ = 0;
	battleDataId_ = 0;
	sneakattack_ = SAT_None;
	actionIndex_ = 0;
	upWave_ = downWave_ = 0;
	waitCalcOrderTimeout_ = 0;
	guildSceneId_ = 0;
	guildNpcId_ = 0;
	upLosePlayerSum_ = 0;
	downLosePlayerSum_ = 0;

	if(battleRobot_ != NULL && battleRobot_->isPlayerData_)
	{
		DEL_MEM(battleRobot_);
		battleRobot_ = NULL;
	}
	
}
void Battle::checkBattleStatus(){
	bool hasPlayer = false;
	for(size_t i=0; i<entities_.size(); ++i){

	}
}

bool Battle::checkCurrentAction(){
	COM_ReportAction *pra = getCurrentAction();
	if(NULL == pra)
		return false;
	Entity* pEnt = findEntityById(pra->casterId_);
	if(NULL == pEnt)
		return false;
	if(isInvalid(pEnt))
		return false;
	return true;
}

COM_ReportAction *Battle::getCurrentAction(){
	if(actionIndex_ < 0 || actionIndex_ >= roundReport_.actionResults_.size())
		return NULL;
	return &(roundReport_.actionResults_[actionIndex_]);
}