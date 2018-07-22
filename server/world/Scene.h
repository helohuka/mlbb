#ifndef __SCENE_H__
#define __SCENE_H__

#include "config.h"
#include "gwhandler.h"

class Player;

class SceneBroadcaster : public BINChannelBroadcaster< Server2ClientStub , ClientHandler>{
public:
	SceneBroadcaster():BINChannelBroadcaster< Server2ClientStub , ClientHandler>(GatewayHandler::instance()){}
};

class Scene : public BINChannelBroadcaster< Server2ClientStub , ClientHandler>{
public:
	Scene(S32 sceneId,SceneType st)
		:BINChannelBroadcaster< Server2ClientStub , ClientHandler>(GatewayHandler::instance())
		,sceneId_(sceneId)
		,sceneType_(st)
	{}
	Scene* copy(){
		return NEW_MEM(Scene,sceneId_,sceneType_);
	}
public:
	void addPlayer(Player* p);
	void delPlayer(Player* p);
	Player* getPlayerById(U32 playerId);
	void allTransforMainScene();
	bool empty(){
		return idStore_.empty();
	}
	
	void kickPlayer(S64 playerId);
	
	void addNpcs(std::vector< S32 >& npcs);
	void delNpc(int32 npcId);
	bool addBattleNpc(int32 npcId);
	void delBattleNpc(int32 npcId);
	int32 sceneId_;
	SceneType sceneType_;
	IdPlayerMap idStore_;
	PlayerList	store_;
	std::vector< S32 > npcs_;
	std::vector< S32 > battleNpcs_;
	
};

class SceneManager{
public:
	SINGLETON_FUNCTION(SceneManager);
public:	
	void init();
	void fini();
	void tick();
	Scene* getScene(int32 sceneId);
	
	Scene* openSceneCopy(int32 sceneId);
	bool closeSceneCopy(int32 instId);

public:
	std::vector<S32> copystore_;
	std::vector<S32> freedCopyId_;
	std::map<S32,Scene*> scenes_;
};

#endif