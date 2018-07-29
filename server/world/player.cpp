/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

#include "config.h"
#include "account.h"
#include "player.h"
#include "client.h"
#include "battle.h"
#include "itemtable.h"
#include "DebrisTable.h"
#include "baby.h"
#include "dbhandler.h"
#include "worldserv.h"
#include "Scene.h"
#include "team.h"
#include "employeeTable.h"
#include "EmployeeConfig.h"
#include "employeeoutput.h"
#include "employee.h"
#include "ComScriptEvn.h"
#include "LotteryTable.h"
#include "LiveSkilltable.h"
#include "math.h"
#include "EndlessStair.h"
#include "profession.h"
#include "BattleData.h"
#include "Shop.h"
#include "GameEvent.h"
#include "titleTable.h"
#include "monster.h"
#include "Quest.h"
#include "challengeTable.h"
#include "PVPrunkTable.h"
#include "DailyReward.h"
#include "pvpJJC.h"
#include "PeriodEvent.h"
#include "Activity.h"
#include "DropTable.h"
#include "sttable.h"
#include "Guild.h"
#include "sceneplayer.h"
#include "scenehandler.h"
#include "ArtifactLevelTable.h"
#include "ArtifactConfigTable.h"
#include "ArtifactChangeData.h"
#include "Activities.h"
#include "Robot.h"
#include "loghandler.h"
#include "GuildData.h"
#include "Guild.h"
#include "pkActity.h"
#include "npctable.h"
#include "copyscene.h"
#include "GMCmdMgr.h"
#include "TokenParser.h"
#include "robotTable.h"
#include "EmployeeQuestSystem.h"
#include "EmployeeQuestTable.h"
#include "GameRuler.h"
#ifdef _WIN32
_CrtMemState mbegin, mend, mdiff;
#endif

class ItemStackSort
{
public:
	bool operator()(COM_Item* l, COM_Item* r)
	{
		if(l->stack_ < r->stack_)
			return true;
		else /*if(l->stack_ > r->stack_)
			return false;
		else */
			return false;
	}
};

NamePlayerMap Player::nameStore_;
IdPlayerMap   Player::idStore_;
PlayerList	  Player::store_;
Player::Player(Account *handler)
: InnerPlayer()
, isSceneLoaded_(false)
, account_(handler)
, pingTime_(0)
, loginTime_(0)
, logoutTime_(0)
, onlinetime_(0)
, onlinetimeflag_(false)
, onlinereward_(0)
, isFund_(false)
, teamId_(0)
, isLeavingTeam_(false)
, rivalTimes_(0)
, rivalWaitTimes_(0)
, promoteAward_(0)
, signs_(0)
, signFlag_(false)
, sevenflag_(false)
, fatchMailTimeout_(0)
, fatchMailId_(0)
, sellIdMax_(0)
, worldTalkTime_(0)
, initSyncEmpIndex_(-1)
, waitSell_(false)
, curTier_(0)
, magicItemLevel_(1)
, magicItemExp_(0)
, magicItemJob_(JT_Axe)
, magicTupoLevel_(-1)
, sceneId_(0)
, autoBattle_(false)
, nextDoingTime_(30)
, rollEmpCounter_(0)
, rollEmpHigh_(false)
, updateVisiblePlayersTimeout_(UPDATE_VISIBLE_PLAYERS_TIMEOUT)
, firstRechargeDiamond_(false)
, isFirstRechargeGift_(false)
, wishShareNum_(0)
, warriortrophyNum_(0)
, exitGuildTime_(0)
, noBattleTime_(0)
, gaterMaxNum_(0)
, saveFreq_(SAVE_FREQ)
, isSorting_(false)
{
	isFatchMail_ = false;
	filterTypes_.push_back(SFT_Team);
	filterTypes_.push_back(SFT_Friend);
	filterTypes_.push_back(SFT_Guild);
	filterTypes_.push_back(SFT_All);
	bagItmes_.resize(Global::get<int>(C_BagMaxSize) ,NULL);
	battleEmpsGroup1_.resize(BATTLEEMP_MAX,0);
	battleEmpsGroup2_.resize(BATTLEEMP_MAX,0);
	itemStorage_.resize(Global::get<int>(C_ItemStroageGridMax),NULL);
	babyStorage_.resize(Global::get<int>(C_BabyStroageGridMax),NULL);
	fuwen_.resize((IST_FuWenAssist-IST_Fashion)*2,NULL);
	openDoubleTimeFlag_ = false;
	isGm_ = (bool)Env::get<int>(V_GM);
	noTalkTime_ = 0;
	scenePlayer_ = NEW_MEM(ScenePlayer,this);
	copynums_.clear();
	fundtag_.clear();
}

Player::~Player(){
	//ACE_DEBUG((LM_DEBUG,"Player::~Player() %d:%s\n" ,playerId_,playerName_.c_str()));
	if(scenePlayer_)
		DEL_MEM(scenePlayer_);
}

void Player::doScript(std::string const& script){
	std::string cmdName;
	const char* pcontent = script.c_str();
	if( !TokenParser::getToken( pcontent, cmdName ) )
		return ;
	std::map< std::string, GMCmdMgr::GmCmd >::iterator iter = GMCmdMgr::GmCmds_.find( cmdName );
	if( iter == GMCmdMgr::GmCmds_.end() )
		return ;

	std::string err;
	if( !ScriptEnv::callGMCmd( getHandleId(), iter->second.scriptFunName_.c_str(), pcontent, err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("%s\n"), err.c_str() ) );
	}
}

void Player::deattachClient(){
	if(!getClient())return;
	TeamLobby::instance()->delChannel(this);
	Team* p = myTeam();
	if(p)p->removeChannel(getClient());
	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(s) s->removeChannel(getClient());
	WorldBroadcaster::instance()->removeChannel(getClient());
}

void Player::reattachClient(){
	
	TeamLobby::instance()->addChannel(getClient());
	Team* p = myTeam();
	if(p)p->addChannel(getClient());
	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(s) s->addChannel(getClient());
	WorldBroadcaster::instance()->addChannel(getClient());
	visiblePlayers_.clear();
}

void Player::saveAll(){
	for(size_t i=0; i< store_.size(); ++i){
		if(store_[i] != NULL)
			store_[i]->save();
	}
}

void Player::OnlinePlayerAddMoney(U32 money){
	for (size_t i=0;i<store_.size();++i)
	{
		store_[i]->addMoney(money);
	}
}

void 
Player::login()
{

	ACE_DEBUG((LM_DEBUG,"Player::login() BEGIN %d:%s\n" ,playerId_,playerName_.c_str()));

	float curtime = WorldServ::instance()->curTime_;
	pingTime_ = curtime;
	loginTime_ = curtime;
	float offlinetime = loginTime_ - logoutTime_;
	ACE_Date_Time loginDT, logoutDT, nowDT;
	loginDT.update(ACE_Time_Value(loginTime_));
	logoutDT.update(ACE_Time_Value(logoutTime_));
	nowDT.update(ACE_Time_Value(curtime));
	
	
	
	Festival::request(this);
	RechargeTotal::request(this);
	DiscountStore::request(this);
	RechargeSingle::request(this);
	EmployeeActivityTotal::request(this);
	MinGift::request(this);
	Zhuanpan::request(this);
	IntegralShop::request(this);
	
	if(createTime_ - curtime > Global::get<int>(C_SelfChargeTotal) * ONE_DAY_SEC)
	{
		RechargeTotal::requestSelf(this);
	}
	if(createTime_ - curtime > Global::get<int>(C_SelfDiscountStore) * ONE_DAY_SEC)
	{
		DiscountStore::requestSelf(this);
	}
	if(createTime_ - curtime > Global::get<int>(C_SelfChargeEvery) * ONE_DAY_SEC)
	{
		RechargeSingle::requestSelf(this);
	}
	MySelfRecharge::requestSelf(this);
	HotShop::request(this);
	addReputation(0,true); ///���¼���ƺ� ��;
	if(loginDT.year() != logoutDT.year() || loginDT.month() != logoutDT.month() || loginDT.day() != logoutDT.day())
	{
		signFlag_ = false;
	}
	if(loginDT.month() != loginDT.month()){
		signs_ = 0;
	}

	if(((int)properties_[PT_VipLevel])!=VL_None){
		properties_[PT_VipTime] -= offlinetime;
		if(properties_[PT_VipTime] <= 0.f)
		{
			properties_[PT_VipLevel] = VL_None;
			properties_[PT_VipTime] = 0;
			WorldServ::instance()->updateContactInfo(this);
		}
	}

	calcProperty();
	Employeeoutput::calcofflinetime(this,offlinetime);
	COM_PlayerInst inst;
	getPlayerInst(inst);
	//������ˢ����
	dirtyProp_.clear();
	if(isFirstLogin_){
		offlinetime = 0;
	}
	if(offlinetime > Global::get<int>(C_OfflineExperienceTimeMin))
	{
		float offlinetime1 = offlinetime > Global::get<int>(C_OfflineExperienceTimeMax) ? Global::get<int>(C_OfflineExperienceTimeMax) : offlinetime;
		float hour = offlinetime1 / 3600.F;
		inst.offlineExp_ = hour * Global::get<int>(C_OfflineHourExperience);
		addExp(inst.offlineExp_);
	}
	
	{
		ACE_Date_Time dt(ACE_OS::gettimeofday());
		const std::vector<S32>* rewards = DailyReward::getMonthRewards(dt.month());
		if(rewards)
		{
			CALL_CLIENT(this,initSignUp(*rewards,signs_,false,false,false));
		}
	}

	CALL_CLIENT(this,enterGameOk(inst));
	
	initBabies();
	initNpc();
	init();
	initSelling();
	//��ʼ��������Ϣ
	Guild::memberOnline(this);
	DayliActivity::reqeust(this);
	ReversalCard::request(this);
	cleanTeamQuest();

	for (size_t i=0; i<completeQuest_.size(); ++i)
	{
		const Quest*q = Quest::getQuestById(completeQuest_[i]);
		if(NULL == q){
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		
	}

	isFirstLogin_ = false;

	if(loginDT.year() != logoutDT.year() || loginDT.month() != logoutDT.month() || loginDT.day() != logoutDT.day())
	{
		passZeroHour();
	}
	else
	{
		CALL_CLIENT(this,initQuest(currentQuest_,completeQuest_));
	}
	
	if(getProp(PT_VipTime) > 0)
		CALL_CLIENT(this,sycnVipflag(viprewardflag_));

	//Guild::requestGuildShopItems(this);
	
	CALL_CLIENT(this,sceneFilterOk(filterTypes_));
	SceneHandler::instance()->connect(scenePlayer_);
	SGE_ScenePlayerInfo info;
	getScenePlayerInfo(info);
	scenePlayer_->initScenePlayer(info);
	
	for(size_t i=0; i<npcList_.size(); ++i){
		scenePlayer_->addNpc(npcList_[i]);
	}
	
	updateSelfRecharge();
	updateSysRecharge();
	updateSelfOnceRecharge(getProp(PT_MagicCurrency));
	updateSysOnceRecharge(getProp(PT_MagicCurrency));

	WorldBroadcaster::instance()->addChannel(getClient());
	WorldServ::instance()->updateContactInfo(this);
	ACE_DEBUG((LM_DEBUG,"Player::login() END %d:%s %d\n" ,playerId_,playerName_.c_str(),(int)getProp(PT_Diamond)));
	
	if(!myGuild()){
		cleanGuildInfomation();
	}

	COM_ContactInfo * myinfo = WorldServ::instance()->findContactInfo(playerId_);
	myinfo->isLine_ = true;
	updateItemUseTimeout(offlinetime);
	updateBabySellTimeout(offlinetime);
	

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = (WorldServ::instance()->curTime_ - createTime_)/ONE_DAY_SEC;
	GameEvent::procGameEvent(GET_Online,params,1,getHandleId());
	
	for(size_t i=0;i<blacklist_.size(); ++i){
		CALL_CLIENT(this,addBlacklistOK(blacklist_[i]));
	}

	//////////////////////////////////////////////////////////////////////////
	CALL_CLIENT(this,updateRandSubmitQuestCount(submitQuestCount_));
	//WorldServ::instance()->pushOrderLog(account_,playerId_,getProp(PT_Level),"11112312313TEST",99,TimeFormat::StrLocalTimeNow());
	
}

void
Player::logout()
{
	logoutTime_ = WorldServ::instance()->curTime_;
	ACE_DEBUG((LM_DEBUG,"Player::logout() BEGIN %d:%s\n" ,playerId_,playerName_.c_str()));

	WorldServ::instance()->pushLoginLog(this);
	openDoubleTimeFlag_ = false;

	if(isBattle())
	{
		Battle* pBattle = Battle::find(battleId_);
		if(pBattle)
			pBattle->flee(this);
	}

	Guild::memberOffline(this);
	EmployeeQuestSystem::updatePlayerEmployeeQuest(playerId_);

	save();
	freeBabies();
	freeBagItems();
	freeEmployees();
	freeItemStorage();
	freeBabyStorage();
	
	if(myTeam())
		myTeam()->exitTeam(this);

	SceneHandler::instance()->disconnect(scenePlayer_);
	Scene* s = myScene();
	if(s)
	{
		s->delPlayer(this);
		SceneBroadcaster broadcaster;
		for(size_t i=0; i<broadcastPlayers_.size(); ++i){
			Player* p = Player::getPlayerByInstId(broadcastPlayers_[i]);
			if(p && p->getClient())
			{
				broadcaster.addChannel(p->getClient());
			}
		}
		broadcaster.delFormScene(playerId_);
		
	}
	COM_ContactInfo * myinfo = WorldServ::instance()->findContactInfo(playerId_);
	myinfo->isLine_ = false;
	GameEvent::procGameEvent(GET_Offline,NULL,0,getHandleId());
	ACE_DEBUG((LM_DEBUG,"Player::logout() END %d:%s\n" ,playerId_,playerName_.c_str()));

	Player::removePlayer(this);
	
}

void Player::calcProperty(){

	PlayerTmpTable::Core const *tmp = PlayerTmpTable::getTemplateById((S32)getProp(PT_TableId));
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in player-template-table\n"),(S32)getProp(PT_TableId)));
		return ;
	}
#define __COPY_PROPERTIES(TO,FR,ID) (TO).properties_[ID] = (FR).properties_[ID];

	COM_PlayerInst inst;
	inst.properties_.resize(PT_Max);
	__COPY_PROPERTIES(inst,(*tmp),PT_NoSleep);			///����˯
	__COPY_PROPERTIES(inst,(*tmp),PT_NoPetrifaction);	///��ʯ��
	__COPY_PROPERTIES(inst,(*tmp),PT_NoDrunk);			///������
	__COPY_PROPERTIES(inst,(*tmp),PT_NoChaos);			///������
	__COPY_PROPERTIES(inst,(*tmp),PT_NoForget);			///������
	__COPY_PROPERTIES(inst,(*tmp),PT_NoPoison);			///����
	__COPY_PROPERTIES(inst,(*tmp),PT_Assassinate);		///��ɱ
	
	__COPY_PROPERTIES(inst,(*this),PT_Money);
	__COPY_PROPERTIES(inst,(*this),PT_Diamond);
	__COPY_PROPERTIES(inst,(*this),PT_MagicCurrency);			//ħ����
	__COPY_PROPERTIES(inst,(*this),PT_EmployeeCurrency);		//�����յ�
	__COPY_PROPERTIES(inst,(*this),PT_Gather);   //�ɼ��ڿ�����
	__COPY_PROPERTIES(inst,(*this),PT_Level);       ///�ȼ�  
	__COPY_PROPERTIES(inst,(*this),PT_Exp);         ///����
	__COPY_PROPERTIES(inst,(*this),PT_ConvertExp);	///�ת������
	__COPY_PROPERTIES(inst,(*this),PT_OneDayReputation); ///һ��ɻ���������
	__COPY_PROPERTIES(inst,(*this),PT_Reputation);  ///����
	__COPY_PROPERTIES(inst,(*this),PT_TableId);     ///��Դ������
	__COPY_PROPERTIES(inst,(*this),PT_AssetId);		///����ʾ��Դ
	__COPY_PROPERTIES(inst,(*this),PT_Sex);			    ///�Ա�
	__COPY_PROPERTIES(inst,(*this),PT_BagNum);    ///����������λ��
	__COPY_PROPERTIES(inst,(*this),PT_Race);		///����
	__COPY_PROPERTIES(inst,(*this),PT_Profession);	///ְҵ
	__COPY_PROPERTIES(inst,(*this),PT_ProfessionLevel); ///ְҵ�ȼ�
	///��ǰ�ƺ�
	__COPY_PROPERTIES(inst,(*this),PT_Title); 
	__COPY_PROPERTIES(inst,(*this),PT_GuildID);
	__COPY_PROPERTIES(inst,(*this),PT_Glamour); ///����ֵ
	__COPY_PROPERTIES(inst,(*this),PT_DoubleExp);	//˫������
	__COPY_PROPERTIES(inst,(*this),PT_TongjiComplateTimes); ///ͳ����ɼ��� ����
	__COPY_PROPERTIES(inst,(*this),PT_VipLevel); ///VIP �ȼ�
	__COPY_PROPERTIES(inst,(*this),PT_VipTime);		///vipʱ��

	__COPY_PROPERTIES(inst,(*this),PT_Stama); 		///���� һ������
	__COPY_PROPERTIES(inst,(*this),PT_Strength);	///����
	__COPY_PROPERTIES(inst,(*this),PT_Power); 		///ǿ��
	__COPY_PROPERTIES(inst,(*this),PT_Speed); 	 	///�ٶ�
	__COPY_PROPERTIES(inst,(*this),PT_Magic);	 	///ħ��
	__COPY_PROPERTIES(inst,(*tmp),PT_Durability);	///�;� 	

	__COPY_PROPERTIES(inst,(*tmp),PT_HpMax);		///����ֵ
	__COPY_PROPERTIES(inst,(*tmp),PT_MpMax);		///ħ��ֵ
	__COPY_PROPERTIES(inst,(*tmp),PT_Attack);		///������
	__COPY_PROPERTIES(inst,(*tmp),PT_Defense);		///������
	__COPY_PROPERTIES(inst,(*tmp),PT_Agile);		///����
	inst.properties_[PT_Spirit] = inst.properties_[PT_Reply] = 100;
	__COPY_PROPERTIES(inst,(*tmp),PT_Magicattack);	///ħ��
	__COPY_PROPERTIES(inst,(*tmp),PT_Magicdefense);///��ħ
	__COPY_PROPERTIES(inst,(*tmp),PT_Damage);		///�˺�
	__COPY_PROPERTIES(inst,(*tmp),PT_SneakAttack);	///͵Ϯ����
	__COPY_PROPERTIES(inst,(*tmp),PT_Hit);				///����  ��������
	__COPY_PROPERTIES(inst,(*tmp),PT_Dodge);			///����
	__COPY_PROPERTIES(inst,(*tmp),PT_Crit);			///��ɱ
	__COPY_PROPERTIES(inst,(*tmp),PT_counterpunch);	///����
	__COPY_PROPERTIES(inst,(*tmp),PT_Front);		///�Ƿ�ǰ�� 0���� 1ǰ��
	__COPY_PROPERTIES(inst,(*tmp),PT_Wind);		///�� ˮ������
	__COPY_PROPERTIES(inst,(*tmp),PT_Land);		///��
	__COPY_PROPERTIES(inst,(*tmp),PT_Water);		///ˮ
	__COPY_PROPERTIES(inst,(*tmp),PT_Fire);		///��
	__COPY_PROPERTIES(inst,(*tmp),PT_Free);		//���ɵ���
	__COPY_PROPERTIES(inst,(*tmp),PT_FightingForce); ///ս����
	__COPY_PROPERTIES(inst,(*this),PT_Free); ///δ�ӵ����Ե�
	CALC_PLAYER_PRO_TRANS_STAMA((inst),properties_[PT_Stama]);
	CALC_PLAYER_PRO_TRANS_STRENGTH((inst),properties_[PT_Strength]);
	CALC_PLAYER_PRO_TRANS_POWER((inst),properties_[PT_Power]);
	CALC_PLAYER_PRO_TRANS_SPEED((inst),properties_[PT_Speed]);
	CALC_PLAYER_PRO_TRANS_MAGIC((inst),properties_[PT_Magic]);


	for (size_t i=PT_None; i < PT_Max; ++i)
	{
		__COPY_PROPERTIES((*this),inst,i);
	}
#undef __COPY_PROPERTIES
	for (size_t i=0; i<equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL) continue;
		addEquipmentEffect(equipItems_[i]);
	}

	for (size_t i=0; i<fuwen_.size(); ++i){
		if(fuwen_[i] == NULL) continue;
		addEquipmentEffect(fuwen_[i]);
	}
	//����I����
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
	if(levelData  != NULL && getOpenSubSystemFlag(OSSF_MagicItem))
	{
		int levelNum = (magicTupoLevel_-5)/5;
		float num= levelNum *0.1;
		for(size_t i=0;i<levelData->propValue_.size();i++)
		{
			ArtifactLevelTable::ArtifactPropData prop =levelData->propValue_[i];
			properties_[prop.type_] += prop.value_ + prop.value_ * num;
		}
	}

	//ˮ������
	crystalUpdata(true);
	
	//��������
	resetPassiveSkill();
	calcFightingForce();

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];
	
	refreshProperty();
}

void
Player::syncProp()
{
	if(!dirtyProp_.empty())
	{
		std::vector<COM_PropValue> prop;
		
		if(dirtyProp_.size() > SYNC_PROP_MAX){
			prop.insert(prop.begin(),dirtyProp_.begin(),dirtyProp_.begin()+SYNC_PROP_MAX);
			dirtyProp_.erase(dirtyProp_.begin(),dirtyProp_.begin() + SYNC_PROP_MAX);
		}
		else {
			prop = dirtyProp_;
			dirtyProp_.clear();
		}		

		Team* t = myTeam();
		if(t) t->syncTeamDirtyProp(playerId_,prop);

		CALL_CLIENT(this, syncProperties(playerId_,prop));
	}
	
	for(size_t i=0; i<babies_.size(); ++i)
		babies_[i]->syncProp();

	for (size_t i=0; i<employees_.size(); ++i)
		employees_[i]->syncProp();
}

void
Player::ping()
{
	time_t curtime = WorldServ::instance()->curTime_;
	pingTime_ = battleAtkTime_ = curtime;
	if(getBattleBaby())
		getBattleBaby()->battleAtkTime_ = battleAtkTime_;
	CALL_CLIENT(this,pong());
}

void
Player::genDBPlayerInst(U8 playerTmpId, std::string const &playerName, SGE_DBPlayerData &out)
{
	PlayerTmpTable::Core const *tmp = PlayerTmpTable::getTemplateById(playerTmpId);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in player-template-table\n"),playerTmpId));
		return ;
	}
	out.instId_ = WorldServ::instance()->getMaxGuid();
	out.versionNumber_ = VERSION_NUMBER;
	out.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		out.properties_[i] = tmp->properties_[i];
	out.properties_[PT_Spirit] = 100; ///BUG 409 �������߹� (�Ǳ���,���ʺܸߣ�
	out.properties_[PT_Reply] = 100;
	out.properties_[PT_ProfessionLevel] = 1;
	///�ӳ�ʼ��ģ������ʼ�� 1��2 ������
	float stama	 = (float)out.properties_[PT_Stama]; 	
	float strength = (float)out.properties_[PT_Strength];
	float power	 = (float)out.properties_[PT_Power]; 	
	float speed	 = (float)out.properties_[PT_Speed];	
	float magic	 = (float)out.properties_[PT_Magic];	
	
	CALC_PLAYER_PRO_TRANS_STAMA(out,stama);
	CALC_PLAYER_PRO_TRANS_STRENGTH(out,strength);
	CALC_PLAYER_PRO_TRANS_POWER(out,power);
	CALC_PLAYER_PRO_TRANS_SPEED(out,speed);
	CALC_PLAYER_PRO_TRANS_MAGIC(out,magic);

	out.properties_[PT_HpCurr] = out.properties_[PT_HpMax];
	out.properties_[PT_MpCurr] = out.properties_[PT_MpMax];
	
	for (size_t i = 0; i < tmp->defaultSkill_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = tmp->defaultSkill_[i].first;
		skill.skillExp_ = 0;
		skill.skillLevel_ = tmp->defaultSkill_[i].second;

		out.skill_.push_back(skill);
	}	
	
	bool firstBabyIsBattle = true;
	for (size_t i=0; i<tmp->defaultBaby_.size(); ++i)
	{
		COM_BabyInst inst;
		Baby::genBabyData(tmp->defaultBaby_[i],inst); ///instId Warning
		inst.isBattle_ = firstBabyIsBattle;
		firstBabyIsBattle = false;
		out.babies_.push_back(inst);
	}
	
	out.achValues_.resize(AT_Max,0);
	out.properties_[PT_TableId] = playerTmpId;
	out.properties_[PT_Money] = Global::get<S32>(C_InitMoney);
	out.properties_[PT_Diamond] = Global::get<S32>(C_InitDiamond);
	out.properties_[PT_BagNum] = Global::get<int>(C_BagInitSize);
	S32 defaultTitle = TitleTable::findTitleByValue(0);
	out.properties_[PT_Title] = defaultTitle;
	out.titles_.push_back(defaultTitle);
	out.instName_= playerName;
	out.hbInfo_.tier_ = 1;
	out.hbInfo_.surplus_ = Global::get<int>(C_HundredChallengeNum);
	out.hbInfo_.resetNum_ = 1;
	out.rivalNum_ = Global::get<int>(C_JJCRivalNum);
	out.pvpInfo_.section_	= 1;
	out.pvpInfo_.value_		= 0;
	out.pvpInfo_.winValue_	= 0;
	out.pvpInfo_.contWin_	= 0;
	out.magicItemLevel_ = 1;
	out.magicItemeExp_= 0;
	out.magicItemeJob_ = JT_Axe;
	out.empBattleGroup_ = EBG_GroupOne;
	out.itemStoreSize_ = Global::get<int>(C_ItemStroagePageGridNum);
	out.babyStoreSize_ = Global::get<int>(C_BabyStroagePageGridNum);
	ArtifactConfigTable::ArtifactConfigData const* configData = ArtifactConfigTable::getArtifactById(out.magicItemLevel_,out.magicItemeJob_);
	if(configData == NULL)
		return;
	out.magicTupoLevel_ = configData->tupoLevel_;
	//out.activationCounter_.resize(ACT_Max,0);
	out.greenBoxFreeNum_ = Global::get<int>(C_BoxGreenFreeNum);
	
	float ff = CALC_BASE_FightingForce((&out));

	for(int i=0; i<out.skill_.size(); ++i)
		ff += CALC_SKILL_FightingForceCOM(out.skill_[i]);

	out.properties_[PT_FightingForce] = ff;

	for(size_t i=0; i<Activity::records_.size(); ++i)
	{
		if(!Activity::records_[i])
			continue;
		COM_Activity act;
		act.actId_ = Activity::records_[i]->actId_;
		act.counter_ = 0;

		out.activity_.activities_.push_back(act);
	}
}

void
Player::transforDBPlayer2SimplePlayer(COM_SimplePlayerInst &inst, SGE_DBPlayerData &data)
{
	inst.instId_= data.instId_;
	inst.instName_ = data.instName_;
	inst.properties_.resize(PT_Max);
	for(S32 i=PT_None; i<PT_Max; ++i)
		inst.properties_[i] = (S32)data.properties_[i];

	for (size_t i=0; i<data.skill_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = data.skill_[i].skillID_;
		skill.skillExp_ = data.skill_[i].skillExp_;
		skill.skillLevel_ = data.skill_[i].skillLevel_;
		inst.skill_.push_back(skill);
	}

	for (size_t i=0; i < data.equips_.size(); ++i)
	{
		inst.equips_.push_back(data.equips_[i]);	
	}

	for (size_t i = 0; i < data.babies_.size(); ++i)
	{
		if(!data.babies_[i].isBattle_)
			continue;
		inst.babies1_.push_back(data.babies_[i]);
	}

	if(data.empBattleGroup_ == EBG_GroupOne){
		for (size_t i = 0; i < data.employees_.size(); ++i)
		{
			if(std::find(data.employeeGroup1_.begin(),data.employeeGroup1_.end(),data.employees_[i].instId_) != data.employeeGroup1_.end()){
				inst.battleEmps_.push_back(data.employees_[i]);
			}
		}
	}
	else if(data.empBattleGroup_ == EBG_GroupTwo){
		for (size_t i = 0; i < data.employees_.size(); ++i)
		{
			if(std::find(data.employeeGroup2_.begin(),data.employeeGroup2_.end(),data.employees_[i].instId_) != data.employeeGroup2_.end()){
				inst.battleEmps_.push_back(data.employees_[i]);
			}
		}
	}
	
}

Player*
Player::createPlayer(Account *pAcc,SGE_DBPlayerData &data,int32 serverId)
{
	Player *ret = NEW_MEM(Player,pAcc);
	ret->setDBPlayerData(data);
	ret->phoneNumber_ = pAcc->phoneNumber_;
	ret->serverId_ = serverId;
	nameStore_[ret->playerName_] = ret;
	idStore_[ret->playerId_] = ret;
	store_.push_back(ret);
	return ret;
}

Player*
Player::getPlayerByName(std::string const &playerName){
	return nameStore_[playerName];
}

Player*
Player::getPlayerByInstId(U32 instId){
	return idStore_[instId];
}

void
Player::removePlayer(std::string const &playerName)
{
	Player* p = nameStore_[playerName];
	if(!p)
		return;
	PlayerList::iterator i = std::find(store_.begin(),store_.end(),p);
	if(i!=store_.end())
		store_.erase(i);
	idStore_[p->playerId_] = NULL;
	nameStore_[playerName] = NULL;
	// TODO delete;
	//ACE_DEBUG((LM_INFO,"DELETE Player\n"));
	DEL_MEM(p);
}

void
Player::removePlayer(Player* player)
{
	if(player)
		removePlayer(player->playerName_);
}

void 
Player::updatePlayer(float dt)
{
	for (size_t i=0;i<store_.size();++i)
		store_[i]->tick(dt);
}

void Player::cleanActivationAll(ActivityType type){
	for (size_t i=0;i<store_.size();++i)
		store_[i]->cleanActivation(type);
}

void Player::OnlinePlayerPassZeroHour()
{
	for (size_t i=0;i<store_.size();++i)
		store_[i]->passZeroHour();
}

void Player::uiBehavior(UIBehaviorType type){
	SGE_LogUIBehavior log;
	log.accountGuid_ = account_->guid_;
	log.accountName_ = account_->username_;
	if(getClient()){
		char cb[256] = {'\0'};
		//getClient()->getRemoteAddr().string_to_addr(cb);
		log.clientIp_ = cb;
	}
	log.playerGuid_ = getGUID();
	log.playerName_ = getNameC();
	log.type_ = type;
	LogHandler::instance()->playerUIBehavior(log);
}

