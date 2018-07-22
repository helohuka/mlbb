using UnityEngine;
using System.Collections;

public class ArenaEntryUI : UIBase
{
	public UIButton closeBtn;
	public UIButton pvpBtn;
	public UIButton pveBtn;
	public UILabel arenaOnlineTitleLab;
	public UILabel arenaOfflineTitleLab;
	public UILabel arenaIn1Lab;
	public UILabel arenaIn2Lab;
	public UILabel arenaOnlineInfoLab;
	public UILabel arenaOfflineInfoLab;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (pvpBtn.gameObject, EnumButtonEvent.OnClick, OnPvp, 0, 0);
		UIManager.SetButtonEventHandler (pveBtn.gameObject, EnumButtonEvent.OnClick, OnPve, 0, 0);
		OpenPanelAnimator.PlayOpenAnimation(this.panel);
        GuideManager.Instance.RegistGuideAim(pveBtn.gameObject, GuideAimType.GAT_OfflineJJC);

		arenaOnlineTitleLab.text= LanguageManager.instance.GetValue("arenaOnlineTitleLab");
		arenaOfflineTitleLab.text= LanguageManager.instance.GetValue("arenaOfflineTitleLab");
		arenaIn1Lab.text= LanguageManager.instance.GetValue("arenaIn1Lab");
		arenaIn2Lab.text= LanguageManager.instance.GetValue("arenaIn1Lab");
		arenaOnlineInfoLab.text= LanguageManager.instance.GetValue("arenaOnlineInfoLab");
		arenaOfflineInfoLab.text= LanguageManager.instance.GetValue("arenaOfflineInfoLab");


        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_JJCEntryUI);
	}


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ArenaEntryPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ArenaEntryPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_ArenaEntryPanel);
	}
	
	#endregion

	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	private void OnPvp(ButtonScript obj, object args, int param1, int param2)
	{
		if( !GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_PVPJJC))

		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		/*if(!TeamSystem.IsTeamOpenPvp())
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("teamlevelpvp"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("teamlevelpvp"));
			return ;
		}
		if(!TeamSystem.IsTeamLeader())
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("teamopen"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("teamopen"));
			return;
		}
		*/
		else
		{
			NetConnection.Instance.requestMyJJCTeamMsg();
			ArenaPvpPanelUI.SwithShowMe ();
		}
	}


	private void OnPve(ButtonScript obj, object args, int param1, int param2)
	{
		if(TeamSystem.MemberCount > 0)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("teamnopve"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("teamnopve"));
			return;
		}
		if( !GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_JJC))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		ArenaUI.SwithShowMe ();
	}

}

