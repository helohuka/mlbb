#include "config.h"
#include "handler.h"
#include "server.h"
#include "routine.h"
#include "SQLHelper.h"
#include "TableSystem.h"

#include "ComEnv.h"
#include "ComGlobal.h"
#include "ComScriptEvn.h"

std::string dbAddr_;		///<mysql	地址
std::string dbName_;		///<mysql	数据库名
std::string dbUser_;		///<mysql	数据库用户名
std::string dbPwd_;			///<mysql	密码

int 
Server::init (int argc, ACE_TCHAR *argv[])
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Init tempStation serv... \n")));
	
	///读取LUA
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin load lua...\n")));
	ScriptEnv::init();

#include "ComScriptRegster.h"
#include "ComScriptApi.h"
	std::string err;
	if( !ScriptEnv::loadFile( "env.lua", err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load env.lua failed:%s\n"), err.c_str() ) );
		SRV_ASSERT(0);
	}

	if(!TableSystem::instance()->Load()){
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("load tables failed:%s\n"), err.c_str() ) );
		SRV_ASSERT(0);
	}

	/// \初始化记时器
	ACE_Time_Value t;
	t.set((double)1.0/(double)(TIMER_FREQ));
	reactor()->schedule_timer(this, NULL, ACE_Time_Value(0), t);
	
	ACE_DEBUG((LM_INFO,ACE_TEXT("Begin accept world server...\n")));

	ACE_INET_Addr addr(Env::get<const char*>(V_WorldListenGMT));

	ACE_Connector<WorldHandler, ACE_SOCK_CONNECTOR > connector;
	WorldHandler *p = WorldHandler::instance();
	connector.reactor(reactor());
	if(connector.connect(p,addr) == -1)
	{
		//SRV_ASSERT(0);
	}
	
	{
		static ACE_Acceptor< ClientHandler , ACE_SOCK_ACCEPTOR > accepter;
		ACE_INET_Addr addr(Env::get<const char*>(V_GMTListenWebServer));
		if(accepter.open(addr) == -1)
		{
			SRV_ASSERT(0);
			return 0;
		}
	}

	dbAddr_  = Env::get<const char*>(V_MysqlHost);
	dbName_	 = Env::get<const char*>(V_DatabaseName);
	dbUser_	 = Env::get<const char*>(V_MysqlUser);
	dbPwd_	 = Env::get<const char*>(V_MysqlPassword);
	SQLTask::sinit();

	return 0;
}

int 
Server::fini (void)
{
	ACE_DEBUG((LM_TRACE,ACE_TEXT("Fini tempStation serv... ")));

	// 退出计时器.
	reactor()->cancel_timer(this);
	return 0;
}

int 
Server::handle_timeout (const ACE_Time_Value &current_time, const void *act)
{
	enum { INTERVAL = 60 };
	static float interval = INTERVAL;
	static U64 oldTime = current_time.get_msec();
	U64 currTime = current_time.get_msec() ;
	float delta =  (currTime - oldTime) / 1000.F ;
	interval -= delta;
	if(interval <= 0.F)
	{
		checkGiftCatch();
		interval += INTERVAL;
	}
	return 0;
}

void Server::checkGiftCatch()
{
	if(keygiftcatch_.empty())
		return;

	static const char * pCode =
		"INSERT INTO KeyGiftTable(cdkey,pfIp,giftName,playerName,useTime,rewardItem)"
		"VALUES(?,?,?,?,?,?);";

	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	for (size_t i = 0; i < keygiftcatch_.size(); ++i)
	{
		
		prep_stmt->setString( 1, keygiftcatch_[i].key_.c_str());
		prep_stmt->setString( 2, keygiftcatch_[i].pfid_.c_str());
		prep_stmt->setString( 3, keygiftcatch_[i].giftname_.c_str());
		prep_stmt->setString( 4, keygiftcatch_[i].playerName_.c_str());
		prep_stmt->setInt( 5 , int(keygiftcatch_[i].usetime_));

		char buffer1[sizeof(COM_GiftItem)*32] = {0};
		ProtocolMemWriter protocol2(buffer1 , sizeof(buffer1));
		protocol2.writeVector(keygiftcatch_[i].items_);
		sql::SQLString	tmpStr1(buffer1,protocol2.length());
		prep_stmt->setString( 6 , tmpStr1);

		prep_stmt->executeUpdate();
	}
	prep_stmt->close();
	DB_EXEC_UNGUARD_RETURN
	ACE_DEBUG((LM_INFO,ACE_TEXT("GMT TO DB CEKEY NUM[%d]\n"),keygiftcatch_.size()));
	keygiftcatch_.clear();
}



