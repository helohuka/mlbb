
#include "config.h"
#include "account.h"
#include "client.h"
#include "player.h"
#include "GMCmdMgr.h"
#include "worldserv.h"
#include "dbhandler.h"
#include "loginhandler.h"
#include "battle.h"
#include "Scene.h"
#include "team.h"
#include "pvpJJC.h"
#include "Guild.h"
#include "GameEvent.h"
#include "FilterWord.h"
#include "Activity.h"
#include "EndlessStair.h"
#include "pkActity.h"
#include "Guild.h"
#include "Activities.h"
#include "loghandler.h"
#include "exam.h"
#include "lastOrder.h"
#define SAFE_CALL_PLAYER(fun) do{if(NULL!=player_) player_->fun; }while(0)
#define SAFE_CALL_CLIENT(fun) do{if(NULL!=client_) client_->fun; }while(0)


std::map<std::string, VerifiedAccount> VerifiedAccount::cache_;

VerifiedAccount* 
VerifiedAccount::get(std::string& name)
{
	if(cache_.find(name) == cache_.end())
		return NULL;
	return &cache_[name];	
}

void 
VerifiedAccount::add(VerifiedAccount va)
{
	ACE_DEBUG((LM_DEBUG,"VerifiedAccount::add %s\n",va.username_.c_str()));
	cache_[va.username_] = va;
}

void 
VerifiedAccount::del(std::string& name)
{
	ACE_DEBUG((LM_DEBUG,"VerifiedAccount::del %s\n",name.c_str()));
	std::map<std::string, VerifiedAccount>::iterator itr = cache_.find(name);
	if(cache_.end() == itr)
		return;
	cache_.erase(itr);
}

void 
VerifiedAccount::del(ClientHandler *client)
{
	for (std::map<std::string, VerifiedAccount>::iterator itr=cache_.begin();itr!=cache_.end();++itr)
	{
		if(itr->second.client_ == client)
		{
			cache_.erase(itr);
			return;
		}
	}
}

std::map<std::string, Account*>	Account::sessionStore_;
std::map<std::string,Account*> Account::accountStore_;

Account::Account(ClientHandler *handler)
:gmlev_(GML_All)
,state_(None)
,username_("")
,password_("")
,client_(handler)
,player_(NULL)
,logintime_(0)
,logouttime_(0)
,createtime_(0)
,sessiontime_(0)
{
	SRV_ASSERT(client_);
	client_->account_ = this;
	ipaddr_ = client_->ip_;
	serverId_ = client_->serverId_;
	lastping_ = WorldServ::instance()->curTime_;
	
}

Account::~Account()
{
	ACE_DEBUG((LM_DEBUG,"Account::~Account() %s\n",username_.c_str()));
	logout();
};


///========================================================================
///@group
///@{
bool
Account::requestPhoto()
{
	if(!client_)
		return false;
	/// BUG 1 重复 2 下线问题

	/*if(player_->battleId_ != 0)
		return true;

	WorldServ::instance()->tmpBattle_.push_back(player_);*/
	//state_ = S_SessionLogin;
	ACE_DEBUG((LM_DEBUG,"Account::requestPhoto %s\n",username_.c_str()));

	if(miniPlayerList_.empty())
	{
		DBHandler::instance()->queryPlayerSimpleInformation(username_,client_->serverId_);
	}
	else
	{
		COM_ReconnectInfo info;
		info.reconnectProcess_ = RECT_LoginOk;
		info.sessionKey_ = sessionkey_;
		info.roles_ = miniPlayerList_;
		if(player_ != NULL)
		{
			player_->visiblePlayers_.clear();
			info.reconnectProcess_ = RECT_EnterGameOk;
	
			Festival::request(player_);
			ReversalCard::request(player_);

			RechargeTotal::request(player_);
			DiscountStore::request(player_);
			RechargeSingle::request(player_);

			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfChargeTotal) * ONE_DAY_SEC)
			{
				RechargeTotal::requestSelf(player_);
			}
			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfDiscountStore) * ONE_DAY_SEC)
			{
				DiscountStore::requestSelf(player_);
			}
			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfChargeEvery) * ONE_DAY_SEC)
			{
				RechargeSingle::requestSelf(player_);
			}
			
			HotShop::request(player_);
			MinGift::request(player_);
			EmployeeActivityTotal::request(player_);
			
			player_->getPlayerInst(info.playerInst_);
			player_->getSceneInfo(info.sceneInfo_);

			Team*p = player_->myTeam();
			if(p)
				p->getTeamInfo(info.team_);
				
			Battle* pbattle = player_->myBattle();
			if(pbattle)
			{
				info.reconnectProcess_ = RECT_EnterBattleOk;
				pbattle->getInitData(info.initBattle_,player_->getGUID());
			}
			
		}
		SAFE_CALL_CLIENT(reconnection(info));
		if(player_ != NULL)
		{
			player_->initSignup();
			player_->initBabies();
			player_->initNpc();
			player_->initQuest();
			player_->initSelling();
			player_->initMail();
			
			DayliActivity::reqeust(player_);
			Guild::memberOnline(player_);
		}
	}
	return true;
}

bool
Account::openvip(VipLevel vl)
{	
	if(!player_)
		return true;
	if(vl >= VL_Max || vl <= VL_None)
		return true;
	player_->setProp(PT_VipLevel,vl);
	player_->setProp(PT_VipTime,2592000);
	WorldServ::instance()->updateContactInfo(player_);
	return true;
}

bool
Account::ping()
{
	time_t curtime = WorldServ::instance()->curTime_;
	lastping_ = curtime;
	SAFE_CALL_PLAYER(ping());
	sessiontime_ = SESSION_VALID_TIMEOUT;
	return true;
}

void 
Account::reConnect(ClientHandler* cl)
{	
	//state_ = S_SessionLogin;
	ACE_DEBUG((LM_DEBUG,"Account reconnect %s\n",username_.c_str()));
	client_ = cl;
	client_->account_ = this;
	ipaddr_ = client_->ip_;
	if(miniPlayerList_.empty())
	{
		DBHandler::instance()->queryPlayerSimpleInformation(username_,client_->serverId_);
	}
	else
	{
		COM_ReconnectInfo info;
		info.reconnectProcess_ = RECT_LoginOk;
		info.sessionKey_ = sessionkey_;
		info.roles_ = miniPlayerList_;
		if(player_ != NULL)
		{
			Zhuanpan::request(player_);
			Festival::request(player_);
			RechargeTotal::request(player_);
			DiscountStore::request(player_);
			RechargeSingle::request(player_);
			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfChargeTotal) * ONE_DAY_SEC)
			{
				RechargeTotal::requestSelf(player_);
			}
			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfDiscountStore) * ONE_DAY_SEC)
			{
				DiscountStore::requestSelf(player_);
			}
			if(player_->createTime_ - WorldServ::instance()->curTime_ > Global::get<int>(C_SelfChargeEvery) * ONE_DAY_SEC)
			{
				RechargeSingle::requestSelf(player_);
			}
			HotShop::request(player_);
			MinGift::request(player_);
			EmployeeActivityTotal::request(player_);
			IntegralShop::request(player_);
			Zhuanpan::request(player_);

			player_->reattachClient();
			info.reconnectProcess_ = RECT_EnterGameOk;
			player_->getPlayerInst(info.playerInst_);
			player_->getSceneInfo(info.sceneInfo_);
			if(player_->getProp(PT_GuildID) != 0)
			{
				Guild::memberOnline(player_);
			}

			Team*p = player_->myTeam();
			if(p){
				p->getTeamInfo(info.team_);
			}

			if(player_->isBattle()){
				Battle* pbattle = Battle::find(player_->battleId_);
				if(pbattle){
					info.reconnectProcess_ = RECT_EnterBattleOk;
					pbattle->getInitData(info.initBattle_,player_->getGUID());
				}
			}
		}
		SAFE_CALL_CLIENT(reconnection(info));

		if(player_ != NULL){
			player_->initSignup();
			player_->initBabies();
			player_->initNpc();
			player_->initQuest();
			player_->initSelling();
			player_->initMail();
			CALL_CLIENT(player_,updateRandSubmitQuestCount(player_->submitQuestCount_));
			Guild::memberOnline(player_);
			DayliActivity::reqeust(player_);
		}
	}
}

