using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamFZUI : MonoBehaviour {

	public UIButton Danbtn;
	public UIButton MegaphoneBtn;
	public UIButton InviteBtn;
	public UIButton BackCityBtn;
	public UIButton PartnerBtn;
	public UIButton worldBtn;
	public UIButton ExtBtn;
	public UIButton JiaMiBtn;
	public UIButton changeBtn;
	public UILabel mubiaolabel;
	public UILabel passwordLabel;
	public UILabel minLevelLabel;
	public UILabel maxLevelLabel;
	public GameObject mimaKuangObj;
	private int endTime;
	private float currentTime;
	private int startTime =30;
	private int second;
	public UILabel timelabel;
	private bool isCountDown = false;
	private static TeamFZUI TeamFZUIInstance = null;

	public static TeamFZUI Instance 
	{
		get{
			return TeamFZUIInstance;
		}
	}
	
	void Awake()
	{
		TeamFZUIInstance = this;
	}
	private COM_TeamInfo teamInfo_;
	public COM_TeamInfo TeamInfo
	{
		set
		{
			if(value != null)
			{
				teamInfo_ = value;
				mubiaolabel.text =  LanguageManager.instance.GetValue(teamInfo_.type_.ToString()) ;
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
		timelabel.gameObject.SetActive(false);
		UIManager.SetButtonEventHandler (Danbtn.gameObject, EnumButtonEvent.OnClick, OnClickDanbtn, 0, 0);
		UIManager.SetButtonEventHandler (MegaphoneBtn.gameObject, EnumButtonEvent.OnClick, OnClickMegaphone, 0, 0);
		UIManager.SetButtonEventHandler (InviteBtn.gameObject, EnumButtonEvent.OnClick, OnClickInvite, 0, 0);
		UIManager.SetButtonEventHandler (BackCityBtn.gameObject, EnumButtonEvent.OnClick, OnClickBackCity, 0, 0);
		UIManager.SetButtonEventHandler (PartnerBtn.gameObject, EnumButtonEvent.OnClick, OnClickPvp, 0, 0);
		UIManager.SetButtonEventHandler (worldBtn.gameObject, EnumButtonEvent.OnClick, OnClickworld, 0, 0);
		UIManager.SetButtonEventHandler (ExtBtn.gameObject, EnumButtonEvent.OnClick, OnClickExt, 0, 0);
		UIManager.SetButtonEventHandler (JiaMiBtn.gameObject, EnumButtonEvent.OnClick, OnClickJiaMi, 0, 0);
		UIManager.SetButtonEventHandler (changeBtn.gameObject, EnumButtonEvent.OnClick, OnClickchange, 0, 0);
		TeamSystem.OnExitIteam += ExitIteamOk;
		TeamSystem.OnbackCity += ReturnMainScene;
        GuideManager.Instance.RegistGuideAim(worldBtn.gameObject, GuideAimType.GAT_TeamWorldMapBtn);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_TeamUISelectMapOpen);
	}
	void ExitIteamOk()
	{

		gameObject.SetActive (false);
//		TeamUIPanel.Instance.DYObj.SetActive (false);
//		TeamUIPanel.Instance.KSObj.SetActive (true);
//		TeamUIPanel.Instance.HidenMaxMenberSizeUI ();
//		TeamUIPanel.Instance.ClearRosObj ();
	}
	void OnClickMegaphone(ButtonScript obj, object args, int param1, int param2)
{
		timelabel.gameObject.SetActive(true);
		startTime = 30;
		currentTime = 0;
		isCountDown = true;
		COM_Chat chat_com = new COM_Chat();
		chat_com.ck_ = ChatKind.CK_World;
		chat_com.content_ = LanguageManager.instance.GetValue ("yijianhanhua").Replace ("{n}", TeamInfo.leaderName_).Replace ("{n1}", TeamInfo.minLevel_.ToString()).Replace ("{n2}", TeamInfo.maxLevel_.ToString()).Replace("{t1}",TeamInfo.teamId_.ToString()).Replace("{t2}",TeamInfo.needPassword_.ToString());
		NetConnection.Instance.sendChat (chat_com,"");
		MegaphoneBtn.isEnabled = false;

	}
	void OnClickDanbtn(ButtonScript obj, object args, int param1, int param2)
	{

	}
	void OnClickInvite(ButtonScript obj, object args, int param1, int param2)
	{
		YaoQingUI.ShowMe();
	}
	void OnClickBackCity(ButtonScript obj, object args, int param1, int param2)
	{
        if (TeamSystem.IsInTeam() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
		{
			MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("tishiDuiwu"), () => {
//				NetConnection.Instance.exitLobby ();
//				ReturnMainScene();
//				ExitSceneOk();
				TeamSystem.BackCity();
			});
		}
		else
		{
			ReturnMainScene();
			//TeamUIPanel.Instance.ExitSceneOk();
		}

	}
	void ReturnMainScene()
	{
        NetConnection.Instance.transforScene(1);
	}
	void OnClickPvp(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetProperty(PropertyType.PT_Level) < 20)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		if(!TeamSystem.IsTeamOpenPvp())
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
		else
		{
			NetConnection.Instance.requestMyJJCTeamMsg();
            NetConnection.Instance.joinPvpLobby();
		}
	}
	void OnClickworld(ButtonScript obj, object args, int param1, int param2)
	{
		WorldMap.ShowMe ();
	}
	void OnClickExt(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.exitTeam ();
		NetConnection.Instance.exitLobby ();
	}
	void OnClickJiaMi(ButtonScript obj, object args, int param1, int param2)
	{
		mimaKuangObj.SetActive (true);
	}
	void OnClickchange(ButtonScript obj, object args, int param1, int param2)
	{
		ChangeTeamTargetUI.ShowMe ();
	}

	void OnDestroy()
	{
		TeamSystem.OnExitIteam -= ExitIteamOk;
		TeamSystem.OnbackCity -= ReturnMainScene;
	}
	public void Countdown()
	{
		
		currentTime += Time.fixedDeltaTime;
		endTime = startTime - Mathf.CeilToInt(currentTime);
		int shiwei = endTime / 10;
		int gewei = endTime % 10;


		timelabel.text = shiwei + "" + gewei;


		if( endTime <= 0 )
		{
			MegaphoneBtn.isEnabled = true;
			timelabel.gameObject.SetActive(false);
			isCountDown = false;
		}

	}
	// Update is called once per frame
	void Update () {
	   if(isCountDown)
		{
			Countdown();
		}
	}
}
