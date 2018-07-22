#ifndef __WORLD___HANDLER_H__
#define __WORLD___HANDLER_H__
#include "config.h"
#include "sceneplayer.h"
class WorldHandler	
	: public BINChannelConnection<ScenePlayer,SGE_Scene2WorldStub,SGE_World2SceneProxy>
	, SGE_World2SceneProxy
{
public:
	SINGLETON_FUNCTION(WorldHandler);
public:
#include "SGE_World2SceneMethods.h"
public:
	WorldHandler():BINChannelConnection<ScenePlayer,SGE_Scene2WorldStub,SGE_World2SceneProxy>(false,0XFFFFFF,0XFFFFFF){ setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
	Channel *accept();
public:
};

#endif