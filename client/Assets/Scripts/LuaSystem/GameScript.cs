using System;
using UnityEngine;
using UniLua;
using System.Collections.Generic;
using System.Text;

public static class GameScript {

	public const string LIB_NAME = "GameLib.cs";

    static Dictionary<ScriptGameEvent, string> funcs_ = new Dictionary<ScriptGameEvent, string>();

    private static ILuaState L;

    public delegate void HideRenwuListHandler();
    public static event HideRenwuListHandler OnHideRenwuList;

	/// <summary>
	/// Opens the lib. Regist lib functions for script.
	/// </summary>
	/// <returns>The lib.</returns>
	/// <param name="lua">Lua.</param>
	public static int OpenLib(ILuaState lua)
	{
		NameFuncPair[] define = new NameFuncPair[]
		{
			new NameFuncPair("LoadScript", LoadScript),
			new NameFuncPair("RegistEvent", RegistEvent),
			new NameFuncPair("PushSkillAffectIndex", PushSkillAffectIndex),
			new NameFuncPair("BanOperation", BanOperation),
			new NameFuncPair("BanSkill", BanSkill),
			new NameFuncPair("DoSkill", DoSkill),

            new NameFuncPair("PushBattleScene", PushBattleScene),
            new NameFuncPair("PushFBScene", PushFBScene),

            /************************************************************************/
            /*新手指引                                                               */
            /************************************************************************/
            new NameFuncPair("CreateGuide", CreateGuide),                       //创建指引(UI)
            new NameFuncPair("CreateGuideWithItemID", CreateGuideWithItemID),   //创建指引(道具id)
            new NameFuncPair("CreateGuideWithSkillID", CreateGuideWithSkillID), //创建指引(技能id)
            new NameFuncPair("CreateGuideInScene", CreateGuideInScene),         //创建指引(场景NPC)
            new NameFuncPair("CreateGuideInBattle", CreateGuideInBattle),       //创建指引(战斗中角色)
            new NameFuncPair("CreateGuideInMainScene", CreateGuideInMainScene), //创建指引(主城中的物体)
            new NameFuncPair("ClearGuide", ClearGuide),                         //清除指引
            new NameFuncPair("GuideIsFinish", GuideIsFinish),                   //判断指引完成
            new NameFuncPair("FinishGuide", FinishGuide),                       //设置指引完成
            /************************************************************************/
            /*新手指引                                                               */
            /************************************************************************/

            new NameFuncPair("BeginTalk", BeginTalk),                   //对话UI接口
            new NameFuncPair("CloseAllSubUI", CloseAllSubUI),           //关闭所有打开的子界面
            new NameFuncPair("GetBattleID", GetBattleID),               //获取战斗id
            new NameFuncPair("SetAutoBattle", SetAutoBattle),           //开关自动战斗
            new NameFuncPair("ShowBattleReward", ShowBattleReward),     //打开战斗结算界面
            new NameFuncPair("DisplayBottomBtns", DisplayBottomBtns),   //打开最下排按钮
            new NameFuncPair("HideRenwuList", HideRenwuList),         //隐藏任务追踪面板
            new NameFuncPair("TalkedToNpc", TalkedToNpc),               //向服务器发送和哪个npc说了话

            /************************************************************************/
            /*获取玩家i属性                                                    */
            /************************************************************************/
            new NameFuncPair("GetIProp", GetIProp),

            /************************************************************************/
            /*需要LOADING提示的UI                                                    */
            /************************************************************************/
            new NameFuncPair("RegUI", RegistSlowerUI),

            /************************************************************************/
            /*获得宠物的特效和UI                                                     */
            /************************************************************************/
            new NameFuncPair("ShowGainBaby", ShowGainBabyEffect),

            /************************************************************************/
            /*强制显示摇杆                                                           */
            /************************************************************************/
            new NameFuncPair("DisplayStick", DisplayStick),

            /************************************************************************/
            /*弹出popText                                                           */
            /************************************************************************/
            new NameFuncPair("PopText", PopTextInterface),

            /************************************************************************/
            /*弹出MessageBox 接快捷购买                                              */
            /************************************************************************/
            new NameFuncPair("MessageBoxQuickUse", MessageBoxFastBuyInterface),

            /************************************************************************/
            /*传送的某个场景                                              */
            /************************************************************************/
            new NameFuncPair("TransforScene", TransforScene),


			new NameFuncPair("RegistzhizuoUI", RegistzhizuoUI),

            /************************************************************************/
            /*剧情开始                                              */
            /************************************************************************/
            new NameFuncPair("PlaySense", PlaySense),

            /************************************************************************/
            /*剧情结束                                              */
            /************************************************************************/
            new NameFuncPair("QuitSense", QuitSense),

            /************************************************************************/
            /*剧情对话                                              */
            /************************************************************************/
            new NameFuncPair("SenseTalk", SenseTalk),

            /************************************************************************/
            /*剧情继续                                              */
            /************************************************************************/
            new NameFuncPair("SenseNext", SenseNext),

            /************************************************************************/
            /*获取玩家或队伍的等级                                              */
            /************************************************************************/
            new NameFuncPair("PlayerLevel", PlayerLevel),
            new NameFuncPair("TeamMemberLevel", TeamMemberLevel),
            //是否有队员暂离
            new NameFuncPair("HasLeaveMember", HasLeaveMember),

            new NameFuncPair("ShowMsgBox", ShowMessageBox),

            //判断任务是否完成
            new NameFuncPair("IsQuestFinished", IsQuestFinished),

            //战斗中角色冒泡说话
            new NameFuncPair("BubbleTalk", BubbleTalk),

            //当前场景id
            new NameFuncPair("CurrentSceneID", CurrentSceneID),
		};
		
		lua.L_NewLib(define);

        L = lua;

        return 1;
	}

