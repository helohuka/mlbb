using UnityEngine;
using System.Collections;

public class SelectedRoleStartGame : MonoBehaviour {

	public UIButton startBtn;
	public UIButton delBtn;
	public UILabel nameLabel;
	public UILabel levelLbael;
	public UILabel OccupationLabel;
	private COM_SimpleInformation _SimpleInformation;

	public COM_SimpleInformation SimpleInformation
	{
		set
		{
			if(value != null)
			{
				_SimpleInformation = value;
				nameLabel.text = _SimpleInformation.instName_;
				levelLbael.text = _SimpleInformation.level_.ToString();
				OccupationLabel.text = Profession.get (_SimpleInformation.jt_, _SimpleInformation.jl_).jobName_;
			}
		}
		get
		{
			return _SimpleInformation;
		}
	}

	// Use this for initialization
	void Start () {
		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnClickEnterB, 0, 0);
		UIManager.SetButtonEventHandler (delBtn.gameObject, EnumButtonEvent.OnClick, OnClickdel, 0, 0);
	}
	private void OnClickEnterB(ButtonScript obj, object args, int param1, int param2)
	{

			NetConnection.Instance.enterGame((uint)CreatePlayerRole.roleId);
	}
	private void OnClickdel(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shanchujuese"),()=>{
			NetConnection.Instance.deletePlayer(SimpleInformation.instName_);
		});

	}

}
