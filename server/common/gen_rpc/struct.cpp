/* arpcc auto generated cpp file. */
#include "FieldMask.h"
#include "struct.h"
//=============================================================
SGE_OrderInfo::SGE_OrderInfo():
productId_(0)
,productCount_(0)
,amount_(0)
{}
void SGE_OrderInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((productId_==0)?false:true);
	__fm__.writeBit((productCount_==0)?false:true);
	__fm__.writeBit((amount_==0)?false:true);
	__fm__.writeBit(orderId_.length()?true:false);
	__fm__.writeBit(payTime_.length()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize productId_
	{
		if(productId_ != 0){
		__s__->writeType(productId_);
		}
	}
	// serialize productCount_
	{
		if(productCount_ != 0){
		__s__->writeType(productCount_);
		}
	}
	// serialize amount_
	{
		if(amount_ != 0){
		__s__->writeType(amount_);
		}
	}
	// serialize orderId_
	{
		if(orderId_.length()){
		__s__->writeType(orderId_);
		}
	}
	// serialize payTime_
	{
		if(payTime_.length()){
		__s__->writeType(payTime_);
		}
	}
}
bool SGE_OrderInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize productId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(productId_)) return false;
		}
	}
	// deserialize productCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(productCount_)) return false;
		}
	}
	// deserialize amount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(amount_)) return false;
		}
	}
	// deserialize orderId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(orderId_, 65535)) return false;
		}
	}
	// deserialize payTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(payTime_, 65535)) return false;
		}
	}
		return true;
}
void SGE_OrderInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize productId_
	ss << "\"productId_\":";
	{
		ss << (S64)productId_;
	}
	 ss << ",\n";
	// serialize productCount_
	ss << "\"productCount_\":";
	{
		ss << (S64)productCount_;
	}
	 ss << ",\n";
	// serialize amount_
	ss << "\"amount_\":";
	{
		ss << (double)amount_;
	}
	 ss << ",\n";
	// serialize orderId_
	ss << "\"orderId_\":";
	{
		ss << "\"" << orderId_ << "\"";
	}
	 ss << ",\n";
	// serialize payTime_
	ss << "\"payTime_\":";
	{
		ss << "\"" << payTime_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADLoginTotalContent::COM_ADLoginTotalContent():
totalDays_(0)
,status_(0)
{}
void COM_ADLoginTotalContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((totalDays_==0)?false:true);
	__fm__.writeBit(itemIds_.size()?true:false);
	__fm__.writeBit(itemStacks_.size()?true:false);
	__fm__.writeBit((status_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize totalDays_
	{
		if(totalDays_ != 0){
		__s__->writeType(totalDays_);
		}
	}
	// serialize itemIds_
	if(itemIds_.size())
	{
		size_t __len__ = (size_t)itemIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemIds_[i]);
		}
	}
	// serialize itemStacks_
	if(itemStacks_.size())
	{
		size_t __len__ = (size_t)itemStacks_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemStacks_[i]);
		}
	}
	// serialize status_
	{
		if(status_ != 0){
		__s__->writeType(status_);
		}
	}
}
bool COM_ADLoginTotalContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize totalDays_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(totalDays_)) return false;
		}
	}
	// deserialize itemIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemIds_[i])) return false;
		}
	}
	// deserialize itemStacks_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStacks_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemStacks_[i])) return false;
		}
	}
	// deserialize status_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(status_)) return false;
		}
	}
		return true;
}
void COM_ADLoginTotalContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize totalDays_
	ss << "\"totalDays_\":";
	{
		ss << (S64)totalDays_;
	}
	 ss << ",\n";
	// serialize itemIds_
	ss << "\"itemIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStacks_
	ss << "\"itemStacks_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStacks_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemStacks_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize status_
	ss << "\"status_\":";
	{
		ss << (S64)status_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADLoginTotal::COM_ADLoginTotal():
loginDays_(0)
,sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADLoginTotal::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((loginDays_==0)?false:true);
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize loginDays_
	{
		if(loginDays_ != 0){
		__s__->writeType(loginDays_);
		}
	}
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADLoginTotal::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize loginDays_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(loginDays_)) return false;
		}
	}
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADLoginTotal::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize loginDays_
	ss << "\"loginDays_\":";
	{
		ss << (S64)loginDays_;
	}
	 ss << ",\n";
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADChargeTotalContent::COM_ADChargeTotalContent():
currencyCount_(0)
,status_(0)
{}
void COM_ADChargeTotalContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((currencyCount_==0)?false:true);
	__fm__.writeBit(itemIds_.size()?true:false);
	__fm__.writeBit(itemStacks_.size()?true:false);
	__fm__.writeBit((status_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize currencyCount_
	{
		if(currencyCount_ != 0){
		__s__->writeType(currencyCount_);
		}
	}
	// serialize itemIds_
	if(itemIds_.size())
	{
		size_t __len__ = (size_t)itemIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemIds_[i]);
		}
	}
	// serialize itemStacks_
	if(itemStacks_.size())
	{
		size_t __len__ = (size_t)itemStacks_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemStacks_[i]);
		}
	}
	// serialize status_
	{
		if(status_ != 0){
		__s__->writeType(status_);
		}
	}
}
bool COM_ADChargeTotalContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize currencyCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(currencyCount_)) return false;
		}
	}
	// deserialize itemIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemIds_[i])) return false;
		}
	}
	// deserialize itemStacks_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStacks_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemStacks_[i])) return false;
		}
	}
	// deserialize status_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(status_)) return false;
		}
	}
		return true;
}
void COM_ADChargeTotalContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize currencyCount_
	ss << "\"currencyCount_\":";
	{
		ss << (S64)currencyCount_;
	}
	 ss << ",\n";
	// serialize itemIds_
	ss << "\"itemIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStacks_
	ss << "\"itemStacks_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStacks_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemStacks_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize status_
	ss << "\"status_\":";
	{
		ss << (S64)status_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADChargeTotal::COM_ADChargeTotal():
recharge_(0)
,sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADChargeTotal::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((recharge_==0)?false:true);
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize recharge_
	{
		if(recharge_ != 0){
		__s__->writeType(recharge_);
		}
	}
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADChargeTotal::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize recharge_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(recharge_)) return false;
		}
	}
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADChargeTotal::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize recharge_
	ss << "\"recharge_\":";
	{
		ss << (S64)recharge_;
	}
	 ss << ",\n";
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADDiscountStoreContent::COM_ADDiscountStoreContent():
price_(0)
,itemId_(0)
,discount_(0)
,buyLimit_(0)
{}
void COM_ADDiscountStoreContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((price_==0)?false:true);
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((discount_==0)?false:true);
	__fm__.writeBit((buyLimit_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize price_
	{
		if(price_ != 0){
		__s__->writeType(price_);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize discount_
	{
		if(discount_ != 0){
		__s__->writeType(discount_);
		}
	}
	// serialize buyLimit_
	{
		if(buyLimit_ != 0){
		__s__->writeType(buyLimit_);
		}
	}
}
bool COM_ADDiscountStoreContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize price_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(price_)) return false;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize discount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(discount_)) return false;
		}
	}
	// deserialize buyLimit_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(buyLimit_)) return false;
		}
	}
		return true;
}
void COM_ADDiscountStoreContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize price_
	ss << "\"price_\":";
	{
		ss << (S64)price_;
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize discount_
	ss << "\"discount_\":";
	{
		ss << (double)discount_;
	}
	 ss << ",\n";
	// serialize buyLimit_
	ss << "\"buyLimit_\":";
	{
		ss << (S64)buyLimit_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADDiscountStore::COM_ADDiscountStore():
sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADDiscountStore::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADDiscountStore::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADDiscountStore::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADChargeEveryContent::COM_ADChargeEveryContent():
currencyCount_(0)
,status_(0)
,count_(0)
{}
void COM_ADChargeEveryContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((currencyCount_==0)?false:true);
	__fm__.writeBit(itemIds_.size()?true:false);
	__fm__.writeBit(itemStacks_.size()?true:false);
	__fm__.writeBit((status_==0)?false:true);
	__fm__.writeBit((count_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize currencyCount_
	{
		if(currencyCount_ != 0){
		__s__->writeType(currencyCount_);
		}
	}
	// serialize itemIds_
	if(itemIds_.size())
	{
		size_t __len__ = (size_t)itemIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemIds_[i]);
		}
	}
	// serialize itemStacks_
	if(itemStacks_.size())
	{
		size_t __len__ = (size_t)itemStacks_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemStacks_[i]);
		}
	}
	// serialize status_
	{
		if(status_ != 0){
		__s__->writeType(status_);
		}
	}
	// serialize count_
	{
		if(count_ != 0){
		__s__->writeType(count_);
		}
	}
}
bool COM_ADChargeEveryContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize currencyCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(currencyCount_)) return false;
		}
	}
	// deserialize itemIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemIds_[i])) return false;
		}
	}
	// deserialize itemStacks_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStacks_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemStacks_[i])) return false;
		}
	}
	// deserialize status_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(status_)) return false;
		}
	}
	// deserialize count_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(count_)) return false;
		}
	}
		return true;
}
void COM_ADChargeEveryContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize currencyCount_
	ss << "\"currencyCount_\":";
	{
		ss << (S64)currencyCount_;
	}
	 ss << ",\n";
	// serialize itemIds_
	ss << "\"itemIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStacks_
	ss << "\"itemStacks_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStacks_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemStacks_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize status_
	ss << "\"status_\":";
	{
		ss << (S64)status_;
	}
	 ss << ",\n";
	// serialize count_
	ss << "\"count_\":";
	{
		ss << (S64)count_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADChargeEvery::COM_ADChargeEvery():
currentCount_(0)
,sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADChargeEvery::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((currentCount_==0)?false:true);
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize currentCount_
	{
		if(currentCount_ != 0){
		__s__->writeType(currentCount_);
		}
	}
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADChargeEvery::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize currentCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(currentCount_)) return false;
		}
	}
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADChargeEvery::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize currentCount_
	ss << "\"currentCount_\":";
	{
		ss << (S64)currentCount_;
	}
	 ss << ",\n";
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADCardsContent::COM_ADCardsContent():
count_(0)
,rewardId_(0)
{}
void COM_ADCardsContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((count_==0)?false:true);
	__fm__.writeBit((rewardId_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize count_
	{
		if(count_ != 0){
		__s__->writeType(count_);
		}
	}
	// serialize rewardId_
	{
		if(rewardId_ != 0){
		__s__->writeType(rewardId_);
		}
	}
}
bool COM_ADCardsContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize count_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(count_)) return false;
		}
	}
	// deserialize rewardId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rewardId_)) return false;
		}
	}
		return true;
}
void COM_ADCardsContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize count_
	ss << "\"count_\":";
	{
		ss << (S64)count_;
	}
	 ss << ",\n";
	// serialize rewardId_
	ss << "\"rewardId_\":";
	{
		ss << (S64)rewardId_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADCards::COM_ADCards():
sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADCards::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADCards::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADCards::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADHotRoleContent::COM_ADHotRoleContent():
type_((EntityType)(0))
,buyNum_(0)
,roleId_(0)
,price_(0)
{}
void COM_ADHotRoleContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((type_==(EntityType)(0))?false:true);
	__fm__.writeBit((buyNum_==0)?false:true);
	__fm__.writeBit((roleId_==0)?false:true);
	__fm__.writeBit((price_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize buyNum_
	{
		if(buyNum_ != 0){
		__s__->writeType(buyNum_);
		}
	}
	// serialize roleId_
	{
		if(roleId_ != 0){
		__s__->writeType(roleId_);
		}
	}
	// serialize price_
	{
		if(price_ != 0){
		__s__->writeType(price_);
		}
	}
}
bool COM_ADHotRoleContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 7) return false;
		type_ = (EntityType)__e__;
		}
	}
	// deserialize buyNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(buyNum_)) return false;
		}
	}
	// deserialize roleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleId_)) return false;
		}
	}
	// deserialize price_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(price_)) return false;
		}
	}
		return true;
}
void COM_ADHotRoleContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(EntityType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize buyNum_
	ss << "\"buyNum_\":";
	{
		ss << (S64)buyNum_;
	}
	 ss << ",\n";
	// serialize roleId_
	ss << "\"roleId_\":";
	{
		ss << (S64)roleId_;
	}
	 ss << ",\n";
	// serialize price_
	ss << "\"price_\":";
	{
		ss << (S64)price_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADHotRole::COM_ADHotRole():
sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADHotRole::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADHotRole::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADHotRole::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADEmployeeTotalContent::COM_ADEmployeeTotalContent():
outputCount_(0)
,status_(0)
{}
void COM_ADEmployeeTotalContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((outputCount_==0)?false:true);
	__fm__.writeBit(itemIds_.size()?true:false);
	__fm__.writeBit(itemStacks_.size()?true:false);
	__fm__.writeBit((status_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize outputCount_
	{
		if(outputCount_ != 0){
		__s__->writeType(outputCount_);
		}
	}
	// serialize itemIds_
	if(itemIds_.size())
	{
		size_t __len__ = (size_t)itemIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemIds_[i]);
		}
	}
	// serialize itemStacks_
	if(itemStacks_.size())
	{
		size_t __len__ = (size_t)itemStacks_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(itemStacks_[i]);
		}
	}
	// serialize status_
	{
		if(status_ != 0){
		__s__->writeType(status_);
		}
	}
}
bool COM_ADEmployeeTotalContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize outputCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(outputCount_)) return false;
		}
	}
	// deserialize itemIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemIds_[i])) return false;
		}
	}
	// deserialize itemStacks_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStacks_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(itemStacks_[i])) return false;
		}
	}
	// deserialize status_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(status_)) return false;
		}
	}
		return true;
}
void COM_ADEmployeeTotalContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize outputCount_
	ss << "\"outputCount_\":";
	{
		ss << (S64)outputCount_;
	}
	 ss << ",\n";
	// serialize itemIds_
	ss << "\"itemIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStacks_
	ss << "\"itemStacks_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStacks_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)itemStacks_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize status_
	ss << "\"status_\":";
	{
		ss << (S64)status_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADEmployeeTotal::COM_ADEmployeeTotal():
outputNum_(0)
,sinceStamp_(0)
,endStamp_(0)
{}
void COM_ADEmployeeTotal::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((outputNum_==0)?false:true);
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize outputNum_
	{
		if(outputNum_ != 0){
		__s__->writeType(outputNum_);
		}
	}
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_ADEmployeeTotal::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize outputNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(outputNum_)) return false;
		}
	}
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADEmployeeTotal::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize outputNum_
	ss << "\"outputNum_\":";
	{
		ss << (S64)outputNum_;
	}
	 ss << ",\n";
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GiftItem::COM_GiftItem():
itemId_(0)
,itemNum_(0)
{}
void COM_GiftItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((itemNum_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize itemNum_
	{
		if(itemNum_ != 0){
		__s__->writeType(itemNum_);
		}
	}
}
bool COM_GiftItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize itemNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemNum_)) return false;
		}
	}
		return true;
}
void COM_GiftItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize itemNum_
	ss << "\"itemNum_\":";
	{
		ss << (S64)itemNum_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ADGiftBag::COM_ADGiftBag():
sinceStamp_(0)
,endStamp_(0)
,isflag_(false)
,price_(0)
,oldprice_(0)
{}
void COM_ADGiftBag::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(isflag_);
	__fm__.writeBit((price_==0)?false:true);
	__fm__.writeBit((oldprice_==0)?false:true);
	__fm__.writeBit(itemdata_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize isflag_
	{
	}
	// serialize price_
	{
		if(price_ != 0){
		__s__->writeType(price_);
		}
	}
	// serialize oldprice_
	{
		if(oldprice_ != 0){
		__s__->writeType(oldprice_);
		}
	}
	// serialize itemdata_
	if(itemdata_.size())
	{
		size_t __len__ = (size_t)itemdata_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			itemdata_[i].serialize(__s__);
		}
	}
}
bool COM_ADGiftBag::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize isflag_
	{
		isflag_ = __fm__.readBit();
	}
	// deserialize price_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(price_)) return false;
		}
	}
	// deserialize oldprice_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(oldprice_)) return false;
		}
	}
	// deserialize itemdata_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemdata_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!itemdata_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ADGiftBag::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize isflag_
	ss << "\"isflag_\":";
	{
		ss << (isflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize price_
	ss << "\"price_\":";
	{
		ss << (S64)price_;
	}
	 ss << ",\n";
	// serialize oldprice_
	ss << "\"oldprice_\":";
	{
		ss << (S64)oldprice_;
	}
	 ss << ",\n";
	// serialize itemdata_
	ss << "\"itemdata_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemdata_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			itemdata_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Sevenday::COM_Sevenday():
quest_(0)
,stype_((AchievementType)(0))
,qvalue_(0)
,isfinish_(false)
,isreward_(false)
{}
void COM_Sevenday::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((quest_==0)?false:true);
	__fm__.writeBit((stype_==(AchievementType)(0))?false:true);
	__fm__.writeBit((qvalue_==0)?false:true);
	__fm__.writeBit(isfinish_);
	__fm__.writeBit(isreward_);
	__s__->write(__fm__.masks_, 1);
	// serialize quest_
	{
		if(quest_ != 0){
		__s__->writeType(quest_);
		}
	}
	// serialize stype_
	{
		EnumSize __e__ = (EnumSize)stype_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize qvalue_
	{
		if(qvalue_ != 0){
		__s__->writeType(qvalue_);
		}
	}
	// serialize isfinish_
	{
	}
	// serialize isreward_
	{
	}
}
bool COM_Sevenday::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize quest_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(quest_)) return false;
		}
	}
	// deserialize stype_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 60) return false;
		stype_ = (AchievementType)__e__;
		}
	}
	// deserialize qvalue_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(qvalue_)) return false;
		}
	}
	// deserialize isfinish_
	{
		isfinish_ = __fm__.readBit();
	}
	// deserialize isreward_
	{
		isreward_ = __fm__.readBit();
	}
		return true;
}
void COM_Sevenday::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize quest_
	ss << "\"quest_\":";
	{
		ss << (S64)quest_;
	}
	 ss << ",\n";
	// serialize stype_
	ss << "\"stype_\":";
	{
		ss << "\"" << ENUM(AchievementType).getItemName(stype_) << "\"";
	}
	 ss << ",\n";
	// serialize qvalue_
	ss << "\"qvalue_\":";
	{
		ss << (S64)qvalue_;
	}
	 ss << ",\n";
	// serialize isfinish_
	ss << "\"isfinish_\":";
	{
		ss << (isfinish_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isreward_
	ss << "\"isreward_\":";
	{
		ss << (isreward_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ZhuanpanContent::COM_ZhuanpanContent():
id_(0)
,item_(0)
,itemNum_(0)
,rate_(0)
,maxdrop_(0)
{}
void COM_ZhuanpanContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((id_==0)?false:true);
	__fm__.writeBit((item_==0)?false:true);
	__fm__.writeBit((itemNum_==0)?false:true);
	__fm__.writeBit((rate_==0)?false:true);
	__fm__.writeBit((maxdrop_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize id_
	{
		if(id_ != 0){
		__s__->writeType(id_);
		}
	}
	// serialize item_
	{
		if(item_ != 0){
		__s__->writeType(item_);
		}
	}
	// serialize itemNum_
	{
		if(itemNum_ != 0){
		__s__->writeType(itemNum_);
		}
	}
	// serialize rate_
	{
		if(rate_ != 0){
		__s__->writeType(rate_);
		}
	}
	// serialize maxdrop_
	{
		if(maxdrop_ != 0){
		__s__->writeType(maxdrop_);
		}
	}
}
bool COM_ZhuanpanContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize id_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(id_)) return false;
		}
	}
	// deserialize item_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(item_)) return false;
		}
	}
	// deserialize itemNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemNum_)) return false;
		}
	}
	// deserialize rate_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rate_)) return false;
		}
	}
	// deserialize maxdrop_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(maxdrop_)) return false;
		}
	}
		return true;
}
void COM_ZhuanpanContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize id_
	ss << "\"id_\":";
	{
		ss << (S64)id_;
	}
	 ss << ",\n";
	// serialize item_
	ss << "\"item_\":";
	{
		ss << (S64)item_;
	}
	 ss << ",\n";
	// serialize itemNum_
	ss << "\"itemNum_\":";
	{
		ss << (S64)itemNum_;
	}
	 ss << ",\n";
	// serialize rate_
	ss << "\"rate_\":";
	{
		ss << (S64)rate_;
	}
	 ss << ",\n";
	// serialize maxdrop_
	ss << "\"maxdrop_\":";
	{
		ss << (S64)maxdrop_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Zhuanpan::COM_Zhuanpan():
zhuanpanId_(0)
{}
void COM_Zhuanpan::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit((zhuanpanId_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize zhuanpanId_
	{
		if(zhuanpanId_ != 0){
		__s__->writeType(zhuanpanId_);
		}
	}
}
bool COM_Zhuanpan::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize zhuanpanId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(zhuanpanId_)) return false;
		}
	}
		return true;
}
void COM_Zhuanpan::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize zhuanpanId_
	ss << "\"zhuanpanId_\":";
	{
		ss << (S64)zhuanpanId_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ZhuanpanData::COM_ZhuanpanData():
sinceStamp_(0)
,endStamp_(0)
{}
void COM_ZhuanpanData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit(contents_.size()?true:false);
	__fm__.writeBit(rarity_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
	// serialize rarity_
	if(rarity_.size())
	{
		size_t __len__ = (size_t)rarity_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			rarity_[i].serialize(__s__);
		}
	}
}
bool COM_ZhuanpanData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize rarity_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		rarity_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!rarity_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ZhuanpanData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize rarity_
	ss << "\"rarity_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)rarity_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			rarity_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_IntegralContent::COM_IntegralContent():
