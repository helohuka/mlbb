using UnityEngine;
using System.Collections;

public class FamilyShopBuyUI : MonoBehaviour {


	public UILabel _BuyTitle;
	public UILabel _XiaohaoLable;
	public UILabel _DesLable;
	public UILabel _GMLable;


	public UITexture iconSp;
	public UILabel pnum;
	public UILabel nameLabel;
	public UILabel DesLabel;
	public UILabel numLabel;
	public UIButton PlusBtn;
	public UIButton MinusBtn;
	public UIButton DetermineBtn;
	public UIButton CancelBtn;
	public UISprite moneyIcon;
	public UILabel needMoneyLab;
	private int count = 0;
	private int maxCount = 0;

    private HomeShopData hdata_;
	public HomeShopData Hdata
	{
		set
		{
			if(value != null)
			{
				hdata_ = value;
				count =1;
				//iconSp.spriteName = ItemData.GetData(spData_.Itemid_).icon_;
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(hdata_.Itemid_).icon_, iconSp);
				nameLabel.text = hdata_.name_;
				//numLabel.text = count.ToString();
				needMoneyLab.text = hdata_.Price_.ToString();
//				if(hdata_.timeLimit_==0)
//				{
//					maxCount = 20;
//				}else
//				{
					maxCount = hdata_.timeLimit_;
//				}
				
				pnum.text = hdata_.Num_.ToString();
				DesLabel.text =ItemData.GetData(hdata_.Itemid_).desc_;
			}
		}
		get
		{
			return hdata_;
		}
	}
	void Start () {
		_BuyTitle.text = LanguageManager.instance.GetValue("Guild_BuyTitle");
		_XiaohaoLable.text = LanguageManager.instance.GetValue("Guild_Xiaohao");
		_DesLable.text = LanguageManager.instance.GetValue("Guild_DesBuy");
		_GMLable.text = LanguageManager.instance.GetValue("Guild_GMBuy");
		//UIManager.SetButtonEventHandler (PlusBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlus, 0, 0);
		//UIManager.SetButtonEventHandler (MinusBtn.gameObject, EnumButtonEvent.OnClick, OnClickMinus, 0, 0);
		UIManager.SetButtonEventHandler (DetermineBtn.gameObject, EnumButtonEvent.OnClick, OnClicketermine, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);
		GuildSystem.BuyGuildshopOK += BuyGuildshopOK;

	}
	void BuyGuildshopOK(short items)
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue("EN_MallBuyOk"));
	}
//	void OnClickPlus(ButtonScript obj, object args, int param1, int param2)
//	{
//		count++;
//		if(count>maxCount)count = maxCount;
//		numLabel.text = count.ToString();
//		needMoneyLab.text = (Hdata.Price_ * count).ToString ();
//	}
//	void OnClickMinus(ButtonScript obj, object args, int param1, int param2)
//	{
//		count--;
//		if(count<1)count = 1;
//		numLabel.text = count.ToString();
//		needMoneyLab.text = (Hdata.Price_ * count).ToString ();
//	}
	void OnClicketermine(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum) <= BagSystem.instance.GetBagSize())
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("bagfull"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}
		string type = "";
		type = LanguageManager.instance.GetValue ("Guild_gongxian");
		int gongx = GuildSystem.GetGuildMemberSelf (GamePlayer.Instance.InstId).contribution_;
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shopbuyitem").Replace("{n}",(count*Hdata.Num_).ToString()).Replace("{n1}",Hdata.name_).Replace("{n2}",(count*Hdata.Price_).ToString()+type), () => {

			if(gongx<Hdata.Price_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("familyGongxian"));
				return;
			}



			NetConnection.Instance.buyGuildItem(Hdata.id_,count);
		});
		gameObject.SetActive (false);

	}
	void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void OnDestroy()
	{
		GuildSystem.BuyGuildshopOK += BuyGuildshopOK;
	}

}
