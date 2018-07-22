#include "config.h"
#include "account.h"
#include "client.h"
#include "player.h"
#include "entity.h"
#include "skilltable.h"
#include "sttable.h"
#include "itemtable.h"
#include "profession.h"
#include "GameEvent.h"
#include "worldserv.h"
#include "pvpJJC.h"
#include "battle.h"
Entity::Entity()
:battleId_(0)
,battleValue_(0)
,battlePosition_(BP_None)
,battleRunaway_(false)
,openDoubleTimeFlag_(false)
,isJoinBattle_(true)
{
	properties_.resize(PT_Max,0);
	battleTmpProp_.resize(PT_Max,0);
	achValues_.resize(AT_Max,0);
	equipItems_.resize(ES_Max,0);
}

Entity::~Entity()
{
	//ACE_DEBUG((LM_DEBUG,"Entity::~Entity()\n"));
	clearSkills();

	for( size_t i=0; i<equipItems_.size(); ++i){
		if(equipItems_[i])
			DEL_MEM(equipItems_[i]);
	}

	equipItems_.clear();

}

///========================================================================
///@group
///@{
float
Entity::getProp(PropertyType type)
{
	return properties_[type];
}

void
Entity::setProp(PropertyType type, float val)
{
	if(UtlMath::feq(properties_[type] , val))
		return;
	
	if(type == PT_HpCurr){
		if(val > properties_[PT_HpMax])
			val =  properties_[PT_HpMax];
		else if(val < 0.F)
			val = 0.F;
	}
	else if(type == PT_MpCurr){
		if(val > properties_[PT_MpMax])
			val =  properties_[PT_MpMax];
		else if(val < 0.F)
			val = 0.F;
	}
	else if(type == PT_HpMax){
		if(properties_[PT_HpCurr] > val)
		{
			properties_[PT_HpCurr] =  val;
			addDirtyProp(PT_HpCurr,val);
		}
		else if(val < 0.F)
			val = 0.F;
	}
	else if(type == PT_MpMax){
		if(properties_[PT_MpCurr] > val)
		{
			properties_[PT_MpCurr] =  val;
			addDirtyProp(PT_MpCurr,val);
		}
		else if(val < 0.F)
			val = 0.F;
	}

	if(type != PT_Reputation && val < 0.F)
		val = 0.F;
	properties_[type] = val;

	addDirtyProp(type,val);
}

void Entity::addDirtyProp(PropertyType type, float val){
	for(size_t i=0; i<dirtyProp_.size(); ++i){
		if(dirtyProp_[i].type_ == type){
			dirtyProp_[i].value_ = val;
			return;
		}
	}
	COM_PropValue pv;
	pv.type_ = type;
	pv.value_ = val;

	dirtyProp_.push_back(pv);
}

void 
Entity::addHp(float hp)
{
	float currentHp = getProp(PT_HpCurr);
	currentHp += hp;

	if(currentHp <= 0)
		currentHp = 0;
	else if(currentHp > getProp(PT_HpMax))
		currentHp = getProp(PT_HpMax);

	setProp(PT_HpCurr,currentHp);
}

void
Entity::addExp(U32 exp, bool extra)
{
	InnerPlayer* player = asPlayer();
	if(NULL == player && asBaby())
		player = asBaby()->getOwner();
	
	//顶级不给经验
	if(asPlayer() && player->getProp(PT_Level) >= Global::get<float>(C_PlayerMaxLevel) )
		return;
	else if(asBaby() && asBaby()->getProp(PT_Level) >= player->getProp(PT_Level) + 5)
		return;

	if(extra)
	{
		///VIP
		if(player){
			if((int)player->getProp(PT_VipLevel) == VL_1){
				exp += 0.1 * exp;
			}
			else if((int)player->getProp(PT_VipLevel) == VL_2){
				exp += 0.2 * exp;
			}
		}

		exp = exp*caleExpExtraValue();
	}
	float convertexp = getProp(PT_ConvertExp);
	if(player != NULL && convertexp > 0)
	{
		float con = exp * 0.3;
		if(convertexp >= con){
			convertexp -= con;
		}
		else{
			con = convertexp;
			convertexp = 0;
		}
		setProp(PT_ConvertExp,convertexp);
		exp += con;
		if(con > 0)
			CALL_CLIENT(player,sycnConvertExp(con));
	}

	if(exp > Global::get<float>(C_OnceMaxExp))
	{
		exp = Global::get<float>(C_OnceMaxExp);
		ACE_DEBUG( (LM_DEBUG, ACE_TEXT("addExp More than a single experience!!!!!!!!!!!!!\n") ) );
	}

	if(exp <=0 ) exp = 0;
	uint64 curExp = getProp(PT_Exp);
	curExp += exp;			
	setProp(PT_Exp,curExp);

	chackLevelUp();
}