id_(0)
,itemid_(0)
,times_(0)
,cost_(0)
{}
void COM_IntegralContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((id_==0)?false:true);
	__fm__.writeBit((itemid_==0)?false:true);
	__fm__.writeBit((times_==0)?false:true);
	__fm__.writeBit((cost_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize id_
	{
		if(id_ != 0){
		__s__->writeType(id_);
		}
	}
	// serialize itemid_
	{
		if(itemid_ != 0){
		__s__->writeType(itemid_);
		}
	}
	// serialize times_
	{
		if(times_ != 0){
		__s__->writeType(times_);
		}
	}
	// serialize cost_
	{
		if(cost_ != 0){
		__s__->writeType(cost_);
		}
	}
}
bool COM_IntegralContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize id_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(id_)) return false;
		}
	}
	// deserialize itemid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemid_)) return false;
		}
	}
	// deserialize times_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(times_)) return false;
		}
	}
	// deserialize cost_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(cost_)) return false;
		}
	}
		return true;
}
void COM_IntegralContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize id_
	ss << "\"id_\":";
	{
		ss << (S64)id_;
	}
	 ss << ",\n";
	// serialize itemid_
	ss << "\"itemid_\":";
	{
		ss << (S64)itemid_;
	}
	 ss << ",\n";
	// serialize times_
	ss << "\"times_\":";
	{
		ss << (S64)times_;
	}
	 ss << ",\n";
	// serialize cost_
	ss << "\"cost_\":";
	{
		ss << (S64)cost_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_IntegralData::COM_IntegralData():
sinceStamp_(0)
,endStamp_(0)
,integral_(0)
,isflag_(false)
{}
void COM_IntegralData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sinceStamp_==0)?false:true);
	__fm__.writeBit((endStamp_==0)?false:true);
	__fm__.writeBit((integral_==0)?false:true);
	__fm__.writeBit(isflag_);
	__fm__.writeBit(contents_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sinceStamp_
	{
		if(sinceStamp_ != 0){
		__s__->writeType(sinceStamp_);
		}
	}
	// serialize endStamp_
	{
		if(endStamp_ != 0){
		__s__->writeType(endStamp_);
		}
	}
	// serialize integral_
	{
		if(integral_ != 0){
		__s__->writeType(integral_);
		}
	}
	// serialize isflag_
	{
	}
	// serialize contents_
	if(contents_.size())
	{
		size_t __len__ = (size_t)contents_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serialize(__s__);
		}
	}
}
bool COM_IntegralData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sinceStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sinceStamp_)) return false;
		}
	}
	// deserialize endStamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(endStamp_)) return false;
		}
	}
	// deserialize integral_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(integral_)) return false;
		}
	}
	// deserialize isflag_
	{
		isflag_ = __fm__.readBit();
	}
	// deserialize contents_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		contents_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!contents_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_IntegralData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sinceStamp_
	ss << "\"sinceStamp_\":";
	{
		ss << (S64)sinceStamp_;
	}
	 ss << ",\n";
	// serialize endStamp_
	ss << "\"endStamp_\":";
	{
		ss << (S64)endStamp_;
	}
	 ss << ",\n";
	// serialize integral_
	ss << "\"integral_\":";
	{
		ss << (S64)integral_;
	}
	 ss << ",\n";
	// serialize isflag_
	ss << "\"isflag_\":";
	{
		ss << (isflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize contents_
	ss << "\"contents_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)contents_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			contents_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void SGE_SysActivity::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__s__->write(__fm__.masks_, 2);
	// serialize loginData_
	{
		loginData_.serialize(__s__);
	}
	// serialize chData_
	{
		chData_.serialize(__s__);
	}
	// serialize stData_
	{
		stData_.serialize(__s__);
	}
	// serialize ceData_
	{
		ceData_.serialize(__s__);
	}
	// serialize acData_
	{
		acData_.serialize(__s__);
	}
	// serialize hrData_
	{
		hrData_.serialize(__s__);
	}
	// serialize etdata_
	{
		etdata_.serialize(__s__);
	}
	// serialize gbdata_
	{
		gbdata_.serialize(__s__);
	}
	// serialize zpdata_
	{
		zpdata_.serialize(__s__);
	}
	// serialize icdata_
	{
		icdata_.serialize(__s__);
	}
}
bool SGE_SysActivity::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize loginData_
	{
		if(__fm__.readBit()){
		if(!loginData_.deserialize(__r__)) return false;
		}
	}
	// deserialize chData_
	{
		if(__fm__.readBit()){
		if(!chData_.deserialize(__r__)) return false;
		}
	}
	// deserialize stData_
	{
		if(__fm__.readBit()){
		if(!stData_.deserialize(__r__)) return false;
		}
	}
	// deserialize ceData_
	{
		if(__fm__.readBit()){
		if(!ceData_.deserialize(__r__)) return false;
		}
	}
	// deserialize acData_
	{
		if(__fm__.readBit()){
		if(!acData_.deserialize(__r__)) return false;
		}
	}
	// deserialize hrData_
	{
		if(__fm__.readBit()){
		if(!hrData_.deserialize(__r__)) return false;
		}
	}
	// deserialize etdata_
	{
		if(__fm__.readBit()){
		if(!etdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize gbdata_
	{
		if(__fm__.readBit()){
		if(!gbdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize zpdata_
	{
		if(__fm__.readBit()){
		if(!zpdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize icdata_
	{
		if(__fm__.readBit()){
		if(!icdata_.deserialize(__r__)) return false;
		}
	}
		return true;
}
void SGE_SysActivity::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize loginData_
	ss << "\"loginData_\":";
	{
		loginData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize chData_
	ss << "\"chData_\":";
	{
		chData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize stData_
	ss << "\"stData_\":";
	{
		stData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize ceData_
	ss << "\"ceData_\":";
	{
		ceData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize acData_
	ss << "\"acData_\":";
	{
		acData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize hrData_
	ss << "\"hrData_\":";
	{
		hrData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize etdata_
	ss << "\"etdata_\":";
	{
		etdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize gbdata_
	ss << "\"gbdata_\":";
	{
		gbdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize zpdata_
	ss << "\"zpdata_\":";
	{
		zpdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize icdata_
	ss << "\"icdata_\":";
	{
		icdata_.serializeJson(ss);
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_LoginInfo::COM_LoginInfo():
version_(0)
{}
void COM_LoginInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(username_.length()?true:false);
	__fm__.writeBit(password_.length()?true:false);
	__fm__.writeBit((version_==0)?false:true);
	__fm__.writeBit(sessionkey_.length()?true:false);
	__fm__.writeBit(mac_.length()?true:false);
	__fm__.writeBit(idfa_.length()?true:false);
	__fm__.writeBit(devicetype_.length()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize username_
	{
		if(username_.length()){
		__s__->writeType(username_);
		}
	}
	// serialize password_
	{
		if(password_.length()){
		__s__->writeType(password_);
		}
	}
	// serialize version_
	{
		if(version_ != 0){
		__s__->writeType(version_);
		}
	}
	// serialize sessionkey_
	{
		if(sessionkey_.length()){
		__s__->writeType(sessionkey_);
		}
	}
	// serialize mac_
	{
		if(mac_.length()){
		__s__->writeType(mac_);
		}
	}
	// serialize idfa_
	{
		if(idfa_.length()){
		__s__->writeType(idfa_);
		}
	}
	// serialize devicetype_
	{
		if(devicetype_.length()){
		__s__->writeType(devicetype_);
		}
	}
}
bool COM_LoginInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize username_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(username_, 65535)) return false;
		}
	}
	// deserialize password_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(password_, 65535)) return false;
		}
	}
	// deserialize version_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(version_)) return false;
		}
	}
	// deserialize sessionkey_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sessionkey_, 65535)) return false;
		}
	}
	// deserialize mac_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mac_, 65535)) return false;
		}
	}
	// deserialize idfa_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(idfa_, 65535)) return false;
		}
	}
	// deserialize devicetype_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(devicetype_, 65535)) return false;
		}
	}
		return true;
}
void COM_LoginInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize username_
	ss << "\"username_\":";
	{
		ss << "\"" << username_ << "\"";
	}
	 ss << ",\n";
	// serialize password_
	ss << "\"password_\":";
	{
		ss << "\"" << password_ << "\"";
	}
	 ss << ",\n";
	// serialize version_
	ss << "\"version_\":";
	{
		ss << (S64)version_;
	}
	 ss << ",\n";
	// serialize sessionkey_
	ss << "\"sessionkey_\":";
	{
		ss << "\"" << sessionkey_ << "\"";
	}
	 ss << ",\n";
	// serialize mac_
	ss << "\"mac_\":";
	{
		ss << "\"" << mac_ << "\"";
	}
	 ss << ",\n";
	// serialize idfa_
	ss << "\"idfa_\":";
	{
		ss << "\"" << idfa_ << "\"";
	}
	 ss << ",\n";
	// serialize devicetype_
	ss << "\"devicetype_\":";
	{
		ss << "\"" << devicetype_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_FPosition::COM_FPosition():
x_(0)
,z_(0)
,isLast_(false)
{}
void COM_FPosition::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((x_==0)?false:true);
	__fm__.writeBit((z_==0)?false:true);
	__fm__.writeBit(isLast_);
	__s__->write(__fm__.masks_, 1);
	// serialize x_
	{
		if(x_ != 0){
		__s__->writeType(x_);
		}
	}
	// serialize z_
	{
		if(z_ != 0){
		__s__->writeType(z_);
		}
	}
	// serialize isLast_
	{
	}
}
bool COM_FPosition::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize x_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(x_)) return false;
		}
	}
	// deserialize z_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(z_)) return false;
		}
	}
	// deserialize isLast_
	{
		isLast_ = __fm__.readBit();
	}
		return true;
}
void COM_FPosition::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize x_
	ss << "\"x_\":";
	{
		ss << (double)x_;
	}
	 ss << ",\n";
	// serialize z_
	ss << "\"z_\":";
	{
		ss << (double)z_;
	}
	 ss << ",\n";
	// serialize isLast_
	ss << "\"isLast_\":";
	{
		ss << (isLast_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ScenePlayerInformation::COM_ScenePlayerInformation():
isLeader_(false)
,isTeamMember_(false)
,isInBattle_(false)
,vip_(0)
,instId_(0)
,assetId_(0)
,weaponItemId_(0)
,fashionId_(0)
,hpMax_(0)
,hpCrt_(0)
,mpMax_(0)
,mpCrt_(0)
,level_(0)
,battlePower_(0)
,jl_(0)
,magicLv_(0)
,openSubSystemFlag_(0)
,title_(0)
,jt_((JobType)(0))
,type_((EntityType)(0))
,showBabyTableId_(0)
{}
void COM_ScenePlayerInformation::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<4> __fm__;
	__fm__.writeBit(isLeader_);
	__fm__.writeBit(isTeamMember_);
	__fm__.writeBit(isInBattle_);
	__fm__.writeBit((vip_==0)?false:true);
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((assetId_==0)?false:true);
	__fm__.writeBit((weaponItemId_==0)?false:true);
	__fm__.writeBit((fashionId_==0)?false:true);
	__fm__.writeBit((hpMax_==0)?false:true);
	__fm__.writeBit((hpCrt_==0)?false:true);
	__fm__.writeBit((mpMax_==0)?false:true);
	__fm__.writeBit((mpCrt_==0)?false:true);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((battlePower_==0)?false:true);
	__fm__.writeBit((jl_==0)?false:true);
	__fm__.writeBit((magicLv_==0)?false:true);
	__fm__.writeBit((openSubSystemFlag_==0)?false:true);
	__fm__.writeBit((title_==0)?false:true);
	__fm__.writeBit(instName_.length()?true:false);
	__fm__.writeBit(guildeName_.length()?true:false);
	__fm__.writeBit((jt_==(JobType)(0))?false:true);
	__fm__.writeBit((type_==(EntityType)(0))?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit((showBabyTableId_==0)?false:true);
	__fm__.writeBit(showBabyName_.length()?true:false);
	__s__->write(__fm__.masks_, 4);
	// serialize isLeader_
	{
	}
	// serialize isTeamMember_
	{
	}
	// serialize isInBattle_
	{
	}
	// serialize vip_
	{
		if(vip_ != 0){
		__s__->writeType(vip_);
		}
	}
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize assetId_
	{
		if(assetId_ != 0){
		__s__->writeType(assetId_);
		}
	}
	// serialize weaponItemId_
	{
		if(weaponItemId_ != 0){
		__s__->writeType(weaponItemId_);
		}
	}
	// serialize fashionId_
	{
		if(fashionId_ != 0){
		__s__->writeType(fashionId_);
		}
	}
	// serialize hpMax_
	{
		if(hpMax_ != 0){
		__s__->writeType(hpMax_);
		}
	}
	// serialize hpCrt_
	{
		if(hpCrt_ != 0){
		__s__->writeType(hpCrt_);
		}
	}
	// serialize mpMax_
	{
		if(mpMax_ != 0){
		__s__->writeType(mpMax_);
		}
	}
	// serialize mpCrt_
	{
		if(mpCrt_ != 0){
		__s__->writeType(mpCrt_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize battlePower_
	{
		if(battlePower_ != 0){
		__s__->writeType(battlePower_);
		}
	}
	// serialize jl_
	{
		if(jl_ != 0){
		__s__->writeType(jl_);
		}
	}
	// serialize magicLv_
	{
		if(magicLv_ != 0){
		__s__->writeType(magicLv_);
		}
	}
	// serialize openSubSystemFlag_
	{
		if(openSubSystemFlag_ != 0){
		__s__->writeType(openSubSystemFlag_);
		}
	}
	// serialize title_
	{
		if(title_ != 0){
		__s__->writeType(title_);
		}
	}
	// serialize instName_
	{
		if(instName_.length()){
		__s__->writeType(instName_);
		}
	}
	// serialize guildeName_
	{
		if(guildeName_.length()){
		__s__->writeType(guildeName_);
		}
	}
	// serialize jt_
	{
		EnumSize __e__ = (EnumSize)jt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize originPos_
	{
		originPos_.serialize(__s__);
	}
	// serialize showBabyTableId_
	{
		if(showBabyTableId_ != 0){
		__s__->writeType(showBabyTableId_);
		}
	}
	// serialize showBabyName_
	{
		if(showBabyName_.length()){
		__s__->writeType(showBabyName_);
		}
	}
}
bool COM_ScenePlayerInformation::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<4> __fm__;
	if(!__r__->read(__fm__.masks_, 4)) return false;
	// deserialize isLeader_
	{
		isLeader_ = __fm__.readBit();
	}
	// deserialize isTeamMember_
	{
		isTeamMember_ = __fm__.readBit();
	}
	// deserialize isInBattle_
	{
		isInBattle_ = __fm__.readBit();
	}
	// deserialize vip_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(vip_)) return false;
		}
	}
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize assetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(assetId_)) return false;
		}
	}
	// deserialize weaponItemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(weaponItemId_)) return false;
		}
	}
	// deserialize fashionId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(fashionId_)) return false;
		}
	}
	// deserialize hpMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(hpMax_)) return false;
		}
	}
	// deserialize hpCrt_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(hpCrt_)) return false;
		}
	}
	// deserialize mpMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mpMax_)) return false;
		}
	}
	// deserialize mpCrt_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mpCrt_)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize battlePower_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(battlePower_)) return false;
		}
	}
	// deserialize jl_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(jl_)) return false;
		}
	}
	// deserialize magicLv_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicLv_)) return false;
		}
	}
	// deserialize openSubSystemFlag_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(openSubSystemFlag_)) return false;
		}
	}
	// deserialize title_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(title_)) return false;
		}
	}
	// deserialize instName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instName_, 65535)) return false;
		}
	}
	// deserialize guildeName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildeName_, 65535)) return false;
		}
	}
	// deserialize jt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		jt_ = (JobType)__e__;
		}
	}
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 7) return false;
		type_ = (EntityType)__e__;
		}
	}
	// deserialize originPos_
	{
		if(__fm__.readBit()){
		if(!originPos_.deserialize(__r__)) return false;
		}
	}
	// deserialize showBabyTableId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showBabyTableId_)) return false;
		}
	}
	// deserialize showBabyName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showBabyName_, 65535)) return false;
		}
	}
		return true;
}
void COM_ScenePlayerInformation::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize isLeader_
	ss << "\"isLeader_\":";
	{
		ss << (isLeader_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isTeamMember_
	ss << "\"isTeamMember_\":";
	{
		ss << (isTeamMember_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isInBattle_
	ss << "\"isInBattle_\":";
	{
		ss << (isInBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize vip_
	ss << "\"vip_\":";
	{
		ss << (S64)vip_;
	}
	 ss << ",\n";
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize assetId_
	ss << "\"assetId_\":";
	{
		ss << (S64)assetId_;
	}
	 ss << ",\n";
	// serialize weaponItemId_
	ss << "\"weaponItemId_\":";
	{
		ss << (S64)weaponItemId_;
	}
	 ss << ",\n";
	// serialize fashionId_
	ss << "\"fashionId_\":";
	{
		ss << (S64)fashionId_;
	}
	 ss << ",\n";
	// serialize hpMax_
	ss << "\"hpMax_\":";
	{
		ss << (S64)hpMax_;
	}
	 ss << ",\n";
	// serialize hpCrt_
	ss << "\"hpCrt_\":";
	{
		ss << (S64)hpCrt_;
	}
	 ss << ",\n";
	// serialize mpMax_
	ss << "\"mpMax_\":";
	{
		ss << (S64)mpMax_;
	}
	 ss << ",\n";
	// serialize mpCrt_
	ss << "\"mpCrt_\":";
	{
		ss << (S64)mpCrt_;
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize battlePower_
	ss << "\"battlePower_\":";
	{
		ss << (S64)battlePower_;
	}
	 ss << ",\n";
	// serialize jl_
	ss << "\"jl_\":";
	{
		ss << (S64)jl_;
	}
	 ss << ",\n";
	// serialize magicLv_
	ss << "\"magicLv_\":";
	{
		ss << (S64)magicLv_;
	}
	 ss << ",\n";
	// serialize openSubSystemFlag_
	ss << "\"openSubSystemFlag_\":";
	{
		ss << (S64)openSubSystemFlag_;
	}
	 ss << ",\n";
	// serialize title_
	ss << "\"title_\":";
	{
		ss << (S64)title_;
	}
	 ss << ",\n";
	// serialize instName_
	ss << "\"instName_\":";
	{
		ss << "\"" << instName_ << "\"";
	}
	 ss << ",\n";
	// serialize guildeName_
	ss << "\"guildeName_\":";
	{
		ss << "\"" << guildeName_ << "\"";
	}
	 ss << ",\n";
	// serialize jt_
	ss << "\"jt_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(jt_) << "\"";
	}
	 ss << ",\n";
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(EntityType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize originPos_
	ss << "\"originPos_\":";
	{
		originPos_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize showBabyTableId_
	ss << "\"showBabyTableId_\":";
	{
		ss << (S64)showBabyTableId_;
	}
	 ss << ",\n";
	// serialize showBabyName_
	ss << "\"showBabyName_\":";
	{
		ss << "\"" << showBabyName_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_BattleEntityInformation::COM_BattleEntityInformation():
type_((EntityType)(0))
,instId_(0)
,tableId_(0)
,assetId_(0)
,jt_((JobType)(0))
,battlePosition_((BattlePosition)(0))
,weaponItemId_(0)
,fashionId_(0)
,hpMax_(0)
,hpCrt_(0)
,mpMax_(0)
,mpCrt_(0)
,level_(0)
{}
void COM_BattleEntityInformation::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((type_==(EntityType)(0))?false:true);
	__fm__.writeBit(instName_.length()?true:false);
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((tableId_==0)?false:true);
	__fm__.writeBit((assetId_==0)?false:true);
	__fm__.writeBit((jt_==(JobType)(0))?false:true);
	__fm__.writeBit((battlePosition_==(BattlePosition)(0))?false:true);
	__fm__.writeBit((weaponItemId_==0)?false:true);
	__fm__.writeBit((fashionId_==0)?false:true);
	__fm__.writeBit((hpMax_==0)?false:true);
	__fm__.writeBit((hpCrt_==0)?false:true);
	__fm__.writeBit((mpMax_==0)?false:true);
	__fm__.writeBit((mpCrt_==0)?false:true);
	__fm__.writeBit((level_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize instName_
	{
		if(instName_.length()){
		__s__->writeType(instName_);
		}
	}
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize tableId_
	{
		if(tableId_ != 0){
		__s__->writeType(tableId_);
		}
	}
	// serialize assetId_
	{
		if(assetId_ != 0){
		__s__->writeType(assetId_);
		}
	}
	// serialize jt_
	{
		EnumSize __e__ = (EnumSize)jt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize battlePosition_
	{
		EnumSize __e__ = (EnumSize)battlePosition_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize weaponItemId_
	{
		if(weaponItemId_ != 0){
		__s__->writeType(weaponItemId_);
		}
	}
	// serialize fashionId_
	{
		if(fashionId_ != 0){
		__s__->writeType(fashionId_);
		}
	}
	// serialize hpMax_
	{
		if(hpMax_ != 0){
		__s__->writeType(hpMax_);
		}
	}
	// serialize hpCrt_
	{
		if(hpCrt_ != 0){
		__s__->writeType(hpCrt_);
		}
	}
	// serialize mpMax_
	{
		if(mpMax_ != 0){
		__s__->writeType(mpMax_);
		}
	}
	// serialize mpCrt_
	{
		if(mpCrt_ != 0){
		__s__->writeType(mpCrt_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
}
bool COM_BattleEntityInformation::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 7) return false;
		type_ = (EntityType)__e__;
		}
	}
	// deserialize instName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instName_, 65535)) return false;
		}
	}
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize tableId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(tableId_)) return false;
		}
	}
	// deserialize assetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(assetId_)) return false;
		}
	}
	// deserialize jt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		jt_ = (JobType)__e__;
		}
	}
	// deserialize battlePosition_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		battlePosition_ = (BattlePosition)__e__;
		}
	}
	// deserialize weaponItemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(weaponItemId_)) return false;
		}
	}
	// deserialize fashionId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(fashionId_)) return false;
		}
	}
	// deserialize hpMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(hpMax_)) return false;
		}
	}
	// deserialize hpCrt_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(hpCrt_)) return false;
		}
	}
	// deserialize mpMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mpMax_)) return false;
		}
	}
	// deserialize mpCrt_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mpCrt_)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
		return true;
}
void COM_BattleEntityInformation::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(EntityType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize instName_
	ss << "\"instName_\":";
	{
		ss << "\"" << instName_ << "\"";
	}
	 ss << ",\n";
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize tableId_
	ss << "\"tableId_\":";
	{
		ss << (S64)tableId_;
	}
	 ss << ",\n";
	// serialize assetId_
	ss << "\"assetId_\":";
	{
		ss << (S64)assetId_;
	}
	 ss << ",\n";
	// serialize jt_
	ss << "\"jt_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(jt_) << "\"";
	}
	 ss << ",\n";
	// serialize battlePosition_
	ss << "\"battlePosition_\":";
	{
		ss << "\"" << ENUM(BattlePosition).getItemName(battlePosition_) << "\"";
	}
	 ss << ",\n";
	// serialize weaponItemId_
	ss << "\"weaponItemId_\":";
	{
		ss << (S64)weaponItemId_;
	}
	 ss << ",\n";
	// serialize fashionId_
	ss << "\"fashionId_\":";
	{
		ss << (S64)fashionId_;
	}
	 ss << ",\n";
	// serialize hpMax_
	ss << "\"hpMax_\":";
	{
		ss << (S64)hpMax_;
	}
	 ss << ",\n";
	// serialize hpCrt_
	ss << "\"hpCrt_\":";
	{
		ss << (S64)hpCrt_;
	}
	 ss << ",\n";
	// serialize mpMax_
	ss << "\"mpMax_\":";
	{
		ss << (S64)mpMax_;
	}
	 ss << ",\n";
	// serialize mpCrt_
	ss << "\"mpCrt_\":";
	{
		ss << (S64)mpCrt_;
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SimpleInformation::COM_SimpleInformation():
instId_(0)
,level_(0)
,asset_id_(0)
,weaponItemId_(0)
,fashionId_(0)
,section_(0)
,jt_((JobType)(0))
,jl_(0)
{}
void COM_SimpleInformation::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((asset_id_==0)?false:true);
	__fm__.writeBit(instName_.length()?true:false);
	__fm__.writeBit((weaponItemId_==0)?false:true);
	__fm__.writeBit((fashionId_==0)?false:true);
	__fm__.writeBit((section_==0)?false:true);
	__fm__.writeBit((jt_==(JobType)(0))?false:true);
	__fm__.writeBit((jl_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize asset_id_
	{
		if(asset_id_ != 0){
		__s__->writeType(asset_id_);
		}
	}
	// serialize instName_
	{
		if(instName_.length()){
		__s__->writeType(instName_);
		}
	}
	// serialize weaponItemId_
	{
		if(weaponItemId_ != 0){
		__s__->writeType(weaponItemId_);
		}
	}
	// serialize fashionId_
	{
		if(fashionId_ != 0){
		__s__->writeType(fashionId_);
		}
	}
	// serialize section_
	{
		if(section_ != 0){
		__s__->writeType(section_);
		}
	}
	// serialize jt_
	{
		EnumSize __e__ = (EnumSize)jt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize jl_
	{
		if(jl_ != 0){
		__s__->writeType(jl_);
		}
	}
}
bool COM_SimpleInformation::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize asset_id_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(asset_id_)) return false;
		}
	}
	// deserialize instName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instName_, 65535)) return false;
		}
	}
	// deserialize weaponItemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(weaponItemId_)) return false;
		}
	}
	// deserialize fashionId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(fashionId_)) return false;
		}
	}
	// deserialize section_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(section_)) return false;
		}
	}
	// deserialize jt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		jt_ = (JobType)__e__;
		}
	}
	// deserialize jl_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(jl_)) return false;
		}
	}
		return true;
}
void COM_SimpleInformation::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize asset_id_
	ss << "\"asset_id_\":";
	{
		ss << (S64)asset_id_;
	}
	 ss << ",\n";
	// serialize instName_
	ss << "\"instName_\":";
	{
		ss << "\"" << instName_ << "\"";
	}
	 ss << ",\n";
	// serialize weaponItemId_
	ss << "\"weaponItemId_\":";
	{
		ss << (S64)weaponItemId_;
	}
	 ss << ",\n";
	// serialize fashionId_
	ss << "\"fashionId_\":";
	{
		ss << (S64)fashionId_;
	}
	 ss << ",\n";
	// serialize section_
	ss << "\"section_\":";
	{
		ss << (S64)section_;
	}
	 ss << ",\n";
	// serialize jt_
	ss << "\"jt_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(jt_) << "\"";
	}
	 ss << ",\n";
	// serialize jl_
	ss << "\"jl_\":";
	{
		ss << (S64)jl_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_AccountInfo::COM_AccountInfo():
guid_(0)
,createtime_(0)
{}
void COM_AccountInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((guid_==0)?false:true);
	__fm__.writeBit(username_.length()?true:false);
	__fm__.writeBit(password_.length()?true:false);
	__fm__.writeBit((createtime_==0)?false:true);
	__fm__.writeBit(phoneNumber_.length()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize guid_
	{
		if(guid_ != 0){
		__s__->writeType(guid_);
		}
	}
	// serialize username_
	{
		if(username_.length()){
		__s__->writeType(username_);
		}
	}
	// serialize password_
	{
		if(password_.length()){
		__s__->writeType(password_);
		}
	}
	// serialize createtime_
	{
		if(createtime_ != 0){
		__s__->writeType(createtime_);
		}
	}
	// serialize phoneNumber_
	{
		if(phoneNumber_.length()){
		__s__->writeType(phoneNumber_);
		}
	}
}
bool COM_AccountInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize guid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guid_)) return false;
		}
	}
	// deserialize username_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(username_, 65535)) return false;
		}
	}
	// deserialize password_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(password_, 65535)) return false;
		}
	}
	// deserialize createtime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createtime_)) return false;
		}
	}
	// deserialize phoneNumber_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(phoneNumber_, 65535)) return false;
		}
	}
		return true;
}
void COM_AccountInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize guid_
	ss << "\"guid_\":";
	{
		ss << (S64)guid_;
	}
	 ss << ",\n";
	// serialize username_
	ss << "\"username_\":";
	{
		ss << "\"" << username_ << "\"";
	}
	 ss << ",\n";
	// serialize password_
	ss << "\"password_\":";
	{
		ss << "\"" << password_ << "\"";
	}
	 ss << ",\n";
	// serialize createtime_
	ss << "\"createtime_\":";
	{
		ss << (S64)createtime_;
	}
	 ss << ",\n";
	// serialize phoneNumber_
	ss << "\"phoneNumber_\":";
	{
		ss << "\"" << phoneNumber_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_PropValue::COM_PropValue():
type_((PropertyType)(0))
,value_(0)
{}
void COM_PropValue::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((type_==(PropertyType)(0))?false:true);
	__fm__.writeBit((value_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize value_
	{
		if(value_ != 0){
		__s__->writeType(value_);
		}
	}
}
bool COM_PropValue::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 63) return false;
		type_ = (PropertyType)__e__;
		}
	}
	// deserialize value_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(value_)) return false;
		}
	}
		return true;
}
void COM_PropValue::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(PropertyType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize value_
	ss << "\"value_\":";
	{
		ss << (double)value_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Item::COM_Item():
itemId_(0)
,instId_(0)
,stack_(0)
,isBind_(false)
,isLock_(false)
,strLevel_(0)
,slot_(0)
,skillID_(0)
,durability_(0)
,durabilityMax_(0)
,usedTimeout_(0)
,lastSellTime_(0)
{}
void COM_Item::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((stack_==0)?false:true);
	__fm__.writeBit(isBind_);
	__fm__.writeBit(isLock_);
	__fm__.writeBit((strLevel_==0)?false:true);
	__fm__.writeBit((slot_==0)?false:true);
	__fm__.writeBit((skillID_==0)?false:true);
	__fm__.writeBit((durability_==0)?false:true);
	__fm__.writeBit((durabilityMax_==0)?false:true);
	__fm__.writeBit((usedTimeout_==0)?false:true);
	__fm__.writeBit((lastSellTime_==0)?false:true);
	__fm__.writeBit(propArr.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize stack_
	{
		if(stack_ != 0){
		__s__->writeType(stack_);
		}
	}
	// serialize isBind_
	{
	}
	// serialize isLock_
	{
	}
	// serialize strLevel_
	{
		if(strLevel_ != 0){
		__s__->writeType(strLevel_);
		}
	}
	// serialize slot_
	{
		if(slot_ != 0){
		__s__->writeType(slot_);
		}
	}
	// serialize skillID_
	{
		if(skillID_ != 0){
		__s__->writeType(skillID_);
		}
	}
	// serialize durability_
	{
		if(durability_ != 0){
		__s__->writeType(durability_);
		}
	}
	// serialize durabilityMax_
	{
		if(durabilityMax_ != 0){
		__s__->writeType(durabilityMax_);
		}
	}
	// serialize usedTimeout_
	{
		if(usedTimeout_ != 0){
		__s__->writeType(usedTimeout_);
		}
	}
	// serialize lastSellTime_
	{
		if(lastSellTime_ != 0){
		__s__->writeType(lastSellTime_);
		}
	}
	// serialize propArr
	if(propArr.size())
	{
		size_t __len__ = (size_t)propArr.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			propArr[i].serialize(__s__);
		}
	}
}
bool COM_Item::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize stack_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(stack_)) return false;
		}
	}
	// deserialize isBind_
	{
		isBind_ = __fm__.readBit();
	}
	// deserialize isLock_
	{
		isLock_ = __fm__.readBit();
	}
	// deserialize strLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(strLevel_)) return false;
		}
	}
	// deserialize slot_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(slot_)) return false;
		}
	}
	// deserialize skillID_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skillID_)) return false;
		}
	}
	// deserialize durability_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(durability_)) return false;
		}
	}
	// deserialize durabilityMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(durabilityMax_)) return false;
		}
	}
	// deserialize usedTimeout_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(usedTimeout_)) return false;
		}
	}
	// deserialize lastSellTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(lastSellTime_)) return false;
		}
	}
	// deserialize propArr
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		propArr.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!propArr[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_Item::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize stack_
	ss << "\"stack_\":";
	{
		ss << (S64)stack_;
	}
	 ss << ",\n";
	// serialize isBind_
	ss << "\"isBind_\":";
	{
		ss << (isBind_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isLock_
	ss << "\"isLock_\":";
	{
		ss << (isLock_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize strLevel_
	ss << "\"strLevel_\":";
	{
		ss << (S64)strLevel_;
	}
	 ss << ",\n";
	// serialize slot_
	ss << "\"slot_\":";
	{
		ss << (S64)slot_;
	}
	 ss << ",\n";
	// serialize skillID_
	ss << "\"skillID_\":";
	{
		ss << (S64)skillID_;
	}
	 ss << ",\n";
	// serialize durability_
	ss << "\"durability_\":";
	{
		ss << (S64)durability_;
	}
	 ss << ",\n";
	// serialize durabilityMax_
	ss << "\"durabilityMax_\":";
	{
		ss << (S64)durabilityMax_;
	}
	 ss << ",\n";
	// serialize usedTimeout_
	ss << "\"usedTimeout_\":";
	{
		ss << (S64)usedTimeout_;
	}
	 ss << ",\n";
	// serialize lastSellTime_
	ss << "\"lastSellTime_\":";
	{
		ss << (S64)lastSellTime_;
	}
	 ss << ",\n";
	// serialize propArr
	ss << "\"propArr\":";
	{
		ss << "[";
		size_t __len__ = (size_t)propArr.size();
		for(size_t i = 0; i < __len__; i++)
		{
			propArr[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_DropItem::COM_DropItem():
itemId_(0)
,itemNum_(0)
{}
void COM_DropItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((itemNum_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize itemNum_
	{
		if(itemNum_ != 0){
		__s__->writeType(itemNum_);
		}
	}
}
bool COM_DropItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize itemNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemNum_)) return false;
		}
	}
		return true;
}
void COM_DropItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize itemNum_
	ss << "\"itemNum_\":";
	{
		ss << (S64)itemNum_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_CrystalProp::COM_CrystalProp():
level_(0)
,type_((PropertyType)(0))
,val_(0)
{}
void COM_CrystalProp::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((type_==(PropertyType)(0))?false:true);
	__fm__.writeBit((val_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize val_
	{
		if(val_ != 0){
		__s__->writeType(val_);
		}
	}
}
bool COM_CrystalProp::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 63) return false;
		type_ = (PropertyType)__e__;
		}
	}
	// deserialize val_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(val_)) return false;
		}
	}
		return true;
}
void COM_CrystalProp::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(PropertyType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize val_
	ss << "\"val_\":";
	{
		ss << (S64)val_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_CrystalData::COM_CrystalData():
level_(0)
{}
void COM_CrystalData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit(props_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize props_
	if(props_.size())
	{
		size_t __len__ = (size_t)props_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			props_[i].serialize(__s__);
		}
	}
}
bool COM_CrystalData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize props_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		props_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!props_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_CrystalData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize props_
	ss << "\"props_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)props_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			props_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_QuestTarget::COM_QuestTarget():
targetId_(0)
,targetNum_(0)
{}
void COM_QuestTarget::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((targetId_==0)?false:true);
	__fm__.writeBit((targetNum_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize targetId_
	{
		if(targetId_ != 0){
		__s__->writeType(targetId_);
		}
	}
	// serialize targetNum_
	{
		if(targetNum_ != 0){
		__s__->writeType(targetNum_);
		}
	}
}
bool COM_QuestTarget::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize targetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(targetId_)) return false;
		}
	}
	// deserialize targetNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(targetNum_)) return false;
		}
	}
		return true;
}
void COM_QuestTarget::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize targetId_
	ss << "\"targetId_\":";
	{
		ss << (S64)targetId_;
	}
	 ss << ",\n";
	// serialize targetNum_
	ss << "\"targetNum_\":";
	{
		ss << (S64)targetNum_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Skill::COM_Skill():
skillID_(0)
,skillExp_(0)
,skillLevel_(0)
{}
void COM_Skill::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((skillID_==0)?false:true);
	__fm__.writeBit((skillExp_==0)?false:true);
	__fm__.writeBit((skillLevel_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize skillID_
	{
		if(skillID_ != 0){
		__s__->writeType(skillID_);
		}
	}
	// serialize skillExp_
	{
		if(skillExp_ != 0){
		__s__->writeType(skillExp_);
		}
	}
	// serialize skillLevel_
	{
		if(skillLevel_ != 0){
		__s__->writeType(skillLevel_);
		}
	}
}
bool COM_Skill::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize skillID_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skillID_)) return false;
		}
	}
	// deserialize skillExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skillExp_)) return false;
		}
	}
	// deserialize skillLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skillLevel_)) return false;
		}
	}
		return true;
}
void COM_Skill::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize skillID_
	ss << "\"skillID_\":";
	{
		ss << (S64)skillID_;
	}
	 ss << ",\n";
	// serialize skillExp_
	ss << "\"skillExp_\":";
	{
		ss << (S64)skillExp_;
	}
	 ss << ",\n";
	// serialize skillLevel_
	ss << "\"skillLevel_\":";
	{
		ss << (S64)skillLevel_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_QuestInst::COM_QuestInst():
questId_(0)
{}
void COM_QuestInst::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((questId_==0)?false:true);
	__fm__.writeBit(targets_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize questId_
	{
		if(questId_ != 0){
		__s__->writeType(questId_);
		}
	}
	// serialize targets_
	if(targets_.size())
	{
		size_t __len__ = (size_t)targets_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serialize(__s__);
		}
	}
}
bool COM_QuestInst::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize questId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(questId_)) return false;
		}
	}
	// deserialize targets_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		targets_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!targets_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_QuestInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize questId_
	ss << "\"questId_\":";
	{
		ss << (S64)questId_;
	}
	 ss << ",\n";
	// serialize targets_
	ss << "\"targets_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)targets_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_State::COM_State():
stateId_(0)
,turn_(0)
,tick_(0)
,type_(0)
,value0_(0)
,value1_(0)
{}
void COM_State::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((stateId_==0)?false:true);
	__fm__.writeBit((turn_==0)?false:true);
	__fm__.writeBit((tick_==0)?false:true);
	__fm__.writeBit((type_==0)?false:true);
	__fm__.writeBit((value0_==0)?false:true);
	__fm__.writeBit((value1_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize stateId_
	{
		if(stateId_ != 0){
		__s__->writeType(stateId_);
		}
	}
	// serialize turn_
	{
		if(turn_ != 0){
		__s__->writeType(turn_);
		}
	}
	// serialize tick_
	{
		if(tick_ != 0){
		__s__->writeType(tick_);
		}
	}
	// serialize type_
	{
		if(type_ != 0){
		__s__->writeType(type_);
		}
	}
	// serialize value0_
	{
		if(value0_ != 0){
		__s__->writeType(value0_);
		}
	}
	// serialize value1_
	{
		if(value1_ != 0){
		__s__->writeType(value1_);
		}
	}
}
bool COM_State::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize stateId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(stateId_)) return false;
		}
	}
	// deserialize turn_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(turn_)) return false;
		}
	}
	// deserialize tick_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(tick_)) return false;
		}
	}
	// deserialize type_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(type_)) return false;
		}
	}
	// deserialize value0_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(value0_)) return false;
		}
	}
	// deserialize value1_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(value1_)) return false;
		}
	}
		return true;
}
void COM_State::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize stateId_
	ss << "\"stateId_\":";
	{
		ss << (S64)stateId_;
	}
	 ss << ",\n";
	// serialize turn_
	ss << "\"turn_\":";
	{
		ss << (S64)turn_;
	}
	 ss << ",\n";
	// serialize tick_
	ss << "\"tick_\":";
	{
		ss << (S64)tick_;
	}
	 ss << ",\n";
	// serialize type_
	ss << "\"type_\":";
	{
		ss << (S64)type_;
	}
	 ss << ",\n";
	// serialize value0_
	ss << "\"value0_\":";
	{
		ss << (S64)value0_;
	}
	 ss << ",\n";
	// serialize value1_
	ss << "\"value1_\":";
	{
		ss << (S64)value1_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Entity::COM_Entity():
type_((EntityType)(0))
,instId_(0)
,battlePosition_((BattlePosition)(0))
{}
void COM_Entity::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((type_==(EntityType)(0))?false:true);
	__fm__.writeBit(instName_.length()?true:false);
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((battlePosition_==(BattlePosition)(0))?false:true);
	__fm__.writeBit(properties_.size()?true:false);
	__fm__.writeBit(skill_.size()?true:false);
	__fm__.writeBit(equips_.size()?true:false);
	__fm__.writeBit(states_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize instName_
	{
		if(instName_.length()){
		__s__->writeType(instName_);
		}
	}
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize battlePosition_
	{
		EnumSize __e__ = (EnumSize)battlePosition_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize properties_
	if(properties_.size())
	{
		size_t __len__ = (size_t)properties_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(properties_[i]);
		}
	}
	// serialize skill_
	if(skill_.size())
	{
		size_t __len__ = (size_t)skill_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			skill_[i].serialize(__s__);
		}
	}
	// serialize equips_
	if(equips_.size())
	{
		size_t __len__ = (size_t)equips_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			equips_[i].serialize(__s__);
		}
	}
	// serialize states_
	if(states_.size())
	{
		size_t __len__ = (size_t)states_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			states_[i].serialize(__s__);
		}
	}
}
bool COM_Entity::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 7) return false;
		type_ = (EntityType)__e__;
		}
	}
	// deserialize instName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instName_, 65535)) return false;
		}
	}
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize battlePosition_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		battlePosition_ = (BattlePosition)__e__;
		}
	}
	// deserialize properties_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		properties_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(properties_[i])) return false;
		}
	}
	// deserialize skill_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		skill_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!skill_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize equips_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		equips_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!equips_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize states_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		states_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!states_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_Entity::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(EntityType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize instName_
	ss << "\"instName_\":";
	{
		ss << "\"" << instName_ << "\"";
	}
	 ss << ",\n";
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize battlePosition_
	ss << "\"battlePosition_\":";
	{
		ss << "\"" << ENUM(BattlePosition).getItemName(battlePosition_) << "\"";
	}
	 ss << ",\n";
	// serialize properties_
	ss << "\"properties_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)properties_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (double)properties_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize skill_
	ss << "\"skill_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)skill_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			skill_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize equips_
	ss << "\"equips_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)equips_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			equips_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize states_
	ss << "\"states_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)states_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			states_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_BabyInst::COM_BabyInst():
