#define  CONNTYPE BINConnection
#include "config.h"
#include "case.h"
#include "ProtocolMemWriter.h"
#include "ProtocolMemReader.h"
#include "tmptable.h"
#include "itemtable.h"

TestCase::TestCase(int32 idx,std::string username,std::string rolename)
:BINConnection< Client2ServerStub, Server2ClientProxy , unsigned short>(0XFFFF,0XFFFF)
,index_(idx)
,username_(username)
,playerName_(rolename)
,pingTime_(VALID_PING_TIME){
	setProxy(this); 
}
TestCase::~TestCase(){}

bool TestCase::pong(){return true;}
bool TestCase::errorno(ErrorNo e){
	ACE_DEBUG((LM_INFO,"%s|%s Error %s\n",username_.c_str(),currentPlayer_.instName_.c_str(),ENUM(ErrorNo).getItemName(e)));
	return true;}
bool TestCase::teamerrorno(std::string& name, ErrorNo e){return true;}
bool TestCase::loginok(STRING& sessionkey, std::vector< COM_SimpleInformation >& players){
	//ACE_DEBUG((LM_INFO,"%s|%s Login ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	sessionkey_ = sessionkey;
	players_ = players;
	if(players_.empty()){
		int32 idx = UtlMath::randN(PlayerTmpTable::data_.size());
	
		if(playerName_.empty())
			createPlayer(MakeUniquePlayerName(),PlayerTmpTable::data_[idx].id_);
		else
			createPlayer(playerName_,PlayerTmpTable::data_[idx].id_);
	}
	else{
		enterGame(players[0].instId_);
	}
	return true;
}

bool TestCase::logoutOk(){
	//ACE_DEBUG((LM_INFO,"%s|%s Logout ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	return true;
}
bool TestCase::reconnection(COM_ReconnectInfo& recInfo){
	//ACE_DEBUG((LM_INFO,"%s|%s Reconnection\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	if(recInfo.reconnectProcess_ >= RECT_LoginOk){
		players_ = recInfo.roles_;
	}
	if(recInfo.reconnectProcess_ >= RECT_EnterGameOk){
		currentPlayer_ = recInfo.playerInst_;
		sceneInfo_ = recInfo.sceneInfo_;
	}
	sceneLoaded();
	return true;
}
bool TestCase::sessionfailed(){
	ACE_DEBUG((LM_INFO,"%s|%s Session failed\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	return true;
}
bool TestCase::createPlayerOk(COM_SimpleInformation& player){
	//ACE_DEBUG((LM_INFO,"%s|%s Create player ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	players_.push_back(player);
	enterGame(player.instId_);
	return true;
}
bool TestCase::deletePlayerOk(STRING& name){
	//ACE_DEBUG((LM_INFO,"%s|%s Delete player ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	for(size_t i=0; i<players_.size(); ++i){
		if(players_[i].instName_ == name){
			players_.erase(players_.begin() + i);
			return true;
		}
	}
	return true;
}
bool TestCase::enterGameOk(COM_PlayerInst& inst){
	//ACE_DEBUG((LM_INFO,"%s|%s Enter game ok\n",username_.c_str(),inst.instName_.c_str()));
	currentPlayer_ = inst;

	/*COM_Chat chat;
	chat.ck_ = CK_World;
	std::stringstream ss;
	ss << inst.instId_;
	chat.content_ = ss.str();
	sendChat(chat,"");*/

	return true;
}
bool TestCase::initBabies(std::vector< COM_BabyInst >& insts){
	//ACE_DEBUG((LM_INFO,"%s|%s Init babies ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	babies_ = insts;
	return true;
}
bool TestCase::initEmployees(std::vector< COM_EmployeeInst >& insts, bool isFlag){
	//ACE_DEBUG((LM_INFO,"%s|%s Init employees ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	employees_.insert(employees_.end(),insts.begin(),insts.end());
	return true;
}

bool TestCase::addBaby(COM_BabyInst& inst){
	//ACE_DEBUG((LM_INFO,"%s|%s Add baby ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	babies_.push_back(inst);
	return true;
}
bool TestCase::delBabyOK(U32 babyInstId){
	//ACE_DEBUG((LM_INFO,"%s|%s Delete baby ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	for (size_t i=0; i<babies_.size(); ++i){
		if(babies_[i].instId_ == babyInstId)
		{
			babies_.erase(babies_.begin() + i);
			return true;
		}
	}
	return true;
}
bool TestCase::learnSkillOk(COM_Skill& inst){
	//ACE_DEBUG((LM_INFO,"%s|%s Learn skill ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	currentPlayer_.skill_.push_back(inst);
	return true;
}
bool TestCase::forgetSkillOk(U32 skid){
	//ACE_DEBUG((LM_INFO,"%s|%s Forget skill ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	for(size_t i=0; i<currentPlayer_.skill_.size(); ++i){
		if(currentPlayer_.skill_[i].skillID_ == skid){
			currentPlayer_.skill_.erase(currentPlayer_.skill_.begin() + i);
			return true;
		}
	}
	return true;
}
bool TestCase::addSkillExp(U32 skid, U32 uExp, ItemUseFlag flag){
	//ACE_DEBUG((LM_INFO,"%s|%s Add skill exp ok\n",username_.c_str(),currentPlayer_.instName_.c_str()));
	for(size_t i=0; i<currentPlayer_.skill_.size(); ++i){
		if(currentPlayer_.skill_[i].skillID_ == skid){
			currentPlayer_.skill_[i].skillExp_ += uExp;
			break;
		}
	}
	return true;}
