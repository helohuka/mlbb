using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabyList : MonoBehaviour {

	public static int babyId;
	public GameObject skillListObj;
	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;
	private List<Baby> babylist;
	private List<GameObject>itemsList = new List<GameObject>();
	void Start () {
		item.SetActive (false);
		babylist =	GamePlayer.Instance.babies_list_;
		AddItems (babylist);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLearningSkill_BabyListUI);
    }
	public void AddItems(List<Baby> Entitylist)
	{
		for (int i = 0; i<Entitylist.Count; i++) {
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			BabyCell mbCell = o.GetComponent<BabyCell>();
			mbCell.BabyMainData = Entitylist[i];
			o.transform.localPosition = Vector3.zero;
			o.transform.localScale= new Vector3(1,1,1);	
			grid.repositionNow = true;
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickDoSkill, Entitylist[i].InstId, 0);
			itemsList.Add(o);
            if (i == 0)
                GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_FirstLearningBabySkill_BabyList);
		}
//		if(itemsList != null && itemsList.Count > 0)
//			buttonClick (itemsList[0]);
	}
	void OnClickDoSkill(ButtonScript obj, object args, int param1, int param2)
	{
		babyId = param1;
	    Baby mbaby =GamePlayer.Instance.GetBabyInst (param1);
		if (IsContainsSkillId (mbaby))
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("babyAlreadyLearnedSkill"), PopText.WarningType.WT_Warning);	
		}else
		{
			skillListObj.SetActive(true);
		}
		if(BabySkill.RefreshbabyData !=null)
		{
			BabySkill.RefreshbabyData(babyId);
		}


	}
	bool IsContainsSkillId(Baby cbaby)
	{
		for(int i =0;i<cbaby.SkillInsts.Count;i++)
		{
			if(BabySkillLearning.SkillId == cbaby.SkillInsts[i].skillID_)
			{
				return true;
			}
		}
		return false;
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
