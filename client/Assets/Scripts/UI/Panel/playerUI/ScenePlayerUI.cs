using UnityEngine;
using System.Collections;

public class ScenePlayerUI : MonoBehaviour {

	public UITexture iocn;
	public UILabel levelLabel;
	public UILabel zhandouLabel;
	public UILabel hpLabel;
	public UILabel mpLabel;

	public UILabel jiazuLabel;
	public UILabel zhiyeLabel;
	public UIButton CKBtn;
	public UIButton JHYBtn;
	public UIButton jdwBtn;
	public UIButton closeBtn;
	public UIButton pvpBtn;


	//public GameObject tipsObj;
	public GameObject hudObj;
	//public GameObject playerInfoObj;

	public UISlider hpSlider;
	public UISlider mpSlider;


    private COM_ScenePlayerInformation playerInst_;
    public COM_ScenePlayerInformation PlayerInst
	{
		set
		{
			if(value != null)
			{
				playerInst_ = value;
				levelLabel.text = PlayerInst.level_.ToString ();
				
				int crtHp = (int)PlayerInst.hpCrt_;
				int crtMp = (int)PlayerInst.mpCrt_;
				zhiyeLabel.text = "职业"+ Profession.get (playerInst_.jt_,(int)playerInst_.jl_).jobName_;

				jiazuLabel.text = "家族"+ playerInst_.guildeName_;
				

				zhandouLabel.text = "战斗力"+ PlayerInst.battlePower_.ToString();
				int asdId = 0;
				if(PlayerInst.fashionId_ == 0)
				{
					asdId = PlayerInst.assetId_;
				}else
				{
					ItemData fasion = ItemData.GetData(PlayerInst.fashionId_);
					if(fasion != null)
						asdId = fasion.weaponEntityId_;
					else
						asdId = PlayerInst.assetId_;
				}
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)asdId).assetsIocn_, iocn);
                gameObject.SetActive(true);
			}
		}
		get
		{
			return playerInst_;
		}
	}

	SceneData ssd;
	float maxLevel;
	void Start () {
		//UIManager.SetButtonEventHandler (hudObj.gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler (CKBtn.gameObject, EnumButtonEvent.OnClick, OnClickCK, 0, 0);
		UIManager.SetButtonEventHandler (pvpBtn.gameObject, EnumButtonEvent.OnClick, OnClickpvp, 0, 0);
		UIManager.SetButtonEventHandler (JHYBtn.gameObject, EnumButtonEvent.OnClick, OnClickJHY, 0, 0);
		 ssd = SceneData.GetData (GameManager.SceneID);
		UIManager.SetButtonEventHandler (jdwBtn.gameObject, EnumButtonEvent.OnClick, OnClickJDW, 0, 0);
		if(ssd.sceneType_ == SceneType.SCT_TeamPK||ssd.sceneType_ == SceneType.SCT_AlonePK||ssd.sceneType_ == SceneType.SCT_Instance || ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			pvpBtn.isEnabled = true;
			UIManager.SetButtonEventHandler (pvpBtn.gameObject, EnumButtonEvent.OnClick, OnClickpvp, 0, 0);
		}else
		{
			pvpBtn.isEnabled = false;
		}
		GlobalValue.Get(Constant.C_PlayerMaxLevel, out maxLevel);
//		TeamSystem.OnChangejoinL += joinLobbyOk;
//		TeamSystem.OnInitMyTeam += InitMyTeamOk;
	}
	

	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		//tipsObj.SetActive (false);
		gameObject.SetActive (false);

	}
	void OnClickShowTips(ButtonScript obj, object args, int param1, int param2)
	{
		//tipsObj.SetActive (true);
		UIManager.SetButtonEventHandler (CKBtn.gameObject, EnumButtonEvent.OnClick, OnClickCK, 0, 0);
		UIManager.SetButtonEventHandler (pvpBtn.gameObject, EnumButtonEvent.OnClick, OnClickpvp, 0, 0);
		UIManager.SetButtonEventHandler (JHYBtn.gameObject, EnumButtonEvent.OnClick, OnClickJHY, 0, 0);

	}
	void OnClickpvp(ButtonScript obj, object args, int param1, int param2)
	{
		if(Prebattle.Instance.FindPlayer((int)PlayerInst.instId_)==null)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duifangbuzaichangjing"));
			return;
		}
		if(Prebattle.Instance.FindPlayer((int)PlayerInst.instId_)!=null&& Prebattle.Instance.FindPlayer((int)PlayerInst.instId_).playerData_.isInBattle_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("zhengzaizhandou"));
			return;
		}
		if(GuildSystem.IsMyGuildMember((int)PlayerInst.instId_)&&ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunenggongjibenjiazu"));
			return;
		}
		if(/*ssd.sceneType_ == SceneType.SCT_TeamPK||ssd.sceneType_ == SceneType.SCT_AlonePK &&*/TeamSystem.isTeamMember ((int)PlayerInst.instId_))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunenggongji"));
			return;
		}
		if(GuildSystem.battleState==1&&ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jiazuzhanopen"));
			return;
		}
		if(GuildSystem.battleState == 3)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jiazuzhanojieshu"));
			return;
		}
		NetConnection.Instance.requestPk ((uint)PlayerInst.instId_);
	}
	void OnClickCK(ButtonScript obj, object args, int param1, int param2)
	{
		//TeamPlayerInfo.SwithShowMe (PlayerInst);
        // 发送协议请求详细数据
        // 详细数据返回后 调用TeamPlayerInfo.SwithShowMe (PlayerInst);显示界面
        NetConnection.Instance.querySimplePlayerInst((uint)PlayerInst.instId_);
	}
	void OnClickJHY(ButtonScript obj, object args, int param1, int param2)
	{



		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Friend))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("haoyouWeikai"));
		}else
			if(!(((PlayerInst.openSubSystemFlag_) &(0x1 << (int)OpenSubSystemFlag.OSSF_Friend)) != 0))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("Duifanghaoyou"));
		}
		else
		{
			if(FriendSystem.Instance().IsmyFriend((int)PlayerInst.instId_))
			{
			 PopText.Instance.Show(LanguageManager.instance.GetValue("alreadyhave"));
			}else
			{
				int fMax = 0;
				GlobalValue.Get(Constant.C_FriendMax, out fMax);
				if(FriendSystem.Instance().friends_.Count >= fMax)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
					return;
				}
				NetConnection.Instance.addFriend ((uint)PlayerInst.instId_);
			}

			gameObject.SetActive (false);
		}

	}
	void OnClickJDW(ButtonScript obj, object args, int param1, int param2)
	{

		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
			return;
		}
		//if(!(((PlayerInst.openSubSystemFlag_) &(0x1 << (int)OpenSubSystemFlag.OSSF_Team)) != 0))
		//{
		//	PopText.Instance.Show(LanguageManager.instance.GetValue("Duifangduiwu"));
		//	return;
		//}
        SceneData ssd = SceneData.GetData (GameManager.SceneID);
        //if(ssd.sceneType_ == SceneType.SCT_AlonePK||ssd.sceneType_ == SceneType.SCT_TeamPK)
        //{
        //    PopText.Instance.Show(LanguageManager.instance.GetValue("bunengzudui"));
        //    return;
        //}
		if(ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			if(!GuildSystem.IsInMyGuild(PlayerInst.instId_))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengyaoqingdifang"));
				return;
			}

		}
		if(TeamSystem.IsInTeam())
		{
            if (!TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
                PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeaderCanOperate"));
            else
            {
                NetConnection.Instance.inviteTeamMember(PlayerInst.instName_);
                //PopText.Instance.Show(LanguageManager.instance.GetValue("yaoqingchenggong"));
                gameObject.SetActive(false);
            }
		}else
		{
			//NetConnection.Instance.jointLobby();
			
			COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
			cti.type_ = TeamType.TT_MainQuest;
            cti.name_ = LanguageManager.instance.GetValue("autoTeam");
			//cti.pwd_ = passWordInput.text;
			cti.maxMemberSize_ = 5;
			cti.minLevel_ = 1;
			cti.maxLevel_ = (ushort)maxLevel;
			NetConnection.Instance.createTeam(cti);
           // NetConnection.Instance.inviteTeamMember(PlayerInst.instName_);
			PopText.Instance.Show(LanguageManager.instance.GetValue("yaoqingchenggong"));
		}
	}

	void joinLobbyOk()
	{
		
//		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
//		cti.type_ = TeamType.TT_None;
//		cti.name_ = "";
//		//cti.pwd_ = passWordInput.text;
//		cti.maxMemberSize_ = 5;
//		cti.minLevel_ = 1;
//		cti.maxLevel_ = 60;
//		NetConnection.Instance.createTeam(cti);
//		NetConnection.Instance.exitLobby();
	}
	void InitMyTeamOk()
	{
		NetConnection.Instance.inviteTeamMember (PlayerInst.instName_);
		gameObject.SetActive (false);
	}

	void OnEnable()
	{
		TeamSystem.OnChangejoinL += joinLobbyOk;
		TeamSystem.OnInitMyTeam += InitMyTeamOk;
	}
	void OnDisable()
	{
		TeamSystem.OnChangejoinL -= joinLobbyOk;
		TeamSystem.OnInitMyTeam -= InitMyTeamOk;
	}
//	public static void ShowMe(COM_SimplePlayerInst player)
//	{
//        if (player == null)
//            return;
//
//        PlayerInst = player;
//        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ScenePlayerPanel, false);
//	}
//	public static void SwithShowMe()
//	{
//        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_ScenePlayerPanel);
//	}
//	public static void HideMe()
//	{
//		//ross.Clear();
//        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_ScenePlayerPanel);
//	}
//	public override void Destroyobj ()
//	{
//        PlayerInst = null;
//	}
}
