/** File generate by <hotlala8088@gmail.com> 2015/01/13  
*/
#include "config.h"

#include "worldserv.h"
#include "client.h"
#include "account.h"
#include "gwhandler.h"
#include "dbhandler.h"
#include "loginhandler.h"
#include "GMThandler.h"
#include "loghandler.h"
#include "CSVParser.h"
#include "tmptable.h"
#include "skilltable.h"
#include "sttable.h"
#include "TokenParser.h"
#include "battle.h"
#include "itemtable.h"
#include "DropTable.h"
#include "npctable.h"
#include "monstertable.h"
#include "scenetable.h"
#include "employeeoutput.h"
#include "employeeTable.h"
#include "Quest.h"
#include "LiveSkilltable.h"
#include "team.h"
#include "LotteryTable.h"
#include "randName.h"
#include "robotTable.h"
#include "EndlessStair.h"
#include "profession.h"
#include "BattleData.h"
#include "Shop.h"
#include "GameEvent.h"
#include "titleTable.h"
#include "achievementTable.h"
#include "challengeTable.h"
#include "Activity.h"
#include "DailyReward.h"
#include "pvpJJC.h"
#include "PVPrunkTable.h"
#include "EmployeeConfig.h"
#include "DebrisTable.h"
#include "tinyxml/tinyxml.h"
#include "scenehandler.h"
#include "TableSystem.h"
#include "Scene.h"
#include "Guild.h"
#include "DropTable.h"
#include "exam.h"
#include "carriers.h"
#include "json/json.h"
//#include "curl/curl.h"
#include "timer.h"
#include "EmployeeQuestSystem.h"
#include "MD5.h"
#if defined(WIN32)
#include <DbgHelp.h>
DWORD filer(LPEXCEPTION_POINTERS pExceptionInfo)
{
	HANDLE hfile = CreateFile("dump.dmp",GENERIC_WRITE,0,NULL,CREATE_ALWAYS,FILE_FLAG_BACKUP_SEMANTICS,NULL);
	if(hfile == INVALID_HANDLE_VALUE)
		return EXCEPTION_EXECUTE_HANDLER;
	MINIDUMP_EXCEPTION_INFORMATION minidump;
	minidump.ThreadId = GetCurrentThreadId();
	minidump.ExceptionPointers = pExceptionInfo;
	minidump.ClientPointers		= false;
	MiniDumpWriteDump(GetCurrentProcess(),GetCurrentProcessId(),hfile,MiniDumpNormal,&minidump,NULL,NULL);	
	CloseHandle(hfile);
	return EXCEPTION_EXECUTE_HANDLER;
}
void dump()
{
	__try
	{
		RaiseException( EXCEPTION_BREAKPOINT, 0, 0, NULL);
	}
	__except(filer(GetExceptionInformation()))
	{

	}

}
#endif


void assertPrepare(){
	Player::saveAll();
}
//-------------------------------------------------------------------------
/**
 */
///========================================================================
///@group
///@{

class ContactInfoSort
{
public:
	bool operator()(const SGE_ContactInfoExt* l, const SGE_ContactInfoExt* r)
	{
		if(l->value_ > r->value_)
			return true;
		else 
			return false;
	}
};

void 
WorldServ::storeCmd(std::string & cmd)
{
	if(cmd.size()==0)
		return;
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard( gmcmdmutex_ );
	gmcmdqueue_.push(cmd);
}

void 
WorldServ::doCmd()
{
	if(gmcmdqueue_.empty())
		return;
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard( gmcmdmutex_ );
	std::string cmd = gmcmdqueue_.back();
	gmcmdqueue_.pop();
	char const * ptr = cmd.c_str();
	if( TokenParser::checkToken(ptr , "dump" ))
	{
#if defined(WIN32)
		dump();
#endif
		return ;
	}
	else if( TokenParser::checkToken( ptr, "quit" ))
	{
		quitFlag_=true;
		return ;
	}
	else if( TokenParser::checkToken( ptr, "script" ) )
	{
		std::string err;
		if( !ScriptEnv::loadChunk( ptr, err ) )
		{
			ACE_DEBUG((LM_ERROR, ACE_TEXT("%s\n"), err.c_str()));
		}
		return ;
	}
	return ;
}
///@}

int 
WorldServ::init (int argc, ACE_TCHAR *argv[])
{
	ACE_DEBUG((LM_INFO,"Init world serv... \n",""));
	maxGuid_ = 0;
	quitFlag_ = false;
	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, ACE_Time_Value(0), t);

	//CURLcode result = curl_global_init(CURL_GLOBAL_ALL);

	/*if(CURLE_OK != result)
	{
		/// curl 全局初始化失败
		ACE_DEBUG((LM_INFO, ACE_TEXT("curl_global_init(CURL_GLOBAL_ALL) = %d is failed\n"), result));
		return -1;
	}*/
	ScriptEnv::init();
	UtlMath::init();
