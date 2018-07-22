
#include "config.h"
#include "ChannelConnection.h"
#include "Channel.h"
ChannelConnection::ChannelConnection(bool isC , size_t rbSize, size_t sbSize/*, size_t bktSize*/):
Connection(rbSize,sbSize),
isConnector_(isC),
guidGen_(1),
msgLen_(NULL)/*,
nodes_(NULL),
bktSize_(bktSize),
nodeNum_(0)*/
{
	// allocate 16k memory buckets.
	//nodes_ = (Node**)ACE_OS::malloc(getBucketSize()*sizeof(Node*));
	//ACE_OS::memset(nodes_, 0, getBucketSize()*sizeof(Node*));
}

ChannelConnection::~ChannelConnection()
{
	// close all channels.
	/*std::vector<Channel*> channels;
	getAllChannels(channels);
	for(size_t i = 0; i < channels.size(); i++)
		disconnect(channels[i]);*/
	// free buckets.
	//ACE_OS::free(nodes_);
	for (NodeContainerType::iterator i=nodes_.begin(); i!=nodes_.end();++i)
	{
		disconnect(i->second);
	}

	nodes_.clear();
}

void ChannelConnection::connect(Channel* c)
{
	ACE_ASSERT(isConnector_);
	// 首先关闭channel.
	
	if (NULL == c)
	{
		ACE_ASSERT(0);
	}
	
	c->conn_ = this;
	c->guid_ = guidGen_++;
	///@todo 没有处理guid重复问题.
	addChannel(c->guid_, c);
	
	// Header.
	char type = (char)PT_ChanConnect;
	fill(&type, sizeof(char));
	fill(&(c->guid_), sizeof(unsigned int));
	// 写入一个临时的长度.
	msgLen_ = sendBuf_.wr_ptr();
	unsigned int len = 0;
	fill(&len, sizeof(unsigned int));
	flush();
}

void ChannelConnection::disconnect(Channel*c)
{
	SRV_ASSERT(c);
	if(c->conn_ != this){
		ACE_DEBUG((LM_INFO,"Deconnect channel %d conn is not this %d\n",c->guid_,(int64)c->conn_));
		return;
		//SRV_ASSERT(0);
	}
	//ACE_DEBUG((LM_INFO,"Deconnect channel %d\n",c->guid_));
	char type = (char)PT_ChanDisconnect;
	fill(&type,sizeof(char));
	fill(&(c->guid_), sizeof(unsigned int));
	msgLen_ = sendBuf_.wr_ptr();
	unsigned int len = 0;
	fill(&len, sizeof(unsigned int));
	flush();
	// 移除这个channel.
	c->conn_ = NULL;
	SRV_ASSERT(removeChannel(c->guid_));
	c->handleClose();
}

void ChannelConnection::fill(void* data, size_t size)
{
	Connection::fill(data, size);
}

void ChannelConnection::flush()
{
	*((unsigned int*)msgLen_) = (unsigned int)((sendBuf_.wr_ptr() - msgLen_) - sizeof(unsigned int));
	// Send whole message.
	Connection::flush();
}

void ChannelConnection::initChannelSendingData(Channel* c)
{
	// Header.
	char type = (char)PT_ChanData;
	unsigned int chId = c->guid_;
	fill(&type, sizeof(char));
	fill(&chId, sizeof(unsigned int));
	// 写入一个临时的长度.
	msgLen_ = sendBuf_.wr_ptr();
	unsigned int len = 0;
	fill(&len, sizeof(unsigned int));
}

void ChannelConnection::initGlobalSendingData()
{
	// Header.
	char type = (char)PT_GlobalData;
	fill(&type, sizeof(char));
	// 写入一个临时的长度.
	msgLen_ = sendBuf_.wr_ptr();
	unsigned int len = 0;
	fill(&len, sizeof(unsigned int));
}

