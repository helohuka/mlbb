#include "BacklogServ.h"
#include "config.h"

int
BacklogSession::open(void* p)
{
	int r = Connection::open(p);
		

	ACE_INET_Addr const &remote = getRemoteAddr();

	ACE_DEBUG((LM_DEBUG,ACE_TEXT("One client conneted at address(%s:%d)\n"),remote.get_host_addr(),remote.get_port_number()));

	setProxy(this);
	return r;
}

int
BacklogSession::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if(ACE_OS::last_error() == EWOULDBLOCK)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("ACE_OS::last_error() == EWOULDBLOCK\n")));
		return 0;
	}

	BINConnection< NullStub, BacklogProxy >::handle_close(handle,close_mask);
	status_ = Connection::RemoteClosed;
	return 0;
}

bool
BacklogSession::log(STRING& key, STRING& msg, STRING& stack, STRING& version)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("%s,%s,%s,%s\n"),key.c_str(),version.c_str(),msg.c_str(),stack.c_str()));
	return true;
}

//////////////////////////////////////////////////////////////////////////

int
BacklogServ::init(int argc, ACE_TCHAR *argv[])
{
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

	ACE_INET_Addr addr(Env::get<const char*>(V_BacklogListen));
	if(this->open(addr) == -1)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("BacklogServ::init %s field\n"),Env::get<const char*>(V_BacklogListen)));
		return -1;
	}
	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, t, t);
	
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init backlog serv succ... \n")));
	return 0;
}

int
BacklogServ::fini()
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini backlog serv succ...\n")));

	// 退出计时器.
	this->reactor()->cancel_timer(this);

	return 0;
}

int 
BacklogServ::make_svc_handler(BacklogSession *&sh)
{
	if( ACE_Acceptor<BacklogSession, ACE_SOCK_ACCEPTOR>::make_svc_handler(sh) == -1)
	{
		return -1;
	}
	connections_.push_back(sh);

	return 0;
}

int
BacklogServ::handle_timeout(const ACE_Time_Value &current_time, const void *act /* = 0 */)
{
	destroyInvalidConnections();
	return 0;
}