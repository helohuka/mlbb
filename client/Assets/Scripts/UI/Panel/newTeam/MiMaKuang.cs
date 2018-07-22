using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class MiMaKuang : MonoBehaviour {

	public UIButton DetermineBtn;
	public UIButton CancelBtn;
	public UIButton QCBtn;
	public UIInput passWordInput;
	void Start () {
		UIManager.SetButtonEventHandler (DetermineBtn.gameObject, EnumButtonEvent.OnClick, OnClickDetermine, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);
		UIManager.SetButtonEventHandler (QCBtn.gameObject, EnumButtonEvent.OnClick, OnClickQC, 0, 0);

	}
	void OnClickDetermine(ButtonScript obj, object args, int param1, int param2)
	{
		Regex reg = new Regex("^[A-Za-z0-9]{0,6}$");
		if (!reg.IsMatch(passWordInput.value))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("mimaJia"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("mimaJia"));
		}
		else
		{
			COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
		//	cti.type_ = TeamUIPanel.teamInfo_.type_;
		//	cti.name_ = TeamUIPanel.teamInfo_.name_;
			cti.pwd_ = passWordInput.text;
			cti.maxMemberSize_ = 5;
			cti.minLevel_ = (ushort)TeamSystem.minLevel;
			cti.maxLevel_ = (ushort)TeamSystem.maxLevel;
			NetConnection.Instance.changeTeam(cti);

			gameObject.SetActive (false);
		}

			

	}
	void OnClickQC(ButtonScript obj, object args, int param1, int param2)
	{

		passWordInput.value = "";
		
	}
	void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
		
	}

}
