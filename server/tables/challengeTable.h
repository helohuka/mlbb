#ifndef __CHALLENGETABLE_H__
#define __CHALLENGETABLE_H__
#include "config.h"

class ChallengeTable
{
public:
	struct Core
	{
		S32 id_;
		S32 battleId_;
		S32 senceId_;
	};

public:
	static bool load(char const *fn);
	static bool check();

	static Core const * getDataById(U32 id);
	static Core const * getDataByBattleId(U32 id);
	static bool isHundredSence(U32 senceId);

public:
	static std::vector<Core> data_;
};

#endif