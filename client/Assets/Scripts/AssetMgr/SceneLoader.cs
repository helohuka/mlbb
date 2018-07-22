using UnityEngine;
using System.Collections.Generic;

public class SceneLoader {

    static SceneLoader inst_ = null;
    public static SceneLoader Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new SceneLoader();
            return inst_;
        }
    }

	Dictionary<string, AssetBundle> refDic_;

	public void Init()
	{
		refDic_ = new Dictionary<string, AssetBundle>();
	}

    public delegate void SceneLoadHandler();
    SceneLoadHandler callback_;
    public void LoadScene(string sceneName, SceneLoadHandler callback)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		callback();
#else
        if (refDic_.ContainsKey(sceneName))
        {
            if (callback != null)
            {
                callback();
                callback = null;
            }
            return;
        }
        callback_ = callback;
        AssetLoader.LoadAssetBundle(sceneName, AssetLoader.EAssetType.ASSET_STAGE, OnSceneLoaded, new ParamData(sceneName));
#endif
    }

    void OnSceneLoaded(AssetBundle bundle, ParamData data)
    {
		if(bundle != null && data != null)
		{
			if (!refDic_.ContainsKey(data.szParam))
			{
				refDic_.Add(data.szParam, bundle);
			}
		}
        if (callback_ != null)
        {
            callback_();
            callback_ = null;
        }
    }

    List<string> toDelete = new List<string>();
    public void Update()
    {
        if (StageMgr.Loading)
            return;

        if (refDic_ != null)
        {
            toDelete.Clear();
            foreach (string refScene in refDic_.Keys)
            {
                if (!Application.loadedLevelName.Equals(refScene))
                    toDelete.Add(refScene);
            }
            for (int i = 0; i < toDelete.Count; ++i)
            {
                refDic_[toDelete[i]].Unload(false);
                refDic_.Remove(toDelete[i]);
            }
        }
    }
}
enum APPLE_3749ccd0b7ca488087b085bef1471fe0
{

}