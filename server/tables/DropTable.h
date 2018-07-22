#ifndef __DROP_TABLE_H__
#define __DROP_TABLE_H__

#include "config.h"

class DropTable
{
public:
	struct DropItem
	{
		U32		itemId_;
		U32		itemNum_;
		U32		probType_;
		U32		prob_;
	};

	struct Drop
	{
		Drop():exp_(0),money_(0),diamond_(0){}
		void reset(){
			exp_ = 0;
			money_ = 0;
			diamond_  = 0;
			items_.clear();
		}
		U32						dropID_;
		U32						exp_;
		U32						money_;
		U32						diamond_;
		std::vector<DropItem>	items_;
	};

public:
	static bool load(char const *fn);
	static void clear();
	static bool check();
	static Drop const * getDropBaseById(U32 id);
	static Drop const* getDropById(U32 dropId);

public:
	static std::map< S32 , Drop* >	data_;
};

#endif