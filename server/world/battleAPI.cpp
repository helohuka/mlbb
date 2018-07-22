#include "config.h"
#include "battle.h"
#include "player.h"
#include "itemtable.h"
#include "DropTable.h"
#include "monster.h"
#include "team.h"
#include "pvpJJC.h"
#include "PVPrunkTable.h"
#include "sttable.h"
#include "Robot.h"
#include "EndlessStair.h"
#include "profession.h"
#include "GameEvent.h"
#include "Scene.h"
#include "challengeTable.h"
#include "sceneplayer.h"
#include "pkActity.h"
#include "Guild.h"
#include "loghandler.h"
static BattlePosition	downPosition[5] = { BP_Down2,BP_Down1,BP_Down3,BP_Down0,BP_Down4 };
static BattlePosition	upPosition[5]   = { BP_Up2,BP_Up1,BP_Up3,BP_Up0,BP_Up4 };
static BattlePosition	monsterPosition[10] = { BP_Up2,BP_Up1,BP_Up3,BP_Up0,BP_Up4,BP_Up7,BP_Up6,BP_Up8,BP_Up5,BP_Up9 };
#define ISFLY(VAL,HP) ((S32)abs(VAL) >= (S32)abs(HP * 1.5F))

void Battle::changeProp(ChangePropType cpt, BattlePosition pos, PropertyType type, float val, bool isBao)
{
	COM_ReportAction* pRep = getCurrentAction();
	if (CPT_State != cpt && pRep == NULL)
		return;
	Entity *p = findEntityByPos(pos);
	if(NULL == p)
	{
		ACE_DEBUG((LM_ERROR,"changeProp findEntityByPos(%d) Entity is null !!!!\n",pos));
		return;
	}	
	if(p->isDeadth()){
		ACE_DEBUG((LM_ERROR,"This monster is deadth!!! do not hit it please !!!! %s\n",p->getNameC()));
	}

	float current =  p->getProp(type) + val;

	/*if(p->asMonster() && type == PT_HpCurr)
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("this monster[%d] PT_HpCurr[%f] changeHp[%f] \n"),p->getProp(PT_TableId),current,val));*/

	COM_ReportTarget target;
	target.position_ = (BattlePosition)pos;
	
	target.prop_.type_ = type;
	target.prop_.value_ = (S32)current;
	target.bao_ = isBao;
	
	if(type == PT_HpCurr && ((S32)current <= 0) && p->getProp(type)){ //计算是否击飞
		S32 totalVal = 0;
		if(pRep) 
			totalVal = totalAtkVal(pRep->uinteGroup_,pRep->uniteNum_,val);
		target.fly_ = ISFLY(totalVal,p->getProp(PT_HpMax));
	}

	switch(cpt)
	{
	case CPT_Normal:
		pRep->targets_.push_back(target);
		break;
	case CPT_State:
		roundReport_.targets_.push_back(target);
		break;
	case CPT_Fanji:
		if(!pRep->targets_.empty())
		{
			COM_ReportTargetBase target2;
			target2 = (COM_ReportTargetBase)target;
			pRep->targets_.back().prop2_.push_back(target2);
		}break;
	default: SRV_ASSERT(0); return;
	}

	if(target.fly_){
		//处理击飞
		if(p->asPlayer()){
			p->setProp(type,current);
			if(flee(p->asPlayer(),true))
				return;///飞了 并且战斗销毁
		}
		else if(p->asEmployee()){
			entities_.erase(std::find(entities_.begin(),entities_.end(),p));
			p->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild));
			current =  p->getProp(type);//飞了不做操作
		}
		else if(p->asRobot()){
			entities_.erase(std::find(entities_.begin(),entities_.end(),p));
			p->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET));
			current =  p->getProp(type);//飞了不做操作
		}
	}


	if(type == PT_HpCurr)
	{
		if(((S32)current <= 0) && p->getProp(type))
		{
			if(pRep)
			{
				Entity* caster = findEntityById (pRep->casterId_);

				if(caster != NULL && caster->asPlayer())
				{
					GEParam params[1];
					params[0].type_ = GEP_INT;
					params[0].value_.i = (S32)val;
					GameEvent::procGameEvent(GET_BattleChangeProp,params,1,caster->getHandleId());
				}
			}
		}
		if(current > p->getProp(PT_HpMax)){
			current = p->getProp(PT_HpMax);
		}
	}

	p->setProp(type,current);

	//ACE_DEBUG((LM_INFO,"changeProp changetype[%d]--target[%s]--targetPos[%d]--PropertyType[%d]--changePropVal[%f]--currentPropVal[%f]\n",cpt,p->getNameC(),pos,(S32)type,val,current));

	if(p->isDeadth())
	{
		if(pRep)
		{
			Entity* caster = findEntityById (pRep->casterId_);
			if(caster != NULL && caster->asPlayer())
			{
				calcPlayerKillEvent(caster->asPlayer(),p);
			}
		}

		p->clearState(); //死掉之后清除BUFF
		//怪物死亡移除战斗 新手战斗除外
		if(p->asMonster() && battleDataId_ != 1)
		{
			killmonsters_.push_back(p->asMonster());
			eraseEntityByPos(pos);
			regenPosTable();
		}
		else if(p->asBaby() && target.fly_)
			fly(p);
	}
}

void 
Battle::changeProp(S8 pos, PropertyType type,float val, bool bao)
{
	changeProp(CPT_Normal,(BattlePosition)pos,type,val,bao);
}

void
Battle::changePropBystate(S8 pos, PropertyType type, float val)
{
	changeProp(CPT_State,(BattlePosition)pos,type,val,false);
}

void
Battle::changePropByfanji(S8 pos, PropertyType type, float val, bool bao)
{
	changeProp(CPT_Fanji,(BattlePosition)pos,type,val,bao);
}

void
Battle::addActionCounter(U32 casterId, U32 uTargetPos, PropertyType type, float val,bool bao)
{
	if(getCurrentAction()->counters_.size() > COUNTER_MAX)
		return;

	COM_ReportActionCounter counter;
	counter.casterId_ = casterId;
	counter.targetPosition_ = (BattlePosition)uTargetPos;

	Entity *p = findEntityByPos(uTargetPos);
	if(NULL == p)
		return;
	float current = p->getProp(type) + val;
	p->setProp(type,current);

	COM_ReportTarget target;
	target.position_ = (BattlePosition)uTargetPos;
	target.prop_.type_ = type;
	target.prop_.value_ = (S32)current;
	target.bao_ = bao;
	counter.props_ = target;

	getCurrentAction()->counters_.push_back(counter);

	if(p->isDeadth())
	{
		//怪物死亡移除战斗 新手战斗除外
		if(p->asMonster() && battleDataId_ != 1)
		{
			killmonsters_.push_back(p->asMonster());
			eraseEntityByPos(uTargetPos);
			regenPosTable();
		}
	}
}

