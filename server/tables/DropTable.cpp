#include "DropTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
std::map< S32 , DropTable::Drop* >  DropTable::data_;

bool
DropTable::load(char const *fn)
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
	
	clear();

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"DropID");

		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Drop has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		Drop* pCore = new Drop;
		pCore->dropID_		= id;
		pCore->exp_			= csv.get_int(row,"exp");
		pCore->money_		= csv.get_int(row,"money");
		pCore->diamond_		= csv.get_int(row,"diamond");

		DropItem item1;
		item1.itemId_		= csv.get_int(row,"item-1");
		item1.itemNum_		= csv.get_int(row,"item-num-1");
		item1.probType_		= csv.get_int(row,"pro-type-1");
		item1.prob_			= csv.get_int(row,"probability-1");

		DropItem item2;
		item2.itemId_		= csv.get_int(row,"item-2");
		item2.itemNum_		= csv.get_int(row,"item-num-2");
		item2.probType_		= csv.get_int(row,"pro-type-2");
		item2.prob_			= csv.get_int(row,"probability-2");

		DropItem item3;
		item3.itemId_		= csv.get_int(row,"item-3");
		item3.itemNum_		= csv.get_int(row,"item-num-3");
		item3.probType_		= csv.get_int(row,"pro-type-3");
		item3.prob_			= csv.get_int(row,"probability-3");

		DropItem item4;
		item4.itemId_		= csv.get_int(row,"item-4");
		item4.itemNum_		= csv.get_int(row,"item-num-4");
		item4.probType_		= csv.get_int(row,"pro-type-4");
		item4.prob_			= csv.get_int(row,"probability-4");

		DropItem item6;
		item6.itemId_		= csv.get_int(row,"item-6");
		item6.itemNum_		= csv.get_int(row,"item-num-6");
		item6.probType_		= csv.get_int(row,"pro-type-6");
		item6.prob_			= csv.get_int(row,"probability-6");

		DropItem item5;
		item5.itemId_		= csv.get_int(row,"item-5");
		item5.itemNum_		= csv.get_int(row,"item-num-5");
		item5.probType_		= csv.get_int(row,"pro-type-5");
		item5.prob_			= csv.get_int(row,"probability-5");

		pCore->items_.push_back(item1);
		pCore->items_.push_back(item2);
		pCore->items_.push_back(item3);
		pCore->items_.push_back(item4);
		pCore->items_.push_back(item5);
		pCore->items_.push_back(item6);


		data_[id] = pCore;
	}

	return true;
}

void 
DropTable::clear(){
	for(std::map< S32 , Drop* >::iterator i = data_.begin(),e = data_.end(); i!=e; ++i){
		if(i->second){
			delete i->second;
		}
	}

	data_.clear();
}

bool
DropTable::check()
{
	for(std::map< S32 , Drop* >::iterator i = data_.begin(),e = data_.end(); i!=e; ++i){
		Drop* dp = i->second;
		SRV_ASSERT(dp);
		for(size_t k=0;k<dp->items_.size(); ++k){
			if(dp->items_[k].itemId_ == 0){continue;}
			else if(dp->items_[k].probType_ == 1){}	//万分比
			else if(dp->items_[k].probType_ == 2){}	//万分比
			else { ACE_DEBUG((LM_ERROR,"DropTable Prob is undefine id(%d) item(%d) \n ",i->first,k)); return false;}
		
			SRV_ASSERT(ItemTable::getItemById(dp->items_[k].itemId_));
		}
	}
	return true;
}

DropTable::Drop const *
DropTable::getDropBaseById(U32 id)
{
	return data_[id];
}

DropTable::Drop const *
DropTable::getDropById(U32 dropId)
{
	const Drop* pdate = getDropBaseById(dropId);
	if(pdate == NULL)
		return NULL;
	static Drop core_;
	core_.reset();
	Drop* core = &core_;
	core->exp_ = pdate->exp_;
	core->money_ = pdate->money_;
	core->diamond_ = pdate->diamond_;
	if(pdate->items_.empty())
		return core;
	for (size_t i = 0; i < pdate->items_.size(); ++i)
	{
		if(pdate->items_[i].itemId_ == 0)
			continue;

		if(pdate->items_[i].probType_ == 1)	//万分比
		{
			enum
			{
				RATIO = 10000
			};

			float ratio = (float)UtlMath::randN(RATIO);

			if(ratio <= pdate->items_[i].prob_)
			{
				core->items_.push_back(pdate->items_[i]);
			}
		}
		else if (pdate->items_[i].probType_ == 2)  //权重
		{
			std::vector<std::pair<int ,int> > tmp;

			for (size_t k = 0; k < pdate->items_.size(); ++k)
			{
				std::pair<int ,int> pool;
				pool = std::make_pair(k,pdate->items_[k].prob_);
				tmp.push_back(pool);
			}

			U32 index = UtlMath::randWeight(tmp);
			core->items_.push_back(pdate->items_[index]);
			break;
		}
		else
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Drop TABEL ITEM PRO-TYPE ERROR[%D]\n"),i));
			return NULL;
		}
	}

	return core;
}