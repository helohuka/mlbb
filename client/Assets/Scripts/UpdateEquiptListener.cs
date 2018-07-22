using UnityEngine;
using System.Collections;

public class UpdateEquiptListener : MonoBehaviour {

    public GameObject weapon_;
    public ParamData data_;
    public ENTITY_ID weaponAssId_;
    public int dressAssId_;
    public string layerName_;

    Transform bindPoint_;

    bool hasDestroy = false;
    public void SetWeapon(GameObject weapon, ParamData data, string layerName = "Default")
    {
        hasDestroy = false;
        weapon_ = weapon;
        data_ = data;
        layerName_ = layerName;
    }

    public void RemoveWeaponHandler(uint target, uint slot)
    {
        if (data_ != null && target != data_.iParam)
            return;

		if((EquipmentSlot)slot == EquipmentSlot.ES_DoubleHand)
		{
			if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
			{
				return;
			}
			else
			{
				if (weapon_ != null)
				{
					Destroy(weapon_);
					weapon_ = null;
				}
			}
		}
		else
		{
			if ((EquipmentSlot)slot == EquipmentSlot.ES_SingleHand ||
			    (EquipmentSlot)slot == EquipmentSlot.ES_DoubleHand)
			{
				if (weapon_ != null)
				{
					Destroy(weapon_);
					weapon_ = null;
				}
			}
		}
	
    }

    public void RemoveWeaponDirectly(int target)
    {
        if (data_ != null && target != data_.iParam)
            return;

        if (weapon_ != null)
        {
            Destroy(weapon_);
            weapon_ = null;
        }
    }

    public void UpdateHandler(ENTITY_ID weaponAssetId)
    {
		if((int)weaponAssetId == 0)
			return;

        weaponAssId_ = weaponAssetId;
        WeaponAssetMgr.LoadAsset((ENTITY_ID)dressAssId_, weaponAssId_, (AssetBundle bundle, ParamData data) =>
        {
            if (hasDestroy)
            {
                WeaponAssetMgr.DeleteAsset(bundle, false);
                return;
            }
            if (weapon_ != null)
            {
                Destroy(weapon_);
                weapon_ = null;
            }
            if(EntityAssetsData.GetData((int)weaponAssetId).bindPoint_.Contains("L"))
                bindPoint_ = gameObject.GetComponent<WeaponHand>().weaponLeftHand_;
            else
                bindPoint_ = gameObject.GetComponent<WeaponHand>().weaponRightHand_;
           
            weapon_ = (GameObject)GameObject.Instantiate(bundle.mainAsset, bindPoint_.position, bindPoint_.rotation) as GameObject;
			WeaponAssetMgr.DeleteAsset(bundle, false);
            weapon_.transform.parent = bindPoint_;
            //weapon_.transform.localScale = Vector3.one;
            NGUITools.SetChildLayer(transform, LayerMask.NameToLayer(layerName_));

        }, null);
    }

    public void UpdateHandler(uint target, COM_Item equip)
    {
        if (data_ != null && target != data_.iParam)
            return;

        if(equip.slot_ != (ushort)EquipmentSlot.ES_Fashion)
        {
            ENTITY_ID weaponId = (ENTITY_ID)ItemData.GetData((int)equip.itemId_).weaponEntityId_;
            UpdateHandler(weaponId);
        }
    }

    public void UpdateHandler(int target, int itemid)
    {
        if (data_ != null && target != data_.iParam)
            return;

        if (itemid == 0)
        {
            RemoveWeaponDirectly(target);
            return;
        }

        ItemData item = ItemData.GetData(itemid);
        if (item != null && item.slot_ != EquipmentSlot.ES_Fashion)
        {
            ENTITY_ID weaponId = (ENTITY_ID)ItemData.GetData(itemid).weaponEntityId_;
            UpdateHandler(weaponId);
        }
    }

    public void RemoveHandler()
    {
        GamePlayer.Instance.WearEquipEvent -= UpdateHandler;
        GamePlayer.Instance.DelEquipEvent -= RemoveWeaponHandler;
    }

    void OnDestroy()
    {
        hasDestroy = true;
        GamePlayer.Instance.WearEquipEvent -= UpdateHandler;
        GamePlayer.Instance.DelEquipEvent -= RemoveWeaponHandler;
    }
}
