using UnityEngine;
using System.Collections;

public class EmployessControlUI :UIBase
{
	public UIButton closeBtn;
	public UIButton tavernBtn;
	public UIButton posBtn;
	public UIButton listBtn;
	public UIButton soulBtn;
	public UIButton tujianBtn;
	public GameObject tavernObj;
	public GameObject posObj;
	public GameObject listObj;
	public GameObject soulObj;
	public GameObject tujianObj;
	public UISprite listBtnImg;
	public UISprite tavernBtnImg;
	public UISprite posBtnImg;

	public UILabel tabBtnListLab;
	public UILabel tabBtnPosLab;
	public UILabel tabBtnTavernLab;

	private static int panelId;
	void Start ()
	{
		//
		//EventMgr.Instance.RegisterEvent(EEventType.EEventType_UpdateEmployessJiuGuanUI , UpdateJIuGuanTabUI );
		//EventMgr.Instance.RegisterEvent(EEventType.EEventType_UpdateEmployessShangZhenUI , UpdateShangZhenTabUI );
		//EventMgr.Instance.RegisterEvent(EEventType.EEventType_UpdateEmployessLiebiaoUI , UpdateLieBiaoTabUI );

        hasDestroy = false;
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (tavernBtn.gameObject, EnumButtonEvent.OnClick, OnClickTavern, 0, 0);
		UIManager.SetButtonEventHandler (listBtn.gameObject, EnumButtonEvent.OnClick, OnClickList, 0, 0);
		UIManager.SetButtonEventHandler (posBtn.gameObject, EnumButtonEvent.OnClick, OnClickPos, 0, 0);
		UIManager.SetButtonEventHandler (soulBtn.gameObject, EnumButtonEvent.OnClick, OnClickSoul, 0, 0);
		UIManager.SetButtonEventHandler (tujianBtn.gameObject, EnumButtonEvent.OnClick, OnClickTujian, 0, 0);


		tavernBtn.isEnabled = false;

		GamePlayer.Instance.DelEmployeeEnvent += new RequestEventHandler<uint> (OnDelEmployeeEvolveOk);
		EmployessSystem.instance.employeeRedEnvent += new RequestEventHandler<int> (OnEmployeeRedEvent);


		tabBtnListLab.text = LanguageManager.instance.GetValue("tabBtnListLab");
		tabBtnPosLab.text = LanguageManager.instance.GetValue("tabBtnPosLab");
		tabBtnTavernLab.text = LanguageManager.instance.GetValue("tabBtnTavernLab");


		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{
		
			if(panelId == 1)
			{
				if(GamePlayer.Instance.EmployeeList.Count>0&&GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>12)
				{
					posBtn.isEnabled = true;
					tavernBtn.isEnabled = true;
					listBtn.isEnabled = false;
					tavernObj.gameObject.SetActive (false);
					if(posObj!=null)
					{
						posObj.gameObject.SetActive(false);
					}
					listObj.gameObject.SetActive(true);
					listObj.GetComponent<EmployeeControlListUI> ().AddScrollViewItem (GamePlayer.Instance.EmployeeList);
				}
				else
				{
					posBtn.isEnabled = true;
					tavernBtn.isEnabled = false;
					listBtn.isEnabled = true;
					tavernObj.gameObject.SetActive (true);
				}
			}
			else if(panelId ==2)
			{
				posBtn.isEnabled = false;
				tavernBtn.isEnabled = true;
				listBtn.isEnabled = true;

				tavernObj.SetActive(false);
				listObj.SetActive(false);

				if(posObj!=null)
				{
					if(posObj.gameObject.activeSelf)
					{
						return;
					}
					posObj.gameObject.SetActive(true);
					posObj.GetComponent<PartnerPostion>().UpdataPanel();
				}
				else
				{
					LoadUI (UIASSETS_ID.UIASSETS_EmployeePosPanel);
				}
			}
			else if(panelId ==3)
			{
				posBtn.isEnabled = true;
				tavernBtn.isEnabled = true;
				listBtn.isEnabled = false;
				tavernObj.gameObject.SetActive (false);
				if(posObj!=null)
				{
					posObj.gameObject.SetActive(false);
				}
				listObj.gameObject.SetActive(true);
				listObj.GetComponent<EmployeeControlListUI> ().AddScrollViewItem (GamePlayer.Instance.EmployeeList);
			}

			EmployessSystem.instance.UpdateEmployeeRed ();
			UIManager.Instance.LoadMoneyUI(this.gameObject);
		});

