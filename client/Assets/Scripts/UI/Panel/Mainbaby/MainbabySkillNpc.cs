using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabySkillNpc : UIBase {


	public UILabel _SkillTitle;

	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;

	private Dictionary<int, NpcData> metad;
	private List<NpcData> listNdata = new List<NpcData>();
	void Start () {
		_SkillTitle.text = LanguageManager.instance.GetValue("kill_Title");

		item.SetActive (false);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject,EnumButtonEvent.OnClick,OnclickClose,0,0);
		metad = NpcData.GetData ();
		foreach (NpcData npd in metad.Values)
		{
			if(npd.AssetsId == UIASSETS_ID.UIASSETS__BabySkillLearning)
			{
				listNdata.Add(npd);
			}
		}
		InitItem (listNdata);
		this.gameObject.transform.localPosition = new Vector3 (0, 0, -2000);
	}
	void InitItem(List<NpcData> ndts)
	{
		//CloseItem ();
		for(int i =0;i<ndts.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(item)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid.transform;
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			NpcCell nCell = clone.GetComponent<NpcCell>();
			nCell.Npcdata = ndts[i];

		}
		grid.Reposition ();
	}
	void OnclickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
		//gameObject.SetActive (false);
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BabySkillLearningNpcPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BabySkillLearningNpcPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_BabySkillLearningNpcPanel);
	}

	public override void Destroyobj ()
	{
//		if(MainbabyUI.UpdateTabelBtnStateOk != null)
//		{
//			MainbabyUI.UpdateTabelBtnStateOk();
//		}
	}
}
