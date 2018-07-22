using UnityEngine;
using System.Collections;

public class StoreTips : MonoBehaviour {

	public UILabel _LevelLable;
	public UILabel _SpeciesLable;
	public UILabel _PriceLable;
	public UILabel _DesLable;
	public UILabel _BuyLable;

	public UITexture iconSp;
	public UILabel pnum;
	public UILabel nameLabel;
	public UILabel DesLabel;
	public UILabel levelLabel;
	public UILabel zhongleiLabel;
	public UILabel jiageLabel;
	public UISprite iconKuang;
	//public UILabel numLabel;
	public UIInput numInput;
	public UIButton PlusBtn;
	public UIButton MinusBtn;
	public UIButton DetermineBtn;
	public UIButton CancelBtn;
	public int count = 1;
	public int maxCount = 0;
	public UILabel buyNumLab;
	void Start () {
		InitUIText ();
		UIManager.SetButtonEventHandler (PlusBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlus, 0, 0);
		UIManager.SetButtonEventHandler (MinusBtn.gameObject, EnumButtonEvent.OnClick, OnClickMinus, 0, 0);
		UIManager.SetButtonEventHandler (DetermineBtn.gameObject, EnumButtonEvent.OnClick, OnClicketermine, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);

	}
	void InitUIText()
	{
		_LevelLable.text = LanguageManager.instance.GetValue("Store_Level");
		_SpeciesLable.text = LanguageManager.instance.GetValue("Store_Species");
		_PriceLable.text = LanguageManager.instance.GetValue("Store_Price");
		_DesLable.text = LanguageManager.instance.GetValue("tipsmisoshu");
		_BuyLable.text = LanguageManager.instance.GetValue("Store_Buy");
	}

	private ShopData spData_;
	
	public ShopData SpData
	{
		set
		{
			if(value != null)
			{
				spData_ = value;
				count =1;
				ItemData ida = ItemData.GetData(spData_._Itemid);
				//iconSp.spriteName = ItemData.GetData(spData_.Itemid_).icon_;
				HeadIconLoader.Instance.LoadIcon (ida.icon_, iconSp);
				levelLabel.text = ida.level_.ToString();
				jiageLabel.text = spData_._Price.ToString();
				zhongleiLabel.text = LanguageManager.instance.GetValue(ida.subType_.ToString());
				nameLabel.text = spData_._Name;
				//numLabel.text = count.ToString();
				numInput.value = count.ToString();
				if(spData_._Purchase==0)
				{
					maxCount = 999;
				}else
				{
					maxCount = spData_._Purchase;
				}

				pnum.text = spData_._Num.ToString();
				DesLabel.text =ItemData.GetData(spData_._Itemid).desc_;
				iconKuang.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData(spData_._Itemid).quality_) ;
			}
		}
		get
		{
			return spData_;
		}
	}
	public void InputNum()
	{
		if(numInput.value == "0"||numInput.value =="")
		{
			numInput.value ="1";
		}
		if(numInput.value != "")
		{
			count = int.Parse(numInput.value);
		}

	}

	void OnClickPlus(ButtonScript obj, object args, int param1, int param2)
	{
		count++;
		if(count>maxCount)count = maxCount;
		//numLabel.text = count.ToString()
		numInput.value = count.ToString();
	}
	void OnClickMinus(ButtonScript obj, object args, int param1, int param2)
	{
		count--;
		if(count<1)count = 1;
		///numLabel.text = count.ToString();
		numInput.value = count.ToString();
	}
	void OnClicketermine(ButtonScript obj, object args, int param1, int param2)
	{
		//if(GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum) <= BagSystem.instance.GetBagSize())
		//{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("bagfull"));
			//PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			//return;
		//}
		string type = "";
		if(spData_._ShopPayType == ShopPayType.SPT_Diamond)
		{
			type = LanguageManager.instance.GetValue("zuanshi");
		}else if(spData_._ShopPayType == ShopPayType.SPT_Gold)
		{
			type = LanguageManager.instance.GetValue("jinbi");
		}else if(spData_._ShopPayType == ShopPayType.SPT_MagicCurrency)
		{
			type = LanguageManager.instance.GetValue("shuijing");
		}




		if(IsBalanceInadequate(SpData,count)==1)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency)>= (count*SpData._Price))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}", (count*SpData._Price).ToString ()),()=>{
					NetConnection.Instance.shopBuyItem(SpData._Id,count);
					gameObject.SetActive (false);
				});
			}
			else
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("zuanshibuzu"));
			}
		}else if(IsBalanceInadequate(SpData,count)==2)
		{			
			PopText.Instance.Show(LanguageManager.instance.GetValue("jinbibuzu"));
		}
		else if(IsBalanceInadequate(SpData,count)==3)
		{			
			PopText.Instance.Show(LanguageManager.instance.GetValue("noshuijing"));
		}
		else if(IsReachedNumber())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("goumaicishu"));
		}
		else
		{
			MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shopbuyitem").Replace("{n}",(count).ToString()).Replace("{n1}",SpData._Name).Replace("{n2}",(count*SpData._Price).ToString()+type), () => {
			NetConnection.Instance.shopBuyItem(SpData._Id,count);
			if (type.Equals(LanguageManager.instance.GetValue("zuanshi")))
				CommonEvent.ExcutePurchase(spData_._Itemid, count, spData_._Price);

				gameObject.SetActive (false);
			});
		}
		
	}
	bool IsReachedNumber()
	{
		if(SpData._Purchase==0)
		{
			return false;
		}
		return true;
	}
	int IsBalanceInadequate(ShopData sd,int num)
	{
		if(sd._ShopPayType == ShopPayType.SPT_Diamond)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Diamond]<(sd._Price * num))
			{
				return 1;
			}
		}else
			if(sd._ShopPayType == ShopPayType.SPT_Gold)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Money]<(sd._Price * num))
			{
				return 2;
			}
		}else
			if(sd._ShopPayType == ShopPayType.SPT_MagicCurrency)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_MagicCurrency]<(sd._Price * num))
			{
				return 3;
			}
		}



		return 0;
	}

	public static void shopItemOk()
	{
        PopText.Instance.Show(LanguageManager.instance.GetValue("EN_MallBuyOk"));

	}
	void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
}