using UnityEngine;
using System.Collections;

public class LookTreeUI : UIBase {

	public UIButton CloseBtn;
	public UIButton LookBtn;
	void Start () {
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseObj,1, 0);
		UIManager.SetButtonEventHandler (LookBtn.gameObject, EnumButtonEvent.OnClick, OnClickLookBtn,1, 0);
		WishingTreeSystem.ShowWishOk += ShowOk;
	}

	void ShowOk (COM_Wish Wish)
	{
		DesireUI.SwithShowMe ();
		Hide ();
	}
	
	void OnClickCloseObj(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickLookBtn(ButtonScript obj, object args, int param1, int param2)
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
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_LookchiPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_LookchiPanel);
	}
	public override void Destroyobj ()
	{
		WishingTreeSystem.ShowWishOk -= ShowOk;
	}
}