float
Entity::caleExpExtraValue()
{
	if(getProp(PT_Level) < 10){
		return 1; ///10 级之内不双 影响新手引导
	}
	float extra = 1;
	if(checkStateById(EXT_BUFF_ID_50)){
		extra += Global::get<float>(C_GapWorldLvExtra_One);
	}
	else if(checkStateById(EXT_BUFF_ID_75)){
		extra += Global::get<float>(C_GapWorldLvExtra_Two);
	}
	else if(checkStateById(EXT_BUFF_ID_100)){
		extra += Global::get<float>(C_GapWorldLvExtra_Three);
	}

	InnerPlayer* player = asPlayer();
	if(NULL == player && asBaby())
		player = asBaby()->getOwner();
	if(player){
		if(player->openDoubleTimeFlag_ && player->getProp(PT_DoubleExp) > 0)
			extra += 1;
	}

	return extra;
}

void Entity::calcFightingForce(){
	float ff = CALC_BASE_FightingForce(this);

	if(getProp(PT_Profession) == JT_Ninja || getProp(PT_Profession) == JT_Fighter)
	{
		for(int i=0; i<skills_.size(); ++i)
			ff += CALC_SKILL_SPECIAL_FightingForce(skills_[i]);
	}
	else
	{
		for(int i=0; i<skills_.size(); ++i)
			ff += CALC_SKILL_FightingForce(skills_[i]);
	}

	setProp(PT_FightingForce,ff);

	if(asPlayer()){
		COM_ContactInfo info;
		info.instId_ = getGUID();
		info.level_ = properties_[PT_Level];
		info.name_ = getNameC();
		info.job_ = (JobType)(S32)properties_[PT_Profession];
		info.jobLevel_ = (S32)properties_[PT_ProfessionLevel];
		info.ff_ = properties_[PT_FightingForce];
		WorldServ::instance()->calcPlayerFFRank(info);
	}
	else if (asBaby()){
		COM_BabyRankData info;
		info.instId_ = getGUID();
		info.name_ = getNameC();
		info.ownerName_ = asBaby()->ownerName_;
		info.ff_ = properties_[PT_FightingForce];
		WorldServ::instance()->calcBabyFFRank(info);
	}
	else if (asEmployee()){
		COM_EmployeeRankData info;
		info.instId_ = getGUID();
		info.name_ = getNameC();
		info.ownerName_ = asEmployee()->ownerName_;
		info.ff_ = properties_[PT_FightingForce];
		WorldServ::instance()->calcEmployeeFFRank(info);
	}
}

bool
Entity::isDeadth()
{
	return (int)getProp(PT_HpCurr) <= 0.F;	
}

void
Entity::refreshProperty()
{
	for (size_t i=PT_None; i<PT_Max; ++i)
	{
		if(i == PT_Money || i == PT_Diamond || i == PT_Level) // TODO
			continue;
		COM_PropValue pv;
		pv.type_ = (PropertyType)i;
		pv.value_ = properties_[i];
		dirtyProp_.push_back(pv);
	}
}

void Entity::addSkillCostMana(S32 skid,float manap)
{
	for(size_t i=0; i<skillCostMana_.size(); ++i)
	{
		if(skillCostMana_[i].first == skid)
		{
			skillCostMana_[i].second += manap;
			return ;
		}
	}

	std::pair<S32,float> costmanap(skid,manap);
	skillCostMana_.push_back(costmanap);
}

float Entity::getSkillCostMana(S32 skid)
{
	for(size_t i=0; i<skillCostMana_.size(); ++i)
	{
		if(skillCostMana_[i].first == skid)
		{
			return skillCostMana_[i].second;
		}
	}

	return 0;
}

///@}

///========================================================================
///@group
///@{

