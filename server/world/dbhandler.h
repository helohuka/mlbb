
#ifndef __DB_HANDLER_H__
#define __DB_HANDLER_H__

#include "config.h"

class DBHandler 
	: public BINConnection < SGE_World2DBStub , SGE_DB2WorldProxy >
	, SGE_DB2WorldProxy
{
public:
	SINGLETON_FUNCTION(DBHandler);
public:
	DBHandler():BINConnection(0XFFFFFF,0XFFFFFF),isConnect_(false){setProxy(this);}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);
	int handleReceived(void* data, size_t size){
		

		int ret = BINConnection < SGE_World2DBStub , SGE_DB2WorldProxy >::handleReceived(data,size);
		if(-1 == ret){
			ACE_DEBUG((LM_ERROR,"DB Recv error \n"));
			return size;
		}
		return ret;
	}
	int open(void * /* = 0 */);
public:
	#include "SGE_DB2WorldMethods.h"
public:
	bool isConnect_;
};

#endif