#include "config.h"
#include "battle.h"
#include "player.h"
#include "itemtable.h"
#include "skill.h"
#include "worldserv.h"


void 
Battle::pushRA(COM_Order& order,int sklevel){
	COM_ReportAction ra;
	*((COM_Order*)&ra) = order;
	ra.skillLevel_ = sklevel;
	roundReport_.actionResults_.push_back(ra);
 }

void
Battle::pushOrder(COM_Order &order)
{
	if(activeRound_)
		return;
	Entity *caster = findEntityById(order.casterId_);
	if(NULL == caster)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Push order caster[%d] is null\n"),order.casterId_));
		return;
	}

	Entity *target = findEntityByPos(order.target_);
	if(NULL == target)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Push order target is null\n")));
	}

	enum
	{
		MAX_ORDER = 2
	};
//	ACE_DEBUG((LM_INFO,"Push order battleId %d , battleStatus %d\n",id_,battleState_));

	if(caster->asPlayer() != NULL){
		if( caster->asPlayer()->getBattleBaby() != NULL ){
			if(orderCounter(caster) != 0){
				ACE_DEBUG((LM_DEBUG,"Player caster can not push 2 order with baby\n"));
				return;
			}
		}
	}
	else if(caster->asBaby() != NULL  && orderCounter(caster) != 0)
		return;
	else if(caster->asEmployee() != NULL  && orderCounter(caster) != 0)
		return;
	if (orderCounter(caster) == MAX_ORDER)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Push order Max == 2\n")));
		return;
	}

	if(caster->asBaby())
	{
		calcBabyLoyal(order);
	}
	
	int sklevel = 0;
	Skill* pSk = caster->getSkillById(order.skill_);
	if(pSk == NULL && order.itemId_ == 0 && order.weaponInstId_ == 0)
		return;
	if(pSk){
		SkillTable::Core const * pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);

		if(pCore && (pCore->skType_ == SKT_Passive || pCore->skType_ == SKT_DefaultSecPassive || pCore->skType_ == SKT_DefaultPassive)){
			U32 targetTmp = order.target_;
			cleanOderParam();
			setOrderParam(OPT_BabyId,order.babyId_);
			caster->castSkill(order.skill_,(BattlePosition)targetTmp,id_,posTable_,orderParam_);
		}	

		sklevel = pSk->skLevel_;
	}

	S32 index = getIndex(order.casterId_);
	if(-1 == index)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("can not find caster\n")));
		return ;
	}
	/// 2 动处理
	bool usedSec = caster->asPlayer() && !hasBaby(caster->asPlayer());
	if(usedSec){
		if (!checkSecondOrder(order)){
			return;
		}
	}
	else{
		caster->setBattleActive(true);
		for (size_t i=0; i<entities_.size(); ++i){
			if(entities_[i]->asPlayer()){
				CALL_CLIENT(entities_[i],syncOrderOk(order.casterId_));
			}
		}
	}

	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Push order OK caster(%d)--skill(%d)--targer(%d)\n"),order.casterId_, order.skill_, order.target_));
	
	hasPlayerPushOrder_ = true;
	waitCloseTimeout_ = WAIT_BATTLE_CLOSE_TIMEOUT;
	
	caster->lastOrder_ = order;

	pushRA(order,sklevel);
}

bool
Battle::checkSecondOrder(COM_Order &order)
{
	Entity *caster = findEntityById(order.casterId_);
	if(NULL == caster)
	{
		return false;
	}

	Entity *target = findEntityByPos(order.target_);
	if(NULL == target)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Push order target is null\n")));
	}	
	COM_Order *pOrder = findOrder(caster->getGUID());

	if(pOrder!=NULL)
	{
		pOrder->isSec_ = 1;
		order.isSec_ = 2;
		Skill* pSk = caster->getSkillById(order.skill_);
		if(pSk == NULL && order.itemId_ == 0)
			return false;
		if(pSk)
		{
			const SkillTable::Core* core = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
			if (core && hasOnceMagic(caster->asPlayer()) && ( core->skType_ > SKT_DefaultActive || order.itemId_ != 0) )
			{
				//ACE_DEBUG((LM_INFO,ACE_TEXT("Cannot use two skills\n")));
				return false;
			}
		}
		
		caster->setBattleActive(true);
		for (size_t i=0; i<entities_.size(); ++i)
		{
			if(entities_[i]->asPlayer())
			{
				CALL_CLIENT(entities_[i],syncOrderOk(order.casterId_));
				//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Push order ok 2 caster(%d)--skill(%d)--targer(%d)\n"),order.casterId_, order.skill_, order.target_));
			}
		}
	}
	else
	{
		//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Push order EX caster(%d)--skill(%d)--targer(%d)\n"),order.casterId_, order.skill_, order.target_));
		CALL_CLIENT(caster,syncOrderOkEX());
	}

	return true;
}

