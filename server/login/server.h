
#ifndef __SERVER_H__
#define __SERVER_H__
#include "config.h"
#include "sqltask.h"
#include "handler.h"

class Server 
	: public ACE_Service_Object
{
public:
	SINGLETON_FUNCTION(Server)
public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int handle_timeout (const ACE_Time_Value &current_time, const void *act);
	
public:
};

#endif