	public static void RegistGlobalEnum(Type enumInfo)
	{
		string[] vals = Enum.GetNames (enumInfo);
		for(int i=0; i < vals.Length; ++i)
		{
			LuaMaster.Instance.Lua.PushInteger(i);
			LuaMaster.Instance.Lua.SetGlobal(vals[i]);
		}
	}

    private static int checkTop_;
    public static void CheckBegin()
    {
        checkTop_ = L.GetTop();
    }

    public static bool CheckEnd()
    {
        if (checkTop_ != L.GetTop())
        {
            ClientLog.Instance.LogError("lua stack is not equal. before: " + checkTop_ + "   after: " + L.GetTop());
            L.Pop(-1);
            return false;
        }
        return true;
    }

    public static bool Call(ScriptGameEvent e, object[] paramaters, object[] results, ref string err)
    {
        CheckBegin();
        if(!funcs_.ContainsKey(e))
        {
            err = "ScriptGameEvent " + e.ToString() + "has no receiver!";
            return false;
        }
        if(LanuchFunction(funcs_[e]) == false)
            return false;

        List<object> lst = null;
        if (paramaters != null)
        {
            lst = new List<object>(paramaters);
            lst.Insert(0, (int)e);
        }
        else
        {
            lst = new List<object>();
            lst.Add((int)e);
        }
        bool ret = Call(lst.ToArray(), results, ref err);
        if (!CheckEnd())
        {
            err = "Lua Stk not clean~~";
            return false;
        }
        return ret;
    }

    public static bool Call(string func, object[] paramaters, object[] results, ref string err)
    {
        CheckBegin();
        if(LanuchFunction(func) == false)
            return false;
        List<object> lst = null;
        if (paramaters != null)
        {
            lst = new List<object>(paramaters);
            lst.Insert(0, func);
        }
        else
        {
            lst = new List<object>();
            lst.Add(func);
        }
        bool ret = Call(lst.ToArray(), results, ref err);
        if (!CheckEnd())
        {
            err = "Lua Stk not clean~~";
            return false;
        }
        return ret;
    }

