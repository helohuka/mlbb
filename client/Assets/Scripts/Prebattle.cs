using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Prebattle {

	static private Prebattle inst = null;
	static public Prebattle Instance
	{
		get
		{
			if(inst == null)
				inst = new Prebattle();
			return inst;
		}
	}

	public delegate void delTeamMender(int uid);
	public static delTeamMender delMender;

    public delegate void AllFinished();
    public event AllFinished OnAllReady;

    public delegate void NpcLoaded(int npcId);
    public event NpcLoaded OnNpcLoaded;

    public delegate void TouchOtherPlayerHandler(COM_ScenePlayerInformation player);
    public event TouchOtherPlayerHandler OnTouchOtherPlayer;
	
	public Vector3 cachedPosition_;

    public bool isLoading_ = false;

    GameObject destEff_;

    public bool exitFromBattle_;

    public bool _HeightAdjusted;

	float speed_, rotSpeed_;

    //玩家自己在场景中的化身
    Avatar playerAvatar_ = null;

	public Dictionary<int, Avatar> otherAvatarContainer_ = null;
    List<int> otherPlayerQue_ = null;

    public List<Npc> npcContainer_ = null;

    GameObject leaderMark_ = null;

    SceneData currentScene_ = null;

    int totalCount_, currentCount_ = 0;

    int sceneNpcLoadedCount_, sceneOtherPlayerLoadedCount_;

    public int clickedQuestId_;

    public bool running_ = false;

    public int selectedNpc_ = 0;

    public string battleName_ = "";

    public BattleJudgeType judgeType_ = BattleJudgeType.BJT_None;

    public BattleType battleType_ = BattleType.BT_None;

    public float groundHeight_ = 0f;

    //同场景中其他玩家最高数量
    int OtherPlayerNumMax = 20;

    //一个panel下最多可挂的namelabel
    public int NameLblNumInPanel = 30;

    //模型高度 草 黑科技
    public float modelHeight_ = 0.9f;

    public Vector3 selectPoint_;

    Vector3[] moveToSlot = new Vector3[5];

    public bool activeMoved_ = false;

    int updateNameCount_ = 0;

    public bool senseMode_ = false;

    public List<GameObject> nameRootPanel_;

    public enum WalkState
    {
        WS_Normal,
        WS_AME,
        WS_AFP,
    }

    public WalkState walkState_ = WalkState.WS_Normal;

    bool sceneLoaded_, npcLoaded_, otherLoaded_;

    public bool AllLoaded
    {
        get
        {
            return sceneLoaded_ && npcLoaded_;// && otherLoaded_;
        }
    }

	Prebattle()
	{
        GetInfoOnClick.OnClickInformation += ClickTextWithInfomation;
        GamePlayer.Instance.PlayerLevelUpEvent += new RequestEventHandler<int>(OnLevelUp);
        npcContainer_ = new List<Npc>();
        otherAvatarContainer_ = new Dictionary<int, Avatar>();
        otherPlayerQue_ = new List<int>();
	}



    /// <summary>
    /// 创建副本
    /// </summary>
    public void Init()
    {
        if (GlobalValue.isBattleScene(Application.loadedLevelName))
            return;

        if (isLoading_)
            return;

        isLoading_ = true;
        // 注册事件
        XInput.Instance.OnTouchGround += Move;
        XInput.Instance.OnTouchActor += TouchActor;
        //XInput.Instance.OnTouchDown += OnCancelAuto;
        //GameManager.Instance.OnNpcAdd += AddNpc;
        //GameManager.Instance.OnNpcDelete += DelNpc;
        TeamSystem.OnCreateTeam += CreateTeam;
        TeamSystem.OnLeaderChange += ChangeLeader;
        TeamSystem.OnUpdateMemStateUI += LeaveOrBackTeam;
        TeamSystem.OnExitIteam += ExitTeam;
        PrebattleEvent.getInstance.BackEvent = OnReturnMainScene;

        sceneLoaded_ = false;
        npcLoaded_ = false;
        otherLoaded_ = false;

        speed_ = 2.6f;
        rotSpeed_ = 10f;
        
        LoadAssets();

        //StageMgr.SceneLoadedFinish();
    }
	public bool IsWishingAvailable()
	{
		int Distance = 0;
		int npcId = 0;
		GlobalValue.Get(Constant.C_WishNpcID, out npcId);
		GlobalValue.Get(Constant.C_WishDistance, out Distance);
		Npc whisNpc = FindNpc (npcId);
		if(whisNpc == null)
			return false;
		if(playerAvatar_==null)
			return false;
		if(playerAvatar_.gameObject_==null)
			return false;
		if (Vector3.Distance (playerAvatar_.gameObject_.transform.position, whisNpc.gameObject_.transform.position) < Distance) {
			return true;
		}
		return false;
	}
    GameObject ghost_;
    /// <summary>
    /// 加载所需资源
    /// </summary>
    void LoadAssets()
    {
        //加载玩家数据
        playerAvatar_ = new Avatar();
        playerAvatar_.Init();
        identifyHeight();

        //加载玩家资源
        ENTITY_ID weaponAssetId = 0;
        if (GamePlayer.Instance.WeaponID != 0)
            weaponAssetId = (ENTITY_ID)ItemData.GetData(GamePlayer.Instance.WeaponID).weaponEntityId_;

        GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), weaponAssetId, GamePlayer.Instance.type_, AssetsLoadCallBack, new ParamData(ParamData.AssetType.AT_Player, GamePlayer.Instance.InstId, 0, cachedPosition_, Quaternion.identity), "Default", GamePlayer.Instance.DressID);

        //创建虚拟角色设置摄像机位置
        ghost_ = new GameObject();
        ghost_.transform.position = new Vector3(cachedPosition_.x, groundHeight_, cachedPosition_.z);
        //设置相机焦点Object
        NavTest nt = Camera.main.GetComponent<NavTest>();
        if (nt == null)
            nt = Camera.main.gameObject.AddComponent<NavTest>();
        nt.target = ghost_.transform;

        totalCount_ = 1;

    }

    public void AdjustHeight()
    {
        if (playerAvatar_ == null)
            return;

		if (playerAvatar_.transform_ == null)
			return;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(playerAvatar_.transform_.position.x, 1000f, playerAvatar_.transform_.position.z), Vector3.down, out hit, 10000f, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (Mathf.Approximately(hit.point.y + 0.005f, groundHeight_))
                return;

            groundHeight_ = hit.point.y;
            groundHeight_ = groundHeight_ + 0.005f; //稍微抬高一点 让影子显示出来
            cachedPosition_.y = groundHeight_;
        }
    }

    void identifyHeight()
    {
        if(_HeightAdjusted)
            return;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(cachedPosition_.x, 1000f, cachedPosition_.z), Vector3.down, out hit, 10000f, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (Mathf.Approximately(hit.point.y, groundHeight_))
                return;

            groundHeight_ = hit.point.y;
            groundHeight_ = groundHeight_ + 0.005f; //稍微抬高一点 让影子显示出来
            cachedPosition_.y = groundHeight_;

            _HeightAdjusted = true;
        }
        //else
        //{
        //    if (GameManager.SceneID == 2)
        //    {
        //        //scene还未加载出来
        //        groundHeight_ = 0.955f;
        //        groundHeight_ = groundHeight_ + 0.005f; //稍微抬高一点 让影子显示出来
        //        cachedPosition_.y = groundHeight_;
        //    }
        //}
    }

    /// <summary>
    /// 初进场景所有npc加载完毕
    /// </summary>
    void SceneNpcLoadFinish()
    {
        npcTimerStart = false;
        sceneNpcLoadedCount_ = 0;
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterScene, GameManager.SceneID);

            Animation openGate = GameObject.FindObjectOfType<Animation>();
            if (openGate != null)
            {
				int maxNum = 0;
				GlobalValue.Get(Constant.C_HundredTier, out maxNum);
				if ((battleType_ == BattleType.BT_PVH && judgeType_ == BattleJudgeType.BJT_Win) &&
				    hundredSystem.instance.currentFightLevel_ == maxNum)
				{
					MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("hundredMaxhouxu"),()=>{});
					return;
				}

				// ChallengeData.getHundredNum(GameManager.SceneID);
                // 如果是百人并且赢了 或者 该层已经挑战过
				//(battleType_ == BattleType.BT_PVH && judgeType_ == BattleJudgeType.BJT_Win) &&
				if ( isBaiRenBattled())
                {
                    //开门
                    openGate.Play("Take 001");
                }
                else
                {
                    //否则不播放动画
                    openGate.Stop();
                }

            }
        }, 2);
        AllDone(sceneLoaded_, true);



    }

    bool isBaiRenBattled()
    {
        SceneData ssd = SceneData.GetData(GameManager.SceneID);
        if (ssd.sceneType_ == SceneType.SCT_Bairen && (hundredSystem.instance.currentFightLevel_ < hundredSystem.instance.ChallengeNum && ChallengeData.getHundredNum(GameManager.SceneID) < hundredSystem.instance.ChallengeNum))
            return true;
        return false;
    }

    /// <summary>
    /// 初进场景所有其他玩家加载完毕
    /// </summary>
    void SceneOtherLoadFinish()
    {
        sceneOtherPlayerLoadedCount_ = 0;
        //AllDone(sceneLoaded_, npcLoaded_, true);
    }

    /// <summary>
    /// 所有资源加载完回调
    /// </summary>
    void SceneLoaded()
    {
        isLoading_ = false;
        running_ = true;
        //加载NPC
        LoadNpcAssets();

        //加载其他玩家
        LoadOtherAssets();

        ShowWalkEff();

        if (exitFromBattle_)
        {
            ShowBattleReward();
            exitFromBattle_ = false;
        }
        //else
        //    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainPanelOpen);
        
        AllDone(true, npcLoaded_);
    }

    void AllDone(bool scene, bool npc)
    {
        sceneLoaded_ = scene;
        npcLoaded_ = npc;
        //otherLoaded_ = other;
        if (AllLoaded)
        {
            UpdatePosition();
            StageMgr.SceneLoadedFinish();
            NetConnection.Instance.sceneLoaded();
            StageMgr.sendSceneLoaded_ = true;
			if(OnAssetsLoadNPCFinish != null)
			{
				OnAssetsLoadNPCFinish();
			}
        }
    }

    void UpdatePosition()
    {
        if (otherAvatarContainer_ == null)
            return;

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            other.UpdateLastetPosition();
        }

    }

    public void ChangeWalkEff(WalkState state)
    {
        walkState_ = state;
        ShowWalkEff();
    }

    void ShowWalkEff()
    {
        switch(walkState_)
        {
            case WalkState.WS_Normal:
                SwitchAFPEffect(false);
                SwitchAMEEffect(false);
                GamePlayer.Instance.UpdateAmeInfo(false);
                break;
            case WalkState.WS_AME:
                SwitchAFPEffect(false);
                SwitchAMEEffect(true);
                GamePlayer.Instance.UpdateAmeInfo(true);
                break;
            case WalkState.WS_AFP:
                SwitchAFPEffect(true);
                SwitchAMEEffect(false);
                GamePlayer.Instance.UpdateAmeInfo(false);
                break;
            default:
                break;
        }
    }

    void LoadOtherAssets()
    {
        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            //加载资源
            ENTITY_ID weaponAssetId = 0;
            if (other.playerData_.weaponItemId_ != 0)
                weaponAssetId = (ENTITY_ID)ItemData.GetData(other.playerData_.weaponItemId_).weaponEntityId_;
            int dressId = 0;
            ItemData dress = ItemData.GetData(other.playerData_.fashionId_);
            if (dress != null)
                dressId = dress.weaponEntityId_;
            GameManager.Instance.GetActorClone((ENTITY_ID)other.playerData_.assetId_, weaponAssetId, other.playerData_.type_, AssetsLoadCallBack, new ParamData(ParamData.AssetType.AT_OtherPlayer, other.playerData_.instId_, 0, other.destination_, Quaternion.identity), "Default", dressId);
        }

        if (otherAvatarContainer_.Count == 0)
            SceneOtherLoadFinish();
    }

    void LoadNpcAssets()
    {
        npcTimerStart = true;
        for (int i = 0; i < npcContainer_.Count; ++i)
        {
            NpcData data = NpcData.GetData(npcContainer_[i].npcId_);
            //加载资源
			float y = groundHeight_ + data.PosY;
            GameManager.Instance.GetActorClone((ENTITY_ID)data.AssetsID, (ENTITY_ID)0, EntityType.ET_None, AssetsLoadCallBack, 
                new ParamData(
                    ParamData.AssetType.AT_Npc, 
                    npcContainer_[i].npcId_, 
                    i,
                    new Vector3(data.PosX, y, data.PosZ),
                   Quaternion.AngleAxis(data.RotY, Vector3.up))
                , "Default");
        }

        if (npcContainer_.Count == 0)
            SceneNpcLoadFinish();
    }

    /// <summary>
    /// 退队
    /// </summary>
    void ExitTeam()
    {
		if(GamePlayer.Instance.isInBattle)
			return;

        if (playerAvatar_ == null)
            return;

        SetCameraFocus(playerAvatar_);
        DestroyLeaderMark();
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            playerAvatar_.UpdateName();
        }, 1);
    }

    void CreateTeam(COM_TeamInfo info)
    {
        COM_SimplePlayerInst leader = info.members_[0];
        if (leader != null)
            ChangeLeader((int)leader.instId_);
    }

    /// <summary>
    /// 更换队长
    /// </summary>
    /// <param name="instId"></param>
    void ChangeLeader(int instId)
    {
        if (playerAvatar_ == null)
            return;

        Avatar other = null;
        GamePlayer.Instance.playerSimp_.isLeader_ = false;
        for (int i = 0; i < TeamSystem.MemberCount; ++i)
        {
            int instid = (int)TeamSystem.GetTeamMemberByIndex(i).instId_;
            other = FindPlayer(instid);
            if (other != null)
                other.playerData_.isLeader_ = false;
        }
        other = null;

        COM_SimplePlayerInst self = null;
        if(otherAvatarContainer_.ContainsKey(instId))
            other = otherAvatarContainer_[instId];
        if (other != null)
        {
            other.playerData_.isLeader_ = true;
            self = TeamSystem.GetTeamMemberByInsId(GamePlayer.Instance.InstId);
            //如果自己没有暂离 则更新摄像机焦点
            if (self != null && self.isLeavingTeam_ == false)
                SetCameraFocus(other);
        }
		if (GamePlayer.Instance.InstId == instId)
		{
            GamePlayer.Instance.playerSimp_.isLeader_ = true;
			SetCameraFocus(playerAvatar_);
		}
        else
        {
            NetConnection.Instance.stopMove();
            NetConnection.Instance.stopAutoBattle();
            Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
        }
        GlobalInstanceFunction.Instance.Invoke(() => {
            playerAvatar_.UpdateName();
        }, 1);
    }

    public void LeaveOrBackTeam(int instId, bool isLeave)
    {
        if (playerAvatar_ != null)
        {
            if (GamePlayer.Instance.InstId == instId)
            {
                if (isLeave)
                    SetCameraFocus(playerAvatar_);
                else
                    SetCameraFocus(GetLeader());
            }
        }
    }

    /// <summary>
    /// 添加一个NPC
    /// </summary>
    public void AddNpc(int npcInst)
    {
        Npc npc = FindNpc(npcInst);
        if (npc != null)
            return;

        npc = new Npc();
        npc.Init(npcInst);
        npcContainer_.Add(npc);
        if (running_)
        {
            NpcData npcData = NpcData.GetData(npcInst);
            float y = groundHeight_;
            if (!Mathf.Approximately(npcData.PosY, 0 ))
                y += npcData.PosY;
            GameManager.Instance.GetActorClone((ENTITY_ID)npcData.AssetsID, (ENTITY_ID)0, EntityType.ET_None, AssetsLoadCallBack,
                new ParamData(
                    ParamData.AssetType.AT_Npc,
                    npcData.NpcId,
                    npcContainer_.Count - 1,
                    new Vector3(npcData.PosX, y, npcData.PosZ),
                    Quaternion.AngleAxis(npcData.RotY, Vector3.up))
                , "Default");
        }
    }

    /// <summary>
    /// 删除一个NPC
    /// </summary>
    public void DelNpc(int[] npcIds)
    {
        if (npcContainer_ == null)
            return;

        for (int i = 0; i < npcIds.Length; ++i)
        {
            for (int j = 0; j < npcContainer_.Count; ++j)
            {
                if (npcContainer_[j].npcId_ == npcIds[i])
                {
                    npcContainer_[j].Dispose();
                    npcContainer_.RemoveAt(j);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 找一个npc
    /// </summary>
    /// <param name="npcId"></param>
    public Npc FindNpc(int npcId)
    {
        if (npcContainer_ == null)
            return null;

        for (int i = 0; i < npcContainer_.Count; ++i)
        {
            if (npcContainer_[i].npcId_ == npcId)
            {
                return npcContainer_[i];
            }
        }
        return null;
    }

    public bool AllowShow(int instid)
    {
        if (TeamSystem.IsInTeam() && TeamSystem.IsTeamLeader(instid))
            return true;

        if (TeamSystem.IsInTeam() && TeamSystem.isTeamMember(instid))
            return true;

        if (otherAvatarContainer_ != null && GetVisableOtherNum(instid) < OtherPlayerNumMax)
            return true;

        if (GuildSystem.IsMyGuildMember(instid))
            return true;

        if (FriendSystem.Instance().IsmyFriend(instid))
            return true;

        return false;
    }

    int GetVisableOtherNum(int exceptPlayer)
    {
        int total = 0;
        Avatar other = null;
        for (int i = 0; i < otherPlayerQue_.Count; ++i)
        {
            other = FindPlayer(otherPlayerQue_[i]);
            if (other != null && exceptPlayer == otherPlayerQue_[i])
                continue;

            if (!other.hidden_)
                total += 1;
        }
        return total;
    }

    /// <summary>
    /// 添加一个其他玩家
    /// </summary>
    public void AddOther(COM_ScenePlayerInformation player)
    {
        //如果是家族场景且不是同一家族则筛选掉
        if (GameManager.Instance.CurrentScene.sceneType_ == SceneType.SCT_GuildHome && !GuildSystem.IsMyGuildMember(player.instId_))
            return;

        if (tempDelList_ != null)
        {
            if (tempDelList_.Contains((int)player.instId_))
            {
                Avatar other = FindPlayer((int)player.instId_);
                if (other != null)
                    other.ShowAll();
                tempDelList_.Remove((int)player.instId_);
                return;
            }
        }

        if (FindPlayer((int)player.instId_) != null)
            return;

        Avatar avatar = new Avatar();
        avatar.Init(player, false);
        avatar.destination_ = new Vector3(player.originPos_.x_, groundHeight_, player.originPos_.z_);
        otherAvatarContainer_.Add(player.instId_, avatar);
        otherPlayerQue_.Add(player.instId_);
        //avatar.hidden_ = true;
        if(running_)
        {
            //加载资源
            ENTITY_ID weaponAssetId = 0;
            if (player.weaponItemId_ != 0)
                weaponAssetId = (ENTITY_ID)ItemData.GetData(player.weaponItemId_).weaponEntityId_;
            int dressId = 0;
            ItemData dress = ItemData.GetData(avatar.playerData_.fashionId_);
            if(dress != null)
                dressId = dress.weaponEntityId_;
            GameManager.Instance.GetActorClone((ENTITY_ID)player.assetId_, weaponAssetId, player.type_, AssetsLoadCallBack, new ParamData(ParamData.AssetType.AT_OtherPlayer, player.instId_, otherAvatarContainer_.Count - 1, avatar.destination_, Quaternion.identity), "Default", dressId);
        }
    }

    /// <summary>
    /// 找一个玩家
    /// </summary>
    /// <param name="npcId"></param>
    public Avatar FindPlayer(int instId)
    {
        if (otherAvatarContainer_ == null || !otherAvatarContainer_.ContainsKey(instId))
            return null;

        return otherAvatarContainer_[instId];
    }

    List<int> tempDelList_;
    /// <summary>
    /// 删除一个其他玩家
    /// </summary>
    /// 
    public void DelOther(int instId)
    {
        //如果是家族场景且不是同一家族则筛选掉
        //if (GameManager.Instance.CurrentScene.sceneType_ == SceneType.SCT_GuildHome && !GuildSystem.IsMyGuildMember(instId))
        //    return;

        if (otherAvatarContainer_ == null || otherPlayerQue_ == null)
        {
            return;
        }

        if (tempDelList_ == null)
            tempDelList_ = new List<int>();

        if (!tempDelList_.Contains(instId))
        {
            tempDelList_.Add(instId);
            Avatar other = FindPlayer(instId);
            if (other != null)
                other.HideAll();
        }
    }

    public Avatar GetSelf()
    {
        return playerAvatar_;
    }

    public void WearPlayersOutlook(int instid, int itemid)
    {
        if (itemid == 0)
            return;

        Avatar player = FindPlayer(instid);
        UpdateEquiptListener ueLis = null;
        if (player != null)
        {
            ItemData item = ItemData.GetData(itemid);
            if (item.slot_ == EquipmentSlot.ES_Fashion)
            {
                ENTITY_ID weaponAssetId = 0;
                if (player.playerData_.weaponItemId_ != 0)
                    weaponAssetId = (ENTITY_ID)ItemData.GetData(player.playerData_.weaponItemId_).weaponEntityId_;
                player.playerData_.fashionId_ = itemid;
                GameManager.Instance.GetActorClone((ENTITY_ID)player.playerData_.assetId_, weaponAssetId, player.playerData_.type_, (GameObject go, ParamData data) => 
                {
                    player.Change(go);
                }, null, "Default", item.weaponEntityId_);
            }
            else if(item.slot_ == EquipmentSlot.ES_SingleHand ||
                item.slot_ == EquipmentSlot.ES_DoubleHand)
            {
                player.playerData_.weaponItemId_ = itemid;
                ueLis = player.gameObject_.GetComponent<UpdateEquiptListener>();
                if (ueLis != null)
                    ueLis.UpdateHandler(instid, itemid);
            }
        }
    }

    public void TakeOffPlayersOutlook(int instid, int itemid)
    {
        if (itemid == 0)
            return;

        Avatar player = FindPlayer(instid);
        UpdateEquiptListener ueLis = null;
        if (player != null)
        {
            ItemData item = ItemData.GetData(itemid);
            if (item.slot_ == EquipmentSlot.ES_Fashion)
            {
                ENTITY_ID weaponAssetId = 0;
                if (player.playerData_.weaponItemId_ != 0)
                    weaponAssetId = (ENTITY_ID)ItemData.GetData(player.playerData_.weaponItemId_).weaponEntityId_;
                player.playerData_.fashionId_ = 0;
                GameManager.Instance.GetActorClone((ENTITY_ID)player.playerData_.assetId_, weaponAssetId, player.playerData_.type_, (GameObject go, ParamData data) =>
                {
                    player.Change(go);
                }, null, "Default");
            }
            else if (item.slot_ == EquipmentSlot.ES_SingleHand ||
                item.slot_ == EquipmentSlot.ES_DoubleHand)
            {
                player.playerData_.weaponItemId_ = 0;
                ueLis = player.gameObject_.GetComponent<UpdateEquiptListener>();
                if (ueLis != null)
                    ueLis.RemoveWeaponDirectly(instid);
            }
        }
    }

    public void UpdateSelfOutlook()
    {
        Avatar self = Prebattle.Instance.GetSelf();
        if (self != null)
        {
            string beforeStage = StageMgr.Scene_name;
            GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player, (GameObject go, ParamData data) =>
            {
                if (beforeStage.Equals(StageMgr.Scene_name))
                    self.Change(go);
            }, null, "Default", GamePlayer.Instance.DressID);
        }
    }

    public void UpdateVIP(int instid, int lv)
    {
        Avatar role = null;
        if (instid == GamePlayer.Instance.InstId)
            role = GetSelf();
        else
            role = FindPlayer(instid);
        if (role != null)
            role.UpdateVip(lv);
    }

    /// <summary>
    /// 加载资源总回调
    /// </summary>
	/// 
	public delegate void AssetsLoadNPCFinishEvent();
	public static event AssetsLoadNPCFinishEvent OnAssetsLoadNPCFinish;
    float npcTimer_, NpcTimerMax = 2f;
    bool npcTimerStart = false;
    void AssetsLoadCallBack(GameObject go, ParamData data)
    {
        if (StageMgr.Loading || GlobalValue.isBattleScene(StageMgr.Scene_name))
        {
            ClientLog.Instance.Log("One asset abandoned" + go.name);
            GameObject.Destroy(go);
            isLoading_ = false;
			sceneOtherPlayerLoadedCount_ = 0;
			sceneNpcLoadedCount_ = 0;
            return;
        }

        switch (data.typeParam)
        {
            case ParamData.AssetType.AT_Player:
                go.name = data.iParam.ToString();
                if (playerAvatar_ != null)
                {
                    playerAvatar_.SetObject(go, data.vParam.x, groundHeight_, data.vParam.z);
                    playerAvatar_.speed_ = speed_;
                    playerAvatar_.rotSpeed_ = rotSpeed_;
                    moveToSlot[data.iParam2] = go.transform.position;
                    GamePlayer.Instance.MagicLevelDirty = true;
                    //设置其为摄像机焦点
                    if (playerAvatar_.focus_)
                        SetCameraFocus(playerAvatar_);
                    currentCount_++;
                }
                break;
            case ParamData.AssetType.AT_OtherPlayer:
                sceneOtherPlayerLoadedCount_++;
                go.name = data.iParam.ToString();
                //if (data.iParam2 >= 0 && data.iParam2 < otherAvatarContainer_.Count)
                //{
                if (otherAvatarContainer_.ContainsKey(data.iParam))
                {
                    otherAvatarContainer_[data.iParam].SetObject(go, data.vParam.x, groundHeight_, data.vParam.z);
                    otherAvatarContainer_[data.iParam].speed_ = speed_;
                    otherAvatarContainer_[data.iParam].rotSpeed_ = rotSpeed_;
                    otherAvatarContainer_[data.iParam].UpdateMagicEff();
                }
                else
                {
                    Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    GameObject.Destroy(go);
                }
                //}
                if (sceneOtherPlayerLoadedCount_ == otherAvatarContainer_.Count)
                    SceneOtherLoadFinish();
                break;
            case ParamData.AssetType.AT_Npc:
                npcTimer_ = 0;
                Npc npc = FindNpc(data.iParam);
                if (npc == null)
                    GameObject.Destroy(go);
                else
                {
                    sceneNpcLoadedCount_++;
                    go.name = data.iParam.ToString();
                    if (data.iParam2 >= 0 && data.iParam2 < npcContainer_.Count)
                    {
                        npcContainer_[data.iParam2].SetObject(go, data.vParam.x, data.vParam.y, data.vParam.z, data.qParam);
                    }
                }
                if (sceneNpcLoadedCount_ >= npcContainer_.Count)
                    SceneNpcLoadFinish();
                break;
            default:
                break;
        }
        if (currentCount_ == totalCount_)
        {
            totalCount_ = -1;
            SceneLoaded();

            if (!GuideManager.Instance.IsFinish(41) && StageMgr.Scene_name.Equals(GlobalValue.StageName_piantoudonghuaf))
                senseMode_ = true;
        }

    }

    public void ClearAssetLoadCount()
    {
        sceneOtherPlayerLoadedCount_ = 0;
        sceneNpcLoadedCount_ = 0;
    }

    public void ShowNpcDialog(int npcId)
    {
        if (npcId == 0)
            return;

        if (playerAvatar_ == null)
			return;

        ChangeWalkEff(WalkState.WS_Normal);
        Npc npc = FindNpc(npcId);
        NpcData npcData = NpcData.GetData(npcId);
        if (npc != null && npc.gameObject_ != null && playerAvatar_ != null && playerAvatar_.gameObject_ != null)
        {
            if (npcData.Type != NpcType.NT_Caiji1 && npcData.Type != NpcType.NT_Caiji2 && npcData.Type != NpcType.NT_Caiji3)
                npc.LookAtDir(playerAvatar_.gameObject_.transform);
            playerAvatar_.LookAtDir(npc.gameObject_.transform);
        }
		if (npcData.Type == NpcType.NT_Caiji1 ||npcData.Type == NpcType.NT_Caiji2||npcData.Type == NpcType.NT_Caiji3)
		{
            NpcItemUi.OnProgressBarDown = CaijiProgressFinish;
            if (MainPanle.Instance != null)
                MainPanle.Instance.SetNpcItemUiObj(true, npcId);
        }
        else
            NpcRenwuUI.ShowDialog(npcId);
    }

    void CaijiProgressFinish(int npcId)
    {
        NetConnection.Instance.talkedNpc(npcId);
    }

    public void Transfor2(int instid, COM_FPosition pos)
    {
        if (running_ == false)
            return;

        if (isLoading_)
            return;

        if (GamePlayer.Instance.InstId == instid)
        {
            playerAvatar_.SetPosition(pos.x_, groundHeight_, pos.z_, true);
            playerAvatar_.SetNamePos(new Vector3(pos.x_, groundHeight_, pos.z_));
            }

        if (otherAvatarContainer_.ContainsKey(instid))
        {
            otherAvatarContainer_[instid].SetPosition(pos.x_, groundHeight_, pos.z_, true);
        }
    }

    public void MoveTo(int instId, float x, float z, bool final = false)
    {
        //如果是家族场景且不是同一家族则筛选掉
        if (GameManager.Instance.CurrentScene.sceneType_ == SceneType.SCT_GuildHome && !GuildSystem.IsMyGuildMember(instId))
            return;

        if (playerAvatar_ != null)
        {
            if (GamePlayer.Instance.InstId == instId)
            {
                playerAvatar_.SetMoveTo(x, groundHeight_, z, final);
                if (activeMoved_ == false && TeamSystem.IsInTeam() && TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
                {
                    ChangeWalkEff(WalkState.WS_AFP);
                }
            }
        }

        if (otherAvatarContainer_ != null && otherAvatarContainer_.ContainsKey(instId))
        {
            otherAvatarContainer_[instId].SetMoveTo(x, groundHeight_, z, final);
        }
    }

    /// <summary>
    /// 设置目标点
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="leaveGap"></param>
    void HandleMoveTo(float deltaTime)
    {
        if (running_ == false)
            return;

        if (isLoading_)
            return;

        playerAvatar_.MoveTo(deltaTime);

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            other.MoveTo(deltaTime);
        }
    }

    void Move(Vector3 dest)
    {
		if(GameManager.Instance._IsSenseMode)
		{
			ClientLog.Instance.Log("Is SenseMode@!");
			return;
		}

        activeMoved_ = true;
		if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId) && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return;

        ChangeWalkEff(WalkState.WS_Normal);
        NetConnection.Instance.move(dest.x, dest.z);
        selectPoint_ = new Vector3(dest.x, groundHeight_, dest.z);
        selectedNpc_ = 0;
		if(destEff_ != null)
		{
			destEff_.transform.position = selectPoint_;
			destEff_.SetActive(false);
			destEff_.SetActive(true);
		}
		else
		{
			destEff_ = new GameObject ();
			EffectAPI.PlaySceneEffect((EFFECT_ID)1047, selectPoint_, null, (GameObject go) =>
			                          {
				destEff_ = go;
			});
		}
        if (playerAvatar_ != null)
            playerAvatar_.ClearDestQue();
        if (MainPanle.Instance != null)
        {
            if (MainPanle.Instance.CaijiIng)
                MainPanle.Instance.SetNpcItemUiObj(false);
        }
    }

    void TouchActor(XInput.ActorType type, int instId, Vector3 actorPos)
    {
		if(GameManager.Instance._IsSenseMode)
		{
			ClientLog.Instance.Log("Is SenseMode@!");
			return;
		}

        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId) && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return;

        if(type == XInput.ActorType.AT_Npc)
        {
            selectedNpc_ = instId;
            NetConnection.Instance.moveToNpc(selectedNpc_);
            if (playerAvatar_ != null)
                playerAvatar_.ClearDestQue();

            if (MainPanle.Instance != null)
            {
                if (MainPanle.Instance.CaijiIng)
                    MainPanle.Instance.SetNpcItemUiObj(false);
            }
        }
        else if(type == XInput.ActorType.AT_OtherPlayer)
        {
            if (OnTouchOtherPlayer != null)
                OnTouchOtherPlayer(GetOtherPlayer(instId));
        }
    }

    /// <summary>
    /// 设置相机焦点玩家
    /// </summary>
    public void SetCameraFocus(Avatar avatar)
    {
		if(avatar == null)
			return;

		if(avatar.gameObject_ == null)
			return;

        if (Camera.main == null)
            return;

		if(playerAvatar_ == null)
			return;

        playerAvatar_.focus_ = !avatar.isPlayer_;
        
        //设置相机焦点Object
        NavTest nt = Camera.main.GetComponent<NavTest>();
        if (nt == null) nt = Camera.main.gameObject.AddComponent<NavTest>();
        nt.target = avatar.gameObject_.transform;
        GameObject.Destroy(ghost_);
    }

    COM_ScenePlayerInformation GetOtherPlayer(int instId)
    {
        if (!otherAvatarContainer_.ContainsKey(instId))
            return null;
        Avatar other = otherAvatarContainer_[instId];
        if(other != null)
            return other.playerData_;
        return null;
    }

    Avatar GetLeader()
    {
        if (TeamSystem.IsInTeam() == false)
            return null;

        if (TeamSystem.IsTeamLeader())
            return playerAvatar_;

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            if (other.playerData_.instId_ == TeamSystem.GetMyTeamLeader().instId_)
                return other;
        }

        return null;
    }

    /// <summary>
    /// 更新队长相关标记
    /// </summary>
    public void FlushLeaderMark()
    {
        Avatar leader = GetLeader();
        if (leader == null || leader.gameObject_ == null)
            return;

        EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_duizhang_mark, Vector3.zero, leader.gameObject_.transform, (GameObject markGo) =>
        {
            if (leaderMark_ != null)
                GameObject.Destroy(leaderMark_);
            leaderMark_ = markGo;
        });
    }

    public void BeforeEnterBattle()
    {
        StopSelfActorMove();
        DestroyEvent();

        if (playerAvatar_ != null)
        {
            playerAvatar_.Dispose();
        }

        if (npcContainer_ != null)
        {
            for (int i = 0; i < npcContainer_.Count; ++i)
            {
                npcContainer_[i].Dispose();
            }
        }

        if (otherAvatarContainer_ != null)
        {
            foreach (Avatar other in otherAvatarContainer_.Values)
            {
                other.Dispose();
            }
        }
        isLoading_ = false;
        currentCount_ = 0;
        running_ = false;
    }

    /// <summary>
    /// 销毁队长标记
    /// </summary>
    void DestroyLeaderMark()
    {
        if (leaderMark_ != null)
            GameObject.Destroy(leaderMark_);
    }

    /// <summary>
    /// 销毁副本
    /// </summary>
    public void Fini(bool skipSomeLogic = false)
    {
        totalCount_ = 0;
        currentCount_ = 0;
        updateNameCount_ = 0;
        running_ = false;
        isLoading_ = false;
        battleName_ = "";
        activeMoved_ = false;
        senseMode_ = false;

        DestroyEvent();

        if (playerAvatar_ != null)
        {
            playerAvatar_.Dispose();
        }

        if (npcContainer_ != null)
        {
            for (int i = 0; i < npcContainer_.Count; ++i)
            {
                npcContainer_[i].Dispose();
            }
            npcContainer_.Clear();
        }

        if (otherAvatarContainer_ != null)
        {
            foreach (Avatar other in otherAvatarContainer_.Values)
            {
                other.Dispose();
            }
            otherAvatarContainer_.Clear();
            otherPlayerQue_.Clear();
        }

        if(tempDelList_ != null)
            tempDelList_.Clear();

        if (leaderMark_ != null)
            GameObject.Destroy(leaderMark_);

        _HeightAdjusted = false;
    }

    void DestroyEvent()
    {
        // 销毁事件
        XInput.Instance.OnTouchGround -= Move;
        XInput.Instance.OnTouchActor -= TouchActor;
        TeamSystem.OnCreateTeam -= CreateTeam;
        TeamSystem.OnLeaderChange -= ChangeLeader;
        TeamSystem.OnUpdateMemStateUI -= LeaveOrBackTeam;
        TeamSystem.OnExitIteam -= ExitTeam;
        PrebattleEvent.getInstance.BackEvent = null;
    }

    void OnSceneLoaded(string sceneName)
    {
		if(sceneName.Equals(GlobalValue.StageName_CreateRoleScene) ||
		   sceneName.Equals(GlobalValue.StageName_ReLoginScene))
		{
			return;
		}

        Init();
    }

    /// <summary>
    /// 主动进入一个场景
    /// </summary>
    public void ActiveEnterScene(int sceneId)
    {
		if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader()&& !TeamSystem.memberSelf().isLeavingTeam_)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("duizhangkeyi"), PopText.WarningType.WT_Warning, true);
            return;
        }
        //取消玩家自动操作
        ChangeWalkEff(WalkState.WS_Normal);
        NetConnection.Instance.transforScene(sceneId);
    }


    public COM_SceneInfo nextInfo_;

    /// <summary>
    /// 加入场景协议处理
    /// </summary>
    /// <param name="info"></param>
    public void JoinScene(COM_SceneInfo info)
    {
        if (GameManager.SceneID == (int)info.sceneId_)
        {
            if(running_ && !StageMgr.Loading)
                playerAvatar_.SetPosition(info.position_.x_, groundHeight_, info.position_.z_, true);
            else
                nextInfo_ = info;
        }
        else
        {
            if (StageMgr.Loading)
            {
                nextInfo_ = info;
            }
            else
            {
                Fini();
                GameManager.SceneID = (int)info.sceneId_;
                Prebattle.Instance.EnterSceneOk(new UnityEngine.Vector3(info.position_.x_, 0f, info.position_.z_));
                //添加npc
                for (int i = 0; i < info.npcs_.Length; ++i)
                {
                    AddNpc(info.npcs_[i]);
                }
                //添加其他玩家
                for (int j = 0; j < info.players_.Length; ++j)
                {
                    AddOther(info.players_[j]);
                }
            }
        }
    }

    public bool ExcuteNextScene()
    {
        if (nextInfo_ != null)
        {
            Fini();
            GameManager.SceneID = (int)nextInfo_.sceneId_;
            Prebattle.Instance.EnterSceneOk(new UnityEngine.Vector3(nextInfo_.position_.x_, 0f, nextInfo_.position_.z_));
            //添加npc
            for (int i = 0; i < nextInfo_.npcs_.Length; ++i)
            {
                AddNpc(nextInfo_.npcs_[i]);
            }
            //添加其他玩家
            for (int j = 0; j < nextInfo_.players_.Length; ++j)
            {
                AddOther(nextInfo_.players_[j]);
            }
            nextInfo_ = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 进入场景事件
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="pos"></param>
    public void EnterSceneOk(Vector3 pos, bool resetLocker = false)
    {
        if (isLoading_)
            return;

        cachedPosition_ = pos;
        currentScene_ = GameManager.Instance.CurrentScene;
        battleName_ = currentScene_.battleLevelName_;
        if (GameManager.Instance.SyncLoad)
        {
            StageMgr.LoadingScene(currentScene_.sceneLevelName_);
            GameManager.Instance.SyncLoad = false;
            OnSceneLoaded(currentScene_.sceneLevelName_);
        }
        else
        {
            StageMgr.OnSceneLoaded -= OnSceneLoaded;
            StageMgr.OnSceneLoaded += OnSceneLoaded;
            if (exitFromBattle_)
                StageMgr.LoadingAsyncScene(currentScene_.sceneLevelName_, SwitchScenEffect.SMBlindsTransition, true, false, true, resetLocker);
            else
                StageMgr.LoadingAsyncScene(currentScene_.sceneLevelName_, SwitchScenEffect.LoadingBar, true, false, false, resetLocker);
        }
    }

    /// <summary>
    /// 进入战斗
    /// </summary>
    public void EnterBattle()
    {
        if (running_ == false)
            return;

        if (playerAvatar_ == null)
            return;

        if (!playerAvatar_.HasCachePos())
            cachedPosition_ = playerAvatar_.destination_;
        else
            cachedPosition_ = playerAvatar_.GetLastPosInCache();
        //Fini();
        //BeforeEnterBattle();
    }

    /// <summary>
    /// 退出战斗
    /// </summary>
    public void ExitFromBattle()
    {
        ClientLog.Instance.Log("Exit from battle! ");
        exitFromBattle_ = true;
        updateNameCount_ = 0;
        EnterSceneOk(cachedPosition_);
    }

    public void OnReturnMainScene()
    {
        ChangeWalkEff(WalkState.WS_Normal);
        NetConnection.Instance.transforScene(SceneData.HomeID);
    }

    public bool tooFastOper = false;
    bool tooFastOperTips = false;
    void ClickTextWithInfomation(string info, int questId)
    {
        if (tooFastOper && tooFastOperTips == false)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("tooFastOperation"), PopText.WarningType.WT_Warning);
            tooFastOperTips = true;
            return;
        }

        tooFastOper = true;
        GlobalInstanceFunction.Instance.Invoke(delegate { FastOperation(); }, 0.5f);
        if (!StageMgr.sendSceneLoaded_)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("CannotDoWhenChangeStage"), PopText.WarningType.WT_Tip);
            return;
        }
        //如果已经在寻路或巡逻中 并且id已经记录 不处理
        if (clickedQuestId_ == questId && walkState_ != WalkState.WS_Normal)
        {
            return;
        }
        if (GameManager.Instance.ParseNavMeshInfo(info))
        {
            clickedQuestId_ = questId;
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickMiniQuest);
        }
    }

    void FastOperation()
    {
        tooFastOper = false;
        tooFastOperTips = false;
    }

    public void SwitchAFPEffect(bool on)
    {
        if (playerAvatar_ == null || playerAvatar_.gameObject_ == null)
            return;

        if (on)
        {
            if (afpGo_ == null)
            {
                if (running_ && isLoading_ == false)
				{
					afpGo_ = new GameObject();
                    EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_zidongxunlu, Vector3.zero, playerAvatar_.gameObject_.transform, AutoFindPathEffectCallBack, true, 1f);
				}
            }
            else
            {
                if (afpGo_.activeSelf == false)
                {
                    afpGo_.SetActive(false);
                    afpGo_.SetActive(true);
                }
            }
        }
        else
        {
            if (afpGo_ != null)
            {
                afpGo_.SetActive(false);
            }
        }
    }

    public void SwitchAMEEffect(bool on)
    {
        if (playerAvatar_ == null || playerAvatar_.gameObject_ == null)
            return;

        if (on)
        {
            if (ameGo_ == null)
            {
                if (running_ && isLoading_ == false)
				{
					ameGo_ = new GameObject();
                    EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_zidongyudi, Vector3.zero, playerAvatar_.gameObject_.transform, AutoMeetEnemyEffectCallBack, true, 1f);
				}
            }
            else
            {
                if (ameGo_.activeSelf == false)
                {
                    ameGo_.SetActive(false);
                    ameGo_.SetActive(true);
                }
            }
        }
        else
        {
            if (ameGo_ != null)
            {
                ameGo_.SetActive(false);
            }
        }
    }

    public void UpdateInBattle(int instid, bool inBattle)
    {
        Avatar player = FindPlayer(instid);
        if (player != null)
            player.UpdateInBattle(inBattle);
		TeamSystem.UpdateInBattle (instid,inBattle);
    }

    public void UpdateLeaderMark(int instid, bool isLeader)
    {
        Avatar player = Prebattle.Instance.FindPlayer(instid);
        if (player != null)
		{
            player.UpdateLeaderMark(isLeader);
			if(isLeader)
				player.SwitchFollowBaby(false);
		}
        else
        {
            if (instid == GamePlayer.Instance.InstId)
            {
                if (playerAvatar_ != null)
                    playerAvatar_.UpdateLeaderMark(isLeader);
                GamePlayer.Instance.playerSimp_.isLeader_ = isLeader;
            }
        }
    }

	public void UpdateInTeam(int instid, bool inTeam)
	{
		Avatar player = Prebattle.Instance.FindPlayer(instid);
		if (player != null)
		{
			if(player.playerData_ != null)
				player.playerData_.isTeamMember_ = inTeam;
			if(inTeam)
				player.SwitchFollowBaby(false);
		}
	}

	GameObject afpGo_, ameGo_;
	void AutoFindPathEffectCallBack(GameObject go)
	{
		if(afpGo_ != null)
			go.SetActive(afpGo_.activeSelf);
        afpGo_ = go;
	}

    void AutoMeetEnemyEffectCallBack(GameObject go)
    {
		if(ameGo_ != null)
			go.SetActive(ameGo_.activeSelf);
        ameGo_ = go;
    }

    public void SetCachePos(float x, float y)
    {
        cachedPosition_ = new Vector3(x, 0f, y);
    }

	public int WeaponID(Actor player)
	{
		COM_Item weapon = player.Equips[(int)EquipmentSlot.ES_SingleHand];
		if (weapon == null)
		{
			weapon = player.Equips[(int)EquipmentSlot.ES_DoubleHand];
			if (weapon == null)
				return 0;
		}
		return (int)weapon.itemId_;
	}
	
	void ShowBattleReward()
	{
        if (Battle.Instance.BattleReward != null)
        {
            if (GamePlayer.Instance.isLevelUp_)
            {
                if (GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerLevelUp, GamePlayer.Instance.GetIprop(PropertyType.PT_Level)))
                    return;
            }

            if (GamePlayer.Instance.BattleBaby != null && GamePlayer.Instance.BattleBaby.isLevelUp_)
            {
                ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("chongwushengji").Replace("{n}", GamePlayer.Instance.BattleBaby.Properties[(int)PropertyType.PT_Level].ToString()));
                if (GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLevelUp))
                    return;
            }

			if(	BagSystem.instance.isBattlebagfull)
			{
				BagSystem.instance.isBattlebagfull = false;
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("bagfullsort"),()=>{
					BagUI.SwithShowMe();
				});
			}
            
			if(Battle.Instance.BattleBabyExp>0)
			{
                ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("chongwujingyan").Replace("{n}", Battle.Instance.BattleBabyExp.ToString()));
			}
			if(Battle.Instance.BattleReward.playExp_>0)
                ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("playerjingyan").Replace("{n}", Battle.Instance.BattleReward.playExp_.ToString()));
            
