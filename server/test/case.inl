
#include "ace/Log_Msg.h"

bool
CaseHandler::ping()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("CaseHandler::ping\n")));
	pong();
	return true;
}

bool
CaseHandler::loginok()
{
	return true;
}