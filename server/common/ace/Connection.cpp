
#include "config.h"
#include "Connection.h"
#include "ace/Reactor.h"
#include "Logger.h"
#include "ace/os_include/netinet/os_tcp.h"
Connection::Connection(size_t rbSize, size_t sbSize):
status_(Unestablished),
errno_(0),
recvBuf_(rbSize),
sendBuf_(sbSize),
totalRBytes_(0),
totalWBytes_(0)
{
}

Connection::~Connection()
{
	if(status_ == Established)
	{
		ACE_DEBUG((LM_INFO,"Connection::~Connection()\n"));
		close();
	}
}

const char* Connection::getStatusDesc()
{
	switch(status_)
	{
	case Established:
		return "Established";
	case Unestablished:
		return "RBOverflow";
	case RBOverflow:
		return "RBOverflow";
	case SBOverflow:
		return "SBOverflow";
	case RecvFailed:
		return "RecvFailed";
	case SendFailed:
		return "SendFailed";
	case LocalClosed:
		return "LocalClosed";
	case RemoteClosed:
		return "RemoteClosed";
	case MsgProcFailed:
		return "MsgProcFailed";
	}

	return "unknown";
}

int Connection::open(void * p)
{
	// 获取地址.
	if(stream_.get_remote_addr(remoteAddr_) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(stream_.get_remote_addr(remoteAddr_) == -1) Last error %d\n",ACE_OS::last_error()));
		return -1;
	}
	if(stream_.get_local_addr(localAddr_) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(stream_.get_local_addr(localAddr_) == -1)\n"));
		return -1;
	}
	int nodelay = 1;
	if (-1 == peer().set_option (ACE_IPPROTO_TCP, TCP_NODELAY, &nodelay, sizeof (nodelay)))
	{
		ACE_DEBUG ((LM_ERROR, "set_option \n"));
		return -1;
	}
	// 将socket设置为非阻塞.
	if(stream_.enable(ACE_NONBLOCK) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(stream_.enable(ACE_NONBLOCK) == -1)\n"));
		return -1;
	}

	// 注册可读事件.
	if(reactor()->register_handler(stream_.get_handle(), this, ACE_Event_Handler::READ_MASK) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(reactor()->register_handler(stream_.get_handle(), this, ACE_Event_Handler::READ_MASK) == -1)\n"));
		return -1;
	}
	// 重置send & recv buffers.
	sendBuf_.reset();
	recvBuf_.reset();
	totalRBytes_ = 0;
	totalWBytes_ = 0;
	status_ = Established;
	errno_	= 0;

	return 0;
}

int Connection::close(u_long flags)
{
	if(status_ == Established)
	{
		ACE_Reactor_Mask mask =	ACE_Event_Handler::READ_MASK | 
			ACE_Event_Handler::WRITE_MASK |
			ACE_Event_Handler::DONT_CALL;
		reactor()->remove_handler(stream_.get_handle(), mask);
		handle_close(stream_.get_handle(),mask);
		stream_.close();
		status_ = LocalClosed;
	}
	return 0;
}

void Connection::fill(void* data, size_t size)
{
	//ACE_DEBUG((LM_INFO,"void Connection::fill(void* data, size_t size), %s\n",remoteAddr_.get_host_addr()));
	if(status_ != Established)
		return;
	if(sendBuf_.copy((const char*)data, size) == -1)
	{
		clear();
		status_ = SBOverflow;
		ACE_DEBUG((LM_DEBUG,"Connection::fill(void* data, size_t size) status_ = SBOverflow;\n"));
	}
}

void Connection::flush()
{
	if(status_ != Established)
		return;
	if(!sendBuf_.length())
		return;

	// 尝试做一次发送.
	ssize_t sended = stream_.send(sendBuf_.rd_ptr(), sendBuf_.length());
	if(sended == -1)
	{
	
		if(ACE_OS::last_error() != EWOULDBLOCK)
		{
			// 发送出现错误.
			clear();
			status_ = SendFailed;
			errno_	= ACE_OS::last_error();
			ACE_DEBUG((LM_DEBUG,"Flush  SendFailed , LastError %d Length %d;\n",errno_,sendBuf_.length()));
			return;
		}
	}
	else
	{
		// 发送成功.
		totalWBytes_ += sended;
		sendBuf_.rd_ptr(sended);
		sendBuf_.crunch();
	}

	// 查看sendbuffer中是否还有残留数据.
	if(sendBuf_.length())
	{
		// 注册监听写入事件，等待可写时再次发送.(多次注册会不会有问题？？？？)
		reactor()->register_handler(stream_.get_handle(), this, ACE_Event_Handler::WRITE_MASK);
	}
}

