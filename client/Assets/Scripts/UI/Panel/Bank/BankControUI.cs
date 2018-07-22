using UnityEngine;
using System.Collections;

public class BankControUI : UIBase {

	public UILabel _BankTitle;
	public UILabel _BankBaby;
	public UILabel _BankItems;


	public UIButton closeBtn;
	public UIButton daojuBtn;
	public UIButton babyBtn;
	private GameObject PropsbackObj;
	private GameObject babyObj;
	private static int panelId;
	GameObject roleObj;
	// Use this for initialization
	void Awake()
	{
		
		hasDestroy = false;
		if(BagUI.Instance != null)
		{
			if(BagUI.Instance.GetRole()!= null)
			{
				BagUI.Instance.GetRole().SetActive(false);
			}
		}
		if(MainbabyListUI.babyObj != null)
		MainbabyListUI.babyObj.SetActive (false);
		BankSystem.instance.isopen = true;
	}
	void Start () {

		_BankTitle.text = LanguageManager.instance.GetValue("bank_Title");
		_BankBaby.text = LanguageManager.instance.GetValue("bank_Baby");
		_BankItems.text = LanguageManager.instance.GetValue("bank_Items");



		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		UIManager.SetButtonEventHandler (daojuBtn.gameObject, EnumButtonEvent.OnClick, OnClickdaoju,0, 0);
		UIManager.SetButtonEventHandler (babyBtn.gameObject, EnumButtonEvent.OnClick, OnClickbaby,0, 0);
		//roleObj = BagUI.Instance.GetRole ();
//		if (roleObj != null)
//		   roleObj.SetActive (false);
		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{
			if(panelId == 1)
			{
				daojuBtn.isEnabled = false;
				babyBtn.isEnabled = true;
				if(babyObj != null)
					babyObj.SetActive (false);
				if(PropsbackObj != null)
				{
					PropsbackObj.SetActive(true);
				}else
				{
					LoadUI (UIASSETS_ID.UIASSETS_BankPropspanel);
				}
			}else  if(panelId == 2)
			{
				daojuBtn.isEnabled = true;
				babyBtn.isEnabled = false;
				if(PropsbackObj != null)
					PropsbackObj.SetActive (false);
				if(babyObj != null)
				{
					babyObj.SetActive(true);

				}else
				{
					LoadBabyUI (UIASSETS_ID.UIASSETS_babyBankPanel);
				}
			}

			UIManager.Instance.AdjustUIDepth(gameObject.transform);
		});

	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickdaoju(ButtonScript obj, object args, int param1, int param2)
	{
		panelId = 1;
		daojuBtn.isEnabled = false;
		babyBtn.isEnabled = true;
		if(babyObj != null)
			babyObj.SetActive (false);
		if(PropsbackObj != null)
		{
			PropsbackObj.SetActive(true);
		}else
		{
			LoadUI (UIASSETS_ID.UIASSETS_BankPropspanel);
		}

	}
	void OnClickbaby(ButtonScript obj, object args, int param1, int param2)
	{
		panelId = 2;
		daojuBtn.isEnabled = true;
		babyBtn.isEnabled = false;
		if(PropsbackObj != null)
			PropsbackObj.SetActive (false);
		if(babyObj != null)
		{
			babyObj.SetActive(true);
		}else
		{
			LoadBabyUI (UIASSETS_ID.UIASSETS_babyBankPanel);
		}

	}
	string subUiResPath;
	bool hasDestroy = false;
	GameObject panObj;
	private void LoadUI(UIASSETS_ID id)
	{
		subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
		
		AssetLoader.LoadAssetBundle(subUiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
		                            {
			if (hasDestroy)
			{
				AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
				return;
			}
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf )
			{
				Destroy(go);
				return;
			}
			PropsbackObj = go;
			go.transform.parent = this.panel.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			//UIManager.Instance.AdjustUIDepth(go.transform);
			if(daojuBtn.isEnabled)
			{
				PropsbackObj.gameObject.SetActive(false);
				return;
			}
			UIManager.Instance.AdjustUIDepth (transform);
            AssetInfoMgr.Instance.DecRefCount(subUiResPath, false);
		}
		, null);
	}

    UIASSETS_ID openedBabyBank;
	private void LoadBabyUI(UIASSETS_ID id)
	{
        //subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);

        UIAssetMgr.LoadUI(id, (Assets, paramData) =>
		                            {
			if (hasDestroy)
			{
                //AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
                UIAssetMgr.DeleteAsset(id, true);
				return;
			}
			if( null == Assets || null == Assets.mainAsset )
			{
                UIAssetMgr.DeleteAsset(id, true);
				return ;
			}			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf )
			{
				Destroy(go);
				return;
			}
			babyObj = go;
			go.transform.parent = this.panel.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			//UIManager.Instance.AdjustUIDepth(go.transform);
			if(babyBtn.isEnabled)
			{
				babyObj.gameObject.SetActive(false);
				return;
			}
			UIManager.Instance.AdjustUIDepth (transform);
            //AssetInfoMgr.Instance.DecRefCount(subUiResPath, false);
            openedBabyBank = id;
		}
		, null);
	}


	public static void ShowMe(int pid = 1)
	{
        GuideManager.Instance.ClearMask();
		if(!GamePlayer.Instance.isInitStorageItem || !GamePlayer.Instance.isInitStorageBaby)
		{
			BankSystem.instance.isOpeninitBank = true;
			BankSystem.instance.isOpenBankType = pid;
			NetConnection.Instance.requestStorage(StorageType.ST_Item);
			NetConnection.Instance.requestStorage(StorageType.ST_Baby);

            NetWaitUI.ShowMe();
		}
		else
		{
			panelId = pid;
			UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BankPanel);
		}
	}
	public static void SwithShowMe(int pid = 1)
	{
		panelId = pid;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BankPanel);
	}

	public override void Destroyobj ()
	{
		hasDestroy = true;
		if(BagUI.Instance != null)
		{
			if(BagUI.Instance.GetRole()!= null)
			{
				BagUI.Instance.GetRole().SetActive(true);
			}
		}
		if(GamePlayer.Instance.babies_list_.Count!=0)
		{
			if( MainbabyUI.Instance != null)
			{
				if(MainbabyUI.Instance.isState)
				{
					if(MainbabyListUI.babyObj != null)
						MainbabyListUI.babyObj.SetActive (true);
				}
			}

		}
        UIAssetMgr.DeleteAsset(openedBabyBank, true);
		BankSystem.instance.isopen = false;
//		if (roleObj != null)
//			roleObj.SetActive (true);
	}
}
