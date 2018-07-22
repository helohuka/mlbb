using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoreCardsRewardDisplay : MonoBehaviour {

	public UIButton closeBtn;
	public UIGrid grid;
	public GameObject item;
	public UIScrollView ListView_;
	UIPanel listPanel_;
	BoxCollider listDragArea_;
	void Start () {
		item.gameObject.SetActive (false);
		listPanel_ = ListView_.gameObject.GetComponent<UIPanel>();
		listDragArea_ = ListView_.gameObject.GetComponent<BoxCollider>();
		listDragArea_.size = new Vector3 (listPanel_.GetViewSize().x,listPanel_.GetViewSize().y,0);
		addItem ();
		MoreCardsRewardData.GetRewardData ();
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	void addItem()
	{
		for(int i =0;i< MoreCardsRewardData.GetRewardData ().Count;i++)
		{
			GameObject go = GameObject.Instantiate(item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			UISprite  sp = go.GetComponent<UISprite>();
			ItemData itd = ItemData.GetData(MoreCardsRewardData.GetRewardData ()[i].itemid_);
			UILabel []las = go.GetComponentsInChildren<UILabel>();
			for(int j =0;j<las.Length;j++)
			{
				if(las[j].name.Equals("nameLabel"))
				{
					las[j].text = itd.name_;
				}
//				if(las[j].name.Equals("numLabel"))
//				{
//					las[j].text = MoreCardsRewardData.rewardItemNums[i].ToString();
//				}
			}
			//HeadIconLoader.Instance.LoadIcon (itd.icon_, sp);
			ItemCellUI itc = UIManager.Instance.AddItemCellUI(sp,(uint)MoreCardsRewardData.GetRewardData ()[i].itemid_); 
			itc.ItemCount = MoreCardsRewardData.GetRewardData ()[i].itemnum_;
			itc.showTips = true;
		}

		grid.Reposition ();
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		listDragArea_.center = listPanel_.clipOffset;
	}
}
