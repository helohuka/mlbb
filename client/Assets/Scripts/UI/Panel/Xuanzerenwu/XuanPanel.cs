using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class XuanPanel : UIBase {

	public UILabel _PlayerNameLable;
	public UILabel tipsLable;

	public GameObject RolePanelObj;
	public GameObject createNameObj;
	public UIButton backBtn;
	public UIButton createP;
	public GameObject []iconBtns;
	public GameObject playerInfoObg;
	public  delegate void SetBtnRoles(bool state);
	public  static SetBtnRoles  SetRoleinfo;
	public bool isCreateB;
	private Animator shuAni;
	private int maxCount;
	private int curCount;
	private List<COM_SimpleInformation> roles;
	private List<GameObject> rolesobj = new List<GameObject>();
	private CreatePlayerRole cRole;
	private int posIndex=0;
	private List<int>rolesId = new List<int>();
	private UIEventListener Listener ;
	private Vector3 defPosition;
	public delegate void SetrolesPanleInfo(int insID);
	public static SetrolesPanleInfo SetPlayerPanleInfo;
	public static XuanPanel XuanPanelInstance;
	StartGame stGame;
	CreatePlayerRole crole;
	void Awake()
	{
		XuanPanelInstance = this;
	}
	public static XuanPanel Instance{
		get{
			return XuanPanelInstance;	
		}
	}

	void Start () {
		tipsLable.text = LanguageManager.instance.GetValue ("tishixuanren");
		_PlayerNameLable.text = LanguageManager.instance.GetValue ("Player_name");
		stGame = createNameObj.GetComponent<StartGame>();
		crole = Camera.main.GetComponent<CreatePlayerRole> ();
		RolePanelObj.SetActive (false);
		GameObject shu = GameObject.FindGameObjectWithTag("shu");
		cRole = Camera.main.GetComponent<CreatePlayerRole> ();
		backBtn.gameObject.SetActive (false);
		SetinfoBtnState (false);
		InitPlayerIconBtn ();
		InitAlreadyHadPlayer ();
		SetIconShowState (false);
		SetRoleinfo = SetinfoBtnState;
		createP.gameObject.SetActive (false);
		UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, OnClickbackBtn, 0, 0);
		UIManager.SetButtonEventHandler (createP.gameObject, EnumButtonEvent.OnClick, OnClickcreateP, 0, 0);
		SetPlayerPanleInfo = SetrolesPanle;
	}
	
	private void OnClickbackBtn(ButtonScript obj, object args, int param1, int param2)
	{
		RolePanelObj.SetActive (false);
		CreatePlayerRole.ani.SetInteger("cameraState",2);
		backBtn.gameObject.SetActive(false);
		CreatePlayerRole.isCreate = false;
		SetIconShowState (false);
		//createP.gameObject.SetActive(true);
		CreatePlayerRole.ani.enabled = true;
		SetinfoBtnState(false);
		cRole.HidePlayerObj();
		SetPlayerObjActive (true);
	
	}
	private void OnClickcreateP(ButtonScript obj, object args, int param1, int param2)
	{
		RolePanelObj.SetActive (false);
		CreatePlayerRole.ani.SetInteger("cameraState",1);
		backBtn.gameObject.SetActive(true);
		createP.gameObject.SetActive(false);
		SetinfoBtnState (false);
		isCreateB = true;
		CreatePlayerRole.isCreate = true;
		SetIconShowState (true);
		playerInfoObg.SetActive(false);
		cRole.ShowPlayerObj();
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Butterfly,transform);
		SetPlayerObjActive (false);
	}

	void Update () {
		if (!isCreateB) {
			AnimatorStateInfo info = CreatePlayerRole.ani.GetCurrentAnimatorStateInfo (0);
			if (info.normalizedTime > 1) {
				SetCreateBtnPos();
			}	
		}else
		{
			defPosition = CreatePlayerRole.mainCamera.transform.position;
		}
	}
    public void SetIconShowState(bool isShow)
	{
		for (int i = 0; i<iconBtns.Length; i++) {
			if(i<CreatePlayerRole.playDatas.Count)
			{
				iconBtns[i].gameObject.SetActive(isShow);
			}else
			{
				iconBtns[i].gameObject.SetActive(false);
			}

		}
		backBtn.gameObject.SetActive (isShow);
	}
	void InitAlreadyHadPlayer()
	{
		roles = CreatePlayerRole.GetRoles ();

		maxCount = roles.Count;
		for(int i = 0;i<roles.Count;i++)
		{
			COM_SimpleInformation role = roles[i];
			rolesId.Add(role.instId_);
			CreatePlayerObj(role);
		}
		posIndex+=roles.Count;
		if(posIndex<cRole.playerPos.Length)
		{
			SetCreateBtnPos();
		}

	}

	void InitPlayerIconBtn()
	{
		for (int bCount = 0; bCount< iconBtns.Length; bCount++) {

			if(bCount<CreatePlayerRole.playDatas.Count)
			{
				UITexture [] sps = iconBtns[bCount].GetComponentsInChildren<UITexture>(true);
				foreach(UITexture sp in sps)
				{
					if(sp.gameObject.name.Equals("ICON"))
					{

						HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(CreatePlayerRole.playDatas[bCount].lookID_).assetsIocn_,sp);
					}
				}
				Listener = UIEventListener.Get(iconBtns[bCount]);
				Listener.onClick +=ShowPlayer;
				Listener.parameter = new int []{ bCount,CreatePlayerRole.playDatas[bCount].id_};
			}else
			{
				iconBtns[bCount].gameObject.SetActive(false);
			}


		}
	}
	void ShowPlayer(GameObject sender)
	{
		int []  ids = (int[]) UIEventListener.Get (sender).parameter;
		int index = ids [0];
		int rId = ids [1];
		CreatePlayerRole.roleId = rId;
		SetIconShowState(false);
		stGame.ShowPlayerDes (rId);
		crole.DisplaySelectedRole (rId);
		CreatePlayerRole.ani.enabled = false;
		Hashtable has = new Hashtable();
		has.Add("speed",4f);
		has.Add("time",1f);
		has.Add("onstarttarget", cRole.CameraPositions[index].gameObject);
		has.Add("position",cRole.CameraPositions[rId-1].position);
		iTween.MoveTo(CreatePlayerRole.mainCamera.transform.gameObject,has);
		SetinfoBtnState(true);
	}
	public void SetrolesPanle(int insID)
	{
		for(int i=0;i<gos.Count;i++)
		{
            if (gos[i] == null)
                continue;

			if(int.Parse(gos[i].name) == insID)
			{
				RolePanelObj.SetActive(true);
				RolePanelObj.transform.localPosition = GlobalInstanceFunction.WorldToUI(gos[i].transform.parent.position);
				RolePanelObj.transform.localScale = Vector3.one;
				//RolePanelObj.transform.localPosition = new Vector3(RolePanelObj.transform.position.x,RolePanelObj.transform.position.y-160,0);
			}
		}
		SetPlayerInfo (insID);
	}
	private void SetPlayerInfo(int insID)
	{
		for (int i=0; i<roles.Count; i++)
		{
			if(insID== roles [i].instId_)
			{

				SelectedRoleStartGame rpo = RolePanelObj.GetComponent<SelectedRoleStartGame>();
				rpo.SimpleInformation = roles [i];
//				rpo.nameLabel.text =  roles [i].instName_;
//				rpo.levelLbael.text = roles [i].level_.ToString();
//				rpo.OccupationLabel.text = Profession.get (roles [i].jt_, roles [i].jl_).jobName_;
			}
		}
	}
	public void SetinfoBtnState(bool state)
	{
			createNameObj.SetActive(state);
	}
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	void SetCreateBtnPos()
	{
		if(createP == null)return;
		if (posIndex < cRole.playerPos.Length) {

            createP.transform.parent = gameObject.transform;
            createP.transform.localPosition = GlobalInstanceFunction.WorldToUI(cRole.playerPos[posIndex].position);
            createP.transform.localScale = Vector3.one;
			createP.gameObject.SetActive(true);
		}

	}
	void CreatePlayerObj(COM_SimpleInformation mRole)
	{
        ENTITY_ID weaponAssetId = 0;
        if (mRole.weaponItemId_ != 0)
            weaponAssetId = (ENTITY_ID)ItemData.GetData(mRole.weaponItemId_).weaponEntityId_;
        int dressId = 0;
        ItemData dress = ItemData.GetData(mRole.fashionId_);
        if (dress != null)
            dressId = dress.weaponEntityId_;
        GameManager.Instance.GetActorClone((ENTITY_ID)mRole.asset_id_, weaponAssetId, EntityType.ET_Player, AssetLoadCallBack, new ParamData(mRole.instId_), "Default", dressId);
	}
	List<GameObject> gos = new List<GameObject>();

	public void DelPlayer(string name)
	{
		for(int i = 0;i<roles.Count;i++)
		{
			if(roles[i].instName_ == name)
			{
				if(int.Parse(gos[i].name) == roles[i].instId_)
				{
					Destroy(gos[i]);
					gos.RemoveAt(i);
                    rolesId.RemoveAt(i);
					roles.RemoveAt(i);
                    break;
				}
			}

		}
		RolePanelObj.SetActive (false);
		SetPlayerPos ();
		posIndex--;
		SetCreateBtnPos ();
	}

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{

		ro.name = data.iParam.ToString ();
		rolesobj.Add (ro);
		curCount++;
		if (curCount == maxCount) {
			for(int i =0;i<CreatePlayerRole.GetRoles().Count;i++)
			{
				for(int j=0;j<rolesobj.Count;j++)
				{
					if(CreatePlayerRole.GetRoles()[i].instId_ == int.Parse(rolesobj[j].name))
					{
						gos.Add(rolesobj[j]);
					}
				}
			}
			SetPlayerPos();
		}
	}
	void SetPlayerObjActive(bool isActive)
	{
		for(int i = 0;i<gos.Count;i++)
		{
			gos[i].SetActive(isActive);
		}
	}
	void SetPlayerPos()
	{

		for (int i = 0; i<gos.Count; i++) {
			gos[i].transform.parent = 	cRole.playerPos[i];
			gos[i].transform.localPosition = Vector3.zero;
			gos[i].transform.localScale = new Vector3(4,4,4);
			gos[i].transform.gameObject.name=rolesId[i].ToString(); 
		}
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad (UIASSETS_ID.UIASSETS_PanelXuan);			
	}


}
