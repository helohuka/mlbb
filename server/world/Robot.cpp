//机器人

#include "Robot.h"
#include "tmptable.h"
#include "randName.h"
#include "robotTable.h"
#include "itemtable.h"
#include "player.h"
#include "employee.h"
#include "worldserv.h"

U32 Robot::robotTID_ = 1;

Robot::Robot(U32 robot)
{
	setRobotInst(robot);
}

Robot::Robot(SGE_DBPlayerData& robot)
{
	setRobotInst(robot);
}

Robot::~Robot(){
	//ACE_DEBUG((LM_DEBUG,"Robot::~Robot()\n"));
	for(size_t i=0;i<employees_.size();++i){
		if(employees_[i])
			DEL_MEM(employees_[i]);
	}
	employees_.clear();

	for (size_t i=0; i<babies_.size(); ++i){
		if(babies_[i])
			DEL_MEM(babies_[i]);
	}
	babies_.clear();
}

void
Robot::setRobotInst(U8 robotTableId)
{
	RobotTab::RobotData const* robotData = RobotTab::getRobotDataById(robotTableId);
	
	if(NULL == robotData)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in RobotTab-template-table\n"),robotTableId));
		return ;
	}

	PlayerTmpTable::Core const *tmp = PlayerTmpTable::getTemplateById(robotData->robotTmpId_);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in player-template-table\n"),robotData->robotTmpId_));
		return ;
	}

	robotId_		= MAKE_ROBOT_ID(robotTID_++);
	isPlayerData_	= false;
	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp->properties_[i];
	properties_[PT_Spirit] = 100; ///BUG 409 发现王者怪 (非必现,几率很高）
	properties_[PT_Reply] = 100;
	///从初始化模版计算初始化 1,2 级属性
	float stama	 = properties_[PT_Stama]; 	
	float strength = properties_[PT_Strength];
	float power	 = properties_[PT_Power]; 	
	float speed	 = properties_[PT_Speed];	
	float magic	 = properties_[PT_Magic];	

	CALC_PLAYER_PRO_TRANS_STAMA((*this),stama);
	CALC_PLAYER_PRO_TRANS_STRENGTH((*this),strength);
	CALC_PLAYER_PRO_TRANS_POWER((*this),power);
	CALC_PLAYER_PRO_TRANS_SPEED((*this),speed);
	CALC_PLAYER_PRO_TRANS_MAGIC((*this),magic);

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];

	properties_[PT_Level] = robotData->robotLevel_;
	properties_[PT_TableId] = tmp->id_;
	properties_[PT_Profession] = robotData->job_;
	properties_[PT_ProfessionLevel] = robotData->JobLevel_;

	initSkillById(tmp->defaultSkill_);

	robotName_ = robotData->robotName_;

	class_	= robotData->robotAIclass_;
	babyClass_	= robotData->babyAIclass_;
	Job_			= robotData->job_;
	JobLevel_		= robotData->JobLevel_;
	

	employees_.clear();
	for (size_t i = 0; i < robotData->employees_.size(); ++i)
	{
		EmployeeTable::EmployeeData const*  employee = EmployeeTable::getEmployeeById( robotData->employees_[i]);
		SRV_ASSERT(employee);
		Employee * pEmployee = NEW_MEM(Employee,this);
		SRV_ASSERT(pEmployee);
		COM_EmployeeInst empInst;
		Employee::genEmployeeData(this,employee->Id_,empInst);
		empInst.instId_ = MAKE_ROBOT_ID(robotTID_++);
		pEmployee->setEmployeeInst(empInst);
		employees_.push_back(pEmployee);
	}

	const MonsterClass::Core* robotClazz = MonsterClass::getClassByName(class_);
	const MonsterClass::Core* babyClazz = MonsterClass::getClassByName(babyClass_);
	SRV_ASSERT(robotClazz);
	SRV_ASSERT(babyClazz);

	babyEvents_		= babyClazz->events_;
	robotEvents_	= robotClazz->events_;
	addBabyFromTemlate(robotData->babyId_,robotData->babyLevel_);

	for (size_t i = 0; i < robotData->equips_.size(); ++i)
	{
		COM_Item* pItem = genItemInst(robotData->equips_[i]);
		equipItems_[pItem->slot_] = pItem;
	}

	wearEquipment();
}

