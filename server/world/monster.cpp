#include "monster.h"
#include "monstertable.h"
#include "battle.h"
#include "monstertable.h"
void
static randInitDelta(std::vector<S8> &out)
{
	std::vector<std::pair<S32, S32> > wi;
	for(S32 i=BIG_Stama; i<BIG_Max; ++i)
	{
		std::pair<int ,int> pair(i,1);
		wi.push_back(pair);
	}

	out.resize(BIG_Max);
	for(S8 i=0; i<10; ++i)
	{
		S32 gearId = UtlMath::randWeight(wi);
		++out[gearId];
	}
}
S32 Monster::IDMaker = 1;
Monster::Monster(std::string &className)
:monsterClass_(className)
{
	const MonsterClass::Core* clazz = MonsterClass::getClassByName(monsterClass_);
	SRV_ASSERT(clazz);
	
	instId_			= MAKE_MONSTER_ID(++IDMaker); 
	monsterClass_	= className;
	monsterId_		= clazz->monsterId_;
	monsterLevel_	= clazz->level_;
	monsterEvents_	= clazz->events_;
	monsterDropId_	= clazz->dropId_;

	init();

	if(clazz->isAddValue_)
		appendPropValue(className);
}

Monster::Monster(int32 pId,int32 pLev){
	Monster2Table::Core const * core = Monster2Table::getMonster2Core(pId,pLev);
	SRV_ASSERT(core);
	const MonsterClass::Core* clazz = MonsterClass::getClassByName(core->monsterClass_);
	SRV_ASSERT(clazz);
	instId_	 = MAKE_MONSTER_ID(++IDMaker); 
	monsterId_ = core->monsterId2_;
	monsterLevel_ = pLev;
	monsterClass_ = core->monsterClass_;
	monsterEvents_ = clazz->events_;
	monsterDropId_ = clazz->dropId_;
	
	properties_ = core->properties_;
	properties_[PT_TableId] = monsterId_;
	properties_[PT_AssetId] = core->assetId_;
	properties_[PT_Level]	= monsterLevel_;
	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];
	initSkillById(core->defaultSkill_);
	
	for(size_t i=0; i<skills_.size(); ++i){
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(pCore && pCore->skType_ == SKT_CannotUse){
			properties_[pCore->resistPropType_] += pCore->resistNum_; ///计算被动技能( 宝宝学习 被动技能 直接初始化加上 但是会遗忘 直接走遗忘逻辑删除属性
		}
	}
}

Monster::~Monster(){
	fini();
}

void
Monster::init()
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(monsterId_);
	if(NULL == tmp){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d monster form monster-table\n"),monsterId_));
		return ;
	}

	gearProperties_.resize(BIG_Max);
	gearProperties_[BIG_Stama] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Stama]);
	gearProperties_[BIG_Strength] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Strength]);
	gearProperties_[BIG_Power] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Power]);
	gearProperties_[BIG_Speed] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Speed]);
	gearProperties_[BIG_Magic] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Magic]);

	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(1);
	if (pCore == NULL)
		return;
	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp->properties_[i];
	properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	properties_[PT_Reply] = 100;
	properties_[PT_Stama] = CALC_BABY_BASE(gearProperties_[BIG_Stama],monsterLevel_,pCore->grow_);
	properties_[PT_Strength] = CALC_BABY_BASE(gearProperties_[BIG_Strength],monsterLevel_,pCore->grow_);
	properties_[PT_Power] = CALC_BABY_BASE(gearProperties_[BIG_Power],monsterLevel_,pCore->grow_);
	properties_[PT_Speed] = CALC_BABY_BASE(gearProperties_[BIG_Speed],monsterLevel_,pCore->grow_);
	properties_[PT_Magic] = CALC_BABY_BASE(gearProperties_[BIG_Magic],monsterLevel_,pCore->grow_);

	CALC_BABY_PRO_TRANS_STAMA((*this),properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH((*this),properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER((*this),properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED((*this),properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC((*this),properties_[PT_Magic]);

	properties_[PT_TableId] = tmp->monsterId_;
	properties_[PT_AssetId] = tmp->assetId_;
	properties_[PT_Level]	= monsterLevel_;

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];

	monsterName_ = tmp->name_;

	initSkillById(tmp->defaultSkill_);

	for(size_t i=0; i<skills_.size(); ++i){
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(pCore && pCore->skType_ == SKT_CannotUse){
			properties_[pCore->resistPropType_] += pCore->resistNum_; ///计算被动技能( 宝宝学习 被动技能 直接初始化加上 但是会遗忘 直接走遗忘逻辑删除属性
		}
	}
}


void
Monster::appendPropValue(std::string& className)
{
	const MonsterClass::Core* clazz = MonsterClass::getClassByName(className);

	if(clazz == NULL)
		return;
	for (size_t i = PT_None; i < PT_Max; ++i)
	{
		properties_[i] += clazz->properties_[i];
	}

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];
}

void 
Monster::fini()
{
	//postEvent(ME_Deadth);
}

void
Monster::getMonsterInst(COM_MonsterInst &out)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(monsterId_);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d monster form monster-table\n"),monsterId_));
		return ;
	}

	Entity::getEntityInst(out);

	out.type_ = ET_Monster;
	out.instId_ = instId_;
	out.instName_ = tmp->name_;
	out.gear_.resize(BIG_Max);
	for(S32 i=BIG_None; i<BIG_Max; ++i)
		out.gear_[i] = gearProperties_[i];

	out.properties_.resize(PT_Max);
	for (size_t i=PT_None; i<PT_Max; ++i)
	{
		out.properties_[i] = properties_[i];
	}

	out.battlePosition_ = battlePosition_;
}

void Monster::getBattleEntityInformation(COM_BattleEntityInformation& info){
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(monsterId_);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d monster form monster-table\n"),monsterId_));
		return ;
	}
	Entity::getBattleEntityInformation(info);
	info.type_ = ET_Monster;
	info.instName_ = tmp->name_;
}

void 
Monster::postEvent(AIEvent me,  BattlePosition target, std::map<S32,S32> &posTable)
{
	if (monsterEvents_[me].empty() || monsterEvents_[me].length() == 0){
		return;
	}
	enum {
		ARG_BATTLEID,		//0
		ARG_CASTERPOS,		//1
		ARG_TARGETPOS,		//2
		ARG_POSTABLE,		//3
		ARG_SKILLTABLE,
		ARG_MAX_ = 5
	};

	GEParam param[ARG_MAX_];
	param[ARG_BATTLEID].type_				= GEP_INT;
	param[ARG_BATTLEID].value_.i			= battleId_;
	param[ARG_CASTERPOS].type_				= GEP_INT;
	param[ARG_CASTERPOS].value_.i			= battlePosition_;
	param[ARG_TARGETPOS].type_				= GEP_INT;
	param[ARG_TARGETPOS].value_.i			= target;
	param[ARG_POSTABLE].type_				= GEP_POS_TABLE;
	param[ARG_POSTABLE].value_.hPosTable	= &posTable;
	param[ARG_SKILLTABLE].type_				= GEP_HANDLE_ARRAY;
	param[ARG_SKILLTABLE].value_.hArray		= &aiSkills_;

	std::string err;
	if(!ScriptEnv::callGEProc(monsterEvents_[me].c_str(),getHandleId(),param,ARG_MAX_,err)){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Monster post event %s\n"),err.c_str()));
		return;
	}
}