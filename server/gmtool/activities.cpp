
#include "config.h"
#include "HttpParser.h"
#include "handler.h"

void ClientHandler::gmLoginActivity(Json::Value& json){
	COM_ADLoginTotal data;
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmLogin json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmLogin json[CloseTime].asInt() error\n")));
		return; 
	}
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	if(!json["Content"].isArray()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmLogin json[Content].isArray() error\n")));
		return;
	}
	for (size_t i = 0;i<json["Content"].size(); ++i)
	{
		if(!json["Content"][i]["Rewards"].isArray())
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmLogin json[Content][i][Rewards].isArray() error\n")));
			continue;
		}
		COM_ADLoginTotalContent item;
		for (size_t j = 0; j<json["Content"][i]["Rewards"].size();++j)
		{
			item.itemIds_ .push_back(json["Content"][i]["Rewards"][j]["ItemId"].asInt());
			item.itemStacks_.push_back(json["Content"][i]["Rewards"][j]["ItemStack"].asInt());
		}
		item.totalDays_ = json["Content"][i]["LimitNum"].asInt();
		if(item.itemIds_.empty())
			continue;
		data.contents_.push_back(item);
	}
	WorldHandler::instance()->setLoginTotal(data);
	result(0,"Reset login activity success");
}

void ClientHandler::gmChargeTotal(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmCharge json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmCharge json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ADChargeTotal data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	if(!json["Content"].isArray()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeTotal json[Content].isArray() error\n")));
		return;
	}
	for (size_t i = 0;i<json["Content"].size(); ++i)
	{
		if(!json["Content"][i]["Rewards"].isArray())
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeTotal json[Content][i][Rewards].isArray() error\n")));
			continue;
		}
		COM_ADChargeTotalContent item;
		for (size_t j = 0; j<json["Content"][i]["Rewards"].size();++j)
		{
			item.itemIds_ .push_back(json["Content"][i]["Rewards"][j]["ItemId"].asInt());
			item.itemStacks_.push_back(json["Content"][i]["Rewards"][j]["ItemStack"].asInt());
		}
		item.currencyCount_ = json["Content"][i]["LimitNum"].asInt();
		if(item.itemIds_.empty())
			continue;
		data.contents_.push_back(item);
	}

	WorldHandler::instance()->setChargeTotal(data);
	result(0,"Reset charge total success");
}

void ClientHandler::gmChargeEvery(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeEvery json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeEvery json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ADChargeEvery data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	if(!json["Content"].isArray()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeEvery json[Content].isArray() error\n")));
		return;
	}
	for (size_t i = 0;i<json["Content"].size(); ++i)
	{
		if(!json["Content"][i]["Rewards"].isArray())
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmChargeEvery json[Content][i][Rewards].isArray() error\n")));
			continue;
		}
		COM_ADChargeEveryContent item;
		for (size_t j = 0; j<json["Content"][i]["Rewards"].size();++j)
		{
			item.itemIds_ .push_back(json["Content"][i]["Rewards"][j]["ItemId"].asInt());
			item.itemStacks_.push_back(json["Content"][i]["Rewards"][j]["ItemStack"].asInt());
		}
		item.currencyCount_ = json["Content"][i]["LimitNum"].asInt();
		if(item.itemIds_.empty())
			continue;
		data.contents_.push_back(item);
	}

	WorldHandler::instance()->setChargeEvery(data);

	result(0,"Reset single charge success");
}

