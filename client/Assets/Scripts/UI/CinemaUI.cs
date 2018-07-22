using UnityEngine;
using System.Collections;

public class CinemaUI : UIBase {

    public UIButton skipBtn_;

	static UIEventListener.VoidDelegate callback_ = null;
	// Use this for initialization
	void Start () {
        UIManager.SetButtonEventHandler(skipBtn_.gameObject, EnumButtonEvent.OnClick, OnSkip, 0, 0);
	}


    #region Fixed methods for UIBase derived cass

    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_Cinema);
    }

	public static void ShowMe(UIEventListener.VoidDelegate callback = null )
    {
		callback_ = callback;
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Cinema, false);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_Cinema);
    }

    private void OnSkip(ButtonScript obj, object args, int param1, int param2)
    {
		if(callback_ != null)
		{
			callback_(null);
			return;
		}
        GameObject cinema = GameObject.Find("Cinema(Clone)");
        if (cinema != null)
            cinema.GetComponent<CinemaManager>().QuitSense();
    }

    public override void Destroyobj()
    {
       
    }

    #endregion
}