int Connection::handle_input(ACE_HANDLE fd)
{
	//ACE_DEBUG((LM_INFO,"int Connection::handle_input(ACE_HANDLE fd), %s\n",remoteAddr_.get_host_addr()));
	// 检测sb溢出.
	if(recvBuf_.space() == 0)
	{
		clear();
		status_ = RBOverflow;
		ACE_DEBUG((LM_DEBUG,"Connection::handle_input(ACE_HANDLE fd)if(recvBuf_.space() == 0)status_ = RBOverflow;\n"));
		return -1;
	}

	ssize_t recved = stream_.recv(recvBuf_.wr_ptr(), recvBuf_.space());
	if(recved == -1)
	{
		if(ACE_OS::last_error() == EWOULDBLOCK)
		{
			return 0;
		}
		else
		{
			return -1;
		}
		// 接收出现错误.
		status_ = RecvFailed;
		clear();
		errno_	= ACE_OS::last_error();
		ACE_DEBUG((LM_DEBUG,"Connection::handle_input(ACE_HANDLE fd) if(recved == -1) status_ = RecvFailed;\n"));
		return -1;
	}
	else if(recved == 0)
	{
		// 对方正常关闭了连接.
		status_ = RemoteClosed;
		clear();
		
		return -1;
	}
	else
	{
		// 接收到了消息，进行解编处理.
		totalRBytes_ += recved;
		recvBuf_.wr_ptr(recved);
		
		int processed = handleReceived(recvBuf_.rd_ptr(), recvBuf_.length());
		if(processed == -1)
		{
			// 消息处理出现错误.
			status_ = MsgProcFailed;
			clear();
			return -1;
		}
		else if(processed != 0)
		{
			// 处理成功，crunch recv buffer.
			recvBuf_.rd_ptr((size_t)processed);
			recvBuf_.crunch();
			return 0;
		}
		else
			return 0;
	}
}

int Connection::handle_output(ACE_HANDLE fd)
{
	//ACE_DEBUG((LM_INFO,"int Connection::handle_output(ACE_HANDLE fd), %s\n",remoteAddr_.get_host_addr()));
	if(!sendBuf_.length())
		return -1;

	ssize_t sended = stream_.send(sendBuf_.rd_ptr(), sendBuf_.length());
	if(sended == -1)
	{
		if(ACE_OS::last_error() == EWOULDBLOCK)
		{
			return sendBuf_.length()?0:-1;
		}
		// 发送出现错误.
		clear();
		status_ = SendFailed;
		errno_	= ACE_OS::last_error();
		ACE_DEBUG((LM_DEBUG," HandleOutput Send Failed %d LastError %d n",sendBuf_.length(),errno_));
		return 0;
	}
	else
	{
		// 发送成功, crunch send buffer
		totalWBytes_ += sended;
		sendBuf_.rd_ptr(sended);
		sendBuf_.crunch();
		// 如果sb中还有残留数据，注册下一次可写事件.
		return sendBuf_.length()?0:-1;
	}
}

int Connection::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	return 0;
}

void Connection::clear()
{
	if(ACE_OS::last_error() == EWOULDBLOCK)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("ACE_OS::last_error() == EWOULDBLOCK , Status = %d , Peer = %s\n"),status_,remoteAddr_.get_host_addr()));
		return;
	}
	ACE_DEBUG((LM_INFO,"void Connection::clear(), %s , %d\n",remoteAddr_.get_host_addr(),(int)status_));
	ACE_Reactor_Mask mask =	ACE_Event_Handler::READ_MASK | 
							ACE_Event_Handler::WRITE_MASK |
							ACE_Event_Handler::DONT_CALL;
	reactor()->remove_handler(stream_.get_handle(), mask);
	handle_close(stream_.get_handle(),mask);
	stream_.close();
	
}