isShow_(false)
,isBattle_(false)
,isBind_(false)
,isLock_(false)
,tableId_(0)
,slot_(0)
,intensifyLevel_(0)
,intensifynum_(0)
,lastSellTime_(0)
{}
void COM_BabyInst::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(ownerName_.length()?true:false);
	__fm__.writeBit(isShow_);
	__fm__.writeBit(isBattle_);
	__fm__.writeBit(isBind_);
	__fm__.writeBit(isLock_);
	__fm__.writeBit((tableId_==0)?false:true);
	__fm__.writeBit((slot_==0)?false:true);
	__fm__.writeBit((intensifyLevel_==0)?false:true);
	__fm__.writeBit((intensifynum_==0)?false:true);
	__fm__.writeBit((lastSellTime_==0)?false:true);
	__fm__.writeBit(gear_.size()?true:false);
	__fm__.writeBit(addprop_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize ownerName_
	{
		if(ownerName_.length()){
		__s__->writeType(ownerName_);
		}
	}
	// serialize isShow_
	{
	}
	// serialize isBattle_
	{
	}
	// serialize isBind_
	{
	}
	// serialize isLock_
	{
	}
	// serialize tableId_
	{
		if(tableId_ != 0){
		__s__->writeType(tableId_);
		}
	}
	// serialize slot_
	{
		if(slot_ != 0){
		__s__->writeType(slot_);
		}
	}
	// serialize intensifyLevel_
	{
		if(intensifyLevel_ != 0){
		__s__->writeType(intensifyLevel_);
		}
	}
	// serialize intensifynum_
	{
		if(intensifynum_ != 0){
		__s__->writeType(intensifynum_);
		}
	}
	// serialize lastSellTime_
	{
		if(lastSellTime_ != 0){
		__s__->writeType(lastSellTime_);
		}
	}
	// serialize gear_
	if(gear_.size())
	{
		size_t __len__ = (size_t)gear_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(gear_[i]);
		}
	}
	// serialize addprop_
	if(addprop_.size())
	{
		size_t __len__ = (size_t)addprop_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(addprop_[i]);
		}
	}
}
bool COM_BabyInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize ownerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ownerName_, 65535)) return false;
		}
	}
	// deserialize isShow_
	{
		isShow_ = __fm__.readBit();
	}
	// deserialize isBattle_
	{
		isBattle_ = __fm__.readBit();
	}
	// deserialize isBind_
	{
		isBind_ = __fm__.readBit();
	}
	// deserialize isLock_
	{
		isLock_ = __fm__.readBit();
	}
	// deserialize tableId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(tableId_)) return false;
		}
	}
	// deserialize slot_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(slot_)) return false;
		}
	}
	// deserialize intensifyLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(intensifyLevel_)) return false;
		}
	}
	// deserialize intensifynum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(intensifynum_)) return false;
		}
	}
	// deserialize lastSellTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(lastSellTime_)) return false;
		}
	}
	// deserialize gear_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gear_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(gear_[i])) return false;
		}
	}
	// deserialize addprop_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		addprop_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(addprop_[i])) return false;
		}
	}
		return true;
}
void COM_BabyInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize ownerName_
	ss << "\"ownerName_\":";
	{
		ss << "\"" << ownerName_ << "\"";
	}
	 ss << ",\n";
	// serialize isShow_
	ss << "\"isShow_\":";
	{
		ss << (isShow_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBattle_
	ss << "\"isBattle_\":";
	{
		ss << (isBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBind_
	ss << "\"isBind_\":";
	{
		ss << (isBind_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isLock_
	ss << "\"isLock_\":";
	{
		ss << (isLock_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize tableId_
	ss << "\"tableId_\":";
	{
		ss << (S64)tableId_;
	}
	 ss << ",\n";
	// serialize slot_
	ss << "\"slot_\":";
	{
		ss << (S64)slot_;
	}
	 ss << ",\n";
	// serialize intensifyLevel_
	ss << "\"intensifyLevel_\":";
	{
		ss << (S64)intensifyLevel_;
	}
	 ss << ",\n";
	// serialize intensifynum_
	ss << "\"intensifynum_\":";
	{
		ss << (S64)intensifynum_;
	}
	 ss << ",\n";
	// serialize lastSellTime_
	ss << "\"lastSellTime_\":";
	{
		ss << (S64)lastSellTime_;
	}
	 ss << ",\n";
	// serialize gear_
	ss << "\"gear_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gear_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)gear_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize addprop_
	ss << "\"addprop_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)addprop_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (double)addprop_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_EmployeeInst::COM_EmployeeInst():
isBattle_(false)
,weaponId_(0)
,quality_((QualityColor)(0))
,star_(0)
,soul_(0)
{}
void COM_EmployeeInst::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(ownerName_.length()?true:false);
	__fm__.writeBit(isBattle_);
	__fm__.writeBit((weaponId_==0)?false:true);
	__fm__.writeBit((quality_==(QualityColor)(0))?false:true);
	__fm__.writeBit((star_==0)?false:true);
	__fm__.writeBit((soul_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize ownerName_
	{
		if(ownerName_.length()){
		__s__->writeType(ownerName_);
		}
	}
	// serialize isBattle_
	{
	}
	// serialize weaponId_
	{
		if(weaponId_ != 0){
		__s__->writeType(weaponId_);
		}
	}
	// serialize quality_
	{
		EnumSize __e__ = (EnumSize)quality_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize star_
	{
		if(star_ != 0){
		__s__->writeType(star_);
		}
	}
	// serialize soul_
	{
		if(soul_ != 0){
		__s__->writeType(soul_);
		}
	}
}
bool COM_EmployeeInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize ownerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ownerName_, 65535)) return false;
		}
	}
	// deserialize isBattle_
	{
		isBattle_ = __fm__.readBit();
	}
	// deserialize weaponId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(weaponId_)) return false;
		}
	}
	// deserialize quality_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 16) return false;
		quality_ = (QualityColor)__e__;
		}
	}
	// deserialize star_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(star_)) return false;
		}
	}
	// deserialize soul_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(soul_)) return false;
		}
	}
		return true;
}
void COM_EmployeeInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize ownerName_
	ss << "\"ownerName_\":";
	{
		ss << "\"" << ownerName_ << "\"";
	}
	 ss << ",\n";
	// serialize isBattle_
	ss << "\"isBattle_\":";
	{
		ss << (isBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize weaponId_
	ss << "\"weaponId_\":";
	{
		ss << (S64)weaponId_;
	}
	 ss << ",\n";
	// serialize quality_
	ss << "\"quality_\":";
	{
		ss << "\"" << ENUM(QualityColor).getItemName(quality_) << "\"";
	}
	 ss << ",\n";
	// serialize star_
	ss << "\"star_\":";
	{
		ss << (S64)star_;
	}
	 ss << ",\n";
	// serialize soul_
	ss << "\"soul_\":";
	{
		ss << (S64)soul_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ContactInfo::COM_ContactInfo():
instId_(0)
,level_(0)
,exp_(0)
,job_((JobType)(0))
,assetId_(0)
,jobLevel_(0)
,vip_((VipLevel)(0))
,ff_(0)
,rank_(0)
,section_(0)
,value_(0)
,isLine_(false)
{}
void COM_ContactInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit(name_.length()?true:false);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((exp_==0)?false:true);
	__fm__.writeBit((job_==(JobType)(0))?false:true);
	__fm__.writeBit((assetId_==0)?false:true);
	__fm__.writeBit((jobLevel_==0)?false:true);
	__fm__.writeBit((vip_==(VipLevel)(0))?false:true);
	__fm__.writeBit((ff_==0)?false:true);
	__fm__.writeBit((rank_==0)?false:true);
	__fm__.writeBit((section_==0)?false:true);
	__fm__.writeBit((value_==0)?false:true);
	__fm__.writeBit(isLine_);
	__s__->write(__fm__.masks_, 2);
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize name_
	{
		if(name_.length()){
		__s__->writeType(name_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize exp_
	{
		if(exp_ != 0){
		__s__->writeType(exp_);
		}
	}
	// serialize job_
	{
		EnumSize __e__ = (EnumSize)job_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize assetId_
	{
		if(assetId_ != 0){
		__s__->writeType(assetId_);
		}
	}
	// serialize jobLevel_
	{
		if(jobLevel_ != 0){
		__s__->writeType(jobLevel_);
		}
	}
	// serialize vip_
	{
		EnumSize __e__ = (EnumSize)vip_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize ff_
	{
		if(ff_ != 0){
		__s__->writeType(ff_);
		}
	}
	// serialize rank_
	{
		if(rank_ != 0){
		__s__->writeType(rank_);
		}
	}
	// serialize section_
	{
		if(section_ != 0){
		__s__->writeType(section_);
		}
	}
	// serialize value_
	{
		if(value_ != 0){
		__s__->writeType(value_);
		}
	}
	// serialize isLine_
	{
	}
}
bool COM_ContactInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize name_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(name_, 65535)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize exp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exp_)) return false;
		}
	}
	// deserialize job_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		job_ = (JobType)__e__;
		}
	}
	// deserialize assetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(assetId_)) return false;
		}
	}
	// deserialize jobLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(jobLevel_)) return false;
		}
	}
	// deserialize vip_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 4) return false;
		vip_ = (VipLevel)__e__;
		}
	}
	// deserialize ff_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ff_)) return false;
		}
	}
	// deserialize rank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rank_)) return false;
		}
	}
	// deserialize section_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(section_)) return false;
		}
	}
	// deserialize value_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(value_)) return false;
		}
	}
	// deserialize isLine_
	{
		isLine_ = __fm__.readBit();
	}
		return true;
}
void COM_ContactInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize name_
	ss << "\"name_\":";
	{
		ss << "\"" << name_ << "\"";
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize exp_
	ss << "\"exp_\":";
	{
		ss << (double)exp_;
	}
	 ss << ",\n";
	// serialize job_
	ss << "\"job_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(job_) << "\"";
	}
	 ss << ",\n";
	// serialize assetId_
	ss << "\"assetId_\":";
	{
		ss << (S64)assetId_;
	}
	 ss << ",\n";
	// serialize jobLevel_
	ss << "\"jobLevel_\":";
	{
		ss << (S64)jobLevel_;
	}
	 ss << ",\n";
	// serialize vip_
	ss << "\"vip_\":";
	{
		ss << "\"" << ENUM(VipLevel).getItemName(vip_) << "\"";
	}
	 ss << ",\n";
	// serialize ff_
	ss << "\"ff_\":";
	{
		ss << (S64)ff_;
	}
	 ss << ",\n";
	// serialize rank_
	ss << "\"rank_\":";
	{
		ss << (S64)rank_;
	}
	 ss << ",\n";
	// serialize section_
	ss << "\"section_\":";
	{
		ss << (S64)section_;
	}
	 ss << ",\n";
	// serialize value_
	ss << "\"value_\":";
	{
		ss << (S64)value_;
	}
	 ss << ",\n";
	// serialize isLine_
	ss << "\"isLine_\":";
	{
		ss << (isLine_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_ContactInfoExt::SGE_ContactInfoExt():
rolefirst_(0)
,rolelast_(0)
,logoutTime_(0)
,gold_(0)
,diamond_(0)
,magicgold_(0)
,guildContribute_(0)
,serverid_(0)
{}
void SGE_ContactInfoExt::serialize(ProtocolWriter* __s__) const
{
	COM_ContactInfo::serialize(__s__);
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((rolefirst_==0)?false:true);
	__fm__.writeBit((rolelast_==0)?false:true);
	__fm__.writeBit((logoutTime_==0)?false:true);
	__fm__.writeBit((gold_==0)?false:true);
	__fm__.writeBit((diamond_==0)?false:true);
	__fm__.writeBit((magicgold_==0)?false:true);
	__fm__.writeBit((guildContribute_==0)?false:true);
	__fm__.writeBit(accName_.length()?true:false);
	__fm__.writeBit(userid_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(pfname_.length()?true:false);
	__fm__.writeBit((serverid_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize rolefirst_
	{
		if(rolefirst_ != 0){
		__s__->writeType(rolefirst_);
		}
	}
	// serialize rolelast_
	{
		if(rolelast_ != 0){
		__s__->writeType(rolelast_);
		}
	}
	// serialize logoutTime_
	{
		if(logoutTime_ != 0){
		__s__->writeType(logoutTime_);
		}
	}
	// serialize gold_
	{
		if(gold_ != 0){
		__s__->writeType(gold_);
		}
	}
	// serialize diamond_
	{
		if(diamond_ != 0){
		__s__->writeType(diamond_);
		}
	}
	// serialize magicgold_
	{
		if(magicgold_ != 0){
		__s__->writeType(magicgold_);
		}
	}
	// serialize guildContribute_
	{
		if(guildContribute_ != 0){
		__s__->writeType(guildContribute_);
		}
	}
	// serialize accName_
	{
		if(accName_.length()){
		__s__->writeType(accName_);
		}
	}
	// serialize userid_
	{
		if(userid_.length()){
		__s__->writeType(userid_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize pfname_
	{
		if(pfname_.length()){
		__s__->writeType(pfname_);
		}
	}
	// serialize serverid_
	{
		if(serverid_ != 0){
		__s__->writeType(serverid_);
		}
	}
}
bool SGE_ContactInfoExt::deserialize(ProtocolReader* __r__)
{
	if(!COM_ContactInfo::deserialize(__r__)) return false;
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize rolefirst_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolefirst_)) return false;
		}
	}
	// deserialize rolelast_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolelast_)) return false;
		}
	}
	// deserialize logoutTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(logoutTime_)) return false;
		}
	}
	// deserialize gold_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gold_)) return false;
		}
	}
	// deserialize diamond_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(diamond_)) return false;
		}
	}
	// deserialize magicgold_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicgold_)) return false;
		}
	}
	// deserialize guildContribute_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildContribute_)) return false;
		}
	}
	// deserialize accName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accName_, 65535)) return false;
		}
	}
	// deserialize userid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(userid_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize pfname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfname_, 65535)) return false;
		}
	}
	// deserialize serverid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(serverid_)) return false;
		}
	}
		return true;
}
void SGE_ContactInfoExt::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_ContactInfo::serializeJson(ss,false);
	// serialize rolefirst_
	ss << "\"rolefirst_\":";
	{
		ss << (S64)rolefirst_;
	}
	 ss << ",\n";
	// serialize rolelast_
	ss << "\"rolelast_\":";
	{
		ss << (S64)rolelast_;
	}
	 ss << ",\n";
	// serialize logoutTime_
	ss << "\"logoutTime_\":";
	{
		ss << (S64)logoutTime_;
	}
	 ss << ",\n";
	// serialize gold_
	ss << "\"gold_\":";
	{
		ss << (S64)gold_;
	}
	 ss << ",\n";
	// serialize diamond_
	ss << "\"diamond_\":";
	{
		ss << (S64)diamond_;
	}
	 ss << ",\n";
	// serialize magicgold_
	ss << "\"magicgold_\":";
	{
		ss << (S64)magicgold_;
	}
	 ss << ",\n";
	// serialize guildContribute_
	ss << "\"guildContribute_\":";
	{
		ss << (S64)guildContribute_;
	}
	 ss << ",\n";
	// serialize accName_
	ss << "\"accName_\":";
	{
		ss << "\"" << accName_ << "\"";
	}
	 ss << ",\n";
	// serialize userid_
	ss << "\"userid_\":";
	{
		ss << "\"" << userid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfname_
	ss << "\"pfname_\":";
	{
		ss << "\"" << pfname_ << "\"";
	}
	 ss << ",\n";
	// serialize serverid_
	ss << "\"serverid_\":";
	{
		ss << (S64)serverid_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Achievement::COM_Achievement():
achId_(0)
,achType_((AchievementType)(0))
,achValue_(0)
,isAch_(false)
,isAward_(false)
{}
void COM_Achievement::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((achId_==0)?false:true);
	__fm__.writeBit((achType_==(AchievementType)(0))?false:true);
	__fm__.writeBit((achValue_==0)?false:true);
	__fm__.writeBit(isAch_);
	__fm__.writeBit(isAward_);
	__s__->write(__fm__.masks_, 1);
	// serialize achId_
	{
		if(achId_ != 0){
		__s__->writeType(achId_);
		}
	}
	// serialize achType_
	{
		EnumSize __e__ = (EnumSize)achType_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize achValue_
	{
		if(achValue_ != 0){
		__s__->writeType(achValue_);
		}
	}
	// serialize isAch_
	{
	}
	// serialize isAward_
	{
	}
}
bool COM_Achievement::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize achId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(achId_)) return false;
		}
	}
	// deserialize achType_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 60) return false;
		achType_ = (AchievementType)__e__;
		}
	}
	// deserialize achValue_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(achValue_)) return false;
		}
	}
	// deserialize isAch_
	{
		isAch_ = __fm__.readBit();
	}
	// deserialize isAward_
	{
		isAward_ = __fm__.readBit();
	}
		return true;
}
void COM_Achievement::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize achId_
	ss << "\"achId_\":";
	{
		ss << (S64)achId_;
	}
	 ss << ",\n";
	// serialize achType_
	ss << "\"achType_\":";
	{
		ss << "\"" << ENUM(AchievementType).getItemName(achType_) << "\"";
	}
	 ss << ",\n";
	// serialize achValue_
	ss << "\"achValue_\":";
	{
		ss << (S64)achValue_;
	}
	 ss << ",\n";
	// serialize isAch_
	ss << "\"isAch_\":";
	{
		ss << (isAch_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isAward_
	ss << "\"isAward_\":";
	{
		ss << (isAward_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_HundredBattle::COM_HundredBattle():
playerId_(0)
,tier_(0)
,curTier_(0)
,surplus_(0)
,resetNum_(0)
{}
void COM_HundredBattle::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerId_==0)?false:true);
	__fm__.writeBit((tier_==0)?false:true);
	__fm__.writeBit((curTier_==0)?false:true);
	__fm__.writeBit((surplus_==0)?false:true);
	__fm__.writeBit((resetNum_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize playerId_
	{
		if(playerId_ != 0){
		__s__->writeType(playerId_);
		}
	}
	// serialize tier_
	{
		if(tier_ != 0){
		__s__->writeType(tier_);
		}
	}
	// serialize curTier_
	{
		if(curTier_ != 0){
		__s__->writeType(curTier_);
		}
	}
	// serialize surplus_
	{
		if(surplus_ != 0){
		__s__->writeType(surplus_);
		}
	}
	// serialize resetNum_
	{
		if(resetNum_ != 0){
		__s__->writeType(resetNum_);
		}
	}
}
bool COM_HundredBattle::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerId_)) return false;
		}
	}
	// deserialize tier_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(tier_)) return false;
		}
	}
	// deserialize curTier_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(curTier_)) return false;
		}
	}
	// deserialize surplus_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(surplus_)) return false;
		}
	}
	// deserialize resetNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(resetNum_)) return false;
		}
	}
		return true;
}
void COM_HundredBattle::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerId_
	ss << "\"playerId_\":";
	{
		ss << (S64)playerId_;
	}
	 ss << ",\n";
	// serialize tier_
	ss << "\"tier_\":";
	{
		ss << (S64)tier_;
	}
	 ss << ",\n";
	// serialize curTier_
	ss << "\"curTier_\":";
	{
		ss << (S64)curTier_;
	}
	 ss << ",\n";
	// serialize surplus_
	ss << "\"surplus_\":";
	{
		ss << (S64)surplus_;
	}
	 ss << ",\n";
	// serialize resetNum_
	ss << "\"resetNum_\":";
	{
		ss << (S64)resetNum_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_PlayerVsPlayer::COM_PlayerVsPlayer():
playerInst_(0)
,section_(0)
,value_(0)
,winNum_(0)
,battleNum_(0)
,winValue_(0)
,contWin_(0)
,isCont_(false)
{}
void COM_PlayerVsPlayer::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerInst_==0)?false:true);
	__fm__.writeBit((section_==0)?false:true);
	__fm__.writeBit((value_==0)?false:true);
	__fm__.writeBit((winNum_==0)?false:true);
	__fm__.writeBit((battleNum_==0)?false:true);
	__fm__.writeBit((winValue_==0)?false:true);
	__fm__.writeBit((contWin_==0)?false:true);
	__fm__.writeBit(isCont_);
	__s__->write(__fm__.masks_, 1);
	// serialize playerInst_
	{
		if(playerInst_ != 0){
		__s__->writeType(playerInst_);
		}
	}
	// serialize section_
	{
		if(section_ != 0){
		__s__->writeType(section_);
		}
	}
	// serialize value_
	{
		if(value_ != 0){
		__s__->writeType(value_);
		}
	}
	// serialize winNum_
	{
		if(winNum_ != 0){
		__s__->writeType(winNum_);
		}
	}
	// serialize battleNum_
	{
		if(battleNum_ != 0){
		__s__->writeType(battleNum_);
		}
	}
	// serialize winValue_
	{
		if(winValue_ != 0){
		__s__->writeType(winValue_);
		}
	}
	// serialize contWin_
	{
		if(contWin_ != 0){
		__s__->writeType(contWin_);
		}
	}
	// serialize isCont_
	{
	}
}
bool COM_PlayerVsPlayer::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerInst_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerInst_)) return false;
		}
	}
	// deserialize section_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(section_)) return false;
		}
	}
	// deserialize value_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(value_)) return false;
		}
	}
	// deserialize winNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(winNum_)) return false;
		}
	}
	// deserialize battleNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(battleNum_)) return false;
		}
	}
	// deserialize winValue_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(winValue_)) return false;
		}
	}
	// deserialize contWin_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(contWin_)) return false;
		}
	}
	// deserialize isCont_
	{
		isCont_ = __fm__.readBit();
	}
		return true;
}
void COM_PlayerVsPlayer::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerInst_
	ss << "\"playerInst_\":";
	{
		ss << (S64)playerInst_;
	}
	 ss << ",\n";
	// serialize section_
	ss << "\"section_\":";
	{
		ss << (S64)section_;
	}
	 ss << ",\n";
	// serialize value_
	ss << "\"value_\":";
	{
		ss << (S64)value_;
	}
	 ss << ",\n";
	// serialize winNum_
	ss << "\"winNum_\":";
	{
		ss << (S64)winNum_;
	}
	 ss << ",\n";
	// serialize battleNum_
	ss << "\"battleNum_\":";
	{
		ss << (S64)battleNum_;
	}
	 ss << ",\n";
	// serialize winValue_
	ss << "\"winValue_\":";
	{
		ss << (double)winValue_;
	}
	 ss << ",\n";
	// serialize contWin_
	ss << "\"contWin_\":";
	{
		ss << (S64)contWin_;
	}
	 ss << ",\n";
	// serialize isCont_
	ss << "\"isCont_\":";
	{
		ss << (isCont_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Activity::COM_Activity():
actId_(0)
,counter_(0)
{}
void COM_Activity::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((actId_==0)?false:true);
	__fm__.writeBit((counter_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize actId_
	{
		if(actId_ != 0){
		__s__->writeType(actId_);
		}
	}
	// serialize counter_
	{
		if(counter_ != 0){
		__s__->writeType(counter_);
		}
	}
}
bool COM_Activity::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize actId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(actId_)) return false;
		}
	}
	// deserialize counter_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(counter_)) return false;
		}
	}
		return true;
}
void COM_Activity::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize actId_
	ss << "\"actId_\":";
	{
		ss << (S64)actId_;
	}
	 ss << ",\n";
	// serialize counter_
	ss << "\"counter_\":";
	{
		ss << (S64)counter_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ActivityTable::COM_ActivityTable():
reward_(0)
{}
void COM_ActivityTable::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(activities_.size()?true:false);
	__fm__.writeBit(flag_.size()?true:false);
	__fm__.writeBit((reward_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize activities_
	if(activities_.size())
	{
		size_t __len__ = (size_t)activities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			activities_[i].serialize(__s__);
		}
	}
	// serialize flag_
	if(flag_.size())
	{
		size_t __len__ = (size_t)flag_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(flag_[i]);
		}
	}
	// serialize reward_
	{
		if(reward_ != 0){
		__s__->writeType(reward_);
		}
	}
}
bool COM_ActivityTable::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize activities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		activities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!activities_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize flag_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		flag_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(flag_[i])) return false;
		}
	}
	// deserialize reward_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(reward_)) return false;
		}
	}
		return true;
}
void COM_ActivityTable::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize activities_
	ss << "\"activities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)activities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			activities_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize flag_
	ss << "\"flag_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)flag_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)flag_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize reward_
	ss << "\"reward_\":";
	{
		ss << (S64)reward_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_CourseGift::COM_CourseGift():
id_(0)
,timeout_(0)
{}
void COM_CourseGift::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((id_==0)?false:true);
	__fm__.writeBit((timeout_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize id_
	{
		if(id_ != 0){
		__s__->writeType(id_);
		}
	}
	// serialize timeout_
	{
		if(timeout_ != 0){
		__s__->writeType(timeout_);
		}
	}
}
bool COM_CourseGift::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize id_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(id_)) return false;
		}
	}
	// deserialize timeout_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(timeout_)) return false;
		}
	}
		return true;
}
void COM_CourseGift::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize id_
	ss << "\"id_\":";
	{
		ss << (S64)id_;
	}
	 ss << ",\n";
	// serialize timeout_
	ss << "\"timeout_\":";
	{
		ss << (double)timeout_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SimplePlayerInst::COM_SimplePlayerInst():
isLeavingTeam_(false)
,isBattle_(false)
,autoBattle_(false)
,isTeamLeader_(false)
,sceneId_(0)
,openSubSystemFlag_(0)
,createTime_(0)
{}
void COM_SimplePlayerInst::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(isLeavingTeam_);
	__fm__.writeBit(isBattle_);
	__fm__.writeBit(autoBattle_);
	__fm__.writeBit(isTeamLeader_);
	__fm__.writeBit((sceneId_==0)?false:true);
	__fm__.writeBit((openSubSystemFlag_==0)?false:true);
	__fm__.writeBit((createTime_==0)?false:true);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(babies1_.size()?true:false);
	__fm__.writeBit(battleEmps_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize isLeavingTeam_
	{
	}
	// serialize isBattle_
	{
	}
	// serialize autoBattle_
	{
	}
	// serialize isTeamLeader_
	{
	}
	// serialize sceneId_
	{
		if(sceneId_ != 0){
		__s__->writeType(sceneId_);
		}
	}
	// serialize openSubSystemFlag_
	{
		if(openSubSystemFlag_ != 0){
		__s__->writeType(openSubSystemFlag_);
		}
	}
	// serialize createTime_
	{
		if(createTime_ != 0){
		__s__->writeType(createTime_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize scenePos_
	{
		scenePos_.serialize(__s__);
	}
	// serialize pvpInfo_
	{
		pvpInfo_.serialize(__s__);
	}
	// serialize babies1_
	if(babies1_.size())
	{
		size_t __len__ = (size_t)babies1_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			babies1_[i].serialize(__s__);
		}
	}
	// serialize battleEmps_
	if(battleEmps_.size())
	{
		size_t __len__ = (size_t)battleEmps_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			battleEmps_[i].serialize(__s__);
		}
	}
}
bool COM_SimplePlayerInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize isLeavingTeam_
	{
		isLeavingTeam_ = __fm__.readBit();
	}
	// deserialize isBattle_
	{
		isBattle_ = __fm__.readBit();
	}
	// deserialize autoBattle_
	{
		autoBattle_ = __fm__.readBit();
	}
	// deserialize isTeamLeader_
	{
		isTeamLeader_ = __fm__.readBit();
	}
	// deserialize sceneId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sceneId_)) return false;
		}
	}
	// deserialize openSubSystemFlag_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(openSubSystemFlag_)) return false;
		}
	}
	// deserialize createTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createTime_)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize scenePos_
	{
		if(__fm__.readBit()){
		if(!scenePos_.deserialize(__r__)) return false;
		}
	}
	// deserialize pvpInfo_
	{
		if(__fm__.readBit()){
		if(!pvpInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize babies1_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babies1_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!babies1_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize battleEmps_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		battleEmps_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!battleEmps_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_SimplePlayerInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize isLeavingTeam_
	ss << "\"isLeavingTeam_\":";
	{
		ss << (isLeavingTeam_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBattle_
	ss << "\"isBattle_\":";
	{
		ss << (isBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize autoBattle_
	ss << "\"autoBattle_\":";
	{
		ss << (autoBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isTeamLeader_
	ss << "\"isTeamLeader_\":";
	{
		ss << (isTeamLeader_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize sceneId_
	ss << "\"sceneId_\":";
	{
		ss << (S64)sceneId_;
	}
	 ss << ",\n";
	// serialize openSubSystemFlag_
	ss << "\"openSubSystemFlag_\":";
	{
		ss << (S64)openSubSystemFlag_;
	}
	 ss << ",\n";
	// serialize createTime_
	ss << "\"createTime_\":";
	{
		ss << (S64)createTime_;
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize scenePos_
	ss << "\"scenePos_\":";
	{
		scenePos_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize pvpInfo_
	ss << "\"pvpInfo_\":";
	{
		pvpInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize babies1_
	ss << "\"babies1_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babies1_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			babies1_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize battleEmps_
	ss << "\"battleEmps_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)battleEmps_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			battleEmps_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_PlayerInst::COM_PlayerInst():
isLeavingTeam_(false)
,isBattle_(false)
,autoBattle_(false)
,isTeamLeader_(false)
,sceneId_(0)
,openSubSystemFlag_(0)
,createTime_(0)
,onlineTimeFlag_(false)
,onlineTime_(0)
,isFund_(false)
,openDoubleTimeFlag_(false)
,isFirstLogin_(false)
,firstRechargeDiamond_(false)
,isFirstRechargeGift_(false)
,offlineExp_(0)
,rivalTime_(0)
,rivalNum_(0)
,promoteAward_(0)
,guideIdx_(0)
,noTalkTime_(0)
,wishShareNum_(0)
,warriortrophyNum_(0)
,employeelasttime_(0)
,employeeonecount_(0)
,employeetencount_(0)
,greenBoxTimes_(0)
,blueBoxTimes_(0)
,greenBoxFreeNum_(0)
,magicItemLevel_(0)
,magicItemeExp_(0)
,magicItemeJob_((JobType)(0))
,magicTupoLevel_(0)
,guildContribution_(0)
,exitGuildTime_(0)
,sevenflag_(false)
,signFlag_(false)
,viprewardflag_(false)
{}
void COM_PlayerInst::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<9> __fm__;
	__fm__.writeBit(isLeavingTeam_);
	__fm__.writeBit(isBattle_);
	__fm__.writeBit(autoBattle_);
	__fm__.writeBit(isTeamLeader_);
	__fm__.writeBit((sceneId_==0)?false:true);
	__fm__.writeBit((openSubSystemFlag_==0)?false:true);
	__fm__.writeBit((createTime_==0)?false:true);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(onlineTimeFlag_);
	__fm__.writeBit((onlineTime_==0)?false:true);
	__fm__.writeBit(onlineTimeReward_.size()?true:false);
	__fm__.writeBit(isFund_);
	__fm__.writeBit(fundtags_.size()?true:false);
	__fm__.writeBit(openDoubleTimeFlag_);
	__fm__.writeBit(isFirstLogin_);
	__fm__.writeBit(firstRechargeDiamond_);
	__fm__.writeBit(isFirstRechargeGift_);
	__fm__.writeBit((offlineExp_==0)?false:true);
	__fm__.writeBit((rivalTime_==0)?false:true);
	__fm__.writeBit((rivalNum_==0)?false:true);
	__fm__.writeBit((promoteAward_==0)?false:true);
	__fm__.writeBit((guideIdx_==0)?false:true);
	__fm__.writeBit((noTalkTime_==0)?false:true);
	__fm__.writeBit((wishShareNum_==0)?false:true);
	__fm__.writeBit((warriortrophyNum_==0)?false:true);
	__fm__.writeBit((employeelasttime_==0)?false:true);
	__fm__.writeBit((employeeonecount_==0)?false:true);
	__fm__.writeBit((employeetencount_==0)?false:true);
	__fm__.writeBit((greenBoxTimes_==0)?false:true);
	__fm__.writeBit((blueBoxTimes_==0)?false:true);
	__fm__.writeBit((greenBoxFreeNum_==0)?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(openScenes_.size()?true:false);
	__fm__.writeBit(copyNum_.size()?true:false);
	__fm__.writeBit((magicItemLevel_==0)?false:true);
	__fm__.writeBit((magicItemeExp_==0)?false:true);
	__fm__.writeBit((magicItemeJob_==(JobType)(0))?false:true);
	__fm__.writeBit((magicTupoLevel_==0)?false:true);
	__fm__.writeBit(cachedNpcs_.size()?true:false);
	__fm__.writeBit(gft_.size()?true:false);
	__fm__.writeBit(babycache_.size()?true:false);
	__fm__.writeBit(titles_.size()?true:false);
	__fm__.writeBit((guildContribution_==0)?false:true);
	__fm__.writeBit((exitGuildTime_==0)?false:true);
	__fm__.writeBit(guildSkills_.size()?true:false);
	__fm__.writeBit(gmActivities_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(sevenflag_);
	__fm__.writeBit(signFlag_);
	__fm__.writeBit(sevendata_.size()?true:false);
	__fm__.writeBit(viprewardflag_);
	__fm__.writeBit(phoneNumber_.length()?true:false);
	__fm__.writeBit(levelgift_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(fuwen_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(coursegift_.size()?true:false);
	__s__->write(__fm__.masks_, 9);
	// serialize isLeavingTeam_
	{
	}
	// serialize isBattle_
	{
	}
	// serialize autoBattle_
	{
	}
	// serialize isTeamLeader_
	{
	}
	// serialize sceneId_
	{
		if(sceneId_ != 0){
		__s__->writeType(sceneId_);
		}
	}
	// serialize openSubSystemFlag_
	{
		if(openSubSystemFlag_ != 0){
		__s__->writeType(openSubSystemFlag_);
		}
	}
	// serialize createTime_
	{
		if(createTime_ != 0){
		__s__->writeType(createTime_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize scenePos_
	{
		scenePos_.serialize(__s__);
	}
	// serialize pvpInfo_
	{
		pvpInfo_.serialize(__s__);
	}
	// serialize onlineTimeFlag_
	{
	}
	// serialize onlineTime_
	{
		if(onlineTime_ != 0){
		__s__->writeType(onlineTime_);
		}
	}
	// serialize onlineTimeReward_
	if(onlineTimeReward_.size())
	{
		size_t __len__ = (size_t)onlineTimeReward_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(onlineTimeReward_[i]);
		}
	}
	// serialize isFund_
	{
	}
	// serialize fundtags_
	if(fundtags_.size())
	{
		size_t __len__ = (size_t)fundtags_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(fundtags_[i]);
		}
	}
	// serialize openDoubleTimeFlag_
	{
	}
	// serialize isFirstLogin_
	{
	}
	// serialize firstRechargeDiamond_
	{
	}
	// serialize isFirstRechargeGift_
	{
	}
	// serialize offlineExp_
	{
		if(offlineExp_ != 0){
		__s__->writeType(offlineExp_);
		}
	}
	// serialize rivalTime_
	{
		if(rivalTime_ != 0){
		__s__->writeType(rivalTime_);
		}
	}
	// serialize rivalNum_
	{
		if(rivalNum_ != 0){
		__s__->writeType(rivalNum_);
		}
	}
	// serialize promoteAward_
	{
		if(promoteAward_ != 0){
		__s__->writeType(promoteAward_);
		}
	}
	// serialize guideIdx_
	{
		if(guideIdx_ != 0){
		__s__->writeType(guideIdx_);
		}
	}
	// serialize noTalkTime_
	{
		if(noTalkTime_ != 0){
		__s__->writeType(noTalkTime_);
		}
	}
	// serialize wishShareNum_
	{
		if(wishShareNum_ != 0){
		__s__->writeType(wishShareNum_);
		}
	}
	// serialize warriortrophyNum_
	{
		if(warriortrophyNum_ != 0){
		__s__->writeType(warriortrophyNum_);
		}
	}
	// serialize employeelasttime_
	{
		if(employeelasttime_ != 0){
		__s__->writeType(employeelasttime_);
		}
	}
	// serialize employeeonecount_
	{
		if(employeeonecount_ != 0){
		__s__->writeType(employeeonecount_);
		}
	}
	// serialize employeetencount_
	{
		if(employeetencount_ != 0){
		__s__->writeType(employeetencount_);
		}
	}
	// serialize greenBoxTimes_
	{
		if(greenBoxTimes_ != 0){
		__s__->writeType(greenBoxTimes_);
		}
	}
	// serialize blueBoxTimes_
	{
		if(blueBoxTimes_ != 0){
		__s__->writeType(blueBoxTimes_);
		}
	}
	// serialize greenBoxFreeNum_
	{
		if(greenBoxFreeNum_ != 0){
		__s__->writeType(greenBoxFreeNum_);
		}
	}
	// serialize hbInfo_
	{
		hbInfo_.serialize(__s__);
	}
	// serialize openScenes_
	if(openScenes_.size())
	{
		size_t __len__ = (size_t)openScenes_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(openScenes_[i]);
		}
	}
	// serialize copyNum_
	if(copyNum_.size())
	{
		size_t __len__ = (size_t)copyNum_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(copyNum_[i]);
		}
	}
	// serialize magicItemLevel_
	{
		if(magicItemLevel_ != 0){
		__s__->writeType(magicItemLevel_);
		}
	}
	// serialize magicItemeExp_
	{
		if(magicItemeExp_ != 0){
		__s__->writeType(magicItemeExp_);
		}
	}
	// serialize magicItemeJob_
	{
		EnumSize __e__ = (EnumSize)magicItemeJob_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize magicTupoLevel_
	{
		if(magicTupoLevel_ != 0){
		__s__->writeType(magicTupoLevel_);
		}
	}
	// serialize cachedNpcs_
	if(cachedNpcs_.size())
	{
		size_t __len__ = (size_t)cachedNpcs_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(cachedNpcs_[i]);
		}
	}
	// serialize gft_
	if(gft_.size())
	{
		size_t __len__ = (size_t)gft_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(gft_[i]);
		}
	}
	// serialize babycache_
	if(babycache_.size())
	{
		size_t __len__ = (size_t)babycache_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(babycache_[i]);
		}
	}
	// serialize titles_
	if(titles_.size())
	{
		size_t __len__ = (size_t)titles_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(titles_[i]);
		}
	}
	// serialize guildContribution_
	{
		if(guildContribution_ != 0){
		__s__->writeType(guildContribution_);
		}
	}
	// serialize exitGuildTime_
	{
		if(exitGuildTime_ != 0){
		__s__->writeType(exitGuildTime_);
		}
	}
	// serialize guildSkills_
	if(guildSkills_.size())
	{
		size_t __len__ = (size_t)guildSkills_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			guildSkills_[i].serialize(__s__);
		}
	}
	// serialize gmActivities_
	if(gmActivities_.size())
	{
		size_t __len__ = (size_t)gmActivities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			EnumSize __e__ = (EnumSize)gmActivities_[i];
			__s__->writeType(__e__);
		}
	}
	// serialize festival_
	{
		festival_.serialize(__s__);
	}
	// serialize selfRecharge_
	{
		selfRecharge_.serialize(__s__);
	}
	// serialize sysRecharge_
	{
		sysRecharge_.serialize(__s__);
	}
	// serialize selfDiscountStore_
	{
		selfDiscountStore_.serialize(__s__);
	}
	// serialize sysDiscountStore_
	{
		sysDiscountStore_.serialize(__s__);
	}
	// serialize selfOnceRecharge_
	{
		selfOnceRecharge_.serialize(__s__);
	}
	// serialize sysOnceRecharge_
	{
		sysOnceRecharge_.serialize(__s__);
	}
	// serialize empact_
	{
		empact_.serialize(__s__);
	}
	// serialize selfCards_
	{
		selfCards_.serialize(__s__);
	}
	// serialize myselfRecharge_
	{
		myselfRecharge_.serialize(__s__);
	}
	// serialize hotdata_
	{
		hotdata_.serialize(__s__);
	}
	// serialize gbdata_
	{
		gbdata_.serialize(__s__);
	}
	// serialize sevenflag_
	{
	}
	// serialize signFlag_
	{
	}
	// serialize sevendata_
	if(sevendata_.size())
	{
		size_t __len__ = (size_t)sevendata_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			sevendata_[i].serialize(__s__);
		}
	}
	// serialize viprewardflag_
	{
	}
	// serialize phoneNumber_
	{
		if(phoneNumber_.length()){
		__s__->writeType(phoneNumber_);
		}
	}
	// serialize levelgift_
	if(levelgift_.size())
	{
		size_t __len__ = (size_t)levelgift_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(levelgift_[i]);
		}
	}
	// serialize activity_
	{
		activity_.serialize(__s__);
	}
	// serialize fuwen_
	if(fuwen_.size())
	{
		size_t __len__ = (size_t)fuwen_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			fuwen_[i].serialize(__s__);
		}
	}
	// serialize crystalData_
	{
		crystalData_.serialize(__s__);
	}
	// serialize integralData_
	{
		integralData_.serialize(__s__);
	}
	// serialize coursegift_
	if(coursegift_.size())
	{
		size_t __len__ = (size_t)coursegift_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			coursegift_[i].serialize(__s__);
		}
	}
}
bool COM_PlayerInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<9> __fm__;
	if(!__r__->read(__fm__.masks_, 9)) return false;
	// deserialize isLeavingTeam_
	{
		isLeavingTeam_ = __fm__.readBit();
	}
	// deserialize isBattle_
	{
		isBattle_ = __fm__.readBit();
	}
	// deserialize autoBattle_
	{
		autoBattle_ = __fm__.readBit();
	}
	// deserialize isTeamLeader_
	{
		isTeamLeader_ = __fm__.readBit();
	}
	// deserialize sceneId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sceneId_)) return false;
		}
	}
	// deserialize openSubSystemFlag_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(openSubSystemFlag_)) return false;
		}
	}
	// deserialize createTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createTime_)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize scenePos_
	{
		if(__fm__.readBit()){
		if(!scenePos_.deserialize(__r__)) return false;
		}
	}
	// deserialize pvpInfo_
	{
		if(__fm__.readBit()){
		if(!pvpInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize onlineTimeFlag_
	{
		onlineTimeFlag_ = __fm__.readBit();
	}
	// deserialize onlineTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(onlineTime_)) return false;
		}
	}
	// deserialize onlineTimeReward_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		onlineTimeReward_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(onlineTimeReward_[i])) return false;
		}
	}
	// deserialize isFund_
	{
		isFund_ = __fm__.readBit();
	}
	// deserialize fundtags_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		fundtags_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(fundtags_[i])) return false;
		}
	}
	// deserialize openDoubleTimeFlag_
	{
		openDoubleTimeFlag_ = __fm__.readBit();
	}
	// deserialize isFirstLogin_
	{
		isFirstLogin_ = __fm__.readBit();
	}
	// deserialize firstRechargeDiamond_
	{
		firstRechargeDiamond_ = __fm__.readBit();
	}
	// deserialize isFirstRechargeGift_
	{
		isFirstRechargeGift_ = __fm__.readBit();
	}
	// deserialize offlineExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(offlineExp_)) return false;
		}
	}
	// deserialize rivalTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalTime_)) return false;
		}
	}
	// deserialize rivalNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalNum_)) return false;
		}
	}
	// deserialize promoteAward_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(promoteAward_)) return false;
		}
	}
	// deserialize guideIdx_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guideIdx_)) return false;
		}
	}
	// deserialize noTalkTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(noTalkTime_)) return false;
		}
	}
	// deserialize wishShareNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(wishShareNum_)) return false;
		}
	}
	// deserialize warriortrophyNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(warriortrophyNum_)) return false;
		}
	}
	// deserialize employeelasttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeelasttime_)) return false;
		}
	}
	// deserialize employeeonecount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeeonecount_)) return false;
		}
	}
	// deserialize employeetencount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeetencount_)) return false;
		}
	}
	// deserialize greenBoxTimes_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(greenBoxTimes_)) return false;
		}
	}
	// deserialize blueBoxTimes_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(blueBoxTimes_)) return false;
		}
	}
	// deserialize greenBoxFreeNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(greenBoxFreeNum_)) return false;
		}
	}
	// deserialize hbInfo_
	{
		if(__fm__.readBit()){
		if(!hbInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize openScenes_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		openScenes_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(openScenes_[i])) return false;
		}
	}
	// deserialize copyNum_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		copyNum_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(copyNum_[i])) return false;
		}
	}
	// deserialize magicItemLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicItemLevel_)) return false;
		}
	}
	// deserialize magicItemeExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicItemeExp_)) return false;
		}
	}
	// deserialize magicItemeJob_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		magicItemeJob_ = (JobType)__e__;
		}
	}
	// deserialize magicTupoLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicTupoLevel_)) return false;
		}
	}
	// deserialize cachedNpcs_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		cachedNpcs_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(cachedNpcs_[i])) return false;
		}
	}
	// deserialize gft_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gft_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(gft_[i], 65535)) return false;
		}
	}
	// deserialize babycache_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babycache_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(babycache_[i])) return false;
		}
	}
	// deserialize titles_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		titles_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(titles_[i])) return false;
		}
	}
	// deserialize guildContribution_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildContribution_)) return false;
		}
	}
	// deserialize exitGuildTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exitGuildTime_)) return false;
		}
	}
	// deserialize guildSkills_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		guildSkills_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!guildSkills_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize gmActivities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gmActivities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			EnumSize __e__;
			if(!__r__->readType(__e__) || __e__ >= 21) return false;
			gmActivities_[i] = (ADType)__e__;
		}
	}
	// deserialize festival_
	{
		if(__fm__.readBit()){
		if(!festival_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfRecharge_
	{
		if(__fm__.readBit()){
		if(!selfRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysRecharge_
	{
		if(__fm__.readBit()){
		if(!sysRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfDiscountStore_
	{
		if(__fm__.readBit()){
		if(!selfDiscountStore_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysDiscountStore_
	{
		if(__fm__.readBit()){
		if(!sysDiscountStore_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfOnceRecharge_
	{
		if(__fm__.readBit()){
		if(!selfOnceRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysOnceRecharge_
	{
		if(__fm__.readBit()){
		if(!sysOnceRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize empact_
	{
		if(__fm__.readBit()){
		if(!empact_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfCards_
	{
		if(__fm__.readBit()){
		if(!selfCards_.deserialize(__r__)) return false;
		}
	}
	// deserialize myselfRecharge_
	{
		if(__fm__.readBit()){
		if(!myselfRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize hotdata_
	{
		if(__fm__.readBit()){
		if(!hotdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize gbdata_
	{
		if(__fm__.readBit()){
		if(!gbdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize sevenflag_
	{
		sevenflag_ = __fm__.readBit();
	}
	// deserialize signFlag_
	{
		signFlag_ = __fm__.readBit();
	}
	// deserialize sevendata_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		sevendata_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!sevendata_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize viprewardflag_
	{
		viprewardflag_ = __fm__.readBit();
	}
	// deserialize phoneNumber_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(phoneNumber_, 65535)) return false;
		}
	}
	// deserialize levelgift_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		levelgift_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(levelgift_[i])) return false;
		}
	}
	// deserialize activity_
	{
		if(__fm__.readBit()){
		if(!activity_.deserialize(__r__)) return false;
		}
	}
	// deserialize fuwen_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		fuwen_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!fuwen_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize crystalData_
	{
		if(__fm__.readBit()){
		if(!crystalData_.deserialize(__r__)) return false;
		}
	}
	// deserialize integralData_
	{
		if(__fm__.readBit()){
		if(!integralData_.deserialize(__r__)) return false;
		}
	}
	// deserialize coursegift_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		coursegift_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!coursegift_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_PlayerInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize isLeavingTeam_
	ss << "\"isLeavingTeam_\":";
	{
		ss << (isLeavingTeam_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBattle_
	ss << "\"isBattle_\":";
	{
		ss << (isBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize autoBattle_
	ss << "\"autoBattle_\":";
	{
		ss << (autoBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isTeamLeader_
	ss << "\"isTeamLeader_\":";
	{
		ss << (isTeamLeader_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize sceneId_
	ss << "\"sceneId_\":";
	{
		ss << (S64)sceneId_;
	}
	 ss << ",\n";
	// serialize openSubSystemFlag_
	ss << "\"openSubSystemFlag_\":";
	{
		ss << (S64)openSubSystemFlag_;
	}
	 ss << ",\n";
	// serialize createTime_
	ss << "\"createTime_\":";
	{
		ss << (S64)createTime_;
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize scenePos_
	ss << "\"scenePos_\":";
	{
		scenePos_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize pvpInfo_
	ss << "\"pvpInfo_\":";
	{
		pvpInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize onlineTimeFlag_
	ss << "\"onlineTimeFlag_\":";
	{
		ss << (onlineTimeFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize onlineTime_
	ss << "\"onlineTime_\":";
	{
		ss << (double)onlineTime_;
	}
	 ss << ",\n";
	// serialize onlineTimeReward_
	ss << "\"onlineTimeReward_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)onlineTimeReward_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)onlineTimeReward_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize isFund_
	ss << "\"isFund_\":";
	{
		ss << (isFund_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize fundtags_
	ss << "\"fundtags_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)fundtags_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)fundtags_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize openDoubleTimeFlag_
	ss << "\"openDoubleTimeFlag_\":";
	{
		ss << (openDoubleTimeFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isFirstLogin_
	ss << "\"isFirstLogin_\":";
	{
		ss << (isFirstLogin_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize firstRechargeDiamond_
	ss << "\"firstRechargeDiamond_\":";
	{
		ss << (firstRechargeDiamond_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isFirstRechargeGift_
	ss << "\"isFirstRechargeGift_\":";
	{
		ss << (isFirstRechargeGift_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize offlineExp_
	ss << "\"offlineExp_\":";
	{
		ss << (double)offlineExp_;
	}
	 ss << ",\n";
	// serialize rivalTime_
	ss << "\"rivalTime_\":";
	{
		ss << (double)rivalTime_;
	}
	 ss << ",\n";
	// serialize rivalNum_
	ss << "\"rivalNum_\":";
	{
		ss << (S64)rivalNum_;
	}
	 ss << ",\n";
	// serialize promoteAward_
	ss << "\"promoteAward_\":";
	{
		ss << (S64)promoteAward_;
	}
	 ss << ",\n";
	// serialize guideIdx_
	ss << "\"guideIdx_\":";
	{
		ss << (S64)guideIdx_;
	}
	 ss << ",\n";
	// serialize noTalkTime_
	ss << "\"noTalkTime_\":";
	{
		ss << (double)noTalkTime_;
	}
	 ss << ",\n";
	// serialize wishShareNum_
	ss << "\"wishShareNum_\":";
	{
		ss << (S64)wishShareNum_;
	}
	 ss << ",\n";
	// serialize warriortrophyNum_
	ss << "\"warriortrophyNum_\":";
	{
		ss << (S64)warriortrophyNum_;
	}
	 ss << ",\n";
	// serialize employeelasttime_
	ss << "\"employeelasttime_\":";
	{
		ss << (S64)employeelasttime_;
	}
	 ss << ",\n";
	// serialize employeeonecount_
	ss << "\"employeeonecount_\":";
	{
		ss << (S64)employeeonecount_;
	}
	 ss << ",\n";
	// serialize employeetencount_
	ss << "\"employeetencount_\":";
	{
		ss << (S64)employeetencount_;
	}
	 ss << ",\n";
	// serialize greenBoxTimes_
	ss << "\"greenBoxTimes_\":";
	{
		ss << (double)greenBoxTimes_;
	}
	 ss << ",\n";
	// serialize blueBoxTimes_
	ss << "\"blueBoxTimes_\":";
	{
		ss << (double)blueBoxTimes_;
	}
	 ss << ",\n";
	// serialize greenBoxFreeNum_
	ss << "\"greenBoxFreeNum_\":";
	{
		ss << (S64)greenBoxFreeNum_;
	}
	 ss << ",\n";
	// serialize hbInfo_
	ss << "\"hbInfo_\":";
	{
		hbInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize openScenes_
	ss << "\"openScenes_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)openScenes_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)openScenes_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize copyNum_
	ss << "\"copyNum_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)copyNum_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)copyNum_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize magicItemLevel_
	ss << "\"magicItemLevel_\":";
	{
		ss << (S64)magicItemLevel_;
	}
	 ss << ",\n";
	// serialize magicItemeExp_
	ss << "\"magicItemeExp_\":";
	{
		ss << (S64)magicItemeExp_;
	}
	 ss << ",\n";
	// serialize magicItemeJob_
	ss << "\"magicItemeJob_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(magicItemeJob_) << "\"";
	}
	 ss << ",\n";
	// serialize magicTupoLevel_
	ss << "\"magicTupoLevel_\":";
	{
		ss << (S64)magicTupoLevel_;
	}
	 ss << ",\n";
	// serialize cachedNpcs_
	ss << "\"cachedNpcs_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)cachedNpcs_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)cachedNpcs_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gft_
	ss << "\"gft_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gft_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << "\"" << gft_[i] << "\"";
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babycache_
	ss << "\"babycache_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babycache_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)babycache_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize titles_
	ss << "\"titles_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)titles_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)titles_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize guildContribution_
	ss << "\"guildContribution_\":";
	{
		ss << (S64)guildContribution_;
	}
	 ss << ",\n";
	// serialize exitGuildTime_
	ss << "\"exitGuildTime_\":";
	{
		ss << (S64)exitGuildTime_;
	}
	 ss << ",\n";
	// serialize guildSkills_
	ss << "\"guildSkills_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)guildSkills_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			guildSkills_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gmActivities_
	ss << "\"gmActivities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gmActivities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << "\"" << ENUM(ADType).getItemName(gmActivities_[i]) << "\"";
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize festival_
	ss << "\"festival_\":";
	{
		festival_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfRecharge_
	ss << "\"selfRecharge_\":";
	{
		selfRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysRecharge_
	ss << "\"sysRecharge_\":";
	{
		sysRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfDiscountStore_
	ss << "\"selfDiscountStore_\":";
	{
		selfDiscountStore_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysDiscountStore_
	ss << "\"sysDiscountStore_\":";
	{
		sysDiscountStore_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfOnceRecharge_
	ss << "\"selfOnceRecharge_\":";
	{
		selfOnceRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysOnceRecharge_
	ss << "\"sysOnceRecharge_\":";
	{
		sysOnceRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize empact_
	ss << "\"empact_\":";
	{
		empact_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfCards_
	ss << "\"selfCards_\":";
	{
		selfCards_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize myselfRecharge_
	ss << "\"myselfRecharge_\":";
	{
		myselfRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize hotdata_
	ss << "\"hotdata_\":";
	{
		hotdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize gbdata_
	ss << "\"gbdata_\":";
	{
		gbdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sevenflag_
	ss << "\"sevenflag_\":";
	{
		ss << (sevenflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize signFlag_
	ss << "\"signFlag_\":";
	{
		ss << (signFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize sevendata_
	ss << "\"sevendata_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)sevendata_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			sevendata_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize viprewardflag_
	ss << "\"viprewardflag_\":";
	{
		ss << (viprewardflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize phoneNumber_
	ss << "\"phoneNumber_\":";
	{
		ss << "\"" << phoneNumber_ << "\"";
	}
	 ss << ",\n";
	// serialize levelgift_
	ss << "\"levelgift_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)levelgift_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)levelgift_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize activity_
	ss << "\"activity_\":";
	{
		activity_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize fuwen_
	ss << "\"fuwen_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)fuwen_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			fuwen_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize crystalData_
	ss << "\"crystalData_\":";
	{
		crystalData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize integralData_
	ss << "\"integralData_\":";
	{
		integralData_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize coursegift_
	ss << "\"coursegift_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)coursegift_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			coursegift_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void COM_MonsterInst::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(gear_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize gear_
	if(gear_.size())
	{
		size_t __len__ = (size_t)gear_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(gear_[i]);
		}
	}
}
bool COM_MonsterInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize gear_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gear_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(gear_[i])) return false;
		}
	}
		return true;
}
void COM_MonsterInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize gear_
	ss << "\"gear_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gear_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)gear_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ReportState::COM_ReportState():
add_(false)
,ownerId_(0)
,addQueue_(0)
{}
void COM_ReportState::serialize(ProtocolWriter* __s__) const
{
	COM_State::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(add_);
	__fm__.writeBit((ownerId_==0)?false:true);
	__fm__.writeBit((addQueue_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize add_
	{
	}
	// serialize ownerId_
	{
		if(ownerId_ != 0){
		__s__->writeType(ownerId_);
		}
	}
	// serialize addQueue_
	{
		if(addQueue_ != 0){
		__s__->writeType(addQueue_);
		}
	}
}
bool COM_ReportState::deserialize(ProtocolReader* __r__)
{
	if(!COM_State::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize add_
	{
		add_ = __fm__.readBit();
	}
	// deserialize ownerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ownerId_)) return false;
		}
	}
	// deserialize addQueue_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(addQueue_)) return false;
		}
	}
		return true;
}
void COM_ReportState::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_State::serializeJson(ss,false);
	// serialize add_
	ss << "\"add_\":";
	{
		ss << (add_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize ownerId_
	ss << "\"ownerId_\":";
	{
		ss << (S64)ownerId_;
	}
	 ss << ",\n";
	// serialize addQueue_
	ss << "\"addQueue_\":";
	{
		ss << (S64)addQueue_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ReportTargetBase::COM_ReportTargetBase():
position_((BattlePosition)(0))
,bao_(false)
,fly_(false)
{}
void COM_ReportTargetBase::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((position_==(BattlePosition)(0))?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(bao_);
	__fm__.writeBit(fly_);
	__s__->write(__fm__.masks_, 1);
	// serialize position_
	{
		EnumSize __e__ = (EnumSize)position_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize prop_
	{
		prop_.serialize(__s__);
	}
	// serialize bao_
	{
	}
	// serialize fly_
	{
	}
}
bool COM_ReportTargetBase::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize position_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		position_ = (BattlePosition)__e__;
		}
	}
	// deserialize prop_
	{
		if(__fm__.readBit()){
		if(!prop_.deserialize(__r__)) return false;
		}
	}
	// deserialize bao_
	{
		bao_ = __fm__.readBit();
	}
	// deserialize fly_
	{
		fly_ = __fm__.readBit();
	}
		return true;
}
void COM_ReportTargetBase::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize position_
	ss << "\"position_\":";
	{
		ss << "\"" << ENUM(BattlePosition).getItemName(position_) << "\"";
	}
	 ss << ",\n";
	// serialize prop_
	ss << "\"prop_\":";
	{
		prop_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize bao_
	ss << "\"bao_\":";
	{
		ss << (bao_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize fly_
	ss << "\"fly_\":";
	{
		ss << (fly_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void COM_ReportTarget::serialize(ProtocolWriter* __s__) const
{
	COM_ReportTargetBase::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(prop2_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize prop2_
	if(prop2_.size())
	{
		size_t __len__ = (size_t)prop2_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			prop2_[i].serialize(__s__);
		}
	}
}
bool COM_ReportTarget::deserialize(ProtocolReader* __r__)
{
	if(!COM_ReportTargetBase::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize prop2_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		prop2_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!prop2_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ReportTarget::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_ReportTargetBase::serializeJson(ss,false);
	// serialize prop2_
	ss << "\"prop2_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)prop2_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			prop2_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Order::COM_Order():
status_((OrderStatus)(0))
,casterId_(0)
,target_(0)
,skill_(0)
,itemId_(0)
,weaponInstId_(0)
,babyId_(0)
,isSec_(0)
,uinteGroup_(0)
,uniteNum_(0)
,isNo_(false)
{}
void COM_Order::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((status_==(OrderStatus)(0))?false:true);
	__fm__.writeBit((casterId_==0)?false:true);
	__fm__.writeBit((target_==0)?false:true);
	__fm__.writeBit((skill_==0)?false:true);
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((weaponInstId_==0)?false:true);
	__fm__.writeBit((babyId_==0)?false:true);
	__fm__.writeBit((isSec_==0)?false:true);
	__fm__.writeBit((uinteGroup_==0)?false:true);
	__fm__.writeBit((uniteNum_==0)?false:true);
	__fm__.writeBit(isNo_);
	__s__->write(__fm__.masks_, 2);
	// serialize status_
	{
		EnumSize __e__ = (EnumSize)status_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize casterId_
	{
		if(casterId_ != 0){
		__s__->writeType(casterId_);
		}
	}
	// serialize target_
	{
		if(target_ != 0){
		__s__->writeType(target_);
		}
	}
	// serialize skill_
	{
		if(skill_ != 0){
		__s__->writeType(skill_);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize weaponInstId_
	{
		if(weaponInstId_ != 0){
		__s__->writeType(weaponInstId_);
		}
	}
	// serialize babyId_
	{
		if(babyId_ != 0){
		__s__->writeType(babyId_);
		}
	}
	// serialize isSec_
	{
		if(isSec_ != 0){
		__s__->writeType(isSec_);
		}
	}
	// serialize uinteGroup_
	{
		if(uinteGroup_ != 0){
		__s__->writeType(uinteGroup_);
		}
	}
	// serialize uniteNum_
	{
		if(uniteNum_ != 0){
		__s__->writeType(uniteNum_);
		}
	}
	// serialize isNo_
	{
	}
}
bool COM_Order::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize status_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 9) return false;
		status_ = (OrderStatus)__e__;
		}
	}
	// deserialize casterId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(casterId_)) return false;
		}
	}
	// deserialize target_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(target_)) return false;
		}
	}
	// deserialize skill_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skill_)) return false;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize weaponInstId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(weaponInstId_)) return false;
		}
	}
	// deserialize babyId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyId_)) return false;
		}
	}
	// deserialize isSec_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(isSec_)) return false;
		}
	}
	// deserialize uinteGroup_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(uinteGroup_)) return false;
		}
	}
	// deserialize uniteNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(uniteNum_)) return false;
		}
	}
	// deserialize isNo_
	{
		isNo_ = __fm__.readBit();
	}
		return true;
}
void COM_Order::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize status_
	ss << "\"status_\":";
	{
		ss << "\"" << ENUM(OrderStatus).getItemName(status_) << "\"";
	}
	 ss << ",\n";
	// serialize casterId_
	ss << "\"casterId_\":";
	{
		ss << (S64)casterId_;
	}
	 ss << ",\n";
	// serialize target_
	ss << "\"target_\":";
	{
		ss << (S64)target_;
	}
	 ss << ",\n";
	// serialize skill_
	ss << "\"skill_\":";
	{
		ss << (S64)skill_;
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize weaponInstId_
	ss << "\"weaponInstId_\":";
	{
		ss << (S64)weaponInstId_;
	}
	 ss << ",\n";
	// serialize babyId_
	ss << "\"babyId_\":";
	{
		ss << (S64)babyId_;
	}
	 ss << ",\n";
	// serialize isSec_
	ss << "\"isSec_\":";
	{
		ss << (S64)isSec_;
	}
	 ss << ",\n";
	// serialize uinteGroup_
	ss << "\"uinteGroup_\":";
	{
		ss << (S64)uinteGroup_;
	}
	 ss << ",\n";
	// serialize uniteNum_
	ss << "\"uniteNum_\":";
	{
		ss << (S64)uniteNum_;
	}
	 ss << ",\n";
	// serialize isNo_
	ss << "\"isNo_\":";
	{
		ss << (isNo_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ReportActionCounter::COM_ReportActionCounter():
casterId_(0)
,targetPosition_(0)
{}
void COM_ReportActionCounter::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((casterId_==0)?false:true);
	__fm__.writeBit((targetPosition_==0)?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(states_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize casterId_
	{
		if(casterId_ != 0){
		__s__->writeType(casterId_);
		}
	}
	// serialize targetPosition_
	{
		if(targetPosition_ != 0){
		__s__->writeType(targetPosition_);
		}
	}
	// serialize props_
	{
		props_.serialize(__s__);
	}
	// serialize states_
	if(states_.size())
	{
		size_t __len__ = (size_t)states_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			states_[i].serialize(__s__);
		}
	}
}
bool COM_ReportActionCounter::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize casterId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(casterId_)) return false;
		}
	}
	// deserialize targetPosition_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(targetPosition_)) return false;
		}
	}
	// deserialize props_
	{
		if(__fm__.readBit()){
		if(!props_.deserialize(__r__)) return false;
		}
	}
	// deserialize states_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		states_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!states_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ReportActionCounter::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize casterId_
	ss << "\"casterId_\":";
	{
		ss << (S64)casterId_;
	}
	 ss << ",\n";
	// serialize targetPosition_
	ss << "\"targetPosition_\":";
	{
		ss << (S64)targetPosition_;
	}
	 ss << ",\n";
	// serialize props_
	ss << "\"props_\":";
	{
		props_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize states_
	ss << "\"states_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)states_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			states_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ReportAction::COM_ReportAction():
zhuachongOk_(false)
,skillLevel_(0)
,huweiPosition_(0)
,bp0_((BattlePosition)(0))
,bp1_((BattlePosition)(0))
{}
void COM_ReportAction::serialize(ProtocolWriter* __s__) const
{
	COM_Order::serialize(__s__);
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(zhuachongOk_);
	__fm__.writeBit((skillLevel_==0)?false:true);
	__fm__.writeBit((huweiPosition_==0)?false:true);
	__fm__.writeBit((bp0_==(BattlePosition)(0))?false:true);
	__fm__.writeBit((bp1_==(BattlePosition)(0))?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(eraseEntities_.size()?true:false);
	__fm__.writeBit(targets_.size()?true:false);
	__fm__.writeBit(stateIds_.size()?true:false);
	__fm__.writeBit(counters_.size()?true:false);
	__fm__.writeBit(dynamicEntities_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize zhuachongOk_
	{
	}
	// serialize skillLevel_
	{
		if(skillLevel_ != 0){
		__s__->writeType(skillLevel_);
		}
	}
	// serialize huweiPosition_
	{
		if(huweiPosition_ != 0){
		__s__->writeType(huweiPosition_);
		}
	}
	// serialize bp0_
	{
		EnumSize __e__ = (EnumSize)bp0_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize bp1_
	{
		EnumSize __e__ = (EnumSize)bp1_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize baby_
	{
		baby_.serialize(__s__);
	}
	// serialize eraseEntities_
	if(eraseEntities_.size())
	{
		size_t __len__ = (size_t)eraseEntities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(eraseEntities_[i]);
		}
	}
	// serialize targets_
	if(targets_.size())
	{
		size_t __len__ = (size_t)targets_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serialize(__s__);
		}
	}
	// serialize stateIds_
	if(stateIds_.size())
	{
		size_t __len__ = (size_t)stateIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			stateIds_[i].serialize(__s__);
		}
	}
	// serialize counters_
	if(counters_.size())
	{
		size_t __len__ = (size_t)counters_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			counters_[i].serialize(__s__);
		}
	}
	// serialize dynamicEntities_
	if(dynamicEntities_.size())
	{
		size_t __len__ = (size_t)dynamicEntities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			dynamicEntities_[i].serialize(__s__);
		}
	}
}
bool COM_ReportAction::deserialize(ProtocolReader* __r__)
{
	if(!COM_Order::deserialize(__r__)) return false;
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize zhuachongOk_
	{
		zhuachongOk_ = __fm__.readBit();
	}
	// deserialize skillLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(skillLevel_)) return false;
		}
	}
	// deserialize huweiPosition_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(huweiPosition_)) return false;
		}
	}
	// deserialize bp0_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		bp0_ = (BattlePosition)__e__;
		}
	}
	// deserialize bp1_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		bp1_ = (BattlePosition)__e__;
		}
	}
	// deserialize baby_
	{
		if(__fm__.readBit()){
		if(!baby_.deserialize(__r__)) return false;
		}
	}
	// deserialize eraseEntities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		eraseEntities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(eraseEntities_[i])) return false;
		}
	}
	// deserialize targets_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		targets_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!targets_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize stateIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		stateIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!stateIds_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize counters_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		counters_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!counters_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize dynamicEntities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		dynamicEntities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!dynamicEntities_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ReportAction::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Order::serializeJson(ss,false);
	// serialize zhuachongOk_
	ss << "\"zhuachongOk_\":";
	{
		ss << (zhuachongOk_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize skillLevel_
	ss << "\"skillLevel_\":";
	{
		ss << (S64)skillLevel_;
	}
	 ss << ",\n";
	// serialize huweiPosition_
	ss << "\"huweiPosition_\":";
	{
		ss << (S64)huweiPosition_;
	}
	 ss << ",\n";
	// serialize bp0_
	ss << "\"bp0_\":";
	{
		ss << "\"" << ENUM(BattlePosition).getItemName(bp0_) << "\"";
	}
	 ss << ",\n";
	// serialize bp1_
	ss << "\"bp1_\":";
	{
		ss << "\"" << ENUM(BattlePosition).getItemName(bp1_) << "\"";
	}
	 ss << ",\n";
	// serialize baby_
	ss << "\"baby_\":";
	{
		baby_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize eraseEntities_
	ss << "\"eraseEntities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)eraseEntities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)eraseEntities_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize targets_
	ss << "\"targets_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)targets_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize stateIds_
	ss << "\"stateIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)stateIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			stateIds_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize counters_
	ss << "\"counters_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)counters_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			counters_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize dynamicEntities_
	ss << "\"dynamicEntities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)dynamicEntities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			dynamicEntities_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void COM_BattleReport::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(stateIds_.size()?true:false);
	__fm__.writeBit(actionResults_.size()?true:false);
	__fm__.writeBit(targets_.size()?true:false);
	__fm__.writeBit(waveEntities_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize stateIds_
	if(stateIds_.size())
	{
		size_t __len__ = (size_t)stateIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			stateIds_[i].serialize(__s__);
		}
	}
	// serialize actionResults_
	if(actionResults_.size())
	{
		size_t __len__ = (size_t)actionResults_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			actionResults_[i].serialize(__s__);
		}
	}
	// serialize targets_
	if(targets_.size())
	{
		size_t __len__ = (size_t)targets_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serialize(__s__);
		}
	}
	// serialize waveEntities_
	if(waveEntities_.size())
	{
		size_t __len__ = (size_t)waveEntities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			waveEntities_[i].serialize(__s__);
		}
	}
}
bool COM_BattleReport::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize stateIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		stateIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!stateIds_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize actionResults_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		actionResults_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!actionResults_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize targets_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		targets_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!targets_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize waveEntities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		waveEntities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!waveEntities_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_BattleReport::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize stateIds_
	ss << "\"stateIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)stateIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			stateIds_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize actionResults_
	ss << "\"actionResults_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)actionResults_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			actionResults_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize targets_
	ss << "\"targets_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)targets_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			targets_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize waveEntities_
	ss << "\"waveEntities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)waveEntities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			waveEntities_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_InitBattle::COM_InitBattle():
battleId_(0)
,bt_((BattleType)(0))
,roundCount_(0)
,opType_((OperateType)(0))
,sneakAttack_((SneakAttackType)(0))
{}
void COM_InitBattle::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((battleId_==0)?false:true);
	__fm__.writeBit((bt_==(BattleType)(0))?false:true);
	__fm__.writeBit((roundCount_==0)?false:true);
	__fm__.writeBit((opType_==(OperateType)(0))?false:true);
	__fm__.writeBit((sneakAttack_==(SneakAttackType)(0))?false:true);
	__fm__.writeBit(actors_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize battleId_
	{
		if(battleId_ != 0){
		__s__->writeType(battleId_);
		}
	}
	// serialize bt_
	{
		EnumSize __e__ = (EnumSize)bt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize roundCount_
	{
		if(roundCount_ != 0){
		__s__->writeType(roundCount_);
		}
	}
	// serialize opType_
	{
		EnumSize __e__ = (EnumSize)opType_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize sneakAttack_
	{
		EnumSize __e__ = (EnumSize)sneakAttack_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize actors_
	if(actors_.size())
	{
		size_t __len__ = (size_t)actors_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			actors_[i].serialize(__s__);
		}
	}
}
bool COM_InitBattle::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize battleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(battleId_)) return false;
		}
	}
	// deserialize bt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 10) return false;
		bt_ = (BattleType)__e__;
		}
	}
	// deserialize roundCount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roundCount_)) return false;
		}
	}
	// deserialize opType_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 4) return false;
		opType_ = (OperateType)__e__;
		}
	}
	// deserialize sneakAttack_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 4) return false;
		sneakAttack_ = (SneakAttackType)__e__;
		}
	}
	// deserialize actors_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		actors_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!actors_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_InitBattle::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize battleId_
	ss << "\"battleId_\":";
	{
		ss << (S64)battleId_;
	}
	 ss << ",\n";
	// serialize bt_
	ss << "\"bt_\":";
	{
		ss << "\"" << ENUM(BattleType).getItemName(bt_) << "\"";
	}
	 ss << ",\n";
	// serialize roundCount_
	ss << "\"roundCount_\":";
	{
		ss << (S64)roundCount_;
	}
	 ss << ",\n";
	// serialize opType_
	ss << "\"opType_\":";
	{
		ss << "\"" << ENUM(OperateType).getItemName(opType_) << "\"";
	}
	 ss << ",\n";
	// serialize sneakAttack_
	ss << "\"sneakAttack_\":";
	{
		ss << "\"" << ENUM(SneakAttackType).getItemName(sneakAttack_) << "\"";
	}
	 ss << ",\n";
	// serialize actors_
	ss << "\"actors_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)actors_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			actors_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_CreateTeamInfo::COM_CreateTeamInfo():
type_((TeamType)(0))
,maxMemberSize_(0)
,minLevel_(0)
,maxLevel_(0)
{}
void COM_CreateTeamInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((type_==(TeamType)(0))?false:true);
	__fm__.writeBit((maxMemberSize_==0)?false:true);
	__fm__.writeBit(name_.length()?true:false);
	__fm__.writeBit(pwd_.length()?true:false);
	__fm__.writeBit((minLevel_==0)?false:true);
	__fm__.writeBit((maxLevel_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize maxMemberSize_
	{
		if(maxMemberSize_ != 0){
		__s__->writeType(maxMemberSize_);
		}
	}
	// serialize name_
	{
		if(name_.length()){
		__s__->writeType(name_);
		}
	}
	// serialize pwd_
	{
		if(pwd_.length()){
		__s__->writeType(pwd_);
		}
	}
	// serialize minLevel_
	{
		if(minLevel_ != 0){
		__s__->writeType(minLevel_);
		}
	}
	// serialize maxLevel_
	{
		if(maxLevel_ != 0){
		__s__->writeType(maxLevel_);
		}
	}
}
bool COM_CreateTeamInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		type_ = (TeamType)__e__;
		}
	}
	// deserialize maxMemberSize_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(maxMemberSize_)) return false;
		}
	}
	// deserialize name_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(name_, 65535)) return false;
		}
	}
	// deserialize pwd_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pwd_, 65535)) return false;
		}
	}
	// deserialize minLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(minLevel_)) return false;
		}
	}
	// deserialize maxLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(maxLevel_)) return false;
		}
	}
		return true;
}
void COM_CreateTeamInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(TeamType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize maxMemberSize_
	ss << "\"maxMemberSize_\":";
	{
		ss << (S64)maxMemberSize_;
	}
	 ss << ",\n";
	// serialize name_
	ss << "\"name_\":";
	{
		ss << "\"" << name_ << "\"";
	}
	 ss << ",\n";
	// serialize pwd_
	ss << "\"pwd_\":";
	{
		ss << "\"" << pwd_ << "\"";
	}
	 ss << ",\n";
	// serialize minLevel_
	ss << "\"minLevel_\":";
	{
		ss << (S64)minLevel_;
	}
	 ss << ",\n";
	// serialize maxLevel_
	ss << "\"maxLevel_\":";
	{
		ss << (S64)maxLevel_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SimpleTeamInfo::COM_SimpleTeamInfo():
teamId_(0)
,curMemberSize_(0)
,job_((JobType)(0))
,joblevel_(0)
,needPassword_(false)
,isRunning_(false)
,isWelcome_(false)
{}
void COM_SimpleTeamInfo::serialize(ProtocolWriter* __s__) const
{
	COM_CreateTeamInfo::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((teamId_==0)?false:true);
	__fm__.writeBit((curMemberSize_==0)?false:true);
	__fm__.writeBit(leaderName_.length()?true:false);
	__fm__.writeBit((job_==(JobType)(0))?false:true);
	__fm__.writeBit((joblevel_==0)?false:true);
	__fm__.writeBit(needPassword_);
	__fm__.writeBit(isRunning_);
	__fm__.writeBit(isWelcome_);
	__s__->write(__fm__.masks_, 1);
	// serialize teamId_
	{
		if(teamId_ != 0){
		__s__->writeType(teamId_);
		}
	}
	// serialize curMemberSize_
	{
		if(curMemberSize_ != 0){
		__s__->writeType(curMemberSize_);
		}
	}
	// serialize leaderName_
	{
		if(leaderName_.length()){
		__s__->writeType(leaderName_);
		}
	}
	// serialize job_
	{
		EnumSize __e__ = (EnumSize)job_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize joblevel_
	{
		if(joblevel_ != 0){
		__s__->writeType(joblevel_);
		}
	}
	// serialize needPassword_
	{
	}
	// serialize isRunning_
	{
	}
	// serialize isWelcome_
	{
	}
}
bool COM_SimpleTeamInfo::deserialize(ProtocolReader* __r__)
{
	if(!COM_CreateTeamInfo::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize teamId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(teamId_)) return false;
		}
	}
	// deserialize curMemberSize_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(curMemberSize_)) return false;
		}
	}
	// deserialize leaderName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(leaderName_, 65535)) return false;
		}
	}
	// deserialize job_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		job_ = (JobType)__e__;
		}
	}
	// deserialize joblevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(joblevel_)) return false;
		}
	}
	// deserialize needPassword_
	{
		needPassword_ = __fm__.readBit();
	}
	// deserialize isRunning_
	{
		isRunning_ = __fm__.readBit();
	}
	// deserialize isWelcome_
	{
		isWelcome_ = __fm__.readBit();
	}
		return true;
}
void COM_SimpleTeamInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_CreateTeamInfo::serializeJson(ss,false);
	// serialize teamId_
	ss << "\"teamId_\":";
	{
		ss << (S64)teamId_;
	}
	 ss << ",\n";
	// serialize curMemberSize_
	ss << "\"curMemberSize_\":";
	{
		ss << (S64)curMemberSize_;
	}
	 ss << ",\n";
	// serialize leaderName_
	ss << "\"leaderName_\":";
	{
		ss << "\"" << leaderName_ << "\"";
	}
	 ss << ",\n";
	// serialize job_
	ss << "\"job_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(job_) << "\"";
	}
	 ss << ",\n";
	// serialize joblevel_
	ss << "\"joblevel_\":";
	{
		ss << (S64)joblevel_;
	}
	 ss << ",\n";
	// serialize needPassword_
	ss << "\"needPassword_\":";
	{
		ss << (needPassword_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isRunning_
	ss << "\"isRunning_\":";
	{
		ss << (isRunning_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isWelcome_
	ss << "\"isWelcome_\":";
	{
		ss << (isWelcome_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void COM_TeamInfo::serialize(ProtocolWriter* __s__) const
{
	COM_SimpleTeamInfo::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(members_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize members_
	if(members_.size())
	{
		size_t __len__ = (size_t)members_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			members_[i].serialize(__s__);
		}
	}
}
bool COM_TeamInfo::deserialize(ProtocolReader* __r__)
{
	if(!COM_SimpleTeamInfo::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize members_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		members_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!members_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_TeamInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_SimpleTeamInfo::serializeJson(ss,false);
	// serialize members_
	ss << "\"members_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)members_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			members_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_JJCBattleMsg::COM_JJCBattleMsg():
rank_(0)
,isWin_(false)
,curTime_(0)
{}
void COM_JJCBattleMsg::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(defier_.length()?true:false);
	__fm__.writeBit(bydefier_.length()?true:false);
	__fm__.writeBit((rank_==0)?false:true);
	__fm__.writeBit(isWin_);
	__fm__.writeBit((curTime_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize defier_
	{
		if(defier_.length()){
		__s__->writeType(defier_);
		}
	}
	// serialize bydefier_
	{
		if(bydefier_.length()){
		__s__->writeType(bydefier_);
		}
	}
	// serialize rank_
	{
		if(rank_ != 0){
		__s__->writeType(rank_);
		}
	}
	// serialize isWin_
	{
	}
	// serialize curTime_
	{
		if(curTime_ != 0){
		__s__->writeType(curTime_);
		}
	}
}
bool COM_JJCBattleMsg::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize defier_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(defier_, 65535)) return false;
		}
	}
	// deserialize bydefier_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(bydefier_, 65535)) return false;
		}
	}
	// deserialize rank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rank_)) return false;
		}
	}
	// deserialize isWin_
	{
		isWin_ = __fm__.readBit();
	}
	// deserialize curTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(curTime_)) return false;
		}
	}
		return true;
}
void COM_JJCBattleMsg::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize defier_
	ss << "\"defier_\":";
	{
		ss << "\"" << defier_ << "\"";
	}
	 ss << ",\n";
	// serialize bydefier_
	ss << "\"bydefier_\":";
	{
		ss << "\"" << bydefier_ << "\"";
	}
	 ss << ",\n";
	// serialize rank_
	ss << "\"rank_\":";
	{
		ss << (S64)rank_;
	}
	 ss << ",\n";
	// serialize isWin_
	ss << "\"isWin_\":";
	{
		ss << (isWin_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize curTime_
	ss << "\"curTime_\":";
	{
		ss << (S64)curTime_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_BattleEmp::COM_BattleEmp():
empBattleGroup_((EmployeesBattleGroup)(0))
{}
void COM_BattleEmp::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((empBattleGroup_==(EmployeesBattleGroup)(0))?false:true);
	__fm__.writeBit(employeeGroup1_.size()?true:false);
	__fm__.writeBit(employeeGroup2_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize empBattleGroup_
	{
		EnumSize __e__ = (EnumSize)empBattleGroup_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize employeeGroup1_
	if(employeeGroup1_.size())
	{
		size_t __len__ = (size_t)employeeGroup1_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup1_[i]);
		}
	}
	// serialize employeeGroup2_
	if(employeeGroup2_.size())
	{
		size_t __len__ = (size_t)employeeGroup2_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup2_[i]);
		}
	}
}
bool COM_BattleEmp::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize empBattleGroup_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 5) return false;
		empBattleGroup_ = (EmployeesBattleGroup)__e__;
		}
	}
	// deserialize employeeGroup1_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup1_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup1_[i])) return false;
		}
	}
	// deserialize employeeGroup2_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup2_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup2_[i])) return false;
		}
	}
		return true;
}
void COM_BattleEmp::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize empBattleGroup_
	ss << "\"empBattleGroup_\":";
	{
		ss << "\"" << ENUM(EmployeesBattleGroup).getItemName(empBattleGroup_) << "\"";
	}
	 ss << ",\n";
	// serialize employeeGroup1_
	ss << "\"employeeGroup1_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup1_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup1_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize employeeGroup2_
	ss << "\"employeeGroup2_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup2_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup2_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Gather::COM_Gather():
gatherId_(0)
,flag_((GatherStateType)(0))
,num_(0)
{}
void COM_Gather::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((gatherId_==0)?false:true);
	__fm__.writeBit((flag_==(GatherStateType)(0))?false:true);
	__fm__.writeBit((num_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize gatherId_
	{
		if(gatherId_ != 0){
		__s__->writeType(gatherId_);
		}
	}
	// serialize flag_
	{
		EnumSize __e__ = (EnumSize)flag_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize num_
	{
		if(num_ != 0){
		__s__->writeType(num_);
		}
	}
}
bool COM_Gather::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize gatherId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gatherId_)) return false;
		}
	}
	// deserialize flag_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 4) return false;
		flag_ = (GatherStateType)__e__;
		}
	}
	// deserialize num_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(num_)) return false;
		}
	}
		return true;
}
void COM_Gather::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize gatherId_
	ss << "\"gatherId_\":";
	{
		ss << (S64)gatherId_;
	}
	 ss << ",\n";
	// serialize flag_
	ss << "\"flag_\":";
	{
		ss << "\"" << ENUM(GatherStateType).getItemName(flag_) << "\"";
	}
	 ss << ",\n";
	// serialize num_
	ss << "\"num_\":";
	{
		ss << (S64)num_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_DBPlayerData::SGE_DBPlayerData():
versionNumber_(0)
,freeze_(false)
,seal_(false)
,signs_(0)
,sellIdMax_(0)
,push_(0)
,acceptRandQuestCounter_(0)
,submitRandQuestCounter_(0)
,itemStoreSize_(0)
,babyStoreSize_(0)
,loginTime_(0)
,logoutTime_(0)
,genItemMaxGuid_(0)
,gaterMaxNum_(0)
,firstRollEmployeeCon_(false)
,firstRollEmployeeDia_(false)
,empBattleGroup_((EmployeesBattleGroup)(0))
{}
void SGE_DBPlayerData::serialize(ProtocolWriter* __s__) const
{
	COM_PlayerInst::serialize(__s__);
	//field mask
	FieldMask<5> __fm__;
	__fm__.writeBit((versionNumber_==0)?false:true);
	__fm__.writeBit(freeze_);
	__fm__.writeBit(seal_);
	__fm__.writeBit((signs_==0)?false:true);
	__fm__.writeBit((sellIdMax_==0)?false:true);
	__fm__.writeBit((push_==0)?false:true);
	__fm__.writeBit((acceptRandQuestCounter_==0)?false:true);
	__fm__.writeBit((submitRandQuestCounter_==0)?false:true);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit((itemStoreSize_==0)?false:true);
	__fm__.writeBit((babyStoreSize_==0)?false:true);
	__fm__.writeBit(orders_.size()?true:false);
	__fm__.writeBit((loginTime_==0)?false:true);
	__fm__.writeBit((logoutTime_==0)?false:true);
	__fm__.writeBit((genItemMaxGuid_==0)?false:true);
	__fm__.writeBit((gaterMaxNum_==0)?false:true);
	__fm__.writeBit(firstRollEmployeeCon_);
	__fm__.writeBit(firstRollEmployeeDia_);
	__fm__.writeBit(employees_.size()?true:false);
	__fm__.writeBit(itemStorage_.size()?true:false);
	__fm__.writeBit(babyStorage_.size()?true:false);
	__fm__.writeBit(babies_.size()?true:false);
	__fm__.writeBit(bagItems_.size()?true:false);
	__fm__.writeBit(quests_.size()?true:false);
	__fm__.writeBit(completeQuests_.size()?true:false);
	__fm__.writeBit(mineReward_.size()?true:false);
	__fm__.writeBit(jjcBattleMsg_.size()?true:false);
	__fm__.writeBit(friend_.size()?true:false);
	__fm__.writeBit(blacklist_.size()?true:false);
	__fm__.writeBit(achValues_.size()?true:false);
	__fm__.writeBit(achievement_.size()?true:false);
	__fm__.writeBit((empBattleGroup_==(EmployeesBattleGroup)(0))?false:true);
	__fm__.writeBit(employeeGroup1_.size()?true:false);
	__fm__.writeBit(employeeGroup2_.size()?true:false);
	__fm__.writeBit(gatherData_.size()?true:false);
	__fm__.writeBit(compoundList_.size()?true:false);
	__s__->write(__fm__.masks_, 5);
	// serialize versionNumber_
	{
		if(versionNumber_ != 0){
		__s__->writeType(versionNumber_);
		}
	}
	// serialize freeze_
	{
	}
	// serialize seal_
	{
	}
	// serialize signs_
	{
		if(signs_ != 0){
		__s__->writeType(signs_);
		}
	}
	// serialize sellIdMax_
	{
		if(sellIdMax_ != 0){
		__s__->writeType(sellIdMax_);
		}
	}
	// serialize push_
	{
		if(push_ != 0){
		__s__->writeType(push_);
		}
	}
	// serialize acceptRandQuestCounter_
	{
		if(acceptRandQuestCounter_ != 0){
		__s__->writeType(acceptRandQuestCounter_);
		}
	}
	// serialize submitRandQuestCounter_
	{
		if(submitRandQuestCounter_ != 0){
		__s__->writeType(submitRandQuestCounter_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize itemStoreSize_
	{
		if(itemStoreSize_ != 0){
		__s__->writeType(itemStoreSize_);
		}
	}
	// serialize babyStoreSize_
	{
		if(babyStoreSize_ != 0){
		__s__->writeType(babyStoreSize_);
		}
	}
	// serialize orders_
	if(orders_.size())
	{
		size_t __len__ = (size_t)orders_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			orders_[i].serialize(__s__);
		}
	}
	// serialize loginTime_
	{
		if(loginTime_ != 0){
		__s__->writeType(loginTime_);
		}
	}
	// serialize logoutTime_
	{
		if(logoutTime_ != 0){
		__s__->writeType(logoutTime_);
		}
	}
	// serialize genItemMaxGuid_
	{
		if(genItemMaxGuid_ != 0){
		__s__->writeType(genItemMaxGuid_);
		}
	}
	// serialize gaterMaxNum_
	{
		if(gaterMaxNum_ != 0){
		__s__->writeType(gaterMaxNum_);
		}
	}
	// serialize firstRollEmployeeCon_
	{
	}
	// serialize firstRollEmployeeDia_
	{
	}
	// serialize employees_
	if(employees_.size())
	{
		size_t __len__ = (size_t)employees_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			employees_[i].serialize(__s__);
		}
	}
	// serialize itemStorage_
	if(itemStorage_.size())
	{
		size_t __len__ = (size_t)itemStorage_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			itemStorage_[i].serialize(__s__);
		}
	}
	// serialize babyStorage_
	if(babyStorage_.size())
	{
		size_t __len__ = (size_t)babyStorage_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			babyStorage_[i].serialize(__s__);
		}
	}
	// serialize babies_
	if(babies_.size())
	{
		size_t __len__ = (size_t)babies_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			babies_[i].serialize(__s__);
		}
	}
	// serialize bagItems_
	if(bagItems_.size())
	{
		size_t __len__ = (size_t)bagItems_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			bagItems_[i].serialize(__s__);
		}
	}
	// serialize quests_
	if(quests_.size())
	{
		size_t __len__ = (size_t)quests_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serialize(__s__);
		}
	}
	// serialize completeQuests_
	if(completeQuests_.size())
	{
		size_t __len__ = (size_t)completeQuests_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(completeQuests_[i]);
		}
	}
	// serialize mineReward_
	if(mineReward_.size())
	{
		size_t __len__ = (size_t)mineReward_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			mineReward_[i].serialize(__s__);
		}
	}
	// serialize jjcBattleMsg_
	if(jjcBattleMsg_.size())
	{
		size_t __len__ = (size_t)jjcBattleMsg_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			jjcBattleMsg_[i].serialize(__s__);
		}
	}
	// serialize friend_
	if(friend_.size())
	{
		size_t __len__ = (size_t)friend_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			friend_[i].serialize(__s__);
		}
	}
	// serialize blacklist_
	if(blacklist_.size())
	{
		size_t __len__ = (size_t)blacklist_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			blacklist_[i].serialize(__s__);
		}
	}
	// serialize achValues_
	if(achValues_.size())
	{
		size_t __len__ = (size_t)achValues_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(achValues_[i]);
		}
	}
	// serialize achievement_
	if(achievement_.size())
	{
		size_t __len__ = (size_t)achievement_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			achievement_[i].serialize(__s__);
		}
	}
	// serialize empBattleGroup_
	{
		EnumSize __e__ = (EnumSize)empBattleGroup_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize employeeGroup1_
	if(employeeGroup1_.size())
	{
		size_t __len__ = (size_t)employeeGroup1_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup1_[i]);
		}
	}
	// serialize employeeGroup2_
	if(employeeGroup2_.size())
	{
		size_t __len__ = (size_t)employeeGroup2_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup2_[i]);
		}
	}
	// serialize gatherData_
	if(gatherData_.size())
	{
		size_t __len__ = (size_t)gatherData_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			gatherData_[i].serialize(__s__);
		}
	}
	// serialize compoundList_
	if(compoundList_.size())
	{
		size_t __len__ = (size_t)compoundList_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(compoundList_[i]);
		}
	}
}
bool SGE_DBPlayerData::deserialize(ProtocolReader* __r__)
{
	if(!COM_PlayerInst::deserialize(__r__)) return false;
	//field mask
	FieldMask<5> __fm__;
	if(!__r__->read(__fm__.masks_, 5)) return false;
	// deserialize versionNumber_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(versionNumber_)) return false;
		}
	}
	// deserialize freeze_
	{
		freeze_ = __fm__.readBit();
	}
	// deserialize seal_
	{
		seal_ = __fm__.readBit();
	}
	// deserialize signs_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(signs_)) return false;
		}
	}
	// deserialize sellIdMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellIdMax_)) return false;
		}
	}
	// deserialize push_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(push_)) return false;
		}
	}
	// deserialize acceptRandQuestCounter_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(acceptRandQuestCounter_)) return false;
		}
	}
	// deserialize submitRandQuestCounter_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(submitRandQuestCounter_)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize itemStoreSize_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemStoreSize_)) return false;
		}
	}
	// deserialize babyStoreSize_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyStoreSize_)) return false;
		}
	}
	// deserialize orders_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		orders_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!orders_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize loginTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(loginTime_)) return false;
		}
	}
	// deserialize logoutTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(logoutTime_)) return false;
		}
	}
	// deserialize genItemMaxGuid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(genItemMaxGuid_)) return false;
		}
	}
	// deserialize gaterMaxNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gaterMaxNum_)) return false;
		}
	}
	// deserialize firstRollEmployeeCon_
	{
		firstRollEmployeeCon_ = __fm__.readBit();
	}
	// deserialize firstRollEmployeeDia_
	{
		firstRollEmployeeDia_ = __fm__.readBit();
	}
	// deserialize employees_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employees_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!employees_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize itemStorage_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStorage_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!itemStorage_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize babyStorage_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babyStorage_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!babyStorage_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize babies_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babies_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!babies_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize bagItems_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		bagItems_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!bagItems_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize quests_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		quests_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!quests_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize completeQuests_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		completeQuests_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(completeQuests_[i])) return false;
		}
	}
	// deserialize mineReward_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		mineReward_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!mineReward_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize jjcBattleMsg_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		jjcBattleMsg_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!jjcBattleMsg_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize friend_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		friend_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!friend_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize blacklist_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		blacklist_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!blacklist_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize achValues_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		achValues_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(achValues_[i])) return false;
		}
	}
	// deserialize achievement_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		achievement_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!achievement_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize empBattleGroup_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 5) return false;
		empBattleGroup_ = (EmployeesBattleGroup)__e__;
		}
	}
	// deserialize employeeGroup1_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup1_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup1_[i])) return false;
		}
	}
	// deserialize employeeGroup2_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup2_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup2_[i])) return false;
		}
	}
	// deserialize gatherData_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gatherData_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!gatherData_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize compoundList_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		compoundList_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(compoundList_[i])) return false;
		}
	}
		return true;
}
void SGE_DBPlayerData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_PlayerInst::serializeJson(ss,false);
	// serialize versionNumber_
	ss << "\"versionNumber_\":";
	{
		ss << (S64)versionNumber_;
	}
	 ss << ",\n";
	// serialize freeze_
	ss << "\"freeze_\":";
	{
		ss << (freeze_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize seal_
	ss << "\"seal_\":";
	{
		ss << (seal_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize signs_
	ss << "\"signs_\":";
	{
		ss << (S64)signs_;
	}
	 ss << ",\n";
	// serialize sellIdMax_
	ss << "\"sellIdMax_\":";
	{
		ss << (S64)sellIdMax_;
	}
	 ss << ",\n";
	// serialize push_
	ss << "\"push_\":";
	{
		ss << (S64)push_;
	}
	 ss << ",\n";
	// serialize acceptRandQuestCounter_
	ss << "\"acceptRandQuestCounter_\":";
	{
		ss << (S64)acceptRandQuestCounter_;
	}
	 ss << ",\n";
	// serialize submitRandQuestCounter_
	ss << "\"submitRandQuestCounter_\":";
	{
		ss << (S64)submitRandQuestCounter_;
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize itemStoreSize_
	ss << "\"itemStoreSize_\":";
	{
		ss << (S64)itemStoreSize_;
	}
	 ss << ",\n";
	// serialize babyStoreSize_
	ss << "\"babyStoreSize_\":";
	{
		ss << (S64)babyStoreSize_;
	}
	 ss << ",\n";
	// serialize orders_
	ss << "\"orders_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)orders_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			orders_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize loginTime_
	ss << "\"loginTime_\":";
	{
		ss << (S64)loginTime_;
	}
	 ss << ",\n";
	// serialize logoutTime_
	ss << "\"logoutTime_\":";
	{
		ss << (S64)logoutTime_;
	}
	 ss << ",\n";
	// serialize genItemMaxGuid_
	ss << "\"genItemMaxGuid_\":";
	{
		ss << (S64)genItemMaxGuid_;
	}
	 ss << ",\n";
	// serialize gaterMaxNum_
	ss << "\"gaterMaxNum_\":";
	{
		ss << (S64)gaterMaxNum_;
	}
	 ss << ",\n";
	// serialize firstRollEmployeeCon_
	ss << "\"firstRollEmployeeCon_\":";
	{
		ss << (firstRollEmployeeCon_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize firstRollEmployeeDia_
	ss << "\"firstRollEmployeeDia_\":";
	{
		ss << (firstRollEmployeeDia_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize employees_
	ss << "\"employees_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employees_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			employees_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStorage_
	ss << "\"itemStorage_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStorage_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			itemStorage_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babyStorage_
	ss << "\"babyStorage_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babyStorage_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			babyStorage_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babies_
	ss << "\"babies_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babies_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			babies_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize bagItems_
	ss << "\"bagItems_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)bagItems_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			bagItems_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize quests_
	ss << "\"quests_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)quests_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize completeQuests_
	ss << "\"completeQuests_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)completeQuests_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)completeQuests_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize mineReward_
	ss << "\"mineReward_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)mineReward_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			mineReward_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize jjcBattleMsg_
	ss << "\"jjcBattleMsg_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)jjcBattleMsg_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			jjcBattleMsg_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize friend_
	ss << "\"friend_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)friend_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			friend_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize blacklist_
	ss << "\"blacklist_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)blacklist_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			blacklist_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize achValues_
	ss << "\"achValues_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)achValues_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)achValues_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize achievement_
	ss << "\"achievement_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)achievement_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			achievement_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize empBattleGroup_
	ss << "\"empBattleGroup_\":";
	{
		ss << "\"" << ENUM(EmployeesBattleGroup).getItemName(empBattleGroup_) << "\"";
	}
	 ss << ",\n";
	// serialize employeeGroup1_
	ss << "\"employeeGroup1_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup1_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup1_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize employeeGroup2_
	ss << "\"employeeGroup2_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup2_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup2_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gatherData_
	ss << "\"gatherData_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gatherData_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			gatherData_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize compoundList_
	ss << "\"compoundList_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)compoundList_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)compoundList_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_BattleOverClearing::COM_BattleOverClearing():
isFly_(false)
,playExp_(0)
,playLevel_(0)
,playFree_(0)
,pvpJJCGrade_(0)
,money_(0)
,babyExp_(0)
,babyLevel_(0)
{}
void COM_BattleOverClearing::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(isFly_);
	__fm__.writeBit((playExp_==0)?false:true);
	__fm__.writeBit((playLevel_==0)?false:true);
	__fm__.writeBit((playFree_==0)?false:true);
	__fm__.writeBit((pvpJJCGrade_==0)?false:true);
	__fm__.writeBit((money_==0)?false:true);
	__fm__.writeBit((babyExp_==0)?false:true);
	__fm__.writeBit((babyLevel_==0)?false:true);
	__fm__.writeBit(items_.size()?true:false);
	__fm__.writeBit(skills_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize isFly_
	{
	}
	// serialize playExp_
	{
		if(playExp_ != 0){
		__s__->writeType(playExp_);
		}
	}
	// serialize playLevel_
	{
		if(playLevel_ != 0){
		__s__->writeType(playLevel_);
		}
	}
	// serialize playFree_
	{
		if(playFree_ != 0){
		__s__->writeType(playFree_);
		}
	}
	// serialize pvpJJCGrade_
	{
		if(pvpJJCGrade_ != 0){
		__s__->writeType(pvpJJCGrade_);
		}
	}
	// serialize money_
	{
		if(money_ != 0){
		__s__->writeType(money_);
		}
	}
	// serialize babyExp_
	{
		if(babyExp_ != 0){
		__s__->writeType(babyExp_);
		}
	}
	// serialize babyLevel_
	{
		if(babyLevel_ != 0){
		__s__->writeType(babyLevel_);
		}
	}
	// serialize items_
	if(items_.size())
	{
		size_t __len__ = (size_t)items_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serialize(__s__);
		}
	}
	// serialize skills_
	if(skills_.size())
	{
		size_t __len__ = (size_t)skills_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			skills_[i].serialize(__s__);
		}
	}
}
bool COM_BattleOverClearing::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize isFly_
	{
		isFly_ = __fm__.readBit();
	}
	// deserialize playExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playExp_)) return false;
		}
	}
	// deserialize playLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playLevel_)) return false;
		}
	}
	// deserialize playFree_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playFree_)) return false;
		}
	}
	// deserialize pvpJJCGrade_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pvpJJCGrade_)) return false;
		}
	}
	// deserialize money_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(money_)) return false;
		}
	}
	// deserialize babyExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyExp_)) return false;
		}
	}
	// deserialize babyLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyLevel_)) return false;
		}
	}
	// deserialize items_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		items_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!items_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize skills_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		skills_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!skills_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_BattleOverClearing::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize isFly_
	ss << "\"isFly_\":";
	{
		ss << (isFly_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize playExp_
	ss << "\"playExp_\":";
	{
		ss << (S64)playExp_;
	}
	 ss << ",\n";
	// serialize playLevel_
	ss << "\"playLevel_\":";
	{
		ss << (S64)playLevel_;
	}
	 ss << ",\n";
	// serialize playFree_
	ss << "\"playFree_\":";
	{
		ss << (S64)playFree_;
	}
	 ss << ",\n";
	// serialize pvpJJCGrade_
	ss << "\"pvpJJCGrade_\":";
	{
		ss << (S64)pvpJJCGrade_;
	}
	 ss << ",\n";
	// serialize money_
	ss << "\"money_\":";
	{
		ss << (S64)money_;
	}
	 ss << ",\n";
	// serialize babyExp_
	ss << "\"babyExp_\":";
	{
		ss << (S64)babyExp_;
	}
	 ss << ",\n";
	// serialize babyLevel_
	ss << "\"babyLevel_\":";
	{
		ss << (S64)babyLevel_;
	}
	 ss << ",\n";
	// serialize items_
	ss << "\"items_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)items_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize skills_
	ss << "\"skills_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)skills_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			skills_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Addprop::COM_Addprop():
type_((PropertyType)(0))
,uVal_(0)
{}
void COM_Addprop::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((type_==(PropertyType)(0))?false:true);
	__fm__.writeBit((uVal_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize uVal_
	{
		if(uVal_ != 0){
		__s__->writeType(uVal_);
		}
	}
}
bool COM_Addprop::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 63) return false;
		type_ = (PropertyType)__e__;
		}
	}
	// deserialize uVal_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(uVal_)) return false;
		}
	}
		return true;
}
void COM_Addprop::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(PropertyType).getItemName(type_) << "\"";
	}
	 ss << ",\n";
	// serialize uVal_
	ss << "\"uVal_\":";
	{
		ss << (S64)uVal_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ExpendItem::COM_ExpendItem():
itemInstId_(0)
,num_(0)
{}
void COM_ExpendItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((itemInstId_==0)?false:true);
	__fm__.writeBit((num_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize itemInstId_
	{
		if(itemInstId_ != 0){
		__s__->writeType(itemInstId_);
		}
	}
	// serialize num_
	{
		if(num_ != 0){
		__s__->writeType(num_);
		}
	}
}
bool COM_ExpendItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize itemInstId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemInstId_)) return false;
		}
	}
	// deserialize num_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(num_)) return false;
		}
	}
		return true;
}
void COM_ExpendItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize itemInstId_
	ss << "\"itemInstId_\":";
	{
		ss << (S64)itemInstId_;
	}
	 ss << ",\n";
	// serialize num_
	ss << "\"num_\":";
	{
		ss << (S64)num_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Chat::COM_Chat():
ck_((ChatKind)(0))
,isAudio_(false)
,audioTime_(0)
,isMe(false)
,teamId_(0)
,teamType_((TeamType)(0))
,teamMinLevel_(0)
,teamMaxLevel_(0)
{}
void COM_Chat::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((ck_==(ChatKind)(0))?false:true);
	__fm__.writeBit(isAudio_);
	__fm__.writeBit(content_.length()?true:false);
	__fm__.writeBit((audioTime_==0)?false:true);
	__fm__.writeBit(audio_.size()?true:false);
	__fm__.writeBit(isMe);
	__fm__.writeBit((teamId_==0)?false:true);
	__fm__.writeBit((teamType_==(TeamType)(0))?false:true);
	__fm__.writeBit((teamMinLevel_==0)?false:true);
	__fm__.writeBit((teamMaxLevel_==0)?false:true);
	__fm__.writeBit(teamName_.length()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize ck_
	{
		EnumSize __e__ = (EnumSize)ck_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize isAudio_
	{
	}
	// serialize content_
	{
		if(content_.length()){
		__s__->writeType(content_);
		}
	}
	// serialize audioTime_
	{
		if(audioTime_ != 0){
		__s__->writeType(audioTime_);
		}
	}
	// serialize audio_
	if(audio_.size())
	{
		size_t __len__ = (size_t)audio_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(audio_[i]);
		}
	}
	// serialize isMe
	{
	}
	// serialize teamId_
	{
		if(teamId_ != 0){
		__s__->writeType(teamId_);
		}
	}
	// serialize teamType_
	{
		EnumSize __e__ = (EnumSize)teamType_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize teamMinLevel_
	{
		if(teamMinLevel_ != 0){
		__s__->writeType(teamMinLevel_);
		}
	}
	// serialize teamMaxLevel_
	{
		if(teamMaxLevel_ != 0){
		__s__->writeType(teamMaxLevel_);
		}
	}
	// serialize teamName_
	{
		if(teamName_.length()){
		__s__->writeType(teamName_);
		}
	}
}
bool COM_Chat::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize ck_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 8) return false;
		ck_ = (ChatKind)__e__;
		}
	}
	// deserialize isAudio_
	{
		isAudio_ = __fm__.readBit();
	}
	// deserialize content_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(content_, 65535)) return false;
		}
	}
	// deserialize audioTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(audioTime_)) return false;
		}
	}
	// deserialize audio_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		audio_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(audio_[i])) return false;
		}
	}
	// deserialize isMe
	{
		isMe = __fm__.readBit();
	}
	// deserialize teamId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(teamId_)) return false;
		}
	}
	// deserialize teamType_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		teamType_ = (TeamType)__e__;
		}
	}
	// deserialize teamMinLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(teamMinLevel_)) return false;
		}
	}
	// deserialize teamMaxLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(teamMaxLevel_)) return false;
		}
	}
	// deserialize teamName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(teamName_, 65535)) return false;
		}
	}
		return true;
}
void COM_Chat::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize ck_
	ss << "\"ck_\":";
	{
		ss << "\"" << ENUM(ChatKind).getItemName(ck_) << "\"";
	}
	 ss << ",\n";
	// serialize isAudio_
	ss << "\"isAudio_\":";
	{
		ss << (isAudio_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize content_
	ss << "\"content_\":";
	{
		ss << "\"" << content_ << "\"";
	}
	 ss << ",\n";
	// serialize audioTime_
	ss << "\"audioTime_\":";
	{
		ss << (S64)audioTime_;
	}
	 ss << ",\n";
	// serialize audio_
	ss << "\"audio_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)audio_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)audio_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize isMe
	ss << "\"isMe\":";
	{
		ss << (isMe ? "true" : "false");
	}
	 ss << ",\n";
	// serialize teamId_
	ss << "\"teamId_\":";
	{
		ss << (S64)teamId_;
	}
	 ss << ",\n";
	// serialize teamType_
	ss << "\"teamType_\":";
	{
		ss << "\"" << ENUM(TeamType).getItemName(teamType_) << "\"";
	}
	 ss << ",\n";
	// serialize teamMinLevel_
	ss << "\"teamMinLevel_\":";
	{
		ss << (S64)teamMinLevel_;
	}
	 ss << ",\n";
	// serialize teamMaxLevel_
	ss << "\"teamMaxLevel_\":";
	{
		ss << (S64)teamMaxLevel_;
	}
	 ss << ",\n";
	// serialize teamName_
	ss << "\"teamName_\":";
	{
		ss << "\"" << teamName_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ChatInfo::COM_ChatInfo():
audioId_(0)
,assetId_(0)
,instId_(0)
{}
void COM_ChatInfo::serialize(ProtocolWriter* __s__) const
{
	COM_Chat::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((audioId_==0)?false:true);
	__fm__.writeBit((assetId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit((instId_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize audioId_
	{
		if(audioId_ != 0){
		__s__->writeType(audioId_);
		}
	}
	// serialize assetId_
	{
		if(assetId_ != 0){
		__s__->writeType(assetId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
}
bool COM_ChatInfo::deserialize(ProtocolReader* __r__)
{
	if(!COM_Chat::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize audioId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(audioId_)) return false;
		}
	}
	// deserialize assetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(assetId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
		return true;
}
void COM_ChatInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Chat::serializeJson(ss,false);
	// serialize audioId_
	ss << "\"audioId_\":";
	{
		ss << (S64)audioId_;
	}
	 ss << ",\n";
	// serialize assetId_
	ss << "\"assetId_\":";
	{
		ss << (S64)assetId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_EndlessStair::COM_EndlessStair():
rank_(0)
,job_((JobType)(0))
,joblevel_(0)
,assetId_(0)
,level_(0)
,rivalTime_(0)
,rivalNum_(0)
{}
void COM_EndlessStair::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((rank_==0)?false:true);
	__fm__.writeBit(name_.length()?true:false);
	__fm__.writeBit((job_==(JobType)(0))?false:true);
	__fm__.writeBit((joblevel_==0)?false:true);
	__fm__.writeBit((assetId_==0)?false:true);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((rivalTime_==0)?false:true);
	__fm__.writeBit((rivalNum_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize rank_
	{
		if(rank_ != 0){
		__s__->writeType(rank_);
		}
	}
	// serialize name_
	{
		if(name_.length()){
		__s__->writeType(name_);
		}
	}
	// serialize job_
	{
		EnumSize __e__ = (EnumSize)job_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize joblevel_
	{
		if(joblevel_ != 0){
		__s__->writeType(joblevel_);
		}
	}
	// serialize assetId_
	{
		if(assetId_ != 0){
		__s__->writeType(assetId_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize rivalTime_
	{
		if(rivalTime_ != 0){
		__s__->writeType(rivalTime_);
		}
	}
	// serialize rivalNum_
	{
		if(rivalNum_ != 0){
		__s__->writeType(rivalNum_);
		}
	}
}
bool COM_EndlessStair::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize rank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rank_)) return false;
		}
	}
	// deserialize name_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(name_, 65535)) return false;
		}
	}
	// deserialize job_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		job_ = (JobType)__e__;
		}
	}
	// deserialize joblevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(joblevel_)) return false;
		}
	}
	// deserialize assetId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(assetId_)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize rivalTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalTime_)) return false;
		}
	}
	// deserialize rivalNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalNum_)) return false;
		}
	}
		return true;
}
void COM_EndlessStair::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize rank_
	ss << "\"rank_\":";
	{
		ss << (S64)rank_;
	}
	 ss << ",\n";
	// serialize name_
	ss << "\"name_\":";
	{
		ss << "\"" << name_ << "\"";
	}
	 ss << ",\n";
	// serialize job_
	ss << "\"job_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(job_) << "\"";
	}
	 ss << ",\n";
	// serialize joblevel_
	ss << "\"joblevel_\":";
	{
		ss << (S64)joblevel_;
	}
	 ss << ",\n";
	// serialize assetId_
	ss << "\"assetId_\":";
	{
		ss << (S64)assetId_;
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize rivalTime_
	ss << "\"rivalTime_\":";
	{
		ss << (double)rivalTime_;
	}
	 ss << ",\n";
	// serialize rivalNum_
	ss << "\"rivalNum_\":";
	{
		ss << (S64)rivalNum_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_BabyRankData::COM_BabyRankData():
instId_(0)
,rank_(0)
,ff_(0)
{}
void COM_BabyRankData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((rank_==0)?false:true);
	__fm__.writeBit(name_.length()?true:false);
	__fm__.writeBit(ownerName_.length()?true:false);
	__fm__.writeBit((ff_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize rank_
	{
		if(rank_ != 0){
		__s__->writeType(rank_);
		}
	}
	// serialize name_
	{
		if(name_.length()){
		__s__->writeType(name_);
		}
	}
	// serialize ownerName_
	{
		if(ownerName_.length()){
		__s__->writeType(ownerName_);
		}
	}
	// serialize ff_
	{
		if(ff_ != 0){
		__s__->writeType(ff_);
		}
	}
}
bool COM_BabyRankData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize rank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rank_)) return false;
		}
	}
	// deserialize name_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(name_, 65535)) return false;
		}
	}
	// deserialize ownerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ownerName_, 65535)) return false;
		}
	}
	// deserialize ff_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ff_)) return false;
		}
	}
		return true;
}
void COM_BabyRankData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize rank_
	ss << "\"rank_\":";
	{
		ss << (S64)rank_;
	}
	 ss << ",\n";
	// serialize name_
	ss << "\"name_\":";
	{
		ss << "\"" << name_ << "\"";
	}
	 ss << ",\n";
	// serialize ownerName_
	ss << "\"ownerName_\":";
	{
		ss << "\"" << ownerName_ << "\"";
	}
	 ss << ",\n";
	// serialize ff_
	ss << "\"ff_\":";
	{
		ss << (S64)ff_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_EmployeeRankData::COM_EmployeeRankData():
instId_(0)
,rank_(0)
,ff_(0)
{}
void COM_EmployeeRankData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((instId_==0)?false:true);
	__fm__.writeBit((rank_==0)?false:true);
	__fm__.writeBit(name_.length()?true:false);
	__fm__.writeBit(ownerName_.length()?true:false);
	__fm__.writeBit((ff_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize instId_
	{
		if(instId_ != 0){
		__s__->writeType(instId_);
		}
	}
	// serialize rank_
	{
		if(rank_ != 0){
		__s__->writeType(rank_);
		}
	}
	// serialize name_
	{
		if(name_.length()){
		__s__->writeType(name_);
		}
	}
	// serialize ownerName_
	{
		if(ownerName_.length()){
		__s__->writeType(ownerName_);
		}
	}
	// serialize ff_
	{
		if(ff_ != 0){
		__s__->writeType(ff_);
		}
	}
}
bool COM_EmployeeRankData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize instId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(instId_)) return false;
		}
	}
	// deserialize rank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rank_)) return false;
		}
	}
	// deserialize name_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(name_, 65535)) return false;
		}
	}
	// deserialize ownerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ownerName_, 65535)) return false;
		}
	}
	// deserialize ff_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ff_)) return false;
		}
	}
		return true;
}
void COM_EmployeeRankData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize instId_
	ss << "\"instId_\":";
	{
		ss << (S64)instId_;
	}
	 ss << ",\n";
	// serialize rank_
	ss << "\"rank_\":";
	{
		ss << (S64)rank_;
	}
	 ss << ",\n";
	// serialize name_
	ss << "\"name_\":";
	{
		ss << "\"" << name_ << "\"";
	}
	 ss << ",\n";
	// serialize ownerName_
	ss << "\"ownerName_\":";
	{
		ss << "\"" << ownerName_ << "\"";
	}
	 ss << ",\n";
	// serialize ff_
	ss << "\"ff_\":";
	{
		ss << (S64)ff_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_MailItem::COM_MailItem():
itemId_(0)
,itemStack_(0)
{}
void COM_MailItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((itemStack_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize itemStack_
	{
		if(itemStack_ != 0){
		__s__->writeType(itemStack_);
		}
	}
}
bool COM_MailItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize itemStack_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemStack_)) return false;
		}
	}
		return true;
}
void COM_MailItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize itemStack_
	ss << "\"itemStack_\":";
	{
		ss << (S64)itemStack_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Mail::COM_Mail():
mailId_(0)
,mailType_((MailType)(0))
,timestamp_(0)
,money_(0)
,diamond_(0)
,isRead_(false)
{}
void COM_Mail::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((mailId_==0)?false:true);
	__fm__.writeBit((mailType_==(MailType)(0))?false:true);
	__fm__.writeBit((timestamp_==0)?false:true);
	__fm__.writeBit(sendPlayerName_.length()?true:false);
	__fm__.writeBit(recvPlayerName_.length()?true:false);
	__fm__.writeBit(title_.length()?true:false);
	__fm__.writeBit(content_.length()?true:false);
	__fm__.writeBit((money_==0)?false:true);
	__fm__.writeBit((diamond_==0)?false:true);
	__fm__.writeBit(items_.size()?true:false);
	__fm__.writeBit(isRead_);
	__s__->write(__fm__.masks_, 2);
	// serialize mailId_
	{
		if(mailId_ != 0){
		__s__->writeType(mailId_);
		}
	}
	// serialize mailType_
	{
		EnumSize __e__ = (EnumSize)mailType_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize timestamp_
	{
		if(timestamp_ != 0){
		__s__->writeType(timestamp_);
		}
	}
	// serialize sendPlayerName_
	{
		if(sendPlayerName_.length()){
		__s__->writeType(sendPlayerName_);
		}
	}
	// serialize recvPlayerName_
	{
		if(recvPlayerName_.length()){
		__s__->writeType(recvPlayerName_);
		}
	}
	// serialize title_
	{
		if(title_.length()){
		__s__->writeType(title_);
		}
	}
	// serialize content_
	{
		if(content_.length()){
		__s__->writeType(content_);
		}
	}
	// serialize money_
	{
		if(money_ != 0){
		__s__->writeType(money_);
		}
	}
	// serialize diamond_
	{
		if(diamond_ != 0){
		__s__->writeType(diamond_);
		}
	}
	// serialize items_
	if(items_.size())
	{
		size_t __len__ = (size_t)items_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serialize(__s__);
		}
	}
	// serialize isRead_
	{
	}
}
bool COM_Mail::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize mailId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mailId_)) return false;
		}
	}
	// deserialize mailType_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 2) return false;
		mailType_ = (MailType)__e__;
		}
	}
	// deserialize timestamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(timestamp_)) return false;
		}
	}
	// deserialize sendPlayerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sendPlayerName_, 65535)) return false;
		}
	}
	// deserialize recvPlayerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(recvPlayerName_, 65535)) return false;
		}
	}
	// deserialize title_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(title_, 65535)) return false;
		}
	}
	// deserialize content_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(content_, 65535)) return false;
		}
	}
	// deserialize money_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(money_)) return false;
		}
	}
	// deserialize diamond_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(diamond_)) return false;
		}
	}
	// deserialize items_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		items_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!items_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize isRead_
	{
		isRead_ = __fm__.readBit();
	}
		return true;
}
void COM_Mail::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize mailId_
	ss << "\"mailId_\":";
	{
		ss << (S64)mailId_;
	}
	 ss << ",\n";
	// serialize mailType_
	ss << "\"mailType_\":";
	{
		ss << "\"" << ENUM(MailType).getItemName(mailType_) << "\"";
	}
	 ss << ",\n";
	// serialize timestamp_
	ss << "\"timestamp_\":";
	{
		ss << (S64)timestamp_;
	}
	 ss << ",\n";
	// serialize sendPlayerName_
	ss << "\"sendPlayerName_\":";
	{
		ss << "\"" << sendPlayerName_ << "\"";
	}
	 ss << ",\n";
	// serialize recvPlayerName_
	ss << "\"recvPlayerName_\":";
	{
		ss << "\"" << recvPlayerName_ << "\"";
	}
	 ss << ",\n";
	// serialize title_
	ss << "\"title_\":";
	{
		ss << "\"" << title_ << "\"";
	}
	 ss << ",\n";
	// serialize content_
	ss << "\"content_\":";
	{
		ss << "\"" << content_ << "\"";
	}
	 ss << ",\n";
	// serialize money_
	ss << "\"money_\":";
	{
		ss << (S64)money_;
	}
	 ss << ",\n";
	// serialize diamond_
	ss << "\"diamond_\":";
	{
		ss << (S64)diamond_;
	}
	 ss << ",\n";
	// serialize items_
	ss << "\"items_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)items_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize isRead_
	ss << "\"isRead_\":";
	{
		ss << (isRead_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildBuilding::COM_GuildBuilding():
level_(0)
,struction_(0)
{}
void COM_GuildBuilding::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((struction_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize struction_
	{
		if(struction_ != 0){
		__s__->writeType(struction_);
		}
	}
}
bool COM_GuildBuilding::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize struction_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(struction_)) return false;
		}
	}
		return true;
}
void COM_GuildBuilding::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize struction_
	ss << "\"struction_\":";
	{
		ss << (S64)struction_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildRequestData::COM_GuildRequestData():
roleId_(0)
,level_(0)
,time_(0)
,prof_(0)
,profLevel_(0)
{}
void COM_GuildRequestData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((roleId_==0)?false:true);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit(roleName_.length()?true:false);
	__fm__.writeBit((time_==0)?false:true);
	__fm__.writeBit((prof_==0)?false:true);
	__fm__.writeBit((profLevel_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize roleId_
	{
		if(roleId_ != 0){
		__s__->writeType(roleId_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize roleName_
	{
		if(roleName_.length()){
		__s__->writeType(roleName_);
		}
	}
	// serialize time_
	{
		if(time_ != 0){
		__s__->writeType(time_);
		}
	}
	// serialize prof_
	{
		if(prof_ != 0){
		__s__->writeType(prof_);
		}
	}
	// serialize profLevel_
	{
		if(profLevel_ != 0){
		__s__->writeType(profLevel_);
		}
	}
}
bool COM_GuildRequestData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize roleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleId_)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize roleName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleName_, 65535)) return false;
		}
	}
	// deserialize time_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(time_)) return false;
		}
	}
	// deserialize prof_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(prof_)) return false;
		}
	}
	// deserialize profLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(profLevel_)) return false;
		}
	}
		return true;
}
void COM_GuildRequestData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize roleId_
	ss << "\"roleId_\":";
	{
		ss << (S64)roleId_;
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize roleName_
	ss << "\"roleName_\":";
	{
		ss << "\"" << roleName_ << "\"";
	}
	 ss << ",\n";
	// serialize time_
	ss << "\"time_\":";
	{
		ss << (S64)time_;
	}
	 ss << ",\n";
	// serialize prof_
	ss << "\"prof_\":";
	{
		ss << (S64)prof_;
	}
	 ss << ",\n";
	// serialize profLevel_
	ss << "\"profLevel_\":";
	{
		ss << (S64)profLevel_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildProgen::COM_GuildProgen():
mId_(0)
,lev_(0)
,exp_(0)
{}
void COM_GuildProgen::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((mId_==0)?false:true);
	__fm__.writeBit((lev_==0)?false:true);
	__fm__.writeBit((exp_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize mId_
	{
		if(mId_ != 0){
		__s__->writeType(mId_);
		}
	}
	// serialize lev_
	{
		if(lev_ != 0){
		__s__->writeType(lev_);
		}
	}
	// serialize exp_
	{
		if(exp_ != 0){
		__s__->writeType(exp_);
		}
	}
}
bool COM_GuildProgen::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize mId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mId_)) return false;
		}
	}
	// deserialize lev_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(lev_)) return false;
		}
	}
	// deserialize exp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exp_)) return false;
		}
	}
		return true;
}
void COM_GuildProgen::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize mId_
	ss << "\"mId_\":";
	{
		ss << (S64)mId_;
	}
	 ss << ",\n";
	// serialize lev_
	ss << "\"lev_\":";
	{
		ss << (S64)lev_;
	}
	 ss << ",\n";
	// serialize exp_
	ss << "\"exp_\":";
	{
		ss << (S64)exp_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Guild::COM_Guild():
guildLevel_(0)
,createTime_(0)
,guildId_(0)
,guildContribution_(0)
,fundz_(0)
,presentNum_(0)
,master_(0)
,noFundzDays_(0)
{}
void COM_Guild::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((guildLevel_==0)?false:true);
	__fm__.writeBit((createTime_==0)?false:true);
	__fm__.writeBit((guildId_==0)?false:true);
	__fm__.writeBit((guildContribution_==0)?false:true);
	__fm__.writeBit((fundz_==0)?false:true);
	__fm__.writeBit((presentNum_==0)?false:true);
	__fm__.writeBit((master_==0)?false:true);
	__fm__.writeBit(masterName_.length()?true:false);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit(notice_.length()?true:false);
	__fm__.writeBit(requestList_.size()?true:false);
	__fm__.writeBit((noFundzDays_==0)?false:true);
	__fm__.writeBit(buildings_.size()?true:false);
	__fm__.writeBit(progenitus_.size()?true:false);
	__fm__.writeBit(progenitusPositions_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize guildLevel_
	{
		if(guildLevel_ != 0){
		__s__->writeType(guildLevel_);
		}
	}
	// serialize createTime_
	{
		if(createTime_ != 0){
		__s__->writeType(createTime_);
		}
	}
	// serialize guildId_
	{
		if(guildId_ != 0){
		__s__->writeType(guildId_);
		}
	}
	// serialize guildContribution_
	{
		if(guildContribution_ != 0){
		__s__->writeType(guildContribution_);
		}
	}
	// serialize fundz_
	{
		if(fundz_ != 0){
		__s__->writeType(fundz_);
		}
	}
	// serialize presentNum_
	{
		if(presentNum_ != 0){
		__s__->writeType(presentNum_);
		}
	}
	// serialize master_
	{
		if(master_ != 0){
		__s__->writeType(master_);
		}
	}
	// serialize masterName_
	{
		if(masterName_.length()){
		__s__->writeType(masterName_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize notice_
	{
		if(notice_.length()){
		__s__->writeType(notice_);
		}
	}
	// serialize requestList_
	if(requestList_.size())
	{
		size_t __len__ = (size_t)requestList_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			requestList_[i].serialize(__s__);
		}
	}
	// serialize noFundzDays_
	{
		if(noFundzDays_ != 0){
		__s__->writeType(noFundzDays_);
		}
	}
	// serialize buildings_
	if(buildings_.size())
	{
		size_t __len__ = (size_t)buildings_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			buildings_[i].serialize(__s__);
		}
	}
	// serialize progenitus_
	if(progenitus_.size())
	{
		size_t __len__ = (size_t)progenitus_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			progenitus_[i].serialize(__s__);
		}
	}
	// serialize progenitusPositions_
	if(progenitusPositions_.size())
	{
		size_t __len__ = (size_t)progenitusPositions_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(progenitusPositions_[i]);
		}
	}
}
bool COM_Guild::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize guildLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildLevel_)) return false;
		}
	}
	// deserialize createTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createTime_)) return false;
		}
	}
	// deserialize guildId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildId_)) return false;
		}
	}
	// deserialize guildContribution_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildContribution_)) return false;
		}
	}
	// deserialize fundz_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(fundz_)) return false;
		}
	}
	// deserialize presentNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(presentNum_)) return false;
		}
	}
	// deserialize master_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(master_)) return false;
		}
	}
	// deserialize masterName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(masterName_, 65535)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize notice_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(notice_, 65535)) return false;
		}
	}
	// deserialize requestList_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		requestList_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!requestList_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize noFundzDays_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(noFundzDays_)) return false;
		}
	}
	// deserialize buildings_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		buildings_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!buildings_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize progenitus_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		progenitus_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!progenitus_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize progenitusPositions_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		progenitusPositions_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(progenitusPositions_[i])) return false;
		}
	}
		return true;
}
void COM_Guild::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize guildLevel_
	ss << "\"guildLevel_\":";
	{
		ss << (S64)guildLevel_;
	}
	 ss << ",\n";
	// serialize createTime_
	ss << "\"createTime_\":";
	{
		ss << (S64)createTime_;
	}
	 ss << ",\n";
	// serialize guildId_
	ss << "\"guildId_\":";
	{
		ss << (S64)guildId_;
	}
	 ss << ",\n";
	// serialize guildContribution_
	ss << "\"guildContribution_\":";
	{
		ss << (S64)guildContribution_;
	}
	 ss << ",\n";
	// serialize fundz_
	ss << "\"fundz_\":";
	{
		ss << (S64)fundz_;
	}
	 ss << ",\n";
	// serialize presentNum_
	ss << "\"presentNum_\":";
	{
		ss << (S64)presentNum_;
	}
	 ss << ",\n";
	// serialize master_
	ss << "\"master_\":";
	{
		ss << (S64)master_;
	}
	 ss << ",\n";
	// serialize masterName_
	ss << "\"masterName_\":";
	{
		ss << "\"" << masterName_ << "\"";
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize notice_
	ss << "\"notice_\":";
	{
		ss << "\"" << notice_ << "\"";
	}
	 ss << ",\n";
	// serialize requestList_
	ss << "\"requestList_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)requestList_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			requestList_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize noFundzDays_
	ss << "\"noFundzDays_\":";
	{
		ss << (S64)noFundzDays_;
	}
	 ss << ",\n";
	// serialize buildings_
	ss << "\"buildings_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)buildings_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			buildings_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize progenitus_
	ss << "\"progenitus_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)progenitus_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			progenitus_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize progenitusPositions_
	ss << "\"progenitusPositions_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)progenitusPositions_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)progenitusPositions_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildShopItem::COM_GuildShopItem():
shopId_(0)
,buyLimit_(0)
{}
void COM_GuildShopItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((shopId_==0)?false:true);
	__fm__.writeBit((buyLimit_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize shopId_
	{
		if(shopId_ != 0){
		__s__->writeType(shopId_);
		}
	}
	// serialize buyLimit_
	{
		if(buyLimit_ != 0){
		__s__->writeType(buyLimit_);
		}
	}
}
bool COM_GuildShopItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize shopId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(shopId_)) return false;
		}
	}
	// deserialize buyLimit_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(buyLimit_)) return false;
		}
	}
		return true;
}
void COM_GuildShopItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize shopId_
	ss << "\"shopId_\":";
	{
		ss << (S64)shopId_;
	}
	 ss << ",\n";
	// serialize buyLimit_
	ss << "\"buyLimit_\":";
	{
		ss << (S64)buyLimit_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildMember::COM_GuildMember():
level_(0)
,shopRefreshTimes_(0)
,guildId_(0)
,profType_(0)
,profLevel_(0)
,contribution_(0)
,job_(0)
,roleId_(0)
,offlineTime_(0)
,joinTime_(0)
,signflag_(false)
{}
void COM_GuildMember::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((shopRefreshTimes_==0)?false:true);
	__fm__.writeBit((guildId_==0)?false:true);
	__fm__.writeBit((profType_==0)?false:true);
	__fm__.writeBit((profLevel_==0)?false:true);
	__fm__.writeBit((contribution_==0)?false:true);
	__fm__.writeBit((job_==0)?false:true);
	__fm__.writeBit((roleId_==0)?false:true);
	__fm__.writeBit((offlineTime_==0)?false:true);
	__fm__.writeBit(roleName_.length()?true:false);
	__fm__.writeBit((joinTime_==0)?false:true);
	__fm__.writeBit(signflag_);
	__fm__.writeBit(shopItems_.size()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize shopRefreshTimes_
	{
		if(shopRefreshTimes_ != 0){
		__s__->writeType(shopRefreshTimes_);
		}
	}
	// serialize guildId_
	{
		if(guildId_ != 0){
		__s__->writeType(guildId_);
		}
	}
	// serialize profType_
	{
		if(profType_ != 0){
		__s__->writeType(profType_);
		}
	}
	// serialize profLevel_
	{
		if(profLevel_ != 0){
		__s__->writeType(profLevel_);
		}
	}
	// serialize contribution_
	{
		if(contribution_ != 0){
		__s__->writeType(contribution_);
		}
	}
	// serialize job_
	{
		if(job_ != 0){
		__s__->writeType(job_);
		}
	}
	// serialize roleId_
	{
		if(roleId_ != 0){
		__s__->writeType(roleId_);
		}
	}
	// serialize offlineTime_
	{
		if(offlineTime_ != 0){
		__s__->writeType(offlineTime_);
		}
	}
	// serialize roleName_
	{
		if(roleName_.length()){
		__s__->writeType(roleName_);
		}
	}
	// serialize joinTime_
	{
		if(joinTime_ != 0){
		__s__->writeType(joinTime_);
		}
	}
	// serialize signflag_
	{
	}
	// serialize shopItems_
	if(shopItems_.size())
	{
		size_t __len__ = (size_t)shopItems_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			shopItems_[i].serialize(__s__);
		}
	}
}
bool COM_GuildMember::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize shopRefreshTimes_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(shopRefreshTimes_)) return false;
		}
	}
	// deserialize guildId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildId_)) return false;
		}
	}
	// deserialize profType_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(profType_)) return false;
		}
	}
	// deserialize profLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(profLevel_)) return false;
		}
	}
	// deserialize contribution_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(contribution_)) return false;
		}
	}
	// deserialize job_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(job_)) return false;
		}
	}
	// deserialize roleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleId_)) return false;
		}
	}
	// deserialize offlineTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(offlineTime_)) return false;
		}
	}
	// deserialize roleName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleName_, 65535)) return false;
		}
	}
	// deserialize joinTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(joinTime_)) return false;
		}
	}
	// deserialize signflag_
	{
		signflag_ = __fm__.readBit();
	}
	// deserialize shopItems_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		shopItems_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!shopItems_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_GuildMember::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize shopRefreshTimes_
	ss << "\"shopRefreshTimes_\":";
	{
		ss << (S64)shopRefreshTimes_;
	}
	 ss << ",\n";
	// serialize guildId_
	ss << "\"guildId_\":";
	{
		ss << (S64)guildId_;
	}
	 ss << ",\n";
	// serialize profType_
	ss << "\"profType_\":";
	{
		ss << (S64)profType_;
	}
	 ss << ",\n";
	// serialize profLevel_
	ss << "\"profLevel_\":";
	{
		ss << (S64)profLevel_;
	}
	 ss << ",\n";
	// serialize contribution_
	ss << "\"contribution_\":";
	{
		ss << (S64)contribution_;
	}
	 ss << ",\n";
	// serialize job_
	ss << "\"job_\":";
	{
		ss << (S64)job_;
	}
	 ss << ",\n";
	// serialize roleId_
	ss << "\"roleId_\":";
	{
		ss << (S64)roleId_;
	}
	 ss << ",\n";
	// serialize offlineTime_
	ss << "\"offlineTime_\":";
	{
		ss << (S64)offlineTime_;
	}
	 ss << ",\n";
	// serialize roleName_
	ss << "\"roleName_\":";
	{
		ss << "\"" << roleName_ << "\"";
	}
	 ss << ",\n";
	// serialize joinTime_
	ss << "\"joinTime_\":";
	{
		ss << (S64)joinTime_;
	}
	 ss << ",\n";
	// serialize signflag_
	ss << "\"signflag_\":";
	{
		ss << (signflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize shopItems_
	ss << "\"shopItems_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)shopItems_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			shopItems_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_GuildViewerData::COM_GuildViewerData():
guid_(0)
,level_(0)
,memberNum_(0)
,memberTotal_(0)
,guildRank_(0)
{}
void COM_GuildViewerData::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((guid_==0)?false:true);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(notice_.length()?true:false);
	__fm__.writeBit((level_==0)?false:true);
	__fm__.writeBit((memberNum_==0)?false:true);
	__fm__.writeBit((memberTotal_==0)?false:true);
	__fm__.writeBit((guildRank_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize guid_
	{
		if(guid_ != 0){
		__s__->writeType(guid_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize notice_
	{
		if(notice_.length()){
		__s__->writeType(notice_);
		}
	}
	// serialize level_
	{
		if(level_ != 0){
		__s__->writeType(level_);
		}
	}
	// serialize memberNum_
	{
		if(memberNum_ != 0){
		__s__->writeType(memberNum_);
		}
	}
	// serialize memberTotal_
	{
		if(memberTotal_ != 0){
		__s__->writeType(memberTotal_);
		}
	}
	// serialize guildRank_
	{
		if(guildRank_ != 0){
		__s__->writeType(guildRank_);
		}
	}
}
bool COM_GuildViewerData::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize guid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guid_)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize notice_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(notice_, 65535)) return false;
		}
	}
	// deserialize level_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(level_)) return false;
		}
	}
	// deserialize memberNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(memberNum_)) return false;
		}
	}
	// deserialize memberTotal_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(memberTotal_)) return false;
		}
	}
	// deserialize guildRank_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildRank_)) return false;
		}
	}
		return true;
}
void COM_GuildViewerData::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize guid_
	ss << "\"guid_\":";
	{
		ss << (S64)guid_;
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize notice_
	ss << "\"notice_\":";
	{
		ss << "\"" << notice_ << "\"";
	}
	 ss << ",\n";
	// serialize level_
	ss << "\"level_\":";
	{
		ss << (S64)level_;
	}
	 ss << ",\n";
	// serialize memberNum_
	ss << "\"memberNum_\":";
	{
		ss << (S64)memberNum_;
	}
	 ss << ",\n";
	// serialize memberTotal_
	ss << "\"memberTotal_\":";
	{
		ss << (S64)memberTotal_;
	}
	 ss << ",\n";
	// serialize guildRank_
	ss << "\"guildRank_\":";
	{
		ss << (S64)guildRank_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SearchContext::COM_SearchContext():
ist_((ItemSubType)(0))
,rt_((RaceType)(0))
,itemId_(0)
,babyId_(0)
,begin_(0)
,limit_(0)
{}
void COM_SearchContext::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(title_.length()?true:false);
	__fm__.writeBit((ist_==(ItemSubType)(0))?false:true);
	__fm__.writeBit((rt_==(RaceType)(0))?false:true);
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((babyId_==0)?false:true);
	__fm__.writeBit((begin_==0)?false:true);
	__fm__.writeBit((limit_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize title_
	{
		if(title_.length()){
		__s__->writeType(title_);
		}
	}
	// serialize ist_
	{
		EnumSize __e__ = (EnumSize)ist_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize rt_
	{
		EnumSize __e__ = (EnumSize)rt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize babyId_
	{
		if(babyId_ != 0){
		__s__->writeType(babyId_);
		}
	}
	// serialize begin_
	{
		if(begin_ != 0){
		__s__->writeType(begin_);
		}
	}
	// serialize limit_
	{
		if(limit_ != 0){
		__s__->writeType(limit_);
		}
	}
}
bool COM_SearchContext::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize title_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(title_, 65535)) return false;
		}
	}
	// deserialize ist_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 48) return false;
		ist_ = (ItemSubType)__e__;
		}
	}
	// deserialize rt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 11) return false;
		rt_ = (RaceType)__e__;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize babyId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyId_)) return false;
		}
	}
	// deserialize begin_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(begin_)) return false;
		}
	}
	// deserialize limit_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(limit_)) return false;
		}
	}
		return true;
}
void COM_SearchContext::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize title_
	ss << "\"title_\":";
	{
		ss << "\"" << title_ << "\"";
	}
	 ss << ",\n";
	// serialize ist_
	ss << "\"ist_\":";
	{
		ss << "\"" << ENUM(ItemSubType).getItemName(ist_) << "\"";
	}
	 ss << ",\n";
	// serialize rt_
	ss << "\"rt_\":";
	{
		ss << "\"" << ENUM(RaceType).getItemName(rt_) << "\"";
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize babyId_
	ss << "\"babyId_\":";
	{
		ss << (S64)babyId_;
	}
	 ss << ",\n";
	// serialize begin_
	ss << "\"begin_\":";
	{
		ss << (S64)begin_;
	}
	 ss << ",\n";
	// serialize limit_
	ss << "\"limit_\":";
	{
		ss << (S64)limit_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SellItem::COM_SellItem():
guid_(0)
,sellPlayerId_(0)
,sellPrice(0)
,ist_((ItemSubType)(0))
,rt_((RaceType)(0))
,time_(0)
{}
void COM_SellItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((guid_==0)?false:true);
	__fm__.writeBit((sellPlayerId_==0)?false:true);
	__fm__.writeBit((sellPrice==0)?false:true);
	__fm__.writeBit(title_.length()?true:false);
	__fm__.writeBit((ist_==(ItemSubType)(0))?false:true);
	__fm__.writeBit((rt_==(RaceType)(0))?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit((time_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize guid_
	{
		if(guid_ != 0){
		__s__->writeType(guid_);
		}
	}
	// serialize sellPlayerId_
	{
		if(sellPlayerId_ != 0){
		__s__->writeType(sellPlayerId_);
		}
	}
	// serialize sellPrice
	{
		if(sellPrice != 0){
		__s__->writeType(sellPrice);
		}
	}
	// serialize title_
	{
		if(title_.length()){
		__s__->writeType(title_);
		}
	}
	// serialize ist_
	{
		EnumSize __e__ = (EnumSize)ist_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize rt_
	{
		EnumSize __e__ = (EnumSize)rt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize item_
	{
		item_.serialize(__s__);
	}
	// serialize baby_
	{
		baby_.serialize(__s__);
	}
	// serialize time_
	{
		if(time_ != 0){
		__s__->writeType(time_);
		}
	}
}
bool COM_SellItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize guid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guid_)) return false;
		}
	}
	// deserialize sellPlayerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellPlayerId_)) return false;
		}
	}
	// deserialize sellPrice
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellPrice)) return false;
		}
	}
	// deserialize title_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(title_, 65535)) return false;
		}
	}
	// deserialize ist_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 48) return false;
		ist_ = (ItemSubType)__e__;
		}
	}
	// deserialize rt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 11) return false;
		rt_ = (RaceType)__e__;
		}
	}
	// deserialize item_
	{
		if(__fm__.readBit()){
		if(!item_.deserialize(__r__)) return false;
		}
	}
	// deserialize baby_
	{
		if(__fm__.readBit()){
		if(!baby_.deserialize(__r__)) return false;
		}
	}
	// deserialize time_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(time_)) return false;
		}
	}
		return true;
}
void COM_SellItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize guid_
	ss << "\"guid_\":";
	{
		ss << (S64)guid_;
	}
	 ss << ",\n";
	// serialize sellPlayerId_
	ss << "\"sellPlayerId_\":";
	{
		ss << (S64)sellPlayerId_;
	}
	 ss << ",\n";
	// serialize sellPrice
	ss << "\"sellPrice\":";
	{
		ss << (S64)sellPrice;
	}
	 ss << ",\n";
	// serialize title_
	ss << "\"title_\":";
	{
		ss << "\"" << title_ << "\"";
	}
	 ss << ",\n";
	// serialize ist_
	ss << "\"ist_\":";
	{
		ss << "\"" << ENUM(ItemSubType).getItemName(ist_) << "\"";
	}
	 ss << ",\n";
	// serialize rt_
	ss << "\"rt_\":";
	{
		ss << "\"" << ENUM(RaceType).getItemName(rt_) << "\"";
	}
	 ss << ",\n";
	// serialize item_
	ss << "\"item_\":";
	{
		item_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize baby_
	ss << "\"baby_\":";
	{
		baby_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize time_
	ss << "\"time_\":";
	{
		ss << (S64)time_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SelledItem::COM_SelledItem():
guid_(0)
,sellPlayerId_(0)
,sellTime_(0)
,selledTime_(0)
,itemId_(0)
,babyId_(0)
,itemStack_(0)
,price_(0)
,tax_(0)
{}
void COM_SelledItem::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((guid_==0)?false:true);
	__fm__.writeBit((sellPlayerId_==0)?false:true);
	__fm__.writeBit((sellTime_==0)?false:true);
	__fm__.writeBit((selledTime_==0)?false:true);
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((babyId_==0)?false:true);
	__fm__.writeBit((itemStack_==0)?false:true);
	__fm__.writeBit((price_==0)?false:true);
	__fm__.writeBit((tax_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize guid_
	{
		if(guid_ != 0){
		__s__->writeType(guid_);
		}
	}
	// serialize sellPlayerId_
	{
		if(sellPlayerId_ != 0){
		__s__->writeType(sellPlayerId_);
		}
	}
	// serialize sellTime_
	{
		if(sellTime_ != 0){
		__s__->writeType(sellTime_);
		}
	}
	// serialize selledTime_
	{
		if(selledTime_ != 0){
		__s__->writeType(selledTime_);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize babyId_
	{
		if(babyId_ != 0){
		__s__->writeType(babyId_);
		}
	}
	// serialize itemStack_
	{
		if(itemStack_ != 0){
		__s__->writeType(itemStack_);
		}
	}
	// serialize price_
	{
		if(price_ != 0){
		__s__->writeType(price_);
		}
	}
	// serialize tax_
	{
		if(tax_ != 0){
		__s__->writeType(tax_);
		}
	}
}
bool COM_SelledItem::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize guid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guid_)) return false;
		}
	}
	// deserialize sellPlayerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellPlayerId_)) return false;
		}
	}
	// deserialize sellTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellTime_)) return false;
		}
	}
	// deserialize selledTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(selledTime_)) return false;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize babyId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyId_)) return false;
		}
	}
	// deserialize itemStack_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemStack_)) return false;
		}
	}
	// deserialize price_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(price_)) return false;
		}
	}
	// deserialize tax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(tax_)) return false;
		}
	}
		return true;
}
void COM_SelledItem::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize guid_
	ss << "\"guid_\":";
	{
		ss << (S64)guid_;
	}
	 ss << ",\n";
	// serialize sellPlayerId_
	ss << "\"sellPlayerId_\":";
	{
		ss << (S64)sellPlayerId_;
	}
	 ss << ",\n";
	// serialize sellTime_
	ss << "\"sellTime_\":";
	{
		ss << (S64)sellTime_;
	}
	 ss << ",\n";
	// serialize selledTime_
	ss << "\"selledTime_\":";
	{
		ss << (S64)selledTime_;
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize babyId_
	ss << "\"babyId_\":";
	{
		ss << (S64)babyId_;
	}
	 ss << ",\n";
	// serialize itemStack_
	ss << "\"itemStack_\":";
	{
		ss << (S64)itemStack_;
	}
	 ss << ",\n";
	// serialize price_
	ss << "\"price_\":";
	{
		ss << (S64)price_;
	}
	 ss << ",\n";
	// serialize tax_
	ss << "\"tax_\":";
	{
		ss << (S64)tax_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_BuyContent::SGE_BuyContent():
playerId_(0)
,guid_(0)
,diamond_(0)
,magic_(0)
,isBabyFull_(false)
,isBagFull_(false)
{}
void SGE_BuyContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerId_==0)?false:true);
	__fm__.writeBit((guid_==0)?false:true);
	__fm__.writeBit((diamond_==0)?false:true);
	__fm__.writeBit((magic_==0)?false:true);
	__fm__.writeBit(isBabyFull_);
	__fm__.writeBit(isBagFull_);
	__s__->write(__fm__.masks_, 1);
	// serialize playerId_
	{
		if(playerId_ != 0){
		__s__->writeType(playerId_);
		}
	}
	// serialize guid_
	{
		if(guid_ != 0){
		__s__->writeType(guid_);
		}
	}
	// serialize diamond_
	{
		if(diamond_ != 0){
		__s__->writeType(diamond_);
		}
	}
	// serialize magic_
	{
		if(magic_ != 0){
		__s__->writeType(magic_);
		}
	}
	// serialize isBabyFull_
	{
	}
	// serialize isBagFull_
	{
	}
}
bool SGE_BuyContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerId_)) return false;
		}
	}
	// deserialize guid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guid_)) return false;
		}
	}
	// deserialize diamond_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(diamond_)) return false;
		}
	}
	// deserialize magic_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magic_)) return false;
		}
	}
	// deserialize isBabyFull_
	{
		isBabyFull_ = __fm__.readBit();
	}
	// deserialize isBagFull_
	{
		isBagFull_ = __fm__.readBit();
	}
		return true;
}
void SGE_BuyContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerId_
	ss << "\"playerId_\":";
	{
		ss << (S64)playerId_;
	}
	 ss << ",\n";
	// serialize guid_
	ss << "\"guid_\":";
	{
		ss << (S64)guid_;
	}
	 ss << ",\n";
	// serialize diamond_
	ss << "\"diamond_\":";
	{
		ss << (S64)diamond_;
	}
	 ss << ",\n";
	// serialize magic_
	ss << "\"magic_\":";
	{
		ss << (S64)magic_;
	}
	 ss << ",\n";
	// serialize isBabyFull_
	ss << "\"isBabyFull_\":";
	{
		ss << (isBabyFull_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBagFull_
	ss << "\"isBagFull_\":";
	{
		ss << (isBagFull_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_ScenePlayerInfo::SGE_ScenePlayerInfo():
playerId_(0)
,playerLevel_(0)
,sceneId_(0)
,entryId_(0)
{}
void SGE_ScenePlayerInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerId_==0)?false:true);
	__fm__.writeBit((playerLevel_==0)?false:true);
	__fm__.writeBit((sceneId_==0)?false:true);
	__fm__.writeBit((entryId_==0)?false:true);
	__fm__.writeBit(currentQuestIds_.size()?true:false);
	__fm__.writeBit(accecptQuestIds_.size()?true:false);
	__fm__.writeBit(openScenes_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize playerId_
	{
		if(playerId_ != 0){
		__s__->writeType(playerId_);
		}
	}
	// serialize playerLevel_
	{
		if(playerLevel_ != 0){
		__s__->writeType(playerLevel_);
		}
	}
	// serialize sceneId_
	{
		if(sceneId_ != 0){
		__s__->writeType(sceneId_);
		}
	}
	// serialize entryId_
	{
		if(entryId_ != 0){
		__s__->writeType(entryId_);
		}
	}
	// serialize currentQuestIds_
	if(currentQuestIds_.size())
	{
		size_t __len__ = (size_t)currentQuestIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(currentQuestIds_[i]);
		}
	}
	// serialize accecptQuestIds_
	if(accecptQuestIds_.size())
	{
		size_t __len__ = (size_t)accecptQuestIds_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(accecptQuestIds_[i]);
		}
	}
	// serialize openScenes_
	if(openScenes_.size())
	{
		size_t __len__ = (size_t)openScenes_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(openScenes_[i]);
		}
	}
}
bool SGE_ScenePlayerInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerId_)) return false;
		}
	}
	// deserialize playerLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerLevel_)) return false;
		}
	}
	// deserialize sceneId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sceneId_)) return false;
		}
	}
	// deserialize entryId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(entryId_)) return false;
		}
	}
	// deserialize currentQuestIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		currentQuestIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(currentQuestIds_[i])) return false;
		}
	}
	// deserialize accecptQuestIds_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		accecptQuestIds_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(accecptQuestIds_[i])) return false;
		}
	}
	// deserialize openScenes_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		openScenes_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(openScenes_[i])) return false;
		}
	}
		return true;
}
void SGE_ScenePlayerInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerId_
	ss << "\"playerId_\":";
	{
		ss << (S64)playerId_;
	}
	 ss << ",\n";
	// serialize playerLevel_
	ss << "\"playerLevel_\":";
	{
		ss << (S64)playerLevel_;
	}
	 ss << ",\n";
	// serialize sceneId_
	ss << "\"sceneId_\":";
	{
		ss << (S64)sceneId_;
	}
	 ss << ",\n";
	// serialize entryId_
	ss << "\"entryId_\":";
	{
		ss << (S64)entryId_;
	}
	 ss << ",\n";
	// serialize currentQuestIds_
	ss << "\"currentQuestIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)currentQuestIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)currentQuestIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize accecptQuestIds_
	ss << "\"accecptQuestIds_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)accecptQuestIds_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)accecptQuestIds_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize openScenes_
	ss << "\"openScenes_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)openScenes_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)openScenes_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_SceneInfo::COM_SceneInfo():
