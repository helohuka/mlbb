using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TeamUI : UIBase {

	public UILabel _MainTitleLable;
	public UILabel _MyTeamNameLable;
	public UILabel _TeamTargetLable;
	public UILabel _PromptLable;
	public UILabel _CreateTeamLable;
	public UILabel _ETeamLable;
	public UILabel _PartnerLable;
	public UILabel _ConvenientTeamLable;
	public UILabel _TemporarilyLable;
	public UILabel _BackTeamLable;
	public UILabel _InviteTeamLable;
	public UILabel _MegaphoneLable;


	public delegate void UpdateMemberPosition ();
	public static UpdateMemberPosition UpdateMemberPositionUIOk;



	public List <GameObject> teamCells = new List<GameObject>();
	public UIButton CloseBtn;
	public UIButton cerateTeamBtn;
	public UIButton BianjieBtn;
	public GameObject duaizhangObj;
	public GameObject duaiyuanObj;
	public GameObject datingObj;
	public GameObject duaizhangTips;
	public GameObject duaiyuanTips;
	public GameObject mask_;
    public GameObject guideObj_;

	public UILabel teamnameLabel;
	public UILabel mubiaoLabel;
	public GameObject mubiaoObj;
	public GameObject teamnameObj;
	public UIButton XiugaiBtn;
	public UIButton YaoqingBtn;
	public UIButton hanhuaBt;
	public UIButton tuichuBtn;
	public UIButton huobanBtn;
	public UIButton zanshiBtn;
	public UIButton backTeamBtn;
	public UIButton tipsCloseBtn;
	public UIButton HyBtn;
	public UIButton playerInfoBtn;
	//public GameObject chenkPlayerObj;
	public static List<GameObject>rosobj = new List<GameObject>();

	private static bool isCreate;
	private bool hasDestroy_;

	void InitUIText()
	{
		_MainTitleLable.text = LanguageManager.instance.GetValue ("Team_MainTitle");
		_MyTeamNameLable.text = LanguageManager.instance.GetValue ("Team_MyTeamName");
		_TeamTargetLable.text = LanguageManager.instance.GetValue ("Team_TeamTarget");
		_PromptLable.text = LanguageManager.instance.GetValue ("Team_Prompt");
		_CreateTeamLable.text = LanguageManager.instance.GetValue ("Team_Create");
		_ETeamLable.text = LanguageManager.instance.GetValue ("Team_ETeam");
		_PartnerLable.text = LanguageManager.instance.GetValue ("Team_Partner");
		_ConvenientTeamLable.text = LanguageManager.instance.GetValue ("Team_Convenient");
		_TemporarilyLable.text = LanguageManager.instance.GetValue ("Team_Temporarily");
		_BackTeamLable.text = LanguageManager.instance.GetValue ("Team_BackTeam");
		_InviteTeamLable.text = LanguageManager.instance.GetValue ("Team_Invite");
		_MegaphoneLable.text = LanguageManager.instance.GetValue ("Team_Megaphone");
	}

	void Start () {
		hasDestroy_ = false;
		NetConnection.Instance.requestFriendList ();
		InitUIText ();
		guideAnimator_ = gameObject.GetComponent<Animator>();
		RestMemberCellUI ();
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (cerateTeamBtn.gameObject, EnumButtonEvent.OnClick, OnClickCreate, 0, 0);
		UIManager.SetButtonEventHandler (BianjieBtn.gameObject, EnumButtonEvent.OnClick, OnClickBianjie, 0, 0);
		HidenMaxMenberSizeUI ();
		if(!TeamSystem.IsInTeam())
		{
            GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId], (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player, AssetLoadSelfCallBack, new ParamData(GamePlayer.Instance.InstId, 0, (int)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId]), "UI", GamePlayer.Instance.DressID);

			uint[] bemps = GamePlayer.Instance.GetEmployeesBattles((int)GamePlayer.Instance.CurEmployeesBattleGroup);
			for(int i =0;i<bemps.Length;i++)
			{
				if(bemps[i] == 0)
				{
					continue;
				}
				//index=i;
				Employee emp = GamePlayer.Instance.GetEmployeeById((int)bemps[i]);
				GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetEmployeeById((int)bemps[i]).GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)0, EntityType.ET_Emplyee, AssetLoadEmployeeCallBack, new ParamData((int)bemps[i], i,GamePlayer.Instance.GetEmployeeById((int)bemps[i]).GetIprop(PropertyType.PT_AssetId)),"UI");
			}
			datingObj.SetActive (true);
			mubiaoObj.SetActive(false);
			teamnameObj.SetActive(false);
			tuichuBtn.gameObject.SetActive(false);
		}else
		{
			ResTeamLeaderBtnState();
			tuichuBtn.gameObject.SetActive(true);
			datingObj.SetActive (false);
			teamCellsState();
			UIManager.SetButtonEventHandler (tuichuBtn.gameObject, EnumButtonEvent.OnClick, OnClickTuichu, 0, 0);
		}
		if(huobanBtn.gameObject.activeSelf)
		{
			UIManager.SetButtonEventHandler (huobanBtn.gameObject, EnumButtonEvent.OnClick, OnClickHuoban, 0, 0);
		}
		SceneLoaded ();
		TeamSystem.OnChangejoinL += ShowUIPanel;
		TeamSystem.OnExitIteam += ExitIteamOk;
		TeamSystem.OnCreateTeam += CreateTeamOk;
		TeamSystem.OnChangeTeam += ChangeTeamOk;
		TeamSystem.OnUpdateTeamMB += UpdateTeamMBOk;
		TeamSystem.OnLeaderChange += changeLeaderOk;
		TeamSystem.OnDelMenber+= DeTeamMunber;
		FriendSystem.Instance().UpdateFriend += new UpdateFriendHandler(OnUpdataFriendList);
		TeamSystem.OnUpdateMemStateUI += UpdatememberState;
		//UpdateMemberPositionUIOk = openOrOffBtn;
		//UpdateUI ();
		//teamInfo_ = TeamSystem._MyTeamInfo;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainTeamUI);
        
        // if guide index
        int idx = 0;
        GlobalValue.Get(Constant.C_TeamGuideStep1, out idx);
        if (!GuideManager.Instance.IsFinish(idx))
        {
            guideAnimator_.SetTrigger("Step1");
            CreateMask();
            GuideManager.Instance.SetFinish(idx);
        }
	}
	void UpdatememberState(int pid,bool islev)
	{
		for(int i =0;i<teamCells.Count;i++)
		{
			TeamMemberCell tcell = teamCells[i].GetComponent<TeamMemberCell>();
			if(tcell.PlayerInst == null)continue;
			if(tcell.PlayerInst.instId_ == (uint)pid)
			{
				if(islev)
				{
					tcell.heiObj.SetActive(true);
					tcell.StateSp.spriteName = "zanli";
					tcell.OpenBtn.gameObject.SetActive(false);
					tcell.offBtn.gameObject.SetActive(false);
				}else
				{
					tcell.heiObj.SetActive(false);
				}

			}
		}
		zanshiBtn.gameObject.SetActive (!islev);
		backTeamBtn.gameObject.SetActive (islev);
	}

	void OnUpdataFriendList(COM_ContactInfo contact,bool isNew)
	{
		
		if(isNew)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("addfriendok"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("addfriendok"));
		}
		else
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("delfriendOk"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("delfriendOk"));
		}

	}
	void SceneLoaded()
	{
		if (TeamSystem.IsInTeam())
		{
			ClearRosObj ();
			rosobj.Clear ();
			ResteamCellsState ();
			ShowTeamMembers();
		}
		
//		if(TeamSystem.isYQ)
//		{
//			ShowUIPanel();
//			TeamSystem.isYQ = false;
//		}
		if(TeamSystem.isTarget)
		{
			ShowUIPanel();
			TeamSystem.isTarget = false;
		}
	}





	void DeTeamMunber(int uid)
	{
		
		COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
//		for (int i = 0; i < members.Length; i++)
//		{
//			if (members[i].instId_ == uid)
//			{
//				for(int j = 0;j<rosobj.Count;j++)
//				{
//					if(int.Parse(rosobj[j].name) == uid)
//					{
//						ChatSystem.PushSystemMessage(members[i].instName_ + LanguageManager.instance.GetValue("likaifangjian"));
//						DestroyGameObj((ENTITY_ID)members[i].properties_[(int)PropertyType.PT_AssetId], true, rosobj[j]);
//						Destroy(rosobj[j]);
//						rosobj.RemoveAt(j);
//						//members[i] = null;
//						break;
//					}
//				}
				
//			}
//		}
		for(int i = 0;i<teamCells.Count;i++)
		{
			TeamMemberCell tcell = teamCells [i].GetComponent<TeamMemberCell> ();
			if(tcell.PlayerInst == null)continue;
			if(tcell.PlayerInst.instId_ == uid)
			{
				tcell.StateSp.spriteName = "dengdaizhong";
				tcell.OpenBtn.gameObject.SetActive (false);
				tcell.offBtn.gameObject.SetActive (true);
				tcell.RestMemberCellInfo();
				UIManager.RemoveButtonAllEventHandler (teamCells[i].gameObject);
				ChatSystem.PushSystemMessage(tcell.PlayerInst.instName_ + LanguageManager.instance.GetValue("likaifangjian"));
				DestroyGameObj((ENTITY_ID)tcell.PlayerInst.properties_[(int)PropertyType.PT_AssetId], true, tcell.modPos.GetChild(0).gameObject);
				Destroy(tcell.modPos.GetChild(0).gameObject);
			}
		}
		StartCoroutine (Do ());
	}
	IEnumerator Do()
	{
		yield return new WaitForSeconds (0.5f);
		UpdateUI ();
	}
	void changeLeaderOk(int uid)
	{
		COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
		COM_SimplePlayerInst temp = teamCells [0].GetComponent<TeamMemberCell> ().PlayerInst;
		if(uid != temp.instId_)
		{
			for (int i = 0; i < members.Length; ++i)
			{
				if ((int)members[i].instId_ == uid)
				{
					//idx = i;
					ChatSystem.PushSystemMessage(members[i].instName_ + LanguageManager.instance.GetValue("duizhang"));
				}
				DeTeamMunber((int)members[i].instId_);

			}
			GlobalInstanceFunction.Instance.Invoke(()=>{
				ShowTeamMembers ();
				UpdateUI ();
			},1);

		}


//		int idx = 0;
//		if(GamePlayer.Instance.InstId == uid)
//		{
//			
//			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shengweiduizhang"),null,true);
//		}
		
//		for (int i = 0; i < members.Length; ++i)
//		{
//			if ((int)members[i].instId_ == uid)
//			{
//				idx = i;
//				
//				ChatSystem.PushSystemMessage(members[i].instName_ + LanguageManager.instance.GetValue("duizhang"));
//				break;
//			}
//		}

		//TeamSystem.UpdateMainTeamInfo ();
	}
	void ChangeTeamOk(COM_TeamInfo team)
	{
		teamInfo_ = team;
		UpdateUI ();
		openOrOffBtn ();
		teamCellsState();
	}
	void UpdateTeamMBOk(COM_SimplePlayerInst info)
	{
		index = TeamSystem.GetTeamMembers ().Length-1;
		ENTITY_ID weaponAssetId = 0;
		if(GlobalInstanceFunction.Instance.WeaponID(info) != 0)
            weaponAssetId = (ENTITY_ID)ItemData.GetData(GlobalInstanceFunction.Instance.WeaponID(info)).weaponEntityId_;
        GameManager.Instance.GetActorClone((ENTITY_ID)info.properties_[(int)PropertyType.PT_AssetId], weaponAssetId, EntityType.ET_Player, AssetLoadCallBack, new ParamData((int)info.instId_, index, (int)info.properties_[(int)PropertyType.PT_AssetId]), "UI", GlobalInstanceFunction.Instance.GetDressId(info.equips_));
		UIManager.SetButtonEventHandler (teamCells[index].gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
	}
	void DestroyGameObj(ENTITY_ID eId,bool unLoadAllLoadedObjects,GameObject obj)
	{
		PlayerAsseMgr.DeleteAsset(eId, unLoadAllLoadedObjects);
		Destroy (obj);
	}
	int index;
	void AssetLoadSelfCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy_)
		{
			Destroy(ro);
			return;
		}
		//TeamMemberCell tcell = findEmptyPos ();
		TeamMemberCell tcell = teamCells[0].GetComponent<TeamMemberCell>();
		tcell.heiObj.SetActive (false);
		tcell.huobanLable.gameObject.SetActive (false);
		tcell.duizhangSp.spriteName = "duizhang";
		//tcell.modPos.transform.localPosition =  new Vector3 (20f,-166f,-100f);
		ro.transform.parent =tcell.modPos;
		ro.name = data.iParam.ToString();
		tcell.gameObject.name = data.iParam.ToString();
		ro.transform.localPosition = Vector3.zero;
		ro.transform.rotation = new Quaternion(ro.transform.rotation.x, -180, ro.transform.rotation.z, ro.transform.rotation.w);
		ro.transform.localScale = new Vector3(EntityAssetsData.GetData(data.iParam3).zoom_ ,EntityAssetsData.GetData(data.iParam3).zoom_,EntityAssetsData.GetData(data.iParam3).zoom_ );
		tcell.NameLabel.text = GamePlayer.Instance.InstName;
		tcell.levelLabel.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Level).ToString();
		tcell.preSp.spriteName = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel)).jobtype_.ToString ();
		tcell.preLabel.text = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
		ro.AddComponent<EffectLevel> ();
		EffectLevel el = ro.GetComponent<EffectLevel> ();
		el.target = tcell.gameObject.GetComponent<UIWidget>();
