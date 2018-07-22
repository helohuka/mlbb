#ifndef __PRODUCT_H__
#define __PRODUCT_H__
#include "config.h"
#include <mysql_connection.h>
#include <mysql_driver.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>
#include <ace/Get_Opt.h>
#include <ace/Date_Time.h>



class RefreshSql
{
	enum {OLD_VERSION = 0};
	enum
	{
		HOST,	///<地址
		USER,	///用户名
		PWD,	///用户密码
		SCHEMA,	///<数据库名
		MAX
	};



public:
	RefreshSql();
	void init(int argc,char ** argv);
	void fini();
	void run();
	void check();
	void process(int current , int total , int sec);
private:
	bool open_mysql();
	bool close_mysql();
private:
	RefreshSql(RefreshSql const &){}
	RefreshSql const & operator=(RefreshSql const &){}
	int							total_rows_;
	int							trans_rows_;
	sql::Connection*			pmysql_;
	std::vector<sql::SQLString>	params_;
};
#endif ///\end  #ifndef __PRODUCT_H__