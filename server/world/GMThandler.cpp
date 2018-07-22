#include "config.h"
#include "GMThandler.h"
#include "account.h"
#include "client.h"
#include "worldserv.h"
#include "player.h"
#include "Activity.h"
#include "loghandler.h"
#include "Shop.h"
#include "dbhandler.h"
int 
GMTHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("GMT serv closed...!!!\n")));
	isConnect_ = false;
	return Connection::handle_close(handle,close_mask);
}

bool
GMTHandler::noTalkPlayer(U32 playerId, U32 time)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	if(time <= 0)
		return true;
	player->setNoTalkTime(time);
	return true;
}

bool
GMTHandler::sealPlayer(U32 playerId)
{ ///封账号

	std::string accname = WorldServ::instance()->getAccontNameByPlayerId(playerId);
	if(accname.empty()){
		ACE_DEBUG((LM_ERROR,"Can not find account name by player id(%d) in seal player \n",playerId));
		return true;
	}

	LoginHandler::instance()->setAccountSeal(accname,true);
		
	return true;
}

bool
GMTHandler::openTalkPlayer(U32 playerId)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	player->setNoTalkTime(0.f);
	return true;
}

bool
GMTHandler::unsealPlayer(U32 playerId)
{
	std::string accname = WorldServ::instance()->getAccontNameByPlayerId(playerId);
	if(accname.empty()){
		ACE_DEBUG((LM_ERROR,"Can not find account name by player id(%d) in unseal player \n",playerId));
		return true;
	}

	LoginHandler::instance()->setAccountSeal(accname,false);

	return true;
}

bool
GMTHandler::kickPlayer(U32 playerId)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	if(player->account_)
	{
		Account::removeAccountByName(player->account_->username_);
	}
	return true;
}

bool
GMTHandler::addExp(U32 playerId,U32 exp)
{

	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	player->addExp(exp);
	return true;
}

bool
GMTHandler::openGM(U32 playerId)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	player->isGm_ = true;
	return true;
}


bool
GMTHandler::closeGM(U32 playerId)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	player->isGm_ = false;
	return true;
}

bool
GMTHandler::addMoney(U32 playerId,int32 money)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	
	player->addMoney(money);

	return true;
}

bool
GMTHandler::addDiamond(U32 playerId,int32 dia)
{
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	ACE_DEBUG((LM_INFO,"GMT add diamond %s(%d) %d\n",player->getNameC(),playerId,dia));
	player->addDiamond(dia);
	return true;
}


bool
GMTHandler::sendMailAllOnline(COM_Mail& mail,int32 lowLevel,int32 highLevel,int64 lowTime, int64 highTime)
{
	WorldServ::instance()->sendMailAllOnline(mail,lowLevel,highLevel,lowTime,highTime);
	return true;
}

bool
GMTHandler::gmtNotice(NoticeSendType bType, std::string& note, U64 thetime, S64 itvtime){
	WorldServ::instance()->gmNotice(bType,note,thetime,itvtime);
	return true;
}


bool GMTHandler::setChargeTotal(COM_ADChargeTotal &data){
	RechargeTotal::update(data);
	return true;
}
bool GMTHandler::setChargeEvery(COM_ADChargeEvery &data){
	RechargeSingle::update(data);
	return true;
}
bool GMTHandler::setDiscountStore(COM_ADDiscountStore &data){
	DiscountStore::update(data);
	return true;
}
bool GMTHandler::setLoginTotal(COM_ADLoginTotal &data){
	Festival::update(data);
	return true;
}
bool GMTHandler::setHotRole(COM_ADHotRole& data){
	HotShop::update(data);
	return true;
}

bool GMTHandler::setEmployeeActivity(COM_ADEmployeeTotal& data){
	EmployeeActivityTotal::update(data);
	return true;
}

bool GMTHandler::setMinGiftBagActivity(COM_ADGiftBag& data){
	MinGift::update(data);
	return true;
}

bool GMTHandler::makeOrder(uint32 playerId, SGE_GmtOrder & order){
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
	{
		SGE_OrderInfo info;
		info.productId_ = order.shopId_;
		info.productCount_ = 1;
		info.amount_ = order.payment_;
		info.orderId_ = order.orderId_;
		info.payTime_ = TimeFormat::StrLocalTimeNow();
		DBHandler::instance()->insertLoseCharge(playerId,info);
	}
	else {
		player->orderFromGMT(order.shopId_,1,order.orderId_,TimeFormat::StrLocalTimeNow(),order.payment_);
	}
	return true;
}
bool GMTHandler::doScript(std::string& script){
	ACE_DEBUG((LM_INFO,"GMT DO SCRIPT ===> %s <===\n",script.c_str()));
	WorldServ::instance()->storeCmd(script);
	return true;
}
bool GMTHandler::playerDoScript(uint32 playerId, std::string& script){
	ACE_DEBUG((LM_INFO,"GMT PLAYER DO SCRIPT ===> %d %s <===\n",playerId, script.c_str()));
	Player* player = Player::getPlayerByInstId(playerId);
	if(player == NULL)
		return true;
	player->doScript(script);
	return true;
}

bool GMTHandler::setZhuanpanActivity(COM_ZhuanpanData& data){
	Zhuanpan::update(data);
	return true;
}
bool GMTHandler::setIntegralshop(COM_IntegralData& data){
	IntegralShop::update(data);
	return true;
}