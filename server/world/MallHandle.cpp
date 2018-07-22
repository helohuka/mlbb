#include "config.h"
#include "MallHandle.h"
#include "account.h"
#include "client.h"
#include "dbhandler.h"
#include "worldserv.h"

int 
MallHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Mall serv closed...!!!\n")));
	return Connection::handle_close(handle,close_mask);
}
bool MallHandler::fetchSellOk(S32 playerid, std::vector< COM_SellItem >& items, S32 total){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->fetchSellOk(items,total);
	return true;}
bool MallHandler::fetchMySellOk(S32 playerid, std::vector< COM_SellItem >& items){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->initSellingOk(items);
	return true;}
bool MallHandler::fetchSelledItemOk(S32 playerId, std::vector< COM_SelledItem >& items){
	Player* player = Player::getPlayerByInstId(playerId);
	if (NULL==player){
		return true;}
	player->initSelledOk(items);
	return true;
}
bool MallHandler::sellOk(S32 playerid, COM_SellItem& item){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->sellOk(item);
	return true;}
bool MallHandler::unSellOk(S32 playerid, S32 sellid){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->unSellOk(sellid);
	return true;}
bool MallHandler::buyOk(S32 playerid, COM_SellItem& item){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->buyOk(item);
	return true;}
bool MallHandler::buyFail(S32 playerid,ErrorNo error){
	Player* player = Player::getPlayerByInstId(playerid);
	if (NULL==player){
		return true;}
	player->buyFail(error);
	return true;}
