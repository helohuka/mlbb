
#ifndef __HANDLER_H__
#define __HANDLER_H__


#include "config.h"
class LogStubDummy{};
class WorldHandler
	: public BINConnection< LogStubDummy , SGE_LogProxy>
	, SGE_LogProxy
{
public:
	SINGLETON_FUNCTION(WorldHandler);
	WorldHandler(){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
#include "SGE_LogMethods.h"
};

#endif