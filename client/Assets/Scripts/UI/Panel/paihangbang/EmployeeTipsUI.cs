using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EmployeeTipsUI : UIBase {

	public List<UISprite> star = new List<UISprite> ();
	public List<UITexture> equipList = new List<UITexture> ();
	public List<UITexture> skillIconList = new List<UITexture> ();
	public UILabel nameLab;
	public UILabel jobLab;
	public UISprite jobIcon;
	public UILabel employessNumLab;
	public UILabel shengmingLabel;
	public UILabel jingshengLabel;
	public UILabel moliLabel;
	public UILabel jishaLabel;
	public UILabel gongjiLabel;
	public UILabel mingzhongLabel;
	public UILabel fangyuLabel;
	public UILabel minjieLabel;
	public UILabel fanjiLabel;
	public UILabel huifuLabel;
	public UILabel zhandouliLabel;
	public UILabel shanbiLabel;
	public Transform mpos;
	private GameObject babyObj;
	public UIButton closeBtn;
	public UISprite qualitySp;
	public UISprite numPl;
	private COM_EmployeeInst _EInst;

	private List<string> _icons = new List<string>();
	public COM_EmployeeInst EmployeeInst
	{
		set
		{
			if(value != null)
			{
				_EInst = value;
				employessNumLab.text = _EInst.properties_[(int)PropertyType.PT_FightingForce].ToString();
				nameLab.text = _EInst.instName_;
				jobLab.text = Profession.get((JobType)_EInst.properties_[(int)PropertyType.PT_Profession], 
				                             (int)_EInst.properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_; 
				jobIcon.spriteName = ((JobType)_EInst.properties_ [(int)PropertyType.PT_Profession]).ToString ();
				moliLabel.text = _EInst.properties_ [(int)PropertyType.PT_MpCurr].ToString();
				shengmingLabel.text = _EInst.properties_ [(int)PropertyType.PT_HpCurr].ToString();
				jingshengLabel.text = _EInst.properties_ [(int)PropertyType.PT_Spirit].ToString();
				jishaLabel.text = _EInst.properties_ [(int)PropertyType.PT_Sex].ToString();
				gongjiLabel.text = _EInst.properties_ [(int)PropertyType.PT_Attack].ToString();
				mingzhongLabel.text = _EInst.properties_ [(int)PropertyType.PT_Hit].ToString();
				fangyuLabel.text = _EInst.properties_ [(int)PropertyType.PT_Defense].ToString();
				minjieLabel.text = _EInst.properties_ [(int)PropertyType.PT_Agile].ToString();
				fanjiLabel.text = _EInst.properties_ [(int)PropertyType.PT_counterpunch].ToString();
				huifuLabel.text = _EInst.properties_ [(int)PropertyType.PT_Reply].ToString();
				zhandouliLabel.text = _EInst.properties_ [(int)PropertyType.PT_FightingForce].ToString();
				shanbiLabel.text = _EInst.properties_ [(int)PropertyType.PT_Dodge].ToString();

				for(int i =0;i<star.Count;i++)
				{
					star[i].gameObject.SetActive(false);
				}

				for(int j =0;j<_EInst.star_ && j<5;j++)
				{
					star[j].gameObject.SetActive(true);
				}
				for(int i =0;i<equipList.Count;i++)
				{
					if(equipList[i] != null)
					{
						UITexture te = equipList[i].GetComponent<UITexture>();
						te.mainTexture = null;
					}
				}
				GameManager.Instance.GetActorClone((ENTITY_ID) EmployeeData.GetData((int)_EInst.properties_[(int)PropertyType.PT_TableId]).asset_id, 
				                                   (ENTITY_ID)_EInst.weaponId_, EntityType.ET_Emplyee, AssetLoadCallBack,new ParamData((int)_EInst.instId_),"UI");
				for(int i =0;i<_EInst.equips_.Length;i++)
				{
					UITexture te = equipList[(int)_EInst.equips_[i].slot_].GetComponent<UITexture>();
					HeadIconLoader.Instance.LoadIcon (ItemData.GetData((int)_EInst.equips_[i].itemId_).icon_, te);

					if(!_icons.Contains(ItemData.GetData((int)_EInst.equips_[i].itemId_).icon_))
					{
						_icons.Add(ItemData.GetData((int)_EInst.equips_[i].itemId_).icon_);
					}

				}
				qualitySp.spriteName = EmployessSystem.instance.GetQualityYuanBack((int)_EInst.quality_);
				numPl.spriteName = LanguageManager.instance.GetValue(_EInst.quality_.ToString());
//				EmployeeConfigData ecd = EmployeeConfigData.GetData((int)_EInst.properties_[(int)PropertyType.PT_TableId],(int)_EInst.star_);
//				for(int i =0;i< ecd.items.Count;i++)
//				{
//					ItemData edata = ItemData.GetData(ecd.items[i]);
//					UITexture te = equipList[(int)edata.slot_].GetComponent<UITexture>();
//					HeadIconLoader.Instance.LoadIcon (ItemData.GetData(ecd.items[i]).icon_, te);	
//				}


				for(int i =0;i<skillIconList.Count;i++)
				{
					if(skillIconList[i] != null)
					{
						UITexture te = skillIconList[i].GetComponent<UITexture>();
						te.mainTexture = null;
					}
				}
				for(int i =0;i<_EInst.skill_.Length;i++)
				{
					if(skillIconList[i] != null)
					{
						UITexture te = skillIconList[i].GetComponent<UITexture>();
						HeadIconLoader.Instance.LoadIcon (SkillData.GetData((int)_EInst.skill_[i].skillID_,(int)_EInst.skill_[i].skillLevel_)._ResIconName, te);	

						if(!_icons.Contains(SkillData.GetData((int)_EInst.skill_[i].skillID_,(int)_EInst.skill_[i].skillLevel_)._ResIconName))
						{
							_icons.Add(SkillData.GetData((int)_EInst.skill_[i].skillID_,(int)_EInst.skill_[i].skillLevel_)._ResIconName);
						}

					}
				}

			}
		}
		get
		{
			return _EInst;
		}
	}
	public void SetEmployeeInst(COM_EmployeeInst inst)
	{

		EmployeeInst = inst;
	}
	void DelModes()
	{
		if(mpos.childCount>0)
		{
			Destroy(mpos.GetChild(0).gameObject);
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if (!this.gameObject.activeSelf)
			return;
		if(babyObj != null)
		{
			Destroy(ro);
		}
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		//ro.transform.localPosition = Vector3.zero;
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = new Quaternion (ro.transform.localRotation.x,180f,ro.transform.localRotation.z,ro.transform.localRotation.w) /*Quaternion.Euler (10f, 180f, 0f)*/;
		EffectLevel el =ro.AddComponent<EffectLevel>();
		el.target =ro.transform.parent.parent.GetComponent<UISprite>();
		babyObj = ro;
	}
	void OnDisable()
	{
		DelModes ();
	}


	public override void Destroyobj ()
	{

	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}
