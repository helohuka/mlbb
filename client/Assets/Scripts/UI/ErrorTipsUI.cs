using UnityEngine;
using System.Collections;

public class ErrorTipsUI : UIBase
{
	public UILabel tishiLabel;
	public UISprite kuang;
	static string content_;
	
	void Start () {
		tishiLabel.text = string.Format("[b]{0}[-]", content_);
		
		kuang.GetComponent<TweenAlpha> ().SetOnFinished (OnAlphaFinish);
		
	}
	
	public static void ShowMe(string content)
	{
		content_ = content;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__ErrorTipsUI);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__ErrorTipsUI);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS__ErrorTipsUI);
	}
	public override void Destroyobj ()
	{
		
	}
	
	void OnAlphaFinish()
	{
		Hide();
	}
}

