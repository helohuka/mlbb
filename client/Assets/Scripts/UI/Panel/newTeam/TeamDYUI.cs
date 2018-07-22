using UnityEngine;
using System.Collections;

public class TeamDYUI : MonoBehaviour {

	public UILabel mubiaoLabel;
	public UILabel passwordLabel;
	public UILabel minLevelLabel;
	public UILabel maxLevelLabel;
	public UIButton extTeam;
	private COM_TeamInfo teamInfo_;
	public COM_TeamInfo TeamInfo
	{
		set
		{
			if(value != null)
			{
				teamInfo_ = value;
				mubiaoLabel.text = LanguageManager.instance.GetValue(teamInfo_.type_.ToString());
				passwordLabel.text = teamInfo_.pwd_;
				minLevelLabel.text = teamInfo_.minLevel_.ToString()+"级";
				maxLevelLabel.text = teamInfo_.maxLevel_.ToString()+"级";
			}
		}
		get
		{
			return teamInfo_;
		}
	}
	void Start () {

		UIManager.SetButtonEventHandler (extTeam.gameObject, EnumButtonEvent.OnClick, OnClickextTeam, 0, 0);
		TeamSystem.OnExitIteam += ExitIteamOk;
	}
	void ExitIteamOk()
	{
		gameObject.SetActive (false);
//		TeamUIPanel.Instance.DYObj.SetActive (false);
//		TeamUIPanel.Instance.KSObj.SetActive (true);
//		TeamUIPanel.Instance.HidenMaxMenberSizeUI ();
//		TeamUIPanel.Instance.ClearRosObj ();
	}
	void OnClickbackCity(ButtonScript obj, object args, int param1, int param2)
	{

			MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("tishiDuiwu"), () => {

				NetConnection.Instance.exitTeam ();
			    NetConnection.Instance.exitLobby ();
			});


	}
	void OnClickHuoabn(ButtonScript obj, object args, int param1, int param2)
	{
		
	}
	void OnClickextTeam(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.exitTeam ();
		//ChatSystem.instance.AddchatInfo(TeamInfo.name_+LanguageManager.instance.GetValue("likaiduiwu"),ChatKind.CK_System);
	}
	void OnDestroy()
	{
		TeamSystem.OnExitIteam -= ExitIteamOk;
	}


}
