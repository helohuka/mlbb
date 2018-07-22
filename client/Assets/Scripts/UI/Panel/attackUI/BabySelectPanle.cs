using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabySelectPanle : MonoBehaviour {

	public GameObject item;
	public UIButton RecycleBtn;
	public GameObject closeBtn;

	UIGrid grid;
    List<Baby> displayList_;
	UIEventListener listener_;
    bool initialized_;

	void Start () {

        displayList_ = new List<Baby>();
		item.SetActive (false);
		grid = gameObject.GetComponentInChildren<UIGrid> ();
		UIPanel pan = gameObject.GetComponent<UIPanel> ();
		pan.depth = 19;
		UIEventListener.Get (closeBtn).onClick = OnClose;
		UIManager.SetButtonEventHandler (RecycleBtn.gameObject, EnumButtonEvent.OnClick, OnClickRecycle, 0, 0);

        UpdateUI();
        initialized_ = true;
	}

    void OnEnable()
    {
        if (initialized_)
            UpdateUI();
    }

    /// <summary>
    /// 
    /// </summary>

    void UpdateUI()
    {
        displayList_.Clear();
        for (int i = 0; i < GamePlayer.Instance.babies_list_.Count; ++i)
        {
            if (GamePlayer.Instance.babies_list_[i].isDead)
                continue;

            if (GamePlayer.Instance.BattleBaby != null &&
                (GamePlayer.Instance.BattleBaby.InstId == GamePlayer.Instance.babies_list_[i].InstId))
                continue;

            displayList_.Add(GamePlayer.Instance.babies_list_[i]);
        }
        UpdateObj();
        UpdateData();
    }

    void UpdateObj()
    {
        int objCount = grid.transform.childCount;
        int validBaby = displayList_.Count;

        int more = validBaby - objCount;
        for (int i = 0; i < more; ++i)
        {
            GameObject o = GameObject.Instantiate(item) as GameObject;
            o.SetActive(true);
            o.name = o.name + i;
            o.transform.parent = grid.transform;
            o.transform.localPosition = new Vector3(0, 0, 0);
            o.transform.localScale = new Vector3(1, 1, 1);
            listener_ = UIEventListener.Get(o);
            listener_.parameter = displayList_[i].InstId;
            listener_.onClick += OnBabyClick;
        }
        if (more <= 0)
        {
            for (int i = objCount - 1; i >= objCount - Mathf.Abs(more); --i)
            {
                grid.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < displayList_.Count; ++i)
        {
            grid.transform.GetChild(i).gameObject.SetActive(true);
            listener_ = UIEventListener.Get(grid.transform.GetChild(i).gameObject);
            listener_.parameter = displayList_[i].InstId;
        }

        grid.repositionNow = true;
    }

    void UpdateData()
    {
        for (int i = 0; i < displayList_.Count; ++i)
        {
            BabySelectCell bsCell = grid.transform.GetChild(i).GetComponent<BabySelectCell>();
            bsCell.Mbaby = displayList_[i];
        }
    }

	private void OnClickRecycle(ButtonScript obj, object args, int param1, int param2)
	{
        if (GamePlayer.Instance.BattleBaby == null)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("NoBabyRecall"));
            return;
        }
        
		if(AttaclEvent.getInstance.BabyEvent!= null)
            AttaclEvent.getInstance.BabyEvent(GamePlayer.Instance.BattleBaby.InstId);

        BattleActor babyInBattle = Battle.Instance.GetActorByInstId(GamePlayer.Instance.BattleBaby.InstId);
        if (babyInBattle.isDead == false)
        {
            Baby baby = new Baby();
            baby.Resize();
            baby.InstId = babyInBattle.InstId;
            baby.SetIprop(PropertyType.PT_TableId, babyInBattle.battlePlayer.tableId_);
            baby.SetIprop(PropertyType.PT_Level, babyInBattle.battlePlayer.level_);
            baby.SetIprop(PropertyType.PT_HpCurr, babyInBattle.battlePlayer.hpCrt_);
            baby.SetIprop(PropertyType.PT_MpCurr, babyInBattle.battlePlayer.mpCrt_);
            baby.SetIprop(PropertyType.PT_HpMax, babyInBattle.battlePlayer.hpMax_);
            baby.SetIprop(PropertyType.PT_MpMax, babyInBattle.battlePlayer.mpMax_);
            displayList_.Add(baby);
        }

		AttaclPanel.Instance.CloseBabyWindow();
	}

	void OnClose(GameObject sender)
	{
		AttaclPanel.Instance.CloseBabyWindow ();
	}

	void OnBabyClick(GameObject sender)
	{
		int instd = (int) UIEventListener.Get (sender).parameter;

		int babylevel = GamePlayer.Instance.GetBabyInst (instd).GetIprop (PropertyType.PT_Level);
		int playerLevel = GamePlayer.Instance.GetIprop (PropertyType.PT_Level);
		if(babylevel - playerLevel>5)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("babylevel"));
		}else
		{
			if (AttaclEvent.getInstance.BabyEvent != null)
				AttaclEvent.getInstance.BabyEvent(instd);
		}

        

	}
}
