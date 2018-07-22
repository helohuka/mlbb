/** File generate by <hotlala8088@gmail.com> 2015/01/14  
 */

#include "config.h"
#include "worldhandler.h"

ClientChannel* WorldHandler::connectChannel(){
	
	for(size_t i=0; i<channelPool_.size(); ++i){
		if(!channelPool_[i]->isValid()){
			connect(channelPool_[i]);
			return channelPool_[i];
		}
	}
	return NULL;
}

int  WorldHandler::open(void * /* = 0 */){
	
	channelPool_.resize(ChannelPoolSize);
	for(int i=0; i<ChannelPoolSize; ++i){
		channelPool_[i] = new ClientChannel(i);
	}

	return Connection::open();
}


int WorldHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask){
	if(close_mask == ACE_Event_Handler::WRITE_MASK)
	{
		ACE_DEBUG((LM_INFO,"Write mask close \n"));
		return 0;
	}
	for(int i=0; i<ChannelPoolSize; ++i){
		delete channelPool_[i];
	}
	channelPool_.clear();

	ACE_Reactor_Mask mask =	ACE_Event_Handler::READ_MASK | 
		ACE_Event_Handler::WRITE_MASK |
		ACE_Event_Handler::DONT_CALL;
	reactor()->remove_handler(stream_.get_handle(), mask);
	stream_.close();

	SRV_ASSERT(0);
	return 0;
}