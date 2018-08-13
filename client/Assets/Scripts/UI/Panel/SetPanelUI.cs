using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class SetPanelUI : UIBase {

	public UILabel _TitleLable;
	public UILabel _NickNameLable;
	public UILabel _PlayerIdLable;
	public UILabel _MusicLable;
	public UILabel _SoundLable;
	public UILabel _ExitLable;
	public UILabel _ExchangeLable;
	public UILabel _SwitchingLable;
	public UILabel _versionLable;
    public UILabel _onlyFriendLabel;
    public UILabel _onlyTeamLabel;
    public UILabel _onlyGuildeLabel;
    public UILabel _staticLabel;
    public UILabel _filterAllLabel;
    public UILabel VersionContent;

	public UILabel AccountsLabel;
	public UILabel serverLabel;
	public UILabel versionLabel; 

	public UIButton musicOn;
	public UIButton musicOff;
	public UIButton SoundOn;
	public UIButton SoundOff;

    public UIToggle onlyFriend;
    public UIToggle onlyTeam;
    public UIToggle onlyGuilde;
    public UIToggle filterAll;

	public UIButton duihuaBtn;
	public UIButton gonggaoBtn;
	public UIButton switchAccountBtn;
	public UIButton unRegBtn;
	public UIButton switchPlayerBtn;
	public UITexture icon;
	public UILabel versionLab;
	public bool isMusicOn;
	public bool isSoundOn;
	public UIButton Lbtn;
	public UIButton Rbtn;
	public UIButton Ebtn;
	public UISprite qualitySp;
	string[] qualityLevel = {"liuchang","jingzhi","gaoqing"};
	int index = 0;
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("SetPanel_Title");
		_NickNameLable.text = LanguageManager.instance.GetValue("SetPanel_NickName");
		_PlayerIdLable.text = LanguageManager.instance.GetValue("SetPanel_PlayerId");
		_MusicLable.text = LanguageManager.instance.GetValue("SetPanel_Music");
		_SoundLable.text = LanguageManager.instance.GetValue("SetPanel_Sound");
		_ExitLable.text = LanguageManager.instance.GetValue("SetPanel_Exit");
		_ExchangeLable.text = LanguageManager.instance.GetValue("SetPanel_Exchange");
//        if (game.GameUser.getInstance().isFunctionSupported("logout"))
//            _SwitchingLable.text = LanguageManager.instance.GetValue("unreg");
//        else
			_SwitchingLable.text = LanguageManager.instance.GetValue("SetPanel_zhuxiao");
		_versionLable.text = LanguageManager.instance.GetValue("SetPanel_version");
        _onlyFriendLabel.text = LanguageManager.instance.GetValue("SetPanel_onlyFriend");
        _onlyTeamLabel.text = LanguageManager.instance.GetValue("SetPanel_onlyTeam");
        _onlyGuildeLabel.text = LanguageManager.instance.GetValue("SetPanel_onlyGuilde");
        _staticLabel.text = LanguageManager.instance.GetValue("SetPanel_staticLbl");
        _filterAllLabel.text = LanguageManager.instance.GetValue("SetPanel_filterAll");

		VersionContent.text = GameManager.Instance.GetVersionNum();//PlayerPrefs.GetString ("Version");
        //if(File.Exists(Application.persistentDataPath+ "/CopyVersion.txt"))
        //{
        //    string[] strArr = File.ReadAllText(Application.persistentDataPath + "/CopyVersion.txt").Split(';');
        //    VersionContent.gameObject.SetActive(true);
        //    VersionContent.text = strArr[0];
        //}
	}

    void Awake()
    {
        SceneFilterType[] filters = GameManager.Instance.loadFilters();
        if (filters != null)
        {
            for (int i = 0; i < filters.Length; ++i)
            {
                switch (filters[i])
                {
                    case SceneFilterType.SFT_Friend:
                        onlyFriend.value = true;
                        filterAll.value = false;
                        break;
                    case SceneFilterType.SFT_Team:
                        onlyTeam.value = true;
                        filterAll.value = false;
                        break;
                    case SceneFilterType.SFT_Guild:
                        onlyGuilde.value = true;
                        filterAll.value = false;
                        break;
                    case SceneFilterType.SFT_All:
                        filterAll.value = true;
                        onlyFriend.value = false;
                        onlyTeam.value = false;
                        onlyGuilde.value = false;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            filterAll.value = false;
            onlyFriend.value = false;
            onlyTeam.value = false;
            onlyGuilde.value = false;
        }
    }

	void Start () {
		InitUIText ();
		AccountsLabel.text = GamePlayer.Instance.InstName;
		serverLabel.text = GamePlayer.Instance.InstId.ToString ();
        //int GiftBtnState = 0;
        //GlobalValue.Get(Constant.C_GiftBtn, out GiftBtnState);
        duihuaBtn.gameObject.SetActive(GameManager.ServId_ != 999);
		UIManager.SetButtonEventHandler (duihuaBtn.gameObject, EnumButtonEvent.OnClick, OnClickduihuaBtn, 0, 0);
		UIManager.SetButtonEventHandler (gonggaoBtn.gameObject, EnumButtonEvent.OnClick, OnGongGao, 0, 0);
       // UIManager.SetButtonEventHandler(switchAccountBtn.gameObject, EnumButtonEvent.OnClick, OnUnReg, 0, 0);

	 //   if (game.GameUser.getInstance().isFunctionSupported("logout"))
		//{
		//	unRegBtn.gameObject.SetActive(true);
		//	UIManager.SetButtonEventHandler(unRegBtn.gameObject, EnumButtonEvent.OnClick, OnUnReg, 0, 0);
		//}else
		{
			unRegBtn.gameObject.SetActive(false);
		}




		UIManager.SetButtonEventHandler (musicOn.gameObject, EnumButtonEvent.OnClick, OnClickmusicOff, 0, 0);
		UIManager.SetButtonEventHandler (musicOff.gameObject, EnumButtonEvent.OnClick, OnClickmusicOn, 0, 0);
		UIManager.SetButtonEventHandler (SoundOn.gameObject, EnumButtonEvent.OnClick,  OnClickSoundOff, 0, 0);
		UIManager.SetButtonEventHandler (SoundOff.gameObject, EnumButtonEvent.OnClick, OnClickSoundOn, 0, 0);

		UIManager.SetButtonEventHandler (switchPlayerBtn.gameObject, EnumButtonEvent.OnClick, OnswitchAccount, 0, 0);

		UIManager.SetButtonEventHandler (Lbtn.gameObject, EnumButtonEvent.OnClick, OnClicL, 0, 0);
		UIManager.SetButtonEventHandler (Rbtn.gameObject, EnumButtonEvent.OnClick,  OnClickR, 0, 0);
		UIManager.SetButtonEventHandler (Ebtn.gameObject, EnumButtonEvent.OnClick, OnClickE, 0, 0);
		int lev = Convert2SpIdx(QualitySettings.GetQualityLevel ());
		qualitySp.spriteName = qualityLevel[lev];
		HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_, icon);
	}

	int Convert2QuaIdx(int spIdx)
	{
		switch(spIdx)
		{
		case 0:
			return 0;
		case 1:
			return 3;
		case 2:
		default:
			return 5;
		}
	}

	int Convert2SpIdx(int QuaIdx)
	{
		switch(QuaIdx)
		{
		case 0:
			return 0;
		case 1:
			return 0;
		case 2:
			return 0;
		case 3:
			return 1;
		case 4:
			return 1;
		case 5:
			return 2;
		default:
			return 0;
		}
	}

    void Update()
    {
        if (SoundTools.settingDirty)
        {
            SetMusicState(PlayerPrefs.GetInt("settingMusic") == 0);
            SetSoundState(PlayerPrefs.GetInt("settingSound") == 0);
        }
    }


	void OnClicL(ButtonScript obj, object args, int param1, int param2)
	{
		if (index == 0)
		{
			index = 0;
		}
		else
		{
			index--;
		}
		qualitySp.spriteName = qualityLevel[index];
	}
	void OnClickR(ButtonScript obj, object args, int param1, int param2)
	{
		if (index >= qualityLevel.Length-1)
		{
			index =qualityLevel.Length-1;
		}
		else
		{
			index++;
		}


		qualitySp.spriteName = qualityLevel[index];
	}
	void OnClickE(ButtonScript obj, object args, int param1, int param2)
	{
        int ql = Convert2QuaIdx(index);

        if ((ql == 0 && ql != GameManager.Instance.QualityLv) || (ql != 0 && GameManager.Instance.QualityLv == 0))
        {
            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("needRestart"), () =>
            {
                ConfirmQS(ql, false);
                Application.Quit();
            });
        }
        else
        {
            ConfirmQS(ql);
        }
	}

    void ConfirmQS(int lv, bool withSet = true)
    {
        if(withSet)
            QualitySettings.SetQualityLevel(lv, true);
        GameManager.Instance.QualityLv = lv;
        PlayerPrefs.SetString("UserSetQualityLevel", "set");
        PlayerPrefs.SetInt("QualityLevel", lv);
    }

	//公告是退出功能
	void OnGongGao(ButtonScript obj, object args, int param1, int param2)
	{
        //if (game.GameUser.getInstance().isFunctionSupported("exit"))
        //{
        //    game.GameUser.getInstance().callFuncWithParam("exit");
        //}
        //else
        {
            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("ConfirmQuit"), () =>
            {
                Application.Quit();
            });
        }
	}
	void OnUnReg(ButtonScript obj, object args, int param1, int param2)
	{
        //if (game.GameUser.getInstance().isFunctionSupported("logout"))
        //{
        //    game.GameUser.getInstance().callFuncWithParam("logout");
        //}
//        else
//        {
//            NetConnection.Instance.logout();
//            Hide();
//        }
	}
	
	void OnswitchAccount(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.logout();
		Hide();
	}
	void OnClickduihuaBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxInputUI.ShowMe (EnterLottery);
	}
	void OnClickmusicOn(ButtonScript obj, object args, int param1, int param2)
	{
		SetMusicState (true);
		SoundTools.OpenMusic ();
	}
	void OnClickmusicOff(ButtonScript obj, object args, int param1, int param2)
	{
		SetMusicState (false);
		SoundTools.SetStopMusic ();
	}
	void OnClickSoundOn(ButtonScript obj, object args, int param1, int param2)
	{
		SetSoundState (true);
		SoundTools.OpenSound ();
	}
	void OnClickSoundOff(ButtonScript obj, object args, int param1, int param2)
	{
		SetSoundState (false);
		SoundTools.StopSound ();
	}
    public void OnClickOnlyCheck()
    {
        List<SceneFilterType> filters = new List<SceneFilterType>();
        if (onlyFriend.value == true)
            filters.Add(SceneFilterType.SFT_Friend);

        if (onlyTeam.value == true)
            filters.Add(SceneFilterType.SFT_Team);

        if (onlyGuilde.value == true)
            filters.Add(SceneFilterType.SFT_Guild);

        if (filters.Count > 0)
            filterAll.value = false;

        NetConnection.Instance.sceneFilter(filters.ToArray());
        GameManager.Instance.saveFilters(filters.ToArray());
    }
    
    public void OnClickFilterAll()
    {
        List<SceneFilterType> filters = new List<SceneFilterType>();
        if (filterAll.value == true)
        {
            onlyFriend.value = false;
            onlyTeam.value = false;
            onlyGuilde.value = false;
            filters.Add(SceneFilterType.SFT_All);
        }
        NetConnection.Instance.sceneFilter(filters.ToArray());
        GameManager.Instance.saveFilters(filters.ToArray());
    }
	void EnterLottery(string str)
	{
        if (string.IsNullOrEmpty(str))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("emptyStr"), PopText.WarningType.WT_Warning);
            return;
        }

        if (BagSystem.instance.GetEmptySlotNum() <= 0)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("beibaokongjianbuzu"), PopText.WarningType.WT_Warning);
            return;
        }
		NetConnection.Instance.redemptionSpree (str);
	}
	
	private void SetMusicState(bool isOn)
	{
		musicOn.gameObject.SetActive (isOn);
		musicOff.gameObject.SetActive (!isOn);
	}
	private void SetSoundState(bool isOn)
	{
		SoundOn.gameObject.SetActive (isOn);
		SoundOff.gameObject.SetActive (!isOn);
	}

	public void CloseSelf()
	{
		Hide ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Setpanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_Setpanel);
	}
	public override void Destroyobj ()
	{
        HeadIconLoader.Instance.Delete(EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_Setpanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
}
