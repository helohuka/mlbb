
#include "config.h"
#include "ComEnv.h"
#include "ComGlobal.h"
#include "ComScriptEvn.h"

#include "gwserv.h"
#include "worldhandler.h"
#include "clientHandler.h"
#include "json/json.h"

//-------------------------------------------------------------------------
/**
 */

int 
GatewayServ::init (int argc, ACE_TCHAR *argv[])
{
	enum {
		ClsPoolSize = 4000
	};
	startTime_  = ACE_OS::gettimeofday().get_msec();
	totalOutgoing_ = 0;
	totalIncoming_ = 0;
	maxIncoming_= 0;
	maxOutgoing_= 0;
	meanIncoming_= 0;
	meanOutgoing_= 0;
	lastIncoming_= 0;
	lastOutgoing_= 0;

	clientPool_.resize(ClsPoolSize);

	for(int i=0; i<ClsPoolSize; ++i){
		clientPool_[i] = new ClientHandler(i);
	}

	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init gateway serv... \n")));
	
	///读取LUA
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin load lua...\n")));
	ScriptEnv::init();

#include "ComScriptRegster.h"
#include "ComScriptApi.h"
	std::string err;
	if( !ScriptEnv::loadFile( "env.lua", err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load env.lua failed:%s\n"), err.c_str() ) );
		SRV_ASSERT(0);
	}
	connectWorld();
	acceptClient();
	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, t, t);
	//reactor()->schedule_timer(this, WorldHandler::instance(), ACE_Time_Value(0), t);
	
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init gateway serv succ... \n")));
	return 0;
}

//-------------------------------------------------------------------------
/**
 */
int 
GatewayServ::fini (void)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini world serv... \n")));
	for(int i=0; i<clientPool_.size(); ++i){
		clientPool_[i]->close();
		delete clientPool_[i];
	}
	clientPool_.clear();
	// 退出计时器.
	reactor()->cancel_timer(this);

	return 0;
}

//-------------------------------------------------------------------------
/**
 */
int 
GatewayServ::handle_timeout (const ACE_Time_Value &tv, const void *act)
{
	enum { INTERVAL = 5 };
	static float interval = INTERVAL;
	static U64 oldTime = tv.get_msec();
	U64 currTime = tv.get_msec() ;
	interval -= (currTime - oldTime) / 1000.F ;
	oldTime = currTime;
	if(interval <= 0.F)
	{
		float onlineTime = (currTime - startTime_) / 1000.F;
		WorldHandler * p = WorldHandler::instance();
		maxIncoming_ = maxIncoming_ < (totalIncoming_ - lastIncoming_) ? (totalIncoming_ - lastIncoming_) : maxIncoming_;
		maxOutgoing_ = maxOutgoing_ < (totalOutgoing_ - lastOutgoing_) ? (totalOutgoing_ - lastOutgoing_) : maxOutgoing_;
		meanIncoming_ = (uint64)(totalIncoming_ / onlineTime);
		meanOutgoing_ = (uint64)(totalOutgoing_ / onlineTime);
		size_t numCls =  getActiveNum();//(ClientManager::instance()->getConnctionNum());
		ACE_DEBUG((LM_INFO, "GW <==> WD INCOMING %d OUTGOING %d\n",p->getTotalReadBytes(),p->getTotalWriteBytes()));
		ACE_DEBUG((LM_INFO, "GW <==> CS INCOMING (%q,%q,%q) OUTGOING (%q,%q,%q) NUM %d\n",totalIncoming_,maxIncoming_,meanIncoming_,totalOutgoing_,maxOutgoing_,meanOutgoing_,numCls));
		interval += INTERVAL;
		lastOutgoing_ = totalOutgoing_;
		lastIncoming_ = totalIncoming_;
	}
	//ClientManager::instance()->destroyInvalidConnections();
	return 0;
}

//-------------------------------------------------------------------------
/**
 */
void GatewayServ::connectWorld()
{
	ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenGateway));
	ACE_Connector<WorldHandler, ACE_SOCK_CONNECTOR > connector;
	WorldHandler * p = WorldHandler::instance();
	if(connector.connect(p,addr) == -1)
	{
		ACE_ASSERT(0);
	}
}
void GatewayServ::acceptClient(){
	/*int port = Env::get<int>(V_GatewayListenClient);
	if (port != 0){
		acceptClient(port);
		return;
	}*/

	std::string conf = Env::get<std::string>(V_GatewayListenClientMultiIndoor);
	if(!conf.empty()){
		acceptClient(conf);
		return;
	}
	SRV_ASSERT(0);
}


void GatewayServ::acceptClient(int port){
	/*IPV4ClientAccepter * accepter = new IPV4ClientAccepter();
	accepter->init(Env::get<int32>(V_GameServId),port);
	accepters_.push_back(accepter);*/
}

void GatewayServ::acceptClient(std::string const& v){
	std::vector<std::string> arr = String::Split(v,";");
	for(size_t i=0; i<arr.size(); ++i){
		std::vector<std::string> parts = String::Split(arr[i],",");
		SRV_ASSERT(parts.size() == 2);
		int indoor = String::Convert<int>(parts[0]);
		int port = String::Convert<int>(parts[1]);
		IPV4ClientAccepter * accepter = new IPV4ClientAccepter();
		accepter->init(indoor,port);
		accepters_.push_back(accepter);
	}
}

ClientHandler* GatewayServ::getClientHandler(uint32 id){
	if(id >= clientPool_.size())
		return NULL;
	return clientPool_[id];
}

int GatewayServ::getActiveNum(){
	int r = 0;
	for(size_t i=0; i<clientPool_.size(); ++i){
		if(!clientPool_[i]->getActive()){
			++r;
		}
	}
	return r;
}

ClientHandler* GatewayServ::getDeactiveHandler(){
	for(size_t i=0; i<clientPool_.size(); ++i){
		if(!clientPool_[i]->getActive()){
			return clientPool_[i];
		}
	}
	return NULL;
}
