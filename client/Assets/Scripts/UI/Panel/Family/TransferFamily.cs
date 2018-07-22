using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TransferFamily : MonoBehaviour {

	public UIButton enterBtn;
	public UIButton cancelBtn;
	public UIButton CloseBtn;
	public UIToggle DeputyToggle;
	public UIToggle mainToggle;
	private long targetId;
	private List<UIToggle> Toggles = new List<UIToggle> ();
	public List<UILabel> labels = new List<UILabel> ();
	void Start () {
		Toggles.Add (DeputyToggle);
		Toggles.Add (mainToggle);

		UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		UIManager.SetButtonEventHandler(cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcancel, 0, 0);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler(DeputyToggle.gameObject, EnumButtonEvent.OnClick, OnClickDeputyToggle, 0, 0);
		UIManager.SetButtonEventHandler(mainToggle.gameObject, EnumButtonEvent.OnClick, OnClickmainToggle, 0, 0);
		GuildSystem.UpdateMemberJobOk += UpdatememberPosition;
		UpdatememberPosition ();
	}
	private void UpdatememberPosition()
	{
		for(int i =0;i<labels.Count;i++)
		{
			labels[i].text = "空";
			Toggles[i].GetComponent<BoxCollider>().enabled = false;
		}

		for(int i =0;i<GuildSystem.GetVicePremiers().Count;i++)
		{
			labels[i].text = GuildSystem.GetVicePremiers()[i].roleName_;
			Toggles[i].GetComponent<BoxCollider>().enabled = true;
		}
	}
	private void OnClickDeputyToggle(ButtonScript obj, object args, int param1, int param2)
	{
		UIToggle tog = obj.GetComponent<UIToggle> ();
        if (GuildSystem.GetVicePremiers().Count > 0)
		{
		    targetId = GuildSystem.GetVicePremiers ()[0].roleId_;

			RadioToggle (tog);
		}else
		{
			tog.value = false;
		}
	}
	private void OnClickmainToggle(ButtonScript obj, object args, int param1, int param2)
	{
		UIToggle tog = obj.GetComponent<UIToggle> ();
        if (GuildSystem.GetVicePremiers().Count > 1)
		{
		    targetId = GuildSystem.GetVicePremiers ()[1].roleId_;
		
			RadioToggle (tog);
		}else
		{
			tog.value = false;
		}
	}
	private void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{
		if(targetId == 0)return;
		NetConnection.Instance.transferPremier((int)targetId) ;
		gameObject.SetActive (false);
	}
	private void OnClickcancel(ButtonScript obj, object args, int param1, int param2)
	{

		gameObject.SetActive (false);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void RadioToggle(UIToggle tog)
	{
		for(int i =0;i<Toggles.Count;i++)
		{
			if(tog.gameObject.name.Equals(Toggles[i].gameObject.name))
			{
				Toggles[i].value = true;
			}else
			{
				Toggles[i].value = false;
			}
		}
	}
	void OnDestroy()
	{
		GuildSystem.UpdateMemberJobOk -= UpdatememberPosition;
	}

}