void 
Battle::changePosition(BattlePosition src, BattlePosition dest)
{
	Entity *pSrc = findEntityByPos(src);
	if(NULL == pSrc)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find src = %d at Battle::changePosition(BattlePosition src, BattlePosition dest)\n"),src));
		return ;
	}

	Entity *pDest = findEntityByPos(dest);
	if(NULL == pDest)
	{
		if(pSrc->asBaby())
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find dest = %d at Battle::changePosition(BattlePosition src, BattlePosition dest)\n"),dest));
			return ;
		}
		pSrc->battlePosition_ = dest;
	}
	else
	{
		if(pSrc->battlePosition_ == pDest->battlePosition_)
			ACE_DEBUG((LM_ERROR,ACE_TEXT(" Change position is equal!!!!\n")));
		BattlePosition bp = pSrc->battlePosition_;
		pSrc->battlePosition_ = pDest->battlePosition_;
		pDest->battlePosition_ = bp;

	}

	getCurrentAction()->bp0_ = src;
	getCurrentAction()->bp1_ = dest;

	regenPosTable();
	ACE_DEBUG((LM_INFO,ACE_TEXT("Changed position src = %d dest = %d \n"),src,dest));
}

void 
Battle::selectBaby(Player *player, U32 instId, bool isBattle)
{
	if(!isInThisBattle(player))
		return;

	Baby *pBaby = player->getBattleBaby();
	if(NULL != pBaby )
	{ ///从当前场景中移出这个宝宝
		if(isInThisBattle(pBaby))
		{
			entities_.erase(std::find(entities_.begin(),entities_.end(),pBaby));
		}
		pBaby->clearAttachedProperties(); //清除战斗临时附加属性
		if(pBaby->isDeadth())
			deadthBaby_.push_back(pBaby);//13320 BUG】战斗中宠物死亡后，收回还可以1血召出来
	}

	player->selectBaby(instId,isBattle);

	if(NULL != pBaby && pBaby->getGUID() == instId)
	{
		getCurrentAction()->status_ = OS_ShouBaobao; ///受宠是TRUE  刘兆南 say
		pBaby->clearAttachedProperties(); //清除战斗临时附加属性
		//pBaby->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH));
		return;
	}

	pBaby = player->findBaby(instId);

	if (pBaby == NULL)
	{
		ACE_DEBUG((LM_ERROR,"can not find baby id=%d!!!\n",instId));
		return;
	}

	if(pBaby->isDeadth())
	{
		ACE_DEBUG((LM_ERROR,"baby isDeadth id=%d!!!\n",instId));
		return;
	}
	
	pBaby->initBattleStatus(id_,player->battleForce_,calcFriendPosition(player));
	entities_.push_back(pBaby);
	pBaby->getBattleEntityInformation(getCurrentAction()->baby_);
	regenPosTable();
	
	getCurrentAction()->status_ = OS_FangBaobao; /// isOk 当前标记为是 放宝宝 环视收薄薄
	

}

bool 
Battle::flee(Player* player,bool isFly)
{
	if (!player)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Flee battleId = %d \n"),id_));
		return false;
	}
	
	COM_ReportAction* pra = getCurrentAction();
	if(NULL == pra)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Player flee has none action %d %d\n"),id_,player->getGUID()));
		player->battleRunaway_ = true;
		player->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild));
		Baby* baby = player->getBattleBaby();
		if(baby){
			baby->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild));
		}
		removePlayer(player,pra);
		
		return  false;
	}

	Entity* caster = findEntityById (pra->casterId_);
	if(caster != NULL && caster->asPlayer())
	{
		calcPlayerKillEvent(caster->asPlayer(),player);
	}
	
	S8 groupid = pra->uinteGroup_; ///被打飞 干掉合击
	for(int i=0; i<roundReport_.actionResults_.size(); ++i){
		if(roundReport_.actionResults_[i].uinteGroup_ == groupid){
			roundReport_.actionResults_[i].uinteGroup_ = 0;
			roundReport_.actionResults_[i].uniteNum_ = 0;
		}
	}

	pra->status_ = isFly ? OS_Flee:OS_RunawayOk;
	Baby* baby = player->getBattleBaby();
	if(baby)
	{
		pra->babyId_ = baby->getGUID();
	}
	
	player->battleRunaway_ = !isFly;
	
	if(player->battleForce_ == GT_Down)
		++downLosePlayerSum_;
	else
		++upLosePlayerSum_;

	player->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild));
	
	if(baby){
		baby->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET)&&(battleType_!=BT_PK1)&&(battleType_!=BT_PK2)&&(battleType_!=BT_Guild));
	}
	
	COM_BattleOverClearing com;
	
	com.isFly_ = isFly;
	com.playLevel_ = (S32)player->getProp(PT_Level);
	com.playFree_  = (U32)player->getProp(PT_Free);
	if(baby)
		com.babyLevel_ = (U32)baby->getProp(PT_Level);
	
	if(battleType_ == BT_PVR)
	{
		EndlessStair::instance()->calcRank(player,battleRobot_->getNameC(),false);
	}
	else if(battleType_ == BT_PVH)
	{
		player->hundredNum_-=1;

		COM_HundredBattle hb;
		hb.tier_	= player->tier_;
		hb.curTier_ = player->curTier_;
		hb.surplus_ = player->hundredNum_;
		hb.resetNum_= player->hundredresetNum_;
		CALL_CLIENT(player,syncHundredInfo(hb));
	}
	else if(battleType_ == BT_PK1 || battleType_ == BT_PK2)
	{
		calcBattlePKFlee(player);
	}
	

	removePlayer(player,(isFly?pra:NULL));
	regenPosTable();
	updateUnite();
	COM_BattleReport br = roundReport_;
	while(true){
		if(br.actionResults_.empty())
			break;
		else if(br.actionResults_.back().status_ == pra->status_ )
			break;
		else
			br.actionResults_.pop_back();
	}
	
	CALL_CLIENT(player,syncOneTurnAction(br)); ///这尼玛为啥在这里
	{
		///家族场景活动相关BOSS
		if(player->isInGuildScene() && (NULL == player->myGuild())){
			com.isFly_ = true; ///飞回主城
		}

	}
	CALL_CLIENT(player,exitBattleOk(BJT_Lose,com));
	
	Team* p = player->myTeam();
	if(p)p->exitTeam(player);

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = id_;
	GameEvent::procGameEvent(GET_Flee,params,1,player->getHandleId());

	return true;
}

void
Battle::fly(Entity* entity)
{
	entities_.erase(std::find(entities_.begin(),entities_.end(),entity));
	if(entity->asBaby()){
		entity->asBaby()->isBattle_ = false;
	}
	entity->cleanBattleStatus((battleType_!=BT_PVE)&&(battleType_!=BT_PVH)&&(battleType_!=BT_PET));
	
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = id_;
	GameEvent::procGameEvent(GET_Flee,params,1,entity->getHandleId());
}

Entity *
Battle::findEntityByPos(S8 pos)
{
	for (size_t i=0; i<entities_.size(); ++i)
	{
		if((S8)entities_[i]->battlePosition_ == pos)
		{
			return entities_[i];
		}
	}
	return NULL;
}

