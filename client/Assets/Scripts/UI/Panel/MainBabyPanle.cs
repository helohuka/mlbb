using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBabyPanle : UIBase {

	public UILabel name;
	public UILabel level;
	public UILabel zhongcheng;
	public UILabel exp;
	public UILabel babyHpTex;
	public UILabel babyMpTex;
	public UISlider fireSlider;
	public UISlider earthSlider;
	public UISlider gustSlider;
	public UISlider waterSlider;


	public Transform TopLeft;
	public Transform BottomRight;
	public Transform ModelPos;
	public GameObject item;
	public UIButton closeBtn;

	private UIGrid grid;
	List<Baby> babylist;
	private UIEventListener Listener;
	private List<GameObject> itemsList = new List<GameObject>();

	private bool isBattle;
	void Start () {
		item.SetActive (false);
		UIViewport port = ApplicationEntry.Instance.ui3DCamera.GetComponent<UIViewport> ();
		ApplicationEntry.Instance.ui3DCamera.transform.position = new Vector3 (0.25f,0.28f,-1.5f);
		ApplicationEntry.Instance.ui3DCamera.fieldOfView = 29;
		port.topLeft = TopLeft;
		port.bottomRight = BottomRight;
		grid = GetComponentInChildren<UIGrid> ();
		babylist =	GamePlayer.Instance.babies_list_;
		AddItems (babylist);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClosebtn, 0, 0);
		isBattle = GamePlayer.Instance.isInBattle;
	}
	private UIButton chuzhan;
	private UIButton daiming;
	public void AddItems(List<Baby> Entitylist)
	{
		for (int i = 0; i<Entitylist.Count; i++) {
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			UIButton []cordBtns = o.GetComponentsInChildren<UIButton>();
			foreach(UIButton btn in cordBtns)
			{

				if(btn.name.Equals("chuzhanButton"))
				{
					chuzhan = btn;
					UIManager.SetButtonEventHandler (btn.gameObject, EnumButtonEvent.OnClick, OnClickCZ, Entitylist[i].InstId, 0);
				}
				if(btn.name.Equals("daimingButton"))
				{
					daiming = btn;
					UIManager.SetButtonEventHandler (btn.gameObject, EnumButtonEvent.OnClick, OnClickDM,Entitylist[i].InstId, 0);
				}
				if(chuzhan != null&& daiming !=null)
				{
					if(isBattle)
					{
						chuzhan.enabled = false;
						daiming.enabled = true;
						chuzhan.defaultColor = Color.white;
						daiming.defaultColor = Color.gray;
					}else
					{
						chuzhan.enabled = true;
						daiming.enabled = false;
						chuzhan.defaultColor = Color.gray;
						daiming.defaultColor = Color.white;
					}
				}

			}

			o.transform.parent = grid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			Listener = UIEventListener.Get(o);
			Listener.onClick +=buttonClick;
			Listener.parameter = new int[]{Entitylist[i].InstId, Entitylist[i].GetIprop(PropertyType.PT_AssetId)};
			grid.repositionNow = true;
			itemsList.Add(o);
		}

	}
	private void OnClickCZ(ButtonScript obj, object args, int param1, int param2)
	{
		chuzhan.defaultColor = Color.gray;
		daiming.defaultColor = Color.white;
		chuzhan.enabled = false;
		daiming.enabled = true;
		GamePlayer.Instance.BabyState (param1, true);
		NetConnection.Instance.setBattlebaby((uint)param1,true);
	}
	private void OnClickDM(ButtonScript obj, object args, int param1, int param2)
	{
		chuzhan.defaultColor = Color.white;
		daiming.defaultColor = Color.gray;
		chuzhan.enabled = true;
		daiming.enabled = false;
		GamePlayer.Instance.BabyState (param1, false);
		NetConnection.Instance.setBattlebaby((uint)param1,false);
	}
	int asid;
	GameObject babyObj;
	public void buttonClick(GameObject sender)
	{
		if (asid != 0 && babyObj != null) {
			DestroyBaby((ENTITY_ID)asid,true,babyObj);
		}
		int []ids = (int [])UIEventListener.Get (sender).parameter;
	    int	uId = ids [0];
		int	asseId = ids[1];
		asid = asseId;
			for (int i = 0; i<itemsList.Count; i++)
			{
				if(sender.name.Equals(itemsList[i].name))
				{
					UISprite []sps = sender.GetComponentsInChildren<UISprite>(true);
					for(int k = 0;k<sps.Length;k++)
					{
						if(sps[k].transform.name=="selectBackground")
						{
							sps[k].enabled = true;
							break;
						}
					}
				}else
				{
					UISprite []sps = sender.GetComponentsInChildren<UISprite>(true);
					for(int k = 0;k<sps.Length;k++)
					{
						if(sps[k].transform.name=="selectBackground")
						{
							sps[k].enabled = false;
							break;
						}
					}
				}
	        }

		Baby Inst = GamePlayer.Instance.GetBabyInst (uId);
		if (Inst == null)
			return;
		SetBabyInfo (Inst.InstName,Inst.GetIprop(PropertyType.PT_HpCurr),Inst.GetIprop(PropertyType.PT_MpCurr), Inst.GetIprop(PropertyType.PT_Land), Inst.GetIprop(PropertyType.PT_Water), Inst.GetIprop(PropertyType.PT_Fire), Inst.GetIprop(PropertyType.PT_Wind));
        GameManager.Instance.GetActorClone((ENTITY_ID)asseId, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, new ParamData(Inst.InstId), "UI");
	
	}
	void DestroyBaby(ENTITY_ID eId,bool unLoadAllLoadedObjects,GameObject obj)
	{
		PlayerAsseMgr.DeleteAsset (eId, unLoadAllLoadedObjects);
		Destroy (obj);
	}
	void SetBabyInfo(string mbabyName,int mhp,int mMptex,int LandValue,int waterValue, int fireValue,int windValue)
	{
		name.text = mbabyName.ToString();
		level.text = "等级：";
		exp.text = "经验值：";
		zhongcheng.text = "忠诚度：";
		babyHpTex.text ="生命：" + mhp.ToString();
		babyMpTex.text = "魔力：" + mMptex.ToString();
		fireSlider.value = fireValue / 100f;
		earthSlider.value = LandValue / 100f;
		gustSlider.value = windValue / 100f;
		waterSlider.value = waterValue / 100f;
	}
    void AssetLoadCallBack(GameObject ro, ParamData data)
    {
        NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
        ro.transform.parent = ModelPos;
        ro.transform.localPosition = Vector3.forward * 1000f;
        ModelPos.transform.localScale = Vector3.one;
        ro.transform.localPosition = Vector3.one;
        ro.transform.LookAt(ApplicationEntry.Instance.ui3DCamera.transform.position);
        EffectLevel el = ro.AddComponent<EffectLevel>();
        el.target = ro.transform.parent.parent.GetComponent<UISprite>();
        babyObj = ro;
    }

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_chongwuPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_chongwuPanel);
	}

	private void OnClickClosebtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	public override void Destroyobj ()
	{
        if (babyObj != null)
        {
            Destroy(babyObj);
            babyObj = null;
        }
	}
}
