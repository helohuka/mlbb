
#include "clientHandler.h"
#include "gwserv.h"
#include "gzip.h"
#include "ace/os_include/netinet/os_tcp.h"

//-------------------------------------------------------------------------
/**
 */

ClientHandler::ClientHandler()
:handlerId_(100000)
,isActive_(false)
{

}

ClientHandler::ClientHandler(int32 handlerId)
:handlerId_(handlerId)
,isActive_(false)
,timerId_(-1)
{
}

ClientHandler::~ClientHandler(){
}

std::string ClientHandler::infomation(){
	std::stringstream ss;
	ss << " Host:[" << remoteAddr_.get_host_addr() << ":" << remoteAddr_.get_port_number() << "] Handler:["<< handlerId_ << "] Channel:[" << channelId_ << "] LastError:[" << errno_ << "]";
	return ss.str();
}

int
ClientHandler::open(void* p)
{
	int r = Connection::open(p);
	
	if(r == -1 )
		return r;

	std::stringstream ssip;
	ssip << remoteAddr_.get_host_addr() << ":" << remoteAddr_.get_port_number();
	WorldHandler::instance()->syncConnectInfo(inDoor_, ssip.str());

	ClientChannel * channel = WorldHandler::instance()->connectChannel();
	SRV_ASSERT(channel);
	channelId_ = channel->getChannelId();
	channel->attachHandle(handlerId_);
	ACE_DEBUG((LM_INFO,ACE_TEXT("Open Socket%s\n"),infomation().c_str()));
	isActive_ = true;

	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	timerId_ = reactor()->schedule_timer(this, NULL, ACE_Time_Value(0), t);
	lastTime_ = ACE_OS::gettimeofday();
	return r;
}

int ClientHandler::handle_timeout(const ACE_Time_Value &current_time, const void *act )
{
	//心跳
	ACE_Time_Value d=current_time-lastTime_ ;
	if(d.sec()>300)
	{
		ACE_DEBUG((LM_INFO,"ClientHandler timeout %d | %d\n",channelId_,handlerId_));
		close();
		return 0;
	}
	return 0;
}

int ClientHandler::handleReceived(void* data, size_t size){

	lastTime_=ACE_OS::gettimeofday();

	GatewayServ::instance()->totalIncoming_ += size;

	ClientChannel* channel = WorldHandler::instance()->getChannel(channelId_);
	if(!channel || !channel->isValid())
	{
		ACE_DEBUG((LM_INFO,"if(!channel || !channel->isValid()) %d | %d\n",channelId_,handlerId_));
		close();
		return size;
	}

	U16 l = 0;
	S8* m = (S8*)data;
	S8* p = (S8*)data;
	
	//ACE_Guard<ACE_Recursive_Thread_Mutex> guard( mutex_ );
	//ACE_DEBUG((LM_DEBUG,"Send to world %d\n",incoming_.length()));
	while( (size_t)(p-m) < size)
	{
		U8 a = p-m;
		l = *(U16*)p;

		if(l>size)
			return 0;
		else if (l <= 0){
			ACE_DEBUG((LM_INFO,"Invalid sending data %d | %d\n",channelId_,handlerId_));
			close();
			return size;
		}
		p = p + sizeof(U16);
		channel->fillBegin();
		channel->fill(p,l);
		channel->fillEnd();
		//ACE_DEBUG((LM_DEBUG," %s send to world.\n",infomation().c_str()));
		//ACE_DEBUG((LM_INFO,ACE_TEXT("One message size = %d\n"),l));
		p = p + l;
	}
	return (p-m);
}

//-------------------------------------------------------------------------
/**
 */
int
ClientHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if (close_mask == ACE_Event_Handler::WRITE_MASK)  {
		return 0;  
	}
	ACE_DEBUG((LM_INFO," %s socket close.\n",infomation().c_str()));
	ClientChannel * channel = WorldHandler::instance()->getChannel(channelId_);
	if(channel){
		channel->dettachHandle();
		channel->close();
	}
	isActive_ = false;
	channelId_ = -1;
	if(timerId_ != -1){
		reactor()->cancel_timer(timerId_);
		timerId_ = -1;
	}
	return 0;
}


int ClientHandler::close(u_long flags ){
	if(!isActive_){
		ACE_DEBUG((LM_INFO,"Close invalid handler \n"));
		return 0;
	}
	ACE_DEBUG((LM_DEBUG,"%s close.\n",infomation().c_str()));
	ACE_Reactor_Mask mask =	ACE_Event_Handler::READ_MASK | 
		ACE_Event_Handler::WRITE_MASK |
		ACE_Event_Handler::DONT_CALL;
	reactor()->remove_handler(peer().get_handle(), mask);
	peer().close();
	isActive_ = false;
	ClientChannel * channel = WorldHandler::instance()->getChannel(channelId_);
	if(channel){
		channel->dettachHandle();
		channel->close();
	}
	channelId_ = -1;
	if(timerId_ != -1){
		reactor()->cancel_timer(timerId_);
		timerId_ = -1;
	}
	
	return 0;
}


//-------------------------------------------------------------------------
/**
 */

void IPV4ClientAccepter::init(int32 indoor, int port)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("IPV4ClientAcceptor::init\n")));
	
	inDoor_ = indoor;

	ACE_INET_Addr addr(port);
	if(this->open(addr) == -1)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("IPV6ClientAccepter::init %d field\n"),port));
		return ;
	}

	ACE_DEBUG((LM_INFO,ACE_TEXT("Lisen client at %d %s:%d\n"),indoor,addr.get_host_addr(),addr.get_port_number()));
}

int IPV4ClientAccepter::fini (void){
	ACE_DEBUG((LM_TRACE,ACE_TEXT("IPV4ClientAccepter fini... \n")));
	close();
	return 0;
}

int IPV4ClientAccepter::make_svc_handler(ClientHandler *&sh){
	sh =  GatewayServ::instance()->getDeactiveHandler();
	if(!sh){
		ACE_DEBUG((LM_INFO,"Client is max!!\n"));
		return -1;
	}
	
	sh->inDoor_ = inDoor_; //设置入口信息

	if( ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::make_svc_handler(sh) == -1)
	{
		ACE_DEBUG((LM_INFO,"ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::make_svc_handler(sh) == -1\n"));
		return -1;
	}
	
	return 0;
}

int IPV4ClientAccepter::accept_svc_handler(ClientHandler *sh){
	int r = ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::accept_svc_handler(sh);
	if(-1 == r)
	{
		ACE_DEBUG((LM_INFO,"ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::accept_svc_handler(sh) == -1\n"));
	}
	return r;
}

int IPV4ClientAccepter::activate_svc_handler (ClientHandler *sh){
	int r = ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::activate_svc_handler(sh);
	if(-1 == r)
	{
		ACE_DEBUG((LM_INFO,"ACE_Acceptor<ClientHandler, ACE_SOCK_ACCEPTOR>::activate_svc_handler(sh) == -1\n"));
	}
	return r;
}
