#include "lastOrder.h"
#include "CSVParser.h"
#include "TokenParser.h"
std::map<std::string, LastOrderTable::Data*> LastOrderTable::data_;
bool LastOrderTable::load(char const *fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	clear();
	for(size_t row=0; row<csv.get_records_counter(); ++row){
		Data* pData = new Data();
		pData->pfId_ = csv.get_string(row,"PFID");
		pData->pfName_ = csv.get_string(row,"PFNAME");
		pData->orderId_ = csv.get_string(row,"ORDERID");
		pData->accountId_= csv.get_string(row,"ACCOUNTID");
		pData->payment_ = csv.get_int(row,"PAYMENT");
		
		if(data_[pData->accountId_]){
			data_[pData->accountId_]->payment_ += pData->payment_;
			delete pData;
		}else {
			data_[pData->accountId_] = pData;
		}
	}
	return true;
}
void LastOrderTable::clear(){
}
const LastOrderTable::Data* LastOrderTable::getDataByAccountName(std::string & accountName){
	if(data_.find(accountName) == data_.end())
		return NULL;
	return data_[accountName];
}