void ChannelConnection::initBCSendingData(std::set<Channel*>& channels)
{
	// Header.
	char type = (char)PT_ChanBCData;
	fill(&type, sizeof(char));
	// 临时填入channel数量，并保留这个数量的指针，已备接下来对channel有效性检查时修改.
	unsigned int* pNumChan = (unsigned int*)sendBuf_.wr_ptr();
	unsigned int numChan = 0;
	fill(&numChan, sizeof(unsigned int));
	// Channels.
	for(std::set<Channel*>::iterator iter = channels.begin(); iter != channels.end(); ++iter)
	{
		Channel* ch = *iter;
		if(ch == NULL || ch->conn_ != this)
			continue;
		unsigned int guid = ch->guid_;
		fill(&guid, sizeof(unsigned int));
		// 递增有效的channel数量.
		*pNumChan = *pNumChan + 1;
	}
	// 写入一个临时的长度.
	msgLen_ = sendBuf_.wr_ptr();
	unsigned int len = 0;
	fill(&len, sizeof(unsigned int));
}

void ChannelConnection::fillSendingData(void* data, size_t size)
{
	fill(data, size);
}

void ChannelConnection::flushSendingData()
{
	*((unsigned int*)msgLen_) = (unsigned int)((sendBuf_.wr_ptr() - msgLen_) - sizeof(unsigned int));
	// Send whole message.
	flush();
}

bool ChannelConnection::handleGlobalData(void* data, size_t size)
{
	return true;
}

