
#include "handler.h"
#include "config.h"
#include "routine.h"

#define CREATE_ROUTINE(P,TYPE) TYPE* P = NULL; Routine::create(P); SRV_ASSERT(P);

int 
WorldHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	SRV_ASSERT(0);
	return Connection::handle_close(handle,close_mask);
}

bool WorldHandler::account(SGE_Account& data){
	CREATE_ROUTINE(p,LogAccount);
	p->data_ = data;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::login(SGE_Login& data){
	CREATE_ROUTINE(p,LogLogin);
	p->data_ = data;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::order(SGE_Order& data){
	CREATE_ROUTINE(p,LogOrder);
	p->data_ = data;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::playerUIBehavior(SGE_LogUIBehavior& core){
	CREATE_ROUTINE(p,LogUIBehavior);
	p->core_ = core;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::playersay(U32 senderId, std::string& senderName, COM_Chat& chat){
	CREATE_ROUTINE(p,LogPlayerSay);
	p->playerId_ = senderId;
	p->playerName_ = senderName;
	p->core_ = chat;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::playerTrack(SGE_LogProduceTrack& data){
	CREATE_ROUTINE(p,LogPlayerTrack);
	p->logdata_ = data;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::role(SGE_LogRole& data){
	CREATE_ROUTINE(p,LogRole);
	p->logdata_ = data;
	SQLTask::spost(p);
	return true;
}