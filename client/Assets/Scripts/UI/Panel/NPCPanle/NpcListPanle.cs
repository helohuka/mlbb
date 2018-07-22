using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NpcListPanle : UIBase {

	public UILabel titleLable;
	public UILabel tishiLable;
	public UIButton CloseBtn;
	public UIGrid grid;
	public GameObject item;
	void Start () {
		item.SetActive (false);
		titleLable.text = LanguageManager.instance.GetValue ("NpcTitle");
		tishiLable.text = LanguageManager.instance.GetValue ("NpcTishi");
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		AddItem (Prebattle.Instance.npcContainer_);
	}
	   
	void AddItem(List<Npc> npcs)
	{
		for(int i=0;i< npcs.Count;i++)
		{
			GameObject go = GameObject.Instantiate (item)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			go.transform.localScale = Vector3.one;
			UIManager.SetButtonEventHandler (go, EnumButtonEvent.OnClick, OnClickButton,npcs[i].npcId_, 0);
			UILabel nameLable = go.GetComponentInChildren<UILabel>();
			nameLable.text =NpcData.GetData(npcs[i].npcId_).Name;
			grid.repositionNow = true;
		}


	}
	private void OnClickButton(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.moveToNpc (param1);
		Hide ();
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_NpcLPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_NpcLPanel);
	}
	public static void HideMe()
	{
		//ross.Clear();
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_NpcLPanel);
	}
	public override void Destroyobj ()
	{

	}
}