void
Account::deConnect(bool needDisconnect)
{
	ACE_DEBUG((LM_DEBUG,"Account deconnect %s\n",username_.c_str()));
	SAFE_CALL_PLAYER(deattachClient());
	SAFE_CALL_PLAYER(save());
	if(client_){
		client_->account_ = NULL;
		if(needDisconnect)
			GatewayHandler::instance()->disconnect(client_);
		client_ = NULL;
		
	}
}

bool Account::sessionlogin(COM_LoginInfo& info){return true;}
bool Account::login(COM_LoginInfo& info){return true;}

bool Account::logout()
{
	if(player_){
		for(size_t i=0; i<miniPlayerList_.size(); ++i){
			if(miniPlayerList_[i].instId_ == player_->playerId_){
				player_->getSimpleInfo(miniPlayerList_[i]);
				break;
			}
		}
	}
	SAFE_CALL_PLAYER(logout());
	player_ = NULL;
	ACE_DEBUG((LM_DEBUG,"Account logout %s\n",username_.c_str()));
	WorldServ::instance()->pushLoginLog(this);

	SAFE_CALL_CLIENT(loginok(sessionkey_,miniPlayerList_));
	return true;
}

bool
Account::requestBag()
{
	if(player_ == NULL)
		return true;
	player_->initBagItem();
	return true;
}

bool
Account::requestEmployees()
{
	if(player_ == NULL)
		return true;
	player_->initEmployees(true);
	return true;
}

bool
Account::requestStorage(StorageType tp)
{
	if(player_ == NULL)
		return true;
	if(tp == ST_Item)
		player_->initItemStorage();
	else if(tp == ST_Baby)
		player_->initBabyStorage();
	return true;
}

bool
Account::requestAchievement()
{
	if(player_ == NULL)
		return true;
	player_->initAchievement();
	return true;
}

bool
Account::initminig()
{
	if(player_ == NULL)
		return true;
	player_->initGather();
	return true;
}
bool
Account::requestCompound()
{
	if(player_ == NULL)
		return true;
	player_->initCompound();
	return true;
}

bool
Account::learnSkill(U32 skid)
{
	SAFE_CALL_PLAYER(learnSkill(skid));
	return true;
}

bool
Account::forgetSkill(U32 skid)
{
	SAFE_CALL_PLAYER(forgetSkill(skid));
	return true;
}

bool
Account::sendChat(COM_Chat& content, STRING& targetName)
{
	SAFE_CALL_PLAYER(sendChat(content,targetName.c_str()));
	return true;
}

bool
Account::requestAudio(int32 audioId){
	SAFE_CALL_PLAYER(requestAudio(audioId));
	return true;
}


bool
Account::publishItemInst(ItemContainerType type, U32 itemInstId, ChatKind chatType, std::string& playerName)
{
	SAFE_CALL_PLAYER(publishItemInst(type,itemInstId,chatType,playerName));
	return true;
}

bool
Account::queryItemInst(S32 showId)
{
	SAFE_CALL_PLAYER(queryItemInst(showId));
	return true;
}

bool
Account::publishbabyInst(ChatKind type,U32 babyInstId, std::string& playerName)
{
	SAFE_CALL_PLAYER(publishbabyInst(type,babyInstId,playerName));
	return true;
}

bool
Account::querybabyInst(S32 showId)
{
	SAFE_CALL_PLAYER(querybabyInst(showId));
	return true;
}

bool 
Account::useItem(U32 slot, U32 target, U32 stack)
{
	SAFE_CALL_PLAYER(useItem(slot,target,stack));
	return true;
}

bool 
Account::wearEquipment(U32 target, U32 bagSlot)
{
	SAFE_CALL_PLAYER(wearEquipment(target,bagSlot));
	return true;
}

bool
Account::delEquipment(U32 target, U32 itemInstId)
{
	SAFE_CALL_PLAYER(delEquipment(target,itemInstId));
	return true;
}

bool 
Account::setPlayerFront(bool isFront)
{
	SAFE_CALL_PLAYER(setPlayerFront(isFront));
	return true;
}

bool
Account::transforScene(S32 sceneId)
{
	SAFE_CALL_PLAYER(transforScene(sceneId));
	return true;
}

bool
Account::talkedNpc(S32 npcId){
	SAFE_CALL_PLAYER(talkedNpc(npcId));
	return true; 
}

bool 
Account::createTeam(COM_CreateTeamInfo& cti)
{
	if(NULL == player_)
		return true;
	
	if(cti.name_.empty())
		return true;
	
	if(FilterWord::strHasSymbols(cti.name_))
		return true;
	
	if(player_->isTeamMember())
		return true;
	Team* t = TeamLobby::instance()->apply();
	t->create(player_,cti);
	return true;
}
bool 
Account::changeTeam(COM_CreateTeamInfo& info)
{
	if(NULL == player_)
		return true;

	if(!player_->isTeamMember())
		return true;

	Team* p = player_->myTeam();

	if(p == NULL)
		return true;

	p->change(player_,info);

	return true;
}
bool 
Account::kickTeamMember(U32 uuid)
{
	SAFE_CALL_PLAYER(kickTeamMember(uuid));
	return true;
}
bool 
Account::changeTeamLeader(U32 uuid)
{
	if(NULL == player_)
		return true;

	Team* p = player_->isTeamLeader();

	if(p == NULL)
		return true;
	
	if(Warriorchoose::instance()->isWarriorchoose(p->teamId_))
		Warriorchoose::instance()->teamWarriorstop(p);

	p->changeTeamLeader(uuid);

	return true;
}

bool
Account::joinTeam(U32 teamId, STRING& pwd)
{
	SAFE_CALL_PLAYER(joinTeam(teamId,pwd));
	return true;
}

bool
Account::exitTeam()
{
	SAFE_CALL_PLAYER(exitTeam());
	return true;
}

