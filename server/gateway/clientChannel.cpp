#include "config.h"
#include "clientHandler.h"
#include "clientChannel.h"
#include "worldhandler.h"
#include "gwserv.h"
bool 
ClientChannel::handleReceived( void* data, size_t size )
{
	ClientHandler* handler = GatewayServ::instance()->getClientHandler(handleId_);
	if(NULL == handler)
		return true;
	
	handler->fill(&size,sizeof(U16));
	handler->fill(data,size);
	handler->flush();

	GatewayServ::instance()->totalOutgoing_ += (size + sizeof(U16));
	return true; 
}

bool 
ClientChannel::handleClose()
{
	ACE_DEBUG((LM_DEBUG,"Client channel handle close %d | %d\n", channelId_ ,getGuid()));

	ClientHandler* handler = GatewayServ::instance()->getClientHandler(handleId_);
	if(handler != NULL)
	{
		handler->dettachChannel();
		handler->close();
	}
	handleId_ = -1;
	
	return true;
}

void ClientChannel::close(){

	ACE_DEBUG((LM_DEBUG,"Client channel close %d | %d\n", channelId_ ,getGuid()));
	ClientHandler* handler = GatewayServ::instance()->getClientHandler(handleId_);
	if(handler != NULL)
	{
		handler->dettachChannel();
		handler->close();
	}
	handleId_ = -1;
	WorldHandler::instance()->disconnect(this);
	
}
