#ifndef __LAST_ORDER_H__
#define __LAST_ORDER_H__

#include "config.h"

class LastOrderTable{
public:
	struct Data{
		std::string pfId_;
		std::string pfName_;
		std::string orderId_;
		std::string accountId_;
		int32 payment_;
	};

	static bool load(char const *fn);
	static void clear();
	static const Data* getDataByAccountName(std::string & accountName);

	static std::map<std::string, Data*> data_;
};

#endif