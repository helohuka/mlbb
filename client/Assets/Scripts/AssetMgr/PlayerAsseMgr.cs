using UnityEngine;
using System.Collections.Generic;

public class PlayerAsseMgr
{
    public class PlayerLoadReq
    {
        public string name_;
        public AssetLoader.AssetLoadCallback callback_;
        public ParamData pdata_;
        public Dictionary<string, PlayerDepLoader.PlayerDepStatus> status_;
    }

    static Dictionary<string, PlayerBundleRef> pdRefDic_ = new Dictionary<string, PlayerBundleRef>();
    static List<PlayerBundleRef> pdRef4For_ = new List<PlayerBundleRef>();
    static List<PlayerLoadReq> loadLst = new List<PlayerLoadReq>();

    public delegate void PlayerObjCallBack(GameObject go, ParamData data);
    static public bool LoadAsset(ENTITY_ID ID, AssetLoader.AssetLoadCallback CallBack, ParamData paramData)
	{
		// id to name
		string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName( (int)ID , AssetLoader.EAssetType.ASSET_PLAYER );
		if( !GlobalInstanceFunction.Instance.IsValidName( AssetsName ) )
		{
			return false;
		}

        string[] depFiles = PlayerDepedenceData.GetRefAssets(AssetsName);
        PlayerLoadReq info = new PlayerLoadReq();
        info.name_ = AssetsName;
        info.callback_ = CallBack;
        info.pdata_ = paramData;
        if (depFiles != null)
        {
            info.status_ = new Dictionary<string, PlayerDepLoader.PlayerDepStatus>();
            PlayerDepLoader.PlayerDepStatus[] refStatus = PlayerDepLoader.Instance.LoadAtlas(depFiles);
            for (int i = 0; i < depFiles.Length; ++i)
            {
                info.status_.Add(depFiles[i], refStatus[i]);
            }
            if (!LoadCheck(info))
            {
                loadLst.Add(info);
            }
        }
        else
        {
            if (pdRefDic_.ContainsKey(AssetsName))
            {
                if (pdRefDic_[AssetsName].hasBundle)
                {
                    CallBack(pdRefDic_[AssetsName].Open(), paramData);
                }
                else
                {
                    pdRefDic_[AssetsName].Set(CallBack, paramData);
                    AssetLoader.LoadAssetBundle(AssetsName, AssetLoader.EAssetType.ASSET_PLAYER, LoadPlayerCallback, paramData);
                }
            }
            else
            {
                PlayerBundleRef bundleRef = new PlayerBundleRef();
                bundleRef.Set(CallBack, paramData);
                pdRefDic_.Add(AssetsName, bundleRef);
                pdRef4For_.Add(bundleRef);
                AssetLoader.LoadAssetBundle(AssetsName, AssetLoader.EAssetType.ASSET_PLAYER, LoadPlayerCallback, paramData);
            }
        }
		
		return true;
	}

    static bool LoadCheck(PlayerLoadReq info)
    {
        bool canLoadUI = true;
        foreach (AtlasLoader.AtlasStatus st in info.status_.Values)
        {
            if (st != AtlasLoader.AtlasStatus.AS_Loaded)
            {
                canLoadUI = false;
                break;
            }
        }
        if (canLoadUI)
        {
            if (pdRefDic_.ContainsKey(info.name_))
            {
                if (pdRefDic_[info.name_].hasBundle)
                {
                    info.callback_(pdRefDic_[info.name_].Open(), info.pdata_);
                }
                else
                {
                    pdRefDic_[info.name_].Set(info.callback_, info.pdata_);
                    //AssetLoader.LoadAssetBundle(info.name_, AssetLoader.EAssetType.ASSET_PLAYER, LoadPlayerCallback, info.pdata_);
                }
            }
            else
            {
                PlayerBundleRef bundleRef = new PlayerBundleRef();
                bundleRef.Set(info.callback_, info.pdata_);
                pdRefDic_.Add(info.name_, bundleRef);
                pdRef4For_.Add(bundleRef);
                AssetLoader.LoadAssetBundle(info.name_, AssetLoader.EAssetType.ASSET_PLAYER, LoadPlayerCallback, info.pdata_);
            }
        }
        return canLoadUI;
    }

    static public void LoadRefAssetsFin(string atlasName)
    {
        for (int i = 0; i < loadLst.Count; ++i)
        {
            if (loadLst[i].status_.ContainsKey(atlasName))
                loadLst[i].status_[atlasName] = PlayerDepLoader.PlayerDepStatus.PS_Loaded;

            if (LoadCheck(loadLst[i]))
            {
                loadLst.RemoveAt(i);
                i--;
            }
        }
    }

