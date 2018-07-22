
#include "config.h"
#include "client.h"
#include "account.h"
#include "player.h"
#include "tmptable.h"
#include "FilterWord.h"
#include "gwhandler.h"
#include "dbhandler.h"
#include "loginhandler.h"
#include "worldserv.h"
#include "broadcaster.h"
#include "team.h"
#include "player.h"
#include "robotTable.h"


#define __USER_CHECK //ACE_DEBUG((LM_INFO,"CLIENT PROXY USER(%s) LINE(%d)\n",account_->username_.c_str(),__LINE__));

#define SAFE_CALL_ACCOUNT(fun) do{if(NULL!=account_){ __USER_CHECK; (account_->fun);} else{ /*ACE_DEBUG((LM_DEBUG,"Account is Nil File[%s]Line[%d]\n",__FILE__,__LINE__));*/} return true;}while(0);
ClientHandler::ClientHandler()
:account_(NULL){
	setProxy(this);	
}

ClientHandler::~ClientHandler(){
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Delete client\n"))); 	
};

std::string ClientHandler::infomation(){
	std::stringstream ss;
	ss << " Channel[" << getGuid() << "]";
	if(account_)
	{
		ss << " Account[" << account_->username_ << "]";
		if(account_->player_)
			ss << " Player[" << account_->player_->getNameC() << "]";
	}
	return ss.str();
}

bool 
ClientHandler::handleClose()
{ ///远端关闭
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Handle close %s\n"),infomation().c_str())); 	
	VerifiedAccount::del(this); ///强制删除验证用户
	if(account_) 
		account_->deConnect(false);
	Channel::handleClose();
	DEL_MEM(this);
	//isUsed_ = false;
	return true;
}



///========================================================================
///@group
///@{
bool
ClientHandler::requestBag()
{
	SAFE_CALL_ACCOUNT(requestBag());
	return true;
}

bool
ClientHandler::requestEmployees()
{
	SAFE_CALL_ACCOUNT(requestEmployees());
	return true;
}

bool
ClientHandler::requestStorage(StorageType tp)
{
	SAFE_CALL_ACCOUNT(requestStorage(tp));
	return true;
}

bool
ClientHandler::requestAchievement()
{
	SAFE_CALL_ACCOUNT(requestAchievement());
	return true;
}

bool
ClientHandler::requestPhoto()
{
	SAFE_CALL_ACCOUNT(requestPhoto());
	return true;
}

bool
ClientHandler::initminig()
{
	SAFE_CALL_ACCOUNT(initminig());
	return true;
}

bool
ClientHandler::requestCompound()
{
	SAFE_CALL_ACCOUNT(requestCompound());
	return true;
}

bool
ClientHandler::openvip(VipLevel vl)
{
	SAFE_CALL_ACCOUNT(openvip(vl));	
	return true;
}

bool
ClientHandler::sendChat(COM_Chat& content, STRING& targetName)
{
	SAFE_CALL_ACCOUNT(sendChat(content,targetName));	
	return true;
}

bool
ClientHandler::requestAudio(int32 audioId){
	SAFE_CALL_ACCOUNT(requestAudio(audioId));
	return true;
}

bool
ClientHandler::publishItemInst(ItemContainerType type, U32 itemInstId, ChatKind chatType, std::string& playerName)
{
	SAFE_CALL_ACCOUNT(publishItemInst(type,itemInstId,chatType,playerName));
	return true;
}

bool
ClientHandler::queryItemInst(S32 showId)
{
	SAFE_CALL_ACCOUNT(queryItemInst(showId));
	return true;
}

bool
ClientHandler::publishbabyInst(ChatKind type,U32 babyInstId, std::string& playerName)
{
	SAFE_CALL_ACCOUNT(publishbabyInst(type,babyInstId,playerName));
	return true;
}

bool
ClientHandler::querybabyInst(S32 showId)
{
	SAFE_CALL_ACCOUNT(querybabyInst(showId));
	return true;
}

bool 
ClientHandler::useItem(U32 slot, U32 target, U32 stack)
{
	SAFE_CALL_ACCOUNT(useItem(slot,target,stack));
	return true;
}

bool
ClientHandler::talkedNpc(S32 npcId){
SAFE_CALL_ACCOUNT(talkedNpc(npcId));
}

bool 
ClientHandler::wearEquipment(U32 target, U32 bagSlot)
{
	SAFE_CALL_ACCOUNT(wearEquipment(target,bagSlot));
	return true;
}

bool 
ClientHandler::delEquipment(U32 target,U32 itemInstId)
{
	SAFE_CALL_ACCOUNT(delEquipment(target,itemInstId));
	return true;
}

bool 
ClientHandler::setPlayerFront(bool isFront)
{
	SAFE_CALL_ACCOUNT(setPlayerFront(isFront));
	return true;
}

bool 
ClientHandler::createTeam(COM_CreateTeamInfo& cti)
{
	SAFE_CALL_ACCOUNT(createTeam(cti));
	return true;
}
bool 
ClientHandler::changeTeam(COM_CreateTeamInfo& cti)
{
	SAFE_CALL_ACCOUNT(changeTeam(cti));
	return true;
}
bool 
ClientHandler::kickTeamMember(U32 uuid)
{
	SAFE_CALL_ACCOUNT(kickTeamMember(uuid));
	return true;
}
bool 
ClientHandler::changeTeamLeader(U32 uuid)
{
	SAFE_CALL_ACCOUNT(changeTeamLeader(uuid));
	return true;
}

