using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPvpPanelUI : UIBase
{
	public UILabel timeLab;
	public UISprite timeImg;
	public UIButton closeBtn;
	public UIButton startBtn;
	public UIButton stopBtn;

	public List<ArenaPvpPlayerCellUI> leftPlayers = new List<ArenaPvpPlayerCellUI>();
	public List<ArenaPvpPlayerCellUI> rightPlayers = new List<ArenaPvpPlayerCellUI>();
	public GameObject pkObj;


	private float _matching;
	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnStart, 0, 0);
		UIManager.SetButtonEventHandler (stopBtn.gameObject, EnumButtonEvent.OnClick, OnStop, 0, 0);
		ArenaPvpSystem.Instance.PvpMatchingEnven += new RequestEventHandler<bool>(PvpMatching);
		ArenaPvpSystem.Instance.playerTeamEnven += new RequestEventHandler<COM_SimpleInformation[]>(PvpTeamEnven);
		ArenaPvpSystem.Instance.playerSingleEnven += new  RequestEventHandler<COM_SimpleInformation>(PvpSingleEnven);
		TeamSystem.OnUpdateTeamMB += UpdateTeamMBOk;
		TeamSystem.OnDelMenber+= OnDelTeamPlayerEnvt;

		UpdateMyInfo ();

		ArenaSystem.Instance.openPvP = true;
		ArenaPvpSystem.Instance.openPvpUI = true;
		OpenPanelAnimator.PlayOpenAnimation(this.panel);
		GameManager.chatobj.SetActive(true);
		Vector3 chatPos = new Vector3 ();
		chatPos = GameManager.chatobj.transform.localPosition;
		UIManager.Instance.AdjustUIDepth(GameManager.chatobj.transform);
		GameManager.chatobj.transform.localPosition = chatPos; 

		startBtn.gameObject.SetActive(false);
		stopBtn.gameObject.SetActive(false);
		if(TeamSystem.IsTeamLeader())
		{
			startBtn.gameObject.SetActive(true);
		}

	}

	void Update ()
	{
		if(ArenaPvpSystem.Instance.PvpMatching )
		{
			timeImg.gameObject.SetActive(true);
			timeLab.text =  FormatTime((int)ArenaPvpSystem.Instance.GetMatchingTimeOut);
		}
		else
		{
			timeImg.gameObject.SetActive(false);
		}
	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ArenaPvpPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ArenaPvpPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_ArenaPvpPanel);
	}
	
	#endregion
	
	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.stopMatching ();
		ArenaSystem.Instance.openPvP = false;
		ArenaPvpSystem.Instance.openPvpUI = false;
		if(TeamSystem.IsTeamLeader())
		{
			NetConnection.Instance.warriorStop();
		}
		ArenaPvpSystem.Instance.PvpMatching = false;
       //NetConnection.Instance.exitPvpLobby();
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	private void OnStart(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.startMatching ();
		NetConnection.Instance.warriorStart ();
	}

	private void OnStop(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.stopMatching ();
		NetConnection.Instance.warriorStop ();
	}

	private void  UpdateMyInfo()
	{
		COM_PlayerVsPlayer info = ArenaPvpSystem.Instance.MyInfo; 
		if(info == null)
		{
			return;
		}
		PlayerData pdata = PlayerData.GetData ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_TableId]);
		EntityAssetsData enData = EntityAssetsData.GetData (pdata.lookID_);

		if(!_icons.Contains(enData.assetsIocn_))
		{
			_icons.Add(enData.assetsIocn_);
		}

		Profession profession = Profession.get((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), (int)GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));

		COM_SimplePlayerInst[] team = TeamSystem.GetTeamMembers ();
		if(team != null && team.Length > 0)
		{
			MyTeamInfo(team);
		}
		else
		{
			MysingleInfo(info);
		}
	}


	private void MyTeamInfo(COM_SimplePlayerInst[] team)
	{

		for(int j=0;j<leftPlayers.Count;j++)
		{
			leftPlayers[j].icon.gameObject.SetActive(false);
		}
		uint section = 0;
		for(int i =0;i<team.Length;i++)
		{
			leftPlayers[i].gameObject.SetActive(true);
			leftPlayers[i].infoImg.gameObject.SetActive(true);
			leftPlayers[i].nobody.gameObject.SetActive(false);
			leftPlayers[i].namelab.text =team[i].instName_;
			leftPlayers[i].levelLab.text= team[i].properties_[(int)PropertyType.PT_Level].ToString();
			leftPlayers[i].professionImg.spriteName = ((JobType)team[i].properties_[(int)PropertyType.PT_Profession]).ToString();
			EntityAssetsData eData = EntityAssetsData.GetData ((int)team[i].properties_[(int)PropertyType.PT_AssetId]);
			HeadIconLoader.Instance.LoadIcon(eData.assetsIocn_, leftPlayers[i].icon);
			if(!_icons.Contains(eData.assetsIocn_))
			{
				_icons.Add(eData.assetsIocn_);
			}

			leftPlayers[i].icon.gameObject.SetActive(true);
			leftPlayers[i].professionLab.text = Profession.get ((JobType)team[i].properties_[(int)PropertyType.PT_Profession], (int)team[i].properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_;	
			//	section += team[i].section_;??
		}

		List<COM_SimplePlayerInst> myTeam = new List<COM_SimplePlayerInst> ();
		for(int k =0;k<team.Length;k++)
		{
			if(team[k].instId_ != GamePlayer.Instance.InstId)
			{
				myTeam.Add(team[k]);
			}
		}




	}

	private void MysingleInfo(COM_PlayerVsPlayer info)
	{
		leftPlayers[0].gameObject.SetActive(true);
		leftPlayers[0].infoImg.gameObject.SetActive(true);
		leftPlayers[0].nobody.gameObject.SetActive(false);
		leftPlayers [0].namelab.text = GamePlayer.Instance.InstName; 
		leftPlayers[0].levelLab.text= GamePlayer.Instance.GetIprop(PropertyType.PT_Level).ToString();
		leftPlayers[0].professionImg.spriteName = ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession)).ToString();
		EntityAssetsData eData = EntityAssetsData.GetData ((int)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId));
		HeadIconLoader.Instance.LoadIcon(eData.assetsIocn_, leftPlayers[0].icon);

		if(!_icons.Contains(eData.assetsIocn_))
		{
			_icons.Add(eData.assetsIocn_);
		}

		leftPlayers[0].icon.gameObject.SetActive(true);
	}



	public void PvpMatching(bool matching)
	{
		if (!pkObj.activeSelf) 
		{
			pkObj.SetActive (true);
		}
		if(matching)
		{
			startBtn.gameObject.SetActive(false);
			stopBtn.gameObject.SetActive(false);
			closeBtn.gameObject.SetActive (false);
			if(TeamSystem.IsTeamLeader())
			{
				stopBtn.gameObject.SetActive(true);
			}

			for(int i=0;i<rightPlayers.Count;i++)
			{
				//rightPlayers[i].gameObject.SetActive(true);
				rightPlayers[i].StartRoll();
			}
		}
		else
		{
			startBtn.gameObject.SetActive(false);
			stopBtn.gameObject.SetActive(false);
			closeBtn.gameObject.SetActive (true);
			if(TeamSystem.IsTeamLeader())
			{
				startBtn.gameObject.SetActive(true);
			}
			for(int i=0; i < rightPlayers.Count; ++i)
			{
				rightPlayers[i].StopRoll();
			}
		}
	}


	void PvpTeamEnven(COM_SimpleInformation[] team)
	{
		if (!pkObj.activeSelf) 
		{
			pkObj.SetActive (true);
		}
		ArenaPvpSystem.Instance.PvpMatching = false;
		for(int i=0;i<team.Length;i++)
		{
			rightPlayers[i].StopRoll();
			rightPlayers[i].nobody.gameObject.SetActive(false);
			rightPlayers[i].infoImg.gameObject.SetActive(true);
			rightPlayers[i].icon.gameObject.SetActive(true);
			rightPlayers[i].namelab.text = team[i].instName_;
			rightPlayers[i].professionImg.spriteName = team[i].jt_.ToString();
			rightPlayers[i].levelLab.text = team[i].level_.ToString();
			EntityAssetsData eData = EntityAssetsData.GetData (team[i].asset_id_);
			HeadIconLoader.Instance.LoadIcon( eData.assetsIocn_,rightPlayers[i].icon);
			if(!_icons.Contains(eData.assetsIocn_))
			{
				_icons.Add(eData.assetsIocn_);
			}


			rightPlayers[i].professionLab.text = Profession.get ((JobType)team[i].jt_, (int)team[i].jl_).jobName_;	
		}

		for(int i =rightPlayers.Count;i>team.Length;i--)
		{
			rightPlayers[i-1].gameObject.SetActive(false);
		}

		closeBtn.gameObject.SetActive (false);
		stopBtn.gameObject.SetActive (false);
		startBtn.gameObject.SetActive (false);
		StartCoroutine (DelayBattle (3f));
	}


	void OnDelTeamPlayerEnvt(int a)
	{
		COM_SimplePlayerInst[] team = TeamSystem.GetTeamMembers ();
		if(team != null && team.Length > 0)
		{
			MyTeamInfo(team);
		}
	}
	void PvpSingleEnven(COM_SimpleInformation player)
	{
		ArenaPvpSystem.Instance.PvpMatching = false;
	
		for(int i=0;i<rightPlayers.Count;i++)
		{
			rightPlayers[i].StopRoll();
		}

		rightPlayers[0].nobody.gameObject.SetActive(false);
		rightPlayers[0].infoImg.gameObject.SetActive(true);
		rightPlayers[0].icon.gameObject.SetActive(true);
		rightPlayers[0].namelab.text = player.instName_;
		rightPlayers[0].professionImg.spriteName = player.jt_.ToString();
		rightPlayers[0].levelLab.text = player.level_.ToString();
		EntityAssetsData eData = EntityAssetsData.GetData (player.asset_id_);
		HeadIconLoader.Instance.LoadIcon( eData.assetsIocn_,rightPlayers[0].icon);
		if(!_icons.Contains(eData.assetsIocn_))
		{
			_icons.Add(eData.assetsIocn_);
		}
		rightPlayers[0].professionLab.text = Profession.get ((JobType)player.jt_, (int)player.jl_).jobName_;	

		closeBtn.gameObject.SetActive (false);
		stopBtn.gameObject.SetActive (false);
		startBtn.gameObject.SetActive (false);
		StartCoroutine (DelaysingleBattle (3f));
	}



	protected override void DoHide ()
	{
		ArenaPvpSystem.Instance.PvpMatchingEnven -= PvpMatching;
		ArenaPvpSystem.Instance.playerTeamEnven -= PvpTeamEnven;
		ArenaPvpSystem.Instance.playerSingleEnven -= PvpSingleEnven;
		TeamSystem.OnUpdateTeamMB -= UpdateTeamMBOk;
		TeamSystem.OnDelMenber -= OnDelTeamPlayerEnvt;

		base.DoHide ();
	}

	public override void Destroyobj () 
	{
		GameObject.Destroy (gameObject);
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}

	}

	public string FormatTime(int time)
	{
		int min = time/60;
		int second = time%60;
		return DoubleTime(min) + ":" + DoubleTime(second);
	}

	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}

	IEnumerator DelayBattle(float dTime)
	{
		yield return new WaitForSeconds(dTime);
		if(TeamSystem.IsInTeam())
		{
			if(TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
			{
				NetConnection.Instance.jjcBattleGo (ArenaPvpSystem.Instance.TeamId);
			}
		}
	}

	IEnumerator DelaysingleBattle(float dTime)
	{
		yield return new WaitForSeconds(dTime);

		NetConnection.Instance.jjcBattleGo ((uint)ArenaPvpSystem.Instance.playerSingle.instId_);
	}


	void UpdateTeamMBOk(COM_SimplePlayerInst info)
	{
		UpdateMyInfo ();
	}

}

