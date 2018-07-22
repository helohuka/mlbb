using UnityEngine;
using System.Collections;

public class FamilyListCell : MonoBehaviour {

	public UILabel lookLable;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel numLabel;
	public UILabel HeadsLabel;
	public UILabel membersLabel;
	public UIButton lookBtn;
	public GameObject lookObj;
	private COM_GuildViewerData _guildViewerData;
	public COM_GuildViewerData GuildViewerData
	{
		set{
			if(value != null)
			{
				_guildViewerData = value;
				numLabel.text = _guildViewerData.guid_.ToString();
				levelLabel.text = _guildViewerData.level_.ToString();
				nameLabel.text = _guildViewerData.guildName_.ToLower();
				HeadsLabel.text = _guildViewerData.playerName_.ToLower();
				membersLabel.text = _guildViewerData.memberNum_ +"/"+_guildViewerData.memberTotal_;
			}
			
		}
		get
		{
			return _guildViewerData;
		}
	}
	void Start () {
		lookLable.text = LanguageManager.instance.GetValue ("Guild_look");
		UIManager.SetButtonEventHandler(lookBtn.gameObject, EnumButtonEvent.OnClick, OnClickLook, 0, 0);
	}
	private void OnClickLook(ButtonScript obj, object args, int param1, int param2)
	{
		lookObj.SetActive (true);
		LookTips ltips = lookObj.GetComponent<LookTips>();
		ltips.GuildViewerData = GuildViewerData;
	}

}