int ChannelConnection::handleReceived(void* data, size_t size)
{
	int handled = 0;
	while(1)
	{
		char* curData = (char*)data + handled;
		size_t curDataSize = size - handled;

		// 解析消息头
		if(curDataSize < sizeof(char))
			return handled;
		char* type = (char*)curData;

		// 判断协议类型.
		switch(*type)
		{
		case PT_ChanConnect:
			{
				if(isConnector_)
				{
					// 连接模式不应该受到这个协议.
					return -1;
				}

				// 检查消息头完整性.
				size_t hdrSize = sizeof(char) + sizeof(unsigned int)*2;
				if(curDataSize < hdrSize)
					return handled;
				unsigned int* chId	= (unsigned int*)(curData+1);
				unsigned int* len	= chId+1;
				
				// 检查数据完整性.
				if(curDataSize < hdrSize + *len)
					return handled;
				// 处理消息
				if(findChannel(*chId))
				{
					// 这个channel已经存在，出现错误.
					return -1;
				}
				Channel* ch = accept();
				if(ch)
				{
					ch->conn_ = this;
					ch->guid_ = *chId;
					addChannel(*chId, ch);
				}
				else 
					SRV_ASSERT(0);
				handled += hdrSize + *len;
				ACE_DEBUG((LM_DEBUG,"Connect channel %d\n",(*chId)));
			}
			break;
		case PT_ChanDisconnect:
			{
				size_t hdrSize = sizeof(char) + sizeof(unsigned int)*2;
				if(curDataSize < hdrSize)
					return handled;
				unsigned int* chId	= (unsigned int*)(curData+1);
				unsigned int* len	= chId+1;

				// 检查数据完整性.
				if(curDataSize < hdrSize + *len)
					return handled;
				// 处理消息
				Channel* ch = findChannel(*chId);
				ACE_DEBUG((LM_DEBUG,"Deconnect channel %d \n",(*chId)));
				if(ch)
				{
					removeChannel(*chId);
					ch->handleClose();
				}
				else
				{
					ACE_DEBUG((LM_DEBUG,"Deconnect channel %d not found\n",(*chId)));
					/*return -1;*/
				}
				handled += hdrSize + *len;
			}
			break;
		case PT_ChanData:
			{
				// 检查消息头完整性.
				size_t hdrSize = sizeof(char) + sizeof(unsigned int)*2;
				if(curDataSize < hdrSize)
					return handled;
				unsigned int* chId	= (unsigned int*)(curData+1);
				unsigned int* len	= chId+1;

				// 检查数据完整性.
				if(curDataSize < hdrSize + *len)
					return handled;
				if(*len == 0){
					ACE_DEBUG((LM_ERROR,"PT_ChanData The length is 0 %d\n",(*chId)));
					return size;
				}
				// 处理消息
				Channel* ch = findChannel(*chId);
				if(ch)
				{
					if(!ch->handleReceived(curData+hdrSize, *len))
					{
						ACE_DEBUG((LM_ERROR,"Chan data recv error\n"));
						disconnect(ch);
					}
				}
				else 
				{
					ACE_DEBUG((LM_DEBUG,"Can not find channel (%d)\n",(*chId)));
				}
				handled += hdrSize + *len;
				//ACE_DEBUG((LM_DEBUG,"Handled size %d\n",handled));
			}
			break;
		case PT_ChanBCData:

			{
				// 检查消息头完整性.
				size_t hdrSize = sizeof(char) + sizeof(unsigned int);
				if(size - handled < hdrSize)
					return handled;
				unsigned int* numChan	= (unsigned int*)(curData+1);
				unsigned int* chanIds	= (unsigned int*)(curData + hdrSize);
				hdrSize += (*numChan)*sizeof(unsigned int) + sizeof(unsigned int);
				if(curDataSize < hdrSize)
					return handled;
				unsigned int* len		= (unsigned int*)(curData + hdrSize - sizeof(unsigned int));
				if(curDataSize < hdrSize + *len)
					return handled;
				// 处理消息.
				void* bcdata			= curData + hdrSize;
				for(unsigned int i = 0; i < *numChan; i++)
				{
					Channel* ch = findChannel(chanIds[i]);
					if(ch)
					{
						if(!ch->handleReceived(bcdata, *len))
						{
							ACE_DEBUG((LM_ERROR,"Chan bc data recv error\n"));
							disconnect(ch);
						}
					}
				}
				handled += hdrSize + *len;
				//ACE_DEBUG((LM_DEBUG,"BC Handled size %d num chan %d\n",handled,(*numChan)));
			}
			break;
		case PT_GlobalData:
			{
				// 检查完整性.
				size_t hdrSize = sizeof(char) + sizeof(unsigned int);
				if(curDataSize < hdrSize)
					return handled;
				unsigned int* len = (unsigned int*)(curData+1);
				if(curDataSize < hdrSize + *len)
					return handled;
				if(!handleGlobalData(curData+sizeof(char)+sizeof(unsigned int), *len))
					return -1;
				handled += hdrSize + *len;
			}
			break;
		default:
			{
				// 协议头错误，返回消息处理错误，断开连接.
				return -1;
			}
		}
	}

	// Never goes here.
	ACE_ASSERT(0);
	return handled;
}

bool ChannelConnection::addChannel(unsigned int key, Channel* value)
{
	SRV_ASSERT(key == value->getGuid());
	nodes_.insert(NodeContainerType::value_type(key,value));
	return true;
}

bool ChannelConnection::removeChannel(unsigned int key)
{
	NodeContainerType::iterator itr = nodes_.find(key) ;
	if(itr == nodes_.end()){
		ACE_DEBUG((LM_ERROR,"ChannalConnection remove node %d can not find\n",key));
		return true;
	}
	nodes_.erase(itr);
	return true;
}

Channel* ChannelConnection::findChannel(unsigned int key)
{
	//ACE_DEBUG((LM_INFO,"Find channel %d\n",key));
	/*unsigned int hid = getHash(key);
	Node* hash_n = nodes_[hid];
	while(hash_n)
	{
		if(hash_n->key_ == key)
			return hash_n->chan_;
		hash_n = hash_n->next_;
	}
	return NULL;*/
	NodeContainerType::iterator itr = nodes_.find(key) ;
	if(itr == nodes_.end())
		return NULL;
	return itr->second;
}
