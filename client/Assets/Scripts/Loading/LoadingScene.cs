using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

	static LoadingScene inst = null;
	public static LoadingScene Instance
	{
		get { return inst; }
	}

	private AsyncOperation async;
	private int  process;//加载的进度  
	private int _nowprocess;
	public bool ui_loaded_;	//UI加载完毕标记
    public bool dynamicResLoaded_;//动态资源加载标记
	public UITexture back;
    public bool manualCloseUI_;//手动关闭loading界面

    bool destroyed;

	public delegate void UpdateProgress(float progress);
	public UpdateProgress UpdateCallBack;

	public delegate void Reset();
	public Reset ResetCallBack;

    public delegate void InLoadingScene();
    public static event InLoadingScene OnBeginLoading;

    string scenenicon = "";

	// Use this for initialization
	void Start () {
		inst = this;
		if(GameManager.Instance!=null)
		{
			GameManager.Instance.SetChatUIActive(false);
		}
        destroyed = false;
        if (OnBeginLoading != null)
            OnBeginLoading();
		SceneData sdata = SceneData.GetData (GameManager.SceneID);
		
		if(sdata == null)
		{
			scenenicon = "loding";
		}else
		{
			scenenicon = sdata.scene_icon;
		}
		HeadIconLoader.Instance.LoadIcon (scenenicon, back);
        ui_loaded_ = !StageMgr.withUI_;
        dynamicResLoaded_ = !StageMgr.withDynRes_;
        manualCloseUI_ = StageMgr.manualClose_;
		// 注册loading形式
		LoadingProgressbar progressbar = GetComponentInChildren<LoadingProgressbar>();
		progressbar.UseThisType();
		//加载场景模型
        SceneLoader.Instance.LoadScene(StageMgr.Scene_name, () =>
        {
            StartCoroutine(loading());
        });
		//加载UI资源
		GlobalInstanceFunction.LoadSceneUI(StageMgr.Scene_name, UILoadFinish);

        Resources.UnloadUnusedAssets();
	}


	// Update is called once per frame
	void Update () {
		if(async == null)
			return;
		process = (int)(async.progress*100);
		int toProcess;
		// 如果进度没到100 按帧率缓动进度条
		if(async.progress < 0.9f)
		{
			toProcess = (int)(async.progress * 100);
		}
		else
		{
			// 进度到100，就直接跳转吧
			toProcess = 100;
			//			_nowprocess = toProcess;
		}
		
		if(_nowprocess < toProcess)
		{
			_nowprocess += Mathf.CeilToInt(Time.deltaTime * 100f);
			if(_nowprocess >= toProcess)
				_nowprocess = toProcess;
		}

		if(UpdateCallBack != null)
			UpdateCallBack(_nowprocess);

		if (_nowprocess == 100) 
		{

            if (ui_loaded_ && dynamicResLoaded_)
				EnterScene(async);
		}
	}

	void UILoadFinish()
	{
		ui_loaded_ = true;
        if (_nowprocess == 100 && dynamicResLoaded_)
            EnterScene(async);
	}

    public void DynamicResLoaded()
    {
        dynamicResLoaded_ = true;
        if (_nowprocess == 100 && ui_loaded_)
            EnterScene(async);
    }

    public void EnterScene(AsyncOperation async = null)
	{
		if(async != null)
			async.allowSceneActivation = true;
        ScenePreloader.Instance.AllowEnter();
        StageMgr.ExcuteBeginLoadEvent();
        GlobalInstanceFunction.Instance.Invoke (() => {

            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            StageMgr.ExcuteLoadedEvent();
            if (!manualCloseUI_)
                DestroyLoadingUI();
        }, 1);
		async = null;
	}

    public void DestroyLoadingUI()
    {
        if (destroyed)
            return;
		if(GameManager.Instance!=null)
		{
			GameManager.Instance.SetChatUIActive(true);
		}
        if (StageMgr.Loadtype == SwitchScenEffect.LoadingBar)
        {
            //GlobalInstanceFunction.Instance.ReleaseTexture(transform);
            UIAssetMgr.DeleteAsset(UIASSETS_ID.UIASSETS_LoadingPanel, true);
            Destroy(gameObject);
            GameManager.Instance.EnableDelayCheck(true);
            HeadIconLoader.Instance.Delete(scenenicon);
        }
    }

    void OnDestroy()
    {
        destroyed = true;
    }

    public	IEnumerator loading()  
	{
        if (ScenePreloader.Instance.isPreLoading)
            async = ScenePreloader.Instance.async_;
        else
            async = Application.LoadLevelAsync(StageMgr.Scene_name);
        async.allowSceneActivation = false;
        yield return async;
	}
}