void 
Entity::insertState(U32 stId,S32 v0  , S32 v1)
{
	StateTable::Core const * pCore = StateTable::getStateById(stId);
	if(NULL == pCore)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not instert state by id = %d, entity = %d\n"),stId,getGUID()));
		return;
	}

	COM_State st;
	st.stateId_ =stId;
	st.tick_ = pCore->tick_;
	st.turn_ = pCore->turn_;
	st.type_ = pCore->type_;

	st.value0_ = v0;
	st.value1_ = v1;

	states_.push_back(st);

	if(asPlayer() && !isBattle()){
		CALL_CLIENT(asPlayer(),insertState(st));
	}
	if(isBattle()){
		curRoundStates_.push_back(stId);
	}
}

void Entity::setStateValue(U32 sdId, S32 v0 , S32 v1){
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(states_[i].stateId_ == sdId)
		{
			states_[i].value0_ = v0;
			states_[i].value1_ = v1;
			if(asPlayer() && !isBattle()){
				CALL_CLIENT(asPlayer(),updattState(states_[i]));
			}
			return ; ///BUG
		}
	}
}

void Entity::getStateValue(U32 stId, S32& v0, S32& v1){
	v0=v1=0;
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(states_[i].stateId_ == stId)
		{
			v0 = states_[i].value0_ ;
			v1 = states_[i].value1_ ;
			return ; ///BUG
		}
	}
}

bool 
Entity::checkState(U32 type)
{
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(states_[i].type_ == type)
			return true;
	}

	return false;
}

bool 
Entity::checkStateById(U32 id){
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(states_[i].stateId_== id)
			return true;
	}

	return false;
}

void 
Entity::removeState(U32 stId)
{
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(states_[i].stateId_ == stId)
		{
			states_.erase(states_.begin() + i);
			if(asPlayer() && !isBattle()){
				CALL_CLIENT(asPlayer(),removeState(stId));
			}
			return ; ///BUG
		}
	}
}

bool
Entity::updateState(std::map<S32,S32> &posTable)
{
	for (size_t i=0; i<states_.size(); ++i)
	{
		if(std::find(curRoundStates_.begin(),curRoundStates_.end(),states_[i].stateId_) != curRoundStates_.end()){
			continue;
		}

		StateTable::Core const *pCore = StateTable::getStateById(states_[i].stateId_);

		if (!pCore)
		{
			return false;
		}

		enum 
		{
			ARG_BATTLEID,		//0
			ARG_POSITION,
			ARG_POSTABLE,		//1

			ARG_MAX_ //= 3
		};

		static GEParam param[ARG_MAX_];

		U8 argIndex = 0;

		param[ARG_BATTLEID].type_ = GEP_INT;
		param[ARG_BATTLEID].value_.i = this->battleId_;

		param[ARG_POSITION].type_ = GEP_INT;
		param[ARG_POSITION].value_.i = (S32)this->battlePosition_;

		param[ARG_POSTABLE].type_ = GEP_POS_TABLE;
		param[ARG_POSTABLE].value_.hPosTable = &posTable;

		std::string err;
		if(false == ScriptEnv::callGEProc(pCore->updateClass_.c_str(), this->handleId_,param,ARG_MAX_,err))
		{
			return false;
		}
	}
	curRoundStates_.clear();
	return true;
}

void 
Entity::clearState(bool battle)
{
	if(battle)
	{
		for (size_t i=0; i<states_.size();)
		{
			StateTable::Core const * pCore = StateTable::getStateById(states_[i].stateId_);
			if(pCore != NULL && pCore->battleDelete_)
			{
				//ACE_DEBUG((LM_ERROR,ACE_TEXT("clear battleState stateid = %d, entity = %d\n"),states_[i].stateId_,getGUID()));
				states_.erase(states_.begin() + i);
			}
			else
			{
				++i;
			}
		}
		curRoundStates_.clear();
	}
}

///@}

///========================================================================
///@group Skill
///@{
void Entity::clearSkills(){
	for (size_t i=0;i<skills_.size(); ++i)
	{
		if(skills_[i])
			DEL_MEM(skills_[i]);
	}
	skills_.clear();
}

void 
Entity::initSkillById(std::vector< std::pair<S32,S32> > const &skillIds)
{///移到表里
	clearSkills();
	for(size_t i=0; i<skillIds.size(); ++i)
	{
		SkillTable::Core const *pCore = SkillTable::getSkillById(skillIds[i].first,skillIds[i].second);
		if(NULL == pCore) return ;
		Skill *skill = NEW_MEM(Skill);
		skill->init(pCore);
		skills_.push_back(skill);
	}
}

