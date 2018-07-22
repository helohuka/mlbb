using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityHTTP;

public class ApplicationEntry : MonoBehaviour
{

    public GameObject uiRoot;
    public Camera ui3DCamera;
    public Object nameLabel;
    public GameObject switchSceneMask_;
    public AudioSource audioSource_;
    public GameObject cinemaPre_;
    static ApplicationEntry inst = null;
    GameObject uiLoadingPanel_;
    string fixContent_ = "";
    string fixNoticeHtp = "";
    Camera uiCamera_;
    public Camera UICamera
    {
        get { return uiCamera_; }
    }

    public string host_;
    public int port_;
    public bool isChcekFile;
    public bool isLoadFileFinish;

    public string ResVersion;
    public bool ResVerIsDirty;

    bool mayShowSysNotice;
    bool mayPullResFolderName;

    static public ApplicationEntry Instance
    {
        get { return inst; }
    }
    CCPRestSDK.CCPRestSDK api;

    string version = "";
    string platformPath = "";
    // Use this for initialization
    void Start()
    {
        //玩家是否手动设置了画质
        string userSet = PlayerPrefs.GetString("UserSetQualityLevel");
        GameManager.Instance.QualityLv = PlayerPrefs.GetInt("QualityLevel", GameManager.Instance.QualityLv);
        if (string.IsNullOrEmpty(userSet))
        {
            //如果玩家没设置过并且是0 则设置为3 中等画质
            if (GameManager.Instance.QualityLv == 0)
                GameManager.Instance.QualityLv = 3;
        }
        QualitySettings.SetQualityLevel(GameManager.Instance.QualityLv);

        inst = this;
        DontDestroyOnLoad(uiRoot);
        DontDestroyOnLoad(ui3DCamera);
        DontDestroyOnLoad(this);

        GameManager.Instance.JudgeIsPad();
#if UNITY_IOS || UNITY_IPHONE
        XyskIOSAPI.SetNotBackup();
#endif

        uiCamera_ = uiRoot.GetComponentInChildren<UICamera>().camera;

        Application.targetFrameRate = 30;
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // catch global log
        Application.RegisterLogCallback(logReport);

        VersionManager.Instance.finishDownFileEvent += new RequestEventHandler<int>(OnFinishDownFileEvent);
        VersionManager.Instance.CopyEvent += new RequestEventHandler<int>(OnCopyEvent);

        NetConnection.Instance.discard();

        version = GameManager.Instance.GetVersionNum();
        platformPath = GameManager.Instance.PlatformToString();
        if (string.IsNullOrEmpty(version))
        {
            //版本获取不到，请下载最新游戏包
            return;
        }
        //拼合cdn地址
        //GlobalValue.cdnservhost = string.Format("{0}{1}/{2}", GlobalValue.cdnservhost, version, platformPath);

        PlayerDepLoader.Instance.OnPlayerLoaded += PlayerAsseMgr.LoadRefAssetsFin;
        EffectDepLoader.Instance.OnEffectLoaded += EffectAssetMgr.LoadRefAssetsFin;

        //        AssetLoader.LoadAssetBundle("commonAssets", AssetLoader.EAssetType.ASSET_UI, (AssetBundle bundle, ParamData data) =>
        //        {
        //            //PopText.Instance.Init();
        //            //NpcHeadChat.Instance.Init();
        //            //UIManager.Instance.InitIconCell();
        //            //GuideManager.Instance.creator.InitArrow();
        //            //AssetLoader.LoadAssetBundle("PlayerShader", AssetLoader.EAssetType.ASSET_PLAYER, null, null);
        //            //StartCoroutine(PullResFolderName());
        //            mayPullResFolderName = true;
        //			TransferRate._Inst.Send("Load CommonAssets End");
        //        }, null, Configure.assetsPathstreaming);
        mayPullResFolderName = true;
        mayShowSysNotice = true;

        //cinemaPre_ = Resources.Load<GameObject>("Cinema");

        //Caching.CleanCache();
    }

    public void LoginUIOk()
    {
        OnCopyEvent(1);
    }


