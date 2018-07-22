#ifndef __PROFESSION_H__
#define __PROFESSION_H__

#include "config.h"

class Profession
{
	
public:
	static void clear();
	static bool load(const char* fn);
	static bool check();
	
	static const Profession* get(JobType type, S32 level);
	
	bool canUseItem(ItemSubType type, S32 lev) const;
	bool canUseSkill(S32 group, S32 sklev) const ;
	bool canLearnSkill(S32 group) const ;
	
	S32 getSkillMaxLevel(S32 skid) const ;
	S32 getItemMaxLevel(ItemSubType type) const;
	inline bool IsProudSkill(S32 group) const
	{
		return(std::find(proudskillgroup_.begin(),proudskillgroup_.end(),group)!=proudskillgroup_.end());
	}
public:

	S32		id_;
	JobType jobtype_;
	S32		joblevel_;
	std::vector<std::pair<S32,S32> >		 canuseskill_;
	std::vector<std::pair<ItemSubType,S32> > canuseitem_;
	std::vector<S32> proudskillgroup_;
	std::vector<std::pair<PropertyType,S32> >		 profprop_;		//职业推荐属性
private:
	static std::vector<Profession*> professions_;
};

#endif