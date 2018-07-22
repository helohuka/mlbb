/************************************************************************/
/**
 * @file	tmptable.h
 * @date	2015-3-2015/03/03 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/
#ifndef __TMP_TABLE_H__
#define __TMP_TABLE_H__
#include "config.h"
#include "CSVParser.h"

class PlayerTmpTable
{
public:
	struct Core
	{
		S32 id_;
		S32 defaultSceneId_;
		std::vector<float> properties_;
		std::vector< std::pair<S32,S32> > defaultSkill_;
		std::vector<U32> defaultBaby_;
	};

public:
	static bool load(char const *fn);
	static bool check();

	static Core const * getTemplateById(U32 id);
	
public:
	static std::vector<Core> data_;
};

//角色，宠物升级所需经验
class ExpTable
{
public:
	struct Core
	{
		U32		level_;
		float	playerExp_;
		float	babyExp_;
	};

	static bool load(char const *fn);
	static bool check();
	static Core const * getTemplateById(U32 level);
public:
	static std::vector<Core*>	data_;
};

#endif /// end __TMP_TABLE_H__