/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

#include "config.h"
#include "worldserv.h"


#include "coredump.h"
#include "Logger.h"
//#include "coredump.h"

//class SignalHandler : public ACE_Event_Handler
//{
//public:
//	virtual int handle_signal (int sig, siginfo_t *, ucontext_t *)
//	{
//		// 结束事件循环.
//		ACE_Reactor::end_event_loop();
//		return 0;
//	}
//};

ACE_THR_FUNC_RETURN cmd_line (void *arg) 
{

	for (;;) 
	{
		ACE_OS::sleep(1);
		std::string userInput;
		std::getline (std::cin, userInput, '\n');
		if (!userInput.empty())
		{
			WorldServ::instance()->storeCmd(userInput);
		}
		
	}
	ACE_DEBUG((LM_INFO,"controller quit\n"));
	return 0;
}

int ACE_TMAIN (int argc, ACE_TCHAR *argv[])
{
	try{
#ifndef ACE_WIN32
	if (argc>1&&strcmp(argv[1],"-d")==0)
	{
		daemonize();
	}
	signal(SIGPIPE,SIG_IGN);
#endif
	Logger::instance()->init();
	Logger::instance()->enableFileOut(".world.log","log");
#ifdef ACE_WIN32
	//
	//_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	ACE_REACTOR_MAKE;
#elif defined (ACE_HAS_EVENT_POLL) || defined (ACE_HAS_DEV_POLL)
	ACE_REACTOR_MAKE;
	//ACE_Reactor::instance(new ACE_Reactor(new ACE_Dev_Poll_Reactor, 1), 1);
#else
	ACE_Reactor::instance();
#endif
	//SRV_ASSERT(0);

	//SignalHandler sh;
	//ACE_Reactor::instance()->register_handler(SIGINT, &sh);
	WorldServ::instance()->reactor(ACE_Reactor::instance());
	if(WorldServ::instance()->init(argc,argv) == -1)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Init WorldServ err.")));
		return -1;
	}
	
	ACE_Thread_Manager::instance ()->spawn (cmd_line, ACE_Reactor::instance());
	// 运行ractor event loop.
	ACE_Reactor::run_event_loop();

	} catch(std::exception& e){
		ACE_DEBUG((LM_ERROR, ACE_TEXT("exception failed (%s).\n"), e.what()));
	}
	WorldServ::instance()->fini();
	return 0;
}