using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Exchange : UIBase {


	public UIButton CloseBtn;
	public UIButton ExchangeBtn;
	public GameObject item;
	public UIGrid grid;
	Dictionary<int, LotteryData> metdata = new Dictionary<int, LotteryData>();
	private List<LotteryData> lotDatas = new List<LotteryData>();
	private static Dictionary<int, LotteryData> metaData;
	private List<int>keys = new List<int>();
	private List<ItemData>itemdata = new List<ItemData>();
	private List<ItemData>itdata = new List<ItemData>();
	// Use this for initialization
	void Start () {
		item.gameObject.SetActive (false);
		metaData = LotteryData.GetData ();		
		foreach(int key in metaData.Keys)
		{
			keys.Add(key);
		}				
		for(int i = 0;i<keys.Count;i++)
		{
			LotteryData ldata = LotteryData.GetData(keys[i]);
			lotDatas.Add(ldata);
		}
		for (int j = 0; j<lotDatas.Count; j++)
		{
			//ItemData ida = ItemData.GetData(lotDatas[j].Rewarditem_);
			//itemdata.Add(ida);
		}
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (ExchangeBtn.gameObject, EnumButtonEvent.OnClick, OnClickExchange, 0, 0);
		AddItems (lotDatas);

		for(int k = 0;k<BagSystem.instance.BagItems.Length;k++)
		{
			if(BagSystem.instance.BagItems[k]==null)continue;
			ItemData idata = ItemData.GetData((int)BagSystem.instance.BagItems[k].itemId_);
			itdata.Add(idata);
		}


	}
	void AddItems(List<LotteryData> loDatas)
	{
		for (int i = 0; i<loDatas.Count&&i<itemdata.Count; i++) {
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			UILabel [] las = o.GetComponentsInChildren<UILabel>(true);
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("dengjiLabel"))
				{
					la.text = loDatas[i].RewardName_;
				}
				if(la.gameObject.name.Equals("tuanLabel"))
				{
					la.text = loDatas[i].Win_symbol;
				}
				if(la.gameObject.name.Equals("wupinNameLabel"))
				{
					la.text = itemdata[i].name_;
				}
			}
			UITexture []sps = o.GetComponentsInChildren<UITexture>();
			foreach(UITexture sp in sps)
			{
				if(sp.gameObject.name.Equals("icon"))
				{
					HeadIconLoader.Instance.LoadIcon (itemdata[i].icon_, sp);
				}
			}
			grid.repositionNow = true;
			
		}
		
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickExchange(ButtonScript obj, object args, int param1, int param2)
	{
		for (int i = 0; i<itdata.Count; i++)
		{
			if(itdata[i].subType_ == ItemSubType.IST_Coupun)
			{
				//NetConnection.Instance.expiry ((uint)itdata[i].id_);
			}
		}
        PopText.Instance.Show(LanguageManager.instance.GetValue("CannotExchangeReward"));
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ExchangePanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ExchangePanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_ExchangePanel);
	}

	public override void Destroyobj ()
	{

	}
}
