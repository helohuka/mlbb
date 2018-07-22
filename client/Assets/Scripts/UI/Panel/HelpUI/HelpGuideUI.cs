using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpGuideUI : MonoBehaviour
{

	public List<UIButton> itemButs = new List<UIButton>(); //indx {0 装备  1 技能 2 宠物 3 伙伴 4 职业}.
	public HelpEquipAndSkillUI equipSkillUI;
	public HelpBabyUI babyUI;
	public HelpEmployeeUI employeeUI;
	public HelpProfessionUI professionUI;
	public TuJianUI babyTujianUI;
    public HelpLevelUI levelUI;

	void Start ()
	{
		UIManager.SetButtonEventHandler (itemButs[0].gameObject, EnumButtonEvent.OnClick, OnEquipBtn, 0, 0);
		UIManager.SetButtonEventHandler (itemButs[1].gameObject, EnumButtonEvent.OnClick, OnSkillBtn, 0, 0);
		UIManager.SetButtonEventHandler (itemButs[2].gameObject, EnumButtonEvent.OnClick, OnBabyBtn, 0, 0);
		UIManager.SetButtonEventHandler (itemButs[3].gameObject, EnumButtonEvent.OnClick, OnEmployeeBtn, 0, 0);
		UIManager.SetButtonEventHandler (itemButs[4].gameObject, EnumButtonEvent.OnClick, OnProfessionBtn, 0, 0);
		UIManager.SetButtonEventHandler (itemButs[5].gameObject, EnumButtonEvent.OnClick, OnTuJianBtn, 0, 0);
        UIManager.SetButtonEventHandler(itemButs[6].gameObject, EnumButtonEvent.OnClick, OnLevelBtn, 0, 0);
	}


	private void OnEquipBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		employeeUI.gameObject.SetActive (false);
		babyUI.gameObject.SetActive (false);
		professionUI.gameObject.SetActive (false);
        babyTujianUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		equipSkillUI.gameObject.SetActive (true);
		equipSkillUI.SelectType = 1;
	}

	private void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		employeeUI.gameObject.SetActive (false);
        babyUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		professionUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (true);
		equipSkillUI.SelectType = 2;
	}

	private void OnBabyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		employeeUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (false);
		professionUI.gameObject.SetActive (false);
        babyTujianUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		babyUI.gameObject.SetActive (true);
	}

	private void OnEmployeeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		babyUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (false);
		professionUI.gameObject.SetActive (false);
        babyTujianUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		employeeUI.gameObject.SetActive (true);
		employeeUI.UpdataEmployees ();
	}

	private void OnProfessionBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		babyUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (false);
		employeeUI.gameObject.SetActive (false);
        babyTujianUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		professionUI.gameObject.SetActive (true);
		professionUI.UpdataProfession ();
	}

	private void OnTuJianBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
		babyUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (false);
		professionUI.gameObject.SetActive (false);
        employeeUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
		babyTujianUI.gameObject.SetActive (true);
	}

    private void OnLevelBtn(ButtonScript obj, object args, int param1, int param2)
    {
		for(int i=0;i<itemButs.Count;i++)
		{
			itemButs[i].isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
        babyUI.gameObject.SetActive(false);
        equipSkillUI.gameObject.SetActive(false);
        professionUI.gameObject.SetActive(false);
        employeeUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
        babyTujianUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(true);
        levelUI.UpdateData();
    }

	public void ShowOpenLevel()
	{
		babyUI.gameObject.SetActive(false);
		equipSkillUI.gameObject.SetActive(false);
		professionUI.gameObject.SetActive(false);
		employeeUI.gameObject.SetActive(false);
		levelUI.gameObject.SetActive(false);
		babyTujianUI.gameObject.SetActive(false);
		levelUI.gameObject.SetActive(true);
		levelUI.UpdateData();
		itemButs[6].isEnabled = false;
	}

	public void ShowProfession()
	{
		babyUI.gameObject.SetActive (false);
		equipSkillUI.gameObject.SetActive (false);
		employeeUI.gameObject.SetActive (false);
		babyTujianUI.gameObject.SetActive(false);
		levelUI.gameObject.SetActive(false);
		professionUI.gameObject.SetActive (true);
		professionUI.UpdataProfession ();
		itemButs[4].isEnabled = false;
	}


}