void ClientHandler::gmDiscountStore(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ADDiscountStore data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	if(!json["Rewards"].isArray()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[Rewards].isArray() error\n")));
		return;
	}
	for(int32 i=0; i<json["Rewards"].size(); ++i){
		if(!json["Rewards"][i]["LimitNum"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[Rewards][i][LimitNum].asInt() error\n")));
			return; 
		}
		if(!json["Rewards"][i]["ItemId"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[Rewards][i][ItemId].asInt() error\n")));
			return; 
		}
		if(!json["Rewards"][i]["Price"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[Rewards][i][Price].asInt() error\n")));
			return; 
		}
		if(!json["Rewards"][i]["Discount"].isDouble()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmDiscount json[Rewards][i][Discount].asDouble() error\n")));
			return; 
		}
		COM_ADDiscountStoreContent item;
		item.buyLimit_  = json["Rewards"][i]["LimitNum"].asInt();
		item.itemId_  = json["Rewards"][i]["ItemId"].asInt();
		item.price_ = json["Rewards"][i]["Price"].asInt();
		item.discount_ = json["Rewards"][i]["Discount"].asDouble();
		if(item.itemId_ == 0)
			continue;
		data.contents_.push_back(item);
	}
	//data.endStamp_ = data.sinceStamp_ + ONE_DAY_SEC * data.contents_.size();
	WorldHandler::instance()->setDiscountStore(data);

	result(0,"Reset DiscountStore success");
}

void ClientHandler::gmHotRole(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ADHotRole data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();
	
	for(int32 i=0; i<json["Items"].size(); ++i){
		/*if(!json["Items"][i]["LimitNum"].isBool()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[items][i][LimitNum].asBool() error\n")));
			return; 
		}*/
		if(!json["Items"][i]["RoleId"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[items][i][ItemId].asInt() error\n")));
			return; 
		}
		if(!json["Items"][i]["Price"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[items][i][Price].asInt() error\n")));
			return; 
		}
		if(!json["Items"][i]["BuyLimit"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmHotRole json[items][i][BuyLimit].asInt() error\n")));
			return; 
		}
		COM_ADHotRoleContent content;
		content.type_ = json["Items"][i]["LimitNum"].asBool() ? ET_Emplyee:ET_Baby;
		content.roleId_  = json["Items"][i]["RoleId"].asInt();
		content.price_ = json["Items"][i]["Price"].asInt();
		content.buyNum_ = json["Items"][i]["BuyLimit"].asInt();
		if(content.roleId_ == 0)
			continue;
		data.contents_.push_back(content);
	}

	WorldHandler::instance()->setHotRole(data);

	result(0,"Reset HotRole success");
}

void ClientHandler::gmEmployeeActivity(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmEmployee json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmEmployee json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ADEmployeeTotal data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();
	
	if(!json["Content"].isArray())
		return;
	for (size_t i = 0;i<json["Content"].size(); ++i)
	{
		COM_ADEmployeeTotalContent item;
		for (size_t j = 0; j<json["Content"][i]["Rewards"].size();++j)
		{
			item.itemIds_ .push_back(json["Content"][i]["Rewards"][j]["ItemId"].asInt());
			item.itemStacks_.push_back(json["Content"][i]["Rewards"][j]["ItemStack"].asInt());
		}
		item.outputCount_ = json["Content"][i]["LimitNum"].asInt();
		if(item.itemIds_.empty())
			continue;
		data.contents_.push_back(item);
	}

	WorldHandler::instance()->setEmployeeActivity(data);
	result(0,"Reset EmployeeActivity success");
}

void ClientHandler::gmMinGiftbag(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[CloseTime].asInt() error\n")));
		return; 
	}
	if( !json["Price"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[Price].asInt() error\n")));
		return; 
	}
	if( !json["OldPrice"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[OldPrice].asInt() error\n")));
		return; 
	}
	COM_ADGiftBag data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	data.price_ = json["Price"].asInt();
	data.oldprice_ = json["OldPrice"].asInt();

	for(int32 i=0; i<json["Rewards"].size(); ++i){
		if(!json["Rewards"][i]["ItemId"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[Rewards][i][ItemId].asInt() error\n")));
			return; 
		}
		if(!json["Rewards"][i]["ItemStack"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmMinGiftbag json[Rewards][i][ItemStack].asInt() error\n")));
			return; 
		}
		COM_GiftItem item;
		item.itemId_ = json["Rewards"][i]["ItemId"].asInt();
		item.itemNum_ = json["Rewards"][i]["ItemStack"].asInt();
		if(item.itemId_ == 0)
			continue;
		data.itemdata_.push_back(item);
	}

	WorldHandler::instance()->setMinGiftBagActivity(data);
	result(0,"Reset MinGiftBagActivity success");
}

void ClientHandler::gmZhuanpan(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_ZhuanpanData data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	for(int32 i=0; i<json["Contents"].size(); ++i){
		if(!json["Contents"][i]["ZhuanpanID"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[Contents][i][ZhuanpanID].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["ItemId"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[Contents][i][ItemId].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["ItemStack"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[Contents][i][ItemStack].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["Probability"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[Contents][i][Probability].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["DailyOutput"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmZhuanpan json[Contents][i][DailyOutput].asInt() error\n")));
			return; 
		}
		COM_ZhuanpanContent content;
		content.id_	= json["Contents"][i]["ZhuanpanID"].asInt();
		content.item_ = json["Contents"][i]["ItemId"].asInt();
		content.itemNum_ = json["Contents"][i]["ItemStack"].asInt();
		content.rate_ = json["Contents"][i]["Probability"].asInt();
		content.maxdrop_ = json["Contents"][i]["DailyOutput"].asInt();
		if(content.id_ == 0)
			continue;
		data.contents_.push_back(content);
	}
	
	WorldHandler::instance()->setZhuanpanActivity(data);
	result(0,"Reset MinGiftBagActivity success");
}
void ClientHandler::gmIntegralshop(Json::Value& json){
	if( !json["OpenTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[OpenTime].asInt() error\n")));
		return; 
	}
	if( !json["CloseTime"].isInt()){
		ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[CloseTime].asInt() error\n")));
		return; 
	}
	COM_IntegralData data;
	data.sinceStamp_ = json["OpenTime"].asInt();
	data.endStamp_ = json["CloseTime"].asInt();

	for(int32 i=0; i<json["Contents"].size(); ++i){
		if(!json["Contents"][i]["ID"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[Contents][i][ID].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["ItemId"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[Contents][i][ItemId].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["Times"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[Contents][i][Times].asInt() error\n")));
			return; 
		}
		if(!json["Contents"][i]["Cost"].isInt()){
			ACE_DEBUG((LM_INFO,ACE_TEXT("gmIntegralshop json[Contents][i][Cost].asInt() error\n")));
			return; 
		}
	
		COM_IntegralContent content;
		content.id_	= json["Contents"][i]["ID"].asInt();
		content.itemid_ = json["Contents"][i]["ItemId"].asInt();
		content.times_ = json["Contents"][i]["Times"].asInt();
		content.cost_ = json["Contents"][i]["Cost"].asInt();
		if(content.itemid_ == 0)
			continue;
		data.contents_.push_back(content);
	}

	WorldHandler::instance()->setIntegralshop(data);
	result(0,"Reset MinGiftBagActivity success");
}
