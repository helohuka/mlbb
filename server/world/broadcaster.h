#ifndef __BROADCASTER_H__
#define __BROADCASTER_H__

#include "config.h"
#include "client.h"
#include "gwhandler.h"
class Broadcaster : public BINChannelBroadcaster< Server2ClientStub , ClientHandler>{
public:
	Broadcaster():BINChannelBroadcaster< Server2ClientStub , ClientHandler>(GatewayHandler::instance()){}
};

class WorldBroadcaster : public Broadcaster{
public:
	SINGLETON_FUNCTION(WorldBroadcaster);
};

#endif