sceneId_(0)
{}
void COM_SceneInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((sceneId_==0)?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(players_.size()?true:false);
	__fm__.writeBit(npcs_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize sceneId_
	{
		if(sceneId_ != 0){
		__s__->writeType(sceneId_);
		}
	}
	// serialize position_
	{
		position_.serialize(__s__);
	}
	// serialize players_
	if(players_.size())
	{
		size_t __len__ = (size_t)players_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			players_[i].serialize(__s__);
		}
	}
	// serialize npcs_
	if(npcs_.size())
	{
		size_t __len__ = (size_t)npcs_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(npcs_[i]);
		}
	}
}
bool COM_SceneInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize sceneId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sceneId_)) return false;
		}
	}
	// deserialize position_
	{
		if(__fm__.readBit()){
		if(!position_.deserialize(__r__)) return false;
		}
	}
	// deserialize players_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		players_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!players_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize npcs_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		npcs_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(npcs_[i])) return false;
		}
	}
		return true;
}
void COM_SceneInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize sceneId_
	ss << "\"sceneId_\":";
	{
		ss << (S64)sceneId_;
	}
	 ss << ",\n";
	// serialize position_
	ss << "\"position_\":";
	{
		position_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize players_
	ss << "\"players_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)players_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			players_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize npcs_
	ss << "\"npcs_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)npcs_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)npcs_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ReconnectInfo::COM_ReconnectInfo():
reconnectProcess_((ReconnectType)(0))
{}
void COM_ReconnectInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((reconnectProcess_==(ReconnectType)(0))?false:true);
	__fm__.writeBit(sessionKey_.length()?true:false);
	__fm__.writeBit(roles_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__s__->write(__fm__.masks_, 1);
	// serialize reconnectProcess_
	{
		EnumSize __e__ = (EnumSize)reconnectProcess_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize sessionKey_
	{
		if(sessionKey_.length()){
		__s__->writeType(sessionKey_);
		}
	}
	// serialize roles_
	if(roles_.size())
	{
		size_t __len__ = (size_t)roles_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			roles_[i].serialize(__s__);
		}
	}
	// serialize playerInst_
	{
		playerInst_.serialize(__s__);
	}
	// serialize sceneInfo_
	{
		sceneInfo_.serialize(__s__);
	}
	// serialize team_
	{
		team_.serialize(__s__);
	}
	// serialize initBattle_
	{
		initBattle_.serialize(__s__);
	}
}
bool COM_ReconnectInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize reconnectProcess_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 7) return false;
		reconnectProcess_ = (ReconnectType)__e__;
		}
	}
	// deserialize sessionKey_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sessionKey_, 65535)) return false;
		}
	}
	// deserialize roles_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		roles_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!roles_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize playerInst_
	{
		if(__fm__.readBit()){
		if(!playerInst_.deserialize(__r__)) return false;
		}
	}
	// deserialize sceneInfo_
	{
		if(__fm__.readBit()){
		if(!sceneInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize team_
	{
		if(__fm__.readBit()){
		if(!team_.deserialize(__r__)) return false;
		}
	}
	// deserialize initBattle_
	{
		if(__fm__.readBit()){
		if(!initBattle_.deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ReconnectInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize reconnectProcess_
	ss << "\"reconnectProcess_\":";
	{
		ss << "\"" << ENUM(ReconnectType).getItemName(reconnectProcess_) << "\"";
	}
	 ss << ",\n";
	// serialize sessionKey_
	ss << "\"sessionKey_\":";
	{
		ss << "\"" << sessionKey_ << "\"";
	}
	 ss << ",\n";
	// serialize roles_
	ss << "\"roles_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)roles_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			roles_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize playerInst_
	ss << "\"playerInst_\":";
	{
		playerInst_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sceneInfo_
	ss << "\"sceneInfo_\":";
	{
		sceneInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize team_
	ss << "\"team_\":";
	{
		team_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize initBattle_
	ss << "\"initBattle_\":";
	{
		initBattle_.serializeJson(ss);
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Notice::COM_Notice():
SendType_((NoticeSendType)(0))
,accumulate_(0)
,startTime_(0)
,stopTime_(0)
,interval_(0)
,validate_(false)
{}
void COM_Notice::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((SendType_==(NoticeSendType)(0))?false:true);
	__fm__.writeBit(note_.length()?true:false);
	__fm__.writeBit((accumulate_==0)?false:true);
	__fm__.writeBit((startTime_==0)?false:true);
	__fm__.writeBit((stopTime_==0)?false:true);
	__fm__.writeBit((interval_==0)?false:true);
	__fm__.writeBit(validate_);
	__s__->write(__fm__.masks_, 1);
	// serialize SendType_
	{
		EnumSize __e__ = (EnumSize)SendType_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize note_
	{
		if(note_.length()){
		__s__->writeType(note_);
		}
	}
	// serialize accumulate_
	{
		if(accumulate_ != 0){
		__s__->writeType(accumulate_);
		}
	}
	// serialize startTime_
	{
		if(startTime_ != 0){
		__s__->writeType(startTime_);
		}
	}
	// serialize stopTime_
	{
		if(stopTime_ != 0){
		__s__->writeType(stopTime_);
		}
	}
	// serialize interval_
	{
		if(interval_ != 0){
		__s__->writeType(interval_);
		}
	}
	// serialize validate_
	{
	}
}
bool COM_Notice::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize SendType_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 3) return false;
		SendType_ = (NoticeSendType)__e__;
		}
	}
	// deserialize note_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(note_, 65535)) return false;
		}
	}
	// deserialize accumulate_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accumulate_)) return false;
		}
	}
	// deserialize startTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(startTime_)) return false;
		}
	}
	// deserialize stopTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(stopTime_)) return false;
		}
	}
	// deserialize interval_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(interval_)) return false;
		}
	}
	// deserialize validate_
	{
		validate_ = __fm__.readBit();
	}
		return true;
}
void COM_Notice::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize SendType_
	ss << "\"SendType_\":";
	{
		ss << "\"" << ENUM(NoticeSendType).getItemName(SendType_) << "\"";
	}
	 ss << ",\n";
	// serialize note_
	ss << "\"note_\":";
	{
		ss << "\"" << note_ << "\"";
	}
	 ss << ",\n";
	// serialize accumulate_
	ss << "\"accumulate_\":";
	{
		ss << (double)accumulate_;
	}
	 ss << ",\n";
	// serialize startTime_
	ss << "\"startTime_\":";
	{
		ss << (double)startTime_;
	}
	 ss << ",\n";
	// serialize stopTime_
	ss << "\"stopTime_\":";
	{
		ss << (double)stopTime_;
	}
	 ss << ",\n";
	// serialize interval_
	ss << "\"interval_\":";
	{
		ss << (double)interval_;
	}
	 ss << ",\n";
	// serialize validate_
	ss << "\"validate_\":";
	{
		ss << (validate_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ShowItemInst::COM_ShowItemInst():
showId_(0)
{}
void COM_ShowItemInst::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((showId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(true);
	__s__->write(__fm__.masks_, 1);
	// serialize showId_
	{
		if(showId_ != 0){
		__s__->writeType(showId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize itemInst_
	{
		itemInst_.serialize(__s__);
	}
}
bool COM_ShowItemInst::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize showId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize itemInst_
	{
		if(__fm__.readBit()){
		if(!itemInst_.deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ShowItemInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize showId_
	ss << "\"showId_\":";
	{
		ss << (S64)showId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize itemInst_
	ss << "\"itemInst_\":";
	{
		itemInst_.serializeJson(ss);
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ShowItemInstInfo::COM_ShowItemInstInfo():
showId_(0)
,itemId_(0)
{}
void COM_ShowItemInstInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((showId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit((itemId_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize showId_
	{
		if(showId_ != 0){
		__s__->writeType(showId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
}
bool COM_ShowItemInstInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize showId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
		return true;
}
void COM_ShowItemInstInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize showId_
	ss << "\"showId_\":";
	{
		ss << (S64)showId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ShowbabyInst::COM_ShowbabyInst():
showId_(0)
{}
void COM_ShowbabyInst::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((showId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(true);
	__s__->write(__fm__.masks_, 1);
	// serialize showId_
	{
		if(showId_ != 0){
		__s__->writeType(showId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize babyInst_
	{
		babyInst_.serialize(__s__);
	}
}
bool COM_ShowbabyInst::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize showId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize babyInst_
	{
		if(__fm__.readBit()){
		if(!babyInst_.deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_ShowbabyInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize showId_
	ss << "\"showId_\":";
	{
		ss << (S64)showId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize babyInst_
	ss << "\"babyInst_\":";
	{
		babyInst_.serializeJson(ss);
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_ShowbabyInstInfo::COM_ShowbabyInstInfo():
showId_(0)
,babyId_(0)
{}
void COM_ShowbabyInstInfo::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((showId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit((babyId_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize showId_
	{
		if(showId_ != 0){
		__s__->writeType(showId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize babyId_
	{
		if(babyId_ != 0){
		__s__->writeType(babyId_);
		}
	}
}
bool COM_ShowbabyInstInfo::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize showId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(showId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize babyId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(babyId_)) return false;
		}
	}
		return true;
}
void COM_ShowbabyInstInfo::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize showId_
	ss << "\"showId_\":";
	{
		ss << (S64)showId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize babyId_
	ss << "\"babyId_\":";
	{
		ss << (S64)babyId_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_Account::SGE_Account():
createtime_(0)
{}
void SGE_Account::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(gamename_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(pfname_.length()?true:false);
	__fm__.writeBit(accountid_.length()?true:false);
	__fm__.writeBit((createtime_==0)?false:true);
	__fm__.writeBit(mac_.length()?true:false);
	__fm__.writeBit(idfa_.length()?true:false);
	__fm__.writeBit(ip_.length()?true:false);
	__fm__.writeBit(devicetype_.length()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize gamename_
	{
		if(gamename_.length()){
		__s__->writeType(gamename_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize pfname_
	{
		if(pfname_.length()){
		__s__->writeType(pfname_);
		}
	}
	// serialize accountid_
	{
		if(accountid_.length()){
		__s__->writeType(accountid_);
		}
	}
	// serialize createtime_
	{
		if(createtime_ != 0){
		__s__->writeType(createtime_);
		}
	}
	// serialize mac_
	{
		if(mac_.length()){
		__s__->writeType(mac_);
		}
	}
	// serialize idfa_
	{
		if(idfa_.length()){
		__s__->writeType(idfa_);
		}
	}
	// serialize ip_
	{
		if(ip_.length()){
		__s__->writeType(ip_);
		}
	}
	// serialize devicetype_
	{
		if(devicetype_.length()){
		__s__->writeType(devicetype_);
		}
	}
}
bool SGE_Account::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize gamename_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gamename_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize pfname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfname_, 65535)) return false;
		}
	}
	// deserialize accountid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountid_, 65535)) return false;
		}
	}
	// deserialize createtime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createtime_)) return false;
		}
	}
	// deserialize mac_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mac_, 65535)) return false;
		}
	}
	// deserialize idfa_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(idfa_, 65535)) return false;
		}
	}
	// deserialize ip_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ip_, 65535)) return false;
		}
	}
	// deserialize devicetype_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(devicetype_, 65535)) return false;
		}
	}
		return true;
}
void SGE_Account::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize gamename_
	ss << "\"gamename_\":";
	{
		ss << "\"" << gamename_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfname_
	ss << "\"pfname_\":";
	{
		ss << "\"" << pfname_ << "\"";
	}
	 ss << ",\n";
	// serialize accountid_
	ss << "\"accountid_\":";
	{
		ss << "\"" << accountid_ << "\"";
	}
	 ss << ",\n";
	// serialize createtime_
	ss << "\"createtime_\":";
	{
		ss << (S64)createtime_;
	}
	 ss << ",\n";
	// serialize mac_
	ss << "\"mac_\":";
	{
		ss << "\"" << mac_ << "\"";
	}
	 ss << ",\n";
	// serialize idfa_
	ss << "\"idfa_\":";
	{
		ss << "\"" << idfa_ << "\"";
	}
	 ss << ",\n";
	// serialize ip_
	ss << "\"ip_\":";
	{
		ss << "\"" << ip_ << "\"";
	}
	 ss << ",\n";
	// serialize devicetype_
	ss << "\"devicetype_\":";
	{
		ss << "\"" << devicetype_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_Login::SGE_Login():
roleId_(0)
,logintime_(0)
,logouttime_(0)
,firsttime_(0)
,rolefirsttime_(0)
,firstserid_(0)
,serverid_(0)
{}
void SGE_Login::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(gamename_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(pfname_.length()?true:false);
	__fm__.writeBit(accountid_.length()?true:false);
	__fm__.writeBit((roleId_==0)?false:true);
	__fm__.writeBit((logintime_==0)?false:true);
	__fm__.writeBit((logouttime_==0)?false:true);
	__fm__.writeBit((firsttime_==0)?false:true);
	__fm__.writeBit((rolefirsttime_==0)?false:true);
	__fm__.writeBit((firstserid_==0)?false:true);
	__fm__.writeBit((serverid_==0)?false:true);
	__fm__.writeBit(mac_.length()?true:false);
	__fm__.writeBit(idfa_.length()?true:false);
	__fm__.writeBit(ip_.length()?true:false);
	__fm__.writeBit(devicetype_.length()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize gamename_
	{
		if(gamename_.length()){
		__s__->writeType(gamename_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize pfname_
	{
		if(pfname_.length()){
		__s__->writeType(pfname_);
		}
	}
	// serialize accountid_
	{
		if(accountid_.length()){
		__s__->writeType(accountid_);
		}
	}
	// serialize roleId_
	{
		if(roleId_ != 0){
		__s__->writeType(roleId_);
		}
	}
	// serialize logintime_
	{
		if(logintime_ != 0){
		__s__->writeType(logintime_);
		}
	}
	// serialize logouttime_
	{
		if(logouttime_ != 0){
		__s__->writeType(logouttime_);
		}
	}
	// serialize firsttime_
	{
		if(firsttime_ != 0){
		__s__->writeType(firsttime_);
		}
	}
	// serialize rolefirsttime_
	{
		if(rolefirsttime_ != 0){
		__s__->writeType(rolefirsttime_);
		}
	}
	// serialize firstserid_
	{
		if(firstserid_ != 0){
		__s__->writeType(firstserid_);
		}
	}
	// serialize serverid_
	{
		if(serverid_ != 0){
		__s__->writeType(serverid_);
		}
	}
	// serialize mac_
	{
		if(mac_.length()){
		__s__->writeType(mac_);
		}
	}
	// serialize idfa_
	{
		if(idfa_.length()){
		__s__->writeType(idfa_);
		}
	}
	// serialize ip_
	{
		if(ip_.length()){
		__s__->writeType(ip_);
		}
	}
	// serialize devicetype_
	{
		if(devicetype_.length()){
		__s__->writeType(devicetype_);
		}
	}
}
bool SGE_Login::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize gamename_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gamename_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize pfname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfname_, 65535)) return false;
		}
	}
	// deserialize accountid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountid_, 65535)) return false;
		}
	}
	// deserialize roleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleId_)) return false;
		}
	}
	// deserialize logintime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(logintime_)) return false;
		}
	}
	// deserialize logouttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(logouttime_)) return false;
		}
	}
	// deserialize firsttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(firsttime_)) return false;
		}
	}
	// deserialize rolefirsttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolefirsttime_)) return false;
		}
	}
	// deserialize firstserid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(firstserid_)) return false;
		}
	}
	// deserialize serverid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(serverid_)) return false;
		}
	}
	// deserialize mac_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(mac_, 65535)) return false;
		}
	}
	// deserialize idfa_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(idfa_, 65535)) return false;
		}
	}
	// deserialize ip_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ip_, 65535)) return false;
		}
	}
	// deserialize devicetype_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(devicetype_, 65535)) return false;
		}
	}
		return true;
}
void SGE_Login::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize gamename_
	ss << "\"gamename_\":";
	{
		ss << "\"" << gamename_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfname_
	ss << "\"pfname_\":";
	{
		ss << "\"" << pfname_ << "\"";
	}
	 ss << ",\n";
	// serialize accountid_
	ss << "\"accountid_\":";
	{
		ss << "\"" << accountid_ << "\"";
	}
	 ss << ",\n";
	// serialize roleId_
	ss << "\"roleId_\":";
	{
		ss << (S64)roleId_;
	}
	 ss << ",\n";
	// serialize logintime_
	ss << "\"logintime_\":";
	{
		ss << (S64)logintime_;
	}
	 ss << ",\n";
	// serialize logouttime_
	ss << "\"logouttime_\":";
	{
		ss << (S64)logouttime_;
	}
	 ss << ",\n";
	// serialize firsttime_
	ss << "\"firsttime_\":";
	{
		ss << (S64)firsttime_;
	}
	 ss << ",\n";
	// serialize rolefirsttime_
	ss << "\"rolefirsttime_\":";
	{
		ss << (S64)rolefirsttime_;
	}
	 ss << ",\n";
	// serialize firstserid_
	ss << "\"firstserid_\":";
	{
		ss << (S64)firstserid_;
	}
	 ss << ",\n";
	// serialize serverid_
	ss << "\"serverid_\":";
	{
		ss << (S64)serverid_;
	}
	 ss << ",\n";
	// serialize mac_
	ss << "\"mac_\":";
	{
		ss << "\"" << mac_ << "\"";
	}
	 ss << ",\n";
	// serialize idfa_
	ss << "\"idfa_\":";
	{
		ss << "\"" << idfa_ << "\"";
	}
	 ss << ",\n";
	// serialize ip_
	ss << "\"ip_\":";
	{
		ss << "\"" << ip_ << "\"";
	}
	 ss << ",\n";
	// serialize devicetype_
	ss << "\"devicetype_\":";
	{
		ss << "\"" << devicetype_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_Order::SGE_Order():
roleId_(0)
,rolelv_(0)
,payment_(0)
{}
void SGE_Order::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit(gamename_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(pfname_.length()?true:false);
	__fm__.writeBit(orderid_.length()?true:false);
	__fm__.writeBit((roleId_==0)?false:true);
	__fm__.writeBit((rolelv_==0)?false:true);
	__fm__.writeBit(accountid_.length()?true:false);
	__fm__.writeBit((payment_==0)?false:true);
	__fm__.writeBit(paytime_.length()?true:false);
	__s__->write(__fm__.masks_, 2);
	// serialize gamename_
	{
		if(gamename_.length()){
		__s__->writeType(gamename_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize pfname_
	{
		if(pfname_.length()){
		__s__->writeType(pfname_);
		}
	}
	// serialize orderid_
	{
		if(orderid_.length()){
		__s__->writeType(orderid_);
		}
	}
	// serialize roleId_
	{
		if(roleId_ != 0){
		__s__->writeType(roleId_);
		}
	}
	// serialize rolelv_
	{
		if(rolelv_ != 0){
		__s__->writeType(rolelv_);
		}
	}
	// serialize accountid_
	{
		if(accountid_.length()){
		__s__->writeType(accountid_);
		}
	}
	// serialize payment_
	{
		if(payment_ != 0){
		__s__->writeType(payment_);
		}
	}
	// serialize paytime_
	{
		if(paytime_.length()){
		__s__->writeType(paytime_);
		}
	}
}
bool SGE_Order::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize gamename_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gamename_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize pfname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfname_, 65535)) return false;
		}
	}
	// deserialize orderid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(orderid_, 65535)) return false;
		}
	}
	// deserialize roleId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleId_)) return false;
		}
	}
	// deserialize rolelv_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolelv_)) return false;
		}
	}
	// deserialize accountid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountid_, 65535)) return false;
		}
	}
	// deserialize payment_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(payment_)) return false;
		}
	}
	// deserialize paytime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(paytime_, 65535)) return false;
		}
	}
		return true;
}
void SGE_Order::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize gamename_
	ss << "\"gamename_\":";
	{
		ss << "\"" << gamename_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfname_
	ss << "\"pfname_\":";
	{
		ss << "\"" << pfname_ << "\"";
	}
	 ss << ",\n";
	// serialize orderid_
	ss << "\"orderid_\":";
	{
		ss << "\"" << orderid_ << "\"";
	}
	 ss << ",\n";
	// serialize roleId_
	ss << "\"roleId_\":";
	{
		ss << (S64)roleId_;
	}
	 ss << ",\n";
	// serialize rolelv_
	ss << "\"rolelv_\":";
	{
		ss << (S64)rolelv_;
	}
	 ss << ",\n";
	// serialize accountid_
	ss << "\"accountid_\":";
	{
		ss << "\"" << accountid_ << "\"";
	}
	 ss << ",\n";
	// serialize payment_
	ss << "\"payment_\":";
	{
		ss << (double)payment_;
	}
	 ss << ",\n";
	// serialize paytime_
	ss << "\"paytime_\":";
	{
		ss << "\"" << paytime_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_LogUIBehavior::SGE_LogUIBehavior():
accountGuid_(0)
,playerGuid_(0)
,type_((UIBehaviorType)(0))
{}
void SGE_LogUIBehavior::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((accountGuid_==0)?false:true);
	__fm__.writeBit(accountName_.length()?true:false);
	__fm__.writeBit((playerGuid_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(clientIp_.length()?true:false);
	__fm__.writeBit((type_==(UIBehaviorType)(0))?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize accountGuid_
	{
		if(accountGuid_ != 0){
		__s__->writeType(accountGuid_);
		}
	}
	// serialize accountName_
	{
		if(accountName_.length()){
		__s__->writeType(accountName_);
		}
	}
	// serialize playerGuid_
	{
		if(playerGuid_ != 0){
		__s__->writeType(playerGuid_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize clientIp_
	{
		if(clientIp_.length()){
		__s__->writeType(clientIp_);
		}
	}
	// serialize type_
	{
		EnumSize __e__ = (EnumSize)type_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
}
bool SGE_LogUIBehavior::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize accountGuid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountGuid_)) return false;
		}
	}
	// deserialize accountName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountName_, 65535)) return false;
		}
	}
	// deserialize playerGuid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerGuid_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize clientIp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(clientIp_, 65535)) return false;
		}
	}
	// deserialize type_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 22) return false;
		type_ = (UIBehaviorType)__e__;
		}
	}
		return true;
}
void SGE_LogUIBehavior::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize accountGuid_
	ss << "\"accountGuid_\":";
	{
		ss << (S64)accountGuid_;
	}
	 ss << ",\n";
	// serialize accountName_
	ss << "\"accountName_\":";
	{
		ss << "\"" << accountName_ << "\"";
	}
	 ss << ",\n";
	// serialize playerGuid_
	ss << "\"playerGuid_\":";
	{
		ss << (S64)playerGuid_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize clientIp_
	ss << "\"clientIp_\":";
	{
		ss << "\"" << clientIp_ << "\"";
	}
	 ss << ",\n";
	// serialize type_
	ss << "\"type_\":";
	{
		ss << "\"" << ENUM(UIBehaviorType).getItemName(type_) << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_LogRole::SGE_LogRole():
roleid_(0)
,cachetime_(0)
,serverid_(0)
,firstserid_(0)
,rolefirsttime_(0)
,rolelasttime_(0)
,rolelv_(0)
,gold_(0)
,diamond_(0)
,magicgold_(0)
,vip_(0)
,ce_(0)
{}
void SGE_LogRole::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<3> __fm__;
	__fm__.writeBit(gamename_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(pfname_.length()?true:false);
	__fm__.writeBit((roleid_==0)?false:true);
	__fm__.writeBit((cachetime_==0)?false:true);
	__fm__.writeBit(accountid_.length()?true:false);
	__fm__.writeBit((serverid_==0)?false:true);
	__fm__.writeBit(servername_.length()?true:false);
	__fm__.writeBit((firstserid_==0)?false:true);
	__fm__.writeBit((rolefirsttime_==0)?false:true);
	__fm__.writeBit((rolelasttime_==0)?false:true);
	__fm__.writeBit((rolelv_==0)?false:true);
	__fm__.writeBit((gold_==0)?false:true);
	__fm__.writeBit((diamond_==0)?false:true);
	__fm__.writeBit((magicgold_==0)?false:true);
	__fm__.writeBit((vip_==0)?false:true);
	__fm__.writeBit((ce_==0)?false:true);
	__s__->write(__fm__.masks_, 3);
	// serialize gamename_
	{
		if(gamename_.length()){
		__s__->writeType(gamename_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize pfname_
	{
		if(pfname_.length()){
		__s__->writeType(pfname_);
		}
	}
	// serialize roleid_
	{
		if(roleid_ != 0){
		__s__->writeType(roleid_);
		}
	}
	// serialize cachetime_
	{
		if(cachetime_ != 0){
		__s__->writeType(cachetime_);
		}
	}
	// serialize accountid_
	{
		if(accountid_.length()){
		__s__->writeType(accountid_);
		}
	}
	// serialize serverid_
	{
		if(serverid_ != 0){
		__s__->writeType(serverid_);
		}
	}
	// serialize servername_
	{
		if(servername_.length()){
		__s__->writeType(servername_);
		}
	}
	// serialize firstserid_
	{
		if(firstserid_ != 0){
		__s__->writeType(firstserid_);
		}
	}
	// serialize rolefirsttime_
	{
		if(rolefirsttime_ != 0){
		__s__->writeType(rolefirsttime_);
		}
	}
	// serialize rolelasttime_
	{
		if(rolelasttime_ != 0){
		__s__->writeType(rolelasttime_);
		}
	}
	// serialize rolelv_
	{
		if(rolelv_ != 0){
		__s__->writeType(rolelv_);
		}
	}
	// serialize gold_
	{
		if(gold_ != 0){
		__s__->writeType(gold_);
		}
	}
	// serialize diamond_
	{
		if(diamond_ != 0){
		__s__->writeType(diamond_);
		}
	}
	// serialize magicgold_
	{
		if(magicgold_ != 0){
		__s__->writeType(magicgold_);
		}
	}
	// serialize vip_
	{
		if(vip_ != 0){
		__s__->writeType(vip_);
		}
	}
	// serialize ce_
	{
		if(ce_ != 0){
		__s__->writeType(ce_);
		}
	}
}
bool SGE_LogRole::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<3> __fm__;
	if(!__r__->read(__fm__.masks_, 3)) return false;
	// deserialize gamename_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gamename_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize pfname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfname_, 65535)) return false;
		}
	}
	// deserialize roleid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(roleid_)) return false;
		}
	}
	// deserialize cachetime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(cachetime_)) return false;
		}
	}
	// deserialize accountid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(accountid_, 65535)) return false;
		}
	}
	// deserialize serverid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(serverid_)) return false;
		}
	}
	// deserialize servername_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(servername_, 65535)) return false;
		}
	}
	// deserialize firstserid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(firstserid_)) return false;
		}
	}
	// deserialize rolefirsttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolefirsttime_)) return false;
		}
	}
	// deserialize rolelasttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolelasttime_)) return false;
		}
	}
	// deserialize rolelv_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rolelv_)) return false;
		}
	}
	// deserialize gold_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gold_)) return false;
		}
	}
	// deserialize diamond_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(diamond_)) return false;
		}
	}
	// deserialize magicgold_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicgold_)) return false;
		}
	}
	// deserialize vip_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(vip_)) return false;
		}
	}
	// deserialize ce_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(ce_)) return false;
		}
	}
		return true;
}
void SGE_LogRole::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize gamename_
	ss << "\"gamename_\":";
	{
		ss << "\"" << gamename_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize pfname_
	ss << "\"pfname_\":";
	{
		ss << "\"" << pfname_ << "\"";
	}
	 ss << ",\n";
	// serialize roleid_
	ss << "\"roleid_\":";
	{
		ss << (S64)roleid_;
	}
	 ss << ",\n";
	// serialize cachetime_
	ss << "\"cachetime_\":";
	{
		ss << (S64)cachetime_;
	}
	 ss << ",\n";
	// serialize accountid_
	ss << "\"accountid_\":";
	{
		ss << "\"" << accountid_ << "\"";
	}
	 ss << ",\n";
	// serialize serverid_
	ss << "\"serverid_\":";
	{
		ss << (S64)serverid_;
	}
	 ss << ",\n";
	// serialize servername_
	ss << "\"servername_\":";
	{
		ss << "\"" << servername_ << "\"";
	}
	 ss << ",\n";
	// serialize firstserid_
	ss << "\"firstserid_\":";
	{
		ss << (S64)firstserid_;
	}
	 ss << ",\n";
	// serialize rolefirsttime_
	ss << "\"rolefirsttime_\":";
	{
		ss << (S64)rolefirsttime_;
	}
	 ss << ",\n";
	// serialize rolelasttime_
	ss << "\"rolelasttime_\":";
	{
		ss << (S64)rolelasttime_;
	}
	 ss << ",\n";
	// serialize rolelv_
	ss << "\"rolelv_\":";
	{
		ss << (S64)rolelv_;
	}
	 ss << ",\n";
	// serialize gold_
	ss << "\"gold_\":";
	{
		ss << (S64)gold_;
	}
	 ss << ",\n";
	// serialize diamond_
	ss << "\"diamond_\":";
	{
		ss << (S64)diamond_;
	}
	 ss << ",\n";
	// serialize magicgold_
	ss << "\"magicgold_\":";
	{
		ss << (S64)magicgold_;
	}
	 ss << ",\n";
	// serialize vip_
	ss << "\"vip_\":";
	{
		ss << (S64)vip_;
	}
	 ss << ",\n";
	// serialize ce_
	ss << "\"ce_\":";
	{
		ss << (S64)ce_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_LogProduceTrack::SGE_LogProduceTrack():
timestamp_(0)
,from_(0)
,playerId_(0)
,itemId_(0)
,itemInstId_(0)
,itemStack_(0)
,diamond_(0)
,money_(0)
,exp_(0)
,magic_(0)
{}
void SGE_LogProduceTrack::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<2> __fm__;
	__fm__.writeBit((timestamp_==0)?false:true);
	__fm__.writeBit((from_==0)?false:true);
	__fm__.writeBit((playerId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit((itemId_==0)?false:true);
	__fm__.writeBit((itemInstId_==0)?false:true);
	__fm__.writeBit((itemStack_==0)?false:true);
	__fm__.writeBit((diamond_==0)?false:true);
	__fm__.writeBit((money_==0)?false:true);
	__fm__.writeBit((exp_==0)?false:true);
	__fm__.writeBit((magic_==0)?false:true);
	__s__->write(__fm__.masks_, 2);
	// serialize timestamp_
	{
		if(timestamp_ != 0){
		__s__->writeType(timestamp_);
		}
	}
	// serialize from_
	{
		if(from_ != 0){
		__s__->writeType(from_);
		}
	}
	// serialize playerId_
	{
		if(playerId_ != 0){
		__s__->writeType(playerId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize itemId_
	{
		if(itemId_ != 0){
		__s__->writeType(itemId_);
		}
	}
	// serialize itemInstId_
	{
		if(itemInstId_ != 0){
		__s__->writeType(itemInstId_);
		}
	}
	// serialize itemStack_
	{
		if(itemStack_ != 0){
		__s__->writeType(itemStack_);
		}
	}
	// serialize diamond_
	{
		if(diamond_ != 0){
		__s__->writeType(diamond_);
		}
	}
	// serialize money_
	{
		if(money_ != 0){
		__s__->writeType(money_);
		}
	}
	// serialize exp_
	{
		if(exp_ != 0){
		__s__->writeType(exp_);
		}
	}
	// serialize magic_
	{
		if(magic_ != 0){
		__s__->writeType(magic_);
		}
	}
}
bool SGE_LogProduceTrack::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<2> __fm__;
	if(!__r__->read(__fm__.masks_, 2)) return false;
	// deserialize timestamp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(timestamp_)) return false;
		}
	}
	// deserialize from_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(from_)) return false;
		}
	}
	// deserialize playerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize itemId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemId_)) return false;
		}
	}
	// deserialize itemInstId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemInstId_)) return false;
		}
	}
	// deserialize itemStack_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(itemStack_)) return false;
		}
	}
	// deserialize diamond_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(diamond_)) return false;
		}
	}
	// deserialize money_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(money_)) return false;
		}
	}
	// deserialize exp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exp_)) return false;
		}
	}
	// deserialize magic_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magic_)) return false;
		}
	}
		return true;
}
void SGE_LogProduceTrack::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize timestamp_
	ss << "\"timestamp_\":";
	{
		ss << (S64)timestamp_;
	}
	 ss << ",\n";
	// serialize from_
	ss << "\"from_\":";
	{
		ss << (S64)from_;
	}
	 ss << ",\n";
	// serialize playerId_
	ss << "\"playerId_\":";
	{
		ss << (S64)playerId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize itemId_
	ss << "\"itemId_\":";
	{
		ss << (S64)itemId_;
	}
	 ss << ",\n";
	// serialize itemInstId_
	ss << "\"itemInstId_\":";
	{
		ss << (S64)itemInstId_;
	}
	 ss << ",\n";
	// serialize itemStack_
	ss << "\"itemStack_\":";
	{
		ss << (S64)itemStack_;
	}
	 ss << ",\n";
	// serialize diamond_
	ss << "\"diamond_\":";
	{
		ss << (S64)diamond_;
	}
	 ss << ",\n";
	// serialize money_
	ss << "\"money_\":";
	{
		ss << (S64)money_;
	}
	 ss << ",\n";
	// serialize exp_
	ss << "\"exp_\":";
	{
		ss << (S64)exp_;
	}
	 ss << ",\n";
	// serialize magic_
	ss << "\"magic_\":";
	{
		ss << (S64)magic_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Exam::COM_Exam():
questionIndex_(0)
,rightNum_(0)
,errorNum_(0)
,money_(0)
,exp_(0)
{}
void COM_Exam::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((questionIndex_==0)?false:true);
	__fm__.writeBit((rightNum_==0)?false:true);
	__fm__.writeBit((errorNum_==0)?false:true);
	__fm__.writeBit((money_==0)?false:true);
	__fm__.writeBit((exp_==0)?false:true);
	__fm__.writeBit(questions_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize questionIndex_
	{
		if(questionIndex_ != 0){
		__s__->writeType(questionIndex_);
		}
	}
	// serialize rightNum_
	{
		if(rightNum_ != 0){
		__s__->writeType(rightNum_);
		}
	}
	// serialize errorNum_
	{
		if(errorNum_ != 0){
		__s__->writeType(errorNum_);
		}
	}
	// serialize money_
	{
		if(money_ != 0){
		__s__->writeType(money_);
		}
	}
	// serialize exp_
	{
		if(exp_ != 0){
		__s__->writeType(exp_);
		}
	}
	// serialize questions_
	if(questions_.size())
	{
		size_t __len__ = (size_t)questions_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(questions_[i]);
		}
	}
}
bool COM_Exam::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize questionIndex_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(questionIndex_)) return false;
		}
	}
	// deserialize rightNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rightNum_)) return false;
		}
	}
	// deserialize errorNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(errorNum_)) return false;
		}
	}
	// deserialize money_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(money_)) return false;
		}
	}
	// deserialize exp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exp_)) return false;
		}
	}
	// deserialize questions_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		questions_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(questions_[i])) return false;
		}
	}
		return true;
}
void COM_Exam::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize questionIndex_
	ss << "\"questionIndex_\":";
	{
		ss << (S64)questionIndex_;
	}
	 ss << ",\n";
	// serialize rightNum_
	ss << "\"rightNum_\":";
	{
		ss << (S64)rightNum_;
	}
	 ss << ",\n";
	// serialize errorNum_
	ss << "\"errorNum_\":";
	{
		ss << (S64)errorNum_;
	}
	 ss << ",\n";
	// serialize money_
	ss << "\"money_\":";
	{
		ss << (S64)money_;
	}
	 ss << ",\n";
	// serialize exp_
	ss << "\"exp_\":";
	{
		ss << (S64)exp_;
	}
	 ss << ",\n";
	// serialize questions_
	ss << "\"questions_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)questions_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)questions_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Answer::COM_Answer():
