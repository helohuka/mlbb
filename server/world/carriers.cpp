#include "config.h"
#include "carriers.h"
#include "json/json.h"
#include "HttpParser.h"
#include "player.h"
#include "Shop.h"
#include "account.h"
#include "dbhandler.h"

std::vector<AnyOauth*> LoginOauth::infos_;



LoginOauth::LoginOauth(){
}

LoginOauth::~LoginOauth(){
}

int
LoginOauth::open(void* p)
{
	int r = Connection::open(p);

	ACE_INET_Addr const &remote = getRemoteAddr();

	return r;
}

int 
LoginOauth::handleReceived(void* data, size_t size)
{
	ACE_Message_Block mb((char*)data,size);
	mb.wr_ptr(size);
	HTTP_Request req;
	req.parse_request(mb);
	proc(req.data());
	//mb.release();
	return size;
}

int
LoginOauth::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if(ACE_OS::last_error() == EWOULDBLOCK){
		return 0;
	}
	status_ = Connection::RemoteClosed;

	delete this;
	return 0;
}

void LoginOauth::proc( const char* post )
{
	ACE_DEBUG((LM_DEBUG,"%s\n",post));
	Json::Value json;
	Json::Reader reader;
	if(reader.parse(post,json)){
	}

	if(!json.isObject())
	{
		ACE_DEBUG((LM_ERROR,"if(!json.isObject())\n"));
		close();
		return;
	}

	AnyOauth info;

	if(json["status"] != "ok")
	{
		ACE_DEBUG((LM_ERROR,"if(json[status] != ok)\n"));
		close();
		return;
	}

	if(!json["common"].isObject()){
		ACE_DEBUG((LM_ERROR,"if(!json[common].isObject()){\n"));
		close();
		return;
	}

	info.pluginId_ = json["common"]["plugin_id"].asString();

	if(info.pluginId_.empty()){
		ACE_DEBUG((LM_ERROR,"if(plugin_id.empty())\n"));
		close();
		return;
	}

	info.userId_ = json["common"]["uid"].asString();

	if(info.userId_.empty()){
		ACE_DEBUG((LM_ERROR,"if(userid.empty())\n"));
		close();
		return;
	}
	
	info.pfId_ = json["common"]["channel"].asString();
	if(info.pfId_.empty()){
		close();
		return;
	}
	
	info.pfName_ = json["common"]["user_sdk"].asString();
	if(info.pfName_.empty())
	{
		ACE_DEBUG((LM_ERROR,"if(pfname.empty())\n"));	
		close();
		return;
	}

	info.timeout_ = 60 * 2;
	AnyOauth *pOauth = getOauthByUsername2(info.userId_);//NEW_MEM(AnyOauth,info);
	if (pOauth != NULL){
		pOauth->timeout_ = info.timeout_;
	}
	else {
		pOauth = NEW_MEM(AnyOauth, info);
		infos_.push_back(pOauth);
	}
	ACE_DEBUG((LM_INFO,"Login oauth %s %s %s \n",info.pluginId_.c_str(),info.userId_.c_str(),info.pfName_.c_str()));
	const char *  HTTP_HEADER = "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n\r\n";	
	std::string strHeader= HTTP_HEADER;
	strHeader += "ok";
	fill((void*)strHeader.c_str(),strHeader.size());
	flush();
	close();
}

AnyOauth* LoginOauth::getOauthByUsername(std::string & username){
	for (size_t i = 0; i < infos_.size(); ++i){
		if ((infos_[i]->pluginId_ + "=" + infos_[i]->userId_) == username){
			return infos_[i];
		}
	}

	ACE_DEBUG((LM_ERROR, "Can not find oauth cache %s \n", username.c_str()));
	return NULL;
}

AnyOauth* LoginOauth::getOauthByUsername2(std::string & username){
	for (size_t i = 0; i < infos_.size(); ++i){
		if ( infos_[i]->userId_ == username){
			return infos_[i];
		}
	}

	ACE_DEBUG((LM_ERROR, "Can not find oauth cache %s \n", username.c_str()));
	return NULL;
}


void LoginOauth::removeOauthByUsername(std::string const & username){
	for(size_t i=0; i<infos_.size(); ++i){
		if((infos_[i]->pluginId_ + "=" + infos_[i]->userId_) == username){
			AnyOauth *p = infos_[i];
			DEL_MEM(p);
			infos_.erase(infos_.begin() + i--);
		}
	}
}

void LoginOauth::timeOutOauths(float dt){
	
		for(size_t i=0; i<infos_.size(); ++i){
			infos_[i]->timeout_ -= dt;
			if(infos_[i]->timeout_ <= 0.F){
				AnyOauth *p = infos_[i];
				DEL_MEM(p);
				infos_.erase(infos_.begin() + i--);
			}
		}
	
}

PayNotify::PayNotify(){
}

PayNotify::~PayNotify(){
}

