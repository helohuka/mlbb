/************************************************************************/
/**
 * @file	skilltable.h
 * @date	2015-3-2015/03/05 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

#ifndef __GUILD_TABLE_H__
#define __GUILD_TABLE_H__

#include "config.h"

class GuildShopItemData
{
public:
	S32 id_;
	S32 itemId_;
	S32 itemStack_;
	S32 price_;
	S32 needLevel_;
	S32 timeLimit_;

public:
	static bool load(char const *fn);
	static bool check();
	static const GuildShopItemData* getShopItem(S32 tableId){
		return data_[tableId];
	}
	static void randomItems(std::vector<COM_GuildShopItem>& items,int32 lev);
public:
	static std::map<S32 ,GuildShopItemData*> data_;
	static std::map<S32 ,std::vector<GuildShopItemData*> > cache_;
};

class GuildBuildingData{
public:
	S32 id_;
	S32 level_;
	S32 needMoney_;
	S32 number_;
public:
	static bool load(char const *fn);
	static GuildBuildingData const * getGuildBuidingData(GuildBuildingType gbt,int level);
	static std::vector< std::vector<GuildBuildingData*> > cache_;
};

class GuildBlessingData{
public:
	S32 id_;
	S32 skId_;

	static bool load(char const *fn);
	static bool check();
	static std::vector<GuildBlessingData> cache_;
};

#endif