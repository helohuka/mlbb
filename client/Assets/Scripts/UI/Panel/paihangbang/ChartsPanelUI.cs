using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChartsPanelUI : UIBase {


	public UILabel _ChartsTitle;
	public UILabel _Fighting;
	public UILabel _LevelTitle;
	public UILabel _BabyLable;
	public UILabel _ChartsHB;
	public UILabel _ChartsJJ;

	public UIButton CloseBtn;
	public UIButton LevelBtn;
	public UIButton BabyBtn;
	public UIButton HuoBtn;
	public UIButton JingjiBtn;
	public UIButton ZhdouliBtn;
	public GameObject item;
	//public GameObject playerObj;
	public UILabel levelLabel;
	public UILabel nameLabel;
	public UILabel zhiyeLbel;
	public GameObject EmployeeTipsObj;

	//public Transform mpos;

//	public UILabel namelabel;
//	public UILabel zhiyeLabel;
	public UILabel numLabel;
	List<UIButton> btns = new List<UIButton> ();

	public delegate void RefreshLevel(uint num,COM_ContactInfo [] ContactInfo);
	public static RefreshLevel RefreshLevelOk;
	public delegate void RefreshJJ(uint num,COM_EndlessStair [] EndlessStair);
	public static RefreshJJ RefreshJJOk;

	public delegate void RefreshBabyRank(uint num,COM_BabyRankData [] BabyRankDates);
	public static RefreshBabyRank RefreshBabyRankOk;
	public delegate void RefreshEmpRank(uint num,COM_EmployeeRankData [] EmployeeRankDates);
	public static RefreshEmpRank RefreshEmpRankOk;

	public delegate void requestPlayerFFRank(uint num,COM_ContactInfo [] EmployeeRankDates);
	public static requestPlayerFFRank requestPlayerFFRankOk;

	private int type = 1;
	public UIGrid grid;
	void Start () {
		InitUIText ();
		item.SetActive (false);
		//btns.Add (ZhdouliBtn);
		btns.Add (LevelBtn);
		btns.Add (BabyBtn);
		btns.Add (HuoBtn);
		btns.Add (JingjiBtn);
		//namelabel.text = GamePlayer.Instance.InstName;
		levelLabel.text = LanguageManager.instance.GetValue("dengji");	
		//zhiyeLabel.text = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		//UIManager.SetButtonEventHandler (ZhdouliBtn.gameObject, EnumButtonEvent.OnClick, OnClickJingji, 0, 0);
		UIManager.SetButtonEventHandler (LevelBtn.gameObject, EnumButtonEvent.OnClick, OnClickJingji, 1, 0);
		UIManager.SetButtonEventHandler (BabyBtn.gameObject, EnumButtonEvent.OnClick, OnClickJingji, 2, 0);
		UIManager.SetButtonEventHandler (HuoBtn.gameObject, EnumButtonEvent.OnClick, OnClickJingji, 3, 0);
		UIManager.SetButtonEventHandler (JingjiBtn.gameObject, EnumButtonEvent.OnClick, OnClickJingji, 4, 0);

		//GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId], (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, AssetLoadCallBack,null);
		ButtonSelect(0);
		NetConnection.Instance.requestLevelRank ();
		RefreshLevelOk = LevelPaiOk;
		RefreshJJOk = JJPaiOk;
		RefreshBabyRankOk = BabyRankDateOK;
		RefreshEmpRankOk = EmployeeRankDateok;
		requestPlayerFFRankOk = RequestPlayerFFRankOk;
		ChartsSystem.QueryBabyEventOk += OnQueryBaby;
		ChartsSystem.queryEmployeeEventOk += OnqueryEmployee;
		ChartsSystem.QueryPlayerEventOk += OnQueryPlayer;
	}
	void InitUIText()
	{
		_ChartsTitle.text = LanguageManager.instance.GetValue("Charts_Title");
		_Fighting.text = LanguageManager.instance.GetValue("Charts_Fighting");
		_LevelTitle.text = LanguageManager.instance.GetValue("Charts_Level");
		_BabyLable.text = LanguageManager.instance.GetValue("Charts_Baby");
		_ChartsHB.text = LanguageManager.instance.GetValue("Charts_ChartsHB");
		_ChartsJJ.text = LanguageManager.instance.GetValue("Charts_ChartsJJ");
	}
	void OnQueryBaby(COM_BabyInst Inst)
	{
		ChatBabytips.ShowMe(Inst);
	}
	void OnqueryEmployee(COM_EmployeeInst Inst)
	{
		EmployeeTipsObj.SetActive (true);
		EmployeeTipsUI etps = EmployeeTipsObj.GetComponent<EmployeeTipsUI>();
		etps.SetEmployeeInst (Inst);
	}
	void OnQueryPlayer(COM_SimplePlayerInst Inst)
	{
		//playerObj.SetActive (true);
//		TeamPlayerInfo tinfo = playerObj.GetComponent<TeamPlayerInfo>();
//		tinfo.SetSimplePlayerInst (Inst);
		TeamPlayerInfo.ShowMe (Inst);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
		RefreshJJOk = null;
		RefreshLevelOk = null;
	}
	private void OnClickLevel(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.requestJJCRank ();
	}
	private void OnClickBaby(ButtonScript obj, object args, int param1, int param2)
	{

	}
	private void OnClickHuo(ButtonScript obj, object args, int param1, int param2)
	{

	}
	private void OnClickJingji(ButtonScript obj, object args, int param1, int param2)
	{
		type = param1;
		Refresh ();
//		if (param1 == 0)
//		{
//			levelLabel.text = LanguageManager.instance.GetValue("zhandouli");	
//			nameLabel.text = LanguageManager.instance.GetValue("jueseming");
//			zhiyeLbel.text = LanguageManager.instance.GetValue("zhiye");
//			NetConnection.Instance.requestPlayerFFRank ();
//		}else
		if(param1==1)
		{
			levelLabel.text =LanguageManager.instance.GetValue("dengji"); 	
			nameLabel.text =LanguageManager.instance.GetValue("jueseming");
			zhiyeLbel.text = LanguageManager.instance.GetValue("zhiye");
			NetConnection.Instance.requestLevelRank ();
		}else
			if(param1 ==2)
		{
			levelLabel.text = LanguageManager.instance.GetValue("zhandouli");		
			nameLabel.text = LanguageManager.instance.GetValue("chongwuming");
			zhiyeLbel.text = LanguageManager.instance.GetValue("yongyouzhe");
			NetConnection.Instance.requestBabyRank();

		}else
			if(param1 == 3)
		{
			nameLabel.text =LanguageManager.instance.GetValue("huobanming");
			levelLabel.text =LanguageManager.instance.GetValue("pingfen") ;
			zhiyeLbel.text =LanguageManager.instance.GetValue("yongyouzhe");
			NetConnection.Instance.requestEmpRank();

		}else
			if(param1 ==4)
		{
			nameLabel.text = LanguageManager.instance.GetValue("jueseming");
			levelLabel.text = LanguageManager.instance.GetValue("dengji"); 
			zhiyeLbel.text = LanguageManager.instance.GetValue("zhiye");
			NetConnection.Instance.requestJJCRank ();
		}
		ButtonSelect(param1-1);
	}
	void Refresh()
	{
		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			tr.gameObject.SetActive(false);
		}
	}
	void ButtonSelect(int index)
	{
		for(int i =0;i<btns.Count ;i++)
		{
			if(i == index)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}
	string[] numName = {"diyi","dier","disan"};
//	void AddItem(COM_EndlessStair[] EndlessStairs)
//	{
//		for (int i = 0; i<EndlessStairs.Length; i++) {
//			GameObject clone = GameObject.Instantiate (item)as GameObject;
//			clone.SetActive (true);
//			clone.transform.parent = grid.transform;
//			clone.transform.position = Vector3.zero;
//			clone.transform.localScale = Vector3.one;
//			ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
//			ccell.COM_Endless = EndlessStairs[i];
//			if(i<numName.Length)
//			{
//				ccell.paiSp.spriteName = numName[i];
//				ccell.paiSp.gameObject.SetActive(true);
//			}
//
//			grid.repositionNow = true;
//
//		}
//	}
	private List<GameObject> chatCellPool = new List<GameObject>();
	private List<GameObject> chatCellList = new List<GameObject>();

	void RefreshInfo(int count,COM_ContactInfo[] ContactInfo = null,COM_EndlessStair []EndlessStair = null,COM_BabyRankData []BabyRankDates = null,COM_EmployeeRankData[] EmployeeRankDates =null,COM_ContactInfo[]  ContactInfos = null)
	{
		for(int i=0;i<chatCellList.Count;i++)
		{
			chatCellList[i].transform.parent = null;
			chatCellList[i].SetActive(false);
			chatCellPool.Add(chatCellList[i]);
		}
		chatCellList.Clear ();
		for(int i = 0;i<count;i++)
		{
			if(i<100)
			{
				GameObject clone = null;
				if(chatCellPool.Count>0)
				{
					clone = chatCellPool[0];
					chatCellPool.Remove(clone);	
					
				}else
				{				
					clone = GameObject.Instantiate (item)as GameObject;
				}
				clone.transform.parent = grid.transform;
				clone.SetActive (true);
				clone.transform.position = Vector3.zero;
				clone.transform.localScale = Vector3.one;			
				chatCellList.Add(clone);
				UIManager.SetButtonEventHandler (clone.gameObject, EnumButtonEvent.OnClick, OnClickBtn,0, 0);
//				if(type == 0)
//				{
//
//					ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
//					ccell.COContactInfo = ContactInfos[i];
//					ccell.numLabel.text =  (i+1).ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());;
//					if(i<numName.Length)
//					{
//						ccell.paiSp.spriteName = numName[i];
//						ccell.paiSp.gameObject.SetActive(true);
//					}
//					clone.name = ContactInfos[i].name_;
//					UIManager.SetButtonParam(clone,0,0);
//					
//				}else
					if(type == 1)
				{

					ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
					ccell.ContactInfo = ContactInfo[i];
					ccell.numLabel.text =  (i+1).ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());;
					if(i<numName.Length)
					{
						ccell.paiSp.spriteName = numName[i];
						ccell.paiSp.gameObject.SetActive(true);
					}
					clone.name = ContactInfo[i].name_;
					UIManager.SetButtonParam(clone,0,0);
					//UIManager.SetButtonParam(clone,1,(int)ContactInfo[i].instId_);
				}else
					if(type == 2)
				{
		
					ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
					ccell.BabyRankDate = BabyRankDates[i];
					ccell.numLabel.text =  (i+1).ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());;
					if(i<numName.Length)
					{
						ccell.paiSp.spriteName = numName[i];
						ccell.paiSp.gameObject.SetActive(true);
					}
					UIManager.SetButtonParam(clone,2,(int)BabyRankDates[i].instId_);
					
				}if(type == 3)
				{

					ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
					ccell.EmployeeRankDate = EmployeeRankDates[i];
					ccell.numLabel.text =  (i+1).ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());;
					if(i<numName.Length)
					{
						ccell.paiSp.spriteName = numName[i];
						ccell.paiSp.gameObject.SetActive(true);
					}
					UIManager.SetButtonParam(clone,3,(int)EmployeeRankDates[i].instId_);
				}
				if(type == 4)
				{

					ChartsCell ccell = 	clone.GetComponent<ChartsCell>();
					ccell.COM_Endless = EndlessStair[i];
					ccell.numLabel.text =  (i+1).ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());;
					if(i<numName.Length)
					{
						ccell.paiSp.spriteName = numName[i];
						ccell.paiSp.gameObject.SetActive(true);
					}
					clone.name = EndlessStair[i].name_;
					UIManager.SetButtonParam(clone,4,0);
				}

			}
			grid.Reposition();
		}

	}

	void OnClickBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1==0)
		{
			NetConnection.Instance.queryPlayerbyName(obj.name);
		}else if(param1==1)
		{
			NetConnection.Instance.queryPlayerbyName(obj.name);
		}else if(param1==2)
		{
			NetConnection.Instance.queryBaby((uint)param2);	
		}else if(param1==3)
		{
			NetConnection.Instance.queryEmployee((uint)param2);
		}else if(param1==4)
		{
			NetConnection.Instance.queryPlayerbyName(obj.name);
		}
	}
