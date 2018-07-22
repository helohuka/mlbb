using UnityEngine;
using System.Collections.Generic;

public class AtlasLoader {

    static AtlasLoader inst_ = null;
    static public AtlasLoader Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new AtlasLoader();
            return inst_;
        }
    }

    public enum AtlasStatus
    {
        AS_Loaded,
        AS_WaitLoad,
        AS_Loading,
    }

    public delegate void AtlasStatusHandler(string atlasName);
    public event AtlasStatusHandler OnAtlasLoaded;

    const float AliveTime = 10f;

    List<string> loadLst_;
    Dictionary<string, BundleInfo> atlasRefInMemory_;
    Dictionary<string, AtlasStatus> atlasStatusType_;
    Dictionary<string, int> loadingAtlasRef_;

    public AtlasLoader()
    {
        loadLst_ = new List<string>();
        atlasRefInMemory_ = new Dictionary<string, BundleInfo>();
        atlasStatusType_ = new Dictionary<string, AtlasStatus>();
        loadingAtlasRef_ = new Dictionary<string, int>();
    }

    public AtlasStatus[] LoadAtlas(string[] atlas)
    {
        AtlasStatus[] astu = new AtlasStatus[atlas.Length];
        for (int i = 0; i < atlas.Length; ++i)
        {
            if (loadLst_.Contains(atlas[i]))
            {
                astu[i] = AtlasStatus.AS_Loading;
                if (loadingAtlasRef_.ContainsKey(atlas[i]))
                    loadingAtlasRef_[atlas[i]] += 1;
                continue;
            }
            if (atlasRefInMemory_.ContainsKey(atlas[i]))
            {
                astu[i] = AtlasStatus.AS_Loaded;
                atlasRefInMemory_[atlas[i]].refCount_ += 1;
            }
            else
            {
                astu[i] = AtlasStatus.AS_WaitLoad;
                atlasRefInMemory_.Add(atlas[i], new BundleInfo());
                atlasStatusType_.Add(atlas[i], AtlasStatus.AS_WaitLoad);
                loadLst_.Add(atlas[i]);
                loadingAtlasRef_.Add(atlas[i], 1);
            }
        }
        return astu;
    }

    List<string> tDelet = new List<string>();
    public void Update()
    {
        for (int i = 0; i < loadLst_.Count; ++i)
        {
            if (atlasStatusType_[loadLst_[i]] == AtlasStatus.AS_WaitLoad)
            {
                AssetLoader.LoadAssetBundle(loadLst_[i], AssetLoader.EAssetType.ASSET_UI, AtlasLoaded, new ParamData(loadLst_[i]));
                atlasStatusType_[loadLst_[i]] = AtlasStatus.AS_Loading;
            }
        }

        tDelet.Clear();
        foreach (string atlasName in atlasRefInMemory_.Keys)
        {
            if (atlasRefInMemory_[atlasName].refCount_ <= 0)
            {
                atlasRefInMemory_[atlasName].leftTime_ -= Time.deltaTime;
                if (atlasRefInMemory_[atlasName].bundle_ != null)
                {
                    if (atlasRefInMemory_[atlasName].leftTime_ < 0f)
                    {
                        tDelet.Add(atlasName);
                    }
                }
            }
        }

        for (int i = 0; i < tDelet.Count; ++i)
        {
            atlasRefInMemory_[tDelet[i]].bundle_.Unload(false);
            atlasRefInMemory_.Remove(tDelet[i]);
            atlasStatusType_.Remove(tDelet[i]);
            //ClientLog.Instance.Log("One Atlas has been destroyed. " + tDelet[i]);
        }
    }

    public void DeleteAtlas(string[] atlas)
    {
        if (atlas == null)
            return;

        for (int i = 0; i < atlas.Length; ++i)
        {
            if (atlasRefInMemory_.ContainsKey(atlas[i]))
                atlasRefInMemory_[atlas[i]].refCount_ -= 1;
        }
    }

    //这里异步删除 用索引会不会有问题?
    void AtlasLoaded(AssetBundle bundle, ParamData data)
    {
        int originRef = loadingAtlasRef_.ContainsKey(data.szParam) ? loadingAtlasRef_[data.szParam] : 1;
        atlasRefInMemory_[data.szParam].bundle_ = bundle;
        atlasRefInMemory_[data.szParam].refCount_ = originRef;
        atlasRefInMemory_[data.szParam].leftTime_ = AliveTime;
        atlasStatusType_[data.szParam] = AtlasStatus.AS_Loaded;
        loadLst_.Remove(data.szParam);
        loadingAtlasRef_.Remove(data.szParam);
        if (OnAtlasLoaded != null)
            OnAtlasLoaded(data.szParam);
    }

    class BundleInfo
    {
        public int refCount_;
        public float leftTime_;
        public AssetBundle bundle_;
    }
}
enum APPLE_b289f629d8fc4d5195494f8f1cae818f
{

}