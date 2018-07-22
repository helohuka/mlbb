#include "config.h"
#include "broadcaster.h"
//
//void BroadcasterManager::AddToTeamBroadcaster(S32 sceneId,ClientHandler* handler){
//	Broadcaster* p = NULL;
//	if(teamBroadcaster_.find(sceneId) == teamBroadcaster_.end()){
//		ACE_DEBUG((LM_INFO,"Can not find broadcaster in team broadcasters\n"));
//		teamBroadcaster_[sceneId] = new Broadcaster();
//	}
//	p = teamBroadcaster_[sceneId];
//	p->addChannel(handler);
//}
//Broadcaster* BroadcasterManager::GetTeamBroadcaster(S32 sceneId){
//	Broadcaster* p = NULL;
//	if(teamBroadcaster_.find(sceneId) == teamBroadcaster_.end()){
//		ACE_DEBUG((LM_INFO,"Can not find broadcaster in team broadcasters\n"));
//		teamBroadcaster_[sceneId] = new Broadcaster();
//	}
//	p = teamBroadcaster_[sceneId];
//	return p;
//}
//void BroadcasterManager::DelFromTeamBroadcaster(S32 sceneId,ClientHandler* handler){
//	Broadcaster* p = NULL;
//	if(teamBroadcaster_.find(sceneId) == teamBroadcaster_.end()){
//		ACE_DEBUG((LM_INFO,"Can not find broadcaster in team broadcasters\n"));
//		teamBroadcaster_[sceneId] = new Broadcaster();
//	}
//	p = teamBroadcaster_[sceneId];
//	p->removeChannel(handler);
//}
//
//void BroadcasterManager::AddToChatBroadcaster(ClientHandler* handler){
//	chatBroadcaster_.addChannel(handler);
//}
//Broadcaster* BroadcasterManager::GetChatBroadcaster(){
//	return &chatBroadcaster_;
//}
//void BroadcasterManager::DelFromChatBroadcaster(ClientHandler* handler){
//	chatBroadcaster_.removeChannel(handler);
//}