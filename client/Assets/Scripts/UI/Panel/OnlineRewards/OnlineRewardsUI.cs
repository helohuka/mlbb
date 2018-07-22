using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OnlineRewardsUI : UIBase {

	public GameObject item;
	public UIGrid grid;
	public UILabel decLable;
	public UITexture back;
	private List<GameObject> items = new List<GameObject>();
	void Start () {
		item.SetActive (false);
		addItem ();
		GlobalInstanceFunction.Instance.UpdateOnlineTime += UpdateOnlineTime;
		GamePlayer.Instance.OnOnlineUpdate = OnOnlineUpdate;
		UpdateOnlineTime(MoreActivityData.onlineTime);
		OnOnlineUpdate ();
		decLable.text = LanguageManager.instance.GetValue ("huodongneiyong");
		HeadIconLoader.Instance.LoadIcon("zaixianjiangli1",back);
	}
	void UpdateOnlineTime(float time)
	{
		for(int i =0;i<items.Count;i++)
		{
			OnlineRewardCell onlinecell = items[i].GetComponent<OnlineRewardCell>();
			if((int)time >= onlinecell.TimeReawData._time)
			{
				onlinecell.enterBtn.isEnabled = true;
			}else
			{
				onlinecell.enterBtn.isEnabled = false;
			}
		}
	}
	void addItem()
	{
		foreach(TimerReawData td in TimerReawData.GetData().Values)
		{
			GameObject go = GameObject.Instantiate(item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			OnlineRewardCell onlinecell = go.GetComponent<OnlineRewardCell>();
			onlinecell.TimeReawData = td;
			items.Add(go);
		}
		grid.Reposition ();
	}
	void OnOnlineUpdate()
	{
		for(int i =0;i<items.Count;i++)
		{
			OnlineRewardCell onlinecell = items[i].GetComponent<OnlineRewardCell>();
			for(int j =0;j<GamePlayer.Instance.onlineTimeRewards_.Count;j++)
			{
				if(onlinecell.TimeReawData._Id == GamePlayer.Instance.onlineTimeRewards_[j])
				{
					onlinecell.enterBtn.gameObject.SetActive(false);
					onlinecell.sp.gameObject.SetActive(true);
				}
			}
//			if(i<GamePlayer.Instance.onlineTimeReward_)
//			{
//
//			}else
//			{
//				onlinecell.enterBtn.gameObject.SetActive(true);
//				onlinecell.sp.gameObject.SetActive(false);
//			}
		}
	}
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_OnlinePanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_OnlinePanel);
	}
	void OnDestroy()
	{
		GlobalInstanceFunction.Instance.UpdateOnlineTime -= UpdateOnlineTime;
		GamePlayer.Instance.OnOnlineUpdate = null;
	}
	public override void Destroyobj ()
	{

	}
}
