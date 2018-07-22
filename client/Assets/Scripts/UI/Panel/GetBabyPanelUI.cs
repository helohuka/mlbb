using UnityEngine;
using System.Collections;

public class GetBabyPanelUI : UIBase {

	public UISprite babyTitle;
	public UISprite huobanZhen;
	public GameObject typeObj;
	public GameObject numObj;
	public UISprite backSp;
	public UILabel raceLable;
	public UITexture raceTex;
	public UISprite typeSp;
	public UILabel _EnterLable;
	public UILabel nameLabel;
	public UILabel typeNameLabel;
	public Transform pos;
	public UIButton CloseBtn;
	public UISprite namesp;
	public UISprite getTypeSp;
	public UISprite gun1;
	public UISprite gun2;
	private static int babyId;
	private static int empId;
	private GameObject robaby;
    bool hasDestroy = false;
	void Start () {
		_EnterLable.text = LanguageManager.instance.GetValue ("enter");
        nameLabel.transform.parent.gameObject.SetActive(false);
		babyTitle.gameObject.SetActive (false);
		huobanZhen.gameObject.SetActive (false);
		backSp.gameObject.SetActive (false);
		getTypeSp.gameObject.SetActive (false);
        CloseBtn.gameObject.SetActive(false);
		typeObj.SetActive (false);
		numObj.SetActive (false);
		gun1.gameObject.SetActive (false);
		gun2.gameObject.SetActive (false);
//        ShowGetBaby();
//        EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_GainBabyShine, transform, null, (GameObject obj) =>
//        {
//			if(!hasDestroy)
//			{
				//EffectLevel el = obj.AddComponent<EffectLevel>();
				//el.target = effLv;
//				GlobalInstanceFunction.Instance.Invoke(() =>
//				{
//					if(!hasDestroy)
//					{
                        EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_GainBaby, transform, null, (GameObject go) =>
                                               {
							//EffectLevel el1 = go.AddComponent<EffectLevel>();
							//el1.target = effLv;
							GlobalInstanceFunction.Instance.Invoke(() =>
							{
								if(!hasDestroy)
								{
									if(babyId != 0)
									{
										ShowGetBaby();
									}
									if(empId != 0)
									{
										ShowGetEmp();
									}
								}
							}, 1.5f);
                        });
//					}
//				}, 0.8f);
//			}
//        });
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
        XInput.Instance.dealInput = true;
		Hide ();
        if (StageMgr.Scene_name.Equals(GlobalValue.StageName_piantoudonghuaf))
        {
            Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
            NetConnection.Instance.transforScene(2);
            //if (PrebattleEvent.getInstance.BackEvent != null)
            //{
            //    PrebattleEvent.getInstance.BackEvent();
            //}
            //GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_FirstEnterMainScene);
        }
	}
	public void ShowGetBaby()
	{
        if (hasDestroy)
            return;
		typeObj.SetActive (true);
		numObj.SetActive (true);
		gun1.gameObject.SetActive (true);
		gun2.gameObject.SetActive (true);
		babyTitle.gameObject.SetActive (true);
        nameLabel.transform.parent.gameObject.SetActive(true);
		backSp.gameObject.SetActive (true);
        CloseBtn.gameObject.SetActive(true);
		BabyData bdata = BabyData.GetData (babyId);
//		backSp.spriteName = "fazhen";
//		getTypeSp.spriteName = "hdcw";
		namesp.width = 630;
		typeNameLabel.text = LanguageManager.instance.GetValue ("gxhuodechongwu");
		nameLabel.text = bdata._Name;
		raceLable.text = LanguageManager.instance.GetValue(bdata._RaceType.ToString());
		HeadIconLoader.Instance.LoadIcon (bdata._RaceIcon, raceTex);
		typeSp.spriteName = bdata._Tpye.ToString();
		GameManager.Instance.GetActorClone((ENTITY_ID)bdata._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "3D");
	}


	public void ShowGetEmp()
	{
		if (hasDestroy)
			return;
		typeObj.SetActive (false);
		numObj.SetActive (false);
		gun1.gameObject.SetActive (false);
		gun2.gameObject.SetActive (false);
		getTypeSp.gameObject.SetActive (true);
		huobanZhen.gameObject.SetActive (true);
		//backSp.spriteName = "fazhen2";
		//getTypeSp.spriteName = "hdhb";
		namesp.width = 342;
		nameLabel.transform.parent.gameObject.SetActive(true);
		CloseBtn.gameObject.SetActive(true);
		EmployeeData bdata = EmployeeData.GetData (empId);
		typeNameLabel.text = LanguageManager.instance.GetValue ("gxhuodehuoban");
		nameLabel.text = bdata.name_;
		GameManager.Instance.GetActorClone((ENTITY_ID)bdata.asset_id, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "3D");
	}



	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
        if (hasDestroy)
        {
            Destroy(ro);
            return;
        }

		ro.transform.parent = pos;
		ro.transform.localPosition = Vector3.zero;
		ro.transform.rotation = new Quaternion (ro.transform.rotation.x,-180,ro.transform.rotation.z,ro.transform.rotation.w);
		ro.transform.localScale = new Vector3 (ro.transform.localScale.x*0.8f,ro.transform.localScale.y*0.8f,ro.transform.localScale.z*0.8f);
		if(robaby != null)
		{
			Destroy (robaby);
			robaby = null;
		}
		robaby = ro;
	}

	public static void ShowMe(int babyid)
	{
        XInput.Instance.dealInput = false;
		babyId = babyid;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GetBabyPanelUI);
	}

	public static void ShowEmployee(int id)
	{
		XInput.Instance.dealInput = false;
		empId = id;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GetBabyPanelUI);
	}

	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GetBabyPanelUI);
	}
	public override void Destroyobj ()
	{
        hasDestroy = true;
        babyId = 0;
        empId = 0;
	}
}
