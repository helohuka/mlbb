/************************************************************************/
// File		: SQLHelper.h       
// Author	: GYL
// Date		: 2013-6-1 12:30
/************************************************************************/

#ifndef __SQLHELPER_H__
#define __SQLHELPER_H__
#include "config.h"
///@defgroup Gmt
///@addgroup Gmt
///@{

///@brief 数据库处理 
#include "SqlCommon.h"
class SQLHelper
{
public:
	enum 
	{
		SUCCESS,
		DRIVER_FAIL,
		CONNECT_FAIL,
		EXCEPTION
	};
public:
	
	SQLHelper() : dbContro_(NULL){}
	~SQLHelper();
	
	int		Connect();
	DBC *getDBC(){return dbContro_;}
private:
	DBC	*dbContro_;
};
///@}

#endif