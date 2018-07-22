using UnityEngine;
using System.Collections;

public class OtherFamilyUI : UIBase {

	public UILabel _NumberingLable;
	public UILabel _LevelLable;
	public UILabel _NameLable;
	public UILabel _LeaderLable;
	public UILabel _MemberLable;
	public UILabel _OperatingLable;
	public UIButton CloseBtn;
	public UIButton leftBtn;
	public UIButton rightBtn;
	public UILabel pageLabel;
	public GameObject []items;
	private int curpage;
	private int maxapge;
	void Start () {
		InitUITex ();
		NetConnection.Instance.queryGuildList ((short)curpage);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler(leftBtn.gameObject, EnumButtonEvent.OnClick, OnClickleft, 0, 0);
		UIManager.SetButtonEventHandler(rightBtn.gameObject, EnumButtonEvent.OnClick, OnClickright, 0, 0);
		GuildSystem.QueryGuildListResultOk += RefreshItemDatas;
		for(int i = 0;i<items.Length;i++)
		{
			items[i].SetActive(false);
		}
		
		pageLabel.text = 0+"/"+0;
	}
	void InitUITex()
	{
		_NumberingLable.text = LanguageManager.instance.GetValue("Guild_Numbering");
		_LevelLable.text = LanguageManager.instance.GetValue("Guild_Level");
		_NameLable.text = LanguageManager.instance.GetValue("Guild_Name");
		_LeaderLable.text = LanguageManager.instance.GetValue("Guild_Leader");
		_MemberLable.text = LanguageManager.instance.GetValue("Guild_Member");
		_OperatingLable.text = LanguageManager.instance.GetValue("Guild_Operating");
	}
	void RefreshItemDatas(short page, short pageNum, COM_GuildViewerData[] Datas)
	{
		if(pageNum==0)
		{
			pageLabel.text = page + "/" + pageNum;
		}else
		{
			pageLabel.text = (page+1) + "/" + pageNum;
		}

		maxapge = (int)pageNum;
		for(int i = 0;i<items.Length;i++)
		{
			items[i].SetActive(false);
		}
		
		for(int j =0;j<Datas.Length;j++)
		{
			if(Datas.Length>items.Length)
				break;
			items[j].SetActive(true);
			FamilyListCell fcell = items[j].GetComponent<FamilyListCell>();
			fcell.GuildViewerData = Datas[j];
		}
	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	private void OnClickleft(ButtonScript obj, object args, int param1, int param2)
	{
		if (curpage == 0)
			return;
		curpage--;
		NetConnection.Instance.queryGuildList ((short)curpage);
	}
	private void OnClickright(ButtonScript obj, object args, int param1, int param2)
	{
		if(curpage==maxapge)
		{
			return;
		}
		curpage++;
		NetConnection.Instance.queryGuildList ((short)curpage);
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_LookFamilyPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_LookFamilyPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_LookFamilyPanel);
	}
	public override void Destroyobj ()
	{
		GuildSystem.QueryGuildListResultOk -= RefreshItemDatas;
	}
}
