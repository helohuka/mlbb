using UnityEngine;
using System.Collections;

public class MineSkillCellUI : MonoBehaviour
{
	public UILabel SkillName;
	public UILabel SkillLevel;
	public UIButton gatherBtn;
	public UILabel butLab;
	public UITexture icon;
	private int _skillId;
	private int _gatherId;
	private SkillData _skillData;


	void Start () 
	{
	}


	public int gatherType
	{
		set
		{
			if(_skillId != value)
			{
				_skillId = value;
				//SkillData skill = SkillData.GetData(value);
				//if(skill != null)
				//{
					//SkillInfo = skill;
				//	SkillName.text = skill.name_;
					//icon.spriteName =skill.resIconName;

				//}
			}
		}
		get
		{
			return _skillId;
		}
	}

	public int GatherId
	{
		set
		{
			if(_gatherId != value)
			{
				_gatherId = value;
			}
		}
		get
		{
			return _gatherId;
		}
	}

	public SkillData SkillInfo
	{
		set
		{
			if(value != null)
			{
				_skillData = value;
				SkillName.text = _skillData._Name;
			}
		}
		get
		{
			return _skillData;
		}
	}



}