Entity *
Battle::findEntityById(U32 id)
{
	for (size_t i=0; i<entities_.size(); ++i)
		if(entities_[i]->getGUID() == id)
			return entities_[i];
	return NULL;
}

void 
Battle::eraseEntityByPos(S8 pos)
{
	for (size_t i=0; i<entities_.size(); ++i)
	{
		if((S8)entities_[i]->battlePosition_ == pos)
		{
			entities_.erase(entities_.begin()+i);
			return;
		}
	}
}

bool	
Battle::isInThisBattle(Entity *pEntity)
{
	if(NULL == pEntity)
		return false;
	return (std::find(entities_.begin(),entities_.end(),pEntity) != entities_.end());
}

S32
Battle::getIndex(U32 pId)
{
	for (size_t i=0; i<entities_.size(); ++i)
		if(entities_[i]->getGUID() == pId)
			return i;
	return -1;
}

bool
Battle::hasBaby(Player *player)
{
	return (player->getBattleBaby() != NULL);
}

bool 
Battle::isBattleBaby(U32 babyId)
{
	return findEntityById(babyId) != NULL;
}

BattlePosition 
Battle::calcFriendPosition(Entity *pEntity) ///计算友军位
{
	GroupType gt = pEntity->battleForce_;
	BattlePosition bp = pEntity->battlePosition_;

	enum{ POSITION_STEP = 5};

	if(GT_Up == gt){
		SRV_ASSERT(bp>=BP_Up0);
		SRV_ASSERT(bp<=BP_Up9);

		if(bp<=BP_Up4)
		{
			return (BattlePosition)(bp+POSITION_STEP);
		}
		else
		{
			return (BattlePosition)(bp-POSITION_STEP);
		}

	}
	else if(GT_Down == gt){
		SRV_ASSERT(bp>=BP_Down0);
		SRV_ASSERT(bp<=BP_Down9);

		if(bp<=BP_Down4){
			return (BattlePosition)(bp+POSITION_STEP);
		}
		else{
			return (BattlePosition)(bp-POSITION_STEP);
		}
	}
	else
		SRV_ASSERT(0);

	return BP_Max;
}

void 
Battle::cleanPosTable()
{
	for(S32 bp=BP_Down0; bp<BP_Max;bp++)
	{
		posTable_[bp] = 0;
	}
}

void 
Battle::regenPosTable()
{
	cleanPosTable();
	for (size_t i=0; i<entities_.size(); ++i){
		setPosTable((BattlePosition)((S32)entities_[i]->battlePosition_),entities_[i]);
	}
}

void 
Battle::setPosTable(BattlePosition bp, Entity *p)
{
	if(bp==BP_Max)
		return;

	posTable_[bp] = p->handleId_;
}

bool
Battle::isInvalid(Entity *p)
{
	if(p->isDeadth() || p->battleRunaway_)
		return true;
	
	return false;
}

void
Battle::prepareOrder()
{
	RAList &ral = actions();
	for(size_t i=0; i<ral.size(); ++i){
		Entity* ent = findEntityById(ral[i].casterId_);
		if(NULL == ent){
			ACE_DEBUG((LM_ERROR, "Caster is nil battle %d caster %d\n",id_,ral[i].casterId_));
			ral.erase(ral.begin() + i--);
		}
		else if(checkState(ent)){
			ral.erase(ral.begin() + i--);
		}
	}

}

bool
Battle::checkState(Entity *p)
{	
	if(NULL == p)
		return false;
	if(p->checkState(ST_Sleep))
		return true;
	else if(p->checkState(ST_Basilisk))
		return true;
	return false;
}

BattlePosition
Battle::getHuwei(BattlePosition target)
{
	Entity* ent = findEntityByPos(target);
	if(NULL == ent)
		return BP_None;
	for (size_t i=0; i<huwei_.size(); ++i){
		if(huwei_[i].first == target){
			Entity* p = findEntityByPos(huwei_[i].second);
			if(NULL == p)
				continue;
			else if(isInvalid(p))
				continue;
			else 
				return huwei_[i].second;
		}
	}
	return BP_None;
}

void 
Battle::removePlayer(Player *player,COM_ReportAction* pra){
	S32 index = getIndex(player->getGUID());
	if(-1 == index){
		return ;
	}
	
	for(size_t i=0; i<entities_.size(); ++i){
		if(entities_[i] == player){
			player->battleId_ = 0;
			entities_.erase(entities_.begin() + i--);
		}
		else{
			Baby *b = entities_[i]->asBaby();
			if(b && b->getOwner() == player){
				entities_.erase(entities_.begin() + i--);
				if(pra)
					pra->eraseEntities_.push_back(b->getGUID());
				continue;
			}
			
			Employee* e =  entities_[i]->asEmployee();
			if(e && e->getOwner() == player){
				entities_.erase(entities_.begin() + i--);
				if(pra)
					pra->eraseEntities_.push_back(e->getGUID());
				continue;
			}
			
		}
	}

	for(size_t i=0; i<deadthBaby_.size(); ++i){
		if(deadthBaby_[i]->getOwner() == player){
			deadthBaby_.erase(deadthBaby_.begin() + i--);
		}
	}

}

void Battle::calcBattleRewardPVE(){
	for (size_t i=0; i<entities_.size(); ++i){
		Player* player = entities_[i]->asPlayer();
		if(player){
			calcBattlePlayerRewardPVE(player);
		}
	}
}

