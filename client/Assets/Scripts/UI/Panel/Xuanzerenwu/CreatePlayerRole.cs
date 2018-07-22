using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePlayerRole : MonoBehaviour {

	public static int roleId;
	public Transform[] playerBGPos;
	public Transform[] playerPos;
	public static List<PlayerData> playDatas = new List<PlayerData> ();
	private GameObject currentObj;
	private List<int>roleKeys = new List<int>();
	private int maxCount;
	private int curCount;
	private Dictionary<int, PlayerData> metaDatas = new Dictionary<int, PlayerData> ();
	private List<GameObject> mods = new List<GameObject> ();
	private static List< COM_SimpleInformation> mRoles;
	public static Animator ani;
	public  Transform []CameraPositions;
	public static Camera mainCamera;
	public Transform tran;
	void Start()
	{
		mainCamera = Camera.main;
		metaDatas = PlayerData.GetMetaData ();
		maxCount = metaDatas.Count;
		ani = Camera.main.GetComponent<Animator> ();
		foreach(int sid in metaDatas.Keys)
		{
			PlayerData pData = PlayerData.GetData(sid);
			roleKeys.Add(sid);
			playDatas.Add(pData);
			CreateAlternativePlayerObj(pData , sid );
		}
		HidePlayerObj ();
        StageMgr.SceneLoadedFinish();
	}
	void CreateAlternativePlayerObj(PlayerData pData , int sid )
	{		
        GameManager.Instance.GetActorClone((ENTITY_ID)pData.lookID_, (ENTITY_ID)0, EntityType.ET_Player, AssetLoadCallBack, new ParamData(sid), "Default");
	}	

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		ro.SetActive (false);
		ro.name = data.iParam.ToString();
		mods.Add (ro);
		curCount++;
		if (curCount == maxCount) 
		{
			SetGPpos ();
		}
	}

	void SetGPpos()
	{
		for (int i=0; i<mods.Count; i++) 
		{
			mods[i].transform.parent = 	playerBGPos[int.Parse(mods[i].name) - 1];
			mods[i].transform.localPosition = Vector3.zero;
			mods[i].transform.localScale = new Vector3(3,3,3);
//			mods[i].transform.gameObject.name=roleKeys[i].ToString();
		}
	}

    public	void HidePlayerObj()
	{
		for (int i = 0; i<mods.Count; i++) {
			mods[i].SetActive(false);
		}
	}
	public void ShowPlayerObj()
	{
		for (int i = 0; i<mods.Count; i++) {
			mods[i].SetActive(true);
            mods[i].GetComponent<Animator>().SetInteger("state", 1);
		}
	}

	public static List< COM_SimpleInformation> GetRoles() 
	{
		return mRoles;
	}
	static List<COM_SimpleInformation> RolesList = new List<COM_SimpleInformation> ();
	public static void SetRoles(COM_SimpleInformation[] Roles) 
	{
		RolesList.Clear ();
		RolesList.AddRange (Roles);
//		COM_SimpleInformation sif = new COM_SimpleInformation ();
//		sif.asset_id_ = 1;
//		sif.instId_ = 1;
//		sif.level_ = 1;
//		sif.instName_ = "stest";
//		Roles = new COM_SimpleInformation[]{
//			sif
//		};

		mRoles = RolesList;
	}

	public static bool isCreate = false;
	private bool isMove;
	private Vector3 CurrentPos;
	private Vector3 OldPos;
	private GameObject defObj;
	int count;
	public	void DisplaySelectedRole(int rid)
	{
		for(int i = 0;i<mods.Count;i++)
		{
			if(int.Parse(mods[i].name) == rid)
			{
                if (!mods[i].activeSelf)
				    mods[i].SetActive(true);
			}
            else
			{
                if(mods[i].activeSelf)
				    mods[i].SetActive(false);
			}
		}
	}
	public void RecoveryRoleDisplay()
	{
		for(int i = 0;i<mods.Count;i++)
		{
				mods[i].SetActive(true);
			
		}
	}
	void Update()
	{
		if (UICamera.hoveredObject == null)
		{
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray , out hit))
				{
					if(hit.transform.tag != "shu")
					{
						currentObj = hit.transform.gameObject;
						roleId = int.Parse(currentObj.name);					

					    
						if(isCreate)
						{
							if(XuanPanel.SetRoleinfo !=null)
							{
								XuanPanel.SetRoleinfo(true);
							}
							if(XuanPanel.Instance != null)
							{
								XuanPanel.Instance.createNameObj.GetComponent<StartGame>().ShowPlayerDes(roleId);
								XuanPanel.Instance.SetIconShowState(false);
							}
							DisplaySelectedRole(roleId);
							ani.enabled = false;
							Hashtable has = new Hashtable();
							has.Add("speed",4f);
							has.Add("time",1f);
							//has.Add("looktarget",currentObj.transform.position);
							has.Add("onstarttarget", CameraPositions[roleId-1].gameObject);
							has.Add("position",CameraPositions[roleId-1].position);
							iTween.MoveTo(Camera.main.gameObject,has);

							//iTween.RotateTo(Camera.main.gameObject,has);
						}else
						{
							if(XuanPanel.SetRoleinfo !=null)
							{
								XuanPanel.SetRoleinfo(false);
							}

							if(XuanPanel.SetPlayerPanleInfo !=null)
							{
								XuanPanel.SetPlayerPanleInfo(roleId);
							}


						}
						
					}
				}
			}	
		}


	}
	public void CamerMoveBack()
	{
		Hashtable has = new Hashtable();
		has.Add("speed",4f);
		has.Add("time",1f);
		//has.Add("looktarget",currentObj.transform.position);
		has.Add("onstarttarget", tran.gameObject);
		has.Add("position",tran.position);
		iTween.MoveTo(Camera.main.gameObject,has);
	}
	
    public static void Reset()
    {
        isCreate = false;
        roleId = 0;
        playDatas.Clear();
    }

    public void PullAwayFin()
    {
        XuanPanel.Instance.isCreateB = false;
    }

}
