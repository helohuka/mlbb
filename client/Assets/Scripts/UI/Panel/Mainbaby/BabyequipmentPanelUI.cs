using UnityEngine;
using System.Collections.Generic;

public class BabyequipmentPanelUI : UIBase {

	public UITexture icon;
    public UISprite iconKuang;
    public UISprite iconType;
	public UISprite touSp;
	public UISprite shouSp;
	public UISprite jiaoSp;
	public UISprite shenSp;
	public UISprite skillSp;

	public UISprite leftTopobj;
	public UISprite rightTopobj;
	public UISprite leftBottomobj;
	public UISprite rightBottomobj;
	public UISprite starobj;
	public UISprite skillobj;

	public UIGrid grid;
	public GameObject item;

	public UILabel hpLabel;
	public UILabel mpLable;
	public UILabel attackLable;
	public UILabel defenseLable;
	public UILabel spiritLable;
	public UILabel agileLable;
	public UILabel replyLable;
	public UILabel hpLabelSim;
	public UILabel mpLableSim;
	public UILabel attackLableSim;
	public UILabel defenseLableSim;
	public UILabel spiritLableSim;
	public UILabel agileLableSim;
	public UILabel replyLableSim;

    public BabyEquipTips tips;

    string crtHeadIcon;

    Dictionary<EquipmentSlot, UISprite> slot2Sp;
    List<ItemCellUI> cache;
    SkillCellUI skillCache;
    List<GameObject> bagItemPool;

