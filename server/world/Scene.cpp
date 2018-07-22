#include "config.h"
#include "Scene.h"
#include "scenetable.h"
#include "sceneplayer.h"
#include "scenehandler.h"
#include "player.h"
#include "npctable.h"
void Scene::addPlayer(Player* p){
	if(idStore_[p->getGUID()] != NULL)
		return;
	idStore_[p->getGUID()] = p;
	store_.push_back(p);
	if(p->getClient())
		addChannel(p->getClient());
}

void Scene::delPlayer(Player* p){
	if(idStore_[p->getGUID()] == NULL)
		return;
	idStore_[p->getGUID()] = NULL;
	if(p->getClient())
		removeChannel(p->getClient());
	PlayerList::iterator i = std::find(store_.begin(),store_.end(),p);
	if(i!=store_.end())
		store_.erase(i);
	
	if((sceneType_ != SCT_GuildBattleScene) && (sceneType_ == SCT_Instance) && store_.empty())
		SceneManager::instance()->closeSceneCopy(sceneId_);
}

Player*
Scene::getPlayerById(U32 playerId){
	return idStore_[playerId];
}

void Scene::allTransforMainScene(){
	for(size_t i=0; i<store_.size(); ++i){
		store_[i]->scenePlayer_->transforScene(SceneTable::getHomeScene()->sceneId_);
	}
}

void Scene::kickPlayer(S64 playerId){
	for (size_t i = 0; i < store_.size(); ++i){
		if(store_[i]->getGUID() != playerId)
			continue;
		store_[i]->scenePlayer_->transforScene(SceneTable::getHomeScene()->sceneId_);
		delPlayer(store_[i]);
	}
}

void Scene::addNpcs(std::vector< S32 >& npcs){
	for(size_t i=0; i<npcs.size(); ++i)
	{
		if (NpcTable::getNpcById(npcs[i]) == NULL){
			ACE_DEBUG((LM_ERROR,"Can not find npc %d\n",npcs[i]));
			npcs.erase(npcs.begin() + i--);
		}
		else if(npcs_.end()!=std::find(npcs_.begin(),npcs_.end(),npcs[i])){
			npcs.erase(npcs.begin() + i--);
		}
	}
	if(!npcs.empty())
	{
		npcs_.insert(npcs_.end(),npcs.begin(),npcs.end());
		SceneHandler::instance()->addDynamicNpcs(sceneId_,npcs);
	}
}

void Scene::delNpc(int32 npcId){
	std::vector< S32 >::iterator i = std::find(npcs_.begin(),npcs_.end(),npcId);
	if(i!=npcs_.end()){
		SceneHandler::instance()->delDynamicNpc2(sceneId_,npcId);
		npcs_.erase(i);
	}

	i = std::find(battleNpcs_.begin(),battleNpcs_.end(),npcId);
	if(i!=battleNpcs_.end())
		battleNpcs_.erase(i);
}

bool Scene::addBattleNpc(int32 npcId){
	std::vector< S32 >::iterator i = std::find(npcs_.begin(),npcs_.end(),npcId);
	if(i==npcs_.end()){
		ACE_DEBUG((LM_ERROR,"Push battle npc can not find npc %d %d \n",sceneId_,npcId));
		return false;
	}

	i = std::find(battleNpcs_.begin(),battleNpcs_.end(),npcId);
	if(i!=battleNpcs_.end()){
		ACE_DEBUG((LM_ERROR,"Push battle npc is inbattle %d %d \n",sceneId_,npcId));
		return false;
	}

	battleNpcs_.push_back(npcId);
	return true;
}

void Scene::delBattleNpc(int32 npcId){
	std::vector< S32 >::iterator i = std::find(battleNpcs_.begin(),battleNpcs_.end(),npcId);
	if(i!=battleNpcs_.end())
		battleNpcs_.erase(i);
}


void SceneManager::init(){
	for(SceneTable::SceneMap::iterator itr = SceneTable::scenes_.begin(); itr != SceneTable::scenes_.end(); ++itr){
		Scene* p = NEW_MEM(Scene,itr->second->sceneId_,itr->second->sceneType_);
		scenes_[p->sceneId_] = p;
	}
}

void SceneManager::fini(){
	for(std::map<S32,Scene*>::iterator i=scenes_.begin(); i!=scenes_.end(); ++i){
		if(i->second)
			DEL_MEM(i->second);
	}
	scenes_.clear();
}

void SceneManager::tick(){}

Scene* SceneManager::getScene(S32 sceneId){
	if(scenes_.find(sceneId) == scenes_.end()){
		return NULL;
	}
	return scenes_[sceneId];
}

Scene* SceneManager::openSceneCopy(int32 sceneId){
	Scene* s = getScene(sceneId);
	if(!s /*|| (s->sceneType_ != SCT_Instance)*/)
		return NULL;
	s = s->copy();
	if(freedCopyId_.empty()){
		static int32 copyId = 0;
		if(copyId >= 0XFFFF)
		{
			ACE_DEBUG((LM_ERROR,"Max copy id\n"));
			if(s)
				DEL_MEM(s);
			return NULL;
		}
		sceneId = COMBINE_SCENE_COPY_ID(++copyId,sceneId);
	}
	else{
		sceneId = COMBINE_SCENE_COPY_ID(freedCopyId_.back(),sceneId);
		freedCopyId_.pop_back();
	}
	SceneHandler::instance()->openSceneCopy(sceneId);
	s->sceneId_ = sceneId;
	scenes_[sceneId] = s;
	copystore_.push_back(sceneId);
	return s;
}

bool SceneManager::closeSceneCopy(int32 instId){
	if(scenes_[instId]){
		int32 copyId = GET_SCENE_COPY_ID(instId);
		if(copyId){
			scenes_[instId]->allTransforMainScene();
			DEL_MEM(scenes_[instId]);
			scenes_[instId] = NULL;
			freedCopyId_.push_back(copyId);
			
			for (size_t i = 0; i<copystore_.size(); ++i)
			{
				if(copystore_[i] == instId)
					copystore_.erase(copystore_.begin()+i);
			}
			SceneHandler::instance()->closeSceneCopy(instId);
			return true;
		}
	}
	return false;
}