        GuideManager.Instance.RegistGuideAim(posBtn.gameObject, GuideAimType.GAT_PartnerPositionTab);
	}

	void Update ()
	{
		if(EmployessSystem.instance.GetBattleEmpty())
		{
			posBtnImg.MarkOn(UISprite.MarkAnthor.MA_RightTop,-25,-20);
		}
		else
		{
			posBtnImg.MarkOff();
		}
		if(BoxSystem.Instance.GreenCDTime <= 0 && BoxSystem.Instance.FreeNum > 0)
		{
			tavernBtnImg.GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-25,-20);
			return;
		}
		else if(BoxSystem.Instance.BlueCDTime <= 0)
		{
			tavernBtnImg.GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-25,-20);
			return;
		}
		else
		{
			tavernBtnImg.GetComponent<UISprite>().MarkOff();
		}

	

	}

	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe(int pid = 1)
	{
		panelId = pid;
		if(!GamePlayer.Instance.isInitEmployees)
		{
			EmployessSystem.instance.openEmployeeType = pid;
			EmployessSystem.instance.openUIEmployee = true;
			NetConnection.Instance.requestEmployees();
            NetWaitUI.ShowMe();
		}
		else
		{
			UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_HuobanPanel);
		}
	}
	
	public static void ShowMe(int pid = 1)
	{
		panelId = pid;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_HuobanPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_HuobanPanel);
	}


	#endregion

	private void OnClickTavern(ButtonScript obj, object args, int param1, int param2)
	{
		tavernBtn.isEnabled = false;
		posBtn.isEnabled = true;
		listBtn.isEnabled = true;
		soulBtn.isEnabled = true;
		tujianBtn.isEnabled = true;
		tujianObj.gameObject.SetActive (false);
		soulObj.gameObject.SetActive (false);
		listObj.gameObject.SetActive(false);

		if(posObj!= null)
		{
			posObj.gameObject.SetActive(false);
		}
		tavernObj.gameObject.SetActive (true);
		//EventMgr.Instance.PushEvent ( EEventType.EEventType_UpdateEmployessJiuGuanUI , tavernObj);
	}

	private void OnClickList(ButtonScript obj, object args, int param1, int param2)
	{
		tavernBtn.isEnabled = true;
		posBtn.isEnabled = true;
		listBtn.isEnabled = false;
		tavernObj.gameObject.SetActive (false);
		soulBtn.isEnabled = true;
		tujianBtn.isEnabled = true;
		tujianObj.gameObject.SetActive (false);
		soulObj.gameObject.SetActive (false);
		if(posObj!= null)
		{
			posObj.gameObject.SetActive(false);
		}

		listObj.SetActive ( true );
		listObj.GetComponent<EmployeeControlListUI> ().AddScrollViewItem (GamePlayer.Instance.EmployeeList);
		//listObj.SetActive ( false );

		//EventMgr.Instance.PushEvent ( EEventType.EEventType_UpdateEmployessLiebiaoUI , listObj );
		//listObj.gameObject.SetActive (true);
	//	listObj.GetComponent<EmployeeControlListUI> ().AddScrollViewItem (GamePlayer.Instance.EmployeeList);

	}
	
	private void OnClickPos(ButtonScript obj, object args, int param1, int param2)
	{
		posBtn.isEnabled = false;
		tavernBtn.isEnabled = true;
		listBtn.isEnabled = true;
		tujianBtn.isEnabled = true;
		tujianObj.gameObject.SetActive (false);
		soulBtn.isEnabled = true;
		soulObj.gameObject.SetActive (false);

		tavernObj.gameObject.SetActive (false);
		listObj.gameObject.SetActive(false);

		if(posObj!=null)
		{
			if(posObj.gameObject.activeSelf)
			{
				return;
			}
		//	EventMgr.Instance.PushEvent ( EEventType.EEventType_UpdateEmployessShangZhenUI , posObj );
			posObj.gameObject.SetActive(true);
			posObj.GetComponent<PartnerPostion>().UpdataPanel();
		}
		else
		{
			LoadUI (UIASSETS_ID.UIASSETS_EmployeePosPanel);
		}
	}

	private void OnClickTujian(ButtonScript obj, object args, int param1, int param2)
	{
		tavernBtn.isEnabled = true;
		posBtn.isEnabled = true;
		listBtn.isEnabled = true;
		soulBtn.isEnabled = true;
		tujianBtn.isEnabled = false;
		tavernObj.gameObject.SetActive (false);
		if(posObj!= null)
		{
			posObj.gameObject.SetActive(false);
		}
		listObj.SetActive ( false );
		soulObj.SetActive ( false );
		tujianObj.gameObject.SetActive (true);
		tujianObj.GetComponent<EmployeeTujianUI> ().UpdataEmployees ();
	}

	private void OnClickSoul(ButtonScript obj, object args, int param1, int param2)
	{
		tavernBtn.isEnabled = true;
		posBtn.isEnabled = true;
		listBtn.isEnabled = true;
		soulBtn.isEnabled = false;
		tujianBtn.isEnabled = true;
		tujianObj.gameObject.SetActive (false);
		tavernObj.gameObject.SetActive (false);
		if(posObj!= null)
		{
			posObj.gameObject.SetActive(false);
		}
		
		listObj.SetActive ( false );
		soulObj.gameObject.SetActive (true);
		soulObj.GetComponent<EmployeeSoulUI> ().AddScrollViewItem (GamePlayer.Instance.EmployeeList);
	}

	private void OnClickBuy(ButtonScript obj, object args, int param1, int param2)
	{
		posBtn.isEnabled = true;
		tavernBtn.isEnabled = true ;
		listBtn.isEnabled = true;
		tavernObj.gameObject.SetActive (false);
		if(posObj!= null)
		{
			posObj.gameObject.SetActive(false);
		}
		listObj.gameObject.SetActive(false);

	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{

		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{

			Hide ();
		});
	}

	protected override void DoHide ()
	{
		tavernObj.GetComponent<TavernUI>().hide();
		soulObj.GetComponent<EmployeeSoulUI> ().Hide ();
		GamePlayer.Instance.DelEmployeeEnvent -= OnDelEmployeeEvolveOk;
		EmployessSystem.instance.employeeRedEnvent -= OnEmployeeRedEvent;
		if(posObj != null)
		{
			posObj.GetComponent<PartnerPostion>().Hide();
		}
		if(listObj != null)
		{
			listObj.GetComponent<EmployeeControlListUI>(). Hide();
		}

		base.DoHide ();
	}

	private void LoadUI(UIASSETS_ID id)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );
		
		AssetLoader.LoadAssetBundle (uiResPath, AssetLoader.EAssetType.ASSET_UI,(Assets,paramData)=> {
			if(tavernObj.gameObject.activeSelf || listObj.gameObject.activeSelf)
				return;
			if (hasDestroy )
            {
                AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeePosPanel, AssetLoader.EAssetType.ASSET_UI), true);
                AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeeEquipPanel, AssetLoader.EAssetType.ASSET_UI), true);
                return;
            }
            if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}
			if(listBtn.isEnabled == false)
				return;
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf)
			{
				Destroy(go);
			}
			go.transform.parent = this.panel.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			
			posObj = go;

            AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeePosPanel, AssetLoader.EAssetType.ASSET_UI), false);
            AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeeEquipPanel, AssetLoader.EAssetType.ASSET_UI), false);
		}
		, null);
	}
	void OnDelEmployeeEvolveOk(uint id)
	{
		if(posObj != null)
		{
			posObj.GetComponent<PartnerPostion>().delUpdateEmployee();
		}
	}


	void  OnEmployeeRedEvent(int inst)
	{
		if(inst == -1)
		{
			listBtnImg.GetComponent<UISprite>().MarkOff();
			return;
		}
		if(listBtnImg.gameObject.activeSelf)
		{
			listBtnImg.GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-25,-20);
		}
	}

    bool hasDestroy = false;
	bool isLoadPos = false;
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeePosPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeeEquipPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
		//
		//EventMgr.Instance.UnRegisterEvent ( EEventType.EEventType_UpdateEmployessJiuGuanUI , UpdateJIuGuanTabUI );
		//EventMgr.Instance.UnRegisterEvent ( EEventType.EEventType_UpdateEmployessLiebiaoUI , UpdateLieBiaoTabUI );
		//EventMgr.Instance.UnRegisterEvent ( EEventType.EEventType_UpdateEmployessShangZhenUI , UpdateShangZhenTabUI );
		//
        hasDestroy = true;
    }

	void UpdateJIuGuanTabUI( object obj )
	{
		if (null == obj)
			return;
		GameObject tabObj = obj as GameObject;
		if (null == tabObj)
			return;
		tabObj.SetActive ( true );
	}

	void UpdateShangZhenTabUI( object obj )
	{
		if (null == obj)
			return;
		GameObject tabObj = obj as GameObject;
		if (null == tabObj)
			return;
		tabObj.SetActive ( true );
		tabObj.GetComponent<PartnerPostion>().UpdataPanel();
	}

	void UpdateLieBiaoTabUI( object obj )
	{
		if (null == obj)
			return;
		GameObject tabObj = obj as GameObject;
		if (null == tabObj)
			return;
	
		tabObj.SetActive ( true );
	}

}

