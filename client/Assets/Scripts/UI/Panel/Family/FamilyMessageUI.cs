using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FamilyMessageUI : UIBase {

	public GameObject item;
	public UIGrid grid;
	private List<GameObject> itemlist = new List<GameObject>();
	void Start () {
		item.SetActive (false);
		//addItem (GuildSystem.historyMessage);
	}
	public void addItem(List<string> mes)
	{
		
		for(int i =0;i<mes.Count;i++)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			FamilyShopCell fcell = o.GetComponent<FamilyShopCell>();
			o.SetActive(true);
			o.transform.parent = grid.transform;
			o.transform.localScale= new Vector3(1,1,1);	
			string []str = mes[i].Split(';');
			UILabel [] las = o.GetComponentsInChildren<UILabel>();
			for(int j =0;j< las.Length;j++)
			{
				if(las[j].name.Equals("TimeLabel"))
				{
					las[j].text = str[0];
				}
				if(las[j].name.Equals("jiluLabel"))
				{
					las[j].text = str[1];
				}
			}
			itemlist.Add(o);
			grid.repositionNow = true;
		}
	}
	void OnEnable()
	{
		if (grid == null)
						return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
		addItem (GuildSystem.historyMessage);
	}
	void OnDisable()
	{
		
	}
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilMesagePanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilMesagePanel);
	}
	public static void HideMe()
	{
		
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilMesagePanel);
	}
	public override void Destroyobj ()
	{

	}
}