bool
Account::inviteTeamMember(STRING& name)
{
	if(NULL == player_)
		return true;
	
	
	Team* p = player_->myTeam();
	if(p == NULL)
		return true;


	p->inviteTeamMember(player_,name);

	return true;
}

bool
Account::isjoinTeam(U32 teamId,B8 isFlag)
{
	if(NULL == player_)
		return true;

	player_->isjoinTeam(teamId,isFlag);

	return true;
}

bool
Account::changeTeamPassword(STRING& pwd)
{
	if(NULL == player_)
		return true;

	if(!player_->isTeamMember())
		return true;

	Team* p =  TeamLobby::instance()->getTeam(player_,player_->teamId_);

	if(p == NULL)
		return true;
	p->changeTeamPassword(pwd);

	return true;
}

bool 
Account::jointLobby()
{
	if(NULL == player_)
		return true;

	TeamLobby::instance()->joinLobby(player_);
	return true;
}

bool 
Account::exitLobby()
{
	if(NULL == player_)
		return true;

	TeamLobby::instance()->exitLobby(player_);
	return true;
}

bool
Account::joinTeamRoom()
{
	return true;
}

bool
Account::leaveTeam(){
	SAFE_CALL_PLAYER(leaveTeam());
	return true;
}

bool
Account::backTeam(){
	SAFE_CALL_PLAYER(backTeam());
	return true;
}

bool
Account::refuseBackTeam(){
	SAFE_CALL_PLAYER(refuseBackTeam());
	return true;
}

bool
Account::teamCallMember(S32 playerId){
	SAFE_CALL_PLAYER(teamCallMember(playerId));
	return true;
}

bool
Account::requestJoinTeam(std::string& targetName)
{
	SAFE_CALL_PLAYER(requestJoinTeam(targetName));
	return true;
}

bool
Account::ratifyJoinTeam(std::string& sendName)
{
	SAFE_CALL_PLAYER(ratifyJoinTeam(sendName));
	return true;
}

bool
Account::move(float x, float z)
{
	COM_FPosition pos;
	pos.x_ = x;
	pos.z_ = z;
	SAFE_CALL_PLAYER(move(pos));
	return true;
}

bool 
Account::moveToNpc(S32 npcid)
{
	SAFE_CALL_PLAYER(moveToNpc(npcid));
	return true;
}

bool 
Account::moveToNpc2(NpcType type)
{
	SAFE_CALL_PLAYER(moveToNpc2(type));
	return true;
}

bool 
Account::moveToZone(S32 sceneId, S32 zoneId)
{
	SAFE_CALL_PLAYER(moveToZone(sceneId,zoneId));
	return true;
}

bool 
Account::autoBattle(){
	SAFE_CALL_PLAYER(autoBattle());
	return true;
}
bool
Account::sceneLoaded(){
	SAFE_CALL_PLAYER(sceneLoaded());
	return true;
}

bool
Account::querySimplePlayerInst(U32 playerId){
	SAFE_CALL_PLAYER(queryPlayerInst(playerId));
	return true;
}

bool
Account::stopAutoBattle(){
	SAFE_CALL_PLAYER(stopAutoBattle());
	return true;
}


bool
Account::stopMove(){
	SAFE_CALL_PLAYER(stopMove());
	return true;
}

bool
Account::sortBagItem()
{
	SAFE_CALL_PLAYER(sortBagItem());
	return true;
}

bool
Account::sellBagItem(U32 instId, U32 num)
{
	SAFE_CALL_PLAYER(sellBagItem(instId,num));
	return true;
}

bool
Account::drawLotteryBox(BoxType type,bool isFree)
{
	SAFE_CALL_PLAYER(drawLotteryBox(type,isFree));
	return true;
}

bool 
Account::setBattleEmp(U32 empID, EmployeesBattleGroup group, B8 isBattle)
{
	SAFE_CALL_PLAYER(setBattleEmployee(empID,group,isBattle));
	return true;
}

bool
Account::changeEmpBattleGroup(EmployeesBattleGroup group)
{
	SAFE_CALL_PLAYER(changeEmpBattleGroup(group));
	return true;
}

bool
Account::requestContactInfoById(U32 instId)
{
	SAFE_CALL_PLAYER(findContactInfoById(instId));
	return true;
}

bool
Account::requestContactInfoByName(STRING& name)
{
	SAFE_CALL_PLAYER(findContactInfoByName(name.c_str()));
	return true;
}
bool
Account::addFriend(U32 instId)
{
	SAFE_CALL_PLAYER(addFriend(instId));
	return true;
}

bool
Account::delFriend(U32 instId)
{
	SAFE_CALL_PLAYER(delFriend(instId));
	return true;
}

bool
Account::addBlacklist(U32 instId)
{
	SAFE_CALL_PLAYER(addBlacklist(instId));
	return true;
}

bool
Account::delBlacklist(U32 instId)
{
	SAFE_CALL_PLAYER(delBlacklist(instId));
	return true;
}

bool
Account::requestFriendList()
{
	SAFE_CALL_PLAYER(requestFriendList());
	return true;
}

bool
Account::requestReferrFriend()
{
	SAFE_CALL_PLAYER(referrFriend());
	return true;
}

bool
Account::changeBabyName(U32 babyID, STRING& name)
{
	if(NULL == player_)
		return true;

	Baby* pBaby = player_->findBaby(babyID);

	if(NULL == pBaby)
		return true;

	pBaby->changeBabyName(name.c_str());

	return true;
}

bool 
Account::bagItemSplit(S32 instId,S32 splitNum)
{
	SAFE_CALL_PLAYER(bagItemSplit(instId,splitNum));
	return true;
}

//JJC
bool
Account::requestChallenge(STRING& name)
{
	SAFE_CALL_PLAYER(startChallenge(name));
	return true;
}

bool
Account::requestRival()
{
	SAFE_CALL_PLAYER(findRival());
	return true;
}

bool
Account::requestJJCRank()
{
	SAFE_CALL_PLAYER(requestJJCRank());
	return true;
}

bool
Account::requestLevelRank()
{
	SAFE_CALL_PLAYER(requestLevelRank());
	return true;
}

bool
Account::requestPlayerFFRank()
{
	SAFE_CALL_PLAYER(requestPlayerFFRank());
	return true;
}

bool
Account::requestMySelfJJCData()
{
	SAFE_CALL_PLAYER(requestJJCData());
	return true;
}

bool
Account::requestBabyRank()
{
	SAFE_CALL_PLAYER(requestBabyRank());
	return true;
}

bool
Account::requestEmpRank()
{
	SAFE_CALL_PLAYER(requestEmpRank());
	return true;
}

bool
Account::requestCheckMsg(STRING& name)
{
	SAFE_CALL_PLAYER(checkMsg(name));
	return true;
}

bool
Account::requestMyAllbattleMsg()
{
	SAFE_CALL_PLAYER(requestAllBttleMsg());
	return true;
}

///@}

///========================================================================
///@group Inside Function
///@{

COM_SimpleInformation* 
Account::findDBPlayerById(U32 playerId)
{
	for(size_t i=0; i<miniPlayerList_.size(); ++i)
	{
		if(miniPlayerList_[i].instId_ == playerId)
			return &miniPlayerList_[i];
	}
	return NULL;
}

