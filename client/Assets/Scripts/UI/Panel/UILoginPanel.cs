using System;
using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using UnityHTTP;

public class UILoginPanel : UIBase {

    public UIButton m_LoginB;
    public UIButton debugLoginBtn;
    public UIButton logOutBtn;
	public UIInput userNameInput;
	public UIInput passWordInput;
	public UILabel buildVer;
    public UILabel resVer;
    public UILabel selectServName_;
    public GameObject loginGroup_;
    public GameObject selectServ_;
    public GameObject inputGroup_;
    public GameObject selectServPanel_;
    public GameObject isbn_;
    public UITexture loginback;

    public float OnClickedTimer_ = 0f;

    string servSaveStr = "mhflccreatebyxysk";
    string servInfo_;
    static public string userName_ = ""; //开发模式

	void Start()
	{
        HeadIconLoader.Instance.LoadIcon("loginBack", loginback);
		ConfigLoader.Instance.finishDownFileEvent += new RequestEventHandler<int>(OnStartGame);
		VersionManager.Instance.versionNumEvent += new RequestEventHandler<string>(OnVerSionNumEvent);
        VersionManager.Instance.finishDownFileEvent += new RequestEventHandler<int>(OnFinishDownFileEvent);
        buildVer.text = GameManager.Instance.GetVersionNum();
        OnClickedTimer_ = 0f;
        string localServInfo = PlayerPrefs.GetString(servSaveStr);
        string[] splitted = localServInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        if (splitted.Length == 2)
        {
            GameManager.ServName_ = splitted[0];
            GameManager.ServId_ = int.Parse(splitted[1]);
        }
        selectServName_.text = GameManager.ServName_;

        NetConnection.Instance.OnSocketError += OnSocketException;
        ARPCProxy.OnSessionFailed += SessionFailedHandler;
		CommonEvent.OnUserExternal += OnUserExternal;
		UIManager.SetButtonEventHandler (m_LoginB.gameObject, EnumButtonEvent.OnClick, OnClickm_LoginB, 0, 0);
        UIManager.SetButtonEventHandler(debugLoginBtn.gameObject, EnumButtonEvent.OnClick, OnClickdebugLoginBtn, 0, 0);
        UIManager.SetButtonEventHandler(logOutBtn.gameObject, EnumButtonEvent.OnClick, OnUnReg, 0, 0);

        UIEventListener.Get(selectServ_).onClick += (GameObject go) => {
            selectServPanel_.SetActive(true);
			if(ApplicationEntry.Instance.isChcekFile)
				m_LoginB.gameObject.SetActive(true);
        };

		StageMgr.SceneLoadedFinish ();

        //if(File.Exists(Application.persistentDataPath+ "/CopyVersion.txt"))
        //{
        //    string[] strArr = File.ReadAllText(Application.persistentDataPath + "/CopyVersion.txt").Split(';');
        //    //versionLab.gameObject.SetActive(true);
        //    //versionLab.text = strArr[0];
        //}

		InvokeRepeating("checkSdkInitFinish", 0.1f, 0.5f);
        resVer.text = ApplicationEntry.Instance.ResVersion;
	}

	void checkSdkInitFinish()
	{
		if (GlobalValue.IsDebugMode)
		{
			inputGroup_.SetActive(true);
			CancelInvoke("checkSdkInitFinish");
		}
		else
		{
			//if(gameHandler._SdkInitSuccess)
			//{
			//	TransferRate._Inst.Send("Login SDK Begin");
			//	game.GameUser.getInstance().login();
			//	CancelInvoke("checkSdkInitFinish");
			//}
		}
	}
	
	void SessionFailedHandler()
    {
        if (GameManager.Instance.loginInfo_ != null)
        {
            NetConnection.Instance.login(GameManager.Instance.loginInfo_);
        }
    }

