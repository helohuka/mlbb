using UnityEngine;
using System.Collections;

using UnityEngine;

using System.Collections;

public class minmapUI : MonoBehaviour {
	
	public UITexture backGround;//小地图背景
	
	public UISprite playerMiniLogo;//玩家标记(可旋转)
	
	public UISprite NpcMiniLogo;//NPC标记 如建筑

    public UISprite EntryMiniLogo;//NPC标记 如建筑
	
	public UISprite DirectionArrow;
	
	public Transform Player;//玩家所在位置
	
	public float arrowAngle=0;
	
	//real map size(3d world units)
	
	public float mapWidth=2000;//场景实际宽
	
	public float mapHeight=2000;//场景实际高
	
	//minimap size(texture)
	
	public float miniMapWidth=256;//小地图宽
	
	public float miniMapHeight=256;//小地图高
	
	//
	public GameObject Showbtn;
	private float backAlpha=0.9f;//背景透明度
	
	public string NpcTags="NPC";
	
	private GameObject[] DrawNpcs;
	
	public UITexture sceneName;
    string crtSceneName;
    string crtMap;

	SceneData ssd;
	void Start () {
		Prebattle.OnAssetsLoadNPCFinish += SetMinmapNPC;
		UIManager.SetButtonEventHandler (Showbtn, EnumButtonEvent.OnClick, OnClickshowMap, 0, 0);
        GuideManager.Instance.RegistGuideAim(Showbtn.gameObject, GuideAimType.GAT_MiniMap);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MiniMapOpen);
	}
	private void OnClickshowMap(ButtonScript obj, object args, int param1, int param2)
	{
		WorldMap.SwithShowMe (1);
	}

	void SetMinmapNPC()
	{
		ssd = SceneData.GetData(GameManager.SceneID);
        if (ssd != null)
        {
            if (string.IsNullOrEmpty(crtSceneName) || !crtSceneName.Equals(ssd.nameEffectId_))
            {
                ClearName();
                crtSceneName = ssd.nameEffectId_;
                HeadIconLoader.Instance.LoadIcon(crtSceneName, sceneName);
            }

            if (string.IsNullOrEmpty(crtMap) || !crtMap.Equals(ssd.minmap))
            {
                ClearMap();
                crtMap = ssd.minmap;
                HeadIconLoader.Instance.LoadIcon(crtMap, backGround);
            }

            sceneName.gameObject.SetActive(true);
        }
        else
        {
            sceneName.gameObject.SetActive(false);
            ClearName();
            ClearMap();
        }

		NpcMiniLogo.gameObject.SetActive (false);
        for (int i = 0; i < Prebattle.Instance.npcContainer_.Count; i++)
        {

            GameObject npc = Prebattle.Instance.npcContainer_[i].gameObject_;

            UISprite sp = GameObject.Instantiate(NpcMiniLogo) as UISprite;
            sp.gameObject.SetActive(true);
            sp.transform.parent = backGround.transform;
            sp.transform.localScale = Vector3.one;
            sp.transform.localPosition = new Vector3(((npc.transform.position.x + ssd.offsetX_) * miniMapWidth / mapWidth) * ssd.dir_, ((npc.transform.position.z + ssd.offsetY_) * miniMapHeight / mapHeight) * ssd.dir_, 0);
            sp.transform.localPosition = sp.transform.localPosition * ssd.zoom_;
        }

        EntryMiniLogo.gameObject.SetActive(false);
        for (int i = 0; i < ssd.entrys_.Count; i++)
        {
            UISprite sp = GameObject.Instantiate(EntryMiniLogo) as UISprite;
            sp.gameObject.SetActive(true);
            sp.transform.parent = backGround.transform;
            sp.transform.localScale = Vector3.one;
            sp.transform.localPosition = new Vector3(((ssd.entrys_[i].pos_.x + ssd.offsetX_) * miniMapWidth / mapWidth) * ssd.dir_, ((ssd.entrys_[i].pos_.y + ssd.offsetY_) * miniMapHeight / mapHeight) * ssd.dir_, 0);
            sp.transform.localPosition = sp.transform.localPosition * ssd.zoom_;
        }
	}
	// Update is called once per frame
	
	void Update () {
		if( Prebattle.Instance.GetSelf().gameObject_ != null)
		{
			Player = Prebattle.Instance.GetSelf().gameObject_.transform;
			DrawMiniMap (0, 0, 0);
		}

	}
	void DrawMiniMap(float LeftX,float LeftY,int PointSize)
		
	{
		if (Player == null)
			return;
		if (ssd == null)
			return;

		if (DirectionArrow != null)			
		{//zhucheng x +42.3 y-5  yicun x -16 y -50  erncun x -1 y-3 xuedi x+2 y+2 shamo x+4 y -41 migong1 and migong2 x -3,y +6

			backGround.transform.localPosition = new Vector3(-((Player.position.x+ssd.offsetX_) * miniMapWidth / mapWidth)*ssd.dir_,-((Player.position.z+ssd.offsetY_) * miniMapHeight / mapHeight)*ssd.dir_,0);
			backGround.transform.localPosition = backGround.transform.localPosition*ssd.zoom_;			
		}

	}
	void OnDestroy()
	{
		Prebattle.OnAssetsLoadNPCFinish -= SetMinmapNPC;
        ClearName();
        ClearMap();
	}

    void ClearName()
    {
        if (!string.IsNullOrEmpty(crtSceneName))
            HeadIconLoader.Instance.Delete(crtSceneName);
        crtSceneName = "";
    }

    void ClearMap()
    {
        if (!string.IsNullOrEmpty(crtMap))
            HeadIconLoader.Instance.Delete(crtMap);
        crtMap = "";
    }
}