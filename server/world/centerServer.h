#ifndef __CENTERSERVER_H__
#define __CENTERSERVER_H__
#include "config.h"
#include "curl/curl.h"
class SyncCentreServerTask : public ACE_Task<ACE_MT_SYNCH>{
public:	
	SyncCentreServerTask();
	~SyncCentreServerTask();
	virtual int init();
	virtual int fini();
	virtual int svc(void);

	void pushAdded(std::vector<SGE_ContactInfoExt> infos);
	void pushAdded(SGE_ContactInfoExt info);
	void pushDeled(SGE_ContactInfoExt info);
protected:
	void syncCenterServer();
	int curlPost();
	static size_t curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p) ;
private:
	bool runing_;
	CURL *curl_;			
	ACE_Recursive_Thread_Mutex			syncMutex_;
	std::vector<SGE_ContactInfoExt>		syncAdd_;
	std::vector<SGE_ContactInfoExt>		syncDel_;
	std::queue<std::string>				postQueue_;
	time_t								lastPostTime_;
	int32								serverIdIndex_;
};

#endif