void Player::passZeroHour()
{
	ACE_Date_Time creatDT,logoutDT, nowDT;
	creatDT.update(ACE_Time_Value(createTime_));
	logoutDT.update(ACE_Time_Value(logoutTime_));
	nowDT.update(ACE_Time_Value(WorldServ::instance()->curTime_));
	if(nowDT.day() > logoutDT.day())
	{
		++(festival_.loginDays_);
		updateFestival();
	}
	if(logoutDT.month() != nowDT.month()){
		signs_ = 0;
	}
	int32 currentDay= WorldServ::instance()->curTime_/ ONE_DAY_SEC;
	int32 lououtDay = logoutTime_ / ONE_DAY_SEC;
	if(currentDay - lououtDay == 1) //�����¼�� �����ٵǲż���
		calcConvertExp();
	resetCard(true);
	resetIntegralState();
	signFlag_ = false;

	CALL_CLIENT(this,signUp(signFlag_));
	//if(PeriodEvent::getCurHour() == Global::get<int>(C_TongjiRefreshHour)){
	resetActivation();//	cleanActivation(ACT_None);// bug?
	//}

	for(size_t i=0; i<completeQuest_.size();++i)
	{
		const Quest* q = Quest::getQuestById(completeQuest_[i]);
		if(q == NULL){
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if(q->questKind_ == QK_Daily)
		{
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if(q->questKind_ == QK_Wishing)
		{
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if(q->questKind_ == QK_Guild)
		{
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if(q->questKind_ == QK_Sub1){
			completeQuest_.erase(completeQuest_.begin() + i--); ///清除 
		}
	}
	CALL_CLIENT(this,initQuest(currentQuest_,completeQuest_));

	hundredNum_ = Global::get<int>(C_HundredChallengeNum);
	hundredresetNum_ = Global::get<int>(C_HundredChallengeNum);
	tier_ = 1;
	COM_HundredBattle hb;
	hb.tier_	= tier_;
	hb.curTier_ = curTier_;
	hb.surplus_ = hundredNum_;
	hb.resetNum_= hundredresetNum_;
	CALL_CLIENT(this,syncHundredInfo(hb));
	
	wishShareNum_ = 0;
	copynums_.clear();
	CALL_CLIENT(this,initCopyNums());
	rivalNum_   = Global::get<int>(C_JJCRivalNum);
	greenBoxFreeNum_ = Global::get<int>(C_BoxGreenFreeNum);
	warriortrophyNum_ = 0;
	CALL_CLIENT(this,requestOpenBuyBox(greenBoxTimes_,blueBoxTimes_,greenBoxFreeNum_));

	ACE_DEBUG((LM_INFO,"requestOpenBuyBox Player[%d] greenBoxTimes_[%f] blueBoxTimes_[%f]\n",playerId_,greenBoxTimes_,blueBoxTimes_));

	{
		ACE_Date_Time dt(ACE_OS::gettimeofday());
		if(dt.day() == 1)
		{
			signs_ = 0;
			const std::vector<S32>* rewards = DailyReward::getMonthRewards(dt.month());
			if(rewards)
			{
				CALL_CLIENT(this,initSignUp(*rewards,signs_,false,false,false));
			}
		}
	}

	{
		if(getProp(PT_VipTime) > 0 && !viprewardflag_){
			viprewardflag_ = true;
			CALL_CLIENT(this,sycnVipflag(viprewardflag_));
		}
	}

	//����˫������ /// ��ô�㲻�� 
	float curDouble = getProp(PT_DoubleExp);
	if(curDouble < Global::get<float>(C_DoubleExpMax))
	{
		if((int)getProp(PT_VipLevel) == VL_1){
			curDouble += Global::get<float>(C_EverydayDoubleExp) + 900;
		}
		else if((int)getProp(PT_VipLevel) == VL_2){
			curDouble += Global::get<float>(C_EverydayDoubleExp) + 1800;
		}
		else
			curDouble += Global::get<float>(C_EverydayDoubleExp);
		
		if(curDouble > Global::get<float>(C_DoubleExpMax))
			curDouble = Global::get<float>(C_DoubleExpMax);
	}
	setProp(PT_DoubleExp,curDouble);
	resetgathers();

	if(PeriodEvent::getCurWeek() == Global::get<int>(C_CleanRandQuestWeekDay)){
		cleanRandQuestCounter(); ///�������һ ���
	}

	refreshOnlineReward();
}

void
Player::freeBabies()
{
	for (size_t i=0; i<babies_.size(); ++i){
		if(babies_[i])
			DEL_MEM(babies_[i]);
	}
	babies_.clear();
}

void 
Player::freeBagItems()
{
	for (size_t i=0; i<bagItmes_.size(); ++i){
		if(bagItmes_[i])
			DEL_MEM(bagItmes_[i]);
	}
	bagItmes_.clear();

	for(size_t i=0; i<fuwen_.size(); ++i){
		if(fuwen_[i])
			DEL_MEM(fuwen_[i]);
	}
	fuwen_.clear();
}

void 
Player::freeEmployees()
{
	for(size_t i=0;i<employees_.size();++i){
		if(employees_[i])
			DEL_MEM(employees_[i]);
	}
	employees_.clear();
}

void
Player::save()
{
	//ACE_DEBUG((LM_INFO,"Save --> %s\n",playerName_.c_str()));
	logoutTime_ = WorldServ::instance()->curTime_; //����Ӹ���һ��logout ʱ��

	//calcProperty(); ///���¼������� 

	SGE_DBPlayerData data;
	getDBPlayerData(data);

	
	//if(account_)
	//{
	//	//ACE_DEBUG((LM_INFO,"Save update account--> %s\n",playerName_.c_str()));
	//	account_->updateDBPlayer(data);
	//}
	if(account_)
		DBHandler::instance()->updatePlayer(account_->username_,data);
	else
		ACE_DEBUG((LM_ERROR,"Save not user %s \n",playerName_.c_str()));
}

void 
Player::tick(float dt)
{
	///��֡��ʼ��
	if(initSyncEmpIndex_ != -1)
		initEmployees();
	
	//����Լ�¼��ֵ������
	if(!orders_.empty())
	{
		ACE_DEBUG((LM_INFO,"Recharge cached orders %d \n",playerId_));
		for(size_t i=0; i<orders_.size(); ++i){
			this->justOrder(orders_[i].productId_,orders_[i].productCount_,orders_[i].orderId_,orders_[i].payTime_,orders_[i].amount_);
		}
		orders_.clear();
	}

	worldTalkTime_ -= dt;
	if(noTalkTime_ >=0.F)
		noTalkTime_ -= dt;
	noBattleTime_ -= dt;
	
	saveFreq_ -= dt;
	if(saveFreq_ <= dt)
	{
		saveFreq_ = SAVE_FREQ;
		save();
	}
	
	if(rivalTimes_ > 0)
	{
		rivalTimes_ -= dt;
		
		if (rivalTimes_ <= 0)
		{
			CALL_CLIENT(this,rivalTimeOK());
		}
	}
	if(greenBoxTimes_ > 0)
	{
		greenBoxTimes_ -=dt;
		if(greenBoxTimes_ <=0)
		{
			CALL_CLIENT(this,requestGreenBoxTimeOk());
		}
	}

	if(blueBoxTimes_ > 0)
	{
		blueBoxTimes_ -=dt;
		if(blueBoxTimes_ <=0)
		{
			CALL_CLIENT(this,requestBlueBoxTimeOk());
		}
	}

	if(onlinetimeflag_)
	{
		onlinetime_ += dt;
	}
	
	///ͬ������
	syncProp();

	{
		U64 t = (WorldServ::instance()->curTime_ - loginTime_) * 0.001;
		if(t%30 == 0)
		{
			GameEvent::procGameEvent(GET_HalfHourAgo,NULL,0,getHandleId());
		}
	}

	caleDoubleExpTime(dt);

	enum{
		FatchMailTimeout = 30
	};
	fatchMailTimeout_ -= dt;
	if(fatchMailTimeout_ <= 0){
		updateItemUseTimeout(FatchMailTimeout);
		updateBabySellTimeout(FatchMailTimeout);
		fatchMail();
		fatchMailTimeout_ += FatchMailTimeout;
	}

	///����VIP
	if((int)getProp(PT_VipLevel)!=VL_None){
		properties_[PT_VipTime] -= dt;
		if(properties_[PT_VipTime] <= 0.f)
		{
			setProp(PT_VipLevel,VL_None);
			WorldServ::instance()->updateContactInfo(this);
		}
	}

	//ͬ����ǰ�������
	if(isSceneLoaded_)
	{
		NpcList npcs;
		while(!syncNpcs_.empty() && npcs.size() < 3){

			npcs.push_back(syncNpcs_.back());
			syncNpcs_.pop_back();
		}
		if(!npcs.empty())
			CALL_CLIENT(this,addNpc(npcs));

		updateVisiblePlayersTimeout_ -= dt;
		if(updateVisiblePlayersTimeout_ <=0.F)
		{
			calcVisiblePlayers();
			updateVisiblePlayersTimeout_ += UPDATE_VISIBLE_PLAYERS_TIMEOUT;
		}
	}
	//���������ʱ
	calcCourseGifttime(dt);
}

ClientHandler *Player::getClient()
{
	if(account_)
		return account_->getClient();
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Player::getClient() account_ is null \n")));
	return NULL;
}

///========================================================================
///@group Properties
///@{

void Player::addBaby(COM_BabyInst& inst,bool isToStorage)
{
	Baby *b = NEW_MEM(Baby,this);
	b->setBabyInst(inst);
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(b->getProp(PT_TableId));
	if(tmp == NULL)
	{
		DEL_MEM(b);
		return;
	}
	babies_.push_back(b);
	CALL_CLIENT(this,addBaby(inst));
	
	if(!isToStorage && !checkBabyCache(tmp->monsterId_))
	{
		babycache_.push_back(tmp->monsterId_);
		//ACE_DEBUG((LM_INFO,"Player [%d] add baby tableid==[%d] \n",getGUID(),tmp->monsterId_));
	}

	COM_BabyRankData info;
	info.instId_ = inst.instId_;
	info.name_ = inst.instName_;
	info.ownerName_ = playerName_;
	info.ff_ = inst.properties_[PT_FightingForce];
	WorldServ::instance()->calcBabyFFRank(info);

	GEParam param[5];
	param[0].type_ = GEP_INT;
	param[0].value_.i = b->getProp(PT_TableId);
	param[1].type_ = GEP_INT;
	param[1].value_.i = b->getGUID();
	param[2].type_ = GEP_INT;
	param[2].value_.i = tmp->monsterType_;
	param[3].type_ = GEP_INT;
	param[3].value_.i = isToStorage;
	param[4].type_ = GEP_INT;
	param[4].value_.i = tmp->race_;
	GameEvent::procGameEvent(GET_AddBaby,param,5,getHandleId());
}

void 
Player::addBaby(S32 monsterId,std::vector<COM_Item> &equips)
{
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(monsterId);
	if(NULL == tmp)
		return;
	COM_BabyInst inst;
	Baby::genBabyData(monsterId,inst);
	inst.equips_ = equips;
	inst.ownerName_ = playerName_;
	DBHandler::instance()->createBaby(playerName_,inst,false);
}

void 
Player::addBaby(S32 monsterId,bool isToStorage)
{
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(monsterId);
	if(NULL == tmp)
		return;
	COM_BabyInst inst;
	Baby::genBabyData(monsterId,inst);
	inst.ownerName_ = playerName_;
	DBHandler::instance()->createBaby(playerName_,inst,isToStorage);
}

void
Player::addBaby2(COM_BabyInst& inst,bool isToStorage)
{
	//ACE_DEBUG((LM_DEBUG,"ADD BABAY\n"));
	inst.instId_ = WorldServ::instance()->getMaxGuid();
	inst.ownerName_ = playerName_;
	
	DBHandler::instance()->createBaby(playerName_,inst,isToStorage);
}

//void Player::delBabyByBabyId(S32 babyId)
//{
//	if(isBattle())
//		return;
//	///ɾ�����б�ID�ĳ���
U32 Player::getminlevelbaby(S32 tableid){
	U32 level = 10000;
	U32 instid = 0;
	for (size_t i=0; i<babies_.size();++i)
	{
		if(babies_[i]->tableId_ != tableid)
			continue;
		if(babies_[i]->properties_[PT_Level] < level)
		{
			level = babies_[i]->properties_[PT_Level];
			instid = babies_[i]->getGUID();
		}
	}

	return instid;
}

void Player::delBaby(S32 instId)
{
	if(isBattle())
		return;

	for(size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->babyId_ == instId)
		{
			if(babies_[i]->isLock_)
				return;
			GEParam param[1];
			param[0].type_ = GEP_INT;
			param[0].value_.i = babies_[i]->tableId_;
			GameEvent::procGameEvent(GET_DelBaby,param,1,getHandleId());

			DBHandler::instance()->deleteBaby(playerName_,instId);
	
			Baby* p = babies_[i];
			babies_.erase(babies_.begin() + i);
			CALL_CLIENT(this,delBabyOK(p->getGUID()));
			//ACE_DEBUG((LM_INFO,"Player %d delete baby %d \n",getGUID(),p->getGUID()));
			DEL_MEM(p);
			WorldServ::instance()->delBabyRank(instId);
			return;
		}
	}
}

void Player::delBabyOk(S32 instId)
{
	WorldServ::instance()->updateBabyRank(instId);
	for(size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->babyId_ == instId)
		{
			Baby* p = babies_[i];
			babies_.erase(babies_.begin() + i);
			CALL_CLIENT(this,delBabyOK(p->getGUID()));
			//ACE_DEBUG((LM_INFO,"Player %d delete baby %d \n",getGUID(),p->getGUID()));
			DEL_MEM(p);
			return;
		}
	}
}

void
Player::remouldBaby(S32 instId)
{
	Baby* pbaby = findBaby(instId);
	if(pbaby == NULL)
		return;
	if(pbaby->getProp(PT_Level) != 1)
	{
		CALL_CLIENT(this, errorno(EN_RemouldBabyLevel));
		return;
	}
	MonsterTable::MonsterData const* pData = MonsterTable::getMonsterById(pbaby->getProp(PT_TableId));
	if(pData == NULL)
		return;
	if(pData->refromMonster_ == 0)
		return;
	if(pData->refrommItem_.empty())
		return;
	for (size_t i = 0; i < pData->refrommItem_.size(); ++i)
	{
		if(getItemNumByItemId(pData->refrommItem_[i]) <= 0)
		{
			CALL_CLIENT(this, errorno(EN_Materialless));
			return;
		}
	}
	std::vector<COM_Item> equips;
	for(size_t i=0; i<pbaby->equipItems_.size(); ++i){
		if(pbaby->equipItems_[i]){
			equips.push_back(*(pbaby->equipItems_[i]));
		}
	}
	//��������,֪ͨDBɾ���������
	delBaby(instId);
	for(size_t i = 0; i < pData->refrommItem_.size(); ++i){
		delBagItemByItemId(pData->refrommItem_[i]);
	}

	addBaby(pData->refromMonster_,equips);
	CALL_CLIENT(this, remouldBabyOK(pData->refromMonster_));
}

bool 
Player::genItemInst(U32 itemId, U32 itemCount, std::vector<COM_Item*> &items,bool isBind, bool isLock )
{
	ItemTable::ItemData const* core = ItemTable::getItemById(itemId);
	if(core  == NULL)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Cant find item in ItemTable  %d \n"),itemId));
		return false;
	}
	
	while(itemCount > 0)
	{
		COM_Item * pItem = NEW_MEM(COM_Item);
		SRV_ASSERT(pItem);

		pItem->itemId_ = itemId; 
		pItem->instId_ = ++genItemMaxGuid_;
		pItem->isBind_ = isBind;
		pItem->isLock_ = isLock;
		pItem->usedTimeout_ = core->useTime_;
		if(core->maxCount_ > itemCount)
		{
			pItem->stack_ = itemCount;
			itemCount = 0;
		}
		else
		{
			pItem->stack_ = core->maxCount_;
			itemCount = itemCount - core->maxCount_;
		}

		for(size_t i =0;i<core->propValue_.size();i++)
		{
			COM_PropValue  prop ;
			prop.type_ = core->propValue_[i].type_;
			std::vector<S32> propArr = core->propValue_[i].value_;
			
			S32 value =0;
			if(propArr [0] == propArr [1])
			{
				value  = propArr [0];
			}
			else
			{			
				if(propArr [0] < 0 && propArr [1] < 0)
				{
					 value = -(rand()%(abs(propArr [1]) - abs(propArr [0])) +abs(propArr[0]));
				} 
				else
				{
					 value = rand()%(propArr [1] - propArr [0]) + propArr[0];
				} 
			}
			prop.value_ = value;
			pItem->propArr.push_back(prop);
		}
		if(core->mainType_ == IMT_Equip)
		{
			if(core->durability_.value_[1] == core->durability_.value_[0])
			{
				pItem->durability_ = core->durability_.value_[0];   
			}
			else
			{
				pItem->durability_ =rand()% (core->durability_.value_[1] - core->durability_.value_[0])+ core->durability_.value_[0];
			}
			pItem->durabilityMax_ = pItem->durability_;
		}
		else if(core->mainType_ == IMT_BabyEquip){ ///����װ��������ܺͼ��ܵȼ�
			if(core->durability_.value_.size() == 2){
				pItem->durability_ = core->durability_.value_[0];
				pItem->durabilityMax_ = core->durability_.value_[1];
			}
			if(pItem->durability_ == 0)
				SkillTable::randBabyEquipSkillInfo(pItem->durability_,pItem->durabilityMax_);
		}
		items.push_back(pItem);
	}
	
	return true;
}

///@}
///========================================================================
///@group Bag
///@{
void Player::initSignup(){
	{
		ACE_Date_Time dt(ACE_OS::gettimeofday());
		const std::vector<S32>* rewards = DailyReward::getMonthRewards(dt.month());
		if(rewards)
		{
			CALL_CLIENT(this,initSignUp(*rewards,signs_,false,false,false));
		}
	}
}

void Player::initBabies()
{
	if(!babies_.empty())
	{
		std::vector<COM_BabyInst> tmps;
		for (size_t i=0; i<babies_.size(); ++i)
		{
			COM_BabyInst insttmp;
			babies_[i]->getBabyInst(insttmp);
			tmps.push_back(insttmp);
		}
		CALL_CLIENT(this, initBabies(tmps));
	}
}

void Player::initEmployees(bool first)
{
	if(first){
		initSyncEmpIndex_ = 0;

		COM_BattleEmp bp;
		bp.empBattleGroup_ = empbattlegroup_;
		bp.employeeGroup1_ = battleEmpsGroup1_;
		bp.employeeGroup2_ = battleEmpsGroup2_;
		CALL_CLIENT(this,initEmpBattleGroup(bp));
	}
	enum{
		OneStepSync = 10
	};

	bool isfinish = false;
	std::vector<COM_EmployeeInst> tmps;
	if(employees_.empty() && initSyncEmpIndex_ != -1){
		initSyncEmpIndex_ = -1;
		CALL_CLIENT(this, initEmployees(tmps,true));
		return;
	}
	else if(initSyncEmpIndex_ != -1)
	{
		for (size_t i=0; i<OneStepSync; ++i)
		{
			if(initSyncEmpIndex_ == employees_.size()){
				initSyncEmpIndex_ = -1;
				break;
			}
			COM_EmployeeInst insttmp;
			employees_[initSyncEmpIndex_]->getEmployeeInst(insttmp);
			tmps.push_back(insttmp);
			++initSyncEmpIndex_;
		}
	
		if(initSyncEmpIndex_ == -1)
			isfinish = true;
		CALL_CLIENT(this, initEmployees(tmps,isfinish));
	}
}

void
Player::initAchievement()
{
	CALL_CLIENT(this, initAchievement(achievement_));
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("initAchievement Ok\n")));
}

void Player::initBagItem()
{
	std::set<int32> filter;
	std::vector<COM_Item> cache;
	for(size_t i=0; i<bagItmes_.size(); ++i){
		if(bagItmes_[i] != NULL)
		{
			if (filter.find(bagItmes_[i]->instId_)!=filter.end()){
				bagItmes_[i]->instId_ = ++genItemMaxGuid_;
			}else{
				filter.insert(bagItmes_[i]->instId_);
			}
			//ACE_DEBUG((LM_DEBUG,"[%d,%d]\n",bagItmes_[i]->itemId_,bagItmes_[i]->instId_));
			cache.push_back(*bagItmes_[i]);
		}
	}

	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("InitBag Ok\n")));
	CALL_CLIENT(this, initBag(cache));
}

void Player::addBagItemByItemId(U32 itemId,U32 itemCount, bool isLock, S32 fromid)
{
	ItemTable::ItemData const * data = ItemTable::getItemById(itemId);
	if(NULL == data)
		return;
	if(0 == itemCount)
		return;
	if(data->mainType_ == IMT_Employee ){
		addEmployee(data->employeeId_);
		return;
	}
	else if(data->mainType_ == IMT_Quest){
		S32 num = checkQuestItem(data->id_);
		if(num ==0 )
			return ;
		itemCount = itemCount > num ? num : itemCount;
	}
	
	bool isBind = data->bindType_ == BIT_Bag;
	U32 oldItemCount = itemCount;
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("addItem  %d  %d\n"),itemId,itemCount));
	int32 curtm = WorldServ::instance()->curTime_;
	///�Ҷѵ�
	for(size_t i=0 ;i<bagItmes_.size(); ++i){
		if(bagItmes_[i] && bagItmes_[i]->itemId_ == itemId && (curtm - bagItmes_[i]->lastSellTime_ >Global::get<int32>(C_MallShopFreezeTime)) && bagItmes_[i]->isBind_ == isBind&& (bagItmes_[i]->isLock_ == isLock) && bagItmes_[i]->stack_ < data->maxCount_){
			bagItmes_[i]->stack_ += itemCount;
			if(bagItmes_[i]->stack_ > data->maxCount_){
				itemCount = bagItmes_[i]->stack_ - data->maxCount_;
				bagItmes_[i]->stack_ = data->maxCount_;
				
				SGE_LogProduceTrack track;
				track.playerId_ = getGUID();
				track.playerName_ = getNameC();
				track.from_ = fromid;
				track.itemId_ = itemId;
				track.itemInstId_ = bagItmes_[i]->instId_;
				track.itemStack_ = itemCount;
				LogHandler::instance()->playerTrack(track);

			}
			else {
				SGE_LogProduceTrack track;
				track.playerId_ = getGUID();
				track.playerName_ = getNameC();
				track.from_ = fromid;
				track.itemId_ = itemId;
				track.itemInstId_ = bagItmes_[i]->instId_;
				track.itemStack_ = itemCount;
				LogHandler::instance()->playerTrack(track);
				itemCount = 0;
			}
			CALL_CLIENT(this,updateBagItem(*bagItmes_[i]));
			if(itemCount == 0)break;
		}
	}
	
	if(itemCount != 0){		
		bool hasBagFullError = false;
		std::vector<COM_Item*> items;
		if(!genItemInst(itemId,itemCount,items,isBind,isLock))
			return ;
		
		for (U32 i=0; i<items.size(); ++i)
		{
			int32 emptySlot = -1;
			if( hasBagFullError || (emptySlot = getFirstEmptySlot()) < 0)
			{
				hasBagFullError = true;
				DEL_MEM(items[i]);
				continue;
			}
		
			items[i]->slot_ = emptySlot ;
			bagItmes_[emptySlot] = items[i];
			CALL_CLIENT(this,addBagItem(*items[i]));

			SGE_LogProduceTrack track;
			track.playerId_ = getGUID();
			track.playerName_ = getNameC();
			track.from_ = fromid;
			track.itemId_ = itemId;
			track.itemInstId_ = items[i]->instId_;
			track.itemStack_ = items[i]->stack_;
			LogHandler::instance()->playerTrack(track);
		}

		if(hasBagFullError){
			ACE_DEBUG((LM_INFO,"Add item by itemId but bag is fully!! %s, %d,%d,form %d\n",playerName_.c_str(),itemId,itemCount,fromid));
			errorMessageToC(EN_BagFull);
		}
	}

	postQuestItemEvent(itemId,oldItemCount);
}

bool Player::addBagItemByInst(COM_Item* item,S32 fromid){
	ItemTable::ItemData const * data = ItemTable::getItemById(item->itemId_);
	if(NULL == data)
		return false;
	S32 emptySlot = getFirstEmptySlot();
	if(emptySlot  < 0)
	{
		ACE_DEBUG((LM_INFO,"Add item by inst but bag is fully!! %s,form %d\n",playerName_.c_str(),fromid));
		errorMessageToC(EN_BagFull);
		DEL_MEM(item);
		return false;
	}

	item->slot_ = emptySlot;
	bagItmes_[emptySlot] = item;
	
	CALL_CLIENT(this,addBagItem(*bagItmes_[emptySlot]));
	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = fromid;
	track.itemId_ = item->itemId_;
	track.itemInstId_ = item->instId_;
	track.itemStack_ = item->stack_;
	LogHandler::instance()->playerTrack(track);
	return true;
}

S32 Player::getFirstEmptySlot()
{
	if(getBagItemSize() >= getProp(PT_BagNum))
		return -1;
	for (U32 i =0;i<bagItmes_.size();i++){
		if(NULL == bagItmes_[i])
			return i ;
	}
	return -1;
}

U32 Player::getBagEmptySlot()
{
	U32 emptyNum = 0;
	for(size_t i = 0; i < (size_t)getProp(PT_BagNum); ++i)
	{
		if(NULL == bagItmes_[i])
			++emptyNum;
	}
	return emptyNum;
}


U32 Player::getBagItemSize()
{
	U32 num =0;
	for(size_t i = 0; i < (size_t)getProp(PT_BagNum); ++i)
	{
		if(NULL != bagItmes_[i])
			++num;
	}
	return num;
}

U32
Player::getItemNumByItemId(U32 itemId)
{
	U32 emptyNum = 0;
	for(size_t i = 0; i < bagItmes_.size(); ++i)
	{
		if(NULL == bagItmes_[i])
			continue;
		if(bagItmes_[i]->itemId_ != itemId)
			continue;
		emptyNum += bagItmes_[i]->stack_;
	}

	return emptyNum;
}

void 
Player::delBagItemByInstId(U32 itemInstId, U32 stack,S32 fromid)
{
	//ACE_DEBUG((LM_INFO,"Del item %d %d \n",itemInstId,stack));
	COM_Item *p = getBagItemByInstId(itemInstId);

	if(NULL == p)
	{
		
		return ;
	}

	SRV_ASSERT(bagItmes_[p->slot_]);
	
	p->stack_ -= stack;
	
	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = fromid;
	track.itemId_ = p->itemId_;
	track.itemInstId_ = itemInstId;
	track.itemStack_ = -(int32)stack;
	LogHandler::instance()->playerTrack(track);

	if(p->stack_ <= 0)
	{
		bagItmes_[p->slot_] = NULL;
		CALL_CLIENT(this,delBagItem(p->slot_));
		DEL_MEM(p);
	}
	else
	{
		CALL_CLIENT(this,updateBagItem(*p));
	}
}

void 
Player::delBagItemByItemId(U32 itemId, U32 stack, int32 fromId)
{
	std::vector<COM_Item*> items;
	getBagItemByItemId(itemId,items);
	if(items.empty())
		return;
	
	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = fromId;
	track.itemId_ = itemId;
	track.itemInstId_ = 0;
	track.itemStack_ = -(int32)stack;
	LogHandler::instance()->playerTrack(track);

	while(stack > 0 && !items.empty())
	{
		COM_Item * current = items.back();
		items.pop_back();
		SRV_ASSERT(current);
		if(stack < current->stack_)
		{
			current->stack_ -= stack;
			stack = 0;
			CALL_CLIENT(this,updateBagItem(*current));
		}
		else
		{///ɾ������� item
			stack -= current->stack_;
			bagItmes_[current->slot_] = NULL;
			CALL_CLIENT(this,delBagItem(current->slot_));
			DEL_MEM(current);
		}
	}

}

void
Player::getBagItemByItemId(U32 itemId, std::vector<COM_Item*> &out)
{
	for (size_t i=0; i<bagItmes_.size(); ++i)
	{
		if( (bagItmes_[i]!=NULL)&&(bagItmes_[i]->itemId_ == itemId) )
		{
			 out.push_back(bagItmes_[i]);
		}
	}
	
	ItemStackSort ss;
	std::sort(out.begin(),out.end(),ss);
}

void
Player::getItemByItemSubType(ItemSubType type,std::vector<COM_Item*> &out)
{
	for (size_t i = 0; i < bagItmes_.size(); ++i)
	{
		if(bagItmes_[i] == NULL)
			continue;
		ItemTable::ItemData const * data = ItemTable::getItemById(bagItmes_[i]->itemId_);
		if(NULL == data)
			continue;
		if(data->subType_ != type)
			continue;
		out.push_back(bagItmes_[i]);
	}
}

COM_Item*
Player::getPlayerEquipBySlot(EquipmentSlot slot)
{
	return equipItems_[slot];
}

void 
Player::move(COM_FPosition& pos){
	if(isBattle())
	{
		ACE_DEBUG((LM_DEBUG,"Move to pos player %s in battle \n",playerName_.c_str()));
		return;
	}
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;
	autoBattle_ = false;
	scenePlayer_->move(pos);
}

void Player::moveToNpc(S32 npcid){
	NpcTable::NpcData const* pNpc = NpcTable::getNpcById(npcid);
	if(pNpc == NULL){
		ACE_DEBUG((LM_DEBUG,"Player %s try move to npc %d  \n",playerName_.c_str(),npcid));
		return;
	}
	if(isBattle())
	{
		ACE_DEBUG((LM_DEBUG,"Move to npc player %s in battle \n",playerName_.c_str()));
		return;
	}
	
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;

	if(pNpc->sceneId_ == SceneTable::getGuildHomeScene()->sceneId_){
		Team* pTeam = myTeam();
		if(pTeam && !pTeam->isSameGuild()){
			errorMessageToC(EN_TeamMemberNoGuild);
			CALL_CLIENT(this,cantMove());
			return;
		}
	}

	autoBattle_ = false;
	scenePlayer_->moveToNpc(npcid);
}

void Player::moveToNpc2(NpcType type){
	if(isBattle())
	{
		ACE_DEBUG((LM_DEBUG,"Move to npc2 player %s in battle \n",playerName_.c_str()));
		return;
	}
	if(type < NT_None || type > NT_Max){
		ACE_DEBUG((LM_DEBUG,"Player %s try move to npc type %d  \n",playerName_.c_str(),type));
		return;
	}
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;
	autoBattle_ = false;
	scenePlayer_->moveToNpc2(type);
}

void Player::moveToZone(S32 sceneId, S32 zoneId){
	if(SceneTable::getSceneById(sceneId) == NULL){
		ACE_DEBUG((LM_DEBUG,"Player %s try move to zone %d %d  \n",playerName_.c_str(),sceneId,zoneId));
		return;
	}
	if(isBattle())
	{
		ACE_DEBUG((LM_DEBUG,"Move to zone player %s in battle \n",playerName_.c_str()));
		return;
	}
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;
	autoBattle_ = false;
	scenePlayer_->moveToZone(sceneId,zoneId);
}
void Player::autoBattle(){
	if(isBattle())
		return;
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;
	autoBattle_ = true;
	scenePlayer_->autoBattle();
}

bool Player::stopAutoBattle(){
	if(isBattle())
		return true;
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return true;
	//ACE_DEBUG((LM_INFO,"void Player::stopAutoBattle(){ \n"));
	scenePlayer_->stopMove();
	return true;
}

bool Player::queryPlayerInst(U32 playerId){
	Player* p = getPlayerByInstId(playerId);
	if(p == NULL)
		return false;
	COM_SimplePlayerInst spi;
	p->getSimplePlayerInst(spi);
	CALL_CLIENT(this, querySimplePlayerInstOk(spi));
	return true;
}

void Player::stopMove(){
	
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;
	//ACE_DEBUG((LM_DEBUG,"void Player::stopMove(){ \n"));
	if(!autoBattle_ && scenePlayer_) 
		scenePlayer_->stopMove();
	else 
		ACE_DEBUG((LM_DEBUG,"Auto battle is true"));
}

bool 
Player::sortBagItem()
{
	uint32 curtm = WorldServ::instance()->curTime_;
	for(size_t i=0; i<bagItmes_.size(); ++i)
	{
		COM_Item* c = bagItmes_[i];
		if(NULL == c)
			continue;
		const ItemTable::ItemData* cd = ItemTable::getItemById(c->itemId_);
		if(NULL == cd)
			continue;
		for(size_t k=i+1; k<bagItmes_.size(); ++k)
		{
			COM_Item* l = bagItmes_[k];
			if(NULL == l)
				continue;
			if(curtm - l->lastSellTime_ < Global::get<int>(C_MallShopFreezeTime))
				continue;
			if(c->isBind_ != l->isBind_)
				continue;
			if(c->isLock_ != l->isLock_)
				continue;
			l->lastSellTime_ = 0;
			if(c->itemId_ == l->itemId_)
			{
				if(c->stack_ < cd->maxCount_)
				{
					c->stack_ += l->stack_;
					l->stack_ = c->stack_ - cd->maxCount_;
					c->stack_ = c->stack_ >= cd->maxCount_ ? cd->maxCount_ : c->stack_;
				}

				if(l->stack_ <= 0)
				{
					bagItmes_[k] = NULL;
					DEL_MEM(l);
				}
			}
		}
	}
	
	ItemTable::ItemSortFunction isf;
	std::sort(bagItmes_.begin(),bagItmes_.end(),isf);
	for(size_t i=0; i<bagItmes_.size(); ++i)
	{
		if(bagItmes_[i])
			bagItmes_[i]->slot_ = i;
	}
	initBagItem();
	CALL_CLIENT(this,sortBagItemOk());
	return true;
}

bool
Player::sellBagItem(U32 instId,U32 num)
{
	COM_Item* item = getBagItemByInstId(instId);
	if(item == NULL)
		return true;
	ItemTable::ItemData const* core= ItemTable::getItemById(item->itemId_);
	if(core == NULL)
		return true;
	
	if(item->stack_< num)
		return true;
	if(item->isLock_)
		return true;
	delBagItemByInstId(instId,num);
	addMoney(core->price_*num);

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 5;
	track.money_ = core->price_*num;
	LogHandler::instance()->playerTrack(track);

	return true;
}

void Player::lockItem(uint32 instId, bool isLock)
{
	for(size_t i=0; i<bagItmes_.size(); ++i){
		if(bagItmes_[i] && bagItmes_[i]->instId_ == instId){
			bagItmes_[i]->isLock_ = isLock;
			CALL_CLIENT(this,updateBagItem(*bagItmes_[i]));
			return;
		}
	}

	for( size_t i=0; i<itemStorage_.size(); ++i){
		if(itemStorage_[i] && itemStorage_[i]->instId_ == instId){
			itemStorage_[i]->isLock_ = isLock;
			CALL_CLIENT(this,updateBagItem(*bagItmes_[i]));
			return;
		}
	}
};



COM_Item*
Player::getBagItemMinStackByItemId(U32 itemId) ///��С�ѵ�
{
	std::vector<COM_Item*> items;
	getBagItemByItemId(itemId,items);
	if(items.empty())
		return NULL;

	return items.back();
}

COM_Item* 
Player::getBagItemByInstId(U32 instId)
{
	for (size_t i=0; i<bagItmes_.size(); ++i)
	{
		if( (bagItmes_[i]!=NULL)&&(bagItmes_[i]->instId_ == instId) )
		{
			return bagItmes_[i];
		}
	}
	return NULL;
}

COM_Item*
Player::getEquipByInstId(U32 instId)
{
	for (size_t i=0; i<equipItems_.size(); ++i)
	{
		if( (equipItems_[i]!=NULL)&&(equipItems_[i]->instId_ == instId) )
		{
			return equipItems_[i];
		}
	}
	return NULL;
}

COM_Item*
Player::getItemInst(ItemContainerType type,U32 itemInstId)
{
	switch(type)
	{
	case ICT_BagContainer:
		{	
			return getBagItemByInstId(itemInstId);
		}
		break;
	case ICT_EquipContainer:
		{
			return getEquipByInstId(itemInstId);
		}
		break;
	default:
		{
			return NULL;
		}
		break;
	}
}

void Player::useItem(U32 slot , U32 target , int32 stack)
{
	if(0 >= stack){
		return;
	}
	//ACE_DEBUG((LM_INFO,"Use item slot %d\n",slot));
	if(slot >= bagItmes_.size()){
		ACE_DEBUG((LM_ERROR,"Can not use this slot %d\n",playerId_));
		return;
	}

	COM_Item* pInst = bagItmes_[slot];
	if(NULL == pInst)
	{
		return;
	}
	ACE_DEBUG((LM_INFO,"Use item %d %d\n",pInst->instId_,pInst->itemId_));
	if(pInst->stack_ <= 0)
	{
		return;
	}
	if(pInst->stack_ < stack){
		stack = pInst->stack_;
	}

	ItemTable::ItemData const * itemData = ItemTable::getItemById(pInst->itemId_);

	if(itemData == NULL)
		return;

	if(itemData ->mainType_ != IMT_Consumables)
	{
		return;
	}
	if(itemData->bindType_ == BIT_Use){
		pInst->isBind_ = true;
	}
	//lUA
	ErrorNo error = EN_None;
	int usestack = 0;
	for(int32 i=0; i<stack; ++i)
	{
	
		error = usessItem(target,pInst->itemId_);
		
		if(error != EN_None){
			ACE_DEBUG((LM_INFO,"Use item %d %d %d error \n",pInst->instId_,pInst->itemId_,error));
			break;
		}
		++usestack;
	}
	if(usestack)
	{
		delBagItemByInstId(pInst->instId_,usestack);
		CALL_CLIENT(this,useItemOk(itemData->id_,usestack));
	}
	if(error != EN_None)
		CALL_CLIENT(this,errorno(error));
}

