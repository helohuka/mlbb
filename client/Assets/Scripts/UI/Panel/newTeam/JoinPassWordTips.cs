using UnityEngine;
using System.Collections;

public class JoinPassWordTips : MonoBehaviour {

	public UIButton enterBtn;
	public UIButton canelBtn;
	public UIInput passWordInput;
	void Start () {
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		UIManager.SetButtonEventHandler (canelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcanelBtn, 0, 0);
		TeamSystem.OnInitMyTeam += HideSelf;
	}
	void HideSelf()
	{
		TeamSystem.OnInitMyTeam -= HideSelf;
		FastTeamPanel.HideMe ();
	}
	void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.joinTeam ((uint)ListTeamCell.teamId,passWordInput.value);


	}
	void OnClickcanelBtn(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
