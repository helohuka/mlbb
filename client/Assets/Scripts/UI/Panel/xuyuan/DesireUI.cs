using UnityEngine;
using System.Collections;

public class DesireUI : UIBase {

	public UIButton CloseBtn;
	public UIButton NextBtn;
	public UIButton AddFBtn;
	public UILabel NameLable;
	public UILabel DesLable;
	private int _Money;
	private int _Exp;
	void Start () {
		NameLable.text = WishingTreeSystem._Wish.playerName_;
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseObj,1, 0);
		UIManager.SetButtonEventHandler (NextBtn.gameObject, EnumButtonEvent.OnClick, OnClickNextBtn,1, 0);
		UIManager.SetButtonEventHandler (AddFBtn.gameObject, EnumButtonEvent.OnClick, OnClickAddBtn,1, 0);
		WishingTreeSystem.ShowWishOk += ShowOk;
		ShowOk (WishingTreeSystem._Wish);
	}
	void ShowOk(COM_Wish Wish)
	{
		DesLable.text = Wish.wish_;
		GlobalValue.Get(Constant.C_WishShareMoney, out _Money);
		GlobalValue.Get(Constant.C_WishShareExp, out _Exp);
		PopText.Instance.Show (LanguageManager.instance.GetValue("xuyuantishi").Replace("{n}",_Money.ToString()).Replace("{n1}",_Exp.ToString()));
	}
	void OnClickCloseObj(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickNextBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int wishShareNum_ = 0;
		GlobalValue.Get(Constant.C_WishShareMaxNum, out wishShareNum_);

		if(GamePlayer.Instance.wishShareNum_<wishShareNum_)
		{
			NetConnection.Instance.requestWish();	
		}
		else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyoucishu"));
		}
		GamePlayer.Instance.wishShareNum_++;
	}
	void OnClickAddBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count >= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		NetConnection.Instance.addFriend (WishingTreeSystem._Wish.playerInstId_);
	}

	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_XuyuanPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_XuyuanPanel);
	}
	public override void Destroyobj ()
	{
		WishingTreeSystem.ShowWishOk -= ShowOk;
	}
}
