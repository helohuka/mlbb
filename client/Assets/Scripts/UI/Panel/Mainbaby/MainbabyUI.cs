using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabyUI : UIBase {


	public UILabel _TitleLable;
	public UILabel _ZDLLable;
	public UILabel _SkillLable;
	public UILabel _NameLable;
	public UILabel _LevelLable;
	public UILabel _ExpLable;
	public UILabel _ZCLable;
	public UILabel _DILable;
	public UILabel _ShuiLable;
	public UILabel _HuoLable;
	public UILabel _FengLable;
	public UILabel _CangKLable;
	public UILabel _FSLable;
	public UILabel _StateLable;
	public UILabel _ProLable;
	public UILabel _HyLable;
	public UILabel _QHLable;
	public UILabel _GZLable;
	public UILabel _TuJLable;

	public delegate void offmake();
	public static offmake OnoffmakeOk;

	public delegate void UpdateBabyInfo();
	public static UpdateBabyInfo UpdateBabyInfoOk;


	public UIButton stateBtn;
	public UIButton PropertyBtn;
	public UIButton reductionBtn;
	public UIButton ReformBtn;
	public UIButton zhuangbeiBtn;
	public UIButton closeBtn;
	public GameObject stateUI;
	public UILabel tishiLabel;
	public static MainbabyUI MainbabyIns;
	public GameObject zhongzuObj;
	public GameObject listObj;
	private List<UIButton> btns = new List<UIButton>();
	public GameObject leftObj;
	public bool isState = true;
	public UIButton tujianBtn;
	public UIButton qianghuaBtn;
    string subUiResPath;
	public delegate void UpdateTabelBtnState();
	public static UpdateTabelBtnState UpdateTabelBtnStateOk;
    bool hasDestroy = false;

	private List<int> loadUIList = new List<int>();

	void Awake()
	{
		MainbabyIns = this;
        hasDestroy = false;
	}
	public static MainbabyUI Instance
	{
		get
		{
			return MainbabyIns;	
		}
	}

	void Start () {
		InitUIText ();
        subUIs_ = new List<string>();
		btns.Add (stateBtn);
		btns.Add (PropertyBtn);
		btns.Add (reductionBtn);
		btns.Add (qianghuaBtn);
		btns.Add (ReformBtn);
		btns.Add (tujianBtn);
		btns.Add (zhuangbeiBtn);
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_PetEquip))
		{
			zhuangbeiBtn.gameObject.SetActive(false);
		}else
		{
			zhuangbeiBtn.gameObject.SetActive(true);
		}
		isState = true;
		UIManager.SetButtonEventHandler (stateBtn.gameObject, EnumButtonEvent.OnClick,OnClickbtn, 0, 0);
		UIManager.SetButtonEventHandler (PropertyBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn, 1, 0);
		UIManager.SetButtonEventHandler (reductionBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn, 2, 0);
		UIManager.SetButtonEventHandler (qianghuaBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn, 3, 0);
		UIManager.SetButtonEventHandler (ReformBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn, 4, 0);
		UIManager.SetButtonEventHandler (tujianBtn.gameObject, EnumButtonEvent.OnClick, OnClicktujianBtn, 5, 0);
		UIManager.SetButtonEventHandler (zhuangbeiBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn,6, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickTColse, 0, 0);
		TabsSelect (0);
		tishiLabel.gameObject.SetActive(false);
		//ShowTabsSelectInfo (0);
		//tishiLabel.gameObject.SetActive (false);
        GuideManager.Instance.RegistGuideAim(stateBtn.gameObject, GuideAimType.GAT_MainBabyStatusBtn);
        GuideManager.Instance.RegistGuideAim(PropertyBtn.gameObject, GuideAimType.GAT_MainBabyPropertyBtn);
        GuideManager.Instance.RegistGuideAim(closeBtn.gameObject, GuideAimType.GAT_MainBabyClose);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyUIOpen);


		GamePlayer.Instance.babyUpdateIpropEvent += new RequestEventHandler<int>(markBabyOn);
		MainbabyListUI.UpdateBabyMarkOnOk += ClearText;
		GamePlayer.Instance.OnBabyUpdate += Babypdate;
		MainbabyListUI.RefreshBabyListOk += SetBabyInfoState;

		markBabyOn (0);
		Babypdate ();
		MainbabyState mstate = stateUI.GetComponent<MainbabyState>();
		if(GamePlayer.Instance.babies_list_.Count>0)
		{
			mstate.UpDateBabyInfo(GamePlayer.Instance.babies_list_[0].InstId);
			//tishiLabel.gameObject.SetActive(false);
		}


		UpdateTabelBtnStateOk = UpdateTabelBtn;
		tableObj.Add (stateUI);
		tableObj.Add (posObj);
		tableObj.Add (reductionObj);
		tableObj.Add (ReformObj);

    }
	void ClearText(int binsId)
	{
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(binsId == GamePlayer.Instance.babies_list_[i].InstId&&GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_Free)>0)
			{
				MarkOn();
				return;
			}
		}
		
		MarkOff();
	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("mainbaby_tit");
		//_ZDLLable.text = LanguageManager.instance.GetValue("mainbaby_ZDL");
		_SkillLable.text = LanguageManager.instance.GetValue("mainbaby_JN");
		_NameLable.text = LanguageManager.instance.GetValue("mainbaby_MZ");
		_LevelLable.text = LanguageManager.instance.GetValue("mainbaby_Level");
		_ExpLable.text = LanguageManager.instance.GetValue("mainbaby_EXP");
		_ZCLable.text = LanguageManager.instance.GetValue("mainbaby_ZC");
		_DILable.text = LanguageManager.instance.GetValue("mainbaby_DI");
		_ShuiLable.text = LanguageManager.instance.GetValue("mainbaby_Shui");
		_HuoLable.text = LanguageManager.instance.GetValue("mainbaby_Huo");
		_FengLable.text = LanguageManager.instance.GetValue("mainbaby_Feng");
		_CangKLable.text = LanguageManager.instance.GetValue("mainbaby_bank");
		_FSLable.text = LanguageManager.instance.GetValue("mainbaby_fangsheng");
		_StateLable.text = LanguageManager.instance.GetValue("mainbaby_state");
		_ProLable.text = LanguageManager.instance.GetValue("mainbaby_shuxing");
		_HyLable.text = LanguageManager.instance.GetValue("mainbaby_huanyuan");
		_QHLable.text = LanguageManager.instance.GetValue("mainbaby_qianhua");
		_GZLable.text = LanguageManager.instance.GetValue("mainbaby_GZ");
		_TuJLable.text = LanguageManager.instance.GetValue("mainbaby_TJ");
	
	
	}





	void UpdateTabelBtn()
	{
		TabsSelect (0);
		OnClickbtn (null,null,0,0);
	}
	void SetBabyInfoState(int binsId)
	{
		markBabyOn (0);
		Babypdate();

	}
	bool IsdepositBabyOK(int insId)
	{
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].InstId==insId)
			{
				return true;
			}
		}
		return false;
	}
	void Babypdate()
	{
	    if(GamePlayer.Instance.babies_list_.Count==0)
		{

			tishiLabel.gameObject.SetActive(true);

//			if(batstate == 4)
//				return;
//			tishiLabel.gameObject.SetActive(true);
//			stateUI.SetActive (false);
//			if(posObj!=null)
//			posObj.SetActive(false);
//			if(reductionObj!=null)
//			reductionObj.SetActive(false);
//			if(ReformObj!=null)
//			ReformObj.SetActive(false);
			if(MainbabyListUI.babyObj != null)
				MainbabyListUI.babyObj.SetActive(false);
//
//			zhongzuObj.SetActive(false);
		}else
		{
			tishiLabel.gameObject.SetActive(false);
			zhongzuObj.SetActive(true);
			if(isState)
			{
							if(stateUI!=null)
							stateUI.SetActive (true);
				if(BankSystem.instance.isopen)
				{
					if(MainbabyListUI.babyObj != null)
						MainbabyListUI.babyObj.SetActive(false);
				}else
				{
					if(MainbabyListUI.babyObj != null)
						MainbabyListUI.babyObj.SetActive(true);
				}

			}else
			{
				if(batstate ==1)
				{
					if(posObj!=null && !posObj.activeSelf)
						posObj.SetActive(true);
				}else
					if(batstate ==2)
				{
					if(reductionObj!=null&& !reductionObj.activeSelf)
						reductionObj.SetActive(true);
				}else
					if(batstate ==3)
				{
					if(QianghuaObj != null&& !QianghuaObj.activeSelf)
					{
						QianghuaObj.SetActive(true);
						
					}
				}else
					if(batstate ==4)
				{
					if(ReformObj!=null&& !ReformObj.activeSelf)
						ReformObj.SetActive(true);
				}

				if(stateUI!=null)
					stateUI.SetActive (false);
				if(MainbabyListUI.babyObj != null)
				MainbabyListUI.babyObj.SetActive(false);






			}
		}
	}
	int batstate = 0;
	void OnClicktujianBtn(ButtonScript obj, object args, int param1, int param2)
	{
		TabsSelect (param1);
		batstate = 5;
		if(MainbabyListUI.babyObj !=null)
		MainbabyListUI.babyObj.SetActive (false);
		TuJianUI.ShowMe ();
	}
	List<GameObject>tableObj = new List<GameObject>();
	void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(OnoffmakeOk != null)
		{
			OnoffmakeOk();
		}
		if(UpdateBabyInfoOk != null)
		{
			UpdateBabyInfoOk();
		}
		TabsSelect (param1);

		if(param1 == 0)
		{
			isState = true;
			tishiLabel.gameObject.SetActive(true);
			if(MainbabyListUI.babyObj != null)
			MainbabyListUI.babyObj.SetActive (true);
			zhongzuObj.SetActive (true);
			listObj.SetActive(true);
			stateUI.SetActive (true);
			if(posObj !=null)
				posObj.SetActive(false);
			if(reductionObj!= null)
				reductionObj.SetActive(false);
			if(ReformObj != null)
				ReformObj.SetActive(false);
			if (posObj != null)
			{
				posObj.SetActive(false);
			}
			if(epuObj != null)
			{
				epuObj.SetActive(false);
			}
			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(false);				
			}
			NetWaitUI.HideMe();
			Babypdate ();
			MainbabyReformUI.isMainbabyReformUI = false;
		}else
			if(param1 == 1)
		{
            
			batstate = 1;
			Babypdate ();
			isState = false;
			tishiLabel.gameObject.SetActive(false);
			if(MainbabyListUI.babyObj != null)
			MainbabyListUI.babyObj.SetActive (false);
			zhongzuObj.SetActive (false);
			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(false);				
			}
			if(reductionObj!= null)
				reductionObj.SetActive(false);
			if(ReformObj != null)
				ReformObj.SetActive(false);
			if(epuObj != null)
			{
				epuObj.SetActive(false);
			}
			stateUI.SetActive (false);
			listObj.SetActive(true);
			if(posObj != null)
			{
				posObj.SetActive(true);
                NetWaitUI.HideMe();
			
			}else
			{
				NetWaitUI.ShowMe();
				LoadUI (UIASSETS_ID.UIASSETS_mainbabyshuxingPanel,1);
				loadUIList.Add((int)UIASSETS_ID.UIASSETS_mainbabyshuxingPanel);
			}
			MainbabyReformUI.isMainbabyReformUI = false;
		}else
			if(param1 == 2)
		{
           
			batstate = 2;
			isState = false;
			tishiLabel.gameObject.SetActive(false);
			if(MainbabyListUI.babyObj != null)
			MainbabyListUI.babyObj.SetActive (false);
			if(posObj!= null)
				posObj.SetActive(false);
			if(ReformObj != null)
				ReformObj.SetActive(false);
			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(false);				
			}
			if(epuObj != null)
			{
				epuObj.SetActive(false);
			}
			stateUI.SetActive (false);
			listObj.SetActive(true);
		
			if(reductionObj != null)
			{
				reductionObj.SetActive(true);
                NetWaitUI.HideMe();
			
			}else
			{
				NetWaitUI.ShowMe();
				LoadUI (UIASSETS_ID.UIASSETS_huanyuanPanel,2);
				loadUIList.Add((int)UIASSETS_ID.UIASSETS_huanyuanPanel);
			}
			MainbabyReformUI.isMainbabyReformUI = false;

		}else if(param1 == 3)
		{
           
			batstate = 3;
			isState = false;
			tishiLabel.gameObject.SetActive(false);
			if(MainbabyListUI.babyObj != null)
				MainbabyListUI.babyObj.SetActive (false);
			if(posObj!= null)
				posObj.SetActive(false);
			if(ReformObj != null)
				ReformObj.SetActive(false);
			stateUI.SetActive (false);
			if(epuObj != null)
			{
				epuObj.SetActive(false);
			}
			if(reductionObj != null)
			{
				reductionObj.SetActive(false);
			}
			listObj.SetActive(true);

			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(true);
                NetWaitUI.HideMe();
				
			}else
			{
				NetWaitUI.ShowMe();
				LoadUI (UIASSETS_ID.UIASSETS_QianghuaPanel,3);
			}
			MainbabyReformUI.isMainbabyReformUI = false;
		}else
			if(param1 == 4)
		{
           
			batstate = 4;
			isState = false;
			tishiLabel.gameObject.SetActive(false);
			if(MainbabyListUI.babyObj != null)
			MainbabyListUI.babyObj.SetActive (false);

			if(epuObj != null)
			{
				epuObj.SetActive(false);
			}

			if(posObj!= null)
				posObj.SetActive(false);
			if(reductionObj != null)
				reductionObj.SetActive(false);
			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(false);				
			}
			stateUI.SetActive (false);
			listObj.SetActive(false);

			if(ReformObj != null)
			{
				ReformObj.SetActive(true);
                NetWaitUI.HideMe();

			}else
			{
				NetWaitUI.ShowMe();
				LoadUI (UIASSETS_ID.UIASSETS_babygaizaoPanel,4);
				loadUIList.Add((int)UIASSETS_ID.UIASSETS_babygaizaoPanel);
			}
			MainbabyReformUI.isMainbabyReformUI = true;
		}else
		if(param1 == 6)
		{
			
			batstate = 6;
			isState = false;
			tishiLabel.gameObject.SetActive(false);
			if(MainbabyListUI.babyObj != null)
				MainbabyListUI.babyObj.SetActive (false);
			if(posObj!= null)
				posObj.SetActive(false);
			if(reductionObj != null)
				reductionObj.SetActive(false);
			if(QianghuaObj != null)
			{
				QianghuaObj.SetActive(false);				
			}
			if(ReformObj != null)
				ReformObj.SetActive(false);
			stateUI.SetActive (false);
			listObj.SetActive(true);
			
			if(epuObj != null)
			{
				epuObj.SetActive(true);
				NetWaitUI.HideMe();
				
			}else
			{
				NetWaitUI.ShowMe();
				LoadUI (UIASSETS_ID.UIASSETS_BabyEquPanle,6);
				loadUIList.Add((int)UIASSETS_ID.UIASSETS_BabyEquPanle);
			}
			MainbabyReformUI.isMainbabyReformUI = false;
		}
	}
	void SwitchTabels(int index)
	{

	}
