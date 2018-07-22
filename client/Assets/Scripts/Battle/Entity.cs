using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Entity
{
	public EntityType type_;
    public bool isLevelUp_;
	private string      instName_;
	private int       instId_;
	protected float[] properties_ ;//= new int[(int)PropertyType.PT_Max];
    protected List<COM_Skill> skillInsts_ = new List<COM_Skill>();
	private COM_Item[] equips_ = new COM_Item[(int)EquipmentSlot.ES_Max];
	private string instIcon_;
    public List<COM_State> buffList_;
	public EntityActionController ControlEntity;

    public delegate void IPropUpdateEvent();
    public event IPropUpdateEvent OnIPropUpdate;
    public WearEquipEventHandler WearEquipEvent;
    public DelEquipEventHandler DelEquipEvent;

    public delegate void StateUpdateHandler();
    public event StateUpdateHandler OnStateUpdate;

    public COM_Skill suitSkill;
    public bool isHighestSuitSkill;

    public bool isEquipDirty_;

    public bool isPropDirty_;

    private string gameobjectname_;

    public string GameObjectName
    {
        get { return gameobjectname_; }
    }

    public bool IsAi()
    {
        return (type_ == EntityType.ET_Emplyee || type_ == EntityType.ET_Monster);
    }

    public void OnPropUpdate()
    {
        if (OnIPropUpdate!=null)
            OnIPropUpdate();
    }

	public void SetEntity(COM_Entity entity)
	{
		type_ = entity.type_;
		instId_ = (int)entity.instId_;
		instName_ = entity.instName_;
		properties_ = entity.properties_;
        buffList_ = new List<COM_State>(entity.states_);
		//skillInsts_ = entity.skill_;

        skillInsts_.Clear();
		skillInsts_.AddRange(entity.skill_);

		for(int i=0; i < equips_.Length; ++i)
		{
			equips_[i] = null;
		}

		for(int k =0;k<entity.equips_.Length;k++)
		{
			equips_[ entity.equips_[k].slot_] = entity.equips_[k];
		}

        if (type_ == EntityType.ET_Baby)
            CalcBabyEquipSkill();
	
        gameobjectname_ = Enum.GetName(typeof(EntityType), type_) + "[" + instId_.ToString() + "]";

	}

    public COM_Entity GetEntity()
    {
        COM_Entity entity = new COM_Entity();
        entity.type_ = type_;
        entity.instId_ = (uint)instId_;
        entity.instName_ = instName_;
        entity.properties_ = properties_;
        entity.states_ = buffList_.ToArray();
        entity.skill_ = skillInsts_.ToArray();
        List<COM_Item> equips = new List<COM_Item>();
        for (int k = 0; k < equips_.Length; k++)
        {
            equips.Add(equips_[k]);
        }
        entity.equips_ = equips.ToArray();
        return entity;
    }

	// only read once
	private int changeVal_;
	public int ChangeAttributeValue
	{
		set { changeVal_ = value; }
		get
		{
			int tVal = changeVal_;
			changeVal_ = 0;
			return tVal;
		}
	}

	private PropertyType changeType_;
	public PropertyType ChangeAttributeType
	{
		set { changeType_ = value; }
		get { return changeType_; }
	}

	public bool isDead
	{
		get { return properties_[(int)PropertyType.PT_HpCurr] <= 0; }
	}

	public int InstId
	{
        set { instId_ = value; }
		get { return (int)instId_; }
	}
	public string instIcon
	{
		get { return instIcon_; }
	}
	public string InstName
	{
		get { return instName_; }
		set { instName_ = value;}
	}
	
	public string AssetName
	{
		get { return EntityAssetsData.GetData(GetIprop(PropertyType.PT_AssetId)).assetsName_; }
	}

	public List<COM_Skill> SkillInsts
	{
		get { return skillInsts_; }
	}

    public COM_Skill GetSkillCore(int id)
    {
        for (int i = 0; i < skillInsts_.Count; ++i)
        {
            if(skillInsts_[i].skillID_ == (uint)id){
                return skillInsts_[i];
            }
        }
        if (suitSkill != null && (int)suitSkill.skillID_ == id)
            return suitSkill;
        return null;
    }

    List<int> _SkillIds = new List<int>();
    public int[] SkillIds
    {
        get
        {
            _SkillIds.Clear();
            for (int i = 0; i < skillInsts_.Count; ++i)
            {
                _SkillIds.Add((int)skillInsts_[i].skillID_);
            }
            return _SkillIds.ToArray();
        }
    }

	public COM_Item[] Equips
	{
		set { equips_ = value; }
		get { return equips_; }
	}

	public float[] Properties
	{
		get { return properties_; }
		set{properties_ = value;}
	}

    public float GetProperty(PropertyType type)
    {
        return properties_[(int)type];
    }

    public void Resize()
    {
        properties_ = new float[(int)PropertyType.PT_Max];
    }

	virtual public void SetIprop(COM_PropValue[] props)
	{
		for(int i=0; i < props.Length; ++i)
		{
            if (props[i].type_ == PropertyType.PT_Level)
            {
                isLevelUp_ = (int)props[i].value_ > GetIprop(PropertyType.PT_Level);
                
                if (isLevelUp_)
                    ClientLog.Instance.Log(" InstID: " + instId_ + " is Level Up" + "   Player ID is : " + GamePlayer.Instance.InstId);
            }
            else
                isLevelUp_ = false;
			properties_[(int)props[i].type_] = props[i].value_;
            if(isLevelUp_)
            {
                if (instId_ == GamePlayer.Instance.InstId)
                {
                    CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.LevelUp);
                }
                else if (GamePlayer.Instance.BattleBaby != null && GamePlayer.Instance.BattleBaby.InstId == instId_)
                {
                    if (GamePlayer.Instance.BabyLevelUpEvent != null)
                        GamePlayer.Instance.BabyLevelUpEvent(new int[] { instId_, (int)properties_[(int)props[i].type_] });
                }
            }
		}
        if (OnIPropUpdate != null)
            OnIPropUpdate();

        isPropDirty_ = true;
	}

	public void SetIprop(PropertyType type, int value)
	{
		properties_ [(int)type] = value;
        if (OnIPropUpdate != null)
            OnIPropUpdate();

        isPropDirty_ = true;
	}
	
	public int GetIprop(PropertyType type)
	{
		if(properties_ == null)
			return 0;

		return (int)properties_ [(int)type];
	}

	public string GetWeaponAction()
	{
		COM_Item weapon = equips_ [(int)EquipmentSlot.ES_SingleHand];
        if (weapon == null)
        {
            weapon = equips_[(int)EquipmentSlot.ES_DoubleHand];
            if (weapon == null)
            {
                return Enum.GetName(typeof(WeaponActionType), WeaponActionType.WAT_None);
            }
        }
		
		ItemData item = ItemData.GetData ((int)weapon.itemId_);
		return Enum.GetName (typeof(WeaponActionType), item.weaponActionType_);
	}

    public WeaponType GetWeaponType()
    {
        COM_Item weapon = equips_[(int)EquipmentSlot.ES_SingleHand];
        if (weapon == null)
        {
            weapon = equips_[(int)EquipmentSlot.ES_DoubleHand];
            if (weapon == null)
            {
                return WeaponType.WT_None;
            }
        }

        ItemData item = ItemData.GetData((int)weapon.itemId_);
        return item.weaponType_;
    }

	public int WeaponAssetID
	{
		get
		{
			COM_Item weapon = equips_ [(int)EquipmentSlot.ES_SingleHand];
            if (weapon == null)
            {
                weapon = equips_[(int)EquipmentSlot.ES_DoubleHand];
                if(weapon == null)
                    return 0;
            }
            return ItemData.GetData((int)weapon.itemId_).weaponEntityId_;
		}
	}

    public int WeaponID
    {
        get
        {
            COM_Item weapon = equips_[(int)EquipmentSlot.ES_SingleHand];
            if (weapon == null)
            {
                weapon = equips_[(int)EquipmentSlot.ES_DoubleHand];
                if (weapon == null)
                    return 0;
            }
            return (int)weapon.itemId_;
        }
    }

    public int DressID
    {
        get
        {
            COM_Item dress = equips_[(int)EquipmentSlot.ES_Fashion];
            if (dress == null)
                return 0;
            ItemData data = ItemData.GetData((int)dress.itemId_);
            if (data != null)
                return data.weaponEntityId_;
            return 0;
        }
    }

    public void wearEquip(COM_Item equip)
    {
		//if (Equips.Length > (int)EquipmentSlot.ES_Max)
        //{
          //  return;
        //}

        Equips[(int)equip.slot_] = equip;
        if (WearEquipEvent != null)
        {
            WearEquipEvent((uint)InstId, equip);
        }
        isEquipDirty_ = true;

        if (type_ == EntityType.ET_Baby)
            CalcBabyEquipSkill();
    }

    public void demontWeapon()
    {
        Equips[(int)EquipmentSlot.ES_SingleHand] = null;
        Equips[(int)EquipmentSlot.ES_DoubleHand] = null;
        if (DelEquipEvent != null)
        {
            DelEquipEvent((uint)InstId, (uint)EquipmentSlot.ES_SingleHand);
            DelEquipEvent((uint)InstId, (uint)EquipmentSlot.ES_DoubleHand);
        }
    }

    public bool rangeWeapon()
    {
        COM_Item weapon = equips_[(int)EquipmentSlot.ES_SingleHand];
        if (weapon == null)
        {
            weapon = equips_[(int)EquipmentSlot.ES_DoubleHand];
            if (weapon == null)
                return true;
        }
        ItemData item = ItemData.GetData((int)weapon.itemId_);
        bool range = item.weaponType_ == WeaponType.WT_Bow | item.weaponType_ == WeaponType.WT_Knife ;//| item.weaponType_ == WeaponType.WT_V;
        return range;
    }

    public COM_Item GetEquipByInstId(int instid)
    {
        for (int i = 0; i < equips_.Length; ++i)
        {
            if(equips_[i].instId_ == instid)
                return equips_[i];
        }
        return null;
    }

    public void CalcBabyEquipSkill()
    {
        suitSkill = null;
        Dictionary<int, COM_Skill> skillStatus = new Dictionary<int, COM_Skill>();
        COM_Skill skill = null;
        for (int i = 0; i < equips_.Length; ++i)
        {
            if (equips_[i] != null)
            {
                //skillid
                if (equips_[i].durability_ != 0)
                {
                    if (!skillStatus.ContainsKey((int)equips_[i].durability_))
                    {
                        skill = new COM_Skill();
                        skill.skillID_ = (uint)equips_[i].durability_;
                        skill.skillLevel_ = (uint)equips_[i].durabilityMax_;
                        skill.skillExp_ = 1;
                        skillStatus.Add(equips_[i].durability_, skill);
                    }
                    else
                        skillStatus[equips_[i].durability_].skillExp_++;
                }
            }
        }

        foreach (int skillId in skillStatus.Keys)
        {
            if (skillStatus[skillId].skillExp_ == 4)
            {
                suitSkill = skillStatus[skillId];
                suitSkill.skillLevel_ = skillStatus[skillId].skillLevel_ + 1;
                isHighestSuitSkill = true;
            }
            else if (skillStatus[skillId].skillExp_ == 3)
            {
                suitSkill = skillStatus[skillId];
                isHighestSuitSkill = false;
            }
        }
    }

    public void AddState(COM_State state)
    {
        buffList_.Add(state);
        //for (int i = 0; i < buffList_.Count; ++i)
        //{
        //    if (buffList_[i] == null)
        //    {
        //        buffList_[i] = state;
              //  if (OnStateUpdate != null)
                //    OnStateUpdate();

        //        break;
        //    }
        //}
    }

    public void RemoveState(uint id)
    {
        for (int i = 0; i < buffList_.Count; ++i)
        {
            if (buffList_[i].stateId_ == id)
            {
                buffList_.RemoveAt(i);
                if (OnStateUpdate != null)
                    OnStateUpdate();
                break;
            }
        }
    }

    public void UpdateState(COM_State state)
    {
        for (int i = 0; i < buffList_.Count; ++i)
        {
            if (buffList_[i].stateId_ == state.stateId_)
            {
                buffList_[i] = state;
               // if (OnStateUpdate != null)
                 //   OnStateUpdate();
                break;
            }
        }
    }

    public int GetSkillCost(int skillid)
    {
        SkillData skill = null;
        for (int i = 0; i < skillInsts_.Count; ++i)
        {
            if (skillInsts_[i].skillID_ == skillid)
            {
                skill = SkillData.GetData(skillid, (int)skillInsts_[i].skillLevel_);
                return skill._Cost_mana;
            }
        }
        return 0;
    }

    public COM_BattleEntityInformation RequestBattlePhoto()
    {
        COM_BattleEntityInformation bei = new COM_BattleEntityInformation();
        
        bei.type_ = type_;
        bei.instName_ = instName_;
        bei.instId_ = instId_;
        bei.tableId_ = GetIprop(PropertyType.PT_TableId);
        bei.assetId_ = GetIprop(PropertyType.PT_AssetId);
        bei.hpMax_ = GetIprop(PropertyType.PT_HpMax);
        bei.hpCrt_ = GetIprop(PropertyType.PT_HpCurr);
        bei.mpMax_ = GetIprop(PropertyType.PT_MpMax);
        bei.mpCrt_ = GetIprop(PropertyType.PT_MpCurr);
        bei.level_ = GetIprop(PropertyType.PT_Level);

        return bei;
    }
}


