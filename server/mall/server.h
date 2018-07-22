
#ifndef __SERVER_H__
#define __SERVER_H__
#include "config.h"
#include "sqltask.h"
#include "handler.h"

class Server 
	: public ACE_Service_Object
{
public:
	static Server *instance()
	{
		static Server instance;
		return &instance;
	}
	WorldHandler* worldhandler(){return &worldhandler_;}
public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int handle_timeout (const ACE_Time_Value &current_time, const void *act);
public:
	WorldHandler		 worldhandler_;
};

#endif