bool TestCase::skillLevelUp(U32 instId, COM_Skill& inst){return true;}
bool TestCase::enterBattleOk(COM_InitBattle& initBattle){
	//ACE_DEBUG((LM_INFO,"%s|%s Enter battle ok \n",username_.c_str(),currentPlayer_.instName_.c_str()));
	currentPlayer_.isBattle_ = true;
	COM_Order order;
	order.casterId_ = currentPlayer_.instId_;
	order.skill_ = currentPlayer_.skill_[0].skillID_;
	order.target_ = currentPlayer_.battlePosition_;
	syncOrder(order);
	return true;
}
bool TestCase::exitBattleOk(BattleJudgeType bjt, COM_BattleOverClearing& init){
	//ACE_DEBUG((LM_INFO,"%s|%s Exit battle ok(%s)\n",username_.c_str(),currentPlayer_.instName_.c_str(), ENUM(BattleJudgeType).getItemName(bjt)));
	currentPlayer_.isBattle_ = false;
	return true;
}
bool TestCase::syncOrderOk(U32 uid){return true;}
bool TestCase::syncOrderOkEX(){return true;}
bool TestCase::syncOneTurnAction(COM_BattleReport& reports){
	//ACE_DEBUG((LM_INFO,"%s|%s Sync one trun action  \n",username_.c_str(),currentPlayer_.instName_.c_str()));
	COM_Order order;
	order.casterId_ = currentPlayer_.instId_;
	order.skill_ = currentPlayer_.skill_[0].skillID_;
	order.target_ = currentPlayer_.battlePosition_;
	syncOrder(order);
	return true;
}
bool TestCase::syncProperties(U32 guid, std::vector< COM_PropValue >& props){return true;}
bool TestCase::receiveChat(COM_ChatInfo& info,COM_ContactInfo& myinfo){
	//ACE_DEBUG((LM_INFO,"%s|%s Receive chat ok \n",username_.c_str(),currentPlayer_.instName_.c_str()));
	return true;
}
bool TestCase::initBag(std::vector< COM_Item >& items){
	//ACE_DEBUG((LM_INFO,"%s|%s Init bag ok \n",username_.c_str(),currentPlayer_.instName_.c_str()));
	bag_ = items;
	return true;
}