void
Entity::initSkillByInst(std::vector<COM_Skill> const &skillInsts)
{
	clearSkills();
	for(size_t i=0; i<skillInsts.size(); ++i)
	{
		SkillTable::Core const *pCore = SkillTable::getSkillById(skillInsts[i].skillID_,skillInsts[i].skillLevel_);
		if(NULL == pCore)
		{
			pCore = SkillTable::getSkillById(skillInsts[i].skillID_,1);
		}
		if(NULL == pCore)
		{
			continue;
		}
		Skill *skill = NEW_MEM(Skill);
		skill->init(pCore);
		
		skill->skExp_ = skillInsts[i].skillExp_;
		skills_.push_back(skill);
	}
}

void 
Entity::learnSkill(S32 skId,S32 skLv)
{
	{
		Baby *b = asBaby();
		if(b){
			Skill* sk= b->getEquipSkill();
			if(sk){
				if(sk->skId_ == skId){
					return;
				}
			}
		}

	}
	
	SkillTable::Core const *pCore = SkillTable::getSkillById(skId,skLv);
	if(NULL == pCore)
		return;
	
	if (SKT_CannotUse == pCore->skType_)
		learnPassiveSkill(pCore->id_,pCore->level_);
	else
	{
		Skill *skill = NEW_MEM(Skill);
		skill->init(pCore);
		skills_.push_back(skill);
	}


	calcFightingForce();
}

void
Entity::learnPassiveSkill(S32 skId, S32 skLv)
{
	SkillTable::Core const *pCore = SkillTable::getSkillById(skId,skLv);
	if(NULL == pCore)
		return ;

	Skill* oldSkill = getSkillById(skId);
	if(oldSkill){
		SkillTable::Core const *pOldCore = SkillTable::getSkillById(oldSkill->skId_,oldSkill->skLevel_);
		if(NULL == pOldCore)
			return ;
		setProp(pCore->resistPropType_,getProp(pCore->resistPropType_)-pCore->resistNum_);

		oldSkill->skLevel_ = skLv;
	}
	else{
	Skill *skill = NEW_MEM(Skill);
	skill->init(pCore);
	skills_.push_back(skill);
	}

	setProp(pCore->resistPropType_,getProp(pCore->resistPropType_)+pCore->resistNum_);
}

void Entity::resetPassiveSkill(){
	for(size_t i=0; i<skills_.size(); ++i){
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(NULL == pCore)
			return ;
		if(pCore->skType_ == SKT_CannotUse)
			setProp(pCore->resistPropType_,getProp(pCore->resistPropType_)+pCore->resistNum_);
	}
}

void 
Entity::forgetSkill(S32 skId)
{
	for (size_t i=0; i<skills_.size(); ++i)
	{
		if (skId == skills_[i]->skId_)
		{
			DEL_MEM(skills_[i]);
			skills_.erase(skills_.begin() + i);
			break;
		}
	}

	calcFightingForce();
}

Skill*
Entity::getSkillById(S32 skId)
{
	for(size_t i=0; i<skills_.size(); ++i)
	{
		if(skills_[i]->skId_ == skId)
			return skills_[i];
	}
	
	Baby* thisBaby = asBaby();

	if(thisBaby){
		Skill* pSk = thisBaby->getEquipSkill();
		if(pSk && pSk->skId_ == skId){
			return pSk;
		}
	}
	
	return NULL;
}

std::vector<S32>
Entity::getSkillIds(){
	return aiSkills_;
}

void
Entity::addSkillExp(S32 skillID, U32 exp,ItemUseFlag flag)
{
	ACE_DEBUG((LM_ERROR,"Can not do it\n"));
}

void 
Entity::setEntityInst(COM_Entity& entity){
	for (size_t i = 0; i < entity.states_.size(); ++i){
		states_.push_back(entity.states_[i]);
	}

	for(size_t i=0;i<entity.equips_.size(); ++i){
		COM_Item *p = NEW_MEM(COM_Item,entity.equips_[i]);
		equipItems_[p->slot_] = p;
	}
}

