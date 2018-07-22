using UnityEngine;
using System.Collections;

public enum chatType
{
	baby,
	item,
	team,
}
public class OpenTeam : MonoBehaviour {

	public delegate void TeamNumbers();
	public static TeamNumbers TeamNumbersOk;
	public static bool isSceneHan = false;
	public static string password;
	static bool isTopteam;
	COM_SimpleTeamInfo minfot;
	//public static chatType type;
	void Start()
	{
		TeamNumbersOk = Teams;
//		ChatSystem.ShowItemInstResOk += showInsetItem;
//		ChatSystem.ShowBabyInstResOk += showbabyInst;
	}
//	void showInsetItem(COM_ShowItemInst ItemInst)
//	{
//		ItemsTips.ShowMe((int)ItemInst.itemInst_.itemId_);
//	}
//	void showbabyInst(COM_ShowbabyInst babyInst)
//	{
//		ChatBabytips.ShowMe(babyInst.babyInst_);
//	}
	void Teams()
	{
		if(!isTopteam)return;
		if(GetCurTeam(TeamSystem.hTeamid)!= null)
		{
			COM_SimpleTeamInfo infot = GetCurTeam(TeamSystem.hTeamid);
			minfot = infot;
			int level = GamePlayer.Instance.GetIprop(PropertyType.PT_Level);
			if(level<infot.minLevel_||level>infot.maxLevel_)
			{
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dengjitishi"));
				PopText.Instance.Show(LanguageManager.instance.GetValue("dengjitishi"));
			}
			else
				if(infot.curMemberSize_==infot.maxMemberSize_)
			{
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("duiyuanyiman"));
				PopText.Instance.Show(LanguageManager.instance.GetValue("duiyuanyiman"));
			}
			else 
			{

				NetConnection.Instance.joinTeam((uint)infot.teamId_,infot.pwd_);
			}


		}else
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("hanhuatishi"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("hanhuatishi"));
		}
		isTopteam = false;
		NetConnection.Instance.exitLobby ();
	}

	COM_SimpleTeamInfo mTeaminfo;
	void OnClick ()
	{

		UILabel lbl = GetComponent<UILabel>();
		if (lbl != null)
		{
			string url = lbl.GetUrlAtPosition(UICamera.lastHit.point);

			if (!string.IsNullOrEmpty(url))
			{

				string [] typs = url.Split(',');
				int type = int.Parse(typs[0]);
				if(type == 1)
				{
					if (!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
						return;
					}
					
					if (TeamSystem.IsInTeam())
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("yiyouduiwu"));
						return;
					}
					
					TeamSystem.isHanHua = true;
					//TeamSystem.isYQ = false;
					isTopteam = true;
					TeamSystem.hTeamid = int.Parse(typs[1]);
					if(GamePlayer.isIngroupScene)
					{
						isSceneHan = true;
					}else
					{
						isSceneHan = false;
					}
					NetConnection.Instance.jointLobby();
				}else
					if(type == 2)
				{
					NetConnection.Instance.queryItemInst(int.Parse(typs[1]));
				}else
					if(type == 3)
				{
					NetConnection.Instance.querybabyInst(int.Parse(typs[1]));
				}



//				if(type == chatType.baby)
//				{
//					int bid = int.Parse(url);
//					NetConnection.Instance.querybabyInst(bid);
//					//ChatBabytips.ShowMe(babyinset);
//				}else
//					if(type == chatType.item)
//				{
//					//NetConnection.Instance.queryItemInst();
//					//ItemsTips.ShowMe(comItem);
//				}else
//					if(type == chatType.team)
//				{
//                    if (!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Castle))
//                    {
//                        return;
//                    }
//
//                    if (TeamSystem.IsInTeam())
//                    {
//                        PopText.Instance.Show(LanguageManager.instance.GetValue("yiyouduiwu"));
//                        return;
//                    }
//					
//					TeamSystem.isHanHua = true;
//					TeamSystem.isYQ = false;
//					isTopteam = true;
//					string []urls = url.Split(',');
//					TeamSystem.hTeamid = int.Parse(urls[0]);
//					if(GamePlayer.isIngroupScene)
//					{
//						isSceneHan = true;
//					}else
//					{
//						isSceneHan = false;
//					}
//					NetConnection.Instance.jointLobby();
				//}



			}
		}
	}


	void InPutpassWord(string str)
	{
		password = str;
		StageMgr.LoadingAsyncScene(GlobalValue.StageName_groupScene);
		//NetConnection.Instance.joinTeam((uint)minfot.teamId_,str);
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
}
