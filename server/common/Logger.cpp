//==============================================================================
/**
	@date:		2012:5:2  
	@file: 		logger.cpp
	@author: 	gyl
*/
//==============================================================================
#include "config.h"
#include "Logger.h"

LoggerCallBack::LoggerCallBack(){}

void LoggerCallBack::log(ACE_Log_Record &log_record)
{
	
	enum {
		TimeLength = 32,
		NewMsgLength = ACE_MAXLOGMSGLEN + 64
	};
	//获得时间
	time_t	curTime;
	ACE_OS::time(&curTime);	
	
	tm* curLocalTime = ACE_OS::localtime(&curTime);

	ACE_TCHAR strTime[TimeLength] = {'\0'} ;
	ACE_OS::snprintf(strTime , TimeLength -1 , "%04d-%02d-%02d %02d:%02d:%02d" , 
		curLocalTime->tm_year + 1900 , curLocalTime->tm_mon +1 , curLocalTime->tm_mday , curLocalTime->tm_hour , curLocalTime->tm_min , curLocalTime->tm_sec );
	 
	//基本的信息
	const ACE_TCHAR *oldMag = log_record.msg_data();
	//<总信息的长度
	
	

	ACE_TCHAR strNewMsg[NewMsgLength] = {'\0'};

	ACE_OS::snprintf(strNewMsg , NewMsgLength -1 ,  "%s  %s" , strTime , oldMag  );
	
	log_record.msg_data(strNewMsg);
};


Logger* Logger::instance()
{
	static Logger inst;
	return &inst;
}

void Logger::init()
{
	ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::MSG_CALLBACK);		
	ACE_Log_Msg::instance()->msg_callback(&callBack_);					
}


void  Logger::enableFileOut( const ACE_TCHAR* fileName,const ACE_TCHAR* dirName)
{
	fileName_ =fileName;
	dirName_  =dirName;
	std::string dirNameStr="";
	if (!dirName_.empty())
	{
		ACE_OS::mkdir(dirName_.c_str());
		dirNameStr=dirName_+"/"; 
	}

	time_t	curTime;
	ACE_OS::time(&curTime);	
	tm*		curLocalTime = ACE_OS::localtime(&curTime);	
	char	curTimeStr[1024]={0};
	sprintf(curTimeStr, "%04d-%02d-%02d-%02d-%02d-%02d",curLocalTime->tm_year+1900,curLocalTime->tm_mon+1,curLocalTime->tm_mday,curLocalTime->tm_hour,curLocalTime->tm_min,curLocalTime->tm_sec);

	std::string logFileName=dirNameStr+curTimeStr+fileName_;
	ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::OSTREAM);			
	ACE_Log_Msg::instance()->msg_ostream(&outFile_);					
	outFile_.open(logFileName.c_str());
 	
  	if (!outFile_.is_open())
  	{
		SRV_ASSERT(0);
	}
}

void Logger::finit()
{
	if(outFile_.is_open())
	{
		outFile_.flush();
		outFile_.close(); 
	}
}

void Logger::logPriorityMask(u_long logType)
{
	ACE_Log_Msg::instance()->priority_mask(logType, ACE_Log_Msg::PROCESS);
}
