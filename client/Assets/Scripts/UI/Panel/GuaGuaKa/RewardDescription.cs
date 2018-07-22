using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardDescription : MonoBehaviour {

	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;
	private int count;
	private List<LotteryData> lotDatas = new List<LotteryData>();
	private static Dictionary<int, LotteryData> metaData;
	private List<int>keys = new List<int>();
	void Start () {
		item.gameObject.SetActive (false);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		metaData = LotteryData.GetData ();

		foreach(int key in metaData.Keys)
		{
			keys.Add(key);
		}


		for(int i = 0;i<keys.Count;i++)
		{
			LotteryData ldata = LotteryData.GetData(keys[i]);
			lotDatas.Add(ldata);
		}


		AddItems (lotDatas);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void AddItems(List<LotteryData> loDatas)
	{
		for (int i = 0; i<loDatas.Count; i++) {
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			UILabel [] las = o.GetComponentsInChildren<UILabel>(true);
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("jianliNameLabel"))
				{
					la.text = loDatas[i].RewardName_;
				}
				if(la.gameObject.name.Equals("jianliNeiRongLabel"))
				{
					la.text = loDatas[i].Win_symbol;
				}
			}
			grid.repositionNow = true;
			
		}
		
	}

}
