#ifndef __DEBRIS_TABLE_H__
#define __DEBRIS_TABLE_H__

#include "config.h"
class DebrisTable
{
public:

	struct DebrisData
	{
		S32 id_;
		S32 debrisId_;
		S32 needNum_;
		S32 itemNum_;
		S32 itemId_;
		ItemSubType  subType_;
	};


public:
	static bool load(char const *fn);
	static bool check();
	static DebrisData const * getItemById(U32 id);

public:
	static std::map< S32 , DebrisData* >  data_;
};

#endif