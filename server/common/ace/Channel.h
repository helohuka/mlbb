#ifndef __Channel_h__
#define __Channel_h__

#include "predef.h"

class ChannelConnection;

/** 建立在真实的 ChannelConnection 之上的一个虚拟通讯连接.

	对于使用者来说， Channel 可以被看作与一个 Connection 一样，是一个独立
	的双向网络通讯管道，可以通过 *SendingData() 向对端发送数据，并通过重载 
	handleReceived 处理对端发送过来的数据。

	在传输层，所有的这些Channel 共享一个 ChannelConnection 对象，进行数据
	传输。每一个Channel 对象都有一个guid，用来在传输层数据中标识出属于自己的
	消息.
*/
class Channel
{
public:
	friend class ChannelConnection;

	/** Channel 在默认构造后没有有效的 ChannelConnection 与之关联，需要通
		过 ChannelConnection 的两种模式与之进行关联.
	*/
	Channel();

	/** Channel 会在析构函数中调用close.
	*/
	virtual ~Channel();

	/** 获得当前关联的 ChannelConnection.
		@return NULL 表示当前没有关联.
	*/
	ChannelConnection* getConn()	{ return conn_; }

	/** 监测这个 Channel 当前是否有效.
		当这个 Channel 与一个 ChannelConnection 有效关联，返回true
	*/
	bool isValid();
	unsigned int getGuid(){
		return guid_;
	}
	/** 初始化需要发送的数据. */
	void fillBegin();
	/** 填充需要发送的数据. */
	void fill(void* data, size_t size);
	/** 发送被填充的数据. */
	void fillEnd();

	/** 从管道的另一端接收到数据.
		派生类需要重载这个接口，处理接收到的数据
		@param data 数据指针.
		@param size 数据大小.
		@return 返回处理是否成功.
	*/
	virtual bool handleReceived( void* data, size_t size )	{ return true; }
	virtual bool handleConnect(){return true;}
	virtual bool handleClose();

protected:
	unsigned int		guid_;
	ChannelConnection*	conn_;
};

#endif//__Channel_h__
