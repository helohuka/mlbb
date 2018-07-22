
#include "itemtable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "skilltable.h"

bool ItemTable::ItemSortFunction::operator()(struct COM_Item* r, struct COM_Item* l)
{
	if(NULL == l && NULL == r)
		return false;
	if(NULL == l)
		return true;
	if(NULL == r)
		return false;
	
	const ItemData *ld = ItemTable::getItemById(l->itemId_), *rd = ItemTable::getItemById(r->itemId_);
	{
		if(NULL == ld && NULL == rd)
			return false;
		if(NULL == ld)
			return true;
		if(NULL == rd)
			return false;
	}
	{
		if(ld->mainType_ < rd->mainType_)
			return true;
		else if(ld->mainType_ > rd->mainType_)
			return false;
	}
	{
		if(ld->subType_ < rd->subType_)
			return true;
		else if(ld->subType_ > rd->subType_)
			return false;
	}
	{
		if(l->itemId_ < r->itemId_)
			return true;
		else if(l->itemId_ > r->itemId_)
			return false;
	}
	{
		if(l->stack_ < r->stack_)
			return true;
		else if(l->stack_ > r->stack_)
			return false;
	}
	{
		if(l->instId_ < r->instId_)
			return true;
		else if(l->instId_ > r->instId_)
			return false;
	}
	
	return false;
}


std::map< S32 , ItemTable::ItemData* >  ItemTable::data_;

std::string 
inline itemCompileScript(std::string &chunk, S32 skid, char const *fix)
{
	char id_str[128]={0};
	sprintf(id_str, "Item_%d_", skid);

	std::string func = id_str;				
	std::string err;
	
	func += fix;
	if(!ScriptEnv::loadScriptProc( chunk.c_str(), err, func.c_str()))
	{
		SRV_ASSERT(0);
	}
	return func;
}

