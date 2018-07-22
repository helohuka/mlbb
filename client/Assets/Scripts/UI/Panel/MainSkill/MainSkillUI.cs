using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainSkillUI : UIBase {

 
	public	UILabel zhiyeTitle;
	public UILabel description;
	public UILabel skillName;
	public UILabel skillLevel;
	public UISprite skillIcon;
	public GameObject item;
	public	List<int> skillIds = new List<int> ();
	public UIButton closeBtn;
	private List<GameObject> items = new List<GameObject> ();
	private List<SkillData> skillist = new List<SkillData> ();
	public UIGrid grid;
	private UIEventListener Listener;


	void Start () {
		item.SetActive (false);
		InitDada ();
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseBtn, 0, 0);
	}

	void AddScrollViewItem(List<COM_Skill> skills)
	{
		if (item == null)
			return;
        for (int i = 0; i < skills.Count; i++)
        {
            SkillData skdata = SkillData.GetData((int)skills[i].skillID_,(int)skills[i].skillLevel_);
			GameObject o  = Instantiate(item)as GameObject;
			o.SetActive(true);
			o.transform.parent = grid.transform;
			UISprite []sp = o.GetComponentsInChildren<UISprite>();
			UILabel []las = o.GetComponentsInChildren<UILabel>();
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("skillNameLabel"))
				{
                    la.text = skdata._Name;
				}
				if(la.gameObject.name.Equals("levelnumLabel"))
				{
                    la.text = skills[i].skillExp_.ToString();
				}
			}

			for(int j=0; j < sp.Length; ++j)
			{
				if(!sp[j].gameObject.name.Equals("suo"))
					continue;
				string iconName = skdata._ResIconName;
				if(!string.IsNullOrEmpty(iconName))
					sp[j].spriteName = iconName;
			}

			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			Listener = UIEventListener.Get(o);
			Listener.onClick += ButtonClick;
			Listener.parameter = skills[i].skillID_;
		
			items.Add(o);
			grid.repositionNow = true;
		}

		if(items != null && items.Count > 0)
			ButtonClick(items[0]);
	}

	void ShowSkillInfo(int sid)
	{
		for(int i=0;i< skillist.Count;i++)
		{
			if(skillist[i]._Id==sid)
			{
				description.text = skillist[i]._Desc;
				skillName.text = skillist[i]._Name;
				skillLevel.text = skillist[i]._Level.ToString();
				string iconName = "hb_jinengkuang";
				if(!string.IsNullOrEmpty(skillist[i]._ResIconName))
					iconName = skillist[i]._ResIconName;
				skillIcon.spriteName = iconName;
			}
		}

	}
	void ButtonClick(GameObject sender)
	{

		int sid = (int)UIEventListener.Get(sender).parameter;
		ShowSkillInfo (sid);

	}

	void InitDada()
	{
		zhiyeTitle.text = "";
		
		AddScrollViewItem( GamePlayer.Instance.SkillInsts);
	}

	public void  RefreshData()
	{
		skillIds.Clear ();
		skillist.Clear ();
		for(int i = 0;i<items.Count; i++)
		{
			GameObject.Destroy(items[i]);
		}
		items.Clear ();
		InitDada ();
	}



	public override void Destroyobj ()
	{

	}
	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_SkillView);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_SkillView);
	}
	
	void OnClickCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	
	#endregion
}
