using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EmployeeEvolveUI : MonoBehaviour
{
	public UIButton jinhuaBtn;
	public UISprite jinhuaQIcon;
	public UITexture jinhuaIcon;
	public UILabel nameLab;
	public UISprite jobIcon;
	public GameObject jinjieObj;
	public UILabel maxLab;

	public UISprite nextText;
	public UILabel nextTextLab;
	public UISprite propImg;
	public UISprite propNumImg;
	private Employee employeeInst;

	private List<string> _icons = new List<string>();
	
	void Start ()
	{
		UIManager.SetButtonEventHandler (jinhuaBtn.gameObject, EnumButtonEvent.OnClick, OnJinHuaBtn, 0, 0);
		UpdateEvolveInfo();
	}



	private void OnJinHuaBtn(ButtonScript obj, object args, int param1, int param2)
	{

		EmployessSystem.instance._OldEmployee = null;
		EmployessSystem.instance._OldEmployee = new Employee();
		//EmployessSystem.instance._OldEmployee = employeeInst;
		EmployessSystem.instance._OldEmployee.Properties = new List<float> (employeeInst.Properties).ToArray ();
		//EmployessSystem.instance.oldProp = employeeInst.Properties;
		NetConnection.Instance.requestEvolve ((uint)employeeInst.InstId);
	}
	
	public void UpdateEvolveInfo()
	{
		gameObject.SetActive (true);
		employeeInst = EmployessSystem.instance.CurEmployee;
		if (employeeInst == null)
			return;
		HeadIconLoader.Instance.LoadIcon(
		EntityAssetsData.GetData(EmployeeData.GetData(employeeInst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,jinhuaIcon);

		if(!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData(employeeInst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_))
		{
			_icons.Add(EntityAssetsData.GetData(EmployeeData.GetData(employeeInst.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_);
		}

		jinhuaQIcon.spriteName = EmployessSystem.instance.GetQualityBack ((int)employeeInst.quality_);
		List<Employee> employees = GamePlayer.Instance.EmployeeList;
		nextTextLab.text = LanguageManager.instance.GetValue ("huobanjinjie").Replace("{n}","10");
		jobIcon.spriteName = ((JobType)employeeInst.GetIprop (PropertyType.PT_Profession)).ToString ();
		propNumImg.spriteName = LanguageManager.instance.GetValue(employeeInst.quality_.ToString());
		propImg.spriteName = EmployessSystem.instance.GetAddQualityNUmBack((int)employeeInst.quality_);
		nameLab.text = employeeInst.InstName;
		bool isCanEvolve = false;

		maxLab.gameObject.SetActive(false);
		jinjieObj.gameObject.SetActive(true);
		int employeeNum = 0;
		if (EmployeeData.GetData (employeeInst.GetIprop(PropertyType.PT_TableId)).evolutionNum.Length <= (int)employeeInst.quality_-1)
		{
			//jinhuaBtn.isEnabled = false;
			//jinhuaIcon.gameObject.SetActive(false);
			maxLab.gameObject.SetActive(true);
			jinjieObj.gameObject.SetActive(false);
			return;
	    }
		if((int)employeeInst.quality_ > (int)QualityColor.QC_Orange)
		{
			//jinhuaBtn.isEnabled = false;
			//gameObject.SetActive (false);
			maxLab.gameObject.SetActive(true);
			jinjieObj.gameObject.SetActive(false);
			return;
		}
    	int needNum = int.Parse(EmployeeData.GetData(employeeInst.GetIprop(PropertyType.PT_TableId)).evolutionNum[(int)employeeInst.quality_-1]);
		jinhuaQIcon.transform.Find("num").GetComponent<UILabel>().text =  employeeInst.soul_ +"/"+needNum;	
		jinhuaQIcon.gameObject.SetActive(true);
		if(employeeInst.soul_ >=  needNum)
		{
			isCanEvolve = true;
			jinhuaQIcon.transform.Find("num").GetComponent<UILabel>().color = Color.green;
		}
		if(! isCanEvolve)
		{
			jinhuaBtn.isEnabled = false;
			jinhuaIcon.color = Color.gray;
			jinhuaQIcon.transform.Find("num").GetComponent<UILabel>().color = Color.red;
		}
		else
		{
			jinhuaBtn.isEnabled = true; 
		}
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}
	
	