questionIndex_(0)
,money_(0)
,exp_(0)
,isRigth_(false)
{}
void COM_Answer::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((questionIndex_==0)?false:true);
	__fm__.writeBit((money_==0)?false:true);
	__fm__.writeBit((exp_==0)?false:true);
	__fm__.writeBit(isRigth_);
	__s__->write(__fm__.masks_, 1);
	// serialize questionIndex_
	{
		if(questionIndex_ != 0){
		__s__->writeType(questionIndex_);
		}
	}
	// serialize money_
	{
		if(money_ != 0){
		__s__->writeType(money_);
		}
	}
	// serialize exp_
	{
		if(exp_ != 0){
		__s__->writeType(exp_);
		}
	}
	// serialize isRigth_
	{
	}
}
bool COM_Answer::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize questionIndex_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(questionIndex_)) return false;
		}
	}
	// deserialize money_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(money_)) return false;
		}
	}
	// deserialize exp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exp_)) return false;
		}
	}
	// deserialize isRigth_
	{
		isRigth_ = __fm__.readBit();
	}
		return true;
}
void COM_Answer::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize questionIndex_
	ss << "\"questionIndex_\":";
	{
		ss << (S64)questionIndex_;
	}
	 ss << ",\n";
	// serialize money_
	ss << "\"money_\":";
	{
		ss << (S64)money_;
	}
	 ss << ",\n";
	// serialize exp_
	ss << "\"exp_\":";
	{
		ss << (S64)exp_;
	}
	 ss << ",\n";
	// serialize isRigth_
	ss << "\"isRigth_\":";
	{
		ss << (isRigth_ ? "true" : "false");
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Wishing::COM_Wishing():
wt_((WishType)(0))
{}
void COM_Wishing::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((wt_==(WishType)(0))?false:true);
	__fm__.writeBit(wish_.length()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize wt_
	{
		EnumSize __e__ = (EnumSize)wt_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize wish_
	{
		if(wish_.length()){
		__s__->writeType(wish_);
		}
	}
}
bool COM_Wishing::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize wt_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 6) return false;
		wt_ = (WishType)__e__;
		}
	}
	// deserialize wish_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(wish_, 65535)) return false;
		}
	}
		return true;
}
void COM_Wishing::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize wt_
	ss << "\"wt_\":";
	{
		ss << "\"" << ENUM(WishType).getItemName(wt_) << "\"";
	}
	 ss << ",\n";
	// serialize wish_
	ss << "\"wish_\":";
	{
		ss << "\"" << wish_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_Wish::COM_Wish():
playerInstId_(0)
{}
void COM_Wish::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerInstId_==0)?false:true);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit(wish_.length()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize playerInstId_
	{
		if(playerInstId_ != 0){
		__s__->writeType(playerInstId_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize wish_
	{
		if(wish_.length()){
		__s__->writeType(wish_);
		}
	}
}
bool COM_Wish::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerInstId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerInstId_)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize wish_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(wish_, 65535)) return false;
		}
	}
		return true;
}
void COM_Wish::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerInstId_
	ss << "\"playerInstId_\":";
	{
		ss << (S64)playerInstId_;
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize wish_
	ss << "\"wish_\":";
	{
		ss << "\"" << wish_ << "\"";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_GmtOrder::SGE_GmtOrder():
shopId_(0)
,payment_(0)
{}
void SGE_GmtOrder::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((shopId_==0)?false:true);
	__fm__.writeBit(orderId_.length()?true:false);
	__fm__.writeBit((payment_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize shopId_
	{
		if(shopId_ != 0){
		__s__->writeType(shopId_);
		}
	}
	// serialize orderId_
	{
		if(orderId_.length()){
		__s__->writeType(orderId_);
		}
	}
	// serialize payment_
	{
		if(payment_ != 0){
		__s__->writeType(payment_);
		}
	}
}
bool SGE_GmtOrder::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize shopId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(shopId_)) return false;
		}
	}
	// deserialize orderId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(orderId_, 65535)) return false;
		}
	}
	// deserialize payment_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(payment_)) return false;
		}
	}
		return true;
}
void SGE_GmtOrder::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize shopId_
	ss << "\"shopId_\":";
	{
		ss << (S64)shopId_;
	}
	 ss << ",\n";
	// serialize orderId_
	ss << "\"orderId_\":";
	{
		ss << "\"" << orderId_ << "\"";
	}
	 ss << ",\n";
	// serialize payment_
	ss << "\"payment_\":";
	{
		ss << (double)payment_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_KeyContent::COM_KeyContent():
usetime_(0)
{}
void COM_KeyContent::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(giftname_.length()?true:false);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(key_.length()?true:false);
	__fm__.writeBit(playerName_.length()?true:false);
	__fm__.writeBit((usetime_==0)?false:true);
	__fm__.writeBit(items_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize giftname_
	{
		if(giftname_.length()){
		__s__->writeType(giftname_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize key_
	{
		if(key_.length()){
		__s__->writeType(key_);
		}
	}
	// serialize playerName_
	{
		if(playerName_.length()){
		__s__->writeType(playerName_);
		}
	}
	// serialize usetime_
	{
		if(usetime_ != 0){
		__s__->writeType(usetime_);
		}
	}
	// serialize items_
	if(items_.size())
	{
		size_t __len__ = (size_t)items_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serialize(__s__);
		}
	}
}
bool COM_KeyContent::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize giftname_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(giftname_, 65535)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize key_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(key_, 65535)) return false;
		}
	}
	// deserialize playerName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerName_, 65535)) return false;
		}
	}
	// deserialize usetime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(usetime_)) return false;
		}
	}
	// deserialize items_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		items_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!items_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void COM_KeyContent::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize giftname_
	ss << "\"giftname_\":";
	{
		ss << "\"" << giftname_ << "\"";
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize key_
	ss << "\"key_\":";
	{
		ss << "\"" << key_ << "\"";
	}
	 ss << ",\n";
	// serialize playerName_
	ss << "\"playerName_\":";
	{
		ss << "\"" << playerName_ << "\"";
	}
	 ss << ",\n";
	// serialize usetime_
	ss << "\"usetime_\":";
	{
		ss << (S64)usetime_;
	}
	 ss << ",\n";
	// serialize items_
	ss << "\"items_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)items_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			items_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
COM_EmployeeQuestInst::COM_EmployeeQuestInst():
status_((EmployeeQuestStatus)(0))
,questId_(0)
,timeout_(0)
{}
void COM_EmployeeQuestInst::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((status_==(EmployeeQuestStatus)(0))?false:true);
	__fm__.writeBit((questId_==0)?false:true);
	__fm__.writeBit((timeout_==0)?false:true);
	__fm__.writeBit(usedEmployees_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize status_
	{
		EnumSize __e__ = (EnumSize)status_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize questId_
	{
		if(questId_ != 0){
		__s__->writeType(questId_);
		}
	}
	// serialize timeout_
	{
		if(timeout_ != 0){
		__s__->writeType(timeout_);
		}
	}
	// serialize usedEmployees_
	if(usedEmployees_.size())
	{
		size_t __len__ = (size_t)usedEmployees_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(usedEmployees_[i]);
		}
	}
}
bool COM_EmployeeQuestInst::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize status_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 3) return false;
		status_ = (EmployeeQuestStatus)__e__;
		}
	}
	// deserialize questId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(questId_)) return false;
		}
	}
	// deserialize timeout_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(timeout_)) return false;
		}
	}
	// deserialize usedEmployees_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		usedEmployees_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(usedEmployees_[i])) return false;
		}
	}
		return true;
}
void COM_EmployeeQuestInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize status_
	ss << "\"status_\":";
	{
		ss << "\"" << ENUM(EmployeeQuestStatus).getItemName(status_) << "\"";
	}
	 ss << ",\n";
	// serialize questId_
	ss << "\"questId_\":";
	{
		ss << (S64)questId_;
	}
	 ss << ",\n";
	// serialize timeout_
	ss << "\"timeout_\":";
	{
		ss << (S64)timeout_;
	}
	 ss << ",\n";
	// serialize usedEmployees_
	ss << "\"usedEmployees_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)usedEmployees_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)usedEmployees_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_EmployeeQuestInst::SGE_EmployeeQuestInst():
