#include "employee.h"
#include "player.h"
#include "monstertable.h"
#include "itemtable.h"
#include "EmployeeConfig.h"
#include "worldserv.h"
#include "GameEvent.h"
#include "profession.h"

Employee::Employee():Entity(),owner_(NULL),isBattle_(false){

}

Employee::~Employee(){
	//ACE_DEBUG((LM_DEBUG,"Employee::~Employee()\n"));
}

ClientHandler*
Employee::getClient(){
	if(owner_)return owner_->getClient();return NULL;
}

Employee::Employee(InnerPlayer* owner)
:owner_(owner)
,isBattle_(false)
{
	ownerName_ = owner_->getNameC();
	equipItems_.resize(ES_Max ,NULL);
}



void Employee::genEmployeeData(InnerPlayer* owner, U32 table, COM_EmployeeInst& out)
{
	EmployeeTable::EmployeeData const * tmp = EmployeeTable::getEmployeeById(table);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d EmployeeData form EmployeeTable-table\n"),table));
		return ;
	}

	float grow = 0;
	if(tmp->quality_ > tmp->grows_.size() || tmp->grows_[tmp->quality_] == 0){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find grow[%d] form employee-table\n"),tmp->quality_ ));
	}
	//资质系数=资质*0.1
	grow = tmp->grows_[tmp->quality_] * 0.1;

	out.instId_ = WorldServ::instance()->getMaxGuid();
	out.instName_ = tmp->name_;
	out.ownerName_ = owner->getNameC();
	
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = tmp->properties_[i];

	out.properties_[PT_Level] = owner->getProp(PT_Level);
	out.properties_[PT_TableId] = table;
	///从初始化模版计算初始化 1，2 级属性
	out.properties_[PT_Stama] *= out.properties_[PT_Level]; 	
	out.properties_[PT_Strength]*= out.properties_[PT_Level];
	out.properties_[PT_Power]*= out.properties_[PT_Level]; 	
	out.properties_[PT_Speed]*= out.properties_[PT_Level];	
	out.properties_[PT_Magic]*= out.properties_[PT_Level];	
	
	out.properties_[PT_Stama] *=grow;
	out.properties_[PT_Strength] *= grow;
	out.properties_[PT_Power]*= grow;
	out.properties_[PT_Speed]*= grow;
	out.properties_[PT_Magic]*= grow;

	out.properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	out.properties_[PT_Reply] = 100;
	out.quality_		= tmp->quality_;
	out.star_		= tmp->star_;
	CALC_PLAYER_PRO_TRANS_STAMA(out,out.properties_[PT_Stama]);
	CALC_PLAYER_PRO_TRANS_STRENGTH(out,out.properties_[PT_Strength]);
	CALC_PLAYER_PRO_TRANS_POWER(out,out.properties_[PT_Power]);
	CALC_PLAYER_PRO_TRANS_SPEED(out,out.properties_[PT_Speed]);
	CALC_PLAYER_PRO_TRANS_MAGIC(out,out.properties_[PT_Magic]);

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
			out.properties_[pCore->resistPropType_] += pCore->resistNum_; ///计算被动技能( 佣兵不会学习和遗忘 被动技能 直接初始化加上
		}
	}

	float ff = CALC_BASE_FightingForceCOM(out);
	for(int i=0; i<out.skill_.size(); ++i)
		ff += CALC_SKILL_FightingForceCOM(out.skill_[i]);
	out.properties_[PT_FightingForce] = ff;

	
}

void Employee::skillLevelUp(S32 skillId){
	Skill* pSk = getSkillById(skillId);
	if(pSk == NULL)
		return;
	Profession const * prof = Profession::get((JobType)(int)getProp(PT_Profession),(int)getProp(PT_ProfessionLevel));
	if(NULL == prof)
		return; //没有职业

	SkillTable::Core const *pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(pCore == NULL) return;
	SkillTable::Core const *pNextCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_ + 1);
	if(pNextCore == NULL) return;
	if(pCore->skType_ == SKT_CannotUse){
		setProp(pCore->resistPropType_,getProp(pCore->resistPropType_) - pCore->resistNum_);
	}
	
	if(pCore->skType_ == SKT_CannotUse){
		setProp(pCore->resistPropType_,getProp(pNextCore->resistPropType_) + pNextCore->resistNum_);
	}
	pSk->skLevel_++;
	pSk->reset();


	COM_Skill inst;
	inst.skillID_ = pSk->skId_;
	inst.skillExp_ = pSk->skExp_;
	inst.skillLevel_ = pSk->skLevel_;

	CALL_CLIENT(owner_,skillLevelUp(getGUID(),inst));
}

