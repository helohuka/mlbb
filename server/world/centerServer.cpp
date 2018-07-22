#include "config.h"
#include "centerServer.h"
#include "worldserv.h"
#include "json/json.h"
SyncCentreServerTask::SyncCentreServerTask()
:runing_(false)
,curl_(NULL)
,serverIdIndex_(0){
}
SyncCentreServerTask::~SyncCentreServerTask(){
	fini();
}
int SyncCentreServerTask::init()
{
	curl_  = curl_easy_init();
	if(NULL == curl_)
	{
		/// handle 初始化失败
		ACE_DEBUG((LM_INFO, ACE_TEXT("curl_easy_init() = NULL is failed\n")));
		return -1;
	}
	curl_easy_setopt(curl_, CURLOPT_TIMEOUT, 2L);    /*timeout 30s,add by edgeyang*/
	curl_easy_setopt(curl_, CURLOPT_NOSIGNAL, 1L);    /*no signal,add by edgeyang*/
	lastPostTime_ = WorldServ::instance()->curTime_;

	runing_ = true;

	return activate();
}

int SyncCentreServerTask::fini()
{
	runing_ = false;
	if(NULL != curl_)
	{
		curl_easy_cleanup(curl_);

		curl_ = NULL;
	}
	return 0;
}

int SyncCentreServerTask::svc()
{
	Logger::instance()->init();
	enum{
		POST_INTERVAL = 5
	};
	while(runing_){

		time_t curtime = WorldServ::instance()->curTime_;
		if(curtime - lastPostTime_ >= POST_INTERVAL)
		{
			syncCenterServer();

			int httpCode = -1;
			for(int i=0; i<5; ++i){
				httpCode = curlPost();
				if(httpCode == 0)
					break;
			}
			if(httpCode != 0){
				//ACE_DEBUG((LM_ERROR,"CURL http code = %d\n",httpCode));
			}
			lastPostTime_ = curtime;
		}
		ACE_OS::sleep(1);
	}
	return 0;
}

void SyncCentreServerTask::pushAdded(std::vector<SGE_ContactInfoExt> infos){
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(syncMutex_);
	syncAdd_.insert(syncAdd_.end(),infos.begin(),infos.end());
}

void SyncCentreServerTask::pushAdded(SGE_ContactInfoExt info){
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(syncMutex_);
	syncAdd_.push_back(info);
}
void SyncCentreServerTask::pushDeled(SGE_ContactInfoExt info){
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(syncMutex_);
	syncDel_.push_back(info);
}

void SyncCentreServerTask::syncCenterServer(){
	Json::UInt num = Player::store_.size();
	Json::Value jroot(Json::objectValue);
	jroot["capa"] = Json::Value(num);
	jroot["version"] = Json::Value(VERSION_NUMBER);
	jroot["added"] = Json::Value(Json::objectValue);
	jroot["deled"] = Json::Value(Json::objectValue);
	{

		int32 servId = 0 ;
		if(serverIdIndex_ >= WorldServ::instance()->inDoorIds_.size()){
			serverIdIndex_ = 0;
		}
		if(serverIdIndex_ < WorldServ::instance()->inDoorIds_.size()){
			servId = WorldServ::instance()->inDoorIds_[serverIdIndex_++];
		}
		ACE_Guard<ACE_Recursive_Thread_Mutex> guard(syncMutex_);

		for(size_t i=0; i<syncAdd_.size(); ++i){
			std::string &acc = syncAdd_[i].accName_;
			std::string &pla = syncAdd_[i].name_;
			if(servId != syncAdd_[i].serverid_){
				continue;
			}

			if(!jroot["added"].isMember(acc)){
				jroot["added"][acc] = Json::Value(Json::arrayValue);
			}

			jroot["added"][acc].append(Json::Value(pla));

			syncAdd_.erase(syncAdd_.begin() + i--);
		}

		for(size_t i=0; i<syncDel_.size(); ++i){
			std::string &acc = syncDel_[i].accName_;
			std::string &pla = syncDel_[i].name_;
			if(servId != syncDel_[i].serverid_){
				continue;
			}
			if(!jroot["deled"].isMember(acc)){
				jroot["deled"][acc] = Json::Value(Json::arrayValue);
			}

			jroot["deled"][acc].append(Json::Value(pla));
			
			syncDel_.erase(syncDel_.begin() + i--);
		}

		jroot["id"] = servId;

		if(servId == 0){
			return;
		}
	}

	Json::FastWriter jwriter;
	postQueue_.push(jwriter.write(jroot));
}

int SyncCentreServerTask::curlPost(){

	if(postQueue_.empty())
		return 0;
	std::string req = std::string("json=") + postQueue_.front();
	curl_easy_setopt(curl_, CURLOPT_URL, (Env::get<std::string>(V_CenterServerHost) + SERVER_INIT).c_str());  
	curl_easy_setopt(curl_, CURLOPT_POSTFIELDS,req.c_str());
	curl_easy_setopt(curl_, CURLOPT_POST, 1); 
	curl_easy_setopt(curl_, CURLOPT_VERBOSE, 0);
	curl_easy_setopt(curl_, CURLOPT_CONNECTTIMEOUT , 2);		///<设置连接超时时间 10 秒
	curl_easy_setopt(curl_, CURLOPT_WRITEFUNCTION,&SyncCentreServerTask::curlCallback);///<设置数据回调 

	CURLcode result = curl_easy_perform(curl_);

	switch(result)
	{
	case CURLE_OK:						///<一切安好 

		break;
	case CURLE_UNSUPPORTED_PROTOCOL:	///<不支持协议URL头决定
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_UNSUPPORTED_PROTOCOL \n")));
		return -1;
	case CURLE_COULDNT_CONNECT:			///<不能连接到主机或者代理
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_COULDNT_CONNECT \n")));
		return -1;
	case CURLE_REMOTE_ACCESS_DENIED:	///<访问被拒绝
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_REMOTE_ACCESS_DENIED \n")));
		return -1;
	case CURLE_HTTP_RETURNED_ERROR:		///<http返回错误
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_HTTP_RETURNED_ERROR \n")));
		return -1;
	default:
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform unkown error code(%d) \n"), (int)result));
		return -1;
	}

	enum { HTTP_RESPONSE_SUCCESS = 200U };

	long httpCode = 0;
	curl_easy_getinfo(curl_,CURLINFO_RESPONSE_CODE,&httpCode);	///<获得HTTP 返回值,目前只要不等于 200 就返回-1再次请求

	if(HTTP_RESPONSE_SUCCESS!=httpCode)
	{
		//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl CURLINFO_RESPONSE_CODE (%q)\n"),httpCode));
		return httpCode;
	}

	postQueue_.pop();
	return 0;
}

size_t SyncCentreServerTask::curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p){
	return size * nmemb;
}
