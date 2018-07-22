using UnityEngine;
using System.Collections;

public class HelpBabyUI : MonoBehaviour
{

	public UIButton propBtn;
	public UIButton bookBtn;
	public UIButton skillBtn;

	void Start ()
	{
		UIManager.SetButtonEventHandler (propBtn.gameObject, EnumButtonEvent.OnClick, OnPropBtn, 0, 0);
		UIManager.SetButtonEventHandler (bookBtn.gameObject, EnumButtonEvent.OnClick, OnBookBtn, 0, 0);
		UIManager.SetButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick, OnSkillBtn, 0, 0);
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
		{
			propBtn.gameObject.SetActive(false);
			skillBtn.gameObject.SetActive(false);
		}
	}


	private void OnPropBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MainBabyPanle.ShowMe ();
	}

	private void OnBookBtn(ButtonScript obj, object args, int param1, int param2)
	{
		//TuJianUI.ShowMe ();
	}
	private void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MainbabySkillNpc.ShowMe ();
		//MainBabyPanle.ShowMe ();
	}
}