void 
Battle::pushOrderByAI(COM_Order &order)
{
	Entity *caster = findEntityById(order.casterId_);
	if(NULL == caster)
	{	///尼玛宝宝被干飞了 为啥要push AI
		return;
	}
	if(caster->isDeadth())
		return;

	Entity *target = findEntityByPos(order.target_);
	if(NULL == target)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("AI[%d] Push order target is null order.target_[%d]\n"),order.casterId_,order.target_));
	}

	enum
	{
		MAX_ORDER = 2
	};

	if (orderCounter(caster) == MAX_ORDER)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("AI Push order Max == 2\n")));
		return;
	}
	COM_Order* pOrder = findOrder(order.casterId_);
	if(caster->asMonster() &&  pOrder != NULL)
	{
		pOrder->isSec_ = 1;
		order.isSec_ = 2;	//monster二动
	}
	int sklevel = 0;
	Skill* pSk = caster->getSkillById(order.skill_);
	if(pSk == NULL){
		ACE_DEBUG((LM_INFO,ACE_TEXT("AI monster[%d] Push order can not use skill[%d] target[%d]\n"),order.casterId_,order.skill_,order.target_));
		return;
	}
	sklevel = pSk->skLevel_;
	const SkillTable::Core* pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);

	if(pCore && (pCore->skType_ == SKT_Passive || pCore->skType_ == SKT_DefaultSecPassive || pCore->skType_ == SKT_DefaultPassive))
	{
		U32 targetTmp = order.target_;
		cleanOderParam();
		caster->castSkill(order.skill_,(BattlePosition)targetTmp,id_,posTable_,orderParam_);
	}

	S32 index = getIndex(order.casterId_);
	if(-1 == index)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("What fuck lzn!!!!\n")));
		return ;
	}

	caster->setBattleActive(true);

	pushRA(order,sklevel);

	//ACE_DEBUG((LM_INFO,ACE_TEXT("AI monster[%d] Push order finish skill[%d] target[%d]\n"),order.casterId_,order.skill_,order.target_));
}

void 
Battle::pushOrderTimeout(U32 instId)
{
	ACE_DEBUG((LM_DEBUG,"SYNC ORDER TIME OUT %d\n",instId));
	Entity *pEntity = findEntityById(instId);
	if(NULL == pEntity)
		return;
	if(pEntity->isDeadth())
		return;
	Player *pPlayer = pEntity->asPlayer();
	if(pPlayer)
	{
		
		pPlayer->setBattleActive(true);
		Baby *pBaby = pPlayer->getBattleBaby();
		if(pBaby)
		{
			pBaby->setBattleActive(true);
		}
	}
}

COM_Order *
Battle::findOrder(U32 uuid,bool skipDontCare)
{ 
	Entity* ent = findEntityById(uuid);
	if(NULL == ent)
		return NULL;
	RAList &ral = actions();
	for(size_t i=0; i<ral.size(); ++i)
	{
		if(ral[i].casterId_ == uuid){
			Skill* skinst = ent->getSkillById(ral[i].skill_);
			const SkillTable::Core*  skitem = NULL;
			if(skinst){
				skitem = SkillTable::getSkillById(skinst->skId_,skinst->skLevel_);
			}
			if(skipDontCare)
				return &ral[i];
			else if(skitem && (skitem->dontCare_ != true))
				return &ral[i]; 
		} 
	}
	return NULL;
}

COM_Order* 
Battle::getCurrentOrder(){
	RAList &ral = actions();
	if(actionIndex_ < 0 || actionIndex_ >= ral.size())
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("currOrder orderIdx_[%d] error\n"),actionIndex_));
		return NULL;
	}
	return &(ral[actionIndex_]);
}

void 
Battle::cleanOderParam()
{
	for (S32 bp=OPT_None; bp<OPT_Max; ++bp){
		orderParam_[bp] = 0;
	}
}

void 
Battle::setOrderParam(OrderParamType opt, S32 param)
{
	orderParam_[opt] = param;
}

///order次数
U32
Battle::orderCounter(Entity *pEntity){
	U32 counter = 0;
	RAList &ral = actions();
	for (size_t i = 0; i < ral.size(); ++i){
		if (ral[i].casterId_ == pEntity->getGUID()){
			++counter;
		}
	}

	return counter;
}

////order是否使用SKT_Active
bool
Battle::hasOnceMagic(Entity *pEntity){
	COM_Order *order = findOrder(pEntity->getGUID());
	if(NULL == order)
		return false;
	if(order->itemId_ != 0)
		return false;
	Skill* pSk = pEntity->getSkillById(order->skill_);
	if(pSk == NULL)
		return false;
	const SkillTable::Core* core = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(NULL == core)
		return false;
	if(core->skType_ < SKT_DefaultActive)
		return false;
	return true;
}

class OrderComp
{
public:
	static inline float computeIndex(Entity *en)
	{
		if(!en)
			return 0;

		float res = 0.f;
		CALC_COMPUTE_ORDER(en,res);
		return res;
	}
public:
	OrderComp(Battle *battle):battle_(battle){}

	bool operator()(const COM_Order& lorder,const COM_Order& rorder)
	{
		Entity* pL = battle_->findEntityById(lorder.casterId_);
		Entity* pR = battle_->findEntityById(rorder.casterId_);

		if(!pL || !pR)
			return false;
		
		
		float l_index = computeIndex(pL) - ((lorder.isSec_ == 2)?100:0);
		float r_index = computeIndex(pR) - ((rorder.isSec_ == 2)?100:0);
		return (l_index > r_index);
	}
	
	Battle * battle_; ///所在战斗

};


void
Battle::orderSort()
{
	OrderComp comp(this);
	RAList &ral = actions();
	std::sort(ral.begin(), ral.end(),comp);
}

