
#include "config.h"
#include "loginhandler.h"
#include "account.h"
#include "client.h"
#include "dbhandler.h"
#include "worldserv.h"
#include "loghandler.h"
int
LoginHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Login serv closed...!!!\n")));
	return Connection::handle_close(handle,close_mask);
}

bool
LoginHandler::queryAccountOk(COM_AccountInfo& info, bool isNew,bool isSeal)
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Query account %s ok ...!!!\n"),info.username_.c_str()));

	VerifiedAccount *va = VerifiedAccount::get(info.username_);
	if(NULL == va)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Account is offline  %s...!!!\n"),info.username_.c_str()));
		return true;
	}
	
	if(!va->client_)
	{
		VerifiedAccount::del(info.username_);
		return true;
	}
	if(isSeal){
		//已经被冻结
		ACE_DEBUG((LM_DEBUG,"Accout(%s) is seal can not login!\n",info.username_.c_str()));
		CALL_CLIENT(va,errorno(EN_AccountIsSeal));
		GatewayHandler::instance()->disconnect(va->client_);
		return true;
	}

	Account* acc = Account::getAccountByName(info.username_);
	if(acc != NULL)
	{
		acc->deConnect();
		
		if(acc->serverId_ == va->client_->serverId_){
			acc->reConnect(va->client_);
			acc->sdkInfo_ = va->sdkInfo_;
			VerifiedAccount::del(va->username_);
			return true;
		}
		
		Account::removeAccountByName(info.username_);
	}
	acc = Account::createAccount(va->client_,info);
	acc->sdkInfo_ = va->sdkInfo_;
	SRV_ASSERT(acc);
	DBHandler::instance()->queryPlayerSimpleInformation(info.username_,acc->client_->serverId_);
	
	
	if(isNew){
		WorldServ::instance()->pushAccountLog(acc);
		CALL_CLIENT(acc,logoutOk()); ///用这个协议通知前端是新帐号
	}

	VerifiedAccount::del(va->username_);

	return true;
}

bool LoginHandler::setAccountSealOk(std::string& accountname){
	Account::removeAccountByName(accountname);
	return true;
}