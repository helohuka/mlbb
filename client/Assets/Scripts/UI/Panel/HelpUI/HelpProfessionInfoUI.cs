using UnityEngine;
using System.Collections;

public class HelpProfessionInfoUI : MonoBehaviour
{
	public UILabel nameLab;
	public UISprite jobIcon;
	public UILabel descLab;
	public UILabel jiadianLab;
	public UILabel skillLab;
	public UILabel equipLab;
	public UIButton closeBtn;
	public UIButton goSkillBtn;
	public UIButton goMakeBtn;
	public UIButton backBtn;



	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnColseBtn, 0, 0);
		UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, OnColseBtn, 0, 0);
		UIManager.SetButtonEventHandler (goSkillBtn.gameObject, EnumButtonEvent.OnClick, OnSkillBtn, 0, 0);
		UIManager.SetButtonEventHandler (goMakeBtn.gameObject, EnumButtonEvent.OnClick, OnMakeBtn, 0, 0);

	}
	
	public Profession professData
	{
		set
		{
			nameLab.text= value.jobName_;
			jobIcon.spriteName = value.jobtype_.ToString();
			descLab.text= value.Describe_;
			skillLab.text= value.RecommendSkills_;
			string [] Attribute = value.Recommand_.Split(';');
			jiadianLab.text = "";
			for(int i = 0;i<Attribute.Length;i++)
			{
				string [] addStr = Attribute[i].Split(':');
				jiadianLab.text+= LanguageManager.instance.GetValue( addStr[0]) +" +"+addStr[1] +"\n";
			}
			equipLab.text = value.RecommendEquippes_;
		}
		get
		{
			return null;
		}
	}


	private void OnColseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

	void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Bar))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		LearningUI.SwithShowMe ();
	}

	void OnMakeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Make))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		CompoundUI.SwithShowMe ();
	}


}