void 
Robot::setRobotInst(SGE_DBPlayerData &tmp)
{
	isPlayerData_	= true;
	robotId_ = tmp.instId_;
	robotName_ = tmp.instName_;
	properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		properties_[i] = tmp.properties_[i];

	initSkillByInst(tmp.skill_);

	babies_.clear();
	for(S32 i=0; i<tmp.babies_.size(); ++i)
	{
		if(!tmp.babies_[i].isBattle_)
			continue;
		Baby * pBaby = NEW_MEM(Baby,this);
		SRV_ASSERT(pBaby);
		pBaby->setBabyInst(tmp.babies_[i]);
		babies_.push_back(pBaby);
	}

	employees_.clear();

	for(S32 i=0; i<tmp.employees_.size(); ++i)
	{
		if(!tmp.employees_[i].isBattle_)
			continue;
		Employee * pEmployee = NEW_MEM(Employee,this);
		SRV_ASSERT(pEmployee);
		pEmployee->setEmployeeInst(tmp.employees_[i]);
		employees_.push_back(pEmployee);
	}

	equipItems_.resize(ES_Max ,NULL);
	for ( size_t i = 0 ; i < tmp.equips_.size(); ++i )
	{
		COM_Item *pItem = NEW_MEM(COM_Item,tmp.equips_[i]);
		equipItems_[pItem->slot_] = pItem;
	}

	PlayerAI const* pData = PlayerAI::getAI((JobType)(U32)properties_[PT_Profession]);

	if (pData == NULL)
	{
		ACE_DEBUG((LM_DEBUG,"-----------------JJC BDplayerAI is nil--------------------\n"));
		return;
	}

	class_	= pData->playerClass_;
	babyClass_	= pData->babyClass_;

	const MonsterClass::Core* robotClazz = MonsterClass::getClassByName(class_);
	const MonsterClass::Core* babyClazz = MonsterClass::getClassByName(babyClass_);
	SRV_ASSERT(robotClazz);
	SRV_ASSERT(babyClazz);

	babyEvents_		= babyClazz->events_;
	robotEvents_	= robotClazz->events_;
}
void 
Robot::getRobotInst(COM_SimplePlayerInst &out)
{
	out.type_ = ET_Player;
	out.instId_= robotId_;
	out.instName_ = robotName_;
	out.battlePosition_ = battlePosition_;
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = (S32)properties_[i];

	for (size_t i=0; i<skills_.size(); ++i){
		COM_Skill skill;
		skill.skillID_ = skills_[i]->skId_;
		skill.skillExp_ = skills_[i]->skExp_;
		skill.skillLevel_ = skills_[i]->skLevel_;
		out.skill_.push_back(skill);
	}

	for (size_t i=0; i < equipItems_.size(); ++i){
		if( equipItems_[i] != NULL){
			out.equips_.push_back(*equipItems_[i]);	
		}
	}

	for (size_t i = 0; i < babies_.size(); ++i)
	{
		COM_BabyInst inst;
		babies_[i]->getBabyInst(inst);
		out.babies1_.push_back(inst);
	}
}

void 
Robot::addBabyFromTemlate(U32 uBabyTmpId,U32 uLevel)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(uBabyTmpId);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),uBabyTmpId));
		return;
	}

	Baby *p = NEW_MEM(Baby,this);
	p->babyId_ = MAKE_ROBOT_ID(robotTID_++);
	SRV_ASSERT(p);
	p->setPropFromTable(tmp,uLevel);
	p->properties_[PT_Level] = (float)uLevel;
	
	babies_.push_back(p);
}

COM_Item* 
Robot::genItemInst(U32 itemId)
{
	ItemTable::ItemData const* core = ItemTable::getItemById(itemId);
	if(core  == NULL)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Cant find item in ItemTable  %d \n"),itemId));
		return NULL;
	}
	
	COM_Item * pItem = NEW_MEM(COM_Item);
	SRV_ASSERT(pItem);

	pItem->itemId_ = itemId; 
	pItem->instId_ = ++genItemMaxGuid_;
	pItem->slot_ = core->slot_;

	for(size_t i =0;i<core->propValue_.size();i++)
	{
		COM_PropValue  prop ;
		prop.type_ = core->propValue_[i].type_;
		std::vector<S32> propArr = core->propValue_[i].value_;

		S32 value =0;
		if(propArr [0] == propArr [1])
		{
			value  = propArr [0];
		}
		else
		{	
			if(propArr [0] < 0 && propArr [1] < 0)
			{
				value = -rand()%(abs(propArr [1]) - abs(propArr [0])) + abs(propArr[0]);
			}
			else
			{
				value = rand()%(propArr [1] - propArr [0]) + propArr[0];
			} 
		}
		prop.value_ = value;
		pItem->propArr.push_back(prop);
	}

	return pItem;
}

void
Robot::wearEquipment()
{
	for (size_t i = 0; i < equipItems_.size(); ++i)
	{
		if(!equipItems_[i])
			continue;
		//更新I属性
		for(size_t j=0;j<equipItems_[i]->propArr.size();j++)
		{
			COM_PropValue prop = equipItems_[i]->propArr[j];
			float propV =  getProp(prop.type_);
			setProp(prop.type_,propV +prop.value_);
		}
	}
}

const ItemTable::ItemData*
Robot::getWearEquipData(EquipmentSlot eSlot)
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
