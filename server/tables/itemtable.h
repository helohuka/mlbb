/************************************************************************/
/**
 * @file	skilltable.h
 * @date	2015-3-2015/03/05 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

#ifndef __ITEM_TABLE_H__
#define __ITEM_TABLE_H__

#include "config.h"

class ItemTable
{
public:

	struct ItemPropData
	{
		PropertyType	type_;
		std::vector<S32> 	value_;
	};

	struct ItemData
	{
		S32 id_;
		S32 level_;
		S32 price_;
		S32 needMoney_;
		S32 maxCount_;
		S32 quality_;
		S32 skillId_;
		S32 employeeId_;
		S32 titleId_;
		S32 addValue_;
		S32 artifactVol_;
		BindType bindType_;
		ItemPropData durability_;
		std::pair<S32,float> influenceSkill_;
		S32 useTime_;
		ItemMainType mainType_;
		ItemSubType  subType_;
		EquipmentSlot slot_;
		JobType		profession_;
		WeaponType	weaponType_;
		QualityColor color_;
		std::vector<ItemPropData> propValue_;
		
		std::pair<PropertyType,int> wd_;
		std::pair<PropertyType,float> wp_;

		std::string	 name_;
		
		std::string  gloAction_;

		std::vector<std::pair<PropertyType,float> >  WeaponP_;
		std::vector<std::pair<PropertyType,float> >  ArmorP_;
		std::vector<std::pair<PropertyType,float> >  WeaponD_;
		std::vector<std::pair<PropertyType,float> >  ArmorD_;

		inline bool isWeapon() const
		{
			return subType_ >=IST_Axe && subType_ <= IST_Knife/*&& subType_ <=IST_V*/;
		}

		inline bool isArmor() const
		{
			return subType_ >=IST_Hat && subType_ <= IST_Shield;
		}
	};

	struct ItemSortFunction
	{
		bool operator()(struct COM_Item* l, struct COM_Item* r);
	};
public:
	static bool load(char const *fn);
	static void clear();
	static bool check();
	static ItemData const * getItemById(U32 id);
	static ItemPropData  strToken(std::string str,ItemPropData prop);
	
	static void  strTokenGem(std::string str,	std::vector<std::pair< PropertyType , float > > & gem );
	
public:
	static std::map< S32 , ItemData* >  data_;
};

class RuneTable {
public:
	struct Data{
		int32 itemId_;
		std::pair<int32, int32> needItem_;
		int32 resultItemId_;
	};
	static bool load(char const *fn);
	static bool check();
	static Data const * getDataById(U32 id);

	static std::map<int32,Data*> data_;
};

#endif