using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GrowthfundUI : UIBase {

	public GameObject item;
	public UIGrid grid;
	public GameObject btn;
	public UITexture back;
	private int openFund;
	private List<GameObject> items = new List<GameObject>();
	void Start () {
		item.SetActive (false);
		addItem ();
		UIManager.SetButtonEventHandler(btn.gameObject, EnumButtonEvent.OnClick, OnenterBtn, 0, 0);
		GamePlayer.Instance.OnGrowthfundUpdate += OnGrowthfundUpdate;
		OnGrowthfundUpdate (GamePlayer.Instance.fund_);
		UpdateGrowthfundState ();
		HeadIconLoader.Instance.LoadIcon("changzhangjijin3",back);
		//GlobalValue.Get(Constant.C_OpenFund, out openFund);
	}
	void addItem()
	{
		foreach(GrowthFundData td in GrowthFundData.GetData().Values)
		{
			GameObject go = GameObject.Instantiate(item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			GrowthfundCell onlinecell = go.GetComponent<GrowthfundCell>();
			onlinecell.GrowthReawData = td;
			items.Add(go);
		}
		grid.Reposition ();
	}
	void OnGrowthfundUpdate(int val)
	{
		for(int i =0;i<items.Count;i++)
		{
			GrowthfundCell onlinecell = items[i].GetComponent<GrowthfundCell>();
			for(int j =0;j<GamePlayer.Instance.fundtags_.Count;j++)
			{
				if(onlinecell.GrowthReawData._Iv == GamePlayer.Instance.fundtags_[j])
				{
					onlinecell.enterBtn.gameObject.SetActive(false);
					onlinecell.sp.gameObject.SetActive(true);
				}
//				else
//				{
//					onlinecell.enterBtn.gameObject.SetActive(true);
//					onlinecell.sp.gameObject.SetActive(false);
//				}
			}
		}
		GrowthfundCell onlinecellone = items[0].GetComponent<GrowthfundCell>();
		if(val < onlinecellone.GrowthReawData._Iv)
		{
			UpdateGrowthfundState();
		}
	}
	void UpdateGrowthfundState()
	{
		for(int i =0;i<items.Count;i++)
		{
			GrowthfundCell onlinecell = items[i].GetComponent<GrowthfundCell>();
			if(onlinecell.GrowthReawData._Iv <= GamePlayer.Instance.GetIprop(PropertyType.PT_Level)&&GamePlayer.Instance.isFund_ )
			{
			    onlinecell.enterBtn.isEnabled = true;
			}else
			{
				onlinecell.enterBtn.isEnabled = false;
			}

		}
	}
	private void OnenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			PopText.Instance.Show(LanguageManager.instance.GetValue("zhanoougoumai"),PopText.WarningType.WT_Warning,true);

		}else
		{
			if (GamePlayer.Instance.isFund_) {
				PopText.Instance.Show (LanguageManager.instance.GetValue ("yijinggoumaiguo"));
				return;
			}
            int growFund = 0;
            GlobalValue.Get(Constant.C_GrowFundShopID, out growFund);
            //gameHandler.PayProduct(growFund);
			//StoreUI.SwithShowMe(2);
		}
       

//		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("querengoumaishuijing"), () => {
//			if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency)<openFund)
//			{
//                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("notEnoughMagicCurrency"), delegate
//                {
//                    StoreUI.SwithShowMe(1);
//                });
//				return ;
//			}
//			NetConnection.Instance.buyFund (0);
//		});

	}
	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GrowthfundPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GrowthfundPanel);
	}
	void OnDestroy()
	{
		GamePlayer.Instance.OnGrowthfundUpdate -= OnGrowthfundUpdate;
	}
	public override void Destroyobj ()
	{

	}
}
