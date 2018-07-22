using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeControlListUI : MonoBehaviour
{
	public UIButton fireBtn;
	public UIButton buyBtn;
	public GameObject empCell;
	public UIGrid grid;
	public UILabel empNum;

	public UILabel fireBtnLab;
	public UILabel buyBtnLab;
	public UILabel explainLab;
	public UILabel empNumLab;
	public UILabel empMoneyLab;
	private List<GameObject> emplyoeeCellList = new List<GameObject>();  
	private List<GameObject> emplyoeeCellPool = new List<GameObject>();

	public GameObject selectObj;
	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (fireBtn.gameObject, EnumButtonEvent.OnClick, OnFire, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuy, 0, 0);
		GamePlayer.Instance.DelEmployeeEnvent += new RequestEventHandler<uint> (OnDelEmployeeOk);
		GamePlayer.Instance.UpdateEmployeeEnvent += new EmpOnBattleEvent (UpdateEmployeeEvent);
		GamePlayer.Instance.EmployeeStarOkEnvent += new RequestEventHandler<Employee> (OnEmployeeStarOk);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;

		fireBtnLab.text = LanguageManager.instance.GetValue("fireBtnLab");
		buyBtnLab.text = LanguageManager.instance.GetValue("buyBtnLab");
		explainLab.text = LanguageManager.instance.GetValue("explainLab");
		empNumLab.text = LanguageManager.instance.GetValue("empNumLab");

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerListTabShow);
	}



	private void OnFire(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeFireUI.ShowMe();
	}

	private void OnBuy(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeBuySkillUI.ShowMe ();
	}

	void OnDelEmployeeOk(uint id)
	{
		AddScrollViewItem (GamePlayer.Instance.EmployeeList);
	}

	void UpdateEmployeeEvent(Employee emp,int grop)
	{
		for( int i=0;i<emplyoeeCellList.Count;i++)
		{
			if(emplyoeeCellList[i].GetComponent<EmployeeCellUI>().Employee.InstId == emp.InstId )
			{
				EmployeeCellUI cell = emplyoeeCellList[i].GetComponent<EmployeeCellUI>();
				cell.Employee = emp;
				cell.GetComponent<EmployeeCellUI>().UpdateRed();
				cell.qAddImg.spriteName = LanguageManager.instance.GetValue(emp.quality_.ToString());
				cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)emp.quality_);
				cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)emp.quality_);
				cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)emp.quality_);
				cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)emp.quality_);
				cell.professionImg.spriteName = ((JobType)emp.GetIprop(PropertyType.PT_Profession)).ToString();
				cell.fightingNumLab.text = emp.GetIprop(PropertyType.PT_FightingForce).ToString();
				if( emp.isForBattle_)
				{
					emplyoeeCellList[i].transform.FindChild("inBattle").gameObject.SetActive(true);
				}
				else
				{
					emplyoeeCellList[i].transform.FindChild("inBattle").gameObject.SetActive(false);
				}

			}
		}
	}

	void OnEmployeeStarOk(Employee inst)
	{
		for(int i= 0;i<emplyoeeCellList.Count;i++)
		{
		   if(emplyoeeCellList[i].GetComponent<EmployeeCellUI>().Employee.InstId == inst.InstId)
		    {
				emplyoeeCellList[i].GetComponent<EmployeeCellUI>().Employee = inst;
		    }
		}
	}


	public void AddScrollViewItem(List<Employee> Edata)
	{
		if (empCell == null)
			return;
		empNum.text = Edata.Count.ToString()+"/100";
		empMoneyLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_EmployeeCurrency).ToString ();
		if(emplyoeeCellList.Count>0)
		{
			foreach(GameObject o in emplyoeeCellList)
			{
				o.transform.parent = null;
				emplyoeeCellPool.Add(o);
				o.transform.FindChild("select").gameObject.SetActive(false);
				o.gameObject.SetActive(false);
			}
			emplyoeeCellList.Clear();
		}

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
				o  = Instantiate(empCell)as GameObject;
			}
			
			if( battles[i].isForBattle_)
			{
				o.transform.FindChild("inBattle").gameObject.SetActive(true);
			}
			else
			{
				o.transform.FindChild("inBattle").gameObject.SetActive(false);
			}
			
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = battles[i];
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(battles[i].quality_.ToString());
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)battles[i].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)battles[i].quality_);
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)battles[i].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)battles[i].quality_);
			cell.professionImg.spriteName = ((JobType)battles[i].GetIprop(PropertyType.PT_Profession)).ToString();
			cell.fightingNumLab.text = battles[i].GetIprop(PropertyType.PT_FightingForce).ToString();
			cell.UpdateRed();
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,cell.icon);
			
			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(battles[i].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
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



		
		for (int j = 0; j<Edata.Count; j++) 
		{
			if(!gameObject.activeSelf)
				return;
			if(battles.Contains(Edata[j]))
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

			if( Edata[j].isForBattle_)
			{
				o.transform.FindChild("inBattle").gameObject.SetActive(true);
			}
			else
			{
				o.transform.FindChild("inBattle").gameObject.SetActive(false);
			}

			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = Edata[j];
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(Edata[j].quality_.ToString());
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)Edata[j].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)Edata[j].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)Edata[j].quality_);
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)Edata[j].quality_);
			cell.professionImg.spriteName = ((JobType)Edata[j].GetIprop(PropertyType.PT_Profession)).ToString();
			cell.fightingNumLab.text = Edata[j].GetIprop(PropertyType.PT_FightingForce).ToString();
			cell.UpdateRed();
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(Edata[j].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,cell.icon);
		
			if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(Edata[j].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(Edata[j].GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
			}
			UIManager.SetButtonEventHandler(o,EnumButtonEvent.OnClick,OnEmployeeCell,0,0);
			o.transform.parent = grid.transform; 
			o.SetActive(true);
			emplyoeeCellList.Add(o);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			if (j == 0)
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

		if(selectObj == null)
		{
			selectObj = obj.gameObject;
			selectObj.transform.FindChild("select").gameObject.SetActive(true);
		}
		else if(selectObj != obj.gameObject)
		{
			selectObj.transform.FindChild("select").gameObject.SetActive(false);
		}

		Employee employee = cell.Employee;
		if (employee == null)
			return;
		employee = GamePlayer.Instance.GetEmployeeById (employee.InstId);
		EmployessSystem.instance.CurEmployee = employee;
		EmployeeListUI.ShowMe ();
	}

	public void Hide()
	{
		GamePlayer.Instance.DelEmployeeEnvent -= OnDelEmployeeOk;
		GamePlayer.Instance.UpdateEmployeeEnvent -= UpdateEmployeeEvent;
		GamePlayer.Instance.EmployeeStarOkEnvent -= OnEmployeeStarOk;
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;

		if(emplyoeeCellPool.Count > 0)
		{
			for(int i=0;i< emplyoeeCellPool.Count;i++)
			{
				Destroy(emplyoeeCellPool[i]);
				emplyoeeCellPool[i] = null;
			}
			emplyoeeCellPool.Clear();
		}

		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

	public void UpdateMoney()
	{
		empMoneyLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_EmployeeCurrency).ToString ();
	}

}

