#ifndef __HANDLER_H__
#define __HANDLER_H__
#include "config.h"
#include "json/json.h"



class ClientHandler
	:public Connection
{
public:
	ClientHandler();
	~ClientHandler();
public:

	int open(void * /* = 0 */);

	int handleReceived(void* data, size_t size);

	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
private:
	/** 解析post param*/
	void procPostData(const char* post);
	void gmCommandModule(Json::Value& json);
	void gmNoticeModule(Json::Value& json);
	void gmInsertMailModule(Json::Value& json);
	void gmCurrencyModule(Json::Value& json);
	void gmAddExpModule(Json::Value& json);
	void gmQueryRoleModule(Json::Value& json);
	void gmLoginActivity(Json::Value& json);
	void gmChargeTotal(Json::Value& json);
	void gmChargeEvery(Json::Value& json);
	void gmDiscountStore(Json::Value& json);
	void gmHotRole(Json::Value& json);
	void gmEmployeeActivity(Json::Value& json);
	void gmMinGiftbag(Json::Value& json);
	void gmDoScript(Json::Value& json);
	void gmMakeOrder(Json::Value& json);
	void gmQueryRMB();
	void gmQueryDia();
	void gmQueryMoney();
	void gmZhuanpan(Json::Value& json);
	void gmIntegralshop(Json::Value& json);
	void gmQueryRoleList(Json::Value& json);
	void result(int errorno, const char* errordesc);	
	void resultgzip(int errorno, const char* errordesc);	
};


class WorldHandler
	: public BINConnection< SGE_GMT2WorldStub , SGE_World2GMTProxy >
	, SGE_World2GMTProxy
{
public:
	SINGLETON_FUNCTION(WorldHandler);
	WorldHandler(){setProxy(this);}
public:
#include "SGE_World2GMTMethods.h"
};



#endif