void Battle::calcBattlePlayerRewardPVE(Player* player){
	Baby* baby = player->getBattleBaby();
	
	COM_BattleOverClearing boc;
	for(int i=0; i<monsters_.size(); ++i){
		Monster* pMonster = monsters_[i];

		if(pMonster == NULL){
			//ACE_DEBUG((LM_ERROR,"Calc total monster exp not finded momster %d why ? \n", pMonster->monsterId_));
			continue;
		}
		if(player->getForce() == pMonster->getForce())
			continue;
		else if(!pMonster->isDeadth())
			continue;
		const DropTable::Drop* pDrop = DropTable::getDropById(pMonster->monsterDropId_);

		if(NULL == pDrop){
			//ACE_DEBUG((LM_ERROR,"Calc total monster exp not finded drop %d why ? \n", pMonster->monsterDropId_));
			continue;
		}
		float exp = 0;
		float babyExp = 0;
		CALC_NORMAL_EXP(pDrop->exp_,player->getProp(PT_Level),monsters_[i]->getProp(PT_Level),exp);
		if(baby)
			CALC_NORMAL_EXP(pDrop->exp_,baby->getProp(PT_Level),monsters_[i]->getProp(PT_Level),babyExp);
		Team* pTeam = player->myTeam();
		if(pTeam)
		{
			exp = pTeam->calcTeamMemberExp(exp);
			if(player->isTeamLeader() && pTeam->getMemberSize() >= 2){
				exp = CALC_LEADER_EXP(exp);
			}
		} 
		boc.playExp_ += exp;
		boc.babyExp_ += babyExp;
		boc.money_ += pDrop->money_;
		if(!pDrop->items_.empty()){
			for(size_t l=0; l<pDrop->items_.size(); ++l){
				S32 dropItemNum = 0;
				const ItemTable::ItemData* item = ItemTable::getItemById(pDrop->items_[l].itemId_);
				if(NULL == item)
					continue;
				if(item->mainType_ == IMT_Quest){
					dropItemNum = player->checkQuestItem(pDrop->items_[l].itemId_); //任务道具截断
					if(0 == dropItemNum)
						continue;
				}
				
				bool hasone = false;
				for(size_t k=0; k<boc.items_.size(); ++k){
					if(boc.items_[k].itemId_ == pDrop->items_[l].itemId_){
						hasone = true;
						boc.items_[k].itemNum_ += pDrop->items_[l].itemNum_;
						break;
					}
				}
				if(!hasone){
					COM_DropItem di ;
					di.itemId_ = pDrop->items_[l].itemId_;
					di.itemNum_ =  pDrop->items_[l].itemNum_;
					boc.items_.push_back(di);
				}
			}
		}
	}
	boc.playLevel_ = (int)player->getProp(PT_Level);
	//玩家顶级后不给经验
	if(player->getProp(PT_Level) < Global::get<float>(C_PlayerMaxLevel)){
		float tmpExp = player->getProp(PT_Exp); 
		player->addExp(boc.playExp_);
		boc.playExp_ = player->getProp(PT_Exp) - tmpExp;
	}
	boc.playLevel_ =  (int)player->getProp(PT_Level) - boc.playLevel_;
	boc.playFree_  = player->getProp(PT_Free);
	boc.playLevel_ =  (int)player->getProp(PT_Level);
	//player->setProp(PT_Free,boc.playFree_);
	boc.skills_ = player->calcSkillExp();
	player->addMoney(boc.money_);
	for(size_t i=0; i<boc.items_.size(); ++i){
		player->addBagItemByItemId(boc.items_[i].itemId_,boc.items_[i].itemNum_,false,9);
	}
	if(baby){
		if(player->getProp(PT_Level) < Global::get<float>(C_PlayerMaxLevel)+5){
			float tmpExp = baby->getProp(PT_Exp); 
			baby->addExp(boc.babyExp_); 
			boc.babyExp_ = baby->getProp(PT_Exp) - tmpExp;
		}
		boc.babyLevel_ = baby->getProp(PT_Level);
		//boc.babyProp_ = baby->properties_;
	}

	BattleJudgeType bst ;
	if(battleWinner_ == GT_Down){
		bst = player->battleForce_ == GT_Down ? BJT_Win : BJT_Lose;
	}
	else if(battleWinner_ == GT_Up){
		bst = player->asPlayer()->battleForce_ == GT_Up ? BJT_Win : BJT_Lose;
	}else 
		bst = BJT_Lose;
	checkCleaner(boc);
	if(battleType_ == BT_PET && bst == BJT_Win)
	{
		enum {
			ARG0,
			ARG_MAX_,
		};
		GEParam param[ARG_MAX_];
		param[ARG0].type_  = GEP_INT;
		param[ARG0].value_.i = 1;
		GameEvent::procGameEvent(GET_Pet,param,ARG_MAX_,player->getHandleId());
	}
	{
		///家族场景活动相关BOSS
		if(player->isInGuildScene() && (NULL == player->myGuild())){
			boc.isFly_ = true; ///飞回主城
		}

	}
	CALL_CLIENT(player,exitBattleOk(bst,boc));

	SGE_LogProduceTrack track;
	track.playerId_ = player->getGUID();
	track.playerName_ = player->getNameC();
	track.from_ = 9;
	track.money_ = boc.money_;
	track.exp_ = boc.playExp_;
	LogHandler::instance()->playerTrack(track);

	if(bst == BJT_Win)
		checkKillMonster(player);
}

void Battle::calcBattleRewardPVR(){
	Entity* pDown = NULL;
	Entity* pUp   = NULL;

	for (size_t i = 0; i < entities_.size(); ++i){
		if (entities_[i]->asPlayer()){
			Baby* pBaby = entities_[i]->asPlayer()->getBattleBaby();
			BattleJudgeType bst ;
			if(battleWinner_ == GT_Down){
				bst = entities_[i]->asPlayer()->battleForce_ == GT_Down ? BJT_Win : BJT_Lose;
			}
			else if(battleWinner_ == GT_Up){
				bst = entities_[i]->asPlayer()->battleForce_ == GT_Up ? BJT_Win : BJT_Lose;
			}else 
				bst = BJT_Lose;

			COM_BattleOverClearing boc ;
			if(bst == BJT_Win){
				bool isWin = false;
				if(battleWinner_ == GT_Down){
					isWin = true;
				}
				if(battleRobot_)
					EndlessStair::instance()->calcRank(entities_[i],battleRobot_->getNameC(),isWin);
			}

			if(pBaby){
				boc.babyLevel_ = pBaby->getProp(PT_Level);
				//boc.babyProp_ = baby->properties_;
			}

			boc.playLevel_ = entities_[i]->asPlayer()->getProp(PT_Level);
			boc.playFree_ = entities_[i]->asPlayer()->getProp(PT_Free);
			checkCleaner(boc);
			CALL_CLIENT(entities_[i]->asPlayer(),exitBattleOk(bst,boc));
		}
	}
}

void Battle::calcBattleRewardPVH(){
	for (size_t i=0; i<entities_.size(); ++i){
		Player* player = entities_[i]->asPlayer();
		if(player){
			calcBattlePlayerRewardPVH(player);
		}
	}
}