	private void OnClickm_LoginB(ButtonScript obj, object args, int param1, int param2)
	{
        if(string.IsNullOrEmpty(userName_))
        {
			if (GlobalValue.IsDebugMode)
			{
				inputGroup_.SetActive(true);
			}
			else
			{
				//game.GameUser.getInstance().login();
			}
        }
        else
        {
			if (string.IsNullOrEmpty(GameManager.ServName_) && !GlobalValue.IsDebugMode)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("SelectServ"), null, true);
                return;
            }

            SelectServPanel ssp = selectServPanel_.GetComponent<SelectServPanel>();
            string host = "127.0.0.1";//ssp.Host(GameManager.ServName_);//"121.69.36.174";"testmhflc.tanyu.mobi";//"120.26.58.230";
            int port = 28000;// ssp.Port(GameManager.ServName_);//20101;20401
            ApplicationEntry.Instance.ConnectToWorld(host, port);
            string localSaveServInfo = GameManager.ServName_ + ":" + GameManager.ServId_;
            PlayerPrefs.SetString(servSaveStr, localSaveServInfo);
            if (!string.IsNullOrEmpty(host))
                loginGroup_.SetActive(false);
			ErrorTipsUI.ShowMe("连接中...请稍候...");
			TransferRate._Inst.Send("Connect World Begin");
        }
	}

    void OnUserExternal(int code)
    {
   //     if (code == (int)game.UserActionResultCode.kLoginSuccess)
   //     {
			//if (GlobalValue.IsDebugMode)
			//	userName_ = userNameInput.value;
   //         else
   //             userName_ = game.GameUser.getInstance().getPluginId() + "=" + game.GameUser.getInstance().getUserID();
			//TransferRate._Inst.Send("Show Update Notice");
   //         StartCoroutine(pullServs(userName_));
   //     }
   //     else if (code == (int)game.UserActionResultCode.kInitSuccess)
   //     {
   //         if (!GlobalValue.channelID.Equals(999)/*神域之门 曙光 即拓*/)
   //             isbn_.SetActive(true);
   //     }
    }

    void OnClickdebugLoginBtn(ButtonScript obj, object args, int param1, int param2)
    {
        if (string.IsNullOrEmpty(userNameInput.value))
            return;

        //OnUserExternal((int)game.UserActionResultCode.kLoginSuccess);
        inputGroup_.SetActive(false);
    }

    void OnUnReg(ButtonScript obj, object args, int param1, int param2)
    {
        //if (game.GameUser.getInstance().isFunctionSupported("logout"))
        //{
        //    game.GameUser.getInstance().callFuncWithParam("logout");
        //}
    }

	public override void Destroyobj ()
	{
        NetConnection.Instance.OnSocketError -= OnSocketException;
        ARPCProxy.OnSessionFailed -= SessionFailedHandler;
		CommonEvent.OnUserExternal -= OnUserExternal;
        ConfigLoader.Instance.finishDownFileEvent -= OnStartGame;
		VersionManager.Instance.versionNumEvent -= OnVerSionNumEvent;
        VersionManager.Instance.finishDownFileEvent -= OnFinishDownFileEvent;
		GameObject.Destroy (gameObject);
	}
	
	void Update()
	{
        if (OnClickedTimer_ > 0f)
            OnClickedTimer_ -= Time.deltaTime;

        if (OnClickedTimer_ < 0f && m_LoginB.collider.enabled == false)
        {
			m_LoginB.collider.enabled = true;
			m_LoginB.GetComponentInChildren<UISprite>().color = Color.white;
        }

        if (GameManager.serChanged_)
        {
            selectServName_.text = GameManager.ServName_;
            GameManager.serChanged_ = false;
        }

        if (ApplicationEntry.Instance.ResVerIsDirty)
        {
            resVer.text = ApplicationEntry.Instance.ResVersion;
            ApplicationEntry.Instance.ResVerIsDirty = false;
            //if(GlobalValue.channelID.Equals("160136"))
            //    logOutBtn.gameObject.SetActive(true);
        }
	}



    void OnStartGame(int num)
    {
        StartCoroutine(ServNotice());
    }
	void OnFinishDownFileEvent(int num)
	{
		m_LoginB.gameObject.SetActive(true);
	}

    string updateContent_ = "";
    
    IEnumerator ServNotice()
    {
        LaunchLoginInfo();
        NetConnection.Instance.sessionlogin(GameManager.Instance.loginInfo_);
        //WWWForm form = new WWWForm();
        //form.AddField("servid", GameManager.ServId_.ToString());
        //Request www = new Request("post", GlobalValue.centerservhost + GlobalValue.servNotice, form);
        //www.Send ();
        //while( !www.isDone )
        //{
        //    yield return null;
        //}

        //if (www.isDone)
        //{
        //    if (www.exception == null && www.response != null)
        //    {
        //        updateContent_ = www.response.Text;
        //        if(string.IsNullOrEmpty(updateContent_))
        //        {
        //            LaunchLoginInfo();
        //            NetConnection.Instance.sessionlogin(GameManager.Instance.loginInfo_);
        //            ErrorTipsUI.ShowMe("连接中...请稍候...");
        //            ApplicationEntry.Instance.isChcekFile = true;
        //        }
        //        else
        //        {
        //            NoticeManager.Instance.ShowUpdateNotice("区服公告", updateContent_, (GameObject go) =>
        //            {
        //                LaunchLoginInfo();
        //                NetConnection.Instance.sessionlogin(GameManager.Instance.loginInfo_);
        //                ErrorTipsUI.ShowMe("连接中...请稍候...");
        //                ApplicationEntry.Instance.isChcekFile = true;
        //            });
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("Pull ServNotice  isDone! has error");
        //        LaunchLoginInfo();
        //        NetConnection.Instance.sessionlogin(GameManager.Instance.loginInfo_);
        //        ErrorTipsUI.ShowMe("连接中...请稍候...");
        //        ApplicationEntry.Instance.isChcekFile = true;
        //    }
        //}
        yield return null;
    }

    void LaunchLoginInfo()
    {
        if (GlobalValue.IsDebugMode)
        {
            GameManager.Instance.loginInfo_ = new COM_LoginInfo();
            GameManager.Instance.loginInfo_.username_ = userName_;
            GameManager.Instance.loginInfo_.password_ = "";
            GameManager.Instance.loginInfo_.version_ = Configure.VersionNumber;
            GameManager.Instance._Account = GameManager.Instance.loginInfo_.username_;
            GameManager.Instance.loginInfo_.sessionkey_ = Configure.Sessionkey;
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            GameManager.Instance.loginInfo_.idfa_ = XyskIOSAPI.GetIDFA();
#elif UNITY_ANDROID
            GameManager.Instance.loginInfo_.mac_ = XyskAndroidAPI.getMacAndroid();
#endif
        }
    }

	void OnVerSionNumEvent(string str)
	{
        //if (!versionLab.gameObject.activeSelf) 
        //{
        //    versionLab.gameObject.SetActive(true);
        //    versionLab.text =str;
        //}
	}

    IEnumerator pullServs(string userName)
    {
        //WWWForm form = new WWWForm();
        //form.AddField("username", userName);
        //form.AddField("version", GameManager.Instance.GetVersionNum());
        //form.AddField("channel", GlobalValue.channelID);

        //Request post = new Request("post", GlobalValue.centerservhost + GlobalValue.servListUrl, form);
        //post.Send ();
        //while( !post.isDone )
        //{
        //    
        //}

        //if (post.isDone)
        //{
        //    if (post.exception == null && post.response != null)
        //    {
        //        servInfo_ = post.response.Text;
        //        selectServPanel_.GetComponent<SelectServPanel>().SetData(servInfo_);
        //        selectServ_.SetActive(true);
        //    }
        //    else
        //    {
        //        userName_ = "";
        //        ApplicationEntry.Instance.PostSocketErr(90090);
        //    }
        // }

        yield return null;
    }

    void OnSocketException(int errCode)
    {
        loginGroup_.SetActive(true);
    }
}
