/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */


#ifndef __CASE_H__
#define __CASE_H__
#include "config.h"
#include "action.h"


class TestCase
	: public BINConnection< Client2ServerStub, Server2ClientProxy , unsigned short>
	, public Server2ClientProxy
{
public:
	std::string MakeUniqueUsername(){
		std::stringstream sstream;
		sstream << "U" << ACE_OS::getpid() << ACE_Thread::self() << "C" << index_ ;
		return sstream.str();
	}
	std::string MakeUniquePlayerName(){
		std::stringstream sstream;
		sstream << "P" << ACE_OS::getpid() << ACE_Thread::self() << "C"<< index_ ;
		return sstream.str();
	}
	bool RollBy100(){
		return UtlMath::randN(100) < 50;
	}
	TestCase(){
		SRV_ASSERT(0);
	}
	TestCase(int32 idx,std::string username = "",std::string rolename = "");
	~TestCase();
public:
#include "Server2ClientMethods.h"
public:
	void update(float dt);
	void updateAction(float dt);
	void setCaseAction(RobotActionType actiontype);
public:
	int32 index_;
	float		pingTime_;
	std::string username_;
	std::string playerName_;
	std::string sessionkey_;
	std::vector< COM_SimpleInformation > players_;
	COM_PlayerInst currentPlayer_;
	COM_SceneInfo sceneInfo_;
	std::vector< COM_BabyInst > babies_;
	std::vector< COM_EmployeeInst > employees_;
	std::vector< COM_Item> bag_;
	std::vector< COM_QuestInst> questInsts_;
	std::vector< int32 > questComplates_;
	
	COM_InitBattle battleInfo_;

	std::vector<Action> actions_;
};


#endif