bool ItemTable::load(char const *fn)
{
	
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
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Item has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		ItemData * pCore = new ItemData;
		pCore->id_ = id;
		pCore->level_ = csv.get_int(row,"Level");
		pCore->price_ = csv.get_int(row,"Price");
		pCore->needMoney_ = csv.get_int(row,"NeedMoney");
		pCore->maxCount_ = csv.get_int(row,"MaxCount");
		pCore->employeeId_ = csv.get_int(row,"EmployeeId");
		pCore->addValue_ = csv.get_int(row,"AddValue");
		pCore->artifactVol_ = csv.get_int(row,"ArtifactExp");
		pCore->titleId_ = csv.get_int(row,"Title");
		pCore->useTime_ = csv.get_int(row,"time") * ONE_DAY_SEC;
		pCore->bindType_ = (BindType)ENUM(BindType).getItemId(csv.get_string(row,"BindType"));
		{
			std::string stt = csv.get_string(row,"ItemMainType");
			int stte = ENUM(ItemMainType).getItemId(stt);
			if(-1 == stte)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("ItemMainType error in row %d , id is %d\n"),row,id));
				SRV_ASSERT(0);	
			}
			pCore->mainType_ = (ItemMainType)stte;
		}

		{
			std::string strInfluenceSkill = csv.get_string(row,"InfluenceSkill");
			if(!strInfluenceSkill.empty())
			{
				const char* pchar = strInfluenceSkill.c_str();
				std::string token;
				TokenParser::getToken( pchar , token , ';');
				pCore->influenceSkill_.first =  atoi(token.c_str());
				TokenParser::getToken( pchar , token , ';');
				pCore->influenceSkill_.second = atof(token.c_str());
			}
		}

		{
			std::string stt = csv.get_string(row,"ItemSubType");
			int stte = ENUM(ItemSubType).getItemId(stt);
			if(-1 == stte)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("ItemSubType error in row %d , id is %d\n"),row,id));
				SRV_ASSERT(0);	
			}
			pCore->subType_ = (ItemSubType)stte;

		}
		if(pCore->mainType_  == IMT_Equip || IMT_EmployeeEquip)
		{
			std::string stt = csv.get_string(row,"EquipmentSlot");
			int stte = ENUM(EquipmentSlot).getItemId(stt);
			if(-1 == stte)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("EquipmentSlot error in row %d , id is %d\n"),row,id));
				SRV_ASSERT(0);	
			}
			pCore->slot_ = (EquipmentSlot)stte;
		}

		{
			std::string stt = csv.get_string(row,"Quality");
			int stte = ENUM(QualityColor).getItemId(stt);
			if(-1 == stte)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Quality error in row %d , id is %d\n"),row,id));
				SRV_ASSERT(0);	
			}
			pCore->color_ = (QualityColor)stte;
		}


		std::string strtmp = csv.get_string(row,"WeaponType");
		int iWt = ENUM(WeaponType).getItemId(strtmp);
		if(-1 == iWt)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("itemTable Kind error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		pCore->weaponType_ = (WeaponType)iWt;

		pCore->name_ = csv.get_string(row,"Name");

		pCore->skillId_ = csv.get_int(row, "skillID");

		pCore->gloAction_ = csv.get_string(row,"GloAction");
		pCore->gloAction_ = " return " + pCore->gloAction_;
		pCore->gloAction_ = itemCompileScript(pCore->gloAction_,id,"GloAction");
		
		
		//
		//pCore->durability_.type_ = PT_Durability;	
		ItemPropData	dcpv;
		std::string dstr = csv.get_string(row,"Durability");
		dcpv.type_ = PT_Durability;
		if(!dstr.empty())
		{
			pCore->durability_ = strToken(dstr,dcpv);
		}

		ItemPropData	cpv;
		std::string str = csv.get_string(row,"HpMax");
		cpv.type_ = PT_HpMax;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		 str = csv.get_string(row,"MpMax");
		cpv.type_ = PT_MpMax;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Attack");
		cpv.type_ = PT_Attack;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Defense");
		cpv.type_ = PT_Defense;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Agile");
		cpv.type_ = PT_Agile;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Spirit");
		cpv.type_ = PT_Spirit;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Reply");
		cpv.type_ = PT_Reply;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Hit");
		cpv.type_ = PT_Hit;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		
		str = csv.get_string(row,"Magicattack");
		cpv.type_ = PT_Magicattack;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		
		str = csv.get_string(row,"Magicdefense");
		cpv.type_ = PT_Magicdefense;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Dodge");
		cpv.type_ = PT_Dodge;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"Crit");
		cpv.type_ = PT_Crit;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	
		str = csv.get_string(row,"counterpunch");
		cpv.type_ = PT_counterpunch;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	
	
		str = csv.get_string(row,"Wind");
		cpv.type_ = PT_Wind;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	
		str = csv.get_string(row,"Land");
		cpv.type_ = PT_Land;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	
		str = csv.get_string(row,"Water");
		cpv.type_ = PT_Water;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	
		str = csv.get_string(row,"Fire");
		cpv.type_ = PT_Fire;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
		str = csv.get_string(row,"PT_SneakAttack");
		cpv.type_ = PT_SneakAttack;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}	

		str = csv.get_string(row,"NoSleep");
		cpv.type_ = PT_NoSleep;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}

		str = csv.get_string(row,"NoPetrifaction");
		cpv.type_ = PT_NoPetrifaction;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}

		str = csv.get_string(row,"NoDrunk");
		cpv.type_ = PT_NoDrunk;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}

		str = csv.get_string(row,"NoChaos");
		cpv.type_ = PT_NoChaos;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}
	
		str = csv.get_string(row,"Poison");
		cpv.type_ = PT_NoPoison;
		if(!str.empty())
		{
			pCore->propValue_.push_back(strToken(str,cpv));
		}

		str = csv.get_string(row,"Weapon_D");
		if(!str.empty())
		{
			strTokenGem(str,pCore->WeaponD_);
		}
		
		str = csv.get_string(row,"Weapon_P");
		if(!str.empty())
		{
			strTokenGem(str,pCore->WeaponP_);
		}
	
		str = csv.get_string(row,"Armor_D");
		if(!str.empty())
		{
			strTokenGem(str,pCore->ArmorD_);
		}

		str = csv.get_string(row,"Armor_P");
		if(!str.empty())
		{
			strTokenGem(str,pCore->ArmorP_);
		}

		data_[id] = pCore;
	}

	return true;
}