void Battle::calcBattlePlayerRewardPVH(Player* player){
	Baby* baby = player->getBattleBaby();

	COM_BattleOverClearing boc;
	for(int i=0; i<monsters_.size(); ++i){
		Monster* pMonster = monsters_[i];
		if(pMonster == NULL){
			//ACE_DEBUG((LM_ERROR,"Calc total monster exp not finded momster %d why ? \n", pMonster->monsterId_));
			continue;
		}
		if(player->getForce() == pMonster->getForce())
			continue;
		else if(!pMonster->isDeadth())
			continue;
		const DropTable::Drop* pDrop = DropTable::getDropById(pMonster->monsterDropId_);

		if(NULL == pDrop){
			ACE_DEBUG((LM_ERROR,"Calc total monster exp not finded drop %d why ? \n", pMonster->monsterDropId_));
			continue;
		}
		float exp = 0;
		float babyExp = 0;
		CALC_NORMAL_EXP(pDrop->exp_,player->getProp(PT_Level),monsters_[i]->getProp(PT_Level),exp);
		if(baby)
			CALC_NORMAL_EXP(pDrop->exp_,baby->getProp(PT_Level),monsters_[i]->getProp(PT_Level),babyExp);
		if(player->isTeamLeader()){
			exp = CALC_LEADER_EXP(exp);
		} 

		boc.playExp_ += exp;
		boc.babyExp_ += babyExp;
		boc.money_ += pDrop->money_;
		if(!pDrop->items_.empty())
		{
			for(size_t l=0; l<pDrop->items_.size(); ++l){
				S32 dropItemNum = 0;
				const ItemTable::ItemData* item = ItemTable::getItemById(pDrop->items_[l].itemId_);
				if(NULL == item)
					continue;
				if(item->mainType_ == IMT_Quest)
				{
					dropItemNum = player->checkQuestItem(pDrop->items_[l].itemId_); //任务道具截断
					if(0 == dropItemNum)
						continue;
				}

				bool hasone = false;
				for(size_t k=0; k<boc.items_.size(); ++k){
					if(boc.items_[k].itemId_ == pDrop->items_[l].itemId_){
						hasone = true;
						boc.items_[k].itemNum_ += pDrop->items_[l].itemNum_;
						break;
					}
				}
				if(!hasone){
					COM_DropItem di ;
					di.itemId_ = pDrop->items_[l].itemId_;
					di.itemNum_ =  pDrop->items_[l].itemNum_;
					boc.items_.push_back(di);
				}
			}
		}
	}
	boc.playLevel_ = (int)player->getProp(PT_Level);
	//玩家顶级后不给经验
	if(player->getProp(PT_Level) < Global::get<float>(C_PlayerMaxLevel))
	{
		float tmpExp = player->getProp(PT_Exp); 
		player->addExp(boc.playExp_);
		boc.playExp_ = player->getProp(PT_Exp) - tmpExp;
	}
	boc.playLevel_ =  (int)player->getProp(PT_Level) - boc.playLevel_;
	boc.playFree_  = player->getProp(PT_Free);
	boc.playLevel_ =  (int)player->getProp(PT_Level);
	//player->setProp(PT_Free,boc.playFree_);
	if(player->curTier_ >= player->tier_)	//当前挑战小于当前可挑战,不给予奖励
	{
		boc.skills_ = player->calcSkillExp();
		player->addMoney(boc.money_);
		for(size_t i=0; i<boc.items_.size(); ++i){
			player->addBagItemByItemId(boc.items_[i].itemId_,boc.items_[i].itemNum_,false,9);
		}

		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 9;
		track.money_ = boc.money_;
		track.exp_ = boc.playExp_;
		LogHandler::instance()->playerTrack(track);
	}
	
	if(baby){
		if(player->getProp(PT_Level) < Global::get<float>(C_PlayerMaxLevel)+5){
			float tmpExp = baby->getProp(PT_Exp); 
			baby->addExp(boc.babyExp_); 
			boc.babyExp_ = baby->getProp(PT_Exp) - tmpExp;
		}
		boc.babyLevel_ = baby->getProp(PT_Level);
		//boc.babyProp_ = baby->properties_;
	}

	BattleJudgeType bst ;
	if(battleWinner_ == GT_Down){
		bst = player->battleForce_ == GT_Down ? BJT_Win : BJT_Lose;
	}
	else if(battleWinner_ == GT_Up){
		bst = player->asPlayer()->battleForce_ == GT_Up ? BJT_Win : BJT_Lose;
	}else 
		bst = BJT_Lose;
	checkCleaner(boc);
	CALL_CLIENT(player,exitBattleOk(bst,boc));
	if(bst == BJT_Win)
	{
		if(player->curTier_ > player->tier_)
			player->tier_ = player->curTier_;

		if(player->curTier_ < player->tier_)	//当前挑战的和当前可挑战的不同,不给予奖励
			return;
		ChallengeTable::Core const *tmp = ChallengeTable::getDataById(player->curTier_);
		if(tmp == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("caleReward Don't find this Data[%d] in the ChallengeTable table\n"),player->curTier_));
			return;
		}
		if(player->curTier_ == Global::get<int>(C_HundredTier))
		{	
			player->tier_ = 1;
			player->hundredNum_-=1;
			COM_HundredBattle hb;
			hb.tier_	= player->tier_;
			hb.curTier_ = player->curTier_;
			hb.surplus_ = player->hundredNum_;
			hb.resetNum_= player->hundredresetNum_;
			CALL_CLIENT(player,syncHundredInfo(hb));
		}
		else
		{
			++player->tier_;
			COM_HundredBattle hb;
			hb.tier_	= player->tier_;
			hb.curTier_ = player->curTier_;
			hb.surplus_ = player->hundredNum_;
			hb.resetNum_= player->hundredresetNum_;
			CALL_CLIENT(player,syncHundredInfo(hb));
			player->scenePlayer_->setEntryFlag(player->playerId_,true);
		}
		
	}
	else
	{
		player->hundredNum_-=1;
		COM_HundredBattle hb;
		hb.tier_	= player->tier_;
		hb.curTier_ = player->curTier_;
		hb.surplus_ = player->hundredNum_;
		hb.resetNum_= player->hundredresetNum_;
		CALL_CLIENT(player,syncHundredInfo(hb));
	}

	enum {
		ARG0,
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG0].type_  = GEP_INT;
	param[ARG0].value_.i = 1;
	GameEvent::procGameEvent(GET_Challenge,param,ARG_MAX_,player->getHandleId());
}

void Battle::calcBattleRewardGuild(){
	
	Guild* pWinGuild = NULL;
	for(size_t i=0; i<entities_.size();++i){
		Player* player = entities_[i]->asPlayer();
		if(player){
			player->noBattleTime_ = Global::get<int>(C_NoBattleTime);
			COM_BattleOverClearing boc ;
			BattleJudgeType bst = player->battleForce_ == battleWinner_ ? BJT_Win : BJT_Lose;
			
			if(!pWinGuild && (BJT_Win == bst)){
				pWinGuild = player->myGuild();
			}
			if(BJT_Lose == bst){
				if(player->battleForce_ == GT_Down)
					++downLosePlayerSum_;
				else 
					++upLosePlayerSum_;
			}
			
			boc.playLevel_ = entities_[i]->asPlayer()->getProp(PT_Level);
			boc.playFree_ = entities_[i]->asPlayer()->getProp(PT_Free);
			Baby* pbaby = player->getBattleBaby();
			if(pbaby)
				boc.babyLevel_ = pbaby->getProp(PT_Level);
			checkCleaner(boc);
			CALL_CLIENT(player,exitBattleOk(bst,boc));
		}
	}
	if(!pWinGuild)
		return;
	if(guildNpcId_){
		Scene* s = SceneManager::instance()->getScene(guildSceneId_);
		if(s)
			s->delNpc(guildNpcId_);
		pWinGuild->addGuildBattleWinCount(5);
	}
	else{
		pWinGuild->addGuildBattleWinCount(battleWinner_ == GT_Down ? upLosePlayerSum_ : downLosePlayerSum_);
	}
}

