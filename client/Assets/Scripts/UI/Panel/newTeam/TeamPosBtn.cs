using UnityEngine;
using System.Collections;

public class TeamPosBtn : MonoBehaviour {

	public UIButton KaiBtn;
	public UIButton guanBtn;
	public bool isStart;
	void Start () {
//		KaiBtn.gameObject.SetActive (false);
//		guanBtn.gameObject.SetActive (true);
		UIManager.SetButtonEventHandler (KaiBtn.gameObject, EnumButtonEvent.OnClick, OnClicCloseKai, 0, 0);
		UIManager.SetButtonEventHandler (guanBtn.gameObject, EnumButtonEvent.OnClick, OnClicGuan, 0, 0);
	}
	void OnClicCloseKai(ButtonScript obj, object args, int param1, int param2)
	{
		isStart = false;
		TeamSystem.maxMembers++;
		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
//		cti.type_ = TeamUIPanel.teamInfo_.type_;
//		cti.name_ = TeamUIPanel.teamInfo_.name_;
//		cti.pwd_ = TeamUIPanel.teamInfo_.pwd_;
		cti.maxMemberSize_ = (byte)TeamSystem.maxMembers;
//		cti.minLevel_ = (ushort)TeamUIPanel.teamInfo_.minLevel_;
//		cti.maxLevel_ = (ushort)TeamUIPanel.teamInfo_.maxLevel_;
		NetConnection.Instance.changeTeam (cti);
		guanBtn.gameObject.SetActive (true);
		KaiBtn.gameObject.SetActive (false);
	}
	void OnClicGuan(ButtonScript obj, object args, int param1, int param2)
	{
		isStart = true;
		TeamSystem.maxMembers--;

		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
//		cti.type_ = TeamUIPanel.teamInfo_.type_;
//		cti.name_ = TeamUIPanel.teamInfo_.name_;
//		cti.pwd_ = TeamUIPanel.teamInfo_.pwd_;
		cti.maxMemberSize_ = (byte)TeamSystem.maxMembers;
//		cti.minLevel_ = (ushort)TeamUIPanel.teamInfo_.minLevel_;
//		cti.maxLevel_ = (ushort)TeamUIPanel.teamInfo_.maxLevel_;
		NetConnection.Instance.changeTeam (cti);
		KaiBtn.gameObject.SetActive (true);
		guanBtn.gameObject.SetActive (false);
	}
	public void HidenBtn()
	{
		KaiBtn.gameObject.SetActive (false);
		guanBtn.gameObject.SetActive (false);
	}

}