void Entity::getBattleEntityInformation(COM_BattleEntityInformation& info)
{
	info.instId_= this->getGUID();
	info.instName_ = this->getNameC();
	info.battlePosition_ = battlePosition_;
	info.tableId_ = getProp(PT_TableId);
	info.assetId_ = getProp(PT_AssetId);
	if(equipItems_[ES_SingleHand])
		info.weaponItemId_ = equipItems_[ES_SingleHand]->itemId_;
	else if(equipItems_[ES_DoubleHand])
		info.weaponItemId_ = equipItems_[ES_DoubleHand]->itemId_;
	if(equipItems_[ES_Fashion])
		info.fashionId_ = equipItems_[ES_Fashion]->itemId_;
	
 	info.hpMax_ = getProp(PT_HpMax);
	info.hpCrt_ = getProp(PT_HpCurr);
	info.mpMax_ = getProp(PT_MpMax);
	info.mpCrt_ = getProp(PT_MpCurr);
	info.level_ = getProp(PT_Level);
}

void 
Entity::getEntityInst(COM_Entity& out)
{
	out.instId_= getGUID();
	out.instName_ = getNameC();
	out.properties_ = properties_;
	out.battlePosition_ = battlePosition_;
	for (size_t i=0; i < equipItems_.size(); ++i)
	{
		if( equipItems_[i] != NULL)
		{
			out.equips_.push_back(*equipItems_[i]);	
		}
	}

	for (size_t i=0; i<skills_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = skills_[i]->skId_;
		skill.skillExp_ = skills_[i]->skExp_;
		skill.skillLevel_ = skills_[i]->skLevel_;

		out.skill_.push_back(skill);
	}
	
	for (size_t i=0; i<states_.size(); ++i)
	{
		StateTable::Core const * pCore = StateTable::getStateById(states_[i].stateId_);
		if(!pCore || pCore->battleDelete_)
			continue;
		out.states_.push_back(states_[i]);
	}

	
}

std::vector<COM_Skill>& Entity::calcSkillExp(){
	for(size_t i=0; i<skills_.size(); ++i){
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(NULL == pCore)
			continue;
		if(pCore->skType_ == SKT_CannotUse)
			battleUseSkill_.push_back(skills_[i]->skId_);
	}
	
	for (size_t i=0; i<battleUseSkill_.size(); ++i)
	{
		Skill* psk = getSkillById(battleUseSkill_[i]);
		if(psk == NULL)
		{
			ACE_DEBUG((LM_DEBUG,ACE_TEXT("calcSkillExp can not find skill[%d]\n"),battleUseSkill_[i]));
			continue;
		}

		const SkillTable::Core* pC = SkillTable::getSkillById(psk->skId_,psk->skLevel_);
		if(pC == NULL)
		{
			ACE_DEBUG((LM_DEBUG,ACE_TEXT("calcSkillExp can not find skill[%d] by SkillTable\n"),psk->skId_));
			continue;
		}

		if(asPlayer() && !asPlayer()->canLevelUpSkill(psk->skId_,psk->skLevel_))
			continue;

		addSkillExp(psk->skId_,pC->dropExp_,IUF_Battle);
		COM_Skill sk;
		sk.skillID_ = psk->skId_;
		sk.skillLevel_ = psk->skLevel_;
		sk.skillExp_ = pC->dropExp_;
		useSkillCounter_.push_back(sk);
	}

	return useSkillCounter_;
}

COM_Item* Entity::getEquipment(U32 instId){
	for(size_t i=0;i<equipItems_.size(); ++i){
		if(equipItems_[i] == NULL)	
			continue;
		if(equipItems_[i]->instId_ == instId)
			return equipItems_[i];
	}
	return NULL;
}
S32 Entity::getEquipSlot(const ItemTable::ItemData* item){
	if(item->slot_ == ES_Ornament_0 || item->slot_ == ES_Ornament_1)
	{
		if(equipItems_[ES_Ornament_0]!=NULL && equipItems_[ES_Ornament_1]!=NULL)
		{
			const ItemTable::ItemData *Ornament0= ItemTable::getItemById(equipItems_[ES_Ornament_0]->itemId_);
			if(Ornament0!=NULL && Ornament0->subType_ == item->subType_)
			{
				return ES_Ornament_0;
			}
			const ItemTable::ItemData *Ornament1= ItemTable::getItemById(equipItems_[ES_Ornament_1]->itemId_);
			if(Ornament1!=NULL && Ornament1->subType_ == item->subType_)
			{
				return ES_Ornament_1;
			}
			return ES_Ornament_0;
		}
		else if(equipItems_[ES_Ornament_0] != NULL)
		{
			const ItemTable::ItemData *Ornament0= ItemTable::getItemById(equipItems_[ES_Ornament_0]->itemId_);
			if(Ornament0!=NULL && Ornament0->subType_ == item->subType_)
			{
				return ES_Ornament_0;
			}

			return ES_Ornament_1;
		}
		else if(equipItems_[ES_Ornament_1] != NULL)
		{
			const ItemTable::ItemData *Ornament1= ItemTable::getItemById(equipItems_[ES_Ornament_1]->itemId_);
			if(Ornament1!=NULL && Ornament1->subType_ == item->subType_)
			{
				return ES_Ornament_1;
			}
			return ES_Ornament_0;
		}
		
		return ES_Ornament_0;
	}
	
	return item->slot_;
}

