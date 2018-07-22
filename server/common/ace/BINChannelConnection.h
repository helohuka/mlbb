#ifndef __BINChannelConnection_h__
#define __BINChannelConnection_h__

#include "predef.h"
#include "ProtocolMemReader.h"
#include "ProtocolWriter.h"

#include "ChannelConnection.h"

/** 基于BIN协议的 ChannelConnection. 
*/
template<
	class CHANNEL,
	class BIN_STUB, 
	class BIN_PROXY 
>
class BINChannelConnection :
	public ChannelConnection,
	public BIN_STUB,
	public ProtocolWriter /* protocol writer for BIN_STUB */
{
public:
	BINChannelConnection(bool isConnector, size_t rbSize = 0XFFFF, size_t sbSize = 0XFFFF):
	ChannelConnection(isConnector, rbSize, sbSize),
	proxy_(NULL)
	{}

	/** 设置proxy指针. */
	void setProxy(BIN_PROXY* p)		{ proxy_ = p; }

	/** 获得这个连接管理的所有 Channel 对象列表. */
	void getAllChannels(ChannelConnection::NodeContainerType& channels)
	{
		ChannelConnection::getAllChannels(channels);
	}

	/** 连接一个channel. 
		将连接初始化数据通过BIN struct整编.
		@param c 需要建立的channel对象.
		@param initData 连接初始化数据.
	*/
	void connectChannel(CHANNEL* c)
	{
		connect(c);
	}

	/** 用来初始化被动 CHANNEL 的对象.
		派生类需要重载这个函数对channel进行初始化.
		@param initData channel初始化数据.
	*/
	virtual Channel* accept() { return new CHANNEL();}

	virtual bool handleGlobalData(void* data, size_t size)
	{
		if(!proxy_)
			return size;

		// 将接收到的消息通过 BIN_PROXY::dispatch 进行分派，调用对应的接口。
		ProtocolMemReader r(data, size);
		return proxy_->dispatch(&r);
	}

	/** Stub events. */
	virtual ProtocolWriter* methodBegin()
	{
		initGlobalSendingData();
		return this;
	}
	virtual void methodEnd()
	{
		flushSendingData();
	}
	/** BIN_ProtocolWriter interface. */
	virtual void write(const void* data, size_t len)
	{
		fillSendingData((void*)data, len);
	}

private:
	BIN_PROXY*		proxy_;
};

#endif//__BINChannelConnection_h__
