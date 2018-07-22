//==============================================================================
/**
	@date:		2012:4:9  
	@file: 		main.cpp
	@author: 	Lucifer
*/
//==============================================================================
#include "config.h"
#include "server.h"
#include "Logger.h"
void assertPrepare(){}
class SignalHandler : public ACE_Event_Handler
{
public:
	virtual int handle_signal (int sig, siginfo_t *, ucontext_t *)
	{
		// 结束事件循环.
		ACE_Reactor::end_event_loop();
		return 0;
	}
};

ACE_THR_FUNC_RETURN controller (void *arg) 
{
	ACE_Reactor *reactor = static_cast<ACE_Reactor *> (arg);
	for (;;) {
		std::string userInput;
		std::getline (std::cin, userInput, '\n');
		if (userInput == "quit") {
			reactor->end_reactor_event_loop();
			break;
		}
	}
	return 0;
}
int ACE_TMAIN(int argc, ACE_TCHAR *argv[])
{
#ifndef ACE_WIN32
	if (argc>1&&strcmp(argv[1],"-d")==0)
	{
		daemonize();
	}
	signal(SIGPIPE,SIG_IGN);
#endif
	Logger::instance()->init();
	Logger::instance()->enableFileOut(".login.log","log");
#ifdef ACE_WIN32
	ACE_REACTOR_MAKE;
#elif defined (ACE_HAS_EVENT_POLL) || defined (ACE_HAS_DEV_POLL)
	ACE_Reactor::instance(new ACE_Reactor(new ACE_Dev_Poll_Reactor, 1), 1);
#else
	ACE_Reactor::instance();
#endif
	
	Server::instance()->reactor(ACE_Reactor::instance());

	Server::instance()->init(argc,argv);
	SignalHandler sh;
	ACE_Reactor::instance()->register_handler(SIGINT, &sh);

	// 运行ractor event loop.
	ACE_Reactor::run_event_loop();
	return 0;
}