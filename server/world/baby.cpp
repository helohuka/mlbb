#include "config.h"
#include "baby.h"
#include "player.h"
#include "monstertable.h"
#include "Robot.h"
#include "FilterWord.h"
#include "monster.h"
#include "GameEvent.h"
#include "worldserv.h"
#include "loghandler.h"
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

void
Baby::genBabyData(U32 babyTmpId,COM_BabyInst& out)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(babyTmpId);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),babyTmpId));
		return ;
	}

	out.instId_ =  WorldServ::instance()->getMaxGuid();

	out.instName_ = tmp->name_;
	out.tableId_ = babyTmpId;
	out.slot_ = -1;
	out.gear_.resize(BIG_Max);
	out.gear_[BIG_Stama] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Stama]);
	out.gear_[BIG_Strength] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Strength]);
	out.gear_[BIG_Power] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Power]);
	out.gear_[BIG_Speed] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Speed]);
	out.gear_[BIG_Magic] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Magic]);

	out.intensifyLevel_ = 1;
	out.intensifynum_ = 0;

	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(out.intensifyLevel_);
	if (pCore == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find intensifyLevel_ = %d form PetIntensive-table\n"),out.intensifyLevel_));
		return;
	}

	std::vector<S8> delta;
	randInitDelta(delta);
	out.addprop_.resize(PT_Max);
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = tmp->properties_[i];
	
	out.properties_[PT_Level] = tmp->monsterLevel_;
	out.properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	out.properties_[PT_Reply] = 100;
	out.properties_[PT_Glamour] =  Global::get<int>(C_BabyLoyalMin);		//宠物忠诚度默认60
	out.properties_[PT_Stama] = CALC_BABY_BASE(out.gear_[BIG_Stama],out.properties_[PT_Level],pCore->grow_);
	out.properties_[PT_Strength] = CALC_BABY_BASE(out.gear_[BIG_Strength],out.properties_[PT_Level],pCore->grow_);
	out.properties_[PT_Power] = CALC_BABY_BASE(out.gear_[BIG_Power],out.properties_[PT_Level],pCore->grow_);
	out.properties_[PT_Speed] = CALC_BABY_BASE(out.gear_[BIG_Speed],out.properties_[PT_Level],pCore->grow_);
	out.properties_[PT_Magic] = CALC_BABY_BASE(out.gear_[BIG_Magic],out.properties_[PT_Level],pCore->grow_);

	CALC_BABY_PRO_TRANS_STAMA(out,out.properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH(out,out.properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER(out,out.properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED(out,out.properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC(out,out.properties_[PT_Magic]);

	out.properties_[PT_TableId] = tmp->monsterId_;
	out.properties_[PT_AssetId] = tmp->assetId_;
	out.properties_[PT_Race] = tmp->race_;

	out.properties_[PT_HpCurr] = out.properties_[PT_HpMax];
	out.properties_[PT_MpCurr] = out.properties_[PT_MpMax];

	for (size_t i = 0; i < tmp->defaultSkill_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = tmp->defaultSkill_[i].first;
		skill.skillExp_ = 0;
		skill.skillLevel_ = tmp->defaultSkill_[i].second;

		out.skill_.push_back(skill);

		SkillTable::Core const *pCore = SkillTable::getSkillById(skill.skillID_,skill.skillLevel_);
		if(pCore && pCore->skType_ == SKT_CannotUse){
			out.properties_[pCore->resistPropType_] += pCore->resistNum_; ///计算被动技能( 宝宝学习 被动技能 直接初始化加上 但是会遗忘 直接走遗忘逻辑删除属性
		}
	}

	float ff = CALC_BASE_FightingForce((&out));

	for(int i=0; i<out.skill_.size(); ++i)
		ff += CALC_SKILL_FightingForceCOM(out.skill_[i]);

	out.properties_[PT_FightingForce] = ff;
}

Baby::Baby(InnerPlayer *owner)
{
	isBattle_ = false;
	SRV_ASSERT(owner);
	static U32 def = 100000;
	babyId_ = def++; ///测试用
	owner_ = owner;
	ownerName_ = owner_->getNameC();
	intensifyLevel_ = 1;
	intensifynum_ = 0;
	properties_.resize(PT_Max);
	addprop_.resize(PT_Max);
	gear_.resize(BIG_Max);
}

Baby::~Baby(){
	//ACE_DEBUG((LM_DEBUG,"Baby::~Baby()\n"));
}

ClientHandler *
Baby::getClient()
{
	SRV_ASSERT(owner_);
	return owner_->getClient();
}

void
Baby::syncProp()
{
	if(!dirtyProp_.empty())
	{
		std::vector<COM_PropValue> prop;

		if(dirtyProp_.size() > SYNC_PROP_MAX){
			prop.insert(prop.begin(),dirtyProp_.begin(),dirtyProp_.begin()+SYNC_PROP_MAX);
			dirtyProp_.erase(dirtyProp_.begin(),dirtyProp_.begin() + SYNC_PROP_MAX);
		}
		else {
			prop = dirtyProp_;
			dirtyProp_.clear();
		}		
		CALL_CLIENT(this, syncProperties(getGUID(),prop));
	}
}

void 
Baby::addProperty(const std::vector<COM_Addprop> &props)
{
	float lastHpMax = properties_[PT_HpMax];
	float lastMpMax = properties_[PT_MpMax];
	for (size_t i = 0; i < props.size(); ++i)
	{
		U32 freeNum = getProp(PT_Free);

		if (props[i].uVal_ > freeNum || props[i].uVal_ <= 0)
			continue;

		U32 curNum = freeNum - props[i].uVal_;

		setProp(PT_Free,curNum);

		float curProp = getProp(props[i].type_);
		curProp+=props[i].uVal_;
		setProp(props[i].type_, curProp);
		addprop_[props[i].type_] += props[i].uVal_;
		switch(props[i].type_)
		{
		case PT_Stama:
			{
				CALC_PLAYER_PRO_TRANS_STAMA((*this), props[i].uVal_);
			}
			break;
		case PT_Strength:
			{
				CALC_PLAYER_PRO_TRANS_STRENGTH((*this), props[i].uVal_);
			}
			break;
		case PT_Power:
			{
				CALC_PLAYER_PRO_TRANS_POWER((*this), props[i].uVal_);
			}
			break;
		case PT_Speed:
			{
				CALC_PLAYER_PRO_TRANS_SPEED((*this), props[i].uVal_);
			}
			break;
		case PT_Magic:
			{
				CALC_PLAYER_PRO_TRANS_MAGIC((*this), props[i].uVal_);
			}
			break;
		default:
			break;
		}
	}
	float hp = lastHpMax - properties_[PT_HpMax];
	float mp = lastMpMax - properties_[PT_MpMax];
	if(hp > 0)
	{
		properties_[PT_HpCurr] += hp;
		if(properties_[PT_HpCurr] > properties_[PT_HpMax])
		{
			properties_[PT_HpCurr] = properties_[PT_HpMax];
		}
	}
	if(mp > 0)
	{
		properties_[PT_MpCurr] += mp;
		if(properties_[PT_MpCurr] > properties_[PT_MpMax])
		{
			properties_[PT_MpCurr] = properties_[PT_MpMax];
		}
	}

	setProp(PT_HpCurr,getProp(PT_HpCurr));
	setProp(PT_MpCurr,getProp(PT_MpCurr));

	refreshProperty();
}

void
Baby::changeProp(PropertyType type, float uVal)
{
	float current = getProp((PropertyType)type);
	float curProp = current + uVal;

	if(type == PT_HpCurr && curProp > getProp(PT_HpMax))
		curProp = getProp(PT_HpMax);
	if(type == PT_MpCurr && curProp > getProp(PT_MpMax))
		curProp = getProp(PT_MpMax);

	setProp((PropertyType)type,curProp);
}

void Baby::initBattleStatus(U32 battleId,GroupType battleForce,BattlePosition battlePos,bool initActive ){
	Entity::initBattleStatus(battleId,battleForce,battlePos,initActive);

	if (!owner_){
		return;
	}

	Player* player = owner_->asPlayer();
	if (!player){
		return;
	}


	COM_GuildMember* g = player->myGuildMember();
	if (g)
	{
		for (size_t i = 0; i < player->guildSkills_.size(); ++i){
			SkillTable::Core const* pCore = SkillTable::getSkillById(player->guildSkills_[i].skillID_, player->guildSkills_[i].skillLevel_);
			SRV_ASSERT(pCore);
			if (pCore->skType_ != SKT_GuildBabySkill)
				continue;
			addAttachedPropertyD(pCore->resistPropType_,pCore->resistNum_);
		}
	}

}

void 
Baby::cleanBattleStatus(bool resetProperty)
{
	Entity::cleanBattleStatus(resetProperty);
}

///========================================================================
///@group Properties
///@{


void 
Baby::setPropFromTable(MonsterTable::MonsterData const *tmp,U32 level)
{
	properties_.clear();
	properties_.resize(PT_Max,0.f);
	addprop_.resize(PT_Max,0.f);
	babyName_ = tmp->name_;

	gear_[BIG_Stama] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Stama]);
	gear_[BIG_Strength] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Strength]);
	gear_[BIG_Power] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Power]);
	gear_[BIG_Speed] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Speed]);
	gear_[BIG_Magic] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Magic]);

	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(intensifyLevel_);
	if (pCore == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("setPropFromTable::Can not find intensifyLevel_ = %d form PetIntensive-table\n"),intensifyLevel_));
		return;
	}

	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp->properties_[i];

	properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	properties_[PT_Reply] = 100;
	properties_[PT_Level] = level;
	properties_[PT_Stama] = CALC_BABY_BASE(gear_[BIG_Stama],properties_[PT_Level],pCore->grow_);
	properties_[PT_Strength] = CALC_BABY_BASE(gear_[BIG_Strength],properties_[PT_Level],pCore->grow_);
	properties_[PT_Power] = CALC_BABY_BASE(gear_[BIG_Power],properties_[PT_Level],pCore->grow_);
	properties_[PT_Speed] = CALC_BABY_BASE(gear_[BIG_Speed],properties_[PT_Level],pCore->grow_);
	properties_[PT_Magic] = CALC_BABY_BASE(gear_[BIG_Magic],properties_[PT_Level],pCore->grow_);

	CALC_BABY_PRO_TRANS_STAMA((*this),properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH((*this),properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER((*this),properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED((*this),properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC((*this),properties_[PT_Magic]);

	properties_[PT_TableId] = tmp->monsterId_;
	properties_[PT_AssetId] = tmp->assetId_;
	//properties_[PT_Level]	= tmp->monsterLevel_;
	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];

	skills_.resize(tmp->skillNum_);

	initSkillById( tmp->defaultSkill_);

	calcFightingForce();
}

void
Baby::setPropFromMonster(Monster* monster)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(monster->monsterId_);
	if(NULL == tmp)
		return;
	properties_ = monster->properties_;
	gear_ = monster->gearProperties_;
	tableId_ = monster->monsterId_;
	initSkillById(tmp->defaultSkill_);
	calcFightingForce();
}

void 
Baby::resetBaby()
{
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(getProp(PT_TableId));
	if(NULL == tmp)
		return ;
	setPropFromTable(tmp);
	properties_[PT_Level] = 1;
	properties_[PT_Glamour] =  Global::get<int>(C_BabyLoyalMin);		//宠物忠诚度默认60
	addprop_[PT_Stama] = 0;
	addprop_[PT_Strength] = 0;
	addprop_[PT_Power] = 0;
	addprop_[PT_Speed] = 0;
	addprop_[PT_Magic] = 0;
	for(size_t i=0; i<equipItems_.size(); ++i){
		if(equipItems_[i])
			addEquipmentEffect(equipItems_[i]);
	}
	resetPassiveSkill();
	checkEquipGroup();
	COM_BabyInst inst ;
	getBabyInst(inst);
	CALL_CLIENT(this,refreshBaby(inst));
	GEParam params[2];
	params[0].type_ = GEP_INT;
	params[0].value_.i = getProp(PT_TableId);
	params[1].type_ = GEP_INT;
	params[1].value_.i = getGUID();
	GameEvent::procGameEvent(GET_ResetBaby,params,2,owner_->getHandleId());
}

void
Baby::calcProperties()
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(properties_[PT_TableId]);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),properties_[PT_TableId]));
		return ;
	}

	std::vector<float> tmpprop;
	tmpprop.resize(PT_Max,0.f);
	tmpprop = properties_;
	
	properties_.clear();
	properties_.resize(PT_Max,0.f);

	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(intensifyLevel_);
	if (pCore == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("calcintensifyProp::Can not find intensifyLevel_ = %d form PetIntensive-table\n"),intensifyLevel_));
		return;
	}

	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp->properties_[i];
	/*for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmpprop[i];*/

	properties_[PT_Level]	= tmpprop[PT_Level];
	properties_[PT_TableId] = tmpprop[PT_TableId];
	properties_[PT_AssetId] = tmpprop[PT_AssetId];
	properties_[PT_Glamour] = tmpprop[PT_Glamour];
	properties_[PT_Exp]		= tmpprop[PT_Exp];
	properties_[PT_Free]	= tmpprop[PT_Free];
	
	properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	properties_[PT_Reply] = 100;

	properties_[PT_Stama] = CALC_BABY_BASE(gear_[BIG_Stama],properties_[PT_Level],pCore->grow_) + addprop_[PT_Stama];
	properties_[PT_Strength] = CALC_BABY_BASE(gear_[BIG_Strength],properties_[PT_Level],pCore->grow_) + addprop_[PT_Strength];
	properties_[PT_Power] = CALC_BABY_BASE(gear_[BIG_Power],properties_[PT_Level],pCore->grow_) + addprop_[PT_Power];
	properties_[PT_Speed] = CALC_BABY_BASE(gear_[BIG_Speed],properties_[PT_Level],pCore->grow_) + addprop_[PT_Speed];
	properties_[PT_Magic] = CALC_BABY_BASE(gear_[BIG_Magic],properties_[PT_Level],pCore->grow_) + addprop_[PT_Magic];

	CALC_BABY_PRO_TRANS_STAMA((*this),properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH((*this),properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER((*this),properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED((*this),properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC((*this),properties_[PT_Magic]);

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];

	resetPassiveSkill();

	for (size_t i = 0; i < equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL)
			continue;
		addEquipmentEffect(equipItems_[i],1,true);
	}

	calcFightingForce();
	refreshProperty();
	checkEquipGroup();
}

void Baby::calcProperties2(){
	int32 pp = addprop_[PT_Stama] + addprop_[PT_Strength] + addprop_[PT_Power] + addprop_[PT_Speed] + addprop_[PT_Magic] ;
	if (pp > properties_[PT_Level] - 1){
		properties_[PT_Free] = properties_[PT_Level] - 1;
		addprop_[PT_Stama] = addprop_[PT_Strength] = addprop_[PT_Power] = addprop_[PT_Speed] = addprop_[PT_Magic] = 0 ;
	}
	calcProperties();
}


///@}

void Baby::forgetSkill(S32 skId){
	Skill* pSk = getSkillById(skId);
	if(pSk == NULL)
		return;
	if(getEquipSkill() != NULL){
		if(getEquipSkill()->skId_ == skId)
			return;
	}
	SkillTable::Core const *pTmpCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(NULL == pTmpCore)
		return;
	if(pTmpCore->skType_ == SKT_CannotUse)
	{
		setProp(pTmpCore->resistPropType_, getProp(pTmpCore->resistPropType_) - pTmpCore->resistNum_);
		}

	Entity::forgetSkill(skId);
}

///========================================================================
///@group
///@{

void
Baby::setBabyInst(COM_BabyInst &tmp)
{
	setEntityInst(tmp);
	isBind_ = tmp.isBind_;
	isLock_ = tmp.isLock_;
	isShow_ = tmp.isShow_;
	lastSellTime_ = tmp.lastSellTime_;
	ownerName_		= tmp.ownerName_;
	babyId_			= tmp.instId_;
	tableId_		= tmp.tableId_;
	babyName_		= tmp.instName_;
	slot_			= tmp.slot_;
	intensifyLevel_ = tmp.intensifyLevel_;
	intensifynum_	= tmp.intensifynum_;
	
	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp.properties_[i];
	addprop_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		addprop_[i] = tmp.addprop_[i];
	gear_.resize(BIG_Max);
	for(S32 i=BIG_None; i<BIG_Max; ++i)
		gear_[i] = tmp.gear_[i];

	initSkillByInst(tmp.skill_);

	isBattle_ = tmp.isBattle_;

	calcProperties2(); //临时放到这
}

void 
Baby::getBabyInst(COM_BabyInst &out)
{
	getEntityInst(out);
	out.ownerName_	= ownerName_;
	out.isBind_ = isBind_;
	out.isLock_ = isLock_;
	out.isShow_ = isShow_;
	out.type_		= ET_Baby;
	out.instId_		= babyId_;
	out.tableId_	= tableId_;
	out.lastSellTime_ = lastSellTime_;
	out.instName_	= babyName_;
	out.slot_		= slot_;
	out.intensifyLevel_ = intensifyLevel_;
	out.intensifynum_	= intensifynum_;
	//out.properties_ = properties_;

	out.addprop_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.addprop_[i] = addprop_[i];
	out.gear_.resize(BIG_Max);
	for(S32 i=BIG_None; i<BIG_Max; ++i)
		out.gear_[i] = gear_[i];

	out.battlePosition_ = battlePosition_;
	out.isBattle_ = isBattle_;
}

void Baby::getBattleEntityInformation(COM_BattleEntityInformation& info){
	Entity::getBattleEntityInformation(info);
	info.type_ = ET_Baby;
	info.instName_ = this->babyName_;
}

void
Baby::changeBabyName(const char* name)
{
	if (name == NULL)
		return;
	//判断是否有标点符号
	if (FilterWord::strHasSymbols(name))
	{
		CALL_CLIENT(owner_,errorno(EN_FilterWord));
		return;
	}
	std::string strname = name;
	//判断是否有屏蔽字
	if (FilterWord::replace(strname))
	{
		CALL_CLIENT(owner_,errorno(EN_FilterWord));
		return;
	}

	babyName_ = name;
	COM_BabyRankData info;
	info.instId_ = getGUID();
	info.name_ = getNameC();
	info.ownerName_ = ownerName_;
	info.ff_ = properties_[PT_FightingForce];
	WorldServ::instance()->calcBabyFFRank(info);

	CALL_CLIENT(owner_, changeBabyNameOK(this->babyId_, babyName_));
}

void
Baby::chackLevelUp()
{
	U32 curlevel = getProp(PT_Level);

	ExpTable::Core const* pCore = ExpTable::getTemplateById(curlevel);

	if(!pCore)
		return;

	uint64 curExp = getProp(PT_Exp);

	while(curExp >= pCore->babyExp_)
	{
		if(curlevel >= Global::get<float>(C_PlayerMaxLevel)+5)
		{
			setProp(PT_Exp,pCore->babyExp_-1);
			break;
		}
		levelup();
		curlevel = getProp(PT_Level);
		pCore = ExpTable::getTemplateById(curlevel);

		GEParam params[3];
		params[0].type_ = GEP_INT;
		params[0].value_.i = getGUID();
		params[1].type_ = GEP_INT;
		params[1].value_.i = curlevel;
		params[2].type_ = GEP_INT;
		params[2].value_.i = tableId_;
		GameEvent::procGameEvent(GET_BabyLevelUp,params,3,owner_->getHandleId());

		if(!pCore)
			return;	
	}
}

void
Baby::levelup()
{	
	clearAttachedProperties();
	enum
	{
		ADD_FREE = 1
	};

	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById((S32)getProp(PT_TableId));
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in player-template-table\n"),(S32)getProp(PT_TableId)));
		return ;
	}

	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(intensifyLevel_);
	if (pCore == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("baby levelup :: Can not find intensifyLevel_ = %d form PetIntensive-table\n"),intensifyLevel_));
		return;
	}

	COM_BabyInst inst;
	inst.gear_ = gear_;
	inst.properties_ = properties_;
	inst.properties_[PT_Level]+=1;
	inst.properties_[PT_Free] += ADD_FREE;

	inst.properties_[PT_Stama] += CALC_BABY_LEVELUP(inst.gear_[BIG_Stama],pCore->grow_);
	inst.properties_[PT_Strength] += CALC_BABY_LEVELUP(inst.gear_[BIG_Strength],pCore->grow_);
	inst.properties_[PT_Power] += CALC_BABY_LEVELUP(inst.gear_[BIG_Power],pCore->grow_);
	inst.properties_[PT_Speed] += CALC_BABY_LEVELUP(inst.gear_[BIG_Speed],pCore->grow_);
	inst.properties_[PT_Magic] += CALC_BABY_LEVELUP(inst.gear_[BIG_Magic],pCore->grow_);

	CALC_CLEAN_PRO((inst));
	
	inst.properties_[PT_Attack] = tmp->properties_[PT_Attack];
	inst.properties_[PT_Defense] = tmp->properties_[PT_Defense];
	inst.properties_[PT_Agile] = tmp->properties_[PT_Agile];
	inst.properties_[PT_HpMax] = tmp->properties_[PT_HpMax];
	inst.properties_[PT_MpMax] = tmp->properties_[PT_MpMax];
	inst.properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	inst.properties_[PT_Reply] = 100;
	CALC_BABY_PRO_TRANS_STAMA((inst),inst.properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH((inst),inst.properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER((inst),inst.properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED((inst),inst.properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC((inst),inst.properties_[PT_Magic]);

	float hp = properties_[PT_HpCurr];
	float mp = properties_[PT_MpCurr];
	
	for (size_t i=0; i<inst.properties_.size(); ++i)
	{
		setProp((PropertyType)i,inst.properties_[i]);
	}
	resetPassiveSkill();

	for (size_t i = 0; i < equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL)
			continue;
		addEquipmentEffect(equipItems_[i],1,true);
	}

	setProp(PT_HpCurr,hp);
	setProp(PT_MpCurr,mp);

	calcFightingForce();
	calcLoyal();
	
}


void 
Baby::postEvent(AIEvent me, BattlePosition target, std::map<S32,S32> &posTable)
{
	//ACE_DEBUG((LM_INFO,"Baby::postEvent(AIEvent %d, BattlePosition %d, std::map<S32,S32> &posTable)\n",me,target));
	if(isDeadth() || !owner_ || owner_->isDeadth())
	{
		ACE_DEBUG((LM_INFO,"Baby::postEvent if(isDeadth() || !owner_ || owner_->isDeadth())\n"));
		return;
	}
	if (owner_->asInnerPlayer()->babyEvents_[me].empty() || owner_->asInnerPlayer()->babyEvents_[me].length() == 0)
	{
		ACE_DEBUG((LM_INFO,"Baby::postEvent if (owner_->asInnerPlayer()->babyEvents_[me].empty() || owner_->asInnerPlayer()->babyEvents_[me].length() == 0)\n"));
		return;
	}
	
	enum 
	{
		ARG_BATTLEID,		//0
		ARG_CASTERPOS,		//1
		ARG_TARGETPOS,		//2
		ARG_POSTABLE,		//3
		ARG_SKILLTABLE,		//4
		ARG_MAX_
	};

	GEParam param[ARG_MAX_];
	param[ARG_BATTLEID].type_		= GEP_INT;
	param[ARG_BATTLEID].value_.i	= battleId_;
	param[ARG_CASTERPOS].type_		= GEP_INT;
	param[ARG_CASTERPOS].value_.i	= battlePosition_;
	param[ARG_TARGETPOS].type_		= GEP_INT;
	param[ARG_TARGETPOS].value_.i	= target;
	param[ARG_POSTABLE].type_		= GEP_POS_TABLE;
	param[ARG_POSTABLE].value_.hPosTable	= &posTable;


	aiSkills_.clear();
	for (size_t k = 0; k <skills_.size(); ++k)
	{
		aiSkills_.push_back(skills_[k]->skId_);
	}
	param[ARG_SKILLTABLE].type_		= GEP_HANDLE_ARRAY;
	param[ARG_SKILLTABLE].value_.hArray	= &aiSkills_;
	std::string err;
	if( !ScriptEnv::callGEProc(owner_->asInnerPlayer()->babyEvents_[me].c_str(),getHandleId(),param,ARG_MAX_,err) )
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Baby post event %s\n"),err.c_str()));
		return;
	}
}

void
Baby::babyLearnSkill(U32 oldSkId, U32 newSkId, U32 newSkLv)
{
	if(oldSkId != 0){
	//oldSkId != 0 为替换技能
		if(equipSkill_.skId_ == oldSkId){
			return;
		}
	}
	SkillTable::Core const *pCore = SkillTable::getSkillById(newSkId,newSkLv);
	if(pCore == NULL)
		return;
	if( 0 != oldSkId && getSkillById(oldSkId) == NULL){
		ACE_DEBUG((LM_DEBUG,"oldskill is null %d\n",oldSkId));
		return ;
	}
	if(!canUseSkill(oldSkId,newSkId,newSkLv))
		return;

	owner_->asPlayer()->addMoney(-pCore->learnCoin_);
	SGE_LogProduceTrack track;
	track.playerId_ = owner_->asPlayer()->getGUID();
	track.playerName_ = owner_->asPlayer()->getNameC();
	track.from_ = 10;
	track.money_ = -pCore->learnCoin_;
	LogHandler::instance()->playerTrack(track);

	if(oldSkId == 0)
	{
		learnSkill(newSkId,newSkLv);
	}
	else
	{	
		forgetSkill(oldSkId);
		learnSkill(newSkId,newSkLv);
	}

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = skills_.size();
	GameEvent::procGameEvent(GET_BabyLearnSkill,params,1,owner_->getHandleId());

	CALL_CLIENT(owner_, babyLearnSkillOK(babyId_,newSkId));
}

bool
Baby::canUseSkill(U32 oldSkId, U32 newSkId, U32 newSkLv)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(properties_[PT_TableId]);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),properties_[PT_TableId]));
		return false;
	}
	if(oldSkId == 0)
	{
		if(skills_.size() >= tmp->skillNum_)
			return false;
	}

	SkillTable::Core const *pCore = SkillTable::getSkillById(newSkId,newSkLv);

	if(pCore == NULL)
		return false;

	if(getSkillById(newSkId) != NULL)
		return false;
	if(getProp(PT_Level) < pCore->learnLv_)
		return false;

	if(owner_->getProp(PT_Money) < pCore->learnCoin_)
		return false;

	if(pCore->isPhysic_)
		return ((getProp(PT_Level) / 20 + 1)>= pCore->level_);
	else
		return ((getProp(PT_Level) / 10 + 1)>= pCore->level_);
}

