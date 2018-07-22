#ifndef __COPYSCENE_H__
#define __COPYSCENE_H__

#include "config.h"

class CopyScene
{
public:
	struct copyTableData
	{
		U32			copyLevel_;
		U32			sceneId_;
		U32			questId_;
		U32			endQuest_;		//副本结束任务
		U32			copyNum_;		//每日可进入次数
		std::vector<U32> nextSecnes_;
	};
public:
	static bool load(char const *fn);
	static bool check();

	static U32	getCopyStartQuestById(U32 id);
	static U32	getCopyLevelById(U32 id);
	static U32	getCopyNumById(U32 id);
	static bool isNextCopySecne(U32 copylevel,U32 nextId);
	static U32	getCopyAllNumByLevel(U32 playerLevel);
public:
	static std::vector<copyTableData> data_;
};


#endif