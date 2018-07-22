#include "player.h"
#include "employee.h"
#include "employeeTable.h"
#include "EmployeeConfig.h"
#include "GameEvent.h"
#include "dbhandler.h"

bool Player::addEmployee(U32 employeeId)
{
	/*if(isBattle())
		return true;*/
	EmployeeTable::EmployeeData const*  employee = EmployeeTable::getEmployeeById(employeeId);
	if(employee  == NULL)
		return false;
	Employee* pEmployee = getEmployeeByTableId(employeeId);
	if(pEmployee != NULL)
	{
		calcEmployeeSoul(pEmployee);
	}
	else
	{
		COM_EmployeeInst  inst;
		Employee::genEmployeeData(this,employeeId,inst);
		DBHandler::instance()->createEmployee(playerName_,inst);

		Employee* p = NEW_MEM(Employee,this);
		p->setEmployeeInst(inst);
		employees_.push_back(p);

		CALL_CLIENT(this,addEmployee(inst));
	}

	GEParam params[3];
	params[0].type_ = GEP_INT;
	params[0].value_.i = employeeId;
	params[1].type_ = GEP_INT;
	params[1].value_.i = employee->quality_;
	params[2].type_ = GEP_INT;
	params[2].value_.i = employees_.size();
	GameEvent::procGameEvent(GET_RecruitEmp,params,3,getHandleId());

	return true;
}

bool
Player::delEmployee(U32 empInstId)
{
	if(isBattle())
		return true;
	std::string empname = "";
	std::vector<U32> delEmployees;
	for (size_t i=0; i<employees_.size(); ++i)
	{
		if(employees_[i]->instId_ == empInstId)
		{
			if(findBattleEmployee(EBG_GroupOne,empInstId))
				setBattleEmployee(empInstId,EBG_GroupOne,false);
			if(findBattleEmployee(EBG_GroupTwo,empInstId))
				setBattleEmployee(empInstId,EBG_GroupTwo,false);
			empname = employees_[i]->getNameC();
			DEL_MEM(employees_[i]);
			employees_.erase(employees_.begin() + i);
			delEmployees.push_back(empInstId);
			break;
		}
	}
	if(delEmployees.empty())
		return false;
	ACE_DEBUG((LM_INFO,"PLAYER DELETE EMPLOYEE ==> PLAYERNAME[%s] EMPLOYEENAME[%s] INSTID[%d]\n",getNameC(),empname.c_str(),empInstId));
	CALL_CLIENT(this, delEmployeeOK(delEmployees));
	DBHandler::instance()->deleteEmployee(playerName_,delEmployees);

	return true;
}

void
Player::dismissalEmployees(std::vector< U32 >& instIds)
{
	if(isBattle())
		return;
	if(instIds.empty())
		return;
	std::vector<U32> delEmployees;
	for(size_t i=0; i<employees_.size(); ++i){
		if(std::find(instIds.begin(),instIds.end(),employees_[i]->instId_) == instIds.end())
			continue;
		if(calcEmployeeCurrency(employees_[i]) == false)
			continue;
		if(findBattleEmployee(EBG_GroupOne,employees_[i]->instId_))
			setBattleEmployee(employees_[i]->instId_,EBG_GroupOne,false);
		if(findBattleEmployee(EBG_GroupTwo,employees_[i]->instId_))
			setBattleEmployee(employees_[i]->instId_,EBG_GroupTwo,false);
		delEmployees.push_back(employees_[i]->instId_);
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("dismissalEmployees ===> ownername[%s]==employeename[%s]===employeeinstid[%d]\n"),getNameC(),employees_[i]->getNameC(),employees_[i]->instId_));
		DEL_MEM(employees_[i]);
		employees_.erase(employees_.begin() + i--);
		
	}
	
	if(delEmployees.empty())
		return;
	CALL_CLIENT(this, delEmployeeOK(delEmployees));
	DBHandler::instance()->deleteEmployee(playerName_,delEmployees);
}

bool
Player::delEmployeeOk(std::vector< U32 >& instIds)
{
	/*for (size_t k = 0; k < instIds.size(); ++k)
	{
		if(findBattleEmp(EBG_GroupOne,instIds[k]))
			setBattleEmp(instIds[k],EBG_GroupOne,false);
		if(findBattleEmp(EBG_GroupTwo,instIds[k]))
			setBattleEmp(instIds[k],EBG_GroupTwo,false);
		Employee* emp = findEmployee(instIds[k]);
		if(emp == NULL)
			continue;
		employees_.erase(std::find(employees_.begin(),employees_.end(),emp));	
		delete emp;
		
	}
	CALL_CLIENT(this, delEmployeeOK(instIds));*/
	return true;
}

Employee*
Player::findEmployee(U32 instID)
{
	if(0 == instID)
		return NULL;
	for (size_t i = 0; i < employees_.size();++i)
	{
		if (employees_[i]->instId_ == instID)
		{
			return employees_[i];
		}
	}
	return NULL;
}

