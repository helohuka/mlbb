
#ifndef __SQL_H__
#define __SQL_H__

#include "config.h"
#include "server.h"
#include "routine.h"

///初始化

class AddMoneyModule : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GMT;}
	std::string		name_;
	U32				moneyValue_;
};

class AddExpModule : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GMT;}
	std::string		name_;
	U32				value_;
};

#endif