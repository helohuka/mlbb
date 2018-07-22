#ifndef __ACHIEVEMENT_H__
#define __ACHIEVEMENT_H__

#include "config.h"

class AchievementTable
{
public:

	struct AchievementData
	{
		S32					achId_;
		AchievementType		type_;
		S32					titleid_;
		S32					value_;			//成就达成条件
		S32					dropId_;		//达成奖励itemId
	};

public:
	static bool load(char const *fn);
	static bool check();
	static AchievementData const * getAchievementById(U32 id);
	static void geAchDataByType(AchievementType achtype,std::vector<AchievementData*> ach);
	static U32 getAchievementNum();
public:
	static std::map<S32,AchievementData* >  data_;
};

#endif