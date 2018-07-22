virtual bool account(SGE_Account& data);
virtual bool login(SGE_Login& data);
virtual bool order(SGE_Order& data);
virtual bool role(SGE_LogRole& data);
virtual bool playersay(U32 senderId, std::string& senderName, COM_Chat& chat);
virtual bool playerTrack(SGE_LogProduceTrack& data);
virtual bool playerUIBehavior(SGE_LogUIBehavior& core);
