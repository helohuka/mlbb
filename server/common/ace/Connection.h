#ifndef __SVC_EventHandler_h__
#define __SVC_EventHandler_h__
#include "predef.h"


/** 基于TCP的双向网络连接.

	Connection 将一个 ACE_Event_Handler 与 一个 ACE_SOCK_Stream，以及接收和发
	送数据的buffer(ACE_MessageBlock)整合到一起，实现基本的基于TCP socket连接的网
	络发送和接收数据的功能。

	@par 连接的初始化.
	Connection 被设计为可以与 ACE_Acceptor 和 ACE_Connector 搭配使用.
	可以通过一个 ACE_Acceptor 作为被动连接的工厂类，常见和初始化一个 Connection，
	也可以通过一个 ACE_Connector 主动连接一个 Connection。

	@par 连接状态.
	每个Connection都保存着当前的状态，当连接对象被创建后，默认状态为 Unestablished。
	如果通过 ACE_Acceptor 或 ACE_Connector 有效建立连接后，状态会切换成 Established。
	当使用过程中出现任何错误，连接都会变得无效，并切换成相应的状态。只有在 Established
	状态下，连接才可以有效使用.使用着可以通过 getStatus() 来获得当前的状态，用以
	判断连接当前是否有效.

	@par 数据发送.
	使用者首先因该调用 fillSendBuffer() 想sendbuffer填充数据，然后调用 flushSendBuffer
	将填充好的数使用着据进行一次发送。使用者可以多次调用 fillSendBuffer() 顺序填
	入数据。flushSendBuffer()调用后不能保证数据一定被有效发出去了，如果有没有发出
	的数据，Connection 会想 reactor 注册可写事件，并在可写事件发生时再次进行
	发送.

	@par 数据接收.
	使用着需要派生并重载 handleReceived() 函数，处理接受到的数据，并将处理过的数
	据大小返回给 Connection。 Connection 负责对recvbuffer进行管理.
*/
class Connection : public ACE_Event_Handler
{
public:
	typedef ACE_SOCK_Stream	stream_type;
	typedef ACE_INET_Addr	addr_type;

	/** 构造函数.
		@param rbSize 接收buffer大小.
		@param sbSize 发送buffer大小.
	*/
	Connection(size_t rbSize = 0XFFFF * 2, size_t sbSize = 0XFFFF * 2);

	/** 析构函数. */
	virtual ~Connection();

	/** 连接的当前状态. */
	enum Status
	{
		/** 已通过connect或accept建立有效连接.
			这是唯一表示连接可以使用的状态.
		*/
		Established,
		/** 连接初始状态，还未通过connect或accept建立有效连接. */
		Unestablished,
		/** 接收buffer溢出. */
		RBOverflow,
		/** 发送buffer溢出. */
		SBOverflow,
		/** recv错误. errno中保存了对应的错误号. */
		RecvFailed,
		/** send错误. errno中保存了对应的错误号. */
		SendFailed,
		/** 本地通过close奖连接关闭. */
		LocalClosed,
		/** 远端已经关闭连接. */
		RemoteClosed,
		/** 对接受到的消息处理出现错误. */
		MsgProcFailed,
	};
	/** 获得连接当前的状态. */
	Status getStatus()						{ return status_; }
	/** 获得连接当前状态描述字符串. */
	const char* getStatusDesc();
	/** 获得连接当前的errno. */
	int getErrno()							{ return errno_; }
	/** 获得这个连接的socket stream. (ACE_Acceptor 和 ACE_Connector) */
	ACE_SOCK_Stream& peer()					{ return stream_; }
	/** 获得本地链接地址. */
	const ACE_INET_Addr& getLocalAddr()		{ return localAddr_; }
	/** 获得远端连接地址. */
	const ACE_INET_Addr& getRemoteAddr()	{ return remoteAddr_; }
	/** 获得这个连接总读取字节数. */
	ACE_UINT64 getTotalReadBytes()			{ return totalRBytes_; }
	/** 获得这个连接总写入字节数. */
	ACE_UINT64 getTotalWriteBytes()			{ return totalWBytes_; }

