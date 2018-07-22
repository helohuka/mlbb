
#ifndef __SERVER_H__
#define __SERVER_H__
#include "config.h"
#include "sqltask.h"
#include "handler.h"

class Server 
	: public ACE_Service_Object
{
public:
	SINGLETON_FUNCTION(Server)
public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int handle_timeout (const ACE_Time_Value &current_time, const void *act);
	void initData();
	void initRank();

	void addBabyInst(COM_BabyInst &inst);
	void delBabyInst(U32 instId);
	void updateBabyInst(COM_BabyInst &inst);
	bool getBabyInst(U32 instId, COM_BabyInst &inst);
	void getPlayerBabyList(std::string& playerName, std::vector<COM_BabyInst>& out);


	void addEmployeeInst(COM_EmployeeInst &inst);
	void delEmployeeInst(U32 instId);
	void updateEmployeeInst(COM_EmployeeInst &inst);
	bool getEmployeeInst(U32 instId, COM_EmployeeInst &inst);
	void getPlayerEmployeeList(std::string& playerName, std::vector<COM_EmployeeInst>& out);
public:
	U32 maxGuid_;
	std::map<U32,COM_ActivityTable>		tables_;
	std::vector<SGE_ContactInfoExt>		contactCache_;
	std::vector<COM_ContactInfo>		playerFFrankCache_;
	std::vector<COM_ContactInfo>		playerlevelrankCache_;
	std::vector<COM_BabyRankData>		babyFFrankCache_;
	std::vector<COM_EmployeeRankData>	employeeFFrankCache_;
	std::vector<SGE_PlayerEmployeeQuest> employeeQuestCache_;  

	std::map<std::string, std::vector<COM_BabyInst*> > playerBabyCache_;
	std::map<U32,COM_BabyInst*> idBabyCache_;

	std::map<std::string, std::vector<COM_EmployeeInst*> > playerEmployeeCache_;
	std::map<U32, COM_EmployeeInst*> idEmployeeCache_;
};

#endif