const ItemTable::ItemData* Entity::getEquipmentItemData(EquipmentSlot slot){
	if(slot<=ES_None || slot >=ES_Max)
		return NULL;
	if(NULL == equipItems_[slot])
		return NULL;
	ItemTable::ItemData const *pItem = ItemTable::getItemById(equipItems_[slot]->itemId_);

	return pItem;
}

void Entity::getWeapon(S32& weaponId,WeaponType& weaponType){
	if(equipItems_.empty())
		return;
	if(equipItems_[ES_SingleHand])
		weaponId = equipItems_[ES_SingleHand]->itemId_;
	else if(equipItems_[ES_DoubleHand])
		weaponId = equipItems_[ES_DoubleHand]->itemId_;

	const ItemTable::ItemData* item = ItemTable::getItemById(weaponId);

	if(item){
		weaponType = item->weaponType_;
	}
}

COM_Item* Entity::wearEquipment(COM_Item* equip){
	const ItemTable::ItemData *item = ItemTable::getItemById(equip->itemId_);
	if(NULL == item)
		return NULL;
	
	if(item->subType_ == IST_Shield)
	{
		equip->slot_ = ES_DoubleHand;
	}
	else
	{
		equip->slot_ = getEquipSlot(item);
	}
	COM_Item* takeoffEquip = equipItems_[equip->slot_];
	equipItems_[equip->slot_] = equip;

	if(equipItems_[ES_Ornament_0] == equipItems_[ES_Ornament_1]){
		equipItems_[ES_Ornament_1] = NULL; //强制错误修正 这两个槽位 有可能穿同样的装备
	}
	if(equip->durability_>0 )
	{
		if(equip->durability_<equip->durabilityMax_/2)
			addEquipmentEffect(equip,0.5,true);
		else 
			addEquipmentEffect(equip,1,true);
	}
	else if(equip->durability_<=0 && asEmployee()!= NULL)
	{
		addEquipmentEffect(equip,1,true);
	}

	if(takeoffEquip){
		if(takeoffEquip->durability_>0 )
		{
			if(takeoffEquip->durability_< takeoffEquip->durabilityMax_/2)
				delEquipmentEffect(takeoffEquip,0.5,true);
			else 
				delEquipmentEffect(takeoffEquip,1,true);
		}
	}
	return takeoffEquip;
}

COM_Item* Entity::takeoffEquipment(U32 instId){

	for(size_t i=0;i<equipItems_.size(); ++i){
		if(equipItems_[i] && (equipItems_[i]->instId_ == instId) )
		{
			COM_Item* tmp = equipItems_[i];
			equipItems_[i] = NULL;
			if(tmp->durability_ > 0){
				if(tmp->durability_< tmp->durabilityMax_/2)
					delEquipmentEffect(tmp,0.5,true);
				else 
					delEquipmentEffect(tmp,1,true);
			}
			return tmp;
		}
	}
	return NULL;
}

void Entity::addEquipmentEffect(const COM_Item* equip,float ratio,bool needSkillCost ){
	const ItemTable::ItemData *item = ItemTable::getItemById(equip->itemId_);
	if(NULL == item)
		return ;

	for(size_t i=0;i<equip->propArr.size();i++)
	{
		COM_PropValue prop = equip->propArr[i];
		setProp(prop.type_,getProp(prop.type_) + prop.value_ * ratio);
	}
	if(needSkillCost)
		addSkillCostMana(item->influenceSkill_.first,item->influenceSkill_.second);

	calcFightingForce();
}
void Entity::delEquipmentEffect(const COM_Item* equip,float ratio,bool needSkillCost ){
	const ItemTable::ItemData *item = ItemTable::getItemById(equip->itemId_);
	if(NULL == item)
		return ;
	/*if(equip->durability_ > 0)
	{*/
		for(size_t i=0;i<equip->propArr.size();i++)
		{
			COM_PropValue prop = equip->propArr[i];
			setProp(prop.type_,getProp(prop.type_) - prop.value_ * ratio);
		}
	/*}*/
	if(needSkillCost)
		addSkillCostMana(item->influenceSkill_.first,-item->influenceSkill_.second);

	calcFightingForce();
}

