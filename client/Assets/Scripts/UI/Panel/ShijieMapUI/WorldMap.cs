using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMap : UIBase {
	public UILabel _TitleLable;
	public UIButton closeBtn;
	public UIButton FLBtn;
	public UIButton YERBtn;
    public List<UIButton>btns = new List<UIButton>();

	public UIButton npcBtn;
    public UIButton entryBtn;
	public UITexture minmap;
	public UISprite selfSp;
	public Transform Player;
	public float mapWidth=2000;//场景实际宽
	public float mapHeight=2000;//场景实际高
	public float miniMapWidth=256;//小地图宽
	public float miniMapHeight=256;//小地图高

	public GameObject worldObj;
	public GameObject curObj;
	public UIButton worlodBtn;
	public UIButton backBtn;
    public UILabel worldLbl;
    public UILabel backLbl;
	public UITexture SceneName;
	SceneData ssd;
	private static int num = 0;
	private int[] sceneIds = {30,4,5,402};
	private Dictionary<int, SceneData> metaData;
	private List<int> ksys = new List<int> ();
	private List<SceneData> Datas = new List<SceneData> ();
	private List<SceneData> FinishDatas = new List<SceneData> ();
	void Start () {

        worldLbl.text = LanguageManager.instance.GetValue("WordMap");
        backLbl.text = LanguageManager.instance.GetValue("CurMap");
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (FLBtn.gameObject, EnumButtonEvent.OnClick, OnClickDoFL, 0, 0);
		UIManager.SetButtonEventHandler (YERBtn.gameObject, EnumButtonEvent.OnClick, OnClickDoYER, 0, 0);

		UIManager.SetButtonEventHandler (worlodBtn.gameObject, EnumButtonEvent.OnClick, OnClickWorldMap, 0, 0);
		UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, OnClickCurMap, 0, 0);



		npcBtn.gameObject.SetActive (false);
        entryBtn.gameObject.SetActive(false);
		//OpenScene ();

        GuideManager.Instance.RegistGuideAim(worlodBtn.gameObject, GuideAimType.GAT_WorldMapWorldBtn);
        GuideManager.Instance.RegistGuideAim(YERBtn.gameObject, GuideAimType.GAT_WorldMapER);
        GuideManager.Instance.RegistGuideAim(FLBtn.gameObject, GuideAimType.GAT_WorldMapFL);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WorldMapOpen);

	    ssd = SceneData.GetData(GameManager.SceneID);
		HeadIconLoader.Instance.LoadIcon (ssd.minmap, minmap);
		if(num == 0)
		{
			worldObj.SetActive (true);
			curObj .SetActive (false);
			OSceneData ();
            _TitleLable.text = LanguageManager.instance.GetValue("WordMap");
		}else
		{
			worldObj.SetActive (false);
			curObj .SetActive (true);
			LoadCurMapData ();
            _TitleLable.text = LanguageManager.instance.GetValue("CurMap");
		}

	}
	List<UIButton> gos = new List<UIButton>();
	void LoadNpcBtn()
	{
		for (int i = 0; i < Prebattle.Instance.npcContainer_.Count;i++ )
			
		{
			//yicun x-3 y -8
			GameObject npc = null;
			npc = Prebattle.Instance.npcContainer_[i].gameObject_;
            if (npc == null)
                continue;
			
			UIButton sp  = GameObject.Instantiate(npcBtn)as UIButton;
			sp.gameObject.SetActive(true);
			sp.transform.parent = minmap.transform;
			sp.transform.localScale = Vector3.one;
			sp.transform.localPosition = new Vector3(((npc.transform.position.x+ssd.offsetX_) * miniMapWidth / mapWidth)*ssd.dir_,((npc.transform.position.z+ssd.offsetY_) * miniMapHeight / mapHeight)*ssd.dir_,0);
			sp.transform.localPosition = sp.transform.localPosition*ssd.zoom_;
			UIManager.SetButtonEventHandler (sp.gameObject, EnumButtonEvent.OnClick, OnClickMoveNpc, Prebattle.Instance.npcContainer_[i].npcId_, 0);
			UILabel la = sp.GetComponentInChildren<UILabel>();
			la.text = NpcData.GetData( Prebattle.Instance.npcContainer_[i].npcId_).Name;
			gos.Add(sp);
		}
	}

    void LoadEntryBtn()
    {
        SceneData toSceneData = null;
        UIButton sp = null;
        UIEventListener listener = null;
        for (int i = 0; i < ssd.entrys_.Count; i++)
        {
            sp = GameObject.Instantiate(entryBtn) as UIButton;
            sp.gameObject.SetActive(true);
            sp.transform.parent = minmap.transform;
            sp.transform.localScale = Vector3.one;
            sp.transform.localPosition = new Vector3(((ssd.entrys_[i].pos_.x + ssd.offsetX_) * miniMapWidth / mapWidth) * ssd.dir_, ((ssd.entrys_[i].pos_.y + ssd.offsetY_) * miniMapHeight / mapHeight) * ssd.dir_, 0);
            sp.transform.localPosition = sp.transform.localPosition * ssd.zoom_;
            listener = UIEventListener.Get(sp.gameObject);
            listener.parameter = ssd.entrys_[i].pos_;
            listener.onClick += OnClickMoveEntry;
            UILabel la = sp.GetComponentInChildren<UILabel>();
            toSceneData = SceneData.GetData(ssd.entrys_[i].toSceneId_);
            la.text = toSceneData.sceneName_;
        }
    }

	void Update () {
		if( Prebattle.Instance.GetSelf().gameObject_ != null)
		{
			Player = Prebattle.Instance.GetSelf().gameObject_.transform;
			DrawMiniMap ();
		}
//		if(Input.GetMouseButtonDown(0))
//		{
//			Ray ray =UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
//
//			RaycastHit hit;
//			if (Physics.Raycast(ray, out hit, 1000)) 
//			{
//				Debug.LogError("==================="+ hit.point);
//				MoveTo(hit.point);
//			}
//		}
	}
	void MoveTo(Vector3 point)
	{
		point = point / 140f;
		float x = point.x * mapWidth / miniMapWidth - 42.3f;
		float z = point.y * mapHeight / miniMapHeight + 5;

		Vector3 pp =Camera.main.ScreenToWorldPoint (new Vector3 (x,0.9f,z));
		NetConnection.Instance.move (pp.x,pp.z);
	}
	void DrawMiniMap()
	{
		if (Player == null)
			return;
		if (selfSp != null)
		{
//			minmap.transform.localPosition = new Vector3(-((Player.position.x +ssd.offsetX_) * miniMapWidth / mapWidth),-((Player.position.z +ssd.offsetY_) * miniMapHeight / mapHeight),0);
//			minmap.transform.localPosition = minmap.transform.localPosition*140f;
			//shilianshandong shuixia x+1,y+2     jiakeshandong x-15 y 0  xuedi x+5 y +5 zoom 160  wangzhemuxue x-11  y-13 jiazu,x+7 ,y+11 dir -1
			selfSp.transform.localPosition = new Vector3(((Player.position.x+ssd.offsetX_)*ssd.dir_ * miniMapWidth / mapWidth),((Player.position.z+ssd.offsetY_) * miniMapHeight / mapHeight)*ssd.dir_,0);
			selfSp.transform.localPosition = selfSp.transform.localPosition*ssd.zoom_;
		}
	}

	void OnClickWorldMap(ButtonScript obj, object args, int param1, int param2)
	{

		for (int i = 0; i < gos.Count; ++i)
        {
			Destroy(gos[i].gameObject);
			gos.RemoveAt(i);
        }
		worldObj.SetActive (true);
		curObj .SetActive (false);
		OSceneData ();
        _TitleLable.text = LanguageManager.instance.GetValue("WordMap");
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WorldMapToWorld);
        }, 1);
	}
	void OnClickCurMap(ButtonScript obj, object args, int param1, int param2)
	{
		worldObj.SetActive (false);
		curObj .SetActive (true);
		LoadCurMapData ();
        _TitleLable.text = LanguageManager.instance.GetValue("CurMap");
	}
	void LoadCurMapData()
	{
		SceneData ssd = SceneData.GetData(GameManager.SceneID);
		if (ssd != null)
		{
            HeadIconLoader.Instance.LoadIcon(ssd.nameEffectId_, SceneName);
			SceneName.gameObject.SetActive(true);
		}
		else
			SceneName.gameObject.SetActive(false);
		LoadNpcBtn ();
        LoadEntryBtn();
	}

	void OnClickMoveNpc(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.moveToNpc (param1);
	}

    void OnClickMoveEntry(GameObject go)
    {
        UIEventListener listener = UIEventListener.Get(go);
        Vector2 pos = (Vector2)listener.parameter;
        NetConnection.Instance.move(pos.x, pos.y);
    }

	void OSceneData()
	{
		metaData = SceneData.GetData ();

        for (int i = 0; i < btns.Count; ++i)
        {
            UIManager.RemoveButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick);
            UIManager.SetButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick, OnNotOpen, 0, 0);
            btns[i].GetComponentInChildren<UISprite>().color = new Color(0f, 1f, 1f);
        }

		for(int i =0;i<sceneIds.Length;i++)
		{
			if (GamePlayer.Instance.GetSceneAvaliable(sceneIds[i]))
			{
				btns[i].enabled = true;
				btns[i].GetComponentInChildren<UISprite>().color = Color.white;
				UIManager.SetButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick, OnClickDoDuplicate,sceneIds[i], 0);
			}else
			{
				UIManager.RemoveButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick);
				UIManager.SetButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick, OnLimitOpen, GamePlayer.Instance.GetSceneOpenLimit(sceneIds[i]), 0);
				btns[i].GetComponentInChildren<UISprite>().color = new Color(0f, 1f, 1f);
			}
		}
	}
    void OnNotOpen(ButtonScript obj, object args, int param1, int param2)
    {
        PopText.Instance.Show(LanguageManager.instance.GetValue("notOpen"), PopText.WarningType.WT_Warning);
    }

    void OnLimitOpen(ButtonScript obj, object args, int param1, int param2)
    {
        QuestData data = QuestData.GetData(param1);
        MessageBoxUI.ShowMe(string.Format(LanguageManager.instance.GetValue("limitOpen"), LanguageManager.instance.GetValue(data.questKind_.ToString()), data.questName_), null, true);
    }

	public static void ShowMe(int index=0)
	{
		num = index;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__WordMapUI);
	}
	public static void SwithShowMe(int index=0)
	{
		num = index;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__WordMapUI);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS__WordMapUI);
	}
	void OnClickDoDuplicate(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();
					Prebattle.Instance.ActiveEnterScene (param1);
				});
			}
		}else if (ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaijiazuzhan"),()=>{
				
				Prebattle.Instance.ActiveEnterScene (param1);
			});
		}
		else
		{
			Prebattle.Instance.ActiveEnterScene(param1);
		}
		Hide ();
	}
	void OnClickDoFL(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();
					Prebattle.Instance.ActiveEnterScene (1);
				});
			}
		}
		else if (ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaijiazuzhan"),()=>{

				Prebattle.Instance.ActiveEnterScene (1);
			});
		}else 
		{
			Prebattle.Instance.ActiveEnterScene(1);
		}

		Hide ();
	}
	void OnClickDoYER(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();
					Prebattle.Instance.ActiveEnterScene (2);
				});
			}
		}
		else if (ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaijiazuzhan"),()=>{
				
				Prebattle.Instance.ActiveEnterScene (2);
			});
		}else
		{
			Prebattle.Instance.ActiveEnterScene(2);
		}

		Hide ();
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	


	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS__WordMapUI, AssetLoader.EAssetType.ASSET_UI), true);
	}
}
