using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FamilyMonsterUI : UIBase
{
	public UIButton closeBtn;
	public UIButton battleBtn;
	public List<FamilyMonsterCellUI> cellList = new List<FamilyMonsterCellUI>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (battleBtn.gameObject, EnumButtonEvent.OnClick, OnClickBattle, 0, 0);
		COM_Guild guild = FamilySystem.instance.GuildData;
		if (guild == null)
			return;
		for(int i=0;i<guild.progenitus_.Length;i++)
		{
			cellList[i].Monster = guild.progenitus_[i];
		}
	}

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyMonster);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyMonster);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyMonster);
	}
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	private void OnClickBattle(ButtonScript obj, object args, int param1, int param2)
	{
		FamilyMonsterBattleUI.ShowMe ();
	}
}

