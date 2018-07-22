
#ifndef __GATEWAY_H__
#define __GATEWAY_H__

#include "config.h"
#include "worldhandler.h"
namespace Json{
	class Value;
}
class GatewayServ
	:public ACE_Service_Object
{
	typedef std::vector<ClientHandler*> ClientList;
	typedef std::vector<IPV4ClientAccepter*> AccepterList;
public:
	static GatewayServ* instance() {static GatewayServ serv; return &serv;}

public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int handle_timeout (const ACE_Time_Value &current_time, const void *act);
	
	ClientHandler* getClientHandler(uint32 id);
	ClientHandler* getDeactiveHandler();
	int getActiveNum();
private:
	void connectWorld();
	void acceptClient();
	void acceptClient(int port);
	void acceptClient(std::string const& j);
public:
	uint64 totalOutgoing_;
	uint64 totalIncoming_;
	uint64 maxIncoming_;
	uint64 maxOutgoing_;
	uint64 meanIncoming_;
	uint64 meanOutgoing_;
	uint64 lastIncoming_;
	uint64 lastOutgoing_;
	uint64 startTime_;

	ClientList clientPool_;
	AccepterList accepters_;
public:
	
};

#endif