#include "config.h"
#include "scenehandler.h"

int 
SceneHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Scene serv closed...!!!\n")));
	isConnect_ = false;
	Connection::handle_close(handle,close_mask);
	return -1; //不再监听尼玛close事件
}

