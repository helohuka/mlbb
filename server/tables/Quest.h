#ifndef __QUEST_H__
#define __QUEST_H__

#include "config.h"
class Player;
class Quest
{
public:

	static std::vector<Quest*> cache_;
	static std::map<S32, Quest*> map_;
	static std::vector<Quest*> tongji_;
	static std::vector<Quest*> rand_;


	static bool load(char const *fn);
	static void clear();
	static bool check();

	static const Quest* getQuestById(S32 id)
	{
		std::map<S32, Quest*>::iterator itr = map_.find(id);
		if(itr == map_.end())
			return NULL;
		return itr->second;
	}
	static const Quest* randomTongjiQuest(S32 minlevel);
	static const Quest* randomRandQuest();

public:
	Quest():questId_(0),questKind_((QuestKind)0),questType_((QuestType)0),needLevel_(0),acceptNpcId_(0),submitNpcId_(0),requireType_((RequireType)0),targetSceneId_(0),targetNpcId_(0),dropId_(0),attenuationdrop_(0),jt_((JobType)0),jl_(0),postQuestId_(0){}
	S32			questId_;		///任务ID
	QuestKind	questKind_;		///任务分类
	QuestType	questType_;		///任务类型
	S32			needLevel_;		///
	S32			acceptNpcId_;	///接受任务NPC
	S32			submitNpcId_;	///提交任务NPC	
	S32			title_;
	RequireType	requireType_;	///前置类型
	S32			requirement_;	///前置类型值(与潜质类型有关
	std::vector<S32> prevQuest_;///上一个任务
	std::vector<S32> nextQuest_;
	std::vector<S32> needItemId_;
	std::vector<S32>			target_;	///任务目标与任务类型相关
	std::vector<S32>			targetNum_;	///任务目标计数
	S32			targetSceneId_;
	S32			targetNpcId_;

	///========================================================================
	///@group loot
	///@{
	U32		dropId_;
	U32		attenuationdrop_;
	JobType jt_;
	S32		jl_;
	///@}

	///========================================================================
	///@group script
	///@{
	std::string acceptScript_;
	std::string submitScript_;
	S32 postQuestId_; ///next 
	///@}
};

#endif