doTime_(0)
,refreshTime_(0)
{}
void SGE_EmployeeQuestInst::serialize(ProtocolWriter* __s__) const
{
	COM_EmployeeQuestInst::serialize(__s__);
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((doTime_==0)?false:true);
	__fm__.writeBit((refreshTime_==0)?false:true);
	__s__->write(__fm__.masks_, 1);
	// serialize doTime_
	{
		if(doTime_ != 0){
		__s__->writeType(doTime_);
		}
	}
	// serialize refreshTime_
	{
		if(refreshTime_ != 0){
		__s__->writeType(refreshTime_);
		}
	}
}
bool SGE_EmployeeQuestInst::deserialize(ProtocolReader* __r__)
{
	if(!COM_EmployeeQuestInst::deserialize(__r__)) return false;
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize doTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(doTime_)) return false;
		}
	}
	// deserialize refreshTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(refreshTime_)) return false;
		}
	}
		return true;
}
void SGE_EmployeeQuestInst::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_EmployeeQuestInst::serializeJson(ss,false);
	// serialize doTime_
	ss << "\"doTime_\":";
	{
		ss << (S64)doTime_;
	}
	 ss << ",\n";
	// serialize refreshTime_
	ss << "\"refreshTime_\":";
	{
		ss << (S64)refreshTime_;
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
void SGE_PlayerEmployeeQuestArray::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit(value_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize value_
	if(value_.size())
	{
		size_t __len__ = (size_t)value_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			value_[i].serialize(__s__);
		}
	}
}
bool SGE_PlayerEmployeeQuestArray::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize value_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		value_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!value_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void SGE_PlayerEmployeeQuestArray::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize value_
	ss << "\"value_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)value_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			value_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_PlayerEmployeeQuest::SGE_PlayerEmployeeQuest():
