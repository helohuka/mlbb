using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class YaoQingUI : UIBase {

	public UILabel _TitleLable;
	public UILabel _FriendsLable;
	public UILabel _FamilyLable;
	public UILabel _NearbyLable;

	public UIButton HaoYouBtn;
	public UIButton FuJinBtn;
	public UIButton JiaZuBtn;
	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;
	public List<Avatar>otherPlayer = new List<Avatar> ();
	private List<UIButton>btns = new List<UIButton> ();
	private List<GameObject> HaoYouCellPool = new List<GameObject>();
	private List<GameObject> HaoYouCellList = new List<GameObject>();
	private List<COM_ContactInfo> friendList = new List<COM_ContactInfo>();
	void Start () {
		//setdepth ();
		item.SetActive (false);
		btns.Add (HaoYouBtn);
		btns.Add (JiaZuBtn);
		btns.Add (FuJinBtn);
		UIManager.SetButtonEventHandler(HaoYouBtn.gameObject, EnumButtonEvent.OnClick, OnClicHaoY,0, 0);
		UIManager.SetButtonEventHandler(FuJinBtn.gameObject, EnumButtonEvent.OnClick, OnClicHaoY,2, 0);
		UIManager.SetButtonEventHandler(JiaZuBtn.gameObject, EnumButtonEvent.OnClick, OnClicHaoY,1, 0);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClicClose,0, 0);
		foreach(COM_ContactInfo cinfo in FriendSystem.Instance().friends_)
		{
			friendList.Add(cinfo);
		}
		foreach(Avatar op in Prebattle.Instance.otherAvatarContainer_.Values)
		{
			otherPlayer.Add(op);
		}
		TabsSelect (0);
		SwitchMembers (0);
	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Team_Title");
		_FriendsLable.text = LanguageManager.instance.GetValue("Team_Friends");
		_FamilyLable.text = LanguageManager.instance.GetValue("Team_Family");
		_NearbyLable.text = LanguageManager.instance.GetValue("Team_Nearby");
	}
	void setdepth()
	{
		UIPanel [] panels = ApplicationEntry.Instance.uiRoot.GetComponentsInChildren<UIPanel>();
		int depth = 0;
		for(int i =0;i<panels.Length;i++)
		{
			depth+=panels[i].depth;
		}
		UIPanel selfPanel = gameObject.GetComponent<UIPanel>();
		selfPanel.depth = depth+1;
//		selfPanel.clipOffset = Vector3.zero;
//		selfPanel.transform.position = Vector3.zero;
	}
	void clearObj()
	{
		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}
	void RefreshTeam(List<COM_ContactInfo> infos,List<COM_GuildMember>GuildMembers,List<Avatar>otherPlayers)
	{

		if(infos !=null)
		{
			for(int i=0;i<infos.Count;i++)
			{
				GameObject clone = GameObject.Instantiate (item)as GameObject;
				clone.transform.parent = grid.transform;
				clone.SetActive (true);
				clone.transform.localPosition = Vector3.zero;
				clone.transform.localScale = Vector3.one;
				HaoYouCell listCell = clone.GetComponent<HaoYouCell>();
				listCell.ContactInfo = infos[i];
			}
		}else
			if(GuildMembers !=null)
		{
			for(int i=0;i<GuildMembers.Count;i++)
			{
				if(GuildMembers[i].roleId_ == GamePlayer.Instance.InstId)
				{
					continue;
				}
				GameObject clone = GameObject.Instantiate (item)as GameObject;
				clone.transform.parent = grid.transform;
				clone.SetActive (true);
				clone.transform.localPosition = Vector3.zero;
				clone.transform.localScale = Vector3.one;
				HaoYouCell listCell = clone.GetComponent<HaoYouCell>();
				listCell.GuildMember = GuildMembers[i];
			}
		}else
			if(otherPlayers !=null)
		{
			for(int i=0;i<otherPlayers.Count;i++)
			{
				GameObject clone = GameObject.Instantiate (item)as GameObject;
				clone.transform.parent = grid.transform;
				clone.SetActive (true);
				clone.transform.localPosition = Vector3.zero;
				clone.transform.localScale = Vector3.one;
				HaoYouCell listCell = clone.GetComponent<HaoYouCell>();
				listCell.Avatarr = otherPlayers[i];
			}
		}
		
//		for(int i=0;i<HaoYouCellList.Count;i++)
//		{
//			HaoYouCellList[i].transform.parent = null;
//			HaoYouCellList[i].SetActive(false);
//			HaoYouCellPool.Add(HaoYouCellList[i]);
//		}
//		HaoYouCellList.Clear ();
//		for(int i = 0;i<infos.Count;i++)
//		{
//			GameObject clone = null;
//			if(HaoYouCellPool.Count>0)
//			{
//				clone = HaoYouCellPool[0];
//				HaoYouCellPool.Remove(clone);
//			}else
//			{
//				clone = GameObject.Instantiate (item)as GameObject;
//			}
//			clone.transform.parent = grid.transform;
//			clone.SetActive (true);
//			clone.transform.position = Vector3.zero;
//			clone.transform.localScale = Vector3.one;
//			
//			HaoYouCellList.Add(clone);
//			HaoYouCell listCell = clone.GetComponent<HaoYouCell>();
//			listCell.ContactInfo = infos[i];
//		}
		grid.repositionNow = true;
	}
	void SwitchMembers(int index)
	{
		switch(index)
		{
		case 0:
			RefreshTeam (friendList,null,null);
			break;
		case 1:
			RefreshTeam (null,GuildSystem.GuildMembers,null);
			break;
		case 2:
			RefreshTeam (null,null,otherPlayer);
			break;
		}
	}
	void OnClicClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClicHaoY(ButtonScript obj, object args, int param1, int param2)
	{
		clearObj ();
		TabsSelect (param1);
		SwitchMembers (param1);
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_YaoqingTeamPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_YaoqingTeamPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_YaoqingTeamPanel);
	}
	public override void Destroyobj ()
	{

	}
	void TabsSelect(int index)
	{
		for (int i = 0; i<btns.Count; i++) 
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}
			else
			{
				btns[i].isEnabled = true;
			}
		}
	}
}