bool
ClientHandler::joinTeam(U32 teamId,STRING& pwd)
{
	SAFE_CALL_ACCOUNT(joinTeam(teamId,pwd));
	return true;
}

bool
ClientHandler::exitTeam()
{
	SAFE_CALL_ACCOUNT(exitTeam());
	return true;
}

bool
ClientHandler::inviteTeamMember(STRING& name)
{
	SAFE_CALL_ACCOUNT(inviteTeamMember(name));
	return true;
}

bool
ClientHandler::isjoinTeam(U32 teamId,B8 isFlag)
{
	SAFE_CALL_ACCOUNT(isjoinTeam(teamId,isFlag));
	return true;
}

bool 
ClientHandler::jointLobby()
{
	SAFE_CALL_ACCOUNT(jointLobby());
	return true;
}

bool
ClientHandler::changeTeamPassword(STRING& pwd)
{
	SAFE_CALL_ACCOUNT(changeTeamPassword(pwd));
	return true;
}

bool 
ClientHandler::exitLobby()
{
	SAFE_CALL_ACCOUNT(exitLobby());
	return true;
}

bool
ClientHandler::joinTeamRoom()
{
	SAFE_CALL_ACCOUNT(joinTeamRoom());
	return true;
}

bool
ClientHandler::leaveTeam(){
	SAFE_CALL_ACCOUNT(leaveTeam());
	return true;
}

bool
ClientHandler::backTeam(){
	SAFE_CALL_ACCOUNT(backTeam());
	return true;
}

bool
ClientHandler::refuseBackTeam(){
	SAFE_CALL_ACCOUNT(refuseBackTeam());
	return true;
}

bool
ClientHandler::teamCallMember(S32 playerId){
	SAFE_CALL_ACCOUNT(teamCallMember(playerId));
	return true;
}

bool
ClientHandler::requestJoinTeam(std::string& targetName)
{
	SAFE_CALL_ACCOUNT(requestJoinTeam(targetName));
	return true;
}

bool
ClientHandler::ratifyJoinTeam(std::string& sendName)
{
	SAFE_CALL_ACCOUNT(ratifyJoinTeam(sendName));
	return true;
}

bool 
ClientHandler::move(float x, float z)
{
	SAFE_CALL_ACCOUNT(move(x,z));
	return true;
}

bool 
ClientHandler::moveToNpc(S32 npcid)
{
	SAFE_CALL_ACCOUNT(moveToNpc(npcid));
	return true;
}

bool 
ClientHandler::moveToNpc2(NpcType type)
{
	SAFE_CALL_ACCOUNT(moveToNpc2(type));
	return true;
}

bool 
ClientHandler::moveToZone(S32 sceneId, S32 zoneId)
{
	SAFE_CALL_ACCOUNT(moveToZone(sceneId,zoneId));
	return true;
}

bool 
ClientHandler::autoBattle(){
	SAFE_CALL_ACCOUNT(autoBattle());
	return true;
}

bool
ClientHandler::stopAutoBattle(){
	SAFE_CALL_ACCOUNT(stopAutoBattle());
	return true;
}


bool
ClientHandler::stopMove(){
	SAFE_CALL_ACCOUNT(stopMove());
	return true;
}

bool
ClientHandler::sceneLoaded(){
	SAFE_CALL_ACCOUNT(sceneLoaded());
	return true;
}

bool
ClientHandler::querySimplePlayerInst(U32 playerId){
	SAFE_CALL_ACCOUNT(querySimplePlayerInst(playerId));
	return true;
}

bool 
ClientHandler::sortBagItem()
{
	SAFE_CALL_ACCOUNT(sortBagItem());
	return true;
}

bool 
ClientHandler::drawLotteryBox(BoxType type,bool isFree)
{
	SAFE_CALL_ACCOUNT(drawLotteryBox(type,isFree));
	return true;
}

bool
ClientHandler::setBattleEmp(U32 empID,EmployeesBattleGroup group, B8 isBattle)
{
	SAFE_CALL_ACCOUNT(setBattleEmp(empID,group,isBattle));
	return true;
}

bool
ClientHandler::changeEmpBattleGroup(EmployeesBattleGroup group)
{
	SAFE_CALL_ACCOUNT(changeEmpBattleGroup(group));
	return true;
}

bool
ClientHandler::requestContactInfoById(U32 instId)
{
	SAFE_CALL_ACCOUNT(requestContactInfoById(instId));
	return true;
}

bool
ClientHandler::requestContactInfoByName(STRING& name)
{
	SAFE_CALL_ACCOUNT(requestContactInfoByName(name));
	return true;
}

bool
ClientHandler::addFriend(U32 instId)
{
	SAFE_CALL_ACCOUNT(addFriend(instId));
	return true;
}

bool
ClientHandler::delFriend(U32 instId)
{
	SAFE_CALL_ACCOUNT(delFriend(instId));
	return true;
}

