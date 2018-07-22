#include "player.h"
#include "baby.h"
#include "worldserv.h"
#include "itemtable.h"
#include "monstertable.h"
#include "GameEvent.h"

//////////////////////////////////////////////////////////////////////////

void
Player::depositItem(U32 instid)
{
	COM_Item* pItem = getBagItemByInstId(instid);
	if(pItem == NULL)
		return;
	ItemTable::ItemData const * data = ItemTable::getItemById(pItem->itemId_);
	if(NULL == data)
		return;
	if(data->subType_ == IST_PVP || data->mainType_ == IMT_Quest)
		return;
	S32 slot = findItemStorageFirstemptySlot();
	if(slot == -1)
		return;
	if(itemStorage_[slot] != NULL)
		return;

	bagItmes_[pItem->slot_] = NULL;//delBagItemByInstId(instid);
	CALL_CLIENT(this,delBagItem(pItem->slot_)); //通知前端删除 背包道具
	itemStorage_[slot] = pItem;
	itemStorage_[slot]->slot_ = slot;
	
	CALL_CLIENT(this,depositItemOK(*itemStorage_[slot]));
}

void
Player::getoutItem(U32 instid)
{
	COM_Item* pitem = findItemFromStorageByInstId(instid);
	if(pitem == NULL)
		return;
	
	S32 emptySlot = getFirstEmptySlot();
	if(emptySlot  < 0)
	{
		CALL_CLIENT(this,errorno(EN_BagFull));
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	itemStorage_[pitem->slot_] = NULL;
	CALL_CLIENT(this,getoutItemOK(pitem->slot_));
	pitem->slot_ = emptySlot;
	bagItmes_[emptySlot] = pitem;
	CALL_CLIENT(this,addBagItem(*bagItmes_[emptySlot]));
}

bool
Player::depositBaby(U32 instid)
{
	S32 slot = findBabyStorageFirstemptySlot();
	if(slot == -1)
	{
		CALL_CLIENT(this,errorno(EN_BabyStorageFull));
		return false;
	}
	if(babyStorage_[slot]!= NULL)
		return false;
	
	Baby* pbaby = findBaby(instid);
	if(pbaby == NULL || pbaby->isBattle())
		return false;
	if(pbaby->isShow_)
		return false;
	COM_BabyInst *pinst = NEW_MEM(COM_BabyInst);
	pbaby->getBabyInst(*pinst);
	pinst->slot_ = slot;
	babyStorage_[slot] = pinst;

	for(size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->babyId_ == instid)
		{
			Baby* p = babies_[i];
			babies_.erase(babies_.begin() + i);
			DEL_MEM(p);
		}
	}
	DBHandler *dbh = DBHandler::instance();
	SRV_ASSERT(dbh);
	dbh->updateBaby(*pinst);
	
	CALL_CLIENT(this,depositBabyOK(*pinst));

	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = pinst->tableId_;
	GameEvent::procGameEvent(GET_DepositBaby,param,1,getHandleId());

	return true;
}	

bool
Player::depositBaby(COM_BabyInst& inst)
{
	S32 slot = findBabyStorageFirstemptySlot();
	if(slot == -1)
	{
		CALL_CLIENT(this,errorno(EN_BabyStorageFull));
		return false;
	}

	if(babyStorage_[slot]!= NULL)
		return false;

	COM_BabyInst* tInst = NEW_MEM(COM_BabyInst,inst);

	tInst->slot_ = slot;
	babyStorage_[slot] = tInst;
	DBHandler *dbh = DBHandler::instance();
	SRV_ASSERT(dbh);
	dbh->updateBaby(*tInst);

	CALL_CLIENT(this,depositBabyOK(*tInst));

	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(tInst->tableId_);
	if(tmp == NULL)
	{
		return false;
	}
	if(!checkBabyCache(tmp->monsterId_))
	{
		babycache_.push_back(tmp->monsterId_);
		//ACE_DEBUG((LM_INFO,"Player [%d] add baby to storage tableid==[%d] \n",getGUID(),tmp->monsterId_));
	}

	GEParam param[5];
	param[0].type_ = GEP_INT;
	param[0].value_.i = tmp->monsterId_;
	param[1].type_ = GEP_INT;
	param[1].value_.i = tInst->instId_;
	param[2].type_ = GEP_INT;
	param[2].value_.i = tmp->monsterType_;
	param[3].type_ = GEP_INT;
	param[3].value_.i = false;
	param[4].type_ = GEP_INT;
	param[4].value_.i = tmp->race_;
	GameEvent::procGameEvent(GET_AddBaby,param,5,getHandleId());

	return true;
}

