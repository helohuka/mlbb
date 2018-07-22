using UnityEngine;
using System.Collections;

public class PhoneBinding : UIBase {

	public UIInput phoneNumInput;
	public UIInput maNumInput;
	public UIButton huoquBtn;
	public UIButton enterBtn;
	public UISprite iconKung;
	public UITexture back;
	void Start () {
		int hpid = 0;
		HeadIconLoader.Instance.LoadIcon("leijichongzhi1", back);
		GlobalValue.Get(Constant.C_PhoneItem, out hpid);
		UIManager.Instance.AddItemCellUI (iconKung,(uint)hpid ).showTips = true;
		UIManager.SetButtonEventHandler (huoquBtn.gameObject, EnumButtonEvent.OnClick,OnClickhuoquBtn, 0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick,OnClickenterBtn, 0, 0);
        if (!string.IsNullOrEmpty(GameManager.Instance.mobileNum))
            phoneNumInput.value = GameManager.Instance.mobileNum;
	}
	void OnClickhuoquBtn(ButtonScript obj, object args, int param1, int param2)
	{
        if (GlobalInstanceFunction.Instance.SMSStartCount)
        {
            PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("smssendfast"), GlobalInstanceFunction.Instance.GetSMSLeftCD), PopText.WarningType.WT_Warning);
            return;
        }
		if(phoneNumInput.value == "")
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingshurushoujihao"));
			return;
		}
        GameManager.Instance.mobileNum = phoneNumInput.value;
        NetConnection.Instance.verificationSMS(GameManager.Instance.mobileNum, "");
        //XyskIOSAPI.RequestCode(mobileStr, game.GameUser.getInstance().getUserID());
        GlobalInstanceFunction.Instance.SMSStartCount = true;
	}
	void OnClickenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.verificationSMS (phoneNumInput.value, maNumInput.value);
        //XyskIOSAPI.Auth(GameManager.Instance.mobileNum, game.GameUser.getInstance().getUserID(), maNumInput.value);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_PhoneBinding);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_PhoneBinding);
	}

	public override void Destroyobj ()
	{

	}
}
