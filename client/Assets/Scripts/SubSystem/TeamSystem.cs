using UnityEngine;
using System.Collections;
using System.Collections.Generic;
static public class TeamSystem {

	public delegate void backCityHandler();
	public static event backCityHandler OnbackCity;

	public delegate void LeaderChangeHandler(int instId);
	public static event LeaderChangeHandler OnLeaderChange;

	public delegate void CreateTeamHandler(COM_TeamInfo info);
	public static event CreateTeamHandler OnCreateTeam;

	public delegate void InitMyTeamHandler();
	public static event InitMyTeamHandler OnInitMyTeam;

	public delegate void ChangeMyTeamHandler(COM_TeamInfo info);
	public static event ChangeMyTeamHandler OnChangeTeam;

	public delegate void UpdateMyLobTeamHandler();
	public static event UpdateMyLobTeamHandler OnUpdateMyTeamInfo;

	public delegate void joinHandler();
	public static event joinHandler OnChangejoinL;

	public static int maxMembers = 5;
	public delegate void DelMenberHandler(int instId);
	public static event DelMenberHandler OnDelMenber;

	public delegate void UpdateMainteamList();
	public static event UpdateMainteamList OnUpdateMainteamList;

	public delegate void updateTeamDirtyProp(COM_SimplePlayerInst [] PlayerInsts);
	public static event updateTeamDirtyProp OnTeamDirtyProps;

	public delegate void UpdateTeamMB(COM_SimplePlayerInst info);
	public static event UpdateTeamMB OnUpdateTeamMB;
	public delegate void UpdateMemStateUI(int pid,bool isleave);
	public static event UpdateMemStateUI OnUpdateMemStateUI;
	public delegate void BackCityUI();
	public static event BackCityUI OnBackCityUI;

	public delegate void ExitTeamHandler();
	public static event ExitTeamHandler OnExitIteam;
	static public bool isTuiteam;
    static private List<COM_SimpleTeamInfo> _LobbyTeams = new List<COM_SimpleTeamInfo>();
	static public bool isHanHua;
	static public bool isYQ;
	static public int hTeamid;
	static public bool isYqEnd;
	static private string myname;
	//static public bool isIncity;

