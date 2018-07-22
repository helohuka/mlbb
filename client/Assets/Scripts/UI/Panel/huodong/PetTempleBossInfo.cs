using UnityEngine;
using System.Collections.Generic;

public class PetTempleBossInfo : MonoBehaviour {

    public delegate void SelectBossHandler(int idx);
    public event SelectBossHandler OnSelect;

    public UISprite side_;

    public UILabel name_;

    public UILabel desc_;

    public Transform skillGrid_;

    public Transform modelRoot_;

    public GameObject disableMask_;

    public UILabel timeBegin_;

    int index_;

    bool destroyed_;

    public PetActivityData padata_;

    List<GameObject> itemPool_;

	// Use this for initialization
	void Start () {
        destroyed_ = false;
	}

    void OnEnable()
    {
        side_.spriteName = "jn_jinlan";
    }

    public void SetData(PetActivityData data, int index)
    {
        if (itemPool_ == null)
            itemPool_ = new List<GameObject>();

        padata_ = data;
        BabyData baby = BabyData.GetData(data.monsterID_);
        if (baby != null)
        {
            name_.text = baby._Name;
            desc_.text = baby._Desc;
            GameManager.Instance.GetActorClone((ENTITY_ID)baby._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, (GameObject go, ParamData pData) =>
            {
                if (destroyed_)
                {
                    Destroy(go);
                    return;
                }
                go.transform.parent = modelRoot_;
                go.transform.localScale = Vector3.one * EntityAssetsData.GetData(pData.iParam).zoom_ * 0.7f;
                go.transform.localPosition = new Vector3(0f, 0f, -150f);
                go.transform.Rotate(Vector3.up, 180f);
                disableMask_.transform.localPosition = new Vector3(0f, 0f, go.transform.localPosition.z * 2f);
            }, new ParamData(baby._AssetsID), "UI");
        }

        string[] skills = data.skillIDs_;
        SkillData skill = null;
        GameObject skillGo = null;
        SkillCellUI icon = null;
        for (int i = 0; i < skills.Length; ++i)
        {
            skill = SkillData.GetMinxiLevelData(int.Parse(skills[i]));
            if (skill == null)
                continue;

            if (i >= itemPool_.Count)
            {
                skillGo = new GameObject("Skill_" + skill._Id);
                skillGo.AddComponent<UISprite>().depth = 0;
                icon = UIManager.Instance.AddSkillCellUI(skillGo.GetComponent<UISprite>(), skill, 0f, 0f, 0.7f);
                icon.showTips = true;
                skillGo.transform.parent = skillGrid_.transform;
                skillGo.transform.localScale = Vector3.one;
                itemPool_.Add(skillGo);
            }
            else
            {
                itemPool_[i].name = "Skill_" + skill._Id;
                icon = UIManager.Instance.AddSkillCellUI(itemPool_[i].GetComponent<UISprite>(), skill);
                icon.showTips = true;
            }
        }
        skillGrid_.GetComponent<UIGrid>().Reposition();

        bool todayOpen = false;
        string desc = "";
        for (int i = 0; i < data.openTimeDesc_.Length; ++i)
        {
            desc += "周" + ConvertUpper(int.Parse(data.openTimeDesc_[i]));
            int compareDay = int.Parse(data.openTimeDesc_[i]);
            if (compareDay == 7)
                compareDay = 0;
            if ((int)System.DateTime.Today.DayOfWeek == compareDay)
            {
                todayOpen = true;
            }
        }
        desc += "开启";
        timeBegin_.text = string.Format("[b]{0}[-]", desc);
        disableMask_.SetActive(!todayOpen);
        
        index_ = index;
    }


    public void ResetSelection()
    {
        side_.spriteName = "jn_jinlan";
    }

    void OnClick()
    {
        side_.spriteName = "xuanzhong";
        if (OnSelect != null)
            OnSelect(index_);
    }

    void OnDestroy()
    {
        destroyed_ = true;
    }

    string ConvertUpper(int num)
    {
        switch(num)
        {
            case 1:
                return "一";
            case 2:
                return "二";
            case 3:
                return "三";
            case 4:
                return "四";
            case 5:
                return "五";
            case 6:
                return "六";
            case 0:
                return "日";
            default:
                return "";
        }
    }
}
