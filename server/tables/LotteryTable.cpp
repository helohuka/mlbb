#include "LotteryTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "DropTable.h"

std::map<U32, LotteryTable::LotteryCore* > LotteryTable::data_;

bool
LotteryTable::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		LotteryCore* pCore = new LotteryCore;

		S32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Lottery has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		pCore->id_			= id;
		pCore->lotteryID_	= csv.get_int(row,"LotteryID");
		pCore->dropId_		= csv.get_int(row,"Drop");
		pCore->rate_		= csv.get_int(row,"RewardRate");
		pCore->rewardLv_	= csv.get_int(row,"RewardLv");

		data_[id] = pCore;
	}
	return true;
}

bool
LotteryTable::check()
{
	std::map< U32 , LotteryCore* >::iterator itr = data_.begin();

	while (itr != data_.end())
	{
		LotteryCore* pCore = itr->second;

		if(pCore == NULL)
			return false;

		const DropTable::Drop* drop = DropTable::getDropBaseById(pCore->dropId_);
		if(drop == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("CouponId_ Don't find this DropId[%d] in the item table\n"),pCore->dropId_));
			return false;
		}

		++itr;
	}

	return true;
}

LotteryTable::LotteryCore const*
LotteryTable::getLottery(U32 itemId, U32 ranking)
{
	std::map< U32 , LotteryCore* >::iterator itr = data_.begin();

	while (itr != data_.end())
	{
		LotteryCore* pCore = itr->second;

		if(pCore == NULL)
			return NULL;

		if (pCore->lotteryID_ == itemId && pCore->rewardLv_ == ranking)
		{
			return pCore;
		}

		++itr;
	}

	return NULL;
}

LotteryTable::LotteryCore const*
LotteryTable::getLotteryByCouponId(U32 itemId)
{
	return data_[itemId];
}

//---------------------------------------------------------------------------

U32
LotteryTable::randLottery(U32 itemId)
{
	std::map< U32 , LotteryCore* >::iterator itr = data_.begin();
	std::vector<std::pair<int ,int> > tmp;
	
	for (;itr != data_.end(); ++itr)
	{
		LotteryCore* pCore = itr->second;

		if(pCore == NULL)
		{
			ACE_DEBUG((LM_ERROR, ACE_TEXT("randLottery canot find LotteryCore")));
			SRV_ASSERT(pCore);
		}

		if(pCore->lotteryID_ != itemId)
			continue;
	
		std::pair<int ,int> pool;

		pool = std::make_pair(pCore->rewardLv_,pCore->rate_);

		tmp.push_back(pool);
	}

	U32 index = UtlMath::randWeight(tmp);

	return index;
}