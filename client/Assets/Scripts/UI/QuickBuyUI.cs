using UnityEngine;
using System.Collections;

public class QuickBuyUI : UIBase
{

	public UIInput input;
	public UILabel _TitleLable;
	public UILabel _ConsumptionLable;
	public UILabel _DesLable;
	public UILabel _BuyLable;
    public delegate void CancelHandler();
    static CancelHandler cancelCallback_;

    public delegate void BuyHandler();
    static BuyHandler buyCallback_;
	public UISprite iconBk;
	public UITexture iconSp;
	public UILabel pnum;
	public UILabel nameLabel;
	public UILabel DesLabel;
	//public UILabel numLabel;
	public UIButton PlusBtn;
	public UIButton MinusBtn;
	public UIButton DetermineBtn;
	public UIButton CancelBtn;
	public UISprite moneyIcon;
	public UILabel needMoneyLab;
	private int count = 0;
	private int maxCount = 0;

	private ShopData spData_;
	private static int itemId_;


	void Start () 
	{
		InitUIText ();
		UIManager.SetButtonEventHandler (PlusBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlus, 0, 0);
		UIManager.SetButtonEventHandler (MinusBtn.gameObject, EnumButtonEvent.OnClick, OnClickMinus, 0, 0);
		UIManager.SetButtonEventHandler (DetermineBtn.gameObject, EnumButtonEvent.OnClick, OnClicketermine, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);

		UIManager.Instance.LoadMoneyUI (this.gameObject);

		SpData = ShopData.GetData (itemId_);
		gameObject.transform.localPosition = new Vector3 (0,0,-2000);
	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Buy_Title");
		_ConsumptionLable.text = LanguageManager.instance.GetValue("Buy_Consumption");
		_DesLable.text = LanguageManager.instance.GetValue("Buy_Des");
		_BuyLable.text = LanguageManager.instance.GetValue("Buy_Buy");
	}


	#region Fixed methods for UIBase derived cass

	public void InputNum()
	{
		if(input.value == "0")
		{
			input.value ="1";
		}
		if(input.value != "")
		{
			count = int.Parse(input.value);
			needMoneyLab.text = (spData_._Price * count).ToString ();
		}

	}

