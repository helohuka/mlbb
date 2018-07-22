
#ifndef __LOG_HANDLER_H__
#define __LOG_HANDLER_H__

class SGE_LogPorxyDummy{public:bool dispatch(ProtocolReader* reader){return true;}};
class LogHandler 
	: public BINConnection < SGE_LogStub , SGE_LogPorxyDummy >
{
public:
	SINGLETON_FUNCTION(LogHandler);
public:	
	LogHandler():isConnect_(false){}
	int handle_close(ACE_HANDLE handle, ACE_Reactor_Mask close_mask);

public:
	bool isConnect_;
};


#endif