/** File generate by <hotlala8088@gmail.com> 2015/01/14  
 */

#ifndef __WORLD_HANDLER_H__
#define __WORLD_HANDLER_H__

#include "config.h"
#include "proto.h"
#include "clientHandler.h"
#include "clientChannel.h"
struct __ProxyDummy{

	bool dispatch(ProtocolReader* reader){return true;}
};
class WorldHandler
	: public BINChannelConnection<ClientChannel,SGE_Gateway2WorldStub,__ProxyDummy>
{
	enum {
		ChannelPoolSize = 4000
	};
public:
	typedef std::vector<ClientChannel*> ChannelList;
public:
	SINGLETON_FUNCTION(WorldHandler);
public:
#ifdef _WIN32
	WorldHandler():BINChannelConnection<ClientChannel,SGE_Gateway2WorldStub,__ProxyDummy>(true,0XFFFFFF,0XFFFFFF){}
#else 
	WorldHandler():BINChannelConnection<ClientChannel,SGE_Gateway2WorldStub,__ProxyDummy>(true,0XFFFFFFF,0XFFFFFFF){}
#endif
	
	/*Channel* makeChannel() { return new ClientChannel(); }*/
public:
	int open(void * /* = 0 */);

	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
	
	ClientChannel* getChannel(uint32 id){
		if(id>= channelPool_.size())
			return NULL;
		return channelPool_[id];
	}
	ClientChannel* connectChannel();
private:
	ChannelList channelPool_;
};



#endif