bool TestCase::addBagItem(COM_Item& item){
	//ACE_DEBUG((LM_INFO,"%s|%s Add bag item %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),item.instId_));
	bag_.push_back(item);

	ItemTable::ItemData const* data = ItemTable::getItemById(item.itemId_);

	if(data->mainType_ == IMT_Equip){
		for(size_t i=0 ; i<currentPlayer_.equips_.size(); ++i){
			if(currentPlayer_.equips_[i].slot_ == data->slot_){
				ItemTable::ItemData const* data2 = ItemTable::getItemById(currentPlayer_.equips_[i].itemId_);
				if(data2->level_ < data->level_)
					wearEquipment(currentPlayer_.instId_,item.instId_);
				
				return true;
			}
		}

		wearEquipment(currentPlayer_.instId_,item.instId_);
	}
	return true;
}
bool TestCase::delBagItem(U16 slot){
	//ACE_DEBUG((LM_INFO,"%s|%s Del bag item %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),(int32)slot));
	for(size_t i=0; i<bag_.size(); ++i){
		if(bag_[i].slot_ == slot){
			bag_.erase(bag_.begin() + i);
			return true;
		}
	}
	return true;
}
bool TestCase::updateBagItem(COM_Item& item){
	for(size_t i=0; i<bag_.size(); ++i){
		if(bag_[i].slot_ == item.slot_){
			bag_[i] = item;
			return true;
		}
	}
	return true;
}
bool TestCase::initPlayerEquips(std::vector< COM_Item >& equips){
	currentPlayer_.equips_ = equips;
	return true;
}
bool TestCase::wearEquipmentOk(U32 target, COM_Item& equip){
	//ACE_DEBUG((LM_INFO,"%s|%s Wear equipment ok %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),equip.instId_));
	if(currentPlayer_.instId_  == target){
		for(size_t i=0; i<currentPlayer_.equips_.size(); ++i){
			if(currentPlayer_.equips_[i].slot_ == equip.slot_){
				currentPlayer_.equips_[i] = equip;
				return true;
			}
		}
	}
	currentPlayer_.equips_.push_back(equip);
	return true;
}
bool TestCase::delEquipmentOk(U32 target, U32 itemInstId){
	//ACE_DEBUG((LM_INFO,"%s|%s Del equipment ok %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),itemInstId));
	if(currentPlayer_.instId_  == target){
		for(size_t i=0; i<currentPlayer_.equips_.size(); ++i){
			if(currentPlayer_.equips_[i].instId_ == itemInstId){
				currentPlayer_.equips_.erase(currentPlayer_.equips_.begin() + i);
				return true;
			}
		}
	}
	return true;
}
bool TestCase::sortBagItemOk(){return true;}
bool TestCase::joinTeamOk(COM_TeamInfo& team){
	//ACE_DEBUG((LM_INFO," joinTeamOk username[%s]\n",username_));
	return true; 
}
bool TestCase::addTeamMember(COM_SimplePlayerInst& info){return true;}
bool TestCase::delTeamMember(S32 instId){return true;}
bool TestCase::exitTeamOk(B8 iskick){ return true;}
bool TestCase::drawLotteryBoxRep(std::vector< COM_Item >& items){return true;}
bool TestCase::addEmployee(COM_EmployeeInst& employee){return true;}
bool TestCase::sycnEmployeeSoul(S32 guid, U32 soulNum){return true;}
bool TestCase::acceptQuestOk(COM_QuestInst& inst){
	//ACE_DEBUG((LM_INFO,"%s|%s Accecpt quest %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),inst.questId_));
	questInsts_.push_back(inst);
	return true;
}
bool TestCase::submitQuestOk(S32 questId){
	//ACE_DEBUG((LM_INFO,"%s|%s Submit quest %d \n",username_.c_str(),currentPlayer_.instName_.c_str(),questId));
	questComplates_.push_back(questId);
	return true;
}
bool TestCase::giveupQuestOk(S32 questId){
	return true;
}
bool TestCase::initQuest(std::vector< COM_QuestInst >& qlist, std::vector< S32 >& clist){
	questInsts_ = qlist;
	questComplates_ = clist;
	return true;
}
bool TestCase::updateQuestInst(struct COM_QuestInst & q){
	for(size_t i=0; i<questInsts_.size(); ++i){
		if(questInsts_[i].questId_ == q.questId_){
			questInsts_[i] = q;
			return true;
		}
	}
	return true;
}
bool TestCase::createTeamOk(COM_TeamInfo& team){
	//ACE_DEBUG((LM_INFO," createTeamOk username[%s]\n",username_));
	return true;
}
bool TestCase::changeTeamOk(COM_TeamInfo& team){return true;}
bool TestCase::changeTeamLeaderOk(S32 uuid){return true;}
bool TestCase::updateTeam(COM_TeamInfo& team){return true;}
bool TestCase::jointLobbyOk(std::vector< COM_SimpleTeamInfo >& infos){return true;}
bool TestCase::syncDelLobbyTeam(U32 teamId){return true;}
bool TestCase::syncUpdateLobbyTeam(COM_SimpleTeamInfo& info){return true;}
bool TestCase::syncAddLobbyTeam(COM_SimpleTeamInfo& team){return true;}
bool TestCase::exitLobbyOk(){return true;}
bool TestCase::joinTeamRoomOK(COM_TeamInfo& team){return true;}
bool TestCase::addFriendOK(COM_ContactInfo& inst){return true;}
bool TestCase::delFriendOK(U32 instId){return true;}
bool TestCase::addBlacklistOK(COM_ContactInfo& inst){return true;}
bool TestCase::delBlacklistOK(U32 instId){return true;}
bool TestCase::referrFriendOK(std::vector< COM_ContactInfo >& insts){return true;}
bool TestCase::requestContactInfoOk( COM_ContactInfo& contact){return true;}
bool TestCase::compoundItemOk(COM_Item& item){return true;}
bool TestCase::changeBabyNameOK(U32 babyId, STRING& name){return true;}
//bool Case::friendChat(COM_ContactInfo& info,COM_Chat& content){return true;}
bool TestCase::findFriendFail(){return true;}
bool TestCase::openBagGridOk(S32 num){return true;}
bool TestCase::evolveOK(S32 guid,QualityColor qc){return true;}
bool TestCase::upStarOK(S32 guid,S32 star,COM_Skill& sk){return true;}
bool TestCase::delEmployeeOK(std::vector< U32 >& instids){return true;}
bool TestCase::requestChallengeOK(B8 isOK){return true;}
bool TestCase::requestRivalOK(std::vector< COM_EndlessStair >& infos){return true;}
bool TestCase::requestJJCRankOK(U32 myRank,std::vector< COM_EndlessStair >& infos){return true;}
bool TestCase::requestMySelfJJCDataOK(COM_EndlessStair& info){return true;}
bool TestCase::rivalTimeOK(){return true;}
bool TestCase::checkMsgOK(COM_SimplePlayerInst& inst){return true;}
bool TestCase::requestMyAllbattleMsgOK(std::vector< COM_JJCBattleMsg >& infos){return true;}
bool TestCase::myBattleMsgOK(COM_JJCBattleMsg& info){return true;}
bool TestCase::initGuide(U32 pro){ return true; }
bool TestCase::buyShopItemOk(S32 id){return true;}
bool TestCase::babyLearnSkillOK(U32 instId, U32 newSkId){return true;}
bool TestCase::addPlayerTitle(S32 title){return true;}
bool TestCase::delPlayerTitle(S32 title){return true;}
bool TestCase::requestOpenBuyBox(F32 greenTime,F32 blueTime, S32 greenFreeNum){return true;}
bool TestCase::requestGreenBoxTimeOk(){return true;}
bool TestCase::requestBlueBoxTimeOk(){return true;}
bool TestCase::syncOpenSystemFlag(U64 flag){
	return true;
}
bool TestCase::requestLevelRankOK(U32 myRank,std::vector< COM_ContactInfo >& infos){return true;}
bool TestCase::syncActivity(COM_ActivityTable& table){return true;}
bool TestCase::updateAchievementinfo(COM_Achievement& achs){return true;}
bool TestCase::syncHundredInfo(COM_HundredBattle& hb){return true;}
bool TestCase::initSignUp(std::vector< S32 >& info, S32 process, B8 sign7, B8 sign14, B8 sign28){return true;}
bool TestCase::signUp(bool){return true;}
bool TestCase::requestSignupRewardOk7(){return true;}
bool TestCase::requestSignupRewardOk14(){return true;}
bool TestCase::requestSignupRewardOk28(){return true;}
bool TestCase::initGather(U32 allnum,std::vector< COM_Gather >& gathers){return true;}
bool TestCase::openGatherOK(COM_Gather& gather){return true;}
bool TestCase::miningOk(std::vector< COM_DropItem >& items, COM_Gather& gather,U32 gatherNum){return true;}
bool TestCase::sycnDoubleExpTime(B8 isFlag,F32 times){return true;}
bool TestCase::initNpc(NpcList& npcList){return true;}
bool TestCase::addNpc(NpcList& npcList){return true;}
bool TestCase::stopMatchingOK(F32 times){return true;}
bool TestCase::startMatchingOK(){return true;}
bool TestCase::updatePvpJJCinfo(COM_PlayerVsPlayer& info){return true;}
bool TestCase::syncMyJJCTeamMember(){return true;}
bool TestCase::syncEnemyPvpJJCTeamInfo(std::vector< COM_SimpleInformation >& infos,U32 teamID_){return true;}
bool TestCase::syncEnemyPvpJJCPlayerInfo(COM_SimpleInformation& info){return true;}
bool TestCase::exitPvpJJCOk(){return true;}
bool TestCase::updateActivityStatus(ActivityType type, B8 open){return true;}
bool TestCase::inviteJoinTeam(U32 teamId,STRING& name){return true;}
bool TestCase::appendMail(std::vector< COM_Mail >& mails){return true;}
bool TestCase::delMail(S32 mailId){return true;}
//bool Case::sycnShowItem(COM_Chat& content,COM_Item& item){return true;}
bool TestCase::sycnStates(std::vector< COM_State >& states){return true;}
bool TestCase::createGuildOK(){return true;}
bool TestCase::boardcastNotice(std::string& content, bool){return true;}
bool TestCase::delGuildOK(){return true;}
bool TestCase::leaveGuildOk(std::string&, bool){return true;}
bool TestCase::initGuildData(COM_Guild& guild){return true;}
bool TestCase::initGuildMemberList(std::vector< COM_GuildMember >& member){return true;}
bool TestCase::modifyGuildMemberList(COM_GuildMember& member, ModifyListFlag flag){return true;}
bool TestCase::modifyGuildList(COM_GuildViewerData& data, ModifyListFlag flag){return true;}
bool TestCase::queryGuildListResult(S16 page, S16 pageNum, std::vector< COM_GuildViewerData >& guildList){return true;}
bool TestCase::inviteGuild(std::string& sendName, std::string& guildName){return true;}
bool TestCase::levelupGuildSkillOk(COM_Skill& skInst){return true;}
bool TestCase::presentGuildItemOk(S32 val){return true;}
bool TestCase::progenitusAddExpOk(COM_GuildProgen& mInst){return true;}
bool TestCase::setProgenitusPositionOk(std::vector< S32 >& positions){return true;}
bool TestCase::initMySelling(std::vector< COM_SellItem >& items){return true;}
bool TestCase::initMySelled(std::vector< COM_SelledItem >& items){return true;}
bool TestCase::fetchSellingOk(std::vector< COM_SellItem >& items, S32 total){return true;}
bool TestCase::sellingOk(COM_SellItem& item){return true;}
bool TestCase::selledOk(COM_SelledItem& item){return true;}
bool TestCase::unsellingOk(S32 sellid){return true;}
bool TestCase::requestBabyRankOK(U32 myRank,std::vector< COM_BabyRankData >& infos){return true;}
bool TestCase::requestEmpRankOK(U32 myRank,std::vector< COM_EmployeeRankData >& infos){return true;}
bool TestCase::insertState(COM_State& st){return true;}
bool TestCase::updattState(COM_State& st){return true;}
bool TestCase::removeState(U32 stid){return true;}
bool TestCase::updateActivityCounter(ActivityType type,S32 counter,S32 reward){return true;}
bool TestCase::requestPlayerFFRankOK(U32 myRank,std::vector< COM_ContactInfo >& infos){return true;}
bool TestCase::refreshBaby(COM_BabyInst& inst){return true;}
bool TestCase::syncTeamDirtyProp(S32 guid, std::vector< COM_PropValue >& props){return true;}
bool TestCase::fetchSellingOk2(std::vector< COM_SellItem >& items, S32 total){return true;}
bool TestCase::updateMailOk(COM_Mail& mail){return true;}
bool TestCase::useItemOk(S32 itemId, S32 stack){return true;}
bool TestCase::depositItemOK(COM_Item& item){return true;}
bool TestCase::getoutItemOK(U16 slot){return true;}
bool TestCase::depositBabyOK(COM_BabyInst& baby){return true;}
bool TestCase::getoutBabyOK(U16 slot){return true;}
bool TestCase::delStorageBabyOK(U16 slot){return true;}
bool TestCase::sortItemStorageOK(std::vector< COM_Item >& items){return true;}
bool TestCase::sortBabyStorageOK(std::vector< U32 >& babys){return true;}
bool TestCase::openStorageGrid(StorageType tp, U16 gridNum){return true;}
bool TestCase::initItemStorage(U16 gridNum,std::vector< COM_Item >& items){return true;}
bool TestCase::initBabyStorage(U16 gridNum,std::vector< COM_BabyInst >& babys){return true;}
bool TestCase::requestFixItemOk(COM_Item& item){return true;}
bool TestCase::makeDebirsItemOK(){return true;}
bool TestCase::updateMagicItem(S32 level, S32 exp){return true;}
bool TestCase::changeMagicJobOk(JobType job){return true;}
bool TestCase::setNoTalkTime(F32 t){return true;}
bool TestCase::leaveTeamOk(S32 playerId){return true;}
bool TestCase::backTeamOK(S32 playerId){return true;}
bool TestCase::teamCallMemberBack(){return true;}
bool TestCase::autoBattleResult(bool isOk){return true;}
bool TestCase::requestActivityRewardOK(U32 ar){return true;}
bool TestCase::requestJoinTeamTranspond(std::string& reqName){return true;}
bool TestCase::refuseBackTeamOk(S32 playerId){return true;}
bool TestCase::queryOnlinePlayerOK(bool isOnline){return true;}
bool TestCase::petActivityNoNum(std::string& name){return true;}
bool TestCase::updateGuildShopItems(std::vector<COM_GuildShopItem>& items){return true;}
bool TestCase::intensifyBabyOK(U32 babyid, U32 intensifyLevel){return true;}
bool TestCase::setTeamLeader(S32 playerId, bool isLeader){return true;}
bool TestCase::redemptionSpreeOk(){return true;}
bool TestCase::initEmpBattleGroup(struct COM_BattleEmp &){return true;}
bool TestCase::initAchievement(class std::vector<struct COM_Achievement>&){return true;}
bool TestCase::querySimplePlayerInstOk(struct COM_SimplePlayerInst &){return true;}
bool TestCase::sceneFilterOk(std::vector<SceneFilterType>&){return true;}
bool TestCase::syncExam(COM_Exam& exam){return true;}
bool TestCase::syncExamAnswer(COM_Answer& answer){return true;}
bool TestCase::wishingOK(){return true;}
bool TestCase::requestpvprankOK(std::vector< COM_ContactInfo >& infos){return true;}
bool TestCase::requestAudioOk(S32 audioId, std::vector< U8 >& content){return true;}
bool TestCase::leaderCloseDialogOk(){return true;}
bool TestCase::scenePlayerWearEquipment(U32 target, U32 itemId){return true;}
bool TestCase::scenePlayerDoffEquipment(U32 target, U32 itemId){return true;}
bool TestCase::warriorStartOK(){return true;}
bool TestCase::warriorStopOK(){return true;}
bool TestCase::syncWarriorEnemyTeamInfo(std::vector< COM_SimpleInformation >& infos, U32 teamID_){return true;}
bool TestCase::updateGuildBuilding(GuildBuildingType type, COM_GuildBuilding& building){return true;}
bool TestCase::updateGuildFundz(S32 val){return true;}
bool TestCase::openWarriorchooseUI(){return true;}
bool TestCase::openGuildBattle(std::string& otherName,int a, int b,bool,int){return true;}
bool TestCase::syncGuildBattleWinCount(S32 myWin, S32 otherWin){return true;}
bool TestCase::startOnlineTime(){return true;}
bool TestCase::buyFundOK(bool flag){return true;}
bool TestCase::requestFundRewardOK(U32 level){return true;}
bool TestCase::requestOnlineTimeRewardOK(U32 index){return true;}
bool TestCase::updateFestival(COM_ADLoginTotal& festival){return true;}
bool TestCase::updateSelfRecharge(COM_ADChargeTotal& val){return true;}
bool TestCase::updateSysRecharge(COM_ADChargeTotal& val){return true;}
bool TestCase::updateSelfDiscountStore(COM_ADDiscountStore& val){return true;}
bool TestCase::updateSysDiscountStore(COM_ADDiscountStore& val){return true;}
bool TestCase::updateSelfOnceRecharge(COM_ADChargeEvery& val){return true;}
bool TestCase::updateSysOnceRecharge(COM_ADChargeEvery& val){return true;}
bool TestCase::agencyActivity(ADType type, bool isFlag){return true;}
bool TestCase::openCardOK(COM_ADCardsContent& data){return true;}
bool TestCase::resetCardOK(){return true;}
bool TestCase::sycnHotRole(COM_ADHotRole& data){return true;}
bool TestCase::hotRoleBuyOk(U16 buyNum){return true;}
bool TestCase::updateSevenday(COM_Sevenday& data){return true;}
bool TestCase::joinCopySceneOK(S32 secneid){return true;}
bool TestCase::updateEmployeeActivity(COM_ADEmployeeTotal& data){return true;}
bool TestCase::firstRechargeOK(bool isFlag){return true;}
bool TestCase::firstRechargeGiftOK(bool isFlag){return true;}
bool TestCase::sycnVipflag(bool isFlag){return true;}
bool TestCase::updateShowBaby(U32 playerId, U32 showBabyTableId, std::string& showBabyName){return true;}
bool TestCase::updateMySelfRecharge(COM_ADChargeTotal& val){return true;}
bool TestCase::verificationSMSOk(std::string& phoneNumber){return true;}
bool TestCase::requestLevelGiftOK(S32 level){return true;}
bool TestCase::sycnConvertExp(S32 val){return true;}
bool TestCase::updateMinGiftActivity(COM_ADGiftBag& val){return true;}
bool TestCase::orderOk(std::string& orderId, S32 shopId){return true;}
bool TestCase::updateTeamMember(S32 playerId, bool isMember){return true;}
//////////////////////////////////////////////////////////////////////////


