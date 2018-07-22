using UnityEngine;
using System.Collections.Generic;

public class UIAssetMgr 
{
    class LoadInfo
    {
        public string name_;
        public AssetLoader.AssetLoadCallback callback_;
        public ParamData pdata_;
        public Dictionary<string, AtlasLoader.AtlasStatus> status_;
    }

    static Dictionary<string, UIBundleRef> uiRefDic_ = new Dictionary<string, UIBundleRef>();
    static List<UIBundleRef> uiRef4For_ = new List<UIBundleRef>();
    static List<LoadInfo> loadLst = new List<LoadInfo>();
	public static bool LoadUI( UIASSETS_ID ID ,AssetLoader.AssetLoadCallback CallBack,ParamData paramData)
	{
		string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName( (int)ID , AssetLoader.EAssetType.ASSET_UI );
		if( !GlobalInstanceFunction.Instance.IsValidName(AssetsName) )
		{
			return false;
		}

        LoadUI(AssetsName, CallBack, paramData);
		return true;
	}


	public static bool LoadUI( string assetsName ,AssetLoader.AssetLoadCallback CallBack,ParamData paramData)
	{
        LoadInfo info = new LoadInfo();
        info.name_ = assetsName;
        info.callback_ = CallBack;
        info.pdata_ = paramData;
        string[] refAtlas = UIDepedenceData.GetRefAtlas(assetsName);
        if (refAtlas == null)
        {
            //因为MessageBoxPanel界面打开时，处于混沌伊始状态(无数据，无网络) 所以在此写死其所需的图集 囧
            if (assetsName.Equals("MessageBoxPanel") || assetsName.Equals("ItemCell") || assetsName.Equals("BabyCell") || assetsName.Equals("StateCell") || assetsName.Equals("SkillCell"))
                refAtlas = new string[] { "CommomAtlas" };
            if(assetsName.Equals("Notice"))
                refAtlas = new string[] { "noticeAtlas" };
            if (assetsName.Equals("LoginPanel"))
                refAtlas = new string[] { "NewLoginAtlas", "CommomAtlas" };
        }

        if (refAtlas != null)
        {
            info.status_ = new Dictionary<string, AtlasLoader.AtlasStatus>();
            AtlasLoader.AtlasStatus[] refStatus = AtlasLoader.Instance.LoadAtlas(refAtlas);
            for (int i = 0; i < refAtlas.Length; ++i)
            {
                info.status_.Add(refAtlas[i], refStatus[i]);
            }
            if (!LoadCheck(info))
            {
                loadLst.Add(info);
            }
        }
        else
        {
            if (uiRefDic_.ContainsKey(assetsName))
            {
                if (uiRefDic_[assetsName].hasBundle)
                {
                    CallBack(uiRefDic_[assetsName].Open(), paramData);
                }
                else
                {
                    uiRefDic_[assetsName].Open(CallBack);
                    AssetLoader.LoadAssetBundle(assetsName, AssetLoader.EAssetType.ASSET_UI, LoadUICallback, paramData);
                }
            }
            else
            {
                UIBundleRef bundleRef = new UIBundleRef();
                bundleRef.Open(CallBack);
                uiRefDic_.Add(assetsName, bundleRef);
                uiRef4For_.Add(bundleRef);
                AssetLoader.LoadAssetBundle(assetsName, AssetLoader.EAssetType.ASSET_UI, LoadUICallback, paramData);
            }
        }
		
		return true;
	}

    static bool LoadCheck(LoadInfo info)
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
            if (uiRefDic_.ContainsKey(info.name_))
            {
                if (uiRefDic_[info.name_].hasBundle)
                {
                    info.callback_(uiRefDic_[info.name_].Open(), info.pdata_);
                }
                else
                {
                    uiRefDic_[info.name_].Open(info.callback_);
                    AssetLoader.LoadAssetBundle(info.name_, AssetLoader.EAssetType.ASSET_UI, LoadUICallback, info.pdata_);
                }
            }
            else
            {
                UIBundleRef bundleRef = new UIBundleRef();
                bundleRef.Open(info.callback_);
                uiRefDic_.Add(info.name_, bundleRef);
                uiRef4For_.Add(bundleRef);
                AssetLoader.LoadAssetBundle(info.name_, AssetLoader.EAssetType.ASSET_UI, LoadUICallback, info.pdata_);
            }
        }
        return canLoadUI;
    }

    static public void LoadRefAtlasFin(string atlasName)
    {
        for(int i=0; i < loadLst.Count; ++i)
        {
            if(loadLst[i].status_.ContainsKey(atlasName))
                loadLst[i].status_[atlasName] = AtlasLoader.AtlasStatus.AS_Loaded;

            if (LoadCheck(loadLst[i]))
            {
                loadLst.RemoveAt(i);
                i--;
            }
        }
    }

	static public void DeleteAsset( UIASSETS_ID ID , bool unLoadAllLoadedObjects )
	{
		string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName( (int)ID , AssetLoader.EAssetType.ASSET_UI );
        DeleteAsset(AssetsName);
        string[] refAtlas = UIDepedenceData.GetRefAtlas(AssetsName);
        if (refAtlas == null)
            ClientLog.Instance.Log(AssetsName + " UI has no refAtlas! AssetID is : " + ID);
        AtlasLoader.Instance.DeleteAtlas(refAtlas);
        //AssetInfoMgr.Instance.DecRefCount( AssetsName, unLoadAllLoadedObjects );
	}

    static public void DeleteAsset(string assetName)
    {
        foreach (string bundleName in uiRefDic_.Keys)
        {
            if (bundleName.Equals(assetName))
                uiRefDic_[bundleName].Close();
        }
    }
	
    static void LoadUICallback(AssetBundle bundle, ParamData data)
    {
        if (bundle.mainAsset == null)
            return;

        string name = bundle.mainAsset.name;
        if (uiRefDic_.ContainsKey(name))
        {
            uiRefDic_[name].CallFuncWithParam(bundle, data);
            uiRefDic_[name].destroyBundle_ = true;
        }
    }

    static public void Update()
    {
        for (int i = 0; i < uiRef4For_.Count; ++i)
        {
            uiRef4For_[i].Update();
        }
    }
}

class UIBundleRef
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

    //UI资源
    AssetBundle uiBundle_;

    AssetLoader.AssetLoadCallback callback_;

    public bool hasBundle
    {
        get { return uiBundle_ != null; }
    }

    public AssetBundle Open(AssetLoader.AssetLoadCallback callback)
    {
        refCount_ += 1;
        callback_ = callback;
        CalcParam();

        return uiBundle_;
    }

    public AssetBundle Open()
    {
        refCount_ += 1;
        CalcParam();

        return uiBundle_;
    }

    public void CallFuncWithParam(AssetBundle bundle, ParamData data)
    {
        uiBundle_ = bundle;
        if (callback_ != null)
            callback_(uiBundle_, data);
        callback_ = null;
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
        destroyTime_ = 2 * freq_;
    }

    public void Update()
    {
        if (refCount_ <= 0 && uiBundle_ != null)
        {
            timer_ += Time.deltaTime;
            if (timer_ > destroyTime_)
            {
                uiBundle_.Unload(true);
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

    public void DestroyBundle()
    {
        if (uiBundle_ != null)
        {
            uiBundle_.Unload(false);
            uiBundle_ = null;
        }
    }
}enum APPLE_ab249a5604aa41c1ae4d6c89429ecf10
{

}