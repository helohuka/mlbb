using UnityEngine;
using System.Collections;

public class MagicOpenUI : UIBase
{
	public UITexture magicIcon;
	public GameObject txObj;
	public GameObject textLab;
	public UISprite closeImg;



	void Start ()
	{
		closeImg.gameObject.SetActive (false);
		//UIManager.SetButtonEventHandler (closeImg.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		textLab.GetComponent<TypewriterEffect> ().overBack = CallBack;
	}
	

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_MagicOPenPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MagicOPenPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_MagicOPenPanel);
	}
	
	public override void Destroyobj ()
	{
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_MagicOPenPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();	
	}

	void CallBack()
	{
		txObj.gameObject.SetActive (true);
		textLab.gameObject.SetActive (false);
		closeImg.gameObject.SetActive (true);
        //ArtifactLevelData data = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel, (int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);
        //if (data != null)
            HeadIconLoader.Instance.LoadIcon("shendun", magicIcon);
		StartCoroutine (DelayAction());
	}


	IEnumerator DelayAction()
	{
		while(true)
		{
			yield return new WaitForSeconds(2);
			StopAllCoroutines ();
			Hide();
			GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMagicTipClose);
		}
	}



}

