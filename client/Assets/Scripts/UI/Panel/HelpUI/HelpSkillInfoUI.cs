using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpSkillInfoUI : MonoBehaviour
{
	public UILabel nameLab;
	public UITexture icon;
	public UILabel descLab;
	public UILabel getWayLab;
	private SkillData _skillData;
	public UIButton jiuguanBtn;
	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (jiuguanBtn.gameObject, EnumButtonEvent.OnClick, OnClickJiuguan, 0, 0);
	}

	public SkillData Skill
	{
		set
		{
			if(value != null)
			{
				_skillData = value;
				nameLab.text = _skillData._Name;
				descLab.text= _skillData._Desc;
				//getWayLab.text= 
				HeadIconLoader.Instance.LoadIcon(_skillData._ResIconName,icon);

				if(!_icons.Contains(_skillData._ResIconName))
				{
					_icons.Add(_skillData._ResIconName);
				}
			}
		}
	}
	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

	private void OnClickJiuguan(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Bar))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		LearningUI.SwithShowMe ();
	}
}

