virtual bool fetchSellOk(S32 playerid, std::vector< COM_SellItem >& items, S32 totalSize);
virtual bool fetchMySellOk(S32 playerid, std::vector< COM_SellItem >& items);
virtual bool fetchSelledItemOk(S32 playerId, std::vector< COM_SelledItem >& items);
virtual bool sellOk(S32 playerid, COM_SellItem& item);
virtual bool unSellOk(S32 playerid, S32 sellid);
virtual bool buyOk(S32 playerid, COM_SellItem& item);
virtual bool buyFail(S32 playerid, ErrorNo errorno);