void
Battle::calcBattleRewardPVP()
{
	for(size_t i=0; i<entities_.size();++i){
		Player* player = entities_[i]->asPlayer();
		if(player)
		{
			//player->caleSinglePvpPlayerGrade(battleWinner_);
			COM_BattleOverClearing boc ;
			BattleJudgeType bst ;
			if(battleWinner_ == GT_Down){
				bst = player->battleForce_ == GT_Down ? BJT_Win : BJT_Lose;
			}
			else if(battleWinner_ == GT_Up){
				bst = player->battleForce_ == GT_Up ? BJT_Win : BJT_Lose;
			}else 
				bst = BJT_Lose;
			/*PvpRunkTable::PvpRunkDate const* pCore = PvpRunkTable::getPvpRunkById(player->pvpInfo_.section_);
			if(pCore == NULL)
				continue;*/
			U32 dropid = Warriorchoose::instance()->sendtrophy(player,battleWinner_ == player->battleForce_?true:false);
			const DropTable::Drop* pDrop = DropTable::getDropById(dropid);
			if(NULL == pDrop){
				ACE_DEBUG((LM_ERROR,"Calc total calcBattleRewardPVP finded drop %d why ? \n", dropid));
				continue;
			}

			if(!pDrop->items_.empty())
			{
				for(size_t l=0; l<pDrop->items_.size(); ++l){
					S32 dropItemNum = 0;
					const ItemTable::ItemData* item = ItemTable::getItemById(pDrop->items_[l].itemId_);
					if(NULL == item)
						continue;
					if(item->mainType_ == IMT_Quest){
						dropItemNum = entities_[i]->asPlayer()->checkQuestItem(pDrop->items_[l].itemId_); //任务道具截断
						if(0 == dropItemNum)
							continue;
					}
					else{
						dropItemNum = pDrop->items_[l].itemNum_;
					}

					bool hasone = false;
					for(size_t k=0; k<boc.items_.size(); ++k){
						if(boc.items_[k].itemId_ == pDrop->items_[l].itemId_){
							hasone = true;
							boc.items_[k].itemNum_ += dropItemNum;
							break;
						}
					}
					if(!hasone){
						COM_DropItem di ;
						di.itemId_ = pDrop->items_[l].itemId_;
						di.itemNum_ =  dropItemNum;
						boc.items_.push_back(di);
					}
				}
			}
			boc.pvpJJCGrade_ = player->battleValue_;
			boc.playLevel_ = entities_[i]->asPlayer()->getProp(PT_Level);
			boc.playFree_ = entities_[i]->asPlayer()->getProp(PT_Free);

			Baby* pbaby = player->getBattleBaby();
			if(pbaby)
				boc.babyLevel_ = pbaby->getProp(PT_Level);

			checkCleaner(boc);
			//player->promoteAward(battleType_);
			CALL_CLIENT(player,exitBattleOk(bst,boc));
		}
	}
}

void
Battle::calcBattleReward()
{
	if (battleType_ == BT_PVE|| 
		battleType_ == BT_PET){
		calcBattleRewardPVE();
	}
	else if (battleType_ == BT_PVR){
		calcBattleRewardPVR();
	}
	else if (battleType_ == BT_PVH){
		calcBattleRewardPVH();
	}
	else if (battleType_ == BT_Guild){
		calcBattleRewardGuild();
	}
	else if (battleType_ == BT_PVP){
		calcBattleRewardPVP();
	}
	else if(battleType_ == BT_PK1 || BT_PK2 == battleType_){
		calcBattleRewardPK();
	}
}

void
Battle::checkKillMonster(Player* player){
	if(player == NULL)
		return;
	for (size_t i=0; i<killmonsters_.size(); ++i)
	{
		if(killmonsters_[i] == NULL)
			continue;
		player->postQuestKillAiEvent(atoi(killmonsters_[i]->monsterClass_.c_str()),1);
		player->postQuestEvent(killmonsters_[i]->monsterId_,1);
	}
}

Monster*
Battle::findMonster(U32 id)
{
	for (size_t i = 0; i < monsters_.size(); ++i){
		if(monsters_[i]->monsterId_ == id)
			return monsters_[i];
	}

	return NULL;
}

BattlePosition
Battle::findPosition(Entity* pEntity,GroupType type)
{
	if (type == GT_Down){
		for (size_t i = 0; i < 5; ++i){
			Entity* pEnt1 = findEntityByPos(downPosition[i]);
			Entity* pEnt2 = findEntityByPos(downPosition[i] + 5);
			if (pEnt1 == NULL && pEnt2 == NULL){
				if(downPosition[i] == BP_Down2 && pEntity->asEmployee())
					continue;

				return downPosition[i];
			}
		}
	}

	if (type == GT_Up){
		for (size_t i = 0; i < 5; ++i){
			Entity* pEnt1 = findEntityByPos(upPosition[i]);
			Entity* pEnt2 = findEntityByPos(upPosition[i] + 5);
			if (pEnt1 == NULL && pEnt2 == NULL){
				if(upPosition[i] == BP_Up2 && pEntity->asEmployee())
					continue;

				return upPosition[i];
			}
		}
	}
	ACE_DEBUG((LM_ERROR, "Find Position BP_None\n"));
	return BP_None;
}

BattlePosition
Battle::findMonsterPosition(){
	for (size_t i = 0; i < 10; ++i){
		Entity* pEntity = findEntityByPos(monsterPosition[i]);

		if(pEntity == NULL)
			return monsterPosition[i];
	}
	ACE_DEBUG((LM_ERROR, "Find Monster Position BP_None\n"));
	return BP_None;
}

void
Battle::changeReportSkillId(U32 skillId,S8 pos){

	Entity* pEntity = findEntityByPos(pos);
	if(NULL == pEntity)
	{
		ACE_DEBUG((LM_ERROR, "Battle::changeReportSkillId(U32 skillId,S8 pos){ if(NULL == pEntity) %d\n",pos));
		return ;
	}

	COM_ReportAction * pra = getCurrentAction();
	if(pra)
	{
		if(pra->casterId_  != pEntity->getGUID())
		{
			ACE_DEBUG((LM_ERROR, "Battle::changeReportSkillId(U32 skillId,S8 pos){ if(pra->casterId_  != pEntity->getGUID() %d,%d) \n",pra->casterId_ ,pEntity->getGUID()));
			return ;
		}
		
		Entity* caster = findEntityById(pra->casterId_);
		if(caster){
			Skill *sk = caster->getSkillById(skillId);
			if(sk){
				pra->skillLevel_ = sk->skLevel_;
			}
		}

		pra->skill_ = skillId;	
	}
}

void
Battle::changeOrderSkillId(U32 skillId){
	if(getCurrentOrder() == NULL)
		return;
	getCurrentOrder()->skill_ = skillId;
}

void Battle::changeOrderTarget(S8 pos){
	if(getCurrentOrder() == NULL)
		return;
	getCurrentOrder()->target_ = pos;
}

