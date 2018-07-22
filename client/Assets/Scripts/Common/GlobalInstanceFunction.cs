using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;

public class GlobalInstanceFunction
{
	float time_stamp_;

	List<InvokePkg> invokeLst;

	List<ScalePkg> scaleLst;

	GameObject	MainCamera;

    public Texture2D maskTex_;

	static private GlobalInstanceFunction s_Instance = null;
	static public GlobalInstanceFunction Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = new GlobalInstanceFunction();
				s_Instance.Init();
			}
			return s_Instance;
		}
	}

	public void Init()
	{
		invokeLst = new List<InvokePkg>();
		scaleLst = new List<ScalePkg> ();
	}

	public static void LoadSceneUI(string uiName, UIFactory.SceneUICallBack callback)
	{
        if (GlobalValue.isFBScene(uiName)) 
		{
			UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_MainPanel, callback);
		}
        //else if (uiName == GlobalValue.StageName_piantoudonghuaf)
        //{
        //    UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_EmptyPanel, callback);
        //}
        else if (uiName == GlobalValue.StageName_groupScene)
        {
			//TeamUIPanel.SwithShowMe();
            //UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_TeamPanel, callback);
        }
        else if (uiName == GlobalValue.StageName_CreateRoleScene)
        {
            UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_PanelXuan, callback);
		}
        else if (uiName == GlobalValue.StageName_ReLoginScene)
        {
            UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_LoginPanel, callback);
        }
        else if (GlobalValue.isBattleScene(uiName))
        {
            UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_AttackPanel, callback);
        }
		else if(!string.IsNullOrEmpty(uiName))
		{
			//UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_PrebattlePanel, callback);
			UIFactory.Instance.LoadUIPanel(UIASSETS_ID.UIASSETS_MainPanel, callback);
		}
		else
		{
			if(callback != null)
				callback();
		}
	}

    public void NpcOpenUI(UIASSETS_ID id)
    {
        switch (id)
        {
            case UIASSETS_ID.UIASSETS__BabySkillLearning:
                BabySkillLearning.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_ProfessionPanel:
				ProfessionPanel.SwithShowMe();
                //UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ProfessionPanel);
                break;
            case UIASSETS_ID.UIASSETS_ExchangePanel:
                Exchange.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS__LearningUI:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Bar))
                    LearningUI.SwithShowMe();
                break;
		case UIASSETS_ID.UIASSETS_FamilyPanel:
				FamilyPanelUI.SwithShowMe();
				break;
		case UIASSETS_ID.UIASSETS_FamilinfoPanel:
				MyFamilyInfo.SwithShowMe();
				break;
		case UIASSETS_ID.UIASSETS_FamilShopPanel:
				FamilyShopUI.SwithShowMe(true);
				break;
            case UIASSETS_ID.UIASSETS__StoreUI:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Shop))
                    StoreUI.SwithShowMe(2);
                break;
            case UIASSETS_ID.UIASSETS__Arena:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_JJC))
				{
                    ArenaUI.SwithShowMe();
				}
				else
				{
					int level =0;
					GlobalValue.Get(Constant.C_PVPJJCOpenlevel, out level);
					if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < level)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
						return;
					}
				}
                break;
            case UIASSETS_ID.UIASSETS_ArenaPvpPanel:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_PVPJJC))
                {
					bool isOpen = ActivitySystem.Instance.GetInfoState(7) == ActivitySystem.ActivityInfo.ActivityState.AS_Open;
					if(!isOpen)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("jjcmeikai"));
						return;
					}
					
                    if (TeamSystem.IsInTeam())
                    {
                        if (!TeamSystem.IsTeamLeader())
                        {
                            PopText.Instance.Show(LanguageManager.instance.GetValue("teamopen"));
                            return;
						}
						
                        COM_SimplePlayerInst[] team = TeamSystem.GetTeamMembers();
						if(team == null || team.Length <3)
						{
							PopText.Instance.Show(LanguageManager.instance.GetValue("arenapvpnum"));
							return;
						}
                        if (team != null && team.Length > 0)
                        {
                            for (int i = 0; i < team.Length; i++)
                            {
                                if (team[i].isLeavingTeam_)
                                {
                                    PopText.Instance.Show(LanguageManager.instance.GetValue("teamMemberLeavingNoopen"));
                                    return;
                                }
                            }

							int level =0;
							GlobalValue.Get(Constant.C_PVPJJCOpenlevel, out level);
							for (int i = 0; i < team.Length; i++)
							{
								if (team[i].properties_[(int)PropertyType.PT_Level] < level)
								{
									PopText.Instance.Show(LanguageManager.instance.GetValue("duiyuandengji"));
									return;
								}
							}
                        }
					}
					else
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("pvpzudui"));
						return;	
					}
					
				NetConnection.Instance.joinWarriorchoose();	

                 //NetConnection.Instance.requestpvprank();
                 //NetConnection.Instance.requestMyJJCTeamMsg();
				 //ArenaPvpPanelUI.SwithShowMe();

                }
				else
				{
					int level =0;
					GlobalValue.Get(Constant.C_PVPJJCOpenlevel, out level);
					if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < level)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
						return;
					}
				}
                break;
            case UIASSETS_ID.UIASSETS__WordMapUI:
                WorldMap.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_GatherPanel:
               SkillViewUI.SwithShowMe(1);
                break;
            case UIASSETS_ID.UIASSETS_AuctionHousePanel:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_AuctionHouse))
                {
                    if (AuctionHouseSystem.Open_ == false)
                    {
                        PopText.Instance.Show(LanguageManager.instance.GetValue("AuctionHouseClosed"), PopText.WarningType.WT_Warning);
                        return;
                    }
                    AuctionHousePanel.SwithShowMe();
                }
                break;
            case UIASSETS_ID.UIASSETS_HundredUI:
                if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Hundred))
                    HundredUI.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_EmailPanel:
                EmailUI.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_LookchiPanel:
                LookTreeUI.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_PetTemple:
                PetTemple.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_FanilyBank:
                FanilyBankUI.SwithShowMe();
                break;
            case UIASSETS_ID.UIASSETS_FamilyCollection:
                FamilyCollectionUI.SwithShowMe();
                break;
			case UIASSETS_ID.UIASSETS_GuildBattlePanel:
				GuildBattleEnterScene.SwithShowMe();
				break;
		case UIASSETS_ID.UIASSETS_FamilyMonster:
		{
			if(FamilySystem.instance.GuildMember != null && (int) GuildSystem.GetGuildMemberSelf ((int)GamePlayer.Instance.InstId).job_ >=  (int)GuildJob.GJ_VicePremier)
			{
				FamilyMonsterUI.SwithShowMe();
			}
			else
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_CommandPositionLess"));
			}
		}
			break;
		case UIASSETS_ID.UIASSETS_CopyOpenPanel:
			CopyOpenUI.SwithShowMe();
			break;
		case UIASSETS_ID.UIASSETS_EmployeeTask:
				NetConnection.Instance.requestEmployeeQuest ();
			break;
		case UIASSETS_ID.UIASSETS_EquipUIPanel:
			EquipUIPanel.SwithShowMe();
			break;
        }

    }

	public bool	IsValidName( string szName )
	{
		if(null == szName || szName.Equals("") || szName.Equals("0") )
		{
			return false;
		}
		
		return true;
	}

	// 0 ~ (不包括)range 内不重复随机count个整数作为索引值
	// 例:  RandomUniqueIndexInRage(5, 3)  [0~5)中随机3个不重复的数字
	public List<int> RandomUniqueIndexInRange(int range, int count)
	{
		if(count > range || range < 0 || count <= 0)
			return null;

		List<int> l = new List<int> ();
		int val;
		while(count > 0)
		{
			val = Random.Range(0, range);
			if(l.Contains(val) == false)
			{
				l.Add(val);
				count --;
			}
		}
		return l;
	}

    public string Convert2Name(int sceneId)
    {
        SceneData ssd = SceneData.GetData(sceneId);
        if (ssd == null)
        {
            ClientLog.Instance.LogWarning("There is no data with SceneID: " + sceneId);
            return "";
        }
        return ssd.sceneXml_;
    }

	public string GetAssetsName( int AssetsID , AssetLoader.EAssetType Type )
	{
		string AssetsName = "";
		if( Type == AssetLoader.EAssetType.ASSET_EFFECT )
		{
			EffectAssetsData AssetsData = EffectAssetsData.GetData ( AssetsID );
			if( null == AssetsData )
			{
				return "";
			}
			AssetsName = AssetsData.assetsName_;
		}
		else if( Type == AssetLoader.EAssetType.ASSET_PLAYER ||
                 Type == AssetLoader.EAssetType.ASSET_WEAPON)
		{
			EntityAssetsData AssetsData = EntityAssetsData.GetData( AssetsID );
			if( null == AssetsData )
			{
				return "";
			}
			AssetsName = AssetsData.assetsName_;
		}
		else if( Type == AssetLoader.EAssetType.ASSET_UI )
		{
			UIAssetsData AssetsData = UIAssetsData.GetData( AssetsID );
			if( null == AssetsData )
			{
				return "";
			}
			AssetsName = AssetsData.assetsName_;
		}
		else if( Type == AssetLoader.EAssetType.ASSET_SOUND )
		{
			SoundAssetsData AssetsData = SoundAssetsData.GetData( AssetsID );
			if( null == AssetsData )
			{
				return "";
			}
			AssetsName = AssetsData.assetsName_;
		}
		else if( Type == AssetLoader.EAssetType.ASSET_MUSIC )
		{
			MusicAssetsData AssetsData = MusicAssetsData.GetData( AssetsID );
			if( null == AssetsData )
			{
				return "";
			}
			AssetsName = AssetsData.assetsName_;
		}

		return AssetsName;
	}

    public void ReleaseTexture(Transform ui)
    {
        HashSet<Material> matSet = new HashSet<Material>();
        Transform[] ts = ui.GetComponentsInChildren<Transform>();
        UISprite[] uiSprites = ui.GetComponentsInChildren<UISprite>();
        UILabel[] uiLabels = ui.GetComponentsInChildren<UILabel>();
        UIAtlas[] uiAtlas = ui.GetComponentsInChildren<UIAtlas>();

        for (int i = 0; i < uiAtlas.Length; ++i)
        {
            if (uiAtlas[i].spriteMaterial != null && uiAtlas[i].spriteMaterial.mainTexture != null)
            {
                GameObject.DestroyImmediate(uiAtlas[i].spriteMaterial.mainTexture, true);
                uiAtlas[i].spriteMaterial.mainTexture = null;
            }
        }

        for (int i = 0; i < uiSprites.Length; ++i)
        {
            if (uiSprites[i].atlas != null)
            {
                if (uiSprites[i].atlas.name.Equals("CommomAtlas"))
                    continue;

                GameObject.DestroyImmediate(uiSprites[i].atlas.spriteMaterial.mainTexture, true);
                uiSprites[i].atlas.spriteMaterial.mainTexture = null;
            }
        }

        for (int i = 0; i < uiLabels.Length; ++i)
        {
            if (uiLabels[i].bitmapFont == null)//绑定FontManager.font
            {
                continue;
            }
            else if (uiLabels[i].bitmapFont.isDynamic == false)
            {
                GameObject.DestroyImmediate(uiLabels[i].bitmapFont.material.mainTexture, true);
                uiLabels[i].bitmapFont.material.mainTexture = null;
            }
        }
    }

    /// <summary>
    /// 缩放特效
    /// </summary>
    /// <param name="gameObj"></param>
    /// <param name="scale"></param>
    public static void ScaleParticleSystem(GameObject gameObj, float scale)
    {
        var particles = gameObj.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].startSize *= scale;
            particles[i].startSpeed *= scale;
            particles[i].startRotation *= scale;
            particles[i].transform.localScale *= scale;
        }
    }

	public void ShakeCameraPosstion()
	{
		MainCamera = GameObject.Find("Main Camera");
		if( null == MainCamera ) return ;
		iTween.ShakePosition( MainCamera , new Vector3(0.5f,0f,0f) , 0.7f );
	}
	public static Vector2 WorldToUI(Vector3 point)
	{
        if (Camera.main == null)
        {
            ClientLog.Instance.Log("Camera.main   is   null...........");
            return point;
        }
        if (ApplicationEntry.Instance == null)
        {
            ClientLog.Instance.Log("ApplicationEntry.Instance   is   null...........");
            return point;
        }

        Vector2 scnPos = Camera.main.WorldToScreenPoint(point);
        if (ApplicationEntry.Instance.UICamera == null)
        {
            ClientLog.Instance.Log("UICamera.currentCamera   is   null...........");
            return point;
        }
        Vector2 uiPos = ApplicationEntry.Instance.UICamera.ScreenToWorldPoint(scnPos);
        
        uiPos = new Vector2(uiPos.x * ApplicationEntry.Instance.World2UIHeight, uiPos.y * ApplicationEntry.Instance.World2UIHeight);
        return uiPos;
	}

    public static Vector2 ScreenToUI(Vector2 point)
    {
        Vector2 uiPos = ApplicationEntry.Instance.UICamera.ScreenToWorldPoint(point);
        if (ApplicationEntry.Instance == null)
            return point;
        uiPos = new Vector2(uiPos.x * ApplicationEntry.Instance.World2UIHeight, uiPos.y * ApplicationEntry.Instance.World2UIHeight);
        return uiPos;
    }

    public void MakeUIMask(GameObject go)
    {
        BoxCollider mask = go.GetComponent<BoxCollider>();
        if (mask == null)
            mask = go.AddComponent<BoxCollider>();
        mask.isTrigger = true;
        if (ApplicationEntry.Instance == null)
            return;
        mask.size = new Vector2(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight + 100f); //todo 虚拟键盘 动态大小
        if (maskTex_ == null)
        {
            MakeMask();
        }
        GameObject maskTex = new GameObject();
        maskTex.transform.parent = go.transform;
		maskTex.transform.localPosition = Vector3.zero;
        maskTex.layer = go.layer;
        UITexture texture = maskTex.AddComponent<UITexture>();
        texture.mainTexture = maskTex_;
        texture.color = new Color(0f, 0f, 0f, 0.9f);
        texture.width = (int)mask.size.x;
        texture.height = (int)mask.size.y;
        //texture.MakePixelPerfect();
        texture.depth = -1;
		//texture.alpha = 0.7f;
    }

    public void MakeMask()
    {
        if (maskTex_ == null)
        {
            if (ApplicationEntry.Instance == null)
                return;
            maskTex_ = new Texture2D((int)ApplicationEntry.Instance.UIWidth, (int)ApplicationEntry.Instance.UIHeight);
            for (int i = 0; i < maskTex_.width; ++i)
            {
                for (int j = 0; j < maskTex_.height; ++j)
                {
                    maskTex_.SetPixel(i, j, new Color(0f, 0f, 0f, 0.75f));
                }
            }
            maskTex_.Apply(false, true);
        }
    }

	public void setTimeScale( float 	tTime )
	{
		Time.timeScale = tTime;
	}

	public void changeTimeScale(float axis)
	{
		if(axis == 1)
			return;

		Time.timeScale *= axis;
	}

    public int WeaponID(COM_SimplePlayerInst player)
    {
        if (player.equips_.Length <= 0)
            return 0;

        for (int i = 0; i < player.equips_.Length; i++)
        {
            if (player.equips_[i].slot_ == (ushort)EquipmentSlot.ES_SingleHand)
            {
                return (int)player.equips_[i].itemId_;
            }
            else if (player.equips_[i].slot_ == (ushort)EquipmentSlot.ES_DoubleHand)
            {
                return (int)player.equips_[i].itemId_;
            }
        }
        return 0;
    }

    public int GetDressId(COM_Item[] equips)
    {
        if (equips == null)
            return 0;

        for (int i = 0; i < equips.Length; ++i)
        {
            if (equips[i].slot_ == (short)EquipmentSlot.ES_Fashion)
            {
                ItemData dress = ItemData.GetData((int)equips[i].itemId_);
                if (dress != null)
                    return dress.weaponEntityId_;
                else
                    return 0;
            }
        }
        return 0;
    }

    public int DayPass(int timeStamp)
    {
        try
        {
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            System.DateTime gaptime = System.DateTime.MinValue;
            long gapSec = Define.GetTimeStamp() - timeStamp;
            if (gapSec < 0 || timeStamp == 0)
                return 999;
            gaptime = startTime.AddSeconds(gapSec);
            return gaptime.Day;
        }
        catch (System.Exception ex)
        {
            return 999;
        }
    }

    public delegate void DeadHandler();
    public event DeadHandler OnDeadFinish;
	public delegate void UpdatecounDown();
	public event UpdatecounDown UpdatecounDownTime;
	public delegate void UpdateOnlinecounDown(float time);
	public event UpdateOnlinecounDown UpdateOnlineTime;

    int SMSTimer = 120;
    public float SMSCrtTimer = 0f;
    public bool SMSStartCount = false;
    public int GetSMSLeftCD
    {
        get
        {
            return (int)(SMSTimer - SMSCrtTimer);
        }
    }
	public static bool isOnkeyCountDown = false;
	public static int OnkeyendTime;
	private float OnkeycurrentTime;
	private int OnkeystartTime =30;
	private int Onkeysecond;
	public void OnKeyCountdown()
	{
		OnkeycurrentTime += RealDeltaTime;
		OnkeyendTime = OnkeystartTime - Mathf.CeilToInt(OnkeycurrentTime);
	    if( OnkeyendTime <= 0 )
		{
			isOnkeyCountDown = false;
			OnkeyendTime = 0;
			OnkeycurrentTime = 0;
		}		
	}

    static float realPreTime;
    static public float RealDeltaTime
    {
        get
        {
            float delta = Time.realtimeSinceStartup - realPreTime;
            return delta;
        }
    }

	public void Update()
	{
        if (SMSStartCount)
        {
            SMSCrtTimer += RealTime.deltaTime;
            if (SMSCrtTimer > SMSTimer)
            {
                SMSStartCount = false;
                SMSCrtTimer = 0f;
            }
        }
		if(isOnkeyCountDown)
		{
			OnKeyCountdown();
		}
		if(UpdatecounDownTime != null)
		{
			UpdatecounDownTime();
		}
		if(isGuildBattleStart)
		{
			GuildBattleCountdown();
		}
        lock(invokeLst)
        {
            if (openTimer_)
            {
                deadTimer_ += RealTime.deltaTime;
                if (deadTimer_ > DeadMaxTime)
                {
                    ClientLog.Instance.Log("计时器关闭");
                    if (OnDeadFinish != null)
                        OnDeadFinish();
                    deadTimer_ = 0f;
                    openTimer_ = false;
                }
            }

            for (int i = 0; i < invokeLst.Count; ++i)
            {
                invokeLst[i].dead_line_ -= RealTime.deltaTime;
                if (invokeLst[i].dead_line_ <= 0f)
                {
                    invokeLst[i].call_back_();
					if(i > invokeLst.Count - 1)
						continue;
                    if(!invokeLst[i].isRepeat_)
                    {
                        invokeLst.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        invokeLst[i].dead_line_ = invokeLst[i].dead_lineMax_;
                    }
                }

                invokeLst[i].frame_line_ -= 1;
                if (invokeLst[i].frame_line_ <= 0)
                {
                    invokeLst[i].call_back_();
					if(i > invokeLst.Count - 1)
						continue;
                    if (!invokeLst[i].isRepeat_)
                    {
                        invokeLst.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        invokeLst[i].frame_line_ = invokeLst[i].frame_lineMax_;
                    }
                }
            }

            for (int j = 0; j < scaleLst.Count; ++j)
            {
                scaleLst[j].time_crt_ += RealTime.deltaTime;
                scaleLst[j].trans_.localScale = Vector3.Lerp(scaleLst[j].from_, scaleLst[j].to_, scaleLst[j].time_crt_ / scaleLst[j].time_end_);
                if (scaleLst[j].time_crt_ / scaleLst[j].time_end_ > 1f)
                {
                    if (scaleLst[j].call_back_ != null)
                        scaleLst[j].call_back_();
                    scaleLst.RemoveAt(j--);
                }
            }
        }
		if(GamePlayer.Instance.onlineTimeFlag_ || MoreActivityData.onlineStart)
		{
			float tt = GamePlayer.Instance.onlineTime_+= Time.deltaTime;
			MoreActivityData.onlineTime = tt;
			if(UpdateOnlineTime != null)
			{
				UpdateOnlineTime(tt);
			}

			MoreActivityData.instance.UpdateOnlineTime(tt);

		}

        realPreTime = Time.realtimeSinceStartup;
	}
	float currentTime = 0;
	float endTime = 0;
	float preTimeStamp = 0;
	int startTime = 3600;
	public delegate void GuildBattCounDownt(string time);
	public event GuildBattCounDownt GuildBattCounDownTime;
	public static bool isGuildBattleStart = false;
	public string  StateTime;
	public void GuildBattleCountdown()
	{

        //float deltaTime = Time.realtimeSinceStartup - preTimeStamp;
		GuildSystem.guildBatatleStateTime -= RealTime.deltaTime;
        //endTime = startTime - Mathf.CeilToInt(GuildSystem.guildBatatleStateTime );
		//		string fmt = "HH:mm:ss";
		//		Define.FormatUnixTimestamp(ref fmt, (int)endTime);
        //preTimeStamp = Time.realtimeSinceStartup;
        int hour = (int)GuildSystem.guildBatatleStateTime / 3600;
        int min = ((int)GuildSystem.guildBatatleStateTime % 3600) / 60;
        int second = (int)GuildSystem.guildBatatleStateTime % 60;
		string time = DoubleTime(hour) + ":" + DoubleTime(min) + ":" + DoubleTime(second);
		StateTime = time.ToString ();
//		if(GuildBattCounDownTime != null)
//		{
//			GuildBattCounDownTime(time);
//		}
        if (GuildSystem.guildBatatleStateTime <= 0)
		{
			isGuildBattleStart = false;
		}
	}
	
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}
	public void ScaleLerp(Transform trans, float from, float to, float time, ScalePkg.CallBack callback)
	{
		trans.localScale = new Vector3 (from, from, from);
		ScalePkg sp = new ScalePkg ();
		sp.trans_ = trans;
		sp.from_ = new Vector3(from, from, from);
		sp.to_ = new Vector3(to, to, to);
		sp.time_end_ = time;
		sp.time_crt_ = 0f;
		sp.call_back_ = callback;
		scaleLst.Add (sp);
	}

	public void Invoke(InvokePkg.CallBack callback, float time)
	{
        InvokePkg ip = new InvokePkg();
        ip.frame_line_ = int.MaxValue;
        ip.dead_line_ = time;
        ip.dead_lineMax_ = time;
        ip.call_back_ = callback;
        invokeLst.Add(ip);
	}

	public void Invoke(InvokePkg.CallBack callback, int frame)
	{
        lock (invokeLst)
        {
            InvokePkg ip = new InvokePkg();
            ip.frame_line_ = frame;
            ip.frame_lineMax_ = frame;
            ip.dead_line_ = float.MaxValue;
            ip.call_back_ = callback;
            invokeLst.Add(ip);
        }
	}

    public void InvokeRepeat(InvokePkg.CallBack callback, int frame)
    {
        lock (invokeLst)
        {
            InvokePkg ip = new InvokePkg();
            ip.frame_line_ = frame;
            ip.frame_lineMax_ = frame;
            ip.dead_line_ = float.MaxValue;
            ip.call_back_ = callback;
            ip.isRepeat_ = true;
            invokeLst.Add(ip);
        }
    }

    public void InvokeRepeat(InvokePkg.CallBack callback, float time)
    {
        lock (invokeLst)
        {
            InvokePkg ip = new InvokePkg();
            ip.dead_line_ = time;
            ip.dead_lineMax_ = time;
            ip.call_back_ = callback;
            ip.isRepeat_ = true;
            invokeLst.Add(ip);
        }
    }

    public void Clear()
    {
        invokeLst.Clear();
    }

    public void ClearInvokeRepeat()
    {
        for (int i = 0; i < invokeLst.Count; )
        {
            if (invokeLst[i].isRepeat_)
                invokeLst.RemoveAt(i);
            else
                i++;
        }
    }

    public static bool Serialize<T>(T value, ref string serializeXml)
    {
        if (value == null)
        {
            return false;
        }
        try
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter);

            xmlserializer.Serialize(writer, value);

            serializeXml = stringWriter.ToString();

            writer.Close();
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
        }
    }

    //卡死计时器...
    public bool openTimer_ = false;
    public float deadTimer_ = 0f;

    public float DeadMaxTime = 15f;
}

public class ScalePkg
{
	public delegate void CallBack();

	public Transform trans_;
	public Vector3 from_;
	public Vector3 to_;
	public float time_crt_;
	public float time_end_;
	public CallBack call_back_;
}

public class InvokePkg
{
	public delegate void CallBack();
	
	public float dead_line_;
    public float dead_lineMax_;
	public int frame_line_;
    public int frame_lineMax_;
	public CallBack call_back_;
    public bool isRepeat_;
}