	void Start () {
        slot2Sp = new Dictionary<EquipmentSlot, UISprite>();
        slot2Sp.Add(EquipmentSlot.ES_Head, touSp);
        slot2Sp.Add(EquipmentSlot.ES_SingleHand, shouSp);
        slot2Sp.Add(EquipmentSlot.ES_Boot, jiaoSp);
        slot2Sp.Add(EquipmentSlot.ES_Body, shenSp);

        cache = new List<ItemCellUI>();
        bagItemPool = new List<GameObject>();

        if (GamePlayer.Instance.babies_list_.Count == 0)
            return;

        if (GamePlayer.Instance.babies_list_.Count <= MainbabyListUI.CrtSelectIdx)
        {
            MainbabyListUI.CrtSelectIdx = GamePlayer.Instance.babies_list_.Count - 1;
            return;
        }

        UpdateHead();
        UpdateEquip();
        UpdateAttr();
        UpdateAttrPlus();
        UpdateBag();

        MainbabyListUI.SelectDirty = false;
        BagSystem.instance.isBabyEquipDirty_ = false;

        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] != null)
            GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isEquipDirty_ = false;
	}
	void OnEnable()
	{	
		

	}

	void Update () {

        if (GamePlayer.Instance.babies_list_.Count == 0)
        {
            Destroyobj();
            return;
        }

        if (GamePlayer.Instance.babies_list_.Count <= MainbabyListUI.CrtSelectIdx)
        {
            MainbabyListUI.CrtSelectIdx = GamePlayer.Instance.babies_list_.Count - 1;
            return;
        }

        if (MainbabyListUI.SelectDirty)
        {
            UpdateHead();
            UpdateEquip();
			UpdateAttr();
            UpdateAttrPlus();
            MainbabyListUI.SelectDirty = false;
        }

        //穿脱装备
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] != null)
        {
            if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isEquipDirty_)
            {
                UpdateEquip();
                UpdateAttrPlus();
                GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isEquipDirty_ = false;
            }
        }

        //获得或删除装备
        if (BagSystem.instance.isBabyEquipDirty_)
        {
            UpdateBag();
            BagSystem.instance.isBabyEquipDirty_ = false;
        }

        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] != null)
        {
            if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isPropDirty_)
            {
                UpdateAttr();
                GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isPropDirty_ = false;
            }
        }
	}

    void UpdateEquip()
    {
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] == null)
        {
            ClearEquip();
            return;
        }
        ClearEquip();
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            for (int i = 0; i < GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips.Length; ++i)
            {
                if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i] != null)
                {
                    ItemData equipData = ItemData.GetData((int)GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i].itemId_);
                    EquipmentSlot es = (EquipmentSlot)equipData.slot_;
                    if (slot2Sp.ContainsKey(es))
                    {
                        UISprite sp = slot2Sp[es];
                        if (sp != null)
                        {
                            ItemCellUI equip = UIManager.Instance.AddItemInstCellUI(sp, GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i]);
                            UIManager.SetButtonEventHandler(equip.gameObject, EnumButtonEvent.OnClick, OnClickEquipOnBody, i, 0);
                            cache.Add(equip);
                        }
                    }
                }
            }
        }, 1);
        UpdateSkill();
    }

    void UpdateSkill()
    {
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] == null ||
            GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].suitSkill == null)
        {
            ClearSkill();
            ClearEffect();
            return;
        }
        
        SkillData sData = SkillData.GetData((int)GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].suitSkill.skillID_, (int)GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].suitSkill.skillLevel_);
        skillCache = UIManager.Instance.AddSkillCellUI(skillSp, sData);
        skillCache.showTips = true;

        ShowEffect(GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].isHighestSuitSkill);
    }

    void UpdateHead()
    {
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] == null)
        {
            ClearHead();
            return;
        }
        ClearHead();
        BabyData bd = BabyData.GetData(GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_TableId));
        EntityAssetsData ead = EntityAssetsData.GetData(bd._AssetsID);
        iconKuang.spriteName = BabyData.GetPetQuality(bd._PetQuality);
        iconType.spriteName = bd._Tpye.ToString();
        crtHeadIcon = ead.assetsIocn_;
        HeadIconLoader.Instance.LoadIcon(crtHeadIcon, icon);
    }

    void UpdateBag()
    {
        int idx = 0;
        for (; idx < BagSystem.instance._BabyEquips.Count; ++idx)
        {
            if (idx >= bagItemPool.Count)
            {
                GameObject go = (GameObject)GameObject.Instantiate(item) as GameObject;
                go.transform.parent = grid.transform;
                go.transform.localScale = Vector3.one;
                bagItemPool.Add(go);
            }
            bagItemPool[idx].SetActive(true);
            ItemCellUI cell = UIManager.Instance.AddItemInstCellUI(bagItemPool[idx].GetComponent<UISprite>(), BagSystem.instance._BabyEquips[idx]);
            UIDragScrollView uidsv = cell.gameObject.GetComponent<UIDragScrollView>();
            if (uidsv == null)
                cell.gameObject.AddComponent<UIDragScrollView>();
            UIManager.SetButtonEventHandler(cell.gameObject, EnumButtonEvent.OnClick, OnClickEquipInBag, (int)BagSystem.instance._BabyEquips[idx].instId_, 0);
        }
        for (; idx < bagItemPool.Count; ++idx)
        {
            bagItemPool[idx].SetActive(false);
        }

        grid.Reposition();
    }

    private void OnClickEquipInBag(ButtonScript obj, object args, int param1, int param2)
    {
        Baby baby = GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx];
        if (baby == null)
            return;

        COM_Item inst = BagSystem.instance.GetItemByInstId(param1);
        ItemData item = ItemData.GetData((int)inst.itemId_);
        if (baby.Equips[(int)item.slot_] == null)
            tips.ShowTips((uint)baby.InstId, inst, false);
        else
            tips.ShowTips((uint)baby.InstId, baby.Equips[(int)item.slot_], inst);
    }

    private void OnClickEquipOnBody(ButtonScript obj, object args, int param1, int param2)
    {
        Baby baby = GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx];
        if (baby == null)
            return;

        COM_Item inst = baby.Equips[param1];
        ItemData item = ItemData.GetData((int)inst.itemId_);
        tips.ShowTips((uint)baby.InstId, inst, true);
    }

    void UpdateAttrPlus()
    {
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] == null)
        {
            ClearAttr();
            return;
        }
        int hpPlus = CalcEquipAttr(PropertyType.PT_HpMax);
        int mpPlus = CalcEquipAttr(PropertyType.PT_MpMax);
        int atkPlus = CalcEquipAttr(PropertyType.PT_Attack);
        int defPlus = CalcEquipAttr(PropertyType.PT_Defense);
        int sprPlus = CalcEquipAttr(PropertyType.PT_Spirit);
        int agiPlus = CalcEquipAttr(PropertyType.PT_Agile);
        int repPlus = CalcEquipAttr(PropertyType.PT_Reply);

        hpLabelSim.text = hpPlus == 0? "": "+" + hpPlus.ToString();
        mpLableSim.text = mpPlus == 0 ? "" : "+" + mpPlus.ToString();
        attackLableSim.text = atkPlus == 0 ? "" : "+" + atkPlus.ToString();
        defenseLableSim.text = defPlus == 0 ? "" : "+" + defPlus.ToString();
        spiritLableSim.text = sprPlus == 0 ? "" : "+" + sprPlus.ToString();
        agileLableSim.text = agiPlus == 0 ? "" : "+" + agiPlus.ToString();
        replyLableSim.text = repPlus == 0 ? "" : "+" + repPlus.ToString();
    }

    void UpdateAttr()
    {
        if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx] == null)
        {
            ClearAttr();
            return;
        }

        int hpPlus = CalcEquipAttr(PropertyType.PT_HpMax);
        int mpPlus = CalcEquipAttr(PropertyType.PT_MpMax);
        int atkPlus = CalcEquipAttr(PropertyType.PT_Attack);
        int defPlus = CalcEquipAttr(PropertyType.PT_Defense);
        int sprPlus = CalcEquipAttr(PropertyType.PT_Spirit);
        int agiPlus = CalcEquipAttr(PropertyType.PT_Agile);
        int repPlus = CalcEquipAttr(PropertyType.PT_Reply);

        hpLabel.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_HpMax) - hpPlus).ToString();
        mpLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_MpMax) - mpPlus).ToString();
        attackLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_Attack) - atkPlus).ToString();
        defenseLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_Defense) - defPlus).ToString();
        spiritLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_Spirit) - sprPlus).ToString();
        agileLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_Agile) - agiPlus).ToString();
        replyLable.text = (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].GetIprop(PropertyType.PT_Reply) - repPlus).ToString();
    }

    int CalcEquipAttr(PropertyType pType)
    {
        float total = 0;
        for (int i = 0; i < GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips.Length; ++i)
        {
            if (GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i] != null)
            {
                for (int j = 0; j < GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i].propArr.Length; ++j)
                {
                    if(GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i].propArr[j].type_ == pType)
                        total += GamePlayer.Instance.babies_list_[MainbabyListUI.CrtSelectIdx].Equips[i].propArr[j].value_;
                }
            }
        }
        return (int)total;
    }

    void ShowEffect(bool isHighest)
    {
        leftTopobj.gameObject.SetActive(true);
        rightTopobj.gameObject.SetActive(true);
        leftBottomobj.gameObject.SetActive(true);
        rightBottomobj.gameObject.SetActive(true);
        starobj.gameObject.SetActive(true);
        skillobj.gameObject.SetActive(isHighest);
    }

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BabyEquPanle);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BabyEquPanle);
	}

    void ClearEquip()
    {
        for (int i = 0; i < cache.Count; ++i)
        {
            if (cache[i] == null)
                continue;
            GameObject.Destroy(cache[i].gameObject);
        }
        cache.Clear();
    }

    void ClearSkill()
    {
        if (skillCache != null)
        {
            GameObject.Destroy(skillCache.gameObject);
            skillCache = null;
        }
    }

    void ClearHead()
    {
        if (!string.IsNullOrEmpty(crtHeadIcon))
        {
            HeadIconLoader.Instance.Delete(crtHeadIcon);
            crtHeadIcon = "";
        }
    }

    void ClearAttr()
    {
        hpLabel.text = "";
        mpLable.text = "";
        attackLable.text = "";
        defenseLable.text = "";
        spiritLable.text = "";
        agileLable.text = "";
        replyLable.text = "";

        hpLabelSim.text = "";
        mpLableSim.text = "";
        attackLableSim.text = "";
        defenseLableSim.text = "";
        spiritLableSim.text = "";
        agileLableSim.text = "";
        replyLableSim.text = "";
    }

    void ClearEffect()
    {
        leftTopobj.gameObject.SetActive(false);
        rightTopobj.gameObject.SetActive(false);
        leftBottomobj.gameObject.SetActive(false);
        rightBottomobj.gameObject.SetActive(false);
        starobj.gameObject.SetActive(false);
        skillobj.gameObject.SetActive(false);
    }

	void OnDestroy()
	{

	}
	public override void Destroyobj ()
	{
        ClearHead();
        ClearEquip();
        ClearAttr();
        ClearSkill();
        ClearEffect();
	}
}
