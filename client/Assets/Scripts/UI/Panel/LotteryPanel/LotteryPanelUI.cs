using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LotteryPanelUI : UIBase {

	public static bool isOpen;
	public GameObject []itemsObj;
	public UIButton oneBtn;
	public UIButton tenBtn;
	public UIButton closeBtn;
	public UILabel item;
	public UIGrid grid;
	private bool isStart;
	int slowDownStep = 8;
	public GameObject tenItemObj;
	public UIButton CloseBtn;
	public UIButton enterBtn;
	public UIButton RTenBtn;
	public GameObject item_ten;
	public UIGrid grid_ten;

	public UIGrid conGrid;
	public UIScrollBar sbar;
	public UILabel timeLab;
    List<string> iconNames_;
	public UILabel oneLable;
	public UILabel tenLable;
	public GameObject oneObj;
	public UIButton oneEnterB;
	public UIButton rtenB;
	public UISprite oneiconback;
	public UILabel onenameL;
	uint [] itemsIds;
//	List<uint>itemids = new List<uint> ();
	private List<string> names = new List<string>(); 
	private bool  isTen;
	private bool  isOne;
	void Awake()
	{
        iconNames_ = new List<string>();
		//NetConnection.Instance.openZhuanpan ();
	}
	int one =0;
	int ten = 0;
	void Start () {

		isOpen = true;
		item_ten.SetActive (false);
		item.gameObject.SetActive (false);
		//UIManager.Instance.LoadMoneyUI(this.gameObject);
		UIManager.SetButtonEventHandler (oneBtn.gameObject, EnumButtonEvent.OnClick, OnClickOne, 0, 0);
		UIManager.SetButtonEventHandler (tenBtn.gameObject, EnumButtonEvent.OnClick, OnClickTen, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);

		UIManager.SetButtonEventHandler (RTenBtn.gameObject, EnumButtonEvent.OnClick, OnClickRTenBtn, 0, 0);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseBtn, 0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenterBtn, 0, 0);

		UIManager.SetButtonEventHandler (oneEnterB.gameObject, EnumButtonEvent.OnClick, OnClickoneEnter, 0, 0);
		UIManager.SetButtonEventHandler (rtenB.gameObject, EnumButtonEvent.OnClick, OnClickrten, 0, 0);

		for(int i =0;i<itemsObj.Length;i++)
		{
			UISprite sp = itemsObj[i].GetComponent<UISprite>();
			names.Add(sp.spriteName);
		}
		item.text = "";
		InitLotteryData ();
		ZhuanPanSystem.Gozhuanpan += ShowLotteryTeams;
		ZhuanPanSystem.UpdateZhuanpanNoticeOk += UpdateZhuanpanNoticeOk;
		ZhuanPanSystem.ZhuanpanNotOk += SycnZhuanpanNoticeOk;
		SycnZhuanpanNoticeOk (ZhuanPanSystem.ZhuanpanList);
		GlobalValue.Get(Constant.C_ZhuanPanOneGo, out one);
		GlobalValue.Get(Constant.C_ZhuanPanTenGo, out ten);
		oneLable.text = LanguageManager.instance.GetValue ("Guild_Xiaohao") + one.ToString ();
		tenLable.text = LanguageManager.instance.GetValue ("Guild_Xiaohao") + ten.ToString ();

		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)ZhuanPanSystem.zhuanData.sinceStamp_);
		Define.FormatUnixTimestamp(ref efmt, (int)ZhuanPanSystem.zhuanData.endStamp_);
		
		timeLab.text = LanguageManager.instance.GetValue("zhuanpantime").Replace("{n}", sfmt +" - "+  efmt);


	}
	void UpdateZhuanpanNoticeOk(COM_Zhuanpan Zhuanpan)
	{
		ResItem ();

		ShowItems (ZhuanPanSystem.ZhuanpanList);
	}
	void SycnZhuanpanNoticeOk(List<COM_Zhuanpan> zhans)
	{
		ResItem ();
		ShowItems (zhans);
	}

	void ShowLotteryTeams(uint[] items)
	{
		itemsIds = items;
		returnslot (items);
	}
	void ResItem()
	{
		if(conGrid == null)return;
		foreach(Transform tr in conGrid.transform)
		{
			Destroy(tr.gameObject);
		}
	}
	void ShowItems(List<COM_Zhuanpan> zhans)
	{
	
		for(int i =zhans.Count-1;i>=0;i--)
		{
			COM_ZhuanpanContent adata = ZhuanPanSystem.GetData((int)zhans[i].zhuanpanId_);
			//ZhuanpanConfigData adata = ZhuanpanConfigData.GetData((int)zhans[i].zhuanpanId_);
			GameObject go = GameObject.Instantiate(item.gameObject)as GameObject;
			go.SetActive(true);
			go.transform.parent = conGrid.transform;
			go.transform.localScale = Vector3.one;
			UILabel la = go.GetComponent<UILabel>();
			la.text = zhans[i].playerName_+":"+"[-][f6f204]"+ " "+ItemData.GetData((int)adata.item_).name_;
			conGrid.repositionNow = true;
		}
		GlobalInstanceFunction.Instance.Invoke (() => {sbar.value = 0;}, 2);

	}

	void returnslot(uint[] items)
	{
		List<int> slots = new List<int>();
		for(int i =0;i<itemsObj.Length;i++)
		{
			LitteryPanleCell lcell = itemsObj[i].GetComponent<LitteryPanleCell>();
			if(lcell.Data.id_ == (int)items[items.Length-1])
			{
				slot = i;
				return;
			}
		}
	}
	void OnClickoneEnter(ButtonScript obj, object args, int param1, int param2)
	{
		oneObj.SetActive (false);
	}
	void OnClickrten(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			return;
		}
		isOne = true;
		isTen = false;
		int cost = 0;

		GlobalValue.Get (Constant.C_ZhuanPanOneGo, out cost);
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) < cost)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shuijingbuzu"));
			return;
		}
		Reset ();
		isStart = true;
		oneBtn.isEnabled = false;
		tenBtn.isEnabled = false;
		oneObj.SetActive (false);
		NetConnection.Instance.zhuanpanGo (1);
		StartCoroutine (RunLottery());
	}
	void OnClickRTenBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{

			return;
		}
		int itemCount = BagSystem.instance.GetBagSize ();
		int bagSize  = GamePlayer.Instance.GetIprop (PropertyType.PT_BagNum);
		int num = bagSize - itemCount;
		if(num<10)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("beibaokongjianbuzu"));
			return;
		}
		foreach(Transform tr in grid_ten.transform)
		{
			Destroy(tr.gameObject);
		}
		tenItemObj.SetActive (false);
		isTen = true;
		isOne = false;
		int cost = 0;
		GlobalValue.Get (Constant.C_ZhuanPanTenGo, out cost);
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) < cost)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shuijingbuzu"));
			return;
		}
		Reset ();
		isStart = true;
		tenBtn.isEnabled = false;
		oneBtn.isEnabled = false;
		NetConnection.Instance.zhuanpanGo (10);
		StartCoroutine (RunLottery());
	}
	void OnClickCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		foreach(Transform tr in grid_ten.transform)
		{
			Destroy(tr.gameObject);
		}
		tenItemObj.SetActive (false);
	}
	void OnClickenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		foreach(Transform tr in grid_ten.transform)
		{
			Destroy(tr.gameObject);
		}
		tenItemObj.SetActive (false);
	}
	void OnClickOne(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			return;
		}
		isOne = true;
		isTen = false;
		int cost = 0;
		GlobalValue.Get (Constant.C_ZhuanPanOneGo, out cost);
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) < cost)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shuijingbuzu"));
			return;
		}
		Reset ();
		isStart = true;
		oneBtn.isEnabled = false;
		tenBtn.isEnabled = false;
		NetConnection.Instance.zhuanpanGo (1);
		StartCoroutine (RunLottery());
	}
	void OnClickTen(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			return;
		}
		int itemCount = BagSystem.instance.GetBagSize ();
		int bagSize  = GamePlayer.Instance.GetIprop (PropertyType.PT_BagNum);
		int num = bagSize - itemCount;
		if(num<10)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("beibaokongjianbuzu"));
			return;
		}
		isTen = true;
		isOne = false;
		int cost = 0;
		GlobalValue.Get (Constant.C_ZhuanPanTenGo, out cost);
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) < cost)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shuijingbuzu"));
			return;
		}
		Reset ();
		isStart = true;
		tenBtn.isEnabled = false;
		oneBtn.isEnabled = false;
		NetConnection.Instance.zhuanpanGo (10);
		StartCoroutine (RunLottery());
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();

	}
	int i = 0;
	float totalTime_ = 0.025f;
	int slot = -1;
	int count = 0;
	string spname;
	void Reset()
	{
		if(slot < 0 || slot > itemsObj.Length)
			return;

		UISprite sp = itemsObj [slot].GetComponent<UISprite> ();
		sp.spriteName = names[slot];
		slot = -1;
		i = 0;
		totalTime_ = 0.025f;
	}

	bool beginSlowDown = false;
	IEnumerator RunLottery()
	{
		UISprite sp = null;
		while(isStart)
		{
			//一圈了
			if(i == itemsObj.Length)
			{
				count++;
				i = 0;
			}

			sp = itemsObj[i].GetComponent<UISprite>();
		    spname = sp.spriteName;
			sp.spriteName = "choujiangkuang3";
			yield return new WaitForSeconds(totalTime_);
			sp.spriteName = spname;




			//6圈之后 获得的索引 是当前转到的索引+5 并且有索引
			if(count>=6 && i == GetIdx(slot, slowDownStep) && slot != -1)
			{
				beginSlowDown = true;
			}

			if(beginSlowDown)
			{
				//时间就加
				totalTime_ += (slowDownStep - GetCount(slot, i)) * 0.025f;
				ClientLog.Instance.Log(totalTime_);
				if(slot == i)
				{
					count = 0;
					beginSlowDown = false;
					break;
				}
			}
			i++;
		}
		if(slot> 0 && slot < itemsObj.Length)
		{

			sp = itemsObj[slot].GetComponent<UISprite>();
			sp.spriteName = "choujiangkuang3";
			isStart = false;
			StopCoroutine(RunLottery());
			oneBtn.isEnabled = true;
			tenBtn.isEnabled = true;
			//ResItem ();
			//ShowItems (ZhuanPanSystem.ZhuanpanList);
			if(isOne)
			{
				ShoeOneItems();
			}
			if(isTen)
			{
				ShoeTenItems();
			}
		}
			


	}

	int GetCount(int pos1, int pos2)
	{
		int count = pos1 - pos2;
		return count >= 0 ? count : itemsObj.Length + count;
	}

	int GetIdx(int pos, int gap)
	{
		int idx = pos - gap;
		return idx >= 0? idx: itemsObj.Length + idx;
	}

	void InitLotteryData()
	{
//		foreach(KeyValuePair<int, ZhuanpanConfigData> pair in ZhuanpanConfigData.GetData())
//		{
//			LitteryPanleCell lcell = itemsObj[pair.Key-1].GetComponent<LitteryPanleCell>();
//			lcell.Data = pair.Value;
//		}

		for(int i=0;i<ZhuanPanSystem.zdata.Count;i++)
		{
			LitteryPanleCell lcell = itemsObj[i].GetComponent<LitteryPanleCell>();
			lcell.Data = ZhuanPanSystem.zdata[i];
		}

	}
	void ShoeTenItems()
	{
		tenItemObj.SetActive (true);
		addTenItem ();
	}
	void ShoeOneItems()
	{	
		//onenameL;
		oneObj.SetActive (true);

		COM_ZhuanpanContent adata = ZhuanPanSystem.GetData((int)itemsIds[0]);
		if(adata == null)
			return ;
		ItemCellUI icell = UIManager.Instance.AddItemCellUI (oneiconback, (uint)adata.item_);
		onenameL.text = ItemData.GetData((int)adata.item_).name_;
		icell.ItemCount = (int)adata.itemNum_;
	}
	void addTenItem()
	{


        string iconName = "";
		for(int i =0;i<itemsIds.Length ;i++)
		{
			GameObject go = GameObject.Instantiate(item_ten)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid_ten.transform;
			go.transform.localScale = Vector3.one;
			COM_ZhuanpanContent adata = ZhuanPanSystem.GetData((int)itemsIds[i]);
			if(adata == null)
				continue;

			//UITexture tex = go.GetComponentInChildren<UITexture>();
            iconName = ItemData.GetData((int)adata.item_).icon_;
            iconNames_.Add(iconName);
			UISprite sp = go.GetComponent<UISprite>();
			UIManager.Instance.AddItemCellUI(sp,(uint)adata.item_).ItemCount = (int)adata.itemNum_;

            //HeadIconLoader.Instance.LoadIcon(iconName, tex);
			UILabel lab = go.GetComponentInChildren<UILabel>();
			lab.text = ItemData.GetData((int)adata.item_).name_;
			grid_ten.repositionNow = true;
		}

	}


	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_LotteryPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_LotteryPanel);
	}
	void OnDestroy()
	{
		isOpen = false;
		ZhuanPanSystem.Gozhuanpan -= ShowLotteryTeams;
		ZhuanPanSystem.UpdateZhuanpanNoticeOk -= UpdateZhuanpanNoticeOk;
		ZhuanPanSystem.ZhuanpanNotOk -= SycnZhuanpanNoticeOk;
		
		for (int i = 0; i < iconNames_.Count; i++)
		{
			HeadIconLoader.Instance.Delete(iconNames_[i]);
		}
		iconNames_.Clear();
		StopCoroutine(RunLottery());
	}
	public override void Destroyobj ()
	{
		isOpen = false;
		ZhuanPanSystem.Gozhuanpan -= ShowLotteryTeams;
		ZhuanPanSystem.UpdateZhuanpanNoticeOk -= UpdateZhuanpanNoticeOk;
		ZhuanPanSystem.ZhuanpanNotOk -= SycnZhuanpanNoticeOk;

        for (int i = 0; i < iconNames_.Count; i++)
        {
            HeadIconLoader.Instance.Delete(iconNames_[i]);
        }
        iconNames_.Clear();
        StopCoroutine(RunLottery());
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_LotteryPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}

}
