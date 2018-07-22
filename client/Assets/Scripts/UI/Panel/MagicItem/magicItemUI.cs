using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class magicItemUI : UIBase
{
	public UIButton closeBtn;
	public UIButton levelUpBtn;
	public UIButton tuPoBtn;
	public UIButton huanBtn;

	public GameObject levelUpObj;
	public GameObject tupoObj;

	public UILabel MagicTitleLab;
	public UILabel MagicSelectItemLab;
	public UILabel MagicUIInfoLab;
	public UILabel MagicSelectItem1Lab;
	public UILabel MagiclevelBtnLab;
	public UILabel MagicTupoBtnLab;

	private List<GameObject> effectObjList = new List<GameObject>(); 

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0, 0);
		UIManager.SetButtonEventHandler (tuPoBtn.gameObject, EnumButtonEvent.OnClick, OnClickTuPo, 0, 0);
		UIManager.SetButtonEventHandler (huanBtn.gameObject, EnumButtonEvent.OnClick, OnClickHuan, 0, 0);
		GamePlayer.Instance.MagicItmeEnvet += new RequestEventHandler<int> (OnMagicLevelEvent);
		GamePlayer.Instance.MagicItmeJobEnvet += new RequestEventHandler<int> (OnMagicJobEvent);
		GamePlayer.Instance.MagicItmeTupoEnvet += new RequestEventHandler<int> (OnMagicTupoEnvet);
		UIManager.Instance.LoadMoneyUI (this.gameObject);


		MagicTitleLab.text = LanguageManager.instance.GetValue("MagicTitleLab");
		MagicSelectItemLab.text = LanguageManager.instance.GetValue("MagicSelectItemLab");
		MagicUIInfoLab.text = LanguageManager.instance.GetValue("MagicUIInfoLab");
		MagicSelectItem1Lab.text = LanguageManager.instance.GetValue("MagicSelectItem1Lab");
		MagiclevelBtnLab.text = LanguageManager.instance.GetValue("MagiclevelBtnLab");
		MagicTupoBtnLab.text = LanguageManager.instance.GetValue("MagicTupoBtnLab");

		OpenPanelAnimator.PlayOpenAnimation (this.panel, () => {
			levelUpObj.GetComponent<magicItemLevelUpUI> ().UpdataItems ();
			GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMagicUIOpen);
		});
		levelUpBtn.isEnabled = false;
	}


	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_MagicItemPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MagicItemPanel);
	}


	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_MagicItemPanel);
	}
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_MagicItemPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	
	#endregion


	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();
		});
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		levelUpBtn.isEnabled = false;
		tuPoBtn.isEnabled = true;
		huanBtn.isEnabled = true;

		levelUpObj.gameObject.SetActive (true);
		levelUpObj.GetComponent<magicItemLevelUpUI> ().UpdataItems ();
		tupoObj.gameObject.SetActive (false);
	}

	private void OnClickTuPo(ButtonScript obj, object args, int param1, int param2)
	{
		levelUpBtn.isEnabled = true;
		tuPoBtn.isEnabled = false;
		huanBtn.isEnabled = true;

		levelUpObj.gameObject.SetActive (false);
		tupoObj.gameObject.SetActive (true);
		tupoObj.GetComponent<magicItemTupoUI>().UpdateMagicItem ();
	}

	private void OnClickHuan(ButtonScript obj, object args, int param1, int param2)
	{
		levelUpBtn.isEnabled = true;
		tuPoBtn.isEnabled = true;
		huanBtn.isEnabled = false;


		levelUpObj.gameObject.SetActive (true);
		levelUpObj.GetComponent<magicItemLevelUpUI> ().updateZhaunhuan ();
		tupoObj.gameObject.SetActive (false);

	}

	void OnMagicLevelEvent(int level)
	{
		if(level%10 ==0)
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("tupochenggong"));
			tupoObj.GetComponent<magicItemTupoUI> ().UpdateMagicItem();
		}
		else
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("sqsjchenggong"));
			levelUpObj.GetComponent<magicItemLevelUpUI> ().UpdataItems();

            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMagicLevelUp);
		}
	}

	void OnMagicJobEvent(int job)
	{
		PopText.Instance.Show ( LanguageManager.instance.GetValue("zhuanhuanchenggong"));
		levelUpObj.GetComponent<magicItemLevelUpUI> ().updateZhaunhuan ();
	}
	void OnMagicTupoEnvet(int level)
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue("tupochenggong"));
		tupoObj.GetComponent<magicItemTupoUI>().UpdateMagicItem ();
		tupoObj.GetComponent<magicItemTupoUI> ().suolianImg.gameObject.SetActive (false);
		EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_UI_magicTupo, transform);
		//EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_magicTuposuolian,transform);

	}

	protected override void DoHide ()
	{
		GamePlayer.Instance.MagicItmeEnvet -= OnMagicLevelEvent;
		GamePlayer.Instance.MagicItmeJobEnvet -= OnMagicJobEvent;
		GamePlayer.Instance.MagicItmeTupoEnvet -= OnMagicTupoEnvet;
		base.DoHide ();
	}

}