bool
ClientHandler::addBlacklist(U32 instId)
{
	SAFE_CALL_ACCOUNT(addBlacklist(instId));
	return true;
}

bool
ClientHandler::delBlacklist(U32 instId)
{
	SAFE_CALL_ACCOUNT(delBlacklist(instId));
	return true;
}

bool
ClientHandler::requestReferrFriend()
{
	SAFE_CALL_ACCOUNT(requestReferrFriend());
	return true;
}

bool
ClientHandler::requestFriendList()
{
	SAFE_CALL_ACCOUNT(requestFriendList());
	return true;
}

bool
ClientHandler::changeBabyName(U32 babyID, STRING& name)
{
	if (FilterWord::strHasSymbols(name))
	{
		errorno(EN_FilterWord);
		return true;
	}

	SAFE_CALL_ACCOUNT(changeBabyName(babyID,name));
	return true;
}

bool
ClientHandler::sellBagItem(U32 instId, U32 num)
{
	SAFE_CALL_ACCOUNT(sellBagItem(instId,num));
	return true;
}

bool
ClientHandler::bagItemSplit(S32 instId,S32 splitNum)
{
		SAFE_CALL_ACCOUNT(bagItemSplit(instId,splitNum));	
	return true;
}

bool
ClientHandler::requestEvolve(U32 empInstId)
{
	SAFE_CALL_ACCOUNT(requestEvolve(empInstId));
	return true;
}

bool
ClientHandler::requestUpStar(U32 empInstId)
{
	SAFE_CALL_ACCOUNT(requestUpStar(empInstId));
	return true;
}

bool
ClientHandler::empSkillLevelUp(U32 empId, S32 skillId)
{
	SAFE_CALL_ACCOUNT(empSkillLevelUp(empId,skillId));
	return true;
}


bool
ClientHandler::requestDelEmp(U32 empInstId)
{
	SAFE_CALL_ACCOUNT(requestDelEmp(empInstId));
	return true;
}
///@}


//////////////////////////////////////////////////////////////////////////

bool
ClientHandler::ping()
{
	SAFE_CALL_ACCOUNT(ping());
	return true;
}

bool
ClientHandler::sessionlogin(COM_LoginInfo& info)
{
	ACE_DEBUG((LM_INFO,"Client session login %s %s\n",info.username_.c_str(),info.sessionkey_.c_str()));
	if(info.version_ != VERSION_NUMBER)
	{
		errorno(EN_VersionNotMatch);
		return false;
	}
	VerifiedAccount *p = VerifiedAccount::get(info.username_);
	if (p != NULL) {
		ACE_DEBUG((LM_ERROR, "ClientHandler::login(COM_LoginInfo& info) scend %s\n", info.username_.c_str()));
		return true;
	}
	Account* pacc = Account::getAccountBySessionkey(info.sessionkey_);
	
	if(pacc && pacc->serverId_ == serverId_)
	{
		if(!pacc->isOffline())
		{
			if(pacc->client_ == this){
				ACE_DEBUG((LM_ERROR,"Session login client is already connect!!! %s\n",info.username_.c_str()));
				return true;
			}

			ACE_DEBUG((LM_ERROR,"Session login client is online!!! %s\n",info.username_.c_str()));
			pacc->deConnect(); //这个可能出现 两个 client 对应一个account 情况
		}
		pacc->reConnect(this);

	}
	else
	{
		ACE_DEBUG((LM_ERROR,"Session login failed !!! %s %s\n",info.username_.c_str(),info.sessionkey_.c_str()));
		sessionfailed();
		Account::removeAccountByName(info.username_);
	}

	return true;
}

bool
ClientHandler::login(COM_LoginInfo& info)
{
	
	//if(info.version_ != VERSION_NUMBER)
	//{
	//	errorno(EN_VersionNotMatch);
	//	return false;
	//}

	if(info.username_.empty() || info.username_.length() == 0)
		return false;
	

	//判断账户内是否有屏蔽字
	if (FilterWord::replace(info.username_))
	{
		errorno(EN_FilterWord);
		return false;
	}
	

	if(account_){
		ACE_DEBUG((LM_ERROR,"Client not use second login\n"));
		return true;
	}
	
	bool isrobot = RobotActionTable::isRobot(info.username_);

	VerifiedAccount va;
	VerifiedAccount *p = VerifiedAccount::get(info.username_);
	if(p != NULL){
		ACE_DEBUG((LM_ERROR,"ClientHandler::login(COM_LoginInfo& info) scend %s\n",info.username_.c_str()));
		return true;
	}
	
	va.username_ = info.username_;
	va.password_ = info.password_;
	va.client_ = this;

	VerifiedAccount::add(va);
	LoginHandler::instance()->queryAccount(info);
	ACE_DEBUG((LM_DEBUG,"Client login %s, %d\n",info.username_.c_str(), getGuid()));
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("ClientHandler::login() va.username_[%s]\n"),va.username_.c_str()));
	return true;
}

bool
ClientHandler::logout()
{ ///登出处理
	SAFE_CALL_ACCOUNT(logout());
	return true;
}

///========================================================================
///@group Create Player
///@{
bool
ClientHandler::createPlayer(STRING& playername, U8 playerTmpId)
{
	if (FilterWord::strHasSymbols(playername))
	{
		errorno(EN_FilterWord);
		return true;
	}

	SAFE_CALL_ACCOUNT(createPlayer(playername,playerTmpId));
}