	/** 连接已通过 ACEConnector 或 ACE_Acceptor 有效建立.
		此函数主要负责对这个连接进行一些必要的初始化工作, 之后这个连接的状态被设置
		成 Established.
		@return 0表示成功，-1表示失败(失败后会调用close)
	*/
	virtual int open (void * = 0);

	/** 主动关闭此连接.
		此函数调用后，连接的状态被设置为LocalClosed.
	*/
	virtual int close (u_long flags = 0);

	/**	将数据填充到send buffer，等待接下来发送.
		可以多次调用这个函数，然后一次性调用 flushSendBuffer进行发送.
		@param data 数据指针.
		@param size 数据大小.
	*/
	virtual void fill(void* data, size_t size);

	/** 将当前的sendbuffer中的数据进行一次发送. 
		此函数会尝试一次发送，如果没用成功，则会注册可写事件，等待发送数据.
	*/
	virtual void flush();

	/** 被监听的连接收到数据事件.
		派生类需要重载这个接口，处理接收到的数据，并将处理过的数据大小返回。
		@param data 数据指针.
		@param size 数据大小.
		@return 返回处理了多少数据. -1表示出现错误.
	*/
	virtual int handleReceived(void* data, size_t size) { return size; }
	
public:
	/** ACE_Event_Handler Interface. */
	virtual int handle_input(ACE_HANDLE fd);
	/** ACE_Event_Handler Interface. */
	virtual int handle_output(ACE_HANDLE fd);
	/** ACE_Event_Handler Interface. */
	virtual int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
protected:
	/** 清理当前的连接. */
	void clear();

	Status					status_;		///< 当前的连接状态.
	int						errno_;			///< 当前的错误号.
	ACE_SOCK_Stream			stream_;		///< tcp socket steam. 
	ACE_INET_Addr			localAddr_;		///< 本地连接地址.
	ACE_INET_Addr			remoteAddr_;	///< 远端连接地址.
	ACE_Message_Block		sendBuf_;		///< 发送buffer.
	ACE_Message_Block		recvBuf_;		///< 接受buffer.
	ACE_UINT64				totalRBytes_;	///< 这个连接总读取字节数.
	ACE_UINT64				totalWBytes_;	///< 这个连接总写入字节数.
};

/** 连接管理器. 
	
	ConnectionManager 负责管理多个 CONN 对象。将有效的Connection
	对象添加到一个 ConnectionManager 后，可以随时通过 destroyInvalidConnections()
	函数销毁无效的 CONN 对象。 ConnectionManager 在被销毁时同时销毁
	所有的 CONN 对象。
*/
template<class CONN>
class ConnectionManager
{
public:
	typedef std::vector<CONN*> ConnectionList;

	ConnectionManager() 
	{
	}

	virtual ~ConnectionManager() 
	{
		destroyAllConnections(); 
	}

	/** 销毁所有无效的连接. */
	void destroyInvalidConnections()
	{
		for(typename ConnectionList::iterator iter = connections_.begin(); iter != connections_.end();)
		{
			
			if(!checkConnection(*iter))
			{
				delete *iter;
				iter = connections_.erase(iter);
			}
			else
				++iter;
		}

		//ACE_DEBUG((LM_DEBUG,"Current conneted handler %d \n",connections_.size()));
	}

	/** 销毁所有的连接. */
	void destroyAllConnections()
	{
		for(typename ConnectionList::iterator iter = connections_.begin(); iter != connections_.end(); ++iter)
			delete *iter;
		connections_.clear();
	}

protected:
	/** 检查一个连接的有效性. 
		此函数默认只对连接本身的状态进行检测，派生类可以重载此函数实现自定义的检
		查方法.
		@return true 表示有效.
	*/
	virtual bool checkConnection(CONN* c)
	{
		/*time_t t = ACE_OS::gettimeofday().sec();
		if(!c->chackAwaken(t)){
			c->close();
		}*/
		return (c->getStatus() == Connection::Established);
	}

	ConnectionList connections_;
};

#endif//__SVC_EventHandler_h__