//		ross.Add (roTitleClone);
		rosobj.Add (ro);
		//UpdateUI ();
	}
	void AssetLoadEmployeeCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy_)
		{
			Destroy(ro);
			return;
		}
		TeamMemberCell tcell = findEmptyPos ();
		tcell.heiObj.SetActive (false);
		tcell.huobanLable.gameObject.SetActive (true);
		//tcell.modPos.transform.localPosition = new Vector3 (20f,-166f,-100f);
		ro.transform.parent =tcell.modPos;
		ro.name = data.iParam.ToString();
		tcell.gameObject.name = data.iParam.ToString();
		ro.transform.localPosition = Vector3.zero;
		ro.transform.rotation = new Quaternion(ro.transform.rotation.x, -180, ro.transform.rotation.z, ro.transform.rotation.w);
		ro.transform.localScale = new Vector3(EntityAssetsData.GetData(data.iParam3).zoom_ ,EntityAssetsData.GetData(data.iParam3).zoom_,EntityAssetsData.GetData(data.iParam3).zoom_ );
		Employee emp = GamePlayer.Instance.GetEmployeeById(data.iParam);
		tcell.NameLabel.text = emp.InstName;
		tcell.levelLabel.text = emp.GetIprop(PropertyType.PT_Level).ToString();
		tcell.preSp.spriteName = Profession.get ((JobType)emp.GetIprop (PropertyType.PT_Profession),emp.GetIprop (PropertyType.PT_ProfessionLevel)).jobtype_.ToString ();
		tcell.preLabel.text = Profession.get ((JobType)emp.GetIprop(PropertyType.PT_Profession), emp.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
		ro.AddComponent<EffectLevel> ();
		EffectLevel el = ro.GetComponent<EffectLevel> ();
		el.target = tcell.gameObject.GetComponent<UIWidget>();
		//		ross.Add (roTitleClone);
		rosobj.Add (ro);
	}
	
	void ShowUIPanel()
	{
		
		if(isCreate)
		{
			CreateTeamUIPanel.ShowMe ();
		}
		else
			if(TeamSystem.isHanHua)
		{
			HuanHuaTeam();
		}
//		else if(TeamSystem.isYQ)
//		{
//			YaoQingTeam();
//		}
		else
		{
			FastTeamPanel.ShowMe ();
		}
	}
	private COM_SimpleTeamInfo hteaminfo;
	void HuanHuaTeam()
	{
		COM_SimpleTeamInfo teamInfo = GetCurTeam(TeamSystem.hTeamid);
		hteaminfo = teamInfo;
		if(hteaminfo==null)return;
		if(!OpenTeam.isSceneHan)
			NetConnection.Instance.joinTeam((uint)teamInfo.teamId_,OpenTeam.password);
	}
	void YaoQingTeam()
	{
		COM_SimpleTeamInfo teamInfo = GetCurTeam(TeamSystem.hTeamid);
		hteaminfo = teamInfo;
		if(!OpenTeam.isSceneHan)
			NetConnection.Instance.isjoinTeam((uint)teamInfo.teamId_,true);
		
	}
	COM_SimpleTeamInfo GetCurTeam(int teamId)
	{
		for(int i =0;i<TeamSystem.LobbyTeams.Count;i++)
		{
			if(teamId == TeamSystem.LobbyTeams[i].teamId_)
			{
				return TeamSystem.LobbyTeams[i];
			}
		}
		return null;
	}
	private bool isLeader_ = false;
	Animator guideAnimator_;
	public static  COM_TeamInfo teamInfo_;
	public void UpdateUI()
	{
		if (!TeamSystem.IsInTeam())
		{
			tuichuBtn.gameObject.SetActive(false);
			return;
		}
		tuichuBtn.gameObject.SetActive(true);
		UIManager.SetButtonEventHandler (tuichuBtn.gameObject, EnumButtonEvent.OnClick, OnClickTuichu, 0, 0);
		// is leader
		isLeader_ = TeamSystem.IsTeamLeader();
        duaiyuanObj.gameObject.SetActive(!isLeader_);
		duaizhangObj.gameObject.SetActive(isLeader_);
		mubiaoObj.SetActive(true);
		teamnameObj.SetActive(true);
		teamnameLabel.text = TeamSystem._MyTeamInfo.name_;
		mubiaoLabel.text = LanguageManager.instance.GetValue( TeamSystem._MyTeamInfo.type_.ToString())+"("+TeamSystem._MyTeamInfo.minLevel_.ToString()+"级"+"-"+TeamSystem._MyTeamInfo.maxLevel_.ToString()+"级"+")" ;
		huobanBtn.gameObject.SetActive (isLeader_);
		if(huobanBtn.gameObject.activeSelf)
		{
			UIManager.SetButtonEventHandler (huobanBtn.gameObject, EnumButtonEvent.OnClick, OnClickHuoban, 0, 0);
		}
		if(duaizhangObj.activeSelf)
		{
			UIManager.SetButtonEventHandler (hanhuaBt.gameObject, EnumButtonEvent.OnClick, OnClickHuanhua, 0, 0);
			UIManager.SetButtonEventHandler (XiugaiBtn.gameObject, EnumButtonEvent.OnClick, OnClickXiugai, 0, 0);
			UIManager.SetButtonEventHandler (YaoqingBtn.gameObject, EnumButtonEvent.OnClick, OnClickYaoqing, 0, 0);
			
		}else if(duaiyuanObj.activeSelf)
		{
			UIManager.SetButtonEventHandler (zanshiBtn.gameObject, EnumButtonEvent.OnClick, OnClickzanshi, 0, 0);
			UIManager.SetButtonEventHandler (backTeamBtn.gameObject, EnumButtonEvent.OnClick, OnClickbackTeam, 0, 0);
		}
		COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
		for (int i = 0; i < members.Length; i++)
		{
			if(isLeader_)
			{
				if (TeamSystem.IsTeamLeader((int)members[i].instId_))
				{
					UpdateMaxMenberS();
				}
			}else
			{
				ShowTeamPosition();
				//HidenMaxMenberSizeUI();
			}
			for (int j = 0; j<teamCells.Count; j++)
			{
				TeamMemberCell tecll = teamCells[j].GetComponent<TeamMemberCell>();
				if(tecll.modPos.transform.childCount == 0)continue;
				if (int.Parse(teamCells[j].name) == members[i].instId_)
				{
					

					if (TeamSystem.IsTeamLeader((int)members[i].instId_))
					{
						tecll.duizhangSp.spriteName = "duizhang";
					}
					else
					{
						tecll.duizhangSp.spriteName = "duiyuan";						
					}
					if(tecll.PlayerInst != null)
					{
						if(tecll.PlayerInst.isLeavingTeam_)
						{
							backTeamBtn.gameObject.SetActive(true);
							zanshiBtn.gameObject.SetActive(false);
							tecll.heiObj.SetActive(true);
							tecll.StateSp.spriteName = "zanli";
							tecll.OpenBtn.gameObject.SetActive(false);
							tecll.offBtn.gameObject.SetActive(false);
						}else
						{
							backTeamBtn.gameObject.SetActive(false);
							zanshiBtn.gameObject.SetActive(true);
							tecll.heiObj.SetActive(false);
						}
					}
				}

			}
		}
		openOrOffBtn ();
	}
	public void HidenMaxMenberSizeUI()
	{
		for(int i=0;i<teamCells.Count;i++)
		{
			TeamMemberCell tpb = teamCells[i].GetComponent<TeamMemberCell>();
			tpb.HidenBtn();
		}
	}
	public void ShowTeamPosition()
	{
		for(int i=0;i<teamCells.Count;i++)
		{
			TeamMemberCell tpb = teamCells[i].GetComponent<TeamMemberCell>();
			tpb.OpenBtn.gameObject.SetActive(false);
			tpb.offBtn.gameObject.SetActive(false);
		}
	}
	public void UpdateMaxMenberS()
	{
		for(int i =0;i<teamCells.Count&&i<rosobj.Count;i++)
		{
			TeamMemberCell tpb = teamCells[i].GetComponent<TeamMemberCell>();
			if(tpb.modPos.transform.childCount>0)
			{

				tpb.HidenBtn();
			}else
			{
				//TeamMemberCell tpb = teamCells[i].GetComponent<TeamMemberCell>();
				if(tpb.isStart)
				{
					tpb.OpenBtn.gameObject.SetActive(true);
					tpb.offBtn.gameObject.SetActive(false);
				}else
				{
					tpb.OpenBtn.gameObject.SetActive(false);
					tpb.offBtn.gameObject.SetActive(true);
				}
				
				
				
			}
		}
	}
	void CreateMask()
	{
		mask_ = GameObject.Instantiate(Resources.Load<GameObject>("Mask")) as GameObject;
		mask_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
		mask_.GetComponent<UIPanel>().depth = gameObject.GetComponent<UIPanel>().depth + 1;
		mask_.GetComponentInChildren<BoxCollider>().size = new Vector3(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight, 0f);
        mask_.transform.localScale = Vector3.one;
	}
	TeamMemberCell findEmptyPos()
	{
		for (int i = 1; i < teamCells.Count; ++i )
		{
			TeamMemberCell tell = teamCells[i].GetComponent<TeamMemberCell>();
			if (tell.modPos.childCount ==0 &&tell.isOpen)
			{
				tell.heiObj.SetActive(false);
				UIManager.SetButtonEventHandler (teamCells[i].gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
				return tell;
			}
		}
		return null;
	}
	void ResteamCellsState()
	{
		for (int i = 0; i < teamCells.Count; ++i )
		{
			TeamMemberCell tell = teamCells[i].GetComponent<TeamMemberCell>();
			tell.RestMemberCellInfo();
			tell.heiObj.SetActive(true);
			tell.huobanLable.gameObject.SetActive(false);
		}
	}
    public void CanDealGuideInput()
    {
        MaskClick.OnClickMask += DealGuideInput;
    }

    void DealGuideInput()
    {
        if(guideObj_ != null)
            Destroy(guideObj_);
        if(mask_ != null)
            Destroy(mask_);
        MaskClick.OnClickMask -= DealGuideInput;
    }
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickCreate(ButtonScript obj, object args, int param1, int param2)
	{
		isCreate = true;
		TeamSystem.isHanHua = false;
		//TeamSystem.isYQ = false;
		NetConnection.Instance.jointLobby ();
	}


	void OnClickBianjie(ButtonScript obj, object args, int param1, int param2)
	{
		isCreate = false;
		TeamSystem._teamType = TeamType.TT_None;
		TeamSystem.isHanHua = false;
		//TeamSystem.isYQ = false;
		NetConnection.Instance.jointLobby ();
	}
	void OnClickShowTips(ButtonScript obj, object args, int param1, int param2)
	{
		TeamMemberCell tcell = obj.GetComponent<TeamMemberCell> ();
		if(tcell.PlayerInst == null)return;
//		for(int i =0;i<TeamSystem.GetTeamMembers().Length;i++)
//		{
		if(isLeader_)
		{
			if(TeamSystem.IsTeamLeader((int)tcell.PlayerInst.instId_))
			{
				duaizhangTips.SetActive(false);
				duaiyuanTips.SetActive(false);
			}else
			{
				duaizhangTips.SetActive(true);
				TeamTips tp = duaizhangTips.GetComponent<TeamTips>();
				tp.uId = (int)tcell.PlayerInst.instId_;
				duaizhangTips.transform.localPosition = new Vector3(obj.transform.localPosition.x,obj.transform.localPosition.y,duaizhangTips.transform.localPosition.z) ;

				duaiyuanTips.SetActive(false);
			}
		}else
		{
			if(GamePlayer.Instance.InstId == (int)tcell.PlayerInst.instId_)
			{
				duaizhangTips.SetActive(false);
				duaiyuanTips.SetActive(false);
			}else
			{
				duaizhangTips.SetActive(false);
				duaiyuanTips.SetActive(true);
				duaiyuanTips.transform.localPosition =new Vector3(obj.transform.localPosition.x,obj.transform.localPosition.y,duaiyuanTips.transform.localPosition.z) ;
				UIManager.SetButtonEventHandler (tipsCloseBtn.gameObject, EnumButtonEvent.OnClick, OnClicktipsClose, 0, 0);
				UIManager.SetButtonEventHandler (HyBtn.gameObject, EnumButtonEvent.OnClick, OnClickHy, (int)tcell.PlayerInst.instId_, 0);
				UIManager.SetButtonEventHandler (playerInfoBtn.gameObject, EnumButtonEvent.OnClick, OnClickplayerInfo,(int)tcell.PlayerInst.instId_, 0);

			}
		}


//		}
	}
	void OnClicktipsClose(ButtonScript obj, object args, int param1, int param2)
	{
		duaiyuanTips.SetActive (false);
	}void OnClickHy(ButtonScript obj, object args, int param1, int param2)
	{
		COM_SimplePlayerInst _player = TeamSystem.GetTeamMemberByInsId (param1);
		if(FriendSystem.Instance().IsmyFriend((int)_player.instId_))
		{
			LanguageManager.instance.GetValue("alreadyhave");
		}else
		{
			int fMax = 0;
			GlobalValue.Get(Constant.C_FriendMax, out fMax);
			if(FriendSystem.Instance().friends_.Count >= fMax)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
				return;
			}
			NetConnection.Instance.addFriend (_player.instId_);
		}

	}
	void OnClickplayerInfo(ButtonScript obj, object args, int param1, int param2)
	{

		COM_SimplePlayerInst _player = TeamSystem.GetTeamMemberByInsId (param1);

		//chenkPlayerObj.SetActive (true);
//		TeamPlayerInfo tpinfo = chenkPlayerObj.GetComponent<TeamPlayerInfo>();
//		tpinfo.SPlayerInfo = _player;
		TeamPlayerInfo.ShowMe (_player);
	}

	void OnClickTuichu(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();

				});
			}else
			{
				NetConnection.Instance.exitTeam ();
				NetConnection.Instance.exitLobby ();
			}
			
		}else
		{
			NetConnection.Instance.exitTeam ();
			NetConnection.Instance.exitLobby ();
		}


	}