//            if(judgeType_ == BattleJudgeType.BJT_Win)
//                ClearingPanel.ShowMe();
//            else if (judgeType_ == BattleJudgeType.BJT_Lose)
//				ClearingFailurePanel.ShowMe();
			QuestSystem.UpdateQuest();

		}
        GamePlayer.Instance.isInBattle = false;
        GlobalInstanceFunction.Instance.Invoke( delegate
        {
            PopText.Instance.ExcuteCache();
            if(judgeType_ == BattleJudgeType.BJT_Lose && RaiseUpSystem.Instance.HasItem)
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleOverRewardOpen);
        }, 1f);
	}

    public void HideAllPeople()
    {
        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            other.HideAll();
        }
        for (int i = 0; i < npcContainer_.Count; ++i)
        {
            npcContainer_[i].HideAll();
        }
        senseMode_ = true;
    }

    public void ShowAllPeople()
    {
        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            other.ShowAll();
        }
        for (int i = 0; i < npcContainer_.Count; ++i)
        {
            npcContainer_[i].ShowAll();
        }
        senseMode_ = false;
    }

    public bool Running
    {
        get { return running_; }
    }

    float lastTimeStamp_;
	public void Update ()
	{
        float deltaTime = Time.realtimeSinceStartup - lastTimeStamp_;
        lastTimeStamp_ = Time.realtimeSinceStartup;

        if(npcTimerStart)
        {
            npcTimer_ += deltaTime;
            if (npcTimer_ > NpcTimerMax)
            {
                npcTimer_ = 0f;
                SceneNpcLoadFinish();
            }
        }

		if(!running_)
			return;

        if (StageMgr.Loading)
            return;

		if (!TeamSystem.IsInTeam() || (TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))||TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            XInput.Instance.Update();

        if (GamePlayer.Instance.ShowBabyDirty)
        {
            Baby baby = GamePlayer.Instance.GetShowBaby();
            if (baby != null)
                playerAvatar_.SwitchFollowBaby(true, baby.GetIprop(PropertyType.PT_TableId), baby.InstName);
            else
                playerAvatar_.SwitchFollowBaby(false);
            GamePlayer.Instance.ShowBabyDirty = false;
        }

        HandleMoveTo(deltaTime);
        
        // 更新所有玩家名字位置和动画
		if(TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader())
            playerAvatar_.UpdateName();
        playerAvatar_.UpdateAnimation();
        playerAvatar_.UpdateLeaderMark(GamePlayer.Instance.playerSimp_.isLeader_);
        if (GamePlayer.Instance.MagicLevelDirty)
            playerAvatar_.UpdateMagicEff();

        if(playerAvatar_.myBaby_ != null)
            playerAvatar_.myBaby_.UpdateName();

        if (senseMode_)
            return;

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            //other.UpdateVisable();
            if (!other.hidden_)
            {
                other.UpdateName();
                other.UpdateAnimation();
                other.UpdateInBattle(other.playerData_.isInBattle_);
                other.UpdateLeaderMark(other.playerData_.isLeader_);
            }
        }

        if (tempDelList_ != null && tempDelList_.Count > 0)
        {
            int instId = tempDelList_[0];
            tempDelList_.RemoveAt(0);
            if (otherAvatarContainer_.ContainsKey(instId))
            {
                otherAvatarContainer_[instId].Dispose();
                otherAvatarContainer_.Remove(instId);
                otherPlayerQue_.Remove(instId);
            }
        }

        for (int i = 0; i < npcContainer_.Count; ++i)
        {
            if(npcContainer_[i] != null)
                npcContainer_[i].UpdateHeight();
        }

        identifyHeight();
    }

    void OnLevelUp(int lv)
    {
        if (running_ == false || isLoading_)
            return;

        if (playerAvatar_ == null || playerAvatar_.gameObject_ == null)
            return;

        EffectAPI.PlaySceneEffect((EFFECT_ID)GlobalValue.EFFECT_PlayerLvUpInScene, Vector3.zero, playerAvatar_.gameObject_.transform, null, true, 0f);
		QuestSystem.UpdateQuest();
	}

    public bool CanReachable(Vector3 srcPos, Vector3 destPos)
    {
        NavMeshPath path = new NavMeshPath();
        bool ret = NavMesh.CalculatePath(srcPos, destPos, 1, path);
        ret = (path.status == NavMeshPathStatus.PathComplete);
        return ret;
    }

    void OnCancelAuto()
    {
        //如果不是两种自动状态
        if (walkState_ != WalkState.WS_Normal)
        {
            StopAllAutoSystem();
            NetConnection.Instance.stopMove();
        }
    }

    void OnRenwuHide()
    {
        if(MainPanle.Instance != null)
        {
            MainPanle.Instance.gameObject.SetActive(true);
        }
        NpcRenwuUI.OnNpcRenwuHide -= OnRenwuHide;
    }

    public string CurrentBattleName
    {
        get 
		{
			if(currentScene_ == null)
				return "";
			return currentScene_.battleLevelName_;
		}
    }

    void YAxisLookAt(Transform src, Vector3 dest)
    {
        dest.y = src.position.y;
        src.LookAt(dest);
    }

    public void StopAllAutoSystem()
    {
        if (!running_)
            return;

        if (StageMgr.Loading)
            return;

        SwitchAFPEffect(false);
        SwitchAMEEffect(false);

        GamePlayer.Instance.UpdateAmeInfo(false);
    }

    public void StopSelfActorMove()
    {
        if (playerAvatar_ != null)
            playerAvatar_.PlayerStoped_ = true;
    }

    bool rangeProtector_;

    float autoFindBattleTimer_;

    public void ResetCurrentQuest()
    {
        clickedQuestId_ = 0;
    }

    public bool KillOneActor()
    {
        if (!running_)
            return false;

        int instid = ActorHideFilter();
        if (instid != -1)
        {
            otherAvatarContainer_[instid].Hide();
            return true;
        }
        
        return false;
    }

    public void KillAllActor()
    {
        if (!running_)
            return;

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            if (!other.hidden_)
                other.Hide();
        }
    }

    public bool RebornOneActor()
    {
        if (!running_)
            return false;

        int instid = ActorShowFilter();
        if (instid != -1)
        {
            otherAvatarContainer_[instid].Show();
            return true;
        }
        return false;
    }

    public void RebornAllActor()
    {
        if (!running_)
            return;

        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            if (other.hidden_)
                other.Show();
        }
    }

    int ActorHideFilter()
    {
        int forceIndex = -1;
        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            //如果是队友跳过筛选
            if (TeamSystem.isTeamMember(other.playerData_.instId_))
                continue;

            //如果她不是我的好友
            if (!FriendSystem.Instance().IsmyFriend(other.playerData_.instId_))
            {
                //如果他和我不是同一家族
                if (!GuildSystem.IsInMyGuild(other.playerData_.instId_))
                {
                    //如果他没被隐藏
                    if (!other.hidden_)
                        return other.playerData_.instId_;
                }
                else
                    forceIndex = other.playerData_.instId_;
            }
            else
                forceIndex = other.playerData_.instId_;
        }

        return forceIndex;
    }

    int ActorShowFilter()
    {
        foreach (Avatar other in otherAvatarContainer_.Values)
        {
            //如果是队友跳过筛选
            if (TeamSystem.isTeamMember(other.playerData_.instId_))
                continue;

            //如果他和我是同一家族
            if (GuildSystem.IsInMyGuild(other.playerData_.instId_))
            {
                //如果他被隐藏
                if (other.hidden_)
                    return other.playerData_.instId_;
            }

            //如果她是我的好友
            if (FriendSystem.Instance().IsmyFriend(other.playerData_.instId_))
            {
                //如果他被隐藏
                if (other.hidden_)
                    return other.playerData_.instId_;
            }
        }

        return -1;
    }
}

