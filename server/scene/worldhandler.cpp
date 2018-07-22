#include "config.h"
#include "worldhandler.h"
#include "sceneplayer.h"
#include "scene.h"

int 
WorldHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("World serv closed...!!!\n")));
	Connection::handle_close(handle,close_mask);
	SRV_ASSERT(0);
	return -1; //不再监听尼玛close事件
}

Channel*
WorldHandler::accept(){
	return new ScenePlayer();
}
bool WorldHandler::initDynamicNpcs(NpcType type, int32 count){
	SceneManager::instance()->InitDynamicNpcs(type,count);
	return true;
}
bool WorldHandler::refreshDynamicNpcs(NpcType type, int32 count){
	SceneManager::instance()->RefreshDynamicNpcs(type,count);
	return true;
}
bool WorldHandler::finiDynamicNpcs(NpcType type){
	SceneManager::instance()->FiniDynamicNpcs(type);
	return true;
}

bool WorldHandler::addDynamicNpcs(int32 sceneId, std::vector<int32> &npcs){
	Scene* s = SceneManager::instance()->FindScene(sceneId);
	if(s)
		s->AddDynamicNpcs(npcs);
	return true;
}

bool WorldHandler::delDynamicNpc(int32 npcId){
	SceneManager::instance()->DelDynamicNpc(npcId);
	return true;
}
bool WorldHandler::delDynamicNpc2( int32 sceneId, int32 npcId){
	Scene* s = SceneManager::instance()->FindScene(sceneId);
	if(s)
		s->DelDynamicNpc(npcId);
	return true;
}


bool WorldHandler::openSceneCopy(int32 instId){
	if(!SceneManager::instance()->OpenSceneCopy(instId)){
		ACE_DEBUG((LM_ERROR, "Can not open scene copy %d %d \n",GET_SCENE_COPY_ID(instId),GET_SCENE_ORIGINAL_ID(instId)));
	}
	return true;
}

bool WorldHandler::closeSceneCopy(int32 instId){
	if(!SceneManager::instance()->CloseSceneCopy(instId)){
		ACE_DEBUG((LM_ERROR, "Can not close scene copy %d %d \n",GET_SCENE_COPY_ID(instId),GET_SCENE_ORIGINAL_ID(instId)));
	}
	return true;
}