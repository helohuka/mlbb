using UnityEngine;
using System.Collections;

public class FamilyCell : MonoBehaviour {


	public UILabel _Sehenqing;
	public UILabel _LookLable;


	public int Page;
	public int Index;
	public UILabel numLbel;
	public UILabel levelLabel;
	public UILabel nameLabel;
	public UILabel HeadsNameLabel;
	public UILabel membersLabel;
	public UIButton requestBtn;
	public UIButton lookBtn;
	public GameObject lookObj;
	public  bool isrequest = true;
	private COM_GuildViewerData _guildViewerData;
	public COM_GuildViewerData GuildViewerData
	{
		set{
			if(value != null)
			{
				_guildViewerData = value;
				isrequest = GuildSystem.GetRequest (Page, Index);
				numLbel.text = _guildViewerData.guid_.ToString();
				levelLabel.text = _guildViewerData.level_.ToString();
				nameLabel.text = _guildViewerData.guildName_.ToLower();
				HeadsNameLabel.text = _guildViewerData.playerName_.ToLower();
				membersLabel.text = _guildViewerData.memberNum_ +"/"+_guildViewerData.memberTotal_;
				UILabel la = requestBtn.GetComponentInChildren<UILabel> ();
				if (isrequest)
				{

					//la.text = "已申请";
					_Sehenqing.text = LanguageManager.instance.GetValue("Guild_Yshenqing");
					requestBtn.isEnabled = false;
				}
				else
				{
					requestBtn.isEnabled = true;
					_Sehenqing.text = LanguageManager.instance.GetValue("Guild_shenqing");
					//la.text = "申请";
				}
			}

		}
		get
		{
			return _guildViewerData;
		}
	}

	void Start () {

		_LookLable.text = LanguageManager.instance.GetValue("Guild_chakan");
		UIManager.SetButtonEventHandler(requestBtn.gameObject, EnumButtonEvent.OnClick, OnClickrequest, 0, 0);
		UIManager.SetButtonEventHandler(lookBtn.gameObject, EnumButtonEvent.OnClick, OnClickLook, 0, 0);


			
	}

	public void OnClickrequest(ButtonScript obj, object args, int param1, int param2)
	{
		if(GuildSystem.battleState==0&&IsExitGuild24())
		{
			isrequest = !isrequest;
			GuildSystem.UpdateRequest (Page, Index, isrequest);
			UILabel la = obj.GetComponentInChildren<UILabel> ();
			la.text = LanguageManager.instance.GetValue("Guild_Yshenqing");
			UIButton btn = obj.GetComponent<UIButton> ();
			requestBtn.isEnabled = false;
		}
		NetConnection.Instance.requestJoinGuild (GuildViewerData.guid_);
	}
	bool IsExitGuild24()
	{
		if (GamePlayer.Instance.exitguildtime == 0)
						return true;
		System.TimeSpan ts = System.DateTime.Now - Define.TransUnixTimestamp(GamePlayer.Instance.exitguildtime);
	    if(ts.Hours>=24)
		{
			return true;
		}
		return false;
	}


	private void OnClickLook(ButtonScript obj, object args, int param1, int param2)
	{
		lookObj.SetActive (true);
		LookFamilyInfoUI ltips = lookObj.GetComponent<LookFamilyInfoUI> ();
		ltips.GuildViewerData = GuildViewerData;
	}
	public void SetrequestBtn()
	{
		isrequest =true;
		GuildSystem.UpdateRequest (Page, Index, isrequest);
		UILabel la = requestBtn.GetComponentInChildren<UILabel> ();
		la.text = LanguageManager.instance.GetValue("Guild_Yshenqing");
		UIButton btn = requestBtn.GetComponent<UIButton> ();
		btn.isEnabled = false;
	}
}
