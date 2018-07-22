#ifndef __LIVESKILLTABLE_H__
#define __LIVESKILLTABLE_H__

#include "config.h"

class GatherTable
{
public:
	struct GatherCore
	{
		U32					id_;				//ID
		MineType			type_;
		S32					level_;
		U32					superdrop_;
		U32					dropId_;
		S32					money_;
		S32					addmoney_;
		S32					itemId_;
	};

public:
	static bool load(char const *fn);
	static bool check();
	static std::vector<int32> getItemsByLevel(S32 lev);
	static GatherCore const * getGatherById(S32 id);

public:
	static std::map< S32 , GatherCore* >  gatherData_;
};

class MakeTable
{
public:
	struct MakeCore
	{
		U32					id_;					//可以生产的道具ID，与ITEM表中一致			
		U32					kind_;					//生产需要的技能			
		U32					level_;					//生产需要该技能的等级
		U32					gainSkillExp_;			//生产该物品后获得的技能经验
		PropertyType		costType_;				//进行采集或生产该道具时，需要消耗的资源
		U32					costNum_;				//进行采集或生产该道具时，需要消耗该资源的数量	
		U32					profession_;			//专精职业：专精职业生产此物品时，获得经验翻倍
		S32					needMoney_;				//制造消耗钱
		S32					chance_;				//产出绝品概率万分比
		S32					newItem_;				//绝品装备
		std::vector<U32>	costItems_;				//生产该道具需要消耗的道具ID，可以填多个与道具数量一一对应
		std::vector<U32>	costItemNum_;			//生产该道具需要的合成道具数量，可以填多个，与道具ID一一对应
	};

public:
	static bool load(char const *fn);
	static bool check();

	static MakeCore const * getMakeById(S32 id);

public:
	static std::map< S32 , MakeCore* >  MakeData_;
};

#endif