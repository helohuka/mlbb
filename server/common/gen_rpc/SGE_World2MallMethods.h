virtual bool fetchSell(S32 playerid, COM_SearchContext& context);
virtual bool fetchMySell(S32 playerid);
virtual bool fetchSelledItem(S32 playerId);
virtual bool sell(COM_SellItem& item);
virtual bool unSell(S32 playerid, S32 sellid);
virtual bool buy(SGE_BuyContent& content);
virtual bool insertSelledItem(COM_SelledItem& item);