    public static bool teamIsDirty_;
	public static int minLevel=1;
	public static int maxLevel=60;
	public static List<COM_PlayerInst> PlayerInsts = new List<COM_PlayerInst> ();
	public static TeamType _teamType;
	public static bool isTarget;
	public static bool isBattleScene;
    public static bool wantToEnterTeamScene_;
	public static bool[] uiFlag_ = {true,true,true,true,true};
	public static bool openInitUI;
    public static int UIShowType;
	public static List<COM_SimpleTeamInfo> LobbyTeams
	{
		get
		{
			return _LobbyTeams;
		}
	}
	static public bool AwayTeam(int insId)
	{
		if (!IsInTeam ())
			return false;
		for(int i =0;i<_MyTeamInfo.members_.Length;i++ )
		{
			if(_MyTeamInfo.members_[i].instId_ == (uint)insId&&_MyTeamInfo.members_[i].isLeavingTeam_)
			{
				return true;
			}
		}
		return false;
	}
    static public int RealTeamCount()
    {
        int count = 0;
        if (!IsInTeam())
            return count;
        for (int i = 0; i < _MyTeamInfo.members_.Length; i++)
        {
            if (!_MyTeamInfo.members_[i].isLeavingTeam_)
            {
                count++;
            }
        }
        return count;
    }
	static public void YaoQingMesage()
	{
		MessageBoxUI.ShowMe (myname+LanguageManager.instance.GetValue("yaoqingduiyuan"), () => {
			
			COM_SimpleTeamInfo teamInfo = GetCurTeam(TeamSystem.hTeamid);
			int level = GamePlayer.Instance.GetIprop(PropertyType.PT_Level);
			if(level<teamInfo.minLevel_||level>teamInfo.maxLevel_)
			{
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dengjitishi"));
				PopText.Instance.Show(LanguageManager.instance.GetValue("dengjitishi"));
			}else 
			{
				NetConnection.Instance.isjoinTeam((uint)teamInfo.teamId_,true);
				//NetConnection.Instance.joinTeam((uint)teamInfo.teamId_,teamInfo.pwd_);
			}
			isYqEnd = false;
		},false,()=>{
			isYqEnd = false;
		});
	}
    ///初始化大厅列表
    static public void InitLobbyTeams(COM_SimpleTeamInfo[] infos)
	{
		_LobbyTeams.Clear ();
        _LobbyTeams.AddRange(infos);
		if (OnChangejoinL != null)
			OnChangejoinL ();
		
		if(isYqEnd)
		{
			MessageBoxUI.ShowMe (myname+LanguageManager.instance.GetValue("yaoqingduiyuan"), () => {
				
				COM_SimpleTeamInfo teamInfo = GetCurTeam(TeamSystem.hTeamid);
				int level = GamePlayer.Instance.GetIprop(PropertyType.PT_Level);
				if(level<teamInfo.minLevel_||level>teamInfo.maxLevel_)
				{
					//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dengjitishi"));
					PopText.Instance.Show(LanguageManager.instance.GetValue("dengjitishi"));
				}else 
				{
					NetConnection.Instance.isjoinTeam((uint)teamInfo.teamId_,true);
					//NetConnection.Instance.joinTeam((uint)teamInfo.teamId_,teamInfo.pwd_);
				}
				isYqEnd = false;
			},false,()=>{
				isYqEnd = false;
			});

		}else
		{

			if(OpenTeam.TeamNumbersOk != null)
			{
				OpenTeam.TeamNumbersOk();
			}
			if(OnUpdateMyTeamInfo !=null)
			{
				OnUpdateMyTeamInfo();
			}
		}
		isYqEnd = false;
	}

	
    //更新大厅内的队伍
    static public void SyncUpdateLobbyTeam(COM_SimpleTeamInfo tInfo)
	{
        for (int i = 0; i < _LobbyTeams.Count; ++i)
        {
            if (_LobbyTeams[i].teamId_ == tInfo.teamId_)
            {
                _LobbyTeams[i] = tInfo;
                break;
            }
        }
		if(OnUpdateMyTeamInfo !=null)
		{
			OnUpdateMyTeamInfo();
		}
	}
	//添加大厅队伍
    static public void SyncAddLobbyTeam(COM_SimpleTeamInfo teamInfo)
	{
        _LobbyTeams.Add(teamInfo);
		if(FastTeamPanel.gameUpdateLobby != null)
		{
			FastTeamPanel.gameUpdateLobby (teamInfo);
		}
	}
    ///删除大厅队伍
    static public void SyncDelLobbyTeam(int uid)
    {
        for (int i = 0; i < _LobbyTeams.Count; ++i)
        {
            if (_LobbyTeams[i].teamId_ == uid)
            {
                _LobbyTeams.RemoveAt(i);
                break;
            }
        }
		if(OnUpdateMyTeamInfo !=null)
		{
			OnUpdateMyTeamInfo();
		}
	}

    static public void CleanLobbyTeams()
    {
        _LobbyTeams.Clear();
		//Prebattle.Instance.ExitSceneOk ();

    }

    ///个人队伍相关信息
    static public COM_TeamInfo _MyTeamInfo;
	static private List<Pair<int, bool>> _Readys = new List<Pair<int, bool>> ();

    static public bool IsInTeam()
    {
        return _MyTeamInfo != null && _MyTeamInfo.teamId_ != 0;
    }

    static public int MemberCount
    {
        get
        {
            if(!IsInTeam()) 
                return 0;
            return _MyTeamInfo.members_.Length;
        }
    }

    static public void InitMyTeam(COM_TeamInfo info)
    {
        _MyTeamInfo = info;
		TeamUI.teamInfo_ = info;
		//CreateTeamUIPanel.HideMe ();
		//TeamUI.SwithShowMe ();
		if(OnCreateTeam != null)
		{
			OnCreateTeam(info);
		}
		if(OnInitMyTeam != null)
		{
			OnInitMyTeam();
		}
		CreateTeamUIPanel.HideMe ();
		if(OnUpdateMainteamList !=null)
		{
			OnUpdateMainteamList();
		}
        GamePlayer.Instance.ShowBabyDirty = true;
        teamIsDirty_ = true;
    }