U32
Player::findEmployeebyTableid(U32 tableid){
	for (size_t i=0; i<employees_.size();++i)
	{
		if(employees_[i] && employees_[i]->getProp(PT_TableId) == tableid)
			return employees_[i]->instId_;
	}
	return 0;
}

void
Player::removeEmployee()
{
	if(isBattle())
		return;
	std::vector<U32> delEmployees;
	std::stringstream ss;
	for (size_t i = 0; i < employees_.size(); ++i)
	{
		if(employees_[i]->isBattle_ || employees_[i]->color_ > QC_Blue)
			continue;
		//回收处理
		if(calcEmployeeCurrency(employees_[i]) == false)
			continue;
		delEmployees.push_back(employees_[i]->instId_);
		ss <<"employeename:" << employees_[i]->getNameC()<<"=="<< "instid:" <<employees_[i]->instId_ << ",";
		DEL_MEM(employees_[i]);
		employees_.erase(employees_.begin() + i--);
	}
	if(delEmployees.empty())
		return;
	ACE_DEBUG((LM_INFO,"Player[%s] remove employee===> %s\n",getNameC(),ss.str().c_str()));
	CALL_CLIENT(this, delEmployeeOK(delEmployees));
	DBHandler::instance()->deleteEmployee(playerName_,delEmployees);
}

void
Player::dismissalSoul(U32 instid, U32 soulNum)
{
	Employee* pEmp = findEmployee(instid);
	if(pEmp == NULL)
		return;
	if(pEmp->soul_ == 0)
		return;
	if(soulNum == 0)
		return;
	if(pEmp->soul_ < soulNum)
		return;

	U32 tableid = pEmp->getProp(PT_TableId);
	EmployeeTable::EmployeeData const *pEmpData = EmployeeTable::getEmployeeById(tableid);
	if(pEmpData == NULL)
		return;
	float point = CALC_EMPLOYEERECYCLE_POINT(pEmpData->quality_) * soulNum;
	setProp(PT_EmployeeCurrency,getProp(PT_EmployeeCurrency) + point);
	pEmp->soul_ -= soulNum;
	CALL_CLIENT(this,sycnEmployeeSoul(instid,pEmp->soul_));
}

void
Player::setBattleEmployee(U32 instID,EmployeesBattleGroup group, bool isBattle)
{
	if(this->isBattle())
		return;
	Employee* pEmp = findEmployee(instID);

	if(!pEmp)
		return;
	std::vector<U32> &grouplist = group == EBG_GroupTwo ? battleEmpsGroup2_ : battleEmpsGroup1_;

	if(isBattle)
	{
		for (size_t i = 0; i < grouplist.size(); ++i)
		{
			Employee* pTmp = findEmployee(grouplist[i]);
			if(pTmp == NULL)
				continue;
			//同名伙伴不能在同一组里
			if(pEmp->instName_ == pTmp->instName_)
				return;
		}
		for (size_t i = 0; i < grouplist.size(); ++i)
		{
			if(grouplist[i] == 0)
			{
				pEmp->isBattle_ = isBattle;
				grouplist[i] = pEmp->instId_;
				break;
			}
		}
	}
	else
	{
		for (size_t i = 0; i < grouplist.size(); ++i)
		{
			if(grouplist[i] == instID)
			{
				EmployeesBattleGroup temp = group == EBG_GroupTwo ? EBG_GroupOne : EBG_GroupTwo;
				bool inother = findBattleEmployee(temp,instID);
				if(!inother)
					pEmp->isBattle_ = isBattle;
				grouplist[i] = 0;
				break;
			}
		}
	}
	
	CALL_CLIENT(this,battleEmployee(instID,group,pEmp->isBattle_));
}

bool
Player::findBattleEmployee(EmployeesBattleGroup group,U32 instId)
{
	std::vector<U32> &grouplist = group == EBG_GroupTwo ? battleEmpsGroup2_ : battleEmpsGroup1_;
	return std::find(grouplist.begin(),grouplist.end(),instId) != grouplist.end();
}

std::vector<U32>&
Player::getCurrentBattleEmployees()
{
	return empbattlegroup_ == EBG_GroupTwo ? battleEmpsGroup2_: battleEmpsGroup1_;
}

void
Player::empEvolve(U32 instId)
{
	if(isBattle())
		return ;
	Employee* pEmp = findEmployee(instId);

	if(pEmp == NULL)
		return;

	EmployeeTable::EmployeeData const *pEmpData = EmployeeTable::getEmployeeById(pEmp->getProp(PT_TableId));

	if(pEmpData == NULL)
		return;

	if(pEmp->color_ > pEmpData->evolutionNum_.size())
		return;

	U32 needNum = pEmpData->evolutionNum_[pEmp->color_ - 1];

	if(pEmp->soul_ < needNum)
		return;
	pEmp->soul_ -= needNum;
	CALL_CLIENT(this,sycnEmployeeSoul(pEmp->instId_,pEmp->soul_));

	pEmp->evolve();
}

