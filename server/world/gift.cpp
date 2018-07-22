//#include "config.h"
//#include "json/json.h"
//#include "gift.h"
//#include "worldserv.h"
//GiftTask::GiftTask()
//:runing_(false)
//,curl_(NULL){
//}
//GiftTask::~GiftTask(){
//	fini();
//}
//int GiftTask::init()
//{
//	curl_  = curl_easy_init();
//	if(NULL == curl_)
//	{
//		/// handle 初始化失败
//		ACE_DEBUG((LM_INFO, ACE_TEXT("curl_easy_init() = NULL is failed\n")));
//		return -1;
//	}
//	curl_easy_setopt(curl_, CURLOPT_TIMEOUT, 2L);    /*timeout 30s,add by edgeyang*/
//	curl_easy_setopt(curl_, CURLOPT_NOSIGNAL, 1L);    /*no signal,add by edgeyang*/
//	runing_ = true;
//
//	return activate();
//}
//
//int GiftTask::fini()
//{
//	runing_ = false;
//	if(NULL != curl_)
//	{
//		curl_easy_cleanup(curl_);
//
//		curl_ = NULL;
//	}
//	return 0;
//}
//int GiftTask::svc()
//{
//	Logger::instance()->init();
//
//	while(runing_){
//
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
//	}
//	return 0;
//}
//
//void GiftTask::pushAdded(std::string const &playerName, std::string const &cdkey,std::vector<std::string> &giftNames){
//	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
//	for(size_t i=0; i<postQueue_.size(); ++i){
//		if(postQueue_[i].playerName_ == playerName){
//			return;
//		}
//	}
//	Json::Value jroot(Json::objectValue);
//	jroot["PlayerName"] = playerName;
//	jroot["CDKey"] = cdkey;
//	jroot["GiftNames"] = Json::Value(Json::arrayValue);
//	for (size_t i = 0; i < giftNames.size(); ++i)
//	{
//		jroot["GiftNames"].append(Json::Value(giftNames[i].c_str()));
//	}
//	Json::FastWriter jwriter;
//	GiftReq req;
//	req.playerName_ = playerName;
//	req.postValue_ = jwriter.write(jroot);
//	postQueue_.push_back(req);
//}
//
//int GiftTask::curlPost(){
//	//ACE_Guard<ACE_Recursive_Thread_Mutex> guard(postMutex_);
//	//if(postQueue_.empty())
//	//	return 0;
//	//std::string req = std::string("json=") + postQueue_.back().postValue_;
//	//curl_easy_setopt(curl_, CURLOPT_URL,  (Env::get<std::string>(V_CenterServerHost) + CDKET_REQ).c_str());  
//	//curl_easy_setopt(curl_, CURLOPT_POSTFIELDS,req.c_str());
//	//curl_easy_setopt(curl_, CURLOPT_POST, 1); 
//	//curl_easy_setopt(curl_, CURLOPT_VERBOSE, 0);
//	//curl_easy_setopt(curl_, CURLOPT_CONNECTTIMEOUT , 2);		///<设置连接超时时间 10 秒
//	//curl_easy_setopt(curl_, CURLOPT_WRITEFUNCTION,&GiftTask::curlCallback);///<设置数据回调 
//
//	//CURLcode result = curl_easy_perform(curl_);
//
//	//switch(result)
//	//{
//	//case CURLE_OK:						///<一切安好 
//
//	//	break;
//	//case CURLE_UNSUPPORTED_PROTOCOL:	///<不支持协议URL头决定
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_UNSUPPORTED_PROTOCOL \n")));
//	//	return -1;
//	//case CURLE_COULDNT_CONNECT:			///<不能连接到主机或者代理
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_COULDNT_CONNECT \n")));
//	//	return -1;
//	//case CURLE_REMOTE_ACCESS_DENIED:	///<访问被拒绝
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_REMOTE_ACCESS_DENIED \n")));
//	//	return -1;
//	//case CURLE_HTTP_RETURNED_ERROR:		///<http返回错误
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform CURLE_HTTP_RETURNED_ERROR \n")));
//	//	return -1;
//	//default:
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl perform unkown error code(%d) \n"), (int)result));
//	//	return -1;
//	//}
//
//	//enum { HTTP_RESPONSE_SUCCESS = 200U };
//
//	//long httpCode = 0;
//	//curl_easy_getinfo(curl_,CURLINFO_RESPONSE_CODE,&httpCode);	///<获得HTTP 返回值,目前只要不等于 200 就返回-1再次请求
//
//	//if(HTTP_RESPONSE_SUCCESS!=httpCode)
//	//{
//	//	//ACE_DEBUG((LM_ERROR , ACE_TEXT("curl CURLINFO_RESPONSE_CODE (%q)\n"),httpCode));
//	//	return httpCode;
//	//}
//
//	postQueue_.pop_back();
//	return 0;
//}
//
//ACE_Recursive_Thread_Mutex	GiftTask::resultMutex_;
//std::vector<GiftResult>		GiftTask::resultQueue_;
//
//size_t GiftTask::curlCallback(void *buffer , size_t size , size_t nmemb , void *user_p){
//	Json::Value json;
//	Json::Reader reader;
//	std::string jsonStr((char*)buffer,size*nmemb);
//	if(!reader.parse(jsonStr,json)){
//		return size * nmemb;
//	}
//	if(!json.isObject()){
//		ACE_DEBUG((LM_ERROR,"if(!json.isObject())\n"));
//		return size * nmemb;
//	}
//	if(!json["PlayerName"].isString()){
//		return size * nmemb;
//	}
//	GiftResult result;
//	result.playerName_ = json["PlayerName"].asString();
//
//	if(!json["GiftName"].isString()){
//		return size * nmemb;
//	}
//	result.giftName_ = json["GiftName"].asString();
//	if(!json["ErrorDesc"].isString()){
//		return size * nmemb;
//	}
//	result.err_ = json["ErrorDesc"].asString();
//
//	if(!json["Items"].isArray()){
//		return size * nmemb;
//	}
//	for(Json::UInt i =0 ; i<json["Items"].size(); ++i){
//		if(!json["Items"][i].isObject()){
//			return size * nmemb;
//		}
//		if(!json["Items"][i]["ItemId"].isInt()){
//			return size * nmemb;
//		}
//		if(!json["Items"][i]["ItemNum"].isInt()){
//			return size * nmemb;
//		}
//		COM_GiftItem item;
//		item.itemId_ = json["Items"][i]["ItemId"].asInt();
//		item.itemNum_ = json["Items"][i]["ItemNum"].asInt();
//		result.items_.push_back(item);
//	}
//	result.diamond_ = json["Diamond"].asInt();
//	ACE_Guard<ACE_Recursive_Thread_Mutex> guard(resultMutex_);
//
//	resultQueue_.push_back(result);
//
//	return size * nmemb;
//}
//
//void GiftTask::updateResult(){
//	/*ACE_Guard<ACE_Recursive_Thread_Mutex> guard(resultMutex_);
//	if(resultQueue_.empty())
//		return;
//	for(size_t i=0; i<resultQueue_.size(); ++i){
//		Player*p = Player::getPlayerByName(resultQueue_[i].playerName_);
//		if(!p){
//			continue;
//		}
//		int stte = ENUM(ErrorNo).getItemId(resultQueue_[i].err_);
//		if(stte == -1){
//			CALL_CLIENT(p,errorno(EN_Idgenhas));
//		}
//		if((ErrorNo)stte == EN_None)
//			p->giftaward(resultQueue_[i].giftName_,resultQueue_[i].items_,resultQueue_[i].diamond_);
//		else if((ErrorNo)stte != EN_Max)
//			CALL_CLIENT(p,errorno((ErrorNo)stte));
//	}*/
//	resultQueue_.clear();
//}
