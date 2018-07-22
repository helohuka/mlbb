#include "employeeoutput.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
#include "player.h"
#include "loghandler.h"

std::vector<Employeeoutput::QualityWeight*> Employeeoutput::qwList_;
std::map<QualityColor,std::vector<Employeeoutput::EmployeeoutputData*> > Employeeoutput::data_;

bool
Employeeoutput::qualityload(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false){
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		QualityWeight * pCore = NEW_MEM(QualityWeight);
		std::string stt = csv.get_string(row,"Quality");
		int stte = ENUM(QualityColor).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("QualityColor error in row %d\n"),row));
			SRV_ASSERT(0);	
		}
		pCore->color_ = (QualityColor)stte;
		pCore->goldWeight_		= csv.get_int(row,"GoldWeight");
		pCore->diamondWeight_	= csv.get_int(row,"DiamondWeight");

		qwList_.push_back(pCore);
	}

	return true;
}

bool
Employeeoutput::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false){
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		EmployeeoutputData* pCore = NEW_MEM(EmployeeoutputData);
		pCore->id_		= csv.get_int(row,"ID");
		std::string stt = csv.get_string(row,"Quality");
		int stte = ENUM(QualityColor).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("QualityColor error in row %d\n"),row));
			SRV_ASSERT(0);	
		}
		pCore->color_			= (QualityColor)stte;
		pCore->goldWeight_		= csv.get_int(row,"GoldWeight");
		pCore->diamondWeight_	= csv.get_int(row,"DiamondWeight");

		std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find(pCore->color_);

		if(itr != data_.end())
		{
			itr->second.push_back(pCore);
		}
		else
		{
			std::vector<EmployeeoutputData*> temp;
			temp.push_back(pCore);
			data_[pCore->color_] = temp;
		}
	}

	return true;
}

bool
Employeeoutput::check()
{
	std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.begin();
	while(itr != data_.end())
	{
		std::vector<EmployeeoutputData*> temp = itr->second;

		for(size_t i = 0; i < temp.size(); ++i)
		{
			SRV_ASSERT(ItemTable::getItemById(temp[i]->id_));
		}
		++itr;
	}

	if(qwList_.size() < 4)
		return false;

	return true;
}

//--------------------------------------QualityWeight------------------------------------------------

QualityColor
Employeeoutput::randQualityGold()
{
	std::vector< std::pair<S32,S32> > pairs;
	for (size_t i=0; i<qwList_.size(); ++i){
		pairs.push_back(std::pair<S32,S32>(i,qwList_[i]->goldWeight_));
	}
	S32 idx = UtlMath::randWeight(pairs);
	return qwList_[idx]->color_;
}

QualityColor
Employeeoutput::randQualityDiamond()
{
	std::vector< std::pair<S32,S32> > pairs;
	for (size_t i=0; i<qwList_.size(); ++i){
		pairs.push_back(std::pair<S32,S32>(i,qwList_[i]->diamondWeight_));
	}
	S32 idx = UtlMath::randWeight(pairs);
	return qwList_[idx]->color_;
}

U32
Employeeoutput::randOther()
{
	QualityColor color = randQualityDiamond();
	while(color == QC_Purple)
	{
		color = randQualityDiamond();
	}

	std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find(color);
	if(itr == data_.end())
		return 0;
	std::vector<EmployeeoutputData*> temp = itr->second;
	std::vector< std::pair<S32,S32> > pairs;
	for (size_t i=0; i<temp.size(); ++i){
		pairs.push_back(std::pair<S32,S32>(i,temp[i]->diamondWeight_));
	}
	S32 idx = UtlMath::randWeight(pairs);
	return temp[idx]->id_;
}

U32
Employeeoutput::randPurple()
{
	std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find((QualityColor)Global::get<int>(C_EmployeeRare));
	if(itr == data_.end())
		return 0;
	std::vector<EmployeeoutputData*> temp = itr->second;
	std::vector< std::pair<S32,S32> > pairs;
	for (size_t i=0; i<temp.size(); ++i){
		pairs.push_back(std::pair<S32,S32>(i,temp[i]->diamondWeight_));
	}
	S32 idx = UtlMath::randWeight(pairs);
	return temp[idx]->id_;
}

//---------------------------------------------------Output----------------------------------------------------