bool
ClientHandler::deletePlayer(STRING& playername)
{
	if (FilterWord::strHasSymbols(playername))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(deletePlayer(playername));
}
///@}
///========================================================================
///@group Enter Game
///@{

bool
ClientHandler::enterGame(U32 playerInstId)
{
	SAFE_CALL_ACCOUNT(enterGame(playerInstId));	
}
//@}
///========================================================================
///@group Enter Game
///@{

bool
ClientHandler::transforScene(S32 sceneId)
{
	SAFE_CALL_ACCOUNT(transforScene(sceneId));
}

///@}

///========================================================================
///@group
///@{

bool
ClientHandler::learnSkill(U32 skid)
{
	SAFE_CALL_ACCOUNT(learnSkill(skid));
	return true;
}

bool 
ClientHandler::forgetSkill(U32 skid)
{
	SAFE_CALL_ACCOUNT(forgetSkill(skid));
	return true;
}

bool 
ClientHandler::syncOrder(COM_Order& order)
{
	SAFE_CALL_ACCOUNT(syncOrder(order));
}

bool
ClientHandler::syncOrderTimeout()
{
	SAFE_CALL_ACCOUNT(syncOrderTimeout());
}

bool
ClientHandler::setBattlebaby(U32 babyID, B8 isBattle)
{
	SAFE_CALL_ACCOUNT(setBattlebaby(babyID,isBattle));
}

bool
ClientHandler::changProp(U32 guid, std::vector< COM_Addprop >& props)
{
	SAFE_CALL_ACCOUNT(changProp(guid,props));
}

bool
ClientHandler::acceptQuest(S32 questId)
{
	SAFE_CALL_ACCOUNT(acceptQuest(questId));
	return true;
}

bool
ClientHandler::submitQuest(S32 npcId, S32 questId)
{
	SAFE_CALL_ACCOUNT(submitQuest(npcId, questId));
	return true;
}

bool
ClientHandler::submitQuest2(S32 npcId, S32 questId, int32 instId)
{
	SAFE_CALL_ACCOUNT(submitQuest2(npcId, questId, instId));
	return true;
}

bool
ClientHandler::giveupQuest(S32 questId)
{
	SAFE_CALL_ACCOUNT(giveupQuest(questId));
	return true;
}

bool
ClientHandler::mining(S32 gatherId,int32 times)
{
	SAFE_CALL_ACCOUNT(mining(gatherId,times));
	return true;
}

bool 
ClientHandler::compoundItem(S32 itemId,S32 gemId)
{
	SAFE_CALL_ACCOUNT(compoundItem(itemId,gemId));
	return true;
}

//JJC
bool
ClientHandler::requestChallenge(STRING& name)
{
	if (FilterWord::strHasSymbols(name))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(requestChallenge(name));
	return true;
}

bool
ClientHandler::requestRival()
{
	SAFE_CALL_ACCOUNT(requestRival());
	return true;
}

bool
ClientHandler::requestJJCRank()
{
	SAFE_CALL_ACCOUNT(requestJJCRank());
	return true;
}

bool
ClientHandler::requestLevelRank()
{
	SAFE_CALL_ACCOUNT(requestLevelRank());
	return true;
}

bool
ClientHandler::requestPlayerFFRank()
{
	SAFE_CALL_ACCOUNT(requestPlayerFFRank());
	return true;
}

bool
ClientHandler::requestBabyRank()
{
	SAFE_CALL_ACCOUNT(requestBabyRank());
	return true;
}

bool
ClientHandler::requestEmpRank()
{
	SAFE_CALL_ACCOUNT(requestEmpRank());
	return true;
}

bool
ClientHandler::requestMySelfJJCData()
{
	SAFE_CALL_ACCOUNT(requestMySelfJJCData());
	return true;
}

bool
ClientHandler::requestCheckMsg(STRING& name)
{
	if (FilterWord::strHasSymbols(name))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(requestCheckMsg(name));
	return true;
}

bool
ClientHandler::requestMyAllbattleMsg()
{
	SAFE_CALL_ACCOUNT(requestMyAllbattleMsg());
	return true;
}


bool
ClientHandler::guideFinish(U64 num)
{
	SAFE_CALL_ACCOUNT(guideFinish(num));
	return true;
}

bool
ClientHandler::enterBattle(S32 battleId)
{
	SAFE_CALL_ACCOUNT(enterBattle(battleId));
	return true;
}

bool 
ClientHandler::getFirstRechargeItem()
{
	SAFE_CALL_ACCOUNT(getFirstRechargeItem());
	return true;
}

bool 
ClientHandler::shopBuyItem(S32 id, S32 num)
{
	SAFE_CALL_ACCOUNT(shopBuyItem(id,num));
	return true;
}

bool
ClientHandler::babyLearnSkill(U32 instId, U32 oldSkId, U32 newSkId,U32 newSkLv)
{
	SAFE_CALL_ACCOUNT(babyLearnSkill(instId,oldSkId,newSkId,newSkLv));
	return true;
}

bool
ClientHandler::setCurrentTitle(S32 title)
{
	SAFE_CALL_ACCOUNT(setCurrentTitle(title));
	return true;
}