void
Employee::syncProp()
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

void Employee::levelup()
{
	EmployeeTable::EmployeeData const *tmp = EmployeeTable::getEmployeeById(getProp(PT_TableId));
	if(NULL == tmp)
		return ;
	
	U32 level = getProp(PT_Level);
	COM_EmployeeInst inst;
	updateProp((int)properties_[PT_TableId], inst);
	inst.properties_[PT_HpCurr] = properties_[PT_HpCurr];
	inst.properties_[PT_MpCurr] = properties_[PT_MpCurr];
	float hp = inst.properties_[PT_HpMax] - properties_[PT_HpMax];
	float mp = inst.properties_[PT_MpMax] - properties_[PT_MpMax];
	if(hp > 0)
	{
		inst.properties_[PT_HpCurr] += hp;
		if(inst.properties_[PT_HpCurr] > inst.properties_[PT_HpMax])
		{
			inst.properties_[PT_HpCurr] = inst.properties_[PT_HpMax];
		}
	}
	if(mp > 0)
	{
		inst.properties_[PT_MpCurr] += mp;
		if(inst.properties_[PT_MpCurr] > inst.properties_[PT_MpMax])
		{
			inst.properties_[PT_MpCurr] = inst.properties_[PT_MpMax];
		}
	}
	setProp(PT_HpMax,inst.properties_[PT_HpMax]);
	setProp(PT_MpMax,inst.properties_[PT_MpMax]);
	for(size_t i=0; i<inst.properties_.size(); ++i){
		setProp((PropertyType)i,inst.properties_[i]);
	}

	setProp(PT_Level,level);

	resetPassiveSkill();
	calcFightingForce();
}

void Employee::setEmployeeInst(COM_EmployeeInst &tmp)
{
	EmployeeTable::EmployeeData const*  edata = EmployeeTable::getEmployeeById(tmp.properties_[PT_TableId]);
	SRV_ASSERT(edata);
	
	instName_	= tmp.instName_;
	instId_		= tmp.instId_;
	color_		= tmp.quality_;
	star_		= tmp.star_;
	soul_		= tmp.soul_;
	isBattle_   = tmp.isBattle_;
	className_  = edata->AIClassName_;
	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp.properties_[i];

	properties_[PT_Level] = owner_->getProp(PT_Level);
	const MonsterClass::Core* clazz = MonsterClass::getClassByName(className_);
	employeeEvents_	= clazz->events_;
	initSkillByInst(tmp.skill_);
	for (size_t i=0; i < tmp.equips_.size(); ++i){
		equipItems_[tmp.equips_[i].slot_] = NEW_MEM(COM_Item,tmp.equips_[i]) ;	
	}
}

void Employee::getEmployeeInst(COM_EmployeeInst &out)
{
	out.ownerName_		= owner_->getNameC();
	out.type_			= ET_Emplyee;
	out.instId_			= instId_;
	out.instName_		= instName_;
	out.battlePosition_ = battlePosition_;
	out.quality_		= color_;
	out.star_			= star_;
	out.soul_			= soul_;
	out.isBattle_  = isBattle_;
	if(equipItems_[ES_SingleHand])
		out.weaponId_ = equipItems_[ES_SingleHand]->itemId_;
	else if(equipItems_[ES_DoubleHand])
		out.weaponId_ = equipItems_[ES_DoubleHand]->itemId_;
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = (S32)properties_[i];

	out.properties_[PT_Level] = owner_->getProp(PT_Level);

	for (size_t i=0; i<skills_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = skills_[i]->skId_;
		skill.skillExp_ = skills_[i]->skExp_;
		skill.skillLevel_ = skills_[i]->skLevel_;
		out.skill_.push_back(skill);
	}

	for (size_t i=0; i < equipItems_.size(); ++i)
	{
		if( equipItems_[i] != NULL)
		{
			out.equips_.push_back(*equipItems_[i]);	
		}
	}
}

void Employee::getBattleEntityInformation(COM_BattleEntityInformation& info){
	Entity::getBattleEntityInformation(info);
	info.type_ = ET_Emplyee;
	info.instName_ = instName_;
}