#include "ComScriptRegster.h"
#include "ComScriptApi.h"
#include "GameScript.h"
	
	std::string err;
	if( !ScriptEnv::loadFile( "env.lua", err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load env.lua failed:%s\n"), err.c_str() ) );
		SRV_ASSERT(0);
	}

	if(!Env::get<int>(V_DebugLog))
	{
		Logger::instance()->logPriorityMask();
	}

	std::string mainLua =std::string(GetScriptFilePath("main.lua"));
	if( !ScriptEnv::loadFile( mainLua.c_str(), err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load %s file failed:%s\n"), mainLua.c_str(), err.c_str() ) );
		SRV_ASSERT(0);
	}	

	ACE_DEBUG((LM_INFO,ACE_TEXT("INFO LOG ENABLED...\n")));
	ACE_DEBUG((LM_ERROR,ACE_TEXT("ERROR LOG ENABLED...\n")));
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("DEBUG LOG ENABLED...\n")));
	
	if(false == loadPeriodEvent(GetTableFilePath("PeriodEvent.xml")))
		SRV_ASSERT(0);
	
	if(false == TableSystem::instance()->Load())
		SRV_ASSERT(0);

	if(false == Activity::load((GetTableFilePath("DailyActivities.csv"))))
		SRV_ASSERT(0);
	if(false == Zhuanpan::load((GetTableFilePath("ZhuanpanConfig.csv"))))
		SRV_ASSERT(0);
	if(false == Activity::loadrewards((GetTableFilePath("ACT_Reward.csv"))))
		SRV_ASSERT(0);
	if(false == ExamTable::load((GetTableFilePath("question.csv"))))
		SRV_ASSERT(0);
	if(false == Employeeoutput::qualityload((GetTableFilePath("QualityWeight.csv"))))
		SRV_ASSERT(0);
	if(false == Employeeoutput::load((GetTableFilePath("EmployeeWeight.csv"))))
		SRV_ASSERT(0);

	if(false == Festival::load((GetTableFilePath("festival.csv"))))
		SRV_ASSERT(0);
	if(false == RechargeTotal::load((GetTableFilePath("recharge.csv"))))
		SRV_ASSERT(0);
	if(false == RechargeSingle::load((GetTableFilePath("recharge_single.csv"))))
		SRV_ASSERT(0);
	if(false == DiscountStore::load((GetTableFilePath("sale.csv"))))
		SRV_ASSERT(0);
	if(false == ReversalCard::loadcardsper((GetTableFilePath("cardsper.csv"))))
		SRV_ASSERT(0);
	if(false == ReversalCard::loadcardreward((GetTableFilePath("cardsreward.csv"))))
		SRV_ASSERT(0);
	if(false == HotShop::load((GetTableFilePath("hotshop.csv"))))
		SRV_ASSERT(0);
	if(false == SevenDayTable::load((GetTableFilePath("Sevendays.csv"))))
		SRV_ASSERT(0);
	if(false == EmployeeActivityTotal::load((GetTableFilePath("EmployeeActivity.csv"))))
		SRV_ASSERT(0);
	if(false == MySelfRecharge::load((GetTableFilePath("recharge_all.csv"))))
		SRV_ASSERT(0);
	if(false == LevelGift::load((GetTableFilePath("LevelGift.csv"))))
		SRV_ASSERT(0);
	if(false == IntegralShop::load((GetTableFilePath("ScoreShop.csv"))))
		SRV_ASSERT(0);
	if(false == CourseGiftTable::load((GetTableFilePath("CourseGift.csv"))))
		SRV_ASSERT(0);

	if(false == TableSystem::instance()->Check())
		SRV_ASSERT(0);
	if(false == Zhuanpan::check())
		SRV_ASSERT(0);
	if(false == Employeeoutput::check())
		SRV_ASSERT(0);

	if(false == Festival::check())
		SRV_ASSERT(0);
	if(false == RechargeTotal::check())
		SRV_ASSERT(0);
	if(false == RechargeSingle::check())
		SRV_ASSERT(0);
	if(false == DiscountStore::check())
		SRV_ASSERT(0);
	if(false == ReversalCard::check())
		SRV_ASSERT(0);
	if(false == HotShop::check())
		SRV_ASSERT(0);
	if(false == SevenDayTable::check())
		SRV_ASSERT(0);
	if(false == EmployeeActivityTotal::check())
		SRV_ASSERT(0);
	if(false == MySelfRecharge::check())
		SRV_ASSERT(0);
	if(false == LevelGift::check())
		SRV_ASSERT(0);
	if(false == IntegralShop::check())
		SRV_ASSERT(0);
	if(false == CourseGiftTable::check())
		SRV_ASSERT(0);
	
	Battle::init();
	EndlessStair::init();
	SceneManager::instance()->init();
	syncCentreTask_.init();
	maxShowItemId_ = 0;

	accept();
	curTime_=ACE_OS::gettimeofday().sec();
	ACE_DEBUG((LM_INFO,ACE_TEXT("Init world serv succ... \n")));
	
	
	std::string conf = Env::get<std::string>(V_GatewayListenClientMultiIndoor);
	std::vector<std::string> arr = String::Split(conf,";");
	for(size_t i=0; i<arr.size(); ++i){
		std::vector<std::string> parts = String::Split(arr[i],",");
		SRV_ASSERT(parts.size() == 2);
		int indoor = String::Convert<int>(parts[0]);
		//int port = String::Convert<int>(parts[1]);
		inDoorIds_.push_back(indoor);
	}
	return 0;
}

//-------------------------------------------------------------------------
/**
 */
int WorldServ::fini (void)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini world serv... ")));

	// 退出计时器.
	reactor()->cancel_timer(this);

	for (size_t i=0; i<contactInfoCache_.size(); ++i)
	{
		DEL_MEM(contactInfoCache_[i]);
	}
	contactInfoCache_.clear();
	contactInfoIdIndex_.clear();
	contactInfoNameIndex_.clear();
	syncCentreTask_.fini();
	Battle::fini();
	EndlessStair::fini();
	//smsTask_.fini();
	//giftTask_.fini();
	//logTask_.fini();
	SceneManager::instance()->fini();
	Guild::clear();
	curl_global_cleanup();

	return 0;
}


//-------------------------------------------------------------------------
/**
 */

int WorldServ::handle_timeout (const ACE_Time_Value &tv, const void *act)
{
	enum { INTERVAL = 5 };
	static float interval = INTERVAL;
	static U64 oldTime = tv.get_msec();
	U64 currTime = tv.get_msec() ;
	float delta =  (currTime - oldTime) / 1000.F ;
	//ACE_DEBUG((LM_INFO,"dt = %f\n",delta));
	oldTime = currTime;
	interval -= delta;
	
	{ ///监测内存
		enum{
			CHECK_INTERVAL = 300
		};

		static float check_interval = CHECK_INTERVAL;
		check_interval -= delta;

		if (check_interval < 0){
			check_interval += CHECK_INTERVAL;

			size_t arena,ordblks,hblkhd,usmblks ,uordblks,fordblks,keepcost ;
			nedalloc::nedmallinfo(arena,ordblks,hblkhd,usmblks ,uordblks,fordblks,keepcost);
			ACE_DEBUG((LM_INFO,">>>> lua %d | arena %d | ordblks %d | hblkhd %d | usmblks %d | uordblks %d | fordblks %d | keepcost %d <<<< \n",ScriptEnv::getUsedMemory(),arena,ordblks,hblkhd,usmblks,uordblks,fordblks,keepcost));
		}

	}

	//获得时间戳
	
	ACE_Date_Time oldDT;
	oldDT.update(ACE_Time_Value(curTime_));
	curTime_=ACE_OS::gettimeofday().sec();
	ACE_Date_Time newDT;
	newDT.update(ACE_Time_Value(curTime_));

	if(oldDT.year() != newDT.year() || oldDT.month() != newDT.month() || oldDT.day() != newDT.day())
		passZeroHour();
	

	//更新周期事件
	updatePeriodEvents();
	updateAverageLevel();
	updateGmNotice(delta);
	checkplayerlevelrank();
	Account::update(delta);

	/// 执行命令
	doCmd();
	GameTimer::checkTimers();
	
	//手机验证相关
	{
		//smsTask_.inout(prepareSMS_,complateSMS_);
		complateVerificationCode();
	}
	//GiftTask::updateResult();
	Battle::updateBattle(delta);
	Player::updatePlayer(delta);
	PvpJJC::tick(delta);
	SceneManager::instance()->tick();

	if(interval <= 0.F)
	{
		Guild::updateBattle(INTERVAL);
		EmployeeQuestSystem::TickRuning(INTERVAL);
		interval += INTERVAL;
	}
	
	if(quitFlag_){
		reactor()->end_event_loop();
	}
	
	return 0;
}

