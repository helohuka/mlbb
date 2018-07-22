#ifndef __CLIENT_CHANNEL_H__
#define __CLIENT_CHANNEL_H__

//-------------------------------------------------------------------------
/**
 */
class ClientChannel
	:public Channel
{
public:
	ClientChannel(){SRV_ASSERT(0);}
	ClientChannel(int32 channelId):channelId_(channelId){}
	void attachHandle(int32 handleId){
		handleId_ = handleId;
	}
	void dettachHandle(){
		handleId_ = -1;
	}
	int32 getChannelId(){
		return channelId_;
	}

	bool handleReceived( void* data, size_t size );
	bool handleClose();
	void close();
private:
	int32 handleId_;
	int32 channelId_;
};

#endif