bool 
Player::openBagGrid(U32 itemId)
{
	ItemTable::ItemData const * itemData = ItemTable::getItemById(itemId);
	
	if (itemData == NULL || itemData->subType_ != IST_OpenGird)
		return false;
	
	enum{
		OPENGIRDNUM = 5
	};

	//todo����
	setProp(PT_BagNum,getProp(PT_BagNum)+OPENGIRDNUM);

	CALL_CLIENT(this,openBagGridOk(getProp(PT_BagNum)));
	return true;
}

bool
Player::bagItemSplit(U32 instId,U32 splitNum)
{
	COM_Item* item = getBagItemByInstId(instId);
	if(item == NULL)
	{
		return true;
	}
	if(item ->stack_ <= splitNum)
	{
		return true;
	}
	
	int slot = getFirstEmptySlot();
	
	if(-1 == slot)
		return true;

	std::vector<COM_Item*> newitems;
	genItemInst(item->itemId_,splitNum,newitems,item->isBind_,item->isLock_);
	if(newitems.empty())
	{
		CALL_CLIENT(this,errorno(EN_NewItemError));
		return true;
	}
	item->stack_ -= splitNum;
	
	for (size_t i = 0; i < newitems.size(); ++i)
	{
		if(newitems[i] == NULL)
			continue;
		newitems[i]->slot_ = slot;
		bagItmes_[slot] = newitems[i];
		CALL_CLIENT(this,addBagItem(*newitems[i]));
	}

	CALL_CLIENT(this,updateBagItem(*item));
	return true;
}

S32 Player::calcEmptyItemNum(U32 itemId){
	ItemTable::ItemData const * itemData = ItemTable::getItemById(itemId);
	if(itemData == NULL)
		return 0;

	//����ѵ�
	S32 itemStack = getItemNumByItemId(itemId) % itemData->maxCount_;
	if(itemStack){
		itemStack = itemData->maxCount_ - itemStack;
	}
	
	itemStack += getBagEmptySlot() * itemData->maxCount_;
	
	return itemStack;
}

void Player::updateItemUseTimeout(S32 tm){
	for(size_t i=0; i<equipItems_.size();++i){
		if(equipItems_[i] && equipItems_[i]->lastSellTime_ > 0){
			equipItems_[i]->lastSellTime_ -= tm;
			if(equipItems_[i]->lastSellTime_  < 0){
				equipItems_[i]->lastSellTime_ = 0;
			}
		}
		if(equipItems_[i]  && equipItems_[i]->usedTimeout_ !=0){
			equipItems_[i]->usedTimeout_ -= tm;
			if(equipItems_[i]->usedTimeout_ ==0 ){
				equipItems_[i]->usedTimeout_ -= 1;
			}

			if(equipItems_[i]->usedTimeout_ < 0){
				delEquipment(playerId_,equipItems_[i]->instId_);
			}
		}
	}
	for(size_t i=0; i<bagItmes_.size(); ++i){
		if(bagItmes_[i] && bagItmes_[i]->lastSellTime_ > 0){
			bagItmes_[i]->lastSellTime_ -= tm;
			if(bagItmes_[i]->lastSellTime_  < 0){
				bagItmes_[i]->lastSellTime_ = 0;
			}
		}

		if(bagItmes_[i] && bagItmes_[i]->usedTimeout_ != 0){
			bagItmes_[i]->usedTimeout_ -= tm;
			if(bagItmes_[i]->usedTimeout_ ==0 ){
				bagItmes_[i]->usedTimeout_ -= 1;
			}

			if(bagItmes_[i]->usedTimeout_ < 0){
				DEL_MEM(bagItmes_[i]);
				bagItmes_[i] = NULL;
				CALL_CLIENT(this,delBagItem(i));
			}
		}
	}
	for (size_t i=0;i<itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] && itemStorage_[i]->lastSellTime_){
			itemStorage_[i]->lastSellTime_ -= tm;
			if(itemStorage_[i]->lastSellTime_  < 0){
				itemStorage_[i]->lastSellTime_ = 0;
			}
		}
		if(itemStorage_[i] && itemStorage_[i]->usedTimeout_ != 0){
			itemStorage_[i]->usedTimeout_ -= tm;
			if(itemStorage_[i]->usedTimeout_ ==0 ){
				itemStorage_[i]->usedTimeout_ -= 1;
			}

			if(itemStorage_[i]->usedTimeout_ < 0){
				DEL_MEM(itemStorage_[i]);
				itemStorage_[i] = NULL;
			}
		}
	}
}
void Player::updateBabySellTimeout(S32 tm){
	for(size_t i=0; i<babies_.size(); ++i){
		babies_[i]->lastSellTime_ -= tm;
		if(babies_[i]->lastSellTime_  < 0){
			babies_[i]->lastSellTime_ = 0;
		}
	}

	for(size_t i=0; i<babyStorage_.size(); ++i){
		if(babyStorage_[i]){
			babyStorage_[i]->lastSellTime_ -= tm;
			if(babyStorage_[i]->lastSellTime_  < 0){
				babyStorage_[i]->lastSellTime_ = 0;
			}
		}
	}
}
void
Player::randSkillExp(U32 itemId)
{
	ItemTable::ItemData const * itemData = ItemTable::getItemById(itemId);
	if(itemData == NULL)
		return;
	Profession const * prof = Profession::get((JobType)(int)getProp(PT_Profession),(int)getProp(PT_ProfessionLevel));
	if(NULL == prof)
		return; //û��ְҵ
	std::vector<U32> skIDs;
	for (size_t i = 0; i < skills_.size(); ++i)
	{
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(pCore == NULL || pCore->exp_ == 0)
			continue;
		if(skills_[i]->skLevel_ >= prof->getSkillMaxLevel(skills_[i]->skId_)){
			continue; ///ְҵ�ļ��ܵȼ�����
		}
		if(!canLevelUpSkill(skills_[i]->skId_,skills_[i]->skLevel_))
			continue;

		skIDs.push_back(skills_[i]->skId_);
	}

	if(skIDs.empty())
	{
		CALL_CLIENT(this, errorno(EN_NoUpSkill));
		return;
	}
	U32 index = UtlMath::randN(skIDs.size());
	addSkillExp(skIDs[index],itemData->addValue_,IUF_Scene);
}

///@}

///========================================================================
///@group PlayerEquips
///@{
void Player::initEquipItems(std::vector<COM_Item>& equipItems)
{	
	std::vector<COM_Item> cache;

	for(size_t i=0; i<equipItems_.size(); ++i)
	{
		if(equipItems_[i] != NULL)
		{
			cache.push_back(*equipItems_[i]); 
		}
	}
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("InitEquip Ok\n")));
	CALL_CLIENT(this,initPlayerEquips(cache));
}

bool Player::wearEquipment(U32 target,U32 itemInstId)
{
	// taget ΪinstID
	Entity* entity = getEntity(target);
	if(NULL == entity)
		return true;

	COM_Item*current = getBagItemByInstId(itemInstId);
	if(NULL == current)
		return true;
	
	const ItemTable::ItemData *item = ItemTable::getItemById(current->itemId_);
	if(NULL == item)
		return true;

	if(item->mainType_ != IMT_Equip && item->mainType_ != IMT_EmployeeEquip&& item->mainType_ != IMT_BabyEquip)
		return true;
	
	if(item->bindType_ == BIT_Use)
		current->isBind_ = true;

	if(item->mainType_ == IMT_Equip ){
		JobType jt = (JobType)(int)entity->getProp(PT_Profession);
		U32 level = (U32) entity->getProp(PT_ProfessionLevel);
		const Profession* profession = Profession::get(jt,level);
		if(NULL == profession)
			return true;
		if(!profession->canUseItem(item->subType_,item->level_))
			return true;

		if(item->subType_ >= IST_Axe && item->subType_ <=IST_Knife ){
			if(item->slot_ == ES_SingleHand){ //װ����������
				if(equipItems_[ES_DoubleHand] != NULL){
					const ItemTable::ItemData *item2 = ItemTable::getItemById(equipItems_[ES_DoubleHand]->itemId_);
					if(NULL == item2){
						//CALL_CLIENT(this,wearEquipmentOk(0,*current));
						errorMessageToC(EN_BagFull);
						return true;
					}
					if(item2->subType_ != IST_Shield){ //��һﲻ�Ƕ�
						//CALL_CLIENT(this,wearEquipmentOk(0,*current));
						errorMessageToC(EN_BagFull);
						return true;
					}
				}
			}else{
				if(equipItems_[ES_SingleHand] != NULL){
					//CALL_CLIENT(this,wearEquipmentOk(0,*current));
					errorMessageToC(EN_BagFull);
					return true;
				}
			}
		}
	}
	
	bagItmes_[current->slot_] = NULL;
	CALL_CLIENT(this,delBagItem(current->slot_));
	COM_Item* takeoffEquip = entity->wearEquipment(current);
	CALL_CLIENT(this,wearEquipmentOk(target,*current));
	if(takeoffEquip)
	{
		if(entity->asEmployee()){
			DEL_MEM(takeoffEquip);
			return true;
		}
		else{
			takeoffEquip->slot_ = getFirstEmptySlot();
			bagItmes_[takeoffEquip->slot_ ] = takeoffEquip;
			CALL_CLIENT(this,addBagItem(*takeoffEquip));
			CALL_CLIENT(this,delEquipmentOk(target,takeoffEquip->instId_));
			const ItemTable::ItemData *item2 = ItemTable::getItemById(takeoffEquip->itemId_);
			if(NULL == item2)
				return NULL;

			if(entity == this){
				if(item2->titleId_ != 0){
					delPlayerTitle(item2->titleId_);
				}
			}
		}
	}
	if(entity == this){
		addPlayerTitle(item->titleId_);
		SceneBroadcaster broadcaster;
		calcBroadcastPlayers(broadcaster);
		broadcaster.scenePlayerWearEquipment(getGUID(),item->id_);

		if(myTeam())
			myTeam()->scenePlayerWearEquipment(getGUID(),item->id_);
	}
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = current->itemId_;
	GameEvent::procGameEvent(GET_WearEquip,params,1,getHandleId());
	
	calcFightingForce();
	ACE_DEBUG((LM_INFO,"Player wear equipment %d %s %d\n",getGUID(),getNameC(),itemInstId));
	return true;
}

bool Player::delEquipment(U32 target,U32 instId)
{	
	Entity* entity = getEntity(target);
	if(NULL == entity)
		return true;
	if(entity->asEmployee())
		return true;

	U32 slot  = getFirstEmptySlot();
	if(-1 == slot )
		return false;
	
	COM_Item* equip  = entity->takeoffEquipment(instId);
	if(NULL == equip)
		return true;
	
	const ItemTable::ItemData *item = ItemTable::getItemById(equip->itemId_);
	if(item == NULL)
		return true;
	equip->slot_ = slot;
	bagItmes_[slot] = equip;
	if(entity == this){
		delPlayerTitle(item->titleId_);
		if(item->titleId_ == getProp(PT_Title))
			setProp(PT_Title, 0);
		SceneBroadcaster broadcaster;
		calcBroadcastPlayers(broadcaster);
		broadcaster.scenePlayerDoffEquipment(getGUID(),item->id_);

		if(myTeam())
			myTeam()->scenePlayerWearEquipment(getGUID(),item->id_);
	}

	CALL_CLIENT(this,delEquipmentOk(target,instId));
	CALL_CLIENT(this,addBagItem(*equip));
	ACE_DEBUG((LM_INFO,"Player delete equipment %d %s %d\n",getGUID(),getNameC(),instId));
	return true;
}

Entity* Player::getEntity(U32 id){
	// id ΪinstID
	Entity* player=NULL;
	if(playerId_==id){
		return this;
	}

	for(size_t i=0; i<babies_.size(); ++i){
		if(id == babies_[i]->babyId_)
			return babies_[i];
	}

	for(size_t i=0; i < employees_.size(); ++i){
		if(id == employees_[i]->instId_){
			return employees_[i];
		}
	}
	return NULL;
}


///@}
///========================================================================

///@group Inside Function
///@{

bool Player::drawLotteryBox(BoxType type, bool isfree){
	if(type == BX_Normal){
		Employeeoutput::rollGreen(this);		
	}
	else if(type == BX_Blue){
		Employeeoutput::rollBlue(this);
	}
	else if(type == BX_Glod){
		Employeeoutput::rollGold(this);
	}
	return true;	
}

///@}
///========================================================================

///@group Inside Function
///@{

void 
Player::getScenePlayerInfo(SGE_ScenePlayerInfo& info){
	if( !(guideIdx_ & (0x1 << (U64)Global::get<int>(C_GuideNewSceneId0)))){
		PlayerTmpTable::Core const * p = PlayerTmpTable::getTemplateById((int)getProp(PT_TableId));
		if(p)
			info.sceneId_ = p->defaultSceneId_;
	}else if( !(guideIdx_ & (0x1 << (U64)Global::get<int>(C_GuideNewSceneId1))))
	{
		PlayerTmpTable::Core const * p = PlayerTmpTable::getTemplateById((int)getProp(PT_TableId));
		if(p)
			info.sceneId_ = p->defaultSceneId_;
	}else 
		info.sceneId_ = SceneTable::getHomeScene()->sceneId_; 
	info.entryId_ = 0;
	info.playerId_ = playerId_;
	info.playerLevel_ = (S32)getProp(PT_Level);
	info.openScenes_ = openScenes_;
	for(size_t i=0; i<currentQuest_.size(); ++i)
		info.currentQuestIds_.push_back(currentQuest_[i].questId_);
}

void 
Player::getSimpleInfo(COM_SimpleInformation &out){
	out.instId_ = playerId_;
	out.instName_ = playerName_;
	out.level_ = (S32)getProp(PT_Level);
	out.asset_id_ = (S32)getProp(PT_AssetId);
	out.jt_ = (JobType)(S32)getProp(PT_Profession);
	out.jl_ = (S32)getProp(PT_ProfessionLevel);
	out.section_ = pvpInfo_.section_;
	if(equipItems_[ES_SingleHand])
		out.weaponItemId_ = equipItems_[ES_SingleHand]->itemId_;
	else if(equipItems_[ES_DoubleHand])
		out.weaponItemId_ = equipItems_[ES_DoubleHand]->itemId_;
	if(equipItems_[ES_Fashion])
		out.fashionId_ = equipItems_[ES_Fashion]->itemId_;
}

void Player::getSimplePlayerInst(COM_SimplePlayerInst& out){
	out.isBattle_ = isBattle();
	out.autoBattle_ = autoBattle_;
	out.isLeavingTeam_ = isLeavingTeam_;
	out.openSubSystemFlag_ = openSubSystemFlag_;
	out.sceneId_ =  GET_SCENE_ORIGINAL_ID(sceneId_);
	out.scenePos_ = position_;
	out.createTime_ = createTime_;
	out.isTeamLeader_ = isTeamLeader();
	getEntityInst(out);
	out.type_ = ET_Player;
	std::vector<U32>& emps = getCurrentBattleEmployees();
	for (size_t i = 0; i < emps.size(); ++i){
		if(emps[i] == 0)
			continue;
		Employee* pEmp = findEmployee(emps[i]);
		if(pEmp == NULL){
			ACE_DEBUG((LM_ERROR,ACE_TEXT("getSimplePlayerInst BattleEmployee is NULL EmployeeInstId[%d]\n"),emps[i]));
			continue;
		}
		COM_EmployeeInst inst;
		pEmp->getEmployeeInst(inst);
		out.battleEmps_.push_back(inst);
	}

	for(size_t i=0; i<babies_.size(); ++i){
		COM_BabyInst binst;
		babies_[i]->getBabyInst(binst);
		out.babies1_.push_back(binst);
	}
	out.pvpInfo_ = pvpInfo_;

	if(myGuild()){
		out.guildName_ = myGuild()->guildData_.guildName_;
	}
}

void
Player::getSimpleScenePlayer(COM_ScenePlayerInformation& out){
	out.isLeader_ = isTeamLeader();
	out.isTeamMember_ = isTeamMember();
	out.isInBattle_ = isBattle();
	out.vip_ = (int16)getProp(PT_VipLevel);
	out.instId_ = playerId_;
	out.instName_ = playerName_;
	out.assetId_ = properties_[PT_AssetId];
	out.hpMax_ = properties_[PT_HpMax];
	out.mpMax_ = properties_[PT_MpMax];
	out.hpCrt_ = properties_[PT_HpCurr];
	out.mpCrt_ = properties_[PT_MpCurr];
	out.level_ = properties_[PT_Level];
	out.battlePower_ = properties_[PT_FightingForce];
	out.jl_ = properties_[PT_ProfessionLevel];
	out.jt_ = (JobType)(S32)properties_[PT_Profession];
	out.title_ = properties_[PT_Title];
	out.openSubSystemFlag_ = openSubSystemFlag_;
	out.originPos_ = position_;
	out.magicLv_ = magicItemLevel_;
	out.type_ = ET_Player;

	if(equipItems_[ES_SingleHand])
		out.weaponItemId_ = equipItems_[ES_SingleHand]->itemId_;
	else if(equipItems_[ES_DoubleHand])
		out.weaponItemId_ = equipItems_[ES_DoubleHand]->itemId_;
	if(equipItems_[ES_Fashion])
		out.fashionId_ = equipItems_[ES_Fashion]->itemId_;

	if(myGuild()){
		out.guildeName_ = myGuild()->guildData_.guildName_;
	}

	for(size_t i=0; i<babies_.size(); ++i){
		if(babies_[i]->isShow_){
			out.showBabyTableId_ = babies_[i]->tableId_;
			out.showBabyName_ = babies_[i]->getNameC();
		}
	}
}

void Player::getBattleEntityInformation(COM_BattleEntityInformation& info){
	Entity::getBattleEntityInformation(info);
	info.type_ = ET_Player;
	info.instName_ = playerName_;
}

void 
Player::setPlayerInst(COM_PlayerInst &tmp)
{
	Entity::setEntityInst(tmp);
	createTime_ = tmp.createTime_;
	onlinetimeflag_ = tmp.onlineTimeFlag_;
	onlinetime_	= tmp.onlineTime_;
	onlinereward_ = tmp.onlineTimeReward_;
	isFund_ = tmp.isFund_;
	fundtag_ = tmp.fundtags_;
	isFirstLogin_ = tmp.isFirstLogin_;
	openSubSystemFlag_ = tmp.openSubSystemFlag_;
	openDoubleTimeFlag_ = tmp.openDoubleTimeFlag_;
	playerId_ = tmp.instId_;
	playerName_ = tmp.instName_;
	properties_.resize(PT_Max);
	guideIdx_ = tmp.guideIdx_;
	greenBoxTimes_		= tmp.greenBoxTimes_;	
	blueBoxTimes_		= tmp.blueBoxTimes_;	
	greenBoxFreeNum_	= tmp.greenBoxFreeNum_;
	activity_ = tmp.activity_;
	//activationCounter_	= tmp.activationCounter_;
	babycache_			= tmp.babycache_;
	wishShareNum_		= tmp.wishShareNum_;
	warriortrophyNum_	= tmp.warriortrophyNum_;
	guildSkills_		= tmp.guildSkills_;
	pvpInfo_ = tmp.pvpInfo_;
	openScenes_ = tmp.openScenes_;
	copynums_  = tmp.copyNum_;
	magicItemLevel_ = tmp.magicItemLevel_;
	magicItemExp_ = tmp.magicItemeExp_;
	magicItemJob_ =JT_Axe;// tmp.magicItemeJob_;
	magicTupoLevel_ = tmp.magicTupoLevel_;
	titles_ = tmp.titles_;
	exitGuildTime_ = tmp.exitGuildTime_;
	guildContribution_ = tmp.guildContribution_;
	festival_ = tmp.festival_;
	firstRechargeDiamond_ = tmp.firstRechargeDiamond_;
	isFirstRechargeGift_ = tmp.isFirstRechargeGift_;
	myselfrecharge_ = tmp.myselfRecharge_ ; //�����ۼƳ�ֵ��פ��
	selfRecharge_ = tmp.selfRecharge_ ; //�����ۼƳ�ֵ
	sysRecharge_ = tmp.sysRecharge_ ; //ϵͳ�ۼƳ�ֵ
	selfDiscountStore_ = tmp.selfDiscountStore_; //������
	sysDiscountStore_ = tmp.sysDiscountStore_ ; //������
	selfOnceRecharge_ = tmp.selfOnceRecharge_ ; ///���ʳ�ֵ
	sysOnceRecharge_ = tmp.sysOnceRecharge_; 
	empact_ = tmp.empact_;
	selfCards_ = tmp.selfCards_;
	hotdata_ = tmp.hotdata_;
	sevenday_ = tmp.sevendata_;
	sevenflag_ = tmp.sevenflag_;
	gbact_ = tmp.gbdata_;
	viprewardflag_ = tmp.viprewardflag_;
	levelgift_ = tmp.levelgift_;
	properties_ = tmp.properties_;
	crystalData_ = tmp.crystalData_;
	icdata_ = tmp.integralData_;
	coursegift_ = tmp.coursegift_;
	//phoneNumber_ = tmp.phoneNumber_;
	//if(activationCounter_.empty())
	//	activationCounter_.resize(ACT_Max,NULL);
	for(size_t i=0; i<tmp.fuwen_.size(); ++i){
		fuwen_[tmp.fuwen_[i].slot_] = NEW_MEM(COM_Item,tmp.fuwen_[i]);
	}
	achValues_.resize(AT_Max);
	initSkillByInst(tmp.skill_);
}

void 
Player::getPlayerInst(COM_PlayerInst &out)
{
	//getSimplePlayerInst(out);
	
	getEntityInst(out);
	out.isBattle_ = isBattle();
	out.autoBattle_ = autoBattle_;
	out.isLeavingTeam_ = isLeavingTeam_;
	out.sceneId_ =  GET_SCENE_ORIGINAL_ID(sceneId_);
	out.scenePos_ = position_;
	out.createTime_ = createTime_;
	out.onlineTimeFlag_ = onlinetimeflag_;
	out.onlineTime_ = onlinetime_;
	out.onlineTimeReward_ = onlinereward_;
	out.isFund_ = isFund_;
	out.fundtags_ = fundtag_;
	out.isTeamLeader_ = isTeamLeader();
	out.isFirstLogin_ = isFirstLogin_;
	out.openSubSystemFlag_ = openSubSystemFlag_;
	out.openDoubleTimeFlag_ = openDoubleTimeFlag_;
	out.guideIdx_ = guideIdx_;
	out.type_ = ET_Player;
	out.hbInfo_.tier_ = tier_;
	out.hbInfo_.surplus_ = hundredNum_;
	out.hbInfo_.resetNum_ = hundredresetNum_;
	out.hbInfo_.curTier_ = curTier_;
	out.rivalNum_ = rivalNum_;
	out.rivalTime_ = rivalTimes_;
	out.greenBoxTimes_		= greenBoxTimes_;	
	out.blueBoxTimes_		= blueBoxTimes_;	
	out.greenBoxFreeNum_	= greenBoxFreeNum_;
	out.pvpInfo_			= pvpInfo_;
	out.babycache_			= babycache_;
	out.openScenes_ = openScenes_;
	out.copyNum_ = copynums_;
	out.magicItemLevel_ = magicItemLevel_;
	out.magicItemeExp_ = magicItemExp_;
	out.magicItemeJob_ = JT_Axe;
	out.magicTupoLevel_ = magicTupoLevel_;
	out.wishShareNum_	= wishShareNum_;
	out.warriortrophyNum_ = warriortrophyNum_;
	out.titles_ = titles_;
	out.guildContribution_ = guildContribution_;
	out.guildSkills_ = guildSkills_;
	out.exitGuildTime_ = exitGuildTime_;
	out.festival_ = festival_;
	out.firstRechargeDiamond_ = firstRechargeDiamond_;
	out.isFirstRechargeGift_ = isFirstRechargeGift_;
	out.myselfRecharge_ = myselfrecharge_ ; //�����ۼƳ�ֵ��פ��
	out.selfRecharge_ = selfRecharge_ ; //�����ۼƳ�ֵ
	out.sysRecharge_ = sysRecharge_ ; //ϵͳ�ۼƳ�ֵ
	out.selfDiscountStore_ = selfDiscountStore_; //������
	out.sysDiscountStore_ = sysDiscountStore_ ; //������
	out.selfOnceRecharge_ = selfOnceRecharge_ ; ///���ʳ�ֵ
	out.sysOnceRecharge_ = sysOnceRecharge_; 
	out.empact_ = empact_;
	out.selfCards_ = selfCards_;
	out.hotdata_ = hotdata_;
	out.gbdata_ = gbact_;
	out.sevendata_ = sevenday_;
	out.levelgift_ = levelgift_;
	out.sevenflag_ = sevenflag_;
	out.signFlag_ = signFlag_;
	out.viprewardflag_ = viprewardflag_;
	if(onlinetimeflag_)
		out.gmActivities_.push_back(ADT_OnlineReward);
	out.crystalData_ = crystalData_;
	out.integralData_ = icdata_;
	out.coursegift_ = coursegift_;
	out.gmActivities_.push_back(ADT_Foundation);
	out.gmActivities_.push_back(ADT_Cards);
	if(account_)
		out.phoneNumber_ = account_->phoneNumber_;
	if(!out.hotdata_.contents_.empty())
		out.gmActivities_.push_back(ADT_HotRole);
	if(!out.empact_.contents_.empty())
		out.gmActivities_.push_back(ADT_BuyEmployee);
	if(!out.sevendata_.empty())
		out.gmActivities_.push_back(ADT_7Days);
	if(!festival_.contents_.empty())
		out.gmActivities_.push_back(ADT_LoginTotal);
	
	if(!out.integralData_.contents_.empty())
		out.gmActivities_.push_back(ADT_IntegralShop);
	
	if(!out.sysRecharge_.contents_.empty())
		out.gmActivities_.push_back(ADT_ChargeTotal);
	if(!out.sysDiscountStore_.contents_.empty())
		out.gmActivities_.push_back(ADT_DiscountStore);
	if(!out.sysOnceRecharge_.contents_.empty())
		out.gmActivities_.push_back(ADT_ChargeEvery);
	if(!out.selfDiscountStore_.contents_.empty())
		out.gmActivities_.push_back(ADT_SelfDiscountStore);
	if(Zhuanpan::isopen_)
		out.gmActivities_.push_back(ADT_Zhuanpan);
	for(size_t i=0; i<out.selfOnceRecharge_.contents_.size(); ++i){
		if(out.selfOnceRecharge_.contents_[i].status_ == 2)
			continue;
		out.gmActivities_.push_back(ADT_SelfChargeEvery);
		break;
	}

	for(size_t i=0; i<out.selfRecharge_.contents_.size(); ++i){
		if(out.selfRecharge_.contents_[i].status_ == 2)
			continue;
		out.gmActivities_.push_back(ADT_SelfChargeTotal);
		break;
	}
	
	for(size_t i=0; i<fuwen_.size(); ++i){
		if(fuwen_[i])
			out.fuwen_.push_back(*fuwen_[i]);
	}
	out.activity_ = activity_;
}

void 
Player::setDBPlayerData(SGE_DBPlayerData &tmp)
{
	setPlayerInst(tmp);
	signs_ = tmp.signs_;

	isFirstLogin_		= tmp.isFirstLogin_;
	openSubSystemFlag_	= tmp.openSubSystemFlag_;
	openDoubleTimeFlag_ = tmp.openDoubleTimeFlag_;
	playerId_			= tmp.instId_;
	playerName_			= tmp.instName_;
	genItemMaxGuid_		= tmp.genItemMaxGuid_;
	rivalNum_			= tmp.rivalNum_;
	rivalTimes_			= tmp.rivalTime_;
	promoteAward_		= tmp.promoteAward_;
	logoutTime_			= tmp.logoutTime_;
	tier_				= tmp.hbInfo_.tier_;
	hundredNum_			= tmp.hbInfo_.surplus_;
	hundredresetNum_	= tmp.hbInfo_.resetNum_;
	greenBoxTimes_		= tmp.greenBoxTimes_;	
	blueBoxTimes_		= tmp.blueBoxTimes_;	
	greenBoxFreeNum_	= tmp.greenBoxFreeNum_;
	employeelasttime_	= tmp.employeelasttime_;
	employeeonecount_	= tmp.employeeonecount_;
	employeetencount_	= tmp.employeetencount_;
	achievement_		= tmp.achievement_;

	achValues_ = tmp.achValues_;
	friend_ = tmp.friend_;
	blacklist_ = tmp.blacklist_;
	pvpInfo_			= tmp.pvpInfo_;
	magicItemLevel_ = tmp.magicItemLevel_;
	magicItemExp_ = tmp.magicItemeExp_;
	magicItemJob_ =JT_Axe;// tmp.magicItemeJob_;
	magicTupoLevel_ = tmp.magicTupoLevel_;
	empbattlegroup_ = tmp.empBattleGroup_;
	firstRollEmployeeCon_ = tmp.firstRollEmployeeCon_; //��һ�ν��roll
	firstRollEmployeeDia_ = tmp.firstRollEmployeeDia_ ; //��һ����ʯroll
	accecptQuestCount_ = tmp.acceptRandQuestCounter_ ;
	submitQuestCount_ = tmp.submitRandQuestCounter_;
	currentQuest_ = tmp.quests_;
	completeQuest_ = tmp.completeQuests_;
	battleMsg_ = tmp.jjcBattleMsg_;
	gaterMaxNum_ = tmp.gaterMaxNum_;
	gathers_   = tmp.gatherData_;
	compounds_ = tmp.compoundList_;
	signFlag_ = tmp.signFlag_;
	babies_.clear();
	for(size_t i=0; i<tmp.babies_.size(); ++i)
	{
		if (tmp.babies_[i].instId_ == 0){
			//ACE_DEBUG((LM_DEBUG, "if (tmp.babyStorage_[i].instId_ == 0) baby name = %s \n", tmp.babyStorage_[i].instName_.c_str()));
			continue;
		}
		if (tmp.babies_[i].slot_ != -1){
			//ACE_DEBUG((LM_DEBUG, "if (tmp.babies_[i].slot_ != -1) baby name = %s \n", tmp.babies_[i].instName_.c_str()));
			continue;
		}
		Baby * pBaby = NEW_MEM(Baby,this);
		SRV_ASSERT(pBaby);
		pBaby->setBabyInst(tmp.babies_[i]);
		babies_.push_back(pBaby);
	}

	employees_.clear();
	for(size_t i=0; i<tmp.employees_.size(); ++i)
	{
		Employee * pEmployee = NEW_MEM(Employee,this);
		SRV_ASSERT(pEmployee);
		pEmployee->setEmployeeInst(tmp.employees_[i]);
		employees_.push_back(pEmployee);
	}

	for(size_t i=0; i<tmp.bagItems_.size(); ++i)
	{
		COM_Item *pItem = NEW_MEM(COM_Item,tmp.bagItems_[i]);
		bagItmes_[pItem->slot_] = pItem;
	}

	itemStorageSize_ = tmp.itemStoreSize_;
	for (size_t i = 0; i < tmp.itemStorage_.size(); ++i){
		if(tmp.itemStorage_[i].instId_ == 0)
			continue;
		COM_Item *pItem = NEW_MEM(COM_Item,tmp.itemStorage_[i]);
		itemStorage_[pItem->slot_] = pItem;
	}

	babyStorageSize_ = tmp.babyStoreSize_;
	std::vector<COM_BabyInst*> tmp_invalid_baby_store;
	for (size_t i = 0; i < tmp.babies_.size(); ++i)
	{
		if (tmp.babies_[i].instId_ == 0){
			//ACE_DEBUG((LM_DEBUG, "if (tmp.babyStorage_[i].instId_ == 0) baby name = %s \n", tmp.babyStorage_[i].instName_.c_str()));
			continue;
		}
		if(tmp.babies_[i].slot_ == -1)
			continue;
		
		COM_BabyInst* pbaby = NEW_MEM(COM_BabyInst, tmp.babies_[i]);
		if(babyStorage_[pbaby->slot_] == NULL)
			babyStorage_[pbaby->slot_] = pbaby;
		else
			tmp_invalid_baby_store.push_back(pbaby);
	}

	for (size_t i = 0; i < babyStorage_.size(); ++i){
		if ((babyStorage_[i] == NULL)&& (!tmp_invalid_baby_store.empty())){
			ACE_DEBUG((LM_DEBUG,ACE_TEXT("SAME BABY[%d] STORAGE SLOT[%d],IS NEW SLOT[%d]\n"),tmp_invalid_baby_store.back()->instId_,tmp_invalid_baby_store.back()->slot_,i));
			babyStorage_[i] = tmp_invalid_baby_store.back();
			babyStorage_[i]->slot_ = i;
			tmp_invalid_baby_store.pop_back();
		}
	}

	for (size_t i = 0; i < tmp_invalid_baby_store.size(); ++i){
		ACE_DEBUG((LM_DEBUG, "for (size_t i = 0; i < tmp_invalid_baby_store.size(); ++i) %s \n", tmp_invalid_baby_store[i]->instName_.c_str()));
		DEL_MEM(tmp_invalid_baby_store[i]);
	}

	gifttype_ = tmp.gft_;
	npcList_ = tmp.cachedNpcs_;
	if(!tmp.employeeGroup1_.empty()){
		battleEmpsGroup1_.resize(tmp.employeeGroup1_.size(),0);
		for(size_t i=0; i<tmp.employeeGroup1_.size(); ++i){
			if(findEmployee(tmp.employeeGroup1_[i]))
				battleEmpsGroup1_[i] = tmp.employeeGroup1_[i];
		}
	}

	if(!tmp.employeeGroup2_.empty()){
		battleEmpsGroup2_.resize(tmp.employeeGroup2_.size(),0);
		for(size_t i=0; i<tmp.employeeGroup2_.size(); ++i){
			if(findEmployee(tmp.employeeGroup2_[i]))
				battleEmpsGroup2_[i] = tmp.employeeGroup2_[i];
		}
	}
	
	orders_ = tmp.orders_;
}