//-------------------------------------------------------------------------
/**
 */

class GatewayAcceptor
	: public ACE_Acceptor< GatewayHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(GatewayHandler *&sh)
	{
		sh = GatewayHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Gateway is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< GatewayHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Gateway connected !!! \n")));
		return 0;
	}
};

class DBAcceptor
	: public ACE_Acceptor< DBHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(DBHandler *&sh)
	{
		sh = DBHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("DB is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< DBHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("DB connected !!! \n")));
		return 0;
	}
};

class LoginAcceptor
	: public ACE_Acceptor< LoginHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(LoginHandler *&sh)
	{
		sh = LoginHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Login is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< LoginHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Login connected !!! \n")));
		return 0;
	}
};

class MallAcceptor
	: public ACE_Acceptor< MallHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(MallHandler *&sh)
	{
		sh = MallHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Mall is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< MallHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Mall connected !!! \n")));
		return 0;
	}
};

class SceneAcceptor
	: public ACE_Acceptor< SceneHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(SceneHandler *&sh)
	{
		sh = SceneHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("SceneHandler is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< SceneHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("SceneHandler conneczted !!! \n")));
		return 0;
	}
};

class GMTAcceptor
	: public ACE_Acceptor< GMTHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(GMTHandler *&sh)
	{
		sh = GMTHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("GMT is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< GMTHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("GMT connected !!! \n")));
		return 0;
	}
};
class LogAcceptor
	: public ACE_Acceptor< LogHandler , ACE_SOCK_ACCEPTOR >
{
public:
	int make_svc_handler(LogHandler *&sh)
	{
		sh = LogHandler::instance();
		if(sh->isConnect_)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Log is already connected !!! \n")));
			return 0;
		}
		ACE_Acceptor< LogHandler , ACE_SOCK_ACCEPTOR >::make_svc_handler(sh);
		sh->isConnect_ = true;
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Log connected !!! \n")));
		return 0;
	}
};



void WorldServ::accept()
{
	{
		static GatewayAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenGateway));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|GATEWAY] field\n"),Env::get<const char*>(V_WorldListenGateway)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|GATEWAY] \n"),Env::get<const char*>(V_WorldListenGateway)));
	}

	{
		static DBAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenDB));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|DATEBASE] field\n"),Env::get<const char*>(V_WorldListenDB)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|DATEBASE] \n"),Env::get<const char*>(V_WorldListenDB)));
	}

	{
		static LoginAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenLogin));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|LOGIN] field\n"),Env::get<const char*>(V_WorldListenLogin)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|LOGIN] \n"),Env::get<const char*>(V_WorldListenLogin)));
	}
	
	{
		static MallAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenMall));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|Mall] field\n"),Env::get<const char*>(V_WorldListenMall)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|Mall] \n"),Env::get<const char*>(V_WorldListenMall)));
	}

	{
		static SceneAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenScene));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|Scene] field\n"),Env::get<const char*>(V_WorldListenScene)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|Scene] \n"),Env::get<const char*>(V_WorldListenScene)));
	}

	{
		static GMTAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenGMT));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|GMT] field\n"),Env::get<const char*>(V_WorldListenGMT)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|GMT] \n"),Env::get<const char*>(V_WorldListenGMT)));
	}

	{
		static LogAcceptor acceptor;
		ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenLogser));
		if(acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("World serv listen [%s|LOG] field\n"),Env::get<const char*>(V_WorldListenLogser)));
			return;
		}
		ACE_DEBUG((LM_ERROR,ACE_TEXT("World serv listen [%s|LOG] \n"),Env::get<const char*>(V_WorldListenLogser)));
	}

	{
		static ACE_Acceptor< PayNotify, ACE_SOCK_ACCEPTOR > acceptor;
		ACE_INET_Addr addr(Env::get<int32>(V_PayListenPort),"0.0.0.0");
		if (acceptor.open(addr) == -1)
		{
			ACE_DEBUG((LM_INFO, ACE_TEXT("World serv listen [%d|ANY-SDK PayNotify] field\n"), Env::get<int32>(V_PayListenPort)));
			return;
		}
		ACE_DEBUG((LM_ERROR, ACE_TEXT("World serv listen [%d|PayNotify] \n"), Env::get<int32>(V_PayListenPort)));
	}

}

void WorldServ::addContactInfo(std::vector<SGE_ContactInfoExt>& info)
{
	for(size_t i=0; i<info.size(); ++i)
	{
		SGE_ContactInfoExt* p = NEW_MEM(SGE_ContactInfoExt,info[i]);
		contactInfoCache_.push_back(p);	
		contactInfoIdIndex_[p->instId_] = p;
		contactInfoNameIndex_[p->name_] = p;
	}
	ContactInfoSort sortFunc;
	std::sort(contactInfoCache_.begin(),contactInfoCache_.end(),sortFunc);
	//syncCentreTask_.pushAdded(info);
	
	//pushRoleLog(contactInfoCache_); /// TEST
}
void WorldServ::addContactInfo(SGE_ContactInfoExt info)
{
	SGE_ContactInfoExt* p = NEW_MEM(SGE_ContactInfoExt,info);
	contactInfoCache_.push_back(p);	
	contactInfoIdIndex_[info.instId_] = p;
	contactInfoNameIndex_[info.name_] = p;
	//syncCentreTask_.pushAdded(info);
}