COM_SimpleInformation* 
Account::findDBPlayerByName(std::string& name)
{
	for(size_t i=0; i<miniPlayerList_.size(); ++i)
	{
		if(miniPlayerList_[i].instName_ == name)
			return &miniPlayerList_[i];
	}

	return NULL;
}

void
Account::deleteDBPlayer(COM_SimpleInformation* p)
{
	for (size_t i=0; i<miniPlayerList_.size();++i)
	{
		if(&miniPlayerList_[i] == p)
		{
			miniPlayerList_.erase(miniPlayerList_.begin() + i);
			return;
		}
	}
}
///@}

///========================================================================
///@group
///@{

void 
Account::getPlayerMini(std::vector<COM_SimpleInformation>& mini)
{
	/*for(size_t i=0; i<dbPlayerCache_.size(); ++i)
	{
		COM_SimpleInformation info;
		info.instId_ = dbPlayerCache_[i].instId_;
		info.level_ = dbPlayerCache_[i].properties_[PT_Level];
		info.asset_id_ = dbPlayerCache_[i].properties_[PT_AssetId];
		info.instName_	= dbPlayerCache_[i].instName_;
		info.jt_ = (JobType)(S32)dbPlayerCache_[i].properties_[PT_Profession];
		info.jl_ = dbPlayerCache_[i].properties_[PT_ProfessionLevel];
		for (size_t k=0; k<dbPlayerCache_[i].equips_.size(); ++k)
		{
			if(dbPlayerCache_[i].equips_[k].slot_ == ES_SingleHand)
				info.weaponItemId_ = dbPlayerCache_[i].equips_[k].itemId_;
			else if(dbPlayerCache_[i].equips_[k].slot_ == ES_DoubleHand)
				info.weaponItemId_ = dbPlayerCache_[i].equips_[k].itemId_;
			if(dbPlayerCache_[i].equips_[k].slot_ == ES_Fashion)
				info.fashionId_ = dbPlayerCache_[i].equips_[k].itemId_;
		}

		mini.push_back(info);
	}*/
}

void Account::setDBMiniPlayers(std::vector<COM_SimpleInformation>& players){
	miniPlayerList_ = players;
	SAFE_CALL_CLIENT(loginok(sessionkey_,miniPlayerList_));
}



//void Account::updateDBPlayer(SGE_DBPlayerData& data){
//	for(size_t i=0; i<dbPlayerCache_.size(); ++i)
//	{
//		if(dbPlayerCache_[i].instId_ == data.instId_){
//			dbPlayerCache_[i] = data;
//			return;
//		}
//	}
//}

void
Account::createPlayerSameName(){
	state_ = S_Normal;
	CALL_CLIENT(this,errorno(EN_PlayerNameSame));
}

void 
Account::createPlayerOk(SGE_DBPlayerData &inst)
{
	state_ = S_Normal;
	if(inst.properties_.empty())
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Create player is error template cannot find\n")));
		return ;
	}
	COM_SimpleInformation info;
	info.instId_ = inst.instId_;
	info.level_ = inst.properties_[PT_Level];
	info.asset_id_ = inst.properties_[PT_AssetId];
	info.instName_	= inst.instName_;
	info.jt_ = (JobType)(int)inst.properties_[PT_Profession];
	info.jl_ = inst.properties_[PT_ProfessionLevel];
	miniPlayerList_.push_back(info);
	SAFE_CALL_CLIENT(createPlayerOk(info));
	if(!username_.empty())
	{
		std::vector<std::string> tmp = String::Split(username_,"=",2);
		if(tmp.size() == 2){
			tmp[0] = std::string("-") + tmp[1];
		}else {
			tmp[0] = std::string("-") + tmp[0];
		}
		WorldServ::instance()->reqCDKey(tmp[0],inst.instName_,inst.gft_); ///请求充值返利
	}
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Create player is ok \n")));
}
///@}

///========================================================================
///@group
///@{
bool
Account::createPlayer(STRING& playername, U8 playerTmpId)
{
	if(miniPlayerList_.size() >= MAX_PLAYERS){
		ACE_DEBUG((LM_INFO,"Can not create player more than 3 (%s)\n",username_.c_str()));
		return true;
	}

	if(state_ == S_NeedDBBack){
		return true;
	}

	if(playername.empty())
	{
		ACE_DEBUG((LM_INFO,"Can not create player name empty %s\n",username_.c_str()));
		SAFE_CALL_CLIENT(errorno(EN_FilterWord));
		return true;
	}

	if(NULL == client_){
		return true;
	}

	//判断角色是否有标点符号
	if (FilterWord::strHasSymbols(playername))
	{
		ACE_DEBUG((LM_INFO,"Can not create player name by symbols %s %s\n",username_.c_str(),playername.c_str()));
		SAFE_CALL_CLIENT(errorno(EN_FilterWord));
		return true;
	}
	//判断角色名称内是否有屏蔽字
	if (FilterWord::replace(playername))
	{
		ACE_DEBUG((LM_INFO,"Can not create player name error %s %s\n",username_.c_str(),playername.c_str()));
		SAFE_CALL_CLIENT(errorno(EN_FilterWord));
		return true;
	}

	Player *player = Player::getPlayerByName(playername);
	if (player!=NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Create player same name %s %s \n"),username_.c_str(),playername.c_str()));
		SAFE_CALL_CLIENT(errorno(EN_PlayerNameSame));
		return true;
	}

	COM_ContactInfo* dbplayer = WorldServ::instance()->findContactInfo(playername);
	if(dbplayer!=NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Create player db same name %s %s \n"),username_.c_str(),playername.c_str()));
		SAFE_CALL_CLIENT(errorno(EN_PlayerNameSame));
		return true;
	}
	
	PlayerTmpTable::Core const *tmp = PlayerTmpTable::getTemplateById(playerTmpId);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Create player cant find tamplate %s %s %d\n"),username_.c_str(),playername.c_str(),playerTmpId));
		return true;
	}

	SGE_DBPlayerData inst;
	Player::genDBPlayerInst(playerTmpId,playername,inst);
	if(miniPlayerList_.empty() && Env::get<int>(V_OpenRMBBack) != 0){
		std::vector<std::string> accs = String::Split(username_,"=");
		if(accs.size() == 2){
			/*LastOrderTable::Data const* pData =	LastOrderTable::getDataByAccountName(accs[1]);
			if(pData){
				inst.properties_[PT_MagicCurrency] = pData->payment_ * 20;
			}*/
			}
		}
	DBHandler::instance()->createPlayer(username_,inst,client_->serverId_);	
	state_ = S_NeedDBBack;

	return true;
}