    void OnCopyEvent(int num)
    {
        //连资源服务器 查看版本
        //..
        if (true)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            VersionManager.Instance.finishDownFileEvent(1);
#else
			//VersionManager.Instance._isCheck = true;
			VersionManager.Instance.finishDownFileEvent(1);
#endif
        }
        else
        {
            Debug.LogError("Cannot connect to server " + host_ + ":" + port_);
        }
    }


    void OnFinishDownFileEvent(int num)
    {
        if (isChcekFile)
            return;

        LuaMaster.Instance.Init();
        ConfigLoader.Instance.Init();

        float speed = 0f;
        GlobalValue.Get(Constant.C_BattleSpeed, out speed);
        Battle.Instance.reportPlaySpeed_ = speed;

        NetConnection.Instance.OnSocketError += SocketHandler;

        ResetGameConfig();

        switchSceneMask_ = new GameObject("SceneMask");
        UIPanel maskPanel = switchSceneMask_.AddComponent<UIPanel>();
        switchSceneMask_.layer = LayerMask.NameToLayer("UI");
        maskPanel.depth = 4000;
        maskPanel.sortingOrder = 4000;
        BoxCollider bc = switchSceneMask_.AddComponent<BoxCollider>();
        bc.size = new Vector3(UIWidth, UIHeight, 0f);
        switchSceneMask_.transform.parent = uiRoot.transform;
        switchSceneMask_.transform.localScale = Vector3.one;
        switchSceneMask_.SetActive(false);

    }

    public void ConnectToWorld(string ip, int port)
    {
        host_ = ip;
        port_ = port;
        NetConnection.Instance.connect(ip, port, FirstConnectCallBack);

        //连接网络时 加载些轻量级资源
        AssetLoader.LoadAssetBundle("NpcNameLabel", AssetLoader.EAssetType.ASSET_UI, (AssetBundle bundle2, ParamData data2) =>
        {
            nameLabel = bundle2.mainAsset;
        }, null, Configure.assetsPathstreaming);

        UIManager.Instance.InitIconCell();

        GuideManager.Instance.creator.InitArrow();

        AssetLoader.LoadAssetBundle("PlayerShader", AssetLoader.EAssetType.ASSET_PLAYER, null, null);
    }

    void LoadNessaryAssets()
    {
        //读表时加载这个资源
        cinemaPre_ = Resources.Load<GameObject>("Cinema");
    }

    void FirstConnectCallBack(System.IAsyncResult ar)
    {
        if (ar != null)
        {
            if (!isChcekFile)
            {
                ConfigLoader.Instance.parseDataFin_ += ApplicationEntry.Instance.ParseDataFinish;
                ConfigLoader.Instance.LoadAndParseData();
                LoadNessaryAssets();
            }
            else
            {
                ConfigLoader.Instance.finishDownFileEvent(1);
            }
            //CancelInvoke("CheckNetWorkState");
            InvokeRepeating("CheckNetWorkState", 0f, 5f);
        }
        else
        {
            ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("NetworkNoReach"));
        }
    }

    void ReconnectCallBack(System.IAsyncResult ar)
    {
        if (ar != null)
        {
            //不是登录界面或重登录界面则返回重登录界面
            if (!string.IsNullOrEmpty(StageMgr.Scene_name) && !StageMgr.Scene_name.Equals(GlobalValue.StageName_ReLoginScene))
                ReturnToLogin();
            else
                ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("connectionSuccess"));
        }
        else
        {
            if (!string.IsNullOrEmpty(StageMgr.Scene_name) && !StageMgr.Scene_name.Equals(GlobalValue.StageName_ReLoginScene))
                ReturnToLogin();
            ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("connectionFailed"));
        }
        netStatusWarning_ = false;
    }

    void ResetGameConfig()
    {
        GlobalInstanceFunction.Instance.changeTimeScale(1f);
    }

    int originDepth = 0;
    public void ShowUILoading()
    {
        UIPanel panel = null;
        panel = uiLoadingPanel_.GetComponent<UIPanel>();
        if (panel == null)
            panel = uiLoadingPanel_.AddComponent<UIPanel>();
        if (GuideManager.Instance.IsGuiding_)
        {
            panel.depth = GuideCreator.GuideDepth + 10;
            panel.sortingOrder = GuideCreator.GuideDepth + 10;
        }
        else
        {
            panel.depth = originDepth;
            panel.sortingOrder = originDepth;
        }
        uiLoadingPanel_.transform.localScale = Vector3.one;
        uiLoadingPanel_.transform.localPosition = new Vector3(0f, 0f, -500f);
        uiLoadingPanel_.SetActive(true);
        UIManager.Instance.AdjustUIDepth(uiLoadingPanel_.transform);
    }

    public void HideUILoading()
    {
        if (uiLoadingPanel_ != null && uiLoadingPanel_.activeSelf)
            uiLoadingPanel_.SetActive(false);
    }

    public float UIWidth
    {
        get
        {
            float width = 0f;
            if (uiRoot != null)
            {
                float s = (float)uiRoot.GetComponent<UIRoot>().activeHeight / Screen.height;
                width = Screen.width * s;
            }
            else
            {
                width = Screen.width;
            }
            return width;
        }
    }

    public float UIHeight
    {
        get
        {
            float height = 0f;
            if (uiRoot != null)
            {
                height = (float)uiRoot.GetComponent<UIRoot>().activeHeight;
            }
            else
            {
                height = Screen.height;
            }
            return height;
        }
    }

    public float World2UIHeight
    {
        get
        {
            float height = 0f;
            if (uiRoot != null)
            {
                height = (float)uiRoot.GetComponent<UIRoot>().activeHeight / 2;
            }
            else
            {
                height = Screen.height;
            }
            return height;
        }
    }

    public void PostSocketErr(int errCode)
    {
        SocketHandler(errCode);
    }

    void SocketHandler(int errCode)
    {
        ClientLog.Instance.Log(errCode + "  " + netStatusWarning_);
        if (netStatusWarning_ && UIManager.Instance.isOpen(UIASSETS_ID.UIASSETS_MessageBoxPanel))
            return;
        else
            netStatusWarning_ = false;

        NetConnection.Instance.discard();
        netStatusWarning_ = true;
        switch (errCode)
        {
            case 90090:
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("pullServInfoFailed"), () =>
                {
                    netStatusWarning_ = false;
                }, true, null, null, "", "", 4001);
                break;
            case 999001:
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("NetworkNoReach"), () =>
                {
                    NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                    netStatusWarning_ = false;
                }, true, null, null, "", "", 4001);
                break;
            case 2333:
                ReconnectCallBack(null);
                //NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                UILoginPanel.userName_ = "";
                GameManager.Instance.loginInfo_ = null;
                netStatusWarning_ = false;
                break;
            case 99001:
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("connectionTimeout"), () =>
                {
                    NetConnection.Instance.connect(host_, port_, FirstConnectCallBack);
                    netStatusWarning_ = false;
                }, true, null, null, "", "", 4001);
                break;
            case 555666:
                if (NetConnection.Instance.IsShutDown)
                {
                    ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("NetworkLag"));
                    NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                }
                //MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("NetworkLag"), () =>
                //{
                //    NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                //    netStatusWarning_ = false;
                //}, true, null, null, "", "", 4001);
                break;
            case 10058:
                if (!string.IsNullOrEmpty(GameManager.ServName_))
                {
                    MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("NetworkShutdown"), () =>
                    {
                        if (!string.IsNullOrEmpty(host_))
                            NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                        netStatusWarning_ = false;
                    }, true, null, null, LanguageManager.instance.GetValue("reconnect"), "", 4001);
                }
                break;
            case 10061:
                if (!string.IsNullOrEmpty(GameManager.ServName_))
                {
                    MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("serverShutdown"), () =>
                    {
                        //Application.Quit();
                        netStatusWarning_ = false;
                    }, true, null, null, "", "", 4001);
                }
                break;
            case 57558:
                ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dataSyncing"));
                GameManager.Instance.ClearCurrentState();
                NetConnection.Instance.requestPhoto();
                //MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("BattleSceneInitErr"), () =>
                //{
                //    ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dataSyncing"));
                //    GameManager.Instance.ClearCurrentState();
                //    NetConnection.Instance.requestPhoto();
                //    netStatusWarning_ = false;
                //}, true, null, null, "", "", 4001);
                break;
            case 88888:
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("RPCErr"), () =>
                {
                    Application.Quit();
                    netStatusWarning_ = false;
                }, true, null, null, "", "", 4001);
                break;
            case 11001:
            case 10051:
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("NetworkNoReach")/* + errCode.ToString()*/, () =>
                {
                    NetConnection.Instance.connect(host_, port_, ReconnectCallBack);
                }, true, null, null, "", "", 4001);
                break;
            //case 1234:
            //    MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("sdkInitError"), () =>
            //    {
            //        GameObject.FindObjectOfType<gameHandler>().Reinit();
            //    }, true, null, null, LanguageManager.instance.GetValue("reconnect"), "", 4001);
            //    break;

            default:
                break;
        }
    }

    IEnumerator PullResFolderName()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("channel", GlobalValue.channelID);
        //form.AddField("version", version);
        //Request www = new Request("post", GlobalValue.centerservhost + GlobalValue.resVersion, form);
        //www.Send ();

        //while( !www.isDone )
        //{
        //    yield return null;
        //}

        //if (www.isDone)
        //{
        //    if (www.exception == null && www.response != null)
        //    {
        //        ResVersion = www.response.Text;
        //        ResVerIsDirty = true;
        //        //GlobalValue.cdnservhost = string.Format("{0}{1}/", GlobalValue.cdnservhost, ResVersion);
        //        //ClientLog.Instance.Log(GlobalValue.cdnservhost);
        //        UIFactory.Instance.LoadUIPanel("LoginPanel", () =>
        //        {
        //            mayShowSysNotice = true;
        //            UIFactory.Instance.OpenUI(GlobalValue.StageName_LoginScene, menuTypes.MAIN);
        //            //AssetLoader.LoadAssetBundle("NpcNameLabel", AssetLoader.EAssetType.ASSET_UI, (AssetBundle bundle2, ParamData data2) =>
        //            //{
        //            //    nameLabel = bundle2.mainAsset;
        //            //}, null, Configure.assetsPathstreaming);
        //            //PopText.Instance.Init();
        //            //NpcHeadChat.Instance.Init();
        //            //UIManager.Instance.InitIconCell();
        //            //GuideManager.Instance.creator.InitArrow();
        //            //AssetLoader.LoadAssetBundle("PlayerShader", AssetLoader.EAssetType.ASSET_PLAYER, null, null);
        //            //cinemaPre_ = Resources.Load<GameObject>("Cinema");

        //        });
        //    }
        //    else
        //    {
        //        ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("resVersionGetError"));
        //        ClientLog.Instance.Log(www.exception);
        //    }
        //}
        yield return null;
    }

    IEnumerator SysNotice()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("channelid", GlobalValue.channelID);
        //Request www = new Request("post", GlobalValue.centerservhost + GlobalValue.sysNotice, form); 
        //www.Send ();
        //while( !www.isDone )
        //{
        //    yield return null;
        //}

        //if (www.isDone)
        //{
        //    if (www.exception == null)
        //    {
        //        if(www.response != null)
        //        {
        //            if(!string.IsNullOrEmpty(www.response.Text))
        //                NoticeManager.Instance.ShowUpdateNotice(LanguageManager.instance.GetValue("sysNotice"), www.response.Text);
        //        }
        //    }
        //}
        yield return null;
    }

    public void ParseDataFinish()
    {
        // init some system which based on config table.
        ActivitySystem.Instance.Init();
        SceneLoader.Instance.Init();

        //这个界面为ui打开前的loading 需要快速加载 所以预先加载到内存
        UIAssetMgr.LoadUI(UIASSETS_ID.UIASSETS_UILoading, (Assets, paramData) =>
        {
            uiLoadingPanel_ = GameObject.Instantiate(Assets.mainAsset) as GameObject;
            uiLoadingPanel_.transform.parent = uiRoot.transform;
            uiLoadingPanel_.transform.localScale = Vector3.zero;
            uiLoadingPanel_.SetActive(false);
            UIPanel panel = uiLoadingPanel_.GetComponent<UIPanel>();
            if (panel == null)
                panel = uiLoadingPanel_.AddComponent<UIPanel>();
            originDepth = panel.depth;

        }, null);

        //加载聊天ui
        GameManager.Instance.InitChatUI("LoginScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {

            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("ConfirmQuit"), () =>
            {
                SDKInterface.Instance.SDKExit();
                    //Application.Quit();
                }, false, null, null, "", "", 2000, true);

        }

        AtlasLoader.Instance.Update();
        PlayerDepLoader.Instance.Update();
        EffectDepLoader.Instance.Update();
        AssetInfoMgr.Instance.Update();
        UIAssetMgr.Update();
        PlayerAsseMgr.Update();
        EffectAssetMgr.Update();
        SceneLoader.Instance.Update();
        GuideManager.Instance.Update();
        GlobalInstanceFunction.Instance.Update();
        EventMgr.Instance.Update();
        NetConnection.Instance.Update();
        AssetLoader.Update();
        GameManager.Instance.Update();
        Prebattle.Instance.Update();
        Battle.Instance.Update();
        EffectMgr.Instance.Update();
        VersionManager.Instance.Update();
        ChatSystem.Update();
        BagSystem.instance.UpdateUsetime();
        BabyData.UpdateUsetime();
        EmployeeTaskSystem.instance.Update();

        if ( mayShowSysNotice)
        {
            mayShowSysNotice = false;
            //公告时 加载点轻量级资源
            PopText.Instance.Init();
            NpcHeadChat.Instance.Init();
            StartCoroutine(SysNotice());
        }


        //PopText.Instance.Init();
        //NpcHeadChat.Instance.Init();
        //StartCoroutine(SysNotice());


        if ( mayPullResFolderName)
        {
            mayPullResFolderName = false;
            //跳过cdn检查
            UIFactory.Instance.LoadUIPanel("LoginPanel", () =>
            {
                UIFactory.Instance.OpenUI(GlobalValue.StageName_LoginScene, menuTypes.MAIN);
                AssetLoader.LoadAssetBundle("NpcNameLabel", AssetLoader.EAssetType.ASSET_UI, (AssetBundle bundle2, ParamData data2) =>
                {
                    nameLabel = bundle2.mainAsset;
                    mayShowSysNotice = true;
                }, null, Configure.assetsPathstreaming);
            });
            StartCoroutine(PullResFolderName());
            //       
        }
       
    }

    public bool netStatusWarning_ = false;
    void CheckNetWorkState()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && netStatusWarning_ == false)
        {
            SocketHandler(999001);
        }