void
Battle::checkUnite(){
	RAList &ral = actions();
	enum{
		SKILL	= 1,			//普攻技能ID
		UNITERATE = 50,			//合击概率
		MAXRATE = 100
	};
	if(ral.empty())
		return;
	
	U8	untieGroup      = 1;
	U32 untieNum		= 1;
	U32 lasttargetPos	= ral[0].target_;
	GroupType lastforce = GT_None;
	U32 lastSkill		= ral[0].skill_;

	Entity* pLastEntity = findEntityById(ral[0].casterId_);

	if(pLastEntity)
		lastforce = pLastEntity->battleForce_;

	U32 roll = UtlMath::randN(MAXRATE);

	for (size_t i = 1; i < ral.size(); ++i){
		Entity* pEtmp = findEntityById(ral[i].casterId_);

		if (pEtmp != NULL && pLastEntity != pEtmp &&
			ral[i].target_  == lasttargetPos && 
			pEtmp->battleForce_ == lastforce && 
			lastSkill == SKILL && 
			ral[i].skill_ == SKILL && 
			!(ral[i].isNo_) && 
			!(ral[i-1].isNo_)){
			COM_Order *oldOrder = findOrder(ral[i].casterId_);
			if(&ral[i] == oldOrder || oldOrder->uinteGroup_ == 0) //相同caster合击二动时,只计算一次
			{
				++untieNum;
				for (size_t j = i-untieNum + 1; j <= i; ++j)
				{
					ral[j].uinteGroup_ = untieGroup;
					ral[j].uniteNum_ = untieNum;
				}
			}
			
		}
		else
		{
			++untieGroup;
			untieNum		= 1;
			lasttargetPos	= ral[i].target_;
			lastSkill		= ral[i].skill_;

			pLastEntity = findEntityById(ral[i].casterId_);

			if(pLastEntity)
				lastforce = pLastEntity->battleForce_;
		}
	}
}

void Battle::updateUnite()
{
	COM_ReportAction *pra = getCurrentAction();

	if(pra->uinteGroup_ == 0)
		return;
	if(pra->uniteNum_ == 0)
		return;
	RAList &ral = actions();
	Entity *target = findEntityByPos(pra->target_);
	if(NULL == target || target->isDeadth()){ ///第一个合击人 把所有的合击都情调
		if(actionIndex_ == 0 || ral[actionIndex_-1].uinteGroup_ != pra->uinteGroup_){
			S8 groupid = pra->uinteGroup_;
			for(int i=actionIndex_; i<ral.size(); ++i){
				if(ral[i].uinteGroup_ == groupid){
					ral[i].uinteGroup_ = ral[i].uniteNum_ = 0;
				}
			}
			return ;
		}
	}
	int sum = 0;
	for (size_t i=0; i<ral.size(); ++i)
	{
		if(ral[i].uinteGroup_ == pra->uinteGroup_)
		{
			Entity *c = findEntityById(ral[i].casterId_);
			if(NULL == c)
				continue;
			else if(isInvalid(c))
				continue;
			else if(checkState(c))
				continue;
			else if(findEntityByPos(ral[i].target_) == NULL)
				continue;
			else
				++sum;
		}
	}
	if(sum != pra->uniteNum_)
	{
		//ACE_DEBUG((LM_INFO,"UPDATE UNITE CASTER ===>caster[%d]==group[%d]==unite[%d]==new[%d]\n",pra->casterId_,pra->uinteGroup_, pra->uniteNum_,sum));
		pra->uniteNum_ = sum;
	}
	
	for(size_t i=0; i<roundReport_.actionResults_.size(); ++i){
		if(roundReport_.actionResults_[i].uinteGroup_ == pra->uinteGroup_){
			roundReport_.actionResults_[i].uniteNum_ = sum;
		}
	}
}

void
Battle::initAI()
{
	for (size_t i = 0; i < entities_.size(); ++i)
	{
		if(entities_[i]->isBattleAtkTimeout())
			entities_[i]->postEvent(ME_Born, (entities_[i]->asMonster() || entities_[i]->asRobot()) ? BP_Down2 : BP_Up2 , posTable_);
	}
}
void
Battle::update(Entity* ent)
{
	if(NULL == ent)
		return;
	if(ent->isDeadth())
		return;
	if(ent->asBaby()){
		InnerPlayer* p = ent->asBaby()->getOwner();
		if(NULL == p)
			return;
		else if(p->isDeadth())
			return;
	}
	
	regenPosTable();

	if(ent->asPlayer() || ent->asBaby()){
		if(ent->lastOrder_.target_ != 0){
			pushOrder(ent->lastOrder_);
		}else{
			ent->postEvent(ME_Update,BP_Up0,posTable_);
		}
		return;
	}
	else if(ent->asEmployee())
	{
		ent->postEvent(ME_Update,BP_Up0,posTable_);
		return;
	}

	switch(battleType_){
		case BT_PVE:
		case BT_PVH:
		case BT_PET:
			{
				ent->postEvent(ME_Update,ent->asMonster() ? BP_Down0 : BP_Up0,posTable_);
				break;
			}
		case BT_PVR:
			{
				ent->postEvent(ME_Update,ent->asRobot() ? BP_Down2 : BP_Up0,posTable_);
				break;
			}
		case BT_PVP:
		case BT_PK2:
		case BT_PK1:
		case BT_Guild:
			{
				ent->postEvent(ME_Update,BP_Up0,posTable_);
				break;
			}
		default:
			{
				ACE_DEBUG((LM_DEBUG,"Battle type not update ai!!!\n"));
				break;		
			}
	}
}

void Battle::catchBaby(S8 pos,Entity* handle)
{
	Entity* pEntity = findEntityByPos(pos);
	if(NULL == pEntity)
		return;
	Monster* pMonster = pEntity->asMonster();
	if(NULL == pMonster)
		return;
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(pMonster->monsterId_);
	if(NULL == tmp)
		return;
	if(tmp->monsterType_ != 1)
		return; //这不是个宠物
	else if(!tmp->pet_)
		return; //这不是个宠物
	else if(!tmp->petProbability_)
		return; //这宠物不能被抓
	else if(handle->isDeadth())
		return; //死的不能抓 TODO 
	else if(pMonster->isDeadth())
		return;

	float base = tmp->petProbability_ , res = 0;
	CALC_CATCH_BABY(handle,pEntity,base,res);
	
	bool isOk = false;
	
	isOk = UtlMath::frand() <= res;
	
	COM_ReportAction* pAction = getCurrentAction();
	if(NULL == pAction)
		return;
	pAction->target_ = pos;
	Entity* pCasterEntity = findEntityById(pAction->casterId_);
	if(NULL == pCasterEntity)
		return;
	Player* pPlayer = pCasterEntity->asPlayer();
	if(NULL == pPlayer)
		return;
	if(!pPlayer->getItemNumByItemId(Global::get<int>(C_CatchPetItemID)))///检查封魔卡,以前是怎么扣得是个谜
		return;
	//pPlayer->delBagItemByItemId(Global::get<int>(C_CatchPetItemID));
	if(pPlayer->babies_.size() >= Global::get<int>(C_BabyMax)){
		S32 slot = pPlayer->findBabyStorageFirstemptySlot();
		if(slot == -1){
			CALL_CLIENT(pPlayer,errorno(EN_BabyStorageFull));
			isOk = false;
		}
		if(isOk)
		{
			pPlayer->addBaby(pMonster->getProp(PT_TableId),true);
			CALL_CLIENT(pPlayer,errorno(EN_BabyFullToStorage));
		}
	}
	else{
		if(isOk)
			pPlayer->addBaby(pMonster->getProp(PT_TableId),false);
	}

	pAction->status_ = OS_Zhuachong;
	pAction->skillLevel_ = 1;
	pAction->zhuachongOk_ = isOk;
	if(isOk){
		eraseEntityByPos(pos);

		GEParam params[1];
		params[0].type_ = GEP_INT;
		params[0].value_.i = 1;
		GameEvent::procGameEvent(GET_CatchBaby,params,1,pPlayer->getHandleId());
	}
}

