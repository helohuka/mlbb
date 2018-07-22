using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EmployeeTaskInfoCell : MonoBehaviour
{

	public UILabel nameLab;
	public UITexture icon;
	public UITexture skill0;
	public UITexture skill1;
	public UITexture skill2;
	public UILabel infoLab;
	public UISprite infoImg;
	public UISprite proffessionImg;
	public UISprite backIng;
	public List<int> monster = new List<int> ();

	public Employee _employee;
	 

	void Start ()
	{

	}

	void Update ()   
	{

	}

	public Employee EmpData
	{
		set
		{
			if(value == null)
				return;
			_employee = value;
			nameLab.text = _employee.InstName;

			proffessionImg.spriteName =  ((JobType) _employee.GetIprop(PropertyType.PT_Profession)).ToString();
			backIng.spriteName  =  EmployessSystem.instance.GetQualityBack((int)_employee.quality_);
			//backIng.spriteName = EmployessSystem.instance.GetCellQualityBack((int)_employee.quality_);
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(_employee.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,icon);
			HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)_employee.SkillInsts[0].skillID_,(int)_employee.SkillInsts[0].skillLevel_)._ResIconName,skill0);
			HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)_employee.SkillInsts[1].skillID_,(int)_employee.SkillInsts[1].skillLevel_)._ResIconName,skill1);
			HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)_employee.SkillInsts[2].skillID_,(int)_employee.SkillInsts[2].skillLevel_)._ResIconName,skill2);

			//if(_employee.isForBattle_)
			//{
			//	infoLab.text = "battle";
			//}
			//else
			//{
				infoLab.text = LanguageManager.instance.GetValue("emplouyeetasklow");
				for(int i= 0;i< _employee.SkillInsts.Count;i++)
				{
					for(int j =0 ;j< monster.Count;j++ )
					{
						EmployeeMonsterData msData = EmployeeMonsterData.GetData(monster[j]);
						if(msData == null)
							continue;
						for(int k=0;k<msData.skills.Count;k++)
						{
							if(EmployeeQuestSkillData.GetData((int)_employee.SkillInsts[i].skillID_,(EmployeeSkillType)msData.skills[k]) > 0)
							{
								infoLab.text =  LanguageManager.instance.GetValue("emplouyeetaskhigh");
								return;
							}
						}
					}
				}
			//}
		}
		get
		{
			return _employee;
		}
	}

}

