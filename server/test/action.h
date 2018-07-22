#ifndef __STATUS_H___
#define __STATUS_H___
#include "config.h"

struct Action{

	enum Type{
		None,
		Deconnect,
		Connect,
		Login,
		MoveToNpc,
		AcceptQuest,
		SubmitQuest,
		SendChat,
		JoinBattle,
		CreateTeam,
	};
	
	class TestCase* owner_;
	Type type_;
	float	   timeout_;
	
	union{
		int32 npcId_;
		int32 questId_;
		int32 battleId_;
	};
	
	std::string chatContent_;

	Action(Type type);

public:
	static void makeConnectActions(class TestCase *target);

	static void makeMainQuestActions(class TestCase *target);

	static void makeBattleDataActions(class TestCase* target);

	static void makeMoveActions(class TestCase* target);

	static void makeRestingActions(class TestCase* target);

	static void makeTeamMoveActions(class TestCase* target);

	static void makeSendChatActions(class TestCase* target);

	static void makeCreateTeamActions(class TestCase* target);

	static bool execAction(Action &action);

	static bool execDeconnect(Action &action);

	static bool execConnect(Action &action);

	static bool execLogin(Action &action);

	static bool execMoveToNpc(Action &action);

	static bool execAcceptQuest(Action &action);

	static bool execSubmitQuest(Action &action);

	static bool execSendChat(Action &action);

	static bool execJoinBattle(Action &action);

	static bool execCreateTeam(Action &action);
};

#endif