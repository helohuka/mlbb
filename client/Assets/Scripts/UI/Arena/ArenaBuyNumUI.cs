using UnityEngine;
using System.Collections;

public class ArenaBuyNumUI : MonoBehaviour
{
	public UIPanel  pane;
	public UIButton closeBtn;
	public UIButton addBtn;
	public UIButton minusBtn;
	public UILabel  buyNumLab;


	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		pane.gameObject.SetActive (false);
		//ApplicationEntry.Instance.ui3DCamera.depth = 1;
	}
}

