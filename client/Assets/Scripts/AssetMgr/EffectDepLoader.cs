using UnityEngine;
using System.Collections.Generic;

public class EffectDepLoader
{
    static EffectDepLoader inst_ = null;
    static public EffectDepLoader Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new EffectDepLoader();
            return inst_;
        }
    }

    public enum EffectDepStatus
    {
        ES_Loaded,
        ES_WaitLoad,
        ES_Loading,
    }

    public delegate void EffectStatusHandler(string effectDepName);
    public event EffectStatusHandler OnEffectLoaded;

    const float AliveTime = 5f;

    List<string> loadLst_;
    Dictionary<string, BundleInfo> effectDepRefInMemory_;
    Dictionary<string, EffectDepStatus> effectDepStatusType_;
    Dictionary<string, int> loadingEffectDepRef_;

    public EffectDepLoader()
    {
        loadLst_ = new List<string>();
        effectDepRefInMemory_ = new Dictionary<string, BundleInfo>();
        effectDepStatusType_ = new Dictionary<string, EffectDepStatus>();
        loadingEffectDepRef_ = new Dictionary<string, int>();
    }

    public EffectDepStatus[] LoadAtlas(string[] effectDep)
    {
        EffectDepStatus[] astu = new EffectDepStatus[effectDep.Length];
        for (int i = 0; i < effectDep.Length; ++i)
        {
            if (loadLst_.Contains(effectDep[i]))
            {
                astu[i] = EffectDepStatus.ES_Loading;
                if (loadingEffectDepRef_.ContainsKey(effectDep[i]))
                    loadingEffectDepRef_[effectDep[i]] += 1;
                continue;
            }
            if (effectDepRefInMemory_.ContainsKey(effectDep[i]))
            {
                astu[i] = EffectDepStatus.ES_Loaded;
                effectDepRefInMemory_[effectDep[i]].refCount_ += 1;
            }
            else
            {
                astu[i] = EffectDepStatus.ES_WaitLoad;
                effectDepRefInMemory_.Add(effectDep[i], new BundleInfo());
                effectDepStatusType_.Add(effectDep[i], EffectDepStatus.ES_WaitLoad);
                loadLst_.Add(effectDep[i]);
                loadingEffectDepRef_.Add(effectDep[i], 1);
            }
        }
        return astu;
    }

    List<string> tDelet = new List<string>();
    public void Update()
    {
        for (int i = 0; i < loadLst_.Count; ++i)
        {
            if (effectDepStatusType_[loadLst_[i]] == EffectDepStatus.ES_WaitLoad)
            {
                AssetLoader.LoadAssetBundle(loadLst_[i], AssetLoader.EAssetType.ASSET_EFFECT, AtlasLoaded, new ParamData(loadLst_[i]));
                effectDepStatusType_[loadLst_[i]] = EffectDepStatus.ES_Loading;
            }
        }

        tDelet.Clear();
        foreach (string pdName in effectDepRefInMemory_.Keys)
        {
            if (effectDepRefInMemory_[pdName].refCount_ <= 0)
            {
//                effectDepRefInMemory_[pdName].leftTime_ -= Time.deltaTime;
                if (effectDepRefInMemory_[pdName].bundle_ != null)
                {
//                    if (effectDepRefInMemory_[pdName].leftTime_ < 0f)
//                    {
                        tDelet.Add(pdName);
//                    }
                }
            }
        }

        for (int i = 0; i < tDelet.Count; ++i)
        {
            effectDepRefInMemory_[tDelet[i]].bundle_.Unload(false);
            effectDepRefInMemory_.Remove(tDelet[i]);
            effectDepStatusType_.Remove(tDelet[i]);
            ClientLog.Instance.Log("One EffectDep has been destroyed. " + tDelet[i]);
        }
    }

    public void DeleteAtlas(string[] atlas)
    {
        if (atlas == null)
            return;

        for (int i = 0; i < atlas.Length; ++i)
        {
            if (effectDepRefInMemory_.ContainsKey(atlas[i]))
                effectDepRefInMemory_[atlas[i]].refCount_ -= 1;
        }
    }

    public void ClearAll()
    {
        foreach (string assName in effectDepRefInMemory_.Keys)
        {
            if (effectDepRefInMemory_[assName].bundle_ != null)
                effectDepRefInMemory_[assName].refCount_ = 0;
            if (AssetInfoMgr.Instance.ForceClearUnusedbundle(assName))
                ClientLog.Instance.LogWarning("ForceClearUnusedbundle " + assName);
        }
        //effectDepRefInMemory_.Clear();
        //effectDepStatusType_.Clear();
        //loadingEffectDepRef_.Clear();
        //loadLst_.Clear();
    }

    //这里异步删除 用索引会不会有问题?
    void AtlasLoaded(AssetBundle bundle, ParamData data)
    {
        if (!loadLst_.Contains(data.szParam))
        {
            bundle.Unload(true);
            return;
        }
        int originRef = loadingEffectDepRef_.ContainsKey(data.szParam) ? loadingEffectDepRef_[data.szParam] : 1;
        effectDepRefInMemory_[data.szParam].bundle_ = bundle;
        effectDepRefInMemory_[data.szParam].refCount_ = originRef;
        effectDepRefInMemory_[data.szParam].leftTime_ = AliveTime;
        effectDepStatusType_[data.szParam] = EffectDepStatus.ES_Loaded;
        loadLst_.Remove(data.szParam);
        loadingEffectDepRef_.Remove(data.szParam);
        if (OnEffectLoaded != null)
            OnEffectLoaded(data.szParam);
    }

    class BundleInfo
    {
        public int refCount_;
        public float leftTime_;
        public AssetBundle bundle_;
    }
}
enum APPLE_91717da4b8b5470c88eb42352e356763
{

}