int
PayNotify::open(void* p){
	int r = Connection::open(p);
	ACE_INET_Addr const &remote = getRemoteAddr();
	ACE_DEBUG((LM_INFO,ACE_TEXT("One client conneted at address(%s:%d)\n"),remote.get_host_addr(),remote.get_port_number()));
	return r;
}

int 
PayNotify::handleReceived(void* data, size_t size)
{
	ACE_Message_Block mb((char*)data,size);
	mb.wr_ptr(size);
	HTTP_Request req;
	req.parse_request(mb);
	proc(req.data());
	return size;
}

int
PayNotify::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if(ACE_OS::last_error() == EWOULDBLOCK)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("ACE_OS::last_error() == EWOULDBLOCK , Status = %d , Peer = %s\n"),status_,remoteAddr_.get_host_addr()));
		return 0;
	}
	ACE_DEBUG((LM_INFO,ACE_TEXT("Client close %s:%d Total Incoming=%Q,Outgoing-%Q\n"),getRemoteAddr().get_host_addr(),getRemoteAddr().get_port_number(),getTotalReadBytes(),getTotalWriteBytes()));
	status_ = Connection::RemoteClosed;

	delete this;
	return 0;
}

void 
PayNotify::proc(const char* post)
{
	if (NULL == post)
	{
		ACE_DEBUG((LM_ERROR, "if (NULL == post)"));
		close();
		return;
	}

	if (strlen(post) > 65535) {
		ACE_DEBUG((LM_ERROR, "if (strlen(post) > 65535) {"));
		close();
		return;
	}

	Json::Value json;
	Json::Reader reader;
	if(reader.parse(post,json)){
	}

	if(!json.isObject()){
		close();
		return;
	}

	bool isPay = json["isPay"].asBool();

	if (isPay) {
		pay(json);
	}
	else {
		pay2(json);
	}
}


void PayNotify::pay(Json::Value &json) {

	std::string orderId = json["orderID"].asString();		//¶©µ¥ºÅ
	int32 payStatus = json["status"].asInt();			//Ö§¸¶×´Ì¬
	float amount = json["amount"].asDouble();				//Ö§¸¶½ð¶î
	int32 channelId = json["channelID"].asInt();
	std::string productId = json["productID"].asString();
	std::string userId = json["userID"].asString();
	std::string roleId = json["roleID"].asString();
	int32 payTime = json["time"].asInt();

	int32 prodId = String::Convert<int32>(productId);

	const Shop::Record *record = Shop::getRecordById(prodId);
	if (!record) {
		ACE_DEBUG((LM_INFO, "RMB by product code not found = %d\n", prodId));
		const char *  HTTP_HEADER = "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n\r\n";
		std::string strHeader = HTTP_HEADER;
		strHeader += "ok";
		fill((void*)strHeader.c_str(), strHeader.size());
		flush();
		close();
		return;
	}



	if (payStatus != 1) {
		const char *  HTTP_HEADER = "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n\r\n";
		std::string strHeader = HTTP_HEADER;
		strHeader += "ok";
		fill((void*)strHeader.c_str(), strHeader.size());
		flush();
		close();
		return;
	}



	Player* player = Player::getPlayerByInstId(String::Convert<uint32>(roleId));

	if (player != NULL) {
		if (!player->orderFromSDK(prodId, 1, orderId, "", amount)) {
			ACE_DEBUG((LM_ERROR, "Rechage error player %s ==>? shop %s \n", roleId.c_str(), productId.c_str()));
		}
	}
	else {
		SGE_OrderInfo info;
		info.productId_ = prodId;
		info.productCount_ = 1;
		info.amount_ = amount;
		info.orderId_ = orderId;
		info.payTime_ = payTime;

		DBHandler::instance()->insertLoseCharge(String::Convert<uint32>(roleId), info);
	}

	const char *  HTTP_HEADER = "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n\r\n";
	std::string strHeader = HTTP_HEADER;
	strHeader += "ok";
	fill((void*)strHeader.c_str(), strHeader.size());
	flush();
	close();
}

void PayNotify::pay2(Json::Value &json) {

	std::string orderId = json["orderID"].asString();		//¶©µ¥ºÅ
	float amount = json["amount"].asDouble();				//Ö§¸¶½ð¶î

	std::string userId = json["username"].asString();
	std::string roleId = json["roleID"].asString();
	int32 payTime = json["time"].asInt();

	Player* player = Player::getPlayerByInstId(String::Convert<uint32>(roleId));

	if (player != NULL) {
		player->addMagicCurrency(amount);
		ACE_DEBUG((LM_INFO, "Rechage PAY2 player %s ==>? RMB %d \n", roleId.c_str(), int(amount)));
	}

	const char *  HTTP_HEADER = "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n\r\n";
	std::string strHeader = HTTP_HEADER;
	strHeader += "ok";
	fill((void*)strHeader.c_str(), strHeader.size());
	flush();
	close();
}