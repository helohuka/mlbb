
#include "config.h"
#include "loghandler.h"
#include "account.h"
#include "client.h"
#include "dbhandler.h"
#include "worldserv.h"
int 
LogHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	isConnect_ = false;
	ACE_DEBUG((LM_INFO,ACE_TEXT("Logser serv closed...!!!\n")));
	return Connection::handle_close(handle,close_mask);
}
