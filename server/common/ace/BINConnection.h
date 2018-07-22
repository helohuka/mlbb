#ifndef __BINConnection_h__
#define __BINConnection_h__


#include "ProtocolMemReader.h"
#include "ProtocolWriter.h"

#include "Connection.h"

/** 基于BIN协议的网络通讯连接.
	BINConnection 基于aprc协议进行连接双方的通讯.
	BINConnection 派生自 一个指定的 BIN stub，所以可以直接调用BIN stub 的
	接口向对端发送数据.
	BINConnection 还保存一个 BIN proxy 的引用，当接收到对方数据后，会调用这
	个 proxy 的接口来 处理数据.

	BIN 协议格式:
	- HdrType: 数据长度.
	- BIN数据
*/
template< class BIN_STUB, class BIN_PROXY , class HdrType = unsigned int>
class BINConnection : 
	public ProtocolWriter,
	public Connection,
	public BIN_STUB
{
public:
	BINConnection(size_t rbSize = 0XFFFFFF, size_t sbSize = 0XFFFFFF):
	Connection(rbSize, sbSize),
	proxy_(NULL),
	hdr_(NULL)
	{
	}

	/** 设置proxy指针. */
	void setProxy(BIN_PROXY* p)		{ proxy_ = p; }

	/** 处理连接接收到的数据.
		handleReceived 将接收到的数据交给BIN proxy进行dispatch，调用相应的函数.
	*/
	virtual int handleReceived(void* data, size_t size)
	{
		if(!proxy_)
			return size;

		// 解析发送过来的协议，并进行BIN dispatch，调用相应的处理函数。 
		int processed = 0;

		// 循环处理，直到不能处理为止.
		unsigned char* d = (unsigned char*)data;
		size_t dLeft = size;
		while(1)
		{
			// 检测数据头长度.
			if(sizeof(HdrType) > dLeft)
				return processed;
			HdrType dLen = *((HdrType*)d);

			// 检查数据完整性.
			size_t dTLen = sizeof(HdrType) + dLen;
			if(dTLen > dLeft)
				return processed;

			// 一个完整消息被接受 交给BIN dispatcher处理.
			ProtocolMemReader r((d+sizeof(HdrType)), dLen);
			if(!proxy_->dispatch(&r))
			{
				ACE_DEBUG((LM_ERROR,"Connection(%s) dispath error\n",remoteAddr_.get_host_addr()));
				return -1;
			}

			d			+= dTLen;
			dLeft		-= dTLen;
			processed	+= dTLen;
		}
		return processed;
	}

	/** Stub events. */
	virtual ProtocolWriter* methodBegin()
	{
		if(getStatus() != Connection::Established)
			return NULL;

		// 保存头指针位置.
		hdr_ = sendBuf_.wr_ptr();
		// 填入一个临时的头部数据.
		HdrType tempHdr = 0;
		fill(&tempHdr, sizeof(HdrType));
		return this;
	}
	virtual void methodEnd()
	{
		if(getStatus() != Connection::Established)
			return;
		// 重置消息头
		*((HdrType*)hdr_) = (HdrType)((sendBuf_.wr_ptr() - hdr_) - sizeof(HdrType));
		// 发送aprc数据.
		flush();
	}
	/** BIN_ProtocolWriter interface. */
	virtual void write(const void* data, size_t len)
	{
		if(getStatus() != Connection::Established)
			return;
		fill((void*)data, len);
	}

private:
	BIN_PROXY*			proxy_;
	char*				hdr_;
};

#endif//__BINConnection_h__
