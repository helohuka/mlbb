using UnityEngine;
using System.Collections;

public class updateNoticeUI : UIBase
{

	public UILabel contentLab;
	public UILabel titleLab;
	public UILabel dayLab;
	public UIButton closeBtn;
	public UIButton okBtn;
	public UIButton hideBtn;

    static string title_;
    static string date_;
    static string content_;
    static UIEventListener.VoidDelegate callback_ = null;

    void Start()
    {
        titleLab.text = title_;
        contentLab.text = content_;
        dayLab.text = date_;
        OpenPanelAnimator.PlayOpenAnimation(this.panel, () =>
        {
            UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
            UIManager.SetButtonEventHandler(okBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
            UIManager.SetButtonEventHandler(hideBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
        });
    }

    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        OpenPanelAnimator.CloseOpenAnimation(this.panel, () =>
        {
            if (callback_ != null)
            {
                callback_(null);
                Clear();
            }
            Hide();
        });
    }

    void Clear()
    {
        title_ = "";
        date_ = "";
        content_ = "";
        callback_ = null;
    }

    void OnDestroy()
    {
        UIManager.RemoveButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(okBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(hideBtn.gameObject, EnumButtonEvent.OnClick);
    }

    public static void ShowMe(string title, string content, string date, UIEventListener.VoidDelegate callback = null)
    {
        title_ = title;
        date_ = date;
        content_ = content;
        callback_ = callback;
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_NoticePanel);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_NoticePanel);
    }

    public override void Destroyobj()
    {

    }
}

