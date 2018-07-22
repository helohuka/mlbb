using UnityEngine;
using System.Collections;

public class WeaponAssetMgr {

    static public bool LoadAsset(ENTITY_ID ID, AssetLoader.AssetLoadCallback CallBack, ParamData paramData)
    {
        // id to name
        string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName((int)ID, AssetLoader.EAssetType.ASSET_WEAPON);
        if (!GlobalInstanceFunction.Instance.IsValidName(AssetsName))
        {
            return false;
        }
        //Load Asset
        AssetLoader.LoadAssetBundle(AssetsName,
                                    AssetLoader.EAssetType.ASSET_WEAPON,
                                    CallBack,
                                    paramData);
        return true;
    }

    static public bool LoadAsset(ENTITY_ID dressId, ENTITY_ID weaponId, AssetLoader.AssetLoadCallback CallBack, ParamData paramData)
    {
        // id to name
        string weaponName = GlobalInstanceFunction.Instance.GetAssetsName((int)weaponId, AssetLoader.EAssetType.ASSET_WEAPON);
        if (!GlobalInstanceFunction.Instance.IsValidName(weaponName))
        {
            return false;
        }
        string dressName = "";
        string assName = "";
        if ((int)dressId != 0)
        {
            dressName = GlobalInstanceFunction.Instance.GetAssetsName((int)dressId, AssetLoader.EAssetType.ASSET_PLAYER);
            //去掉性别
            dressName = dressName.Remove(dressName.Length - 1, 1);
            assName = string.Format("{0}_{1}", dressName, weaponName);
        }
        else
            assName = weaponName;
        
        //Load Asset
        AssetLoader.LoadAssetBundle(assName,
                                    AssetLoader.EAssetType.ASSET_WEAPON,
                                    CallBack,
                                    paramData);
        return true;
    }

    static public void DeleteAsset(ENTITY_ID ID, bool unLoadAllLoadedObjects)
    {// id to name
        string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName((int)ID, AssetLoader.EAssetType.ASSET_WEAPON);
        AssetInfoMgr.Instance.DecRefCount(AssetsName, unLoadAllLoadedObjects);
    }

    static public void DeleteAsset(AssetBundle ToDelete, bool unLoadAllLoadedObjects)
    {
        AssetInfoMgr.Instance.DecRefCount(ToDelete, unLoadAllLoadedObjects);
    }
}
enum APPLE_2c861db956b54d9496d6268095d42246
{

}