void 
Player::getDBPlayerData(SGE_DBPlayerData &out)
{
	getPlayerInst(out);
	
	out.versionNumber_ = VERSION_NUMBER;
	out.signs_ = signs_;
	out.isFirstLogin_ = isFirstLogin_;
	out.openSubSystemFlag_ = openSubSystemFlag_;
	out.openDoubleTimeFlag_ = openDoubleTimeFlag_;
	out.instId_= playerId_;
	out.instName_ = playerName_;
	out.logoutTime_ = logoutTime_;
	out.greenBoxTimes_		= greenBoxTimes_;	
	out.blueBoxTimes_		= blueBoxTimes_;	
	out.greenBoxFreeNum_	= greenBoxFreeNum_;
	out.employeelasttime_	= employeelasttime_;
	out.employeeonecount_	= employeeonecount_;
	out.employeetencount_	= employeetencount_;
	out.jjcBattleMsg_ = battleMsg_;
	out.genItemMaxGuid_ = genItemMaxGuid_;
	out.rivalNum_ = rivalNum_;
	out.rivalTime_ = rivalTimes_;
	out.promoteAward_ = promoteAward_;
	out.hbInfo_.curTier_ = tier_;
	out.hbInfo_.surplus_ = hundredNum_;
	out.pvpInfo_ = pvpInfo_;
	out.magicItemLevel_ = magicItemLevel_;
	out.magicItemeExp_= magicItemExp_;
	out.magicItemeJob_ =JT_Axe;// magicItemJob_;
	out.magicTupoLevel_ = magicTupoLevel_;
	out.firstRollEmployeeCon_ = firstRollEmployeeCon_; //��һ�ν��roll
	out.firstRollEmployeeDia_ = firstRollEmployeeDia_ ; //��һ����ʯroll
	out.achValues_ = achValues_;
	out.achievement_ = achievement_;
	out.friend_ = friend_;
	out.blacklist_ = blacklist_;
	out.gaterMaxNum_ = gaterMaxNum_;
	out.gatherData_ = gathers_;
	out.compoundList_ = compounds_;
	out.compoundList_ = compounds_;
	out.quests_ = currentQuest_;
	out.completeQuests_ = completeQuest_;
	out.acceptRandQuestCounter_ = accecptQuestCount_;
	out.submitRandQuestCounter_ = submitQuestCount_;
	
	out.babies_.resize(babies_.size());
	for(size_t i=0; i<babies_.size(); ++i){
		babies_[i]->getBabyInst(out.babies_[i]);
	}
	for(size_t i=0; i<babyStorage_.size();++i){
		if(babyStorage_[i] == NULL)
			continue;
		out.babies_.push_back(*babyStorage_[i]);
	}
	out.employees_.resize(employees_.size());
	for(size_t i=0; i<employees_.size(); ++i)
	{
		if(employees_[i])
			employees_[i]->getEmployeeInst(out.employees_[i]);
	}

	for (size_t i=0;i<bagItmes_.size(); ++i)
	{
		if(bagItmes_[i]!=NULL)
			out.bagItems_.push_back(*bagItmes_[i]);
	}

	out.itemStoreSize_ = itemStorageSize_;
	for (size_t i = 0; i < itemStorage_.size(); ++i)
	{
		if(itemStorage_[i] != NULL)
			out.itemStorage_.push_back(*itemStorage_[i]);
	}

	out.babyStoreSize_ = babyStorageSize_;

	out.cachedNpcs_ = npcList_;
	
	out.gft_ = gifttype_;
	out.empBattleGroup_ = empbattlegroup_;
	out.employeeGroup1_ = battleEmpsGroup1_;
	out.employeeGroup2_ = battleEmpsGroup2_;

	out.orders_ = orders_;

	if (account_){
		out.pfid_ = account_->sdkInfo_.pfId_;
	}
}
///@}

Baby* 
Player::findBaby(U32 instId)
{
	for (size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->babyId_ == instId)
			return babies_[i];
	}
	return NULL;
}

Baby*
Player::findBabybyTableId(U32 tableId)
{
	for (size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->tableId_ == tableId)
			return babies_[i];
	}
	return NULL;
}

void 
Player::selectBaby(U32 instId,bool isBattle)
{
	//isBattle true�Ƿ�, false����

	if(isBattle == false)
	{
		if(getBattleBaby() == NULL)
		{
			ACE_DEBUG((LM_ERROR,"Has any battle baby!!!\n"));
			return;
		}
		getBattleBaby()->battleId_ = 0;
		getBattleBaby()->isBattle_ = false;
	}
	else
	{
		Baby *pBaby = findBaby(instId);
		if(NULL == pBaby)
			return ;

		if((true == isBattle) && getBattleBaby() != NULL)
		{
			getBattleBaby()->isBattle_ = false;
		}

		pBaby->battleId_ = battleId_;
		pBaby->isBattle_ = true;
	}
}

void Player::lockBaby(U32 instId, bool isLock){
	for(size_t i=0; i<babies_.size(); ++i){
		if(babies_[i] && babies_[i]->babyId_ == instId){
			babies_[i]->isLock_ = isLock;
			COM_BabyInst bi;
			babies_[i]->getBabyInst(bi);
		
			CALL_CLIENT(this,refreshBaby(bi));
			return;
		}
	}

	for( size_t i=0; i<babyStorage_.size(); ++i){
		if(babyStorage_[i] && babyStorage_[i]->instId_ == instId){
			babyStorage_[i]->isLock_ = isLock;
			//CALL_CLIENT(this,updateBaby(*babyStorage_[i]));
			return;
		}
	}
}

void Player::showBaby(U32 instId){
	if(instId == 0){
		for(size_t i=0; i<babies_.size(); ++i){
			if(babies_[i]->isShow_){
				babies_[i]->isShow_ = false;
			}
		}
		SceneBroadcaster broadcaster;
		calcBroadcastPlayers(broadcaster);
		broadcaster.updateShowBaby(playerId_,0,"");
	}
	else {
		for(size_t i=0; i<babies_.size(); ++i){
			if(babies_[i]->isShow_){
				babies_[i]->isShow_ = false;
			}
		}
		for(size_t i=0; i<babies_.size(); ++i){
			if(babies_[i]->babyId_ == instId){
				babies_[i]->isShow_ = true;
				SceneBroadcaster broadcaster;
				calcBroadcastPlayers(broadcaster);
				broadcaster.updateShowBaby(playerId_,babies_[i]->tableId_,babies_[i]->babyName_);
				return;
			}
		}
	}
}
bool
Player::checkBabyCache(S32 monsterId)
{
	for (size_t i = 0; i < babycache_.size(); ++i)
	{
		if(babycache_[i] == monsterId)
			return true;
	}
	return false;
}

U32
Player::calchasBabybyRace(RaceType type)
{
	U32 num = 0;
	for (size_t i = 0; i < babycache_.size(); ++i)
	{
		MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(babycache_[i]);
		if(tmp == NULL)
			continue;
		if(type == tmp->race_)
			++num;
	}
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("baby race[%d]======> num==[%d] \n"),type,num));
	return num;
}

void Player::initBattleStatus(U32 battleId,GroupType battleForce,BattlePosition battlePos,bool initActive ){
	InnerPlayer::initBattleStatus(battleId,battleForce,battlePos,initActive);
	
	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(s) s->syncBattleStatus(playerId_,true);

	Team* t = myTeam();
	if(t) t->syncBattleStatus(playerId_,true);
	

	COM_GuildMember* g = myGuildMember();
	if(g)
	{
		for(size_t i=0; i<guildSkills_.size(); ++i){
			SkillTable::Core const* pCore = SkillTable::getSkillById(guildSkills_[i].skillID_,guildSkills_[i].skillLevel_);
			SRV_ASSERT(pCore);
			if(pCore->skType_ != SKT_GuildPlayerSkill)
				continue;
			addAttachedPropertyD(pCore->resistPropType_,pCore->resistNum_);
		}
	}
}

void 
Player::cleanBattleStatus(bool resetProperty)
{
 	/*Baby *pBaby = getBattleBaby();
	if(pBaby)
		pBaby->cleanBattleStatus(resetProperty);*/
	
	Entity::cleanBattleStatus(resetProperty);

	std::vector<U32>& emps = getCurrentBattleEmployees();
	for (size_t i = 0; i < emps.size(); ++i)
	{
		if(emps[i] == 0)
			continue;
		Employee* pEmp = findEmployee(emps[i]);
		if(pEmp == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("cleanBattleStatus BattleEmployee is NULL EmployeeInstId[%d]\n"),emps[i]));
			continue;
		}
		pEmp->cleanBattleStatus(resetProperty);
	}
	
	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(s) s->syncBattleStatus(playerId_,false);

	Team* t = myTeam();
	if(t) t->syncBattleStatus(playerId_,false);

	scenePlayer_->finishBattle();
}

void 
Player::syncOrder(COM_Order& order)
{
	Battle * battle = Battle::find(battleId_); ///���Ǹ�bug battleId û����
	if(NULL == battle)
	{
		//ACE_DEBUG((LM_ERROR,ACE_TEXT("Player nobattle sync order %d\n"),battleId_));
		return ;
	}
	//ACE_DEBUG((LM_DEBUG,"Player sync order %d , %d\n",getGUID(),order.casterId_));
	battle->pushOrder(order);	
}

ErrorNo
Player::usessItem(S32 target,U32 itemID)
{
	ItemTable::ItemData const *pItem = ItemTable::getItemById(itemID);
	SRV_ASSERT(pItem);

	if (!pItem)
	{
		return EN_Max;
	}

	enum 
	{
		ARG_TARGETINSTID,
		ARG_USEITEMID,		//0
		ARG_BABYSID,
		ARG_MAX_,
	};

	std::vector<S32>	babys;
	for (size_t j = 0; j < babies_.size(); ++j)
	{
		Baby* pbaby = babies_[j];
		if(pbaby == NULL)
			continue;
		babys.push_back(pbaby->getGUID());
	}

	static GEParam param[ARG_MAX_];
	param[ARG_TARGETINSTID].type_ = GEP_INT;
	param[ARG_TARGETINSTID].value_.i= target;
	param[ARG_USEITEMID].type_ = GEP_INT;
	param[ARG_USEITEMID].value_.i= itemID;
	param[ARG_BABYSID].type_ = GEP_HANDLE_ARRAY;
	param[ARG_BABYSID].value_.hArray = &babys;

	std::string err;
	int ret = EN_None;
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("LUA UseItem ACTIVE ==> %s\n"),pItem->gloAction_.c_str()));
	if(false == ScriptEnv::callGEProc(pItem->gloAction_.c_str(), this->handleId_,param,ARG_MAX_,ret,err))
	{
		return EN_Max;
	}

	return (ErrorNo)ret;
}

void 
Player::subProperty(PropertyType pt)
{
	if(pt < PT_Stama || pt > PT_Magic) return;
	if(properties_[pt] == 0) return;

	properties_[pt] -= 1;
	switch(pt)
	{
	case PT_Stama:
		{
			CALC_PLAYER_PRO_TRANS_STAMA((*this), -1);
		}
		break;
	case PT_Strength:
		{
			CALC_PLAYER_PRO_TRANS_STRENGTH((*this), -1);
		}
		break;
	case PT_Power:
		{
			CALC_PLAYER_PRO_TRANS_POWER((*this),  -1);
		}
		break;
	case PT_Speed:
		{
			CALC_PLAYER_PRO_TRANS_SPEED((*this),  -1);
		}
		break;
	case PT_Magic:
		{
			CALC_PLAYER_PRO_TRANS_MAGIC((*this),  -1);
		}
		break;
	default:
		break;
	}

	properties_[PT_HpCurr] = properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax];
	properties_[PT_Free] += 1;
	refreshProperty();
}

void
Player::resetProperty()
{
	PlayerTmpTable::Core const *tmp = PlayerTmpTable::getTemplateById((S32)getProp(PT_TableId));
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d template in player-template-table\n"),(S32)getProp(PT_TableId)));
		return ;
	}

	S32 total = properties_[PT_Stama] + properties_[PT_Strength] + properties_[PT_Power] + properties_[PT_Speed] + properties_[PT_Magic];

	if(total <= 0)
	{
		CALL_CLIENT(this, errorno(EN_PropisNull));
		return;
	}

	properties_[PT_Stama] = properties_[PT_Strength] = properties_[PT_Power] = properties_[PT_Speed] = properties_[PT_Magic] = 0;
	
	properties_[PT_HpCurr] = properties_[PT_HpMax] = tmp->properties_[PT_HpMax];
	properties_[PT_MpCurr] = properties_[PT_MpMax] = tmp->properties_[PT_MpMax];
	properties_[PT_Attack] = tmp->properties_[PT_Attack];
	properties_[PT_Defense] = tmp->properties_[PT_Defense];
	properties_[PT_Agile] = tmp->properties_[PT_Agile];
	properties_[PT_Spirit] = 100; ///BUG 409 �������߹� (�Ǳ���,���ʺܸߣ�
	properties_[PT_Reply] = 100;

	for (size_t i = PT_Magicattack; i < PT_Front; ++i)
	{
		properties_[i] = tmp->properties_[i];
	}

	for (size_t i = PT_Wind; i <= PT_Fire; ++i)
	{
		properties_[i] = tmp->properties_[i];
	}

	///�ӳ�ʼ��ģ������ʼ�� 1��2 ������
	
	properties_[PT_Free] += total;	

	for (size_t i=0; i<equipItems_.size(); ++i)
	{
		//addEquipmentEffect(equipItems_[i]);
		if(equipItems_[i] == NULL) continue;
		addEquipmentEffect(equipItems_[i]);
		//for (size_t j=0; j<equipItems_[i]->propArr.size(); ++j)
		//{
			//properties_[equipItems_[i]->propArr[j].type_] += equipItems_[i]->propArr[j].value_;
		//}
	}
	for (size_t i=0; i<fuwen_.size(); ++i){
		if(fuwen_[i] == NULL) continue;
		addEquipmentEffect(fuwen_[i]);
	}
	//����I����
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
	if(levelData  != NULL && getOpenSubSystemFlag(OSSF_MagicItem))
	{
		int levelNum = (magicTupoLevel_-5)/5;
		float num= levelNum *0.1;
		for(size_t i=0;i<levelData->propValue_.size();i++)
		{
			ArtifactLevelTable::ArtifactPropData prop =levelData->propValue_[i];
			properties_[prop.type_] += prop.value_ + prop.value_ * num;
		}
	}
	//ˮ������
	crystalUpdata(true);

	refreshProperty();
}

void 
Player::addProperty(U32 guid,const std::vector<COM_Addprop> &props)
{
	if (guid != playerId_)
	{
		Baby* pBaby = findBaby(guid);
		if (!pBaby)
			return;
		pBaby->addProperty(props);
		return;
	}
	
	COM_PlayerInst inst; //����ԭ����
	inst.properties_ = properties_;
	for (size_t i = 0; i < props.size(); ++i)
	{

		if (props[i].uVal_ > inst.properties_[PT_Free] || props[i].uVal_ <= 0)
			continue;
		float propv = inst.properties_[props[i].type_] + props[i].uVal_;
		if(propv > CALC_PLAYER_BASEPROP_MAX(inst.properties_[PT_Level]))
			continue;
			
		inst.properties_[PT_Free] -= props[i].uVal_;
		inst.properties_[props[i].type_] += props[i].uVal_;
		switch(props[i].type_)
		{
		case PT_Stama:
			{
				CALC_PLAYER_PRO_TRANS_STAMA((inst), props[i].uVal_);
			}
			break;
		case PT_Strength:
			{
				CALC_PLAYER_PRO_TRANS_STRENGTH((inst), props[i].uVal_);
			}
			break;
		case PT_Power:
			{
				CALC_PLAYER_PRO_TRANS_POWER((inst), props[i].uVal_);
			}
			break;
		case PT_Speed:
			{
				CALC_PLAYER_PRO_TRANS_SPEED((inst), props[i].uVal_);
			}
			break;
		case PT_Magic:
			{
				CALC_PLAYER_PRO_TRANS_MAGIC((inst), props[i].uVal_);
			}
			break;
		default:
			break;
		}
		
	}
	for(size_t k=0;k<inst.properties_.size(); ++k){
		setProp((PropertyType)k,inst.properties_[k]); //����������
	}

	//chengjiu
	GameEvent::procGameEvent(GET_AddFleeProp,NULL,0,getHandleId());
}

void Player::autoaddprop(){
	JobType jt = (JobType)(U32)getProp(PT_Profession);
	U32 level = (U32) getProp(PT_ProfessionLevel);
	const Profession* profession = Profession::get(jt,level);
	if(NULL == profession)
		return;

	COM_PlayerInst inst; //����ԭ����
	inst.properties_ = properties_;
	for (size_t i = 0; i < profession->profprop_.size(); ++i)
	{
		if(profession->profprop_[i].first < PT_None || profession->profprop_[i].first >PT_Max)
			continue;
		if (profession->profprop_[i].second > inst.properties_[PT_Free] || profession->profprop_[i].second <= 0)
			continue;
		float propv = inst.properties_[profession->profprop_[i].first] + profession->profprop_[i].second;
		if(propv > CALC_PLAYER_BASEPROP_MAX(inst.properties_[PT_Level]))
			continue;

		inst.properties_[PT_Free] -= profession->profprop_[i].second;
		inst.properties_[profession->profprop_[i].first] += profession->profprop_[i].second;
		switch(profession->profprop_[i].first)
		{
		case PT_Stama:
			{
				CALC_PLAYER_PRO_TRANS_STAMA((inst), profession->profprop_[i].second);
			}
			break;
		case PT_Strength:
			{
				CALC_PLAYER_PRO_TRANS_STRENGTH((inst), profession->profprop_[i].second);
			}
			break;
		case PT_Power:
			{
				CALC_PLAYER_PRO_TRANS_POWER((inst), profession->profprop_[i].second);
			}
			break;
		case PT_Speed:
			{
				CALC_PLAYER_PRO_TRANS_SPEED((inst), profession->profprop_[i].second);
			}
			break;
		case PT_Magic:
			{
				CALC_PLAYER_PRO_TRANS_MAGIC((inst), profession->profprop_[i].second);
			}
			break;
		default:
			break;
		}

	}
	for(size_t k=0;k<inst.properties_.size(); ++k){
		setProp((PropertyType)k,inst.properties_[k]); //����������
	}

	//chengjiu
	GameEvent::procGameEvent(GET_AddFleeProp,NULL,0,getHandleId());
}

void
Player::changeProp(U32 guid,PropertyType propType, float uVal)
{
	if (guid != 0 && guid != playerId_)
	{
		Baby* pBaby = findBaby(guid);

		if (!pBaby)
			return;

		pBaby->changeProp(propType,uVal);

		return;
	}

	float current = getProp((PropertyType)propType);

	if(propType == PT_DoubleExp && current >= Global::get<float>(C_DoubleExpMax))
	{
		CALL_CLIENT(this,errorno(EN_DoubleExpTimeFull));
		return;
	}

	float curProp = current + uVal;

	if(propType == PT_DoubleExp && curProp > Global::get<float>(C_DoubleExpMax))
		curProp = Global::get<float>(C_DoubleExpMax);

	if(propType == PT_HpCurr && curProp > getProp(PT_HpMax))
		curProp = getProp(PT_HpMax);
	if(propType == PT_MpCurr && curProp > getProp(PT_MpMax))
		curProp = getProp(PT_MpMax);

	setProp((PropertyType)propType,curProp);
}

bool 
Player::setPlayerFront(bool isFront)
{
	setProp(PT_Front,(float)(S32)isFront);
	return true;
}

Scene* Player::myScene()
{
	return SceneManager::instance()->getScene(sceneId_);
}

void 
Player::transforScene(S32 sceneId){
	if(isBattle())
		return;
	
	if(sceneId == SceneTable::getGuildHomeScene()->sceneId_){
		if(!myGuild())
		{
			CALL_CLIENT(this,errorno(EN_NoGuild));
			return;
		}
		if(myTeam()){
			Team* p = myTeam();
			if (!p->isSameGuild())
			{
				errorMessageToC(EN_TeamMemberNoGuild);
				return;
			}
		}
	}else if(myTeam() && !isTeamLeader() && !isLeavingTeam_)
	{
		errorMessageToC(EN_NoTeamLeader);
	}

	scenePlayer_->transforScene(sceneId);
}

void Player::transforHome(){

	transforScene(SceneTable::getHomeScene()->sceneId_);
}

void 
Player::sceneLoaded(){
	isSceneLoaded_ = true;
	scenePlayer_->sceneLoaded();
}

void Player::openScene(S32 sceneId){
	for(size_t i=0; i<openScenes_.size(); ++i){
		if(openScenes_[i] == sceneId)
			return;
	}
	openScenes_.push_back(sceneId);
	scenePlayer_->openScene(sceneId);
	CALL_CLIENT(this, openScene(sceneId));
}

U32 Player::getCopyNum(S32 sceneid)
{
	U32 count = 0;
	for (size_t i = 0; i < copynums_.size(); ++i)
	{
		if(sceneid == copynums_[i])
			++count;
	}

	return count;
}

void Player::startCopy(S32 startsecenId,S32 sceneid)
{
	Team * pTeam = isTeamLeader();
	if(pTeam == NULL)
		return;
	U32 copylevel = CopyScene::getCopyLevelById(startsecenId);
	bool isnext = CopyScene::isNextCopySecne(copylevel,sceneid);
	if(!isnext)
	{
		for (size_t i = 0; i < pTeam->teamMembers_.size(); ++i)
		{
			U32 count = pTeam->teamMembers_[i]->getCopyNum(sceneid);
			if(count >= CopyScene::getCopyNumById(sceneid))
			{
				CALL_CLIENT(this,copynonum(pTeam->teamMembers_[i]->getNameC()));
				return;
			}
		}

		if(pTeam->hasLeaveMember())
			return;

		if(pTeam->teamMembers_.size() < 3)
			return;
	}

	Scene* s = SceneManager::instance()->openSceneCopy(sceneid);
	if(!s)
		return;
	
	transforScene(s->sceneId_);
	for (size_t i = 0; i < pTeam->teamMembers_.size(); ++i)
	{
		pTeam->teamMembers_[i]->copynums_.push_back(sceneid);
	}
	U32 questId = CopyScene::getCopyStartQuestById(sceneid);
	if(questId != 0)	
		pTeam->acceptTeamQuest(this,questId);
	pTeam->joinCopySceneOK(sceneid);
}

void Player::exitCopy()
{
	Team* pteam = myTeam();
	if(pteam == NULL)
		return;
	
	pteam->exitTeam(this);
	cleanCopyQuest();

	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(!s)
		return;
	s->kickPlayer(getGUID());
}

void
Player::chackLevelUp()
{
	U32 curlevel = getProp(PT_Level);
	ExpTable::Core const* pCore = ExpTable::getTemplateById(curlevel);

	if(!pCore)
		return;

	uint64 curExp = getProp(PT_Exp);
	bool isLevelUp = false;
	while (curExp >= pCore->playerExp_)
	{
		isLevelUp = true;
		levelup();
		curlevel = getProp(PT_Level);
		pCore = ExpTable::getTemplateById(curlevel);
		if(!pCore)
			return;	

		if(getProp(PT_Level) >= Global::get<float>(C_PlayerMaxLevel)){
			setProp(PT_Exp, pCore->playerExp_);
			break; //��ȼ�����
		}
	}
	if(isLevelUp)
	{
		COM_ContactInfo info;
		info.instId_ = getGUID();
		info.level_ = curlevel;
		info.name_ = getNameC();
		info.job_ = (JobType)(S32)properties_[PT_Profession];
		info.jobLevel_ = (S32)properties_[PT_ProfessionLevel];
		info.exp_ = properties_[PT_Exp];
		WorldServ::instance()->calcPlayerLevelRank(info);
		scenePlayer_->scenePlayerUpLevel(getProp(PT_Level));
	}
}

void
Player::levelup()
{
	clearAttachedProperties();
	enum
	{
		ADD_FREE = 4
	};

	U32 curlevel = getProp(PT_Level);
	setProp(PT_Level,++curlevel);	
	U32 curFree = getProp(PT_Free);

	setProp(PT_Free,curFree+=ADD_FREE);

	//��ɫ��������Ѫ
	//setProp(PT_HpCurr,getProp(PT_HpMax));
	//setProp(PT_MpCurr,getProp(PT_MpMax));
	///���� ���� 

	for(size_t i=0; i<employees_.size(); ++i)
	{
		employees_[i]->setProp(PT_Level,getProp(PT_Level));
		employees_[i]->levelup();
	}

	if(getProp(PT_GuildID) != 0)
	{
		Guild::memberLevelUp(this);
	}
	
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = curlevel;
	GameEvent::procGameEvent(GET_LevelUp,params,1,getHandleId());

	WorldServ::instance()->updateContactInfo(this);
	calcFightingForce();
}

void
Player::calcConvertExp(){
	if(getProp(PT_ConvertExp) >= Global::get<int>(C_ConvertExpMax))
		return;
	U32 exp = getProp(PT_ConvertExp);
	S32 tongji		= Global::get<int>(C_TongjiTimesMax) - getActivitionCount(ACT_Tongji);
	S32 richang		= 10 - getActivitionCount(ACT_Richang);
	S32 bairen		= 10 - tier_;
	S32 copynum		= CopyScene::getCopyAllNumByLevel(getProp(PT_Level)) - getActivitionCount(ACT_Copy);
	S32 petnum		= Global::get<int>(C_PetActivityNum) - getActivitionCount(ACT_Richang);
	S32 pvrnum	= Global::get<int>(C_JJCRivalNum) - getActivitionCount(ACT_JJC);
	if(tongji > 0)
		exp += tongji * Global::get<int>(C_TongJiExp);
	if(richang > 0)
		exp += richang * Global::get<int>(C_RiChangExp);
	if(bairen > 0)
		exp += bairen * Global::get<int>(C_BaiRenExp);
	if(copynum > 0)
		exp += copynum * Global::get<int>(C_FuBenExp);
	if(petnum > 0)
		exp += petnum * Global::get<int>(C_PetExp);
	if(pvrnum > 0)
		exp += pvrnum * Global::get<int>(C_WuDouExp);
	if(getActivitionCount(ACT_Exam) == 0)
		exp += Global::get<int>(C_ExamExp);
	if(getActivitionCount(ACT_Xuyuan) == 0)
		exp += Global::get<int>(C_PromiseExp);
	if(exp > Global::get<int>(C_ConvertExpMax))
		exp = Global::get<int>(C_ConvertExpMax);
	setProp(PT_ConvertExp,exp);
}

void
Player::learnSkill(S32 skId, U16 sklv)
{
	if(!canLearnSkill(skId,sklv))
		return;

	Entity::learnSkill(skId,sklv);

	if(skId == Global::get<int>(C_BabyLoyalSkillId))
	{
		for (size_t i = 0; i < babies_.size(); ++i)
		{
			if(babies_[i] == NULL)
				continue;
			babies_[i]->calcLoyal();
		}
	}

	COM_Skill inst;
	inst.skillID_ = skId;
	inst.skillExp_= 0;
	inst.skillLevel_ = sklv;

	CALL_CLIENT(this,learnSkillOk(inst));/// todo
	
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = skId;
	GameEvent::procGameEvent(GET_LearnSkill,params,1,getHandleId());
}

void
Player::forgetSkill(S32 skId)
{
	Skill* pSk = getSkillById(skId);
	if(pSk == NULL)
		return;

	SkillTable::Core const *pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(NULL == pCore)
		return;
	if(pCore->skType_ == SKT_CannotUse)
	{
		setProp(pCore->resistPropType_,getProp(pCore->resistPropType_)-pCore->resistNum_);
		}
		
	if(skId == Global::get<int>(C_BabyLoyalSkillId))
	{
		for (size_t i = 0; i < babies_.size(); ++i)
		{
			if(babies_[i] == NULL)
				continue;
			babies_[i]->calcLoyal();
		}
	}

	Entity::forgetSkill(skId); //����ֻ��ɾ��

	CALL_CLIENT(this,forgetSkillOk(skId));
	
}

bool
Player::canLearnSkill(S32 skId, U16 sklv)
{
	SkillTable::Core const *pCore = SkillTable::getSkillById(skId,sklv);
	if(NULL == pCore)
		return false;
	if(pCore->level_ != 1)
		return false;
	if(getSkillById(skId) != NULL)
		return false;

	JobType jt = (JobType)(U32)getProp(PT_Profession);
	U32 level = (U32) getProp(PT_ProfessionLevel);

	if(getProp(PT_Level) < pCore->learnLv_)
		return false;

	const Profession* profession = Profession::get(jt,level);
	if(NULL == profession)
		return false;
	
	if(!profession->canUseSkill(pCore->id_,pCore->level_))
		return false;
	if(pCore->learnpre_ != 0 &&!completeQuest(pCore->learnpre_))
		return false;
	if(getProp(PT_Money) < pCore->learnCoin_)
		return false;

	U32 skillNum = 0;
	for (size_t i = 0; i < skills_.size(); ++i)
	{
		SkillTable::Core const *pSkTmp = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		SRV_ASSERT(pSkTmp);
		
		if(pSkTmp->skType_ == SKT_Active || pSkTmp->skType_ == SKT_CannotUse || pSkTmp->skType_ == SKT_Passive)
			++skillNum;
	}

	if(skillNum >= Global::get<int>(C_LearnSkillMaxNum))
		return false;

	addMoney(-pCore->learnCoin_);
	
	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 10;
	track.money_ = -pCore->learnCoin_;
	LogHandler::instance()->playerTrack(track);

	return true;
}

bool
Player::canLevelUpSkill(S32 skId, U16 sklv)
{
	sklv +=1;
	SkillTable::Core const *pCore = SkillTable::getSkillById(skId,sklv);
	if(NULL == pCore)
		return false;
	return ((getProp(PT_Level) / 10 + 1)>= pCore->level_);
}

void
Player::babyLearnSkill(U32 instId, U32 oldSkId, U32 newSkId,U32 newSkLv)
{
	Baby* pBaby = findBaby(instId);

	if(pBaby == NULL)
		return;
	pBaby->babyLearnSkill(oldSkId,newSkId,newSkLv);
}

void
Player::addSkillExp(S32 skillID, U32 exp,ItemUseFlag flag )
{
	if( (flag == IUF_Battle) && openDoubleTimeFlag_ && getProp(PT_DoubleExp) > 0){
		exp *= 2;// ����˫������
	}

	Skill* pSk = getSkillById(skillID);
	if(pSk == NULL)
		return;

	Profession const * prof = Profession::get((JobType)(int)getProp(PT_Profession),(int)getProp(PT_ProfessionLevel));
	if(NULL == prof)
		return; //û��ְҵ

	if(pSk->skLevel_ >= prof->getSkillMaxLevel(skillID)){
		return; ///ְҵ�ļ��ܵȼ�����
	}

	SkillTable::Core const *pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);

	if(!pCore)
		return;

	if(pCore->exp_ == 0)
		return;

	if(!canLevelUpSkill(pSk->skId_,pSk->skLevel_))
		return;

	pSk->skExp_ += exp;

	CALL_CLIENT(this,addSkillExp(pSk->skId_, pSk->skExp_,flag));/// todo

	if(pSk->skExp_ >= pCore->exp_)
		skillLevelUp(pSk->skId_);

	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = exp;
	GameEvent::procGameEvent(GET_AddSkillExp,param,1,getHandleId());
}

void Player::skillLevelUp(S32 skillId){
	Skill* pSk = getSkillById(skillId);
	if(pSk == NULL)
		return;

	Profession const * prof = Profession::get((JobType)(int)getProp(PT_Profession),(int)getProp(PT_ProfessionLevel));
	if(NULL == prof)
		return; //û��ְҵ

	SkillTable::Core const *pCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);
	if(pCore == NULL) return;
	SkillTable::Core const *pNextCore = SkillTable::getSkillById(pSk->skId_,pSk->skLevel_);;
	
	for(size_t i=pSk->skLevel_ + 1; i<10 ; ++i){ //ֱ���ҵ���һ�� ���ܴ�һ��ֱ�ӵ� 10�� NEXT ����10��
		SkillTable::Core const *pTmpCore = SkillTable::getSkillById(pSk->skId_, i);
		if(pNextCore->exp_ > pSk->skExp_ || NULL == pTmpCore){
			break;
		}
		
		//��ǰ���ܵȼ������ְҵ���ܵȼ����ޣ���������
		if(pNextCore->level_ >= prof->getSkillMaxLevel(pSk->skId_)){
			pSk->skExp_ = 0;
			break;
		}
		if(!canLevelUpSkill(pSk->skId_,pNextCore->level_))
		{
			pSk->skExp_ = pCore->exp_ - 1 ;
			break;
		}
		pNextCore = pTmpCore;
	}
	if(pNextCore == NULL) return;

	
	if(pCore->skType_ == SKT_CannotUse)
	{
		float propv = getProp(pCore->resistPropType_)-pCore->resistNum_;
		setProp(pCore->resistPropType_,propv);
	}
	if(pCore->skType_ == SKT_CannotUse)
	{
		float propv = getProp(pNextCore->resistPropType_)+pNextCore->resistNum_;
		setProp(pNextCore->resistPropType_,propv);
	}
	
	pSk->skLevel_ = pNextCore->level_;
	pSk->reset();

	if(skillId == Global::get<int>(C_BabyLoyalSkillId))
	{
		for (size_t i = 0; i < babies_.size(); ++i)
		{
			if(babies_[i] == NULL)
				continue;
			babies_[i]->calcLoyal();
		}
	}

	

	COM_Skill inst;
	inst.skillID_ = pSk->skId_;
	inst.skillExp_ = pSk->skExp_;
	inst.skillLevel_ = pSk->skLevel_;
	
	CALL_CLIENT(this,skillLevelUp(getGUID(),inst));

	GEParam param[2];
	param[0].type_ = GEP_INT;
	param[0].value_.i = pSk->skId_;
	param[1].type_ = GEP_INT;
	param[1].value_.i = pSk->skLevel_;
	GameEvent::procGameEvent(GET_SkillLevelUp,param,2,getHandleId());
	
}

void
Player::findContactInfoById(U32 instId)
{
	COM_ContactInfo* p = WorldServ::instance()->findContactInfo(instId);

	if(NULL == p)
	{
		CALL_CLIENT(this, findFriendFail());
		return ;
	}

	std::vector<COM_ContactInfo> ret;
	ret.push_back(*p);

	CALL_CLIENT(this, requestContactInfoOk(*p));
}

void
Player::findContactInfoByName(const char* name)
{
	COM_ContactInfo* p = WorldServ::instance()->findContactInfo(name);

	if(NULL == p)
	{
		CALL_CLIENT(this, findFriendFail());
		return ;
	}

	std::vector<COM_ContactInfo> ret;
	ret.push_back(*p);
	//addFriend(p->instId_);
	CALL_CLIENT(this, requestContactInfoOk(*p));
}


//--------Friend---------------

