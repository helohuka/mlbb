#ifndef __BINChannelBroadcaster_h__
#define __BINChannelBroadcaster_h__

#include "ProtocolWriter.h"
#include "ChannelBroadcaster.h"


/** 基于BIN协议的 ChannelBroadcaster.

	BINChannelBroadcaster 将 一个 BIN_STUB 和一个 ChannelBroadcaster 
	的功能进行整合，使用者可以通过调用 BIN_STUB 的函数，将这次调用的数据广播给
	集合中所有的 channel。

	@todo 有待优化，当set为空时应该尽量减少计算量.
*/
template<class BIN_STUB, class CHANNEL>
class BINChannelBroadcaster :
	public ChannelBroadcaster<CHANNEL>,
	public BIN_STUB,
	public ProtocolWriter
{
public:
	BINChannelBroadcaster(ChannelConnection* conn) : ChannelBroadcaster<CHANNEL>(conn) {}

private:
	/** Stub events. */
	virtual ProtocolWriter* methodBegin()
	{
		this->initSendingData();
		return this;
	}
	virtual void methodEnd()
	{
		this->flushSendingData();
	}
	/** BIN_ProtocolWriter interface. */
	virtual void write(const void* data, size_t len)
	{
		this->fillSendingData((void*)data, len);
	}
};


#endif//__BINChannelBroadcaster_h__