bool
Account::deletePlayer(STRING& playername)
{
	if(player_ && player_->playerName_ == playername)
	{
		player_->logout();
		
		player_ = NULL;
	}

	COM_SimpleInformation* dbplayer = findDBPlayerByName(playername);
	if(dbplayer==NULL){
		return true;
	}

	WorldServ::instance()->deleteRank(playername);

	Guild* pGuild = Guild::findGuildByPlayer(dbplayer->instId_);
	if(pGuild){
		Guild::leave(dbplayer->instId_);
	}
	
	WorldServ::instance()->delContactInfo(dbplayer->instId_);
	EndlessStair::instance()->deleteplayerReCalcRank(playername);
	deleteDBPlayer(dbplayer);

	DBHandler::instance()->deletePlayer(playername);

	SAFE_CALL_CLIENT(deletePlayerOk(playername));
	ACE_DEBUG((LM_ERROR,ACE_TEXT("DELETE PLAYER OK PLAYERNAME[%s] ACC[%s]\n"),playername.c_str(),username_.c_str()));
	return true;
}

bool
Account::enterGame(U32 playerInstId)
{
	if(client_ == NULL){
		return true;
	}
	if(player_!=NULL) return true; /// 这里返回人物界面处理 
	COM_SimpleInformation* dbplayer = findDBPlayerById(playerInstId);
	if(NULL == dbplayer)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Enter game cant find player %s %d \n"),username_.c_str(),playerInstId));
		return true;
	}

	if(state_ == S_NeedDBBack){
		return true;
	}
	
	/*if(dbplayer->freeze_){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Enter game cant use freeze player %s %d \n"),username_.c_str(),playerInstId));
		return false;

	}
	if(dbplayer->seal_){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Enter game cant use seal player %s %d \n"),username_.c_str(),playerInstId));
		return false;
	}*/
	
	DBHandler::instance()->queryPlayer(username_,dbplayer->instId_);
	state_ = S_NeedDBBack;
	//player_ = Player::createPlayer(this,*dbplayer,client_->serverId_);
	//SRV_ASSERT(player_);		
	//SAFE_CALL_PLAYER(login());
	
	return true;
}

void Account::enterGame(SGE_DBPlayerData& dbplayer){
	state_ = S_Normal;
	if(!client_)
		return;
	player_ = Player::createPlayer(this,dbplayer,client_->serverId_);
	SRV_ASSERT(player_);		
	SAFE_CALL_PLAYER(login());
}

bool
Account::syncOrder(COM_Order& order)
{
	SAFE_CALL_PLAYER(syncOrder(order));
	return true;
}

bool
Account::syncOrderTimeout()
{
	if(NULL == player_) return true;
	Battle *pBattle = Battle::find(player_->battleId_);
	if(NULL == pBattle)
		return true;
	
	pBattle->pushOrderTimeout(player_->getGUID());
	return true;
}


bool
Account::setBattlebaby(U32 babyID, B8 isBattle)
{
	SAFE_CALL_PLAYER(selectBaby(babyID,isBattle));
	return true;
}

bool
Account::changProp(U32 guid, std::vector< COM_Addprop >& props)
{
	SAFE_CALL_PLAYER(addProperty(guid,props));
	return true;
}

///@}

Account*
Account::createAccount(ClientHandler *ch,const COM_AccountInfo& accinfo)
{
	enum {
		MASK = 0X56,
	};
	if(!ch){
		return NULL;
	}
	std::stringstream sstream;
	sstream << accinfo.username_ << accinfo.password_ << accinfo.guid_ << ch->serverId_ << ACE_OS::gettimeofday().get_msec();
	std::string sessionkey = sstream.str();
	/*for(size_t i=0; i<sessionkey.size(); ++i){
		sessionkey[i] = sessionkey[i] ^ MASK;
	}*/
	SRV_ASSERT(getAccountBySessionkey(sessionkey) == NULL);
	Account *ret = NEW_MEM(Account,ch);
	SRV_ASSERT(ret);
	ret->username_ = accinfo.username_;
	ret->password_ = accinfo.password_;
	ret->guid_	   = accinfo.guid_;
	ret->sessiontime_ = SESSION_VALID_TIMEOUT;
	ret->sessionkey_ = sessionkey;
	ret->createtime_ = accinfo.createtime_;
	ret->phoneNumber_ = accinfo.phoneNumber_;
	ret->logintime_ = WorldServ::instance()->curTime_;
	sessionStore_[ret->sessionkey_] = ret;
	accountStore_[ret->username_] = ret;
	return ret;
}

Account*
Account::getAccountBySessionkey(std::string const &sessionkey)
{
	std::map<std::string,Account*>::iterator itr = sessionStore_.find(sessionkey);
	if(itr==sessionStore_.end())
		return NULL;
	return itr->second;
}

Account*
Account::getAccountByName(std::string const &strName)
{
	std::map<std::string,Account*>::iterator itr = accountStore_.find(strName);
	if(itr==accountStore_.end())
		return NULL;
	return itr->second;
}

void Account::removeAccount(Account* acc){
	{
	std::map<std::string,Account*>::iterator itr = sessionStore_.find(acc->sessionkey_);
	if(itr!=sessionStore_.end()) sessionStore_.erase(itr);
	}
	{
		std::map<std::string,Account*>::iterator itr = accountStore_.find(acc->username_);
		if(itr!=accountStore_.end()) accountStore_.erase(itr);
	}
}

void 
Account::removeAccountBySessionkey(std::string const &sessionkey)
{
	std::map<std::string,Account*>::iterator itr = sessionStore_.find(sessionkey);
	if(itr==sessionStore_.end())
		return;
	Account* p = itr->second;
	accountStore_.erase(accountStore_.find(p->username_));
	sessionStore_.erase(itr);
	if(!p->isOffline()){
		p->deConnect();
	}
	DEL_MEM(p);
}

void 
Account::removeAccountByName(std::string const& accName)
{
	std::map<std::string,Account*>::iterator itr = accountStore_.find(accName);
	if(itr==accountStore_.end())
		return;
	Account* p = itr->second;
	sessionStore_.erase(sessionStore_.find(p->sessionkey_));
	accountStore_.erase(itr);
	if(!p->isOffline()){
		p->deConnect();
	}
	DEL_MEM(p);
}

void
Account::update(float dt)
{
	std::map< std::string, Account* >::iterator itr = sessionStore_.begin(); 
	while(itr != sessionStore_.end())
	{
		if(itr->second)
		{
			itr->second->sessiontime_ -= dt;
			
			if(itr->second->sessiontime_ <= 0)
			{
				removeAccountBySessionkey((itr++)->second->sessionkey_);
				continue;
			}
		}
		++itr;
	}
}

void 
Account::clean()
{
	std::map< std::string, Account* >::iterator itr = accountStore_.begin(), end = accountStore_.end();
	
	while(itr != end)
	{
		if(itr->second)
		{
			if(itr->second->client_){
				itr->second->client_->account_ = NULL;
				//delete itr->second->client_;
			}
			DEL_MEM(itr->second);
		}
		++itr;
	}

	accountStore_.clear();
	sessionStore_.clear();
}

bool
Account::acceptQuest(S32 questId)
{
	SAFE_CALL_PLAYER(prepareAcceptQuest(questId));
	return true;
}

bool
Account::submitQuest(S32 npcId, S32 questId)
{
	SAFE_CALL_PLAYER(prepareSubmitQuest(npcId, questId,0));
	return true;
}

bool
Account::submitQuest2(S32 npcId, S32 questId, int32 instId)
{
	SAFE_CALL_PLAYER(prepareSubmitQuest(npcId, questId,instId));
	return true;
}

