#ifndef __SHOP_H__
#define __SHOP_H__
#include "config.h"
#include "Common.h"
class Shop
{
public:
	struct Record
	{
		S32			id_;
		std::string name_;
		ShopType	type_;
		ShopPayType paytype_;
		S32			pay_;
		S32			itemId_;
		S32			itemNum_;
		std::string idName_;
	};
public:
	
	static void clear();
	static bool load(const char* fn);
	static bool check();

	static const Record* getRecordById(S32 id);
	static const Record* getRecordByName(std::string const& name);
	static std::vector<Record*> records_;
};

#endif