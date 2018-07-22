#ifndef __ChannelConnection_h__
#define __ChannelConnection_h__
#include "predef.h"
#include "Connection.h"
class Channel;


/** Channel 使用的网络传输层基础类.

	ChannelConnection 本身从 Connection 派生，具备了 Connection 的基
	础网络传输功能。 ChannelConnection 负责传输 Channel 的数据和全局数据，
	并管理从属于自己的 Channel 对象。

	@par Channel连接模式与连接的建立.
	ChannelConnection 可以被初始化为两种互斥的模式:
	- Channel connector:
	  只能向对端发起一个Channel的主动连接，不会接受对端channel的连接请求。在此模
	  式下，通过 initChannelConnData() 函数发起一个channel的主动连接，然后可以通
	  过 fillChannelConnData() 函数填充初始连接数据。最后调用 flushChannelConnData()
	  发送连接数据。这样一个有效的channel连接就建立好，并可以使用这个 channel 进
	  行双向的数据传输.
	- Channel acceptor: 
	  只能接受对端的channel连接请求，不能像对端发起channel主动连接。在此模式下，
	  使用着需要通过派生并重载 makeChannel() 函数，创建被动连接的 Channel 对
	  象，并重载 acceptChannel() 函数对于这个有效的channel对象进行初始化，然后使
	  用这个channel 进行双向的数据传输.
	
	@par 连接初始化数据.
	在连接发起方连接时可以发送一些自定义的初始化数据，这些数据会与连接协议一同到
	达被连接方，用于初始化被连接方的channel.

	@par Channel连接的关闭.
	无论使用何种模式建立的channel连接都可以通过 closeChannel() 函数将其关闭。关闭
	后不能在使用这个channel进行后续的通讯。 closeChannel() 只在本端解除 Channel
	和 ChannelConnection 的关联性，并不会通知对端这个channel已经关闭。
	正确的channel关闭方式应该是通过高层的协议约定，在两端各自关闭对应的channel。
	基本流程应该是:
	- A发送一个消息给B
	- A关闭channel
	- B收到消息后关闭channel.

	@par channel数据.
	首先应该调用 initChannelSendingData() 函数初始化要发送的数据，并调用 fillSendingData()
	函数对数据进行填充，最后调用 flushSendingData进行发送。具体的发送功能已经被
	包装在 Channel 中。
	应该重载 Channel::handleReceived() 函数处理接收到的channel数据。

	@par 全局数据.
	首先应该调用 initGlobalSendingData() 函数初始化要发送的数据，并调用 fillSendingData()
	函数对数据进行填充，最后调用 flushSendingData进行发送。
	应该重载 handleGlobalData() 函数处理接收到的全局数据。

	@par 数据广播
	如果需要将一个数据同时传输给多个 Channel ，则应该使用 ChannelConnection 
	的广播功能.ChannelConnection 支持向一组 Channel 广播数据。使用广播可
	以有效减小数据的复制次数，压缩需要传输的数据量，从而有效地提高效率。
	对于广播数据的发送，首先应该调用 initBCSendingData() 函数初始化要发送的数据，
	并调用 fillSendingData() 函数对数据进行填充，最后调用 flushSendingData进行发
	送。
*/
class ChannelConnection : public Connection
{
public:
	typedef boost::unordered_map<unsigned int,Channel*> NodeContainerType;
	/** 协议类型. */
	enum ProtocolType
	{
		/** Channel建立连接协议. */
		PT_ChanConnect,
		/** Channel关闭连接协议*/
		PT_ChanDisconnect,
		/** Channel数据协议. */
		PT_ChanData,
		/** Channel广播数据. */
		PT_ChanBCData,
		/** 非Channel全局数据协议. */
		PT_GlobalData,
	};


	/** 构造函数.
		@param isConnector 这个 ChannelConnection 是 connector 还是 acceptor.
		@param rbSize 接收buffer大小
		@param sbSize 发送buffer大小
		@param bktSize hash bucket size, power of 2(default = 4096).
	*/
	ChannelConnection(
		bool isConnector, 
		size_t rbSize = 0XFFFFFF, 
		size_t sbSize = 0XFFFFFF/*,
		size_t bktSize = 12*/); 
	/** 析构函数.
		在这个connection销毁时，不负责关闭和销毁所有的channel，这个工作应该交给具
		体派生类处理.
	*/
	virtual ~ChannelConnection();

	/** 向对端发起一个 Channel 的连接，并准备通过 fillChannelConnData 填充连
		接数据. 
		@note 只有connector才可以调用这个函数.
		@param c 需要发起连接的 Channel 对象.如果这个channel已经连接到有效的
				 connection，则会自动断开。
	*/
	void connect(Channel* c);
	
	/** 断开一个 Channel 的连接.
		这个操作解除一个 Channel 与这个 ChannelConnection 的联系.
		@note 调用这个函数后，可以安全的删除 Channel 对象.
		@note connector 和 acceptor 都可以调用.
		@param c 需要断开的 Channel 对象.调用者需要保证这个 Channel 对象
				 当前在连接状态.
	*/
	void disconnect(Channel *c);
	/** 填充连接初始数据. */
	void fill(void* data, size_t size);
	/** 发送连接消息. */
	void flush();

	/** 接受了一个有效的 Channel 对象.
		当被初始化为 acceptor 模式后，当创建并初始化了一个有效的 Channel 对象，
		会调用这个函数。此时这个 Channel 对象已经与这个 ChannelConnection
		对象建立了联系。可以在此处作进一步的初始化工作，包括通过这个channel 发送
		消息或者销毁这个channel对象.
	*/
	virtual Channel* accept(){ ACE_ASSERT(0); return NULL;}
	
	/** 获得这个连接管理的所有 Channel 对象列表. */
	void getAllChannels(NodeContainerType& channels){
		channels = nodes_;
	}

	/** 开始发送一个channel的数据. 
		如果需要通过一个channel发送数据，首先应该调用 startChannelData ，然后通过
		fillSendBuffer将数据填入，最后调用 flushData 做最终的发送.
	*/
	void initChannelSendingData(Channel* c);
	/** 开始发送一个全局数据. 
		如果需要发送全局数据，首先应该调用 startGlobalData ，然后通过
		fillSendBuffer将数据填入，最后调用 flushData 做最终的发送.
	*/
	void initGlobalSendingData();
	/** 开始发送一个广播数据. 
		如果需要发送广播数据，首先应该调用 startBCData ，然后通过
		fillSendBuffer将数据填入，最后调用 flushData 做最终的发送.
	*/
	void initBCSendingData(std::set<Channel*>& channels);
	/** 填充待发送数据. */
	void fillSendingData(void* data, size_t size);
	/** 对填充的数据进行最终发送. */
	void flushSendingData();

	/** 处理接收到的非channel数据.
		@param data 数据指针.
		@param size 数据大小.
		@return 返回处理是否成功.
	*/
	virtual bool handleGlobalData(void* data, size_t size);

	int getNodeSize(){return nodes_.size();}

protected:
	/** 处理接收到的数据.
		根据channel协议,交给指定的 Channel 对象处理数据.
	*/
	virtual int handleReceived(void* data, size_t size);

	/** 添加一个channel. */
	bool addChannel(unsigned int key, Channel* value);
	/** 移除一个channel. */
	bool removeChannel(unsigned int key);
	/** 根据guid查找一个 Channel 对象. */
	Channel* findChannel( unsigned int key );

	bool			isConnector_;
	unsigned int	guidGen_;
	char*			msgLen_;

	NodeContainerType nodes_;
};

#endif//__ChannelConnection_h__
