#ifndef __GOWEB_H__
#define __GOWEB_H__

#include "config.h"



struct AnyOauth : public SDKInfo {
	AnyOauth():timeout_(0.F){}
	float timeout_;
};

class LoginOauth
	:public Connection
{
public:
	LoginOauth();
	~LoginOauth();
public:

	int open(void * /* = 0 */);

	int handleReceived(void* data, size_t size);

	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
	
	
public:
	static AnyOauth* getOauthByUsername(std::string & username);
	static AnyOauth* getOauthByUsername2(std::string & username);
	static void removeOauthByUsername(std::string const & username);
	static void timeOutOauths(float dt);
	static std::vector<AnyOauth*> infos_;
private:
	/** ½âÎöpost param*/
	void proc(const char* post);
};

namespace Json {
	class Value;
}

class PayNotify
	:public Connection
{
public:
	PayNotify();
	~PayNotify();
public:

	int open(void * /* = 0 */);

	int handleReceived(void* data, size_t size);

	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
private:
	/** ½âÎöpost param*/
	void proc(const char* post);

	void pay(Json::Value &v);

	void pay2(Json::Value &v);
};



#endif