void
Baby::calcLoyal()
{
	S32 sklev = 0, loyal , babylev;

	Skill* sk = getOwner()->getSkillById(Global::get<int>(C_BabyLoyalSkillId));
	if(sk)
	{
		sklev = sk->skLevel_;
	}

	babylev = (S32)getProp(PT_Level);

	loyal = 100 - 4*(babylev-sklev*10);

	if(loyal < Global::get<int>(C_BabyLoyalMin))
		loyal = Global::get<int>(C_BabyLoyalMin);
	else if(loyal > Global::get<int>(C_BabyLoyalMax))
		loyal = Global::get<int>(C_BabyLoyalMax);

	setProp(PT_Glamour,loyal);
}

S32
Baby::calcrandDelta()
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(properties_[PT_TableId]);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),properties_[PT_TableId]));
		return 0;
	}
	
	S32 index = 0;
	S32 tmd = 0;
	for(S32 i=BIG_Stama; i<BIG_Max; ++i)
	{
		index += gear_[i];
		tmd += tmp->gearProps_[i];
	}
	S32 delta = tmd-index;
	return delta;
}

void
Baby::intensify()
{
	Player* player = owner_->asPlayer();
	if(player == NULL)
		return;
	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(intensifyLevel_);
	if (pCore == NULL){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("intensify:: Can not find intensifyLevel_ = %d form PetIntensive-table\n"),intensifyLevel_));
		return;
	}
	if(pCore->needItemNum_ == 0)
		return;
	if(player->getItemNumByItemId(pCore->needItem_) < pCore->needItemNum_)
		return;
	player->delBagItemByItemId(pCore->needItem_,pCore->needItemNum_);
	U32 roll = UtlMath::randN(100);
	if(roll > pCore->probability_)
	{ //没有成功
		++intensifynum_;
		if(intensifynum_ < pCore->maxtime_)
		{ ///没有到达必成功的基数
			CALL_CLIENT(player,intensifyBabyOK(babyId_,intensifyLevel_));
			return;
		}
	}
	intensifyLevel_++;
	intensifynum_ = 0;
	
	calcProperties();
	CALL_CLIENT(owner_->asPlayer(),intensifyBabyOK(babyId_,intensifyLevel_));
	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = intensifyLevel_;
	GameEvent::procGameEvent(GET_Babyintensify,param,1,owner_->getHandleId());
}


