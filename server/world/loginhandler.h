
#ifndef __LOGIN_HANDLER_H__
#define __LOGIN_HANDLER_H__


class LoginHandler 
	: public BINConnection < SGE_World2LoginStub , SGE_Login2WorldProxy >
	, SGE_Login2WorldProxy
{
public:
	SINGLETON_FUNCTION(LoginHandler);
public:	
	LoginHandler():isConnect_(false){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
#include "SGE_Login2WorldMethods.h"
public:
	bool isConnect_;
};


#endif