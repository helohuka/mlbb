using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PromptPanel : MonoBehaviour {

	public UILabel levelorOccupationLabel;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UIButton EnButton;
	public UIButton CanButton;
	private List<COM_SimpleInformation> roles;
	private string name;
	void Start () {

		roles = CreatePlayerRole.GetRoles ();
		ShowPlayerInfo (CreatePlayerRole.roleId);
		UIManager.SetButtonEventHandler (EnButton.gameObject, EnumButtonEvent.OnClick, OnClickDEnButton, 0, 0);
		UIManager.SetButtonEventHandler (CanButton.gameObject, EnumButtonEvent.OnClick, OnClickCanButton, 0, 0);
	}
	void ShowPlayerInfo(int insId)
	{
		for(int i =0;i<roles.Count;i++)
		{
			if(roles[i].instId_ == insId)
			{
				nameLabel.text =  roles [i].instName_;
				levelorOccupationLabel.text =  Profession.get (roles [i].jt_, roles [i].jl_).jobName_;
				levelLabel.text = roles [i].level_.ToString();
				name = roles [i].instName_;

			}
		}
	}
	void OnClickDEnButton(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.deletePlayer (name);
		gameObject.SetActive (false);
	}
	void OnClickCanButton(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	
}