bool
Account::giveupQuest(S32 questId)
{
	SAFE_CALL_PLAYER(giveupQuest(questId));
	return true;
}

bool
Account::mining(S32 itemId,int32 times)
{
	SAFE_CALL_PLAYER(mining(itemId,times));
	return true;
}

bool 
Account::compoundItem(S32 itemId,S32 gemId)
{
	if(NULL == player_)
		return true;
	player_->makeItem(itemId,gemId);
	return true;
}

bool
Account::requestEvolve(U32 empInstId)
{
	SAFE_CALL_PLAYER(empEvolve(empInstId));
	return true;
}

bool
Account::requestUpStar(U32 empInstId)
{
	SAFE_CALL_PLAYER(empUpstar(empInstId));
	return true;
}

bool
Account::empSkillLevelUp(U32 empId, S32 skillId)
{
	SAFE_CALL_PLAYER(empSkillLevelUp(empId,skillId));
	return true;
}

bool
Account::requestDelEmp(U32 empInstId){
	SAFE_CALL_PLAYER(delEmployee(empInstId));
	return true;
}

bool
Account::guideFinish(U64 guideIdx){
	if(NULL == player_)
		return true;
	player_->guideIdx_ = guideIdx;
	return true;
}

bool
Account::enterBattle(S32 battleId){
	//ACE_DEBUG((LM_DEBUG,"Client say enter battle %d\n",battleId));
	SAFE_CALL_PLAYER(enterBattle(battleId));
	return true;
}

bool 
Account::getFirstRechargeItem(){
	SAFE_CALL_PLAYER(getFirstRechargeItem());
	return true;
}

bool 
Account::shopBuyItem(S32 id, S32 num){
	SAFE_CALL_PLAYER(buyShopItem(id,num));
	return true;
}

bool
Account::babyLearnSkill(U32 instId, U32 oldSkId, U32 newSkId,U32 newSkLv)
{
	SAFE_CALL_PLAYER(babyLearnSkill(instId,oldSkId,newSkId,newSkLv));
	return true;
}

bool
Account::setCurrentTitle(S32 title)
{
	SAFE_CALL_PLAYER(setCurrentTitle(title));
	return true;
}

bool
Account::openBuyBox()
{
	SAFE_CALL_PLAYER(openBuyBox());
	return true;
}

bool
Account::requestAchaward(S32 achId)
{
	SAFE_CALL_PLAYER(requestAchaward(achId));
	return true;
}

bool
Account::sign(S32 index)
{
	SAFE_CALL_PLAYER(sign(index));
	return true;
}

bool
Account::requestSignupReward7()
{
	return true;
}

bool
Account::requestSignupReward14()
{
	return true;
}


bool
Account::requestSignupReward28()
{
	return true;
}

bool
Account::requestActivityReward(S32 itemid)
{
	SAFE_CALL_PLAYER(requestActivityReward(itemid));
	return true;
}

bool
Account::resetHundredTier()
{
	SAFE_CALL_PLAYER(resetHundredTier());
	return true;
}

bool
Account::resetBaby(S32 instId)
{
	SAFE_CALL_PLAYER(resetBaby(instId));
	return true;
}

bool
Account::resetBabyProp(S32 instId)
{
	//SAFE_CALL_PLAYER(resetBabyProp(instId));
	return true;
}

bool
Account::setOpenDoubleTimeFlag(bool isFlag)
{
	SAFE_CALL_PLAYER(setOpenDoubleTimeFlag(isFlag));
	return true;
}

bool
Account::delBaby(S32 instId)
{
	ACE_DEBUG((LM_INFO,"Account delete baby %s %d\n",username_.c_str(),instId));
	SAFE_CALL_PLAYER(delBaby(instId));
	return true;
}

bool
Account::requestMyJJCTeamMsg()
{
	SAFE_CALL_PLAYER(syncMyJJCTeamMsg());
	return true;
}

bool
Account::startMatching()
{
	return true;
	if(NULL == player_)
		return true;
	if(player_->isTeamMember())
	{
		Team* p =  TeamLobby::instance()->getTeam(player_,player_->teamId_);
		if(p == NULL)
			return true;
		if(!p->isTeamLeader(player_))
			return true;

		for (size_t i = 0; i < p->getMemberSize(); ++i){
			if(p->teamMembers_[i]->getProp(PT_Level) < Global::get<int>(C_JJCOpenlevel))
				return true;
		}
		PvpJJC::startMatching(p);
	}
	else{
		PvpJJC::startMatching(player_);
	}
	return true;
}

bool
Account::stopMatching()
{
	return true;
	if(NULL == player_)
		return true;
	if(player_->isTeamMember())
	{
		Team* p =  TeamLobby::instance()->getTeam(player_,player_->teamId_);
		if(p == NULL)
			return true;
		if(!p->isTeamLeader(player_))
			return true;

		PvpJJC::stopMatching(p);
	}
	else{
		PvpJJC::stopMatching(player_);
	}

	return true;
}

bool
Account::jjcBattleGo(U32 id)
{
	if(NULL == player_)
		return true;
	player_->jjcBattleGo(id);
	return true;
}

bool
Account::exitPvpJJC()
{
	if(NULL == player_)
		return true;
	if(player_->isTeamMember())
	{
		Team* p = TeamLobby::instance()->getTeam(player_,player_->teamId_);
		if(p == NULL)
			return true;
		if(!p->isTeamLeader(player_))
			return true;

		PvpJJC::exitTeamPvpJJC(p);
	}
	else{
		PvpJJC::exitSingleJJC(player_->getGUID());
	}

	return true;
}

bool
Account::requestpvprank()
{
	if(player_ == NULL)
		return true;

	std::vector<COM_ContactInfo> infos;
	WorldServ::instance()->getContactInfos(5,infos);
	CALL_CLIENT(player_,requestpvprankOK(infos));
	return true;
}

bool
Account::joinWarriorchoose()
{
	if(player_ == NULL)
		return true;
	Team* pteam = player_->isTeamLeader();
	if(pteam == NULL)
		return true;
	pteam->openWarriorchooseUI();
	return true;
}

bool
Account::warriorStart()
{
	if(player_ == NULL)
		return true;
	Team* pteam = player_->isTeamLeader();
	if(pteam == NULL)
		return true;
	Warriorchoose::instance()->start(pteam->teamId_);
	return true;
}

bool
Account::warriorStop()
{
	if(player_ == NULL)
		return true;
	Team* pteam = player_->isTeamLeader();
	if(pteam == NULL)
		return true;
	Warriorchoose::instance()->stop(pteam->teamId_);
	return true;
}

bool Account::sendMail(STRING& playername, STRING& title, STRING& content){
	SAFE_CALL_PLAYER(sendMail(playername,title,content));
	return true;
}

bool Account::readMail(S32 mailid){
	SAFE_CALL_PLAYER(readMail(mailid));
	return true;
}

bool Account::delMail(S32 mailId){
	SAFE_CALL_PLAYER(delMail(mailId));
	return true;
}

bool Account::getMailItem(S32 mailId){
	SAFE_CALL_PLAYER(getMailItem(mailId));
	return true;
}