void
Player::addFriend(U32 instId)
{
	if(instId == getGUID())
		return;

	if(getProp(PT_Level) <= 5){
		return;
	}

	COM_ContactInfo* p = WorldServ::instance()->findContactInfo(instId);

	if(NULL == p)
	{
		//errorMessageToC(EN_PlayerNoOnline);
		return;
	}

	if(p->level_ <= 5)
	{
		CALL_CLIENT(this,errorno(EN_FirendNotOpen));
		return;
	}

	if(friend_.size() >= Global::get<int>(C_FriendMax))
		return;

	if(findFriendById(instId)  != NULL) 
	{
		CALL_CLIENT(this,errorno(EN_HasFriend));
		return;
	}
	if(findBlacklistById(instId)  != NULL)
	{
		CALL_CLIENT(this,errorno(EN_BlackCannotFriend));
		return;
	}
	
	Player* player = getPlayerByInstId(instId);
	if(player)
		p->isLine_ = true;
	else
		p->isLine_ = false;

	friend_.push_back(*p);
	CALL_CLIENT(this, addFriendOK(*p));

	GameEvent::procGameEvent(GET_AddFriend,NULL,0,getHandleId());
}

void
Player::delFriend(U32 instId)
{
	for (size_t i=0; i<friend_.size(); ++i)
	{
		if(friend_[i].instId_ == instId)
		{
			friend_.erase(friend_.begin() + i);
			break;
		}
	}

	CALL_CLIENT(this, delFriendOK(instId));
}

COM_ContactInfo*
Player::findFriendById(U32 instId)
{
	for (size_t i = 0; i < friend_.size(); ++i)
	{
		if (friend_[i].instId_ == instId)
		{
			return &friend_[i];
		}
	}

	return NULL;
}

void
Player::addBlacklist(U32 instId)
{
	COM_ContactInfo* p = WorldServ::instance()->findContactInfo(instId);

	if(NULL == p)
		return ;
	
	if(findBlacklistById(instId))
		return;

	if(p->level_ <= 5)
	{
		CALL_CLIENT(this,errorno(EN_FirendNotOpen));
		return;
	}

	if(blacklist_.size() >= Global::get<int>(C_FriendMax))
		return;

	if (findFriendById(instId))
	{
		delFriend(instId);
	}

	blacklist_.push_back(*p);

	CALL_CLIENT(this, addBlacklistOK(*p));
}

void
Player::delBlacklist(U32 instId)
{
	for (size_t i=0; i<blacklist_.size(); ++i)
	{
		if(blacklist_[i].instId_ == instId)
		{
			blacklist_.erase(blacklist_.begin() + i);
			break;
		}
	}

	CALL_CLIENT(this, delBlacklistOK(instId));
}

void 
Player::requestFriendList()
{
	updataFriend();
	CALL_CLIENT(this, requestFriendListOK(friend_));
}

void
Player::updataFriend()
{
	for (size_t i = 0; i < friend_.size(); ++i)
	{
		if(friend_[i].instId_ == 0)
			continue;
		Player* p = Player::getPlayerByInstId(friend_[i].instId_);
		if(p == NULL)
		{
			COM_ContactInfo* f = WorldServ::instance()->findContactInfo(friend_[i].instId_);
			if(f == NULL)
				continue;
			friend_[i] = *f;
			friend_[i].isLine_ = false; 
		}
		else
		{
			friend_[i].level_		= p->getProp(PT_Level);
			friend_[i].ff_			= p->getProp(PT_FightingForce);
			friend_[i].assetId_	= p->getProp(PT_AssetId);
			friend_[i].job_		= (JobType)(int)p->getProp(PT_Profession);
			friend_[i].jobLevel_= p->getProp(PT_ProfessionLevel);
			friend_[i].vip_		=(VipLevel)(int)p->getProp(PT_VipLevel);
			friend_[i].isLine_      = true;
		}
	}
}

void
Player::referrFriend()
{
	enum
	{
		REFERRNUM = 6
	};

	std::vector<U32> tmpPlays;
	for(NamePlayerMap::iterator itr = nameStore_.begin(); itr!=nameStore_.end();++itr){
		Player* p = itr->second;
		if(p == NULL)
		{
			continue;
		}
		tmpPlays.push_back(p->playerId_);
	}
	
	std::vector< COM_ContactInfo* > referrList;
	U32 index = 0;
	U32 tmpNum = 0;

	if(tmpPlays.size() < 6)
	{
		for (size_t i = 0; i < tmpPlays.size(); ++i)
		{
			COM_ContactInfo* p = WorldServ::instance()->findContactInfo(tmpPlays[i]);
			if(p == NULL || p->instId_ == playerId_)
				continue;
			if(findFriendById(p->instId_) != NULL)
				continue;
			if(findBlacklistById(p->instId_) != NULL)
				continue;
			Player* player = getPlayerByInstId(p->instId_);
			if(player != NULL && player->getOpenSubSystemFlag(OSSF_Friend))
			{
				referrList.push_back(p);
			}
		}
	}
	else
	{
		for (size_t i = 0; i < REFERRNUM; ++i)
		{
			index = UtlMath::randN(tmpPlays.size());
			if(tmpPlays[index] == playerId_)
				continue;
			if(findFriendById(tmpPlays[index]) != NULL)
				continue;
			if(findBlacklistById(tmpPlays[index]) != NULL)
				continue;
			COM_ContactInfo* ptmp = WorldServ::instance()->findContactInfo(tmpPlays[index]);
			if(ptmp == NULL)
				continue;
			std::vector<COM_ContactInfo*>::iterator itr = std::find(referrList.begin(),referrList.end(),ptmp);
			if(itr!=referrList.end())
				continue;
			Player* p = getPlayerByInstId(ptmp->instId_);
			if(p != NULL && p->getOpenSubSystemFlag(OSSF_Friend))
			{
				referrList.push_back(ptmp);
			}
		}
	}

	std::vector<COM_ContactInfo> conta;
	for (size_t i = 0; i < referrList.size(); ++i)
	{
		if(referrList[i] == NULL)
			continue;
		conta.push_back(*referrList[i]);
	}

	CALL_CLIENT(this,referrFriendOK(conta));
}

COM_ContactInfo* 
Player::findBlacklistById(U32 instId)
{
	for (size_t i = 0; i < blacklist_.size(); ++i)
	{
		if (blacklist_[i].instId_ == instId)
			return &blacklist_[i];
	}
	return NULL;
}

ErrorNo Player::checkGatherStates(S32 gatherId,GatherStateType tp)
{
	GatherTable::GatherCore const * pG =  GatherTable::getGatherById(gatherId);
	if(NULL == pG)
		return EN_GatherLevelLess;
	/*U16 index = 0;
	if(tp == GST_Advanced)
	{
		for (size_t i = MT_None; i < MT_Max; ++i)
		{
			if(i == pG->type_)
				continue;
			if(isGatherOpenAdvanced((MineType)i))
				++index;
		}
	}*/
	//���������߼� 
	//if(index > 1) ���Զ�ѧ�ɸ߼���
	//	return EN_OpenGatherlose;
	//�Ѿ���tpȨ��
	for (size_t i = 0; i < gathers_.size(); ++i)
	{
		if(gathers_[i].gatherId_ == gatherId && tp <= gathers_[i].flag_)
			return EN_OpenGatherRepetition;
	}
	return EN_None;
}

bool Player::isGatherOpenAdvanced(MineType t)
{
	for (size_t i = 0; i < gathers_.size(); ++i)
	{
		GatherTable::GatherCore const * pG =  GatherTable::getGatherById(gathers_[i].gatherId_);
		if(NULL == pG)
			continue;
		if(pG->type_ == t && gathers_[i].flag_ == GST_Advanced)
			return true;
	}

	return false;
}

void
Player::openGather(S32 gatherId,GatherStateType tp)
{
	COM_Gather gather;
	gather.gatherId_ = gatherId;
	gather.flag_	 = tp;
	gather.num_		 = 0;
	gathers_.push_back(gather);
	CALL_CLIENT(this,openGatherOK(gather));
}

GatherStateType
Player::getGatherState(S32 gatherId,GatherStateType tp)
{
	for (size_t i = 0; i < gathers_.size(); ++i)
	{
		if(gathers_[i].gatherId_ == gatherId && tp == gathers_[i].flag_)
			return gathers_[i].flag_;
	}
	return GST_Vulgar;
}

COM_Gather*
Player::getGatherData(S32 gatherId)
{
	for (size_t i = 0; i < gathers_.size(); ++i)
	{
		if(gathers_[i].gatherId_ == gatherId)
			return &gathers_[i];
	}
	return NULL;
}

void Player::mining(S32 gatherId,S32 times){
	GatherTable::GatherCore const * pG =  GatherTable::getGatherById(gatherId);
	if(NULL == pG)
		return;
	if(times <= 0)
		return;

	if((int)getProp(PT_Level) < pG->level_){
		CALL_CLIENT(this,errorno(EN_GatherLevelLess));
		return; //�ȼ�����
	}

	if(gaterMaxNum_ + times > Global::get<int>(C_AllGatherNum))
		return;

	COM_Gather* pdate = getGatherData(gatherId);
	if(pdate == NULL && pG->superdrop_ != 0)
		return;
	else if(pdate == NULL && pG->superdrop_ == 0)
	{
		COM_Gather gather;
		gather.gatherId_ = gatherId;
		gather.flag_	 = GST_Vulgar;
		gather.num_		 = 0;
		gathers_.push_back(gather);
	}

	COM_Gather* pnewdate = getGatherData(gatherId);
	if(pnewdate == NULL)
		return;

	U32 needmoney = 0;

	for (size_t i = 1; i <= times; ++i)
	{
		needmoney += (pnewdate->num_+i -1)/5*pG->addmoney_+pG->money_;
	}

	if((int)getProp(PT_Money) < needmoney){
		CALL_CLIENT(this,errorno(EN_MoneyLess));
		return;
	}

	U32 dropId = 0;
	if(pG->superdrop_ != 0)
	{
		GatherStateType type = getGatherState(gatherId,GST_Advanced);
		if(type == GST_None)
			return;
		if(type == GST_Vulgar)
			dropId = pG->dropId_;
		else
			dropId = pG->superdrop_;
	}
	else
		dropId = pG->dropId_;

	std::vector<COM_DropItem> items;
	for(int32 i=1; i<=times; ++i)
	{
		DropTable::Drop const * pD = DropTable::getDropById(dropId);
		if(NULL == pD){
			return;
		}

		if(getBagEmptySlot() < pD->items_.size()){
			CALL_CLIENT(this,errorno(EN_BagFull));
			return;
		}

		for(size_t j=0; j<pD->items_.size(); ++j){
			COM_DropItem item;
			item.itemId_ = pD->items_[j].itemId_;
			item.itemNum_ = pD->items_[j].itemNum_;
			items.push_back(item);
			addBagItemByItemId(item.itemId_,item.itemNum_,false,12);
		}
		++pnewdate->num_;
		++gaterMaxNum_;
		S32 need = ((pnewdate->num_ -1)/5*pG->addmoney_+pG->money_);
		addMoney(-need);
		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 12;
		track.money_ = -need;
		LogHandler::instance()->playerTrack(track);
	}
	//--mineTimes_[pG->type_];
	CALL_CLIENT(this,miningOk(items,*pnewdate,gaterMaxNum_));

	GEParam params[2];
	params[0].type_ = GEP_INT;
	params[0].value_.i = pG->type_;
	params[1].type_ = GEP_INT;
	params[1].value_.i = times;
	GameEvent::procGameEvent(GET_Gather,params,2,getHandleId());
}

void Player::initGather()
{
	CALL_CLIENT(this,initGather(gaterMaxNum_,gathers_));
}

void Player::resetgathers()
{
	for (size_t i = 0; i < gathers_.size(); ++i){
		gathers_[i].num_ = 0;
	}
	gaterMaxNum_ = 0;
	initGather();
}

void
Player::initCompound()
{
	CALL_CLIENT(this,initcompound(compounds_));
}

void
Player::openCompound(U32 itemId)
{
	MakeTable::MakeCore const* core = MakeTable::getMakeById(itemId);
	if(core == NULL)
		return;
	compounds_.push_back(itemId);
	CALL_CLIENT(this, openCompound(itemId));
}

bool
Player::checkCompound(U32 itemId)
{
	for (size_t i = 0; i < compounds_.size(); ++i)
	{
		if(compounds_[i] == itemId)
			return true;
	}
	return false;
}

bool
Player::makeItem(U32 itemId,U32 gemId)
{
	if(!getBagEmptySlot()){
		CALL_CLIENT(this,errorno(EN_BagFull));
		return false;
	}
	MakeTable::MakeCore const* core = MakeTable::getMakeById(itemId);
	if(core == NULL)
		return false;
	if(!checkCompound(itemId) && core->level_ >= 40)
		return false;

	if(getProp(PT_Money) < core->needMoney_)
	{
		CALL_CLIENT(this,errorno(EN_MoneyLess));
		return false;
	}

	if(gemId > 0)
	{
		std::vector<COM_Item*> gems;
		getBagItemByItemId(gemId,gems);
		if(gems.empty())
		{
			return true;
		}
	}

	for(size_t i =0;i<core->costItems_.size();i++)
	{
		std::vector<COM_Item*> items;
		getBagItemByItemId(core->costItems_[i],items);
		if(items.empty())
		{
			return true;
		}
		size_t num = 0;
		for(size_t j=0;j<items.size();j++)
		{
			num+= items[j]->stack_;
		}
		if(num < core->costItemNum_[i] )
		{
			return true;
		}
	}

	for(size_t i =0;i<core->costItems_.size();i++)
	{
		delBagItemByItemId(core->costItems_[i],core->costItemNum_[i]);
	}
	if(gemId >0)
	{
		delBagItemByItemId(gemId,1);
	}

	bool istaleItem =  false;
	std::vector<COM_Item*> items;
	U32 randk = UtlMath::randN(10000);
	if(randk < core->chance_)
	{
		if(!genItemInst(core->newItem_,1,items))
			return true;
		istaleItem = true;
	}
	else
	{
		if(!genItemInst(itemId,1,items))
			return true;
	}
	for (U32 i=0; i<items.size(); ++i)
	{
		S32 emptySlot = getFirstEmptySlot();
		if(emptySlot  < 0)
		{
			CALL_CLIENT(this,errorno(EN_BagFull));
			DEL_MEM(items[i]);
			return true;
		}
		if(gemId > 0)
		{
			ItemTable::ItemData const* gemCore = ItemTable::getItemById(gemId);
			if(gemCore  == NULL || gemCore->subType_ != IST_Gem)
			{
				return true;
			}
			ItemTable::ItemData const* itemCore = ItemTable::getItemById(itemId);
			if( itemCore->isWeapon() )
			{
				itemAddProp(items[i],gemCore->WeaponD_,gemCore->WeaponP_);
			}
			else if( itemCore->isArmor() )
			{
				itemAddProp(items[i],gemCore->ArmorD_,gemCore->ArmorP_);
			}
		}
		ItemTable::ItemData const * data = ItemTable::getItemById(items[i]->itemId_);
		if(data){
			items[i]->isBind_ = data->bindType_ == BIT_Bag;
		}
		items[i]->slot_ = emptySlot ;
		bagItmes_[emptySlot ] = items[i];
		CALL_CLIENT(this,addBagItem(*items[i]));
	}

	postQuestItemEvent((istaleItem?core->newItem_:itemId),1);
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("addItem  %d  %d\n"),itemId,1));

	addMoney(-core->needMoney_);
	CALL_CLIENT(this, compoundItemOk(*items[0]));

	GEParam params[2];
	params[0].type_ = GEP_INT;
	params[0].value_.i = itemId;
	params[1].type_ = GEP_INT;
	params[1].value_.i = istaleItem;
	GameEvent::procGameEvent(GET_MakeEquip,params,2,getHandleId());

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 12;
	track.money_ = -core->needMoney_;
	LogHandler::instance()->playerTrack(track);

	return true;	
}

void 
Player::itemAddProp(COM_Item *item,std::vector<std::pair< PropertyType , float > > const &d, std::vector<std::pair< PropertyType , float > > const& p)
{
	if(item == NULL)
		return;
	//calc d prop
	for(size_t i=0; i<d.size(); ++i){
		if(PT_Durability ==d[i].first ){
			item->durability_ += d[i].second;
			item->durabilityMax_ +=  d[i].second;
			continue;
		}
		bool has = false;
		for (size_t j=0; j<item->propArr.size(); ++j){
			if(item->propArr[j].type_ == d[i].first){
				item->propArr[j].value_ += d[i].second;
				has = true;
			}
		}
		if(!has){
			COM_PropValue  prop ;
			prop.type_ = d[i].first;
			prop.value_ =d[i].second;
			item->propArr.push_back(prop);
		}
	}

	//calc d prop
	for(size_t i=0; i<p.size(); ++i){
		if(PT_Durability ==p[i].first ){
			item->durability_ += p[i].second *item->durabilityMax_;
			item->durabilityMax_ +=  p[i].second *item->durabilityMax_;
			continue;
		}
		for (size_t j=0; j<item->propArr.size(); ++j){
			if(item->propArr[j].type_ == p[i].first){
				item->propArr[j].value_ += p[i].second * item->propArr[j].value_;
			}
		}
	
	}
	
}
//-----------Lottery----------

void
Player::lotteryGo(U32 itemId)
{
	U32 rewardLv = LotteryTable::randLottery(itemId);

	LotteryTable::LotteryCore const* pCore = LotteryTable::getLottery(itemId,rewardLv);

	if(pCore == NULL)
		return;

	const DropTable::Drop* drop = DropTable::getDropById(pCore->dropId_);
	if(NULL == drop)
		return;
	std::vector<COM_DropItem> dropItem;
	if(!drop->items_.empty()){
		if(getBagEmptySlot() < drop->items_.size()){
			CALL_CLIENT(this,errorno(EN_BagFull));
			return;
		}
		
		for(size_t i=0; i<drop->items_.size(); ++i){
			addBagItemByItemId(drop->items_[i].itemId_,drop->items_[i].itemNum_,false,16);
			COM_DropItem item;
			item.itemId_ = drop->items_[i].itemId_;
			item.itemNum_= drop->items_[i].itemNum_;
			dropItem.push_back(item);
		}
	}

	CALL_CLIENT(this, lotteryOk(pCore->id_,dropItem));
}

bool Player::giveDrop(S32 dropId,bool usebase){
	const DropTable::Drop* drop = usebase ?DropTable::getDropBaseById(dropId): DropTable::getDropById(dropId);
	if(NULL == drop)
		return false;
	if(!drop->items_.empty()){
		if(getBagEmptySlot() < drop->items_.size()){
			CALL_CLIENT(this,errorno(EN_BagFull));
			return false;
		}
		for(size_t i=0; i<drop->items_.size(); ++i){
			addBagItemByItemId(drop->items_[i].itemId_,drop->items_[i].itemNum_,false,9);
		}
	}
	// �ճ������ý�������һ��ϵ��
	//dropId 220000-230000 �ճ�����dropId drop->exp_ ���⴦��
	//����ȼ�=����������(PLAYERLEVEL/10)��*10-5
	// ϵ��=(PLAYERLEVEL+1����Ӧ�ľ���-LV����Ӧ�ľ��飩/������ȼ�+1����Ӧ�ĵȼ�����-����ȼ�����Ӧ�ľ��飩
	//EXP = drop->exp_*ϵ��
	if(drop->exp_){
		if(dropId >= 220000 && dropId <= 230000)
		{
			float playerLevel = getProp(PT_Level);
			float rtt = playerLevel/10.F;
			U32 questlevel = UtlMath::round(rtt)*10 - 5;
			ExpTable::Core const* corp1 = ExpTable::getTemplateById(playerLevel-1);
			ExpTable::Core const* corp2 = ExpTable::getTemplateById(playerLevel);
			ExpTable::Core const* corp3 = ExpTable::getTemplateById(questlevel-1);
			ExpTable::Core const* corp4 = ExpTable::getTemplateById(questlevel);
			if(!corp1 || !corp2 || !corp3 || !corp4)
				return false;
			float coef = (corp2->playerExp_ - corp1->playerExp_)/(corp4->playerExp_ - corp3->playerExp_);
			U32 exp = drop->exp_* coef;
			addExp(exp,false);
		}
		else
			addExp(drop->exp_,false);
	}
	if (getBattleBaby())
	{
		getBattleBaby()->addExp(drop->exp_,false);
	}
	if(drop->money_)
		addMoney(drop->money_);

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 13;
	track.money_ = drop->money_;
	track.exp_ = drop->exp_;
	LogHandler::instance()->playerTrack(track);

	return true;
}

void Player::enterBattle(S32 battleId)
{
	//ACE_DEBUG((LM_INFO, "void Player::enterBattle(%d)\n",battleId));
	const BattleData* pbd = BattleData::getBattleDataById(battleId);
	if(NULL == pbd)
		return;
	if(pbd->battleType_ == BT_PVH)
	{	
		if(myTeam() && !myTeam()->canHundredBattle(pbd->battleId_))
			return;
		else
		{
			ErrorNo en = GameRuler::CanHundredBattle(this,pbd->battleId_);
			if(en!=EN_None && en!=EN_Max){
				errorMessageToC(en);
				return;
			}
		}
	}
	else if(pbd->battleType_ == BT_PET){
		if(!myTeam())
			return;
		if(!DayliActivity::status_[ACT_Pet] || !myTeam()->canPetActivity(pbd->battleId_)){
			return;
		}
	}		

	Battle* battle = Battle::getOneFree();
	if(NULL == battle)
		return ;
	if(myTeam()){
		if(isTeamLeader())
			battle->create(myTeam(),battleId);
		else 
			if(isLeavingTeam_)
				battle->create(this,battleId);
	}
	else 
		battle->create(this,battleId);
	
}

void Player::enterBattle2(S32 battleId)
{
	//ACE_DEBUG((LM_INFO, "void Player::enterBattle(%d)\n",battleId));
	const BattleData* pbd = BattleData::getBattleDataById(battleId);
	if(NULL == pbd)
		return;

	Battle* battle = Battle::getOneFree();
	if(NULL == battle)
		return ;
	if(myTeam()){
		if(isTeamLeader())
			battle->create(myTeam(),battleId);
		else 
			if(isLeavingTeam_)
				battle->create(this,battleId);
	}
	else 
		battle->create(this,battleId);

}


void Player::zoneJoinBattle(S32 zoneId){
	bool isrobot = RobotActionTable::isRobot(account_->username_);
	if(isrobot)
		return ;
	if((JobType)(int)getProp(PT_Profession) == JT_Newbie){
		return ;
	}
	
	if(isTeamMember() && !isTeamLeader() && !isLeavingTeam_)
		return;

	SceneData* sd = SceneTable::getSceneById(GET_SCENE_ORIGINAL_ID(sceneId_));
	if(!sd)
		return;
	
	ZoneInfo* zi = sd->getZone(zoneId);
	if (!zi)
		return;
	
	Battle* pb = Battle::getOneFree();
	if(!pb)
		return;
	if(myTeam()){
		if(isTeamLeader())
			pb->create(myTeam(),zi);
		else if(isLeavingTeam_)
			pb->create(this,zi);
		else 
			return;
	}
	else
		pb->create(this,zi);
}

void
Player::startChallenge(std::string name)
{
	EndlessStair::instance()->creatArena(this,name);
}

void
Player::findRival()
{
	EndlessStair::instance()->checkRival(this);
}

void
Player::requestJJCRank()
{
	EndlessStair::instance()->requestRank(this);
}

void
Player::requestJJCData()
{
	EndlessStair::instance()->requestJJCData(this);
}

void
Player::resetRivalTime()
{
	rivalTimes_ = Global::get<float>(C_JJCRivalTimeCD);
}

void
Player::calcRivalNum()
{
	--rivalNum_;
}

void
Player::checkMsg(const std::string &name)
{
	//EndlessStair::instance()->checkMsg(this,name);

	COM_ContactInfo* pCont = WorldServ::instance()->findContactInfo(name);

	if(pCont == NULL)
	{
		ACE_DEBUG((LM_ERROR,"Can not find in contact info %s\n",name.c_str()));
		return;
	}
	DBHandler::instance()->queryPlayerById(getNameC(),pCont->instId_,true);
}

void
Player::battleMsg(COM_JJCBattleMsg& msg)
{
	if(battleMsg_.size() > JJCBATTLEMSGNUM)
	{
		battleMsg_.pop_back();
		battleMsg_.insert(battleMsg_.begin(),msg);
	}
	else
	{
		battleMsg_.insert(battleMsg_.begin(),msg);
	}
}

void
Player::requestAllBttleMsg()
{
	CALL_CLIENT(this, requestMyAllbattleMsgOK(battleMsg_));
}

void
Player::promoteAward(BattleType type)
{
	if(type == BT_PVR)
	{
		U32 winerRank = EndlessStair::findRankByName(playerName_);

		PvRrewardTable::PvrrewardData const* pCore = PvRrewardTable::getPvrRunkById(winerRank);
		if(pCore == NULL)
			return;
		giveDrop(pCore->dropitem_);
	}
	else if (type == BT_PVP)
	{
		PvpRunkTable::PvpRunkDate const* pCore = PvpRunkTable::getPvpRunkById(pvpInfo_.section_);
		if(pCore == NULL)
			return;
		giveDrop(pCore->dropitem_);
		WorldServ::instance()->updateContactInfo(this);
	}
}

void Player::addMoney(S32 num)
{
	//ACE_DEBUG((LM_INFO,"Player add money %s(%d) %d\n",playerName_.c_str(),playerId_,num));
	S32 i = getProp(PT_Money) + num;
	if(i < 0)
		i = 0;
	
	if (num > 0)
	{
		if((int)getProp(PT_VipLevel) == VL_1){
			i += 0.1 * num;
		}
		else if((int)getProp(PT_VipLevel) == VL_2){
			i += 0.2 * num;
		}
	}

	if(i >= Global::get<int>(C_MoneyMax)){
		i = Global::get<int>(C_MoneyMax);
	}

	setProp(PT_Money,i);

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = num;
	GameEvent::procGameEvent(GET_ChangeMoney,params,1,getHandleId());
}

void Player::addDiamond(S32 num)
{
	//ACE_DEBUG((LM_INFO,"Player add diamond %s(%d) %d\n",playerName_.c_str(),playerId_,num));
	S32 i = getProp(PT_Diamond) + num;
	if(i < 0)
		i = 0;
	
	if(i >= Global::get<int>(C_DiamondMax)){
		i = Global::get<int>(C_DiamondMax);
	}

	setProp(PT_Diamond,i);
	
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = num;
	GameEvent::procGameEvent(GET_ChangeDiamond,params,1,getHandleId());
}

void Player::addMagicCurrency(S32 num)
{
	//ACE_DEBUG((LM_INFO,"Player pay MagicCurrency %s(%d) %d\n",playerName_.c_str(),playerId_,num));
	S32 i = getProp(PT_MagicCurrency) + num;
	if(i < 0)
		i = 0;

	if(i >= Global::get<int>(C_DiamondMax)){
		i = Global::get<int>(C_DiamondMax);
	}

	setProp(PT_MagicCurrency,i);
	
	if(num > 0){
		selfRecharge_.recharge_ += num;
		sysRecharge_.recharge_ += num;
		
		updateSelfRecharge();
		updateSysRecharge();
		updateSelfOnceRecharge(num);
		updateSysOnceRecharge(num);
	}
	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = num;
	GameEvent::procGameEvent(GET_ChangeMagicCurrency,params,1,getHandleId());
}

void Player::getFirstRechargeItem()
{
	if(!firstRechargeDiamond_ || isFirstRechargeGift_)
		return;	
	isFirstRechargeGift_ = true;

	for (size_t i=0; i<Shop::records_.size(); ++i)
	{
		if(Shop::records_[i]->type_ == SIT_FirstRecharge)
		{
			addBagItemByItemId(Shop::records_[i]->itemId_,Shop::records_[i]->itemNum_,false,16);
		}
	}
	CALL_CLIENT(this,firstRechargeGiftOK(isFirstRechargeGift_));
}  

void Player::buyShopItem(S32 id, S32 num)
{
	//ACE_DEBUG((LM_INFO,"Buy shop item %s(%d) %d %d\n",playerName_.c_str(),playerId_,id,num));
	if(num <= 0)
		return;
	const Shop::Record* record= Shop::getRecordById(id);
	
	if(NULL == record) 
		return ;

	if(calcEmptyItemNum(record->itemId_) < record->itemNum_ * num){
		errorMessageToC(EN_BagFull);
		return;
	}

	S32 pay = record->pay_ * num;
	if(pay <= 0)
		return; //Խ�紦��

	if(record->type_ != SIT_Shop && record->type_ != SIT_EmployeeShop&& record->type_ != SIT_Equip)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItem shoptype error ====%d\n" ,record->type_));
		return;
	}
	if(record->paytype_ == SPT_Diamond && getProp(PT_Diamond) < pay)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItem Diamond insufficientn!!!!\n"));
		if(getProp(PT_MagicCurrency) < pay){
			return;
		}
		addMagicCurrency(-pay);
		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 1;
		track.magic_ = -pay;
		LogHandler::instance()->playerTrack(track);
	
		S32 itemnum = record->itemNum_ * num;
		addBagItemByItemId(record->itemId_,itemnum,false,1);
		CALL_CLIENT(this,buyShopItemOk(id));
		return;
	}
	if(record->paytype_ == SPT_Gold && getProp(PT_Money) < pay)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItem SPT_Gold insufficientn!!!!\n"));
		return;
	}
	if(record->paytype_ == SPT_MagicCurrency && getProp(PT_MagicCurrency) < pay)
	{ 
		ACE_DEBUG((LM_ERROR,"buyShopItem SPT_MagicCurrency insufficientn!!!!\n"));
		return;
	}
	if(record->paytype_ == SPT_Diamond){
		addDiamond(-pay);
		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 1;
		track.diamond_ = -pay;
		LogHandler::instance()->playerTrack(track);
	}
	else if (record->paytype_ == SPT_Gold)
	{
		addMoney(-pay);
		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 1;
		track.money_ = -pay;
		LogHandler::instance()->playerTrack(track);
	}
	else if (record->paytype_ == SPT_MagicCurrency)
	{
		addMagicCurrency(-pay);
		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 1;
		track.magic_ = -pay;
		LogHandler::instance()->playerTrack(track);
	}
	S32 itemnum = record->itemNum_ * num;
	addBagItemByItemId(record->itemId_,itemnum,false,1);
	CALL_CLIENT(this,buyShopItemOk(id));
}

bool Player::orderFromSDK(S32 shopId,S32 num,std::string const& orderid,std::string const& paytime,float payment){
	ACE_DEBUG((LM_INFO,"SDK ODER |===> SHOPID(%d) NUM(%d), ORDERID(%s), PAYTIME(%s) PAYMENT(%f) PLAYERID(%d) <===|\n",shopId,num,orderid.c_str(),paytime.c_str(),payment,playerId_));
	if(num <= 0)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB buy item num error num====%d\n" ,num));
		return false; 
	}
	const Shop::Record* record= Shop::getRecordById(shopId);
	if(NULL == record)
	{
		ACE_DEBUG((LM_ERROR,"Can not find shopId====%d\n" ,shopId));
		return false;
	}
	if(record->paytype_ != SPT_RMB)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB record->paytype_ != SPT_RMB shopId====%d\n" ,shopId));
		return false;
	}
	if(record->pay_ < (int32)payment){
		ACE_DEBUG((LM_ERROR,"SDK pushed payment error !!!\n"));
		return false;
	}
	if(!justOrder(shopId,num,orderid,paytime,payment)){
		ACE_DEBUG((LM_ERROR,"SDK JUST ORDER FAIL!!! \n"));
	}
	
	WorldServ::instance()->pushOrderLog(account_,playerId_,getProp(PT_Level),orderid,record->pay_,paytime);
	return true;
}

bool Player::orderFromGMT(S32 shopId,S32 num,std::string const& orderid,std::string const& paytime,float payment){
	ACE_DEBUG((LM_INFO,"GMT ODER |===> SHOPID(%d) NUM(%d), ORDERID(%s), PAYTIME(%s) PAYMENT(%f) PLAYERID(%d)<===|\n ",shopId,num,orderid.c_str(),paytime.c_str(),payment,playerId_));
	if(num <= 0)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB buy item num error num====%d\n" ,num));
		return false;
	}
	const Shop::Record* record= Shop::getRecordById(shopId);
	if(NULL == record)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB can not find shopId====%d\n" ,shopId));
		return false;
	}
	if(record->paytype_ != SPT_RMB)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB record->paytype_ != SPT_RMB shopId====%d\n" ,shopId));
		return false;
	}

	if(!justOrder(shopId,num,orderid,paytime,payment)){
		ACE_DEBUG((LM_ERROR,"GMT JUST ORDER FAIL!!! \n"));
	}

	WorldServ::instance()->pushOrderLog(account_,playerId_,getProp(PT_Level),orderid,record->pay_,paytime);
	return true;
}

