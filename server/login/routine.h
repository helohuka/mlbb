
#ifndef __SQL_H__
#define __SQL_H__

#include "config.h"
#include "server.h"
#include "routine.h"
#include "sqltask.h"

U32 GetInstanceMaxId(SQLTask *pTask);
///初始化

class InitSQLTables : public Routine
{
public:
	U32 go(SQLTask *pTask);
};

///查询账户
class QueryAccount : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACCOUNT1;}
	std::string     username_;
	bool			hasAccount_;
	bool isSeal_;
	COM_AccountInfo account_;

};

///插入账户
class InsertAccount : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACCOUNT0;}
	COM_AccountInfo account_;
};

///冻结账户
class SealAccount : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACCOUNT1;}
	std::string accname_;
	bool isSeal_;
};

class SetPhoneNumber : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACCOUNT1;}
	std::string accname_;
	std::string phoneNumber_;
};

#endif