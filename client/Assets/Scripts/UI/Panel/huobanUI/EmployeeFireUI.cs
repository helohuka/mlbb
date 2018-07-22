using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeFireUI : UIBase
{
	public UIButton closeBtn;
	public UIButton fireBtn;
	public UIButton oneKeyFireBtn;
	public GameObject empCell;
	public UIGrid grid;
	public UILabel employeeNumlab;

	private List<GameObject> emplyoeeCellList = new List<GameObject>();  
	private List<GameObject> emplyoeeCellPool = new List<GameObject>();
	private List<uint> _fireEmployees = new List<uint>();
	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (fireBtn.gameObject, EnumButtonEvent.OnClick, OnFire, 0, 0);
		UIManager.SetButtonEventHandler (oneKeyFireBtn.gameObject, EnumButtonEvent.OnClick, OnOneKeyFire, 0, 0);
		GamePlayer.Instance.DelEmployeeEnvent += new RequestEventHandler<uint> (OnDelEmployeeOk);
		AddScrollViewItem (GamePlayer.Instance.EmployeeList);
	}

	
	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_employeeFrieUI);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_employeeFrieUI);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_employeeFrieUI);
	}

	public override void Destroyobj ()
	{
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_employeeFrieUI, AssetLoader.EAssetType.ASSET_UI), true);
		GameObject.Destroy (gameObject);
	}

	#endregion

	protected override void DoHide ()
	{
		GamePlayer.Instance.DelEmployeeEnvent -= OnDelEmployeeOk;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
		base.DoHide ();
	}
	
	void OnDelEmployeeOk(uint id)
	{
		AddScrollViewItem (GamePlayer.Instance.EmployeeList);
	}

	void AddScrollViewItem(List<Employee> Edata)
	{
		employeeNumlab.text = Edata.Count+"/100";
		if (empCell == null)
			return;

		oneKeyFireBtn.isEnabled = false;
		fireBtn.isEnabled = false;

		if(emplyoeeCellList.Count>0)
		{
			foreach(GameObject o in emplyoeeCellList)
			{
				o.transform.FindChild("red").GetComponent<UISprite>().gameObject.SetActive(false);
				o.transform.parent = null;
				emplyoeeCellPool.Add(o);
				o.gameObject.SetActive(false);
			}
			emplyoeeCellList.Clear();
		}
		
		for (int i = 0; i<Edata.Count; i++)
		{
			if( Edata[i].isForBattle_ || EmployeeTaskSystem.instance.IsTaskEmp(Edata[i].InstId))
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
				o  = Instantiate(empCell)as GameObject;
			}

			fireBtn.isEnabled = true;
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = Edata[i];
			if(Edata[i].quality_ < QualityColor.QC_Purple)
			{
				oneKeyFireBtn.isEnabled = true;
			}
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)Edata[i].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)Edata[i].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite= EmployessSystem.instance.GetCellQualityBack((int)Edata[i].quality_);
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(Edata[i].quality_.ToString());
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)Edata[i].quality_);
			cell.professionImg.spriteName = ((JobType)Edata[i].GetIprop(PropertyType.PT_Profession)).ToString();
			/*for(int c =0 ;c<cell.starList.Count;c++)
			{
				cell.starList[c].gameObject.SetActive(false);
			}
			for(int j =0 ;j<Edata[i].star_&&j<5;j++)
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
			o.transform.parent = grid.transform; 
			o.SetActive(true);
			emplyoeeCellList.Add(o);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			if (i == 0)
			{
				GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_FirstPartner_PosUI);
			}
		}
		grid.Reposition();
	}
		
	private void OnEmployeeCell(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeCellUI cell = obj.GetComponent<EmployeeCellUI> ();
		if(cell == null || cell.Employee == null)
		{
			return;
		}

		if( EmployeeTaskSystem.instance.IsTaskEmp(cell.Employee.InstId))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("huanbanrenwuzhong"));
			return;	
		}

		Employee  employee = GamePlayer.Instance.GetEmployeeById (cell.Employee.InstId);
		if(_fireEmployees.IndexOf((uint)cell.Employee.InstId) < 0)
		{
			obj.transform.FindChild("red").GetComponent<UISprite>().gameObject.SetActive(true);
			_fireEmployees.Add ((uint)employee.InstId);
		
		}
		else
		{
			obj.transform.FindChild("red").GetComponent<UISprite>().gameObject.SetActive(false);
			_fireEmployees.Remove((uint)employee.InstId);
		}
	}
		
	private void OnClose(ButtonScript obj, object args, int param1, int param2)
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
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	private void OnFire(ButtonScript obj, object args, int param1, int param2)
	{
		if(_fireEmployees.Count <= 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("xuezejiegu"));
			return;
		}

		uint skillNum = 0;
		for(int i=0;i<_fireEmployees.Count;i++)
		{
			Employee emp = GamePlayer.Instance.GetEmployeeById((int)_fireEmployees[i]);
			uint soulNum = emp.soul_ ;//> 0 ? (emp.soul_+1): 1;
			skillNum += ((uint)Mathf.Pow((float)2,(float)emp.quality_) *10) ;//* soulNum; 
			if(soulNum > 0)
			{
				EmployeeData eData = EmployeeData.GetData(emp.GetIprop(PropertyType.PT_TableId));
				skillNum += ((uint)Mathf.Pow((float)2,(float)eData.quality_) *10) * soulNum;
			}
		}
		uint[] emps = _fireEmployees.ToArray();
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("jieguhuoban").Replace("{n}",skillNum.ToString()), () => {
			NetConnection.Instance.delEmployee (emps);
		_fireEmployees.Clear ();
		});
	}

	private void OnOneKeyFire(ButtonScript obj, object args, int param1, int param2)
	{
		uint skillNum = 0;
		foreach(Employee e in GamePlayer.Instance.EmployeeList)
		{
			if((int)e.quality_ <= (int)QualityColor.QC_Blue && !e.isForBattle_ && !EmployeeTaskSystem.instance.IsTaskEmp(e.InstId))
			{
				uint soulNum = e.soul_;// > 0 ? (e.soul_+1): 1;
				skillNum += ((uint)Mathf.Pow((float)2,(float)e.quality_) *10);//*soulNum;
				if(soulNum > 0)
				{
					EmployeeData eData = EmployeeData.GetData(e.GetIprop(PropertyType.PT_TableId));
					skillNum += ((uint)Mathf.Pow((float)2,(float)eData.quality_) *10) * soulNum;
				}
			}
		}

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("yijianjieguhuoban").Replace("{n}",skillNum.ToString()), () => {
		NetConnection.Instance.onekeyDelEmp();
		});
	}



}