ItemTable::ItemPropData 
ItemTable::strToken(std::string str,ItemPropData prop)
{
		char const * pChar = str.c_str();
		std::string strToken;
		while( TokenParser::getToken( pChar , strToken , ';'))
		{
			prop.value_.push_back(atoi(strToken.c_str()));
		}
		return prop;
}

void
ItemTable::strTokenGem(std::string str,std::vector<std::pair< PropertyType , float > > & gem )
{
	char const * pChar = str.c_str();
	std::string strToken;

	std::vector<std::string> strs;
	while( TokenParser::getToken( pChar , strToken , ','))
	{
		strs.push_back(strToken.c_str());
	}
	
	std::vector<std::vector<std::string> >props;
	for( int i= 0;i<strs.size();i++)
	{
		std::string strToken1;
		std::vector<std::string> strs1;
		char const * pChar1 = strs[i].c_str();
		while( TokenParser::getToken( pChar1 , strToken1 , ';'))
		{
			strs1.push_back(strToken1.c_str());
		}
		props.push_back(strs1);
	}

	for(int i=0;i<props.size();i++)
	{
		std::pair< PropertyType , float >prop;
		int propt=  ENUM(PropertyType).getItemId(props[i][0]);
		prop.first =  (PropertyType)propt;
		prop.second  = atof(props[i][1].c_str());

		gem.push_back(prop);
	}
}

void ItemTable::clear(){
	std::map< S32 , ItemData* >::iterator itor = data_.begin();

	while(itor != data_.end()){
		if(itor->second){
			delete itor->second;
		}
		++itor;
	}

	data_.clear();
}

bool
ItemTable::check()
{
	std::map< S32 , ItemData* >::iterator itor = data_.begin();

	while(itor != data_.end())
	{
		ItemData* pItem = itor->second;

		if (!pItem)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("itemTable not find item error\n")));
			return false;
		}
		if(pItem->needMoney_ >0 )
		{
			if(pItem->needMoney_  <= pItem->price_)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("itemTable needmoney  >  price_ \n")));
				return false;
			}
		}
		if(pItem->maxCount_ < 1){
			SRV_ASSERT(0);
		}

		for (size_t i = 0; i < pItem->propValue_.size(); ++i)
		{
			if(pItem->propValue_[i].value_.size() < 2)
				return false;
		}

		if(pItem->skillId_){
			SRV_ASSERT(SkillTable::getSkillById(pItem->skillId_,1));
		}

		if(pItem->mainType_ == IMT_Equip || pItem->mainType_ == IMT_EmployeeEquip){
			SRV_ASSERT(pItem->slot_ > ES_None && pItem->slot_ < ES_Max);
		}

		itor++;
	}

	return true;
}

ItemTable::ItemData const*
ItemTable::getItemById(U32 id)
{
	return data_[id];
}


//////////////////////////////////////////////////////////////////////////
std::map<int32,RuneTable::Data*> RuneTable::data_;

bool RuneTable::load(char const *fn){
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
	
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Rune has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		Data* p = new Data();
		p->itemId_ = id;

		std::string needstring = csv.get_string(row,"ItemID");
		if (!needstring.empty()){
			std::vector<std::string> needstringv = String::Split(needstring,";");
			if(needstringv.size() == 2){
				p->needItem_.first = ACE_OS::atoi(needstringv[0].c_str());
				p->needItem_.second = ACE_OS::atoi(needstringv[1].c_str());
			}
		}
		p->resultItemId_ = csv.get_int(row,"Result");

		data_[id] = p;
	}
	return true;
}
bool RuneTable::check(){
	for(std::map<int32,Data*>::iterator i=data_.begin(),e=data_.end(); i!=e; ++i){
		SRV_ASSERT(ItemTable::getItemById(i->second->itemId_));
		SRV_ASSERT(ItemTable::getItemById(i->second->resultItemId_));
		if(i->second->needItem_.first){
			SRV_ASSERT(ItemTable::getItemById(i->second->needItem_.first));
			SRV_ASSERT(i->second->needItem_.second);
		}
	}
	return true;
}
RuneTable::Data const * RuneTable::getDataById(U32 id){
	return data_[id];
}