public class Avatar
{
    public bool isPlayer_;

    public string nameColor_ = "";

    public string nickNameColor_ = "";

    public COM_ScenePlayerInformation playerData_;

    //Object
    public GameObject gameObject_;

    //Transform
    public Transform transform_;

    //Collider
    public Collider collider_;

    //动画控制
    public Animator animator_;

    //遛宠
    public Avatar myBaby_;

    //是否遛宠
    public bool myBabyOut_;

    //宠物名字
    public string babyName_ = "";

    //移动控件
    public CharacterController cc_;

    //移动速度
    public float speed_;

    //转向速度
    public float rotSpeed_;

    //是否为摄像机焦点
    public bool focus_;

    //是否隐藏
    public bool hidden_;

    //名字
    public UILabel nameLabel_;

    //会员标识
    public UISprite vipSp_;

    //名字的Transform
    public Transform nameLblTrans_;

    //当前目标点
    public Vector3 destination_;

    Queue<destQueItem> destQue_;

    //场景最终点
    public bool finalDest_;

    //玩家强制终止
    public bool PlayerStoped_;

    //战斗特效
    public GameObject inBattleEff_;
    public Transform inBattleEffTrans_;

    //队长特效
    public GameObject leaderMark_;
    public Transform leaderMarkTrans_;

