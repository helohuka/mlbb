
#include "handler.h"
#include "config.h"
#include "HttpParser.h"
#include "gzip.h"
ClientHandler::ClientHandler()
{
}

ClientHandler::~ClientHandler(){
}

int
ClientHandler::open(void* p)
{
	int r = Connection::open(p);

	ACE_INET_Addr const &remote = getRemoteAddr();

	ACE_DEBUG((LM_INFO,ACE_TEXT("One client conneted at address(%s:%d)\n"),remote.get_host_addr(),remote.get_port_number()));

	return r;
}

int 
ClientHandler::handleReceived(void* data, size_t size)
{
	ACE_Message_Block mb((char*)data,size);
	mb.wr_ptr(size);
	HTTP_Request req;
	req.parse_request(mb);
	procPostData(req.data());
	return size;
}

int
ClientHandler::handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask)
{
	if(ACE_OS::last_error() == EWOULDBLOCK)
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("ACE_OS::last_error() == EWOULDBLOCK , Status = %d , Peer = %s\n"),status_,remoteAddr_.get_host_addr()));
		return 0;
	}
	ACE_DEBUG((LM_INFO,ACE_TEXT("Client close %s:%d Total Incoming=%Q,Outgoing-%Q\n"),getRemoteAddr().get_host_addr(),getRemoteAddr().get_port_number(),getTotalReadBytes(),getTotalWriteBytes()));
	status_ = Connection::RemoteClosed;
	return 0;
}

void ClientHandler::procPostData( const char* post )
{
	Json::Value json;
	Json::Reader reader;
	if(reader.parse(post,json)){
	}
	std::string str_type = json["Type"].asString();
	int16 type = ENUM(GMT_Protocol).getItemId(str_type);
	
	if(type<=GMT_None || type>=GMT_MAX)
	{
		result(-1,"post data format error");
		ACE_DEBUG((LM_ERROR, ACE_TEXT("post data format error\n")));
		return ;
	}
	
	ACE_DEBUG((LM_INFO,"GMT COMMAND ==> %s <==\n",post));

	switch(type)
	{
	case GMT_GMCommand:
		gmCommandModule(json);
		break;
	case GMT_Notice:
		gmNoticeModule(json);
		break;
	case GMT_InsertMail:
		gmInsertMailModule(json);
		break;
	case GMT_QueryPlayer:
		gmQueryRoleModule(json);
		break;
	case GMT_LoginActivity:
		gmLoginActivity(json);
		break;
	case GMT_7Days:
		break;
	case GMT_Cards:
		break;
	case GMT_ChargeTotal:
		gmChargeTotal(json);
		break;
	case GMT_ChargeEvery:
		gmChargeEvery(json);
		break;
	case GMT_DiscountStore:
		gmDiscountStore(json);
		break;
	case GMT_Foundation:
		break;
	case GMT_LoginTotal:
		break;
	case GMT_OnlineReward:
		break;
	case GMT_HotRole:
		gmHotRole(json);
		break;
	case GMT_SelfChargeTotal:
		break;
	case GMT_SelfChargeEvery:
		break;
	case GMT_ExtractEmployee:
		gmEmployeeActivity(json);
		break;
	case GMT_MinGiftBag:
		gmMinGiftbag(json);
		break;
	case GMT_DoScript:	//执行服务器命
		gmDoScript(json);
		break;
	case GNT_MakeOrder:		//模拟充值
		gmMakeOrder(json);
		break;
	case GMT_QueryRMB:
		//gmQueryRMB();
	case GMT_Zhuanpan:
		gmZhuanpan(json);
		break;
	case GMT_QueryDia:
		//gmQueryDia();
		break;
	case GMT_IntegralShop:
		gmIntegralshop(json);
		break;
	case GMT_QueryMoney:
		//gmQueryMoney();
		break;
	case GMT_QueryRoleList:
		gmQueryRoleList(json);
		break;
	default:
		break;
	}
}

void ClientHandler::result(int errorno, const char* errordesc){
	
	Json::Value jobject(Json::objectValue);
	jobject["error"] = Json::Value((Json::Int)errorno);
	jobject["desc"] = Json::Value(errordesc);

	Json::FastWriter jwriter;
	std::string jstring = jwriter.write(jobject);

	ACE_DEBUG((LM_INFO,"Result length %d\n",jstring.size()));

	std::stringstream ss;
	ss << "HTTP/1.0 200 OK\r\nContent-type: text/html\r\nContent-length: " << jstring.size() << "\r\n\r\n" << jstring;
	
	fill((void*)ss.str().c_str(),ss.str().size());
	flush();
	close();
}

void ClientHandler::resultgzip(int errorno, const char* errordesc){

	Json::Value jobject(Json::objectValue);
	jobject["error"] = Json::Value((Json::Int)errorno);
	jobject["desc"] = Json::Value(errordesc);

	Json::FastWriter jwriter;
	std::string jstring = jwriter.write(jobject);

	Bytef *zdata = new Bytef[jstring.size()];
	uLong zlen = jstring.size();
	int ret = gzcompress((Bytef*)jstring.c_str(),jstring.size(),zdata,&zlen);

	ACE_DEBUG((LM_INFO,"Result length %d:%d:%d\n",jstring.size(),ret,(int32)zlen));

	std::stringstream ss;
	ss << "HTTP/1.0 200 OK\r\nContent-type: text/html\r\n"<< "Content-Encoding: gzip\r\n" << "Content-length: " << zlen << "\r\n\r\n" ;
	
	fill((void*)ss.str().c_str(),ss.str().size());
	fill((void*)zdata,(size_t)zlen);
	flush();
	close();

	delete []zdata;
}

