using UnityEngine;
using System.Collections.Generic;

public class SkillViewTwo : UIBase {
	public static SkillViewTwo scrollViewT;
	public UIGrid grid;
	public GameObject item;
	public GameObject ScrollView;
	public GameObject CloseBtn;
	private List<GameObject> items = new List<GameObject> ();
	UIEventListener Listener;
	private List<int> itemIDs = new List<int>();
	private List<int> itemSkillIds = new List<int>();
	private List<SkillData> SkillDat = new List<SkillData> ();
	AttaclPanel attackPanel;
	void Awake()
	{
		scrollViewT = this;
	}
	public static SkillViewTwo Instance{
		get{
			return scrollViewT;	
		}
	}


	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	void Start () {
        //GetComponent<UIPanel> ().depth = 8;
        //attackPanel = GameObject.Find ("AttackPanel").GetComponent<AttaclPanel>();
        //item.SetActive (false);
        //grid = gameObject.GetComponentInChildren<UIGrid> ();
        //itemIDs = ScrollViewPanel.Instance.skillIds;
	
        //for (int i = 0; i<itemIDs.Count; i++) {
        //    SkillData sdata = SkillData.GetData(itemIDs[i]);	
        //    SkillDat.Add(sdata);
        //}

        //for(int j =0 ;j<SkillDat.Count-1;j++)
        //{
        //    for(int k =SkillDat.Count-1;k>j;k-- )
        //    {
        //        if(string.Compare(SkillDat[k].name_,SkillDat[j].name_)<0)
        //        {
        //            SkillData skd = SkillDat[k];
        //            SkillDat[k] = SkillDat[j];
        //            SkillDat[j] = skd;
        //        }
        //    }

        //}
		
        //AddScrollViewItem (SkillDat);
        //UIEventListener.Get (CloseBtn).onClick = CloseClick;
        //AttaclEvent.getInstance.RefreshSkillEvent = RefreshData;

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_SelectSkillLevel);
	}

	void CloseClick(GameObject sender)
	{
		AttaclPanel.Instance.closeSkillTwoWindow ();
	}
	public void  RefreshData()
	{
        //for(int i = 0;i<items.Count; i++)
        //{
        //    GameObject.Destroy(items[i]);
        //}
        //items.Clear ();
        //SkillDat.Clear ();
        //itemIDs = ScrollViewPanel.Instance.skillIds;
        //for (int i = 0; i<itemIDs.Count; i++) {
        //    SkillData sdata = SkillData.GetData(itemIDs[i]);	
        //    SkillDat.Add(sdata);
        //}
		
        //for(int j =0 ;j<SkillDat.Count-1;j++)
        //{
        //    for(int k =SkillDat.Count-1;k>j;k-- )
        //    {
        //        if(string.Compare(SkillDat[k].name_,SkillDat[j].name_)<0)
        //        {
        //            SkillData skd = SkillDat[k];
        //            SkillDat[k] = SkillDat[j];
        //            SkillDat[j] = skd;
        //        }
        //    }
			
        //}
        //AddScrollViewItem (SkillDat);
		
	}

	List<string>names = new List<string>();
	void AddScrollViewItem(/*List<int> datas*/List<SkillData> datas)
	{
		if (item == null)
			return;
		for (int i = 0; i<datas.Count; i++) {
			GameObject o  =(GameObject) Instantiate(item);
			o.SetActive(true);
			o.transform.parent = grid.transform;
			UILabel[] shps = o.GetComponentsInChildren<UILabel>();
            foreach(UILabel la in shps)
			{
				if(la.gameObject.name.Equals("nameLabel"))
				{
					la.text =datas[i]._Name;
				}
				if(la.gameObject.name.Equals("levelLabel"))
				{
					la.text =datas[i]._Level.ToString();
				}
				if(la.gameObject.name.Equals("haomoLabel"))
				{
					la.text = datas[i]._Cost_mana.ToString();
				}
			}
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			Listener=UIEventListener.Get(o);
			Listener.onClick +=buttonClick;
			Listener.parameter = datas[i]._Id;
			items.Add(o);
            if (i == 0)
                GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_FirstLevelSkill);
		}
		grid.repositionNow = true;
	}
	void buttonClick(GameObject sender)
	{
		int skillId = (int)UIEventListener.Get (sender).parameter;
        if (AttaclEvent.getInstance.SkillShowEvent != null)
            AttaclEvent.getInstance.SkillShowEvent(skillId);
        CloseTwoWindow();
		AttaclPanel.Instance.SetAllButtonVisible(false);
		AttaclPanel.Instance.SetPlayerBackBtnVisible(true);
	}

	void CloseTwoWindow()
	{
		attackPanel.closeSkillTwoWindow ();
		attackPanel.closeSkillWindow ();
	}
}
