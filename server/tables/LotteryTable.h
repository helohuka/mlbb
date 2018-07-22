#ifndef __LOTTERYTABLE_H__
#define __LOTTERYTABLE_H__

#include "config.h"

class LotteryTable
{
public:
	struct LotteryCore
	{
		U32		id_;
		U32		lotteryID_;
		U32		dropId_;
		U32		rewardLv_;
		U32		rate_;
	};

public:
	static bool load(char const *fn);
	static bool check();

	static LotteryCore const * getLottery(U32 itemId, U32 ranking);
	static LotteryCore const * getLotteryByCouponId(U32 itemId);
	static U32 randLottery(U32 itemId);
public:
	static std::map<U32, LotteryCore* > data_;
};

#endif