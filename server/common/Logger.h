//==============================================================================
/**
	@date:		2012:5:2  
	@file: 		logger.h
	@author: 	gyl
*/
//==============================================================================
#ifndef __LOGMSGMGR_H__
#define __LOGMSGMGR_H__
#include "config.h" 

/**
 *	Desc: call back 回调对象
 * */
class LoggerCallBack:	public ACE_Log_Msg_Callback
{
public:
	/**
		Desc : 构造函数	 
	 * */
	LoggerCallBack();
	
	/**
	 *	Desc : log 实际的回调函数
	 * @param log_record log信息
	 * @override 
	 * @retrun void
	 * */
	void log (ACE_Log_Record&);
};

/**
 * Desc :<log日志的管理 包括输出定向  ， log 级别
 * */
class Logger
{


public:
	
	~Logger(){};

	static Logger* instance();
	/**
	 * <Desc :初始化
	 * */
    void init();
	/**
	 * <Desc :设置文件流目标
	 * */
	void enableFileOut ( const ACE_TCHAR* fileName,const ACE_TCHAR* dirName=NULL);

	void logPriorityMask(u_long logType=LM_ERROR|LM_INFO);
	/**
	 * <Desc :释放
	 * */
	void finit();
private:
	Logger(){}

	std::ofstream	  outFile_; //<日志文件
	LoggerCallBack	  callBack_;//<回调处理对象	
	std::string		  fileName_;//<文件名
	std::string		  dirName_; //<目录名
};

#endif