void
Player::getoutBaby(U32 instid)
{
	if(babies_.size() >= Global::get<int>(C_BabyMax))
	{
		CALL_CLIENT(this,errorno(EN_BabyFull));
		return;
	}
	COM_BabyInst* pinst = findBabyFormStorageByInstId(instid);
	if(pinst == NULL)
		return;
	DBHandler *dbh = DBHandler::instance();
	SRV_ASSERT(dbh);
	babyStorage_[pinst->slot_] = NULL;
	CALL_CLIENT(this,getoutBabyOK(pinst->slot_));
	pinst->slot_ = -1;
	dbh->updateBaby(*pinst);
	addBaby(*pinst,true);
	DEL_MEM(pinst);
}

void Player::delStorageBaby(U32 instid){
	COM_BabyInst* pinst = findBabyFormStorageByInstId(instid);
	if(pinst == NULL)
		return;
	if(pinst->isLock_)
		return;
	babyStorage_[pinst->slot_] = NULL;
	DBHandler::instance()->deleteBaby(playerName_, instid);
	WorldServ::instance()->delBabyRank(instid);
	CALL_CLIENT(this,delStorageBabyOK(pinst->slot_));
	DEL_MEM(pinst);
}

S32
Player::findItemStorageFirstemptySlot()
{
	for (size_t i = 0;i < itemStorage_.size() && i < itemStorageSize_; ++i)
	{
		if(itemStorage_[i] == NULL)
			return i;
	}

	return -1;
}

S32
Player::findBabyStorageFirstemptySlot()
{
	for (size_t i = 0; i < babyStorage_.size() && i < babyStorageSize_; ++i)
	{
		if(babyStorage_[i] == NULL)
			return i;
	}

	return -1;
}

COM_Item*
Player::findItemFromStorageByInstId(U32 instid)
{
	for (size_t i = 0; i < itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] == NULL)
			continue;
		if(itemStorage_[i]->instId_ == instid)
			return itemStorage_[i];
	}

	return NULL;
}

COM_BabyInst*
Player::findBabyFormStorageByInstId(U32 instid)
{
	for (size_t i = 0; i < babyStorage_.size(); ++i)
	{
		if(babyStorage_[i] == NULL)
			continue;
		if(babyStorage_[i]->instId_ == instid)
			return babyStorage_[i];
	}
	return NULL;
}

void
Player::sortItemStorage()
{	
	for(size_t i=0; i<itemStorage_.size(); ++i)
	{
		COM_Item* c = itemStorage_[i];
		if(NULL == c)
			continue;
		const ItemTable::ItemData* cd = ItemTable::getItemById(c->itemId_);
		if(NULL == cd)
			continue;
		for(size_t k=i+1; k<itemStorage_.size(); ++k)
		{
			COM_Item* l = itemStorage_[k];
			if(NULL == l)
				continue;

			if(c->itemId_ == l->itemId_)
			{
				if(c->stack_ < cd->maxCount_)
				{
					c->stack_ += l->stack_;
					l->stack_ = c->stack_ - cd->maxCount_;
					c->stack_ = c->stack_ >= cd->maxCount_ ? cd->maxCount_ : c->stack_;
				}

				if(l->stack_ <= 0)
				{
					itemStorage_[k] = NULL;
					DEL_MEM(l);
				}
			}
		}
	}

	ItemTable::ItemSortFunction isf;
	std::sort(itemStorage_.begin(),itemStorage_.end(),isf);
	for(size_t i=0; i<itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] != NULL)
			itemStorage_[i]->slot_ = i;
	}

	std::vector<COM_Item> items;
	for (size_t i = 0;i < itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] != NULL)
			items.push_back(*itemStorage_[i]);
	}
	
	CALL_CLIENT(this,sortItemStorageOK(items));
}