    static bool LanuchFunction(string func)
    {
        if (null == func || func.Equals(""))
            return false;
        L.GetGlobal(func);
        if (false == L.IsFunction(-1))
            return false;
        return true;
    }

    public static bool Call(object[] paramaters, object[] results, ref string err)
    {
        
        ThreadStatus status = ThreadStatus.LUA_ERRERR;

        if (null == paramaters || paramaters.Length == 0)
        {
            if (null == results || results.Length == 0)
            {
                status = L.PCall(0, 0, 0);
            }
            else
            {
                status = L.PCall(0, results.Length, 0);

                for (int i = 0; i < results.Length; ++i)
                {
                    if(results[i] is int)
                    {
                        results[i] = L.ToInteger(-i - 1);
                    }
                    else if(results[i] is float)
                    {
                        results[i] = L.ToNumber(-i - 1);
                    }
                    else if(results[i] is string)
                    {
                        results[i] = L.ToString(-i - 1);
                    }
                    else 
                    {
                        results[i] = L.ToUserData(-i - 1);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < paramaters.Length; ++i)
            {
                if (paramaters[i] is int)
                {
                    L.PushInteger((int)paramaters[i]);
                }
                else if (paramaters[i] is float)
                {
                    L.PushNumber((float)paramaters[i]);
                }
                else if (paramaters[i] is string)
                {
                    L.PushString((string)paramaters[i]);
                }
                else if (paramaters[i] is Array)
                {
                    Array arr = (Array)paramaters[i];
                    if (arr.Length == 0)
                    {
                        L.PushNil();
                    }
                    else
                    {
                        L.NewTable();
                        for (int k = 0; k < arr.Length; ++k)
                        {
                            L.PushInteger(k +1);
                            if (arr.GetValue(k) is int)
                            {
                                L.PushInteger((int)arr.GetValue(k));
                            }
                            else if (arr.GetValue(k) is float)
                            {
                                L.PushNumber((float)arr.GetValue(k));
                            }
                            else if (arr.GetValue(k) is string)
                            {
                                L.PushString((string)arr.GetValue(k));
                            }
                            else 
                            {
                                L.PushNil();
                            }
                            L.SetTable(-3);
                        }
                    }
                }
                else
                {
                    L.PushLightUserData(paramaters[i]);
                }
            }

            if (null == results || results.Length == 0)
            {
                status = L.PCall(paramaters.Length, 0, 0);
            }
            else
            {
                status = L.PCall(paramaters.Length, results.Length, 0);

                for (int i = 0; i < results.Length; ++i)
                {
                    if (results[i] is int)
                    {
                        results[i] = L.ToInteger(-i - 1);
                    }
                    else if (results[i] is float)
                    {
                        results[i] = L.ToNumber(-i - 1);
                    }
                    else if (results[i] is string)
                    {
                        results[i] = L.ToString(-i - 1);
                    }
                    else if(results[i] is bool)
                    {
                        results[i] = L.ToBoolean(-i - 1);
                    }
                    else
                    {
                        results[i] = L.ToUserData(-i - 1);
                    }
                }
            }
        }
        if (results != null)
            L.Pop(results.Length * -1);

        if (ThreadStatus.LUA_OK != status)
        {
            err = L.L_ToString(-1);

            return false;
        }
        else return true;
    }

	public static int LoadScript(ILuaState lua)
	{
		int stk = 1;
        string file = L.ToString(stk);

        string str = (string)L.ToString(stk++);

        StringTool.UTF8String(ref str);

        ThreadStatus status = L.L_DoFile (file);
		if(status != ThreadStatus.LUA_OK)
		{
			string err = L.ToString(-1);
            ClientLog.Instance.LogError(err);
		}
		
		L.Pop (1);

		return 0;
	}

	public static int RegistEvent(ILuaState lua)
	{
        int stk = 1;
        int e = L.ToInteger(stk++);
        string f = L.ToString(stk++);
		
		ScriptGameEvent evt = (ScriptGameEvent)e;
		funcs_.Add (evt, f);
		return 0;
	}

	public static int PushSkillAffectIndex(ILuaState lua)
	{
		int stk = 1;
        int idx = L.ToInteger(stk++);

		Battle.Instance.SkillAffectIndexs.Add (idx);

		return 0;
	}

	public static int BanOperation(ILuaState lua)
	{
		Battle.Instance.SkipUserOperation ();
		return 0;
	}

	public static int BanSkill(ILuaState lua)
	{
		int stk = 1;
        int idx = L.ToInteger(stk++);
		Battle.Instance.DisableSkillBtn (idx);
		return 0;
	}

	public static int DoSkill(ILuaState lua)
	{
		int stk = 1;
        int skl = L.ToInteger(stk++);
		Battle.Instance.DoSkill (skl);
        return 0;
	}

    public static int CreateGuide(ILuaState lua)
    {
        int stk = 1;
        int e = L.ToInteger(stk++);
        GuideAimType gaType = (GuideAimType)e;
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
        //object obj = L.ToObject(stk++);
        string str = L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if(Mathf.Approximately(alpha, 0f))
			GuideManager.Instance.CreateMask(gaType, offsetX, offsetY, rotateType, str,step,mask);
        else
			GuideManager.Instance.CreateMask(gaType, offsetX, offsetY, rotateType, str,step,mask,alpha);
        return 0;
    }

    public static int CreateGuideWithSkillID(ILuaState lua)
    {
        int stk = 1;
        int skillId = L.ToInteger(stk++);
        GameObject skill = LearningUI.Instance.GetSkillObjByID(skillId);
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
        string str = (string)L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if (Mathf.Approximately(alpha, 0f))
			GuideManager.Instance.CreateMask(skill, offsetX, offsetY, rotateType, str,step,mask);
        else
			GuideManager.Instance.CreateMask(skill, offsetX, offsetY, rotateType, str,step,mask, alpha);
        return 0;
    }

    public static int CreateGuideWithItemID(ILuaState lua)
    {
        int stk = 1;
        int itemId = L.ToInteger(stk++);
        BagUI baguiScript = GameObject.FindObjectOfType<BagUI>();
        if (baguiScript == null)
            return 0;

        GameObject icon = baguiScript.GetItemObj(itemId);
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
		string str = (string)L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if (Mathf.Approximately(alpha, 0f))
			GuideManager.Instance.CreateMask(icon, offsetX, offsetY, rotateType, str,step,mask);
        else
			GuideManager.Instance.CreateMask(icon, offsetX, offsetY, rotateType, str,step,mask,alpha);
        return 0;
    }

    public static int CreateGuideInScene(ILuaState lua)
    {
        int stk = 1;
        int npcId = L.ToInteger(stk++);
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
		string str = (string)L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if (Mathf.Approximately(alpha, 0f))
			GuideManager.Instance.CreateMaskInScene(npcId, offsetX, offsetY, rotateType, str,step,mask);
        else
			GuideManager.Instance.CreateMaskInScene(npcId, offsetX, offsetY, rotateType, str,step,mask, alpha);
        
        return 0;
    }

    public static int CreateGuideInBattle(ILuaState lua)
    {
        int stk = 1;
        int bp = L.ToInteger(stk++);
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
		string str = L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if (Mathf.Approximately(alpha, 0f))
            GuideManager.Instance.CreateMaskInBattle(bp, offsetX, offsetY, rotateType, str,step,mask);
        else
            GuideManager.Instance.CreateMaskInBattle(bp, offsetX, offsetY, rotateType, str,step,mask, alpha);
        
        return 0;
    }

    public static int CreateGuideInMainScene(ILuaState lua)
    {
        int stk = 1;
        GuideAimType type = (GuideAimType)L.ToInteger(stk++);
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        GuidePointerRotateType rotateType = (GuidePointerRotateType)L.ToInteger(stk++);
		string str = (string)L.ToString(stk++);
        StringTool.UTF8String(ref str);
        float alpha = (float)L.ToNumber(stk++);
		bool mask = (bool)L.ToBoolean (stk++);
		int step = (int)L.ToInteger (stk++);
        if (Mathf.Approximately(alpha, 0f))
            GuideManager.Instance.CreateMaskInScene(type, offsetX, offsetY, rotateType, str,step,mask);
        else
            GuideManager.Instance.CreateMaskInScene(type, offsetX, offsetY, rotateType, str,step,mask,alpha);
        
        return 0;
    }

    public static int CloseAllSubUI(ILuaState lua)
    {
        UIManager.Instance.DoDeActive();
        return 0;
    }

    public static int ShowBattleReward(ILuaState lua)
    {
        ClearingPanel.ShowMe();
        return 0;
    }

    public static int DisplayBottomBtns(ILuaState lua)
    {
        if (GamePlayer.Instance.BottomBtnState)
            return 0;

        MainPanle.Instance.SetBottomButton(true);
        return 0;
    }

    public static int HideRenwuList(ILuaState lua)
    {
        if (OnHideRenwuList != null)
            OnHideRenwuList();
        return 0;
    }

    public static int TalkedToNpc(ILuaState lua)
    {
        int stk = 1;
        int npcid = L.ToInteger(stk++);

        NetConnection.Instance.talkedNpc(npcid);
        return 0;
    }

    public static int GetBattleID(ILuaState lua)
    {
        L.PushInteger(Battle.Instance.BattleID);
        return 1;
    }

    public static int ClearGuide(ILuaState lua)
    {
        GuideManager.Instance.ClearMask();
        return 0;
    }

    public static int GuideIsFinish(ILuaState lua)
    {
        int stk = 1;
        int idx = L.ToInteger(stk++);

        bool ret = GuideManager.Instance.IsFinish(idx);
        L.PushBoolean(ret);

        return 1;
    }

    public static int FinishGuide(ILuaState lua)
    {
        int stk = 1;
        int idx = L.ToInteger(stk++);

        GuideManager.Instance.SetFinish(idx);

        return 0;
    }

    public static int BeginTalk(ILuaState lua)
    {
        int stk = 1;
        int talkId = L.ToInteger(stk++);

        NpcRenwuUI.ShowDialogByTalk(talkId);

        return 0;
    }

    public static void SetGlobalInteger(ILuaState lua)
    {
        int stk = 1;
        int idx = L.ToInteger(stk++);
        int val = L.ToInteger(stk++);
    }

    public static int SetAutoBattle(ILuaState lua)
    {
        int stk = 1;
        bool autoBattle = L.ToBoolean(stk++);
        Battle.Instance.SetAutoBattle(autoBattle);

        return 0;
    }

    public static int GetIProp(ILuaState lua)
    {
        int stk = 1;
        PropertyType pt = (PropertyType)L.ToInteger(stk++);
        L.PushInteger(GamePlayer.Instance.GetIprop(pt));
        return 2;
    }

    public static int RegistSlowerUI(ILuaState lua)
    {
        int stk = 1;
        string uiName = L.ToString(stk++);
        UIManager.Instance.RegNeedLoading(uiName);
        return 0;
    }

	public static int RegistzhizuoUI(ILuaState lua)
	{
		int stk = 1;
		string uiName = L.ToString(stk++);
		UIManager.Instance.Regzhizuo(uiName);
		return 0;
	}

    public static int PlaySense(ILuaState lua)
    {
        GameObject cinema = GameObject.Find("Cinema(Clone)");
        if (cinema == null)
            return 0;
        int stk = 1;
        int senseIdx = L.ToInteger(stk++);
        cinema.GetComponent<CinemaManager>().PlaySense(senseIdx);
        return 0;
    }

    public static int QuitSense(ILuaState lua)
    {
        GameObject cinema = GameObject.Find("Cinema(Clone)");
        if (cinema == null)
            return 0;
        cinema.GetComponent<CinemaManager>().QuitSense();
        return 0;
    }

    public static int SenseTalk(ILuaState lua)
    {
        GameObject cinema = GameObject.Find("Cinema(Clone)");
        if (cinema == null)
            return 0;

        int stk = 1;
        SenseActorType sat = (SenseActorType)L.ToInteger(stk++);
        string talkKey = L.ToString(stk++);
        int index = L.ToInteger(stk++);
        float timeLeft = (float)L.ToNumber(stk++);
        cinema.GetComponent<CinemaManager>().ActorTalk(sat, talkKey, index, timeLeft);
        return 0;
    }

    public static int SenseNext(ILuaState lua)
    {
        GameObject cinema = GameObject.Find("Cinema(Clone)");
        if (cinema == null)
            return 0;
        cinema.GetComponent<CinemaManager>().NextSense();
        return 0;
    }

    public static int TransforScene(ILuaState lua)
    {
        int stk = 1;
        int sceneId = L.ToInteger(stk++);
        NetConnection.Instance.transforScene(sceneId);
        return 0;
    }


    public static int ShowGainBabyEffect(ILuaState lua)
    {
        int stk = 1;
        int babyId = L.ToInteger(stk++);
        GetBabyPanelUI.ShowMe(babyId);
        return 0;
    }

    public static int DisplayStick(ILuaState lua)
    {
        int stk = 1;
        float offsetX = (float)L.ToNumber(stk++);
        float offsetY = (float)L.ToNumber(stk++);
        XInput.Instance.ForceDisplayStick(new Vector2(offsetX, offsetY));

        return 0;
    }

    public static int PopTextInterface(ILuaState lua)
    {
        int stk = 1;
        string content = L.ToString(stk++);
        if(!string.IsNullOrEmpty(content))
            StringTool.UTF8String(ref content);
        string arg0 = L.ToString(stk++);
        if (!string.IsNullOrEmpty(arg0))
            StringTool.UTF8String(ref arg0);
        string arg1 = L.ToString(stk++);
        if (!string.IsNullOrEmpty(arg1))
            StringTool.UTF8String(ref arg1);
        string arg2 = L.ToString(stk++);
        if (!string.IsNullOrEmpty(arg2))
            StringTool.UTF8String(ref arg2);
        PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue(content), arg0, arg1, arg2), PopText.WarningType.WT_Tip, true);

        return 0;
    }

