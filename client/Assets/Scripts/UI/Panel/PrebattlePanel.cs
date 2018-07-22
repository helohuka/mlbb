using UnityEngine;
using System.Collections;

public class PrebattlePanel : UIBase {

	public GameObject jieSuan;

	public GameObject[] Btns;

	public static PrebattlePanel Prebattle;
	
	void Awake()
	{
		Prebattle = this;
	}
	public static PrebattlePanel Instance
	{
		get
		{
			return Prebattle;	
		}
	}

	// Use this for initialization
	void Start () {
		for (int i =0; i<Btns.Length; i++) {
			UIEventListener.Get (Btns[i]).onClick = btnClick;	
		}
	}

	void btnClick(GameObject button)
	{
		if (button.name == "AUTOButton") {
			if (PrebattleEvent.getInstance.AUTOEvent != null) {
				PrebattleEvent.getInstance.AUTOEvent();
			}	
		}
		else if(button.name == "FanhuiButton")
		{
			if (PrebattleEvent.getInstance.BackEvent != null) {
				PrebattleEvent.getInstance.BackEvent();
			}
		}

	}

	public void ShowClearWindow()
	{
		jieSuan.SetActive(true);
	}
	public void HideClearWindow()
	{
		jieSuan.SetActive(false);
	}


	


	public override void Destroyobj ()
	{
		Destroy (gameObject);
	}
}