    //神器特效
    public GameObject magicLevelEff_;
    public EFFECT_ID EffID;

    //不使用快速消耗队列更新坐标
    bool dontUseQue_ = false;

    //上一次的跑动动画
    string preRunAnimStr_ = "";

    //称号
    string titleName = "";

    class destQueItem
    {
        public Vector3 dest_;
        public bool final_;
    }

    State state_ = State.ST_Idle;
    enum State
    {
        ST_Idle,
        ST_Run,
    }

    public void Init()
    {
        isPlayer_ = true;
        focus_ = true;
        destQue_ = new Queue<destQueItem>();

        dontUseQue_ = TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId);
    }

    public void Init(COM_ScenePlayerInformation actor, bool focus)
    {
        isPlayer_ = false;
        playerData_ = actor;
        focus_ = focus;
        destQue_ = new Queue<destQueItem>();

        dontUseQue_ = TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId);
    }

    public void Init(bool isBaby)
    {
        isPlayer_ = false;
        destQue_ = new Queue<destQueItem>();
        dontUseQue_ = true;
    }

    public void ClearDestQue()
    {
        if(destQue_ != null)
            destQue_.Clear();
    }

    public void UpdateLastetPosition()
    {
        if (destQue_ != null && destQue_.Count != 0)
        {
            while(destQue_.Count > 1)
            {
                destQue_.Dequeue();
            }
            destQueItem dest = destQue_.Dequeue();
            SetPosition(dest.dest_.x, dest.dest_.y, dest.dest_.z, dest.final_);
        }
    }

    public bool HasCachePos()
    {
        return (destQue_ != null && destQue_.Count > 0);
    }

    public Vector3 GetLastPosInCache()
    {
        if (destQue_ != null && destQue_.Count != 0)
        {
            while (destQue_.Count > 1)
            {
                destQue_.Dequeue();
            }
            return destQue_.Dequeue().dest_;
        }
        return Vector3.one;
    }

    public void Change(GameObject go)
    {
        if (gameObject_ == null)
        {
            GameObject.Destroy(go);
            return;
        }

        go.name = gameObject_.name;
        go.transform.rotation = transform_.rotation;
        go.transform.position = transform_.position;
        transform_ = go.transform;
        GameObject.Destroy(gameObject_);
        gameObject_ = go;
        gameObject_.tag = "Player";
        gameObject_.layer = LayerMask.NameToLayer("Player");
        if (!isPlayer_)
            gameObject_.AddComponent<WiseNpc>();
        else
            Prebattle.Instance.SetCameraFocus(this);

        animator_ = gameObject_.GetComponent<Animator>();
        cc_ = gameObject_.GetComponent<CharacterController>();
        BoxCollider bc = gameObject_.GetComponent<BoxCollider>();
        if (bc == null)
            bc = gameObject_.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        collider_ = gameObject_.collider;

        //不可容忍
        if (!isPlayer_ && animator_ == null)
        {
            ClientLog.Instance.LogError("This Actor has nothing animator. Name is : " + playerData_.instName_);
        }

        //添加移动控件
        if (cc_ == null)
        {
            cc_ = gameObject_.AddComponent<CharacterController>();
            cc_.center = new Vector3(0f, 0.5f, 0f);
            cc_.radius = 0f;
            cc_.height = 0.7f;
            cc_.detectCollisions = false;
        }

        gameObject_.SetActive(!Prebattle.Instance.senseMode_);
    }

    public void SetObject(GameObject go, float x, float y, float z, bool isBaby = false)
    {
        transform_ = go.transform;
        gameObject_ = go;
        gameObject_.tag = isBaby? "Untagged": "Player";
        gameObject_.layer = LayerMask.NameToLayer("Player");
        if(!isPlayer_)
            gameObject_.AddComponent<WiseNpc>();

        if (destQue_ != null)
            destQue_.Clear();
        SetPosition(x, y, z, true);

        animator_ = gameObject_.GetComponent<Animator>();
        cc_ = gameObject_.GetComponent<CharacterController>();
        BoxCollider bc = gameObject_.GetComponent<BoxCollider>();
        if (bc == null)
            bc = gameObject_.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        collider_ = gameObject_.collider;

        //不可容忍
        if (!isPlayer_ && animator_ == null)
        {
            ClientLog.Instance.LogError("This Actor has nothing animator. Name is : " + playerData_.instName_);
        }

        //添加移动控件
        if (cc_ == null)
        {
            cc_ = gameObject_.AddComponent<CharacterController>();
            cc_.center = new Vector3(0f, 0.5f, 0f);
            cc_.radius = 0f;
            cc_.height = 0.7f;
            cc_.detectCollisions = false;
        }

        //创建名字
        nameLabel_ = (GameObject.Instantiate(ApplicationEntry.Instance.nameLabel) as GameObject).GetComponent<UILabel>();
        nameLblTrans_ = nameLabel_.transform;
        nameLblTrans_.parent = ApplicationEntry.Instance.uiRoot.transform;
        nameLabel_.name = "NameLabel";
        vipSp_ = nameLblTrans_.GetComponentInChildren<UISprite>();
        if (!isPlayer_)
            nameLblTrans_.localPosition = GlobalInstanceFunction.WorldToUI(new Vector3(go.transform.position.x, go.transform.position.y - go.collider.bounds.size.y / 4, go.transform.position.z));
        else
            nameLblTrans_.localPosition = new Vector2(0f, -40f);
        nameLblTrans_.localScale = Vector3.one;
        int instId = 0;
        bool isLeader;
        bool isInBattle;
		bool isInTeam;
        string playerName;
        if (!isBaby)
        {
            if (isPlayer_)
            {
                instId = GamePlayer.Instance.InstId;
                isLeader = GamePlayer.Instance.playerSimp_.isLeader_;
                isInBattle = GamePlayer.Instance.isInBattle;
                playerName = GamePlayer.Instance.InstName;
                TitleData tData = TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title));
                if (tData == null)
                    titleName = "";
                else
                    titleName = tData.desc_;
                nameColor_ = GlobalValue.PlayerNameInScene;
                nickNameColor_ = GlobalValue.NickNameInScene;
                Baby baby = GamePlayer.Instance.GetShowBaby();
                if (baby != null)
                    SwitchFollowBaby(true, baby.GetIprop(PropertyType.PT_TableId), baby.InstName);
                else
                    SwitchFollowBaby(false);
            }
            else
            {
                instId = playerData_.instId_;
                isLeader = playerData_.isLeader_;
                isInBattle = playerData_.isInBattle_;
				isInTeam = playerData_.isTeamMember_;
                playerName = playerData_.instName_;
                TitleData tData = TitleData.GetTitleData(playerData_.title_);
                if (tData == null)
                    titleName = "";
                else
                    titleName = tData.desc_;
                nameColor_ = GlobalValue.OtherPlayerNameInScene;
                nickNameColor_ = GlobalValue.NickNameInScene;
                SwitchFollowBaby(playerData_.showBabyTableId_ != 0, playerData_.showBabyTableId_, playerData_.showBabyName_);
            }
            UpdateVip(isPlayer_ ? GamePlayer.Instance.vipLevel_ : playerData_.vip_);
            SceneData ssd = SceneData.GetData(GameManager.SceneID);
            if (ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
            {

                //如果是instcane场景 且不是自己家族的 改成红色
                if (!GuildSystem.IsInMyGuild(instId))
                    nameColor_ = GlobalValue.EnemyNameInScene;

            }
            UpdateLeaderMark(isLeader);
            UpdateInBattle(isInBattle);
        }
        else
        {
            nameColor_ = GlobalValue.PlayerNameInScene;
            UpdateVip(0);
        }
        SetTitleName(titleName);
        gameObject_.SetActive(!Prebattle.Instance.senseMode_);
        nameLabel_.gameObject.SetActive(!Prebattle.Instance.senseMode_);
        if (Prebattle.Instance.nameRootPanel_ == null)
            Prebattle.Instance.nameRootPanel_ = new List<GameObject>();

        if (hidden_)
            HideAll();

        GlobalInstanceFunction.Instance.InvokeRepeat(InvokeBabyMove, 1f);
    }

    void InvokeBabyMove()
    {
        if (myBaby_ != null && gameObject_ != null)
        {
            Vector3 bpos = gameObject_.transform.position;
            bpos -= gameObject_.transform.forward;
            myBaby_.SetMoveTo(bpos.x, bpos.y, bpos.z, finalDest_);
        }
    }

    public void SwitchFollowBaby(bool bout, int tableid = 0, string name = "")
    {
		if(playerData_ != null)
		{
			if(bout)
				bout = !playerData_.isLeader_ && !playerData_.isTeamMember_;
		}
        myBabyOut_ = bout;
        if (bout)
        {
            if (myBaby_ != null)
            {
                myBaby_.Dispose();
                myBaby_ = null;
            }
            myBaby_ = new Avatar();
            myBaby_.Init(true);
            myBaby_.babyName_ = name;

            BabyData bd = BabyData.GetData(tableid);
            //加载宠物资源
            GameManager.Instance.GetActorClone((ENTITY_ID)bd._AssetsID, 0, EntityType.ET_Baby, delegate(GameObject go, ParamData data)
            {
                if (!myBabyOut_ || gameObject_ == null)
                {
                    GameObject.Destroy(go);
                    if (myBaby_ != null)
                        myBaby_.Dispose();
                    myBaby_ = null;
                    return;
                }
                Vector3 bbDest = destination_ - gameObject_.transform.forward;
                myBaby_.SetObject(go, bbDest.x, bbDest.y, bbDest.z, true);
                myBaby_.speed_ = speed_ - 1.5f;
                myBaby_.rotSpeed_ = rotSpeed_;
            }, null, "Default");
        }
        else
        {
            if (myBaby_ != null)
            {
                myBaby_.Dispose();
                myBaby_ = null;
            }
        }
    }

    public void UpdateVip(int viplv)
    {
        if (vipSp_ == null)
            return;

        vipSp_.spriteName = string.Format("vip_{0}", viplv);
        vipSp_.gameObject.SetActive(viplv != 0);
        UpdateVipPos();
    }

    public string GetName()
    {
        return isPlayer_ ? GamePlayer.Instance.InstName : playerData_.instName_;
    }

    public void UpdateVisable()
    {
        bool visable;
        if(!isPlayer_)
            visable = Prebattle.Instance.AllowShow(playerData_.instId_);
        else
            visable = Prebattle.Instance.AllowShow(GamePlayer.Instance.InstId);
        if (visable)
            ShowAll();
        else
            HideAll();
    }

    bool inBattleLoading_;
    public void UpdateInBattle(bool inBattle)
    {
        PlayerStoped_ = inBattle;

        if (!isPlayer_)
        {
            if (playerData_ != null)
                playerData_.isInBattle_ = inBattle;
        }

        if (gameObject_ == null)
            return;

        if (Prebattle.Instance.running_ == false)
            return;

        //更新特效
        if (inBattle)
        {
            if (inBattleLoading_)
                return;

            if (inBattleEff_ == null)
            {
                inBattleLoading_ = true;
                EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_InBattle, Vector3.zero, transform_, (GameObject markGo) =>
                {
                    inBattleLoading_ = false;
                    inBattleEff_ = markGo;
                    inBattleEffTrans_ = markGo.transform;
                }, true, Prebattle.Instance.modelHeight_);
            }
            else
            {
                inBattleEffTrans_.localPosition = new Vector3(0f, Prebattle.Instance.modelHeight_, 0f);
                inBattleEff_.SetActive(inBattle);
            }
        }
        else
        {
            if (inBattleEff_ != null)
                inBattleEff_.SetActive(inBattle);
        }
    }

    bool inLeaderLoading_;
    public void UpdateLeaderMark(bool isLeader)
    {
        if (!isPlayer_)
        {
            if (playerData_ != null)
                playerData_.isLeader_ = isLeader;
        }
        else
        {
            GamePlayer.Instance.playerSimp_.isLeader_ = isLeader;
        }

        if (gameObject_ == null)
            return;

        //更新特效
        if (isLeader)
        {
            if (inLeaderLoading_)
                return;

            if (leaderMark_ == null)
            {
                inLeaderLoading_ = true;
                EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_duizhang_mark, Vector3.zero, transform_, (GameObject markGo) =>
                {
                    inLeaderLoading_ = false;
                    leaderMark_ = markGo;
                    leaderMarkTrans_ = markGo.transform;
                }, true);
            }
            else
            {
                leaderMarkTrans_.localPosition = Vector3.zero;
                leaderMark_.SetActive(isLeader);
            }
        }
        else
        {
            if (leaderMark_ != null)
                leaderMark_.SetActive(isLeader);
        }
    }

    public void SetPosition(float x, float y, float z, bool final = false)
    {
        destination_ = new Vector3(x, y, z);
        StopMove();
    }

    public void UpdateName()
    {
        if (gameObject_ == null)
            return;

        if (nameLabel_ != null && nameLblTrans_ != null)
        {
            nameLblTrans_.localPosition = GlobalInstanceFunction.WorldToUI(new Vector3(transform_.position.x, transform_.position.y - collider_.bounds.size.y / 4, transform_.position.z));
        }
        if (inBattleEff_ != null && inBattleEff_.activeSelf)
        {
            inBattleEffTrans_.localPosition = new Vector3(inBattleEffTrans_.localPosition.x, transform_.position.y, inBattleEffTrans_.localPosition.z);
        }

        if (myBaby_ != null)
        {
            myBaby_.UpdateName();
        }
    }

    public void SetNamePos(Vector3 pos)
    {
        if (gameObject_ == null)
            return;

        if (nameLabel_ != null && nameLblTrans_ != null)
        {
            nameLblTrans_.localPosition = GlobalInstanceFunction.WorldToUI(new Vector3(pos.x, pos.y - collider_.bounds.size.y / 4, pos.z));
        }
        if (inBattleEff_ != null && inBattleEff_.activeSelf)
        {
            inBattleEffTrans_.localPosition = new Vector3(inBattleEffTrans_.localPosition.x, pos.y, inBattleEffTrans_.localPosition.z);
        }
    }

    int changeGapFrame = 5;
    int crtGapFrame;

    public void UpdateAnimation()
    {
        if (animator_ != null)
        {
            ItemData weapon = null;
            if(!isPlayer_ && playerData_ != null)
                weapon = ItemData.GetData(playerData_.weaponItemId_);
            WeaponType wt = WeaponType.WT_None;
            if (weapon != null)
                wt = weapon.weaponType_;
            else
            {
                if (isPlayer_)
                    wt = GamePlayer.Instance.GetWeaponType();
            }
            string newStr = wt.ToString();
            newStr = newStr.Replace("WT_None", "");
            newStr = newStr.Replace("WT_Knife", "");
            if (!preRunAnimStr_.Equals(newStr))
            {
                animator_.SetFloat(preRunAnimStr_ + GlobalValue.FMove, 0f);
                preRunAnimStr_ = newStr;
            }
            Vector2 player = new Vector2(transform_.position.x, transform_.position.z);
            Vector2 dest = new Vector2(Prebattle.Instance.selectPoint_.x, Prebattle.Instance.selectPoint_.z);
            crtGapFrame ++;
            if (crtGapFrame >= changeGapFrame)
            {
                crtGapFrame = 0;
                if (isPlayer_)
                {
                    if (state_ == State.ST_Idle && Prebattle.Instance.walkState_ == Prebattle.WalkState.WS_Normal)
                        animator_.SetFloat(preRunAnimStr_ + GlobalValue.FMove, 0f);
                    else
                        animator_.SetFloat(preRunAnimStr_ + GlobalValue.FMove, 1f);
                }
                else
                {
                    if (state_ == State.ST_Idle)
                        animator_.SetFloat(preRunAnimStr_ + GlobalValue.FMove, 0f);
                    else
                        animator_.SetFloat(preRunAnimStr_ + GlobalValue.FMove, 1f);
                }
            }

            if (myBaby_ != null)
                myBaby_.UpdateAnimation();
        }
    }

    public void UpdateMagicEff()
    {
        int magicLevel = 0;
        if (isPlayer_)
        {
            magicLevel = GamePlayer.Instance.MagicItemLevel;
            GamePlayer.Instance.MagicLevelDirty = false;
        }
        else
        {
            magicLevel = playerData_.magicLv_;
        }

        EFFECT_ID newEffId = (EFFECT_ID)0;
        if (magicLevel >= 10)
            newEffId = EFFECT_ID.EFFECT_MagicEffect_1;

        if (magicLevel >= 20)
            newEffId = EFFECT_ID.EFFECT_MagicEffect_2;

        if (magicLevel >= 30)
            newEffId = EFFECT_ID.EFFECT_MagicEffect_3;

        if (isPlayer_)
        {
            if (EffID == newEffId)
                return;
        }

        EffID = newEffId;

        if (magicLevelEff_ != null)
            GameObject.Destroy(magicLevelEff_);

        EffectAPI.PlaySceneEffect(EffID, Vector3.zero, transform_, (GameObject markGo) =>
                {
                    magicLevelEff_ = markGo;
                    CrtClipName ccn = magicLevelEff_.GetComponent<CrtClipName>();
                    if(ccn == null)
                        magicLevelEff_.AddComponent<CrtClipName>();
                });
    }

    public void StopMove()
    {
        if (gameObject_ == null)
            return;

        transform_.position = destination_;
        destination_ = transform_.position;
        if (destQue_.Count > 0)
        {
            destQueItem item = destQue_.Dequeue();
            destination_ = item.dest_;
            finalDest_ = item.final_;
            Avatar player = null;
            if(!isPlayer_)
                player = Prebattle.Instance.FindPlayer(playerData_.instId_);
            if (player != null && player.PlayerStoped_)
                finalDest_ = true;
        }
    }

    public void SetMoveTo(float x, float y, float z, bool final = false)
    {
        destQueItem item = new destQueItem();
        item.dest_ = new Vector3(x, y, z);
        item.final_ = final;
        if (dontUseQue_)
        {
            destination_ = new Vector3(x, y, z);
            finalDest_ = final;
        }
        else
            destQue_.Enqueue(item);

        //if (myBaby_ != null)
        //{
        //    Vector3 bpos = gameObject_.transform.position;
        //    bpos -= gameObject_.transform.forward * 0.3f;
        //    myBaby_.SetMoveTo(bpos.x, bpos.y, bpos.z, final);
        //}
    }

    public bool MoveTo(float deltaTime)
    {
        if (cc_ == null)
            return false;

        if (LeaderIsStop())
        {
            if (!isPlayer_ && playerData_ != null)
            {
                if (TeamSystem.isTeamMember(playerData_.instId_))
                    finalDest_ = true;
            }
        }

        if (PlayerStoped_)
            finalDest_ = true;

        //单位速度
        deltaTime = deltaTime * speed_;

        //单位向量

        Vector3 dir = destination_ - transform_.position;

        Vector3 v = dir.normalized * deltaTime * (destQue_.Count > 5? destQue_.Count: 1);

        dir.y = transform_.forward.y;

        Prebattle.Instance.AdjustHeight();

        if (dir.normalized.Equals(Vector3.zero))
        {
            dir.x = 0f;
            dir.y = 0f;
            dir.z = 0f;
        }

        if (dir.magnitude > v.magnitude)
        {
            cc_.Move(v);
            state_ = State.ST_Run;
        }
        else
        {
            StopMove();
            if (finalDest_)
            {
                state_ = State.ST_Idle;
            }
        }

        Vector3 newDir = Vector3.RotateTowards(transform_.forward, dir, rotSpeed_ * Time.deltaTime, 0f);
        transform_.rotation = Quaternion.LookRotation(newDir);

        if (myBaby_ != null)
            myBaby_.MoveTo(deltaTime);

        return false;
    }

    /// <summary>
    /// Look at target with direction
    /// </summary>
    /// <param name="target"></param>
    public void LookAtDir(Transform target)
    {
        Vector3 position = target.position;
        position.y = gameObject_.transform.position.y;
        gameObject_.transform.LookAt(position);
    }

    public void Dispose()
    {
        if (nameLabel_ != null && nameLabel_.gameObject != null)
            GameObject.Destroy(nameLabel_.gameObject);
        if (gameObject_ != null)
            GameObject.Destroy(gameObject_);
        if (inBattleEff_ != null)
            GameObject.Destroy(inBattleEff_);
        if (leaderMark_ != null)
            GameObject.Destroy(leaderMark_);
        if (magicLevelEff_ != null)
            GameObject.Destroy(magicLevelEff_);
        if (myBaby_ != null)
            myBaby_.Dispose();
        if(destQue_ != null)
            destQue_.Clear();
    }

    bool LeaderIsStop()
    {
        COM_SimplePlayerInst leader = TeamSystem.GetMyTeamLeader();
        if (leader == null)
            return false;

        Avatar leaderAvatar = Prebattle.Instance.FindPlayer((int)leader.instId_);
        if (leaderAvatar == null)
        {
            //如果自己就是队长 则返回自己的标记
            leaderAvatar = Prebattle.Instance.GetSelf();
            if (GamePlayer.Instance.InstId == leader.instId_)
                return leaderAvatar.finalDest_;
            else
                return false;
        }

        return leaderAvatar.finalDest_;
    }

    public void Show()
    {
        hidden_ = false;
        string objName = "";
        GameObject childObj = null;
        for (int i = 0; i < gameObject_.transform.childCount; ++i)
        {
            childObj = gameObject_.transform.GetChild(i).gameObject;
            objName = childObj.name;
            if (!objName.Equals("Plane01"))
                childObj.SetActive(true);
        }
    }

    public void Hide()
    {
        hidden_ = true;
        string objName = "";
        GameObject childObj = null;
        for (int i = 0; i < gameObject_.transform.childCount; ++i)
        {
            childObj = gameObject_.transform.GetChild(i).gameObject;
            objName = childObj.name;
            if(!objName.Equals("Plane01"))
                childObj.SetActive(false);
        }
    }

    public void ShowAll()
    {
        hidden_ = false;
        if(gameObject_ != null)
            gameObject_.SetActive(true);
        if(nameLabel_ != null && nameLabel_.gameObject != null)
            nameLabel_.gameObject.SetActive(true);
        if (leaderMark_ != null)
            leaderMark_.SetActive(true);
        if (inBattleEff_ != null)
            inBattleEff_.SetActive(true);
    }

    public void HideAll()
    {
        hidden_ = true;
        if (gameObject_ != null)
            gameObject_.SetActive(false);
        if (nameLabel_ != null && nameLabel_.gameObject != null)
            nameLabel_.gameObject.SetActive(false);
        if (leaderMark_ != null)
            leaderMark_.SetActive(false);
        if (inBattleEff_ != null)
            inBattleEff_.SetActive(false);
    }
	public bool IsWishingAvailable()
	{
		return false;	
	}

    public void UpdateTitle()
    {
        if (GamePlayer.Instance.titleHide_ && isPlayer_)
        {
            SetTitleName("");
            return;
        }

        string title = "";
        TitleData tData = isPlayer_? TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title)): TitleData.GetTitleData(playerData_.title_);
        if (tData != null)
            title = tData.desc_;

        SetTitleName(title);
    }

    public void UpdateFollowBaby()
    {
        if (isPlayer_)
        {
            if (TeamSystem.teamIsDirty_)
            {
                TeamSystem.teamIsDirty_ = false;
            }
        }
    }

	public void SetInstName(string name,bool isbabys=false,string color = "")
    {
        if (string.IsNullOrEmpty(name))
            return;
		if(!isbabys)
		{
			if (!isPlayer_ && playerData_ != null)
				playerData_.instName_ = name;
			else
				GamePlayer.Instance.InstName = name;
		}
        

        if(!string.IsNullOrEmpty(color))
            nameColor_ = color;

        if (gameObject_ == null)
            return;

        string[] wholeName = null;
        if (nameLabel_ != null && nameLabel_.gameObject != null)
        {
            wholeName = nameLabel_.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (wholeName.Length < 1)
                return;

            nameLabel_.text = string.Format(nameColor_, name);
            if(wholeName.Length > 1)
                nameLabel_.text += "\n" + wholeName[1];
        }
    }

    public void SetTitleName(string name, string color = "")
    {
        if (!string.IsNullOrEmpty(color))
            nickNameColor_ = color;

        if (gameObject_ == null)
            return;

        titleName = name;
        if (nameLabel_ != null && nameLabel_.gameObject != null)
        {
            string playerName = "";
            if (!string.IsNullOrEmpty(name))
            {
                playerName = string.Format(nickNameColor_, name);
            }
            if(isPlayer_ || playerData_ != null)
                playerName += string.Format(nameColor_, isPlayer_ ? GamePlayer.Instance.InstName : playerData_.instName_);
            else
                playerName += string.Format(nameColor_, babyName_);
            nameLabel_.text = playerName;
        }
        UpdateVipPos();
    }

    public void UpdateVipPos()
    {
        if (!isPlayer_ && playerData_ == null)
            return;
        //更新vip标识位置
        int maxCharacter = isPlayer_ ? GamePlayer.Instance.InstName.Length : playerData_.instName_.Length;
        if (maxCharacter < titleName.Length)
            maxCharacter = titleName.Length;

        int posx = -1 * (maxCharacter - 1) * 16 - 50;
        vipSp_.transform.localPosition = new Vector2(posx, 0f);
    }
}

