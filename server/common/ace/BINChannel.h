#ifndef __BINChannel_h__
#define __BINChannel_h__
#include "Channel.h"
#include "ProtocolMemReader.h"
#include "ProtocolWriter.h"



/** 基于bin协议的网络通讯 Channel.

	BINChannel 将一个 BIN_STUB，一个 BIN_PROXY 与一个 Channel 通信管道
	整合到一起，实现了使用这个 Channel 对 BIN借口调用的发送和接收工作。
	BINChannel 将接收到的数据通过 BIN_PROXY 进行消息分派，调用 BIN_PROXY 
	中对应的 BIN 接口函数。使用者可以通过重载 BIN_STUB 中的接口对消息进行处理。
	使用者还可以通过调用这个 BINChannel 的 BIN_STUB 接口发送 BIN 消息。

	@see BINConnection
*/
template< class BIN_STUB, class BIN_PROXY >
class BINChannel :
	public Channel,
	public BIN_STUB,
	public ProtocolWriter /* protocol writer for BIN_STUB */
{
public:
	/** 从这个channel接收到的数据会交给 p 进行分派处理. */
	BINChannel():
	proxy_(NULL)
	{
	}

	/** 设置proxy指针. */
	void setProxy(BIN_PROXY* p)		{ 
		proxy_ = p; }

	/** 处理channel接收到的数据.
		交给BIN proxy进行dispatch.
	*/
	virtual bool handleReceived(void* data, size_t size)
	{
		if(!proxy_)
			return false;

		// 将接收到的消息通过 BIN_PROXY::dispatch 进行分派，调用对应的接口。
		//ACE_DEBUG((LM_ERROR, "Channel(%d) size %d \n",guid_,size));
		ProtocolMemReader r(data, size);
		if(!proxy_->dispatch(&r))
		{
			ACE_DEBUG((LM_ERROR, "Channel %d %d %d dispatch error \n",guid_,size,*(U16*)data));
			return false;
		}
		return true;
	}

	/** Stub events. */
	virtual ProtocolWriter* methodBegin()
	{
		fillBegin();
		return this;
	}
	virtual void methodEnd()
	{
		fillEnd();
	}

	/** BIN_ProtocolWriter interface. */
	virtual void write(const void* data, size_t len)
	{
		fill((void*)data, len);
	}

protected:
	BIN_PROXY*		proxy_;
};

#endif//__BINChannel_h__
