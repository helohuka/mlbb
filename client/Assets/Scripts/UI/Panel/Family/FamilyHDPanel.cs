using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FamilyHDPanel : UIBase {

	public GameObject item;
	public UIGrid grid;
	public UILabel title;
	void Start () {
		title.text = LanguageManager.instance.GetValue ("familyAictive");
		item.SetActive (false);
        List<DaliyActivityData> datas = DaliyActivityData.GetDatas(new ActivityType[] { ActivityType.ACT_Family_0, ActivityType.ACT_Family_1, ActivityType.ACT_Family_2, ActivityType.ACT_Family_3, ActivityType.ACT_Family_4, });
		addItem (datas);
	}

	void addItem(List<DaliyActivityData> datas)
	{
		for(int i =0;i<datas.Count;i++)
		{
			GameObject go = GameObject.Instantiate(item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			FamilyHDcell fcell = go.GetComponent<FamilyHDcell>();
			fcell.Data = datas[i];
			grid.repositionNow = true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyHD);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyHD);
	}
	public static void HideMe()
	{
		
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyHD);
	}
	public override void Destroyobj ()
	{

	}
}
