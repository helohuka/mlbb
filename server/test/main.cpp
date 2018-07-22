/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

//-------------------------------------------------------------------------
/** 测试
 */

#include "config.h"
#include "task.h"
#include "TableSystem.h"

int ACE_TMAIN (int argc, ACE_TCHAR *argv[])
{
#ifndef ACE_WIN32
	if (argc>1&&strcmp(argv[1],"-d")==0)
	{
		daemonize();
	}
	signal(SIGPIPE,SIG_IGN);
#endif
#ifdef ACE_WIN32
	ACE_REACTOR_MAKE;
#elif defined (ACE_HAS_EVENT_POLL) || defined (ACE_HAS_DEV_POLL)
	ACE_Reactor::instance(new ACE_Reactor(new ACE_Dev_Poll_Reactor, 1), 1);
#else
	ACE_Reactor::instance();
#endif
	ScriptEnv::init();
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin load lua...\n")));
#include "ComScriptRegster.h"
#include "ComScriptApi.h"
	std::string err;
	if( !ScriptEnv::loadFile( "env.lua", err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load env.lua failed:%s\n"), err.c_str() ) );
		SRV_ASSERT(0);
	}
	if(!TableSystem::instance()->Load()){
		SRV_ASSERT(0);
	}
	if(!TableSystem::instance()->Check()){
		SRV_ASSERT(0);
	}
	CaseTaskFactory::instance()->init();
	CaseTaskFactory::instance()->initRobot();
	CaseTaskFactory::instance()->run();
	ACE_Reactor::run_event_loop();
	CaseTaskFactory::instance()->fini();
	
	return 0;
}
