using UnityEngine;
using System.Collections;

public class LookTips : MonoBehaviour {

	public UILabel nameLable;
	public UILabel zhuzhangLabel;
	public UILabel expLabel;
	public UILabel levelLabel;
	public UILabel chengyuanLabel;
	public UILabel numLabel;
	public UILabel gonggaoLabel;
	public UIButton closeBtn;
	private COM_GuildViewerData _guildViewerData;
	public COM_GuildViewerData GuildViewerData
	{
		set{
			if(value != null)
			{
				_guildViewerData = value;
				numLabel.text = _guildViewerData.guid_.ToString();
				levelLabel.text = _guildViewerData.level_.ToString();
				nameLable.text = _guildViewerData.guildName_.ToLower();
				zhuzhangLabel.text = _guildViewerData.playerName_.ToLower();
				chengyuanLabel.text = _guildViewerData.memberNum_ +"/"+_guildViewerData.memberTotal_;
				gonggaoLabel.text = _guildViewerData.notice_;
				expLabel.text = "0";
			}
			
		}
		get
		{
			return _guildViewerData;
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
