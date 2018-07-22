/** File generate by <hotlala8088@gmail.com> 2015/01/14  
 */

#ifndef __CLIENT_H__
#define __CLIENT_H__
#include "config.h"
#include "proto.h"

//-------------------------------------------------------------------------
/**
 * 
 */
class ClientChannel;
class ClientHandler : public Connection
{
public:
	typedef ACE_SOCK_Stream	stream_type;
	typedef ACE_INET_Addr	addr_type;

	ClientHandler();
	ClientHandler(int32 handlerId);
	~ClientHandler();

public:
	
	virtual int handleReceived(void* data, size_t size);
	
	virtual int open(void * );

	virtual int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
	
	virtual int close(u_long flags = 0);

	virtual int handle_timeout(const ACE_Time_Value &current_time, const void *act /* = 0 */);
	
	std::string infomation();
	
	void dettachChannel(){
		channelId_ = -1;
	}

	bool getActive(){
		return isActive_;
	}
public:
	int32 inDoor_;

private:
	bool					isActive_;
	int32					handlerId_;
	int32					channelId_;
	ACE_Time_Value			lastTime_;
	int32					timerId_;
	
};


//-------------------------------------------------------------------------
/**
 */

class IPV4ClientAccepter :  public ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>{

public:
	void init (int32 door, int port);
	int fini (void);
	int make_svc_handler(ClientHandler *&sh);
	int accept_svc_handler(ClientHandler *sh);
	int activate_svc_handler (ClientHandler *sh);

private:
	int32 inDoor_; //入口信息
	
};



#endif