//	private int endTime;
//	private float currentTime;
//	private int startTime =30;
//	private int second;
	public UILabel timelabel;
//	private bool isCountDown = false;
	void OnClickHuanhua(ButtonScript obj, object args, int param1, int param2)
	{
		timelabel.gameObject.SetActive(true);
//		startTime = 30;
//		currentTime = 0;


		GlobalInstanceFunction.isOnkeyCountDown = true;
		COM_Chat chat = new COM_Chat ();
		chat.ck_ = ChatKind.CK_World;
		chat.teamId_ = (int)TeamSystem._MyTeamInfo.teamId_;
		chat.teamMaxLevel_ = (short)TeamSystem._MyTeamInfo.maxLevel_;
		chat.teamMinLevel_ = (short)TeamSystem._MyTeamInfo.minLevel_;
		chat.teamType_ = TeamSystem._MyTeamInfo.type_;
		chat.audio_ = null;
		chat.audioTime_ = 0;
		chat.content_ = " ";
		//string content_ = LanguageManager.instance.GetValue ("yijianhanhua").Replace ("{n}",LanguageManager.instance.GetValue( TeamSystem._MyTeamInfo.type_.ToString())).Replace ("{n1}", TeamSystem._MyTeamInfo.minLevel_.ToString()).Replace ("{n2}", TeamSystem._MyTeamInfo.maxLevel_.ToString()).Replace("{t1}","1").Replace("{t2}",TeamSystem._MyTeamInfo.teamId_.ToString());
		//ChatSystem.SendToServer(ChatKind.CK_World,"",content_);
		NetConnection.Instance.sendChat(chat,"");       
		hanhuaBt.isEnabled = false;
	}
	void OnClickXiugai(ButtonScript obj, object args, int param1, int param2)
	{
		ChangeTeamTargetUI.ShowMe ();
	}
	void OnClickYaoqing(ButtonScript obj, object args, int param1, int param2)
	{
		YaoQingUI.ShowMe();
	}
	void OnClickzanshi(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
				NetConnection.Instance.exitCopy();
			});
		}else
		{
			NetConnection.Instance.leaveTeam ();
			backTeamBtn.gameObject.SetActive (true);
			zanshiBtn.gameObject.SetActive (false);
		}


	}
	void OnClickbackTeam(ButtonScript obj, object args, int param1, int param2)
	{
		int sceneid =  TeamSystem.GetMyTeamLeader ().sceneId_;
		if(CopyData.IsCopyScene(sceneid))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("fubenguidui"));
			return;
		}
		NetConnection.Instance.backTeam ();