void
Player::sortBabyStorage()
{
	if(isSorting_)
		return;
	isSorting_ = true;
	MonsterTable::sortBabyFunction isf;
	std::sort(babyStorage_.begin(),babyStorage_.end(),isf);
	for(size_t i=0; i<babyStorage_.size(); ++i){
		if(babyStorage_[i] != NULL)
			babyStorage_[i]->slot_ = i;
	}
	
	std::vector<U32> babys;
	for (size_t i = 0;i < babyStorage_.size(); ++i){
		if(babyStorage_[i] != NULL)
			babys.push_back(babyStorage_[i]->instId_);
	}

	DBHandler *dbh = DBHandler::instance();
	SRV_ASSERT(dbh);

	std::vector<COM_BabyInst> instbabys;
	for (size_t i=0; i<babyStorage_.size();++i)
	{
		if(babyStorage_[i] == NULL)
			continue;
		instbabys.push_back(*babyStorage_[i]); 
	}

	for (size_t i=0; i<instbabys.size(); ++i)
	{
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("SORT BABY STORAGE TO DB BABYID[%d] BABYTABLE[%d] BABYSLOT[%d] \n"),instbabys[i].instId_,instbabys[i].tableId_,instbabys[i].slot_));
	}

	if(!instbabys.empty())
		dbh->updateBabys(playerName_,instbabys);

	CALL_CLIENT(this,sortBabyStorageOK(babys));
}

void
Player::openGrid(StorageType tp)
{
	U32 gridNum = 0;

	if(tp == ST_Item)
	{
		if(itemStorageSize_ >= Global::get<int>(C_ItemStroageGridMax))
			return;
		itemStorageSize_ += Global::get<int>(C_ItemStroagePageGridNum);
		gridNum = itemStorageSize_;
	}
	else if (tp == ST_Baby)
	{
		if(babyStorageSize_ >= Global::get<int>(C_BabyStroageGridMax))
			return;
		babyStorageSize_ += Global::get<int>(C_BabyStroagePageGridNum);
		gridNum = babyStorageSize_;
	}
	CALL_CLIENT(this,openStorageGrid(tp,gridNum));

	enum {
		ARG0,
		ARG1,
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG0].type_  = GEP_INT;
	param[ARG0].value_.i = tp;
	param[ARG1].type_  = GEP_INT;
	param[ARG1].value_.i = gridNum;
	GameEvent::procGameEvent(GET_ExtendStorage,param,ARG_MAX_,handleId_);
}

void
Player::initItemStorage()
{
	std::vector<COM_Item> items;
	for (size_t i = 0; i < itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] == NULL)
			continue;
		items.push_back(*itemStorage_[i]);
	}

	CALL_CLIENT(this,initItemStorage(itemStorageSize_,items));
}

void
Player::initBabyStorage()
{
	std::vector<COM_BabyInst> babys;
	for (size_t i = 0; i < babyStorage_.size(); ++i)
	{
		if(babyStorage_[i] == NULL)
			continue;
		babys.push_back(*babyStorage_[i]);
	}
	
	CALL_CLIENT(this,initBabyStorage(babyStorageSize_,babys));
}

void 
Player::freeItemStorage()
{
	for (size_t i=0; i<itemStorage_.size(); ++i)
	{
		if(itemStorage_[i]){
			DEL_MEM(itemStorage_[i]);
		}
	}
	itemStorage_.clear();
}

void 
Player::freeBabyStorage()
{
	for (size_t i=0; i<babyStorage_.size(); ++i)
	{
		if(babyStorage_[i]){
			DEL_MEM(babyStorage_[i]);
		}
	}
	babyStorage_.clear();
}