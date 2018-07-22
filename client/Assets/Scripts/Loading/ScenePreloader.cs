using UnityEngine;
using System.Collections;

public class ScenePreloader {

    static ScenePreloader inst_;
    static public ScenePreloader Instance
    {
        get
        {
            if(inst_ == null)
                inst_ = new ScenePreloader();
            return inst_;
        }
    }

    public AsyncOperation async_ = null;

    public string crtPreLoadScene_ = "";

	public void PreLoadScene(string sceneName)
    {
		if(GameManager.Instance.reconnectionLocker_)
			return;

        crtPreLoadScene_ = sceneName;
        async_ = Application.LoadLevelAsync(sceneName);
        async_.allowSceneActivation = false;
    }

    public bool isPreLoading
    {
        get
        {
            return async_ != null;
        }
    }

	public bool DiffSceneLoad(string toLoad)
	{
        return (!string.IsNullOrEmpty(crtPreLoadScene_) && !crtPreLoadScene_.Equals(toLoad));
	}

    public void AllowEnter()
    {
        if (async_ != null)
            async_.allowSceneActivation = true;
        async_ = null;
        crtPreLoadScene_ = "";
    }
}
