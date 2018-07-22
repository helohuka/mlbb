
#ifndef __TITLE_TABLE_H__
#define __TITLE_TABLE_H__

#include "config.h"

class TitleTable
{
public:

	struct TitleData
	{
		S32 titleId_;
		S32 minR_;			//范围下限
		S32	maxR_;			//范围上限
	};

public:
	static bool load(char const *fn);
	static bool check();
	static bool isReputationTitle(S32 value);
	static S32 findTitleByValue(S32 value);
	static bool isTitle(S32 title);
public:
	static std::vector<TitleData* >  data_;
};

#endif