playerId_(0)
{}
void SGE_PlayerEmployeeQuest::serialize(ProtocolWriter* __s__) const
{
	//field mask
	FieldMask<1> __fm__;
	__fm__.writeBit((playerId_==0)?false:true);
	__fm__.writeBit(usedEmployees_.size()?true:false);
	__fm__.writeBit(quests_.size()?true:false);
	__s__->write(__fm__.masks_, 1);
	// serialize playerId_
	{
		if(playerId_ != 0){
		__s__->writeType(playerId_);
		}
	}
	// serialize usedEmployees_
	if(usedEmployees_.size())
	{
		size_t __len__ = (size_t)usedEmployees_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(usedEmployees_[i]);
		}
	}
	// serialize quests_
	if(quests_.size())
	{
		size_t __len__ = (size_t)quests_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serialize(__s__);
		}
	}
}
bool SGE_PlayerEmployeeQuest::deserialize(ProtocolReader* __r__)
{
	//field mask
	FieldMask<1> __fm__;
	if(!__r__->read(__fm__.masks_, 1)) return false;
	// deserialize playerId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(playerId_)) return false;
		}
	}
	// deserialize usedEmployees_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		usedEmployees_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(usedEmployees_[i])) return false;
		}
	}
	// deserialize quests_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		quests_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!quests_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void SGE_PlayerEmployeeQuest::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	// serialize playerId_
	ss << "\"playerId_\":";
	{
		ss << (S64)playerId_;
	}
	 ss << ",\n";
	// serialize usedEmployees_
	ss << "\"usedEmployees_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)usedEmployees_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)usedEmployees_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize quests_
	ss << "\"quests_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)quests_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