bool Account::joinPvpLobby(){
	return true;
}
bool Account::exitPvpLobby(){
	return true;
}
bool Account::openZhuanpan(){
	return true;
}

bool
Account::requestState()
{
	if(player_ == NULL)
		return true;
	player_->syncState();

	return true;
}

bool
Account::createGuild(std::string& guildName)
{
	if(player_ == NULL)
		return true;

	Guild::createGuild(player_,guildName);
	
	return true;
}

bool
Account::queryGuildList(S16 page)
{
	if(player_ == NULL)
		return true;
	Guild::queryGuildList(player_,page);
	return true;
}

bool
Account::acceptRequestGuild(S32 playerId)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::acceptRequestGuild(player_,playerId);

	return true;
}

bool
Account::refuseRequestGuild(S32 playerId)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::refuseRequestGuild(player_,playerId);

	return true;
}

bool
Account::requestJoinGuild(U32 guid)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::requestJoinGuild(player_, guid);

	return true;
}

bool
Account::leaveGuild()
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::leave(player_);

	return true;
}

bool
Account::transferPremier(S32 targetId)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::transferPremier(player_,targetId);

	return true;
}

bool
Account::changeMemberPosition(S32 targetId, GuildJob job)
{
	if(player_ == NULL)
		return true;

	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::changePosition(player_,targetId,job);

	return true;
}

bool
Account::changeGuildNotice(std::string& notice)
{
	if(player_ == NULL)
		return true;
	Guild::changeNotice(player_,notice);
	return true;
}

bool
Account::delGuild(U32 guildId)
{
	if(player_ == NULL)
		return true;

	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild* pGuild = player_->myGuild();
	if (!pGuild)
		return true;
	if (pGuild->guildData_.guildId_ != guildId)
		return true;
	COM_GuildMember* pMember = player_->myGuildMember();
	if (!pMember)
		return true;
	if(pMember->job_ != GJ_Premier)
		return true;
	Guild::delGuild(guildId);
	return true;
}

bool
Account::kickOut(S32 guid)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}

	Guild::kickOut(player_,guid);

	return true;
}

bool
Account::inviteJoinGuild(std::string& playerName)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::inviteJoinGuild(player_, playerName);

	return true;
}

bool
Account::respondInviteJoinGuild(std::string& sendName)
{
	if(player_ == NULL)
		return true;
	if(Guild::battleState_ != Guild::BS_Close)
	{
		SAFE_CALL_CLIENT(errorno(EN_TeamMemberHourLess24));
		return true;
	}
	Guild::respondInviteJoinGuild(sendName,player_->playerId_);
	return true;
}


bool Account::buyGuildItem(S32 tableId,int32 times){
	SAFE_CALL_PLAYER(buyGuildItem(tableId,times));
	return true;
}

bool Account::guildsign(){
	if(player_ == NULL)
		return true;
	Guild* guild = player_->myGuild();
	if(guild == NULL)
		return true; 
	guild->guildsign(player_);
	return true;
}

bool Account::fetchSelling(COM_SearchContext& context){
	SAFE_CALL_PLAYER(fetchSell(context,false));
	return true;
}
bool Account::fetchSelling2(COM_SearchContext& context){
	SAFE_CALL_PLAYER(fetchSell(context,true));
	return true;
}

bool Account::selling(S32 iteminstid, S32 babyinstid, S32 price){
	SAFE_CALL_PLAYER(sell(iteminstid,babyinstid,price));
	return true;
}

bool Account::unselling(S32 sellid){
	SAFE_CALL_PLAYER(unsell(sellid));
	return true;
}

bool Account::buy(S32 guid){
	SAFE_CALL_PLAYER(buy(guid));
	return true;
}

bool Account::delEmployee(std::vector< U32 >& emps)
{
	SAFE_CALL_PLAYER(dismissalEmployees(emps));	
	return true;
}

bool
Account::onekeyDelEmp()
{
	SAFE_CALL_PLAYER(removeEmployee());
	return true;
}

bool
Account::delEmployeeSoul(U32 instid, U32 soulNum)
{
	SAFE_CALL_PLAYER(dismissalSoul(instid,soulNum));
	return true;
}

bool
Account::enterHundredScene(S32 level){
	SAFE_CALL_PLAYER(enterHundredScene(level));
	return true;
}
bool
Account::fixItem(S32 instId,FixType type)
{
	SAFE_CALL_PLAYER(fixEquipment(instId,type));
	return true;
}
bool
Account::fixAllItem(std::vector< U32 >& items, FixType type)
{
	SAFE_CALL_PLAYER(fixAllItem(items,type));
	return true;
}

bool
Account::levelUpMagicItem(std::vector< U32 >& items)
{
	SAFE_CALL_PLAYER(levelUpMagicItem(items));
	return true;
}

bool
Account::magicItemOneKeyLevel()
{
	SAFE_CALL_PLAYER(magicItemOneKeyLevel());
	return true;
}

bool
Account::tupoMagicItem(S32 level)
{
	SAFE_CALL_PLAYER(tupoMagicItem(level));
	return true;
}

bool
Account::changeMagicJob(JobType job)
{
	SAFE_CALL_PLAYER(changeMagicJob(job));
	return true;
}

bool
Account::makeDebirsItem(S32 instId,S32 num)
{
	SAFE_CALL_PLAYER(makeDebirsItem(instId, num));
	return true;
}

bool
Account::remouldBaby(S32 instid)
{
	SAFE_CALL_PLAYER(remouldBaby(instid));
	return true;
}

bool
Account::depositItemToStorage(U32 instid)
{
	if(player_ == NULL)
		return true;
	player_->depositItem(instid);
	return true;
}

bool
Account::depositBabyToStorage(U32 instid)
{
	if(player_ == NULL)
		return true;
	player_->depositBaby(instid);
	return true;
}

bool
Account::storageItemToBag(U32 instid)
{	
	if(player_ == NULL)
		return true;
	player_->getoutItem(instid);
	return true;
}

bool
Account::storageBabyToPlayer(U32 instid)
{
	if(player_ == NULL)
		return true;
	player_->getoutBaby(instid);
	return true;
}

bool
Account::sortStorage(StorageType tp)
{
	if(player_ == NULL)
		return true;
	if(tp == ST_Item)
		player_->sortItemStorage();
	else if(tp == ST_Baby)
		player_->sortBabyStorage();
	return true;
}

bool
Account::delStorageBaby(U32 instid)
{
	if(player_ == NULL)
		return true;
	player_->delStorageBaby(instid);
	return true;
}

bool
Account::queryOnlinePlayerbyName(std::string& name)
{
	SAFE_CALL_PLAYER(queryOnlinePlayer(name));
	return true;
}

bool
Account::queryPlayerbyName(std::string& name)
{
	SAFE_CALL_PLAYER(queryPlayerbyName(name));
	return true;
}

bool
Account::queryBaby(U32 instId)
{
	SAFE_CALL_PLAYER(queryBaby(instId));
	return true;
}

bool
Account::queryEmployee(U32 instId)
{
	SAFE_CALL_PLAYER(queryEmployee(instId));
	return true;
}

