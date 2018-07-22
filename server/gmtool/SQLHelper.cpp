/************************************************************************/
// File		: SQLHelper.cpp       
// Author	: GYL
// Date		: 2013-6-1 12:30
/************************************************************************/

#include "ComEnv.h"
#include "SQLHelper.h"
#include "ace/Log_Msg.h"
#include "cppconn/driver.h"
#include "cppconn/resultset.h"
#include "cppconn/statement.h"
#include "cppconn/prepared_statement.h"

int SQLHelper::Connect()
{
	try{

			sql::Driver *driver = get_driver_instance();
			if(NULL==driver)
			{
				return DRIVER_FAIL;
			}
			dbContro_ =driver->connect(Env::get<const char*>(V_MysqlHost),Env::get<const char*>(V_MysqlUser),Env::get<const char*>(V_MysqlPassword));
			if(NULL==dbContro_)
			{
				return CONNECT_FAIL;
			}
			dbContro_->setSchema(Env::get<const char*>(V_DatabaseName));
			driver=NULL;
	}
	catch(std::exception& e)
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql exception failed (%s).\n"), e.what()));
		return EXCEPTION;
	}

	return SUCCESS;
}



SQLHelper::~SQLHelper()
{
DB_EXEC_GUARD
	if(dbContro_)
	{
		dbContro_->close();
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql close \n")));
	}
#if defined(USE_SQLITE)
	delete dbContro_;
#endif
	dbContro_ = NULL;
DB_EXEC_UNGUARD_RETURN
}
