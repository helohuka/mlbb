using UnityEngine;
using System.Collections;

public class LookFamilyInfoUI : MonoBehaviour {

	public UIButton CloseBtn;
	public UILabel familyName;
	public UILabel familymem;
	public UILabel familyZ;
	public UILabel familyLevel;
	public UILabel familyNum;
	public UILabel familyBulletin;
	private COM_GuildViewerData _guildViewerData;
	public COM_GuildViewerData GuildViewerData
	{
		set{
			if(value != null)
			{
				_guildViewerData = value;
				familyNum.text = _guildViewerData.guid_.ToString();
				familyLevel.text = _guildViewerData.level_.ToString();
				familyName.text = _guildViewerData.guildName_.ToLower();
				familyZ.text = _guildViewerData.playerName_.ToLower();
				familymem.text = _guildViewerData.memberNum_ +"/"+_guildViewerData.memberTotal_;
				familyBulletin.text = _guildViewerData.notice_;
			}
			
		}
		get
		{
			return _guildViewerData;
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