//	void AssetLoadCallBack(GameObject ro, ParamData data)
//	{
//
//		NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
//		ro.transform.parent = mpos;
//		ro.transform.localPosition = Vector3.zero;
//		ro.transform.localRotation = new Quaternion(ro.transform.localRotation.x, -180, ro.transform.localRotation.z, ro.transform.localRotation.w);
//		ro.transform.localScale = new Vector3(ro.transform.localScale.x / 2, ro.transform.localScale.y / 2, ro.transform.localScale.z / 2);
//	}

	void LevelPaiOk(uint num,COM_ContactInfo [] ContactInfo)
	{
		List<COM_ContactInfo> ContactInfoL = new List<COM_ContactInfo> ();
		if(num !=0)
		{
			numLabel.text = num.ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());
		}else
		{
			numLabel.text = LanguageManager.instance.GetValue("paiming");
		}
		ContactInfoL.AddRange (ContactInfo);
		ContactInfoL.Sort (SortContactInfo);
		RefreshInfo (ContactInfo.Length,ContactInfoL.ToArray());
	}
	void JJPaiOk(uint num,COM_EndlessStair [] EndlessStair)
	{
		List<COM_EndlessStair> EndlessStairL = new List<COM_EndlessStair> ();
		if(num !=0)
		{
			numLabel.text = num.ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());
		}else
		{
			numLabel.text = LanguageManager.instance.GetValue("paiming");
		}
		EndlessStairL.AddRange (EndlessStair);
		EndlessStairL.Sort (SortEndlessStair);
		RefreshInfo (EndlessStair.Length,null,EndlessStairL.ToArray());
	}
	void BabyRankDateOK(uint num,COM_BabyRankData [] BabyRankDateS)
	{
		List<COM_BabyRankData> BabyRankDate = new List<COM_BabyRankData> ();
		if(num !=0)
		{
			numLabel.text = num.ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());
		}else
		{
			numLabel.text = LanguageManager.instance.GetValue("paiming");
		}
		BabyRankDate.AddRange (BabyRankDateS);
		//BabyRankDate.Sort (SortBabyRank);

		RefreshInfo (BabyRankDateS.Length,null,null,BabyRankDate.ToArray());
	}
	void EmployeeRankDateok(uint num,COM_EmployeeRankData [] EmployeeRankDateS)
	{
		List<COM_EmployeeRankData> EmployeeRankDateL = new List<COM_EmployeeRankData> ();
		if(num !=0)
		{
			numLabel.text = num.ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());
		}else
		{
			numLabel.text = LanguageManager.instance.GetValue("paiming");
		}
		EmployeeRankDateL.AddRange (EmployeeRankDateS);
		EmployeeRankDateL.Sort (SortEmployeeRankDate);
		//AddItem (EndlessStair);
		RefreshInfo (EmployeeRankDateS.Length,null,null,null,EmployeeRankDateL.ToArray());
	}
	void RequestPlayerFFRankOk(uint num,COM_ContactInfo [] ContactInfos)
	{
		List<COM_ContactInfo> ContactInfoL = new List<COM_ContactInfo> ();
		if(num !=0)
		{
			numLabel.text = num.ToString();//LanguageManager.instance.GetValue("paimingnum").Replace("{n}",(i+1).ToString());
		}else
		{
			numLabel.text = LanguageManager.instance.GetValue("paiming");
		}
		ContactInfoL.AddRange (ContactInfos);
		ContactInfoL.Sort (SortContactInfo);
		RefreshInfo (ContactInfos.Length,null,null,null,null,ContactInfoL.ToArray());
	}

	private int SortBabyRank(COM_BabyRankData BabyRank,COM_BabyRankData BabyRank1)
	{
		if(BabyRank.rank_>BabyRank1.rank_)
		{
			return 1;
		}else  if(BabyRank.rank_<BabyRank.rank_)
		{
			return -1;
		}else
		{
			return 0;
		}
	}
	private int SortContactInfo(COM_ContactInfo Info,COM_ContactInfo Info1)
	{
		if(Info.rank_>Info1.rank_)
		{
			return 1;
		}else
		{
			return -1;
		}
	}
	private int SortEmployeeRankDate(COM_EmployeeRankData EmployeeRankDate,COM_EmployeeRankData EmployeeRankDate1)
	{
		if(EmployeeRankDate.rank_>EmployeeRankDate1.rank_)
		{
			return 1;
		}else if(EmployeeRankDate.rank_<EmployeeRankDate1.rank_)
		{
			return -1;
		}else
		{
			return 0;
		}
	}
	private int SortEndlessStair(COM_EndlessStair EndlessStair,COM_EndlessStair EndlessStair1)
	{
		if(EndlessStair.rank_>EndlessStair1.rank_)
		{
			return 1;
		}else
		{
			return -1;
		}
	}
//	private int SortBabyRank(COM_BabyRankData BabyRank,COM_BabyRankData BabyRank1)
//	{
//		if(BabyRank.rank_>BabyRank1.rank_)
//		{
//			return 1;
//		}else
//		{
//			return -1;
//		}
//	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ChartsPanelUI);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ChartsPanelUI);
	}
	public override void Destroyobj ()
	{
        for(int i=0; i < chatCellPool.Count; ++i)
        {
            Destroy(chatCellPool[i]);
        }

        UIManager.RemoveButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(ZhdouliBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(LevelBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(BabyBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(HuoBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(JingjiBtn.gameObject, EnumButtonEvent.OnClick);

        RefreshLevelOk = null;
        RefreshJJOk = null;
        RefreshBabyRankOk = null;
        RefreshEmpRankOk = null;
        requestPlayerFFRankOk = null;

		ChartsSystem.QueryBabyEventOk -= OnQueryBaby;
		ChartsSystem.queryEmployeeEventOk -= OnqueryEmployee;
		ChartsSystem.QueryPlayerEventOk -= OnQueryPlayer;
	}
}