S32
Battle::findMaxMpPos(GroupType type)
{
	if(entities_.empty())
		return -1;
	std::vector<Entity*>	forceEnt;
	for (size_t i = 0; i < entities_.size(); ++i){
		if (entities_[i]->battleForce_ == type){
			forceEnt.push_back(entities_[i]);
		}
	}
	if(forceEnt.empty())
		return -1;
	float curMaxMp = forceEnt[0]->getProp(PT_MpCurr);
	S32 curMaxMpPos = forceEnt[0]->battlePosition_;

	for (size_t i = 0; i < forceEnt.size(); ++i){
		if (forceEnt[i]->battleForce_ == type){
			if(forceEnt[i]->getProp(PT_MpCurr) > curMaxMp){
				curMaxMp = forceEnt[i]->getProp(PT_MpCurr);
				curMaxMpPos = forceEnt[i]->battlePosition_;
			}
		}
	}
	return curMaxMpPos;
}

S32
Battle::findMinHpPos(GroupType type)
{	
	if(entities_.empty())
		return -1;

	std::vector<Entity*>	forceEnt;
	forceEnt.clear();
	for (size_t i = 0; i < entities_.size(); ++i){
		if (entities_[i]->battleForce_ == type){
			forceEnt.push_back(entities_[i]);
		}
	}
	if(forceEnt.empty())
		return -1;

	float curMinHp = forceEnt[0]->getProp(PT_HpCurr);
	S32 curMinHpPos = forceEnt[0]->battlePosition_;
	for (size_t i = 0; i < forceEnt.size(); ++i){
		if(forceEnt[i]->getProp(PT_HpCurr) < curMinHp){
			curMinHp = forceEnt[i]->getProp(PT_HpCurr);
			curMinHpPos = forceEnt[i]->battlePosition_;
		}
	}

	return curMinHpPos;
}

S32
Battle::findMaxHpPos(GroupType type)
{	
	if(entities_.empty())
		return -1;

	std::vector<Entity*>	forceEnt;
	forceEnt.clear();
	for (size_t i = 0; i < entities_.size(); ++i){
		if (entities_[i]->battleForce_ == type){
			forceEnt.push_back(entities_[i]);
		}
	}
	if(forceEnt.empty())
		return -1;

	float curMaxHp = forceEnt[0]->getProp(PT_HpCurr);
	S32 curMaxHpPos = forceEnt[0]->battlePosition_;
	for (size_t i = 0; i < forceEnt.size(); ++i){
		if(forceEnt[i]->getProp(PT_HpCurr) > curMaxHp){
			curMaxHp = forceEnt[i]->getProp(PT_HpCurr);
			curMaxHpPos = forceEnt[i]->battlePosition_;
		}
	}

	return curMaxHpPos;
}


bool
Battle::checkForceHp(GroupType type, float valueHp)
{
	if(entities_.empty())
		return false;

	for (size_t i = 0; i < entities_.size(); ++i){
		if(type ==entities_[i]->battleForce_ && (entities_[i]->getProp(PT_HpCurr)/entities_[i]->getProp(PT_HpMax)) > valueHp )
			return false;
	}

	return true;
}

void
Battle::checkBattleSkill()
{
	COM_ReportAction *pra = getCurrentAction();

	Entity* caster = findEntityById(pra->casterId_);

	Skill *pSk = NULL;
	for (size_t i=0; i<caster->skills_.size(); ++i){
		if(caster->skills_[i]->skId_ == pra->skill_){
			pSk = caster->skills_[i];
			break;
		}
	}

	if(NULL == pSk)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find skill %d \n"),pra->skill_));
		return;
	}

	SkillTable::Core const* p = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);

	if(NULL == p)
		return;

	S32 mana = p->costMana_;

	if(mana)
		mana = mana - mana*caster->getSkillCostMana(pra->skill_);

	pSk->gloCondition_->condition(caster,(BattlePosition)pra->target_,id_,posTable_,orderParam_,mana);
}

BattlePosition
Battle::findBossPos()
{
	for (size_t i = 0; i < entities_.size(); ++i)
	{
		if(entities_[i]->asMonster() == NULL)
			continue;
		
		MonsterTable::MonsterData const* p = MonsterTable::getMonsterById(entities_[i]->asMonster()->getProp(PT_TableId));
		if(p == NULL)
			continue;
		if(p->monsterType_ == 2)
			return entities_[i]->battlePosition_;
	}

	return BP_None;
}

void
Battle::addMonster(std::vector<std::string>& className)
{
	if(className.empty())
		return;
	COM_ReportAction* pAction = getCurrentAction();
	if(NULL == pAction)
		return;
	pAction->status_ = OS_Summon;
	pAction->skillLevel_ = 1;

	for (size_t i = 0; i < className.size(); ++i)
	{
		const MonsterClass::Core* clazz = MonsterClass::getClassByName(className[i]);
		if(clazz == NULL)
			continue;
		BattlePosition pos = findMonsterPosition();
		if(pos == BP_None)
			continue;

		Monster* pMonster = NEW_MEM(Monster,className[i]);
		pMonster->initBattleStatus(id_,GT_Up,pos);
		COM_BattleEntityInformation info;
		pMonster->getBattleEntityInformation(info);
		pAction->dynamicEntities_.push_back(info);
		entities_.push_back(pMonster);
		monsters_.push_back(pMonster);
	}
	regenPosTable();
}

U32
Battle::getMonsterType(BattlePosition pos)
{
	Entity* pEntity = findEntityByPos(pos);
	if(pEntity == NULL || pEntity->asMonster() == NULL)
		return 0;
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(pEntity->asMonster()->getProp(PT_TableId));

	if(tmp == NULL)
		return 0;
	return tmp->monsterType_;
}

void
Battle::resetEntityProp(Entity *p)
{
	if(p == NULL)
		return;
	float hpm = p->getProp(PT_HpMax);
	p->setProp(PT_HpCurr,hpm);
	float mpm = p->getProp(PT_MpMax)/2;
	p->setProp(PT_MpMax,mpm);
	p->setProp(PT_MpCurr,mpm);
}

bool
Battle::findOrder(BattlePosition targetPos,U32 skillId)
{
	RAList &ral = actions();
	for (size_t i = 0; i < ral.size(); ++i){
		if(ral[i].target_ == targetPos && ral[i].skill_ == skillId)
			return true;
	}
	return false;
}

void 
Battle::allLose(){
	COM_BattleOverClearing cboc;
	for (size_t i=0; i<entities_.size(); ++i)
	{
		Player* player = entities_[i]->asPlayer();
		if(player){
			cboc.playLevel_ = (S32)player->getProp(PT_Level);
			cboc.playFree_  = (U32)player->getProp(PT_Free);
			CALL_CLIENT(player,exitBattleOk(BJT_Lose,cboc));
		}
	}
}