public class Npc
{
    public int npcId_;

    public TaskNpc taskData_;

    public GameObject gameObject_;

    public string ObjName_;

    bool heightAdjusted;

    public void Init(int npcInst)
    {
        npcId_ = npcInst;
    }

    public void SetObject(GameObject go, float x, float y, float z, Quaternion rotation)
    {
        heightAdjusted = false;
        gameObject_ = go;
        gameObject_.tag = "NPC";
        gameObject_.layer = LayerMask.NameToLayer("NPC");
        gameObject_.transform.position = new Vector3(x, y, z);
        gameObject_.transform.rotation = rotation;
        ObjName_ = gameObject_.name;
        gameObject_.GetComponent<BoxCollider>().isTrigger = true;
        gameObject_.AddComponent<WiseNpc>();
        UpdateEquiptListener uel = gameObject_.GetComponent<UpdateEquiptListener>();
        if (uel != null)
        {
            uel.RemoveHandler();
        }

        taskData_ = go.GetComponent<TaskNpc>();
        if (taskData_ == null) taskData_ = go.AddComponent<TaskNpc>();

        BoxCollider collider = go.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = go.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }

        if (Prebattle.Instance.senseMode_)
        {
            gameObject_.SetActive(false);
            taskData_.HideName();
        }
    }

    public void UpdateHeight()
    {
        if (gameObject_ == null)
            return;

        if (Prebattle.Instance._HeightAdjusted && !heightAdjusted)
        {
            gameObject_.transform.position = new Vector3(gameObject_.transform.position.x, Prebattle.Instance.groundHeight_, gameObject_.transform.position.z);
            heightAdjusted = true;
        }
    }

    /// <summary>
    /// Look at target with direction
    /// </summary>
    /// <param name="target"></param>
    public void LookAtDir(Transform target)
    {
        if (gameObject_ == null)
            return;

        Vector3 position = target.position;
        position.y = gameObject_.transform.position.y;
        gameObject_.transform.LookAt(position);
    }

    public void Dispose()
    {
        if (gameObject_ != null)
            GameObject.Destroy(gameObject_);
        heightAdjusted = false;
    }

    public void ShowAll()
    {
        if(gameObject_ != null)
            gameObject_.SetActive(true);
        if(taskData_ != null)
            taskData_.ShowName();
    }

    public void HideAll()
    {
        if (gameObject_ != null)
            gameObject_.SetActive(false);
        if (taskData_ != null)
            taskData_.HideName();
    }
}