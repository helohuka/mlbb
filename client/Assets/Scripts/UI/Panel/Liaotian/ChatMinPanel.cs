using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatMinPanel : MonoBehaviour {

	public GameObject TipsObj;
	public GameObject chatPanel;
	public GameObject Item;
	public UIButton AreBtn;
	public UIButton SetBtn;
	public UIGrid grid;

	public GameObject[]items;

	private List<COM_ChatInfo>chatInfos = new List<COM_ChatInfo> ();
	private int MaxCount;
	private List<GameObject> mlist = new List<GameObject>();

	private static ChatMinPanel _chatPanle = null;
	public static ChatMinPanel Instance 
	{
		get{
			return _chatPanle;
		}
	}

	void Start () 
	{
		_chatPanle = this;
		Item.SetActive (false);
		MaxCount = 30;
		//ReceiveChat (ChatSystem.ChatInfos);

		//ChatSystem.instance.ChatPanelOk  += ReceiveChat;
		UIManager.SetButtonEventHandler (AreBtn.gameObject, EnumButtonEvent.OnClick, OnClickAre, 0, 0);
		UIManager.SetButtonEventHandler (SetBtn.gameObject, EnumButtonEvent.OnClick, OnClickSet, 0, 0);
		//UpdataChatItem(ChatSystem.ChatInfos);

	}
	private void OnClickSet(ButtonScript obj, object args, int param1, int param2)
	{
		TipsObj.SetActive (true);
	}
	public void AddEvent()
	{
		//ChatSystem.instance.ChatPanelOk  += UpdataChatItem;
	}

	private void OnClickAre(ButtonScript obj, object args, int param1, int param2)
	{
		chatPanel.SetActive (true);
		UIPanel panel = chatPanel.transform.parent.GetComponent<UIPanel> ();
		if(panel != null)panel.depth = 10;
		gameObject.transform.parent.gameObject.SetActive (false);
	}
	public void Hide()
	{
		//ChatSystem.instance.ChatPanelOk  -= UpdataChatItem;
	}
	public void RefreshInfo()
	{
		//ChatSystem.ChatInfos.Add (info);
		//UpdataChatItem (ChatSystem.ChatInfos);
	}
	void Refresh()
	{
		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}
//	public void ReceiveChat(List<COM_ChatInfo> ChatInfos)
//	{
//		Refresh ();
//		GameObject go = null;
//		for(int i = 0;i<ChatInfos.Count;i++)
//		{
//			if (ChatInfos[i].ck_ != ChatSystem.instance.chatType)
//				continue;
//
//			GameObject clone = GameObject.Instantiate (Item)as GameObject;
//			clone.transform.parent = grid.transform;
//			clone.transform.position = Vector3.zero;
//			clone.transform.localScale = Vector3.one;
//			ShowLineCell cell = clone.GetComponent<ShowLineCell> ();
//			cell.ChatInfo = ChatInfos[i];
//			if (ChatInfos[i].audio_.Length > 0)
//			{
//				cell.isVec = true;	
//			}
//			else
//			{
//				cell.isVec = false;	
//			}
//
//			if (mlist.Count +1 >MaxCount)
//			{
//				go = mlist[0];
//				mlist.Remove(go);	
//				Destroy(go);
//			}
//
//			mlist.Add(clone);
//			grid.repositionNow = true;
//			clone.SetActive(true);
//
//			//grid.Reposition();
//		}
//
//	}

	public	void UpdataChatItem(List<COM_ChatInfo> ChatInfos)
	{

		Refresh ();
		GameObject go = null;
		for(int i = 0;i<ChatInfos.Count;i++)
		{
			GameObject clone = GameObject.Instantiate (Item)as GameObject;
			clone.transform.parent = grid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			ShowLineCell cell = clone.GetComponent<ShowLineCell> ();
			cell.ChatInfo = ChatInfos[i];
			if (ChatInfos[i].audio_.Length > 0)
			{
				cell.isVec = true;	
			}
			else
			{
				cell.isVec = false;	
			}
			
			if (mlist.Count +1 >MaxCount)
			{
				go = mlist[0];
				mlist.Remove(go);	
				Destroy(go);
			}
			
			mlist.Add(clone);
			grid.repositionNow = true;
			clone.SetActive(true);
		}




//		for (int k =0; k<items.Length; k++) 
//		{
//			items[k].gameObject.SetActive(false);	
//		}
//
//		for(int i=ChatInfos.Count-1,j=0;i>=0&&j<items.Length;i--,j++)
//		{
//			if(!ChatSystem.instance.GetTypeIsOPen(ChatInfos[i].ck_))
//				continue;
//			ShowLineCell cell = items[j].GetComponent<ShowLineCell>();
//			if (ChatInfos[i].audio_.Length > 0)
//			{
//				cell.isVec = true;	
//			}
//			else
//			{
//				cell.isVec = false;	
//			}
//			items[j].gameObject.SetActive(true);
//			cell.ChatInfo = ChatInfos[i];
//		}										
				

	}

}
