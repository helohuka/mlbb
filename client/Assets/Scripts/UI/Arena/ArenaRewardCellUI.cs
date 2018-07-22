using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRewardCellUI : MonoBehaviour
{
	public GameObject itemCell;
	public UILabel rankLab;
	public UISprite rankImg;
	public UILabel fenLab;
	public List<GameObject> itemObjs = new List<GameObject> ();


	void Start ()
	{

	}

	public void PvEDayReward(int reward)
	{

	}

	public void PvEWeekReward(int reward)
	{
		
	}

	public void PvPDayReward(int reward)
	{
		for(int j=0;j<itemObjs.Count;j++)
		{
			itemObjs[j].gameObject.SetActive(false);
		}
		DropData dropData = DropData.GetData (reward);
		if(dropData == null)
			return;
		List<int> rewardList = new List<int> ();
		rewardList.Add (dropData.item_1_);
		rewardList.Add (dropData.item_2);
		rewardList.Add (dropData.item_3);
		rewardList.Add (dropData.item_4);
		rewardList.Add (dropData.item_5);
		List<int> rewardNumList = new List<int> ();
		rewardNumList.Add (dropData.item_num_1_);
		rewardNumList.Add (dropData.item_num_2);
		rewardNumList.Add (dropData.item_num_3);
		rewardNumList.Add (dropData.item_num_4);
		rewardNumList.Add (dropData.item_num_5);
	//	fenLab.gameObject.SetActive (false);
		for(int i=0;i<rewardList.Count;i++)
		{
			if(rewardList[i] == 0)
				break;
			itemObjs[i].gameObject.SetActive(true);
			ItemCellUI itemcell =  UIManager.Instance.AddItemCellUI (itemObjs[i].gameObject.GetComponent<UISprite>(),(uint)rewardList[i]);
			itemObjs[i].transform.FindChild("name").GetComponent<UILabel>().text = ItemData.GetData(rewardList[i]).name_; 
			itemObjs[i].transform.FindChild("Label").GetComponent<UILabel>().text = rewardNumList[i].ToString(); 
			itemcell.showTips = true;
			itemObjs[i].SetActive(true);
			itemObjs[i].transform.localScale = Vector3.one;
		}
	}

	public void PvPWeekReward(int reward)
	{
		
	}
}

