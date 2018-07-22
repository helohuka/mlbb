using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

/// <summary>
/// 聊天界面
/// </summary>
public class ChatPanel : MonoBehaviour 
{

    //聊天渲染prefabs
    public GameObject ChatCell;
    
    //滚动条
    public UIScrollBar scrollBar;

    //输入框
    public UIInput input;

    //背景框
    public UISprite background;

    //发送消息按钮
    public UIButton buttonSend;

	//聊天框最多显示多少数据
    public int MaxCount = 30;

    //----------------------- private --------------------//

    //聊天记录历史记录最大条数
    private const int MAXHISTORY = 128;

    //聊天GRID
    private ChatGrid grid;
    
    //聊天prefabs缓存区
    private List<GameObject> mList;

    //缓动坐标
    private TweenPosition tween;
	
    //发送消息历史记录
    private List<string> mTextHistorys;

    //记录位置
    private int mIndexHistory;

    //界面当前是否显示
    private bool mIsShow = true;

	// Use this for initialization
	void Start () 
    {
        mList = new List<GameObject>();
        mTextHistorys = new List<string>();
        grid = GetComponentInChildren<ChatGrid>();
        tween = gameObject.GetComponentInChildren<TweenPosition>();
       
        UIEventListener.Get(buttonSend.gameObject).onClick = OnSendBtnClick;

		AddSpeechInfos("Test", "{0}{1}这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
//		AddSpeechInfos("Test", "#01#02#03#04这就是聊天记录，不知道有什么问题。嘿呀，哟呼。哈哈。嘿嘿！");
	}

    void Update()
    {
        //聊天界面显示
        if (mIsShow)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                input.value = GetHistoryDown();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                input.value = GetHistoryUp();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnSendBtnClick();
            }
        }
    } 

    /// <summary>
    /// 添加系统聊天信息
    /// </summary>
    /// <param name="text"></param>
    /// <param name="bytes"></param>
    public void AddSysInfos(string name , string text , byte[] bytes = null)
    {
        Add(name, "[ff0000]", text, "[ff0000]", bytes);
    }

    /// <summary>
    /// 添加公共聊天信息
    /// </summary>
    /// <param name="text"></param>
    /// <param name="bytes"></param>
    public void AddSpeechInfos(string name , string text , byte[] bytes = null)
    {
        Add(name, "[0000ff]", text, "[ffff00]", bytes);
    }

    /// <summary>
    /// 聊天缓动结束
    /// </summary>
    private void OnTweenComplete()
    {
        if (tween.value == tween.to)
        {
            mIsShow = true;
        }
        else if (tween.value == tween.from)
        {
            mIsShow = false;
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="go"></param>
    private void OnSendBtnClick(GameObject go = null)
    {
        string text = input.value;

        if (!string.IsNullOrEmpty(text))
        {
            if (text.StartsWith("/"))
            {
                AddSysInfos("GM ",text);
            }
            else
            {
                AddSpeechInfos("Test",text);
            }

            AddHistorys(text);

            input.value = "";
            input.isSelected = false;
        }
        else
        {

            return;
        }
    }

    /// <summary>
    /// 添加内容到聊天窗口
    /// </summary>
    /// <param name="text"></param>
    private void Add(string name, string nameColor, string text, string textColor, byte[] bytes)
    {
        GameObject go;

        if (mList.Count >= MaxCount)
        {
            go = mList[0];
            mList.Remove(go);
        }
        else
        {
            go = Instantiate(ChatCell) as GameObject;
        }

        ChatItem cell = go.GetComponent<ChatItem>();
        go.transform.parent = grid.transform;
        go.transform.localScale = ChatCell.transform.localScale;
        cell.nameColor = nameColor;
        cell.infoColor = textColor;
        cell.nameText = name;
        cell.text = text;
        
        mList.Add(go);

        grid.Reposition();
        scrollBar.value = 1;
    }

    //添加聊天历史记录，用于快捷发送
    private void AddHistorys(string text)
    {
        if(!string.IsNullOrEmpty(text))
        {
            if (mTextHistorys.Contains(text))
                mTextHistorys.Remove(text);

            if (mTextHistorys.Count >= MAXHISTORY)
                mTextHistorys.Remove(mTextHistorys[0]);

            mTextHistorys.Add(text);
            mIndexHistory = mTextHistorys.Count;
        }
    }

    private string GetHistoryUp()
    {
        if (mTextHistorys.Count > 0)
        {
            mIndexHistory--;

            if (mIndexHistory < 0)
            {
                mIndexHistory = 0;
            }

            return mTextHistorys[mIndexHistory];
        }
        else
        {
            return "";
        }
    }

    private string GetHistoryDown()
    {
        if (mTextHistorys.Count > 0)
        {
            mIndexHistory++;

            if (mIndexHistory > (mTextHistorys.Count - 1))
            {
                mIndexHistory = mTextHistorys.Count - 1;
            }

            return mTextHistorys[mIndexHistory];
        }
        else
        {
            return "";
        }
    }
}
