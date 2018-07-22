using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillViewUI : UIBase
{
	public UIPanel progressBar;
	public GameObject skillCell;
	public GameObject skillLvelCell;
	public UILabel skillNameLab;
	public UILabel skillLevelLab;
	public UIGrid skillGrid;
	public UIGrid skillinfoGrid;
	public UILabel skillDesc;
	public UISprite topinfo;
	public UILabel zhiyeLab;
	public UIButton closeBtn;
	public UILabel skillNumLab;
	public UIButton forgetBtn;
	public UIButton learnBtn;
	public UIButton kjBtn;
	public UILabel mofaLabel;
	public GameObject messageObj;

	public UISlider expbar;
	public UILabel expLab;
	public UILabel nextDescLabel;
	public UIButton wenhaoBtn;
	public UIButton battleBtn;
	public UIButton liftBtn;
	public GameObject skillObj;
	public GameObject gatherObj;


	private List<GameObject> skillCellList =new List<GameObject>();
	private List<GameObject> skillCellPoolList =new List<GameObject>();
	private List<GameObject> _mainSkillList =new List<GameObject>();
	private int itemid;
	private SkillCellUI _slectSkill; 
	private Profession _profData;
	private int _profType;
	private int _profLevel;

	public static int skillType_;
	public static int minType_ = 1;
	public static int minId_ = 0;

    List<string> iconName_;
	void Start ()
	{
        iconName_ = new List<string>();
		GlobalValue.Get(Constant.C_SkillExpItem, out itemid);
		kjBtn.gameObject.SetActive (false);
		messageObj.SetActive (false);
		UIManager.SetButtonEventHandler (kjBtn.gameObject, EnumButtonEvent.OnClick, OnkjBtn, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (forgetBtn.gameObject, EnumButtonEvent.OnClick, OnForgetBtn, 0, 0);
		UIManager.SetButtonEventHandler (learnBtn.gameObject, EnumButtonEvent.OnClick, OnLearnBtn, 0, 0);
		UIManager.SetButtonEventHandler (wenhaoBtn.gameObject, EnumButtonEvent.OnClick, OnWenhaoBtn, 0, 0);

		UIManager.SetButtonEventHandler (battleBtn.gameObject, EnumButtonEvent.OnClick, OnBattleBtn, 0, 0);
		UIManager.SetButtonEventHandler (liftBtn.gameObject, EnumButtonEvent.OnClick, OnLiftBtn, 0, 0);


		GamePlayer.Instance.SkillExpEnvet += new RequestEventHandler<int> (OnSkillExpEnevt);


		List<COM_Skill> skills = GetMianSkillList ();
		skillNumLab.text = skills.Count + "/10";
		expLab.text = "";
		_profType = GamePlayer.Instance.GetIprop (PropertyType.PT_Profession);
		_profLevel = GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel);
		_profData = Profession.get ((JobType)_profType, _profLevel);
		zhiyeLab.text = _profData.jobName_;
		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{

		
		for(int i =0 ;i<skills.Count;i++ )
		{
			GameObject obj = Object.Instantiate(skillCell.gameObject) as GameObject;
			obj.SetActive(true);
			SkillData  sData = SkillData.GetData((int)skills[i].skillID_, (int)skills[i].skillLevel_);
			SkillCellUI sCell = obj.GetComponent<SkillCellUI>();
			obj.name = sData._Name;
			sCell.SkillName.text = sData._Name;
			sCell.SkillLevel.text = sData._Level.ToString();
			HeadIconLoader.Instance.LoadIcon(sData._ResIconName, obj.GetComponent<SkillCellUI>().SkillIcon);
            if (!iconName_.Contains(sData._ResIconName))
                iconName_.Add(sData._ResIconName);
			sCell.data = sData;
			sCell.skillInst = skills[i];
			
			if(_profData.isProudSkill(_profType,(int)skills[i].skillID_,_profLevel))	  
			{
				obj.transform.FindChild("deyi").GetComponent<UISprite>().gameObject.SetActive(true);
			}

			UIManager.SetButtonEventHandler(obj,EnumButtonEvent.OnClick,OnClickMainSkill,0,0);
			skillGrid.AddChild(obj.transform);
			_mainSkillList.Add(obj);
			obj.transform.localScale = Vector3.one;
			

		}
		
			expbar.gameObject.SetActive(true);
		
			forgetBtn.gameObject.SetActive (false);
			learnBtn.gameObject.SetActive (false);
			curCell = null;
			if(_mainSkillList.Count> 0)
			{
			//	UpdateSKillInfoLevel (_mainSkillList[0].name);
				curCell  =_mainSkillList[0].GetComponent<SkillCellUI> ();
				curCell.back.gameObject.SetActive (true);
				//curCell.buleBack0.gameObject.SetActive (true);
				//curCell.buleBack1.gameObject.SetActive (true);
				UpdatetopInfo (_mainSkillList[0].GetComponent<SkillCellUI> ());
			}

			if(skillType_ != 0)
			{
				liftBtn.isEnabled = false;
				skillObj.gameObject.SetActive(false);
				gatherObj.gameObject.SetActive(true);
				gatherObj.GetComponent<GatherUI> ().ShowMe (minType_,minId_);
			}
			else
			{
				battleBtn.isEnabled = false;
			}
		GamePlayer.Instance.forgetSkillEnvet += new RequestEventHandler<uint> (OnForgetEnvet);
		});
	}


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe( int skillType = 0,int type = 1 , int id = 0)
	{
		skillType_ = skillType;
		minType_ = type;
		minId_ = id;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_SkillPanelUI);
	}
	
	public static void ShowMe(int skillType = 0,int type = 1 , int id = 0)
	{
		skillType_ = skillType;
		minType_ = type;
		minId_ = id;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_SkillPanelUI);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_SkillPanelUI);
	}
	
	#endregion

	private List<COM_Skill>  GetMianSkillList()
	{
		List<COM_Skill> mianSkill = new List<COM_Skill> ();
		List<string> skillStr = new List<string> ();
		List<COM_Skill> skills = GamePlayer.Instance.SkillInsts;
		for(int i =0;i<skills.Count;i++)
		{
			SkillData sData = SkillData.GetData((int)skills[i].skillID_,(int)skills[i].skillLevel_);
			if(sData._SkillType== SkillType.SKT_Active || sData._SkillType == SkillType.SKT_Passive ||sData._SkillType ==  SkillType.SKT_CannotUse)
			{
				if(sData._Name == "null")
				{
					continue;
				}
				if (!skillStr.Contains(sData._Name))
				{
                    skillStr.Add(sData._Name);
					mianSkill.Add(skills[i]);
				}
				else
				{
					for(int j = 0;j<mianSkill.Count;j++)
					{
                        if (SkillData.GetData((int)mianSkill[j].skillID_, (int)mianSkill[j].skillLevel_)._Name == sData._Name)
						{
							if(SkillData.GetData((int)mianSkill[j].skillID_,(int)mianSkill[j].skillLevel_)._Level < sData._Level)
							{
								mianSkill[j] = skills[i];
								break;
							}
						}
					}
				}
			}
		}

		return mianSkill;
	}
	private void OnkjBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (ShopData.GetShopId (itemid) == 0)
			return;
		int shopid = ShopData.GetShopId (itemid);
		if(BagSystem.instance.GetItemMaxNum((uint)itemid)<=0)
		{
			QuickBuyUI.ShowMe(shopid);
		}else
		{
			COM_Item item =	BagSystem.instance.GetItemByItemId((uint)itemid);
			ItemData idata = ItemData.GetData(itemid);
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaoitemwupin").Replace("{n1}",idata.name_).Replace("{n}",idata.desc_),()=>{
				NetConnection.Instance.useItem((uint)item.slot_,(uint)_slectSkill.skillInst.skillID_,(uint)1);
			});

		}

	}
	void usItem()
	{

	}
	private void OnClickMainSkill(ButtonScript obj, object args, int param1, int param2)
	{
		
		SkillCellUI lCell = obj.GetComponent<SkillCellUI>();
		if(curCell != null )
		{
			curCell.back.gameObject.SetActive(false);
			curCell.buleBack0.gameObject.SetActive (false);
			//curCell.buleBack1.gameObject.SetActive (false);
		}
		
		curCell = lCell;
		lCell.back.gameObject.SetActive (true);
		curCell.buleBack0.gameObject.SetActive (true);
		//curCell.buleBack1.gameObject.SetActive (true);
		//UpdateSKillInfoLevel (obj.name);
		UpdatetopInfo (obj.GetComponent<SkillCellUI> ());
	}

    void UpdateDesc(SkillData skdata)
    {
        if (null == skdata)
            return;
        skillDesc.text = skdata._Desc;
        skillDesc.text += "\n";

        SkillData nextdata = SkillData.GetData(skdata._Id, skdata._Level + 1);
        if (nextdata != null)
        {
			//nextDescLabel.text += LanguageManager.instance.GetValue("NextLevlDesc");
			//nextDescLabel.text += ":\n";
			nextDescLabel.text = nextdata._Desc;
        }
    }

	private void UpdatetopInfo(SkillCellUI skillcell)
	{
		if (skillcell == null || skillcell.skillInst == null)
			return;
		messageObj.SetActive (true);
		kjBtn.gameObject.SetActive (true);
		forgetBtn.gameObject.SetActive (true);
		learnBtn.gameObject.SetActive (true);
		_slectSkill = skillcell;
        SkillData sData = SkillData.GetData((int)skillcell.skillInst.skillID_, (int)skillcell.skillInst.skillLevel_);
		topinfo.gameObject.SetActive (true);
        UpdateDesc(sData);
		skillLevelLab.text =skillcell.skillInst.skillLevel_.ToString();
		skillNameLab.text = sData._Name;
		mofaLabel.text = sData._Cost_mana.ToString ();
		float num;
		if(sData._Proficiency ==0)
		{
			num = 1;
			expLab.text =  "1/1";
		}
		else
		{
			num = ((float)skillcell.skillInst.skillExp_ / (float)sData._Proficiency);
			expLab.text = skillcell.skillInst.skillExp_ + "/" + sData._Proficiency;
		}
		//progressBar.baseClipRegion = new Vector4(progressBar.baseClipRegion.x,progressBar.baseClipRegion.y,num,progressBar.baseClipRegion.w);
		expbar.value = num;
	}


	SkillCellUI curCell;

	private void OnClicKindSkill(ButtonScript obj, object args, int param1, int param2)
	{




		forgetBtn.gameObject.SetActive (true);
		learnBtn.gameObject.SetActive (true);
        SkillData sData = obj.GetComponent<SkillCellUI>().data;

		if(sData == null)
		{
			return;
		}
		UpdateDesc(sData);


	}

	private void UpdateSKillInfoLevel(string group)
	{
		_slectSkill = null;
		List<COM_Skill> skills = GamePlayer.Instance.SkillInsts;

		if(skillCellList.Count > 0)
		{
            if (skillCellList[0].GetComponent<SkillCellUI>().data._Name == group)
				return;
		}

		for(int k =0; k<skillCellList.Count;k++)
		{
			skillCellList[k].transform.parent = null;
			skillCellList[k].gameObject.SetActive(false);
			skillCellPoolList.Add(skillCellList[k]); 
		}
		skillCellList.Clear ();

        for (int i = 0; i < skills.Count; ++i)
        {
            SkillData sData = SkillData.GetData((int)skills[i].skillID_, (int)skills[i].skillLevel_);
            if (sData._Name == group)
            {
                GameObject obj;
                if (skillCellPoolList.Count > 0)
                {
                    obj = skillCellPoolList[0];
                    skillCellPoolList.Remove(skillCellPoolList[0]);
                }
                else
                {
                    obj = Object.Instantiate(skillLvelCell.gameObject) as GameObject;
                }
                SkillCellUI sCell = obj.GetComponent<SkillCellUI>();
                sCell.data = sData;
                sCell.skillInst = skills[i];
                sCell.SkillName.text = sData._Name;
                sCell.SkillLevel.text = sData._Level.ToString();
                obj.transform.Find("MP").GetComponent<UILabel>().text = "  " + sData._Cost_mana.ToString() + "MP";
                UIManager.SetButtonEventHandler(obj, EnumButtonEvent.OnClick, OnClicKindSkill, 0, 0);
                obj.transform.parent = skillinfoGrid.transform;
                obj.SetActive(true);
                skillCellList.Add(obj);
                obj.transform.localScale = Vector3.one;
            }

        }
		skillinfoGrid.Reposition ();
	}
	
	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	protected override void DoHide ()
	{
		GamePlayer.Instance.forgetSkillEnvet -= OnForgetEnvet;
		GamePlayer.Instance.SkillExpEnvet -= OnSkillExpEnevt;
		for(int i= 0;i<skillCellPoolList.Count;i++)
		{
			Destroy(skillCellPoolList[i]);
			skillCellPoolList[i] = null;
		}
		skillCellPoolList.Clear();
		base.DoHide ();
	}

	private void OnForgetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (_slectSkill == null)
			return;

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("forgetskill"), () => {
						NetConnection.Instance.forgetSkill ((uint)_slectSkill.data._Id);}); 
	}
	private void OnLearnBtn(ButtonScript obj, object args, int param1, int param2)
	{
		LearningUI.SwithShowMe ();
		//NetConnection.Instance.moveToNpc (9562);
		//Hide ();
	}


	private void OnWenhaoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		HelpUI.SwithShowMe (1,4);
	}

	private void OnBattleBtn(ButtonScript obj, object args, int param1, int param2)
	{
		battleBtn.isEnabled = false;
		liftBtn.isEnabled = true;
		skillObj.gameObject.SetActive(true);
		gatherObj.gameObject.SetActive(false);
	}

	private void OnLiftBtn(ButtonScript obj, object args, int param1, int param2)
	{
		battleBtn.isEnabled = true;
		liftBtn.isEnabled = false;
		skillObj.gameObject.SetActive(false);
		gatherObj.gameObject.SetActive(true);
		gatherObj.GetComponent<GatherUI> ().ShowMe ();
	}

	void OnForgetEnvet(uint skId)
	{
		List<COM_Skill> skills = GetMianSkillList ();
		skillNumLab.text = skills.Count + "/10";

        for (int i = 0; i < _mainSkillList.Count; ++i)
        {
           // if (_mainSkillList[i].GetComponent<SkillCellUI>().data.id_ == (int)skId)
            //{
                //UpdateSKillInfoLevel(x.GetComponent<SkillCellUI>().SkillTabData.name_);
                //_mainSkillList.Remove(_mainSkillList[i]);
                _mainSkillList[i].transform.parent = null;
				skillGrid.RemoveChild(_mainSkillList[i].transform);
				Destroy(_mainSkillList[i]);
               // break;
           // }
        }
		_mainSkillList.Clear ();

		for(int k =0; k<skillCellList.Count;k++)
		{
			skillCellList[k].transform.parent = null;
			skillCellList[k].gameObject.SetActive(false);
			skillCellPoolList.Add(skillCellList[k]); 
		}
		skillCellList.Clear ();

		skillGrid.Reposition ();


		for(int i =0 ;i<skills.Count;i++ )
		{
			GameObject obj = Object.Instantiate(skillCell.gameObject) as GameObject;
			obj.SetActive(true);
			SkillData  sData = SkillData.GetData((int)skills[i].skillID_, (int)skills[i].skillLevel_);
			SkillCellUI sCell = obj.GetComponent<SkillCellUI>();
			obj.name = sData._Name;
			sCell.SkillName.text = sData._Name;
			sCell.SkillLevel.text = sData._Level.ToString();
			HeadIconLoader.Instance.LoadIcon(sData._ResIconName, obj.GetComponent<SkillCellUI>().SkillIcon);
			if (!iconName_.Contains(sData._ResIconName))
				iconName_.Add(sData._ResIconName);
			sCell.data = sData;
			sCell.skillInst = skills[i];
			
			if(_profData.isProudSkill(_profType,(int)skills[i].skillID_,_profLevel))	  
			{
				obj.transform.FindChild("deyi").GetComponent<UISprite>().gameObject.SetActive(true);
			}
			
			UIManager.SetButtonEventHandler(obj,EnumButtonEvent.OnClick,OnClickMainSkill,0,0);
			skillGrid.AddChild(obj.transform);
			_mainSkillList.Add(obj);
			obj.transform.localScale = Vector3.one;
			
			
		}






		skillDesc.text = "";
		nextDescLabel.text = "";
		expLab.text = "";
		forgetBtn.gameObject.SetActive (false);
		learnBtn.gameObject.SetActive (false);
		topinfo.gameObject.SetActive (false);
		kjBtn.gameObject.SetActive (false);
		messageObj.SetActive (false);
		PopText.Instance.Show (LanguageManager.instance.GetValue("yiwangskill"));

		curCell = null;
		if(_mainSkillList.Count > 0)
		{
			curCell = _mainSkillList[0].GetComponent<SkillCellUI> ();
			curCell.back.gameObject.SetActive (true);
			//curCell.buleBack0.gameObject.SetActive (true);
			//curCell.buleBack1.gameObject.SetActive (true);
			UpdatetopInfo (_mainSkillList[0].GetComponent<SkillCellUI> ());
		}

	}

	void OnSkillExpEnevt(int id)
	{
		curCell.skillInst = GamePlayer.Instance.GetSkillById ((int)curCell.skillInst.skillID_);
		UpdatetopInfo (curCell);
		for(int i=0;i<_mainSkillList.Count;i++)
		{
			SkillCellUI cellUI =_mainSkillList[i].GetComponent<SkillCellUI> ();
			cellUI.skillInst= GamePlayer.Instance.GetSkillById ((int)cellUI.skillInst.skillID_);
			cellUI.SkillLevel.text = cellUI.skillInst.skillLevel_.ToString();
			SkillData  sData = SkillData.GetData((int)cellUI.skillInst.skillID_, (int)cellUI.skillInst.skillLevel_);

			cellUI.SkillName.text = sData._Name;
			////if( skillInst.skillID_==curCell.skillInst.skillID_ )
			//{
				//SkillCellUI skcell = _mainSkillList[i].GetComponent<SkillCellUI> ();
				//skcell.SkillLevel.text = curCell.skillInst.skillLevel_.ToString();
				//break;
			//}//
		}
	}

	public override void Destroyobj ()
	{
        for (int i = 0; i < iconName_.Count; ++i)
        {
            HeadIconLoader.Instance.Delete(iconName_[i]);
        }
        iconName_.Clear();
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_SkillPanelUI, AssetLoader.EAssetType.ASSET_UI), true);
	}

}