//	void OnClickstate(ButtonScript obj, object args, int param1, int param2)
//	{
//		TabsSelect (0);
//		isState = true;
//		MainbabyListUI.babyObj.SetActive (true);
//		zhongzuObj.SetActive (true);
//		stateBtn.GetComponent<BoxCollider> ().enabled = false;
//		stateUI.SetActive (true);
//		if (posObj != null)
//		{
//			posObj.SetActive(false);
//		}
//		Babypdate ();
//	}
//	void OnClickProperty(ButtonScript obj, object args, int param1, int param2)
//	{
//		Babypdate ();
//		TabsSelect (1);
//		isState = false;
//		MainbabyListUI.babyObj.SetActive (false);
//		zhongzuObj.SetActive (false);
//		stateBtn.GetComponent<BoxCollider> ().enabled = false;
//		if (GamePlayer.Instance.babies_list_.Count == 0)
//			return;
//		if(posObj != null)
//		{
//			posObj.SetActive(true);
//			stateBtn.GetComponent<BoxCollider> ().enabled = true;
//		}else
//		{
//			LoadUI (UIASSETS_ID.UIASSETS_mainbabyshuxingPanel);
//		}
//		stateUI.SetActive (false);
//
//	}

	void OnClickTColse(ButtonScript obj, object args, int param1, int param2)
    {
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
 	       GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyUIClose);
			Hide ();
		});
	}

	void markBabyOn(int num)
	{
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_Free)>0)
			{
				MarkOn();
				return;
			}
		}

		MarkOff();

	}

	public void markBabyOff()
	{
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].Properties[(int)PropertyType.PT_Free]==0)
			{
				MarkOff();
			}
		}
	}

	public void MarkOn()
	{
		PropertyBtn.GetComponentInChildren<UISprite> ().MarkOn (UISprite.MarkAnthor.MA_RightTop,-10f,-10f);
	}
	public void MarkOff()
	{
		PropertyBtn.GetComponentInChildren<UISprite> ().MarkOff ();
	}

	void TabsSelect(int index)
	{
		for (int i = 0; i<btns.Count; i++) 
		{
			if(i==index)
			{
//				UILabel la = btns[i].transform.FindChild("Label").GetComponentInParent<UILabel>(); 
//
//				la.color = new Color(9,94,0);
				btns[i].isEnabled = false;
			}
			else
			{
				btns[i].isEnabled = true;
//				UILabel la =	btns[i].transform.FindChild("Label").GetComponentInParent<UILabel>(); 
//				la.color =  new Color(139,178,200);

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
	private GameObject reductionObj;
	private GameObject ReformObj;
	private GameObject QianghuaObj;
	private GameObject epuObj;
    List<string> subUIs_;
	private void LoadUI(UIASSETS_ID id,int num)
	{
        subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
        if (subUIs_ == null)
        {
            subUIs_ = new List<string>();
            subUIs_.Add(subUiResPath);
        }
        else
        {
            if(!subUIs_.Contains(subUiResPath))
                subUIs_.Add(subUiResPath);
        }

        AssetLoader.LoadAssetBundle(subUiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
        {
            if (hasDestroy)
            {
                //AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
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
			go.transform.position = Vector3.zero;
			go.transform.localScale = Vector3.one;
			if(num ==1)
			{
				posObj = go;
	            if(PropertyBtn.isEnabled)
				{
					posObj.SetActive(false);
				}
			}else
				if(num ==2)
			{
				reductionObj = go;
				if(reductionBtn.isEnabled)
				{
					reductionObj.SetActive(false);
				}
			}
			else
				if(num == 3)
			{
				QianghuaObj = go;
				if(qianghuaBtn.isEnabled)
				{
					QianghuaObj.SetActive(false);
				}
			}
			else
				if(num == 4)
			{
				ReformObj = go;
				if(ReformBtn.isEnabled)
				{
					ReformObj.SetActive(false);
				}
			}
			else
				if(num == 6)
			{
				epuObj = go;
				if(zhuangbeiBtn.isEnabled)
				{
					epuObj.SetActive(false);
				}
			}
			UIManager.Instance.AdjustUIDepth(go.transform,true,1000);
            NetWaitUI.HideMe();
           // UIManager.Instance.AdjustUIDepth(go.transform);
			//stateBtn.GetComponent<BoxCollider> ().enabled = true;
		}
		, null);
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_chongwuPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_chongwuPanel);
	}


	void OnDestroy()
	{
        btns.Clear();
        UIManager.RemoveButtonEventHandler(stateBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(PropertyBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainBabyStatusBtn);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainBabyPropertyBtn);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainBabyClose);
		GamePlayer.Instance.OnBabyUpdate -= Babypdate;
		MainbabyListUI.RefreshBabyListOk -= SetBabyInfoState;
		MainbabyListUI.UpdateBabyMarkOnOk -= ClearText;
		GamePlayer.Instance.babyUpdateIpropEvent -= markBabyOn;
		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			if(AttaclEvent.getInstance.OnSetPanelActive != null)
			{
				AttaclEvent.getInstance.OnSetPanelActive(true);
			}
		}
	}
	public override void Destroyobj ()
	{
        for (int i = 0; i < subUIs_.Count; ++i)
        {
            AssetInfoMgr.Instance.DecRefCount(subUIs_[i], true);
        }

        //for(int j =0;j<loadUIList.Count;j++)
        //{
        //    AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)loadUIList[j], AssetLoader.EAssetType.ASSET_UI), true);
        //}


        subUiResPath = "";
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_chongwuPanel, AssetLoader.EAssetType.ASSET_UI), true);
		UpdateTabelBtnStateOk = null;
        hasDestroy = true;
	}
}
