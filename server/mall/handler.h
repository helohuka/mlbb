
#ifndef __HANDLER_H__
#define __HANDLER_H__


#include "config.h"

class WorldHandler
	: public BINConnection< SGE_Mall2WorldStub , SGE_World2MallProxy >
	, SGE_World2MallProxy
{
public:
	WorldHandler(){setProxy(this);}
public:
#include "SGE_World2MallMethods.h"
};

#endif