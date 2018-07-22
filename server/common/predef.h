#ifndef ___PRE_DEF_H__
#define ___PRE_DEF_H__
#include "nedmalloc/nedmalloc.h"
#include <stdlib.h>
#include <vector>
#include <map>
#include <unordered_map>
#include <set>
#include <queue>
#include <algorithm>

#include <sstream>
#include <string.h>
#include <iostream>
#include <fstream>

#define BOOST_FILESYSTEM_VERSION 3
#include <boost/filesystem.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/weak_ptr.hpp>
#include <boost/unordered_map.hpp>
#include "LinuxCommon.h"

#include "ace/OS.h" 
#include "ace/Service_Config.h"
#include "ace/Select_Reactor.h"
#include "ace/Dev_Poll_Reactor.h"
#include "ace/Proactor.h"
#include "ace/Acceptor.h"
#include "ace/SOCK_Acceptor.h"
#include "ace/Time_Value.h"
#include "ace/Event_Handler.h"
#include "ace/SOCK_Connector.h"
#include "ace/Connector.h"
#include "ace/Log_Record.h"
#include "ace/Log_Msg_Callback.h"
#include "ace/Log_Msg.h"
#include "ace/OS_NS_stdio.h"
#include "ace/OS_NS_sys_stat.h"
#include "ace/Guard_T.h"
#include "ace/Recursive_Thread_Mutex.h"
#include "ace/Process.h"  
#include "ace/Process_Manager.h"
#include "ace/Atomic_Op.h"
#include "ace/Date_Time.h"


#define SRV_ASSERT(X)\
	do {\
	__assert__(X,__FILE__,__LINE__);\
	} while (0)

inline void __assert__(bool _X_,const char* file, int line)
{
	if (!(_X_))
	{
		//assertPrepare();
#ifdef WIN32
		__asm{ int 3}
#else
		raise(SIGABRT);
#endif
	}
}

#ifdef WIN32
#	pragma warning(error : 4258)
#	pragma warning(error : 4715)
#	pragma warning(disable : 4819)
#	ifndef _CRT_SECURE_NO_WARNINGS
#		define _CRT_SECURE_NO_WARNINGS
#	endif
#else
#	include <unordered_map>
#endif

#endif