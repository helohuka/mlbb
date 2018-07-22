using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public static class GuildSystem {

	public static int curPage = 1;
	public static int maxPage;
	public delegate void QueryGuildListResult(short page, short pageNum, COM_GuildViewerData[] Datas);
	public static event QueryGuildListResult QueryGuildListResultOk;

	public delegate void InitGuildDataInfo(COM_Guild guild);
	public static event InitGuildDataInfo InitGuildDataOk;

	public delegate void updateGuildfndzNum(int num);
	public static event updateGuildfndzNum updateGuildfndzOk; 

	public delegate void InitMemberDataInfo(COM_GuildMember[] members);
	public static event InitMemberDataInfo InitmemberDataOk;

	public delegate void UpdateGuild();
	public static event UpdateGuild UpdateGuildInfo;

	public delegate void UpdateMemberJob();
	public static event UpdateMemberJob UpdateMemberJobOk;

	public delegate void updateGuildShop();
	public static event updateGuildShop updateGuildShopOk;

	public delegate void leaveGuild();
	public static event leaveGuild leaveGuildEnter;

	public static float guildBatatleStateTime;

	public delegate void updateGuildBattleEvent(string otehername);
	public static event updateGuildBattleEvent updateGuildBattleEventOk;

	public delegate void updateGuildBattleWinEvent(int selfCount,int otherCount);
	public static event updateGuildBattleWinEvent updateGuildBattleWinEventOk;


	public delegate void BuyGuildshop(short items);
	public static event BuyGuildshop BuyGuildshopOK;

	public delegate void UpdateGuildMember(COM_GuildMember Member);
	public static event UpdateGuildMember UpdateGuildMemberOk;

	public delegate void UpdateGuildShopCount(COM_GuildMember Member);
	public static event UpdateGuildShopCount UpdateGuildShopCountOk;

	public delegate void startGuildBattle(string otherName, int otherCon, int selfCon);
	public static event startGuildBattle startGuildBattleOk;

	public delegate void UpdateGuildmenbers(COM_GuildMember Member);
	public static event UpdateGuildmenbers UpdateGuildmenbersok;

	public delegate void UpdateGuildLevel(GuildBuildingType type, COM_GuildBuilding building);
	public static event UpdateGuildLevel UpdateGuildLevelok;

	public static List<int> shopitems = new List<int>();
	private static List<COM_GuildMember> VicePremiers = new List<COM_GuildMember> ();

	public static List<COM_GuildMember>GuildMembers = new List<COM_GuildMember> ();
	public static List<COM_GuildRequestData> GuildRequestDatas = new List<COM_GuildRequestData> ();
	public static List<string> historyMessage = new List<string> ();
	public static int selfwinCount;
	public static int otherwinCount;
	public static string otherName;
	public static int otherNum;
	public static int otherlevel;
	public static int battleState;
	public static List<COM_GuildViewerData> ViewerDatas = new List<COM_GuildViewerData> ();
	//public static COM_GuildBattle curGuildBattle;
	public static int defMoney = 10000;
	private static COM_Guild mguild;
	public static COM_Guild Mguild
	{
		set
		{
			mguild = value ;
		}
		get
		{
			return mguild;
		}
	}
	static public bool IsInGuild()
	{
		return mguild != null && mguild.guildId_ != 0;
	}
	static public bool IsGuildLeader(int insetId)
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].roleId_ == insetId && GuildMembers[i].job_ == 3)
			{
				return true;
			}
		}
		return false;
	}

	static public Dictionary<int, bool[]> guildRequestDic_;
	static public bool isExt;
	static public void UpdateRequest(int page, int index, bool flag)
	{
		if(!guildRequestDic_.ContainsKey(page))
			return;

		if(guildRequestDic_[page].Length <= index)
			return;

		guildRequestDic_ [page] [index] = flag;
	}

	static public bool GetRequest(int page, int index)
	{
		if(!guildRequestDic_.ContainsKey(page))
			return false;
		
		if(guildRequestDic_[page].Length <= index)
			return false;

		return guildRequestDic_ [page] [index];
	}

	public static void QueryGuildListResults(short page, short pageNum, COM_GuildViewerData[] Datas)
	{
		if(guildRequestDic_ == null)
			guildRequestDic_ = new Dictionary<int, bool[]>();

		if(!guildRequestDic_.ContainsKey(page))
			guildRequestDic_.Add(page, new bool[Datas.Length]);
		//FamilyPanelUI.ShowMe ();
		ViewerDatas.Clear ();
		ViewerDatas.AddRange (Datas);

		maxPage = (int)pageNum;
		if(QueryGuildListResultOk != null)
		{
			QueryGuildListResultOk(page,pageNum,Datas);
		}
	}
	public static void InitGuildData(COM_Guild guild)
	{
		if(mguild != null && mguild.master_ != guild.master_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("yijiaobangzhu").Replace("{n}",guild.masterName_));
		}
		mguild = guild;
		isExt = false;
		GuildRequestDatas.Clear ();
		GuildRequestDatas.AddRange (guild.requestList_);
		if(InitGuildDataOk != null)
		{
			InitGuildDataOk(guild);
		}
	}
	public static void InitMembers(COM_GuildMember[] members)
	{
		isExt = false;
		GuildMembers.Clear ();
		GuildMembers.AddRange (members);
		st ();
		for(int i=0;i<members.Length;i++)
		{
			if(members[i].roleId_ == GamePlayer.Instance.InstId)
			{
				FamilySystem.instance.GuildMember = members[i];
			}
		}
		if(InitmemberDataOk != null)
		{
			InitmemberDataOk(members);
		}
	}

	public static void UpdateMembers(COM_GuildMember guildMember,ModifyListFlag ListFlag)
	{
		string temp = "";
		string stime = "";
		string dec = "";
		isExt = false;
		if(ListFlag == ModifyListFlag.MLF_Add)
		{
			stime = LanguageManager.instance.GetValue("MLF_time").Replace("{n}",System.DateTime.Now.Month.ToString()).Replace("{n1}",System.DateTime.Now.Day.ToString()).Replace("{n2}",System.DateTime.Now.Hour.ToString()).Replace("{n3}",System.DateTime.Now.Minute.ToString()) ;
			dec = LanguageManager.instance.GetValue("MLF_Add").Replace("{n}",guildMember.roleName_);
			temp = stime+";"+dec;
			if(!ContainsName(guildMember))
			GuildMembers.Add (guildMember);
			if(UpdateGuildmenbersok != null)
			{
				UpdateGuildmenbersok(guildMember);
			}
		}else
			if(ListFlag == ModifyListFlag.MLF_Delete)
		{
			stime = LanguageManager.instance.GetValue("MLF_time").Replace("{n}",System.DateTime.Now.Month.ToString()).Replace("{n1}",System.DateTime.Now.Day.ToString()).Replace("{n2}",System.DateTime.Now.Hour.ToString()).Replace("{n3}",System.DateTime.Now.Minute.ToString()) ;
			dec = LanguageManager.instance.GetValue("MLF_Delete").Replace("{n}",guildMember.roleName_);
			temp = stime+";"+dec;
			if(guildMember.roleId_ == GamePlayer.Instance.InstId)
			{
				guildRequestDic_.Clear();
				historyMessage.Clear();
			}
//			if(tichuCuild(guildMember)&&guildMember.roleId_==GamePlayer.Instance.InstId)
//			{
//				PopText.Instance.Show(LanguageManager.instance.GetValue("tichujiazu"));
//			}


			DelguildMember(guildMember);
		}else
			if(ListFlag == ModifyListFlag.MLF_ChangePosition)
		{
			ChangePosition(guildMember);
			if(UpdateMemberJobOk != null)
			{
				UpdateMemberJobOk();
			}
			stime = LanguageManager.instance.GetValue("MLF_time").Replace("{n}",System.DateTime.Now.Month.ToString()).Replace("{n1}",System.DateTime.Now.Day.ToString()).Replace("{n2}",System.DateTime.Now.Hour.ToString()).Replace("{n3}",System.DateTime.Now.Minute.ToString()) ;
			dec = LanguageManager.instance.GetValue("MLF_ChangePosition").Replace("{n}",guildMember.roleName_).Replace("{n1}",LanguageManager.instance.GetValue(guildMember.job_.ToString()));
			temp = stime+";"+dec;
		}else
			if(ListFlag == ModifyListFlag.MLF_ChangeLevel||ListFlag == ModifyListFlag.MLF_ChangeProfession||ListFlag == ModifyListFlag.MLF_ChangeContribution)
		{
			for(int i =0;i<GuildMembers.Count;i++)
			{
				if(GuildMembers[i].roleId_ == guildMember.roleId_)
				{
					GuildMembers.RemoveAt(i);
					GuildMembers.Add(guildMember);
				}
			}
		}else if(ListFlag == ModifyListFlag.MLF_ChangeOnline)
		{
			for(int i =0;i<GuildMembers.Count;i++)
			{
				if(GuildMembers[i].roleId_ == guildMember.roleId_)
				{
					GuildMembers.RemoveAt(i);
					GuildMembers.Add(guildMember);
				}
			}
		}else if(ListFlag == ModifyListFlag.MLF_ChangeOffline)
		{
			for(int i =0;i<GuildMembers.Count;i++)
			{
				if(GuildMembers[i].roleId_ == guildMember.roleId_)
				{
					GuildMembers.RemoveAt(i);
					GuildMembers.Add(guildMember);
				}
			}
		}
		st ();
		if(InitmemberDataOk != null)
		{
			InitmemberDataOk(GuildMembers.ToArray());
		}
		if(GuildSystem.UpdateGuildShopCountOk != null)
		{
			GuildSystem.UpdateGuildShopCountOk(guildMember);
		}
		if(!temp.Equals(""))
		historyMessage.Add (temp);
	}
	public static bool tichuCuild(COM_GuildMember Member)
	{
		for (int i =0; i<GuildMembers.Count; i++)
		{
			if(Member.roleId_ == GuildMembers[i].roleId_)
			{
				return true;
			}
		}
		return false;
	}
	public static void UpdateGuildData()
	{
		if(UpdateGuildInfo != null)
		{
			UpdateGuildInfo();
		}
	}

	public  static void UpdateGuildList(COM_GuildViewerData ViewerData, ModifyListFlag Flag)
	{

	}

	public static void DismissGuild()
	{
		mguild = new COM_Guild ();
		isExt = true;
		historyMessage.Clear ();
		if(guildRequestDic_ != null)
		guildRequestDic_.Clear();
		MyFamilyInfo.HideMe ();
	}
	static bool ContainsName(COM_GuildMember guildMember)
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(guildMember.roleName_.Equals(GuildMembers[i].roleName_))
			{
				return true;
			}
		}
		return false;
	}
	static public void leaveMyGuild(string name)
	{
		if(name.Equals(GamePlayer.Instance.InstName))
		{
			mguild = new COM_Guild ();
			isExt = true;
			historyMessage.Clear();
		}

	}
   static  void DelguildMember(COM_GuildMember guildMember)
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(guildMember.roleName_.Equals(GuildMembers[i].roleName_))
			{
				GuildMembers.Remove(GuildMembers[i]);

			}
		}

	}
	public static bool IsMyGuildMember(int insetId)
	{
		for(int i =0;i< GuildMembers.Count;i++)
		{
			if(GuildMembers[i].roleId_ == insetId)
			{
				return true;
			}
		}
		return false;
	}

	static void ChangePosition(COM_GuildMember guildMember)
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(guildMember.roleName_.Equals(GuildMembers[i].roleName_))
			{
				GuildMembers.RemoveAt(i);
				GuildMembers.Add(guildMember);
				break;
			}
		}
	}
	static public COM_GuildMember GetPremier()
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].job_ == (sbyte)GuildJob.GJ_Premier)
			{
				return GuildMembers[i];
			}
		}
		return null;
	}
	static public List<COM_GuildMember> GetVicePremiers()
	{
		VicePremiers.Clear ();
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].job_ == (sbyte)GuildJob.GJ_VicePremier)
			{
				VicePremiers.Add(GuildMembers[i]);
			}
		}
		return VicePremiers;
	}
	static public void UpdateGuildLevelbu(GuildBuildingType type, COM_GuildBuilding building)
	{
			if(UpdateGuildLevelok!= null)
			{
				UpdateGuildLevelok(type,building);
			}
	}

    static public bool IsInMyGuild(int instId)
    {
        if (GuildMembers == null || GuildMembers.Count == 0)
            return false;

        for (int i = 0; i < GuildMembers.Count; ++i)
        {
            if (GuildMembers[i].roleId_ == instId)
                return true;
        }
        return false;
    }
	static public void updateGuildShopItems(int[] items)
	{
		shopitems.Clear ();
		shopitems.AddRange (items);
		if(updateGuildShopOk != null)
		{
			updateGuildShopOk();
		}
    }