    public static int MessageBoxFastBuyInterface(ILuaState lua)
    {
        int stk = 1;
        string content = L.ToString(stk++);
        if (!string.IsNullOrEmpty(content))
            StringTool.UTF8String(ref content);
        int shopId = L.ToInteger(stk++);

        if (GameManager.Instance.procCheckBuff_)
        {
            GameManager.Instance.noNeedCheckBuff_ = true;
        }

        MessageBoxUI.ShowMe(content, () =>
        {
            QuickBuyUI.ShowMe(shopId);
        });

        return 0;
    }

    public static int PushBattleScene(ILuaState lua)
    {
        int stk = 1;
        string scene = L.ToString(stk++);
        if(!string.IsNullOrEmpty(scene))
            StringTool.UTF8String(ref scene);

        GlobalValue.AddBattleScene(scene);

        return 0;
    }

    public static int PushFBScene(ILuaState lua)
    {
        int stk = 1;
        string scene = L.ToString(stk++);
        if (!string.IsNullOrEmpty(scene))
            StringTool.UTF8String(ref scene);

        GlobalValue.AddFBScene(scene);

        return 0;
    }

    public static int PlayerLevel(ILuaState lua)
    {
        L.PushInteger(GamePlayer.Instance.GetIprop(PropertyType.PT_Level));
        return 1;
    }

