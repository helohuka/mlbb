//==============================================================================
/**
	@date:		2013:1:15  
	@file: 		LinuxCommon.cpp
	@author		Lucifer
*/
//==============================================================================

#ifndef WIN32
#include "LinuxCommon.h"

int daemonize()
{
	pid_t pid = ACE_OS::fork ();

	if (pid == -1)
		return -1;
	else if (pid != 0)
		ACE_OS::exit (0); // Parent exits.pid_t pid = ACE_OS::fork ();

	if (pid == -1)
		return -1;
	else if (pid != 0)

		//1st child continues.
	ACE_OS::setsid (); // Become session leader.

	ACE_OS::signal (SIGHUP, SIG_IGN);

	pid = ACE_OS::fork ();

	if (pid != 0)
		ACE_OS::exit (0); // First child terminates.+

	for(int i = ACE::max_handles () - 1; i >= 0; i--)
		ACE_OS::close (i);

	int fd = ACE_OS::open ("/dev/null", O_RDWR, 0);
	if (fd != -1)
	{
		ACE_OS::dup2 (fd, ACE_STDIN);
		ACE_OS::dup2 (fd, ACE_STDOUT);
		ACE_OS::dup2 (fd, ACE_STDERR);

		if (fd > ACE_STDERR)
			ACE_OS::close (fd);
	}
	ACE_OS::umask(0);
	return 0;
}


#endif
