using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GuaGuaKaUI : UIBase {

	public UILabel BonusContent;
	public UILabel Explanation;
	public UILabel tishiLabel;
	public GameObject RewardInfoObj;
	//public UIButton RewardExplainBtn;
	public UIButton ScratchRewardBtn;
	public UIButton ZGBtn;
	public UIButton CloseBtn;
	public UISprite Masksp;
	private List<ItemData>itemdata = new List<ItemData>();
	Dictionary<int, LotteryData> metdata = new Dictionary<int, LotteryData>();
	private static int ranking;
	void Start () {
		//UIManager.SetButtonEventHandler (RewardExplainBtn.gameObject, EnumButtonEvent.OnClick, OnClickRewardExplain, 0, 0);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (ScratchRewardBtn.gameObject, EnumButtonEvent.OnClick, OnClickScratchReward, 0, 0);
		UIManager.SetButtonEventHandler (ZGBtn.gameObject, EnumButtonEvent.OnClick, OnClickZGBtn, 0, 0);
		ScratchRewardBtn.gameObject.SetActive (true);
		ZGBtn.gameObject.SetActive (false);
		CloseBtn.gameObject.SetActive (false);

	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
		BagUI.ShowMe ();
	}
//	void OnClickRewardExplain(ButtonScript obj, object args, int param1, int param2)
//	{
//		RewardInfoObj.SetActive (true);
//	}
	void OnClickScratchReward(ButtonScript obj, object args, int param1, int param2)
	{

		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Guaguaka,gameObject.transform);


		ScratchRewardBtn.gameObject.SetActive (false);
		ZGBtn.gameObject.SetActive (true);
		CloseBtn.gameObject.SetActive (true);
		Masksp.gameObject.SetActive (false);
		LotteryData ldata = LotteryData.GetData (ranking);
		BonusContent.text = ldata.Win_symbol;
		isZGBtn = false;
		//tishiLabel.text = LanguageManager.instance.GetValue ("guagua").Replace("{n}",ldata.RewardName_);
		string str = LanguageManager.instance.GetValue ("guagua") + itemnamestr + LanguageManager.instance.GetValue ("guaguaend");
		PopText.Instance.Show (str);
		itemnamestr = "";
	}
	int index;
	public List<int>keys = new List<int>();
	public List<LotteryData>datass = new List<LotteryData>();
	static bool isZGBtn;
	void OnClickZGBtn(ButtonScript obj, object args, int param1, int param2)
	{

		if(!IsBagSystemContainsLottery())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyouguaguaka"));
			return;
		}
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Guaguaka,gameObject.transform);
		for (int k = 0; k<BagSystem.instance.BagItems.Length; k++) 
		{
			if(BagSystem.instance.BagItems[k]==null)continue;
			ItemData idata = ItemData.GetData((int)BagSystem.instance.BagItems[k].itemId_);

			if(idata.subType_ == ItemSubType.IST_Lottery)
			{
			   COM_Item citem = BagSystem.instance.GetItemByItemId((uint)idata.id_);
			   NetConnection.Instance.useItem((uint)citem.slot_,0,1);
				isZGBtn = true;
			}
		}
		LotteryData ldata = LotteryData.GetData (ranking);
		BonusContent.text = ldata.Win_symbol;
		//tishiLabel.text = LanguageManager.instance.GetValue ("guagua").Replace("{n}",ldata.RewardName_);

	}
	bool IsBagSystemContainsLottery()
	{
		for (int k = 0; k<BagSystem.instance.BagItems.Length; k++) 
		{
			if(BagSystem.instance.BagItems[k]==null)continue;
			ItemData idata = ItemData.GetData((int)BagSystem.instance.BagItems[k].itemId_);
			
			if(idata.subType_ == ItemSubType.IST_Lottery)
			{
				return true;
			}
		}
		return false;
	}

	static string itemnamestr;
	public static void ShowGuaGuaKa(int itemid,COM_DropItem[] Items)
	{
		ranking = itemid;
		for(int i =0;i<Items.Length;i++)
		{
			if(Items[i].itemId_ != 0)
			{
				ItemData ids = ItemData.GetData((int)Items[i].itemId_);
				itemnamestr +=ids.name_+Items[i].itemNum_+LanguageManager.instance.GetValue ("guanguaNum") ;
			}
		}
		if(isZGBtn)
		{
			string str = LanguageManager.instance.GetValue ("guagua") + itemnamestr + LanguageManager.instance.GetValue ("guaguaend");
			PopText.Instance.Show (str);
			itemnamestr = "";
			isZGBtn = false;
		}

		ShowMe ();
		BagUI.HideMe ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GuaGuaKaPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GuaGuaKaPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_GuaGuaKaPanel);
	}

	public override void Destroyobj ()
	{

	}
}
