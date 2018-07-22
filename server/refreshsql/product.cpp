#include "product.h"
#include "translation.h"
RefreshSql::RefreshSql()
:total_rows_(0)
,trans_rows_(0)
,pmysql_(NULL)
{
	params_.resize(RefreshSql::MAX);
}

void RefreshSql::init(int argc,char ** argv)
{
	ACE_Get_Opt cmd_opt(argc,argv,ACE_TEXT("h:H:u:U:p:P:s:S:v:V"));
	cmd_opt.long_option(ACE_TEXT("host"),'h',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("HOST"),'H',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("user"),'u',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("USER"),'U',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("pwd"),'p',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("PWD"),'P',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("schema"),'s',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("SCHEMA"),'S',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("version"),'v',ACE_Get_Opt::ARG_REQUIRED);
	cmd_opt.long_option(ACE_TEXT("VERSION"),'V',ACE_Get_Opt::ARG_REQUIRED);
	
	int opt = 0;
	while(EOF != (opt=cmd_opt()))
	{
		switch(opt)
		{
		case 'h':
		case 'H':
			{
				params_[HOST] = cmd_opt.opt_arg();
				break;
			}
		case 'u':
		case 'U':
			{
				params_[USER] = cmd_opt.opt_arg();
				break;
			}
		case 'p':
		case 'P':
			{
				params_[PWD] = cmd_opt.opt_arg();
				break;
			}	
		case 's':
		case 'S':
			{
				params_[SCHEMA] = cmd_opt.opt_arg();
				break;
			}
		case 'v':
		case 'V':
			{
			//	version_ = ACE_OS::atoi(cmd_opt.opt_arg());
				break;
			}
		default:
			break;
		}
	}
	
	ACE_DEBUG((LM_INFO,ACE_TEXT("HOST[%s] USER[%s] PWD[%s] SCHEMA[%s] VERSION[%d] \n"),
		params_[HOST].c_str(),params_[USER].c_str(),params_[PWD].c_str(),params_[SCHEMA].c_str(),(int)OLD_VERSION));

	SRV_ASSERT(true==open_mysql());
}

void RefreshSql::fini()
{
	SRV_ASSERT(true==close_mysql());
}

void RefreshSql::run()
{
DB_EXEC_GUARD

	static const char * select_code = "SELECT * FROM Player";
	static const char * update_code = "UPDATE Player SET BinData=?,VersionNumber=? WHERE PlayerGuid=?";
	
	std::auto_ptr< sql::Statement > stmt(pmysql_->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(select_code));
	
	int sec = ACE_OS::gettimeofday().sec();

	total_rows_ = res->rowsCount();
	while(res->next())
	{
		int64	roleId = res->getInt("PlayerGuid");
		int32	version= res->getInt("VersionNumber");
	
		if(OLD_VERSION!=version)
		{   ///转换版本不符
			ACE_DEBUG((LM_ERROR,ACE_TEXT("OLD_VERSION(%d)!=VERSION(%d) ROLE-ID[%q]\n"),(int)OLD_VERSION,(int)version,roleId));
			continue;
		}
		sql::SQLString oldBinData = res->getString("BinData");
		ProtocolMemReader mr(oldBinData->c_str(),oldBinData->length());
		SGE_DBPlayerData_Old oldInst;
		oldInst.deserialize(&mr);
		oldInst.instId_ =  res->getInt("PlayerGuid");
		oldInst.instName_	= res->getString("PlayerName").c_str();
		oldInst.freeze_ = res->getInt("Freeze");
		oldInst.seal_ =  res->getInt("Seal");

		SGE_DBPlayerData newInst;

		TranslationDBData(oldInst,newInst);
		
		newInst.babies_.clear();
		newInst.employees_.clear();
		
		std::auto_ptr< sql::PreparedStatement > prep_stmt(pmysql_->prepareStatement(update_code));

		enum {BUFFER_SIZE = 1024*1024};
		char *buffer = new char[BUFFER_SIZE];
		
		ProtocolMemWriter mw(buffer,BUFFER_SIZE);
		newInst.serialize(&mw);
		
		sql::SQLString newBinString(buffer,mw.length());

		prep_stmt->setString(1,newBinString);
		prep_stmt->setInt(2,newInst.versionNumber_);
		prep_stmt->setInt(3,newInst.instId_);
		prep_stmt->execute();
		
		int cur_sec = ACE_OS::gettimeofday().sec() - sec ;
		++trans_rows_;
		process(trans_rows_,total_rows_,cur_sec);
	
	}
	ACE_OS::printf("\n");

	check();
DB_EXEC_UNGUARD_RETURN
};

void RefreshSql::check()
{
DB_EXEC_GUARD
	static const char * select_code = "SELECT COUNT(*) as count FROM Player WHERE Player.VersionNumber!=?";
	std::auto_ptr< sql::PreparedStatement > prep_stmt(pmysql_->prepareStatement(select_code));
	prep_stmt->setInt(1,(int)(VERSION_NUMBER));
	std::auto_ptr< sql::ResultSet > res(prep_stmt->executeQuery());

	if(res->next())
	{
		int count = res->getInt("count");
		if(0!=count)
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("can not convert rows = %d\n") , count));
		}
		else
		{
			ACE_DEBUG((LM_INFO,ACE_TEXT("all rows convert complate \n")));
		}
	}

DB_EXEC_UNGUARD_RETURN
}

void RefreshSql::process(int current,int total, int sec )
{
	
	float ratio = (float)current / float(total);	///<比例
	
	enum{BAR_LENGTH=50};
	
	char buffer[BAR_LENGTH+1] = {'\0'};
	int i = 0;
	for(int length=BAR_LENGTH*ratio; i<length;++i)
	{
		buffer[i] = '=';
	}	
	
	for(;i<BAR_LENGTH;++i)
	{
		buffer[i] = ' ';
	}

	buffer[BAR_LENGTH] = '\0';

	ACE_OS::printf("\r%d%%[%s] time=%d",(int)(ratio*100),(char*)buffer,sec);
	
	ACE_OS::fflush(stdout);
	
}

bool RefreshSql::open_mysql()
{
DB_EXEC_GUARD
	sql::Driver *pdriver = sql::mysql::get_driver_instance();
	if(NULL==pdriver)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("pdriver==NULL\n")));
		return false;
	}
	pmysql_ =pdriver->connect(params_[HOST],params_[USER],params_[PWD]);
	if(NULL==pmysql_)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("pmydql==NULL\n")));
		return false;
	}
	pmysql_->setSchema(params_[SCHEMA]);
	pdriver=NULL;
	return true;
DB_EXEC_UNGUARD_RETURN
}

bool RefreshSql::close_mysql()
{
DB_EXEC_GUARD
		if(NULL == pmysql_)
		{
			return true;
		}
		pmysql_->close();
		pmysql_=NULL;

		return true;
DB_EXEC_UNGUARD_RETURN
}

int main(int argc, char **argv)
{
	
	RefreshSql worker;
	worker.init(argc,argv);
	worker.run();
	worker.fini();
	return 0;
}