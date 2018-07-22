
#ifndef __HANDLER_H__
#define __HANDLER_H__


#include "config.h"


class WorldHandler
	: public BINConnection< SGE_DB2WorldStub , SGE_World2DBProxy >
	, SGE_World2DBProxy
{
public:
	SINGLETON_FUNCTION(WorldHandler)
	WorldHandler(){setProxy(this);}
public:
#include "SGE_World2DBMethods.h"
};


#endif