    static public void UpdateMyTeam(COM_TeamInfo info)
    {
        _MyTeamInfo = info;
	
		if(OnChangeTeam !=null)
		{
			OnChangeTeam(info);
		}
    }

	static public void BackCity()
	{
		if(OnbackCity != null)
			OnbackCity();
	}

    static public COM_SimplePlayerInst GetMyTeamLeader()
    {
        return GetTeamMemberByIndex(0);
    }

    static public COM_SimplePlayerInst GetTeamMemberByIndex(int idx)
    {
        if (!IsInTeam())
            return null;
        if (idx < 0 || idx >= _MyTeamInfo.members_.Length)
            return null;
        return _MyTeamInfo.members_[idx];
    }
	static public COM_SimplePlayerInst GetTeamMemberByInsId(int idx)
	{
		if (!IsInTeam())
			return null;
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(_MyTeamInfo.members_[i].instId_ == (uint)idx)
			{
				return _MyTeamInfo.members_[i];
			}
		}
		return null;
	}

    static public bool isTeamMember(int instId)
    {
        if (!IsInTeam())
            return false;
        for (int i = 0; i < _MyTeamInfo.members_.Length; ++i)
        {
            if (_MyTeamInfo.members_[i].instId_ == (uint)instId)
                return true;
        }
        return false;
    }
	
	static public bool IsTeamLeader(int pid)
    {
        if (!IsInTeam())
            return false;
        return GetMyTeamLeader().instId_ == pid;
    }
	public static bool IsSelf(int pid)
	{
		if (!IsInTeam())
			return false;
		for(int i =0;i<GetTeamMembers().Length;i++)
		{
			if(GetTeamMembers()[i].instId_ == pid)
			{
				return true;
			}
		}
		return false;
	}
    static public bool IsTeamLeader()
    {
        return IsTeamLeader(GamePlayer.Instance.InstId);
    }


	static public bool IsTeamOpenPvp()
	{
        if (_MyTeamInfo == null)
            return false;
		if (_MyTeamInfo == null || _MyTeamInfo.members_ == null)
			return false;
		for(int i=0; i < _MyTeamInfo.members_.Length; ++i)
		{
            if (_MyTeamInfo.members_[i].properties_[(int)PropertyType.PT_Level] < 20)
			{
				return false;
			}
		}

		return true;
	}


	static public COM_SimplePlayerInst[] GetTeamMembers()
    {
		if (_MyTeamInfo == null)
			return null;
        return _MyTeamInfo.members_;
    }

	static public void SyncTeamMemberProp(int id,COM_PropValue[] props){

        if (props == null)
            return;

		if (props.Length == 0)
            return;

        if (_MyTeamInfo == null)
            return;

        if (_MyTeamInfo.members_ == null)
            return;

		for (int i = 0; i < _MyTeamInfo.members_.Length; ++i)
		{
			if (_MyTeamInfo.members_[i].instId_ == id)
			{
				for(int j=0; j<props.Length;++j){
					_MyTeamInfo.members_[i].properties_[(int)props[j].type_] = props[j].value_;
				}
				return;
			}

		}
	}

    static public void ChageMyTeamLeader(int playerId)
    {
//        if (_MyTeamInfo.members_[0].instId_ == playerId)
//            return;
		List<COM_SimplePlayerInst> infos = new List<COM_SimplePlayerInst>() ;
        infos.Add(_MyTeamInfo.members_[0]);
        for (int i = 0; i < _MyTeamInfo.members_.Length; ++i)
        {
            if (_MyTeamInfo.members_[i].instId_ == playerId)
            {
                infos[0] = _MyTeamInfo.members_[i];
                continue;
            }
            else
                infos.Add(_MyTeamInfo.members_[i]);
        }

        _MyTeamInfo.members_ = infos.ToArray();
		if(OnLeaderChange != null)
			OnLeaderChange(playerId);
//		if(OnTeamDirtyProps != null)
//		{
//			OnTeamDirtyProps(_MyTeamInfo.members_);
//		}
		if(OnUpdateMainteamList !=null)
		{
			OnUpdateMainteamList();
		}
    }

	static public void AddMyTeamMember(COM_SimplePlayerInst info)
    {
		List<COM_SimplePlayerInst> infos = new List<COM_SimplePlayerInst>();
        infos.AddRange(_MyTeamInfo.members_);
        infos.Add(info);
        _MyTeamInfo.members_ = infos.ToArray();
        _Readys.Add(new  Pair<int, bool>((int)info.instId_,false));
//		if(TeamUIPanel.UpdateTeamMB != null)
//		{
//			TeamUIPanel.UpdateTeamMB(info);
//		}
//		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
//		{
		    if(GamePlayer.Instance.InstId != (int)info.instId_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("jiaruteam").Replace("{n}",info.instName_));
			}
