#include "sceneplayer.h"
#include "player.h"
#include "scenetable.h"


//////////////////////////////////////////////////////////////////////////
ScenePlayer::ScenePlayer(Player* p)
:owner_(p){
	setProxy(this);
}


bool ScenePlayer::joinScene(COM_SceneInfo& info){
	//ACE_DEBUG((LM_INFO,"Join scene %d (%f,%f)\n",info.sceneId_,info.position_.x_,info.position_.z_));
	SRV_ASSERT(owner_);
	owner_->joinScene(info);
	return true;
}

bool ScenePlayer::move2(COM_FPosition& pos){
	SRV_ASSERT(owner_);
	owner_->move2(pos);
	return true;
}

bool ScenePlayer::cantMove(){
		CALL_CLIENT(owner_,cantMove());
		return true;
}

bool ScenePlayer::transfor2(COM_FPosition& pos){
	SRV_ASSERT(owner_);
	owner_->transfor2(pos);
	return true;
}

bool ScenePlayer::autoBattleResult(bool isOk){
	SRV_ASSERT(owner_);
	CALL_CLIENT(owner_,autoBattleResult(isOk));
	return true;
}

bool ScenePlayer::zoneJoinBattle(S32 zoneId){
	//ACE_DEBUG((LM_INFO,"Zone join battle %d \n",zoneId));
	SRV_ASSERT(owner_);
	owner_->zoneJoinBattle(zoneId);
	return true;
}

bool ScenePlayer::playerAddNpc(NpcList& npcs){
	//ACE_DEBUG((LM_INFO,"Player added npcs %d \n"));
	SRV_ASSERT(owner_);
	owner_->addNpc(npcs);
	return true;
}
bool ScenePlayer::playerDelNpc(NpcList& npcs){
	//ACE_DEBUG((LM_INFO,"Player del npcs %d \n"));
	SRV_ASSERT(owner_);
	owner_->delNpc(npcs);
	return true;
}

bool ScenePlayer::talkedNpc(S32 npcid){
	SRV_ASSERT(owner_);
	CALL_CLIENT(owner_,talked2Npc(npcid));
	return true;
}

bool ScenePlayer::findDynamicNpcOK(S32 npcid,bool hasnpc)
{
	SRV_ASSERT(owner_);
	if(hasnpc)
		owner_->talkNpc(npcid);
	return true;
}