U32
Employeeoutput::randGoldOnce()
{
	QualityColor color = randQualityGold();
	std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find(color);
	if(itr == data_.end())
		return 0;
	std::vector<EmployeeoutputData*> temp = itr->second;
	std::vector< std::pair<S32,S32> > pairs;
	for (size_t i=0; i<temp.size(); ++i){
		pairs.push_back(std::pair<S32,S32>(i,temp[i]->goldWeight_));
	}
	S32 idx = UtlMath::randWeight(pairs);
	return temp[idx]->id_;
}

U32
Employeeoutput::randDiamondOnce(Player* player)
{
	U32 itemid = 0;

	if( (player->employeeonecount_ - player->employeelasttime_) < 20)
	{
		QualityColor color = randQualityDiamond();
		std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find(color);
		if(itr == data_.end())
			return 0;
		std::vector<EmployeeoutputData*> temp = itr->second;
		std::vector< std::pair<S32,S32> > pairs;
		for (size_t i=0; i<temp.size(); ++i){
			pairs.push_back(std::pair<S32,S32>(i,temp[i]->diamondWeight_));
		}
		S32 idx = UtlMath::randWeight(pairs);
		itemid = temp[idx]->id_;

		if(color == (QualityColor)Global::get<int>(C_EmployeeRare))
			player->employeelasttime_ = player->employeeonecount_;
	}
	else
	{
		std::map<QualityColor,std::vector<EmployeeoutputData*> >::iterator itr = data_.find((QualityColor)Global::get<int>(C_EmployeeRare));
		if(itr == data_.end())
			return 0;
		std::vector<EmployeeoutputData*> temp = itr->second;
		std::vector< std::pair<S32,S32> > pairs;
		for (size_t i=0; i<temp.size(); ++i){
			pairs.push_back(std::pair<S32,S32>(i,temp[i]->diamondWeight_));
		}
		S32 idx = UtlMath::randWeight(pairs);
		itemid = temp[idx]->id_;

		player->employeelasttime_ = player->employeeonecount_;
	}
	++player->employeeonecount_;

	return itemid;
}

void
Employeeoutput::randDiamondTen(Player* player,std::vector<U32> &out)
{
	enum{ PurpleNum = 2, RollSize = 10 ,RollVar = 10 ,RollMax = 5 };
	++player->employeetencount_;
	for(S32 i=0;i<RollMax&&out.size()!=RollSize;++i){
		if(player->employeetencount_%RollVar != 0)
		{
			U32 itemid = randPurple();
			if(itemid != 0)
			{
				out.push_back(itemid);
			}
			for (size_t i = 0; i < RollSize - 1; ++i)
			{
				itemid = randOther();
				if(itemid == 0)
					continue;
				if(out.size() == RollSize)
					return;
				out.push_back(itemid);
			}
		}
		else
		{
			U32 itemid = 0;
			for (size_t i = 0; i < PurpleNum; ++i)
			{
				itemid = randPurple();
				if(itemid == 0)
				{
					ACE_DEBUG((LM_ERROR,"PurpleNum Employeeoutput::randDiamondTen() == 0\n"));
					continue;
				}
				if(out.size() == RollSize)
					return;
				out.push_back(itemid);
			}

			for (size_t i = 0; i <RollSize - PurpleNum; ++i)
			{
				itemid = randOther();
				if(itemid == 0)
				{
					ACE_DEBUG((LM_ERROR,"RollSize Employeeoutput::randDiamondTen() == 0\n"));
					continue;
				}
				if(out.size() == RollSize)
					return;
				out.push_back(itemid);
			}
		}
	}
}

// -------------------------------------------- [7/19/2016 lwh]

