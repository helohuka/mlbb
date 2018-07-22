using UnityEngine;
using System.Collections;

public class EmployeeJinjieOkUI : UIBase
{
	public UIButton closeBtn;
	public UIButton okBtn;
	public UILabel hpLab;
	public UILabel spiritLab;
	public UILabel magicLab;
	public UILabel critLab;
	public UILabel attLab;
	public UILabel hitLab;
	public UILabel defenseLab;
	public UILabel dodgeLab;
	public UILabel agileLab;
	public UILabel counterpunchLab;
	public UILabel replyLab;

	public UILabel addSmLab;
	public UILabel addMlLab;
	public UILabel addGjLab;
	public UILabel addFyLab;
	public UILabel addMjLab;
	public UILabel addHfLab;
	public UILabel addJsLab;
	public UILabel addBsLab;
	public UILabel addMzLab;
	public UILabel addSbLab;
	public UILabel addFjLab;


	public Transform mpos;
	public UISprite qAddImg;
	public UISprite jobIcon;
	public UISprite pinzhiImg;
	public UILabel profssLab;
	public UILabel nameLab;
	public UILabel zhanliLab;
	private GameObject babyObj;
	private bool hasDestroy;
	private static int employeeInstId;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (okBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		ShowEmployeeInfo ();
		transform.localPosition = new Vector3 (0f, 0f, -500f);
	}

	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe(int id)
	{
		employeeInstId = id;
        NetWaitUI.HideMe();

		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_JinjieOkPanel);
	}
	public static void ShowMe(int id)
	{
		employeeInstId = id;
        NetWaitUI.HideMe();

		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_JinjieOkPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_JinjieOkPanel);
	}
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	#endregion

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	private void ShowEmployeeInfo()
	{
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_employeejinjie1, gameObject.transform,null,(GameObject obj)=>{obj.transform.localPosition = new Vector3(0,0,-1000f);});
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_employeejinjie2, gameObject.transform);
		Employee curEmployee = null;
		curEmployee = GamePlayer.Instance.GetEmployeeById (employeeInstId);
		if (curEmployee == null)
			return;

		//GameManager.Instance.GetActorClone((ENTITY_ID) EmployeeData.GetData(curEmployee.GetIprop(PropertyType.PT_TableId)).asset_id, 
		                                //   (ENTITY_ID)curEmployee.WeaponAssetID, EntityType.ET_Emplyee, AssetLoadCallBack, new ParamData(curEmployee.InstId),"UI");
		nameLab.text = curEmployee.InstName;
		jobIcon.spriteName = ((JobType)curEmployee.GetIprop (PropertyType.PT_Profession)).ToString ();
		profssLab.text =  Profession.get((JobType)curEmployee.GetIprop(PropertyType.PT_Profession), 
		                              curEmployee.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_; 
		pinzhiImg.spriteName = getPinzhiImg ((int)curEmployee.quality_);
		zhanliLab.text =  curEmployee.GetIprop(PropertyType.PT_FightingForce).ToString();
		hpLab.text = curEmployee.GetIprop (PropertyType.PT_HpMax).ToString();
		spiritLab.text = curEmployee.GetIprop (PropertyType.PT_Spirit).ToString(); 
		magicLab.text = curEmployee.GetIprop (PropertyType.PT_MpMax).ToString();
		critLab.text = curEmployee.GetIprop (PropertyType.PT_Crit).ToString();
		attLab.text = curEmployee.GetIprop (PropertyType.PT_Attack).ToString();
		hitLab.text = curEmployee.GetIprop (PropertyType.PT_Hit).ToString();
		defenseLab.text = curEmployee.GetIprop (PropertyType.PT_Defense).ToString();
		dodgeLab.text = curEmployee.GetIprop (PropertyType.PT_Dodge).ToString();
		agileLab.text = curEmployee.GetIprop (PropertyType.PT_Agile).ToString();
		counterpunchLab.text = curEmployee.GetIprop (PropertyType.PT_counterpunch).ToString();
		replyLab.text = curEmployee.GetIprop (PropertyType.PT_Reply).ToString();
	
		Employee oldEmployee = EmployessSystem.instance._OldEmployee;
		if (oldEmployee == null)
			return;
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_HpMax), oldEmployee.GetIprop (PropertyType.PT_HpMax), addSmLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_MpMax), oldEmployee.GetIprop (PropertyType.PT_MpMax), addMlLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Attack), oldEmployee.GetIprop (PropertyType.PT_Attack), addGjLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Defense), oldEmployee.GetIprop (PropertyType.PT_Defense), addFyLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Agile), oldEmployee.GetIprop (PropertyType.PT_Agile), addMjLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Reply), oldEmployee.GetIprop (PropertyType.PT_Reply), addHfLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Spirit), oldEmployee.GetIprop (PropertyType.PT_Spirit), addJsLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Crit), oldEmployee.GetIprop (PropertyType.PT_Crit), addBsLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Hit), oldEmployee.GetIprop (PropertyType.PT_Hit), addMzLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_Dodge), oldEmployee.GetIprop (PropertyType.PT_Dodge), addSbLab);
		ShowAddProp (curEmployee.GetIprop (PropertyType.PT_counterpunch), oldEmployee.GetIprop (PropertyType.PT_counterpunch), addFjLab);
	}

	void ShowAddProp(int newNum,int oldNum,UILabel ui)
	{
		if(newNum- oldNum > 0)
		{
			ui.gameObject.SetActive(true);
			ui.text = "+" +(newNum- oldNum); 
		}
		else
		{
			ui.gameObject.SetActive(false);
		}
	}


	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if (gameObject == null || !this.gameObject.activeSelf)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		
		if(babyObj != null)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		//ro.transform.localPosition = Vector3.zero;
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		//EffectLevel el =ro.AddComponent<EffectLevel>();
		//el.target =ro.transform.parent.parent.GetComponent<UISprite>();
		babyObj = ro;
	}
	private string getPinzhiImg(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_cheng";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "shuxing_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "shuxing_lv";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "shuxing_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "shuxing_zi";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "shuxing_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "shuxing_fen";
		}
		return "";
	}

}

