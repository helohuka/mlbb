#ifndef __DAILY_REWORD_H__
#define __DAILY_REWORD_H__

class DailyReward
{
public:
	
	static bool load(char const *fn);
	static bool check();
	static const std::vector<S32>*  getMonthRewards(S32 month) 
	{
		if(month < 1 || month > 12) return NULL;
		return &monthReward_[month];
	}
	static std::vector< std::vector<S32> > monthReward_;
};


class DiamondsConfig{
public:
	struct Config{
		U32 configId_;
		DiamondConfigClass classType_; //购买类型，
		DiamondConfigType type_;	   //刷新类型
		S32 val0_;                 
		S32 val1_;
		S32 diam_;
	};

	static bool load(char const *fn);
	static const Config* getCondig(DiamondConfigClass clazz, int times);
	static std::vector< std::vector<Config> > configs_;
};

#endif