//		}
		if(OnUpdateTeamMB != null)
		{
			OnUpdateTeamMB(info);
		}
		if(OnUpdateMainteamList !=null)
		{
			OnUpdateMainteamList();
		}
    }

    static public void DelMyTeamMember(int playerId)
    {

		COM_SimplePlayerInst temp = null;
		List<COM_SimplePlayerInst> infos = new List<COM_SimplePlayerInst>();
        for (int i = 0; i < _MyTeamInfo.members_.Length; ++i)
        {
            if (_MyTeamInfo.members_[i].instId_ != playerId)
                infos.Add(_MyTeamInfo.members_[i]);
            else
            {
				temp = _MyTeamInfo.members_[i];
                for (int k = 0; k < _Readys.Count; ++k)
                {
                    if (_Readys[k].first == playerId)
                    {

                        _Readys.RemoveAt(k);
                        break;
                    }
                }
            }
              
        }
        _MyTeamInfo.members_ = infos.ToArray();
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(GamePlayer.Instance.InstId != (int)_MyTeamInfo.members_[i].instId_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("likaiteam").Replace("{n}",temp.instName_));
			}
		}
		if(OnUpdateMainteamList !=null)
		{
			OnUpdateMainteamList();
		}
		if (OnDelMenber!= null)
			OnDelMenber (playerId);
    }

    static public void SetMyTeamMemberReady(int playerId, bool isReady)
    {
        for (int k = 0; k < _Readys.Count; ++k)
        {
            if (_Readys[k].first == playerId)
            {
                _Readys[k].second = isReady;
                break;
            }
        }
    }

    static public bool IsMyTeamMemberAllReady()
    {
        for (int k = 0; k < _Readys.Count; ++k)
        {
            if (_Readys[k].second) continue;
            return false;
        }
        return true;
    }
	static public List<COM_SimpleTeamInfo> targetTeam(TeamType ttype)
	{
		List<COM_SimpleTeamInfo> teams = new List<COM_SimpleTeamInfo> ();
		teams.Clear ();
		for (int i =0; i<LobbyTeams.Count; i++)
		{
			if(LobbyTeams[i].type_ == ttype)
			{
				teams.Add(LobbyTeams[i]);
			}
		}
		return teams;
	}
	static public void CleanMyTeam(bool isKick)
    {
        //if(!GamePlayer.Instance.isInBattle)
        //    NetConnection.Instance.exitScene();
        _MyTeamInfo = new COM_TeamInfo();
        _Readys.Clear();
		if (OnExitIteam != null)
			OnExitIteam ();
		if(isKick)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("messageTichu"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("messageTichu"));
		}

		if(OnUpdateMainteamList !=null)
		{
			OnUpdateMainteamList();
		}

        GamePlayer.Instance.ShowBabyDirty = true;
        teamIsDirty_ = true;
    }
	static public void BackToLobbyTeam(COM_TeamInfo teaminfo)
	{
//        Prebattle.Instance.Fini();
//		StageMgr.LoadingAsyncScene(GlobalValue.StageName_groupScene);
		TeamUI.ShowMe();
	
	}
	static public void inviteJoinTeamOk(uint teamid, string myName)
	{
		if(!GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			NetConnection.Instance.jointLobby ();

		}
		isYqEnd = true;
		isYQ = true;
		isHanHua = false;
		myname = myName;
		TeamSystem.hTeamid = (int)teamid;

	}
    static public COM_SimpleTeamInfo GetCurTeam(int teamId)
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
	static public void UpdateDirtyProp(int guid, COM_PropValue[] props)
	{

		SyncTeamMemberProp (guid, props);

//		if(OnTeamDirtyProps != null)
//		{
//			OnTeamDirtyProps();
//		}
	}
	static public void UpdateMainTeamInfo()
	{
//		if(OnTeamDirtyProps != null)
//		{
//			OnTeamDirtyProps();
//		}
	}
	static public void UpdtaeMainTeamlIST()
	{
		if(OnUpdateMainteamList != null)
		{
			OnUpdateMainteamList();
		}
	}
	static public void leaveTeamOk(int pid)
	{
		COM_SimplePlayerInst temp = null;
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(pid == (int)_MyTeamInfo.members_[i].instId_)
			{
				_MyTeamInfo.members_[i].isLeavingTeam_ = true;
				temp = _MyTeamInfo.members_[i];
			}
		}
