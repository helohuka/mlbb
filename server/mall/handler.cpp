
#include "handler.h"
#include "config.h"
#include "routine.h"

#define CREATE_ROUTINE(P,TYPE) TYPE* P = NULL; Routine::create(P); SRV_ASSERT(P);

bool WorldHandler::fetchSell(S32 playerid, COM_SearchContext& context){
	CREATE_ROUTINE(p,FetchSell);
	p->playerId_ = playerid;
	p->context_ = context;
	p->total_ = 0;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::fetchMySell(S32 playerid){
	CREATE_ROUTINE(p,FetchMySell);
	p->playerId_ = playerid;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::sell(COM_SellItem& item){
	CREATE_ROUTINE(p,Sell);
	p->playerId_ = item.sellPlayerId_;
	p->item_ = item;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::unSell(S32 playerid, S32 sellid){
	CREATE_ROUTINE(p,Unsell);
	p->playerId_ = playerid;
	p->guid_ = sellid;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::buy(SGE_BuyContent&  bc){
	CREATE_ROUTINE(p,Buy);
	p->bc_ = bc;
	p->error_ = EN_MallBuyOk;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::fetchSelledItem(S32 playerId){
	CREATE_ROUTINE(p,FetachSelledItem);
	p->playerId_ = playerId;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::insertSelledItem(COM_SelledItem& item){
	CREATE_ROUTINE(p,InsertSelledItem);
	p->item_ = item;
	SQLTask::spost(p);
	return true;
}