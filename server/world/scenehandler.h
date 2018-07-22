#ifndef ___SCENE_HANDLER_H__
#define ___SCENE_HANDLER_H__
#include "sceneplayer.h"
class SceneHandler 
	: public BINChannelConnection<ScenePlayer,SGE_World2SceneStub,SGE_Scene2WorldProxy>
	, SGE_Scene2WorldProxy
{
public:
#include "SGE_Scene2WorldMethods.h"
public:
	SceneHandler():BINChannelConnection<ScenePlayer,SGE_World2SceneStub,SGE_Scene2WorldProxy>(true,0XFFFFFF,0XFFFFFF),isConnect_(false){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
	SINGLETON_FUNCTION(SceneHandler)

	bool isConnect_ ;
};

#endif