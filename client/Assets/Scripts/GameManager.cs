using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager
{
	static private GameManager inst = null;
	static public GameManager Instance
	{
		get
		{
			if(inst == null)
				inst = new GameManager();
			return inst;
		}
	}

    public static int SceneID = 0;
    public static int SceneAreaNum_ = 0;
    public static int ServId_;
    public static string ServName_;
    public static bool serChanged_;
    public bool isKick = false;
	public string _Account;
    public bool SyncLoad = false;
    private int ping_;
    private float lastPing_;
    private float pongTime_;
	//剧情模式
	public bool _IsSenseMode = false;

    public bool procCheckBuff_;
    public bool noNeedCheckBuff_;

	public bool reconnectionLocker_ = false;

    public bool isLeft;

	private int leaderId_;
	private List<COM_SimpleInformation> teamMember_;
    private Dictionary<int, List<NPCInfo>> sceneNpcSet_;

    const float pingGap_ = 5f;

    public int QualityLv;

    public bool IsPad = false;

    int uid_ = 0;
    int GenerateUid
    {
        get { return uid_++; }
    }

    public delegate void NpcDeleteHandler(int sceneId, int npcId);
    public event NpcDeleteHandler OnNpcDelete;

    public delegate void NpcAddHandler(int npc);
    public event NpcAddHandler OnNpcAdd;

    public COM_InitBattle nextBattle_;

    public bool enableDelayCheck_;

    public string mobileNum;

	public GameManager()
	{
        ping_ = 0;
        lastPing_ = Time.realtimeSinceStartup;
        sceneNpcSet_ = new Dictionary<int, List<NPCInfo>>();
		StageMgr.OnSceneLoaded += InitChatUI;
		UIManager.Instance.showMainPanelEnvent += SetChatUIActive;
		//gameHall = ShowGameHallInfo;
	}

    public void JudgeIsPad()
    {
        string devicemodel = SystemInfo.deviceModel.ToLower().Trim();
        IsPad = devicemodel.StartsWith("ipad");
    }

	public  void SetChatUIActive(bool isActive)
	{
		if(chatobj != null)
		{
            if (isActive && (string.IsNullOrEmpty(StageMgr.Scene_name) ||
                StageMgr.Scene_name.Equals("CreateRoleScene") ||
                StageMgr.Scene_name.Equals("ReturnScene") ||
			                 StageMgr.Scene_name.Equals("LoadingScene")) ||
			    StageMgr.Scene_name.Equals("PalaceScene"))
            {

					chatobj.SetActive(false);
               
            }
            else
			{
//				if(isActive)
//				{
//					UIManager.Instance.AdjustUIDepth(chatobj.transform,false);
//				}
				chatobj.SetActive(isActive);
//				UIPanel pa = chatobj.GetComponent<UIPanel> ();
//				pa.depth = 100;
//				de();
			}
                
		}
	}
	public void hidechatobj()
	{
		if(chatobj != null)
		{
			chatobj.SetActive(false);
		}
	}
	public void showchatobj()
	{
		if(chatobj != null)
		{
			if (string.IsNullOrEmpty(StageMgr.Scene_name) ||StageMgr.Scene_name.Equals("CreateRoleScene") ||  StageMgr.Scene_name.Equals("ReturnScene") || StageMgr.Scene_name.Equals("LoadingScene") || StageMgr.Scene_name.Equals("PalaceScene"))
			                 			                
			{
				
				chatobj.SetActive(false);
				
			}else
			{
				//UIManager.Instance.AdjustUIDepth(chatobj.transform,false);
				chatobj.SetActive(true);
//				UIPanel pa = chatobj.GetComponent<UIPanel> ();
//				pa.depth = 100;
//				de();
			}

		}
	}
	void de()
	{
		UIPanel []pa = chatobj.GetComponentsInChildren<UIPanel> (true);
		for(int i =0;i<pa.Length;i++)
		{
			pa[i].depth+=100;
			pa[i].sortingOrder = pa[i].depth;
		}
	}
	string subUiResPath;
	bool hasDestroy = false;
	public static GameObject chatobj;
	public void InitChatUI(string sceneName)
	{

		if (sceneName.Equals("CreateRoleScene") || sceneName.Equals("ReturnScene") || sceneName.Equals("LoadingScene") || StageMgr.Scene_name.Equals("PalaceScene"))
		{
			if(chatobj!=null)
			{
				chatobj.SetActive(false);
			}else
			{
				LoadUI (UIASSETS_ID.UIASSETS_ChatUI,sceneName);
			}

		}else
		{
			if(chatobj!=null)
			{
				chatobj.SetActive(true);
//				UIPanel pa = chatobj.GetComponent<UIPanel> ();
//				pa.depth = 100;
//				de();
			}else
			{
				LoadUI (UIASSETS_ID.UIASSETS_ChatUI,sceneName);
			}

		}



	}
	private void LoadUI(UIASSETS_ID id,string sceneName)
	{
		subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
		
		UIAssetMgr.LoadUI(subUiResPath, (Assets, paramData) =>
		{
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			go.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
			go.transform.localScale = Vector3.one;
			chatobj = go;
		
			if (sceneName.Equals("LoginScene") || sceneName.Equals("CreateRoleScene") || sceneName.Equals("ReturnScene") || sceneName.Equals("LoadingScene")||StageMgr.Scene_name.Equals("PalaceScene"))
			{
				chatobj.SetActive(false);
			}else
			{
				chatobj.SetActive(true);
			}
		}
		, null);
	}
	public delegate void OnUpdatePlayermake();
	public  event OnUpdatePlayermake UpdatePlayermake;
    /// <summary>
    /// 玩家战斗中 cache 属性更改
    /// </summary>
    /// <param name="props"></param>
    List<Pair<uint,List<COM_PropValue> > > _BattlePropCache = new List<Pair<uint,List<COM_PropValue>>>();
    public void SetIProp(uint guid, COM_PropValue[] props)
    {
        if (GamePlayer.Instance.isInBattle){//如果主角在战斗中
            _CacheProp(guid, props);
            return;
        }

		if (GamePlayer.Instance.isMineBaby((int)guid))
		{
			Baby oldBaby = GamePlayer.Instance.GetBabyInst((int)guid);

			if(oldBaby != null)
			{
				for (int i = 0; i < props.Length; ++i)
				{
					int val = 0;
					string fmtStr = "";
					bool needPop1 = true;
					switch(props[i].type_)
					{
					case PropertyType.PT_Attack:
						val = (int)props[i].value_ -oldBaby.GetIprop(PropertyType.PT_Attack);
						fmtStr += "攻击";
						break;
					case PropertyType.PT_Defense:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Defense);
						fmtStr += "防御";
						break;
					case PropertyType.PT_Agile:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Agile);
						fmtStr += "敏捷";
						break;
					case PropertyType.PT_Spirit:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Spirit);
						fmtStr += "精神";
						break;
					case PropertyType.PT_Reply:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Reply);
						fmtStr += "回复";
						break;
					case PropertyType.PT_Magicattack:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Magicattack);
						fmtStr += "魔攻";
						break;
					case PropertyType.PT_Magicdefense:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Magicdefense);
						fmtStr += "魔抗";
						break;
						//case PropertyType.PT_Damage:
						//    val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Damage);
						//    fmtStr += "伤害";
						//    break;
						//case PropertyType.PT_SneakAttack:
						//    val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_SneakAttack);
						//    fmtStr += "偷袭";
						//    break;
					case PropertyType.PT_Crit:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Crit);
						fmtStr += "必杀";
						break;
					case PropertyType.PT_Hit:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Hit);
						fmtStr += "命中";
						break;
					case PropertyType.PT_counterpunch:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_counterpunch);
						fmtStr += "反击";
						break;
					case PropertyType.PT_Dodge:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Dodge);
						fmtStr += "闪躲";
						break;
					case PropertyType.PT_HpMax:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_HpMax);
						fmtStr += "生命上限";
						break;
					case PropertyType.PT_MpMax:
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_MpMax);
						fmtStr += "魔法上限";
						break;
					case PropertyType.PT_Diamond:
						needPop1 = false;
						val = (int)props[i].value_ - oldBaby.GetIprop(PropertyType.PT_Diamond);
						CommonEvent.ExcuteRewardDiamond(val, "ByDefault");
						break;
					default:
						
						break;
					}
					
					if (val == 0)
						continue;
					
					if(needPop1)
						PopText.Instance.Show(string.Format("{0}{1}{2}", oldBaby.InstName +" "+fmtStr, val > 0 ? "+" : "", val), val > 0 ? PopText.WarningType.WT_Tip: PopText.WarningType.WT_Warning);
				}
			}

		}
        Entity e = GamePlayer.Instance.GetUnit(guid);
        if (e != null)
		{
            if (e.InstId == GamePlayer.Instance.InstId)
            {
                for (int i = 0; i < props.Length; ++i)
                {
                    int val = 0;
                    string fmtStr = "";
					bool needPop = true;
                    switch(props[i].type_)
                    {
                        case PropertyType.PT_Attack:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Attack);
                            fmtStr += "攻击";
                            break;
                        case PropertyType.PT_Defense:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Defense);
                            fmtStr += "防御";
                            break;
                        case PropertyType.PT_Agile:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Agile);
                            fmtStr += "敏捷";
                            break;
                        case PropertyType.PT_Spirit:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Spirit);
                            fmtStr += "精神";
                            break;
                        case PropertyType.PT_Reply:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Reply);
                            fmtStr += "回复";
                            break;
                        case PropertyType.PT_Magicattack:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Magicattack);
                            fmtStr += "魔攻";
                            break;
                        case PropertyType.PT_Magicdefense:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Magicdefense);
                            fmtStr += "魔抗";
                            break;
                        //case PropertyType.PT_Damage:
                        //    val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Damage);
                        //    fmtStr += "伤害";
                        //    break;
                        //case PropertyType.PT_SneakAttack:
                        //    val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_SneakAttack);
                        //    fmtStr += "偷袭";
                        //    break;
                        case PropertyType.PT_Crit:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Crit);
                            fmtStr += "必杀";
                            break;
                        case PropertyType.PT_Hit:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Hit);
                            fmtStr += "命中";
                            break;
                        case PropertyType.PT_counterpunch:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_counterpunch);
                            fmtStr += "反击";
                            break;
                        case PropertyType.PT_Dodge:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Dodge);
                            fmtStr += "闪躲";
                            break;
                        case PropertyType.PT_HpMax:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_HpMax);
                            fmtStr += "生命上限";
                            break;
                        case PropertyType.PT_MpMax:
                            val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_MpMax);
                            fmtStr += "魔法上限";
                            break;
						case PropertyType.PT_Diamond:
							needPop = false;
							val = (int)props[i].value_ - GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond);
							CommonEvent.ExcuteRewardDiamond(val, "ByDefault");
							break;
                        default:
                            
                            break;
                    }

                    if (val == 0)
                        continue;

					if(needPop)
                    	PopText.Instance.Show(string.Format("{0}{1}{2}", fmtStr, val > 0 ? "+" : "", val), val > 0 ? PopText.WarningType.WT_Tip: PopText.WarningType.WT_Warning);
                }
            }
			e.SetIprop(props);
			if(UpdatePlayermake != null)
			{
				UpdatePlayermake();
			}
		}
        else
            ClientLog.Instance.LogError("Can not found entity " + guid.ToString());
    }

    public void GotPong()
    {
        pongTime_ = Time.realtimeSinceStartup;
    }

    public void EnableDelayCheck(bool enable)
    {
        enableDelayCheck_ = enable;
        ClearDelay();
    }

    public float GetPingDelay()
    {
        if (!enableDelayCheck_)
            return 0f;
        float lag = pongTime_ - lastPing_;
        if (lag < pingGap_ * -1)
            lag = lag * -1;
        else if(lag < 0f)
        {
            lag = 0f;
        }
        return lag;
    }

    public void ClearDelay()
    {
        pongTime_ = lastPing_;
    }

    public void ResetBattlePropCache()
    {
        for (int i = 0; i < _BattlePropCache.Count; ++i)
        {
            Entity e = GamePlayer.Instance.GetUnit(_BattlePropCache[i].first);
            if (e != null)
                e.SetIprop(_BattlePropCache[i].second.ToArray());
            else
                ClientLog.Instance.LogError("Can not found entity " + _BattlePropCache[i].first.ToString());
        }

        _BattlePropCache.Clear();
    }

    void _CacheProp(uint guid, COM_PropValue[] props)
    {
        for (int i = 0; i < _BattlePropCache.Count; ++i )
        {
            if (_BattlePropCache[i].first == guid)
            {
                for (int j = 0; j < props.Length; ++j )
                { //去掉重复
                    bool added = false;
                    for (int k = 0; k < _BattlePropCache[i].second.Count; ++k )
                    {
                        if (_BattlePropCache[i].second[k].type_ == props[j].type_)
                        {
                            _BattlePropCache[i].second[k].value_ = props[j].value_;
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                        _BattlePropCache[i].second.Add(props[j]);
                }
                return;
            }
        }

        Pair<uint, List<COM_PropValue>> pair = new Pair<uint, List<COM_PropValue>>(guid, new List<COM_PropValue>());
        pair.second.AddRange(props);
        _BattlePropCache.Add(pair);
        
    }

	public SceneData CurrentScene
	{
		get
		{
			return SceneData.GetData(SceneID);
		}
	}

    public bool ParseNavMeshInfo(string info, NpcType type = NpcType.NT_None)
    {
        if (StageMgr.Loading)
            return false;

        if (TeamSystem.IsTeamLeader() || !TeamSystem.IsInTeam() || TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
        {
            if (type != NpcType.NT_None)
            {
                NetConnection.Instance.moveToNpc2(type);
                return true;
            }

            if (string.IsNullOrEmpty(info))
                return false;

            // "|" 为scene和zone分隔符
            // ";" 为scene和npc分隔符
            string[] parse;
            int sceneId;
            if (info.Contains("|"))
            {
                parse = info.Split('|');
                sceneId = int.Parse(parse[0]);
                int zoneId = int.Parse(parse[1]);
                NetConnection.Instance.moveToZone(sceneId, zoneId);
            }
            else
            {
                parse = info.Split(';');
                sceneId = int.Parse(parse[0]);
                int npcId = int.Parse(parse[1]);

                NpcData npcd = NpcData.GetData(npcId);
                if (npcd.Type == NpcType.NT_Caiji1 || npcd.Type == NpcType.NT_Caiji2 || npcd.Type == NpcType.NT_Caiji3)
                {
                    NetConnection.Instance.moveToNpc2(npcd.Type);
                }
                else
                 NetConnection.Instance.moveToNpc(npcId);
                //Prebattle.Instance.selectedNpc_ = npcId;
            }
            return true;
        }
        return false;
    }

	public void Update()
	{
        if (GamePlayer.Instance.hasCreated_)
        {
            if (GamePlayer.Instance.GetIprop(PropertyType.PT_VipLevel) > 0)
            {
                GamePlayer.Instance.Properties[(int)PropertyType.PT_VipTime] -= Time.deltaTime;
                GamePlayer.Instance.UpdateVipTime(GamePlayer.Instance.GetIprop(PropertyType.PT_VipTime));
            }
        }

        /** ping every 10 sec. */
        if (Time.realtimeSinceStartup - lastPing_ > pingGap_)
        {
            lastPing_ = Time.realtimeSinceStartup;
            
            NetConnection.Instance.ping();
        }
       
	}
    
    public class CallBackPack
    {
        public delegate void ActorLoadedCallBack(GameObject actor, ParamData data);
        public ActorLoadedCallBack actorCallBack_ = null;
        public ParamData data_ = null;
        public string bindName_;
        public GameObject actorObj_;
        public GameObject weaponObj_;
        public bool hasWeapon_;
        public bool hasDress_;
        public ENTITY_ID playerAssetId_;
        public ENTITY_ID weaponAssetId_;
        public string layerName_;
        public int uid_;
    }

    public Dictionary<int, CallBackPack> loadedPack_;

    public void  GetActorClone(ENTITY_ID assetId, ENTITY_ID weaponId, EntityType type, CallBackPack.ActorLoadedCallBack callback, ParamData data = null, string layerName = "3D", int dressId = 0)
    {
        CallBackPack pack = new CallBackPack();
        pack.actorCallBack_ = callback;
        pack.data_ = data;
        ENTITY_ID AssId = 0;
        ENTITY_ID WeassId = weaponId;
        if (dressId != 0)
            AssId = (ENTITY_ID)dressId;
        else
            AssId = assetId;
        pack.playerAssetId_ = AssId;
        pack.weaponAssetId_ = WeassId;
        pack.hasDress_ = dressId != 0;
        pack.layerName_ = layerName;
        pack.uid_ = GenerateUid;
        if (loadedPack_ == null)
            loadedPack_ = new Dictionary<int, CallBackPack>();

        loadedPack_.Add(pack.uid_, pack);

        bool ignoreWeaponUpdate = (type != EntityType.ET_Player);
        if (!ignoreWeaponUpdate)
            pack.hasWeapon_ = weaponId != 0;

        if (PlayerAsseMgr.LoadAsset(AssId, ActorAssetLoaded, new ParamData(pack.uid_, ignoreWeaponUpdate)) == false)
            ClientLog.Instance.LogError("EntityAssetID: " + (int)AssId + " has not found!");
    }

    void ActorAssetLoaded(AssetBundle asset, ParamData data)
    {
        CallBackPack pack = loadedPack_[data.iParam];
        pack.actorObj_ = (GameObject)GameObject.Instantiate(asset.mainAsset) as GameObject;
		//PlayerAsseMgr.DeleteAsset(asset, false);
        //不忽略武器更新
        if(!data.bParam)
        {
            UpdateEquiptListener ueListener = pack.actorObj_.AddComponent<UpdateEquiptListener>();
            ueListener.data_ = pack.data_ == null ? null : pack.data_.Clone();
            if (pack.data_ != null && pack.data_.bParam)
                ueListener.data_.iParam = 0;
            if(pack.hasDress_)
                ueListener.dressAssId_ = (int)pack.playerAssetId_;
            ueListener.layerName_ = pack.layerName_;
            ueListener.SetWeapon(pack.weaponObj_, ueListener.data_, pack.layerName_);
            if(!GlobalValue.isBattleScene(StageMgr.Scene_name))
            {
                GamePlayer.Instance.WearEquipEvent += new WearEquipEventHandler(ueListener.UpdateHandler);
                GamePlayer.Instance.DelEquipEvent += new DelEquipEventHandler(ueListener.RemoveWeaponHandler);
            }
        }
        
        Transform shadow = pack.actorObj_.transform.FindChild("Plane01");
        if(shadow != null)
            shadow.gameObject.SetActive(pack.layerName_.Equals("Default"));
        
        if (!pack.hasWeapon_)
        {
            NGUITools.SetLayer(pack.actorObj_, LayerMask.NameToLayer(pack.layerName_));
            NGUITools.SetChildLayer(pack.actorObj_.transform, LayerMask.NameToLayer(pack.layerName_));
            if (pack.actorCallBack_ != null)
            {
                pack.actorCallBack_(pack.actorObj_, pack.data_);
            }
            loadedPack_.Remove(pack.uid_);
        }
        else
        {
            pack.actorObj_.SetActive(false);
            WeaponAssetMgr.LoadAsset(pack.hasDress_? pack.playerAssetId_: (ENTITY_ID)0, pack.weaponAssetId_, WeaponAssetLoaded, new ParamData(pack.uid_));
        }
    }

    void WeaponAssetLoaded(AssetBundle asset, ParamData data)
    {
        CallBackPack pack = loadedPack_[data.iParam];

        if (pack.actorObj_ == null)
            return;

        Transform bindPoint = null;
        if (EntityAssetsData.GetData((int)pack.weaponAssetId_).bindPoint_.Contains("L"))
            bindPoint = pack.actorObj_.GetComponent<WeaponHand>().weaponLeftHand_;
        else
            bindPoint = pack.actorObj_.GetComponent<WeaponHand>().weaponRightHand_;

        pack.weaponObj_ = (GameObject)GameObject.Instantiate(asset.mainAsset, bindPoint.position, bindPoint.rotation) as GameObject;
		WeaponAssetMgr.DeleteAsset(asset, false);
        pack.weaponObj_.transform.parent = bindPoint;
        pack.actorObj_.GetComponent<UpdateEquiptListener>().SetWeapon(pack.weaponObj_, pack.data_, pack.layerName_);

        NGUITools.SetLayer(pack.actorObj_, LayerMask.NameToLayer(pack.layerName_));
        NGUITools.SetChildLayer(pack.actorObj_.transform, LayerMask.NameToLayer(pack.layerName_));
        if (pack.actorCallBack_ != null)
            pack.actorCallBack_(pack.actorObj_, pack.data_);
        loadedPack_.Remove(pack.uid_);
        pack.actorObj_.SetActive(true);
    }

    public class CreateTeamInfo
    {
        public CreateTeamInfo(sbyte number, ushort sceneId, TeamType type, string teamName, string pw)
        {
            teamNumbers_ = number;
            sceneId_ = sceneId;
            tType_ = type;
            teamName_ = teamName;
            passWord_ = pw;
        }
        public sbyte teamNumbers_;
        public ushort sceneId_;
        public TeamType tType_;
        public string teamName_;
        public string passWord_;
    }

    CreateTeamInfo createTeamInfo_;
    public CreateTeamInfo UniqueInfo
    {
        set { createTeamInfo_ = value; }
        get { return createTeamInfo_; }
    }

    public void InitNpcSet(int[] npcList)
    {
        //sceneNpcSet_.Clear();
        //Dictionary<int, SceneSimpleData> wholeScene = SceneSimpleData.GetData();
        //List<NPCInfo> tnpcList;
        //foreach (KeyValuePair<int, SceneSimpleData> pair in wholeScene)
        //{
        //    tnpcList = SceneData.GetData(pair.Value.sceneXml_).npcList_;
        //    if(!sceneNpcSet_.ContainsKey(pair.Key))
        //    {
        //        sceneNpcSet_.Add(pair.Key, new List<NPCInfo>());
        //    }

        //    for (int i = 0; i < tnpcList.Count; ++i)
        //    {
        //        tnpcList[i].sceneId_ = pair.Key;
        //        sceneNpcSet_[pair.Key].Add(tnpcList[i]);
        //        //if (OnNpcAdd != null)
        //        //    OnNpcAdd(tnpcList[i], false);
        //    }
        //}

        //AddSceneNpc(npcList);
    }

    //public void AddSceneNpc(COM_Npc[] addNpcList)
    //{
    //    if (addNpcList == null)
    //        return;

    //    int sceneId = 0;
    //    for (int i = 0; i < addNpcList.Length; ++i)
    //    {
    //        sceneId = addNpcList[i].sceneId_;
    //        if (!sceneNpcSet_.ContainsKey(sceneId))
    //        {
    //            sceneNpcSet_.Add(sceneId, new List<NPCInfo>());
    //        }
    //        if (OnNpcAdd != null)
    //            OnNpcAdd(npc);
    //    }
    //}

    //public void DelSceneNpc(int sceneId, int npcId)
    //{
    //    if (!sceneNpcSet_.ContainsKey(sceneId))
    //        return;

    //    List<NPCInfo> npc = sceneNpcSet_[sceneId];
    //    for (int i = 0; i < npc.Count; ++i)
    //    {
    //        if (npc[i].id_ == npcId)
    //        {
    //            npc.RemoveAt(i);
    //            if (OnNpcDelete != null)
    //                OnNpcDelete(sceneId, npcId);
    //            break;
    //        }
    //    }
    //}

    //public List<NPCInfo> GetSceneNpcList(int sceneId)
    //{
    //    if (!sceneNpcSet_.ContainsKey(sceneId))
    //        return null;

    //    return sceneNpcSet_[sceneId];
    //}

    //public Vector3 GetNpcPos(int sceneId, int npcId)
    //{
    //    if(!sceneNpcSet_.ContainsKey(sceneId))
    //        return Vector3.zero;

    //    for (int i = 0; i < sceneNpcSet_[sceneId].Count; ++i)
    //    {
    //        if (npcId == sceneNpcSet_[sceneId][i].id_)
    //            return sceneNpcSet_[sceneId][i].position_;
    //    }

    //    return Vector3.zero;
    //}

    //public NPCInfo GetNpc(int sceneId, int npcId)
    //{
    //    if (!sceneNpcSet_.ContainsKey(sceneId))
    //        return null;

    //    for (int i = 0; i < sceneNpcSet_[sceneId].Count; ++i)
    //    {
    //        if(sceneNpcSet_[sceneId][i].id_ == npcId)
    //            return sceneNpcSet_[sceneId][i];
    //    }
    //    return null;
    //}

    //6033 - 6062
    //public NPCInfo GetGuaiWuGongChengNpc()
    //{
    //    List<NPCInfo> npcList = null;
    //    foreach(int sceneId in sceneNpcSet_.Keys)
    //    {
    //        npcList = sceneNpcSet_[sceneId];
    //        for (int i = 0; i < npcList.Count; ++i)
    //        {
    //            if (npcList[i].id_ >= 6033 && npcList[i].id_ <= 6062)
    //                return npcList[i];
    //        }
    //    }
    //    return null;
    //}

    public void ReceiveNotice(string content, bool vip)
    {
        NoticeManager.Instance.PushNotice(content, vip);
    }

    public void saveFilters(SceneFilterType[] filters)
    {
        string[] types = new string[filters.Length];
        for (int i = 0; i < filters.Length; ++i)
        {
            types[i] = ((int)filters[i]).ToString();
        }
        string val = string.Join(":", types);
        PlayerPrefs.SetString("xysk_filters", val);
    }

    public SceneFilterType[] loadFilters()
    {
        string val = PlayerPrefs.GetString("xysk_filters");
        if (string.IsNullOrEmpty(val))
            return null;

        string[] types = val.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        SceneFilterType[] filterTypes = new SceneFilterType[types.Length];
        for (int i = 0; i < types.Length; ++i)
        {
            filterTypes[i] = (SceneFilterType)Enum.Parse(typeof(SceneFilterType), types[i]);
        }
        return filterTypes;
    }

	public void GamePlayerInfoReset()
	{
        BagSystem.instance.Clean();
		GamePlayer.Instance.ClearEmplyee();
		EmailSystem.instance.Clear();
		GamePlayer.Instance.isInitStorageBaby = false;
		GamePlayer.Instance.isInitStorageItem = false;
		GamePlayer.Instance.isInBattle = false;
        GameManager.Instance.noNeedCheckBuff_ = false;
        GameManager.Instance.procCheckBuff_ = false;
		CompoundSystem.instance.isInit = false;
		ArenaPvpSystem.Instance.openPvpUI = false;
        Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
		GatherSystem.instance.OpenGatherList.Clear();
		ExamSystem.ClearData ();
	}

	public string GetVersionNum()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		return XyskAndroidAPI.getPackageVersion();
        #elif UNITY_IOS
        return XyskIOSAPI.GetVersion();
        #endif
		return "1.1.1";
	}

    public string PlatformToString()
    {
#if UNITY_ANDROID
		return "Android/";
#elif UNITY_IOS
        return "IOS/";
#endif
        return "PC/";
    }

    public void ClearCurrentState()
    {
        CinemaManager cm = GameObject.FindObjectOfType<CinemaManager>();
        if (cm != null)
            cm.Clear();
        _IsSenseMode = false;
        nextBattle_ = null;
        XInput.Instance.dealInput = true;
        CreatePlayerRole.Reset();
        GlobalInstanceFunction.Instance.Clear();
        Battle.Instance.ResetData();
        TeamSystem._MyTeamInfo = null;
        Prebattle.Instance.Fini();
        Prebattle.Instance.StopAllAutoSystem();
        Prebattle.Instance.nextInfo_ = null;
        Prebattle.Instance.tooFastOper = false;
        StageMgr.ClearStageLoadQue();
        EffectMgr.Instance.DeleteAll();
        GamePlayer.Instance.isInBattle = false;
        GuildSystem.Clear();
		TeamSystem.Clear ();
        SuccessSystem.Clear();
        GatherSystem.instance.Clear();
		GuideManager.Instance.ClearMask();
		UIFactory.Instance.ClearLoadedUI();
        SoundTools.StopMusic();
        RaiseUpSystem.Clear();
		BagSystem.instance.BagClear ();
		PopText.Instance.Clear ();
		GamePlayer.Instance.babies_list_.Clear ();
    }

    public COM_LoginInfo loginInfo_;
}

class RaiseUpSystem
{
    public delegate void UpdateUIHandler();
    public static event UpdateUIHandler OnUpdateRaisePanelUI;

    static public bool[] warningDic = new bool[(int)RaisePanel.RaiseType.RT_Max];

    static RaiseUpSystem inst_ = null;
    static public RaiseUpSystem Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new RaiseUpSystem();
            return inst_;
        }
    }

    public RaiseUpSystem()
    {
        // popRaise
        GamePlayer.Instance.PlayerLevelUpEvent += OnPlayerLevelUp;
        GamePlayer.Instance.BabyLevelUpEvent += OnBabyLevelUp;
        BagSystem.instance.ItemChanged += OnBetterPartnerEquip;

        // hideRaise
        GamePlayer.Instance.OnIPropUpdate += OnPlayerPropUpdate;
        for (int i = 0; i < GamePlayer.Instance.babies_list_.Count; ++i)
        {
            GamePlayer.Instance.babies_list_[i].OnIPropUpdate += OnBabyPropUpdate;
        }

        GamePlayer.Instance.UpdateEmployeeEnvent += OnUpdateBattle;
        EmployessSystem.instance.employeeRedEnvent += OnGainEmployee;

        MainbabyListUI.RefreshBabyListOk += delegate(int instid) {OnBabyPropUpdate();};
    }

    public bool HasItem
    {
        get
        {
            for (int i = 0; i < warningDic.Length; ++i)
            {
                if(warningDic[i])
                    return true;
            }
            return false;
        }
    }

    public void EnterGame()
    {
        // 装备打造
        if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) % 10 == 0)
        {
            if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Make))
            {
                warningDic[(int)RaisePanel.RaiseType.RT_RaiseEquip] = true;
            }
        }

        // 有剩余点
        if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) > 0)
            warningDic[(int)RaisePanel.RaiseType.RT_RaisePlayer] = true;

        // 出战宝宝有剩余点
        if (GamePlayer.Instance.BattleBaby != null && GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Free) > 0)
        {
            if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
                warningDic[(int)RaisePanel.RaiseType.RT_RaiseBaby] = true;
        }

        // 出战宝宝等级可以提升技能
        if (GamePlayer.Instance.BattleBaby != null)
        {
            if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
            {
                SkillData data = null;
                int lv = GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level);
                if (lv >= 20 && lv < 40)
                {
                    int skillId = 0;
                    int skillLv = 0;
                    for (int i = 0; i < GamePlayer.Instance.BattleBaby.SkillInsts.Count; ++i)
                    {
                        skillId = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillID_;
                        skillLv = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillLevel_;
                        data = SkillData.GetData(skillId, skillLv);
                        if (data._IsPhysic)
                        {
                            if (skillLv == 2)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                        else
                        {
                            if (skillLv == 3)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                    }
                }
                else if (lv >= 40 && lv < 60)
                {
                    int skillId = 0;
                    int skillLv = 0;
                    for (int i = 0; i < GamePlayer.Instance.BattleBaby.SkillInsts.Count; ++i)
                    {
                        skillId = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillID_;
                        skillLv = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillLevel_;
                        data = SkillData.GetData(skillId, skillLv);
                        if (data._IsPhysic)
                        {
                            if (skillLv == 3)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                        else
                        {
                            if (skillLv == 5)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                    }
                }
                else if (lv >= 60)
                {
                    int skillId = 0;
                    int skillLv = 0;
                    for (int i = 0; i < GamePlayer.Instance.BattleBaby.SkillInsts.Count; ++i)
                    {
                        skillId = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillID_;
                        skillLv = (int)GamePlayer.Instance.BattleBaby.SkillInsts[i].skillLevel_;
                        data = SkillData.GetData(skillId, skillLv);
                        if (data._IsPhysic)
                        {
                            if (skillLv == 4)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                        else
                        {
                            if (skillLv == 7)
                                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;
                        }
                    }
                }
            }
        }

        // 佣兵有可上阵位置
		if (GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
		{
			if(GamePlayer.Instance.EmployeeList.Count>0 && EmployessSystem.instance.GetBattleEmpty())
            	warningDic[(int)RaisePanel.RaiseType.RT_PartnerBattle] = true;
		}
        if(OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    void OnPlayerLevelUp(int level)
    {
        if (level % 10 == 0)
        {
            if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Make))
            {
                warningDic[(int)RaisePanel.RaiseType.RT_RaiseEquip] = true;
            }
        }
		if(level > 20)
        	warningDic[(int)RaisePanel.RaiseType.RT_RaisePlayer] = true;
        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    public static void OnPartnerCollect()
    {
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
			return;
		if (warningDic[(int)RaisePanel.RaiseType.RT_PartnerCollect])
            return;
        warningDic[(int)RaisePanel.RaiseType.RT_PartnerCollect] = true;

        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    void OnBetterPartnerEquip(COM_Item item)
    {
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
			return;
        ItemData data = ItemData.GetData((int)item.itemId_);
        if (data.mainType_ != ItemMainType.IMT_EmployeeEquip)
            return;
        List<Employee> emp = GamePlayer.Instance.GetBattleEmployees();
		//
		for (int i = 0; i < emp.Count; ++i)
		{
			EmployeeConfigData employeeConfig = EmployeeConfigData.GetData (emp[i].GetIprop(PropertyType.PT_TableId),(int)emp[i].star_-1);
			for(int j=0;j<employeeConfig.items.Count;j++ )
			{
				if(employeeConfig.items[j] == (int)item.itemId_)
				{
						warningDic[(int)RaisePanel.RaiseType.RT_PartnerEquip] = true;
					if (OnUpdateRaisePanelUI != null)
						OnUpdateRaisePanelUI();
					break;
				}
			}
		}
    }

    void OnBabyLevelUp(int[] info)
    {
        if (!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
            return;

        if (GamePlayer.Instance.BattleBaby != null)
        {
            warningDic[(int)RaisePanel.RaiseType.RT_RaiseBaby] = true;

            if(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level) == 20 ||
                GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level) == 40 ||
                GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level) == 60)
                warningDic[(int)RaisePanel.RaiseType.RT_RaiseSkill] = true;

            if (OnUpdateRaisePanelUI != null)
                OnUpdateRaisePanelUI();
        }
    }

    

    /// <summary>
    /// hide
    /// </summary>
    /// <param name="type"></param>
    void OnPlayerPropUpdate()
    {
        if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) == 0)
            warningDic[(int)RaisePanel.RaiseType.RT_RaisePlayer] = false;
        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    void OnBabyPropUpdate()
    {
        if (!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
            return;

        bool showTip = false;
        for (int i = 0; i < GamePlayer.Instance.babies_list_.Count; ++i)
        {
            if (GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_Free) > 0)
            {
                showTip = true;
                break;
            }
        }
        warningDic[(int)RaisePanel.RaiseType.RT_RaiseBaby] = showTip;
        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    void OnUpdateBattle(Employee inst,int grop)
    {
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
			return;
        if (!EmployessSystem.instance.GetBattleEmpty ())
            warningDic[(int)RaisePanel.RaiseType.RT_PartnerBattle] = false;
		else
			warningDic[(int)RaisePanel.RaiseType.RT_PartnerBattle] = true;
        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    void OnGainEmployee(int id)
    {
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
			return;
        warningDic[(int)RaisePanel.RaiseType.RT_PartnerRaise] = id != -1;
        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    public static void ResetRaise(RaisePanel.RaiseType type)
    {
        if (warningDic[(int)type] == false)
            return;

        warningDic[(int)type] = false;

        if (OnUpdateRaisePanelUI != null)
            OnUpdateRaisePanelUI();
    }

    public static void Clear()
    {
        if (warningDic != null)
        {
            for (int i = 0; i < warningDic.Length; ++i)
            {
                warningDic[i] = false;
            }
        }
    }
}