bool TestCase::joinScene(struct COM_SceneInfo & info){
	sceneInfo_ = info;
	sceneInfo_.position_.isLast_ = true;
	sceneLoaded();
	return true;
}
bool TestCase::addToScene(COM_ScenePlayerInformation & p){
	//scenePlayers_.push_back(p);
	sceneInfo_.players_.push_back(p);
	return true;
}
bool TestCase::delFormScene(int32 playerId){
	for(size_t i=0; i<sceneInfo_.players_.size(); ++i){
		if(sceneInfo_.players_[i].instId_ == playerId){
			sceneInfo_.players_.erase(sceneInfo_.players_.begin() + i);
			return true;
		}
	}
	return true;
}
bool TestCase::move2(int32 playerId,COM_FPosition& pos){
	if(currentPlayer_.instId_ == playerId){
		sceneInfo_.position_ = pos;
	}
	else {
		for(size_t i=0; i<sceneInfo_.players_.size(); ++i){
			if(sceneInfo_.players_[i].instId_ == playerId){
				sceneInfo_.players_[i].originPos_ = pos;
				return true;
			}
		}
	}
	return true;
}
bool TestCase::transfor2(int32 playerId,COM_FPosition& pos){
	if(currentPlayer_.instId_ == playerId){
		sceneInfo_.position_ = pos;  
	}
	else {
		for(size_t i=0; i<sceneInfo_.players_.size(); ++i){
			if(sceneInfo_.players_[i].instId_ == playerId){
				sceneInfo_.players_[i].originPos_ = pos;
				return true;
			}
		}
	}
	return true;
}
bool TestCase::openScene(int32 sceneId){
	return true;
}
bool TestCase::talked2Npc(int32 npcId){
	sceneInfo_.position_.isLast_ = true;
	return true;
}
bool TestCase::talked2Player(int){
	
	return true;
}
bool TestCase::delNpc(std::vector<int32> &npcs){
	for(int32 i=0; i<sceneInfo_.npcs_.size(); ++i){
		if(std::find(npcs.begin(),npcs.end(),sceneInfo_.npcs_[i]) != npcs.end()){
			sceneInfo_.npcs_.erase(sceneInfo_.npcs_.begin() + i);
			--i;
		}
	}
	return true;
}
bool TestCase::lotteryOk(int id,std::vector< COM_DropItem >& dropItem){return true;}

