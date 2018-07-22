
#include "Channel.h"
#include "ChannelConnection.h"

Channel::Channel():
guid_(0),
conn_(NULL)
{
}

Channel::~Channel()
{
}

bool Channel::isValid()
{
	return (conn_ == NULL)?false:true;
}

void Channel::fillBegin()
{
	if(conn_)
		conn_->initChannelSendingData(this);
}

void Channel::fill(void* data, size_t size)
{
	if(conn_)
		conn_->fillSendingData(data, size);
}

void Channel::fillEnd()
{
	if(conn_)
		conn_->flushSendingData();
}

bool Channel::handleClose()
{
	conn_ = NULL;
	return true;
}