bool
ClientHandler::openBuyBox()
{
	SAFE_CALL_ACCOUNT(openBuyBox());
	return true;
}

bool
ClientHandler::requestAchaward(S32 achId)
{
	SAFE_CALL_ACCOUNT(requestAchaward(achId));
	return true;
}

bool
ClientHandler::sign(S32 index)
{
	SAFE_CALL_ACCOUNT(sign(index));
	return true;
}

bool
ClientHandler::requestSignupReward7()
{
SAFE_CALL_ACCOUNT(requestSignupReward7());
}

bool
ClientHandler::requestSignupReward14()
{
SAFE_CALL_ACCOUNT(requestSignupReward14());
}


bool
ClientHandler::requestSignupReward28()
{
SAFE_CALL_ACCOUNT(requestSignupReward28());
}

bool
ClientHandler::requestActivityReward(S32 itemid)
{
	SAFE_CALL_ACCOUNT(requestActivityReward(itemid));
	return true;
}

bool
ClientHandler::resetHundredTier()
{
	SAFE_CALL_ACCOUNT(resetHundredTier());
	return true;
}

bool
ClientHandler::resetBaby(S32 instId)
{
	SAFE_CALL_ACCOUNT(resetBaby(instId));
	return true;
}

bool
ClientHandler::resetBabyProp(S32 instId)
{
	SAFE_CALL_ACCOUNT(resetBabyProp(instId));
	return true;
}

bool
ClientHandler::setOpenDoubleTimeFlag(bool isFlag)
{
	SAFE_CALL_ACCOUNT(setOpenDoubleTimeFlag(isFlag));
	return true;
}

bool
ClientHandler::delBaby(S32 instId)
{
	SAFE_CALL_ACCOUNT(delBaby(instId));
	return true;
}

bool
ClientHandler::requestMyJJCTeamMsg()
{
	SAFE_CALL_ACCOUNT(requestMyJJCTeamMsg());
	return true;
}

bool
ClientHandler::startMatching()
{
	SAFE_CALL_ACCOUNT(startMatching());
	return true;
}

bool
ClientHandler::stopMatching()
{
	SAFE_CALL_ACCOUNT(stopMatching());
	return true;
}

bool
ClientHandler::jjcBattleGo(U32 id)
{
	SAFE_CALL_ACCOUNT(jjcBattleGo(id));
	return true;
}

bool
ClientHandler::exitPvpJJC()
{
	SAFE_CALL_ACCOUNT(exitPvpJJC());
	return true;
}

bool
ClientHandler::requestpvprank()
{
	SAFE_CALL_ACCOUNT(requestpvprank());
	return true;
}

bool
ClientHandler::joinWarriorchoose()
{
	SAFE_CALL_ACCOUNT(joinWarriorchoose());
	return true;
}

bool
ClientHandler::warriorStart()
{
	SAFE_CALL_ACCOUNT(warriorStart());
	return true;
}

bool
ClientHandler::warriorStop()
{
	SAFE_CALL_ACCOUNT(warriorStop());
	return true;
}

bool ClientHandler::sendMail(STRING& playername, STRING& title, STRING& content)
{
	if (FilterWord::strHasSymbols(playername))
	{
		errorno(EN_FilterWord);
		return true;
	}
	if (FilterWord::strHasSymbols(title))
	{
		errorno(EN_FilterWord);
		return true;
	}
	if (FilterWord::strHasSymbols(content))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(sendMail(playername,title,content));
	return true;
}
bool ClientHandler::readMail(S32 mailid){
	SAFE_CALL_ACCOUNT(readMail(mailid));
return true;
}
bool ClientHandler::delMail(S32 mailId){
	SAFE_CALL_ACCOUNT(delMail(mailId));
return true;
}
bool ClientHandler::getMailItem(S32 mailId){
	SAFE_CALL_ACCOUNT(getMailItem(mailId));
	return true;
}

bool ClientHandler::joinPvpLobby(){
	SAFE_CALL_ACCOUNT(joinPvpLobby());
	return true;
}
bool ClientHandler::exitPvpLobby(){
	SAFE_CALL_ACCOUNT(exitPvpLobby());
	return true;
}
bool ClientHandler::openZhuanpan(){
	return true;
}

bool
ClientHandler::requestState()
{
	SAFE_CALL_ACCOUNT(requestState());
	return true;
}

bool
ClientHandler::createGuild(std::string& guildName)
{
	if (FilterWord::strHasSymbols(guildName))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(createGuild(guildName));
	return true;
}

bool
ClientHandler::queryGuildList(S16 page)
{
	SAFE_CALL_ACCOUNT(queryGuildList(page));
	return true;
}

bool
ClientHandler::acceptRequestGuild(S32 playerId)
{
	SAFE_CALL_ACCOUNT(acceptRequestGuild(playerId));
	return true;
}

bool
ClientHandler::refuseRequestGuild(S32 playerId)
{
	SAFE_CALL_ACCOUNT(refuseRequestGuild(playerId));
	return true;
}

bool
ClientHandler::requestJoinGuild(U32 guid)
{
	SAFE_CALL_ACCOUNT(requestJoinGuild(guid));
	return true;
}