bool
Player::justOrder(S32 shopId,S32 num,std::string const& orderid,std::string const& paytime,float payment)
{
	const Shop::Record* record= Shop::getRecordById(shopId);
	if(NULL == record)
	{
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMB can not find shopId====%d\n" ,shopId));
		return false;
	}
	/*if(record->pay_ > payment){ ȥ�����
		ACE_DEBUG((LM_ERROR,"buyShopItemByRMBif(record->pay_ > payment)%d\n" ,shopId));
		return false;
	}*/

	CALL_CLIENT(this,orderOk(orderid,shopId));
	
	if(record->type_ == SIT_Recharge)
	{
		///--����û�۳�
		if( (getProp(PT_MagicCurrency)+record->itemNum_ * num) > Global::get<int>(C_DiamondMax))
			return false;
		 S32 diamond = record->itemNum_ * num;
		 if(diamond < 0)
			 return false;
		addMagicCurrency(diamond);

		myselfrecharge_.recharge_ += record->pay_;
		updatemySelfRecharge();
		updateIntegral(record->pay_);

		SGE_LogProduceTrack track;
		track.playerId_ = getGUID();
		track.playerName_ = getNameC();
		track.from_ = 1;
		track.magic_ = diamond;
		LogHandler::instance()->playerTrack(track);

		WorldServ::instance()->updateContactInfo(this);

		GEParam params[2];
		params[0].type_ = GEP_INT;
		params[0].value_.i = record->type_;
		params[1].type_ = GEP_INT;
		params[1].value_.i = record->itemNum_;
		GameEvent::procGameEvent(GET_Recharge,params,2,getHandleId());

		if(!firstRechargeDiamond_){
			firstRechargeDiamond_ = true;
			CALL_CLIENT(this,firstRechargeOK(firstRechargeDiamond_));
		}

		return true;
	}
	if(record->type_ == SIT_VIP)
	{
		///--����û�۳�
		if(record->itemNum_ < getProp(PT_VipLevel))
			return false;

		GEParam params[2];
		params[0].type_ = GEP_INT;
		params[0].value_.i = record->type_;
		params[1].type_ = GEP_INT;
		params[1].value_.i = record->itemNum_;
		GameEvent::procGameEvent(GET_Recharge,params,2,getHandleId());

		if(record->itemNum_ > getProp(PT_VipLevel)){
			setProp(PT_VipTime,Global::get<float>(C_VipTime));
			viprewardflag_ = true;
			CALL_CLIENT(this,sycnVipflag(viprewardflag_));
		}
		else
		{
			float nowtime = getProp(PT_VipTime);
			setProp(PT_VipTime,Global::get<float>(C_VipTime)+nowtime);
		}
		setProp(PT_VipLevel,record->itemNum_);
		

		if(!firstRechargeDiamond_){
			firstRechargeDiamond_ = true;
			CALL_CLIENT(this,firstRechargeOK(firstRechargeDiamond_));
		}

		WorldServ::instance()->updateContactInfo(this);
		return true;
	}
	if(record->type_ == SIT_Fund)
	{
		if(isFund_)
			return false;
		S32 mlb = record->itemNum_ * num;
		if(mlb < 0)
			return false;
		addMagicCurrency(mlb);
		isFund_ = true;
		CALL_CLIENT(this,buyFundOK(isFund_));

		if(!firstRechargeDiamond_){
			firstRechargeDiamond_ = true;
			CALL_CLIENT(this,firstRechargeOK(firstRechargeDiamond_));
		}

		WorldServ::instance()->updateContactInfo(this);
		return true;
	}

	if(record->type_ == SIT_Giftbag)
	{
		if(!updateMinGiftActivity(shopId))
			return false;
		WorldServ::instance()->updateContactInfo(this);
		return true;
	}

	if(record->type_ == SIT_CourseGiftBag)
	{
		if(!buyCourseGift(shopId))
			return false;
		myselfrecharge_.recharge_ += record->pay_;
		updatemySelfRecharge();
		updateIntegral(record->pay_);
		if(!firstRechargeDiamond_){
			firstRechargeDiamond_ = true;
			CALL_CLIENT(this,firstRechargeOK(firstRechargeDiamond_));
		}

		WorldServ::instance()->updateContactInfo(this);
		return true;
	}

	return false;
}

void Player::wishing(COM_Wishing& wish)
{
	if(getProp(PT_Level) < 30)
		return;

	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = wish.wt_;
	GameEvent::procGameEvent(GET_Wish,param,1,getHandleId());

	COM_Wish wi;
	wi.playerName_	= getNameC();
	wi.playerInstId_= getGUID();
	wi.wish_	   = wish.wish_;
	WorldServ::instance()->savewish(wi);

	CALL_CLIENT(this,wishingOK());
}

void Player::shareWish()
{
	if(getProp(PT_Level) < 30)
		return;

	if(wishShareNum_ >= Global::get<int>(C_WishShareMaxNum))
		return;

	COM_Wish* p = WorldServ::instance()->getWish();
	if(p == NULL)
	{
		CALL_CLIENT(this,errorno(EN_WishNull));
		return;
	}
	CALL_CLIENT(this,shareWishOK(*p));
	wishShareNum_++;
	addMoney(Global::get<int>(C_WishShareMoney));
	addExp(Global::get<int>(C_WishShareExp),false);

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 14;
	track.money_ = Global::get<int>(C_WishShareMoney);
	LogHandler::instance()->playerTrack(track);
}

void Player::addReputation(S32 value,bool islogin)
{
	S32 lastvalue = getProp(PT_Reputation);
	S32 lastTitle = TitleTable::findTitleByValue(lastvalue);
	
	S32 currentvalue = lastvalue + value;
	if(currentvalue > 999999)
		currentvalue = 999999;
	if(currentvalue < -999999)
		currentvalue = -999999;

	S32 currentTitle =  TitleTable::findTitleByValue(currentvalue);

	if(currentTitle != lastTitle)
	{
		if(lastTitle!=0)
			delPlayerTitle(lastTitle);
		if(currentTitle!=0)
			addPlayerTitle(currentTitle);
		//�����ǰ�������ƺŲŸ���
		if(TitleTable::isReputationTitle(getProp(PT_Title)))
			setCurrentTitle(currentTitle);
	}

	if(islogin)
		addPlayerTitle(currentTitle);

	setProp(PT_Reputation,currentvalue);
}

void Player::addPlayerTitle(S32 title)
{
	if(!title)return;
	for (size_t i=0; i<titles_.size(); ++i){
		if(titles_[i] == title)
			return;
	}

	titles_.push_back(title);

	CALL_CLIENT(this,addPlayerTitle(title));
}

void Player::delPlayerTitle(S32 title)
{
	for (size_t i=0; i<titles_.size(); ++i)
	{
		if(titles_[i] == title)
		{
			titles_.erase(titles_.begin() + i);
			CALL_CLIENT(this,delPlayerTitle(title));
			return;
		}
	}
}

void Player::setCurrentTitle(S32 title)
{
	if(title == 0)
	{
		setProp(PT_Title,0);
		return;
	}
	for (size_t i=0; i<titles_.size(); ++i)
	{
		if(titles_[i] == title)
		{
			setProp(PT_Title,title);
			return;
		}
	}
}

void Player::resetGreenBoxTime()
{
	greenBoxTimes_ = Global::get<float>(C_BoxGreenTimeCD);
}

void Player::resetBlueBoxTime()
{
	blueBoxTimes_ = Global::get<float>(C_BoxBlueTimeCD);
}

void Player::openBuyBox(){
	CALL_CLIENT(this,requestOpenBuyBox(greenBoxTimes_,blueBoxTimes_,greenBoxFreeNum_));
}

void
Player::resetHundredTier()
{
	if(hundredresetNum_ < Global::get<int>(C_HundredChallengeNum))
		return;

	tier_ = 1;
	hundredresetNum_ -= 1;
	COM_HundredBattle hb;
	hb.tier_	= tier_;
	hb.curTier_ = curTier_;
	hb.surplus_ = hundredNum_;
	hb.resetNum_ = hundredresetNum_;
	CALL_CLIENT(this,syncHundredInfo(hb));
	scenePlayer_->setEntryFlag(playerId_,false);
}

void
Player::requestLevelRank()
{
	U32 myRank = 0;
	std::vector<COM_ContactInfo> rankdata;
	std::vector<COM_ContactInfo> tmpInfos;
	WorldServ::instance()->getPlayerLevelRank(tmpInfos);
	for (size_t i = 0; i < tmpInfos.size(); ++i)
	{
		if(i < 100)
			rankdata.push_back(tmpInfos[i]);
		if(playerName_ == tmpInfos[i].name_ && myRank == 0)
			myRank = tmpInfos[i].rank_;
	}
	CALL_CLIENT(this,requestLevelRankOK(myRank,rankdata));
}

void
Player::requestPlayerFFRank()
{
	U32 myRank = 0;
	std::vector<COM_ContactInfo> rankdata;
	std::vector<COM_ContactInfo> tmpInfos;
	WorldServ::instance()->getPlayerFFRank(tmpInfos);
	for (size_t i = 0; i < tmpInfos.size(); ++i)
	{
		if(i < 100)
			rankdata.push_back(tmpInfos[i]);
		if(playerName_ == tmpInfos[i].name_ && myRank == 0)
			myRank = tmpInfos[i].rank_;
	}
	CALL_CLIENT(this,requestPlayerFFRankOK(myRank,rankdata));
}
void
Player::requestBabyRank()
{
	U32 myRank = 0;
	std::vector<COM_BabyRankData> rankdata;
	std::vector<COM_BabyRankData> tmpInfos;
	WorldServ::instance()->getBabyFFRank(tmpInfos);
	for (size_t i = 0; i < tmpInfos.size(); ++i)
	{
		if(i < 100)
			rankdata.push_back(tmpInfos[i]);
		Baby* baby = getBattleBaby();
		if(baby == NULL)
			continue;
		if(baby->getGUID() == tmpInfos[i].instId_ && myRank == 0)
			myRank = tmpInfos[i].rank_;
	}
	CALL_CLIENT(this,requestBabyRankOK(myRank,rankdata));
}

void
Player::requestEmpRank()
{
	U32 myRank = 0;
	std::vector<COM_EmployeeRankData> rankdata;
	std::vector<COM_EmployeeRankData> tmpInfos;
	WorldServ::instance()->getEmployeeFFRank(tmpInfos);
	for (size_t i = 0; i < tmpInfos.size(); ++i)
	{
		if(tmpInfos[i].rank_ <= 100)
			rankdata.push_back(tmpInfos[i]);
		if(playerName_ == tmpInfos[i].ownerName_){
			if (tmpInfos[i].rank_ < myRank || myRank == 0){
				myRank = tmpInfos[i].rank_;
			}
		}
	}
	CALL_CLIENT(this,requestEmpRankOK(myRank,rankdata));
}

void
Player::sign(S32 index)
{
	if(signFlag_)
	{
		return ;
	}
	
	U32 l = 0x1 << index;
	if( signs_ & l ) 
		return ;

	signs_ |= l;

	GameEvent::procGameEvent(GET_Sign,NULL,0,handleId_);
	
	ACE_Date_Time dateTime;
	dateTime.update();

	const std::vector<S32>* rewards = DailyReward::getMonthRewards(dateTime.month());
	if(rewards && rewards->size() > index)
	{
		int itemid = (*rewards)[index];

		const ItemTable::ItemData * itemdata = ItemTable::getItemById(itemid);

		if(NULL == itemdata)
			return ;
		addBagItemByItemId(itemid,1,false,3);
	}

	signFlag_ = true;

	CALL_CLIENT(this,signUp(signFlag_));
}

//----
void
Player::setHundredTier(S32 tier)
{
	tier_ = tier;

	COM_HundredBattle hb;
	hb.tier_	= tier_;
	hb.curTier_ = curTier_;
	hb.surplus_ = hundredNum_;
	hb.resetNum_ = hundredresetNum_;
	CALL_CLIENT(this,syncHundredInfo(hb));
}

void 
Player::enterHundredScene(S32 level){

	if(level > tier_)
		return;
	
	if(hundredNum_ <= 0)
		return; //��ս��������

	const ChallengeTable::Core * tmp = ChallengeTable::getDataById(level);

	if(NULL == tmp)
		return; 

	//��ǰ��
	curTier_ = level;
	
	//���뵱ǰ�㳡��
	if(myTeam()){
		if(isTeamLeader() )//&& !(myTeam()->hasLeaveMember()))
			transforScene(tmp->senceId_);
	}
	else 
		transforScene(tmp->senceId_);
}

bool 
Player::canGotoNextScene(){
	if(!ChallengeTable::isHundredSence(sceneId_))
		return true; //���˵���  
	return (tier_ >= curTier_ + 1);
}

void
Player::resetCounter()
{
	hundredNum_ = Global::get<int>(C_HundredChallengeNum);

	COM_HundredBattle hb;
	hb.tier_	= tier_;
	hb.curTier_ = curTier_;
	hb.surplus_ = hundredNum_;
	hb.resetNum_ = hundredresetNum_;
	CALL_CLIENT(this,syncHundredInfo(hb));

	rivalNum_ = Global::get<int>(C_JJCRivalNum);
	requestJJCData();

	resetgathers();
}

void 
Player::resetBaby(S32 babyId)
{
	Baby* pb = findBaby(babyId);
	if(NULL == pb)
		return;
	MonsterTable::MonsterData const *tmp = MonsterTable::getMonsterById(pb->getProp(PT_TableId));
	if(NULL == tmp)
		return ;
	if(tmp->monsterType_ == 3)	//3Ϊ������ﲻ�ܻ�ԭ
		return;
	S32 itemid = Global::get<int>(C_ResetBabyPay);
	if(ItemTable::getItemById(itemid) == NULL)
		return;
	if(getItemNumByItemId(itemid) <= 0)
		return;
	delBagItemByItemId(itemid);

	pb->resetBaby();
}

//void
//Player::resetBabyProp(S32 babyId)
//{
//	Baby* pb = findBaby(babyId);
//	if(NULL == pb)
//		return;
//	S32 itemid = Global::get<int>(C_ResetBabyPay);
//
//	if(ItemTable::getItemById(itemid) == NULL)
//		return;
//	if(getItemNumByItemId(itemid) <= 0)
//		return;
//	delBagItemByItemId(itemid,1);
//}

void
Player::caleDoubleExpTime(float dt)
{
	if(!openDoubleTimeFlag_)
		return;
		
	sycnTime_ += dt;

	if(sycnTime_ >= 60 && getProp(PT_DoubleExp) > 0)
	{
		float curTime = getProp(PT_DoubleExp) - sycnTime_;
		sycnTime_ = 0;
		setProp(PT_DoubleExp,curTime);
	}

	if(getProp(PT_DoubleExp) <= 0 && openDoubleTimeFlag_)
	{
		setProp(PT_DoubleExp,0);
		openDoubleTimeFlag_ = false;
		CALL_CLIENT(this,sycnDoubleExpTime(openDoubleTimeFlag_,getProp(PT_DoubleExp)));
	}
}

void
Player::setOpenDoubleTimeFlag(bool isFlag)
{
	if(getProp(PT_DoubleExp) <= 0)
	{
		CALL_CLIENT(this, errorno(EN_DoubleExpTimeNULL));
		return;
	}
	openDoubleTimeFlag_ = isFlag;
	sycnTime_ = 0;
	CALL_CLIENT(this,sycnDoubleExpTime(isFlag,getProp(PT_DoubleExp)));
}

void Player::addNpc(NpcList& npcs){
	npcList_.insert(npcList_.end(),npcs.begin(),npcs.end());
	CALL_CLIENT(this,addNpc(npcs));
}

void Player::delNpc(NpcList& npcs){
	for(S32 i=0; i<npcList_.size(); ++i){
		if(std::find(npcs.begin(),npcs.end(),npcList_[i]) != npcs.end())
			npcList_.erase(npcList_.begin() + i--);
	}
	CALL_CLIENT(this,delNpc(npcs));
}

void Player::initNpc(){
	checkNpc();
	CALL_CLIENT(this,initNpc(npcList_));
}

void Player::checkNpc(){ 
	for (size_t i=0; i<npcList_.size();++i)
	{
		const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcList_[i]);
		if(!npcd){
			npcList_.erase(npcList_.begin() + i--);
			continue;
		}
		if((npcd->npcType_ >= NT_Caiji1) && (npcd->npcType_ <= NT_Caiji3) ){
			continue;
		}
		if(npcd->sceneId_ != GET_SCENE_ORIGINAL_ID(sceneId_)){
			npcList_.erase(npcList_.begin() + i--);
		}
	}
}

void Player::initQuest(){
	CALL_CLIENT(this,initQuest(currentQuest_,completeQuest_));
}

void Player::localAddNpc(int npcId){
	for(size_t i=0; i<npcList_.size(); ++i){
		if(npcList_[i] == npcId)
			return;
	}
	scenePlayer_->addNpc(npcId);
	npcList_.push_back(npcId);
}

void Player::localDelNpc(int npcId){
	for(size_t i=0; i<npcList_.size(); ++i){
		if(npcList_[i] == npcId){
			scenePlayer_->delNpc(npcId);
			npcList_.erase(npcList_.begin() + i);
			return;
		}		
	}
}

bool Player::hasNpc(int npcId){
	for(size_t i=0; i<npcList_.size(); ++i){
		if(npcList_[i] == npcId)
			return true;
	}
	return false;
}

void Player::talkedNpc(int npcId)
{
	//ACE_DEBUG((LM_ERROR,"Player %s talked to npc  %d\n",playerName_.c_str(),npcId));
	const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcId);
	if(npcd == NULL)
		return;
	if(npcd->npcType_ == NT_Mogu || npcd->npcType_ == NT_Xiji || npcd->npcType_ == NT_SinglePK || npcd->npcType_ == NT_TeamPK){
		scenePlayer_->findDynamicNpc(npcId);
		return;
	}else if (npcd->npcType_ == NT_Caiji1 || npcd->npcType_ == NT_Caiji2 || npcd->npcType_ == NT_Caiji3){
		if (getBagEmptySlot() <= 0){
			errorMessageToC(EN_BagFull); // �ɼ��������˲���talk
			return ;
		}
	}
	talkNpc(npcId);
}

void Player::talkNpc(int npcId)
{
	const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcId);
	if(npcd->sceneId_ != GET_SCENE_ORIGINAL_ID(sceneId_)){
		ACE_DEBUG((LM_ERROR,"Can not talk to other scene npc %s %d %d\n",playerName_.c_str(),GET_SCENE_ORIGINAL_ID(sceneId_),npcId));
		return;
	}
	if(!hasNpc(npcId))
		return;
	GEParam param[1];
	param->type_ = GEP_INT;
	param->value_.i = npcId;
	GameEvent::procGameEvent(GET_TalkNpc,param,1,handleId_);
}

void
Player::calePvpPlayerGrade(Team* tmp, GroupType winGT)
{
	U32	grade = 5;

	if(battleForce_ == winGT)
	{
		for (size_t i = 0; i < tmp->getMemberSize(); ++i)
		{
			if((tmp->teamMembers_[i]->pvpInfo_.section_ - pvpInfo_.section_) <= 0)
				grade += 1;
			else
				grade += (tmp->teamMembers_[i]->pvpInfo_.section_ - pvpInfo_.section_);
		}

		pvpInfo_.value_ += grade;

		if(pvpInfo_.value_ < 0)
			pvpInfo_.value_ = 0;

		battleValue_ += grade;

		if(pvpInfo_.isCont_)
			pvpInfo_.contWin_ += 1;
		else
		{
			pvpInfo_.isCont_ = true;
			pvpInfo_.contWin_= 1;
		}
		pvpInfo_.winNum_ += 1;
	}
	else
	{
		for (size_t i = 0; i < tmp->getMemberSize(); ++i)
		{
			if((pvpInfo_.section_ - tmp->teamMembers_[i]->pvpInfo_.section_) <= 0)
				grade += 1;
			else
				grade += (pvpInfo_.section_ - tmp->teamMembers_[i]->pvpInfo_.section_);
		}

		pvpInfo_.value_ -= grade;

		if(pvpInfo_.value_ < 0)
			pvpInfo_.value_ = 0;

		battleValue_ -= grade;

		if(!pvpInfo_.isCont_)
			pvpInfo_.contWin_ = 0;
	}

	pvpInfo_.battleNum_ += 1;

	pvpInfo_.winValue_ = pvpInfo_.winNum_/pvpInfo_.battleNum_;

	checkJJCsec();
	WorldServ::instance()->updateContactInfo(this);
}

void
Player::caleSinglePvpPlayerGrade(GroupType winGT)
{
	U32	grade = 5;

	if(battleForce_ == winGT)
	{
		grade += 1;
		
		pvpInfo_.value_ += grade;

		if(pvpInfo_.value_ < 0)
			pvpInfo_.value_ = 0;

		battleValue_ += grade;

		if(pvpInfo_.isCont_)
			pvpInfo_.contWin_ += 1;
		else
		{
			pvpInfo_.isCont_ = true;
			pvpInfo_.contWin_= 1;
		}
		pvpInfo_.winNum_ += 1;
	}
	else
	{
		pvpInfo_.isCont_ = false;
		grade += 1;
		pvpInfo_.value_ -= grade;
		if(pvpInfo_.value_ < 0)
			pvpInfo_.value_ = 0;

		battleValue_ -= grade;

		if(!pvpInfo_.isCont_)
			pvpInfo_.contWin_ = 0;
	}

	pvpInfo_.battleNum_ += 1;

	pvpInfo_.winValue_ = pvpInfo_.winNum_/pvpInfo_.battleNum_;

	checkJJCsec();
	WorldServ::instance()->updateContactInfo(this);
}

void
Player::checkJJCsec()
{
	S32 curSec = pvpInfo_.section_;

	const PvpRunkTable::PvpRunkDate * data = PvpRunkTable::getPvpRunkById(pvpInfo_.section_);

	if(data == NULL)
		return;

	if(pvpInfo_.value_ > data->gradeMax_)
	{
		if(pvpInfo_.section_ >= 15)
		{
			CALL_CLIENT(this,updatePvpJJCinfo(pvpInfo_));
			return;
		}
		pvpInfo_.section_ += 1;
		checkJJCsec();
	}
	else if (pvpInfo_.value_ < data->gradeMin_)
	{
		pvpInfo_.section_ -= 1;
		if(pvpInfo_.section_ < 1)
		{
			pvpInfo_.section_ = 1;
			CALL_CLIENT(this,updatePvpJJCinfo(pvpInfo_));
			return;
		}
		checkJJCsec();
	}

	if(pvpInfo_.section_ != curSec)
	{
		GEParam params[1];
		params[0].type_ = GEP_INT;
		params[0].value_.i = pvpInfo_.section_;
		GameEvent::procGameEvent(GET_JJC,params,1,getHandleId());
	}
	CALL_CLIENT(this,updatePvpJJCinfo(pvpInfo_));
}

void
Player::syncMyJJCTeamMsg()
{
	if(!isTeamMember())
		return;

	Team *p = TeamLobby::instance()->getTeam(this,teamId_);
	if(p == NULL)
		return;

	Player* leader = p->getLeader();
	if(leader == NULL)
		return;

	if(leader != this)	//���Ƕӳ�,�������������Ա����ͬ��
	{
		//CALL_CLIENT(this,syncMyJJCTeamMember());
		return;
	}

	for (size_t i = 0; i < p->getMemberSize(); ++i)
	{
		if(p->teamMembers_[i]->isLeavingTeam_)
			return;
		CALL_CLIENT(p->teamMembers_[i],syncMyJJCTeamMember());
	}
}

void Player::jjcBattleGo(U32 id)
{
	PvpJJC::pvpjjcBattleGo(this,id);
}

void Player::initMail(){
	CALL_CLIENT(this,appendMail(mails_));
}

void Player::sendMail(STRING& playername, STRING& title, STRING& content){
	std::vector<COM_MailItem> items;
	WorldServ::instance()->sendMail(this,playername,title,content,items);
}

void Player::readMail(S32 mailId){
	for (size_t i=0; i<mails_.size(); ++i)
	{
		if(mails_[i].mailId_ == mailId && !mails_[i].isRead_){
			mails_[i].isRead_ = true;
			DBHandler::instance()->updateMail(mails_[i]);
			CALL_CLIENT(this,updateMailOk(mails_[i]));
			return ;
		}
	}
}

void Player::delMail(S32 mailId){

	for (size_t i=0; i<mails_.size(); ++i)
	{
		if(mails_[i].mailId_ == mailId){
			mails_.erase(mails_.begin() + i);
			DBHandler::instance()->delMail(playerName_,mailId);
			return ;
		}
	}
}

void Player::fatchMail(){
	if(isFatchMail_)
		return;
	isFatchMail_ = true;
	DBHandler::instance()->fatchMail(playerName_,fatchMailId_);
}

void Player::appendMail(std::vector<COM_Mail>& mails){
	isFatchMail_ = false;
	if(mails.empty())
		return ;
	mails_.insert(mails_.end(),mails.begin(),mails.end());
	fatchMailId_ = mails.back().mailId_;
	CALL_CLIENT(this,appendMail(mails));
}

void Player::getMailItem(S32 mailId){
	if(mails_.empty())
		return;
	for(int i=0; i<mails_.size(); ++i){
		if(mails_[i].mailId_ == mailId){
			//ACE_DEBUG((LM_INFO,"Player get mail item %s(%d)\n",playerName_.c_str(),playerId_));
			//����ʼ�����
			if(!mails_[i].items_.empty())
			{
				if(mails_[i].items_.size() > getBagEmptySlot()){
					CALL_CLIENT(this,errorno(EN_GetMailItemBagFull));
					return;
				}
			
				for(int j=0; j<mails_[i].items_.size();++j){
					addBagItemByItemId(mails_[i].items_[j].itemId_,mails_[i].items_[j].itemStack_,false,4);
				}

				mails_[i].items_.clear();
			}

			if(mails_[i].money_!=0){
				addMoney(mails_[i].money_);

				SGE_LogProduceTrack track;
				track.playerId_ = getGUID();
				track.playerName_ = getNameC();
				track.from_ = 4;
				track.money_ = mails_[i].money_;
				LogHandler::instance()->playerTrack(track);

				mails_[i].money_=0;
			}

			if(mails_[i].diamond_ != 0){
				addDiamond(mails_[i].diamond_);

				SGE_LogProduceTrack track;
				track.playerId_ = getGUID();
				track.playerName_ = getNameC();
				track.from_ = 4;
				track.diamond_ = mails_[i].diamond_;
				LogHandler::instance()->playerTrack(track);

				mails_[i].diamond_ = 0;
			}

			DBHandler::instance()->updateMail(mails_[i]);
			CALL_CLIENT(this,updateMailOk(mails_[i]));
			return;
		}
	}
}

void
Player::syncState()
{
	std::vector<COM_State> tmpSt;
	for (size_t i=0; i<states_.size(); ++i)
	{
		StateTable::Core const * pCore = StateTable::getStateById(states_[i].stateId_);
		if(!pCore || pCore->battleDelete_)
			continue;
		tmpSt.push_back(states_[i]);
	}

	CALL_CLIENT(this,sycnStates(tmpSt));
}

void Player::initSelling(){
	if(MallHandler::instance()->isConnect_ == false){
		return; ///�̻������
	}
	MallHandler::instance()->fetchMySell(playerId_);
	MallHandler::instance()->fetchSelledItem(playerId_);
}

void Player::initSellingOk(std::vector<COM_SellItem>& items){
	if(!items.empty()){
		sellitems_ = items;
	}
	CALL_CLIENT(this,initMySelling(sellitems_));
}

void Player::initSelledOk(std::vector<COM_SelledItem>& items){
	if(!items.empty()){
		selledItems_ = items;
	}
	CALL_CLIENT(this,initMySelled(selledItems_));
}

void Player::fetchSell(COM_SearchContext& context,bool is2){
	if(waitSell_)
		return;
	is2_ = is2;
	if(MallHandler::instance()->isConnect_ == false){
		return; ///�̻������
	}
	MallHandler::instance()->fetchSell(playerId_,context);
	waitSell_ = true;
}

void Player::fetchSellOk(std::vector<COM_SellItem>& items,S32 total){
	if(is2_)
		CALL_CLIENT(this,fetchSellingOk2(items,total));
	else
		CALL_CLIENT(this,fetchSellingOk(items,total));
	waitSell_ = false;
}

void 
Player::sell(S32 iteminstid, S32 babyinstid,S32 price){
	if(!getOpenSubSystemFlag(OSSF_AuctionHouse))
	{
		errorMessageToC(EN_OpenBaoXiangLevel);
		return;
	}
	if(isBattle())
	{
		errorMessageToC(EN_Battle);
		return;
	}
	if(price <=0)
		return;
	if(MallHandler::instance()->isConnect_ == false){
		return; ///�̻������û��
	}
	if(waitSell_)
		return;
	COM_Item* item = getBagItemByInstId(iteminstid);
	Baby* baby = findBaby(babyinstid);
	
	if(!item && !baby)
		return;//��û��

	if(item && baby){
		return; ///����ͬʱ�������ֶ���
	}
	
	if(item ){
		if( item->lastSellTime_ > 0 )
		{
			errorMessageToC(EN_MallSellTimeLess);
			return;
		}
		else if(item->isBind_)
			return ;
		else if(item->isLock_)
			return;
	} else if (baby){
		if(baby->lastSellTime_ > 0 )
		{
			errorMessageToC(EN_MallSellTimeLess);
			return;
		}
		else if(baby->isBind_)
			return ;
		else if(baby->isLock_)
			return;
	}	

	if(Global::get<int>(C_MallSellPay) > getProp(PT_Money)){
		return; /// û��̯λ��
	}

	COM_SellItem sellitem;
	sellitem.time_ = WorldServ::instance()->curTime_;
	sellitem.sellPlayerId_ = playerId_;
	sellitem.sellPrice = price;
	if(item){
		sellitem.item_ = *item;
		ItemTable::ItemData const *p = ItemTable::getItemById(item->itemId_);
		if(NULL == p)
			return;
		//if(p->price_ <=0 )
		//	return; //��������
		sellitem.ist_ = p->subType_;
		sellitem.title_ = p->name_;
		MallHandler::instance()->sell(sellitem);
		waitSell_ = true;
	}
	else if(baby){
		if(baby->isBattle_)
			return ; //��ս����������
		
		baby->getBabyInst(sellitem.baby_);
		sellitem.baby_.ownerName_ = "";
		MonsterTable::MonsterData const* p = MonsterTable::getMonsterById(baby->getProp(PT_TableId));
		if(NULL == p)
			return;
		sellitem.baby_.instName_  = p->name_; ///�۳�ʱ�������ָ�Ĭ��
		sellitem.rt_ = p->race_;
		sellitem.title_ =p->name_;
		MallHandler::instance()->sell(sellitem);
		waitSell_ = true;
	}
}

void Player::sellOk(COM_SellItem item){
	ACE_DEBUG((LM_INFO,"Sell ok guid  %d\n",item.guid_));
	sellitems_.push_back(item);
	if(item.baby_.instId_)
		delBaby(item.baby_.instId_);
	else if(item.item_.instId_)
		delBagItemByInstId(item.item_.instId_,item.item_.stack_);
	addMoney(-Global::get<float>(C_MallSellPay)); //��ȥ̯λ��
	CALL_CLIENT(this,sellingOk(item));
	waitSell_ = false;

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 5;
	track.money_ = -Global::get<int>(C_MallSellPay);
	LogHandler::instance()->playerTrack(track);
}

void Player::unsell(S32 guid){
	if(MallHandler::instance()->isConnect_ == false)
		return; ///�̻������û��
	if(waitSell_)
		return;
	
	for (size_t i=0; i<sellitems_.size(); ++i)
	{
		if(sellitems_[i].guid_ == guid){
			if(sellitems_[i].baby_.instId_ != 0){
				if(babies_.size() >= Global::get<int>(C_BabyMax)){
					CALL_CLIENT(this,errorno(EN_BabyFull));
					return; ///����ȡ���¼�
				}
			}
			else if(sellitems_[i].item_.itemId_ != 0){
				if(getBagEmptySlot() <= 0 ){
					CALL_CLIENT(this,errorno(EN_BagFull));
					return; ///����ȡ���¼�
				}
			}
			MallHandler::instance()->unSell(playerId_,sellitems_[i].guid_);
			waitSell_ = true;
		}
	}
	ACE_DEBUG((LM_INFO,"Unsell befor %d\n",guid));
}

void Player::unSellOk(S32 guid){
	ACE_DEBUG((LM_INFO,"Unsell ok guid  %d\n",guid));
	for (size_t i=0; i<sellitems_.size(); ++i)
	{
		if(sellitems_[i].guid_ == guid){
			if(sellitems_[i].baby_.instId_){
				addBaby2(sellitems_[i].baby_);
				//DBHandler::instance()->resetBabyOwner(playerName_,sellitems_[i].baby_.instId_);
			}
			else if(sellitems_[i].item_.instId_){
				sellitems_[i].item_.instId_ = ++genItemMaxGuid_;
				COM_Item *p = NEW_MEM(COM_Item,sellitems_[i].item_);
				if(!addBagItemByInst(p,5))
					return;
			}
			sellitems_.erase(sellitems_.begin()+i);
			CALL_CLIENT(this,unsellingOk(guid));
			
			break;
		}
	}
	waitSell_ = false;
}

void Player::buy(S32 guid){
	if(!getOpenSubSystemFlag(OSSF_AuctionHouse))
	{
		errorMessageToC(EN_OpenBaoXiangLevel);
		return;
	}
	if(isBattle())
	{
		errorMessageToC(EN_Battle);
		return;
	}
	if(getBagEmptySlot() <=0)
	{
		CALL_CLIENT(this,errorno(EN_BagFull));
		return;
	}
	if(MallHandler::instance()->isConnect_ == false){
		return; ///�̻������û��
	}
	if(waitSell_)
		return;
	if(getProp(PT_VipLevel) < VL_1){
		CALL_CLIENT(this,errorno(EN_NotSuperVip)); //���ǳ���VIP
			return;
	}
	SGE_BuyContent bc;
	bc.playerId_ = playerId_;
	bc.guid_ = guid;
	bc.diamond_ = getProp(PT_Diamond);
	bc.magic_ = getProp(PT_MagicCurrency);
	bc.isBagFull_ = !getBagEmptySlot();
	bc.isBabyFull_ = babies_.size() >= Global::get<int>(C_BabyMax);
	MallHandler::instance()->buy(bc);
	waitSell_ = true;
	//ACE_DEBUG((LM_INFO,"Buy befor %d\n",guid));
}

