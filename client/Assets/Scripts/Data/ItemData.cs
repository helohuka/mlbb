using System;
using System.Collections.Generic;

public class ItemData
{
	public int id_;
	public int level_;
	public int price_;
	public int maxCount_;
	public QualityColor quality_;
	public int skillId_;
	public int employeeId_;
	public string name_;
	public string  icon_;
	public string  desc_;
	public string  acquiringWay_;
	public ItemMainType mainType_;
    public ItemSubType subType_;
    public EquipmentSlot slot_;
	public int AddValue_;
    public ItemUseFlag usedFlag_;
	public JobType professionType_;
    public WeaponType weaponType_;
	public WeaponActionType weaponActionType_;
    public int weaponEntityId_;
	public int artifactExp_;
	public int isShow_;
	public BindType bindType_;
	public  List<KeyValuePair<PropertyType,string[]>>  propArr = new List<KeyValuePair<PropertyType, string[]>>();

	public  List<KeyValuePair<PropertyType,string>>  GemWeaponPropArr = new List<KeyValuePair<PropertyType, string>>();
	public  List<KeyValuePair<PropertyType,string>>  GemArmorPropArr = new List<KeyValuePair<PropertyType, string>>();

	private static Dictionary<int, ItemData> metaData;
	public static List<ItemData> equipData = new List<ItemData>();

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ItemData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ItemData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ItemData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ItemData ();
			data.id_ = parser.GetInt (i, "ID");
			data.level_ = parser.GetInt (i, "Level");
            //data.kind_ = parser.GetString (i, "Kind");
			data.price_ = parser.GetInt (i, "Price");
			data.maxCount_ = parser.GetInt (i, "MaxCount");
			data.quality_ =   (QualityColor)Enum.Parse(typeof(QualityColor), parser.GetString(i, "Quality"));
			data.skillId_ = parser.GetInt(i, "skillID");
			data.name_ = parser.GetString (i, "Name");
			data.icon_= parser.GetString (i, "Icon");
			data.desc_ = parser.GetString (i, "Desc");
			data.isShow_ = parser.GetInt (i, "isshow");
			data.bindType_ =  (BindType)Enum.Parse(typeof(BindType), parser.GetString(i, "BindType"));
			data.acquiringWay_ = parser.GetString(i,"AcquiringWay");
            data.usedFlag_ = (ItemUseFlag)Enum.Parse(typeof(ItemUseFlag), parser.GetString(i, "CanUse"));
            data.mainType_ = (ItemMainType)Enum.Parse(typeof(ItemMainType), parser.GetString(i, "ItemMainType"));
            data.subType_ = (ItemSubType)Enum.Parse(typeof(ItemSubType), parser.GetString(i, "ItemSubType"));
            data.slot_ = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), parser.GetString(i, "EquipmentSlot"));
            if (string.IsNullOrEmpty(parser.GetString(i, "WeaponType")))
                data.weaponType_ = WeaponType.WT_None;
            else
                data.weaponType_ = (WeaponType)Enum.Parse(typeof(WeaponType), parser.GetString(i, "WeaponType"));
			data.weaponActionType_ = (WeaponActionType)Enum.Parse(typeof(WeaponActionType), parser.GetString(i, "AttackAction")); 
            data.weaponEntityId_ = parser.GetInt(i, "WeaponEntityId");
			data.artifactExp_ = parser.GetInt(i, "ArtifactExp");
			data.AddValue_ = parser.GetInt(i, "AddValue");
			string profession = parser.GetString(i, "Profession");
			if(!string.IsNullOrEmpty(profession))
				data.professionType_ = (JobType)Enum.Parse(typeof(JobType), profession);
			if(data.mainType_ == ItemMainType.IMT_Employee )
			{
				data.employeeId_ = parser.GetInt(i, "EmployeeId");
			}

			//  prop 
			string propSre = parser.GetString(i, "Durability");
			string[] propValues;
			KeyValuePair<PropertyType,string[]> dataArr = new KeyValuePair<PropertyType, string[]>();
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Durability").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Durability,propValues);
				data.propArr.Add(dataArr);
			}

			 propSre = parser.GetString(i, "HpMax");

			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "HpMax").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_HpMax,propValues);
				data.propArr.Add(dataArr);
			}


			propSre = parser.GetString(i, "Magicattack");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Magicattack").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Magicattack,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Magicdefense");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Magicdefense").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Magicdefense,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "MpMax");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "MpMax").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_MpMax,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Attack");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Attack").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Attack,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Defense");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Defense").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Defense,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Agile");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Agile").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Agile,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "counterpunch");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "counterpunch").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_counterpunch,propValues);
				data.propArr.Add(dataArr);
			}



			propSre = parser.GetString(i, "Spirit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Spirit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Spirit,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Reply");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Reply").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Reply,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Hit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Hit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Hit,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Dodge");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Dodge").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Dodge,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Crit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Crit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Crit,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Wind");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Wind").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Wind,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Land");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Land").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Land,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Water");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Water").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Water,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Fire");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Fire").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Fire,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "PT_SneakAttack");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "PT_SneakAttack").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_SneakAttack,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoSleep");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoSleep").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoSleep,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoPetrifaction");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoPetrifaction").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoPetrifaction,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoDrunk");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoDrunk").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoDrunk,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoChaos");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoChaos").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoChaos,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Poison");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Poison").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoPoison,propValues);
				data.propArr.Add(dataArr);
			}


			KeyValuePair<PropertyType,string> gemArr = new KeyValuePair<PropertyType, string>();
			string gemWeaponD = parser.GetString(i, "Weapon_D");
			if(!string.IsNullOrEmpty(gemWeaponD))
			{
				string[] wDstr = gemWeaponD.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
				for(int w=0;w<wDstr.Length;w++)
				{
					string[] wDstr1 = wDstr[w].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
					gemArr = new KeyValuePair<PropertyType, string>((PropertyType)Enum.Parse(typeof(PropertyType),wDstr1[0]),wDstr1[1]);
					data.GemWeaponPropArr.Add(gemArr);
				}
			}

			string gemWeaponP = parser.GetString(i, "Weapon_P");
			if(!string.IsNullOrEmpty(gemWeaponP))
			{
				string[] wDstr = gemWeaponP.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
				for(int w=0;w<wDstr.Length;w++)
				{
					string[] wDstr1 = wDstr[w].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
					gemArr = new KeyValuePair<PropertyType, string>((PropertyType)Enum.Parse(typeof(PropertyType),wDstr1[0]),wDstr1[1]);
					data.GemWeaponPropArr.Add(gemArr);
				}
			}

			string gemArmorD = parser.GetString(i, "Armor_D");
			if(!string.IsNullOrEmpty(gemArmorD))
			{
				string[] wDstr = gemArmorD.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
				for(int w=0;w<wDstr.Length;w++)
				{
					string[] wDstr1 = wDstr[w].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
					gemArr = new KeyValuePair<PropertyType, string>((PropertyType)Enum.Parse(typeof(PropertyType),wDstr1[0]),wDstr1[1]);
					data.GemArmorPropArr.Add(gemArr);
				}
			}

			string gemArmorP = parser.GetString(i, "Armor_P");
			if(!string.IsNullOrEmpty(gemArmorP))
			{
				string[] wDstr = gemArmorP.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
				for(int w=0;w<wDstr.Length;w++)
				{
					string[] wDstr1 = wDstr[w].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
					gemArr = new KeyValuePair<PropertyType, string>((PropertyType)Enum.Parse(typeof(PropertyType),wDstr1[0]),wDstr1[1]);
					data.GemArmorPropArr.Add(gemArr);
				}
			}

			//
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("ItemData" + ConfigLoader.Instance.csvext + "ID重复 " + data.id_);
				return;
			}
			metaData[data.id_] = data;

			if(data.mainType_ == ItemMainType.IMT_Equip)
			{
				equipData.Add(data);
			}
		}
		parser.Dispose ();
		parser = null;
	}



	private void GemStrValue()
	{

	}

    public bool canUse_
    {
        get { return usedFlag_ != ItemUseFlag.IUF_None; }
    }

    

	public static ItemData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
		{
			//ClientLog.Instance.Log("Item ID: " + id + "is not exist!");
			return null;
		}
		return metaData[id];
	}
	




}


