using System.Collections.Generic;
using System.Collections;
using System;
public class ARPCProxy : Server2ClientProxy
{

    public delegate void SessionEvent();
    public static SessionEvent OnSessionFailed;
	
	public delegate  void AddPlayerTitleEventHandler(int t);
	public static event  AddPlayerTitleEventHandler OnAddPlayerTitle;
    //public  delegate void PropUpdateHandler (COM_PropValue prop);
    //public static event PropUpdateHandler OnPropUpdate;



    public bool pong()
    {
        GameManager.Instance.GotPong();
        return true;
    }

    public bool errorno(ErrorNo errorCode)
    {
        ClientLog.Instance.Log(errorCode.ToString());
        PopText.Instance.Show(LanguageManager.instance.GetValue(errorCode.ToString()), PopText.WarningType.WT_Tip, true);
        if (errorCode == ErrorNo.EN_BagFull)
        {
            if (GamePlayer.Instance.isInBattle)
            {
                BagSystem.instance.isBattlebagfull = true;
            }
            else
            {
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("bagfullsort"), () =>
                {
                    BagUI.SwithShowMe();
                });
            }
        }
		if(errorCode == ErrorNo.EN_GuildBattleTimeout2)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chuansongGuildBattle"), () =>
			                    {
//				if(GuildSystem.IsInGuild())
//				{
//					if(GuildSystem.Mguild.buildings_[(int)GuildBuildingType.GBT_Main].level_<2)
//					{
//						ClientLog.Instance.LogError("ddsfsdf"+GuildSystem.Mguild.buildings_[(int)GuildBuildingType.GBT_Main].level_);
//						PopText.Instance.Show(LanguageManager.instance.GetValue("dengjibuzu"));
//						return;
//					}
//				}
				NetConnection.Instance.transforGuildBattleScene ();
			});
		}
        return true;
    }

    public bool sessionfailed()
    {
        if (OnSessionFailed != null)
            OnSessionFailed();
        return true;
    }

    public bool reconnection(COM_ReconnectInfo recInfo)
    {
        Configure.Sessionkey = recInfo.sessionKey_;
        CreatePlayerRole.SetRoles(recInfo.roles_);
        GuideManager.Instance.Init(recInfo.playerInst_.guideIdx_);
        GameManager.Instance.GamePlayerInfoReset();

        //
        //if (GameManager.SceneID == 0 && recInfo.reconnectProcess_ != ReconnectType.RECT_LoginOk)
        //{
        //    GameManager.SceneID = PlayerData.GetData((int)recInfo.playerInst_.properties_[(int)PropertyType.PT_TableId]).defaultSceneId_;
        //}
        //

        ///这个是非常处理~~~~！！！！！ 后端没有 enter scene 状态 , 只要enter game 必然在一个场景中， 这里主要区分 主城 与其他场景的差异 ， 历史设计遗漏 ，喷策划·····
        if (recInfo.reconnectProcess_ == ReconnectType.RECT_EnterGameOk)
        {
            if (recInfo.playerInst_.sceneId_ != 1)
            {
                recInfo.reconnectProcess_ = ReconnectType.RECT_EnterSceneOk;
            }
        }

        //GameManager.Instance.CreateTeam(recInfo.team_);

        //Prebattle.Instance.SetCachePos(recInfo.x_, recInfo.y_);
        if (recInfo.team_.teamId_ != 0) //有队伍判断
            TeamSystem.InitMyTeam(recInfo.team_);
        //ClientLog.Instance.Log("Reconnection: " + recInfo.reconnectProcess_.ToString());
        switch (recInfo.reconnectProcess_)
        {
            case ReconnectType.RECT_LoginOk:
                StageMgr.LoadingAsyncScene(GlobalValue.StageName_CreateRoleScene, SwitchScenEffect.LoadingBar, true, false, false, true);
                break;
            case ReconnectType.RECT_EnterGameOk:
            //GamePlayer.Instance.SetPlayer(recInfo.playerInst_);
            ////initBabies(recInfo.playerInst_.babies1_);
            //StageMgr.OnSceneLoaded += LoadedSceneCallBack;
            //StageMgr.LoadingAsyncScene(GlobalValue.StageName_MainScene);
            //break;
            case ReconnectType.RECT_EnterSceneOk:
                GamePlayer.Instance.SetPlayer(recInfo.playerInst_);
                //initBabies(recInfo.playerInst_.babies1_);
                ClientLog.Instance.Log("reconnection enter scene");
                GameManager.SceneID = (int)recInfo.playerInst_.sceneId_;
                Prebattle.Instance.EnterSceneOk(new UnityEngine.Vector3(recInfo.playerInst_.scenePos_.x_, 0f, recInfo.playerInst_.scenePos_.z_), true);
                // add npc 
                for (int i = 0; i < recInfo.sceneInfo_.npcs_.Length; ++i)
                {
                    Prebattle.Instance.AddNpc(recInfo.sceneInfo_.npcs_[i]);
                }
                // add other
                for (int i = 0; i < recInfo.sceneInfo_.players_.Length; ++i)
                {
                    Prebattle.Instance.AddOther(recInfo.sceneInfo_.players_[i]);
                }
                break;
            case ReconnectType.RECT_EnterBattleOk:
                GameManager.SceneID = (int)recInfo.playerInst_.sceneId_;
                GamePlayer.Instance.SetPlayer(recInfo.playerInst_);
                //initBabies(recInfo.playerInst_.babies1_);
                RecconnectionEnterBattle(recInfo.initBattle_);
                Prebattle.Instance.cachedPosition_ = new UnityEngine.Vector3(recInfo.playerInst_.scenePos_.x_, 0f, recInfo.playerInst_.scenePos_.z_);
                // add npc 
                for (int i = 0; i < recInfo.sceneInfo_.npcs_.Length; ++i)
                {
                    Prebattle.Instance.AddNpc(recInfo.sceneInfo_.npcs_[i]);
                }
                // add other
                for (int i = 0; i < recInfo.sceneInfo_.players_.Length; ++i)
                {
                    Prebattle.Instance.AddOther(recInfo.sceneInfo_.players_[i]);
                }
                break;
        }
        CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.LOGIN);
        return true;
    }

    public bool loginok(string sessionkey, COM_SimpleInformation[] roles)
    {
        ClientLog.Instance.Log("loginok");
        Configure.Sessionkey = sessionkey;
        CreatePlayerRole.SetRoles(roles);

        StageMgr.LoadingAsyncScene(GlobalValue.StageName_CreateRoleScene);
        // reset data
        CreatePlayerRole.Reset();
        GlobalInstanceFunction.Instance.Clear();
        Battle.Instance.NewOne();
        Prebattle.Instance.Fini();
        return true;
    }

    public bool logoutOk()
    {
		CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.REGISTER);
        return true;
    }

    public bool createPlayerOk(COM_SimpleInformation player)
    {
        //AnimationCallBack.playerId = player.asset_id_;
        GamePlayer.Instance.isCreate = true;
        NetConnection.Instance.enterGame((uint)player.instId_);
        return true;
    }

    public bool enterGameOk(COM_PlayerInst inst)
    {
		TransferRate._Inst.Send("Final Enter Game");
        GuideManager.Instance.Init(inst.guideIdx_);
        GamePlayer.Instance.SetPlayer(inst);
        CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.LOGIN);
        GamePlayer.Instance.isCreate = false;
		NetConnection.Instance.requestEmployees ();
        return true;
    }

    void LoadedSceneCallBack(string sceneName)
    {
        //        StageMgr.OnSceneLoaded -= LoadedSceneCallBack;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
    }

    public bool addBaby(COM_BabyInst inst)
    {
        GamePlayer.Instance.AddBaby(inst);
        return true;
    }

    public bool delBabyOK(uint inst)
    {
        GamePlayer.Instance.DelBaby((int)inst);
        return true;
    }

    public bool learnSkillOk(COM_Skill skillInst)
    {
        if (SkillInfo.palyerLearningOK != null)
        {
            SkillInfo.palyerLearningOK();
        }
        ScrollViewPanel.Skillid = (int)skillInst.skillID_;
        //SkillInfo.TishiMwssage ();
        GamePlayer.Instance.AddSkill(skillInst);
        return true;
    }

    public bool skillLevelUp(uint who, COM_Skill skill)
    {
        GamePlayer.Instance.SkillLevelUp(who, skill);
        return true;
    }

    public bool forgetSkillOk(uint skid)
    {
        GamePlayer.Instance.RemoveSkill(skid);
        return true;
    }

    public bool addSkillExp(uint id, uint exp, ItemUseFlag flag)
    {
        GamePlayer.Instance.UpdateSkill((int)id, (int)exp, flag);
        return true;
    }

    public bool enterSceneOk(uint sceneId, float x, float y)
    {
        if (sceneId != PlayerData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_TableId)).defaultSceneId_)
        {
            GameManager.SceneID = (int)sceneId;
            ClientLog.Instance.Log("enterSceneOk");
            Prebattle.Instance.EnterSceneOk(new UnityEngine.Vector3(x, 0f, y));
            //			TeamUIPanel.HideMe();
        }

        return true;
    }

    public bool transfor2(int instid, COM_FPosition pos)
    {
        Prebattle.Instance.Transfor2(instid, pos);
        return true;
    }

    public bool broadcastLeaderPosition(float x, float z)
    {
        return true;
    }

    public static bool resetLocker = false;
    public void RecconnectionEnterBattle(COM_InitBattle initBattle)
    {
        resetLocker = true;
        enterBattleOk(initBattle);
    }

    public bool enterBattleOk(COM_InitBattle initBattle)
    {
        //if (GamePlayer.Instance.isInBattle || StageMgr.Loading)
        //{
        //    GameManager.Instance.nextBattle_ = initBattle;
        //    return true;
        //}

        Prebattle.Instance.EnterBattle();
        NpcRenwuUI._NpcId = 0;
        NpcRenwuUI.BattleId = 0;
        //for(int i=0; i < initBattle.actors_.Length; ++i)
        //{
        //    if(initBattle.actors_[i].battlePosition_ == BattlePosition.BP_None)
        //    {
        //        ClientLog.Instance.LogError("Can not EnterBattle. BattlePosition is None! InstID: " + initBattle.actors_[i].instId_);
        //        return true;
        //    }

        //    ClientLog.Instance.Log("Actor InstID: " + initBattle.actors_[i].instId_);
        //}
        if (GamePlayer.Instance.wait4EnterBattleId == 0)
            Battle.Instance.BattleID = (int)initBattle.battleId_;
        else
            Battle.Instance.BattleID = GamePlayer.Instance.wait4EnterBattleId;
        GamePlayer.Instance.wait4EnterBattleId = 0;
        Battle.Instance.InitBattle(initBattle);
        string battleScene = BattleData.GetSceneName(Battle.Instance.BattleID);
        if (string.IsNullOrEmpty(battleScene))
        {
            SceneData ssd = SceneData.GetData(GameManager.SceneID);
            if (ssd == null)
                ssd = SceneData.GetData(PlayerData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_TableId)).defaultSceneId_);
            battleScene = ssd.battleLevelName_;
        }
        StageMgr.OnSceneLoaded += Battle.Instance.LoadBattleSceneFinish;
        StageMgr.LoadingAsyncScene(battleScene, SwitchScenEffect.SMBlindsTransition, true, false, false, resetLocker);
        resetLocker = false;
        return true;
    }

    public bool exitBattleOk(BattleJudgeType bjt, COM_BattleOverClearing overClearing)
    {
        SceneData nextScene = null;
        if (overClearing.isFly_ && SceneData.HomeID != GameManager.SceneID)
        {
            nextScene = SceneData.GetData(SceneData.HomeID);
        }
        else
        {
            nextScene = GameManager.Instance.CurrentScene;
        }
        //Battle.Instance.PreLoadScene(nextScene.sceneLevelName_);
        Battle.Instance.BattleJudgeType = bjt;
        GamePlayer.Instance.isLevelUp_ = GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < (int)overClearing.playLevel_;
        GamePlayer.Instance.SetIprop(PropertyType.PT_Level, (int)overClearing.playLevel_);

        if (GamePlayer.Instance.BattleBaby != null)
        {
			if(overClearing.babyLevel_ != 0)
			{
				GamePlayer.Instance.BattleBaby.isLevelUp_ = GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level) < (int)overClearing.babyLevel_;
				GamePlayer.Instance.BattleBaby.SetIprop(PropertyType.PT_Level, (int)overClearing.babyLevel_);
			}
           
        }

        Battle.Instance.BattleReward = overClearing;
        GuideManager.Instance.InBattleGuide_ = false;

        return true;
    }

    public bool syncOrderOk(uint uid)
    {
        Battle.Instance.SendOrderOk((int)uid);
        return true;
    }

    public bool syncOneTurnAction(COM_BattleReport actions)
    {
        if (GamePlayer.Instance.isInBattle == false)
        {
            Battle.Instance.WaitNextReport = true;
            Battle.Instance.preReport_ = actions;
            return true;
        }

        if (Battle.Instance.CheckBattleState(actions))
        {
            Battle.Instance.CloseAllUIAndResetForBattle();
            Battle.Instance.InitBattleShowTimeQueue(actions);
            Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
        }
        return true;
    }

    public bool syncOrderOkEX()
    {
        Battle.Instance.SendOrderOk(GamePlayer.Instance.InstId);
        return true;
    }

    public bool syncProperties(uint guid, COM_PropValue[] props)
    {
        //for(int i=0; i < props.Length; ++i)
        //{
        //    if(OnPropUpdate != null)
        //        OnPropUpdate(props[i]);
        //}

        GameManager.Instance.SetIProp(guid, props);
        if (GamePlayer.Instance.isMineBaby((int)guid))
        {
            if (MainbabyState.ResBabyInfoOk != null)
            {
                MainbabyState.ResBabyInfoOk((int)guid);
            }
            if (MainbabyListUI.UpdateBabyListUIOk != null)
            {
                MainbabyListUI.UpdateBabyListUIOk((int)guid);
            }
            if (MainbabyReductionUI.RefreshGrowingUpHanOk != null)
            {
                MainbabyReductionUI.RefreshGrowingUpHanOk((int)guid);
            }
            if (Mainbabystrengthen.RefreshGstrengthenOk != null)
            {
                Mainbabystrengthen.RefreshGstrengthenOk((int)guid);
            }
            if (MainbabyProperty.BabyProperty != null)
            {
                MainbabyProperty.BabyProperty((int)guid);
            }
        }
        return true;
    }

    public bool refreshBaby(COM_BabyInst inst)
    {
        GamePlayer.Instance.RefreshBaby(inst);
        return true;
    }

    public bool battleReportOverOk(BattleJudgeType bjt)
    {
        ClientLog.Instance.Log(bjt.ToString());
        return true;
    }

    public bool receiveChat(COM_ChatInfo chatInfo, COM_ContactInfo info)
    {
		if (chatInfo.ck_ != ChatKind.CK_Friend && !FriendSystem.Instance ().IsBlack (chatInfo.instId_))
            ChatSystem.PushRecord(chatInfo);
        else
            FriendSystem.Instance().FriendChatOk(info, chatInfo);
        return true;
    }

    public bool initBag(COM_Item[] items)
    {
        BagSystem.instance.InitBag(items);
        ClientLog.Instance.Log("initBag");
        return true;
    }

    public bool addBagItem(COM_Item item)
    {
        BagSystem.instance.AddItem(item);
        return true;
    }

    public bool delBagItem(ushort instId)
    {

        BagSystem.instance.DelItem((uint)instId);
        return true;
    }

    public bool sortBagItemOk()
    {
        BankSystem.instance.sortBagItemOk();
        BagSystem.instance.sortBagItemOk();
        return true;
    }

    public bool updateBagItem(COM_Item item)
    {
        BagSystem.instance.UpdateItem(item);
        return true;
    }

    public bool initPlayerEquips(COM_Item[] equips)
    {
        GamePlayer.Instance.InitEquips(equips);
        return true;
    }

    public bool wearEquipmentOk(uint target, COM_Item equip)
    {
        GamePlayer.Instance.UpdateEquipments((int)target, equip);
        return true;
    }

    public bool delEquipmentOk(uint target, uint instId)
    {
        GamePlayer.Instance.delEquipmnet(target, instId);
        return true;
    }

    public bool scenePlayerWearEquipment(uint target, uint itemId)
    {
        Prebattle.Instance.WearPlayersOutlook((int)target, (int)itemId);
        return true;
    }

    public bool scenePlayerDoffEquipment(uint target, uint itemId)
    {
        Prebattle.Instance.TakeOffPlayersOutlook((int)target, (int)itemId);
        return true;
    }

    public bool jointLobbyOk(COM_SimpleTeamInfo[] infos)
    {
        TeamSystem.InitLobbyTeams(infos);

        return true;
    }

    public bool exitLobbyOk()
    {
        TeamSystem.CleanLobbyTeams();
        return true;
    }


    public bool syncOpenSystemFlag(ulong flag)
    {
        GamePlayer.Instance.OpenSubSystem = flag;
        return true;
    }


    public bool createTeamOk(COM_TeamInfo team)
    {

        TeamSystem.InitMyTeam(team);

        return true;
    }

    public bool changeTeamOk(COM_TeamInfo team)
    {

        TeamSystem.UpdateMyTeam(team);
        return true;
    }


    public bool joinTeamOk(COM_TeamInfo team)
    {

        TeamSystem.InitMyTeam(team);
        return true;
    }

    public bool addTeamMember(COM_SimplePlayerInst info)
    {
        TeamSystem.AddMyTeamMember(info);
        return true;
    }

    public bool delTeamMember(int instId)
    {
        TeamSystem.DelMyTeamMember(instId);
        return true;
    }

    public bool changeTeamLeaderOk(int uuid)
    {

        TeamSystem.ChageMyTeamLeader(uuid);
        return true;
    }

    public bool exitTeamOk(bool isKick)
    {
        UIFactory.joined = false;
        TeamSystem.CleanMyTeam(isKick);

        return true;
    }

    public bool updateTeam(COM_TeamInfo info)
    {

        TeamSystem.UpdateMyTeam(info);
        return true;
    }

    public bool readyTeamOk(uint Id)
    {
        TeamSystem.SetMyTeamMemberReady((int)Id, true);
        return true;
    }
    public bool unreadyTeamOk(uint Id)
    {
        TeamSystem.SetMyTeamMemberReady((int)Id, false);
        return true;
    }

    public bool joinScene(COM_SceneInfo info)
    {
        ClientLog.Instance.Log("joinScene " + info.sceneId_);
        Prebattle.Instance.JoinScene(info);
        return true;
    }

    public bool cantMove()
    {
        Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
        Prebattle.Instance.StopSelfActorMove();
        return true;
    }

    public bool move2(int instId, COM_FPosition pos)
    {
        Prebattle.Instance.MoveTo(instId, pos.x_, pos.z_, pos.isLast_);
        return true;
    }

    public bool talked2Npc(int instId)
    {
        Prebattle.Instance.ShowNpcDialog(instId);
        return true;
    }

    public bool talked2Player(int instId)
    {
        return true;
    }

    public bool drawLotteryBoxRep(COM_Item[] items)
    {
        GamePlayer.Instance.DrawEmployeeOk(items);
        return true;
    }

    public bool addEmployee(COM_EmployeeInst employee)
    {
        GamePlayer.Instance.AddEmployee(employee);
        return true;
    }

    public bool battleEmployee(int guid, EmployeesBattleGroup group, bool isBattle)
    {
        GamePlayer.Instance.UpdataEmployeesBattle(guid, group, isBattle);
		if(isBattle)
		{
			 if(GamePlayer.Instance.GetEmployeeById(guid)!= null)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("huobanshangzhen").Replace("{n}",GamePlayer.Instance.GetEmployeeById(guid).InstName));
			}
		}
        // GamePlayer.Instance.BattleEmployee(guid, isBattle);

        return true;
    }

    public bool resetMineTimes(ushort[] t0)
    {
        GatherSystem.instance.InitMingTimes(t0);
        return true;
    }

    public bool miningOk(COM_DropItem[] items, COM_Gather gather,uint num)
    {
		GatherSystem.instance.maxNum  = num;
        GatherSystem.instance.AddOpenGather(gather);
        GatherSystem.instance.MiningOk(items);
        return true;
    }

    public bool acceptQuestOk(COM_QuestInst questInst)
    {
        QuestSystem.AcceptQuestOk(questInst);
        return true;
    }

    public bool submitQuestOk(int questId)
    {

        QuestSystem.SubmitQuestOk(questId);
        ClientLog.Instance.Log("submitQuestOk id: " + questId);

        QuestData qdata = QuestData.GetData(questId);
        if (qdata != null)
        {
            if (qdata.DropID_ == 0)
                return true;
            DropData dData = DropData.GetData(qdata.DropID_);
            if (dData == null)
                return true;

            for (int i = 0; i < dData.itemList.Count; i++)
            {
                if (dData.itemList[i] <= 0)
                    continue;
                if (ItemData.GetData(dData.itemList[i]).mainType_ == ItemMainType.IMT_Employee)
                {
                    GetBabyPanelUI.ShowEmployee(dData.itemList[i]);
                    break;
                }
            }
        }
        return true;
    }

    public bool giveupQuestOk(int questId)
    {
        QuestSystem.GiveupQuestOk(questId);
        return true;
    }

    public bool updateQuestInst(COM_QuestInst questInst)
    {
        QuestSystem.UpdateQuest(questInst);
        return true;
    }

    public bool addFriendOK(COM_ContactInfo friendInst)
    {
        FriendSystem.Instance().AddFriend(friendInst);
        return true;
    }

    public bool delFriendOK(uint instId)
    {
        FriendSystem.Instance().DelFriend(instId);
        return true;
    }

    public bool addBlacklistOK(COM_ContactInfo black)
    {
        FriendSystem.Instance().AddBlack(black);
        return true;
    }

    public bool delBlacklistOK(uint instId)
    {
        FriendSystem.Instance().DelBlack(instId);
        return true;
    }

    public bool referrFriendOK(COM_ContactInfo[] infos)
    {
        FriendSystem.Instance().RecommendFriends = infos;
        return true;
    }

    public bool requestContactInfoOk(COM_ContactInfo req)
    {
        FriendSystem.Instance().RequestContactInfoOk(req);
        return true;
    }

    public bool isAddFriend(string name)
    {
        return true;
    }
    public bool lotteryOk(int id, COM_DropItem[] Items)
    {
        GuaGuaKaUI.ShowGuaGuaKa(id, Items);
        return true;
    }

    //public bool requestMazeInfoOk(COM_MazeInfo mazeInfo)
    //{
    //    Prebattle.Instance.ReceiveMazeInfo (mazeInfo);
    //    return true;
    //}
    public bool compoundItemOk(COM_Item item)
    {
        GatherSystem.instance.CompoundOk(item);
        return true;
    }

    public bool changeBabyNameOK(uint uid, string newName)
    {
        if (MainBabyChangeName.babyChangeName != null)
        {
            Entity baby = GamePlayer.Instance.FindBaby((int)uid);
            if (baby == null)
            {
                return true;
            }
            baby.InstName = newName;
            MainBabyChangeName.babyChangeName((int)uid, newName);
        }
        return true;
    }

    public bool syncDelLobbyTeam(uint uid)
    {
        TeamSystem.SyncDelLobbyTeam((int)uid);
        return true;
    }
    public bool syncUpdateLobbyTeam(COM_SimpleTeamInfo tInfo)
    {
        TeamSystem.SyncUpdateLobbyTeam(tInfo);

        return true;
    }
    public bool syncAddLobbyTeam(COM_SimpleTeamInfo teamInfo)
    {
        TeamSystem.SyncAddLobbyTeam(teamInfo);
        return true;
    }
    public bool findFriendFail()
    {
        FriendSystem.Instance().OnFindFriendFail();
        return true;
    }

    public bool openBagGridOk(int num)
    {
        BagSystem.instance.OpenBugGridOk(num);
        return true;
    }

    //public bool syncMiningItems(COM_Item[] item)
    //{
    //    GatherSystem.instance.syncMiningItems(item);
    //    return true;
    //}

    public bool initBabies(COM_BabyInst[] babies)
    {
        GamePlayer.Instance.setBabies(babies);
        return true;
    }


    //public bool starMiningOk(int itemId)
    //{
    //    GatherSystem.instance.StatMineOk ( itemId);
    //    GatherSystem.instance.mineId = itemId;
    //    float time = 3600;
    //    //GlobalValue.Get (Constant.C_MineTimeout, out time);
    //    GatherSystem.instance.GatherTime = time;
    //    return true;
    //}

    //public bool	stepMiningOk(COM_Item item )
    //{
    //    GatherSystem.instance.AddMineItem (item);

    //    return true;
    //} 

    //public bool stopMiningOk()
    //{
    //    GatherSystem.instance.miningOk ();
    //    return true;
    //}


    public bool evolveOK(int guid, QualityColor color)
    {
        GamePlayer.Instance.EmployeeEvolveOk(guid, color);
        return true;
    }

    public bool upStarOK(int guid, int star, COM_Skill skillInst)
    {
        GamePlayer.Instance.EmployeeStarOk(guid, star, skillInst);
        return true;
    }
    public bool delEmployeeOK(uint[] uid)
    {
        GamePlayer.Instance.delEmployee(uid);
        return true;
    }

    public bool initGuide(uint guideMask)
    {
        GuideManager.Instance.Init(guideMask);
        return true;
    }

    public bool requestRivalOK(COM_EndlessStair[] endless)
    {
        ArenaSystem.Instance.Rivals = endless;
        return true;
    }

    public bool requestChallengeOK(bool b)
    {
        return true;
    }

    public bool requestRankOK(COM_EndlessStair[] ranks)
    {
        ArenaSystem.Instance.Ranks = ranks;
        return true;
    }

    public bool requestMySelfJJCDataOK(COM_EndlessStair player)
    {
        ArenaSystem.Instance.MyInfo = player;
        return true;
    }

    public bool rivalTimeOK()
    {
        return true;
    }

    public bool checkMsgOK(COM_PlayerInst inst)
    {

        return true;
    }

    public bool requestMyAllbattleMsgOK(COM_JJCBattleMsg[] msgs)
    {
        ArenaSystem.Instance.BattleMsgs = msgs;
        return true;
    }

    public bool myBattleMsgOK(COM_JJCBattleMsg msg)
    {
        ArenaSystem.Instance.NewBattleMsg(msg);
        return true;
    }
    public bool buyShopItemOk(int shopId)
    {
        StoreTips.shopItemOk();
        return true;
    }
    public bool learnSkillLose(uint skillId, uint d)
    {
        if (SkillInfo.palyerLearningLose != null)
        {
            SkillInfo.palyerLearningLose((int)skillId, (int)d);
        }
        return true;
    }

    public bool babyLearnSkillOK(uint babyid, uint newskId)
    {
        GamePlayer.Instance.BabyLearnSkill(babyid, newskId);
        return true;
    }

    public bool addPlayerTitle(int title)
    {
        if (!GamePlayer.Instance.titles.Contains(title))
        {
            GamePlayer.Instance.titles.Add(title);
        }
		if(OnAddPlayerTitle != null)
		{
			OnAddPlayerTitle(title);
		}
        return true;
    }
    public bool delPlayerTitle(int title)
    {
		for(int i=0;i<GamePlayer.Instance.titles.Count;i++)
		{
			if(GamePlayer.Instance.titles[i]==title)
			{
				GamePlayer.Instance.titles.RemoveAt(i);
			}
		}
        return true;
    }

    public bool requestOpenBuyBox(float greenTime, float blueTime, int greenNum)
    {
        BoxSystem.Instance.requestOpenBuyBox(greenTime, blueTime, greenNum);
        return true;
    }

    public bool requestGreenBoxTimeOk()
    {
        return true;
    }
    public bool requestBlueBoxTimeOk()
    {
        return true;
    }
    public bool addAchievementOK(int nid)
    {
        GamePlayer.Instance.achfinId.Add(nid);
        GamePlayer.Instance.UpdateAchievement();
        //if(MainPanle.Instance!= null)
        //{
        //    MainPanle.Instance.UpdateMark();
        //}
        return true;
    }
    public bool requestAchawardOK(int achid)
    {
        GamePlayer.Instance.DelAchid(achid);
        return true;
    }
    public bool syncAchievementinfoOK(COM_Achievement[] Achievement)
    {
        //GamePlayer.Instance.AddAchievement (Achievement);
        //		AchieveData.ForDisplay (Achievement);
        //		if(SuccessPanelUI.RefreshFinishOk !=null)
        //		{
        //			SuccessPanelUI.RefreshFinishOk(Achievement);
        //		}
        return true;
    }

    public bool syncActivity(COM_ActivityTable table)
    {
        GamePlayer.Instance.ActivityTable = table;
        ActivitySystem.Instance.UpdateactivitieOK();
        if (MainPanle.Instance != null)
        {
            MainPanle.Instance.RewardPrice();
        }

        return true;
    }
    public bool requestActivityRewardOK(uint itemid)
    {
        ActivitySystem.Instance.ReceiveOk(itemid);
        //PopText.Instance.Show (LanguageManager.instance.GetValue("EN_MallBuyOk"));

        return true;
    }
    public bool initQuest(COM_QuestInst[] qlist, int[] clist)
    {
        QuestSystem.InitQuest(ref qlist, ref clist);
        return true;
    }


    public bool syncHundredInfo(COM_HundredBattle HundredBattle)
    {
        hundredSystem.instance.HundredBattle = HundredBattle;
        return true;
    }


    public bool requestJJCRankOK(uint num, COM_EndlessStair[] EndlessStairs)
    {
        if (ChartsPanelUI.RefreshJJOk != null)
        {
            ChartsPanelUI.RefreshJJOk(num, EndlessStairs);
        }
        ArenaSystem.Instance.Ranks = EndlessStairs;
        return true;
    }

    public bool requestLevelRankOK(uint num, COM_ContactInfo[] ContactInfo)
    {
        if (ChartsPanelUI.RefreshLevelOk != null)
        {
            ChartsPanelUI.RefreshLevelOk(num, ContactInfo);
        }
        return true;
    }

    public bool initSignUp(int[] info, int process, bool sign7, bool sign14, bool sign28)
    {
        SignUpManager.Instance.Init(info, process, sign7, sign14, sign28);
        return true;
    }

    public bool signUpOk()
    {
        SignUpManager.Instance.SignUpOk();
        return true;
    }

    public bool requestSignupRewardOk7()
    {
        SignUpManager.Instance.GetReward(7);
        return true;
    }

    public bool requestSignupRewardOk14()
    {
        SignUpManager.Instance.GetReward(14);
        return true;
    }

    public bool requestSignupRewardOk28()
    {
        SignUpManager.Instance.GetReward(28);
        return true;
    }

    public bool deletePlayerOk(string name)
    {
        XuanPanel.Instance.DelPlayer(name);
        return true;
    }
    public bool sycnDoubleExpTime(bool b, float f)
    {
        GamePlayer.Instance.updateDoubleTime(b, f);
        return true;
    }

    //public bool miningTimout()
    //{
    //    GatherSystem.instance.gatherTimeOut();
    //    return true;
    //}

    public bool initNpc(int[] npcList)
    {
        GameManager.Instance.InitNpcSet(npcList);
        return true;
    }

    public bool addNpc(int[] npcList)
    {
        for (int i = 0; i < npcList.Length; ++i)
        {
            Prebattle.Instance.AddNpc(npcList[i]);
        }
        return true;
    }

    public bool delNpc(int[] npcids)
    {
        Prebattle.Instance.DelNpc(npcids);
        return true;
    }

    public bool addToScene(COM_ScenePlayerInformation player)
    {
        Prebattle.Instance.AddOther(player);
        return true;
    }

    public bool openScene(int sceneId)
    {
        GamePlayer.Instance.OpenScene(sceneId);
        return true;
    }

    public bool delFormScene(int instId)
    {
        Prebattle.Instance.DelOther(instId);
        return true;
    }

    public bool startMatchingOK()
    {
        //ArenaPvpSystem.Instance.PvpMatching = true;
        return true;
    }

    public bool stopMatchingOK(float time)
    {
        //ArenaPvpSystem.Instance.PvpMatching = false;
        //ArenaPvpSystem.Instance.StopMatching (0);
        return true;
    }

    public bool updatePvpJJCinfo(COM_PlayerVsPlayer player)
    {
        ArenaPvpSystem.Instance.MyInfo = player;
        return true;
    }

    public bool syncMyJJCTeamMember()
    {
        //ArenaPvpPanelUI.SwithShowMe ();
        NetConnection.Instance.requestpvprank();
        return true;
    }

    public bool syncEnemyPvpJJCTeamInfo(COM_SimpleInformation[] team, uint teamId)
    {
        ArenaPvpSystem.Instance.TeamId = teamId;
        ArenaPvpSystem.Instance.PvpPlayerTeam = team;
        return true;
    }
    public bool joinTeamRoomOK(COM_TeamInfo teaminfo)
    {
        TeamSystem.BackToLobbyTeam(teaminfo);
        return true;
    }

    public bool updateActivityStatus(ActivityType type, bool open)
    {
        ActivitySystem.Instance.Update(type, open);
        ExamSystem.updateActivityStatus(type, open);
        return true;
    }
    public bool inviteJoinTeam(uint teamid, string myName)
    {
        TeamSystem.inviteJoinTeamOk(teamid, myName);

        return true;
    }

    public bool exitTeamSceneOk()
    {
        ///TeamUIPanel.Instance.ExitSceneOk ();
        return true;
    }
    public bool exitPvpJJCOk()
    {
        if (ArenaSystem.Instance.openPvP)
        {
            if (TeamSystem.IsInTeam())
            {
                for (int i = 0; i < TeamSystem.GetTeamMembers().Length; i++)
                {
                    if (TeamSystem.GetTeamMembers()[i].instId_ == (int)GamePlayer.Instance.InstId)
                    {
                        if (!TeamSystem.GetTeamMembers()[i].isLeavingTeam_)
                        {
                            ArenaSystem.Instance.openPvP = false;
                            ArenaPvpPanelUI.HideMe();
                        }
                    }
                }
            }
            else
            {
                ArenaSystem.Instance.openPvP = false;
                ArenaPvpPanelUI.HideMe();
            }




        }
        return true;
    }

    public bool appendMail(COM_Mail[] mails)
    {
        EmailSystem.instance.AppendMail(mails);
        return true;
    }

    public bool delMail(int mailId)
    {
        return true;
    }

    public bool updateMailOk(COM_Mail mail)
    {
        EmailSystem.instance.updateMailOk(mail);
        return true;
    }

    public bool sycnShowItem(COM_Chat content, COM_Item item)
    {
        return true;
    }

    public bool sycnStates(COM_State[] s)
    {
        return true;
    }

    public bool boardcastNotice(string content, bool isGm)
    {
        GameManager.Instance.ReceiveNotice(content, isGm);
        return true;
    }

    public bool createGuildOK()
    {
        GuildSystem.UpdateGuildData();
        return true;
    }

    public bool leaveGuildOk(string name,bool isck)
    {
		if(isck)
		{
			if(name == GamePlayer.Instance.InstName)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("tichujiazu"));
			}
		}else
		{

				PopText.Instance.Show(LanguageManager.instance.GetValue("MLF_Delete").Replace("{n}",name));

		}

		GuildSystem.leaveMyGuild(name);
        MyFamilyInfo.HideMe();
        return true;
    }

    public bool initGuildData(COM_Guild guild)
    {
        GuildSystem.InitGuildData(guild);
        FamilySystem.instance.GuildData = guild;
        return true;
    }

    public bool initGuildMemberList(COM_GuildMember[] GuildMembers)
    {
        GuildSystem.InitMembers(GuildMembers);
        return true;
    }
    public bool modifyGuildMemberList(COM_GuildMember GuildMember, ModifyListFlag ListFlag)
    {
        GuildSystem.UpdateMembers(GuildMember, ListFlag);
        GuildSystem.UpdateGuildData();
        return true;
    }

    public bool initGuildList(COM_GuildViewerData[] GuildViewerDatas)
    {
        return true;
    }

    public bool modifyGuildList(COM_GuildViewerData ViewerData, ModifyListFlag Flag)
    {
        GuildSystem.UpdateGuildList(ViewerData, Flag);
        GuildSystem.UpdateGuildData();
        return true;
    }
    public bool queryGuildListResult(short page, short pageNum, COM_GuildViewerData[] Datas)
    {
        GuildSystem.QueryGuildListResults(page, pageNum, Datas);
        return true;
    }
    public bool delGuildOK()
    {
        GuildSystem.DismissGuild();
        return true;
    }
    public bool initMySelling(COM_SellItem[] items)
    {
        AuctionHouseSystem.InitMySelling(items);
        return true;
    }

    public bool initMySelled(COM_SelledItem[] items)
    {
        AuctionHouseSystem.InitMySelled(items);
        return true;
    }

    public bool fetchSellingOk(COM_SellItem[] items, int totalPage)
    {
        AuctionHouseSystem.UpdatePage(totalPage);
        AuctionHouseSystem.CacheOnePageItems4Display(items);
        return true;
    }

    public bool fetchSellingOk2(COM_SellItem[] items, int totalPage)
    {
        AuctionHouseSystem.UpdateOtherPage(items.Length / AuctionHouseSystem.OtherSellingMax + items.Length % AuctionHouseSystem.OtherSellingMax == 0 ? 0 : 1);
        AuctionHouseSystem.UpdateOtherSelling(items);
        return true;
    }

    public bool searchSellingOk(COM_SellItem[] items)
    {
        return true;
    }

    public bool selledOk(COM_SelledItem item)
    {
        AuctionHouseSystem.AddMySelled(item);
        return true;
    }

    public bool sellingOk(COM_SellItem sellItem)
    {
        AuctionHouseSystem.AddMySelling(sellItem);
        return true;
    }

    public bool unsellingOk(int sellId)
    {
        AuctionHouseSystem.DelMySelling(sellId);
        return true;
    }
    public bool updateAchievementinfo(COM_Achievement Achievement)
    {
        SuccessSystem.UpdateMyAchieve(Achievement, true);
        return true;
    }
    public bool requestBabyRankOK(uint num, COM_BabyRankData[] BabyRankDates)
    {
        if (ChartsPanelUI.RefreshBabyRankOk != null)
        {
            ChartsPanelUI.RefreshBabyRankOk(num, BabyRankDates);
        }


        return true;
    }
    public bool requestEmpRankOK(uint num, COM_EmployeeRankData[] EmployeeRankDate)
    {

        if (ChartsPanelUI.RefreshEmpRankOk != null)
        {
            ChartsPanelUI.RefreshEmpRankOk(num, EmployeeRankDate);
        }

        return true;
    }
    public bool requestPlayerFFRankOK(uint num, COM_ContactInfo[] ContactInfos)
    {
        if (ChartsPanelUI.requestPlayerFFRankOk != null)
        {
            ChartsPanelUI.requestPlayerFFRankOk(num, ContactInfos);
        }
        return true;
    }

    public bool insertState(COM_State State)
    {
        GamePlayer.Instance.AddState(State);
        return true;
    }
    public bool updattState(COM_State State)
    {
        GamePlayer.Instance.UpdateState(State);
        return true;
    }
    public bool removeState(uint iid)
    {
        GamePlayer.Instance.RemoveState(iid);
        return true;
    }

    public bool updateActivityCounter(ActivityType type, int counter, int reward)
    {
        ActivitySystem.Instance.SyncCounter(type, counter);
        ActivitySystem.Instance.SyncReward(reward);
        return true;
    }

    public bool syncTeamDirtyProp(int guid, COM_PropValue[] props)
    {
        TeamSystem.UpdateDirtyProp(guid, props);
        return true;
    }

    public bool useItemOk(int itemId, int stack)
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_UseItemOk, itemId, stack);
        return true;
    }

    public bool syncEnemyPvpJJCPlayerInfo(COM_SimpleInformation player)
    {
        ArenaPvpSystem.Instance.playerSingle = player;

        return true;
    }

    public bool requestFixItemOk(COM_Item item)
    {
        GamePlayer.Instance.fixItemOk(item);
        return true;
    }



    public bool depositItemOK(COM_Item item)
    {
        BankSystem.instance.DepositItem(item);
        return true;
    }
    public bool getoutItemOK(ushort id)
    {
        BankSystem.instance.GetoutItemOK(id);
        return true;
    }

    public bool sortItemStorageOK(COM_Item[] itemid)
    {
        BankSystem.instance.sortItemStorage(itemid);
        return true;
    }

    public bool openStorageGrid(StorageType type, ushort a)
    {
        if (type == StorageType.ST_Item)
        {
            BankSystem.instance.itemNum = (int)a;
        }
        if (type == StorageType.ST_Baby)
        {
            BankSystem.instance.babyNum = (int)a;
        }


        return true;
    }
    public bool initItemStorage(ushort num, COM_Item[] items)
    {
        GamePlayer.Instance.initItemStorage(num, items);
        return true;
    }
    public bool initBabyStorage(ushort num, COM_BabyInst[] babys)
    {
        GamePlayer.Instance.initBabyStorage(num, babys);
        return true;
    }
    public bool sortBabyStorageOK(uint[] babyid)
    {
        BankSystem.instance.SortBabyStorage(babyid);
        return true;
    }
    public bool depositBabyOK(COM_BabyInst inst)
    {
        BankSystem.instance.DepositBabyOK(inst);
        return true;
    }
    public bool getoutBabyOK(ushort id)
    {
        BankSystem.instance.GetoutBabyOK(id);
        return true;
    }

    public bool makeDebirsItemOK()
    {
        GamePlayer.Instance.MakeDebrisItemOk();
        return true;
    }
    public bool updateMagicItem(int level, int exp)
    {
        GamePlayer.Instance.updateMagicItem(level, exp);
        return true;
    }

    public bool changeMagicJobOk(JobType job)
    {
        GamePlayer.Instance.MagicItemJob = job;
        return true;
    }
    public bool setNoTalkTime(float time)
    {
        return true;
    }
    public bool leaveTeamOk(int playerId)
    {
        TeamSystem.leaveTeamOk(playerId);
        return true;
    }
    public bool backTeamOK(int playerId)
    {
        TeamSystem.backTeamOK(playerId);
        return true;
    }
    public bool teamCallMemberBack()
    {
        TeamSystem.teamCallMemberBack();
        return true;
    }

    public bool autoBattleResult(bool bOk)
    {
        if (bOk)
            Prebattle.Instance.activeMoved_ = bOk;
        Prebattle.Instance.ChangeWalkEff(bOk ? Prebattle.WalkState.WS_AME : Prebattle.WalkState.WS_Normal);
        //Prebattle.Instance.clickedQuestId_ = 0;
        GamePlayer.Instance.UpdateAmeInfo(bOk);
        if (bOk == false)
        {
            Prebattle.Instance.StopSelfActorMove();
            PopText.Instance.Show(LanguageManager.instance.GetValue("noMonsterInArea"), PopText.WarningType.WT_Warning);
        }
        return true;
    }

    public bool changeEmpBattleGroupOK(EmployeesBattleGroup group)
    {
        GamePlayer.Instance.CurEmployeesBattleGroup = group;
        return true;
    }
    public bool remouldBabyOK(uint babyId)
    {

        if (MainbabyReformUI.isMainbabyReformUI)
        {
            if (MainbabyReformUI.RefreshbabyInfoOk != null)
            {
                MainbabyReformUI.RefreshbabyInfoOk((int)babyId);
            }
        }

        return true;
    }

    public bool syncBattleStatus(int instid, bool inBattle)
    {
        Prebattle.Instance.UpdateInBattle(instid, inBattle);
        return true;
    }

    public bool setTeamLeader(int instid, bool inBattle)
    {
        Prebattle.Instance.UpdateLeaderMark(instid, inBattle);
        return true;
    }

	public bool updateTeamMember(int playerId, bool isMember)
	{
		Prebattle.Instance.UpdateInTeam(playerId, isMember);
		return true;
	}

    public bool magicItemTupoOk(int level)
    {

        GamePlayer.Instance.MagicTupoLevel = level;
        return true;
    }
    public bool queryPlayerOK(COM_SimplePlayerInst Inst)
    {
        ChartsSystem.queryPlayerOK(Inst);
        return true;
    }
    public bool queryEmployeeOK(COM_EmployeeInst Inst)
    {
        ChartsSystem.queryEmployeeOK(Inst);
        return true;
    }
    public bool queryBabyOK(COM_BabyInst Inst)
    {
        ChartsSystem.queryBabyOK(Inst);
        return true;
    }
    public bool publishItemInstRes(COM_ShowItemInstInfo ShowItem, ChatKind Kind)
    {
        ChatSystem.publishItemInstRes(ShowItem, Kind);
        return true;
    }
    public bool queryItemInstRes(COM_ShowItemInst ItemInst)
    {
        ChatSystem.queryItemInstRes(ItemInst);
        return true;
    }
    public bool publishBabyInstRes(COM_ShowbabyInstInfo InstInfo, ChatKind Kind)
    {
        ChatSystem.publishBabyInstRes(InstInfo, Kind);
        return true;
    }
    public bool queryBabyInstRes(COM_ShowbabyInst babyInst)
    {
        ChatSystem.queryBabyInstRes(babyInst);
        return true;
    }

    public bool querySimplePlayerInstOk(COM_SimplePlayerInst player)
    {
        TeamPlayerInfo.SwithShowMe(player);
        return true;
    }

    public bool inviteGuild(string sendName, string guildName)
    {
        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("yapqingruhui").Replace("{n}", sendName).Replace("{n1}", guildName), () =>
        {
            NetConnection.Instance.respondInviteJoinGuild(sendName);
        },false,null,null,"","",2000,true);

        return true;
    }
    public bool requestJoinTeamTranspond(string reqName)
    {

        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shenqingrudui").Replace("{n}", reqName), () =>
        {
            NetConnection.Instance.ratifyJoinTeam(reqName);
        });


        return true;
    }
    public bool refuseBackTeamOk(int playerId)
    {
        TeamSystem.RefuseBackTeamOk(playerId);

        return true;
    }

    public bool queryOnlinePlayerOK(bool inline)
    {
        FriendSystem.Instance().IsFriendOnLine(inline);
        return true;
    }

    public bool petActivityNoNum(string name)
    {
        PopText.Instance.Show(LanguageManager.instance.GetValue("wucishu").Replace("{n}", name));
        return true;
    }
    public bool updateGuildLevelExp(int id, sbyte level, int exp)
    {
      
        return true;
    }
    public bool updateGuildShopItems(int[] sitems)
    {
        GuildSystem.updateGuildShopItems(sitems);
        return true;
    }

    public bool buyGuildShopItemOk(short times)
    {
        GuildSystem.buyGuildShopItemOk(times);
        return true;
    }
    //	public bool updateGuildBattle(COM_GuildBattle battle)
    //	{
    ////        if (battle.state_ == GuildBattleState.GBS_Entry)
    ////        {
    //			GuildSystem.updateGuildBattle(battle);
    ////        }
    ////        else if (battle.state_ == GuildBattleState.GBS_Start)
    ////        {
    ////		
    ////		}else if (battle.state_ == GuildBattleState.GBS_Start)
    //		return true;
    //	}

    public bool updateGuildMyMember(COM_GuildMember GuildMember)
    {
        GuildSystem.UpdateGuildMemberInfo(GuildMember);
        FamilySystem.instance.GuildMember = GuildMember;
        return true;
    }
    public bool requestFriendListOK(COM_ContactInfo[] list)
    {
        FriendSystem.Instance().requestFriendList(list);
        return true;
    }
	public bool updateGuildShopItems(COM_GuildShopItem[] Items)
	{
		return true;
	}
    public bool zhuanpanOK(uint[] items)
    {
        ZhuanPanSystem.zhuanpanOK(items);
        //
        //		if(LotteryPanelUI.LotteryTeamsOk != null)
        //		{
        //			LotteryPanelUI.LotteryTeamsOk(items);
        //		}
        return true;
    }
    public bool updateZhuanpanNotice(COM_Zhuanpan Zhuanpan)
    {
        ZhuanPanSystem.updateZhuanpanNotice(Zhuanpan);
        //		if(LotteryPanelUI.LotteryTeamsOk != null)
        //		{
        //			LotteryPanelUI.LotteryTNoticeOk(Zhuanpan);
        //		}
        return true;
    }
	public bool sycnZhuanpanData(COM_ZhuanpanData data)
	{
		ZhuanPanSystem.sycnZhuanpanNotice(data);
		return true;
	}