void Player::buyOk(COM_SellItem& item){
	if(getBagEmptySlot() <=0)
	{
		CALL_CLIENT(this,errorno(EN_BagFull));
		return;
	}
	COM_SelledItem sitem;
	sitem.guid_ = item.guid_;
	sitem.sellPlayerId_ = item.sellPlayerId_;
	if(item.baby_.properties_.empty())
		sitem.babyId_ = 0;
	else 
		sitem.babyId_ = item.baby_.properties_[PT_TableId];
	sitem.itemId_ = item.item_.itemId_;
	sitem.itemStack_ = item.item_.stack_;
	sitem.price_ = item.sellPrice;
	sitem.selledTime_ = WorldServ::instance()->curTime_;

	sitem.tax_ = UtlMath::ceil(item.sellPrice * Global::get<float>(C_MallTax));
	//ACE_DEBUG((LM_INFO,"Player mall buy %s(%d) %d %d %d\n",playerName_.c_str(), playerId_, sitem.sellPlayerId_, sitem.itemId_,sitem.babyId_));
	Player *sellplayer = Player::getPlayerByInstId(item.sellPlayerId_);
	if(sellplayer){
		for (size_t i=0; i<sellplayer->sellitems_.size(); ++i)
		{
			if(sellplayer->sellitems_[i].guid_ == item.guid_){
				sellplayer->sellitems_.erase(sellplayer->sellitems_.begin() + i);
				sellplayer->selledItems_.push_back(sitem);
				CALL_CLIENT(sellplayer,selledOk(sitem));
				break;
			}
		}
	}
	else{
		ACE_DEBUG((LM_INFO,"Sell player is offline %d\n",item.sellPlayerId_));
	}
	if(getProp(PT_Diamond) < item.sellPrice)
		addMagicCurrency(-item.sellPrice);
	else  
		addDiamond(-item.sellPrice);

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 5;
	track.diamond_ = -item.sellPrice;
	LogHandler::instance()->playerTrack(track);

	COM_ContactInfo *cif = WorldServ::instance()->findContactInfo(item.sellPlayerId_);
	if(cif!=NULL){
		std::vector<COM_MailItem> itemsss;
		std::string mailSender = Global::get<std::string>(C_MallMailSender);
		std::string mailTitle = Global::get<std::string>(C_MallMailTitle);
		std::string mailContent = Global::get<std::string>(C_MallMailContent);
		WorldServ::instance()->sendMail(mailSender,cif->name_,mailTitle,mailContent,0,item.sellPrice * (1.F-Global::get<float>(C_MallTax)),itemsss);
	}
	else {
		ACE_DEBUG((LM_ERROR,"void Player::buyOk(COM_SellItem& item){ if(cif!=NULL){else { %d\n",item.sellPlayerId_));
	}
	MallHandler::instance()->insertSelledItem(sitem);
	ACE_DEBUG((LM_DEBUG,"BUY %d %d \n",item.baby_.instId_, item.item_.instId_));
	if(item.baby_.instId_){
		item.baby_.ownerName_ = playerName_;
		item.baby_.lastSellTime_ = Global::get<int>(C_MallShopFreezeTime) * ONE_DAY_SEC;
		addBaby2(item.baby_);
		//DBHandler::instance()->resetBabyOwner(playerName_,item.baby_.instId_);
	}
	else if(item.item_.instId_){
		item.item_.lastSellTime_ =  Global::get<int>(C_MallShopFreezeTime) * ONE_DAY_SEC;
		item.item_.instId_ = ++genItemMaxGuid_;
		COM_Item *p = NEW_MEM(COM_Item,item.item_);
		if(!addBagItemByInst(p,5))
			return;
	}

	CALL_CLIENT(this,errorno(EN_MallBuyOk));
	waitSell_ = false;
	//CALL_CLIENT(this,buyResult(1));

}
void Player::buyFail(ErrorNo error){
	///	
	waitSell_ = false;
	CALL_CLIENT(this,errorno(error));
	//CALL_CLIENT(this,buyResult(0));
}

//////////////////////////////////////////////////////////////////////////
bool Player::calcInRect(COM_FPosition pos){
	float r = Global::get<float>(C_SceneVisibleMaxRadius);
	return (std::fabs(pos.x_) - std::fabs(position_.x_) < r) &&  (std::fabs(pos.z_) - std::fabs(position_.z_) < r);
}
void Player::calcBroadcastPlayers(SceneBroadcaster& broadcaster){
	Scene* s = myScene();
	if(!s)
		return;
	for(size_t i=0; i<broadcastPlayers_.size(); ++i)
	{
		Player* p = s->getPlayerById(broadcastPlayers_[i]);
		if(p && p->getClient())
			broadcaster.addChannel(p->getClient());
	}
}
void Player::regBroadcastPlayer(U32 instId){
	if(std::find(broadcastPlayers_.begin(),broadcastPlayers_.end(),instId) != broadcastPlayers_.end())
		return;
	broadcastPlayers_.push_back(instId);
}
void Player::unregBroadcastPlayer(U32 instId){
	std::vector<U32>::iterator itr = std::find(broadcastPlayers_.begin(),broadcastPlayers_.end(),instId);
	if(itr == broadcastPlayers_.end())
		return;
	broadcastPlayers_.erase(itr);
}
bool Player::isCanVisiblePlayer(Player* p){
	if(filterTypes_.empty())
		return false;
	if(p->myTeam()){
		if(teamId_ == p->teamId_ && std::find(filterTypes_.begin(),filterTypes_.end(),SFT_Team) != filterTypes_.end())
			return true;
	}
	if(p->myGuild()){
		if(myGuild() == p->myGuild() && std::find(filterTypes_.begin(),filterTypes_.end(),SFT_Guild) != filterTypes_.end())
			return true;	
	}
	if(findFriendById(p->getGUID())){
		if(std::find(filterTypes_.begin(),filterTypes_.end(),SFT_Friend) != filterTypes_.end())
			return true;
	}
	
	return std::find(filterTypes_.begin(),filterTypes_.end(),SFT_All) != filterTypes_.end();
}	

bool Player::isInVisiblePlayers(U32 instId){
	return std::find(visiblePlayers_.begin(),visiblePlayers_.end(),instId) != visiblePlayers_.end();
}
struct VisiblePlayerSort{
	VisiblePlayerSort(Player* p):player_(p){}

	bool operator()(U32 id0, U32 id1){
		Player* p0 = Player::getPlayerByInstId(id0);
		Player* p1 = Player::getPlayerByInstId(id1);

		if(!p0)
			return false;
		if(!p1)
			return true;
		if(!p0 && !p1)
			return true;
		
		if(player_->myTeam()){
			if(player_->teamId_ == p0->teamId_ && player_->teamId_ == p1->teamId_)
				return p0->getGUID() < p1->getGUID();
			else if(player_->teamId_ == p0->teamId_ && player_->teamId_ != p1->teamId_)
				return true;
			else if(player_->teamId_ != p0->teamId_ && player_->teamId_ == p1->teamId_)
				return false;
		}
		Guild* pGuild = player_->myGuild();
		if(pGuild){
			Guild* pGuild0 = p0->myGuild();
			Guild* pGuild1 = p1->myGuild();
			if(pGuild == pGuild0 && pGuild == pGuild1)
				return p0->getGUID() < p1->getGUID();
			else if(pGuild == pGuild0 && pGuild != pGuild1)
				return true;
			else if(pGuild != pGuild0 && pGuild == pGuild1)
				return false;
		}
		
		return p0->getGUID() < p1->getGUID();
	}

private:
	Player* player_;
};
void Player::calcVisiblePlayers(){
	if(!isSceneLoaded_)
		return;

	int maxVisible = Global::get<int>(C_SceneVisibleMaxNum);

	Scene* s = myScene();
	if(!s)
		return;
	
	if((std::find(filterTypes_.begin(),filterTypes_.end(),SFT_Team) != filterTypes_.end()) || (std::find(filterTypes_.begin(),filterTypes_.end(),SFT_All) != filterTypes_.end())){
		Team* pTeam = myTeam();
		if(pTeam){
			for(size_t i=0; i<pTeam->teamMembers_.size(); ++i){ 
				if(!pTeam->teamMembers_[i])
					continue;
				if(pTeam->teamMembers_[i] == this)
					continue;
				if(pTeam->teamMembers_[i]->sceneId_ != sceneId_)
					continue;
				if(isInVisiblePlayers(pTeam->teamMembers_[i]->getGUID()))
					continue;
				pTeam->teamMembers_[i]->regBroadcastPlayer(playerId_);
				regVisiblePlayer(pTeam->teamMembers_[i]->getGUID());
				COM_ScenePlayerInformation info;
				pTeam->teamMembers_[i]->getSimpleScenePlayer(info);
				CALL_CLIENT(this,addToScene(info));
			}
		}
	}
		
	VisiblePlayerSort compFunc(this);
	std::sort(visiblePlayers_.begin(),visiblePlayers_.end(),compFunc);
	for (size_t i=0; i<visiblePlayers_.size(); ++i)
	{
		Player* p = s->getPlayerById(visiblePlayers_[i]);
		if(!p){
			//p->unregBroadcastPlayer(playerId_);
			CALL_CLIENT(this,delFormScene(visiblePlayers_[i]));
			visiblePlayers_.erase(visiblePlayers_.begin() + i--);
		}
		else if ((myTeam() != NULL)  && (p->myTeam() == myTeam()))
		{
			continue;
		}
		else if(p->sceneId_ != sceneId_){
			p->unregBroadcastPlayer(playerId_);
			CALL_CLIENT(this,delFormScene(visiblePlayers_[i]));
			visiblePlayers_.erase(visiblePlayers_.begin() + i--);
		}
		else if(i >= maxVisible){
			p->unregBroadcastPlayer(playerId_);
			CALL_CLIENT(this,delFormScene(visiblePlayers_[i]));
			visiblePlayers_.erase(visiblePlayers_.begin() + i--);
		}
		else if(!calcInRect(p->position_))
		{
			p->unregBroadcastPlayer(playerId_);
			CALL_CLIENT(this,delFormScene(visiblePlayers_[i]));
			visiblePlayers_.erase(visiblePlayers_.begin() + i--);
		}
		else if(!isCanVisiblePlayer(p))
		{
			p->unregBroadcastPlayer(playerId_);
			CALL_CLIENT(this,delFormScene(visiblePlayers_[i]));
			visiblePlayers_.erase(visiblePlayers_.begin() + i--);
		}

	}
	
	for(IdPlayerMap::iterator i=s->idStore_.begin(), e = s->idStore_.end(); i!=e && visiblePlayers_.size() < maxVisible; ++i)
	{
		Player* p = i->second;
		if(!p){
			continue;
		}
		if(p == this)
			continue;
		if(isInVisiblePlayers(p->getGUID()))
			continue;
		if(!calcInRect(p->position_))
			continue;
		if(isCanVisiblePlayer(p))
		{
			p->regBroadcastPlayer(playerId_);
			regVisiblePlayer(p->getGUID());
			COM_ScenePlayerInformation info;
			p->getSimpleScenePlayer(info);
			CALL_CLIENT(this,addToScene(info));
		}
	}
}
void Player::regVisiblePlayer(U32 instId){
	if(std::find(visiblePlayers_.begin(),visiblePlayers_.end(),instId) != visiblePlayers_.end())
		return;
	visiblePlayers_.push_back(instId);
}
void Player::unregVisiblePlayer(U32 instId){
	std::vector<U32>::iterator itr = std::find(visiblePlayers_.begin(),visiblePlayers_.end(),instId);
	if(itr == visiblePlayers_.end())
		return;
	visiblePlayers_.erase(itr);
}

void Player::move2(COM_FPosition& pos){
	position_ = pos;
	SceneBroadcaster broadcaster;
	calcBroadcastPlayers(broadcaster);
	if(getClient())
		broadcaster.addChannel(getClient());
	broadcaster.move2(playerId_,pos);
}
void Player::transfor2(COM_FPosition& pos){
	position_ = pos;
	//Scene* s = SceneManager::instance()->getScene(sceneId_);
	//SRV_ASSERT(s);
	SceneBroadcaster broadcaster;
	calcBroadcastPlayers(broadcaster);
	if(getClient())
		broadcaster.addChannel(getClient());
	broadcaster.transfor2(playerId_,pos);
}
void Player::joinScene(COM_SceneInfo& info){
	isSceneLoaded_ = false;
	bool needCleanCopyQuest = true;
	syncNpcs_.clear();
	//��ԭ�����㲥����ɾ��
	Scene* s = myScene();
	if(s){//�㲥���³������������һ�����
		s->delPlayer(this);
	}

	sceneId_ = info.sceneId_;
	position_ = info.position_;
	
	checkNpc();
	for(size_t i=0 ; i < info.npcs_.size(); ++i){
		if(std::find(npcList_.begin(),npcList_.end(),info.npcs_[i])==npcList_.end()){
			npcList_.push_back(info.npcs_[i]);
		}
	}

	s = myScene();
	if(s)
		needCleanCopyQuest = s->sceneType_ != SCT_Instance;
	
	if(needCleanCopyQuest){
		cleanCopyQuest();
	}
	visiblePlayers_.clear();
	SRV_ASSERT(s);
	s->addPlayer(this);
	info.sceneId_ = GET_SCENE_ORIGINAL_ID(info.sceneId_);
	CALL_CLIENT(this,joinScene(info));
}

void Player::getSceneInfo(COM_SceneInfo& info){
	isSceneLoaded_ = false;
	syncNpcs_.clear();
	Scene*s = myScene();
	if(s){
		info.sceneId_ = GET_SCENE_ORIGINAL_ID(sceneId_);
		info.position_ = position_;
		info.npcs_ = npcList_;
		//s->getPlayersInScene(this,syncScenePlayers_);
	}
}

void Player::fixEquipment(S32 instId, FixType type)
{
	const float duraPer = 0.8f;
	enum{		
		DURA_MoneyRatio = 10, //��Ǯ��Ϣ˥��ϵ��
		DURA_MoneyRatioMin = 60 , //��Ǯ������Сϵ��
		DURA_MoneyPay = 50,
	};

	COM_Item  *item = getEquipment(instId);
	if(item == NULL) return;
	if(((float)item->durability_ / (float)item->durabilityMax_) > duraPer )
		return;
	ItemTable::ItemData const* core = ItemTable::getItemById(item->itemId_);
	
	switch(type){
		case FT_Money:
			{
				int32 durabilityMax = item->durabilityMax_ - DURA_MoneyRatio;
				durabilityMax = durabilityMax < DURA_MoneyRatioMin ? DURA_MoneyRatioMin : durabilityMax;
				int32 needMoney = (durabilityMax-item->durability_) * DURA_MoneyPay;
				if(needMoney > getProp(PT_Money))
					return;
				addMoney(-needMoney);

				SGE_LogProduceTrack track;
				track.playerId_ = getGUID();
				track.playerName_ = getNameC();
				track.from_ = 15;
				track.money_ = -needMoney;
				LogHandler::instance()->playerTrack(track);
			
				if(item->durability_ <=0){
					addEquipmentEffect(item,1,false);
				}
				else if(item->durability_ < item->durabilityMax_ /2 ){
					addEquipmentEffect(item,0.5,false);
				}

				item->durability_ = item->durabilityMax_ = durabilityMax;
				CALL_CLIENT(this,requestFixItemOk(*item));
			}
			break;
		case FT_Diamond:
			{
				int needDiamond=(item->durabilityMax_ - item->durability_) * 1;
				if(needDiamond> getProp(PT_Diamond))
					return;		
				if(item->durability_ <=0){
					addEquipmentEffect(item,1,false);
				}
				else if(item->durability_ < item->durabilityMax_ /2 ){
					addEquipmentEffect(item,0.5,false);
				}
				item->durability_ = item->durabilityMax_;
				CALL_CLIENT(this,requestFixItemOk(*item));
			}
			break;
		default:
			return ;
	}
}

void Player::calcEquipmentDurability()
{
	U32 randn = UtlMath::randN(100);
	if(randn < Global::get<int>(C_EquipDurVar))
		return;

	for (size_t i=0; i<equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL)
			continue;
		if(equipItems_[i]->durability_ <= 0)
			return;
		int32 lastDurability = equipItems_[i]->durability_;
		equipItems_[i]->durability_ -= 1;

		//û���;ô���
		if(equipItems_[i]->durability_ <= 0){
			delEquipmentEffect(equipItems_[i],0.5,false);
			break;
		}
		
		int32 halfDurabilityMax = equipItems_[i]->durabilityMax_/2;
		//һ���;ô���
		if(lastDurability > halfDurabilityMax && equipItems_[i]->durability_ <= halfDurabilityMax){
			delEquipmentEffect(equipItems_[i],0.5,false);
			break;
		}
	}

	std::vector<COM_Item> cache;
	for(size_t i=0; i<equipItems_.size(); ++i)
	{
		if(equipItems_[i] != NULL)
			cache.push_back(*equipItems_[i]); 
	}
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("InitEquip Ok\n")));
	CALL_CLIENT(this,initPlayerEquips(cache));

}

void  Player::fixAllItem(std::vector< U32 >& items, FixType type)
{
	if(items.empty())
		return;
	for (size_t i = 0; i < items.size(); ++i)
		fixEquipment(items[i],type);			
}

void Player::makeDebirsItem(S32 instId,S32 num)
{
	COM_Item* item = getBagItemByInstId(instId);
	if(item == NULL)
		return ;

	ItemTable::ItemData const* boxItem  = ItemTable::getItemById(item->itemId_);
	if(boxItem->mainType_ != IMT_Debris)
		return ;

	DebrisTable::DebrisData const* debrisItem  = DebrisTable::getItemById(item->itemId_);
	if(debrisItem == NULL)
		return ;
	
	if(item->stack_ < debrisItem->needNum_)
		return ;
	
	if(debrisItem->subType_ == IST_ItemDebris)
		addBagItemByItemId(debrisItem->itemId_,debrisItem->itemNum_,false,12);
	else if(debrisItem->subType_ ==  IST_BabyDebris)
	{
		if(babies_.size() >= Global::get<int>(C_BabyMax))
		{
			CALL_CLIENT(this,errorno(EN_BabyFull));
			return;
		}
		addBaby(debrisItem->itemId_);
	}
	else if(debrisItem->subType_ == IST_EmployeeDebris)
	{
		if(employees_.size() >= EMPLOYEE_MAXNUM)
		{
			CALL_CLIENT(this,errorno(EN_EmployeeIsFull));
			return;
		}
		addEmployee(debrisItem->itemId_);
	}

	delBagItemByInstId(item->instId_,debrisItem->needNum_);
	CALL_CLIENT(this,makeDebirsItemOK());
}

void Player::levelUpMagicItem(std::vector< U32 >& items)
{
	int aExp = 0; 
	for(size_t i=0; i<items.size(); ++i)
	{
		COM_Item* item = getBagItemByInstId(items[i]);
		if(item != NULL)
		{
			ItemTable::ItemData const* artifactItem  = ItemTable::getItemById(item->itemId_);
			if(artifactItem == NULL)
				return;
			aExp += artifactItem->artifactVol_ ;
		}
	}

	if( getProp(PT_Money) < aExp * 100)
	{
		CALL_CLIENT(this,errorno(EN_MoneyLess));
		return;
	}	
	addMoney(-aExp * 100);

	SGE_LogProduceTrack track;
	track.playerId_ = getGUID();
	track.playerName_ = getNameC();
	track.from_ = 6;
	track.money_ = -aExp * 100;
	LogHandler::instance()->playerTrack(track);

	for(size_t i=0; i<items.size(); ++i)
		delBagItemByInstId(items[i],1);

	magicItemLevelUp(magicItemLevel_,aExp + magicItemExp_);
	refreshProperty();
	CALL_CLIENT(this,updateMagicItem(magicItemLevel_,magicItemExp_));	
}

void Player::magicItemLevelUp(S32 level,S32 exp)
{
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(level,JT_Axe);

	if(levelData == NULL)
		return;

	if(exp >= levelData->exp_ )
	{
		ArtifactLevelTable::ArtifactLevelData const* oldlevelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
		if(oldlevelData  == NULL)
			return;
		magicItemLevel_++;
		exp -=levelData->exp_ ;
		ArtifactLevelTable::ArtifactLevelData const* newlevelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
		if(newlevelData == NULL)
			return;
		//����I����
		int levelNum = (magicTupoLevel_-5)/5;
		float num= levelNum *0.1;
		if(oldlevelData != NULL)
		{
			for(size_t i=0;i<oldlevelData->propValue_.size();i++)
			{
				ArtifactLevelTable::ArtifactPropData oldprop =oldlevelData->propValue_[i];
				properties_[oldprop.type_] -= oldprop.value_ + oldprop.value_*num;
			//	setProp(oldprop.type_,properties_[oldprop.type_]);
			}
		}

		//����I����
		for(size_t i=0;i<newlevelData->propValue_.size();i++)
		{
			ArtifactLevelTable::ArtifactPropData prop =newlevelData->propValue_[i];
			properties_[prop.type_] += prop.value_ + prop.value_ *  num;
		//	setProp(prop.type_,properties_[prop.type_]);
		}
		magicItemLevelUp(magicItemLevel_,exp);

		GEParam params[1];
		params[0].type_ = GEP_INT;
		params[0].value_.i = magicItemLevel_;
		
		GameEvent::procGameEvent(GET_Shenqishengji,params,1,getHandleId());

	}
	else
		magicItemExp_ = exp;
}

void  Player::tupoMagicItem(S32 level)
{
	if(magicItemLevel_ != magicTupoLevel_)
		return;
	ArtifactConfigTable::ArtifactConfigData const* configData = ArtifactConfigTable::getArtifactById(magicTupoLevel_/5,JT_Axe);
	if(configData == NULL)
		return;
	  
	for(size_t i=0;i<configData->items_.size();i++){
		if(getItemNumByItemId(configData->items_[i].itemId_) < configData->items_[i].itemNum_){
			return;
		}
	}
	
	for(size_t i=0;i<configData->items_.size();i++){
		delBagItemByItemId(configData->items_[i].itemId_,configData->items_[i].itemNum_);
	}
	
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
	if(levelData == NULL)
		return;
	int tupoNum = magicTupoLevel_/5;
	int levelNum = (magicItemLevel_-5)/5;
	float num = tupoNum*0.1;
	float num1= levelNum*0.1;
	for(size_t i=0;i<levelData->propValue_.size();i++)
	{
		ArtifactLevelTable::ArtifactPropData prop =levelData->propValue_[i];
		properties_[prop.type_] -= prop.value_ + prop.value_ * num1;
		properties_[prop.type_] += prop.value_ + prop.value_ * num;
	}
	magicTupoLevel_ += 5;//configData->tupoLevel_;
	refreshProperty();
	CALL_CLIENT(this,magicItemTupoOk(magicTupoLevel_));
}

void Player::changeMagicJob(JobType job)
{
	return;
}

void Player::initMagicItemLevelUp()
{
	magicItemJob_ =JT_Axe;// (JobType)(S32)getProp(PT_Profession);
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
	if(levelData != NULL)
	{	
		for(size_t i=0;i<levelData ->propValue_.size();i++)
		{
			ArtifactLevelTable::ArtifactPropData prop =levelData->propValue_[i];
			float propV=  getProp(prop.type_);
			setProp(prop.type_,propV + prop.value_);
		}
	}
	CALL_CLIENT(this,changeMagicJobOk(JT_Axe));
}

void Player::magicItemOneKeyLevel()
{
	ArtifactLevelTable::ArtifactLevelData const* levelData = ArtifactLevelTable::getArtifactById(magicItemLevel_,JT_Axe);
	if(levelData == NULL)
		return;
	int needExp =levelData ->exp_ - magicItemExp_;
	if(needExp <=0)
		return;
	if( getProp(PT_MagicCurrency) < needExp)
	{
		CALL_CLIENT(this,errorno(EN_MagicCurrencyLess));
		return;
	}	
	addMagicCurrency(-needExp);
	magicItemLevelUp(magicItemLevel_,levelData ->exp_);
	CALL_CLIENT(this,updateMagicItem(magicItemLevel_,magicItemExp_));	
}

void
Player::errorMessageToC(ErrorNo errt)
{
	if(errt <= EN_None || errt >= EN_Max)
		return;
	CALL_CLIENT(this,errorno(errt));
}

void
Player::queryOnlinePlayer(std::string& name)
{
	bool isOnline = false;
	Player* player = getPlayerByName(name);
	if(player != NULL)
		isOnline = true;
	CALL_CLIENT(this,queryOnlinePlayerOK(isOnline));
}

void
Player::queryPlayerbyName(std::string& name)
{
	enum {
		W_QUERY_PLAYER_INFO = 1,
	};
	COM_ContactInfo* pContactInfo = WorldServ::instance()->findContactInfo(name);
	Robot* pRobot = EndlessStair::instance()->findRobotByName(name);
	if(pContactInfo == NULL && pRobot == NULL)
	{
		errorMessageToC(EN_CannotfindPlayer);
		return;
	}	
	
	if(pContactInfo)
		DBHandler::instance()->queryPlayerById(getNameC(),pContactInfo->instId_,W_QUERY_PLAYER_INFO);
	else if (pRobot)
	{
		COM_SimplePlayerInst inst;
		pRobot->getRobotInst(inst);
		CALL_CLIENT(this,queryPlayerOK(inst));
	}
	else
		errorMessageToC(EN_CannotfindPlayer);
}

void
Player::queryPlayerInstOK(SGE_DBPlayerData &data)
{
	//COM_SimplePlayerInst inst;
	//Player::transforDBPlayer2SimplePlayer(inst,data);
	////��ʱ����
	//CALL_CLIENT(this,queryPlayerOK(inst));
}

void
Player::queryBaby(U32 instid)
{
	DBHandler::instance()->queryBabyById(getNameC(),instid);
}

void Player::queryEmployee(U32 instid)
{ 
	DBHandler::instance()->queryEmployeeById(getNameC(),instid);
}

void Player::requestPK(U32 playerId){

	Player* otherPlayer = Player::getPlayerByInstId(playerId);

	if(NULL == otherPlayer)
		return;
	/*else if((!!myTeam()) != (!!(otherPlayer->myTeam()))){
		return ;
	}*/
	
	if(isInGuildBattleScene() && otherPlayer->isInGuildBattleScene()){
		//����սս��
		if(Guild::battleState_ != Guild::BS_Battle)
			return;
		Guild* pGuild = myGuild();
		Guild* pOtherGuild = otherPlayer->myGuild();
		if(pGuild == NULL || pOtherGuild == NULL || pGuild == pOtherGuild)
			return;
		if(pGuild->bInfo_ != pOtherGuild->bInfo_ || pGuild->bInfo_ == NULL)
			return;
		if(pGuild->bInfo_->hasWinner())
			return;
		if(noBattleTime_ > 0){
			CALL_CLIENT(this,errorno(EN_NoBattleTime));
			return;
		}else if(otherPlayer->noBattleTime_ > 0){
			CALL_CLIENT(this,errorno(EN_OtherNoBattleTime));
			return;
		}
		
		if(myTeam()  && otherPlayer->myTeam()){
			Battle* pBattle = Battle::getOneFree();
			if(isLeavingTeam_ && otherPlayer->isLeavingTeam_){
				pBattle->create(this,otherPlayer,BT_Guild);
			}else if(isLeavingTeam_){
				pBattle->create(this,otherPlayer->myTeam(),BT_Guild);
			}else if(otherPlayer->isLeavingTeam_){
				pBattle->create(otherPlayer,myTeam(),BT_Guild);
			}else{
				pBattle->create(myTeam(),otherPlayer->myTeam(),BT_Guild);
			}
			
		}
		else if(otherPlayer->myTeam()){
			Battle* pBattle = Battle::getOneFree();
			if(otherPlayer->isLeavingTeam_){
				pBattle->create(this,otherPlayer,BT_Guild);
			}else{
				pBattle->create(this,otherPlayer->myTeam(),BT_Guild);
			}
		}
		else if (myTeam() ){
			Battle* pBattle = Battle::getOneFree();
			pBattle->create(otherPlayer,myTeam(),BT_Guild);
			if(isLeavingTeam_){
				pBattle->create(this,otherPlayer,BT_Guild);
			}else{
				pBattle->create(otherPlayer,myTeam(),BT_Guild);
			}
		}
		else 
		{
			Battle* pBattle = Battle::getOneFree();
			pBattle->create(this,otherPlayer,BT_Guild);
		}
		
	}
	else{
		Battle* pBattle = Battle::getOneFree();
		pBattle->create(this,playerId);
	}
}

Guild* Player::myGuild(){
	return Guild::findGuildByPlayer(getGUID());
}
COM_Guild* Player::myGuildData(){
	Guild* pGuild = myGuild();
	if(pGuild)
		return &(pGuild->guildData_);
	return NULL;
}
COM_GuildMember* Player::myGuildMember(){
	Guild* pGuild = myGuild();
	if(pGuild)
		return pGuild->findMember(getGUID());
	return NULL;
}

void Player::updateMyGuildMember(){
	if(myGuildMember())
		CALL_CLIENT(this,updateGuildMyMember(*myGuildMember()));
}

bool Player::buyGuildItem(S32 tableId,S32 times){
	Guild* pGuild = myGuild();
	if(!pGuild)
		return true;
	COM_GuildMember* member = myGuildMember();
	if(NULL == member)
		return true;
	
	for(size_t i=0; i<member->shopItems_.size();++i){
		if(member->shopItems_[i].shopId_ == tableId)
		{
			COM_GuildShopItem &sitem = member->shopItems_[i];
			if(sitem.buyLimit_ <=0 )
				return true;
			sitem.buyLimit_ -= 1;
			const GuildShopItemData* p = GuildShopItemData::getShopItem(tableId);
			if(!p)
				return true;
			if(guildContribution_ < p->price_)
				return true;
			addGuildContribution(-p->price_);
			addBagItemByItemId(p->itemId_,p->itemStack_,false,11);
			CALL_CLIENT(this,updateGuildShopItems(member->shopItems_));
			return true;
		}
	}
	return true;
}

int32 Player::getGuildContribution(){
	return guildContribution_;
}

void Player::addGuildContribution(int32 val){
	guildContribution_ += val;
	if(guildContribution_ <0)
		guildContribution_ = 0;
	if(myGuildMember())
		myGuildMember()->contribution_ = guildContribution_;
	updateMyGuildMember();
	if(myGuild())
		myGuild()->broadcaster_.modifyGuildMemberList(*myGuildMember(),MLF_ChangeContribution);
	SGE_ContactInfoExt* p = WorldServ::instance()->findContactInfoExt(playerId_);
	if(p)
		p->guildContribute_ = guildContribution_;
	
}

int32 Player::getGuildBattleScene(){
	Guild* pGuild = myGuild();

	if(!pGuild)
		return 0;

	COM_GuildMember* pMember = myGuildMember();
	if(!pMember)
		return 0;
	return pGuild->battleSceneCopyId_;
}

bool Player::isInGuildScene(){
	return SceneTable::getGuildHomeScene()->sceneId_ == sceneId_;
}

bool Player::isInGuildBattleScene(){
	return getGuildBattleScene() == sceneId_;
}

void Player::joinGuildBattleScene(){
	
	if(!Guild::isBattlePrepare()){
		ACE_DEBUG((LM_ERROR,"Player join guile battle scene is not prepare\n"));
		if(Guild::isBattleOpen()){
			errorMessageToC(EN_GuildBattleIsStart);
		}else {
			errorMessageToC(EN_GuileBattleIsClose);
		}
		return;
	}	

	Guild* pGuild = myGuild();

	if(!pGuild)
	{
		ACE_DEBUG((LM_ERROR,"Player join guile battle scene can not has guild\n"));
		return ;
	}
	if(pGuild->bInfo_ == NULL){
		errorMessageToC(EN_GuildBattleNotMatch);
		return;
	}
	if(Guild::isBattlePrepare()){
		if(isInGuildBattleScene())
		{
			ACE_DEBUG((LM_ERROR,"Player join guile battle scene is in battle scene\n"));
			return;
		}
		Team* pTeam = myTeam();
		
		if(pTeam ){
			return ;
		}
		
		int32 scId = getGuildBattleScene();
		if(0 == scId)
		{
			ACE_DEBUG((LM_ERROR,"Player join guile battle scene can not find battle scene\n"));
			return;
		}
		scenePlayer_->transforSceneByEntry(scId,pGuild->isLeft_ ? 1 : 2);
	}
}

void
Player::exchangeGift(std::string code)
{
	WorldServ::instance()->reqCDKey(code,playerName_,gifttype_);
}

void
Player::giftaward(const std::string & giftName,std::vector<COM_GiftItem>& items,int32 diamond)
{
	if(std::find(gifttype_.begin(),gifttype_.end(),giftName) == gifttype_.end() )
		gifttype_.push_back(giftName);
	for (size_t i = 0; i < items.size(); ++i)
	{
		addBagItemByItemId(items[i].itemId_,items[i].itemNum_);
	}
	if(diamond){
		addMagicCurrency(diamond);
	}
	CALL_CLIENT(this,redemptionSpreeOk());
}

bool Player::eraseSyncPlayer(S32 playerId){
	
	return false;
}

COM_Skill* Player::findGuildSkill(int32 skId){
	for(size_t i=0; i<guildSkills_.size(); ++i){
		if(guildSkills_[i].skillID_ == skId)
			return &guildSkills_[i];
	}
	return NULL;
}

void Player::cleanGuildInfomation(){
	if (int(getProp(PT_GuildID)) != 0)
	{
		if(sceneId_ == SceneTable::getGuildHomeScene()->sceneId_)
			transforHome();
		cleanGuildQuest();
		setProp(PT_GuildID,0);
		CALL_CLIENT(this,delGuildOK());
	}
}

void Player::refreshOnlineReward(){
	onlinereward_.clear();
	onlinetime_ = 0;
	onlinetimeflag_ = true;
	CALL_CLIENT(this,agencyActivity(ADT_OnlineReward, onlinetimeflag_));
}

void Player::requestOnlineReward(U32 index){
	for (size_t i = 0; i < onlinereward_.size(); ++i)
	{
		if(onlinereward_[i] == index)
			return;
	}
	OnlineTimeClass::onlinetimedata const* co = OnlineTimeClass::getonlinereward(index);
	if(co == NULL)
		return;
	if(onlinetime_ < co->targettime_)
		return;
	for (size_t i=0; i<co->rewards_.size();++i)
	{
		addBagItemByItemId(co->rewards_[i],co->reNum_[i],false,16);
	}
	onlinereward_.push_back(index);
	if(OnlineTimeClass::getMaxIndex() == onlinereward_.size()){
		onlinetimeflag_ = false;
		CALL_CLIENT(this,agencyActivity(ADT_OnlineReward, onlinetimeflag_));
	}
	CALL_CLIENT(this,requestOnlineTimeRewardOK(index));
}

void Player::buyFund(U32 level){
	if(!isFund_){
		return;
	}
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	if(level > getProp(PT_Level))
		return;
	for (size_t i = 0; i < fundtag_.size(); ++i){
		if(fundtag_[i] == level)
			return;
	}
	GrowFundTable::GrowFundData const* co = GrowFundTable::getDataByLevel(level);
	if(co == NULL)
		return;
	addBagItemByItemId(co->item_,co->itemNum_,false,16);
	fundtag_.push_back(level);
	
	CALL_CLIENT(this,requestFundRewardOK(level));
}

