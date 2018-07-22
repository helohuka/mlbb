#include "config.h"
#include "env.h"
#include "logTask.h"
#include "account.h"
#include "player.h"
#include "worldserv.h"
#include "json/json.h"


//TODO 所有调用全部返回 需要优化

LogTask::LogTask()
:runing_(false)
,curl_(NULL){
}
LogTask::~LogTask(){
	fini();
}
int LogTask::init()
{
	//curl_  = curl_easy_init();
	//if(NULL == curl_)
	//{
	//	/// handle 初始化失败
	//	ACE_DEBUG((LM_INFO, ACE_TEXT("curl_easy_init() = NULL is failed\n")));
	//	return -1;
	//}
	//curl_easy_setopt(curl_, CURLOPT_TIMEOUT, 2L);    /*timeout 30s,add by edgeyang*/
	//curl_easy_setopt(curl_, CURLOPT_NOSIGNAL, 1L);    /*no signal,add by edgeyang*/
	//runing_ = true;

	return activate();
}

int LogTask::fini()
{
	/*runing_ = false;
	if(NULL != curl_)
	{
		curl_easy_cleanup(curl_);

		curl_ = NULL;
	}*/
	return 0;
}

int LogTask::svc()
{
	//Logger::instance()->init();
	//enum{
	//	POST_INTERVAL = 5
	//};
	//while(runing_){
	//		int httpCode = -1;
	//		for(int i=0; i<5; ++i){
	//			httpCode = curlPost();
	//			if(httpCode == 0)
	//				break;
	//		}
	//		if(httpCode != 0){
	//			//ACE_DEBUG((LM_ERROR,"CURL http code = %d\n",httpCode));
	//		}
	//		ACE_OS::sleep(1);		
	//}
	return 0;
}

int LogTask::curlPost(){
	
	if(postQueue_.empty())
		return 0;
	//
	//ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	//std::string &req = postQueue_.front();
	//curl_easy_setopt(curl_, CURLOPT_URL,(Env::get<std::string>(V_LogServerHost) + PUSH_LOG).c_str());
	//curl_easy_setopt(curl_, CURLOPT_POSTFIELDS,req.c_str());
	//curl_easy_setopt(curl_, CURLOPT_POST, 1); 
	//curl_easy_setopt(curl_, CURLOPT_VERBOSE, 0);
	//curl_easy_setopt(curl_, CURLOPT_CONNECTTIMEOUT , 2);		///<设置连接超时时间 10 秒
	//curl_easy_setopt(curl_, CURLOPT_WRITEFUNCTION,&LogTask::curlCallback);///<设置数据回调 

	//CURLcode result = curl_easy_perform(curl_);

	//switch(result)
	//{
	//case CURLE_OK:						///<一切安好 

	//	break;
	//case CURLE_UNSUPPORTED_PROTOCOL:	///<不支持协议URL头决定
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_UNSUPPORTED_PROTOCOL \n")));
	//	return -1;
	//case CURLE_COULDNT_CONNECT:			///<不能连接到主机或者代理
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_COULDNT_CONNECT \n")));
	//	return -1;
	//case CURLE_REMOTE_ACCESS_DENIED:	///<访问被拒绝
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_REMOTE_ACCESS_DENIED \n")));
	//	return -1;
	//case CURLE_HTTP_RETURNED_ERROR:		///<http返回错误
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_HTTP_RETURNED_ERROR \n")));
	//	return -1;
	//default:
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform unkown error code(%d) \n"), (int)result));
	//	return -1;
	//}

	//enum { HTTP_RESPONSE_SUCCESS = 200U };

	//long httpCode = 0;
	//curl_easy_getinfo(curl_,CURLINFO_RESPONSE_CODE,&httpCode);	///<获得HTTP 返回值,目前只要不等于 200 就返回-1再次请求

	//if(HTTP_RESPONSE_SUCCESS!=httpCode)
	//{
	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl CURLINFO_RESPONSE_CODE (%q)\n"),httpCode));
	//	return httpCode;
	//}

	postQueue_.pop();
	return 0;
}

size_t LogTask::curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p){
	return size * nmemb;
}

void LogTask::pushAccountLog(Account *acc){
	return;
	if(acc == NULL){
		return ;
	}
	Json::Value root(Json::objectValue);
	root["PFID"] = acc->sdkInfo_.pfId_;
	root["PFName"] = acc->sdkInfo_.pfName_;
	root["AccountName"] = acc->username_;
	root["CreateTime"] = TimeFormat::StrLocalTime(acc->createtime_);
	root["MAC"] = acc->sdkInfo_.mac_;
	root["IDFA"] = acc->sdkInfo_.idfa_;
	root["IP"] = acc->ipaddr_;
	root["DeviceType"] = acc->sdkInfo_.deviceType_;
	Json::FastWriter jwriter;
	std::stringstream ss;
	ss << "type=account&value=" << jwriter.write(root);
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	postQueue_.push(ss.str());
}

