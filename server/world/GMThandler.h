
#ifndef __GMT_HANDLER_H__
#define __GMT_HANDLER_H__


class GMTHandler 
	: public BINConnection<SGE_World2GMTStub, SGE_GMT2WorldProxy >
	, SGE_GMT2WorldProxy
{
public:
	SINGLETON_FUNCTION(GMTHandler);
public:	
	GMTHandler():isConnect_(false){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
#include "SGE_GMT2WorldMethods.h"
public:
	bool isConnect_;
};

#endif