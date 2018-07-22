#ifndef __ROBOTTABLE_H__
#define __ROBOTTABLE_H__
#include "config.h"

class RobotTab
{
public:
	struct RobotData 
	{
		U32					robotId_;
		U32					robotTmpId_;			//读取PlayerData
		U32					robotLevel_;
		std::string			robotAIclass_;
		std::string			robotName_;				//机器人名字
		U32					babyId_;				//机器人携带参战宝宝
		U32					babyLevel_;
		std::string			babyAIclass_;
		std::vector<U32>	employees_;				//机器人携带参战伙伴
		std::vector<U32>	equips_;				//穿戴装备
		JobType				job_;
		U32					JobLevel_;
	};

public:
	static bool load(char const*fn);
	static bool check();
	static RobotData const* getRobotDataById(U32 id);
public:
	static std::map<S32,RobotData*>  data_;
};

class PlayerAI
{
public:
	
	U32			id_;
	JobType		prof_;
	std::string playerClass_;
	std::string babyClass_;

public:
	static bool load(char const*fn);
	static bool check();
	static PlayerAI const* getAI(JobType jt);
public:
	static std::vector<PlayerAI*> data_;
};

class RobotActionTable
{
public:
	struct RobotActionData 
	{
		std::string			robotName_;				//机器人名字
		std::string			userName_;				//机器人账户
		RobotActionType		actionType_;			//行动类型
		JobType				jobtype_;				//职业
		U32					npcid_;					//在那个NPC静止
		U32					title_;					//佩戴的称号
		std::vector<U32>	npclist_;				//巡逻链
		std::vector<U32>	equips_;				//穿戴装备
		//U32					babyId_;				//机器人携带参战宝宝
		//U32					babyLevel_;
		//std::string			babyAIclass_;
		//std::vector<U32>	employees_;				//机器人携带参战伙伴
	};
public:
	static bool load(char const*fn);
	static bool check();
	static RobotActionData const* getActionData(std::string userName);
	static bool isRobot(std::string& name);
public:
	static std::vector<RobotActionData*> actiondata_;
};

#endif