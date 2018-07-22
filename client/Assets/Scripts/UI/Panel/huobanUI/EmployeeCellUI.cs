using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EmployeeCellUI : MonoBehaviour
{
	public UILabel scoreLab;
	public UILabel nameLab;
	public UITexture icon;
	public Transform mods;
	public UISprite pinzhi;
	public UISprite backImg;
	public UILabel professionLab;
	public UISprite professionImg;
	public List<UISprite> starList = new List<UISprite> ();
	public UISprite qAddImg;
	public UISprite qAddImgBack;
	public UILabel fightingNumLab;
	private Employee employee;  

	void Start ()
	{
	
	}



	public Employee  Employee
	{
		set
		{
			if(value != null)
			{
				employee = value;
				nameLab.text = employee.InstName;
				//pinzhi.spriteName  =  EmployessSystem.instance.GetQualityBack((int)employee.quality_);

				//professionLab.text = employee.GetIprop(PropertyType.PT_FightingForce).ToString();//  Profession.get((JobType)employee.GetIprop(PropertyType.PT_Profession), 
				                               //      employee.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
				/*for(int i =0 ;i<starList.Count;i++)
				{
					starList[i].gameObject.SetActive(false);
				}
				for(int j =0 ;j<employee.star_ && j< 5;j++)
				{
					starList[j].gameObject.SetActive(true);
				}
				*/
				for(int i =0;i<starList.Count;i++)
				{
					starList[i].gameObject.SetActive(false);
				}
				int len = (int)employee.star_;
				if(employee.star_ >=6)
				{
					len  = (int)employee.star_- 5;
					for(int j =0;j<len && j<5;j++)
					{
						starList[j].spriteName = "zixingxing";
						starList[j].gameObject.SetActive(true);
					}
				}
				else
				{
					for(int j =0;j<employee.star_ && j<5;j++)
					{
						starList[j].spriteName = "xingxing";
						starList[j].gameObject.SetActive(true);
					}
				}



				//UpdateRed();
			}
		}
		get
		{
			return employee;
		}

	}


	public void UpdateRed()
	{
		//	List<Employee> employees = GamePlayer.Instance.EmployeeList;
		
		bool isCanEvolve = false;
		
		uint employeeNum = Employee.soul_;
		/*foreach(var x in employees)
		{
			if (x.Properties[(int)PropertyType.PT_TableId] == Employee.Properties[(int)PropertyType.PT_TableId] && x.quality_ == Employee.quality_
			    && x.InstId != Employee.InstId)
			{
				employeeNum++;
			}
		}
		*/
		if((int)Employee.quality_ > (int)QualityColor.QC_Orange )
		{
			gameObject.GetComponent<UISprite>().MarkOff();
			return;
		}
	//	Debug.LogError ("-----id  " + (int)(Employee.GetIprop(PropertyType.PT_TableId))+"- Q - " + (int)(Employee.quality_) + "- L  " + EmployeeData.GetData(Employee.GetIprop(PropertyType.PT_TableId)).evolutionNum.Length);
		if(((int)(Employee.quality_)-1) >= EmployeeData.GetData(Employee.GetIprop(PropertyType.PT_TableId)).evolutionNum.Length)
		{
			gameObject.GetComponent<UISprite>().MarkOff();
			return;
		}
		int needNum = int.Parse(EmployeeData.GetData(Employee.GetIprop(PropertyType.PT_TableId)).evolutionNum[(int)Employee.quality_-1]);

		if(employeeNum >=  needNum)
		{
			gameObject.GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10,-15);
		}
		else
		{
			gameObject.GetComponent<UISprite>().MarkOff();
		}


	}

}

