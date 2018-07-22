using UnityEngine;
using System.Collections.Generic;

public class BabyEquipTipItem : MonoBehaviour {

    public UILabel itemName;
    public UILabel itemDesc;
    public UILabel itemLv;
    public UILabel itemKind;
    public UILabel itemGainWay;
    public UIButton equipBtn;
    public UIButton takeoffBtn;
    public UITexture itemIcon;
    public GameObject isLock;
    public GameObject isBind;
	public GameObject iszhuangbei;
    public UIGrid grid;
    public GameObject propItem;
    public UILabel skillName;
	public UISprite backicon;
    List<GameObject> propPool = new List<GameObject>();

    string iconCache;
    uint instId;
    uint targetId;

	public void SetData(uint target, COM_Item equip, bool isInbag)
    {
        targetId = target;
        instId = equip.instId_;
        ItemData data = ItemData.GetData((int)equip.itemId_);
        itemName.text = data.name_;
        itemDesc.text = data.desc_;
        itemLv.text = data.level_.ToString();
        itemKind.text = LanguageManager.instance.GetValue(data.mainType_.ToString());
        itemGainWay.text = data.acquiringWay_;
        equipBtn.gameObject.SetActive(isInbag);
        takeoffBtn.gameObject.SetActive(!isInbag);
        isLock.SetActive(equip.isLock_);
        isBind.SetActive(equip.isBind_);
		iszhuangbei.SetActive (IsequipInbaby((int)equip.instId_));
		UIManager.Instance.AddItemCellUI (backicon, equip.itemId_);
       // HeadIconLoader.Instance.LoadIcon(data.icon_, itemIcon);
        iconCache = data.icon_;
        SkillData skill = SkillData.GetData(equip.durability_, equip.durabilityMax_);
        skillName.text = skill == null? "": skill._Name;

        int idx = 0;
        for (; idx < equip.propArr.Length; ++idx)
        {
            GameObject go = null;
            if (idx >= propPool.Count)
            {
                go = (GameObject)GameObject.Instantiate(propItem) as GameObject;
                go.transform.parent = grid.transform;
                go.transform.localScale = Vector3.one;
                propPool.Add(go);
            }
            else
            {
                go = propPool[idx];
            }
            UILabel[] lbl = go.GetComponentsInChildren<UILabel>(true);
            if (lbl.Length > 0)
            {
                lbl[0].text = string.Format("{0}: {1}", LanguageManager.instance.GetValue(equip.propArr[idx].type_.ToString()), equip.propArr[idx].value_);
                go.SetActive(true);
            }
        }

        for (; idx < propPool.Count; ++idx)
        {
            propPool[idx].SetActive(false);
        }
        grid.repositionNow = true;
    }

	bool IsequipInbaby(int insid)
	{
		COM_Item [] beqs = GamePlayer.Instance.GetBabyInst ((int)targetId).Equips;
		for(int i=0;i< beqs.Length;i++)
		{
			if(beqs[i] == null)continue;
			if(beqs[i].instId_ == insid)
			{
				return true;
			}
		}
		return false;
	}
    public void SetData(COM_Item equipInBody)
    {
        ItemData data = ItemData.GetData((int)equipInBody.itemId_);
        itemName.text = data.name_;
        itemDesc.text = data.desc_;
        itemLv.text = data.level_.ToString();
        itemKind.text = LanguageManager.instance.GetValue(data.mainType_.ToString());
        itemGainWay.text = data.acquiringWay_;
        equipBtn.gameObject.SetActive(false);
        takeoffBtn.gameObject.SetActive(false);
        isLock.SetActive(equipInBody.isLock_);
        isBind.SetActive(equipInBody.isBind_);
		UIManager.Instance.AddItemCellUI (backicon, equipInBody.itemId_);
       // HeadIconLoader.Instance.LoadIcon(data.icon_, itemIcon);
        iconCache = data.icon_;
        SkillData skill = SkillData.GetData(equipInBody.durability_, equipInBody.durabilityMax_);
        skillName.text = skill == null ? "" : skill._Name;

        int idx = 0;
        for (; idx < equipInBody.propArr.Length; ++idx)
        {
            GameObject go = null;
            if (idx >= propPool.Count)
            {
                go = (GameObject)GameObject.Instantiate(propItem) as GameObject;
                go.transform.parent = grid.transform;
                go.transform.localScale = Vector3.one;
                propPool.Add(go);
            }
            else
            {
                go = propPool[idx];
            }
            UILabel[] lbl = go.GetComponentsInChildren<UILabel>(true);
            if (lbl.Length > 0)
            {
                lbl[0].text = string.Format("{0}: {1}", LanguageManager.instance.GetValue(equipInBody.propArr[idx].type_.ToString()), equipInBody.propArr[idx].value_);
                go.SetActive(true);
            }
        }

        for (; idx < propPool.Count; ++idx)
        {
            propPool[idx].SetActive(false);
        }
        grid.repositionNow = true;
    }

    private void OnOperate(ButtonScript obj, object args, int param1, int param2)
    {
        if (targetId == 0 || instId == 0)
            return;

        if(param1 == 0)
		{
            NetConnection.Instance.wearEquipment(targetId, instId);
		}
        else
		{
			if(BagSystem.instance.BagIsFull())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("beibaokongjianbuzu"), PopText.WarningType.WT_Warning);
			}else
			{
				NetConnection.Instance.delEquipment(targetId, instId);
			}

		}
           

        BabyEquipTips bet = null;
        Transform par = gameObject.transform.parent;
        if (par != null)
        {
            bet = par.GetComponent<BabyEquipTips>();
            if(bet != null)
                bet.CloseTip();
        }
    }

    void OnEnable()
    {
        UIManager.SetButtonEventHandler(equipBtn.gameObject, EnumButtonEvent.OnClick, OnOperate, 0, 0);
        UIManager.SetButtonEventHandler(takeoffBtn.gameObject, EnumButtonEvent.OnClick, OnOperate, 1, 0);
    }

    void OnDisable()
    {
        UIManager.RemoveButtonEventHandler(equipBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(takeoffBtn.gameObject, EnumButtonEvent.OnClick);
        HeadIconLoader.Instance.Delete(iconCache);
        iconCache = "";
        instId = 0;
        targetId = 0;
    }
}
