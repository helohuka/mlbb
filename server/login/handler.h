
#ifndef __HANDLER_H__
#define __HANDLER_H__


#include "config.h"

class WorldHandler
	: public BINConnection< SGE_Login2WorldStub , SGE_World2LoginProxy >
	, SGE_World2LoginProxy
{
public:
	SINGLETON_FUNCTION(WorldHandler)
	WorldHandler(){setProxy(this);}
public:
#include "SGE_World2LoginMethods.h"
};

#endif