using UnityEngine;
using System.Collections;


public delegate void boxCallBack();
public delegate void LabelClickCallBack(string param);
public class MessageBoxUI : UIBase
{

	public UILabel _TitleLable;
	public UILabel _EnterLable;
	public UILabel _CanleLable;

	public UIButton okBtn;
	public UIButton cancelBtn;
	public UILabel contentLab;
	public GameObject okCancelBack;
	public UIButton textOk;
    public GetInfoOnClick _Underline;
	static string _content;
	static boxCallBack _callBack;
    static boxCallBack _CancelCallBack;
    static LabelClickCallBack _ClickLable;
	static bool singleBtn_;
    static bool ignoreDestroyOkCallback_;

    static string okDesc_, cancelDesc_;

	static int depth_ = 2000;
    
	void Start ()
	{
		_TitleLable.text = LanguageManager.instance.GetValue ("Mesaage_Title");
		_EnterLable.text = LanguageManager.instance.GetValue ("Mesaage_Enter");
		_CanleLable.text = LanguageManager.instance.GetValue ("Mesaage_Canel");
		UIManager.SetButtonEventHandler (okBtn.gameObject, EnumButtonEvent.OnClick, OnClickOk, 0, 0);
		UIManager.SetButtonEventHandler (cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);
        UIManager.SetButtonEventHandler(textOk.gameObject, EnumButtonEvent.OnClick, OnClickOk, 0, 0);
        GetInfoOnClick.OnClickInformation += ClickInformationHandler;
        contentLab.text = _content;
        okBtn.GetComponentInChildren<UILabel>().text = okDesc_;
        cancelBtn.GetComponentInChildren<UILabel>().text = cancelDesc_;
		if(singleBtn_)
		{
			okCancelBack.gameObject.SetActive(false);
			textOk.gameObject.SetActive(true);
		}

        //if (gameHandler._SdkInitSuccess)
        //{
            GuideManager.Instance.RegistGuideAim(okBtn.gameObject, GuideAimType.GAT_MessageBoxOkBtn);
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MessageBoxOpen);
        //}
		this.gameObject.transform.localPosition = new Vector3 (0, 0, -2000);
    
		UIPanel panel = gameObject.GetComponent<UIPanel>();
		panel.depth = depth_;
		panel.sortingOrder = depth_;
	}
	
	public static void ShowMe(string content, boxCallBack callback = null,bool singleBtn = false, boxCallBack cancelCallBack = null, LabelClickCallBack lccb = null, string okDesc = "", string cancelDesc = "", int depth = 2000, bool ignoreDestroyCallback = false)
	{
        GuideManager.Instance.ClearMask();
		_content = content;
		_callBack = callback;
        _CancelCallBack = cancelCallBack;
        singleBtn_ = singleBtn;
        _ClickLable = lccb;
        ignoreDestroyOkCallback_ = ignoreDestroyCallback;
        if(string.IsNullOrEmpty(okDesc))
            okDesc_ = LanguageManager.instance.GetValue("confirm");
		else
			okDesc_ = okDesc;
        if(string.IsNullOrEmpty(cancelDesc))
            cancelDesc_ = LanguageManager.instance.GetValue("cancel");
		else
			cancelDesc_ = cancelDesc;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MessageBoxPanel);
	}

	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_MessageBoxPanel);
	}

	private void OnClickOk(ButtonScript obj, object args, int param1, int param2)
	{
		if(_callBack != null)
		{
            boxCallBack tmpCb = _callBack;
            _callBack = null;
            tmpCb();
		}
		Hide ();
	}

	bool clickCancel = false;
	private void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
        if (_CancelCallBack != null)
		{
            _CancelCallBack();
			_CancelCallBack = null;
		}
		clickCancel = true;
		Hide ();
	}

    private void ClickInformationHandler(string info, int param = 0)
    {
        if (null == _ClickLable)
            return;

        _ClickLable(info);
    }

	public override void Destroyobj ()
	{
        if (!ignoreDestroyOkCallback_)
        {
            if (_callBack != null && clickCancel == false)
                _callBack();
            _callBack = null;
        }
        ApplicationEntry.Instance.netStatusWarning_ = false;
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_MessageBoxPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
}