bool
ClientHandler::leaveGuild()
{
	SAFE_CALL_ACCOUNT(leaveGuild());
	return true;
}

bool
ClientHandler::transferPremier(S32 targetId)
{
	SAFE_CALL_ACCOUNT(transferPremier(targetId));
	return true;
}

bool
ClientHandler::changeMemberPosition(S32 targetId, GuildJob job)
{
	SAFE_CALL_ACCOUNT(changeMemberPosition(targetId,job));
	return true;
}

bool
ClientHandler::changeGuildNotice(std::string& notice)
{
	if (FilterWord::strHasSymbols(notice))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(changeGuildNotice(notice));
	return true;
}

bool
ClientHandler::delGuild(U32 guildId)
{
	SAFE_CALL_ACCOUNT(delGuild(guildId));
	return true;
}

bool
ClientHandler::kickOut(S32 guid)
{
	SAFE_CALL_ACCOUNT(kickOut(guid));
	return true;
}

bool
ClientHandler::inviteJoinGuild(std::string& playerName)
{
	if (FilterWord::strHasSymbols(playerName))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(inviteJoinGuild(playerName));
	return true;
}

bool
ClientHandler::respondInviteJoinGuild(std::string& sendName)
{
	if (FilterWord::strHasSymbols(sendName))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(respondInviteJoinGuild(sendName));
	return true;
}

bool ClientHandler::buyGuildItem(S32 tableId,int32 times){
	SAFE_CALL_ACCOUNT(buyGuildItem(tableId,times));
	return true;
}

bool ClientHandler::guildsign(){
	SAFE_CALL_ACCOUNT(guildsign());
	return true;
}

bool ClientHandler::fetchSelling(COM_SearchContext& context){
	SAFE_CALL_ACCOUNT(fetchSelling(context));
	return true;
}
bool ClientHandler::fetchSelling2(COM_SearchContext& context){
	SAFE_CALL_ACCOUNT(fetchSelling2(context));
	return true;
}
bool ClientHandler::selling(S32 iteminstid, S32 babyinstid, S32 price){
	SAFE_CALL_ACCOUNT(selling(iteminstid,babyinstid,price));
	return true;
}
bool ClientHandler::unselling(S32 sellid){
	SAFE_CALL_ACCOUNT(unselling(sellid));
	return true;
}
bool ClientHandler::buy(S32 sellid){
	SAFE_CALL_ACCOUNT(buy(sellid));
	return true;
}

bool ClientHandler::delEmployee(std::vector< U32 >& emps)
{
	SAFE_CALL_ACCOUNT(delEmployee(emps));
	return true;
}

bool
ClientHandler::onekeyDelEmp()
{
	SAFE_CALL_ACCOUNT(onekeyDelEmp());
	return true;
}

bool
ClientHandler::delEmployeeSoul(U32 instid, U32 soulNum)
{
	SAFE_CALL_ACCOUNT(delEmployeeSoul(instid,soulNum));
	return true;
}

bool
ClientHandler::enterHundredScene(S32 level){
	SAFE_CALL_ACCOUNT(enterHundredScene(level));
	return true;
}
bool
ClientHandler::fixItem(S32 instId, FixType type)
{
	SAFE_CALL_ACCOUNT(fixItem(instId,type));
	return true;
}
bool
ClientHandler::fixAllItem(std::vector< U32 >& items, FixType type)
{
	SAFE_CALL_ACCOUNT(fixAllItem(items,type));
	return true;
}

bool
ClientHandler::levelUpMagicItem(std::vector< U32 >& items)
{
	SAFE_CALL_ACCOUNT(levelUpMagicItem(items));
	return true;
}

bool
ClientHandler::magicItemOneKeyLevel()
{
	SAFE_CALL_ACCOUNT(magicItemOneKeyLevel());
	return true;
}

bool
ClientHandler::tupoMagicItem(S32 level)
{
	SAFE_CALL_ACCOUNT(tupoMagicItem(level));
	return true;
}

bool
ClientHandler::changeMagicJob(JobType job)
{
	SAFE_CALL_ACCOUNT(changeMagicJob(job));
	return true;
}

bool
ClientHandler::makeDebirsItem(S32 instId,S32 num)
{
	SAFE_CALL_ACCOUNT(makeDebirsItem( instId, num));
	return true;
}

bool
ClientHandler::remouldBaby(S32 instid)
{
	SAFE_CALL_ACCOUNT(remouldBaby(instid));
	return true;
}

bool
ClientHandler::depositItemToStorage(U32 instid)
{
	SAFE_CALL_ACCOUNT(depositItemToStorage(instid));
	return true;
}

bool
ClientHandler::depositBabyToStorage(U32 instid)
{
	SAFE_CALL_ACCOUNT(depositBabyToStorage(instid));
	return true;
}

bool
ClientHandler::storageItemToBag(U32 instid)
{
	SAFE_CALL_ACCOUNT(storageItemToBag(instid));
	return true;
}

bool
ClientHandler::storageBabyToPlayer(U32 instid)
{
	SAFE_CALL_ACCOUNT(storageBabyToPlayer(instid));
	return true;
}

bool
ClientHandler::sortStorage(StorageType tp)
{
	SAFE_CALL_ACCOUNT(sortStorage(tp));
	return true;
}

