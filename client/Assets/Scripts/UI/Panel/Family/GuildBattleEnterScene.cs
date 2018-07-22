using UnityEngine;
using System.Collections;

public class GuildBattleEnterScene : UIBase {

	public UILabel selfNameLable;
	public UILabel selfLevelLable;
	public UILabel selfNumLable;

	public UILabel otherNameLable;
	public UILabel otherLevelLable;
	public UILabel otherNumLable;

	public UILabel wanfajieshaoLable;

	public UIButton closeBtn;
	public UIButton enterBtn;
	void Start () {
		otherNameLable.text = GuildSystem.otherName;
		otherLevelLable.text = GuildSystem.otherlevel.ToString();
		otherNumLable.text = GuildSystem.otherNum.ToString ();
		selfNameLable.text = GuildSystem.Mguild.guildName_;
		selfLevelLable.text = GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_.ToString();
		selfNumLable.text = GuildSystem.GuildMembers.Count.ToString ();
		wanfajieshaoLable.text = LanguageManager.instance.GetValue ("guildBattlejieshao");
		UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	private void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{
		if(GuildSystem.IsInGuild())
		{
			if(GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_<2)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("dengjibuzu"));
			return;
			}
			if(TeamSystem.IsInTeam())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengzudui"));
				return;
			}
		}


		NetConnection.Instance.transforGuildBattleScene ();
	}
	void Update () {
	
	}
	public override void Destroyobj ()
	{

	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GuildBattlePanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GuildBattlePanel);
	}

}
