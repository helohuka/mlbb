#ifndef __BATTLEDATA_H__
#define __BATTLEDATA_H__
#include "config.h"
class BattleData
{
public:
	
	U32				battleId_;
	BattleType		battleType_;
	std::vector<std::vector<std::string> > upGroup_;
	std::vector<std::vector<std::string> > downGroup_;
	
	bool checkInside();
	bool getUpMonsters(uint32 waveIndex,std::vector<std::string>& monsters)const;
	bool getDownMonsters(uint32 waveIndex,std::vector<std::string>& monsters)const;
public:
	static void clear();
	static bool load(const char* fn);
	static bool check();

	
	
	static std::vector<std::string> parseMonsters(std::string strMonsters);

	static BattleData const* getBattleDataById(S32 battleId);
	
	static std::vector<BattleData*>	data_;
};

#endif