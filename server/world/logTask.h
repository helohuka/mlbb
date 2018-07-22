#ifndef __LOG_TASK_H__
#define __LOG_TASK_H__
#include "config.h"
#include "curl/curl.h"
class Account;
class Player;
class LogTask : public ACE_Task<ACE_MT_SYNCH>{
public:	
	
	void pushAccountLog(Account *acc);
	void pushLoginLog(Player *player);
	void pushLoginLog(Account *acc);
	void pushOrderLog(Account *acc, int32 playerId, int32 playerLevel, std::string const &orderId, int32 payment, std::string const &payTime);
	void pushRoleLog(std::vector<SGE_ContactInfoExt*> &infos);
	
	LogTask();
	~LogTask();
	virtual int init();
	virtual int fini();
	virtual int svc(void);

	void pushAdded(std::vector<SGE_ContactInfoExt> infos);
	void pushAdded(SGE_ContactInfoExt info);
	void pushDeled(SGE_ContactInfoExt info);
protected:
	int curlPost();
	static size_t curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p) ;
private:
	bool runing_;
	CURL *curl_;			
	ACE_Recursive_Thread_Mutex			postMutex_;
	std::queue<std::string>				postQueue_;
};

#endif