bool
ClientHandler::delStorageBaby(U32 instid)
{
	SAFE_CALL_ACCOUNT(delStorageBaby(instid));
	return true;
}

bool
ClientHandler::queryOnlinePlayerbyName(std::string& name)
{
	if (FilterWord::strHasSymbols(name))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(queryOnlinePlayerbyName(name));
	return true;
}

bool
ClientHandler::queryPlayerbyName(std::string& name)
{
	if (FilterWord::strHasSymbols(name))
	{
		errorno(EN_FilterWord);
		return true;
	}
	SAFE_CALL_ACCOUNT(queryPlayerbyName(name));
	return true;
}

bool
ClientHandler::queryBaby(U32 instId)
{
	SAFE_CALL_ACCOUNT(queryBaby(instId));
	return true;
}

bool
ClientHandler::queryEmployee(U32 instId)
{
	SAFE_CALL_ACCOUNT(queryEmployee(instId));
	return true;
}

bool
ClientHandler::requestPk(U32 playerId)
{
	SAFE_CALL_ACCOUNT(requestPk(playerId));
	return true;
}

bool
ClientHandler::uiBehavior(UIBehaviorType type){
	//SAFE_CALL_ACCOUNT(uiBehavior(type));
	return true;
}

bool
ClientHandler::entryGuildBattle(){
	SAFE_CALL_ACCOUNT(entryGuildBattle());
	return true;
}

bool
ClientHandler::transforGuildBattleScene(){
	SAFE_CALL_ACCOUNT(transforGuildBattleScene());
	return true;
}

bool
ClientHandler::zhuanpanGo(U32 counter)
{
	SAFE_CALL_ACCOUNT(zhuanpanGo(counter));
	return true;
}

bool
ClientHandler::intensifyBaby(U32 babyid)
{
	SAFE_CALL_ACCOUNT(intensifyBaby(babyid));
	return true;
}

bool
ClientHandler::redemptionSpree(std::string& code)
{
	SAFE_CALL_ACCOUNT(redemptionSpree(code));
	return true;
}

bool
ClientHandler::sceneFilter(std::vector<SceneFilterType>& sft){
	SAFE_CALL_ACCOUNT(sceneFilter(sft));
	return true;
};

bool
ClientHandler::exitCopy()
{
	SAFE_CALL_ACCOUNT(exitCopy());
	return true;
}

bool
ClientHandler::sendExamAnswer(U32 questionIndex, U8 answer)
{
	SAFE_CALL_ACCOUNT(sendExamAnswer(questionIndex,answer));
	return true;
}

bool
ClientHandler::sendwishing(COM_Wishing& wish)
{
	SAFE_CALL_ACCOUNT(sendwishing(wish));
	return true;
}

bool
ClientHandler::requestWish()
{
	SAFE_CALL_ACCOUNT(requestWish());
	return true;
}

bool
ClientHandler::leaderCloseDialog()
{
	SAFE_CALL_ACCOUNT(leaderCloseDialog());
	return true;
}

bool
ClientHandler::updateGuildBuiling(GuildBuildingType gb){
	SAFE_CALL_ACCOUNT(updateGuildBuiling(gb));
	return true;
}
bool
ClientHandler::addGuildMoney(int32 money){
	SAFE_CALL_ACCOUNT(addGuildMoney(money));
	return true;
}
bool
ClientHandler::refreshGuildShop(){
	SAFE_CALL_ACCOUNT(refreshGuildShop());
	return true;
}

bool
ClientHandler::levelupGuildSkill(int32 skId){
	SAFE_CALL_ACCOUNT(levelupGuildSkill(skId));
	return true;
}

bool
ClientHandler::presentGuildItem(int32 num){
	SAFE_CALL_ACCOUNT(presentGuildItem(num));
	return true;
}
bool
ClientHandler::progenitusAddExp(int32 monsterId,bool isSuper){
	SAFE_CALL_ACCOUNT(progenitusAddExp(monsterId,isSuper));
	return true;
}
bool
ClientHandler::setProgenitusPosition(int32 mId,int32 pos){
	SAFE_CALL_ACCOUNT(setProgenitusPosition(mId,pos));
	return true;
}

bool
ClientHandler::requestOnlineReward(U32 index){
	SAFE_CALL_ACCOUNT(requestOnlineReward(index));
	return true;
}

bool
ClientHandler::requestFundReward(U32 level){
	SAFE_CALL_ACCOUNT(requestFundReward(level));
	return true;
}

bool
ClientHandler::openCard(U16 index){
	SAFE_CALL_ACCOUNT(openCard(index));
	return true;
}

bool
ClientHandler::resetCard(){
	SAFE_CALL_ACCOUNT(resetCard());
	return true;
}

bool
ClientHandler::hotRoleBuy(){
	SAFE_CALL_ACCOUNT(hotRoleBuy());
	return true;
}

bool ClientHandler::requestSevenReward(U32 qid){
	SAFE_CALL_ACCOUNT(requestSevenReward(qid));
	return true;
}

