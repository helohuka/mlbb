
#ifndef __SQL_H__
#define __SQL_H__

#include "config.h"
#include "server.h"
#include "routine.h"
#include "sqltask.h"

///初始化

class InitSQLTables : public Routine{
public:
	U32 go(SQLTask *pTask);
};

class FetchSell : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	S32 playerId_;
	COM_SearchContext context_;
	S32 total_;
	std::vector<COM_SellItem> items_;
};

class FetchMySell : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	S32 playerId_;
	std::vector<COM_SellItem> items_;
};


class Sell : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	S32 playerId_;
	COM_SellItem item_;
};

class Unsell : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	S32 playerId_;
	S32 guid_;
	COM_SellItem item_;
};

class Buy : public Routine{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	SGE_BuyContent bc_;
	ErrorNo error_;
	COM_SellItem item_;
};

class InsertSelledItem : public Routine {
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MALL;}
	COM_SelledItem item_;
};

class FetachSelledItem : public Routine {
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_MALL;}
	S32 playerId_;
	std::vector<COM_SelledItem> items_;
};

#endif