//	static public void updateGuildBattle(COM_GuildBattle battle)
//	{
//		curGuildBattle = battle;
//
//		if(updateGuildBattleEventOk != null)
//		{
//			updateGuildBattleEventOk(battle);
//		}
//
//	}
	static public void buyGuildShopItemOk(short times)
	{
		if(BuyGuildshopOK != null)
		{
			BuyGuildshopOK(times);
		}
	}
	static public COM_GuildMember GetGuildMemberSelf(int insetId)
	{
		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].roleId_ == insetId)
			{
				return GuildMembers[i];
			}
		}

		return null;
	}
	static public void UpdateGuildMemberInfo(COM_GuildMember GuildMember)
	{

		for(int i =0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].roleId_ == GuildMember.roleId_)
			{
				GuildMembers.RemoveAt(i);
				GuildMembers.Add(GuildMember);
			}
		}
		if(UpdateGuildMemberOk != null)
		{
			UpdateGuildMemberOk(GuildMember);
		}
	}

    static public int GetMyActionPoint()
    {
//        for (int i = 0; i < GuildMembers.Count; i++)
//        {
//            if (GuildMembers[i].roleId_ == GamePlayer.Instance.InstId)
//            {
//                return GuildMembers[i].guildBattleCon_;
//            }
//        }
        return 0;
    }
	public static int GuildShopLevel(int level)
	{
	
		if(level>1&&level<4)
		{
			return 1;
		}else
			if(level>3&&level<5)
		{
			return 2;
		}else
			if(level ==5)
		{
			return 3;
		}
		return 0;
	}
	public static void openBattle(string otehername,int num, int level )
	{
		//GlobalValue.Get (Constant.C_GuildBattleStartIntervalTime, out guildBatatleStateTime);
		battleState = 1;
		otherName = otehername;
		otherNum = num;
		otherlevel = level;
		EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_jiazhuzhankaiqi, ApplicationEntry.Instance.uiRoot.transform);
		if(updateGuildBattleEventOk != null)
		{
			updateGuildBattleEventOk(otehername);
		}
	}
	public static void closeGuildBattle(bool isWinner)
	{
		battleState = 3;
		if(isWinner)
		{
			EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_jiazuzhandoushengli, ApplicationEntry.Instance.uiRoot.transform);
		}else
		{
			EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_jiazuzhanshibai, ApplicationEntry.Instance.uiRoot.transform);
		}
		//EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_jiazhuzhanjieshu, ApplicationEntry.Instance.uiRoot.transform);

	}
	public static void syncGuildBattleWinCount(int count,int ocount)
	{
		selfwinCount = count;
		otherwinCount = ocount;
		if(updateGuildBattleWinEventOk != null)
		{
			updateGuildBattleWinEventOk(count,ocount);
		}
	}
	public static void updateGuildFanz(int num)
	{
		if(updateGuildfndzOk!= null)
		{
			updateGuildfndzOk(num);
		}
	}
	public static void StartGuildBattle(string otName, int otherCon, int selfCon)
	{
		battleState = 2;
		selfwinCount = selfCon;
		otherwinCount = otherCon;
		otherName = otName;
		EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_jiazuzhandoukaishi, ApplicationEntry.Instance.uiRoot.transform);
		GlobalInstanceFunction.isGuildBattleStart = false;
		if(startGuildBattleOk != null)
		{
			startGuildBattleOk(otherName,otherCon,selfCon);
		}
	}
	public static int GetJopNumber(GuildJob jop)
	{
		int num = 0;
		for(int i=0;i<GuildMembers.Count;i++)
		{
			if(GuildMembers[i].job_ == (int)jop)
			{
				num++;
			}
		}
		return num;
	}
	public static bool IsGuildMessage()
	{
		GuildJob gjob = (GuildJob)Enum.ToObject (typeof(GuildJob), GetGuildMemberSelf(GamePlayer.Instance.InstId).job_);
		if(gjob == GuildJob.GJ_Premier&& GuildRequestDatas.Count>0||gjob == GuildJob.GJ_VicePremier && GuildRequestDatas.Count>0)
		{
			return true;
		}
		return false;
	}
    static public void Clear()
    {
        shopitems.Clear();
        VicePremiers.Clear();
        GuildMembers.Clear();
		historyMessage.Clear ();
			if(guildRequestDic_ != null)
		{
			foreach( bool [] t in guildRequestDic_.Values)
			{
				for(int i=0;i< t.Length;i++)
				{
					t[i]=false;
				}
			}
		}

        //curGuildBattle = null;
        mguild = null;
    }

	static void st()
	{
	
		for (int i = 0; i < GuildMembers.Count; i++)
		{
			for (int j = i; j < GuildMembers.Count; j++)
			{
				if (GuildMembers[i].job_ < GuildMembers[j].job_)
				{
					COM_GuildMember temp = GuildMembers[i];
					GuildMembers[i] = GuildMembers[j];
					GuildMembers[j] = temp;
				}
			}
		}

	
	}




}
