using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class CreateTeamUIPanel : UIBase {




	public UILabel _CreateTitleLable;
	public UILabel _TargetLable;
	public UILabel _TeameNameLable;
	public UILabel _LevelLable;
	public UILabel _EnterTitleLable;

	public UIScrollView leftScro;
	public UIScrollView rightScro;
    public GameObject item_target;
	public UIGrid grid_target;
	public GameObject item_leftlevel;
	public Transform grid_leftlevel;
	//public VIPPackageItemManager vipmanger;
	public GameObject item_rightlevel;
	public Transform grid_rightlevel;

	public UIInput nameInput;
	public UIInput passWordInput;
	public UIButton CloseBtn;
	public UIButton CreateBtn;
	TeamTargetCell _tcell;
	UICenterOnChild centerLeft;
	UICenterOnChild centerright;
	List<GameObject> rightItems = new List<GameObject> ();

	List<GameObject> leftItems = new List<GameObject> ();
	List<GameObject> targetObj = new List<GameObject> ();
	int leftLevel=1;
	int rightLevelm =0;
	float maxLevel = 0;
	TeamType typeTeam;
	bool isTarget;
	void Start () {

		_CreateTitleLable.text = LanguageManager.instance.GetValue("Team_CreateTitle");
		_TargetLable.text = LanguageManager.instance.GetValue("Team_Target");
		_TeameNameLable.text = LanguageManager.instance.GetValue("Team_TeameName");
		_LevelLable.text = LanguageManager.instance.GetValue("Team_Level");
		_EnterTitleLable.text = LanguageManager.instance.GetValue("Team_EnterTitle");
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
				obj.GetComponent<TeamTargetCell>().nameSp.gameObject.SetActive(true);
			}else
			{
				obj.GetComponent<TeamTargetCell>().iconSp.gameObject.SetActive(false);
				obj.GetComponent<TeamTargetCell>().nameSp.gameObject.SetActive(false);
			}
			targetObj.Add(obj);
		}

	    centerLeft = grid_leftlevel.GetComponent<UICenterOnChild> (); 
		centerLeft.onFinished += OnCenterLeftFinished;
		sbar.enabled = false;
	    centerright = grid_rightlevel.GetComponent<UICenterOnChild> (); 
		centerright.onFinished += OnCenterrightFinished;

		UIScrollView sc = grid_leftlevel.GetComponentInParent<UIScrollView>();
		sc.onDragMoveed = scrollveiwMove;

		UIScrollView scr = grid_leftlevel.GetComponentInParent<UIScrollView>();
		sc.onDragMoveed = scrollveiwMover;
		GlobalValue.Get(Constant.C_PlayerMaxLevel, out maxLevel);
		rightLevelm = (int)maxLevel;
		Debug.Log ("aaaaamaxLevel"+maxLevel);
		//VIPPackageItemManager.ScrollviewEventOk += ScrollviewOk;
		rightScro.onDragFinished += ScrollviewOk;
		leftScro.onDragFinished += ScrollviewOk;
		GetminLevel (TeamType.TT_None);
		InitLevelLeft (rightLevelm-leftLevel+1);
		InitLevelRight (rightLevelm);
    }
	void SetBtnEnable(bool isEnabled)
	{
		for(int i=0;i<targetObj.Count;i++)
		{
			UIButton bt = targetObj[i].GetComponent<UIButton>();
			bt.isEnabled = isEnabled;
		}
	}
	void scrollveiwMove()
	{
		isTarget = false;
	}
	void scrollveiwMover()
	{
		sbar.enabled = true;
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
		SetBtnEnable (true);
		if(isTarget)return;
		CreateBtn.isEnabled = true;
		UILabel la = centerLeft.centeredObject.GetComponent<UILabel> ();
		leftLevel = int.Parse (la.text);


	}
	public UIScrollBar sbar;
	void OnCenterrightFinished()
	{

		CreateBtn.isEnabled = true;
		if(sbar.value == 0)
		{
			rightLevelm = (int)maxLevel;
		}else
		{
			rightLevelm = int.Parse (centerright.centeredObject.GetComponent<UILabel>().text);
		}
		sbar.enabled = false;

	}
	int minsLevel;
	void OnClicTeamType(ButtonScript obj, object args, int param1, int param2)
	{
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


		TeamTargetCell tcell = obj.GetComponent<TeamTargetCell>();
		if(_tcell != null)
		{
			_tcell.iconSp.gameObject.SetActive(false);
			_tcell.nameSp.gameObject.SetActive(false);
		}
		_tcell = tcell;
		tcell.iconSp.gameObject.SetActive (true);
		_tcell.nameSp.gameObject.SetActive(true);

		GlobalInstanceFunction.Instance.Invoke (()=>{
			InitLevelLeft (rightLevelm-leftLevel+1);
			UICenterOnIndex CenterOnIndex = grid_leftlevel.GetComponent<UICenterOnIndex>();
			CenterOnIndex.UpdateItemPosition (0);
			isTarget = true;
		},1);
		SetBtnEnable (false);
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
//		Regex reg = new Regex("^[A-Za-z0-9]{0,6}$");
//		if (!reg.IsMatch(passWordInput.value))
//		{
//			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("mimaJia"));
//			PopText.Instance.Show(LanguageManager.instance.GetValue("mimaJia"));
//		}
//		Regex reg1 = new Regex("^[A-Za-z0-9]{0,6}$");
//		if(!reg1.IsMatch(nameInput.value))
//		{
//			ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("mimaJia"));
//		}
//		else
//		{
		if(nameInput.text.Equals(""))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwumingbuweikong"));
			return;
		}
			int tMinLevel = 0;
			int tMaxLevel = 0;
            COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
			cti.type_ = typeTeam;
			cti.name_ = nameInput.text;
            cti.pwd_ = passWordInput.text;
			cti.maxMemberSize_ = 5;
		   if(leftLevel>rightLevelm)
			{
			tMinLevel =rightLevelm;
				tMaxLevel = leftLevel;
			}else
			{
				tMinLevel =leftLevel;
			tMaxLevel = rightLevelm;
			}
			cti.minLevel_ = (ushort)tMinLevel;
			cti.maxLevel_ = (ushort)tMaxLevel;
            NetConnection.Instance.createTeam(cti);
			NetConnection.Instance.exitLobby();
			//TeamUIPanel.Instance.ClearRosObj();
//		}

	}
	void OnDestroy()
	{
		rightScro.onDragFinished -= ScrollviewOk;
		leftScro.onDragFinished -= ScrollviewOk;
		centerLeft.onFinished -= OnCenterLeftFinished;
		centerright.onFinished -= OnCenterrightFinished;
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_CreateTeamPanel);
	}
	public static void SwithShowMe()
	{

		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_CreateTeamPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_CreateTeamPanel);
	}

	public override void Destroyobj ()
	{

	}
}
