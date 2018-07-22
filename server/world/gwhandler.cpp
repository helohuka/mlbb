
#include "config.h"
#include "gwhandler.h"
#include "account.h"
#include "client.h"
#include "player.h"
#ifdef _WIN32
GatewayHandler::GatewayHandler():BINChannelConnection<Channel,__StubDummy,SGE_Gateway2WorldProxy>(false,0XFFFFFF,0XFFFFFF),isConnect_(false){
	setProxy(this);
}

#else 
GatewayHandler::GatewayHandler():BINChannelConnection<Channel,__StubDummy,SGE_Gateway2WorldProxy>(false,0XFFFFFFF,0XFFFFFFF),isConnect_(false){
	setProxy(this);
}

#endif

GatewayHandler::~GatewayHandler(){
}

bool GatewayHandler::syncConnectInfo(int32 indoor, std::string& ip){
	inDoor_ = indoor;
	channelIp_ = ip;
	return true;
}

int 
GatewayHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if(!isConnect_){
		ACE_DEBUG((LM_INFO,ACE_TEXT("Gateway serv already closed!!!\n")));
		return -1;
	}
	if(close_mask == ACE_Event_Handler::WRITE_MASK){
		ACE_DEBUG((LM_INFO,ACE_TEXT("Gateway serv closed by write mask!!!\n")));
		return 0;
	}
	ACE_DEBUG((LM_INFO,ACE_TEXT("Gateway serv closed...!!!\n")));
	isConnect_ = false;
	//Account::clean(); ///清除所有在线玩家
	
	Player::saveAll();
	Account::clean();
	
	Connection::handle_close(handle,close_mask);

	ACE_Reactor::instance()->end_event_loop();
	return -1; //不再监听尼玛close事件
}

Channel *GatewayHandler::accept(){
	ClientHandler *p = NEW_MEM(ClientHandler);
	p->serverId_ = inDoor_;
	p->ip_ = channelIp_;
	return p;
};

int
GatewayHandler::handleReceived(void* data, size_t size)
{
	return ChannelConnection::handleReceived(data,size);
}

