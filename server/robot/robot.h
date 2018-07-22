#ifndef __BACKLOG_SERV_H__
#define __BACKLOG_SERV_H__

#include "config.h"
#include "proto.h"

class NullStub{};

class BacklogSession :public BINConnection< NullStub, BacklogProxy >
	, public BacklogProxy
{
public:
#include "BacklogMethods.h"
public:
	int open(void * /* = 0 */);
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
};


class BacklogServ
	: public ACE_Acceptor<BacklogSession, ACE_SOCK_ACCEPTOR>
	, public ConnectionManager<BacklogSession>
{
public:
	static BacklogServ* instance() {static BacklogServ self; return &self;}
public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int make_svc_handler(BacklogSession *&sh);
	int handle_timeout(const ACE_Time_Value &current_time, const void *act  = 0 );
};

#endif