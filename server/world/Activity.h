/************************************************************************/
/**
 * @file	Activity.h
 * @date	2015-8-2015/08/06 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * @desc    活动系统
 * */
/************************************************************************/

#ifndef __ACTIVITY_H__
#define __ACTIVITY_H__
#include "config.h"
#include "player.h"
#include "timer.h"

class Activity
{
public:
	struct Record
	{
		U32 actId_;
		U32 counter_;		//MAX活动次数
		U32 reward_;		//MAX活跃度
	};
	
	static bool load(const char* fn);
	static bool loadrewards(const char* fn);
public:
	static void update(Player* player, U32 actId, U32 counter);
	static void requestReward(Player* player, U32 index);
	static bool isReward(COM_ActivityTable& table, U32 index);
	inline static const Record* getReward(U32 actId){
		return records_[actId];
	} 
	static void sycnallActData(SGE_SysActivity data);
	static void sycnactivityfromdb(U32 playerGuid, COM_ActivityTable& data);
public:
	static std::map<U32,Record*> records_;
	static std::vector<std::pair<U32,U32> > rewards_;
};

class DayliActivity{
public:
	static bool status_[ACT_Max];
	static void reqeust(Player* p);

	static void mushroomOpen(S32 totalSize);
	static void mushroomRefresh(S32 totalSize);
	static void mushroomClose();
	
	static void xijiOpen(S32 totalSize);
	static void xijiRefresh(S32 totalSize);
	static void xijiClose();

	static void alonepkOpen(S32 totalSize);
	static void alonepkRefresh(S32 totalSize);
	static void alonepkClose();
	static void teampkOpen(S32 totalSize);
	static void teampkRefresh(S32 totalSize);
	static void teampkClose();
	static void examOpen();
	static void examClose();
	static void warriorOpen();
	static void warriorClose();
	static void petbattleopen();	//宠物神殿
	static void petbattleclose();
	static void guildbattleopen();	//家族战
	static void guildbattleclose();
};
//////////////////////////////////转盘////////////////////////////////////////
class ZhuanpanTimer : public GameTimer{
public:
	ZhuanpanTimer(int64 start,int64 stop):GameTimer(start,stop){};
	bool start();
	bool stop();
};

class Zhuanpan{
public:
	static bool load(const char* fn);
	static bool check();
	static const COM_ZhuanpanContent* getzhuanpanDataById(U32 id);
	static void open(int64 start, int64 stop);
	static void open();
	static void close();
	static void request(Player* p);
	static void update(COM_ZhuanpanData& data);

	static COM_ZhuanpanData data_;
	static bool isopen_;
public:
	static void passZeroHour();
	static void initZhuanpan();
	static void randZhuanpan(std::string playerName,U32 counter);
	static bool checkpond(U32 zhuanpanId);
	static void calcPond();
	static bool israrity(U32 zhuanpanId);
	static void saveRarityRecord(std::string playerName, U32 zhuanpanId);
	static std::vector<U32>				pond_;			//转盘池		
	static std::vector< std::pair<std::string,U32> >	record_;		//中奖记录
};


//-------------------------------------------翻牌------------------------------------------------

class ReversalCardTimer : public GameTimer{
public:
	ReversalCardTimer(int64 start,int64 stop):GameTimer(start,stop){};
	bool start();
	bool stop();
};

class ReversalCard{
public:
	struct Cardsper{
		U32		time_;		//次数
		U32		high_;		//概率
		U32		middle_;
		U32		low_;
		U32		cost_;		//花费
	};
	struct CardReward{
		U32		id_;
		U32		itemid_;
		U32		itemnum_;
		U32		type_;			//属于哪个奖池
	};
public:
	static bool loadcardsper(const char* fn);
	static bool loadcardreward(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);

	static const Cardsper* getcardsperbytime(U32 utime);
	static const CardReward* getrewardhigh(Player* p);
	static const CardReward* getrewardmiddle(Player* p);
	static const CardReward* getrewardlow(Player* p);
public:
	static bool isOpen_;
	static uint64 sinceStamp_;
	static uint64 endStamp_;
	static std::vector<Cardsper*>	sper_;
	static std::vector<CardReward*>	high_;
	static std::vector<CardReward*>	middle_;
	static std::vector<CardReward*> low_;
};

//----------------------------------------------------------------------------------------------

class FestivalTimer : public GameTimer{
public:
	FestivalTimer(int64 start,int64 stop):GameTimer(start,stop){};
	bool start();
	bool stop();
};

