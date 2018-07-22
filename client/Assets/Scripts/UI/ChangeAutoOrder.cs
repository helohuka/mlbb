using UnityEngine;
using System.Collections.Generic;

public class ChangeAutoOrder : MonoBehaviour {

    public UIButton attackBtn;
    public UIButton defBtn;
    public UIButton fleeBtn;
    public UIButton closeBtn;

    public UISprite iconBg_;

    public Transform grid_;
    public GameObject item_;

    public OperateType operatType_;

    string resIconName = "";

	// Use this for initialization
	void Start () {
	
	}

    void OnEnable()
    {
        UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
        UIManager.SetButtonEventHandler(attackBtn.gameObject, EnumButtonEvent.OnClick, OnClickattackBtn, 0, 0);
        UIManager.SetButtonEventHandler(defBtn.gameObject, EnumButtonEvent.OnClick, OnClickdefBtn, 0, 0);
        if(fleeBtn != null)
            UIManager.SetButtonEventHandler(fleeBtn.gameObject, EnumButtonEvent.OnClick, OnClickfleeBtn, 0, 0);
        UpdateSkill();

        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_OpenAutoPanel);
        }, 1);
    }

    void UpdateSkill()
    {
        Entity actor = null;
        Profession prof = null;
        switch(operatType_)
        {
            case OperateType.OT_P1:
                actor = GamePlayer.Instance;
                break;
            case OperateType.OT_B:
                actor = GamePlayer.Instance.BattleBaby;
                break;
        }

        bool disable = false;
        if (operatType_ == OperateType.OT_P1)
        {
            if (Battle.Instance.P2SkillId_ != 0 && GamePlayer.Instance.BattleBaby == null)
            {
                if (Battle.Instance.P2SkillId_ != GlobalValue.GetAttackID(GamePlayer.Instance.GetWeaponType()) &&
               Battle.Instance.P2SkillId_ != GlobalValue.SKILL_DEFENSEID &&
               Battle.Instance.P2SkillId_ != GlobalValue.SKILL_FLEEID)
                {
                    disable = true;
                }
            }
            //    AttaclPanel.Instance.SetButtonState(false, AttaclPanel.SKILL_BTN, AttaclPanel.ARTICLE_BTN, AttaclPanel.PET_BTN, AttaclPanel.POSITION_BTN);
        }

        if (actor != null)
        {
            prof = Profession.get((JobType)actor.GetIprop(PropertyType.PT_Profession), actor.GetIprop(PropertyType.PT_ProfessionLevel));
            GameObject go = null;
            SkillData skdata = null;
            bool isProudSkill = false;
            List<COM_Skill> skillList = new List<COM_Skill>(actor.SkillInsts);
            if (actor.suitSkill != null)
                skillList.Add(actor.suitSkill);
			int realidx = 0;
            for (int i = 0; i < skillList.Count; ++i)
            {
                skdata = SkillData.GetData((int)skillList[i].skillID_, (int)skillList[i].skillLevel_);
				if (skdata._SkillType != SkillType.SKT_Active && skdata._SkillType != SkillType.SKT_Passive)
                    continue;

                if (Battle.Instance.disableLst_.Contains(GetSkillIndex(skdata._Id)))
                    continue;
                
                go = GameObject.Instantiate(item_) as GameObject;
				UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, OnClickSkill, skdata._Id, skdata._Level);
                go.transform.parent = grid_;
                go.transform.localScale = Vector3.one;

                UIEventListener.Get(go).parameter = disable ? 0 : 1;

                if(prof == null)
                    isProudSkill = false;
                else
                    isProudSkill = prof.isProudSkill(actor.GetIprop(PropertyType.PT_Profession), skdata._Id, skdata._Level);
				go.GetComponent<ChangeAutoIcon>().SetData(skdata._Id, skdata._Level, isProudSkill);
                go.SetActive(true);

				if(realidx == 0)
					GuideManager.Instance.RegistGuideAim(go, GuideAimType.GAT_FirstAutoSkill);
				realidx++;
            }
            grid_.GetComponent<UIGrid>().Reposition();
        }
    }

    int GetSkillIndex(int id)
    {
        if (Battle.Instance.displayList == null)
            return -1;
        for (int i = 0; i < Battle.Instance.displayList.Length; ++i)
        {
            if (Battle.Instance.displayList[i] == id)
                return i;
        }
        return -1;
    }

    void OnDisable()
    {
        UIManager.RemoveButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(attackBtn.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(defBtn.gameObject, EnumButtonEvent.OnClick);
        if (fleeBtn != null)
            UIManager.RemoveButtonEventHandler(fleeBtn.gameObject, EnumButtonEvent.OnClick);
        for (int i = 0; i < grid_.childCount; ++i)
        {
            Destroy(grid_.GetChild(i).gameObject);
        }
        grid_.DetachChildren();
    }

    public void UpdateIcon(int skillId, int skillLv)
    {
        SkillData skdata = SkillData.GetData(skillId, skillLv);
        //不存在该技能
        if (skdata == null)
        {
            //用普通攻击的图标代替
            skdata = SkillData.GetData(1, 1);
            ClientLog.Instance.Log("没有该技能" + skillId + "   LV: " + skillLv);
            return;
        }

        if (resIconName.Equals(skdata._ResIconName))
            return;

        HeadIconLoader.Instance.Delete(resIconName);
		resIconName = skdata._ResIconName;
        GameObject go = null;
        UITexture tex = null;
        if (iconBg_.transform.childCount == 0)
        {
            go = new GameObject("Icon");
            tex = go.AddComponent<UITexture>();
            tex.depth = iconBg_.depth + 1;
            go.transform.parent = iconBg_.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
        else
        {
            go = iconBg_.transform.GetChild(0).gameObject;
            tex = go.GetComponent<UITexture>();
        }
        HeadIconLoader.Instance.LoadIcon(resIconName, tex);
    }

    private void OnClickSkill(ButtonScript obj, object args, int param1, int param2)
    {
        bool disable = ((int)UIEventListener.Get(obj.gameObject).parameter == 0);
        if (disable)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("NoTwoActiveSkill"), PopText.WarningType.WT_Tip, true);
            return;
        }
        Battle.AutoModule.Instance.ChangeOrder(operatType_, param1);
        UpdateIcon(param1, param2);
        gameObject.SetActive(false);
    }

    private void OnClickattackBtn(ButtonScript obj, object args, int param1, int param2)
    {
        Battle.AutoModule.Instance.ChangeOrder(operatType_, 0);
        UpdateIcon(1, 1);
        gameObject.SetActive(false);
    }

    private void OnClickdefBtn(ButtonScript obj, object args, int param1, int param2)
    {
        Battle.AutoModule.Instance.ChangeOrder(operatType_, GlobalValue.SKILL_DEFENSEID);
        UpdateIcon(GlobalValue.SKILL_DEFENSEID, 1);
        gameObject.SetActive(false);
    }

    private void OnClickfleeBtn(ButtonScript obj, object args, int param1, int param2)
    {
        Battle.AutoModule.Instance.ChangeOrder(OperateType.OT_P1, GlobalValue.SKILL_FLEEID);
        UpdateIcon(GlobalValue.SKILL_FLEEID, 1);
        gameObject.SetActive(false);
    }

    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        gameObject.SetActive(false);
    }

}
