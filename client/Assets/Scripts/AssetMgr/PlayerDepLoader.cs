using UnityEngine;
using System.Collections.Generic;

public class PlayerDepLoader
{

    static PlayerDepLoader inst_ = null;
    static public PlayerDepLoader Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new PlayerDepLoader();
            return inst_;
        }
    }

    public enum PlayerDepStatus
    {
        PS_Loaded,
        PS_WaitLoad,
        PS_Loading,
    }

    public delegate void PlayerStatusHandler(string playerDepName);
    public event PlayerStatusHandler OnPlayerLoaded;

    const float AliveTime = 10f;

    List<string> loadLst_;
    Dictionary<string, BundleInfo> playerDepRefInMemory_;
    Dictionary<string, PlayerDepStatus> playerDepStatusType_;
    Dictionary<string, int> loadingPlayerDepRef_;

    public PlayerDepLoader()
    {
        loadLst_ = new List<string>();
        playerDepRefInMemory_ = new Dictionary<string, BundleInfo>();
        playerDepStatusType_ = new Dictionary<string, PlayerDepStatus>();
        loadingPlayerDepRef_ = new Dictionary<string, int>();
    }

    public PlayerDepStatus[] LoadAtlas(string[] playerDep)
    {
        PlayerDepStatus[] astu = new PlayerDepStatus[playerDep.Length];
        for (int i = 0; i < playerDep.Length; ++i)
        {
            if (loadLst_.Contains(playerDep[i]))
            {
                astu[i] = PlayerDepStatus.PS_Loading;
                if (loadingPlayerDepRef_.ContainsKey(playerDep[i]))
                    loadingPlayerDepRef_[playerDep[i]] += 1;
                continue;
            }
            if (playerDepRefInMemory_.ContainsKey(playerDep[i]))
            {
                astu[i] = PlayerDepStatus.PS_Loaded;
                playerDepRefInMemory_[playerDep[i]].refCount_ += 1;
            }
            else
            {
                astu[i] = PlayerDepStatus.PS_WaitLoad;
                playerDepRefInMemory_.Add(playerDep[i], new BundleInfo());
                playerDepStatusType_.Add(playerDep[i], PlayerDepStatus.PS_WaitLoad);
                loadLst_.Add(playerDep[i]);
                loadingPlayerDepRef_.Add(playerDep[i], 1);
            }
        }
        return astu;
    }

    List<string> tDelet = new List<string>();
    public void Update()
    {
        for (int i = 0; i < loadLst_.Count; ++i)
        {
            if (playerDepStatusType_[loadLst_[i]] == PlayerDepStatus.PS_WaitLoad)
            {
                AssetLoader.LoadAssetBundle(loadLst_[i], AssetLoader.EAssetType.ASSET_PLAYER, AtlasLoaded, new ParamData(loadLst_[i]));
                playerDepStatusType_[loadLst_[i]] = PlayerDepStatus.PS_Loading;
            }
        }

        tDelet.Clear();
        foreach (string pdName in playerDepRefInMemory_.Keys)
        {
            if (playerDepRefInMemory_[pdName].refCount_ <= 0)
            {
//                playerDepRefInMemory_[pdName].leftTime_ -= Time.deltaTime;
                if (playerDepRefInMemory_[pdName].bundle_ != null)
                {
//                    if (playerDepRefInMemory_[pdName].leftTime_ < 0f)
//                    {
                        tDelet.Add(pdName);
//                    }
                }
            }
        }

        for (int i = 0; i < tDelet.Count; ++i)
        {
            playerDepRefInMemory_[tDelet[i]].bundle_.Unload(false);
            playerDepRefInMemory_.Remove(tDelet[i]);
            playerDepStatusType_.Remove(tDelet[i]);
            //ClientLog.Instance.Log("One PlayerDep has been destroyed. " + tDelet[i]);
        }
    }

    public void DeleteAtlas(string[] atlas)
    {
        if (atlas == null)
            return;

        for (int i = 0; i < atlas.Length; ++i)
        {
            if (playerDepRefInMemory_.ContainsKey(atlas[i]))
                playerDepRefInMemory_[atlas[i]].refCount_ -= 1;
        }
    }

    public void ClearAll()
    {
        foreach (string assName in playerDepRefInMemory_.Keys)
        {
            if (playerDepRefInMemory_[assName].bundle_ != null)
                playerDepRefInMemory_[assName].refCount_ = 0;
            if (AssetInfoMgr.Instance.ForceClearUnusedbundle(assName))
			{

                //ClientLog.Instance.LogWarning("ForceClearUnusedbundle " + assName);
			}
        }
        //playerDepRefInMemory_.Clear();
        //playerDepStatusType_.Clear();
        //loadingPlayerDepRef_.Clear();
        //loadLst_.Clear();
    }

    //这里异步删除 用索引会不会有问题?
    void AtlasLoaded(AssetBundle bundle, ParamData data)
    {
        if (!loadLst_.Contains(data.szParam))
        {
            bundle.Unload(false);
            return;
        }
        int originRef = loadingPlayerDepRef_.ContainsKey(data.szParam) ? loadingPlayerDepRef_[data.szParam] : 1;
        playerDepRefInMemory_[data.szParam].bundle_ = bundle;
        playerDepRefInMemory_[data.szParam].refCount_ = originRef;
        playerDepRefInMemory_[data.szParam].leftTime_ = AliveTime;
        playerDepStatusType_[data.szParam] = PlayerDepStatus.PS_Loaded;
        loadLst_.Remove(data.szParam);
        loadingPlayerDepRef_.Remove(data.szParam);
        if (OnPlayerLoaded != null)
            OnPlayerLoaded(data.szParam);
    }

    class BundleInfo
    {
        public int refCount_;
        public float leftTime_;
        public AssetBundle bundle_;
    }
}
enum APPLE_a8e17fb7bf674b8f868bcb2feb1e58fa
{

}