void Baby::intensify(U32 target){
	Player* player = owner_->asPlayer();
	if(player == NULL)
		return;
	PetIntensive::Core const* pCore = PetIntensive::getCoreBylevel(target);
	if (pCore == NULL){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("intensifytotarget::Can not find intensifyLevel_ = %d form PetIntensive-table\n"),target));
		return;
	}
	intensifyLevel_ = target;
	intensifynum_ = 0;
	calcProperties();
	CALL_CLIENT(player,intensifyBabyOK(babyId_,intensifyLevel_));
	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = intensifyLevel_;
	GameEvent::procGameEvent(GET_Babyintensify,param,1,owner_->getHandleId());
}
COM_Item* Baby::wearEquipment(COM_Item* equip){
	const ItemTable::ItemData *item = ItemTable::getItemById(equip->itemId_);
	if(NULL == item)
		return NULL;
	if(!equip)
		return NULL;
	equip->slot_ = item->slot_;

	COM_Item* takeoffEquip = equipItems_[equip->slot_];
	equipItems_[equip->slot_] = equip;


	addEquipmentEffect(equip,1,true);
	
	if(takeoffEquip){
		delEquipmentEffect(takeoffEquip,1,true);
	}
	
	checkEquipGroup();

	return takeoffEquip;
}
COM_Item* Baby::takeoffEquipment(U32 instId){
	for(size_t i=0;i<equipItems_.size(); ++i){
		if(equipItems_[i] && (equipItems_[i]->instId_ == instId) )
		{
			COM_Item* tmp = equipItems_[i];
			equipItems_[i] = NULL;
			delEquipmentEffect(tmp,1,true);
			return tmp;
		}
	}
	return NULL;
}
void Baby::checkEquipGroup(){
	enum {
		CHECK_SAME_COUNT = 2 ///套装界限  如果小于就没有技能清空 等于就是原始等级 大于就加一
	};
	int32 skillId = 0;
	int32 skillLev = 0;
	int32 equipCount = 0;
	for(size_t i=0; i<equipItems_.size(); ++i){
		if(equipItems_[i]){
			if(skillId == equipItems_[i]->durability_){
				++equipCount;
			}else if(skillId == 0){
				skillId =  equipItems_[i]->durability_;
				skillLev = equipItems_[i]->durabilityMax_;
				equipCount = 0;
			}
		}
	}
	if(equipCount < CHECK_SAME_COUNT){
		skillId = 0;
		skillLev = 0;
	}else if(equipCount > CHECK_SAME_COUNT){
		++skillLev;
	}
	
	/*if(equipSkill_.skId_ != skillId || equipSkill_.skLevel_ != skillLev){*/
		if(equipSkill_.skId_ != 0){
			//替换技能观察是不是被动
			SkillTable::Core const* skData = SkillTable::getSkillById(equipSkill_.skId_,equipSkill_.skLevel_);
			if(!skData){
				ACE_DEBUG((LM_ERROR,"Baby equip skill forget error skill can not find %d\n",equipSkill_.skId_));
				return;
			}
			if(skData->skType_ == SKT_CannotUse){
				int32 propNumber = getProp(skData->resistPropType_);
				setProp(skData->resistPropType_, propNumber - skData->resistNum_);
			}
			equipSkill_.fini();
		}
		
		if(skillId != 0){
			//替换技能观察是不是被动
			SkillTable::Core const* skData = SkillTable::getSkillById(skillId,skillLev);
			if(!skData){
				ACE_DEBUG((LM_ERROR,"Baby equip skill forget error skill can not find %d\n",skillId));
				return;
			}
			if(skData->skType_ == SKT_CannotUse){
				int32 propNumber = getProp(skData->resistPropType_);
				setProp(skData->resistPropType_, propNumber + skData->resistNum_);
			}

			equipSkill_.init(skData);
		}
	//}
}
Skill* Baby::getEquipSkill(){
	if(equipSkill_.skId_){
		return &equipSkill_;
	}
	return NULL;
}
///@}
