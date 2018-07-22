#include "config.h"
#include "action.h"
#include "case.h"
#include "tmptable.h"
#include "Quest.h"
#include "BattleData.h"
#include "npctable.h"
#include "robotTable.h"

Action::Action(Type type)
:type_(type)
,timeout_(0){

}

void Action::makeConnectActions(class TestCase *target){
	std::vector<Action> actions;
	
	Action action(Connect);
	action.owner_ = target;
	actions.push_back(action);

	action.type_ = Login;
	actions.push_back(action);

	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeMainQuestActions(TestCase* target){
	int32 firstQuestId = 0;
	bool isInCurrent = false;
	if(target->questInsts_.empty()){

		for(size_t i=0; i<Quest::cache_.size(); ++i){
			if(Quest::cache_[i]->questId_ == 90000){
				int a = 0;
			}
			if(Quest::cache_[i]->questKind_ != QK_Main)
				continue;
			if(std::find(target->questComplates_.begin(),target->questComplates_.end(),Quest::cache_[i]->questId_) != target->questComplates_.end())
				continue;
			if(!Quest::cache_[i]->prevQuest_.empty())
				if(std::find(target->questComplates_.begin(),target->questComplates_.end(),Quest::cache_[i]->prevQuest_[0]) == target->questComplates_.end())
					continue;

			firstQuestId = Quest::cache_[i]->questId_;
			
		}
	}
	else {
		firstQuestId = target->questInsts_[0].questId_;
		isInCurrent = true;
	}

	Quest const* q = Quest::getQuestById(firstQuestId);
	std::vector<Action> actions;
	
	/*{
		Action action(Connect);
		action.owner_ = target;
		actions.push_back(action);

		action.type_ = Login;
		actions.push_back(action);
	}*/

	if(isInCurrent){
		Action action(MoveToNpc);
		action.owner_ = target;
		action.npcId_ = q->submitNpcId_;
		actions.push_back(action);

		action.type_ = SubmitQuest;
		action.questId_ = q->questId_;
		actions.push_back(action);
	}else {
		while(q){
			Action action(MoveToNpc);
			action.owner_ = target;
			action.npcId_ = q->acceptNpcId_;
			actions.push_back(action);

			action.type_ = AcceptQuest;
			action.questId_ = q->questId_;
			actions.push_back(action);

			action.type_ = MoveToNpc;
			action.npcId_ = q->submitNpcId_;
			actions.push_back(action);

			action.type_ = SubmitQuest;
			action.questId_ = q->questId_;
			actions.push_back(action);
			
			q = Quest::getQuestById(q->postQuestId_);
		}
	}
	
	for(size_t i=0; i<BattleData::data_.size(); ++i){
		if(BattleData::data_[i]->battleId_ == 1){
			continue;
		}
		if(BattleData::data_[i]->battleType_ != BT_PVE)
			continue;
		Action action(JoinBattle);
		action.owner_ = target;
		action.battleId_ = BattleData::data_[i]->battleId_;

		actions.push_back(action);
	}

	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeBattleDataActions(TestCase* target){

	std::vector<Action> actions;

	{
		Action action(Connect);
		action.owner_ = target;
		actions.push_back(action);

		action.type_ = Login;
		actions.push_back(action);
	}

	for(size_t i=0; i<BattleData::data_.size(); ++i){
		if(BattleData::data_[i]->battleId_ == 1){
			continue;
		}
		if(BattleData::data_[i]->battleType_ != BT_PVE)
			continue;
		Action action(JoinBattle);
		action.owner_ = target;
		action.battleId_ = BattleData::data_[i]->battleId_;

		actions.push_back(action);
	}

	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeMoveActions(class TestCase* target){
	std::vector<Action> actions;
	{
		std::vector<U32> npclist;
		RobotActionTable::RobotActionData const* pdata = RobotActionTable::getActionData(target->username_);
		if(pdata == NULL)
			return;
		npclist = pdata->npclist_;
		std::random_shuffle(npclist.begin(),npclist.end());
		for (size_t i =0; i<npclist.size(); ++i)
		{
			Action action(MoveToNpc);
			action.owner_ = target;
			action.npcId_ = npclist[i];
			actions.push_back(action);
		}
	}

	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeRestingActions(class TestCase* target){
	std::vector<Action> actions;
	{
		RobotActionTable::RobotActionData const* pdata = RobotActionTable::getActionData(target->username_);
		if(pdata == NULL)
			return;

		Action action(MoveToNpc);
		action.owner_ = target;
		action.npcId_ = pdata->npcid_;
		actions.push_back(action);
	}

	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeTeamMoveActions(class TestCase* target){
	RobotActionTable::RobotActionData const* pdata = RobotActionTable::getActionData(target->username_);
	if(pdata == NULL)
		return;
	std::vector<Action> actions;
	for (size_t i=0;i<pdata->npclist_.size();++i)
	{
		Action action(MoveToNpc);
		action.owner_ = target;
		action.npcId_ = pdata->npclist_[i];
		actions.push_back(action);
	}
	
	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

void Action::makeCreateTeamActions(class TestCase* target){
	std::vector<Action> actions;
	
	Action action(CreateTeam);
	action.owner_ = target;
	actions.push_back(action);
	
	target->actions_.insert(target->actions_.end(),actions.rbegin(),actions.rend());
}

//////////////////////////////////////////////////////////////////////////

bool Action::execAction(Action &action){
#define CASE(T) case T: return exec##T(action);
	switch(action.type_){
		CASE(Deconnect);
		CASE(Connect);
		CASE(Login);
		CASE(MoveToNpc);
		CASE(AcceptQuest);
		CASE(SubmitQuest);
		CASE(SendChat);
		CASE(JoinBattle);
		CASE(CreateTeam);
		default: return true;
	}
	return true;
#undef CASE
}

bool Action::execDeconnect(Action &action){
	action.owner_->close();
	return true;
}

bool Action::execConnect(Action &action){
	if(action.timeout_ < 3){
		return false;
	}
	ACE_Connector<TestCase, ACE_SOCK_CONNECTOR> connector;
	ACE_INET_Addr addr(Env::get<const char*>(V_GatewayHost));
	if(-1 == connector.connect(action.owner_,addr)){
		ACE_DEBUG((LM_ERROR,"Connect gateway error %s\n",Env::get<const char*>(V_GatewayHost)));
		return false;
	}
	return true;
}

bool Action::execLogin(Action &action){
	if(action.owner_->username_.empty())
		action.owner_->username_ = action.owner_->MakeUniqueUsername();
	COM_LoginInfo li;
	li.username_ = action.owner_->username_;
	li.version_ = VERSION_NUMBER;
	action.owner_->login(li);
	return true;
}

bool Action::execMoveToNpc(Action &action){
	//if(action.timeout_ < 0.5F){
	//	return false;
	//}
	if(action.npcId_ > 0){
		action.owner_->moveToNpc(action.npcId_);
		action.npcId_ = -action.npcId_;
		return false;
	}
	else{
		return action.owner_->sceneInfo_.position_.isLast_;
	}
	return true;
}

bool Action::execAcceptQuest(Action &action){
	//if(action.timeout_ < 0.5F){
	//	return false;
	//}
	action.owner_->acceptQuest(action.questId_);
	return true;
}

bool Action::execSubmitQuest(Action &action){
	//if(action.timeout_ < 0.5F){
	//	return false;
	//}
	COM_Chat chat;
	chat.ck_ = CK_GM;
	std::stringstream ss;
	ss << "submit_quest " << action.questId_;
	chat.content_ = ss.str();
	action.owner_->sendChat(chat,"");
	return true;
}

bool Action::execSendChat(Action &action){
	RobotActionTable::RobotActionData const* pdata = RobotActionTable::getActionData(action.owner_->username_);
	if(pdata == NULL)
		return false;
	COM_Chat chat;
	chat.ck_ = CK_GM;
	chat.content_ = action.chatContent_;
	action.owner_->sendChat(chat,"");
	return true;
}

bool Action::execJoinBattle(Action &action){
	if(action.owner_->currentPlayer_.isBattle_){
		return false;
	}
	action.owner_->enterBattle(action.battleId_);
	return true;
}

bool Action::execCreateTeam(Action &action){
	COM_CreateTeamInfo info;
	info.type_ = TT_MainQuest;
	info.name_ = "巡逻";
	info.pwd_  = "";
	info.minLevel_ = 1;
	info.maxLevel_ = 60;
	info.maxMemberSize_ = 5;
	action.owner_->createTeam(info);
	return true;
}