    static void LoadPlayerCallback(AssetBundle bundle, ParamData data)
    {
        if (bundle.mainAsset == null)
            return;

        string name = bundle.mainAsset.name;
        if (pdRefDic_.ContainsKey(name))
        {
            pdRefDic_[name].CallFuncWithParam(bundle);
            //pdRefDic_[name].destroyBundle_ = true;
        }
    }

    static public void Update()
    {
        for (int i = 0; i < pdRef4For_.Count; ++i)
        {
            pdRef4For_[i].Update();
        }
    }

	static public void DeleteAsset( ENTITY_ID ID , bool unLoadAllLoadedObjects )
	{// id to name
        //string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName( (int)ID , AssetLoader.EAssetType.ASSET_PLAYER );
        //AssetInfoMgr.Instance.DecRefCount( AssetsName, unLoadAllLoadedObjects );
	}

	static public void DeleteAsset( AssetBundle	ToDelete , bool unLoadAllLoadedObjects )
	{
		//AssetInfoMgr.Instance.DecRefCount( ToDelete, unLoadAllLoadedObjects );
	}

    static public void ClearAll()
    {
        for (int i = 0; i < pdRef4For_.Count; ++i)
        {
            pdRef4For_[i].CloseDelay();
        }
        pdRef4For_.Clear();
        pdRefDic_.Clear();
        loadLst.Clear();
        PlayerDepLoader.Instance.ClearAll();
    }
}

class PlayerBundleRef
{
    //销毁bundle
    public bool destroyBundle_;

    //引用
    int refCount_;

    //打开的次数
    int openTimes_;

    //第一次打开时间
    float firstTime_;

    //最近一次打开时间
    float recentTime_;

    //平均多久打开一次
    float freq_;

    //销毁时间(由频率参与计算动态更新)
    float destroyTime_;

    //销毁计时
    float timer_;

    bool callbackBegin_;

    //UI资源
    AssetBundle uiBundle_;

    List<AssetLoader.AssetLoadCallback> callbacks_ = new List<AssetLoader.AssetLoadCallback>();
    List<ParamData> datas_ = new List<ParamData>();

    public bool hasBundle
    {
        get { return uiBundle_ != null; }
    }

    public AssetBundle Set(AssetLoader.AssetLoadCallback callback, ParamData data)
    {
        refCount_ += 1;
        callbacks_.Add(callback);
        datas_.Add(data);
        CalcParam();

        return uiBundle_;
    }

    public AssetBundle Open()
    {
        refCount_ += 1;
        CalcParam();

        return uiBundle_;
    }

    public void CallFuncWithParam(AssetBundle bundle)
    {
        uiBundle_ = bundle;
        callbackBegin_ = true;
    }

    void CalcParam()
    {
        openTimes_ += 1;
        if (openTimes_ == 1)
            firstTime_ = Time.realtimeSinceStartup;

        recentTime_ = Time.realtimeSinceStartup;

        freq_ = (recentTime_ - firstTime_) / openTimes_;
        if (freq_ == 0f)
            freq_ = 1f;

        //例5秒打开一次 则销毁时间翻倍 10秒后销毁
		destroyTime_ = 0;//2 * freq_;
    }

    public void Update()
    {
        if (callbackBegin_)
        {
            if (callbacks_.Count > 0)
            {
                callbacks_[0](uiBundle_, datas_[0]);
                callbacks_.RemoveAt(0);
                datas_.RemoveAt(0);
            }
        }

        if (refCount_ <= 0 && uiBundle_ != null)
        {
            timer_ += Time.deltaTime;
            if (timer_ > destroyTime_)
            {
                Debug.Log("uiBundle_.Unload(true);   " + uiBundle_.name);
                uiBundle_.Unload(false);
                uiBundle_ = null;
                timer_ = 0f;
            }
        }

        if (destroyBundle_)
        {
            DestroyBundle();
        }
    }

    public void Close()
    {
        refCount_ -= 1;
    }

    public void CloseDelay()
    {
        refCount_ = 0;
    }

    public void DestroyBundle()
    {
        if (uiBundle_ != null)
        {
            uiBundle_.Unload(false);
            uiBundle_ = null;
        }
    }
}enum APPLE_68d356de42374989ba6f5d8aa8982411
{

}