bool
Account::requestPk(U32 playerId)
{
	SAFE_CALL_PLAYER(requestPK(playerId));
	return true;
}

bool
Account::uiBehavior(UIBehaviorType type){
	SAFE_CALL_PLAYER(uiBehavior(type));
	return true;
}

bool
Account::entryGuildBattle(){
	return true;
}

bool
Account::transforGuildBattleScene(){
	SAFE_CALL_PLAYER(joinGuildBattleScene());
	return true;
}

bool
Account::zhuanpanGo(U32 counter)
{
	if(player_ == NULL)
		return true;
	Zhuanpan::randZhuanpan(player_->getNameC(),counter);
	return true;
}

bool
Account::intensifyBaby(U32 babyid)
{
	if(player_ == NULL)
		return true;
	Baby* pBaby = player_->findBaby(babyid);
	if(NULL == pBaby)
		return true;
	pBaby->intensify();
	return true;
}

bool
Account::redemptionSpree(std::string& code)
{
	if(player_ == NULL)
		return true;
	player_->exchangeGift(code);
	return true;
}

bool
Account::sceneFilter(std::vector<SceneFilterType>& sft){
	if(player_ == NULL)
		return true;
	player_->filterTypes_ = sft;
	return true;
};

bool
Account::exitCopy()
{
	if(player_ == NULL)
		return true;
	SAFE_CALL_PLAYER(exitCopy());
	return true;
}

bool
Account::sendExamAnswer(U32 questionIndex, U8 answer)
{
	if(player_ == NULL)
		return true;
	ExamTable::checkAnswer(player_,questionIndex,answer);
	return true;
}

bool
Account::sendwishing(COM_Wishing& wish)
{
	if(player_ == NULL)
		return true;
	SAFE_CALL_PLAYER(wishing(wish));
	return true;
}

bool
Account::requestWish()
{
	if(player_ == NULL)
		return true;
	SAFE_CALL_PLAYER(shareWish());
	return true;
}

bool
Account::leaderCloseDialog()
{
	if(player_ == NULL)
		return true;
	if(player_->isTeamLeader() == NULL)
		return true;
	player_->isTeamLeader()->leaderCloseDialogOk();
	return true;
}


bool
Account::updateGuildBuiling(GuildBuildingType gb){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::levelupBuilding(player_,gb);
	return true;
}

bool
Account::addGuildMoney(int32 money){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::addGuildMoney(player_,money);
	return true;
}

bool
Account::refreshGuildShop(){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::requestGuildShopItems(player_);
	return true;
}

bool
Account::levelupGuildSkill(int32 skId){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::levelupSkill(player_,skId);
	return true;
}


bool
Account::presentGuildItem(int32 num){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::presentGuildItem(player_,num);
	return true;
}

bool
Account::progenitusAddExp(int32 monsterId,bool isSuper){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::progenitusAddExp(player_,monsterId,isSuper);
	return true;
}

bool
Account::setProgenitusPosition(int32 mId,int32 pos){
	if(player_ == NULL)
		return true;
	if(player_->myGuild() == NULL)
		return true;
	Guild::setProgenitusPosition(player_,mId,pos);
	return true;
}

bool
Account::requestOnlineReward(U32 index){
	if(player_ == NULL)
		return true;
	player_->requestOnlineReward(index);
	return true;
}

bool
Account::requestFundReward(U32 level){
	if(player_ == NULL)
		return true;
	player_->buyFund(level);
	return true;
}

bool
Account::openCard(U16 index){
	if(player_ == NULL)
		return true;
	player_->openCard(index);
	return true;
}

bool
Account::resetCard(){
	if(player_ == NULL)
		return true;
	player_->resetCard();
	return true;
}

bool
Account::hotRoleBuy(){
	if(player_ == NULL)
		return true;
	player_->hotRoleBuy();
	return true;
}

bool
Account::requestSevenReward(U32 qid){
	if(player_ == NULL)
		return true;
	player_->sevenReward(qid);
	return true;
}

bool Account::requestmyselfrechargeleReward(uint32 index){
	if(player_ == NULL)
		return true;
	player_->requestmySelfRecharge(index);
	return true;
}

bool Account::requestChargeTotalSingleReward(uint32 index){
	if(player_ == NULL)
		return true;
	player_->requestSelfRecharge(index);
	return true;
}
//累计充值
bool Account::requestChargeTotalReward(uint32 index){
	if(player_ == NULL)
		return true;
	player_->requestSysRecharge(index);
	return true;
}
bool Account::requestChargeEverySingleReward(uint32 index){
	if(player_ == NULL)
		return true;
	player_->requestSelfOnceRecharge(index);
	return true;
}
//单笔充值
bool Account::requestChargeEveryReward(uint32 index){
	if(player_ == NULL)
		return true;
	player_->requestSysOnceRecharge(index);
	return true;
}
//累计登录
bool Account::requestLoginTotal(uint32 index)	{
	if(player_ == NULL)
		return true;
	player_->requestFestival(index);
	return true;
}
bool Account::buyDiscountStoreSingle(int32 itemId, int32 itemStack){
	if(player_ == NULL)
		return true;
	player_->buySelfDiscountStore(itemId,itemStack);
	return true;
}
//打折商店
bool Account::buyDiscountStore(int32 itemId, int32 itemStack){
	if(player_ == NULL)
		return true;
	player_->buySysDiscountStore(itemId,itemStack);
	return true;
}
//累计抽伙伴
bool Account::requestEmployeeActivityReward(U32 index){
	if(player_ == NULL)
		return true;
	player_->requestEmployeeActivity(index);
	return true;
}

bool Account::vipreward(){
	if(player_ == NULL)
		return true;
	player_->vipreward();
	return true;
}

bool Account::familyLoseLeader(){
	if(player_ == NULL)
		return true;
	Guild::loseLeader(player_);
	return true;
}

bool Account::verificationSMS(std::string &phoneNumber,std::string &code){
	if(player_ == NULL)
		return true;
	player_->verificationSMS(phoneNumber,code);
	return true;
}

bool Account::lockItem(int32 instId, bool isLock){
	if(player_ == NULL)
		return true;
	player_->lockItem(instId,isLock);
	return true;
}
bool Account::lockBaby(int32 instId, bool isLock){
	if(player_ == NULL)
		return true;
	player_->lockBaby(instId,isLock);
	return true;
}

bool Account::showBaby(int32 instId){
	SAFE_CALL_PLAYER(showBaby(instId));
	return true;
}

bool Account::requestLevelGift(int32 level){
	SAFE_CALL_PLAYER(requestLevelGift(level));
	return true;
}

bool Account::wearFuwen(int slot){
	SAFE_CALL_PLAYER(wearFuwen(slot));
	return true;
}
bool Account::takeoffFuwen(int slot){
	SAFE_CALL_PLAYER(takeoffFuwen(slot));
	return true;
}

bool Account::compFuwen(int itemInstId){
	SAFE_CALL_PLAYER(compFuwen(itemInstId));
	return true;
}
#undef SAFE_CALL_PLAYER
#undef SAFE_CALL_CLIENT
