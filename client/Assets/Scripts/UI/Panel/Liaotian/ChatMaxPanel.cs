using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatMaxPanel : MonoBehaviour {

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
	
	private static ChatMaxPanel _chatPanle = null;
	public static ChatMaxPanel Instance 
	{
		get{
			return _chatPanle;
		}
	}
	
	void Start () 
	{
		_chatPanle = this;
		Item.SetActive (false);
		MaxCount = 3;
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
		chatPanel.transform.parent.gameObject.SetActive (true);
		gameObject.SetActive (false);
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

	
	public	void UpdataChatItem(List<COM_ChatInfo> ChatInfos)
	{
		
		for (int k =0; k<items.Length; k++) 
		{
			items[k].gameObject.SetActive(false);	
		}
		
		for(int i=ChatInfos.Count-1,j=0;i>=0&&j<items.Length;i--,j++)
		{
			//if(!ChatSystem.instance.GetTypeIsOPen(ChatInfos[i].ck_))
			//	continue;
			ShowLineCell cell = items[j].GetComponent<ShowLineCell>();
			if (ChatInfos[i].audio_.Length > 0)
			{
				cell.isVec = true;	
			}
			else
			{
				cell.isVec = false;	
			}
			items[j].gameObject.SetActive(true);
			cell.ChatInfo = ChatInfos[i];
		}										
		
		
	}

}
