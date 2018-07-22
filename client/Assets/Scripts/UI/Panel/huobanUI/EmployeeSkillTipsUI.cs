using UnityEngine;
using System.Collections;

public class EmployeeSkillTipsUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel expendLab;
	public UILabel descLab;

	private SkillData _data;
	void Start ()
	{

	}


	public SkillData SkillTabData 
	{
		set
		{
			_data = value;
			nameLab.text = _data._Name;
			levelLab.text = _data._Level.ToString();
			expendLab.text = _data._Cost_mana.ToString();
			descLab.text = _data._Desc;
		}
		get
		{
			return _data;
		}
	}

}

