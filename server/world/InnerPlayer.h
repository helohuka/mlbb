#ifndef __INNERPLAYER_H__
#define __INNERPLAYER_H__
#include "entity.h"
//此类有玩家所有属性
//功能 1 提供部分下线玩家AI 控制
//功能 2 提供JJC 机器人基础
class InnerPlayer : public Entity
{
public:
	InnerPlayer();
	virtual ~InnerPlayer();
	virtual U32	getGUID(){return 0;}
	InnerPlayer* asInnerPlayer(){return this;}
public:
	U32						genItemMaxGuid_;
	std::vector<Baby*>		babies_;
	std::vector<Employee*>	employees_;

public:
	Baby* getBattleBaby();
	void init();
	void postEvent(AIEvent me, BattlePosition target, std::map<S32,S32> &posTable);
	

	std::string		class_;
	std::string		babyClass_;

	std::vector<std::string>			robotEvents_;
	std::vector<std::string>			babyEvents_;
};

#endif