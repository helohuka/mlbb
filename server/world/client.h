/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

#ifndef __WORLDCONN_H__
#define __WORLDCONN_H__


#include "config.h"
#include "proto.h"

//-------------------------------------------------------------------------
/** 注意 先有连接然后才会有 player 
 *  连接建立后会有登陆验证,验证通过后才会建立player进入game logic
 */

class Account;
class ClientHandler 
	: public BINChannel< Server2ClientStub, Client2ServerProxy >
	, public Client2ServerProxy
{
public:
	ClientHandler();
	~ClientHandler();
	bool handleClose();
	std::string infomation();

public:
	#include "Client2ServerMethods.h"
public:
	Account*account_;//账户信息
	int32 serverId_; //入口信息
	std::string ip_;
};

#endif // endif __WORLDCONN_H__
