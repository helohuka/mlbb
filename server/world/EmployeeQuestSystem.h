#ifndef __EMPLOYEE_QUEST_SYSTEM_H__
#define __EMPLOYEE_QUEST_SYSTEM_H__
#include "config.h"

class EmployeeQuestSystem{
	static float CalcSuccessRate(int32 playerId, SGE_EmployeeQuestInst const& questInst);
	static SGE_PlayerEmployeeQuest NewPlayerEmployeeQuest(int32 playerId);
	static void Refresh(EmployeeQuestColor color,std::vector<SGE_EmployeeQuestInst> const & oldQuests, std::vector<SGE_EmployeeQuestInst> &newQuests);
public:
	static std::vector<int32> GetQuestIds(std::vector<SGE_EmployeeQuestInst> const & quests);
public:
	static bool RemoveComplateQuest(int32 playerId, int32 questId);
	static bool HasUsedEmployees(int32 playerId, std::vector<int32> const& employees);
	static bool TryAcceptEmployeeQuest(int32 playerId, int32 questId, std::vector<int32> const& employees, COM_EmployeeQuestInst &out);
	static bool IsEmployeeQuestComplate(int32 playerId, int32 questId);
	static bool IsHasEmployeeQuest(int32 playerId, int32 questId);
	static std::vector<COM_EmployeeQuestInst> GetQuestList(int32 playerId);
	static SGE_PlayerEmployeeQuest *GetSelfEmployeeQuestInst(int32 playerId);
	static SGE_PlayerEmployeeQuest *GetEmployeeQuestInst(int32 playerId);

	static void Insert(SGE_PlayerEmployeeQuest const& quests);
	static void Remove(int32 playerId);
	
	//刷新函数
	static void Refresh(EmployeeQuestColor color);
	static void PeriodRefresh(U32 playerId,SGE_EmployeeQuestInst & curQuest);
	static int64 GetRefreshTime(EmployeeQuestColor color);
	//检测超时函数
	static void Check(EmployeeQuestColor color);
	//时间更新函数 一秒一更
	static void TickRuning(int32 );
	//更新DB数据
	static void initEmployeeQuest(std::vector<SGE_PlayerEmployeeQuest> info);
	static void updatePlayerEmployeeQuest(U32 playerID);

	static PlayerEmployeeQuestTable tables_; 	
	static PlayerEmployeeQuestList	list_;
};

#endif