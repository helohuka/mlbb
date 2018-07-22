
#ifndef __SQL_H__
#define __SQL_H__

#include "config.h"
#include "server.h"
#include "routine.h"
#include "sqltask.h"

class LogAccount : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_ACCOUNT;}
	SGE_Account data_;
};

///登入登出
class LogLogin : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_LOGOUT;}
	SGE_Login data_;
};

class LogOrder : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_ORDER;}
	SGE_Order data_;
};


class LogUIBehavior : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_UIBEHAVIOR;}
	SGE_LogUIBehavior core_;
};

class LogPlayerSay : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_PLAYERSAY;}
	COM_Chat core_;
	uint32 playerId_;
	std::string playerName_;
};

class LogPlayerTrack : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_PLAYERTRACK;}
	SGE_LogProduceTrack logdata_;
};

class LogRole : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_LOGSER_ROLE;}
	SGE_LogRole logdata_;
};


#endif