//累计登录
class Festival{							
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void update(COM_ADLoginTotal& date);
	static bool isOpen_;
	static COM_ADLoginTotal data_;
};

class RechargeTotalTimer : public GameTimer{
public:
	RechargeTotalTimer(int64 start,int64 stop):GameTimer(start,stop){};
	bool start();
	bool stop();
};

//累计充值
class RechargeTotal{
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start1, int64 stop1);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void requestSelf(Player* p);
	static void update(COM_ADChargeTotal& date);
	static bool isOpen_;
	static COM_ADChargeTotal data_;
};

class DiscountStoreTimer : public GameTimer{
public:
	DiscountStoreTimer(int64 st,int64 sp):GameTimer(st,sp){};
	bool start();
	bool stop();
};

class DiscountStore{
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void requestSelf(Player* p);
	static void update(COM_ADDiscountStore& date);
	static bool isOpen_;
	static COM_ADDiscountStore data_;
};

class RechargeSingleTimer : public GameTimer{
public:
	RechargeSingleTimer(int64 st,int64 sp):GameTimer(st,sp){};
	bool start();
	bool stop();
};

//单笔充值
class RechargeSingle{
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void requestSelf(Player* p);
	static void update(COM_ADChargeEvery& date);

	static bool isOpen_;
	static COM_ADChargeEvery data_;
};

//----------------------------------------------------------------------------------------------------------------

class HotShopTimer : public GameTimer{
public:
	HotShopTimer(int64 st,int64 sp):GameTimer(st,sp){};
	bool start();
	bool stop();
};

class HotShop{
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void update(COM_ADHotRole& date);

	static bool isOpen_;
	static COM_ADHotRole data_;
};

//----------------------------------------------------------------------------------------------------------------------

class SevenDayTable{
public:
	struct SevenDay{
		U32					day_;
		U32					quest_;
		AchievementType		type_;
		U32					target_;
		std::vector<std::pair<S32,S32> > reward_;
	};

public:
	static bool load(const char* fn);
	static bool check();

	static const SevenDay* get(U32 quest);

	static std::vector<SevenDay*>	data_;
};

//----------------------------------------------------------------------------------------------------------------------

class EmployeeActivityTotalTimer : public GameTimer{
public:
	EmployeeActivityTotalTimer(int64 st, int64 sp):GameTimer(st,sp){};
	bool start();
	bool stop();
};

class EmployeeActivityTotal{
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void update(COM_ADEmployeeTotal& date);
	static bool isOpen_;
	static COM_ADEmployeeTotal data_;
};

///个人累计充值常驻版
class MySelfRecharge
{
public:
	static bool load(const char* fn);
	static bool check();
	static void requestSelf(Player* p);
	static COM_ADChargeTotal data_;
};

///等级礼包
class LevelGift
{
public:
	struct LevelGiftContent{
		U32					level_;
		std::vector<std::pair<S32,S32> > reward_;
	};
	static bool load(const char* fn);
	static bool check();
	static const LevelGiftContent* get(U32 level);
public:
	static std::vector<LevelGiftContent*> data_;
};

//小额礼包

class MinGiftTimer : public GameTimer{
public:
	MinGiftTimer(int64 st,int64 sp):GameTimer(st,sp){};
	bool start();
	bool stop();
};

class MinGift
{
public:
	static void init();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void update(COM_ADGiftBag& date);
	static bool isOpen_;
	static COM_ADGiftBag data_;
};
//积分商店
class IntegralTimer : public GameTimer{
public:
	IntegralTimer(int64 start,int64 stop):GameTimer(start,stop){};
	bool start();
	bool stop();
};

//累计登录
class IntegralShop{							
public:
	static bool load(const char* fn);
	static bool check();
	static void open(int64 start, int64 stop);
	static void open();
	static void close(); 
	static void request(Player* p);
	static void update(COM_IntegralData& date);
	static bool isOpen_;
	static COM_IntegralData data_;
};

///历程礼包
class CourseGiftTable
{
public:
	struct CourseGiftContent{
		U32					id_;
		U32					level_;
		U32					timeout_;
		std::vector<std::pair<S32,S32> > reward_;
	};
	static bool load(const char* fn);
	static bool check();
	static const CourseGiftContent* getbylevel(U32 level);
	static const CourseGiftContent* getbyshopid(U32 shopid);
public:
	static std::vector<CourseGiftContent*> data_;
};

#endif