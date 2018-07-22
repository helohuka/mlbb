using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DailyTaskUI : MonoBehaviour {

	public GameObject RewardItem;
	public UIGrid RewardGrid;
	public GameObject qustItem;
	public UIGrid qustGrid;
	public UIButton CloseBtn;

	public UILabel timeLable;
	public UILabel GbLabel;
	public delegate void ActivityEvent();
	public static ActivityEvent ActivityEventOk;
	private List<GameObject>ustItemList = new List<GameObject> ();
	private List<ACT_RewardData> actRewData = new List<ACT_RewardData>();
	private List<ActivityData> actData = new List<ActivityData>();
	void Start () {

		RewardItem.SetActive (false);
		qustItem.SetActive (false);
		foreach(KeyValuePair<int, ACT_RewardData> pair in ACT_RewardData.GetData())
		{
			actRewData.Add(pair.Value);
		}
		foreach(KeyValuePair<int, ActivityData> pair in ActivityData.GetData())
		{
			actData.Add(pair.Value);
		}
		ActivityEventOk = PlayerActivityEventOk;
		AddRewardItem (actRewData);
		AddQustItem (actData);
		PlayerActivityEventOk ();

	
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{

	}

	public void Hide()
	{
		if(MainPanle.Instance != null)
		{
			MainPanle.Instance.HidenDaTask();
		}

		ActivityEventOk = null;
	}

	void PlayerActivityEventOk()
	{
		int gb = 0;
		COM_ActivityTable table = GamePlayer.Instance.ActivityTable;
		if (table == null)
			return;
		//GbLabel.text = GamePlayer.Instance.ActivityTable.reward_.ToString();
		GbLabel.text = table.reward_.ToString ();
		for(int i =0;i<ustItemList.Count;i++)
		{
			DailyQustCell aCell = ustItemList[i].GetComponent<DailyQustCell>();
			for(int j =0;j<table.activities_.Length;j++)
			{

				if(aCell.Adata._Id == table.activities_[j].actId_)
				{
					 
//					ActivityData adata = ActivityData.GetData((int)table.activities_[j].actId_);
//					gb+=adata.reward_;
//					GbLabel.text = gb.ToString();
					aCell.RefreshFinishProgress(table.activities_[j].counter_);
				}
			}
		}

	}


	void Update () {
		timeLable.text = (23 - System.DateTime.Now.Hour) + "小时" + (60-System.DateTime.Now.Minute)+"分";
	}
	public void AddRewardItem(List<ACT_RewardData> acRDatas)
	{
		for(int i =0;i<acRDatas.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(RewardItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = RewardGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			DailyRewarCell aCell = clone.GetComponent<DailyRewarCell>();
			aCell.Adata = acRDatas[i];
			RewardGrid.repositionNow = true;
		}
	}
	public void AddQustItem(List<ActivityData> actDatas)
	{
		for(int i =0;i<actDatas.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(qustItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = qustGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			DailyQustCell aCell = clone.GetComponent<DailyQustCell>();
			aCell.Adata = actDatas[i];
			ustItemList.Add(clone);
			qustGrid.repositionNow = true;
		}
	}

	void OnDestroy()
	{
        ActivityEventOk = null;
	}
}