bool 
Entity::castSkill(S32 skId, BattlePosition target, U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam)
{
	Skill *pSk = getSkillById(skId);
	
	if(NULL == pSk){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find skill %d \n"),skId));
		return false ;
	}
	const SkillTable::Core* skdata = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(skdata == NULL)
		return true;

	if(asPlayer() && skdata->dropExp_ != 0){
		battleUseSkill_.push_back(skdata->id_);
	}
	
	if(true == pSk->active(this,target,battleId,posTable,orderParam))
	{
		
		if(asPlayer())
		{
			const Profession* prof = Profession::get((JobType)(S32)getProp(PT_Profession),(S32)getProp(PT_ProfessionLevel));
			if(prof!= NULL)
			{
				bool isDeyi = prof->IsProudSkill(skdata->id_);
				GEParam params[1];
				params[0].type_ = GEP_INT;
				params[0].value_.i = isDeyi ? 1 : 0;

				GameEvent::procGameEvent(GET_UseSkill,params,1,getHandleId());
			}
		}

		return true;
	}

	return false;
}

Battle*
Entity::myBattle()
{ if(isBattle()) return Battle::find(battleId_); return NULL;}

void 
Entity::initBattleStatus(U32 battleId,GroupType battleForce,BattlePosition battlePos,bool initActive /*= false*/){
	battleId_ = battleId;
	battleForce_ = battleForce;
	battlePosition_ = battlePos;
	battleValue_ = 0;
	battleRunaway_ = false;
	battleRunawayNum_ = 0;
	battleActive_ = initActive;
	battleAtkTime_ = WorldServ::instance()->curTime_;
	battleTmpProp_ = properties_;
	for (size_t i=0; i<skills_.size(); ++i){
		SkillTable::Core const *pTmpCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(pTmpCore == NULL || pTmpCore->skType_ == SKT_DefaultActive)
			continue;
		aiSkills_.push_back(skills_[i]->skId_);
	}

	clearAttachedProperties();
}

void 
Entity::cleanBattleStatus(bool resetProperty)
{
	battleId_ = 0;
	battleValue_ = 0;
	battlePosition_ = BP_None;
	battleForce_ = GT_None;
	battleActive_ = false;
	battleRunaway_ = false;
	aiSkills_.clear();
	useSkillCounter_.clear();
	battleUseSkill_.clear();

	clearAttachedProperties();

	if(resetProperty){
		if(battleTmpProp_.empty()){
			return;
		}
		setProp(PT_HpMax, battleTmpProp_[PT_HpMax]);
		setProp(PT_MpMax, battleTmpProp_[PT_MpMax]);
		setProp(PT_HpCurr, battleTmpProp_[PT_HpCurr]);
		setProp(PT_MpCurr, battleTmpProp_[PT_MpCurr]);
		battleTmpProp_.clear();

		///重置 属性 
	}
	else if(isDeadth()){
		setProp(PT_HpCurr,1.0f);
		//setProp(PT_MpCurr,1.0f);
		///重置 属性
	}
	clearState();
	//refreshProperty();
}
void Entity::addAttachedPropertyD(PropertyType pt, float value){
	float val = getProp(pt) * value;
	setProp(pt, getProp(pt) + val);

	COM_PropValue pv;
	pv.type_ = pt;
	pv.value_ = val;
	attachedProperties_.push_back(pv);
}
void Entity::clearAttachedProperties(){
	if(!attachedProperties_.empty()){
		for(size_t i=0; i<attachedProperties_.size(); ++i){
			setProp(attachedProperties_[i].type_,getProp(attachedProperties_[i].type_) - attachedProperties_[i].value_);
		}
		attachedProperties_.clear();
	}
}

bool Entity::isBattleAtkTimeout(){
	if(battleActive_)
		return false;
	bool ret = (WorldServ::instance()->curTime_ - battleAtkTime_) >= Global::get<float>(C_AiAttackTime);
	if(ret)
		battleAtkTime_ = WorldServ::instance()->curTime_;
	return ret;
}
void 
Entity::skillLevelUp(S32 skID)
{

			}


///@}
///========================================================================