void Player::openCard(U16 index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	if(selfCards_.contents_.size() >= Global::get<int>(C_OpenCardMax))
		return;
	for (size_t i = 0; i < selfCards_.contents_.size(); ++i){
		if(selfCards_.contents_[i].count_ == index)
			return;
	}
	if(index > Global::get<int>(C_OpenCardMax) || index < 0)
		return;
	U32	ucount = selfCards_.contents_.size();
	ucount += 1;
	ReversalCard::Cardsper const * core = ReversalCard::getcardsperbytime(ucount);
	if(core == NULL)
		return;
	U32 neednum = getItemNumByItemId(Global::get<int>(C_OpenCardNeedItem));
	if(neednum < core->cost_)
		return;
	COM_ADCardsContent data;
	U32 roll = UtlMath::randN(100);
	if(roll < core->high_){
		ReversalCard::CardReward const * pc = ReversalCard::getrewardhigh(this);
		if(pc == NULL)
			return;
		data.count_ = index;
		data.rewardId_ = pc->id_;
		selfCards_.contents_.push_back(data);
		addBagItemByItemId(pc->itemid_,pc->itemnum_,false,983);
		delBagItemByItemId(Global::get<int>(C_OpenCardNeedItem),core->cost_);
		CALL_CLIENT(this,openCardOK(data));
		return;
	}
	if(roll < core->middle_){
		ReversalCard::CardReward const * pc = ReversalCard::getrewardmiddle(this);
		if(pc == NULL)
			return;
		data.count_ = index;
		data.rewardId_ = pc->id_;
		selfCards_.contents_.push_back(data);
		addBagItemByItemId(pc->itemid_,pc->itemnum_,false,983);
		delBagItemByItemId(Global::get<int>(C_OpenCardNeedItem),core->cost_);
		CALL_CLIENT(this,openCardOK(data));
		return;
	}
	
	ReversalCard::CardReward const * pc = ReversalCard::getrewardlow(this);
	if(pc == NULL)
		return;
	data.count_ = index;
	data.rewardId_ = pc->id_;
	selfCards_.contents_.push_back(data);
	addBagItemByItemId(pc->itemid_,pc->itemnum_,false,983);
	delBagItemByItemId(Global::get<int>(C_OpenCardNeedItem),core->cost_,983);
	CALL_CLIENT(this,openCardOK(data));
	
}

void Player::resetCard(bool ispasszero){
	/*if(!ReversalCard::isOpen_)
		return;*/
	if(!ispasszero)
	{
		U32 neednum = getItemNumByItemId(Global::get<int>(C_OpenCardNeedItem));
		if(neednum < Global::get<int>(C_ResetCardNeedItemNum))
			return;
		delBagItemByItemId(Global::get<int>(C_OpenCardNeedItem),Global::get<int>(C_ResetCardNeedItemNum));
	}
	selfCards_.contents_.clear();
	CALL_CLIENT(this,resetCardOK());
}

bool Player::hasCardReward(U32 id){
	for (size_t i =0; i < selfCards_.contents_.size(); ++i)
	{
		if(selfCards_.contents_[i].rewardId_ == id)
			return true;
	}
	return false;
}

void Player::updateFestival(){
	if(festival_.contents_.empty())
		return;
	
	for(size_t i=0; i<festival_.contents_.size(); ++i){
		if(festival_.contents_[i].status_ == 0 && festival_.contents_[i].totalDays_ <= festival_.loginDays_){
				festival_.contents_[i].status_ = 1;
		}
	}
	CALL_CLIENT(this,updateFestival(festival_));
}

void Player::requestFestival(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}
	if(index < 0 || index >= festival_.contents_.size())
		return;
	if(festival_.contents_[index].status_ != 1)
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < festival_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	festival_.contents_[index].status_ = 2;

	for(size_t i=0; i<festival_.contents_[index].itemIds_.size(); ++i){
		addBagItemByItemId(festival_.contents_[index].itemIds_[i],festival_.contents_[index].itemStacks_[i],false,997);
	}

	CALL_CLIENT(this,updateFestival(festival_));
}

void Player::updateSelfRecharge(){
	if(selfRecharge_.contents_.empty())
		return;
	for(size_t i=0; i<selfRecharge_.contents_.size(); ++i){
		if(selfRecharge_.contents_[i].status_ == 0 && selfRecharge_.recharge_ >=selfRecharge_.contents_[i].currencyCount_ ){
			selfRecharge_.contents_[i].status_ = 1;
		}
	}

	CALL_CLIENT(this,updateSelfRecharge(selfRecharge_));
}

void Player::requestSelfRecharge(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		errorMessageToC(EN_BagFull);
		return;
	}
	if(index < 0 || index >= selfRecharge_.contents_.size())
		return;
	if(selfRecharge_.contents_[index].status_!=1)
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < selfRecharge_.contents_[index].itemIds_.size())
	{
		errorMessageToC(EN_BagFull);
		return;
	}

	selfRecharge_.contents_[index].status_ = 2;

	for(size_t i=0; i<selfRecharge_.contents_[index].itemIds_.size(); ++i){
		addBagItemByItemId(selfRecharge_.contents_[index].itemIds_[i],selfRecharge_.contents_[index].itemStacks_[i],false,998);
	}
	
	CALL_CLIENT(this,updateSelfRecharge(selfRecharge_));
}

void Player::updateSysRecharge(){
	if(sysRecharge_.contents_.empty())
		return;
	for(size_t i=0; i<sysRecharge_.contents_.size(); ++i){
		if(sysRecharge_.contents_[i].status_ == 0 && sysRecharge_.recharge_ >=sysRecharge_.contents_[i].currencyCount_){
			sysRecharge_.contents_[i].status_ = 1;
		}
	}

	CALL_CLIENT(this,updateSysRecharge(sysRecharge_));
}

void Player::requestSysRecharge(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}
	if(index < 0 || index >= sysRecharge_.contents_.size())
		return;
	if(sysRecharge_.contents_[index].status_!=1)
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < sysRecharge_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	sysRecharge_.contents_[index].status_ = 2;

	for(size_t i=0; i<sysRecharge_.contents_[index].itemIds_.size(); ++i){
		addBagItemByItemId(sysRecharge_.contents_[index].itemIds_[i],sysRecharge_.contents_[index].itemStacks_[i],false,996);
	}

	CALL_CLIENT(this,updateSysRecharge(sysRecharge_));
}

void Player::updateSelfDiscountStore(){
	if(selfDiscountStore_.contents_.empty())
		return;
	CALL_CLIENT(this,updateSelfDiscountStore(selfDiscountStore_));
}

void Player::buySelfDiscountStore(int itemId, int buyStack){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0){
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}
	if(selfDiscountStore_.contents_.empty())
		return;
	if(buyStack <= 0)
		return;

	for(size_t i=0; i<selfDiscountStore_.contents_.size(); ++i){
		if(selfDiscountStore_.contents_[i].itemId_ == itemId){
			if(selfDiscountStore_.contents_[i].buyLimit_ < buyStack){
				errorMessageToC(EN_DisShopLimitLess);
				return;
			}
			int32 num = selfDiscountStore_.contents_[i].price_ * selfDiscountStore_.contents_[i].discount_* buyStack;
			if(num > getProp(PT_MagicCurrency)){
				errorMessageToC(EN_MagicCurrencyLess);
				return;
			}
			addMagicCurrency(-num);
			selfDiscountStore_.contents_[i].buyLimit_  -= buyStack;
			addBagItemByItemId(itemId,buyStack,false,995);
			CALL_CLIENT(this,updateSelfDiscountStore(selfDiscountStore_));
			return;
		}
	}
}

void Player::updateSysDiscountStore(){
	if(sysDiscountStore_.contents_.empty())
		return;
	CALL_CLIENT(this,updateSelfDiscountStore(sysDiscountStore_));
}

void Player::buySysDiscountStore(int itemId, int buyStack){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		errorMessageToC(EN_BagFull);
		return;
	}
	if(sysDiscountStore_.contents_.empty())
		return;
	if(buyStack <= 0)
		return;
	
	for(size_t i=0; i<sysDiscountStore_.contents_.size(); ++i){
		if(sysDiscountStore_.contents_[i].itemId_ == itemId){
			if(sysDiscountStore_.contents_[i].buyLimit_ < buyStack){
				errorMessageToC(EN_DisShopLimitLess);
				return;
			}
			int32 num = sysDiscountStore_.contents_[i].price_ * sysDiscountStore_.contents_[i].discount_ * buyStack ;
			if(num > getProp(PT_MagicCurrency)){
				errorMessageToC(EN_MagicCurrencyLess);
				return;
			}
			addMagicCurrency(-num);
			sysDiscountStore_.contents_[i].buyLimit_  -= buyStack;
			addBagItemByItemId(itemId,buyStack,false,993);
			CALL_CLIENT(this,updateSysDiscountStore(sysDiscountStore_));
			return;
		}
	}
}

void Player::updateSelfOnceRecharge(int money){
	if(selfOnceRecharge_.contents_.empty())
		return;
	for(size_t i=0; i<selfOnceRecharge_.contents_.size(); ++i){
		if(selfOnceRecharge_.contents_[i].status_ == 0 && selfOnceRecharge_.contents_[i].currencyCount_ == money){
			selfOnceRecharge_.contents_[i].status_ = 1;
			CALL_CLIENT(this,updateSelfOnceRecharge(selfOnceRecharge_));
			return;
		}
	}
}

void Player::requestSelfOnceRecharge(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}
	if(selfOnceRecharge_.contents_.empty())
		return;
	if(index<0 || index >= selfOnceRecharge_.contents_.size())
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < selfOnceRecharge_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	if(selfOnceRecharge_.contents_[index].status_ == 1){
		selfOnceRecharge_.contents_[index].status_ = 2;
		for(size_t i=0; i<selfOnceRecharge_.contents_[index].itemIds_.size();++i){
			addBagItemByItemId(selfOnceRecharge_.contents_[index].itemIds_[i],selfOnceRecharge_.contents_[index].itemStacks_[i],false,992);
		}
		CALL_CLIENT(this,updateSelfOnceRecharge(selfOnceRecharge_));
	}
}

void Player::updateSysOnceRecharge(int money){
	if(sysOnceRecharge_.contents_.empty())
		return;
	for(size_t i=0; i<sysOnceRecharge_.contents_.size(); ++i){
		if(sysOnceRecharge_.contents_[i].status_ == 0 && sysOnceRecharge_.contents_[i].currencyCount_ == money){
			sysOnceRecharge_.contents_[i].status_ = 1;
			CALL_CLIENT(this,updateSysOnceRecharge(sysOnceRecharge_));
			return;
		}
	}
}

void Player::requestSysOnceRecharge(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}
	if(sysOnceRecharge_.contents_.empty())
		return;
	if(index<0 || index >= sysOnceRecharge_.contents_.size())
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < sysOnceRecharge_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	if(sysOnceRecharge_.contents_[index].status_ == 1){
		sysOnceRecharge_.contents_[index].status_ = 2;
		for(size_t i=0; i<sysOnceRecharge_.contents_[index].itemIds_.size();++i){
			addBagItemByItemId(sysOnceRecharge_.contents_[index].itemIds_[i],sysOnceRecharge_.contents_[index].itemStacks_[i],false,991);
		}
		CALL_CLIENT(this,updateSysOnceRecharge(sysOnceRecharge_));
	}
}
/////////////////////////////////////////////////////////////////////////////////////////////////
void Player::hotRoleBuy(){
	for (size_t i=0;i<hotdata_.contents_.size();++i)
	{
		if(hotdata_.contents_[i].buyNum_ <= 0)
			return;
		if(getProp(PT_MagicCurrency) < hotdata_.contents_[i].price_)
			return;
		if(hotdata_.contents_[i].type_ == ET_Emplyee){
			addEmployee(hotdata_.contents_[i].roleId_);
		}
		else if(hotdata_.contents_[i].type_ == ET_Baby){
			if(babies_.size() >= Global::get<int>(C_BabyMax))
				return;
			addBaby(hotdata_.contents_[i].roleId_);
		}
		hotdata_.contents_[i].buyNum_ -=1;
		addMagicCurrency(-(int32)hotdata_.contents_[i].price_);
		CALL_CLIENT(this,hotRoleBuyOk(hotdata_.contents_[i].buyNum_));
		break;
	}
}

////////////////////////////////////////////////////////////////////////////
void Player::updateEmployeeActivity(bool isflag){
	if(empact_.contents_.empty())
		return;
	if(isflag)
		empact_.outputNum_ += 1;
	for(size_t i=0; i<empact_.contents_.size(); ++i){
		if(empact_.contents_[i].status_ == 0 && empact_.outputNum_ >=empact_.contents_[i].outputCount_){
			empact_.contents_[i].status_ = 1;
		}
	}

	CALL_CLIENT(this,updateEmployeeActivity(empact_));
}

void Player::requestEmployeeActivity(int index){
	S32 emptySlot_ = getFirstEmptySlot();
	if(emptySlot_ < 0)
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	if(index < 0 || index >= empact_.contents_.size())
		return;
	if(empact_.contents_[index].status_!=1)
		return;

	S32 emptySlot = getBagEmptySlot();
	if(emptySlot < empact_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	empact_.contents_[index].status_ = 2;

	for(size_t i=0; i<empact_.contents_[index].itemIds_.size(); ++i){
		addBagItemByItemId(empact_.contents_[index].itemIds_[i],empact_.contents_[index].itemStacks_[i],false,990);
	}

	CALL_CLIENT(this,updateEmployeeActivity(empact_));
}



void Player::verificationSMS(std::string phoneNumber,std::string code){

	//WorldServ::instance()->prepareVerificationCode(phoneNumber,playerId_);
	if(code.empty()){
		if(phoneNumber.empty()){
			return;
		}
		if (!phoneNumber_.empty()){
			errorMessageToC(EN_PhoneNumberError);
			return ;
		}
		if(WorldServ::instance()->smsTimeout_ < WorldServ::instance()->curTime_ - smsTime_){
			WorldServ::instance()->prepareVerificationCode(phoneNumber,playerId_);
			return;
		}
	}
	else{
		if(phoneNumber_ == phoneNumber ){
			if( code == smsCode_){
				if(account_){
					account_->phoneNumber_ = phoneNumber_;
					phoneNumber_.clear();
					CALL_CLIENT(this,verificationSMSOk(phoneNumber_));
					errorMessageToC(EN_PboneNumberSuccess);
					GameEvent::procGameEvent(GET_PhoneNumber,0,0,getHandleId());
					LoginHandler::instance()->setPhoneNumber(account_->username_,account_->phoneNumber_);

				}
			}
			else {
				errorMessageToC(EN_PhoneNumberError);
			}
			// success 
		}else {
			errorMessageToC(EN_PhoneNumberError);
		}
	}
}

//
void Player::vipreward(){
	if(getProp(PT_VipTime) <= 0)
		return;
	if(!viprewardflag_)
		return;
	U32 itemId_ = 0;
	U32 itemStack_ = 0;
	if(getProp(PT_VipLevel) == VL_1){
		itemId_ = Global::get<int>(C_Vip1Reward);
		itemStack_ = Global::get<int>(C_Vip1RewardNum);
	}else if(getProp(PT_VipLevel) == VL_2){
		itemId_ = Global::get<int>(C_Vip2Reward);
		itemStack_ = Global::get<int>(C_Vip2RewardNum);
	}else
		return;
	if(calcEmptyItemNum(itemId_) < itemStack_){
		errorMessageToC(EN_BagFull);
		return;
	}
	addBagItemByItemId(itemId_,itemStack_,false,989);
	viprewardflag_ = false;
	CALL_CLIENT(this,sycnVipflag(viprewardflag_));
}

void Player::updateIntegral(U32 cont){
	if(!icdata_.isflag_ && cont == 0)
	{
		icdata_.integral_ += Global::get<int>(C_EverydayIntegral);
		icdata_.isflag_ = true;
	}	
	if(cont != 0)
		icdata_.integral_ += cont*Global::get<int>(C_IntegralRatio);
	updateIntegralShop();
}

void Player::integralShopBuy(U32 id, U32 num){
	if(icdata_.contents_.empty())
		return;
	if(num == 0)
		return;
	for (size_t i=0; i < icdata_.contents_.size(); ++i)
	{
		if(icdata_.contents_[i].id_ == id)
		{
			ItemTable::ItemData const * data = ItemTable::getItemById(icdata_.contents_[i].itemid_);
			if(NULL == data)
				return;
			if(icdata_.contents_[i].times_ <= 0)
				return;
			if(icdata_.contents_[i].times_ < num)
				return;
			U32 cost = icdata_.contents_[i].cost_*num;
			if(icdata_.integral_ < cost)
				return;
			icdata_.contents_[i].times_ -= num;
			if(icdata_.contents_[i].times_ < 0)
				icdata_.contents_[i].times_ = 0;
			icdata_.integral_ -= cost;
			if(icdata_.integral_ < 0)
				icdata_.integral_ = 0;
			addBagItemByItemId(icdata_.contents_[i].itemid_, num,false,987);
		} 
	}
	updateIntegralShop();
}

void Player::resetIntegralState(){
	if(icdata_.contents_.empty())
		return;
	if(icdata_.isflag_)
		icdata_.isflag_ = false;
	updateIntegralShop();
}

void Player::updateIntegralShop(){
	if(icdata_.contents_.empty())
		return;
	CALL_CLIENT(this,updateIntegralShop(icdata_));
}

void Player::updatemySelfRecharge(){
	if(myselfrecharge_.contents_.empty())
		return;
	for(size_t i=0; i<myselfrecharge_.contents_.size(); ++i){
		if(myselfrecharge_.contents_[i].status_ == 0 && myselfrecharge_.recharge_ >=myselfrecharge_.contents_[i].currencyCount_ ){
			myselfrecharge_.contents_[i].status_ = 1;
		}
	}

	CALL_CLIENT(this,updateMySelfRecharge(myselfrecharge_));
}

void Player::requestmySelfRecharge(int index){
	if(index < 0 || index >= myselfrecharge_.contents_.size())
		return;
	if(myselfrecharge_.contents_[index].status_!=1)
		return;

	S32 emptySlot_ = getBagEmptySlot();
	if(emptySlot_ < myselfrecharge_.contents_[index].itemIds_.size())
	{
		//ACE_DEBUG((LM_INFO,ACE_TEXT("Bag Full\n")));
		return;
	}

	myselfrecharge_.contents_[index].status_ = 2;

	for(size_t i=0; i<myselfrecharge_.contents_[index].itemIds_.size(); ++i){
		addBagItemByItemId(myselfrecharge_.contents_[index].itemIds_[i],myselfrecharge_.contents_[index].itemStacks_[i],false,986);
	}

	CALL_CLIENT(this,updateMySelfRecharge(myselfrecharge_));
}

void Player::requestLevelGift(U32 level){
	if(level > getProp(PT_Level))
		return;
	for (size_t i = 0; i<levelgift_.size(); ++i)
	{
		if(levelgift_[i] == level)
			return;
	}
	LevelGift::LevelGiftContent const* core = LevelGift::get(level);
	if(core == NULL)
		return;
	U32 emptyslot = getBagEmptySlot();

	if(emptyslot < core->reward_.size()){
		errorMessageToC(EN_BagFull);
		return;
	}
	for (size_t i=0; i<core->reward_.size();++i)
	{
		addBagItemByItemId(core->reward_[i].first,core->reward_[i].second,false,16);
	}
	levelgift_.push_back(level);
	CALL_CLIENT(this,requestLevelGiftOK(level));
}

bool Player::updateMinGiftActivity(int32 shopId){
	if(shopId)
	{
		if(!MinGift::isOpen_){
			ACE_DEBUG((LM_ERROR,"MinGift::isOpen_ == false \n"));
			return false;
		}
		if(getBagEmptySlot() < gbact_.itemdata_.size()){
			//errorMessageToC(EN_BagFull);
						
			std::vector<COM_MailItem> items;
			for(size_t i=0 ;i <gbact_.itemdata_.size();++i){
				COM_MailItem item;
				item.itemId_ = gbact_.itemdata_[i].itemId_;
				item.itemStack_ = gbact_.itemdata_[i].itemNum_;
				items.push_back(item);
			}
			const Shop::Record* record= Shop::getRecordById(shopId);
			std::string sender = Global::get<std::string>(C_ShopMailSender);
			std::string content = Global::get<std::string>(C_ShopMailContent);
			std::string title = "";
			if(NULL == record){
				title = record->id_;
			}else {
				title = record->name_;
			}
			
			WorldServ::instance()->sendMail(sender,playerName_,title,content,0,0,items);
			ACE_DEBUG((LM_INFO,"Send mini gift buy mail\n"));
		}else {
			for (size_t i=0; i<gbact_.itemdata_.size();++i)
			{
				addBagItemByItemId(gbact_.itemdata_[i].itemId_,gbact_.itemdata_[i].itemNum_,false,1);
			}
		}
		gbact_.isflag_ = true;
	}
	CALL_CLIENT(this,updateMinGiftActivity(gbact_));
	return true;
}

void Player::wearFuwen(int32 itemInstId){

	COM_Item* pItem = getBagItemByInstId(itemInstId);
	if(NULL == pItem)
		return;
	ItemTable::ItemData const* pData = ItemTable::getItemById(pItem->itemId_);
	if(NULL == pData)
		return;
	if(pData->mainType_ != IMT_FuWen){
		return ;
	}

	int32 slot = (pData->subType_ - IST_FuWenAttack)*2;
	
	if(NULL == fuwen_[slot]){
		//slot = slot
	}else if(NULL == fuwen_[slot+1]){
		slot += 1;
	}else{
		int32 bslot = getFirstEmptySlot();
		if(-1 == bslot){
			int32 sum = getItemNumByItemId(pItem->itemId_);
			if(sum % pData->maxCount_ == 0)
			{
				errorMessageToC(EN_BagFull);
				return;
			}
		}
		///�滻��λ1
		takeoffFuwen(slot);
	}
	
	fuwen_[slot] = NEW_MEM(COM_Item,*pItem);
	fuwen_[slot]->slot_ = slot;
	fuwen_[slot]->stack_ = 1;
	CALL_CLIENT(this,wearFuwenOk(*fuwen_[slot]));
	delBagItemByInstId(itemInstId,1,18);
	Entity::addEquipmentEffect(fuwen_[slot]);
	//
	U32 alllevel = 0;
	for (size_t i=0;i<fuwen_.size(); ++i)
	{
		if(fuwen_[i] == NULL)
			continue;
		ItemTable::ItemData const* pData = ItemTable::getItemById(fuwen_[i]->itemId_);
		if(NULL == pData)
			return;
		alllevel += pData->level_;
	}

	enum {
		ARG0,			//���������ܵȼ�
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG0].type_  = GEP_INT;
	param[ARG0].value_.i = alllevel;
	GameEvent::procGameEvent(GET_WearFuwen,param,ARG_MAX_,handleId_);
}

void Player::takeoffFuwen(int32 slotId){
	enum{ FUWEN_SLOT_MAX = (IST_FuWenAssist - IST_Fashion) * 2};
	if(slotId < 0)
		return;
	if(slotId >= FUWEN_SLOT_MAX)
		return;
	if(NULL == fuwen_[slotId])
		return;

	COM_Item *pItem = fuwen_[slotId];
	int32 slot = getFirstEmptySlot();
	if(-1 == slot){
		int32 sum = getItemNumByItemId(pItem->itemId_);
		ItemTable::ItemData  const* pData = ItemTable::getItemById(pItem->itemId_);
		if(sum % pData->maxCount_ == 0)
		{
			errorMessageToC(EN_BagFull);
			return;
		}
	}
	fuwen_[slotId] = NULL;
	CALL_CLIENT(this,takeoffFuwenOk(slotId));
	addBagItemByItemId(pItem->itemId_,1,pItem->isLock_,18);

	Entity::delEquipmentEffect(pItem);
	DEL_MEM(pItem);
	///ж������

}

void Player::compFuwen(int32 itemInstId){
	
	COM_Item* pItem = getBagItemByInstId(itemInstId);
	if(NULL == pItem)
		return;
	ItemTable::ItemData const* pData = ItemTable::getItemById(pItem->itemId_);
	if(NULL == pData)
		return;
	if(pData->mainType_ != IMT_FuWen){
		return ;
	}
	if(getItemNumByItemId(pData->id_)< Global::get<int>(C_CompRunesNeedNum)){
		return; ///�ϳɵ�����������
	}
	RuneTable::Data const* pRuneData = RuneTable::getDataById(pData->id_);
	if(NULL == pRuneData)
		return;
	if(pRuneData->needItem_.first != 0){
		if(getItemNumByItemId(pRuneData->needItem_.first) < pRuneData->needItem_.second){
			return ;//��Ҫ���ӵ�����������
		}
	}
	
	int32 slot = getFirstEmptySlot();
	if(-1 == slot){
		int32 sum = getItemNumByItemId(pRuneData->resultItemId_);
		ItemTable::ItemData  const* pData = ItemTable::getItemById(pRuneData->resultItemId_);
		if(sum % pData->maxCount_ == 0)
		{
			errorMessageToC(EN_BagFull);
			return;
		}
	}

	if(pItem->stack_ > Global::get<int>(C_CompRunesNeedNum)){
		delBagItemByInstId(pItem->instId_,Global::get<int>(C_CompRunesNeedNum),18);
	}else{
		int32 loseNum =  Global::get<int>(C_CompRunesNeedNum) - pItem->stack_;
		delBagItemByInstId(pItem->instId_,pItem->stack_,18); ///��仰֮��Ͳ���ʹ��pItem
		delBagItemByItemId(pData->id_,loseNum);
	}
	if(pRuneData->needItem_.first != 0){
		delBagItemByItemId(pRuneData->needItem_.first,pRuneData->needItem_.second);
	}
	addBagItemByItemId(pRuneData->resultItemId_,1,false,18);
	CALL_CLIENT(this,compFuwenOk());
}

void Player::requestEmployeeQuest(){
	//ACE_DEBUG((LM_INFO,"Request employee list\n"));
	std::vector<COM_EmployeeQuestInst> questList = EmployeeQuestSystem::GetQuestList(getGUID());
	CALL_CLIENT(this,requestEmployeeQuestOk(questList));
}

void Player::acceptEmployeeQuest(int32 questId, std::vector<int32> const& employees){
	EmployeeQuest const* p = EmployeeQuest::getQuest(questId);
	if(!p)
		return;
	if(!EmployeeQuestSystem::IsHasEmployeeQuest(getGUID(),questId)){
		return; //û���������
	}
	if(employees.empty()){
		return ;//û��Ӷ��
	}
	//for(size_t i=0; i<employees.size(); ++i){
	//	if( std::find(battleEmpsGroup1_.begin(),battleEmpsGroup1_.end(),employees[i]) != battleEmpsGroup1_.end()){
	//		return;//������Ӷ��
	//	}
	//	if( std::find(battleEmpsGroup2_.begin(),battleEmpsGroup2_.end(),employees[i]) != battleEmpsGroup2_.end()){
	//		return;//������Ӷ��
	//	}
	//}
	//���������
	COM_EmployeeQuestInst out;
	if(EmployeeQuestSystem::TryAcceptEmployeeQuest(getGUID(),questId,employees,out)){
		addMoney(-p->cost_);
		CALL_CLIENT(this,acceptEmployeeQuestOk(out));
	}
}

void Player::submitEmployeeQuest(int32 questId){
	if(!EmployeeQuestSystem::IsEmployeeQuestComplate(getGUID(),questId)){
		return ; ///û�������
	}
	bool isSuccess = EmployeeQuestSystem::RemoveComplateQuest(getGUID(),questId);
	if(isSuccess){
		//������
	}
	CALL_CLIENT(this,submitEmployeeQuestOk(questId,isSuccess));
}

void Player::initCrystal(){
	COM_CrystalProp cp;
	CrystalTable::randupprop(cp);
	if(cp.level_ == 0)
		return;
	crystalData_.props_.push_back(cp);
	crystalData_.level_ = crystalData_.props_.size();
	crystalUpdata(true);
	CALL_CLIENT(this,sycnCrystal(crystalData_));
}

void Player::crystalUpdata(bool isflag){
	if(!getOpenSubSystemFlag(OSSF_Cystal))
		return;
	if(isflag)
	{
		for (size_t i = 0; i < crystalData_.props_.size(); ++i)
		{
			if(crystalData_.props_[i].type_ < PT_None || crystalData_.props_[i].type_ > PT_Max)
				continue;
			float val = getProp(crystalData_.props_[i].type_);
			float cur = val + crystalData_.props_[i].val_;
			setProp(crystalData_.props_[i].type_,cur);
		}
	}
	else
	{	
		for (size_t i = 0; i < crystalData_.props_.size(); ++i)
		{
			if(crystalData_.props_[i].type_ < PT_None || crystalData_.props_[i].type_ > PT_Max)
				continue;
			float val = getProp(crystalData_.props_[i].type_);
			float cur = val - crystalData_.props_[i].val_;
			if(cur < 0.F)
			{
				ACE_DEBUG( (LM_DEBUG, ACE_TEXT("crystalUpdata error proptype[%d] cur < 0.F\n"),(int)crystalData_.props_[i].type_ ) );
				continue;
			}
			setProp(crystalData_.props_[i].type_,cur);
		}
	}
}

void Player::crystalUp(){
	U32 nextlevel = crystalData_.level_ + 1;
	CrystalUpTable::CrystalUpData const* cptable = CrystalUpTable::getdata(nextlevel);
	if(cptable == NULL)
		return;
	U32 needitem = Global::get<int>(C_CrystalNeedItem);
	U32 itemnum = getItemNumByItemId(needitem);
	if(itemnum < cptable->neednum_)
		return;
	if(getProp(PT_Money) < cptable->needgold_)
		return;
	U32 prob = UtlMath::randN(100);
	bool isOK = true;
	if(prob > cptable->prob_)
		isOK = false;
	delBagItemByItemId(needitem,cptable->neednum_);
	float curmoney = getProp(PT_Money) - cptable->needgold_;
	setProp(PT_Money,curmoney);
	COM_CrystalProp cp;
	CrystalTable::randupprop(cp);
	if(cp.level_ == 0){
		CALL_CLIENT(this,crystalUpLeveResult(false));
		return;
	}
	if(isOK)
	{
		CALL_CLIENT(this,crystalUpLeveResult(isOK));
		
		float curprop = getProp(cp.type_) + cp.val_;
		setProp(cp.type_,curprop);
		
		crystalData_.props_.push_back(cp);
		crystalData_.level_ = crystalData_.props_.size();
		CALL_CLIENT(this,sycnCrystal(crystalData_));
	}
	else
		CALL_CLIENT(this,crystalUpLeveResult(isOK));
}

void Player::resetCrystalProp(std::vector<int32> locks){
	
	if(locks.size() >= crystalData_.props_.size())
		return;
	for (size_t i = 0; i < locks.size(); ++i)
	{
		if(locks[i]>=crystalData_.props_.size())
			return;
	}

	CALL_CLIENT(this,resetCrystalPropOK());

	int32 need = Global::get<int>(C_CrystalNeedDiamond)*UtlMath::pow(2,locks.size());
	float diamond = getProp(PT_Diamond);
	if(need > diamond)
		return;
	addDiamond(-need);
	crystalUpdata(false);
	for (size_t i = 0; i < crystalData_.props_.size(); ++i)
	{
		bool islock = false;
		for (size_t j = 0; j < locks.size(); ++j)
		{
			if(i == locks[j])
			{
				islock = true;
			}
		}
		if(!islock)
		{
			COM_CrystalProp cp;
			CrystalTable::randresetprop(cp);
			crystalData_.props_[i] = cp;
		}
	}
	crystalUpdata(true);
	CALL_CLIENT(this,sycnCrystal(crystalData_));
}

void Player::setOpenSubSystemFlag(OpenSubSystemFlag ossf)
{
	openSubSystemFlag_ |= ((uint64)0x1L << ossf);
	CALL_CLIENT(this,syncOpenSystemFlag(openSubSystemFlag_));
}
bool Player::getOpenSubSystemFlag(OpenSubSystemFlag ossf)
{
	return !!(openSubSystemFlag_&( (uint64)0x1L << ossf));
}

void Player::addCourseGift(){
	U32 level = getProp(PT_Level);
	CourseGiftTable::CourseGiftContent const* core = CourseGiftTable::getbylevel(level);
	if(core == NULL)
		return;
	COM_CourseGift gift;
	gift.id_ = core->id_;
	gift.timeout_ = core->timeout_;
	coursegift_.push_back(gift);
	CALL_CLIENT(this,sycnCourseGift(coursegift_));
}

void Player::delCourseGift(U32 shopid){
	for (size_t i = 0; i < coursegift_.size(); ++i){
		if(coursegift_[i].id_ == shopid)
		{
			coursegift_.erase(coursegift_.begin() + i);
			CALL_CLIENT(this,sycnCourseGift(coursegift_));
			return;
		}
	}
}

void Player::calcCourseGifttime(float delta){
	if(coursegift_.empty())
		return;
	coursegift_[0].timeout_ -= delta;
	if(coursegift_[0].timeout_ <= 0){
		delCourseGift(coursegift_[0].id_);
	}
	/*for ( size_t i = 0; i < coursegift_.size(); ++i){
		coursegift_[i].timeout_ -= delta;
		if(coursegift_[i].timeout_ <= 0){
			delCourseGift(coursegift_[i].id_);
			return;
		}
	}*/
}

bool Player::buyCourseGift(U32 shopid){
	CourseGiftTable::CourseGiftContent const* core = CourseGiftTable::getbyshopid(shopid);
	if(core == NULL)
		return false;
	S32 num = 0;
	S32 itemnum = 0;
	for (size_t i=0; i<core->reward_.size();++i)
	{
		num += calcEmptyItemNum(core->reward_[i].first);
		itemnum += core->reward_[i].second;
	}

	if(num < itemnum){
		std::vector<COM_MailItem> items;
		for (size_t i=0; i<core->reward_.size();++i)
		{
			COM_MailItem item;
			item.itemId_ = core->reward_[i].first;
			item.itemStack_ = core->reward_[i].second;
			items.push_back(item);	
		}
		const Shop::Record* record= Shop::getRecordById(shopid);
		std::string sender = Global::get<std::string>(C_ShopMailSender);
		std::string content = Global::get<std::string>(C_ShopMailContent);
		std::string title = "";
		if(NULL == record){
			title = record->id_;
		}else {
			title = record->name_;
		}

		WorldServ::instance()->sendMail(sender,playerName_,title,content,0,0,items);
		ACE_DEBUG((LM_INFO,"Send mini gift buy mail\n"));
	}
	for (size_t i=0; i<core->reward_.size();++i)
	{
		addBagItemByItemId(core->reward_[i].first,core->reward_[i].second,false,16);
	}
	delCourseGift(shopid);
	CALL_CLIENT(this,sycnCourseGift(coursegift_));
	return true;
}