bool ClientHandler::requestChargeTotalSingleReward(uint32 index){
	SAFE_CALL_ACCOUNT(requestChargeTotalSingleReward(index));
	return true;
}
//累计充值
bool ClientHandler::requestChargeTotalReward(uint32 index){
	SAFE_CALL_ACCOUNT(requestChargeTotalReward(index));
	return true;
}
bool ClientHandler::requestChargeEverySingleReward(uint32 index){
	SAFE_CALL_ACCOUNT(requestChargeEverySingleReward(index));
	return true;
}
//单笔充值
bool ClientHandler::requestChargeEveryReward(uint32 index){
	SAFE_CALL_ACCOUNT(requestChargeEveryReward(index));
	return true;
}
//累计登录
bool ClientHandler::requestLoginTotal(uint32 index)	{
	SAFE_CALL_ACCOUNT(requestLoginTotal(index));
	return true;
}
bool ClientHandler::buyDiscountStoreSingle(int32 itemId, int32 itemStack){
	SAFE_CALL_ACCOUNT(buyDiscountStoreSingle(itemId,itemStack));
	return true;
}
//打折商店
bool ClientHandler::buyDiscountStore(int32 itemId, int32 itemStack){
	SAFE_CALL_ACCOUNT(buyDiscountStore(itemId,itemStack));
	return true;
}
//累计抽伙伴
bool ClientHandler::requestEmployeeActivityReward(U32 index){
	SAFE_CALL_ACCOUNT(requestEmployeeActivityReward(index));
	return true;
}

bool ClientHandler::vipreward(){
	SAFE_CALL_ACCOUNT(vipreward());
	return true;
}

bool ClientHandler::familyLoseLeader(){
	SAFE_CALL_ACCOUNT(familyLoseLeader());
	return true;
}

bool ClientHandler::verificationSMS(std::string& phoneNumber,std::string& code){
	SAFE_CALL_ACCOUNT(verificationSMS(phoneNumber,code));
	return true;
}
bool ClientHandler::lockItem(int32 instId, bool isLock){
	SAFE_CALL_ACCOUNT(lockItem(instId,isLock));
	return true;
}
bool ClientHandler::lockBaby(int32 instId, bool isLock){
	SAFE_CALL_ACCOUNT(lockBaby(instId,isLock));
	return true;
}
bool ClientHandler::showBaby(int32 instId){
	SAFE_CALL_ACCOUNT(showBaby(instId));
	return true;
}

bool ClientHandler::requestmyselfrechargeleReward(uint32 index){
	SAFE_CALL_ACCOUNT(requestmyselfrechargeleReward(index));
	return true;
}

bool ClientHandler::requestLevelGift(int32 level){
	SAFE_CALL_ACCOUNT(requestLevelGift(level));
	return true;
}

bool ClientHandler::wearFuwen(int slot){
	SAFE_CALL_ACCOUNT(wearFuwen(slot));
	return true;
}
bool ClientHandler::takeoffFuwen(int slot){
	SAFE_CALL_ACCOUNT(takeoffFuwen(slot));
	return true;
}
bool ClientHandler::compFuwen(int itemInstId){
	SAFE_CALL_ACCOUNT(compFuwen(itemInstId));
	return true;
}

bool ClientHandler::requestEmployeeQuest(){
	SAFE_CALL_ACCOUNT(requestEmployeeQuest());
	return true;
}
bool ClientHandler::acceptEmployeeQuest(int32 questId, std::vector<int32> & employees){
	SAFE_CALL_ACCOUNT(acceptEmployeeQuest(questId, employees));
	return true;
}
bool ClientHandler::submitEmployeeQuest(int32 questId){
	SAFE_CALL_ACCOUNT(submitEmployeeQuest(questId));
	return true;
}
bool ClientHandler::crystalUpLevel(){
	SAFE_CALL_ACCOUNT(crystalUpLevel());
	return true;
}
bool ClientHandler::resetCrystalProp(std::vector< int32 >& locklist){
	SAFE_CALL_ACCOUNT(resetCrystalProp(locklist));
	return true;
}
bool ClientHandler::requestEverydayIntegral(){
	SAFE_CALL_ACCOUNT(requestEverydayIntegral());
	return true;
}
bool ClientHandler::buyIntegralItem(U32 id, U32 num){
	SAFE_CALL_ACCOUNT(buyIntegralItem(id,num));
	return true;
}

bool Account::requestEmployeeQuest(){
	if(!player_)
		return true;
	player_->requestEmployeeQuest();
	return true;
}
bool Account::acceptEmployeeQuest(int32 questId, std::vector<int32>& employees){
	if(!player_)
		return true;
	player_->acceptEmployeeQuest(questId,employees);
	return true;
}
bool Account::submitEmployeeQuest(int32 questId){
	if(!player_)
		return true;
	player_->submitEmployeeQuest(questId);
	return true;
}
bool Account::crystalUpLevel(){
	if(!player_)
		return true;
	player_->crystalUp();
	return true;
}
bool Account::resetCrystalProp(std::vector< int32 >& locklist){
	if(!player_)
		return true;
	player_->resetCrystalProp(locklist);
	return true;
}
bool Account::requestEverydayIntegral(){
	if(!player_)
		return true;
	player_->updateIntegral();
	return true;
}
bool Account::buyIntegralItem(U32 id, U32 num){
	if(!player_)
		return true;
	player_->integralShopBuy(id,num);
	return true;
}

///@}