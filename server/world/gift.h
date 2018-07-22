#ifndef __GIFT_H__
#define __GIFT_H__
#include "config.h"
#include "curl/curl.h"

struct GiftReq{
	std::string playerName_;
	std::string postValue_;
};

struct GiftResult{
	int32 diamond_; //钻石
	std::string playerName_;
	std::string giftName_;
	std::string err_;
	std::vector<COM_GiftItem> items_;
};

class GiftTask : public ACE_Task<ACE_MT_SYNCH>{
public:	
	GiftTask();
	~GiftTask();
	virtual int init();
	virtual int fini();
	virtual int svc(void);

	void pushAdded(std::string const &playerName, std::string const &cdkey, std::vector<std::string> &giftNames);
	static void updateResult();
protected:
	int curlPost();
	static size_t curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p) ;
private:
	bool runing_;
	CURL *curl_;			
	ACE_Recursive_Thread_Mutex			postMutex_;
	std::vector<GiftReq>				postQueue_;
	
	static ACE_Recursive_Thread_Mutex	resultMutex_;
	static std::vector<GiftResult>			resultQueue_;
};

#endif