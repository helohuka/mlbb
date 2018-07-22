#ifndef __BUDDY_H__
#define __BUDDY_H__

#include "config.h"
#include "entity.h"
#include "employeeTable.h"
#include "struct.h"

class ClientHandler;
class InnerPlayer;
class Player;
class Employee : public Entity
{
public:
	Employee();
	~Employee();
public:
	static void genEmployeeData(InnerPlayer* owner, U32 table,COM_EmployeeInst& out);
public:
	Employee(InnerPlayer* owner);
	Employee* asEmployee(){return this;}
	U32		getGUID(){return instId_;}
	const char* getNameC(){return instName_.c_str();}
public:
	void setEmployeeInst(COM_EmployeeInst &tmp);
	void getEmployeeInst(COM_EmployeeInst &out);
	void getBattleEntityInformation(COM_BattleEntityInformation& info);
	const ItemTable::ItemData* getWearEquipData(EquipmentSlot eSlot);		///获取伙伴身上穿戴装备
	void cleanBattleStatus(bool resetProperty);
	bool isBattleAtkTimeout(){return !battleActive_;}
public:
	void	evolve();			//进化
	void	evolveresetprop();
	void	upStar();			//升星
	void	updateProp(U32 table,COM_EmployeeInst& out);

	bool	canUseItem(U32 itemId);
public:
	void postEvent(AIEvent me,BattlePosition target, std::map<S32,S32> &posTable);
	void levelup();
	void syncProp();
	virtual void skillLevelUp(S32 skillId);
	ClientHandler* getClient();
	U32			instId_;
	std::string instName_;
	InnerPlayer *owner_;
	std::string ownerName_;
	bool		isBattle_;
	
	QualityColor			color_;			//伙伴品质(进阶)
	U32						star_;			//星级(进化)
	U32						soul_;			//武魂
	std::string className_;
	std::vector<std::string> employeeEvents_;

	InnerPlayer*		getOwner()		{ return owner_;}
};

#endif