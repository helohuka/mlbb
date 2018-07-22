
#include "handler.h"
#include "config.h"
#include "routine.h"

#define CREATE_ROUTINE(P,TYPE) TYPE* P = NULL; Routine::create(P); SRV_ASSERT(P);

bool 
WorldHandler::queryAccount(COM_LoginInfo& info)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Query account %s , %s \n"),info.username_.c_str(),info.password_.c_str()));
	CREATE_ROUTINE(p,QueryAccount);
	p->username_ = info.username_;
	p->account_.username_	= info.username_;
	p->account_.password_	= info.password_;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::setAccountSeal(std::string& accname, bool isSeal){
	ACE_DEBUG((LM_INFO,ACE_TEXT("Seal account %s , %d \n"),accname.c_str(),(int)isSeal));
	CREATE_ROUTINE(p,SealAccount);
	p->accname_ = accname;
	p->isSeal_ = isSeal;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::setPhoneNumber(std::string &accountname, std::string &phoneNumber){
	ACE_DEBUG((LM_INFO,ACE_TEXT("Seal phone Number  %s , %s \n"),accountname.c_str(),phoneNumber.c_str()));
	CREATE_ROUTINE(p,SetPhoneNumber);
	p->accname_ = accountname;
	p->phoneNumber_ = phoneNumber;
	SQLTask::spost(p);
	return true;
}