void WorldServ::delContactInfo(U32 id){
	SGE_ContactInfoExt* p = contactInfoIdIndex_[id];
	if(NULL == p)
		return;
	contactInfoIdIndex_[p->instId_] = NULL;
	contactInfoNameIndex_[p->name_] = NULL;
	std::vector<SGE_ContactInfoExt*>::iterator itr = std::find(contactInfoCache_.begin(),contactInfoCache_.end(),p);
	if(itr!=contactInfoCache_.end())
		contactInfoCache_.erase(itr);
	
	//syncCentreTask_.pushDeled(*p);
	
	DEL_MEM(p);
}

std::string WorldServ::getAccontNameByPlayerId(uint32 playerId){
	if(contactInfoIdIndex_[playerId] == NULL)
		return "";
	return contactInfoIdIndex_[playerId]->accName_;
}

COM_ContactInfo* WorldServ::findContactInfo(U32 id)
{
	if(contactInfoIdIndex_.find(id) == contactInfoIdIndex_.end()) return NULL;
	return contactInfoIdIndex_[id];
}
SGE_ContactInfoExt* WorldServ::findContactInfoExt(uint32 id){
	if(contactInfoIdIndex_.find(id) == contactInfoIdIndex_.end()) return NULL;
	return contactInfoIdIndex_[id];

}
COM_ContactInfo* WorldServ::findContactInfo(std::string name)
{
	if(contactInfoNameIndex_.find(name) == contactInfoNameIndex_.end()) return NULL;
	return contactInfoNameIndex_[name];//可能需要模糊搜索
}

void WorldServ::updateContactInfo(Player* player){
	std::string playerName = player->getNameC();
	if(contactInfoNameIndex_.find(playerName) == contactInfoNameIndex_.end()) return;
	if(contactInfoNameIndex_[playerName] == NULL){
		ACE_DEBUG((LM_ERROR,"Update contact info player is nil %s\n",playerName.c_str()));
		return;
	}
	contactInfoNameIndex_[playerName]->job_ = (JobType)(int)player->getProp(PT_Profession);
	contactInfoNameIndex_[playerName]->jobLevel_ = player->getProp(PT_ProfessionLevel);
	contactInfoNameIndex_[playerName]->assetId_ = player->getProp(PT_AssetId);
	contactInfoNameIndex_[playerName]->level_ = player->getProp(PT_Level);
	contactInfoNameIndex_[playerName]->vip_ = (VipLevel)(int)player->getProp(PT_VipLevel);
	contactInfoNameIndex_[playerName]->gold_ = player->getProp(PT_Money);
	contactInfoNameIndex_[playerName]->diamond_ = player->getProp(PT_Diamond);
	contactInfoNameIndex_[playerName]->magicgold_ = player->getProp(PT_MagicCurrency);
	contactInfoNameIndex_[playerName]->ff_ = player->getProp(PT_FightingForce);
	contactInfoNameIndex_[playerName]->exp_ = player->getProp(PT_Exp);
	contactInfoNameIndex_[playerName]->section_ = player->pvpInfo_.section_;
	contactInfoNameIndex_[playerName]->value_ = player->pvpInfo_.value_;
	contactInfoNameIndex_[playerName]->rolelast_ = player->loginTime_;
	contactInfoNameIndex_[playerName]->isLine_=  true;
	if(player->account_)
	{
		contactInfoNameIndex_[playerName]->userid_ = player->account_->sdkInfo_.userId_;
		contactInfoNameIndex_[playerName]->pfid_ = player->account_->sdkInfo_.pfId_;
		contactInfoNameIndex_[playerName]->pfname_ = player->account_->sdkInfo_.pfName_;
	}
	ContactInfoSort sortFunc;
	std::sort(contactInfoCache_.begin(),contactInfoCache_.end(),sortFunc);
}

void WorldServ::getContactInfos(U32 index,std::vector<COM_ContactInfo>& infos)
{
	for (size_t i = 0; i < index; ++i)
	{
		infos.push_back(*contactInfoCache_[i]);
	}
}

void WorldServ::updateAverageLevel()
{
	if(contactInfoCache_.empty())
		return;

	averageLevel_ = 0;
	U32 index = 0;

	for (size_t i=0; i<contactInfoCache_.size(); ++i)
	{
		if(contactInfoCache_[i]->level_ <= 10)
			continue;

		averageLevel_ += contactInfoCache_[i]->level_;
		++index;
	}

	if(index == 0)
		return;

	averageLevel_ /= index;
}

void WorldServ::passZeroHour()
{ ///过 12 点
	UtlMath::updateSrand();
	Player::OnlinePlayerPassZeroHour();
	Zhuanpan::passZeroHour();
	Guild::passZeroHour();
	updateRoleLogTable();
}

void WorldServ::updateRoleLogTable(){
	pushRoleLog(contactInfoCache_);
}