bool TestCase::remouldBabyOK(unsigned int){return true;}
bool TestCase::battleEmployee(int,enum EmployeesBattleGroup,bool){return true;}
bool TestCase::changeEmpBattleGroupOK(enum EmployeesBattleGroup){return true;}
//////////////////////////////////////////////////////////////////////////
bool TestCase::syncBattleStatus(int,bool){ return true;}
bool TestCase::publishItemInstRes(struct COM_ShowItemInstInfo &,enum ChatKind){ return true;}
bool TestCase::queryItemInstRes(struct COM_ShowItemInst &){ return true;}
bool TestCase::publishBabyInstRes(struct COM_ShowbabyInstInfo &,enum ChatKind){ return true;}
bool TestCase::queryBabyInstRes(struct COM_ShowbabyInst &){ return true;}
bool TestCase::queryPlayerOK(struct COM_SimplePlayerInst &){ return true;}
bool TestCase::queryBabyOK(struct COM_BabyInst &){ return true;}
bool TestCase::queryEmployeeOK(struct COM_EmployeeInst &){ return true;}
bool TestCase::magicItemTupoOk(int){ return true;}
bool TestCase::requestFriendListOK(std::vector< COM_ContactInfo >& insts){return true;}
bool TestCase::updateGuildMemberContribution(S32 val){return true;}
bool TestCase::updateGuildMyMember(COM_GuildMember& member){return true;}
bool TestCase::zhuanpanOK(std::vector< U32 >& pond){return true;}
bool TestCase::sycnZhuanpanData(COM_ZhuanpanData& data){return true;}
bool TestCase::updateZhuanpanNotice(COM_Zhuanpan& zhuanp){return true;}
bool TestCase::initcompound(std::vector< U32 >& compounds){return true;}
bool TestCase::openCompound(U32 compoundId){return true;}
bool TestCase::copynonum(std::string& name){return true;}
bool TestCase::shareWishOK(COM_Wish& wish){return true;}
bool TestCase::startGuildBattle(std::string& otherName, S32 otherCon, S32 selfCon){return true;}
//bool Case::openGuildBattle(int){return true;}
bool TestCase::closeGuildBattle(bool){return true;}
//bool Case::syncGuildBattleWinCount(int){return true;}
bool TestCase::cantMove(){
	sceneInfo_.position_.isLast_ = true;
	return true;
}
bool TestCase::initCopyNums(){return true;}
bool TestCase::wearFuwenOk(struct COM_Item &){return true;}
bool TestCase::takeoffFuwenOk(int){return true;}
bool TestCase::compFuwenOk(void){return true;}
bool TestCase::updateRandSubmitQuestCount(int32 submitCount){return true;}
bool TestCase::updateIntegralShop(struct COM_IntegralData &){return true;}
bool TestCase::requestEmployeeQuestOk(std::vector< COM_EmployeeQuestInst >& questList){return true;}
bool TestCase::acceptEmployeeQuestOk(COM_EmployeeQuestInst& inst){return true;}
bool TestCase::submitEmployeeQuestOk(int,bool){return true;}
bool TestCase::sycnCrystal(struct COM_CrystalData &){return true;}
bool TestCase::crystalUpLeveResult(bool){return true;}
bool TestCase::resetCrystalPropOK(void){return true;}
bool TestCase::sycnCourseGift(std::vector< COM_CourseGift >& data){return true;}

void TestCase::update(float dt){
	pingTime_ -= dt;
	if(pingTime_ <= 0.F){
		pingTime_ += VALID_PING_TIME;
		ping();
		//ACE_DEBUG((LM_INFO,"%s|%s ping \n",username_.c_str(),playerName_.c_str()));
	}
}

void TestCase::updateAction(float dt){
	if(actions_.empty())
		return;
	Action &action = actions_.back();
	//ACE_DEBUG((LM_INFO,"%s|%s start action \n",action.owner_->username_.c_str(),action.owner_->playerName_.c_str()));
	action.timeout_ += dt;
	if(Action::execAction(action)){
		actions_.pop_back();
	}
	
}

void TestCase::setCaseAction(RobotActionType actiontype){
	switch (actiontype)
	{
	case RAT_Resting:
		{
			Action::makeRestingActions(this);
		}
		break;
	case RAT_Move:
		{
			Action::makeMoveActions(this);
		}
		break;
	case RAT_QuestMove:
		{
			Action::makeMainQuestActions(this);
		}
		break;
	case RAT_TeamMove:
		{
			Action::makeTeamMoveActions(this);
		}
		break;
	default:
		break;
	}
}