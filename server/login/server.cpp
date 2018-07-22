

#include "config.h"
#include "ComEnv.h"
#include "ComGlobal.h"
#include "ComScriptEvn.h"

#include "handler.h"
#include "server.h"
#include "routine.h"

std::string dbAddr_;		///<mysql	地址
std::string dbName_;		///<mysql	数据库名
std::string dbUser_;		///<mysql	数据库用户名
std::string dbPwd_;			///<mysql	密码
int 
Server::init (int argc, ACE_TCHAR *argv[])
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init login serv... \n")));
	
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

	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, ACE_Time_Value(0), t);
	
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin accept world server...\n")));

	ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenLogin));

	ACE_Connector<WorldHandler, ACE_SOCK_CONNECTOR > connector;
	WorldHandler *p = WorldHandler::instance();
	if(connector.connect(p,addr) == -1)
	{
		ACE_ASSERT(0);
	}

	dbAddr_  = Env::get<const char*>(V_MysqlHost);
	dbName_	 = Env::get<const char*>(V_DatabaseName);
	dbUser_	 = Env::get<const char*>(V_MysqlUser);
	dbPwd_	 = Env::get<const char*>(V_MysqlPassword);
	SQLTask::sinit();

	InitSQLTables *pInitSQLTables = NULL;
	Routine::create(pInitSQLTables);
	SQLTask::sglob(pInitSQLTables);

	return 0;
}

int 
Server::fini (void)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini login serv... ")));

	// 退出计时器.
	reactor()->cancel_timer(this);
	SQLTask::sfini();
	return 0;
}

int 
Server::handle_timeout (const ACE_Time_Value &current_time, const void *act)
{
	SQLTask::doback();
	SQLTask::ping();
	return 0;
}