bool WorldServ::loadPeriodEvent(const char* fn){
	TiXmlDocument doc( 	fn );
	ACE_DEBUG( ( LM_DEBUG, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	if ( !doc.LoadFile() )
	{
		ACE_DEBUG( (LM_DEBUG, ACE_TEXT( "Could not load period event file '%s'. ErrorRow:'%d'\n"), fn, doc.ErrorRow()));
		return false;
	}
	TiXmlElement* rootElem=doc.FirstChildElement("root");
	SRV_ASSERT(rootElem);
	for(  TiXmlElement* periodElem = rootElem->FirstChildElement("PeriodEvents"); periodElem; periodElem = periodElem->NextSiblingElement("PeriodEvents") )
	{
		for( TiXmlElement* weekElem = periodElem->FirstChildElement( "WeekEvent" ); weekElem; weekElem = weekElem->NextSiblingElement( "WeekEvent" ) )
		{
			for( TiXmlElement* eventElem = weekElem->FirstChildElement( "event" ); eventElem; 
				eventElem = eventElem->NextSiblingElement( "event" ) )
			{
				std::string functionName;
				XML_GET_ATTRIBUTE_S(eventElem, "script",functionName);
				PeriodEvent* newEvent =NEW_MEM(PeriodEvent,PT_Weekly,functionName);
				int day	=0;	
				int hour=0;
				int minute=0;
				int sec	=0;
				XML_GET_ATTRIBUTE_I(eventElem, "day",day);
				XML_GET_ATTRIBUTE_I(eventElem, "hour",hour);
				XML_GET_ATTRIBUTE_I(eventElem, "minute",minute);
				XML_GET_ATTRIBUTE_I(eventElem, "sec",sec);
				newEvent->initTimer(day, hour, minute, sec);
				registerPeriodEvent(newEvent);
			}
		}

		for( TiXmlElement* dayElem = periodElem->FirstChildElement( "DayEvent" ); dayElem; dayElem = dayElem->NextSiblingElement( "DayEvent" ) )
		{
			for( TiXmlElement* eventElem = dayElem->FirstChildElement( "event" ); eventElem; 
				eventElem = eventElem->NextSiblingElement( "event" ) )
			{
				std::string functionName;
				XML_GET_ATTRIBUTE_S(eventElem, "script",functionName);
				PeriodEvent* newEvent =NEW_MEM(PeriodEvent,PT_Daily,functionName);
				int hour=0;
				int minute=0;
				int sec	=0;
				XML_GET_ATTRIBUTE_I(eventElem, "hour",hour);
				XML_GET_ATTRIBUTE_I(eventElem, "minute",minute);
				XML_GET_ATTRIBUTE_I(eventElem, "sec",sec);
				newEvent->initTimer(0, hour, minute, sec);
				registerPeriodEvent(newEvent);
			}
		}
	}

	return true;
}

void WorldServ::registerPeriodEvent( PeriodEvent* newEvent )
{
	// insert into proper position.	
	PeriodEventListIter iter = periodEventsList_.begin();
	for(; iter != periodEventsList_.end(); ++iter)
	{
		if(newEvent->triggerDatetime_ < (*iter)->triggerDatetime_)
			break;
	}
	periodEventsList_.insert(iter, newEvent);
}

void  WorldServ::updatePeriodEvents()
{
	PeriodEventListIter iter  ;
	while((iter=periodEventsList_.begin())!=  periodEventsList_.end()) 
	{		
		if ((*iter)->triggerDatetime_ <= curTime_)
		{
			PeriodEvent* periodEvent = *iter;
			std::string errMsg;

			if( periodEvent->eventTrigger_ )
			{
				periodEvent->eventTrigger_->run();
			}
			if(periodEvent->eventScript_.length() > 0)
			{
				if(!ScriptEnv::call( periodEvent->eventScript_.c_str(), errMsg ) )
				{
					ACE_DEBUG((LM_ERROR,"updatePeriodEvents error script name:%s error:%s\n",periodEvent->eventScript_.c_str(),errMsg.c_str()));
					return ;
				}
			}
			periodEvent->calcNextTimer();
			periodEventsList_.erase(iter);
			registerPeriodEvent(periodEvent);
		}
		else
		{
			break;
		}
	}
}

void WorldServ::sendMail(Player* player, std::string& recvPlayerName, std::string &title, std::string &content, std::vector<COM_MailItem> &items){

	while(items.size() > 5)
		items.pop_back();

	if(NULL == WorldServ::instance()->findContactInfo(recvPlayerName))
		return ;
	COM_Mail mail;
	mail.mailType_ = MT_Normal;
	mail.sendPlayerName_ = player->playerName_;
	mail.recvPlayerName_ = recvPlayerName;
	mail.title_ = title;
	mail.content_ = content;
	mail.items_ = items;
	mail.timestamp_ = WorldServ::instance()->curTime_;
	
	DBHandler::instance()->insertMail(mail);
}

void WorldServ::sendMailByDrop(std::string& sendName, std::string& recvPlayerName, std::string &title, std::string &content , S32 dropId)
{
	DropTable::Drop const* p = DropTable::getDropById(dropId);
	if(!p)
		return;
	std::vector<COM_MailItem> items;
	for(size_t i=0; i<p->items_.size(); ++i)
	{
		COM_MailItem item;
		item.itemId_ = p->items_[i].itemId_;
		item.itemStack_ = p->items_[i].itemNum_;
		items.push_back(item);
	}

	sendMail(sendName,recvPlayerName,title,content,p->money_,p->diamond_,items);
}

void WorldServ::sendMailByDrop(std::string& sendName, std::vector<std::string>&recvs, std::string &title, std::string &content , S32 dropId)
{
	DropTable::Drop const* p = DropTable::getDropById(dropId);
	if(!p)
		return;
	std::vector<COM_MailItem> items;
	for(size_t i=0; i<p->items_.size(); ++i)
	{
		COM_MailItem item;
		item.itemId_ = p->items_[i].itemId_;
		item.itemStack_ = p->items_[i].itemNum_;
		items.push_back(item);
	}

	COM_Mail mail;
	mail.mailType_ = MT_System;
	mail.sendPlayerName_ = sendName;
	mail.title_ = title;
	mail.content_ = content;
	mail.items_ = items;
	mail.timestamp_ = WorldServ::instance()->curTime_;
	
	DBHandler::instance()->insertMailByRecvs(mail,recvs);
}

void WorldServ::sendMailByDropAll(std::string& sendName, std::string &title, std::string &content , S32 dropId)
{
	DropTable::Drop const* p = DropTable::getDropById(dropId);
	if(!p)
		return;
	std::vector<COM_MailItem> items;
	for(size_t i=0; i<p->items_.size(); ++i)
	{
		COM_MailItem item;
		item.itemId_ = p->items_[i].itemId_;
		item.itemStack_ = p->items_[i].itemNum_;
		items.push_back(item);
	}

	COM_Mail mail;
	mail.mailType_ = MT_System;
	mail.sendPlayerName_ = sendName;
	mail.title_ = title;
	mail.content_ = content;
	mail.items_ = items;
	mail.timestamp_ = WorldServ::instance()->curTime_;
	
	std::vector<std::string> recvs;
	for(size_t i=0;i<contactInfoCache_.size(); ++i)
	{
		recvs.push_back(contactInfoCache_[i]->name_);
	}
	DBHandler::instance()->insertMailByRecvs(mail,recvs);
}

void WorldServ::sendMail(std::string& sendName, std::string& recvPlayerName, std::string &title, std::string &content,S32 money , S32 diamond, std::vector<COM_MailItem> &items){
	while(items.size() > 5)
		items.pop_back();
	if(NULL == WorldServ::instance()->findContactInfo(recvPlayerName))
		return ;
	COM_Mail mail;
	mail.mailType_ = MT_System;
	mail.sendPlayerName_ = sendName;
	mail.recvPlayerName_ = recvPlayerName;
	mail.title_ = title;
	mail.content_ = content;
	mail.items_ = items;
	mail.money_ = money;
	mail.diamond_ = diamond;
	mail.timestamp_ = WorldServ::instance()->curTime_;
	
	DBHandler::instance()->insertMail(mail);
}

void WorldServ::sendMail(std::string& sendName, std::vector<std::string>& recvs, std::string &title, std::string &content , S32 money , S32 diamond ,  std::vector<COM_MailItem> &items){
	COM_Mail mail;
	mail.mailType_ = MT_System;
	mail.sendPlayerName_ = sendName;
	mail.title_ = title;
	mail.content_ = content;
	mail.items_ = items;
	mail.money_ = money;
	mail.diamond_ = diamond;
	mail.timestamp_ = WorldServ::instance()->curTime_;

	DBHandler::instance()->insertMailByRecvs(mail,recvs);
}

void WorldServ::sendMailAll(COM_Mail& mail){
	DBHandler::instance()->insertMailAll(mail);
}

void WorldServ::sendMailAllOnline(COM_Mail& mail,int32 lowLevel,int32 highLevel,int64 lowTime, int64 highTime){
	std::vector<std::string> recvs;
	for(size_t i=0; i<Player::store_.size(); ++i)
	{
		if(lowLevel!=0 && Player::store_[i]->getProp(PT_Level) < lowLevel){
			continue;
		}
		if(highLevel!=0 && Player::store_[i]->getProp(PT_Level) > highLevel){
			continue;
		}
		if(lowTime!=0 && Player::store_[i]->createTime_ < lowTime){
			continue;
		}
		if(highTime!=0 && Player::store_[i]->createTime_ > highTime){
			continue;
		}
		recvs.push_back(Player::store_[i]->playerName_);
	}

	DBHandler::instance()->insertMailByRecvs(mail,recvs);
}

void WorldServ::notice(std::string& content, bool isGm){
	WorldBroadcaster::instance()->boardcastNotice(content, isGm);
}

void WorldServ::gmNotice(NoticeSendType bType, std::string& note, U64 thetime, S64 itvtime)
{
	NoticeCmd cmd ;
	cmd.type_ = bType;
	cmd.thetime_ = thetime;
	cmd.itvtime_ = itvtime;
	cmd.dlttime_ = itvtime;
	cmd.content_ = note;
	gmNotice_.push_back(cmd);
}

void WorldServ::updateGmNotice(float delta)
{
	for(size_t i=0; i<gmNotice_.size(); ++i)
	{
		switch(gmNotice_[i].type_){
			case NST_Immediately :
			{
				notice(gmNotice_[i].content_,true);
				gmNotice_.erase(gmNotice_.begin() + i--);
				break;
			}
			case NST_Timming : 
			{
				if(gmNotice_[i].thetime_ <= curTime_){
					notice(gmNotice_[i].content_,true);
					gmNotice_.erase(gmNotice_.begin() + i--);
				}
				break;
			}
			case NST_Loop : 
			{
				gmNotice_[i].dlttime_ -= delta;
				if(gmNotice_[i].dlttime_ <=0.f){
					notice(gmNotice_[i].content_,true);
					gmNotice_[i].dlttime_ = gmNotice_[i].itvtime_;
				}
				if(gmNotice_[i].thetime_ <= curTime_){
					gmNotice_.erase(gmNotice_.begin() + i--);
				}
				
				break;
			}
		}
	}
}


void 
WorldServ::vipitemmaill(std::string& sendName,std::string &title,std::string &content){
	std::vector<COM_MailItem> vip1_items,vip2_items;
	COM_MailItem mit;
	mit.itemId_ = Global::get<int>(C_Vip1Reward);
	mit.itemStack_ = Global::get<int>(C_Vip1RewardNum);
	vip1_items.push_back(mit);
	mit.itemId_ = Global::get<int>(C_Vip2Reward);
	mit.itemStack_ = Global::get<int>(C_Vip2RewardNum);
	vip2_items.push_back(mit);
	for(int i=0; i<contactInfoCache_.size(); ++i)
	{
		if(contactInfoCache_[i]->vip_ == VL_1){
			sendMail(sendName,contactInfoCache_[i]->name_,title,content,0,0,vip1_items);
		}
		else if(contactInfoCache_[i]->vip_ == VL_2){
			sendMail(sendName,contactInfoCache_[i]->name_,title,content,0,0,vip2_items);
		}
	}
}

COM_ShowItemInst*
WorldServ::addShowItem(Player* player,COM_Item& itemInst)
{
	if(NULL==player)
		return NULL;

	COM_ShowItemInst* inst=NEW_MEM(COM_ShowItemInst);
	SRV_ASSERT(NULL != inst);

	inst->showId_=maxShowItemId_++;
	inst->itemInst_=itemInst;
	inst->playerName_=player->playerName_;
	if(showItemInstData_.addData(inst))
		return inst;
	return NULL;
}

COM_ShowItemInst* 
WorldServ::getShowItemById( int32 showId )
{
	return showItemInstData_.getData(showId);
}

void 
WorldServ::sysShowItem(Player* player,int cType,int32 itemInstId,const std::string content)
{
	if( NULL == player )
		return;
	COM_Item* inst = player->getItemInst((ItemContainerType)cType,itemInstId);
	if( NULL == inst)
		return;

	COM_ShowItemInst* showItemInst = addShowItem(player,*inst);
	if( NULL == showItemInst )
		return;

	COM_ShowItemInstInfo info;
	info.showId_=showItemInst->showId_;
	info.itemId_=inst->itemId_;
	info.playerName_=showItemInst->playerName_;

	//WorldPlayer::bcWorldNPublishItemMsg(content,info);
}

COM_ShowbabyInst*
WorldServ::addShowBaby(Player* player,COM_BabyInst& babyInst)
{
	if(player == NULL)
		return NULL;
	COM_ShowbabyInst* inst=NEW_MEM(COM_ShowbabyInst);
	SRV_ASSERT(NULL != inst);
	inst->showId_=maxShowItemId_++;
	inst->babyInst_ = babyInst;
	inst->playerName_=player->playerName_;
	if(showBabyInstData_.addData(inst))
		return inst;
	return NULL;
}

COM_ShowbabyInst*		 
WorldServ::getShowBabyById( int32 showId )
{
	return showBabyInstData_.getData(showId);
}

void
WorldServ::savewish(COM_Wish& wish)
{
	if(wishstore_.size() > Global::get<int>(C_WishStoreMax))
		wishstore_.erase(wishstore_.begin());
	wishstore_.push_back(wish);
}

COM_Wish*
WorldServ::getWish()
{
	if(wishstore_.empty())
		return NULL;
	U32 index = UtlMath::randN(wishstore_.size());

	return &wishstore_[index];
}

AudioInfo const * WorldServ::findAudioInfo(int audioId){
	for(size_t i=0; i<audioCache_.size(); ++i){
		if(audioCache_[i].audioId_ == audioId)
			return &audioCache_[i];
	}
	return NULL;
}
int WorldServ::pushAudioInfo(std::vector<U8>& bytes){
	enum {MAX_Record = 2000};
	static int guid = 0;
	AudioInfo ac(++guid,bytes);
	
	audioCache_.push_back(ac);
	if(audioCache_.size() > MAX_Record)
		audioCache_.erase(audioCache_.begin());
	return guid;
}


//----排行榜-----
class PlayerFFSort
{
public:
	bool operator()(COM_ContactInfo l, COM_ContactInfo r)
	{
		if(l.ff_ > r.ff_)
			return true;
		else if(l.ff_ < r.ff_)
			return false;
		else 
			return false;
	}
};

class PlayerLevelSort
{
public:
	bool operator()(COM_ContactInfo l, COM_ContactInfo r)
	{
		if(l.exp_ > r.exp_)
			return true;
		else if(l.exp_ < r.exp_)
			return false;
		else 
			return false;
	}
};

class BabyFFSort
{
public:
	bool operator()(COM_BabyRankData l, COM_BabyRankData r)
	{
		if(l.ff_ > r.ff_)
			return true;
		else if(l.ff_ < r.ff_)
			return false;
		else 
			return false;
	}
};

class EmployeeFFSort
{
public:
	bool operator()(COM_EmployeeRankData l, COM_EmployeeRankData r)
	{
		if(l.ff_ > r.ff_)
			return true;
		else if(l.ff_ < r.ff_)
			return false;
		else 
			return false;
	}
};


void WorldServ::syncPlayerFFRank(std::vector< COM_ContactInfo >& infos){
	for (size_t i=0; i < infos.size(); ++i){
		playerFFrank_.push_back(infos[i]);
	}
}

void WorldServ::getPlayerFFRank(std::vector< COM_ContactInfo >& infos){
	infos = playerFFrank_;
}

void WorldServ::syncPlayerLevelRank(std::vector< COM_ContactInfo >& infos){
	for (size_t i=0; i < infos.size(); ++i){
		calcPlayerLevelRank(infos[i]);
	}
}

void WorldServ::getPlayerLevelRank(std::vector< COM_ContactInfo >& infos){
	infos = playerlevelrank_;
}

void WorldServ::fatchBabyRankOK(std::vector< COM_BabyRankData >& infos){
	for (size_t i=0; i < infos.size(); ++i){
		calcBabyFFRank(infos[i]);
	}
}

void WorldServ::getBabyFFRank(std::vector< COM_BabyRankData >& infos){
	for (size_t i=0; i < babyffrank_.size(); ++i){
		infos.push_back(babyffrank_[i]);
	}
}

void WorldServ::fatchEmpRankOK(std::vector<COM_EmployeeRankData>& infos){
	for (size_t i=0; i < infos.size(); ++i){
		employeeffrank_.push_back(infos[i]);
	}
}
void WorldServ::getEmployeeFFRank(std::vector<COM_EmployeeRankData>& infos){
	for (size_t i=0; i < employeeffrank_.size(); ++i){
		infos.push_back(employeeffrank_[i]);
	}
}

void WorldServ::updateBabyRank(U32 instid){
	for(size_t i=0; i<babyffrank_.size(); ++i){
		if(babyffrank_[i].instId_ == instid){
			babyffrank_.erase(babyffrank_.begin() + i--);
		}
		if(i < babyffrank_.size())
			babyffrank_[i].rank_ = i+1;
	}
}

void WorldServ::deleteRank(std::string const& playerName){
	
	if(playerName.empty())
		return;
	
	for(size_t i=0; i< playerFFrank_.size(); ++i){
		if(playerFFrank_[i].name_ == playerName){
			playerFFrank_.erase(playerFFrank_.begin() + i--);
		}
		if(i < playerFFrank_.size())
			playerFFrank_[i].rank_ = i+1;
	}
	
	for(size_t i=0; i< playerlevelrank_.size(); ++i){
		if(playerlevelrank_[i].name_ == playerName){
			playerlevelrank_.erase(playerlevelrank_.begin() + i--);
		}
		if(i < playerlevelrank_.size())
			playerlevelrank_[i].rank_ = i+1;
	}
	
	for(size_t i=0; i<babyffrank_.size(); ++i){
		if(babyffrank_[i].ownerName_ == playerName){
			babyffrank_.erase(babyffrank_.begin() + i--);
		}
		if(i < babyffrank_.size())
			babyffrank_[i].rank_ = i+1;
	}

	for(size_t i=0; i<employeeffrank_.size(); ++i){
		if(employeeffrank_[i].ownerName_ == playerName){
			employeeffrank_.erase(employeeffrank_.begin() + i--);
		}
		if(i < employeeffrank_.size())
			employeeffrank_[i].rank_ = i+1;
	}
}

void WorldServ::delBabyRank(U32 instid){
	for(size_t i=0; i<babyffrank_.size(); ++i){
		if(babyffrank_[i].instId_ == instid){
			babyffrank_.erase(babyffrank_.begin() + i--);
		}
		if(i < babyffrank_.size())
			babyffrank_[i].rank_ = i+1;
	}
}

void WorldServ::calcPlayerFFRank(COM_ContactInfo info){
	if(info.instId_ == 0)
		return;
	COM_ContactInfo* pInfo = findPlayerFFRank(info.instId_);
	if(pInfo)
		pInfo->ff_ = info.ff_;
	else
		playerFFrank_.push_back(info);

	PlayerFFSort sort;
	std::sort(playerFFrank_.begin(),playerFFrank_.end(),sort);
	if(playerFFrank_.size() > RANK_MAX){
		playerFFrank_.pop_back();
	}
	for (size_t i=0; i < playerFFrank_.size(); ++i){
		playerFFrank_[i].rank_=i+1;
	}
}
void WorldServ::calcPlayerLevelRank(COM_ContactInfo info){
	if(info.instId_ == 0)
		return;
	COM_ContactInfo* pInfo = findPlayerLevelRank(info.instId_);
	if(pInfo)
	{
		*pInfo = info;
	}
	else
		playerlevelrank_.push_back(info);

	PlayerLevelSort sort;
	std::sort(playerlevelrank_.begin(),playerlevelrank_.end(),sort);

	if(playerlevelrank_.size() > RANK_MAX){
		playerlevelrank_.pop_back();
	}
	for (size_t i=0; i < playerlevelrank_.size(); ++i){
		playerlevelrank_[i].rank_=i+1;
	}
}
void WorldServ::calcBabyFFRank(COM_BabyRankData info){
	if(info.instId_ == 0)
		return;
	COM_BabyRankData* pInfo = findBabyFFRank(info.instId_);
	if(pInfo)
		*pInfo = info;
	else
		babyffrank_.push_back(info);

	BabyFFSort sort;
	std::sort(babyffrank_.begin(),babyffrank_.end(),sort);

	if(babyffrank_.size() > RANK_MAX){
		babyffrank_.pop_back();
	}
	for (size_t i=0; i < babyffrank_.size(); ++i){
		babyffrank_[i].rank_=i+1;
	}
}
void WorldServ::calcEmployeeFFRank(COM_EmployeeRankData info){
	if(info.instId_ == 0)
		return;
	COM_EmployeeRankData* pInfo = findEmployeeFFRank(info.instId_);
	if(pInfo)
		*pInfo = info;
	else
		employeeffrank_.push_back(info);
	EmployeeFFSort sort;
	std::sort(employeeffrank_.begin(),employeeffrank_.end(),sort);

	if(employeeffrank_.size() > RANK_MAX){
		employeeffrank_.pop_back();
	}
	for (size_t i=0; i < employeeffrank_.size(); ++i){
		employeeffrank_[i].rank_=i+1;
	}
}

COM_ContactInfo* WorldServ::findPlayerFFRank(U32 instid){
	for (size_t i=0; i<playerFFrank_.size(); ++i)
	{
		if(playerFFrank_[i].instId_==instid)
			return &playerFFrank_[i];
	}
	return NULL;
}
COM_ContactInfo* WorldServ::findPlayerLevelRank(U32 instid){
	for (size_t i=0; i<playerlevelrank_.size();++i)
	{
		if(playerlevelrank_[i].instId_==instid)
			return &playerlevelrank_[i];
	}
	return NULL;
}
COM_BabyRankData* WorldServ::findBabyFFRank(U32 instid){
	for (size_t i=0; i<babyffrank_.size();++i)
	{
		if(babyffrank_[i].instId_==instid)
			return &babyffrank_[i];
	}
	return NULL;
}
COM_EmployeeRankData* WorldServ::findEmployeeFFRank(U32 instid){
	for (size_t i=0; i<employeeffrank_.size();++i)
	{
		if(employeeffrank_[i].instId_==instid)
			return &employeeffrank_[i];
	}
	return NULL;
}

void WorldServ::checkplayerlevelrank(){
	std::vector<U32> playerIds;
	for (size_t i=0; i < playerlevelrank_.size(); ++i){
		for (size_t j=i+1; j < playerlevelrank_.size(); ++j){
			if(playerlevelrank_[i].instId_ == playerlevelrank_[j].instId_)
				playerIds.push_back(playerlevelrank_[i].instId_);
		}
	}

	if(playerIds.empty())
		return;
	for (size_t i=0; i < playerlevelrank_.size(); ++i){
		if (std::find(playerIds.begin(),playerIds.end(),playerlevelrank_[i].instId_) != playerIds.end()){
			playerlevelrank_.erase(playerlevelrank_.begin() + i);
			return;
		}
	}
}

void WorldServ::reqCDKey(std::string cdkey, std::string playername, std::vector<std::string> &giftNames){
	//giftTask_.pushAdded(playername,cdkey,giftNames);
}


void WorldServ::prepareVerificationCode(std::string phoneNumber, uint32 playerId){
	/*for(size_t i=0; i<prepareSMS_.size(); ++i){
		if(prepareSMS_[i].playerId_ == playerId)
		{
			prepareSMS_[i].phoneNumber_ = phoneNumber;
			return;
		}
	}
	SMSContent sc;
	sc.phoneNumber_ = phoneNumber;
	sc.playerId_ = playerId;
	prepareSMS_.push_back(sc);*/
}

void WorldServ::complateVerificationCode(){
	/*for(size_t i=0; i<complateSMS_.size(); ++i){
		Player* p = Player::getPlayerByInstId(complateSMS_[i].playerId_);

		if(!p){
		complateSMS_.erase(complateSMS_.begin() + i--);

		}else {
		p->smsCode_ = complateSMS_[i].smsCode_;
		p->phoneNumber_ = complateSMS_[i].phoneNumber_;
		p->smsTime_ = curTime_;
		complateSMS_.erase(complateSMS_.begin() + i--);
		}

		}*/
}

void WorldServ::pushAccountLog(Account* acc){
	//logTask_.pushAccountLog(acc);
}
void WorldServ::pushLoginLog(Player *player){
	//logTask_.pushLoginLog(player);
}
void WorldServ::pushLoginLog(Account *acc){
	//logTask_.pushLoginLog(acc);
}
void WorldServ::pushOrderLog(Account *acc, int32 playerId, int32 playerLevel, std::string const &orderId, int32 payment, std::string const &payTime){
	//logTask_.pushOrderLog(acc,playerId,playerLevel,orderId,payment,payTime);
}
void WorldServ::pushRoleLog(std::vector<SGE_ContactInfoExt*> &infos){
	//logTask_.pushRoleLog(infos);
}