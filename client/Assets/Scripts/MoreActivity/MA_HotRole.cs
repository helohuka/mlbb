using System;
using UnityEngine;
using System.Collections;

public class MA_HotRole : MonoBehaviour {

    public UILabel roleName;
    public UILabel roleProf;
    public UILabel roleDesc;

    public UILabel whoSkill;

	public UILabel buyBtnLab;
	public UILabel needMaonylab;
	public UILabel haveNumLab;
	public UILabel employeeSkillLab;

    [Serializable]
    public class MA_HotRoleSkillInfo
    {
        public UISprite icon;
        public UILabel name;
    }

    public MA_HotRoleSkillInfo[] skills;

    public UILabel buyNum_;
    public UILabel itemCost;

    public Transform modsRoot;

    public UIButton buyBtn_;
	public UITexture back;
    bool hasDestroy_;

	// Use this for initialization
	void Start () {

		HeadIconLoader.Instance.LoadIcon("remenhuoban1",back);
        hasDestroy_ = false;
		buyBtnLab.text = LanguageManager.instance.GetValue("buyBtnLab");
		needMaonylab.text = LanguageManager.instance.GetValue("Guild_XiaoHao");
		haveNumLab.text = LanguageManager.instance.GetValue("havenum");
		employeeSkillLab.text = LanguageManager.instance.GetValue("partnerSkill");

        UIManager.SetButtonEventHandler(buyBtn_.gameObject, EnumButtonEvent.OnClick, OnBuy, 0, 0);

        MoreActivityData.TestHotRoleData();

        COM_ADHotRoleContent data = MoreActivityData.GetHotRoleData();
        if (data.type_ == EntityType.ET_Emplyee)
        {
            EmployeeData empData = EmployeeData.GetData((int)data.roleId_);
            roleName.text = empData.name_;
            roleProf.text = string.Format("{0}:{1}", LanguageManager.instance.GetValue("zhiye"), LanguageManager.instance.GetValue(empData.professionType_.ToString()));
            roleDesc.text = empData.desc_;

            whoSkill.text = LanguageManager.instance.GetValue("partnerSkill");

            SkillData skill = null;
            for (int i = 0; i < skills.Length; ++i)
            {
                skills[i].icon.gameObject.SetActive(false);
            }
            int j = 0;
            for(int i=0; i < skills.Length; ++i)
            {
                if(empData.skills_.Count == 0)
                    break;

                if (i < empData.skills_.Count)
                {
                    for (; j < empData.skills_.Count; ++j)
                    {
                        skill = SkillData.GetData((int)empData.skills_[j].skillID_, (int)empData.skills_[j].skillLevel_);
                        if (skill._SkillType == SkillType.SKT_DefaultActive ||
                            skill._SkillType == SkillType.SKT_DefaultPassive ||
                            skill._SkillType == SkillType.SKT_DefaultSecActive ||
                            skill._SkillType == SkillType.SKT_DefaultSecPassive)
                            continue;

                        skill = SkillData.GetData((int)empData.skills_[i].skillID_, (int)empData.skills_[i].skillLevel_);
                        SkillCellUI cell = UIManager.Instance.AddSkillCellUI(skills[i].icon, skill);
                        cell.showTips = true;
                        skills[i].icon.gameObject.SetActive(true);
                        j++;
                        break;
                    }
                }
            }

            GameManager.Instance.GetActorClone((ENTITY_ID)empData.asset_id, (ENTITY_ID)0, EntityType.ET_Emplyee, (GameObject go, ParamData pdata) =>
            {
                if (hasDestroy_)
                {
                    Destroy(go);
                    return;
                }
                EntityAssetsData ead = EntityAssetsData.GetData(empData.asset_id);
                go.transform.parent = modsRoot;
                go.transform.localScale = Vector3.one * ead.zoom_;
                go.transform.localPosition = new Vector3(0f, -130f, 0f);
                go.transform.localRotation = Quaternion.identity;
                ActorRotate ar = modsRoot.gameObject.AddComponent<ActorRotate>();
                ar.modelRoot_ = modsRoot;
            }, null, "UI");
        }
        else
        {
            BabyData bbData = BabyData.GetData((int)data.roleId_);
            roleName.text = bbData._Name;
            roleProf.text = string.Format("{0}:{1}", LanguageManager.instance.GetValue("BabyInfo_BabyInfoRace"), LanguageManager.instance.GetValue(bbData._RaceType.ToString()));
            roleDesc.gameObject.SetActive(false);

            whoSkill.text = LanguageManager.instance.GetValue("babySkill");

            SkillData skill = null;
            for (int i = 0; i < skills.Length; ++i)
            {
                skills[i].icon.gameObject.SetActive(false);
            }
            int j = 0;
            for (int i = 0; i < skills.Length; ++i)
            {
                if (bbData.skills_.Count == 0)
                    break;

                if (i < bbData.skills_.Count)
                {
                    for (; j < bbData.skills_.Count; ++j)
                    {
                        skill = SkillData.GetData((int)bbData.skills_[j].skillID_, (int)bbData.skills_[j].skillLevel_);
                        if (skill._SkillType == SkillType.SKT_DefaultActive ||
                            skill._SkillType == SkillType.SKT_DefaultPassive ||
                            skill._SkillType == SkillType.SKT_DefaultSecActive ||
                            skill._SkillType == SkillType.SKT_DefaultSecPassive)
                            continue;

                        SkillCellUI cell = UIManager.Instance.AddSkillCellUI(skills[i].icon, skill);
                        cell.showTips = true;
                        skills[i].icon.gameObject.SetActive(true);
                        j++;
                        break;
                    }
                }
            }

            GameManager.Instance.GetActorClone((ENTITY_ID)bbData._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, (GameObject go, ParamData pdata) =>
            {
                if (hasDestroy_)
                {
                    Destroy(go);
                    return;
                }
                EntityAssetsData ead = EntityAssetsData.GetData(bbData._AssetsID);
                go.transform.parent = modsRoot;
                go.transform.localScale = Vector3.one * ead.zoom_;
                go.transform.localPosition = new Vector3(0f, -130f, 0f);
                go.transform.localRotation = Quaternion.identity;
                ActorRotate ar = modsRoot.gameObject.AddComponent<ActorRotate>();
                ar.modelRoot_ = modsRoot;
            }, null, "UI");
        }

        UpdateBuyNumBtn();
        //ItemData item = ItemData.GetData((int)data.itemId_);
        //itemName.text = item.name_;
        itemCost.text = data.price_.ToString();
        //ItemCellUI icell = UIManager.Instance.AddItemCellUI(itemIcon, data.itemId_);
        //icell.showTips = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (MoreActivityData.hotRoleBuyNumDirty)
        {
            UpdateBuyNumBtn();
            MoreActivityData.hotRoleBuyNumDirty = false;
        }
	}

    void UpdateBuyNumBtn()
    {
        COM_ADHotRoleContent data = MoreActivityData.GetHotRoleData();
        if (data != null)
        {
            buyNum_.text = data.buyNum_.ToString();
            if (data.buyNum_ > 0)
            {
                buyBtn_.enabled = true;
                buyBtn_.normalSprite = "huanganniu";
            }
            else
            {
                buyBtn_.enabled = false;
                buyBtn_.normalSprite = "huianniu";
            }
        }
    }

    private void OnBuy(ButtonScript obj, object args, int param1, int param2)
    {
        COM_ADHotRoleContent data = MoreActivityData.GetHotRoleData();
        if (data != null)
        {
            if(data.buyNum_ <= 0)
            {
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("goumaicishu"), null, true);
                return;
            }

            if(data.price_ > GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency))
            {
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("notEnoughMagicCurrency"), delegate
                {
                    StoreUI.SwithShowMe(1);
                });
                return;
            }
        }
        NetConnection.Instance.hotRoleBuy();
    }

    void OnDestroy()
    {
        hasDestroy_ = true;
    }
}
