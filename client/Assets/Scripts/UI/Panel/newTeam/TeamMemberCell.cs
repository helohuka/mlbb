using UnityEngine;
using System.Collections;

public class TeamMemberCell : MonoBehaviour {

	public UILabel _OnLable;
	public UILabel _OffLable;
	public UILabel _LevelLable;
	public UILabel _NameLable;


	public Transform modPos;
	public UILabel huobanLable;
	public UILabel levelLabel;
	public UILabel NameLabel;
	public UILabel preLabel;
	public UISprite preSp;
	public UISprite duizhangSp;
	public GameObject heiObj;
	public UIButton OpenBtn;
	public UIButton offBtn;
	public UISprite StateSp;
	public bool isStart;
	public bool isOpen = true;
	private COM_SimplePlayerInst _player;
	public COM_SimplePlayerInst PlayerInst
	{
		set
		{
			if(value != null)
			{
				_player = value;
				NameLabel.text =_player.instName_;
				levelLabel.text = _player.properties_[(int)PropertyType.PT_Level].ToString();
				preLabel.text = Profession.get ((JobType)_player.properties_[(int)PropertyType.PT_Profession],(int)_player.properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_;
				preSp.spriteName = Profession.get ((JobType)_player.properties_[(int)PropertyType.PT_Profession],(int) _player.properties_[(int)PropertyType.PT_ProfessionLevel]).jobtype_.ToString ();
			}
		}
		get
		{
			return _player;
		}
	}
	void InitUIText()
	{
		_OnLable.text = LanguageManager.instance.GetValue("Team_On");
		_OffLable.text = LanguageManager.instance.GetValue("Team_Off");
		_LevelLable.text = LanguageManager.instance.GetValue("Team_Level1");
		_NameLable.text = LanguageManager.instance.GetValue("Team_Name");
	}
	void Start () {
		InitUIText ();
		UIManager.SetButtonEventHandler (OpenBtn.gameObject, EnumButtonEvent.OnClick, OnClickOpen, 0, 0);
		UIManager.SetButtonEventHandler (offBtn.gameObject, EnumButtonEvent.OnClick, OnClickOff, 0, 0);

	}
	void OnClickOpen(ButtonScript obj, object args, int param1, int param2)
	{
		//if(TeamSystem.IsInTeam())
		isStart = false;
		isOpen = true;
		TeamSystem.maxMembers++;
		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
		cti.type_ = TeamSystem._MyTeamInfo.type_;
		cti.name_ = TeamSystem._MyTeamInfo.name_;
		cti.pwd_ = TeamSystem._MyTeamInfo.pwd_;
		cti.maxMemberSize_ = (byte)TeamSystem.maxMembers;
		cti.minLevel_ = (ushort)TeamSystem._MyTeamInfo.minLevel_;
		cti.maxLevel_ = (ushort)TeamSystem._MyTeamInfo.maxLevel_;
		NetConnection.Instance.changeTeam (cti);
		//StateSp.spriteName = "dengdaizhong";
		if(TeamUI.UpdateMemberPositionUIOk != null)
		{
			TeamUI.UpdateMemberPositionUIOk();
		}
		offBtn.gameObject.SetActive (true);
		OpenBtn.gameObject.SetActive (false);
	}
	void OnClickOff(ButtonScript obj, object args, int param1, int param2)
	{
		isStart = true;
		TeamSystem.maxMembers--;
		isOpen = false;
		COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
		cti.type_ = TeamSystem._MyTeamInfo.type_;
		cti.name_ = TeamSystem._MyTeamInfo.name_;
		cti.pwd_ = TeamSystem._MyTeamInfo.pwd_;
		cti.maxMemberSize_ = (byte)TeamSystem.maxMembers;
		cti.minLevel_ = (ushort)TeamSystem._MyTeamInfo.minLevel_;
		cti.maxLevel_ = (ushort)TeamSystem._MyTeamInfo.maxLevel_;
		NetConnection.Instance.changeTeam (cti);
		//StateSp.spriteName = "yiguanbi";
//		if(TeamUI.UpdateMemberPositionUIOk != null)
//		{
//			TeamUI.UpdateMemberPositionUIOk();
//		}
		OpenBtn.gameObject.SetActive (true);
		offBtn.gameObject.SetActive (false);
	}

	public void RestMemberCellInfo()
	{
		NameLabel.text = "";
		levelLabel.text = "";
		preLabel.text = "";
		preSp.spriteName = "";
		heiObj.SetActive (true);
		duizhangSp.spriteName = "";
		PlayerInst = null;

	}


    public	void HidenBtn()
	{
		heiObj.SetActive (false);
//		OpenBtn.gameObject.SetActive (false);
//		offBtn.gameObject.SetActive (false);
	}

}