void 
Employee::postEvent(AIEvent me, BattlePosition target, std::map<S32,S32> &posTable)
{
	if(isDeadth())
		return;
	if (employeeEvents_[me].empty() || employeeEvents_[me].length() == 0)
	{
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
	param[ARG_SKILLTABLE].type_		= GEP_HANDLE_ARRAY;
	param[ARG_SKILLTABLE].value_.hArray	= &aiSkills_;
	
	std::string err;
	if( !ScriptEnv::callGEProc(employeeEvents_[me].c_str(),getHandleId(),param,ARG_MAX_,err) )
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Employee post event %s\n"),err.c_str()));
		return;
	}
}

const ItemTable::ItemData*
Employee::getWearEquipData(EquipmentSlot eSlot)
{
	for (size_t i=0; i<equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL)
			continue;

		ItemTable::ItemData const *pItem = ItemTable::getItemById(equipItems_[i]->itemId_);

		if(pItem == NULL)
			continue;

		if (eSlot == equipItems_[i]->slot_)
			return pItem;
	}

	return NULL;
}

void
Employee::evolve()
{
	if(owner_->asPlayer() == NULL)
		return;
	if(color_ >= QC_Pink || color_ < QC_White)
		return;

	U32 tmp = (U32)color_;

	tmp++;
	color_ = (QualityColor)tmp;
	///需重新计算属性
	evolveresetprop();

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = color_;
	GameEvent::procGameEvent(GET_EmployeeEvolve,params,1,owner_->getHandleId());

	CALL_CLIENT(owner_,evolveOK(instId_,color_));
}

void
Employee::evolveresetprop()
{
	EmployeeTable::EmployeeData const * tmp = EmployeeTable::getEmployeeById(getProp(PT_TableId));
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d EmployeeData form EmployeeTable-table\n"),getProp(PT_TableId)));
		return ;
	}

	//资质系数=资质*0.1
	float grow = tmp->grows_[color_] * 0.1;


	COM_EmployeeInst out;
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = tmp->properties_[i];
	out.properties_[PT_Level] = owner_->getProp(PT_Level);
	out.properties_[PT_TableId] = tmp->Id_;
	///从初始化模版计算初始化 1，2 级属性
	out.properties_[PT_Stama] *= out.properties_[PT_Level]; 	
	out.properties_[PT_Strength]*= out.properties_[PT_Level];
	out.properties_[PT_Power]*= out.properties_[PT_Level]; 	
	out.properties_[PT_Speed]*= out.properties_[PT_Level];	
	out.properties_[PT_Magic]*= out.properties_[PT_Level];	
	
	out.properties_[PT_Stama] *=grow;
	out.properties_[PT_Strength] *= grow;
	out.properties_[PT_Power]*= grow;
	out.properties_[PT_Speed]*= grow;
	out.properties_[PT_Magic]*= grow;

	out.properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	out.properties_[PT_Reply] = 100;
	
	CALC_PLAYER_PRO_TRANS_STAMA(out,out.properties_[PT_Stama]);
	CALC_PLAYER_PRO_TRANS_STRENGTH(out,out.properties_[PT_Strength]);
	CALC_PLAYER_PRO_TRANS_POWER(out,out.properties_[PT_Power]);
	CALC_PLAYER_PRO_TRANS_SPEED(out,out.properties_[PT_Speed]);
	CALC_PLAYER_PRO_TRANS_MAGIC(out,out.properties_[PT_Magic]);

	out.properties_[PT_HpCurr] = out.properties_[PT_HpMax];
	out.properties_[PT_MpCurr] = out.properties_[PT_MpMax];

	for(size_t i=0; i<out.properties_.size(); ++i){
		setProp((PropertyType)i,out.properties_[i]);
	}

	setProp(PT_HpCurr,properties_[PT_HpMax]);
	setProp(PT_MpCurr,properties_[PT_MpMax]);

	std::vector< COM_Item* > items;
	for (size_t i = star_; i > 0; --i)
	{
		if(star_ == i)
		{
			for (size_t t = 0; t < equipItems_.size(); ++t)
			{
				if(equipItems_[t] == NULL)
					continue;
				addEquipmentEffect(equipItems_[t]);
				
			}
		}
		else
		{
			EmployeeConfigTable::EmployeeConfigData const* empConfig = EmployeeConfigTable::getEmployeeConfig(getProp(PT_TableId),i);
			if(empConfig == NULL)
				return;

			for (size_t j = 0; j < empConfig->equips_.size(); ++j)
			{
				if(empConfig->equips_[j] == 0)
					continue;

				owner_->asPlayer()->genItemInst(empConfig->equips_[j],1,items);
				if(items.empty())
					continue;
				addEquipmentEffect(items[0]);
				DEL_MEM(items[0]);
				items.clear();
			}
		}
	}

	for (size_t i=0;i<skills_.size();++i)
	{
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(pCore == NULL) return;
		if(pCore->skType_ == SKT_CannotUse){
			for (size_t j=1;j<=pCore->level_;++j)
			{
				SkillTable::Core const *p = SkillTable::getSkillById(pCore->id_,j);
				if(p == NULL) continue;
				float propv = getProp(p->resistPropType_)+p->resistNum_;
				setProp(p->resistPropType_,propv);
			}
		}
	}
	
	calcFightingForce();
}

