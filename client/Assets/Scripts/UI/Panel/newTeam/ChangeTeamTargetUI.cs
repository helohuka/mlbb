using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChangeTeamTargetUI : UIBase {

//	public UILabel _ChangeTitleLable;
//	public UILabel _ChangeTargetLable;
//	public UILabel _ChangeLevelLable;
//	public UILabel _ChangeEnterLable;
//
//	public GameObject item_target;
//	public UIGrid grid_target;
//	public UIScrollView leftScro;
//	public UIScrollView rightScro;
//	public GameObject item_leftlevel;
//	public Transform grid_leftlevel;
//	public GameObject item_rightlevel;
//	public Transform grid_rightlevel;
//	public UIButton CloseBtn;
//	public UIButton CreateBtn;
//	UICenterOnChild centerLeft;
//	UICenterOnChild centerright;
//	TeamTargetCell _tcell;
//	List<GameObject> leftItems = new List<GameObject> ();
//	TeamType typeTeam;
//	void Start () {
//		_ChangeTitleLable.text = LanguageManager.instance.GetValue("Team_ChangeTitle");
//		_ChangeTargetLable.text = LanguageManager.instance.GetValue("Team_ChangeTarget");
//		_ChangeLevelLable.text = LanguageManager.instance.GetValue("Team_ChangeLevel");
//		_ChangeEnterLable.text = LanguageManager.instance.GetValue("Team_ChangeEnter");
//		item_leftlevel.SetActive (false);
//		item_rightlevel.SetActive (false);
//		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClicClose, 0, 0);
//		UIManager.SetButtonEventHandler(CreateBtn.gameObject, EnumButtonEvent.OnClick, OnClicCreate, 0, 0);
//		item_target.SetActive (false);
//		for (TeamType i = TeamType.TT_None; i < TeamType.TT_Max; ++i)
//		{
//			GameObject obj = GameObject.Instantiate(item_target) as GameObject;
//			obj.SetActive(true);
//			obj.transform.parent = grid_target.transform;
//			obj.transform.localScale = Vector3.one;
//			obj.transform.localPosition = Vector3.zero;
//			obj.name = Enum.GetName(typeof(TeamType),i);
//			UIManager.SetButtonEventHandler(obj, EnumButtonEvent.OnClick, OnClicTeamType,(int)Enum.Parse(typeof(TeamType),Enum.GetName(typeof(TeamType),i)), 0);
//			obj.GetComponent<TeamTargetCell>().nameLabel.text =LanguageManager.instance.GetValue( Enum.GetName(typeof(TeamType),i));
//			obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(false);
//
//		}
//		centerLeft = grid_leftlevel.GetComponent<UICenterOnChild> (); 
//		centerLeft.onFinished += OnCenterLeftFinished;
//		centerright = grid_rightlevel.GetComponent<UICenterOnChild> (); 
//		centerright.onFinished += OnCenterrightFinished;
//		TeamSystem.OnChangeTeam += HideSelf;
//		rightScro.onDragFinished += ScrollviewOk;
//		leftScro.onDragFinished += ScrollviewOk;
//		InitLevelLeft(60);
//		InitLevelRight (60);
//	}
//	void ScrollviewOk()
//	{
//		CreateBtn.isEnabled = false;
//	}

//	void OnCenterLeftFinished()
//	{
//		CreateBtn.isEnabled = true;
//		TeamSystem.minLevel = int.Parse (centerLeft.centeredObject.GetComponent<UILabel> ().text);
//	}
//	void OnCenterrightFinished()
//	{
//		CreateBtn.isEnabled = true;
//		TeamSystem.maxLevel = int.Parse (centerright.centeredObject.GetComponent<UILabel>().text);
//	}
//
//	void clearLeftItem()
//	{
//		for(int i=0;i<leftItems.Count;i++)
//		{
//			Destroy(leftItems[i]);
//		}
//	}
//	void InitLevelLeft(int maxLevel)
//	{
//		for(int i=0;i<maxLevel;i++)
//		{
//			GameObject obj = GameObject.Instantiate(item_leftlevel)as GameObject;
//			obj.SetActive(true);
//			obj.transform.parent = grid_leftlevel.transform;
//			obj.transform.position = Vector3.zero;
//			obj.transform.localScale = Vector3.one;
//			UILabel label = obj.GetComponent<UILabel>();
//			label.text = (leftLevel+i).ToString();
//			//grid_leftlevel.repositionNow = true;
//			grid_leftlevel.GetComponent<UIGrid>().repositionNow = true;
//			leftItems.Add(obj);
//		}
//	}
//	void InitLevelRight(int maxLevel)
//	{
//		for(int i=maxLevel;i>0;i--)
//		{
//			GameObject obj = GameObject.Instantiate(item_rightlevel)as GameObject;
//			obj.SetActive(true);
//			obj.transform.parent = grid_rightlevel.transform;
//			obj.transform.position = Vector3.zero;
//			obj.transform.localScale = Vector3.one;
//			UILabel label = obj.GetComponent<UILabel>();
//			label.text = i.ToString();
//			grid_rightlevel.GetComponent<UIGrid>().repositionNow = true;
//		}
//	}
//	int minsLevel;
//	int leftLevel=1;
//	int rightLevel = 60;
//	void OnClicTeamType(ButtonScript obj, object args, int param1, int param2)
//	{
//		typeTeam = (TeamType)param1;
//		GetminLevel (typeTeam);
//
//		if(typeTeam == TeamType.TT_None)
//		{
//			leftLevel=1;
//		}else
//		{
//			leftLevel = minsLevel;
//		}
//
//		InitLevelLeft (rightLevel-leftLevel+1);
//
//		TeamTargetCell tcell = obj.GetComponent<TeamTargetCell>();
//		if(_tcell != null)
//		{
//			_tcell.iconSp.gameObject.SetActive(false);
//		}
//		_tcell = tcell;
//		tcell.iconSp.gameObject.SetActive (true);
//		
//	}
//	void GetminLevel(TeamType type)
//	{
//		switch(type)
//		{
//		case TeamType.TT_CaoMoGu:
//			GlobalValue.Get(Constant.C_TeamMogu, out minsLevel);
//			break;
//		case TeamType.TT_Copy:
//			GlobalValue.Get(Constant.C_Copy, out minsLevel);
//			break;
//		case TeamType.TT_Daochang:
//			GlobalValue.Get(Constant.C_TeamBairen, out minsLevel);
//			break;
//		case TeamType.TT_Hero:
//			GlobalValue.Get(Constant.C_Hero, out minsLevel);
//			break;
//		case TeamType.TT_JJC:
//			GlobalValue.Get(Constant.C_TeamPVP, out minsLevel);
//			break;
//		case TeamType.TT_Pet:
//			break;
//		case TeamType.TT_ShuaGuai:
//			GlobalValue.Get(Constant.C_TeamOutSide, out minsLevel);
//			break;
//		case TeamType.TT_TongjiQuest:
//			GlobalValue.Get(Constant.C_TeamTongji, out minsLevel);
//			break;
//		case TeamType.TT_Zhanchang:
//			GlobalValue.Get(Constant.C_TeamXiji, out minsLevel);
//			break;
//		default:
//			minsLevel = 1;
//			break;
//			
//		}
//	}
//	void OnClicClose(ButtonScript obj, object args, int param1, int param2)
//	{
//		Hide ();
//	}
//	void OnClicCreate(ButtonScript obj, object args, int param1, int param2)
//	{
//		int tMinLevel = 0;
//		int tMaxLevel = 0;
//			COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
//			cti.type_ = typeTeam;
//			cti.name_ = TeamSystem._MyTeamInfo.name_;
//		cti.pwd_ = TeamSystem._MyTeamInfo.pwd_;
//			cti.maxMemberSize_ = 5;
//		if(TeamSystem.minLevel>TeamSystem.maxLevel)
//		{
//			tMinLevel = TeamSystem.maxLevel;
//			tMaxLevel = TeamSystem.minLevel;
//		}else
//		{
//			tMinLevel = TeamSystem.minLevel;
//			tMaxLevel = TeamSystem.maxLevel;
//		}
//		cti.minLevel_ = (ushort)tMinLevel;
//		cti.maxLevel_ = (ushort)tMaxLevel;
//		NetConnection.Instance.changeTeam (cti);
//	}
	//	public UILabel _CreateTitleLable;
	
	public UIScrollView leftScro;
	public UIScrollView rightScro;
	public GameObject item_target;
	public UIGrid grid_target;
	public GameObject item_leftlevel;
	public Transform grid_leftlevel;
	//public VIPPackageItemManager vipmanger;
	public GameObject item_rightlevel;
	public Transform grid_rightlevel;

	public UIButton CloseBtn;
	public UIButton CreateBtn;
	TeamTargetCell _tcell;
	UICenterOnChild centerLeft;
	UICenterOnChild centerright;
	List<GameObject> rightItems = new List<GameObject> ();
	
	List<GameObject> leftItems = new List<GameObject> ();
	
	int leftLevel=1;
	int rightLevel =0;
	float maxLevel = 0;
	TeamType typeTeam;
	bool isTarget;
	void Start () {
		//_CreateTitleLable.text = LanguageManager.instance.GetValue("Team_CreateTitle")
		//vipmanger = grid_leftlevel.GetComponent<VIPPackageItemManager> ();
		item_leftlevel.SetActive (false);
		item_rightlevel.SetActive (false);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClicClose, 0, 0);
		UIManager.SetButtonEventHandler(CreateBtn.gameObject, EnumButtonEvent.OnClick, OnClicCreate, 0, 0);
		item_target.SetActive (false);
		typeTeam = TeamType.TT_None;
		for (TeamType i = TeamType.TT_None; i < TeamType.TT_Max; ++i)
		{
			GameObject obj = GameObject.Instantiate(item_target) as GameObject;
			obj.SetActive(true);
			obj.transform.parent = grid_target.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.name = Enum.GetName(typeof(TeamType),i);
			UIManager.SetButtonEventHandler(obj, EnumButtonEvent.OnClick, OnClicTeamType,(int)Enum.Parse(typeof(TeamType),Enum.GetName(typeof(TeamType),i)), 0);
			obj.GetComponent<TeamTargetCell>().nameLabel.text = LanguageManager.instance.GetValue( Enum.GetName(typeof(TeamType),i));
			if(i== typeTeam)
			{
				_tcell = obj.GetComponent<TeamTargetCell>();
				obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(true);
			}else
			{
				obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(false);
			}
			
		}
		TeamSystem.OnChangeTeam += HideSelf;
		centerLeft = grid_leftlevel.GetComponent<UICenterOnChild> (); 
		centerLeft.onFinished += OnCenterLeftFinished;
		
		centerright = grid_rightlevel.GetComponent<UICenterOnChild> (); 
		centerright.onFinished += OnCenterrightFinished;
		
		UIScrollView sc = grid_leftlevel.GetComponentInParent<UIScrollView>();
		sc.onDragMoveed = scrollveiwMove;
		GlobalValue.Get (Constant.C_PlayerMaxLevel, out maxLevel);
		rightLevel = (int)maxLevel;
		//VIPPackageItemManager.ScrollviewEventOk += ScrollviewOk;
		rightScro.onDragFinished += ScrollviewOk;
		leftScro.onDragFinished += ScrollviewOk;
		InitLevelLeft (rightLevel-leftLevel+1);
		InitLevelRight (rightLevel);
	}
	void scrollveiwMove()
	{
		isTarget = false;
	}
	void HideSelf(COM_TeamInfo info)
	{
		Hide ();
	}
	void InitLevelLeft(int count)
	{
		clearLeftItem ();
		//		GameObject clone1 = GameObject.Instantiate(item_leftlevel)as GameObject;
		//		clone1.SetActive(true);
		//		clone1.transform.parent = grid_leftlevel;
		//		clone1.transform.localPosition = Vector2.zero;
		//		clone1.transform.localScale = Vector3.one;
		//		UILabel la1 = clone1.GetComponent<UILabel>();
		//		la1.text = "";
		//		leftItems.Add(clone1);
		for(int i=0;i<count;i++)
		{
			GameObject clone = GameObject.Instantiate(item_leftlevel)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid_leftlevel;
			clone.transform.localPosition = Vector2.zero;
			clone.transform.localScale = Vector3.one;
			UILabel la = clone.GetComponent<UILabel>();
			la.text = (leftLevel+i).ToString();
			leftItems.Add(clone);
			grid_leftlevel.GetComponent<UIGrid> ().repositionNow = true;
			
		}
		//grid_leftlevel.GetComponent<UIGrid> ().Reposition ();
	}
	void InitLevelRight(int count)
	{
		//		GameObject clone1 = GameObject.Instantiate(item_rightlevel)as GameObject;
		//		clone1.SetActive(true);
		//		clone1.transform.parent = grid_rightlevel;
		//		clone1.transform.localPosition = Vector2.zero;
		//		clone1.transform.localScale = Vector3.one;
		//		UILabel la1 = clone1.GetComponent<UILabel>();
		//		la1.text = "";
		
		for(int i=count;i>0;i--)
		{
			GameObject clone = GameObject.Instantiate(item_rightlevel)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid_rightlevel;
			clone.transform.localPosition = Vector2.zero;
			clone.transform.localScale = Vector3.one;
			UILabel la = clone.GetComponent<UILabel>();
			la.text = i.ToString();
			//			leftLevel = i;
		}
		grid_rightlevel.GetComponent<UIGrid> ().Reposition ();
	}
	void clearLeftItem()
	{
		for(int i=0;i<leftItems.Count;i++)
		{
			Destroy(leftItems[i]);
		}
	}
	void ScrollviewOk()
	{
		CreateBtn.isEnabled = false;
	}
	void OnCenterLeftFinished()
	{
		if(isTarget)return;
		CreateBtn.isEnabled = true;
		UILabel la = centerLeft.centeredObject.GetComponent<UILabel> ();
		leftLevel = int.Parse (la.text);
		
	}
	void OnCenterrightFinished()
	{
		CreateBtn.isEnabled = true;
		rightLevel = int.Parse (centerright.centeredObject.GetComponent<UILabel>().text);
		
	}
	int minsLevel;
	void OnClicTeamType(ButtonScript obj, object args, int param1, int param2)
	{
		minsLevel = 1;
		typeTeam = (TeamType)param1;
		
		GetminLevel (typeTeam);
		
		//GlobalValue.Get((Constant)param1, out minsLevel);
		if(typeTeam == TeamType.TT_None)
		{
			leftLevel=1;
		}else
		{
			leftLevel = minsLevel;
		}
		InitLevelLeft (rightLevel-leftLevel+1);
		TeamTargetCell tcell = obj.GetComponent<TeamTargetCell>();
		if(_tcell != null)
		{
			_tcell.iconSp.gameObject.SetActive(false);
		}
		_tcell = tcell;
		tcell.iconSp.gameObject.SetActive (true);
		UICenterOnIndex CenterOnIndex = grid_leftlevel.GetComponent<UICenterOnIndex>();
		CenterOnIndex.UpdateItemPosition (0);
		isTarget = true;

	}
	void GetminLevel(TeamType type)
	{
		switch(type)
		{
		case TeamType.TT_CaoMoGu:
			GlobalValue.Get(Constant.C_TeamMogu, out minsLevel);
			break;
		case TeamType.TT_Copy:
			GlobalValue.Get(Constant.C_Copy, out minsLevel);
			break;
		case TeamType.TT_Daochang:
			GlobalValue.Get(Constant.C_TeamBairen, out minsLevel);
			break;
		case TeamType.TT_Hero:
			GlobalValue.Get(Constant.C_Hero, out minsLevel);
			break;
		case TeamType.TT_JJC:
			GlobalValue.Get(Constant.C_TeamPVP, out minsLevel);
			break;
		case TeamType.TT_Pet:
			break;
		case TeamType.TT_ShuaGuai:
			GlobalValue.Get(Constant.C_TeamOutSide, out minsLevel);
			break;
		case TeamType.TT_TongjiQuest:
			GlobalValue.Get(Constant.C_TeamTongji, out minsLevel);
			break;
		case TeamType.TT_Zhanchang:
			GlobalValue.Get(Constant.C_TeamXiji, out minsLevel);
			break;
		default:
			minsLevel = 1;
			break;
			
		}
	}
	
	void OnClicClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClicCreate(ButtonScript obj, object args, int param1, int param2)
	{
		int tMinLevel = 0;
		int tMaxLevel = 0;
		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
		cti.type_ = typeTeam;
		cti.name_ = TeamSystem._MyTeamInfo.name_;
		cti.pwd_ = TeamSystem._MyTeamInfo.pwd_;
		cti.maxMemberSize_ = 5;



		if(leftLevel>rightLevel)
		{
//			tMinLevel =rightLevel;
//			tMaxLevel = leftLevel;
			PopText.Instance.Show(LanguageManager.instance.GetValue("shangxiandengji"));
			return;
		}else
		{
			tMinLevel =leftLevel;
			tMaxLevel = rightLevel;
		}
		cti.minLevel_ = (ushort)tMinLevel;
		cti.maxLevel_ = (ushort)tMaxLevel;
		NetConnection.Instance.changeTeam (cti);
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ChangetTeamPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ChangetTeamPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_ChangetTeamPanel);
	}
	void OnDestroy()
	{
		rightScro.onDragFinished -= ScrollviewOk;
		leftScro.onDragFinished -= ScrollviewOk;
		centerLeft.onFinished -= OnCenterLeftFinished;
		centerright.onFinished -= OnCenterrightFinished;
		TeamSystem.OnChangeTeam -= HideSelf;
	}

	public override void Destroyobj ()
	{
		
	}
}
