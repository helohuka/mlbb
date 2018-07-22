using UnityEngine;
using System.Collections.Generic;

public class StageMgr 
{
    public delegate void LoadingSceneCallBack(string sceneName);
    public static event LoadingSceneCallBack OnSceneLoaded;

    public delegate void BeginLoadingCallBack();
	public static event BeginLoadingCallBack OnSceneBeginLoad;

	public static SwitchScenEffect Loadtype;
	public static Texture2D tex;
	public static bool byc_fin_;
	public static string Scene_name = "";
    public static bool withUI_, withDynRes_, manualClose_;
    public static bool sendSceneLoaded_;

    public static string preScene_;

    static public bool Loading = false;

    static GameObject shitMask_;

    static Queue<WaitLoadInfo> wait4LoadQue_ = new Queue<WaitLoadInfo>();
    class WaitLoadInfo
    {
        public string name_;
        public SwitchScenEffect type_;
        public bool withUI_ = true;
        public bool withDynRes_ = false;
        public bool manualClose_ = false;
		public bool resetLocker_ = false;
		public LoadingSceneCallBack loadCallback_;
		public BeginLoadingCallBack beginCallback_;
    }
    static System.Diagnostics.Stopwatch sw;
    public static void LoadingAsyncScene(string name, SwitchScenEffect type = SwitchScenEffect.LoadingBar, bool withUI = true, bool withDynRes = false, bool manualClose = false, bool resetLocker = false)
    {
		if(GlobalValue.isBattleScene(name))
			GamePlayer.Instance.isInBattle = true;
		else
			GamePlayer.Instance.isInBattle = false;

        GameManager.Instance.EnableDelayCheck(false);
        GuideManager.Instance.ClearMask();
        GlobalInstanceFunction.Instance.ClearInvokeRepeat();

		PlayerAsseMgr.ClearAll ();
		EffectAssetMgr.ClearAll ();
		Resources.UnloadUnusedAssets ();
		if (Loading || ScenePreloader.Instance.DiffSceneLoad(name))
        {
			wait4LoadQue_.Clear();
            WaitLoadInfo wli = new WaitLoadInfo();
            wli.name_ = name;
            wli.type_ = type;
            wli.withUI_ = withUI;
            wli.withDynRes_ = withDynRes;
            wli.manualClose_ = manualClose;
			wli.resetLocker_ = resetLocker;
            wait4LoadQue_.Enqueue(wli);
            return;
        }

        Loading = true;

		if(resetLocker)
			GameManager.Instance.reconnectionLocker_ = false;
		ApplicationEntry.Instance.switchSceneMask_.SetActive(true);

        if (OnSceneBeginLoad != null)
            OnSceneBeginLoad();

        UIManager.Instance.DoDeActive();
        withUI_ = withUI;
        withDynRes_ = withDynRes;
        manualClose_ = manualClose;
        preScene_ = Scene_name;
		Scene_name = name; 
		Loadtype = type;

        if(Scene_name.Equals(GlobalValue.StageName_ReLoginScene))
            GameManager.Instance.ClearCurrentState();

		if (type == SwitchScenEffect.LoadingBar)
		{
			UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_LoadingPanel, () => {
                UIFactory.Instance.CloseCurrentUI();
                UIFactory.Instance.OpenUI(UIASSETS_ID.UIASSETS_LoadingPanel, menuTypes.Popup);
                ApplicationEntry.Instance.switchSceneMask_.SetActive(false);
			});
		}
		else
		{
			GlobalInstanceFunction.LoadSceneUI(Scene_name, () => {
                ShowGame.Instance.ShowScenEffect (type);
			});
		}
        Prebattle.Instance.ClearAssetLoadCount();
        sendSceneLoaded_ = false;
	}
	public static void LoadingScene(string name)
	{
        GameManager.Instance.EnableDelayCheck(false);
        GuideManager.Instance.ClearMask();
        ApplicationEntry.Instance.switchSceneMask_.SetActive(true);
        UIManager.Instance.DoDeActive();
        Prebattle.Instance.ClearAssetLoadCount();
        GlobalInstanceFunction.LoadSceneUI(name, () =>
        {
            Application.LoadLevel(name);
        });
	}

    public static void ExcuteBeginLoadEvent()
    {
        UIFactory.Instance.CloseCurrentUI();
        Prebattle.Instance.BeforeEnterBattle();
    }

    public static void ExcuteAllLoadedEvent()
    {
        ApplicationEntry.Instance.switchSceneMask_.SetActive(false);
        GameManager.Instance.EnableDelayCheck(true);
		if(GlobalValue.isBattleScene(Application.loadedLevelName))
		{
			StageMgr.SceneLoadedFinish();
		}
    }

    public static void ExcuteLoadedEvent()
    {
        Transform trans = null;
        if (ApplicationEntry.Instance != null)
        {
            for (int i = 0; i < ApplicationEntry.Instance.uiRoot.transform.childCount; ++i)
            {
                trans = ApplicationEntry.Instance.uiRoot.transform.GetChild(i);
                if (trans.gameObject.name.Equals("NameLabel"))
                    GameObject.Destroy(trans.gameObject);
            }
        }

        Loading = false;

        if (OnSceneLoaded != null)
            OnSceneLoaded(Scene_name);

		SceneData sd = SceneData.GetData (GameManager.SceneID);
		if(sd == null)return;
		MusicAssetsData mdata = MusicAssetsData.GetData (sd.M_ID);
		if(!GlobalValue.isBattleScene(Scene_name))
			SoundTools.PlayMusic ((MUSIC_ID)mdata.id_);

		if(!BagSystem.instance.IsInit && !Scene_name.Equals("LoginScene") && !Scene_name.Equals("CreateRoleScene"))
		{
			NetConnection.Instance.requestBag();
		}
    }

    public static bool SceneLoadedFinish()
    {
        if (wait4LoadQue_.Count > 0)
        {
            WaitLoadInfo wli = wait4LoadQue_.Dequeue();
            LoadingAsyncScene(wli.name_, wli.type_, wli.withUI_, wli.withDynRes_, wli.manualClose_, wli.resetLocker_);
            if (GlobalValue.isBattleScene(wli.name_))
                return true;
        }
        else if (GameManager.Instance.nextBattle_ != null)
        {
            Battle.Instance.ComboBattle();
            GameManager.Instance.nextBattle_ = null;
            return true;
        }
        else// if (!GamePlayer.Instance.isInBattle)
        {
            if (Prebattle.Instance.ExcuteNextScene() == false)
            {
                if (Scene_name.Equals(GlobalValue.StageName_piantoudonghuaf) || Scene_name.Equals("Village01"))
                    ApplicationEntry.Instance.PlaceCinema();
            }
        }
        return false;
    }

	public static void ClearStageLoadQue()
	{
		if(wait4LoadQue_ != null)
			wait4LoadQue_.Clear();
	}

	public static bool HasNextScene()
	{
		if(wait4LoadQue_ != null)
			return wait4LoadQue_.Count > 0;
		return false;
	}
}
enum APPLE_2924d8e03d864f1a9f3b1fae238dc1b7
{

}