void
Employee::upStar()
{
	if(star_ < 1)
		return;

	star_++;
	///保留当前装备附加属性,删除装备
	for(size_t  i = 0;i < equipItems_.size();i++)
	{
		if( equipItems_[i] == NULL)
			continue;
		CALL_CLIENT(owner_,delEquipmentOk(getGUID(),equipItems_[i]->instId_));
		DEL_MEM(equipItems_[i]);
		equipItems_[i] = NULL;
	}
	
	COM_Skill csk;
	CALL_CLIENT(owner_,upStarOK(instId_,star_,csk));
}

void
Employee::cleanBattleStatus(bool resetProperty)
{
	if(battleId_ != 0)
		Entity::cleanBattleStatus(resetProperty);
	setProp(PT_HpCurr,getProp(PT_HpMax));
	setProp(PT_MpCurr,getProp(PT_MpMax));
}

void
Employee::updateProp(U32 table,COM_EmployeeInst& out)
{
	EmployeeTable::EmployeeData const * tmp = EmployeeTable::getEmployeeById(table);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d EmployeeData form EmployeeTable-table\n"),table));
		return ;
	}

	float grow = 0;
	if(tmp->quality_ > tmp->grows_.size() || tmp->grows_[color_] == 0)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find grow[%d] form Employee-table\n"),tmp->quality_ ));
	}
	
	//资质系数=资质*0.1
	grow = tmp->grows_[color_] * 0.1;

	out.properties_.resize(PT_Max);
	COM_EmployeeInst inst;
	inst.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		inst.properties_[i] = tmp->properties_[i];
	///从初始化模版计算初始化 1，2 级属性
	inst.properties_[PT_Stama]*= grow;	
	inst.properties_[PT_Strength]*= grow;
	inst.properties_[PT_Power]*= grow;
	inst.properties_[PT_Speed]*= grow;	
	inst.properties_[PT_Magic]*= grow;	

	inst.properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	inst.properties_[PT_Reply] = 100;

	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = properties_[i];

	CALC_PLAYER_PRO_TRANS_STAMA(out,inst.properties_[PT_Stama]);
	CALC_PLAYER_PRO_TRANS_STRENGTH(out,inst.properties_[PT_Strength]);
	CALC_PLAYER_PRO_TRANS_POWER(out,inst.properties_[PT_Power]);
	CALC_PLAYER_PRO_TRANS_SPEED(out,inst.properties_[PT_Speed]);
	CALC_PLAYER_PRO_TRANS_MAGIC(out,inst.properties_[PT_Magic]);

	out.properties_[PT_HpCurr] = out.properties_[PT_HpMax];
	out.properties_[PT_MpCurr] = out.properties_[PT_MpMax];
}

bool
Employee::canUseItem(U32 itemId)
{
	EmployeeConfigTable::EmployeeConfigData const* empConfig = EmployeeConfigTable::getEmployeeConfig(getProp(PT_TableId),star_);
	if(empConfig == NULL)
		return false;
	ItemTable::ItemData const* item = ItemTable::getItemById(itemId);
	if(item == NULL)
		return false;
	if( getProp(PT_Level)/10+1 < item->level_ )
		return false;
	for (size_t i = 0; i < empConfig->equips_.size(); ++i)
	{
		if(empConfig->equips_[i] == itemId && i == item->slot_)
			return true;
	}

	return false;
}