/** File generate by <hotlala8088@gmail.com> 2015/01/17  
 */

#ifndef __GATEWAY_HANDLER_H__
#define __GATEWAY_HANDLER_H__

#include "config.h"
#include "client.h"
struct __StubDummy{
	
};
class GatewayHandler
	: public BINChannelConnection<Channel,__StubDummy,SGE_Gateway2WorldProxy>
	, SGE_Gateway2WorldProxy
{
public:
	SINGLETON_FUNCTION(GatewayHandler);
#include "SGE_Gateway2WorldMethods.h"
public:
	GatewayHandler();
	~GatewayHandler();
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
public:
	Channel *accept();
	int handleReceived(void* data, size_t size);
	
public:
	bool isConnect_ ;
	int32 inDoor_; //入口 
	std::string channelIp_; //用户IP
	std::vector<ClientHandler*> handles_;
};

#endif