void Employeeoutput::rollGreen(Player *player)
{
	if(player->employees_.size() >= EMPLOYEE_MAXNUM)
	{
		CALL_CLIENT(player,errorno(EN_EmployeeIsFull));
		return;
	}

	if(player->greenBoxTimes_> 0)
	{
		float money =  player->getProp(PT_Money);
		if(money < Global::get<int>(C_BoxGreenSpend))
			return;
		
		player->addMoney(-Global::get<int>(C_BoxGreenSpend));
		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 2;
		track.money_ = -Global::get<int>(C_BoxGreenSpend);
		LogHandler::instance()->playerTrack(track);
	}
	else
	{
		if(player->greenBoxFreeNum_ <= 0){
			player->addMoney(-Global::get<int>(C_BoxGreenSpend));
			SGE_LogProduceTrack track;
			track.playerId_ = player->getGUID();
			track.playerName_ = player->getNameC();
			track.from_ = 2;
			track.money_ = -Global::get<int>(C_BoxGreenSpend);
			LogHandler::instance()->playerTrack(track);
		}
		else
			player->greenBoxFreeNum_--;
	}

	std::vector<COM_Item> item;

	if(!player->firstRollEmployeeCon_){//1854 【需求】伙伴-伙伴第一次抽取时抽取指定伙伴
		player->firstRollEmployeeCon_ = true;
		COM_Item box;
		box.itemId_ = Global::get<int>(C_FirstRollEmployeeCon);
		box.stack_ = 1;
		item.push_back(box);
		ItemTable::ItemData const* boxItem = ItemTable::getItemById(box.itemId_);
		player->addEmployee(boxItem->employeeId_);
		CALL_CLIENT(player,drawLotteryBoxRep(item));
		if(player->greenBoxTimes_<=0)
		{
			player->resetGreenBoxTime();	
		}
		CALL_CLIENT(player,requestOpenBuyBox(player->greenBoxTimes_,player->blueBoxTimes_,player->greenBoxFreeNum_));
		return;
	}

	U32 itemid = randGoldOnce();
	if(itemid == 0)
	{
		ACE_DEBUG((LM_ERROR,"rollGreen!!! Employeeoutput::randGoldOnce() == 0\n"));
		return;
	}
	//TODO
	COM_Item box;
	box.itemId_ = itemid;
	box.stack_ = 1;
	item.push_back(box);

	for(size_t i =0;i<item.size();i++)
	{
		ItemTable::ItemData const* boxItem  = ItemTable::getItemById(item[i].itemId_);
		if(boxItem->mainType_ == IMT_Employee )
			player->addEmployee(boxItem->employeeId_);
	}

	CALL_CLIENT(player,drawLotteryBoxRep(item));

	if(player->greenBoxTimes_<=0)
	{
		player->resetGreenBoxTime();		
	}
	CALL_CLIENT(player,requestOpenBuyBox(player->greenBoxTimes_,player->blueBoxTimes_,player->greenBoxFreeNum_));
}

void Employeeoutput::rollBlue(Player *player)//钻石
{
	// 
	if(player->employees_.size() >= EMPLOYEE_MAXNUM)
	{
		CALL_CLIENT(player,errorno(EN_EmployeeIsFull));
		return;
	}

	if(player->blueBoxTimes_ > 0)
	{
		if(player->getItemNumByItemId( Global::get<int>(C_BoxBlueSpendItem)) >= Global::get<int>(C_BoxBlueSpend))
			player->delBagItemByItemId(Global::get<int>(C_BoxBlueSpendItem),Global::get<int>(C_BoxBlueSpend),2);
		else if(player->properties_[PT_Diamond] >= Global::get<int>(C_BoxBlueSpendDiamond)){
			S32 diamond = Global::get<int>(C_BoxBlueSpendDiamond);
			if(diamond < 0)
				return;
			player->addDiamond(-diamond);
			SGE_LogProduceTrack track;
			track.playerId_ = player->getGUID();
			track.playerName_ = player->getNameC();
			track.from_ = 2;
			track.diamond_ = -diamond;
			LogHandler::instance()->playerTrack(track);
		}
		else if(player->properties_[PT_MagicCurrency] >= Global::get<int>(C_BoxBlueSpendDiamond)){
			S32 diamond = Global::get<int>(C_BoxBlueSpendDiamond);
			if(diamond < 0)
				return;
			player->addMagicCurrency(-diamond);
			SGE_LogProduceTrack track;
			track.playerId_ = player->getGUID();
			track.playerName_ = player->getNameC();
			track.from_ = 2;
			track.magic_ = -diamond;
			LogHandler::instance()->playerTrack(track);
		}
		else
			return;
			
	}

	std::vector<COM_Item> item;

	if(!player->firstRollEmployeeDia_){//1854 【需求】伙伴-伙伴第一次抽取时抽取指定伙伴
		player->firstRollEmployeeDia_ = true;
		COM_Item box;
		box.itemId_ = Global::get<int>(C_FirstRollEmployeeDia);
		box.stack_ = 1;
		ItemTable::ItemData const* boxItem = ItemTable::getItemById(box.itemId_);
		player->addEmployee(boxItem->employeeId_);
		item.push_back(box);
		CALL_CLIENT(player,drawLotteryBoxRep(item));
		if(player->blueBoxTimes_<=0)
		{
			player->resetBlueBoxTime();	
		}
		CALL_CLIENT(player,requestOpenBuyBox(player->greenBoxTimes_,player->blueBoxTimes_,player->greenBoxFreeNum_));
		return;
	}

	U32 itemid = randDiamondOnce(player);
	if(itemid == 0)
	{
		ACE_DEBUG((LM_ERROR,"rollBlue!!! Employeeoutput::randDiamondOnce() == 0\n"));
		return;
	}
	//TODO
	COM_Item box;
	box.itemId_ = itemid;
	box.stack_ = 1;
	item.push_back(box);

	for(size_t i =0;i<item.size();i++)
	{
		ItemTable::ItemData const* boxItem  = ItemTable::getItemById(item[i].itemId_);
		if(boxItem->mainType_ == IMT_Employee )
			player->addEmployee(boxItem->employeeId_);
		//运营活动顶级招募
		player->updateEmployeeActivity(true);
	}

	CALL_CLIENT(player,drawLotteryBoxRep(item));

	if(player->blueBoxTimes_<=0)
	{
		player->resetBlueBoxTime();	
	}
	CALL_CLIENT(player,requestOpenBuyBox(player->greenBoxTimes_,player->blueBoxTimes_,player->greenBoxFreeNum_));
}

