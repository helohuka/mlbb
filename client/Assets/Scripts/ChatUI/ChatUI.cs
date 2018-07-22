using UnityEngine;
//using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatUI : UIBase
{

	public GameObject minChatObj;
	public GameObject maxChatObj;
	public static ChatUI ChatUIInstance;
	void Awake()
	{
		ChatUIInstance = this;
	}
	public static ChatUI Instance
	{
		get
		{
			return ChatUIInstance;	
		}
	}
    void Start()
    {

		DontDestroyOnLoad (this);
       // __DevelopCase();
    }

    void __DevelopCase()
    {
        for (int i = 0; i < 200; ++i)
        {

            int r = Random.RandomRange(0, 4);
            if (r == 0)
            {
                ChatSystem.PushSystemMessage("" + i + "这个是测试系统平道哦！！！！");
            }
            else if (r == 1)
            {
                COM_ChatInfo cci = new COM_ChatInfo();
                cci.ck_ = ChatKind.CK_World;
                cci.assetId_ = 2;
                cci.content_ = "" + i + "这个是测试World平道哦！！！！";
                ChatSystem.PushRecord(cci);
                ChatSystem.PushPlayerSay(cci);

            }
            else if (r == 2)
            {
                COM_ChatInfo cci = new COM_ChatInfo();
                cci.ck_ = ChatKind.CK_Team;
                cci.assetId_ = 2;
                cci.content_ = "" + i + "这个是测试Team平道哦！！！！";

                ChatSystem.PushPlayerSay(cci);
                ChatSystem.PushRecord(cci);
            }
            else if (r == 3)
            {

                COM_ChatInfo cci = new COM_ChatInfo();
                cci.ck_ = ChatKind.CK_Guild;
                cci.assetId_ = 2;
                cci.content_ = "" + i + "这个是测试Guild平道哦！！！！";
                
                ChatSystem.PushPlayerSay(cci);
                ChatSystem.PushRecord(cci);
            }
            
        }
    }
	public void SwitchChatObjActive()
	{
		if(maxChatObj.activeSelf)
		{
			maxChatObj.SetActive (false);
			minChatObj.SetActive (true);
		}
		else
		{
			maxChatObj.SetActive (true);
			minChatObj.SetActive (false);
		}
	}

	public bool BigChatOpen()
	{
		return maxChatObj.activeSelf;
	}

	public void Close()
	{
		gameObject.SetActive (false);
	}

	public void open()
	{
		gameObject.SetActive (true);
	}


	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ChatUI);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ChatUI);
	}
	public override void Destroyobj ()
	{

	}
}