//		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
//		{
		    if(GamePlayer.Instance.InstId != pid)
		{
				PopText.Instance.Show(LanguageManager.instance.GetValue("zailiteam").Replace("{n}",temp.instName_));
			}
            else
            {
                GamePlayer.Instance.ShowBabyDirty = true;
            }
//		}
		if(OnUpdateMainteamList != null)
		{
			OnUpdateMainteamList();
		}
		if(OnUpdateMemStateUI != null)
		{
			OnUpdateMemStateUI(pid,true);
		}

	}
	static public void backTeamOK(int pid)
	{
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(pid == (int)_MyTeamInfo.members_[i].instId_)
			{
				_MyTeamInfo.members_[i].isLeavingTeam_ = false;
				if(_MyTeamInfo.members_[i].instId_ != (uint)GamePlayer.Instance.InstId)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("guiduichenggong").Replace("{n}",_MyTeamInfo.members_[i].instName_));
				}
                if (pid == GamePlayer.Instance.InstId)
                {
                    Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
                    Prebattle.Instance.StopSelfActorMove();
                }
				break;
			}
		}

		if(OnUpdateMainteamList != null)
		{
			OnUpdateMainteamList();
		}
		if(OnUpdateMemStateUI != null)
		{
			OnUpdateMemStateUI(pid,false);
		}
	}
	public static bool isBattleOpen;
	static public void teamCallMemberBack()
	{

		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(_MyTeamInfo.members_[i].instId_ != GamePlayer.Instance.InstId)
				continue;
			if(!_MyTeamInfo.members_[i].isBattle_)
			{
				if(!GlobalValue.isBattleScene(StageMgr.Scene_name))
				{
					MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("zhaohuan"), () => {
						
						NetConnection.Instance.backTeam();
						Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
					},false,()=>{
						NetConnection.Instance.refuseBackTeam();
					});
				}else
				{
					isBattleOpen = true;
				}

			}else
			{
				isBattleOpen = true;
			}
		}
	}

	public static void BackTeam()
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("zhaohuan"), () => {
			
			NetConnection.Instance.backTeam();
			Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
		},false,()=>{
			NetConnection.Instance.refuseBackTeam();
		});
		isBattleOpen = false;
	}
	
	public static void UpdateInBattle(int instid,bool isbattle)
	{
		if(!IsInTeam())return;
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(_MyTeamInfo.members_[i].instId_ ==(int) instid)
			{
				_MyTeamInfo.members_[i].isBattle_ = isbattle;
			}
		}
	}
	public static void RefuseBackTeamOk(int playerId)
	{
		for(int i =0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(_MyTeamInfo.members_[i].instId_ == (uint)playerId)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("jujuerudui").Replace("{n}",_MyTeamInfo.members_[i].instName_));
				return;
			}
		}
	}
	public static COM_SimplePlayerInst memberSelf()
	{
		for(int i=0;i<_MyTeamInfo.members_.Length;i++)
		{
			if(_MyTeamInfo.members_[i].instId_ == GamePlayer.Instance.InstId)
			{
				return _MyTeamInfo.members_[i];
			}
		}
		return null;
	}
	static public void Clear()
	{
		isHanHua = false;
		isYQ = false;
		hTeamid = 0;
		isYqEnd = false;
		_LobbyTeams.Clear ();
		PlayerInsts.Clear ();
		maxMembers = 5;
		_MyTeamInfo = new COM_TeamInfo ();
	}
}


	
