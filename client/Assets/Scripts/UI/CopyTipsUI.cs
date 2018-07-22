using UnityEngine;
using System.Collections;

public class CopyTipsUI : UIBase {

	public GameObject item;
	public UIGrid grid;
	public UIButton Close;
	void Start () {
		item.SetActive (false);
		UIManager.SetButtonEventHandler (Close.gameObject, EnumButtonEvent.OnClick, onClickClose,0,0);
		AddItem ();
	}
	void AddItem()
	{
		for(int i =0;i<TeamSystem.GetTeamMembers().Length;i++)
		{
			GameObject go = Instantiate(item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			UITexture tex = go.GetComponent<UITexture>();

			PlayerData pdata = PlayerData.GetData ((int)TeamSystem.GetTeamMembers()[i].properties_[(int)PropertyType.PT_TableId]);
			EntityAssetsData enData = EntityAssetsData.GetData (pdata.lookID_);
			HeadIconLoader.Instance.LoadIcon (enData.assetsIocn_, tex);
			UILabel [] las = go.GetComponentsInChildren<UILabel>();
			for(int j =0;j<las.Length;j++)
			{
				if(las[j].name.Equals("duizhanLabel"))
				{
					las[j].text = TeamSystem.GetTeamMembers()[i].instName_;
				}
				if(las[j].name.Equals("zhiyeLabel"))
				{
					las[j].text = Profession.get ((JobType)TeamSystem.GetTeamMembers()[i].properties_[(int)PropertyType.PT_Profession],(int)TeamSystem.GetTeamMembers()[i].properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_;
				}
				if(las[j].name.Equals("dengjiLabel"))
				{
					las[j].text = TeamSystem.GetTeamMembers()[i].properties_[(int)PropertyType.PT_Level].ToString();
				}
				if(las[j].name.Equals("titleLabel"))
				{
					if(TeamSystem.IsTeamLeader((int)TeamSystem.GetTeamMembers()[i].instId_))
					{
						las[j].text = LanguageManager.instance.GetValue("duizhang");
					}else
					{
						las[j].text = LanguageManager.instance.GetValue("duiyuan");
					}

				}
			}
		}
	}
	private void onClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.exitCopy();
		Hide ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_copyTipsPanel);
	}
	public static void SwithShowMe()
	{

		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_copyTipsPanel);
	}
	public static void HideMe()
	{
		
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_copyTipsPanel);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public override void Destroyobj ()
	{

	}
}
