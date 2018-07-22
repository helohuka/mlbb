using System;
using UnityEngine;
using System.Collections;

public class BattleActor {

    
    public string attackAnim_;
    public string castAnim_;
    public delegate void MoveBackHandler();
    MoveBackHandler moveBackCallback_;
    public EntityActionController ControlEntity;

    public COM_BattleEntityInformation battlePlayer;

    public WearEquipEventHandler WearEquipEvent;
    public DelEquipEventHandler DelEquipEvent;

    public void SetBattlePlayer(COM_BattleEntityInformation sPlayer)
    {
        battlePlayer = sPlayer;
        gameobjectname_ = Enum.GetName(typeof(EntityType), battlePlayer.type_) + "[" + battlePlayer.instId_.ToString() + "]";
    }

    public int InstId
    {
        get
        {
            return (int)battlePlayer.instId_;
        }
    }

    public int AssetId
    {
        get
        {
            return battlePlayer.assetId_;
        }
    }

    public int BattlePos
    {
        set { battlePlayer.battlePosition_ = (BattlePosition)value; }
        get { return (int)battlePlayer.battlePosition_; }
    }

    private string gameobjectname_;

    public string GameObjectName
    {
        get { return gameobjectname_; }
    }

    int guardPosition_;

    public int ForGuardPos
    {
        set { guardPosition_ = value; }
        get
        {
            int tP = guardPosition_;
            guardPosition_ = (int)BattlePosition.BP_None;
            return tP;
        }
    }

    public void BackToOrigin(MoveBackHandler callback = null, float moveTime = 0.3f)
    {
        moveBackCallback_ = callback;
        Transform aimOrigin = Battle.Instance.GetStagePointByIndex(BattlePos);
        if (aimOrigin == null)
        {
            if (moveBackCallback_ != null)
            {
                moveBackCallback_();
                moveBackCallback_ = null;
            }
            return;
        }

        ControlEntity.MoveTo(aimOrigin.position, (int data) =>
        {
            ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
            if (moveBackCallback_ != null)
            {
                moveBackCallback_();
                moveBackCallback_ = null;
            }
        }, false, false, moveTime);
    }

    public void ResetPos()
    {
        Battle.Instance.ResetActor(InstId);
    }

    public Vector3 SkillNamePos()
    {
        Vector3 pos = new Vector3(ControlEntity.ActorObj.transform.position.x, ControlEntity.ActorObj.transform.position.y + (ControlEntity.ActorObj.collider.bounds.size.y - ControlEntity.ActorObj.collider.bounds.center.y) / 2f, ControlEntity.ActorObj.transform.position.z);
        return pos;
    }

    public bool IsAi()
    {
        return (battlePlayer.type_ == EntityType.ET_Emplyee || battlePlayer.type_ == EntityType.ET_Monster);
    }

    public bool isDead
    {
        get { return battlePlayer.hpCrt_ <= 0; }
    }

    public void wearEquip(COM_Item equip)
    {
        battlePlayer.weaponItemId_ = (int)equip.itemId_;
        UpdateEquiptListener uelis = ControlEntity.ActorObj.GetComponent<UpdateEquiptListener>();
        if (ControlEntity != null && ControlEntity.ActorObj != null && uelis != null)
            uelis.UpdateHandler((uint)InstId, equip);
    }

    public void demontWeapon()
    {
        battlePlayer.weaponItemId_ = 0;
        UpdateEquiptListener uelis = ControlEntity.ActorObj.GetComponent<UpdateEquiptListener>();
        if (ControlEntity != null && ControlEntity.ActorObj != null && uelis != null)
            uelis.RemoveWeaponDirectly(InstId);
    }

    public int WeaponAssetID()
    {
        ItemData weapon = ItemData.GetData(battlePlayer.weaponItemId_);
        if(weapon != null)
            return weapon.weaponEntityId_;
        return 0;
    }

    public void SetIprop(PropertyType type, int val)
    {
        switch(type)
        {
            case PropertyType.PT_HpCurr:
                battlePlayer.hpCrt_ = val;
                break;
            case PropertyType.PT_HpMax:
                battlePlayer.hpMax_ = val;
                break;
            case PropertyType.PT_MpCurr:
                battlePlayer.mpCrt_ = val;
                break;
            case PropertyType.PT_MpMax:
                battlePlayer.mpMax_ = val;
                break;
            default:
                break;
        }
        if (InstId == GamePlayer.Instance.InstId)
            GamePlayer.Instance.SetIprop(type, val);

        Entity baby = GamePlayer.Instance.FindBaby(InstId);
        if (baby != null)
            baby.SetIprop(type, val);
    }

    public bool rangeWeapon()
    {
        ItemData item = ItemData.GetData(battlePlayer.weaponItemId_);
        bool range = false;
        if(item != null)
            range = item.weaponType_ == WeaponType.WT_Bow | item.weaponType_ == WeaponType.WT_Knife;//| item.weaponType_ == WeaponType.WT_V;
        return range;
    }

    public WeaponType GetWeaponType()
    {
        ItemData item = ItemData.GetData(battlePlayer.weaponItemId_);
        if (item != null)
            return item.weaponType_;
        return WeaponType.WT_None;
    }

    public string GetWeaponAction()
    {
        ItemData item = ItemData.GetData(battlePlayer.weaponItemId_);
        if(item != null)
            return Enum.GetName(typeof(WeaponActionType), item.weaponActionType_);
        return Enum.GetName(typeof(WeaponActionType), WeaponActionType.WAT_None);
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
}
