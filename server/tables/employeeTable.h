
#ifndef __BUDDY_TABLE_H__
#define __BUDDY_TABLE_H__

#include "config.h"

class EmployeeTable
{
public:
	struct EmployeeData
	{
		U32						Id_;
		std::string				name_;
		RaceType				race_;				//种族
		std::string				AIClassName_;
		QualityColor			quality_;			//品质
		U32						star_;				//星级
		JobType					Job_;
		U32						JobLevel_;	
		U32						soul_;				//转换可获得武魂
		std::vector<U32>		skillneed_;			//技能升级所需佣兵之心
		std::vector<float>		grows_;				//品质对应的资质	
		std::vector<U32>		evolutionNum_;		//进阶所需材料数量
		std::vector<float>		properties_;
		std::vector< std::pair<S32,S32> >	defaultSkill_;
	};

public:
	static bool load(char const *fn);
	static bool check();
	static EmployeeData const * getEmployeeById(U32 id);
	static U32	getSkillLevelUpNeed(U32	id,U32 sklv);
public:
	static std::map< S32 , EmployeeData* >  data_;
};

#endif