using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeSoulUI : MonoBehaviour
{
	public GameObject empCell;
	public UIGrid grid;
	public UIButton  fireBtn;

	public GameObject firePanel;
	public UIButton fireCloseBtn;
	public UIButton fireOkBtn;
	public UIButton fireAddBtn;
	public UIButton fireJJBtn;
	public UILabel fireInfoLab;
	public UILabel fireNumLab;
	public UILabel fireYinJiNumLab;
	public UILabel fireZhiXinNumLab;
	public UITexture EmployeeIcon;
	public UISprite iconBack;
	public UISprite professionImg;
	private Employee SelectEmployee; 

	private GameObject selectObj;
	private List<GameObject> emplyoeeCellList = new List<GameObject>();  
	private List<GameObject> emplyoeeCellPool = new List<GameObject>();
	private List<uint> _fireEmployees = new List<uint>();
	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler(fireBtn.gameObject,EnumButtonEvent.OnClick,OnFire,0,0);
		UIManager.SetButtonEventHandler(fireCloseBtn.gameObject,EnumButtonEvent.OnClick,OnCloseFire,0,0);
		UIManager.SetButtonEventHandler(fireOkBtn.gameObject,EnumButtonEvent.OnClick,OnOkFire,0,0);
		UIManager.SetButtonEventHandler(fireAddBtn.gameObject,EnumButtonEvent.OnClick,OnAddNum,0,0);
		UIManager.SetButtonEventHandler(fireJJBtn.gameObject,EnumButtonEvent.OnClick,OnJjNum,0,0);



		GamePlayer.Instance.UpdateEmployeeEnvent += new EmpOnBattleEvent (UpdateEmployeeEvent);
	}

	public void AddScrollViewItem(List<Employee> Edata)
	{
	//	List<Employee> Edata = GamePlayer.Instance.EmployeeList;

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

		for (int j = 0; j<Edata.Count; j++) 
		{
			if(!gameObject.activeSelf)
				return;
			if(Edata[j].soul_ <= 0)
				continue;
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
			
			
			EmployeeCellUI cell = o.GetComponent<EmployeeCellUI>();
			cell.Employee = Edata[j];
			cell.qAddImg.spriteName = LanguageManager.instance.GetValue(Edata[j].quality_.ToString());
			cell.pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)Edata[j].quality_);
			cell.backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int)Edata[j].quality_);
			cell.backImg.GetComponent<UIButton>().normalSprite = EmployessSystem.instance.GetCellQualityBack((int)Edata[j].quality_);
			cell.qAddImgBack.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)Edata[j].quality_);
			cell.professionImg.spriteName = ((JobType)Edata[j].GetIprop(PropertyType.PT_Profession)).ToString();
			cell.fightingNumLab.text = Edata[j].soul_.ToString();
		//	cell.UpdateRed();
			o.transform.FindChild("red").GetComponent<UISprite>().gameObject.SetActive(false); 
		
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

	void UpdateEmployeeEvent(Employee emp,int grop)
	{
        NetWaitUI.HideMe();
		AddScrollViewItem (GamePlayer.Instance.EmployeeList);
	}
	public void Hide()
	{
		GamePlayer.Instance.UpdateEmployeeEnvent -= UpdateEmployeeEvent;
		
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

	private void OnEmployeeCell(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeCellUI cell = obj.GetComponent<EmployeeCellUI> ();
		if(cell == null || cell.Employee == null)
		{
			return;
		}
		if(selectObj != null)
		{
			selectObj.transform.FindChild("red").GetComponent<UISprite>().gameObject.SetActive(false); 
		}
		selectObj = obj.gameObject;
		_fireEmployees.Clear ();
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

	private void OnFire(ButtonScript obj, object args, int param1, int param2)
	{
		if(_fireEmployees.Count <= 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("xuanzeyinji"));
			return;
		}
		SelectEmployee = GamePlayer.Instance.GetEmployeeById((int)_fireEmployees[0]);
		if(SelectEmployee == null)
		{
			return;
		}
		fireNumLab.text = SelectEmployee.soul_.ToString();
		fireYinJiNumLab.text = SelectEmployee.soul_.ToString();
		firePanel.gameObject.SetActive(true);
		iconBack.spriteName = EmployessSystem.instance.GetQualityBack ((int)EmployeeData.GetData (SelectEmployee.GetIprop (PropertyType.PT_TableId)).quality_);
		professionImg.spriteName = ((JobType)SelectEmployee.GetIprop(PropertyType.PT_Profession)).ToString();
		HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(SelectEmployee.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,EmployeeIcon);
		uint yjNum = ((uint)Mathf.Pow ((float)2, (float)EmployeeData.GetData (SelectEmployee.GetIprop (PropertyType.PT_TableId)).quality_) * 10);
		fireZhiXinNumLab.text = (yjNum* SelectEmployee.soul_).ToString ();
		fireInfoLab.text = LanguageManager.instance.GetValue("fireyinji").Replace("{n}",yjNum.ToString());
	}

	private void OnCloseFire(ButtonScript obj, object args, int param1, int param2) 
	{
		firePanel.gameObject.SetActive(false);
	}

	private void OnOkFire(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.delEmployeeSoul (_fireEmployees [0], uint.Parse (fireNumLab.text));
		firePanel.gameObject.SetActive(false);
		_fireEmployees.Clear ();
        NetWaitUI.ShowMe();
	}
	private void OnAddNum(ButtonScript obj, object args, int param1, int param2)
	{
		uint num = uint.Parse (fireNumLab.text);
		if(num == SelectEmployee.soul_)
		{
			return;
		} 
		++num;
		fireNumLab.text = num.ToString ();
		fireYinJiNumLab.text = num +"/"+SelectEmployee.soul_;
		fireZhiXinNumLab.text = (((uint)Mathf.Pow ((float)2, (float)EmployeeData.GetData (SelectEmployee.GetIprop (PropertyType.PT_TableId)).quality_) * 10) * num).ToString ();


	}
	private void OnJjNum(ButtonScript obj, object args, int param1, int param2)
	{
		uint num = uint.Parse (fireNumLab.text);
		if(num <= 1)
		{
			return;
		}
		--num;
		fireNumLab.text = num.ToString ();
		fireYinJiNumLab.text = num +"/"+SelectEmployee.soul_;
		fireZhiXinNumLab.text = (((uint)Mathf.Pow ((float)2, (float)EmployeeData.GetData (SelectEmployee.GetIprop (PropertyType.PT_TableId)).quality_) * 10) * num).ToString ();
	}

}

