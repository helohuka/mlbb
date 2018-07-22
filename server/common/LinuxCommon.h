//==============================================================================
/**
	@date:		2013:1:15  
	@file: 		LinuxCommon.h
	@author		Lucifer
*/
//==============================================================================

#ifndef __LinuxCommon_H__
#define __LinuxCommon_H__

#ifndef WIN32
#include "ace/os_include/sys/os_types.h"
#include "ace/OS_NS_unistd.h"
#include "ace/OS_NS_signal.h"
#include "ace/OS_NS_sys_stat.h"
#include "ace/ACE.h"

int daemonize();

#endif
#endif