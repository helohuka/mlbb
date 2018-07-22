using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FastTeamPanel : UIBase {


	public UILabel _TitleLable;
	public UILabel _TargetLable;
	public UILabel _CreateLable;
	public UILabel _FastMatchLable;
	public UILabel _TishiLable;

	public UIGrid target_Grid;
	public GameObject target_item;
	public UILabel wuLabel;
	public UIGrid team_Grid;
	public GameObject team_item;

	public UIButton CloseBtn;
	public UIButton CreateBtn;
	public UIButton KuaisuBtn;
	public UIButton ReBtn;

	public GameObject tips;
	TeamType typeTeam;
	public delegate void UpdateLobbyTeamInfo(COM_SimpleTeamInfo infos);
	public static UpdateLobbyTeamInfo gameUpdateLobbyInfo;

	public delegate void UpdateLobby(COM_SimpleTeamInfo infos);
	public static UpdateLobby gameUpdateLobby;

	public delegate void InitLobbyTeam(COM_SimpleTeamInfo[] infos);
	public static InitLobbyTeam InitLobbyTeamOK;

	private TeamTargetCell curCell;

	List<COM_SimpleTeamInfo>SimpleTeamInfos = new List<COM_SimpleTeamInfo>();
	void Start () {
		InitUIText ();
		typeTeam = TeamSystem._teamType;
		team_item.SetActive (false);
		target_item.SetActive (false);
		ReBtn.gameObject.SetActive (false);
		wuLabel.gameObject.SetActive (false);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (CreateBtn.gameObject, EnumButtonEvent.OnClick, OnClickCreate, 0, 0);
		UIManager.SetButtonEventHandler (KuaisuBtn.gameObject, EnumButtonEvent.OnClick, OnClickKuaisu, 0, 0);
		//UIManager.SetButtonEventHandler (ReBtn.gameObject, EnumButtonEvent.OnClick, OnClickRe, 0, 0);
		gameUpdateLobby = UpdateLobbyTeam;
		TeamSystem.OnInitMyTeam += HideSelf;
		TeamSystem.OnUpdateMyTeamInfo += updateTeaminfo;
		TeamSystem.OnChangeTeam += UpdateListTeamInfo;
		ShowContingent ();
		AddtargetItems ();
		if(TeamSystem._teamType != TeamType.TT_None)
		GlobalInstanceFunction.Instance.Invoke (() => {UpdateTeamTargetPosition ();}, 1);

	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("TeamF_Title");
		_TargetLable.text = LanguageManager.instance.GetValue("TeamF_Target");
		_CreateLable.text = LanguageManager.instance.GetValue("TeamF_Create");
		_FastMatchLable.text = LanguageManager.instance.GetValue("TeamF_FastMatch");
		_TishiLable.text = LanguageManager.instance.GetValue("TeamF_Tishi");
	}
	void UpdateListTeamInfo(COM_TeamInfo info)
	{
		for(int i =0;i<TeamCellList.Count;i++)
		{
			ListTeamCell listCell = TeamCellList[i].GetComponent<ListTeamCell>();
			if(listCell.SimpleTeamInfo.teamId_ == info.teamId_)
			{
				listCell.SimpleTeamInfo = info;
			}
		}
	}
	void updateTeaminfo()
	{
		RefreshTeamTypeMenbers (typeTeam);

	}
	void HideSelf()
	{
		Hide ();
	}
	void UpdateLobbyTeam(COM_SimpleTeamInfo lobby)
	{
		//SimpleTeamInfos.Add (lobby);
		//RefreshTeam (SimpleTeamInfos);
		RefreshTeamTypeMenbers (typeTeam);
	}
	void ShowContingent()
	{
		RefreshTeamTypeMenbers (typeTeam);
		//TeamTypeMenbers (TeamType.TT_None);
		//SimpleTeamInfos.AddRange (TeamSystem.LobbyTeams);
		//AddTeamItems (SimpleTeamInfos);
	}
	void OnClickCreate(ButtonScript obj, object args, int param1, int param2)
	{
		CreateTeamUIPanel.SwithShowMe ();
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickKuaisu(ButtonScript obj, object args, int param1, int param2)
	{
		int Level = GamePlayer.Instance.GetIprop(PropertyType.PT_Level);
		List<COM_SimpleTeamInfo> simpInfos = TeamSystem.targetTeam (typeTeam);
		if(simpInfos.Count == 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("wuciduiwu"));
			return ;
		}
		for(int i=0;i<simpInfos.Count;i++)
		{
			if(Level<simpInfos[i].minLevel_||Level>simpInfos[i].maxLevel_)
			{
				continue;
			}else
			{

				if(simpInfos[i].isWelcome_)
				{
					continue;
				}
				
				ListTeamCell.teamId = (int)simpInfos[i].teamId_;
				NetConnection.Instance.joinTeam ((uint)simpInfos[i].teamId_,simpInfos[i].pwd_);
				return;
			}
		}
		PopText.Instance.Show(LanguageManager.instance.GetValue("wuciduiwu"));
	}
	void OnClickRe(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.jointLobby ();
	}

	public void AddtargetItems()
	{
		for (TeamType i = TeamType.TT_None; i < TeamType.TT_Max; ++i)
		{
			GameObject obj = GameObject.Instantiate(target_item) as GameObject;
			obj.SetActive(true);
			obj.transform.parent = target_Grid.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.name = Enum.GetName(typeof(TeamType),i);
			UIManager.SetButtonEventHandler(obj, EnumButtonEvent.OnClick, OnClicTeamType,(int)Enum.Parse(typeof(TeamType),Enum.GetName(typeof(TeamType),i)), 0);
			obj.GetComponent<TeamTargetCell>().nameLabel.text =  LanguageManager.instance.GetValue( Enum.GetName(typeof(TeamType),i));
			if(i==typeTeam)
			{
				curCell = obj.GetComponent<TeamTargetCell>();
				obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(true);

			}else
			{
				obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(false);
			}

		}
	}
	void UpdateTeamTargetPosition()
	{
		for (TeamType i = TeamType.TT_None; i < TeamType.TT_Max; ++i)
		{
			if(i==typeTeam)
			{
				UICenterOnIndex CenterOnIndex = target_Grid.GetComponent<UICenterOnIndex>();
				CenterOnIndex.UpdateItemPosition((int)i);
			}

		}
	}
	void OnClicTeamType(ButtonScript obj, object args, int param1, int param2)
	{
		typeTeam = (TeamType)param1;
		TeamTargetCell tcell = obj.GetComponent<TeamTargetCell>();
		if(curCell != null)
		{
			curCell.iconSp.gameObject.SetActive(false);
		}
		curCell = tcell;
		tcell.iconSp.gameObject.SetActive (true);
		RefreshTeamTypeMenbers (typeTeam);
	}
	void RefreshTeamTypeMenbers(TeamType ttype)
	{
		if (TeamSystem.targetTeam (ttype).Count == 0)
		{
			wuLabel.gameObject.SetActive (true);
		}else
		{
			wuLabel.gameObject.SetActive (false);
		}
						
		RefreshTeam (TeamSystem.targetTeam (ttype));
	}

	private List<GameObject> TeamCellPool = new List<GameObject>();
	private List<GameObject> TeamCellList = new List<GameObject>();
	void RefreshTeam(List<COM_SimpleTeamInfo> infos)
	{

			
		for(int i=0;i<TeamCellList.Count;i++)
		{
			TeamCellList[i].transform.parent = null;
			TeamCellList[i].SetActive(false);
			TeamCellPool.Add(TeamCellList[i]);
		}
		TeamCellList.Clear ();
		for(int i = 0;i<infos.Count;i++)
		{
			GameObject clone = null;
			if(TeamCellPool.Count>0)
			{
				clone = TeamCellPool[0];
				TeamCellPool.Remove(clone);
			}else
			{
			 clone = GameObject.Instantiate (team_item)as GameObject;
			}
		    clone.transform.parent = team_Grid.transform;
			clone.SetActive (true);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			
			TeamCellList.Add(clone);
			ListTeamCell listCell = clone.GetComponent<ListTeamCell>();
			listCell.SimpleTeamInfo = infos[i];
		}
		   team_Grid.Reposition ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FastTeamPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FastTeamPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FastTeamPanel);
	}

	void OnDestroy()
	{
		TeamSystem.OnInitMyTeam -= HideSelf;
		gameUpdateLobby = null;
		TeamSystem.OnUpdateMyTeamInfo -= updateTeaminfo;
		TeamSystem.OnChangeTeam -= UpdateListTeamInfo;
		//InitLobbyTeamOK = null;
	}

	public override void Destroyobj ()
	{

	}
}
