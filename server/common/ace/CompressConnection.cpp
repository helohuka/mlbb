
#include "config.h"
#include "Connection.h"
#include "ace/Reactor.h"
#include "Logger.h"
#include "gzip.h"
#include "ace/os_include/netinet/os_tcp.h"
CompressConnection::CompressConnection(size_t rbSize, size_t sbSize):
status_(Unestablished),
errno_(0),
flushBuf_(sbSize),
sendBuf_(sbSize),
recvBuf_(rbSize),
recvGzipBuf_(rbSize),
sendGzipBuf_(sbSize),
totalRBytes_(0),
totalWBytes_(0)
{
}

CompressConnection::~CompressConnection()
{
	if(status_ == Established)
	{
		ACE_DEBUG((LM_INFO,"Connection::~Connection()\n"));
		close();
	}
}

const char* CompressConnection::getStatusDesc()
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

int CompressConnection::open(void * p)
{
	
	// ��ȡ��ַ.
	if(peer().get_remote_addr(remoteAddr_) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(stream_.get_remote_addr(remoteAddr_) == -1)\n"));
		return -1;
	}
	if(peer().get_local_addr(localAddr_) == -1)
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
	// ��socket����Ϊ������.
	if(peer().enable(ACE_NONBLOCK) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(stream_.enable(ACE_NONBLOCK) == -1)\n"));
		return -1;
	}

	// ע��ɶ��¼�.
	if(reactor()->register_handler(peer().get_handle(),this, ACE_Event_Handler::READ_MASK) == -1)
	{
		ACE_DEBUG((LM_ERROR,"if(reactor()->register_handler(stream_.get_handle(), this, ACE_Event_Handler::READ_MASK) == -1)\n"));
		return -1;
	}
	// ����send & recv buffers.
	flushBuf_.reset();
	sendBuf_.reset();
	recvBuf_.reset();
	sendGzipBuf_.reset();
	recvGzipBuf_.reset();
	totalRBytes_ = 0;
	totalWBytes_ = 0;
	status_ = Established;
	errno_	= 0;
	

	return 0;
}

int CompressConnection::close(u_long flags)
{
	if(status_ == Established)
	{
		
		ACE_Reactor_Mask mask =	ACE_Event_Handler::READ_MASK | 
			ACE_Event_Handler::WRITE_MASK |
			ACE_Event_Handler::DONT_CALL;
		reactor()->remove_handler(peer().get_handle(),mask);
		handle_close(peer().get_handle(),mask);
		peer().close();
		shutdown();
		status_ = LocalClosed;
		ACE_DEBUG((LM_INFO,"void Connection::close(), %s , %d\n",remoteAddr_.get_host_addr(),(int)status_));
	}
	return 0;
}

void CompressConnection::fill(void* data, size_t size)
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

void CompressConnection::flush()
{
	if(status_ != Established)
		return;
	if(!sendBuf_.length())
		return;
	

	if(!handle_output(0))
		reactor()->register_handler(peer().get_handle(),this, ACE_Event_Handler::WRITE_MASK);
}

int CompressConnection::handle_input(ACE_HANDLE fd)
{
	//ACE_DEBUG((LM_INFO,"int Connection::handle_input(ACE_HANDLE fd), %s\n",remoteAddr_.get_host_addr()));
	// ���sb���.
	if(recvGzipBuf_.space() == 0)
	{
		clear();
		status_ = RBOverflow;
		ACE_DEBUG((LM_DEBUG,"Connection::handle_input(ACE_HANDLE fd)if(recvBuf_.space() == 0)status_ = RBOverflow;\n"));
		return -1;
	}

	ssize_t recved = peer().recv(recvGzipBuf_.wr_ptr(), recvGzipBuf_.space());
	if(recved == -1)
	{
		if(ACE_OS::last_error() == EWOULDBLOCK)
		{
			return 0;
		}
		// ���ճ��ִ���.
		status_ = RecvFailed;
		clear();
		errno_	= ACE_OS::last_error();
		ACE_DEBUG((LM_DEBUG,"Connection::handle_input(ACE_HANDLE fd) if(recved == -1) status_ = RecvFailed;\n"));
		return -1;
	}
	else if(recved == 0)
	{
		// �Է������ر�������.
		status_ = RemoteClosed;
		clear();
		
		return -1;
	}
	else
	{
		// ���յ�����Ϣ�����н�ദ��.
		totalRBytes_ += recved;
		recvGzipBuf_.wr_ptr(recved);
		
		do{
			if(!tryRecved())
				break;
		}while(1);
	}
	return 0;
}

bool CompressConnection::tryRecved(){
	if(!recvGzipBuf_.length())
		return false;
	size_t* plen = (size_t*)recvGzipBuf_.rd_ptr();
	
	if(*plen > recvGzipBuf_.length()-sizeof(int))
		return false;
	recvGzipBuf_.rd_ptr(sizeof(int));
	
	uLong delen =recvBuf_.space();
	if(-1 == gzdecompress((Bytef*)recvGzipBuf_.rd_ptr(),*(uLong*)plen,(Bytef*)recvBuf_.wr_ptr(),&delen)){
		recvGzipBuf_.rd_ptr(*plen);
		recvGzipBuf_.crunch();
		return false;
		//SRV_ASSERT(0);
	}
	recvGzipBuf_.rd_ptr(*plen);
	recvGzipBuf_.crunch();
	
	recvBuf_.wr_ptr(delen);
	int processed = handleReceived(recvBuf_.rd_ptr(), recvBuf_.length());
	if(processed == -1)
	{
		// ��Ϣ������ִ���.
		status_ = MsgProcFailed;
		clear();
		return false;
	}
	else if(processed != 0)
	{
		// ����ɹ���crunch recv buffer.
		recvBuf_.rd_ptr((size_t)processed);
		recvBuf_.crunch();
		return true;
	}
	else
		return true;
}

int CompressConnection::handle_output(ACE_HANDLE fd)
{
	//ACE_DEBUG((LM_INFO,"int Connection::handle_output(ACE_HANDLE fd), %s\n",remoteAddr_.get_host_addr()));
	if(!sendBuf_.length())
		return -1;

	ssize_t sended = peer().send(sendBuf_.rd_ptr(), sendBuf_.length());
	if(sended == -1)
	{
		if(ACE_OS::last_error() == EWOULDBLOCK)
		{
			return sendBuf_.length()?0:-1;
		}
		// ���ͳ��ִ���.
		clear();
		status_ = SendFailed;
		errno_	= ACE_OS::last_error();
		ACE_DEBUG((LM_DEBUG," HandleOutput Send Failed %d LastError %d n",sendBuf_.length(),errno_));
		return 0;
	}
	else
	{
		// ���ͳɹ�, crunch send buffer
		totalWBytes_ += sended;
		sendBuf_.rd_ptr(sended);
		sendBuf_.crunch();
		// ���sb�л��в������ݣ�ע����һ�ο�д�¼�.
		return sendBuf_.length()?0:-1;
	}
}

int CompressConnection::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	return 0;
}

void CompressConnection::clear()
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
	reactor()->remove_handler(peer().get_handle(),mask);
	handle_close(peer().get_handle(),mask);
	peer().close();
	shutdown();
	
}