void
Player::empUpstar(U32 instId)
{
	if(isBattle())
		return ;
	Employee* pEmp = findEmployee(instId);

	if(pEmp == NULL)
		return;

	EmployeeConfigTable::EmployeeConfigData const* empConfig = EmployeeConfigTable::getEmployeeConfig(pEmp->getProp(PT_TableId),pEmp->star_);
	if(empConfig == NULL)
		return;
	if(getProp(PT_Money) < empConfig->money_)
	{
		CALL_CLIENT(this,errorno(EN_MoneyLess));
		return;
	}
	if(pEmp->equipItems_[ES_Boot] == NULL || pEmp->equipItems_[ES_SingleHand] == NULL || pEmp->equipItems_[ES_Ornament_0] == NULL || pEmp->equipItems_[ES_Head] ==NULL || pEmp->equipItems_[ES_Body] ==NULL)
	{
		CALL_CLIENT(this,errorno(EN_Materialless));
		return;
	}
	ACE_DEBUG((LM_INFO,"Player upstar employee %d %s %d\n",getGUID(),getNameC(),pEmp->instId_));
	pEmp->upStar();
}

void 
Player::empSkillLevelUp(U32 empId, S32 skillId)
{
	Employee* pEmp = findEmployee(empId);
	if(pEmp == NULL)
		return;
	Skill* pSk = pEmp->getSkillById(skillId);
	if(pSk == NULL)
		return;
	SkillTable::Core  const *pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_); 
	if(pCore  == NULL)
		return;
	if(!canLevelUpSkill(pSk->skId_,pSk->skLevel_))
		return;
	U32 need = EmployeeTable::getSkillLevelUpNeed(pEmp->getProp(PT_TableId),pSk->skLevel_);
	if(need == 0)
		return;
	if(getProp(PT_EmployeeCurrency) < need)
		return;
	U32 cur = getProp(PT_EmployeeCurrency) - need;
	setProp(PT_EmployeeCurrency,cur);
	pEmp->skillLevelUp(skillId);
	pEmp->calcFightingForce();
}

void
Player::changeEmpBattleGroup(EmployeesBattleGroup group)
{
	if(isBattle())
		return ;
	empbattlegroup_ = group;
	CALL_CLIENT(this, changeEmpBattleGroupOK(group));
}

Employee*
Player::getEmployeeByTableId(U32 tableId)
{
	for (size_t i = 0; i < employees_.size(); ++i)
	{
		if(employees_[i]->getProp(PT_TableId) == tableId)
		{
			return employees_[i];
		}
	}
	return NULL;
}

void
Player::calcEmployeeSoul(Employee* employee)
{
	//if(isBattle())
	//	return;
	if(!employee)
		return;
	U32 tableid = employee->getProp(PT_TableId);
	EmployeeTable::EmployeeData const *pEmpData = EmployeeTable::getEmployeeById(tableid);
	if(pEmpData == NULL)
		return;
	employee->soul_ += pEmpData->soul_;
	CALL_CLIENT(this,sycnEmployeeSoul(employee->instId_,employee->soul_));
}

bool
Player::calcEmployeeCurrency(Employee* employee)
{
	if(isBattle())
		return false;
	if(employee == NULL)
		return false;
	if(employee->soul_ == 0)
	{
		float point = CALC_EMPLOYEERECYCLE_POINT(employee->color_);
		setProp(PT_EmployeeCurrency,getProp(PT_EmployeeCurrency) + point);
	}
	else
	{	
		U32 tableid = employee->getProp(PT_TableId);
		EmployeeTable::EmployeeData const *pEmpData = EmployeeTable::getEmployeeById(tableid);
		if(pEmpData == NULL)
			return false;
		float point = CALC_EMPLOYEERECYCLE_POINT(pEmpData->quality_) * employee->soul_;
		point += CALC_EMPLOYEERECYCLE_POINT(employee->color_);
		setProp(PT_EmployeeCurrency,getProp(PT_EmployeeCurrency) + point);
	}

	U32 needAll = 0;
	for (size_t i = 0; i < employee->skills_.size();++i)
	{
		if(employee->skills_[i]->skLevel_ <= 1)
			continue;
		for (size_t j = 1; j <= employee->skills_[i]->skLevel_; ++j)
		{
			U32 need = EmployeeTable::getSkillLevelUpNeed(employee->getProp(PT_TableId),j);
			if(need == 0)
				continue;
			needAll += need;
		}
	}

	U32 cur = getProp(PT_EmployeeCurrency) + U32(needAll*0.8);
	setProp(PT_EmployeeCurrency,cur);

	U32 feedback = 0;
	switch (employee->star_)
	{
	case 1:
		feedback = Global::get<int>(C_EmpDelFeedback1);
		break;
	case 2:
		feedback = Global::get<int>(C_EmpDelFeedback2);
		break;
	case 3:
		feedback = Global::get<int>(C_EmpDelFeedback3);
		break;
	case 4:
		feedback = Global::get<int>(C_EmpDelFeedback4);
		break;
	case 5:
		feedback = Global::get<int>(C_EmpDelFeedback5);
		break;
	default:
		break;
	}
	addMoney(feedback);
	return true;
}