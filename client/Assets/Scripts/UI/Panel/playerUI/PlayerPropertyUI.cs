using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerPropertyUI : UIBase {




	public UILabel _NameLabel;
	public UILabel _ProfessionLable;
	public UILabel _RaceLable;
	public UILabel _FamilyLable;
	public UILabel _ExperienceLable;
	public UILabel _RecordLable;
	public UILabel _FightingLable;
	public UILabel _PrestigeLable;
	public UILabel _LevelLable;
	public UILabel _HpLable;
	public UILabel _MpLable;
	public UILabel _LandLable;
	public UILabel _WaterLable;
	public UILabel _FireLable;
	public UILabel _WindLable;
	public UILabel _StationsLable;
	public UILabel _FrontLable;
	public UILabel _BackRowLable;
	public UILabel _StateLable;
	public UILabel _PorLable;
	public UILabel _TitlePorLable;

	public UIButton stateBtn;
	public UIButton PropertyBtn;
	public UIButton closeBtn;
	public GameObject StateUI;
	public UITexture icon;
	public UILabel zhuangTaiLabel;
	public UILabel shuxingLabel;

    string subUiResPath;

	private List<UIButton> btns = new List<UIButton>();

    bool hasDestroy_ = false;
	void IinitUIText()
	{
		_NameLabel.text = LanguageManager.instance.GetValue ("playerPro_Name");
		_ProfessionLable.text = LanguageManager.instance.GetValue ("playerPro_Profession");
		_RaceLable.text = LanguageManager.instance.GetValue ("playerPro_Race");
		_FamilyLable.text = LanguageManager.instance.GetValue("playerPro_Family");
		_ExperienceLable.text = LanguageManager.instance.GetValue("playerPro_Experience");
		_RecordLable.text = LanguageManager.instance.GetValue("playerPro_Record");
		_FightingLable.text = LanguageManager.instance.GetValue("playerPro_Fighting"); 
		_PrestigeLable.text = LanguageManager.instance.GetValue("playerPro_Prestige");
		_LevelLable.text = LanguageManager.instance.GetValue("playerPro_Level");
		_HpLable.text = LanguageManager.instance.GetValue("playerPro_Hp");
		_MpLable.text = LanguageManager.instance.GetValue("playerPro_Mp");
		_LandLable.text = LanguageManager.instance.GetValue("playerPro_Land");
		_WaterLable.text = LanguageManager.instance.GetValue("playerPro_Water");
		_FireLable.text = LanguageManager.instance.GetValue("playerPro_Fire");
		_WindLable.text = LanguageManager.instance.GetValue("playerPro_Wind");
		_StationsLable.text = LanguageManager.instance.GetValue("playerPro_Stations");
		_FrontLable.text = LanguageManager.instance.GetValue("playerPro_Front");
		_BackRowLable.text = LanguageManager.instance.GetValue("playerPro_BackRow");
		_StateLable.text = LanguageManager.instance.GetValue("playerPro_State");
		_PorLable.text = LanguageManager.instance.GetValue("playerPro_Por");
		_TitlePorLable.text =LanguageManager.instance.GetValue("playerPro_TitlePor");
	}
	void Start () {

		IinitUIText ();
        hasDestroy_ = false;

		btns.Add (stateBtn);
		btns.Add (PropertyBtn);
		UIManager.SetButtonEventHandler (stateBtn.gameObject, EnumButtonEvent.OnClick, OnClickstate, 0, 0);
		UIManager.SetButtonEventHandler (PropertyBtn.gameObject, EnumButtonEvent.OnClick, OnClickProperty, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickTColse, 0, 0);
		TabsSelect (0);
		//ShowTabsSelectInfo (0);
		PlayerData pdata = PlayerData.GetData ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_TableId]);
		EntityAssetsData enData = EntityAssetsData.GetData (pdata.lookID_);

		HeadIconLoader.Instance.LoadIcon ("R_"+enData.assetsIocn_,icon);

		//PlayerAsseMgr.LoadAsset ((ENTITY_ID)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId], AssetLoadCallBack, null);

        GuideManager.Instance.RegistGuideAim(closeBtn.gameObject, GuideAimType.GAT_MainPlayerInfoClose);
        GuideManager.Instance.RegistGuideAim(stateBtn.gameObject, GuideAimType.GAT_MainPlayerInfoStatusBtn);
        GuideManager.Instance.RegistGuideAim(PropertyBtn.gameObject, GuideAimType.GAT_MainPlayerInfoPropertyBtn);
		GameManager.Instance.UpdatePlayermake += UpdatePlayermakeOk;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerUIOpen);
		if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Free] > 0)
		{
			PropertyBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10f,-15f);
		}else
		{
			PropertyBtn.GetComponentInChildren<UISprite>().MarkOff();
		}
	}
	void UpdatePlayermakeOk()
	{
		if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Free] > 0)
		{
			PropertyBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10f,-15f);
		}else
		{
			PropertyBtn.GetComponentInChildren<UISprite>().MarkOff();
		}
	}
