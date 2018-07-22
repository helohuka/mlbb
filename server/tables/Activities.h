/************************************************************************/
/**
 * @file	Activities.h
 * @date	2016-1-2016/01/09 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * @desc	活动相关表格
 * */
/************************************************************************/
#include "config.h"
class PetActivity{
public:
	S32			petActId_;				//ID
	std::vector<S32> openWeekdays_;		//开启时间
	std::vector<S32> battleIds_;		//战斗ID
	std::vector<S32> conditionLevels_;	//限制等级
	
	bool isOpen(S32 weekday)const;
	bool condition(S32 level, S32 battleId)const;
	bool isMyBattle(S32 battleId){
		return std::find(battleIds_.begin(),battleIds_.end(),battleId) != battleIds_.end();
	}
	typedef std::vector<PetActivity*> PetActivityContainer;
	
	static PetActivityContainer cache_;
	
	static bool load(const char* fn);
	static void clear();
	static bool check();
	static const PetActivity* getPetActivityById(S32 id);
	static const PetActivity* getPetActivityByBattleId(S32 battleId);
	
};

//--在线活动

class OnlineTimeClass{
public:
	struct onlinetimedata{
		U32		index_;
		float	targettime_;
		std::vector<U32>	rewards_;
		std::vector<U32>	reNum_;
	};
	static bool load(const char* fn);
	static bool check();
	static const onlinetimedata* getonlinereward(U32 index);
	static U32 getMaxIndex();
public:
	static std::vector<onlinetimedata> data_;
};

class GrowFundTable{
public:
	struct GrowFundData{
		U32		level_;
		U32		item_;
		U32		itemNum_;
	};
	static bool load(const char* fn);
	static bool check();
	static const GrowFundData* getDataByLevel(U32 level);
public:
	static std::vector<GrowFundData*> data_;
};