#ifndef __ChannelBroadcaster_h__
#define __ChannelBroadcaster_h__

#include "ChannelConnection.h"
#include <map>

/** 针对一组 CHANNEL 发送广播消息的管理器.
	
	ChannelBroadcaster 管理一个 CHANNEL 的集合，如果通过 ChannelBroadcaster 
	进行消息的发送，这个消息会广播给这个集合中所有的 CHANNEL 对象。

	ChannelBroadcaster 建立在 ChannelConnection 的数据广播接口上。
*/
template<class CHANNEL>
class ChannelBroadcaster 
{
public:
	ChannelBroadcaster(ChannelConnection * conn)
		:conn_(conn)
	{
		SRV_ASSERT(conn);
	}

	virtual ~ChannelBroadcaster()
	{
	}

	/** 添加一个channel对象.
		@warning 这个channel对象必须有效，并且在被ChannelBroadcaster管理过程中
				 不能被关闭.
	*/
	void addChannel(CHANNEL* ch)
	{
		SRV_ASSERT(ch);
		if(!ch->isValid())
		{
			ACE_DEBUG((LM_ERROR,"if(!ch->isValid())\n"));
			return ;
		}
		if(conn_ != ch->getConn()){
			ACE_DEBUG((LM_ERROR,"if(conn_ != ch->getConn())\n"));
			return ;
		}
		//ACE_DEBUG((LM_ERROR,"Insert channel %d\n",ch->getGuid()));
		channels_.insert(ch);
	}

	/** 移除一个channel对象. */
	void removeChannel(CHANNEL* ch)
	{
		SRV_ASSERT(this);
		if(!ch){
			ACE_DEBUG((LM_ERROR,"if(!ch)\n"));
			return ;
		}
		if((conn_ != ch->getConn()))
		{
			ACE_DEBUG((LM_ERROR,"Remove channel %d if((conn_ != ch->getConn()))\n",ch->getGuid()));
		}
		//ACE_DEBUG((LM_ERROR,"Remove channel %d\n",ch->getGuid()));
		if(channels_.find(ch) != channels_.end())
			channels_.erase(ch);
	}

	/** 清除所有的channel对象. */
	void clearChannels()
	{
		channels_.clear();
	}

	/** 准备广播数据. 
		此操作为集合中的每个connection准备好channel集合，等待接下来调用 fillSendingData 
		填充广播数据.
	*/
	void initSendingData()
	{
		conn_->initBCSendingData(channels_);
	}

	/** 填充广播数据. 
		此操作为集合中的每个connection填充好待广播的数据，等待接下来的 flushSendingData
		发送广播数据.
	*/
	void fillSendingData(void* data, size_t size)
	{
		conn_->fillSendingData(data,size);
	}

	/** 发送广播数据. 
		此操作为集合中的每个connection做最终的数据发送，广播结束.
	*/
	void flushSendingData()
	{
		conn_->flushSendingData();
	}
	typedef std::set<Channel*> ChannelSet;
	ChannelSet channels_;
	ChannelConnection* conn_;
};

#endif//__ChannelBroadcaster_h__
