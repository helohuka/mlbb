using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PetPanel : UIBase {
	public GameObject item;

	public UILabel nameLabel;
	public UIGrid grid;
	public UIPanel panel;
	UIEventListener Listener;

    public GameObject Skillitem;

	List<GameObject> itemPool = new List<GameObject>();
	List<SkillData> skillDatas = new List<SkillData>();

    bool isActive = false;
	// Use this for initialization
	void Start () {
		panel.depth = 16;
		nameLabel.text = GamePlayer.Instance.BattleBaby.InstName;
	}
	void UpdateSkillBaby()
	{
		grid = gameObject.GetComponentInChildren<UIGrid> ();

//		nameLabel.text = GamePlayer.Instance.BattleBaby.InstName;
		skillDatas.Clear ();
		List<COM_Skill> skills = GamePlayer.Instance.BattleBaby.SkillInsts;
		List<SkillData> skillData = new List<SkillData>();
		for(int i=0; i < skills.Count; ++i)
		{
			SkillData sData = SkillData.GetMinxiLevelData((int)skills[i].skillID_);
            if (sData._Name.Equals(LanguageManager.instance.GetValue("playerPro_Dodge")))
			{             
				continue;
			}
            if (sData._Name.Equals(LanguageManager.instance.GetValue("playerPro_FightBack")))
			{
				continue;
			}
			skillData.Add(sData);		
		}

		skillDatas.AddRange (skillData);
		GlobalInstanceFunction.Instance.Invoke(() => { AddScrollViewItem (skillData); }, 1);
	}

	void AddScrollViewItem(List<SkillData> datas)
	{
        if (GamePlayer.Instance.BattleBaby != null && GamePlayer.Instance.BattleBaby.suitSkill != null)
        {
            SkillData skill = SkillData.GetData((int)GamePlayer.Instance.BattleBaby.suitSkill.skillID_, (int)GamePlayer.Instance.BattleBaby.suitSkill.skillLevel_);
            datas.Insert(0, skill);
        }
        int idx = 0;
        for (; idx < datas.Count; ++idx)
        {
            GameObject go = null;
            if (idx >= itemPool.Count)
            {
                go = (GameObject)GameObject.Instantiate(Skillitem) as GameObject;
                go.transform.parent = grid.transform;
                go.transform.localScale = Vector3.one;
                itemPool.Add(go);
            }
            else
            {
                go = itemPool[idx];
            }
            babySkillCell cell = go.GetComponent<babySkillCell>();
            cell.clearObj();
            cell.SkData = datas[idx];
            UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, SkillClick, datas[idx]._Id, 0);
            go.SetActive(true);
        }
        for (; idx < itemPool.Count; ++idx)
        {
            itemPool[idx].SetActive(false);
        }
        grid.repositionNow = true;
        if (isActive)
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabySelectSkill);
	}

	void SkillClick(ButtonScript obj, object args, int param1, int param2)
	{
		if(AttaclEvent.getInstance.PetOnEvent!= null)
			AttaclEvent.getInstance.PetOnEvent(param1);
		closeThisWindow ();
		AttaclPanel.Instance.SetBackBtnVisible (true);
	}

	private void closeThisWindow()
	{
		AttaclPanel.Instance.ClosePetWindow();
	}

	void OnEnable()
	{
        isActive = true;
		UpdateSkillBaby ();
	}
	void OnDisable()
	{
        isActive = false;
	}
	public override void Destroyobj ()
	{

	}
}
