using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyOpenUI : UIBase
{

	public UIGrid grid;
	public GameObject cell;
	public GameObject noOpenCell;
	public UIButton clostBtn;
	public UILabel titleLab;
	public UILabel descLab;

	void Start ()
	{
		UIManager.SetButtonEventHandler(clostBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);

		Dictionary<int, CopyData> metaData = CopyData.GetData();
		foreach(var x in metaData.Values)
		{
			GameObject objCell = Object.Instantiate(cell.gameObject) as GameObject;
			CopyOpenCellUI cellUI = objCell.GetComponent<CopyOpenCellUI>();
			cellUI.cellData = x;
			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
		}
		noOpenCell.transform.parent = grid.transform;
		noOpenCell.SetActive(true);
		noOpenCell.transform.localScale = Vector3.one;
	}


	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_CopyOpenPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_CopyOpenPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_CopyOpenPanel);
	}
	
	public override void Destroyobj()
	{
		GameObject.Destroy(gameObject);
	}
	
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation (this.panel, () => {
			Hide ();
		});
	}
	


}