//        if (GameManager.Instance.GetPingDelay() > 10f)
//        {
//            if (!UIManager.Instance.isOpen(UIASSETS_ID.UIASSETS_NetWaitPanel))
//                NetWaitUI.ShowMe();
//        }
//
//        if(GameManager.Instance.GetPingDelay() <= 10f)
//        {
//            if (UIManager.Instance.isOpen(UIASSETS_ID.UIASSETS_NetWaitPanel))
//                NetWaitUI.HideMe();
//        }

        if (GameManager.Instance.GetPingDelay() > 20f && !Application.loadedLevelName.Equals("CreateRoleScene"))
        {
            NetConnection.Instance.disconnect();
            GameManager.Instance.ClearDelay();
        }

        if (NetConnection.Instance.IsShutDown)
        {
            SocketHandler(555666);
        }
    }

    string sLog = "";
    System.Collections.Generic.Queue<string> logQue = new System.Collections.Generic.Queue<string>();
	private void logReport(string cond, string stack, LogType type){

        sLog = "";
		sLog = cond + "\n";
		if(type == LogType.Error || type == LogType.Exception)
            sLog += stack;

        if (logQue.Count > 50)
            logQue.Dequeue();
        logQue.Enqueue(sLog);
		CommonEvent.ExcuteException(sLog);
        //NGUIDebug.Log (sLog);

        //Backlog.Instance.log(SystemInfo.deviceUniqueIdentifier, cond, stack.Replace('\n', '|'), "0");
	}

    float leaveTime_;
	bool isLoadingScene;
	string AppPauseBeforeScene = "";
    void OnApplicationPause(bool pauseStatus)
    {
        GameManager.Instance.EnableDelayCheck(!pauseStatus);
        if (pauseStatus)
        {
            AppPauseBeforeScene = Application.loadedLevelName;
            isLoadingScene = StageMgr.Loading;
            Application.targetFrameRate = 1;
			leaveTime_ = Time.realtimeSinceStartup;
            CommonEvent.ExcuteAppPause();
        }
        else
        {
            Application.targetFrameRate = 30;
			CommonEvent.ExcuteAppResume();
			if(StageMgr.Loading)
			{
				StageMgr.OnSceneLoaded += CheckReconnection;
			}
			else
				CheckReconnection(StageMgr.Scene_name);
        }
    }

	void CheckReconnection(string sceneName)
	{
        if (ScenePreloader.Instance.isPreLoading)
        {
            Debug.Log("CheckReconnection : ScenePreloader.Instance.isPreLoading");
            return;
        }

		StageMgr.OnSceneLoaded -= CheckReconnection;
		GlobalInstanceFunction.Instance.Invoke(() => {
            
            //if(AppPauseBeforeScene.Equals(StageMgr.Scene_name))
            //    return;

			if(!Application.loadedLevelName.Equals("LoginScene") && !Application.loadedLevelName.Equals("ReturnScene"))
		{
			if(NetConnection.Instance.IsShutDown)
			{
				Debug.Log("SocketHandler");
				SocketHandler(555666);
			}
			else
			{
                if (AppPauseBeforeScene.Equals(StageMgr.Scene_name) && GlobalValue.isFBScene(StageMgr.Scene_name))
                    return;

					bool needSync = NetConnection.Instance.discard() || (isLoadingScene && GlobalValue.isBattleScene(StageMgr.Scene_name));
				if (needSync)
				{
                    //NetConnection.Instance.discard();
					Debug.Log("requestPhoto");
					ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dataSyncing"));
					GameManager.Instance.ClearCurrentState();
					NetConnection.Instance.requestPhoto();
				}
			}
		}
		}, 1);
	}

	void OnDestroy()
	{
		NetConnection.Instance.disconnect ();
        NetConnection.Instance.OnSocketError -= SocketHandler;
		VersionManager.Instance.finishDownFileEvent -= OnFinishDownFileEvent;
		VersionManager.Instance.CopyEvent -= OnCopyEvent;
		ConfigLoader.Instance.parseDataFin_ -= ParseDataFinish;
		CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.LOGOUT);
	}

    public void ReturnToLogin()
    {
        if(!ScenePreloader.Instance.isPreLoading)
            GameManager.Instance.ClearCurrentState();
        StageMgr.OnSceneLoaded += OnLoginLoaded;
		StageMgr.LoadingAsyncScene(GlobalValue.StageName_ReLoginScene);
    }

    void OnLoginLoaded(string sceneName)
    {
        if (sceneName.Equals(GlobalValue.StageName_ReLoginScene))
        {
            StageMgr.OnSceneLoaded -= OnLoginLoaded;
            GameManager.Instance.ClearCurrentState();
        }
        VersionManager.Instance.finishDownFileEvent(1);
    }
#if UNITY_IOS
	public void SDK_MomoryClean()
	{
		PlayerAsseMgr.ClearAll();
		EffectAssetMgr.ClearAll();
		Resources.UnloadUnusedAssets ();
		Debug.Log ("SDK_MomoryClean");
	}
#endif
    public void PlaceCinema()
    {
        GameObject.Instantiate(cinemaPre_);
    }
}enum APPLE_24b6559dbcba4b0497759f369e6771c5
{

}