//	void AssetLoadCallBack(AssetBundle bundle, ParamData data)
//	{
//		GameObject ro = GameObject.Instantiate (bundle.mainAsset)as GameObject;
//		ro.layer = LayerMask.NameToLayer("3D");
//		for(int i = 0; i<ro.transform.childCount;i++)
//		{
//			ro.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("3D");
//		}
//		ro.transform.parent = modelPos;
//		ro.transform.localPosition = Vector3.zero;
//		ro.transform.LookAt (ApplicationEntry.Instance.ui3DCamera.transform.position);
//		
//	}
	void OnClickstate(ButtonScript obj, object args, int param1, int param2)
	{
		TabsSelect (0);
		stateBtn.GetComponent<BoxCollider> ().enabled = false;
		StateUI.SetActive (true);
		if (posObj != null)
		 posObj.SetActive (false);
		//ShowTabsSelectInfo (param1);
	}
	void OnClickProperty(ButtonScript obj, object args, int param1, int param2)
	{
		TabsSelect (1);
		StateUI.SetActive (false);
		stateBtn.GetComponent<BoxCollider> ().enabled = false;
		//ShowTabsSelectInfo (param1);
		if (posObj != null)
		{
			stateBtn.GetComponent<BoxCollider> ().enabled = true;
			posObj.SetActive(true);
		}else
		{
			LoadUI (UIASSETS_ID.UIASSETS_PlayerRightShuxing);
		}

	}
	void OnClickTColse(ButtonScript obj, object args, int param1, int param2)
	{
//        for (int i = 0; i < tabsUI.Length; i++)
//        {
//            UIBase ui = tabsUI[i].GetComponent<UIBase>();
//            if(ui != null)
//                ui.Destroyobj();
//        }
		Hide ();
        if (GamePlayer.Instance.BattleBaby != null && (GamePlayer.Instance.BattleBaby.isLevelUp_ || GamePlayer.Instance.hasBabyLevelUp))
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLevelUp);
            GamePlayer.Instance.hasBabyLevelUp = false;
        }
        else
            GuideManager.Instance.ClearGuideAim();
	}
	
	
	void TabsSelect(int index)
	{
		for (int i = 0; i<btns.Count; i++) 
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}
			else
			{
				btns[i].isEnabled = true;
			}
		}
	}
//	void ShowTabsSelectInfo(int index)
//	{
//		for(int i = 0;i<tabsUI.Length;i++)
//		{
//			if(i==index)
//			{
//				tabsUI[i].SetActive(true);
//			}
//			else
//			{
//				tabsUI[i].SetActive(false);
//			}
//		}
//	}
	private GameObject posObj;
	private void LoadUI(UIASSETS_ID id)
	{
        subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);

        AssetLoader.LoadAssetBundle(subUiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
        {
            if (hasDestroy_)
            {
                AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
                return;
            }
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf)
			{
				Destroy(go);
			}
			go.transform.parent = this.panel.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;

            posObj = go;
			stateBtn.GetComponent<BoxCollider> ().enabled = true;
            AssetInfoMgr.Instance.DecRefCount(subUiResPath, false);
		}
		, null);
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_PlayerInfoPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_PlayerInfoPanel);
	}

	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			if(AttaclEvent.getInstance.OnSetPanelActive != null)
			{
				AttaclEvent.getInstance.OnSetPanelActive(true);
			}
		}

        hasDestroy_ = true;
		GameManager.Instance.UpdatePlayermake -= UpdatePlayermakeOk;
    }
}
