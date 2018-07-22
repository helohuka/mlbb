using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpUI : UIBase
{
	public UIButton closeBtn;
	public UIButton guideBtn;   //指引按钮.
	public UIButton wayBtn;     //历程按钮.
	public UIButton bookBtn;     //历程按钮.
	public UIButton strBtn;     //
    public UIButton qaBtn;     //

	public GameObject guideObj;
	public GameObject wayObj;
	public GameObject bookObj;
    public GameObject qaObj;

	public static int _openfun;
	public static int _subFun;
	void Start ()
	{
        hasDestroy = false;
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (guideBtn.gameObject, EnumButtonEvent.OnClick, OnClickGuide, 0, 0);
		UIManager.SetButtonEventHandler (wayBtn.gameObject, EnumButtonEvent.OnClick, OnClickWay, 0, 0);
		UIManager.SetButtonEventHandler (bookBtn.gameObject, EnumButtonEvent.OnClick, OnClickBook, 0, 0);
		UIManager.SetButtonEventHandler (strBtn.gameObject, EnumButtonEvent.OnClick, OnClickStr, 0, 0);
        UIManager.SetButtonEventHandler(qaBtn.gameObject, EnumButtonEvent.OnClick, OnClickQA, 0, 0);
		strBtn.isEnabled = false;
		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{

			bookObj.GetComponent<HelpBookUI>().UpdateStrBtns();
			if(_openfun == 1)
			{
				guideBtn.isEnabled = false;
				wayBtn.isEnabled = true;
				bookBtn.isEnabled = true;
				strBtn.isEnabled = true;
				
				if (bookObj != null)
					bookObj.SetActive (false);
				if (wayObj != null)
					wayObj.SetActive (false);
                if (qaObj != null)
                    qaObj.SetActive(false);
				guideObj.SetActive (true);

				if(_subFun == 4)
				{
					guideObj.GetComponent<HelpGuideUI>().ShowProfession ();
				}
				else if(_subFun == 6)
				{
					guideObj.GetComponent<HelpGuideUI>().ShowOpenLevel();
				}

			}

		});
	}
	
	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe(int openFun = 0, int  subFun = 0)
	{
		_openfun = openFun;
		_subFun = subFun;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_HelpPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_HelpPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_HelpPanel);
	}
	

	#endregion


	private void OnClickGuide(ButtonScript obj, object args, int param1, int param2)
	{
		guideBtn.isEnabled = false;
		wayBtn.isEnabled = true;
		bookBtn.isEnabled = true;
		strBtn.isEnabled = true;
        qaBtn.isEnabled = true;

		if (bookObj != null)
			bookObj.gameObject.SetActive (false);
		if (wayObj != null)
			wayObj.gameObject.SetActive (false);
        qaObj.SetActive(false);
		guideObj.gameObject.SetActive (true);
		if(guideObj.transform.FindChild("equipAndSkill").GetComponent<HelpEquipAndSkillUI>().SelectType == 0)
			guideObj.transform.FindChild("equipAndSkill").GetComponent<HelpEquipAndSkillUI>().SelectType = 1;
	}

	private void OnClickWay(ButtonScript obj, object args, int param1, int param2)
	{
		guideBtn.isEnabled = true;
		wayBtn.isEnabled = false;
		bookBtn.isEnabled = true;
		strBtn.isEnabled = true;
        qaBtn.isEnabled = true;

		guideObj.gameObject.SetActive (false);
        qaObj.SetActive(false);
		if (bookObj != null)
			bookObj.gameObject.SetActive (false);
		if (wayObj == null)
		{
			LoadUI (UIASSETS_ID.UIASSETS_HelpWayPanel, 1);
		}
		else
		{
			wayObj.SetActive (true);
		}
	}

	private void OnClickBook(ButtonScript obj, object args, int param1, int param2)  
	{
		guideBtn.isEnabled = true;
		wayBtn.isEnabled = true;
		bookBtn.isEnabled = false;
		strBtn.isEnabled = true;
        qaBtn.isEnabled = true;

        qaObj.SetActive(false);
		guideObj.gameObject.SetActive (false);
		if (wayObj != null)
			wayObj.gameObject.SetActive (false);
		//if(bookObj == null)
		//{
			//LoadUI (UIASSETS_ID.UIASSETS_HelpBookPanel, 2);
		//}
		//else
		//{
			bookObj.SetActive(true);
			bookObj.GetComponent<HelpBookUI>().UpdateBookBtns();
		//}
	}

	private void OnClickStr(ButtonScript obj, object args, int param1, int param2)
	{
		guideBtn.isEnabled = true;
		wayBtn.isEnabled = true;
		bookBtn.isEnabled = true;
		strBtn.isEnabled = false;
        qaBtn.isEnabled = true;

        qaObj.SetActive(false);
		guideObj.gameObject.SetActive (false);
		if (wayObj != null)
			wayObj.gameObject.SetActive (false);
		//if(bookObj == null)
		//{
		//	LoadUI (UIASSETS_ID.UIASSETS_HelpBookPanel, 2);
		//}
		//else
		//{
			bookObj.SetActive(true);
			bookObj.GetComponent<HelpBookUI>().UpdateStrBtns();
		//}

	}

    private void OnClickQA(ButtonScript obj, object args, int param1, int param2)
	{
		guideBtn.isEnabled = true;
		wayBtn.isEnabled = true;
		bookBtn.isEnabled = true;
        strBtn.isEnabled = true;
        qaBtn.isEnabled = false;

        qaObj.SetActive(true);
        guideObj.gameObject.SetActive(false);
        bookObj.SetActive(false);
        if (wayObj != null)
            wayObj.gameObject.SetActive(false);
	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();
		});
	}

	private void LoadUI(UIASSETS_ID id,int num)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );
		
		AssetLoader.LoadAssetBundle (uiResPath, AssetLoader.EAssetType.ASSET_UI,(Assets,paramData)=> {
            if (hasDestroy)
            {
                AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpBookPanel, AssetLoader.EAssetType.ASSET_UI), true);
                AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpWayPanel, AssetLoader.EAssetType.ASSET_UI), true);
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

			if(num ==1)
			{
				wayObj = go;
				if(wayBtn.isEnabled)
				{
					wayObj.gameObject.SetActive(false);
				}
			}
			else if(num ==2)
			{
				bookObj = go;
				if(bookBtn.isEnabled)
				{
					bookObj.gameObject.SetActive(false);
				}
			}
            AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpBookPanel, AssetLoader.EAssetType.ASSET_UI), false);
            AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpWayPanel, AssetLoader.EAssetType.ASSET_UI), false);
		}
		, null);
	}

    bool hasDestroy = false;
	public override void Destroyobj ()
	{
		 //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpBookPanel, AssetLoader.EAssetType.ASSET_UI), true);
		 //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HelpWayPanel, AssetLoader.EAssetType.ASSET_UI), true);
         hasDestroy = true;
    }
		
}