void Employeeoutput::rollGold(Player *player)//花费砖石
{
	// 
	enum {
		ADD_NUM = 10 ,
	};
	if(player->employees_.size() + ADD_NUM > EMPLOYEE_MAXNUM)
	{
		CALL_CLIENT(player,errorno(EN_EmployeeIsFull));
		return;
	}

	if(player->getItemNumByItemId( Global::get<int>(C_BoxBlueSpendItem)) >= Global::get<int>(C_BoxGoldSpend))
		player->delBagItemByItemId(Global::get<int>(C_BoxBlueSpendItem),Global::get<int>(C_BoxGoldSpend),2);
	else if(player->properties_[PT_Diamond] >= Global::get<int>(C_BoxGoldSpendDiamond)){
		S32 diamond = Global::get<int>(C_BoxGoldSpendDiamond);
		if(diamond < 0)
			return;
		player->addDiamond(-diamond);
		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 2;
		track.diamond_ = -diamond;
		LogHandler::instance()->playerTrack(track);
	}
	else if(player->properties_[PT_MagicCurrency] >= Global::get<int>(C_BoxGoldSpendDiamond)){
		S32 diamond = Global::get<int>(C_BoxGoldSpendDiamond);
		if(diamond < 0)
			return;
		player->addMagicCurrency(-diamond);
		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 2;
		track.magic_ = -diamond;
		LogHandler::instance()->playerTrack(track);
	}
	else
		return;
	
	std::vector<COM_Item> item;
	std::vector<U32> gs;
	randDiamondTen(player,gs);
	for(size_t i=0; i<gs.size(); ++i){
		COM_Item box;
		box.itemId_ = gs[i];
		box.stack_ = 1;
		item.push_back(box);
	}
	//TODO
	if(item.size()<=0)
		return;
	for(size_t i =0;i<item.size();i++)
	{
		ItemTable::ItemData const* boxItem  = ItemTable::getItemById(item[i].itemId_);
		if(boxItem->mainType_ == IMT_Employee )
			player->addEmployee(boxItem->employeeId_);
		//运营活动顶级招募
		player->updateEmployeeActivity(true);
	}

	CALL_CLIENT(player,drawLotteryBoxRep(item));

}

void
Employeeoutput::calcofflinetime(Player* player, float offlinetime)
{
	float greentimecd = player->greenBoxTimes_ - offlinetime;

	if(greentimecd < 0)
		player->greenBoxTimes_ = 0;
	else
		player->greenBoxTimes_ = greentimecd;

	float bluetimecd = player->blueBoxTimes_ - offlinetime;
	if(bluetimecd < 0)
		player->blueBoxTimes_ = 0;
	else
		player->blueBoxTimes_ = bluetimecd;
}
