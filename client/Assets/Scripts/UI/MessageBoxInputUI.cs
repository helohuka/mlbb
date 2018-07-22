using UnityEngine;
using System.Collections;

public delegate void boxInputCallBack(string str);
public class MessageBoxInputUI : UIBase {

	public UILabel _TishiOneLable;
	public UILabel _TiShiTwoLable;
	public UILabel _EnterLable;
	public UILabel _CanelLable;

	public UIButton enterBtn;
	public UIButton canelBtn;
	public UIInput minput;
    static boxInputCallBack _callBack;
	void Start () {
		_TishiOneLable.text = LanguageManager.instance.GetValue ("Message_TishiOne");
		_TiShiTwoLable.text = LanguageManager.instance.GetValue ("Message_TiShiTwo");
		_EnterLable.text = LanguageManager.instance.GetValue ("Mesaage_Enter");
		_CanelLable.text = LanguageManager.instance.GetValue ("Mesaage_Canel");
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickOk, 0, 0);
		UIManager.SetButtonEventHandler (canelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);
	}
	private void OnClickOk(ButtonScript obj, object args, int param1, int param2)
	{
		if(_callBack != null)
		{
			_callBack(minput.value);
		}

	}
	public void CloseBtn()
	{
		Hide ();
	}
	private void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	public static void ShowMe(boxInputCallBack callback = null)
	{
		_callBack = callback;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MessageBoxInputPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_MessageBoxInputPanel);
	}

	public override void Destroyobj ()
	{

	}
}