//    public bool sycnZhuanpanNotice(COM_Zhuanpan[] Zhuanpans)
//    {
//        ZhuanPanSystem.sycnZhuanpanNotice(Zhuanpans);
//        //		if(LotteryPanelUI.LotteryTeamsOk != null)
//        //		{
//        //			LotteryPanelUI.LotteryitemNoticeOk(Zhuanpans);
//        //		}
//        return true;
//    }
    public bool intensifyBabyOK(uint babyid, uint intensifyLevel)
    {
		BabyData.babyReId = (int)babyid;
		BabyData.intensifyLevel = (int)intensifyLevel;
        if (Mainbabystrengthen.RefreshGstrengthenlevelOk != null)
        {
            Mainbabystrengthen.RefreshGstrengthenlevelOk((int)babyid, (int)intensifyLevel);
        }
        return true;
    }
    public bool redemptionSpreeOk()
    {
        PopText.Instance.Show(LanguageManager.instance.GetValue("duihuanchenggong"));
        return true;
    }

    public bool sceneFilterOk(SceneFilterType[] sfType)
    {
        GameManager.Instance.saveFilters(sfType);
        return true;
    }

    public bool checkMsgOK(COM_SimplePlayerInst player)
    {
        ArenaSystem.Instance.checkArenaPlayer(player);
        return true;
    }


    public bool initEmployees(COM_EmployeeInst[] employees, bool flag)
    {
        ClientLog.Instance.Log("initEmployeesinitEmployeesinitEmployees");
        GamePlayer.Instance.setEmployees(employees, flag);
        return true;
    }

    public bool initEmpBattleGroup(COM_BattleEmp battleEmp)
    {
        GamePlayer.Instance.EmployeesBattleGroup1 = battleEmp.employeeGroup1_;
        GamePlayer.Instance.EmployeesBattleGroup2 = battleEmp.employeeGroup2_;
        GamePlayer.Instance.CurEmployeesBattleGroup = battleEmp.empBattleGroup_;

        return true;
    }

    public bool initAchievement(COM_Achievement[] achievement)
    {
        SuccessSystem.InitMyAchieve(achievement);
        return true;
    }
    public bool sceneFilterOk(SceneFilterType t)
    {
        return true;
    }

    public bool initGather(uint num,COM_Gather[] gList)
    {
		GatherSystem.instance.InitOpenGatherList(num,gList);
        return true;
    }

    public bool openGatherOK(COM_Gather g)
    {
        GatherSystem.instance.AddOpenGather(g);
        return true;
    }

    public bool initcompound(uint[] arr)
    {
        CompoundSystem.instance.InitOpenEquip(arr);
        return true;
    }

    public bool openCompound(uint id)
    {
        CompoundSystem.instance.AddOpenEquip(id);
        return true;
    }

    public bool copynonum(string roleName)
    {
        PopText.Instance.Show(LanguageManager.instance.GetValue("fubencishu").Replace("{n}", roleName));
        return true;
    }
    public bool syncExam(COM_Exam exam)
    {
        ExamSystem.SyncExam(exam);
        return true;
    }
    public bool syncExamAnswer(COM_Answer Answer)
    {
        ExamSystem.SyncExamAnswer(Answer);
        return true;
    }
    public bool shareWishOK(COM_Wish Wish)
    {
        WishingTreeSystem.ShareWish(Wish);
        return true;
    }
    public bool wishingOK()
    {
        WishingTreeSystem.WishOk();
        return true;
    }
    public bool sycnEmployeeSoul(int id, uint num)
    {
        GamePlayer.Instance.UpdateEmployeeSoul(id, num);
        return true;
    }

    public bool requestAudioOk(int id, byte[] content)
    {
        ChatSystem.AudioOk(id, content);
        return true;
    }

    public bool requestpvprankOK(COM_ContactInfo[] list)
    {
        GamePlayer.Instance.ContactInfoList = list;
        return true;
    }

    public bool leaderCloseDialogOk()
    {
        if (TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
            return true;

        if (NpcRenwuUI.talking_)
            NpcRenwuUI.HideMe();
        else
            GamePlayer.Instance.leaderSkipTalk = true;
        return true;
    }
    public bool teamerrorno(string name, ErrorNo e)
    {
        PopText.Instance.Show(name + LanguageManager.instance.GetValue(e.ToString()), PopText.WarningType.WT_Tip, true);
        return true;
    }

    public bool levelupGuildSkillOk(COM_Skill skill)
    {
        FamilySystem.instance.UpdateZhufuSkill(skill);
        return true;
    }

    public bool presentGuildItemOk(int num)
    {
        FamilySystem.instance.presentGuildItemOk(num);
        return true;
    }

    public bool progenitusAddExpOk(COM_GuildProgen guild)
    {
        FamilySystem.instance.progenitusAddExpOk(guild);
        return true;
    }

    public bool setProgenitusPositionOk(int[] pos)
    {
        FamilySystem.instance.ProgenPos = pos;
        return true;
    }

    public bool updateGuildFundz(int num)
    {
        FamilySystem.instance.updateGuildFundz(num);
		GuildSystem.updateGuildFanz (num);
        return true;
    }

    public bool updateGuildBuilding(GuildBuildingType type, COM_GuildBuilding building)
    {
		GuildSystem.UpdateGuildLevelbu(type,building);
		FamilySystem.instance.updateGuildBuilding(type, building);
        return true;
    }

    public bool warriorStartOK()
    {
        ArenaPvpSystem.Instance.PvpMatching = true;
        return true;
    }

    public bool warriorStopOK()
    {
        ArenaPvpSystem.Instance.PvpMatching = false;
        ArenaPvpSystem.Instance.StopMatching(0);
        return true;
    }

    public bool syncWarriorEnemyTeamInfo(COM_SimpleInformation[] Information, uint asd)
    {
        ArenaPvpSystem.Instance.TeamId = asd;
        ArenaPvpSystem.Instance.PvpPlayerTeam = Information;
        return true;
    }
    public bool openWarriorchooseUI()
    {
        ArenaPvpPanelUI.SwithShowMe();
        return true;
    }
    public bool updateGuildMemberContribution(int con)
    {
        GamePlayer.Instance.guildContribution_ = con;
        return true;
    }

	public bool closeGuildBattle(bool isWinner) 
	{
		GuildSystem.closeGuildBattle (isWinner);

		return true;
	}
    public bool openGuildBattle(string otherName, int playerNum, int level, bool isLeft,int time)
	{
		GuildSystem.guildBatatleStateTime =(float)time;
        GameManager.Instance.isLeft = isLeft;
		GuildSystem.openBattle (otherName,playerNum,level);
		GlobalInstanceFunction.isGuildBattleStart = true;
		return true;
	}
	public bool syncGuildBattleWinCount(int mycount, int othercou)
	{
		GuildSystem.syncGuildBattleWinCount (mycount,othercou);
		return true;
	}

    public bool agencyActivity(ADType type, bool isFlag)
    {
        GamePlayer.Instance.UpdateAd(type, isFlag);
        return true;
    }

	public bool startGuildBattle(string otherName, int otherCon, int selfCon)
	{
		GuildSystem.StartGuildBattle (otherName,otherCon,selfCon);
		return true;
	}
	public bool startOnlineTime()
	{
		MoreActivityData.onlineStart = true;
		return true;
	}
	public bool requestOnlineTimeRewardOK(uint index)
	{
		GamePlayer.Instance.onlineTimeRewards_ .Add(index);
		if(GamePlayer.Instance.OnOnlineUpdate != null)
		{
			GamePlayer.Instance.OnOnlineUpdate();
		}
		return true;
	}
	public bool updateFestival(COM_ADLoginTotal data)
	{
		MoreActivityData.instance.UpdateLoginTotal (data);
		return true;
	}
	public bool buyFundOK(bool fla)
	{
		GamePlayer.Instance.isFund_ = fla;
		return true;
	}
	public bool requestFundRewardOK(uint level)
	{
		GamePlayer.Instance.fundtags_.Add (level);
		GamePlayer.Instance.fund_ = (int)level;
		if(GamePlayer.Instance.OnGrowthfundUpdate != null)
		{
			GamePlayer.Instance.OnGrowthfundUpdate((int)level);
		}
		return true;
	}
	public bool  updateSelfRecharge(COM_ADChargeTotal data)
	{
		MoreActivityData.instance.UpdateSelfRecharge (data);
		return true;
	}

	public bool  updateSysRecharge(COM_ADChargeTotal data)
	{
		MoreActivityData.instance.UpdateSysRecharge (data);
		return true;
	}

	public bool updateSelfDiscountStore(COM_ADDiscountStore data)
	{
		MoreActivityData.instance.UpdateSelfDiscountStore (data);
		return true;
	}

	public bool updateSysDiscountStore(COM_ADDiscountStore data)
	{
		MoreActivityData.instance.UpdateSysDiscountStore (data);
		return true;
	}

	public bool updateSelfOnceRecharge(COM_ADChargeEvery data)
	{
		MoreActivityData.instance.UpdateSelfChargeEvery (data);
		return true;
	}

	public bool updateSysOnceRecharge(COM_ADChargeEvery data)
	{
		MoreActivityData.instance.UpdateSysChargeEvery (data);
		return true;
	}
	public bool updateEmployeeActivity(COM_ADEmployeeTotal data)
	{
		MoreActivityData.instance.updateEmployeeActivity (data);
		return true;
	}

    public bool openCardOK(COM_ADCardsContent data)
    {
		MoreActivityData.UpdateCardsData(data);
        NetWaitUI.HideMe();
        return true;
    }
    public bool resetCardOK()
    {
        MoreActivityData.ClearCardsData();
        return true;
    }

    public bool sycnHotRole(COM_ADHotRole data)
    {
        MoreActivityData.InitHotRoleData(data);
        return true;
    }

    public bool hotRoleBuyOk(ushort buyNum)
    {
        MoreActivityData.UpdateHotRoleBuyNum((int)buyNum);
        return true;
    }

    public bool updateSevenday(COM_Sevenday data)
    {
        MoreActivityData.Update7Days(data);
        return true;
    }

	public bool joinCopySceneOK(int id)
	{
		GamePlayer.Instance.joinCopySceneOK (id);
		return true;
	}

	public bool firstRechargeOK(bool b)
	{
		GamePlayer.Instance.firstRechargeOK (b);
		return true;
	}

	public bool firstRechargeGiftOK(bool b)
	{
		GamePlayer.Instance.firstRechargeGiftOK (b);
		return true;
	}

	public bool sycnVipflag(bool flag)
	{
		GamePlayer.Instance.sycnVipflag (flag);
		return true;
	}
	public bool updateShowBaby(uint playerId, uint showBabyTableId, string showBabyName)
	{
        Avatar otherPlayer = Prebattle.Instance.FindPlayer((int)playerId);
        if (otherPlayer != null)
        {
            otherPlayer.SwitchFollowBaby(showBabyTableId != 0, (int)showBabyTableId, showBabyName);
        }
		return true;
	}
	public bool updateMySelfRecharge(COM_ADChargeTotal Total)
	{
		GamePlayer.Instance.updateMySelfRecharge(Total);
		return true;
	}
	public bool verificationSMSOk(string num)
	{
		MoreActivityData.verificationSMS (num);
		return true;
	}
	public bool requestLevelGiftOK(int level)
	{
		GamePlayer.Instance.requestLevelGiftOK (level);
		return true;
	}
	public bool sycnConvertExp(int exp)
	{
        string msg = string.Format(LanguageManager.instance.GetValue("gainConvertExp"), exp);
        ChatSystem.PushSystemMessage(msg);
        PopText.Instance.Show(msg, PopText.WarningType.WT_Tip);
        //GamePlayer.Instance.ConvertExp = exp;
		return true;
	}

	public bool signUp(bool sign)
	{
		GamePlayer.Instance.signUp (sign);
		return true;
	}
	public bool delStorageBabyOK(ushort val)
	{
        MainbabyListUI.SelectDirty = true;
		BankSystem.instance.GetoutBabyOK(val);
		return true;
	}

	public bool updateMinGiftActivity(COM_ADGiftBag miaosha)
	{
		GamePlayer.Instance.updateMinGiftActivity (miaosha);
		return true;
	}


	public bool  wearFuwenOk(COM_Item item)
	{
		GamePlayer.Instance.wearFuwenOk (item);
		return true;
	}

	public bool takeoffFuwenOk(int slot)
	{
		GamePlayer.Instance.takeoffFuwenOk (slot);
		return true;
	}

	public bool compFuwenOk()
	{
		GamePlayer.Instance.compFuwenOk ();
		return true;
	}

	public bool initCopyNums()
	{
		GamePlayer.Instance.copyNum_.Clear ();
		return true;
	}

	public bool orderOk(string orderid, int shopid)
	{
		if(string.IsNullOrEmpty(orderid))
			return true;

		ShopData sd = ShopData.GetData(shopid);
		if(sd == null)
			return true;

		//gameHandler gh = UnityEngine.GameObject.FindObjectOfType<gameHandler>();
		//if(gh == null)
		//	return true;

		//gh.OrderOk(orderid, shopid);

		return true;
	}
	public bool updateRandSubmitQuestCount(int count)
	{
		QuestSystem.randCount = count;
		return true;
	}
	public bool requestEmployeeQuestOk(COM_EmployeeQuestInst[] list)
	{
		EmployeeTaskSystem.instance.InitTaskList (list);
		return true;
	}

	public bool acceptEmployeeQuestOk(COM_EmployeeQuestInst inst)
	{
		EmployeeTaskSystem.instance.UpdateTaskInst (inst);
		return true;
	}

	public bool submitEmployeeQuestOk(int id, bool b)
	{
		EmployeeTaskSystem.instance.SubmitEmployeeQuestOk (id, b);
		return true;
	}

	public bool sycnCrystal(COM_CrystalData data)
	{
		GemSystem.instance.sycnCrystal (data);
		return true;
	}
	
	public bool crystalUpLeveResult(bool b)
	{  
		GemSystem.instance.crystalUpLeveResult (b);
		return true;
	}

	public bool resetCrystalPropOK()
	{
		GemSystem.instance.resetCrystalPropOK ();
		return true;
	}

	public bool updateIntegralShop(COM_IntegralData data)
	{
		MoreActivityData.instance.updateIntegralShop (data);
		return true;
	}

	public bool sycnCourseGift(COM_CourseGift[] CourseGifts)
	{
		GamePlayer.Instance.updateLevelRewardShop (CourseGifts);
		return true;
	}
}