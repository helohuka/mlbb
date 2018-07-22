using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartnerPostion : MonoBehaviour {

	public GameObject item;
	public GameObject biaoqian;
	public Transform []posArr;
	public Transform TopLeft;
	public Transform BottomRight;
	public UIGrid grid;
	private UIEventListener Listener;
	private List<Employee> Employs;
	public List<UILabel> drawImg = new List<UILabel> ();
	public List<UILabel> openLevelLab = new List<UILabel> ();
	public UILabel battleNumLab;
	public UIButton EmpListPkBtn;
	public UIButton EmpListMxBtn;

	private List<GameObject> posCellList = new List<GameObject> ();
	private List<GameObject> emplyoeeCellList = new List<GameObject>();  
	private List<GameObject> emplyoeeCellPool = new List<GameObject>();
	public bool _isUpdataBattleList ;


	public UILabel employeeFightTextLab;
	public UILabel team1Lab;
	public UILabel team2Lab;
	public UILabel levelopen5Lab;
	public UILabel levelopen10Lab;
	public UILabel levelopen15Lab;
	public UILabel levelopen20Lab;
	public UILabel pleaseClick5Lab;
	public UILabel pleaseClick10Lab;
	public UILabel pleaseClick15Lab;
	public UILabel pleaseClick20Lab;

	private Employee _employee;
    private Rect[] cellRange_;
	private List<string> _icons = new List<string>();
    bool isPress_ = false;
    bool isMove_ = false;

    Vector3 preMousePos_ = Vector3.one;
	private int _conBattleNum =1;
	private int _selectEmpGroup;
	void Start () 
	{
		GamePlayer.Instance.UpdateEmployeeEnvent += new  EmpOnBattleEvent(OnUpdateBattle);
		employeeFightTextLab.text = LanguageManager.instance.GetValue("employeeFightTextLab");
		team1Lab.text = LanguageManager.instance.GetValue("team1Lab");
		team2Lab.text = LanguageManager.instance.GetValue("team2Lab");
		levelopen5Lab.text = LanguageManager.instance.GetValue("levelopen5Lab");
		levelopen10Lab.text = LanguageManager.instance.GetValue("levelopen10Lab");
		levelopen15Lab.text = LanguageManager.instance.GetValue("levelopen15Lab");
		levelopen20Lab.text = LanguageManager.instance.GetValue("levelopen20Lab");
		pleaseClick5Lab.text = LanguageManager.instance.GetValue("pleaseClick5Lab");
		pleaseClick10Lab.text = LanguageManager.instance.GetValue("pleaseClick10Lab");
		pleaseClick15Lab.text = LanguageManager.instance.GetValue("pleaseClick15Lab");
		pleaseClick20Lab.text = LanguageManager.instance.GetValue("pleaseClick20Lab");

		UIManager.SetButtonEventHandler (EmpListPkBtn.gameObject, EnumButtonEvent.OnClick, OnClickEmpListGroup, (int)EmployeesBattleGroup.EBG_GroupOne, 0);
		UIManager.SetButtonEventHandler (EmpListMxBtn.gameObject, EnumButtonEvent.OnClick, OnClickEmpListGroup, (int)EmployeesBattleGroup.EBG_GroupTwo, 0);
		_selectEmpGroup = (int)GamePlayer.Instance.CurEmployeesBattleGroup;

		if(_selectEmpGroup == (int)EmployeesBattleGroup.EBG_GroupOne)
			EmpListPkBtn.isEnabled = false;
		else if(_selectEmpGroup == (int)EmployeesBattleGroup.EBG_GroupTwo)
			EmpListMxBtn.isEnabled = false;
        //GamePlayer.Instance.EmployeeStarOkEnvent += new RequestEventHandler<COM_EmployeeInst> (OnEmployeeStarOk);
		UpdataPanel ();
        cellRange_ = new Rect[4];
        UISprite sprite = null;
        for (int i = 0; i < posArr.Length; ++i)
        {
            sprite = posArr[i].GetComponent<UISprite>();
            float w = sprite.width;
            float h = sprite.height;
            cellRange_[i] = new Rect(posArr[i].localPosition.x - w / 2f, posArr[i].localPosition.y - h / 2f, w, h);
       }

		int oLevel = 9;
		drawImg[0].gameObject.SetActive(true);
		openLevelLab[0].gameObject.SetActive(false);
		for(int i= 1;i<4;i++)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= oLevel)
			{
				drawImg[i].gameObject.SetActive(true);
				openLevelLab[i].gameObject.SetActive(false);
				_conBattleNum ++;
			}
			else
			{
				drawImg[i].gameObject.SetActive(false);
				openLevelLab[i].gameObject.SetActive(true);
			}
			oLevel += 3;
		}


	}

	public void UpdataPanel()
	{
		//item.SetActive (false);
		uint[] empGroup = GamePlayer.Instance.GetEmployeesBattles((int)GamePlayer.Instance.CurEmployeesBattleGroup);
		Employs = GamePlayer.Instance.EmployeeList;
		AddScrollViewItem (Employs);
		//_isUpdataBattleList = false;
		UpdataBattleEmployee (_selectEmpGroup);//(int)GamePlayer.Instance.CurEmployeesBattleGroup);
		int num = 0;
	//	List<Employee> eList = GamePlayer.Instance.GetBattleEmployees ();
		for(int i =0;i<empGroup.Length;i++)
		{
			if(empGroup[i] == 0)
				continue;

			num+= (GamePlayer.Instance.GetEmployeeById((int)empGroup[i])).GetIprop(PropertyType.PT_FightingForce);
		}
		battleNumLab.text = num.ToString ();
	}

	public void delUpdateEmployee()
	{
		Employs = GamePlayer.Instance.EmployeeList;
		AddScrollViewItem (Employs);
	}

	void AddScrollViewItem(List<Employee> Edata)
	{
		if (item == null)
			return;

		if(emplyoeeCellList.Count>0)
		{
			foreach(GameObject o in emplyoeeCellList)
			{
				o.transform.parent = null;
				emplyoeeCellPool.Add(o);
				o.gameObject.SetActive(false);
			}
			emplyoeeCellList.Clear();
		}

        GameObject buddy = null;
        bool final = false;

		List<Employee> battles = GamePlayer.Instance.GetBattleEmployees ();


		for (int i = 0; i<battles.Count; i++) 
		{
			GameObject o = null;
			if(emplyoeeCellPool.Count>0) 
			{
				o = emplyoeeCellPool[0];        
				emplyoeeCellPool.Remove(o);
			}
			else   
			{
				o  = Instantiate(item)as GameObject;
			}
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = battles[i];
			
			if(cell.Employee.isForBattle_)
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(true);//.spriteName = "hb_renwukuangxuanzhong";
			}
			else
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(false);
				//cell.backImg.spriteName = "hb_renwukuang";
			}
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)battles[i].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)battles[i].quality_);
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)battles[i].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)battles[i].quality_);
			cell.professionImg.spriteName = ((JobType)battles[i].GetIprop(PropertyType.PT_Profession)).ToString();
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(battles[i].quality_.ToString());
			cell.fightingNumLab.text  =battles[i].GetIprop(PropertyType.PT_FightingForce).ToString();
			/*for(int c =0 ;c<cell.starList.Count;c++)
			{
				cell.starList[c].gameObject.SetActive(false);
			}
			for(int j =0 ;j<battles[i].star_ && j<5;j++)
			{
				cell.starList[j].gameObject.SetActive(true);
			}
		*/
			for(int c =0;c<cell.starList.Count;c++)
			{
				cell.starList[c].gameObject.SetActive(false);
			}
			int len = (int)battles[i].star_;
			if(battles[i].star_ >=6)
			{
				len  = (int)battles[i].star_- 5;
				for(int j =0;j<len && j<5;j++)
				{
					cell.starList[j].spriteName = "zixingxing";
					cell.starList[j].gameObject.SetActive(true);
				}
			}
			else
			{
				for(int j =0;j<battles[i].star_ && j<5;j++)
				{
					cell.starList[j].spriteName = "xingxing";
					cell.starList[j].gameObject.SetActive(true);
				}
			}


			
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,cell.icon);
			
			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
			}
			
			UIManager.SetButtonEventHandler(o,EnumButtonEvent.OnClick,OnEmployeeCell,0,0);
			// UIEventListener.Get(o).onPress = OnPressEmp;
			//UIEventListener.Get(o).onDrop = OnDropEmp;
			o.transform.parent = grid.transform; 
			o.SetActive(true);
			emplyoeeCellList.Add(o);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			//cell.UpdateRed();
			//if (i == 0)
			//	buddy = o;
			
			//if (battles[i].isForBattle_ && GamePlayer.Instance.GetEmployeeIsBattle(Edata[i].InstId, _selectEmpGroup) && final == false)
			//{
				//buddy = o;
				//final = true;
			//}
		}
	
		for (int i = 0; i<Edata.Count; i++) 
		{
			if(battles.Contains(Edata[i]))
			{
				continue;
			}
			GameObject o = null;
			if(emplyoeeCellPool.Count>0) 
			{
				o = emplyoeeCellPool[0];        
				emplyoeeCellPool.Remove(o);
			}
			else   
			{
				 o  = Instantiate(item)as GameObject;
			}
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = Edata[i];

			if(cell.Employee.isForBattle_)
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(true);//.spriteName = "hb_renwukuangxuanzhong";
			}
			else
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(false);
				//cell.backImg.spriteName = "hb_renwukuang";
			}
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)Edata[i].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)Edata[i].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)Edata[i].quality_);
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)Edata[i].quality_);
			cell.GetComponent<EmployeeCellUI>().fightingNumLab.text = (Edata[i].GetIprop(PropertyType.PT_FightingForce)).ToString();
			cell.professionImg.spriteName = ((JobType)Edata[i].GetIprop(PropertyType.PT_Profession)).ToString();
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(Edata[i].quality_.ToString());
			cell.fightingNumLab.text  =Edata[i].GetIprop(PropertyType.PT_FightingForce).ToString();
			/*for(int c =0 ;c<cell.starList.Count;c++)
			{
				cell.starList[c].gameObject.SetActive(false);
			}
			for(int j =0 ;j<Edata[i].star_ && j<5;j++)
			{
				cell.starList[j].gameObject.SetActive(true);
			}
			*/
			for(int c =0;c<cell.starList.Count;c++)
			{
				cell.starList[c].gameObject.SetActive(false);
			}
			int len = (int)Edata[i].star_;
			if(Edata[i].star_ >=6)
			{
				len  = (int)Edata[i].star_- 5;
				for(int j =0;j<len && j<5;j++)
				{
					cell.starList[j].spriteName = "zixingxing";
					cell.starList[j].gameObject.SetActive(true);
				}
			}
			else
			{
				for(int j =0;j<Edata[i].star_ && j<5;j++)
				{
					cell.starList[j].spriteName = "xingxing";
					cell.starList[j].gameObject.SetActive(true);
				}
			}


			
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(Edata[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,cell.icon);

			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(Edata[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(Edata[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
			}

			UIManager.SetButtonEventHandler(o,EnumButtonEvent.OnClick,OnEmployeeCell,0,0);
           // UIEventListener.Get(o).onPress = OnPressEmp;
            //UIEventListener.Get(o).onDrop = OnDropEmp;
			o.transform.parent = grid.transform; 
			o.SetActive(true);
			emplyoeeCellList.Add(o);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			//cell.UpdateRed();
			if (cell.Employee.isForBattle_ == false && buddy == null)
                buddy = o;

          //  if (!Edata[i].isForBattle_  && final == false)
           // {
              //  buddy = o;
             //   final = true;
           // }
		}
		grid.Reposition();
        if(buddy != null)
            GuideManager.Instance.RegistGuideAim(buddy, GuideAimType.GAT_FirstPartner_PosUI);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerPositionTabShow);
	}
	//更新已上阵伙伴.
	public void UpdataBattleEmployee(int empGroup)
	{
		if (_isUpdataBattleList)
				return;

		for(int i=0;i< posCellList.Count;i++)
		{
			GameObject.DestroyObject(posCellList[i]);
			posCellList[i] = null;
		}
		posCellList.Clear();
		


		Employs = GamePlayer.Instance.EmployeeList;
		uint[] empGroups = GamePlayer.Instance.GetEmployeesBattles (empGroup);
	
		//int index = 0;
		List<Employee> emps = new List<Employee> ();// GamePlayer.Instance.GetBattleEmployees ();
		int num = 0;
		for(int g=0;g<empGroups.Length;g++)
		{
			if(empGroups[g] == 0)
				continue;
			Employee emp = GamePlayer.Instance.GetEmployeeById((int)empGroups[g]);
			if(emp != null)
			{
				emps.Add(emp);
				num += emp.GetIprop(PropertyType.PT_FightingForce);
			}
		}
		battleNumLab.text = num.ToString ();

		for(int i=0;i<emps.Count;i++)
		{
			GameObject clone = GameObject.Instantiate (biaoqian)as GameObject;
			clone.GetComponent<EmployeeCellUI>().nameLab.text = emps[i].InstName;
			clone.SetActive (true);
			//UIManager.SetButtonEventHandler(clone,EnumButtonEvent.OnClick,OnShowEmployee,i,0);
			UIManager.SetButtonEventHandler(clone.transform.FindChild("down").gameObject,EnumButtonEvent.OnClick,OnDownBattleEmployee,i,0);

			clone.GetComponent<EmployeeCellUI>().Employee =emps[i];
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(emps[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,clone.GetComponent<EmployeeCellUI>().icon);
			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(emps[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(emps[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
			}
			clone.GetComponent<EmployeeCellUI>().qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)emps[i].quality_);
			clone.GetComponent<EmployeeCellUI>().pinzhi.spriteName = EmployessSystem.instance.GetQualityBack((int)emps[i].quality_);
			clone.GetComponent<EmployeeCellUI>().backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)emps[i].quality_);
			clone.GetComponent<EmployeeCellUI>().backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)emps[i].quality_);
			clone.GetComponent<EmployeeCellUI>().qAddImg.spriteName = LanguageManager.instance.GetValue(emps[i].quality_.ToString());
			clone.GetComponent<EmployeeCellUI>().professionImg.spriteName = ((JobType)emps[i].GetIprop(PropertyType.PT_Profession)).ToString();
			clone.GetComponent<EmployeeCellUI>().fightingNumLab.text = (emps[i].GetIprop(PropertyType.PT_FightingForce)).ToString();

			clone.transform.parent = posArr[i];
			clone.transform.localPosition = Vector3.zero;
            clone.transform.localScale = Vector3.one;
			posCellList.Add(clone);
			//GameManager.Instance.GetActorClone((ENTITY_ID)EmployeeData.GetData(emps[i].GetIprop(PropertyType.PT_TableId)).asset_id,
                                            //   (ENTITY_ID)emps[i].WeaponAssetID, BatchLoadAsset, new ParamData(emps[i].InstId), "UI");
		}

	
		/*
		for(int i =0;i<drawImg.Count;i++)
		{
			drawImg[i].gameObject.SetActive(false);
		}

		if(emps.Count < 4)
		{
			for(int j = 0;j <emps.Count +1;j++)
			{
				drawImg[j].gameObject.SetActive(true);
			}
		}
		*/	
		UpdateBattleBack ();
	}
	


    GameObject crtSelectOrigin = null;
    GameObject crtSelect = null;
    Vector3 crtMouseUIPos = Vector3.zero;

    void BeginEmp()
    {
        if (isPress_ && isMove_)
        {
            if (crtSelect != null)
                return;
            if (GamePlayer.Instance.GetEmployeeIsBattle(crtSelectOrigin.GetComponent<EmployeeCellUI>().Employee.InstId))
                return;
            crtSelect = GameObject.Instantiate(crtSelectOrigin) as GameObject;
            crtSelect.transform.parent = transform;
            crtSelect.transform.localScale = Vector3.one;
            EmployeeCellUI cell = crtSelectOrigin.GetComponent<EmployeeCellUI>();
            if (cell == null || cell.Employee == null)
            {
                return;
            }
            //  _employee = cell.Employee;
            _employee = GamePlayer.Instance.GetEmployeeById(cell.Employee.InstId);
            EmployessSystem.instance.CurEmployee = _employee;
        }
    }
    void OnPressEmp(GameObject go, bool isPress)
    {
        crtSelectOrigin = go;
        if (!isPress)
        {

			if (crtSelect == null)
			{
				return;
			}
            if (crtSelect != null)
            {
				GameObject.Destroy(crtSelect);
				crtSelect = null;
            }
            if (InCellRange(crtMouseUIPos))
			{
				if(_employee == null)
				{
					isPress_ = isPress;
					return;
				}
				if(GamePlayer.Instance.GetEmployeeIsBattle(_employee.InstId))
				{
					isPress_ = isPress;
					return;
				}
				if(GetSameName(_employee.InstName))
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("samehuoban"));
					isPress_ = isPress;
					return;	
				}

                int instId = _employee.InstId;
                int assid = _employee.GetIprop(PropertyType.PT_AssetId);

				if(GamePlayer.Instance.GetBattleEmployees().Count>= _conBattleNum)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("shangzhenyiman"));
					isPress_ = isPress;
					return;
				}
				NetConnection.Instance.setBattleEmp((uint)instId,(EmployeesBattleGroup)_selectEmpGroup, true);
                NetWaitUI.ShowMe();
            }
        }
        isPress_ = isPress;
    }

    bool InCellRange(Vector3 mouseUIPos)
    {
        for (int i = 0; i < cellRange_.Length; ++i)
        {
            if (mouseUIPos.x > cellRange_[i].xMin &&
                mouseUIPos.x < cellRange_[i].xMax &&
                mouseUIPos.y > cellRange_[i].yMin &&
                mouseUIPos.y < cellRange_[i].yMax)
                return true;
        }
        return false;
    }

    void OnDropEmp(GameObject go, GameObject aim)
    {
        ClientLog.Instance.Log("sdf");
    }

	void OnDoubleClickEmployee(GameObject obj)
	{
		bDouble = true;
		StopCoroutine ("DelayOneClick");
		if(GamePlayer.Instance.GetEmployeeIsBattle(_employee.InstId))
		{
			return;
		}
		if (GamePlayer.Instance.GetBattleEmployees ().Count >= _conBattleNum)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shangzhenyiman"));
			return;
		}
		if(GetSameName(_employee.InstName))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("samehuoban"));
			return;	
		}

		int instId = _employee.InstId;
		int assid = _employee.GetIprop(PropertyType.PT_AssetId);
		NetConnection.Instance.setBattleEmp((uint)instId,(EmployeesBattleGroup)_selectEmpGroup,true);
        NetWaitUI.ShowMe();
	}

	private void OnEmployeeCell(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeCellUI cell = obj.GetComponent<EmployeeCellUI> ();
		if(cell == null || cell.Employee == null)
		{
			return;
		}

		uint[] emps = GamePlayer.Instance.GetEmployeesBattles (_selectEmpGroup);
		int hNum = 0;
		for(int i =0;i<emps.Length;i++ )
		{
			if(emps[i] != 0)
			{
				hNum++;
			}
		}
		if(hNum >=_conBattleNum)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shangzhenyiman"));
			return;
		}
		if(GetSameName(cell.Employee.InstName))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("samehuoban"));
			return;	
		}
		if( EmployeeTaskSystem.instance.IsTaskEmp(cell.Employee.InstId))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("huanbanrenwuzhong"));
			return;	
		}
		_employee = GamePlayer.Instance.GetEmployeeById(cell.Employee.InstId);
		EmployessSystem.instance.CurEmployee = _employee;

		NetConnection.Instance.setBattleEmp((uint)_employee.InstId,(EmployeesBattleGroup)_selectEmpGroup,true);
        NetWaitUI.ShowMe();

		//_employee = cell.Employee;
		//if (_employee == null)
		//		return;
		//_employee = GamePlayer.Instance.GetEmployeeById (_employee.InstId);
		//EmployessSystem.instance.CurEmployee = _employee;
		//bDouble = false;
	//	EmployeeListUI.ShowMe ();
		//StartCoroutine (DelayOneClick ());
	}
	bool bDouble = false;
	IEnumerator DelayOneClick()
	{
		yield return new WaitForSeconds(0.3f);
		if(!bDouble)
		{
			EmployeeListUI.ShowMe ();
		}
	}
	


	private void OnDownBattleEmployee(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeCellUI cell = obj.transform.parent.GetComponent<EmployeeCellUI> ();
		if(cell == null)
		{
			return;
		}
		posCellList[param1].gameObject.SetActive(false);
		
		NetConnection.Instance.setBattleEmp((uint)cell.Employee.InstId,(EmployeesBattleGroup)_selectEmpGroup,false);
        NetWaitUI.ShowMe();
	}

	void OnEmployeeStarOk(Employee inst)
	{
        //for(int i= 0;i<emplyoeeCellList.Count;i++)
        //{
        //    if(emplyoeeCellList[i].GetComponent<EmployeeCellUI>().Employee.InstId == inst.instId_)
        //    {
        //        Employee emp = new Employee();
        //        emp.SetEntity(inst);
        //        emplyoeeCellList[i].GetComponent<EmployeeCellUI>().Employee = emp;
        //    }
        //}
	}

	public void OnUpdateBattle(Employee inst,int group)
	{
        NetWaitUI.HideMe();
		if(inst.isForBattle_ && group == _selectEmpGroup)
		{
			if(GetInBattle(inst.InstId))
			{
				foreach(GameObject i in posCellList)
				{
					if(i.GetComponent<EmployeeCellUI>().Employee.InstId == inst.InstId)
					{
						i.GetComponent<EmployeeCellUI>().Employee = inst;
						i.GetComponent<EmployeeCellUI>().professionImg.spriteName = ((JobType)inst.GetIprop(PropertyType.PT_Profession)).ToString();
						i.GetComponent<EmployeeCellUI>().pinzhi.spriteName = EmployessSystem.instance.GetQualityBack((int)inst.quality_);
						i.GetComponent<EmployeeCellUI>().backImg. spriteName = EmployessSystem.instance.GetCellQualityBack((int)inst.quality_);
						i.GetComponent<EmployeeCellUI>().backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)inst.quality_);
						i.GetComponent<EmployeeCellUI>().qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)inst.quality_);
						i.GetComponent<EmployeeCellUI>().qAddImg.spriteName = LanguageManager.instance.GetValue(inst.quality_.ToString());
						i.GetComponent<EmployeeCellUI>().fightingNumLab.text = (inst.GetIprop(PropertyType.PT_FightingForce)).ToString();
						break;
					}
				}
				return;
			}
			if(posCellList.Count >= 4)
				return;
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerForBattle);
			int instId = inst.InstId;
            int assid = EmployeeData.GetData(inst.GetIprop(PropertyType.PT_TableId)).asset_id;
			GameObject clone = GameObject.Instantiate (biaoqian)as GameObject;
			clone.GetComponent<EmployeeCellUI>().Employee = inst;
			clone.GetComponent<EmployeeCellUI>().nameLab.text = inst.InstName;
			UIManager.SetButtonEventHandler(clone.transform.FindChild("down").gameObject,EnumButtonEvent.OnClick,OnDownBattleEmployee,posCellList.Count,0);
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(inst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,clone.GetComponent<EmployeeCellUI>().icon);
			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(inst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(inst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
			}
			clone.GetComponent<EmployeeCellUI>().professionImg.spriteName = ((JobType)inst.GetIprop(PropertyType.PT_Profession)).ToString();
			clone.GetComponent<EmployeeCellUI>().fightingNumLab.text = (inst.GetIprop(PropertyType.PT_FightingForce)).ToString();
			clone.GetComponent<EmployeeCellUI>().pinzhi.spriteName = EmployessSystem.instance.GetQualityBack((int)inst.quality_);
			clone.GetComponent<EmployeeCellUI>().backImg. spriteName = EmployessSystem.instance.GetCellQualityBack((int)inst.quality_);
			clone.GetComponent<EmployeeCellUI>().backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)inst.quality_);
			clone.GetComponent<EmployeeCellUI>().qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)inst.quality_);
			clone.GetComponent<EmployeeCellUI>().qAddImg.spriteName =  LanguageManager.instance.GetValue(inst.quality_.ToString());
			clone.SetActive (true);
			clone.transform.parent = posArr[posCellList.Count];
			posCellList.Add(clone);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			
			//List<Employee> emps = GamePlayer.Instance.GetBattleEmployees ();
		
			/*for(int i =0;i<drawImg.Count;i++)
			{
				drawImg[i].gameObject.SetActive(false);
			}

			if(emps.Count < 4)
			{
				for(int j = 0;j <emps.Count +1;j++)
				{
					drawImg[j].gameObject.SetActive(true);
				}
			}
			*/
		}
		else
		{
			if(!GetInBattle(inst.InstId))
				return;
			_isUpdataBattleList = false;
			UpdataBattleEmployee(_selectEmpGroup);
		}

		UpdateBattleBack ();





		uint[] empGroups = GamePlayer.Instance.GetEmployeesBattles (group);
		
		//int index = 0;
		int num = 0;
		for(int g=0;g<empGroups.Length;g++)
		{
			if(empGroups[g] == 0)
				continue;
			Employee emp = GamePlayer.Instance.GetEmployeeById((int)empGroups[g]);
			if(emp != null)
			{
				num += emp.GetIprop(PropertyType.PT_FightingForce);
			}
		}
		battleNumLab.text = num.ToString ();
	}
	

	private bool GetInBattle(int instId)
	{
		bool isBattle = false;
		foreach(GameObject i in posCellList)
		{
			if(i == null)
				continue;
			if(i.GetComponent<EmployeeCellUI>().Employee.InstId == instId)
			{
				isBattle = true;
				break;
			}
		}

		return isBattle;
	}

	private bool GetSameName(string name)
	{
		bool isBattle = false;
		foreach(GameObject i in posCellList)
		{
			if(i == null)
				continue;
			if(i.GetComponent<EmployeeCellUI>().Employee.InstName == name)
			{
				isBattle = true;
				return isBattle; 
			}
		}
		return isBattle;
	}

	public void Hide()
	{
		if(emplyoeeCellPool.Count > 0)
		{
			for(int i=0;i< emplyoeeCellPool.Count;i++)
			{
				Destroy(emplyoeeCellPool[i]);
				emplyoeeCellPool[i] = null;
			}
			emplyoeeCellPool.Clear();
		}
		GamePlayer.Instance.UpdateEmployeeEnvent -= OnUpdateBattle;

		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
		//GamePlayer.Instance.EmployeeStarOkEnvent -= OnEmployeeStarOk;
	}

	private void UpdateBattleBack()
	{
		foreach(GameObject o in emplyoeeCellList)
		{
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();

			if(GamePlayer.Instance.GetEmployeeIsBattle(cell.Employee.InstId,_selectEmpGroup))
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(true);
			}
			else
			{
				cell.transform.FindChild("select").GetComponent<UISprite>().gameObject.SetActive(false);
			}
		}
	}

	private void OnClickEmpListGroup(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1 == (int)EmployeesBattleGroup.EBG_GroupOne)
		{
			_selectEmpGroup = (int)EmployeesBattleGroup.EBG_GroupOne;
			UpdataBattleEmployee((int)EmployeesBattleGroup.EBG_GroupOne);
			EmpListPkBtn.isEnabled = false;
			EmpListMxBtn.isEnabled = true;
		}
		else
		{
			_selectEmpGroup = (int)EmployeesBattleGroup.EBG_GroupTwo;
			UpdataBattleEmployee((int)EmployeesBattleGroup.EBG_GroupTwo);
			EmpListPkBtn.isEnabled = true;
			EmpListMxBtn.isEnabled = false;
		}

		NetConnection.Instance.changeEmpBattleGroup ((EmployeesBattleGroup) param1);

	}


}