//		zanshiBtn.gameObject.SetActive (true);
//		backTeamBtn.gameObject.SetActive (false);

	}
	void OnClickHuoban(ButtonScript obj, object args, int param1, int param2)
	{
		EmployessControlUI.SwithShowMe (2);
	}
	public static void ShowMe()
	{
		if(!GamePlayer.Instance.isInitEmployees)
		{
			TeamSystem.openInitUI = true;
            TeamSystem.UIShowType = 1;
			NetConnection.Instance.requestEmployees();
            NetWaitUI.ShowMe();
		}
		else
		{
			UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_TeamPanel, false);
		}
	}
	public static void SwithShowMe()
	{
        if (!GamePlayer.Instance.isInitEmployees)
        {
            TeamSystem.openInitUI = true;
            TeamSystem.UIShowType = 2;
            NetConnection.Instance.requestEmployees();
            NetWaitUI.ShowMe();
        }
        else
        {
            UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_TeamPanel);
        }
	}
	public static void HideMe()
	{
		//ross.Clear();
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_TeamPanel);
	}
	void CreateTeamOk(COM_TeamInfo info)
	{
		ClearRosObj ();
		rosobj.Clear ();
		teamInfo_ = info;
		ResteamCellsState ();
		datingObj.SetActive (false);
		ResTeamLeaderBtnState();
		ShowTeamMembers ();
		TeamSystem.UpdtaeMainTeamlIST ();
	}
	void ShowTeamMembers()
	{
		if (TeamSystem.IsInTeam())
		{

			COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
			for (int i = 0; i < members.Length; ++i)
			{
                ENTITY_ID weaponAssetId = 0;
                if (GlobalInstanceFunction.Instance.WeaponID(members[i]) != 0)
                    weaponAssetId = (ENTITY_ID)ItemData.GetData(GlobalInstanceFunction.Instance.WeaponID(members[i])).weaponEntityId_;
                GameManager.Instance.GetActorClone((ENTITY_ID)members[i].properties_[(int)PropertyType.PT_AssetId], weaponAssetId, EntityType.ET_Player, AssetLoadCallBack, new ParamData((int)members[i].instId_, i, (int)members[i].properties_[(int)PropertyType.PT_AssetId]), "UI", GlobalInstanceFunction.Instance.GetDressId(members[i].equips_));
			}
		}
	}

	void RestMemberCellUI()
	{
		for(int i =0;i<teamCells.Count;i++)
		{
			TeamMemberCell tmcell = teamCells[i].GetComponent<TeamMemberCell>();
			tmcell.RestMemberCellInfo();
			UIManager.SetButtonEventHandler (teamCells[i].gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
		}
		//openOrOffBtn ();
	}
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy_)
		{
			Destroy(ro);
			return;
		}
		//RestMemberCellUI ();
		COM_SimplePlayerInst csi = TeamSystem.GetTeamMemberByIndex(data.iParam2);
        if (csi == null)
        {
            Destroy(ro);
            return; /// 这里可能有问题
        }
		ro.name = csi.instId_.ToString();
		TeamMemberCell tell;
		if(int.Parse(ro.name) == TeamSystem.GetTeamMembers()[0].instId_)
		{
			tell = teamCells[0].GetComponent<TeamMemberCell>();
			UIManager.SetButtonEventHandler (teamCells[0].gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
		}else
		{
			tell = findEmptyPos ();
		}
		ro.transform.parent = tell.modPos;

		//tell.modPos.transform.localPosition = new Vector3 (20f,-166f,-100f);
		tell.gameObject.name = csi.instId_.ToString();
		ro.transform.localPosition = Vector3.zero;
		ro.transform.rotation = new Quaternion(ro.transform.rotation.x, -180, ro.transform.rotation.z, ro.transform.rotation.w);
		ro.transform.localScale = new Vector3(EntityAssetsData.GetData(data.iParam3).zoom_ ,EntityAssetsData.GetData(data.iParam3).zoom_,EntityAssetsData.GetData(data.iParam3).zoom_ );
		tell.PlayerInst = csi;
		ro.AddComponent<EffectLevel> ();
		EffectLevel el = ro.GetComponent<EffectLevel> ();
		el.target = tell.gameObject.GetComponent<UIWidget>();
		rosobj.Add (ro);
		UpdateUI ();
	}
	public void openOrOffBtn()
	{

		for (int i = 0; i < teamCells.Count; ++i)
		{
			TeamMemberCell tc = teamCells[i].GetComponent<TeamMemberCell>();
			if (tc.modPos.childCount != 0)
			{
				tc.offBtn.isEnabled = false;
				tc.OpenBtn.isEnabled = false;
			}
			else
			{
				if (teamInfo_ != null)
				{
					if (teamInfo_.maxMemberSize_ - 1 == i)
					{
						tc.offBtn.isEnabled = true;
						tc.OpenBtn.isEnabled = false;
					}
					else if (teamInfo_.maxMemberSize_ == i)
					{
						tc.offBtn.isEnabled = false;
						tc.OpenBtn.isEnabled = true;
					}
					else if (teamInfo_.maxMemberSize_ - 1 < i)
					{
						tc.offBtn.isEnabled = false;
						tc.OpenBtn.isEnabled = false;
					}
					else
					{
						tc.offBtn.isEnabled = false;
						tc.OpenBtn.isEnabled = false;
					}
				}
			}
		}
	}
	void ResTeamLeaderBtnState()
	{
		for(int i = 0;i<teamCells.Count-1;i++)
		{
			TeamMemberCell tc = teamCells[i].GetComponent<TeamMemberCell>();
			tc.offBtn.isEnabled = false;
		}
	}
	void teamCellsState()
	{

		for(int i = 0;i<teamCells.Count;i++)
		{
			TeamMemberCell tc = teamCells[i].GetComponent<TeamMemberCell>();

			if(i<TeamSystem._MyTeamInfo.maxMemberSize_)
			{
				if(tc.PlayerInst != null&&tc.PlayerInst.isLeavingTeam_&&tc.modPos.childCount!=0)
				{
					tc.StateSp.spriteName = "zanli";
				}else
				{
					tc.StateSp.spriteName = "dengdaizhong";
				}

				if(TeamSystem.IsTeamLeader())
				{
					if(tc.PlayerInst == null)
					{
						tc.OpenBtn.gameObject.SetActive(false);
						tc.offBtn.gameObject.SetActive(true);
					}

				}

			}else
			{
				if(tc.PlayerInst != null&&tc.PlayerInst.isLeavingTeam_&&tc.modPos.childCount!=0)
				{
					tc.StateSp.spriteName = "zanli";
				}else
				{
					tc.StateSp.spriteName = "yiguanbi";
				}

				if(TeamSystem.IsTeamLeader())
				{
					tc.OpenBtn.gameObject.SetActive(true);
					tc.offBtn.gameObject.SetActive(false);
				}

			}
		}
		if(TeamSystem.IsTeamLeader())
		{
			openOrOffBtn ();
		}

	}
	public void ClearRosObj()
	{
		for(int i=0;i<rosobj.Count;i++)
		{

			DestroyImmediate(rosobj[i].gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		if(GlobalInstanceFunction.isOnkeyCountDown)
		{
			hanhuaBt.isEnabled = false;
			timelabel.gameObject.SetActive(true);
			timelabel.text = GlobalInstanceFunction.OnkeyendTime.ToString();

		}else
		{

			hanhuaBt.isEnabled = true;
			timelabel.gameObject.SetActive(false);
		}
	}
	public override void Destroyobj ()
	{
		hasDestroy_ = true;
		TeamSystem.OnChangejoinL -= ShowUIPanel;
		TeamSystem.OnExitIteam -= ExitIteamOk;
		TeamSystem.OnCreateTeam -= CreateTeamOk;
		TeamSystem.OnChangeTeam -= ChangeTeamOk;
		TeamSystem.OnUpdateTeamMB -= UpdateTeamMBOk;
		TeamSystem.OnLeaderChange -= changeLeaderOk;
		TeamSystem.OnDelMenber -= DeTeamMunber;
		FriendSystem.Instance().UpdateFriend -= OnUpdataFriendList;
		TeamSystem.OnUpdateMemStateUI -= UpdatememberState;
		//UpdateMemberPositionUIOk = null;

        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TeamPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	void ExitIteamOk()
	{
		TeamSystem.maxMembers = 5;
		Hide ();
	}
//	public void Countdown()
//	{
//		
//		currentTime += Time.fixedDeltaTime;
//		endTime = startTime - Mathf.CeilToInt(currentTime);
//		int shiwei = endTime / 10;
//		int gewei = endTime % 10;
//		
//		
//		timelabel.text = shiwei + "" + gewei;
//		
//		
//		if( endTime <= 0 )
//		{
//			hanhuaBt.isEnabled = true;
//			timelabel.gameObject.SetActive(false);
//			isCountDown = false;
//		}
//		
//	}
}
