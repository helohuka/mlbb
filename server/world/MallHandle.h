
#ifndef __MALL_HANDLER_H__
#define __MALL_HANDLER_H__


class MallHandler 
	: public BINConnection < SGE_World2MallStub , SGE_Mall2WorldProxy >
	, SGE_Mall2WorldProxy
{
public:
	SINGLETON_FUNCTION(MallHandler);
public:	
	MallHandler():isConnect_(false){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
#include "SGE_Mall2WorldMethods.h"
public:
	bool isConnect_;
};


#endif