using UnityEngine;
using System.Collections.Generic;

public class HelpLevelUI : MonoBehaviour {

    public UIGrid grid;
    public GameObject itemPre_;
    public UIToggle checkMark_;

    List<GameObject> itemPool_ = new List<GameObject>();
	// Use this for initialization
	void Start () {
        checkMark_.value = !GamePlayer.Instance.nextFuncClose_;
	}

    void OnEnable()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        Dictionary<int, HelpLevelData> allData = HelpLevelData.GetMetaData();
        int i = 0;
        GameObject item;
        HelpLevelItem hlItem;
        string check = string.Empty;
        foreach (HelpLevelData hlData in allData.Values)
        {
            bool first = false;
            if (i >= itemPool_.Count)
            {
                item = (GameObject)GameObject.Instantiate(itemPre_) as GameObject;
                item.transform.parent = grid.transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetActive(true);
                itemPool_.Add(item);
                first = true;
            }
            else
                item = itemPool_[i];

            hlItem = item.GetComponent<HelpLevelItem>();
            if (first)
            {
                if (!string.IsNullOrEmpty(hlData.openui))
                    check = string.Format("{0}>{1}", "ui", hlData.openui);
                else if (!string.IsNullOrEmpty(hlData.npc))
                    check = string.Format("{0}>{1}", "npc", hlData.npc);
                else
                    check = "";
                hlItem.SetData(hlData.icon, hlData.level, hlData.desc, check);
            }
            hlItem.UpdateLock(hlData.level > GamePlayer.Instance.GetIprop(PropertyType.PT_Level));
            i++;
        }

        grid.Reposition();
    }

    public void OnVisableCheck()
    {
        GamePlayer.Instance.nextFuncClose_ = !checkMark_.value;
        PlayerPrefs.SetInt(GamePlayer.Instance.InstId.ToString() + "nextFuncClose", checkMark_.value ? 0 : 1);
        GamePlayer.Instance.levelIsDirty_ = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