    public static void SwithShowMe(int id, BuyHandler bCallback = null, CancelHandler cCallback = null)
	{
		itemId_ = id;
        buyCallback_ = bCallback;
        cancelCallback_ = cCallback;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_QuickBuyPanel);
	}

    public static void ShowMe(int id, BuyHandler bCallback = null, CancelHandler cCallback = null)
	{
		itemId_ = id;
        buyCallback_ = bCallback;
        cancelCallback_ = cCallback;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_QuickBuyPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_QuickBuyPanel);
	}
	public override void Destroyobj ()
	{
        ItemData item = ItemData.GetData(spData_._Itemid);
        string name = "";
        if (item != null)
            name = item.icon_;
        HeadIconLoader.Instance.Delete(name);

        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_QuickBuyPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	
	#endregion


	
	public ShopData SpData
	{
		set
		{
			if(value != null)
			{
				spData_ = value;
				count =1;
				iconBk.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData(spData_._Itemid).quality_);
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(spData_._Itemid).icon_, iconSp);
				nameLabel.text = spData_._Name;
				//numLabel.text = count.ToString();
				input.value =  count.ToString();
				needMoneyLab.text = spData_._Price.ToString();
				if(spData_._ShopPayType == ShopPayType.SPT_Gold)
				{
					moneyIcon.spriteName =	"jinbi";
				}
				else if(spData_._ShopPayType == ShopPayType.SPT_Diamond)
				{
					moneyIcon.spriteName =	"zuanshi";
				}
				else if(spData_._ShopPayType == ShopPayType.SPT_MagicCurrency)
				{
					moneyIcon.spriteName =	"molibi";
				}
 
				if(spData_._Purchase==0)
				{
					maxCount = 99;
				}else
				{
					maxCount = spData_._Purchase;
				}
				
				pnum.text = spData_._Num.ToString();
				DesLabel.text =ItemData.GetData(spData_._Itemid).desc_;
			}
		}
		get
		{
			return spData_;
		}
	}

	void OnClickPlus(ButtonScript obj, object args, int param1, int param2)
	{
		count++;
		if(count>maxCount)count = maxCount;
		input.value = count.ToString();
		//numLabel.text = count.ToString();
		needMoneyLab.text = (spData_._Price * count).ToString ();

	}
	void OnClickMinus(ButtonScript obj, object args, int param1, int param2)
	{
		count--;
		if(count<1)count = 1;
		input.value = count.ToString();
		//numLabel.text = count.ToString();
		needMoneyLab.text = (spData_._Price * count).ToString ();

	}
	void OnClicketermine(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum) <= BagSystem.instance.GetBagSize())
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("bagfull"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}
		string type = "";
		if(spData_._ShopPayType == ShopPayType.SPT_Diamond)
		{
			type = LanguageManager.instance.GetValue("zuanshi");
		}else if(spData_._ShopPayType == ShopPayType.SPT_Gold)
		{
			type = LanguageManager.instance.GetValue("jinbi");
		}
		else if(spData_._ShopPayType == ShopPayType.SPT_MagicCurrency)
		{
			type = LanguageManager.instance.GetValue("shuijing");
		}
		count = int.Parse (input.value);

	
	MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shopbuyitem").Replace("{n}",(count*SpData._Num).ToString()).Replace("{n1}",SpData._Name).Replace("{n2}",(count*SpData._Price).ToString()+type), () => {
			if(IsBalanceInadequate(SpData)==1)
			{
                if (GamePlayer.Instance.Properties[(int)PropertyType.PT_MagicCurrency] < SpData._Price)
                    PopText.Instance.Show(LanguageManager.instance.GetValue("zuanshibuzu"));
                else
                {
                    MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}",SpData._Price.ToString()), delegate
                    {
                        NetConnection.Instance.shopBuyItem(SpData._Id, count);
                        if (buyCallback_ != null)
                            buyCallback_();
                    });
                }
			}else
				if(IsBalanceInadequate(SpData)==2)
			{			
				PopText.Instance.Show(LanguageManager.instance.GetValue("jinbibuzu"));
			}
			else if(IsBalanceInadequate(SpData)==3)
			{			
				PopText.Instance.Show(LanguageManager.instance.GetValue("noshuijing"));
			}
			else if(IsReachedNumber())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("nodiamond"));
			}
			else
			{
				NetConnection.Instance.shopBuyItem(SpData._Id,count);
//				SpData.Num_--;
//				if(SpData.Num_<=0)SpData.Num_ = 0;

                if (buyCallback_ != null)
                    buyCallback_();
			}
			
			Hide ();
		});
		
		
	}
	bool IsReachedNumber()
	{
		if(SpData._Purchase==0)
		{
			return false;
		}
		return true;
	}
	int IsBalanceInadequate(ShopData sd)
	{
		if(sd._ShopPayType == ShopPayType.SPT_Diamond)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Diamond]<sd._Price)
			{
				return 1;
			}
		}else
			if(sd._ShopPayType == ShopPayType.SPT_Gold)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Money]<sd._Price)
			{
				return 2;
			}
		}
		else if(sd._ShopPayType == ShopPayType.SPT_MagicCurrency)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_MagicCurrency]<(sd._Price*count))
			{
				return 3;
			}
		}
		
		
		return 0;
	}
//	bool IsBalanceInadequate()
//	{
//        if (GamePlayer.Instance.Properties[(int)PropertyType.PT_Diamond] < SpData.Price_ * count)
//		{
//			return true;
//		}
//		return false;
//	}
	
	public static void shopItemOk()
	{
        PopText.Instance.Show(LanguageManager.instance.GetValue("EN_MallBuyOk"));
		
	}
	void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
        if (cancelCallback_ != null)
            cancelCallback_();
		Hide ();
	}

}