void LogTask::pushLoginLog(Player *player){
	return;
	if(player == NULL)
		return;
	
	Json::Value root(Json::objectValue);
	root["ServerId"] = root["FirstServerId"] =  player->serverId_;
	{
		std::stringstream ss;
		ss << player->serverId_ << "|" << player->getGUID();
		root["RoleId"] = ss.str();
	}
	root["LoginTime"] = TimeFormat::StrLocalTime(player->loginTime_);
	root["LogoutTime"] = TimeFormat::StrLocalTime(player->loginTime_);
	root["FirstTime"] = TimeFormat::StrLocalTime(player->createTime_);
	root["RoleFirstTime"] = TimeFormat::StrLocalTime(player->createTime_);
	Account * acc = player->account_;
	if(acc){
		root["PFID"]		= acc->sdkInfo_.pfId_;
		root["PFName"]		= acc->sdkInfo_.pfName_;
		root["MAC"]			= acc->sdkInfo_.mac_;
		root["IDFA"]		= acc->sdkInfo_.idfa_;
		root["DeviceType"]	= acc->sdkInfo_.deviceType_;
		root["AccountName"]	= acc->username_;
		root["IP"]			= acc->ipaddr_;
	}

	Json::FastWriter jwriter;
	std::stringstream ss;
	ss << "type=login&value=" << jwriter.write(root);
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	postQueue_.push(ss.str());
}

void LogTask::pushLoginLog(Account *acc){
	return;
	if(acc == NULL)
		return;
	if(acc->client_ == NULL)
		return;
	Json::Value root(Json::objectValue);
	root["ServerId"] = root["FirstServerId"] =  acc->client_->serverId_;
	root["LoginTime"] = TimeFormat::StrLocalTime(acc->logintime_);
	root["LogoutTime"] = TimeFormat::StrLocalTime(acc->logintime_);
	root["PFID"]		= acc->sdkInfo_.pfId_;
	root["PFName"]		= acc->sdkInfo_.pfName_;
	root["MAC"]		= acc->sdkInfo_.mac_;
	root["IDFA"]		= acc->sdkInfo_.idfa_;
	root["DeviceType"] = acc->sdkInfo_.deviceType_;
	root["AccountName"]	= acc->username_;
	root["IP"]		= acc->ipaddr_;

	Json::FastWriter jwriter;
	std::stringstream ss;
	ss << "type=login&value=" << jwriter.write(root);
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	postQueue_.push(ss.str());
}

void LogTask::pushOrderLog(Account *acc, int32 playerId, int32 playerLevel, std::string const &orderId, int32 payment, std::string const &payTime){
	return;
	if(acc == NULL)
		return;
	SGE_ContactInfoExt * info = WorldServ::instance()->findContactInfoExt(playerId);
	if(NULL == info)
		return ;
	Json::Value root(Json::objectValue);
	root["PFID"] = acc->sdkInfo_.pfId_;
	root["PFName"] = acc->sdkInfo_.pfName_;
	root["AccountName"] = acc->username_;
	root["Payment"] = payment;
	root["PayTime"] = payTime;
	root["OrderId"] = orderId;
	root["ServerId"] =info->serverid_;
	{
		std::stringstream ss;
		ss << info->serverid_ << "|" << playerId;
		root["RoleId"] = ss.str();
	}
	root["RoleLevel"] = playerLevel;
	Json::FastWriter jwriter;
	std::stringstream ss;
	ss << "type=order&value=" << jwriter.write(root);
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	postQueue_.push(ss.str());
}

void LogTask::pushRoleLog(std::vector<SGE_ContactInfoExt*> &infos){
	return;
	std::vector<std::string> strpost;

	Json::Value obj(Json::arrayValue);
	std::string strtime = TimeFormat::StrLocalTimeNow("%04d-%02d-%02d");
	for(size_t i=0 ,c = 0; i<infos.size(); ++i, ++c){

		SGE_ContactInfoExt* p = infos[i];
		if(p == NULL)
			continue;
		Json::Value root(Json::objectValue);
		root["ServerId"] = root["FirstServerId"] =infos[i]->serverid_;

		root["AccountName"] = p->accName_;
		root["PFID"]	= p->pfid_;
		root["PFName"]	= p->pfid_;

		root["CacheDate"]	= strtime;
		root["RoleFirstDate"]= TimeFormat::StrLocalTime(p->rolefirst_,"%04d-%02d-%02d");
		root["RoleLastDate"]= TimeFormat::StrLocalTime(p->rolelast_,"%04d-%02d-%02d");
		{
			std::stringstream ss;
			ss << infos[i]->serverid_ << "|" <<  p->instId_;
			root["RoleId"] = ss.str();
		}
		root["RoleLevel"]	= p->level_;
		root["Gold"]	= p->gold_;
		root["Diamond"]	= p->magicgold_;
		root["Vip"]	= p->vip_;
		root["CE"]	= p->ff_;

		obj.append(root);
		
		if(c > 100){
			c = 0;
			Json::FastWriter jwriter;
			std::stringstream ss;
			ss << "type=role&value=" << jwriter.write(obj);
			strpost.push_back(ss.str());
			obj = Json::Value(Json::arrayValue);
		}
	}

	{
		std::stringstream ss;
		Json::FastWriter jwriter;
		ss << "type=role&value=" << jwriter.write(obj);
		strpost.push_back(ss.str());
	}

	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
	for(size_t i=0; i<strpost.size(); ++i)
		postQueue_.push(strpost[i]);
}