    public static int TeamMemberLevel(ILuaState lua)
    { 
        if(!TeamSystem.IsInTeam())
		{
            L.PushNil();
        	return 1;
		}

        L.NewTable();
        COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
        for(int i=0; i < members.Length; ++i)
        {
			L.PushInteger(i +1);
            L.PushInteger((int)members[i].properties_[(int)PropertyType.PT_Level]);
			L.SetTable(-3);
        }

        return 1;
    }

    public static int HasLeaveMember(ILuaState lua)
    {
        if (!TeamSystem.IsInTeam())
        {
            L.PushBoolean(false);
            return 1;
        }

        COM_SimplePlayerInst[] members = TeamSystem.GetTeamMembers();
        for (int i = 0; i < members.Length; ++i)
        {
            if (members[i].isLeavingTeam_)
            {
                L.PushBoolean(true);
                return 1;
            }
        }
        L.PushBoolean(false);
        return 1;
    }

    //message box userData
    static int userData_;

    public static int ShowMessageBox(ILuaState lua)
    {
        int stk = 1;
        string content = L.ToString(stk++);
        userData_ = L.ToInteger(stk++);
        if (string.IsNullOrEmpty(content))
        {
            ClientLog.Instance.LogWarning("Lua API: ShowMessageBox has none string content.");
            return 0;
        }

        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue(content), MessageBoxCallBack);
        return 0;
    }

    static void MessageBoxCallBack()
    {
        string err = "";
        Call("MessageBoxOkHandler", new object[] { userData_ }, null, ref err);
    }

    public static int IsQuestFinished(ILuaState lua)
    {
        int stk = 1;
        int questId = L.ToInteger(stk++);
        L.PushBoolean(QuestSystem.IsComplate(questId));
        return 1;
    }

    public static int BubbleTalk(ILuaState lua)
    {
        int stk = 1;
        int bp = L.ToInteger(stk++);
        int talkid = L.ToInteger(stk++);

        BattleActor bactor = Battle.Instance.GetActorByIdx(bp);
        if (bactor != null && bactor.ControlEntity != null && bactor.ControlEntity.PlayerInfoUI != null)
        {
            TalkData td = TalkData.GetData(talkid);
            if (td != null)
            {
                if(td.Content.Count > 0)
                    bactor.ControlEntity.PlayerInfoUI.GetComponent<Roleui>().ChatBubble(td.Content[0].second);
            }
        }
        return 0;
    }

    public static int CurrentSceneID(ILuaState lua)
    {
        L.PushInteger(GameManager.SceneID);
        return 1;
    }
}

public static class Sys
{
    public const string LIB_NAME = "Sys";

    static ILuaState L;

    public static int OpenLib(ILuaState lua)
    {
        NameFuncPair[] define = new NameFuncPair[]
		{
            new NameFuncPair("log", Log),
		};

        lua.L_NewLib(define);

        L = lua;

        return 1;
    }

    public static int Log(ILuaState l)
    {
        string str = L.ToString(1);
        StringTool.UTF8String(ref str);
        ClientLog.Instance.Log(str);
        return 0;
    }
}

public static class Global_
{
    public const string LIB_NAME = "Global";

    static ILuaState L;

    public static int OpenLib(ILuaState lua)
    {
        NameFuncPair[] define = new NameFuncPair[]
		{
			new NameFuncPair("setString", SetString),
			new NameFuncPair("setFloat", SetFloat),
			new NameFuncPair("setInt", SetInt),
		
		};

        lua.L_NewLib(define);

        L = lua;

        return 1;
    }

    public static int SetString(ILuaState lua)
    {
        Constant idx = (Constant)L.ToInteger(1);
        string val = L.ToString(2);

        GlobalValue.Set(idx, val);

        return 0;
    }

    public static int SetFloat(ILuaState lua)
    {
        Constant idx = (Constant)L.ToInteger(1);
        float val = (float)L.ToNumber(2);

        GlobalValue.Set(idx, val);

        return 0;
    }

    public static int SetInt(ILuaState lua)
    {
        Constant idx = (Constant)L.ToInteger(1);
        int val = L.ToInteger(2);

        GlobalValue.Set(idx, val);

        return 0;
    }
}