using UnityEngine;
using System.Collections;

public class CombatPanel : UIBase {
	public UIButton SkillB;
	public UIButton PacksackB;
	void Start () {

		UIManager.SetButtonEventHandler (SkillB.gameObject, EnumButtonEvent.OnClick, OnClickSkillB, 0, 0);
		UIManager.SetButtonEventHandler (PacksackB.gameObject, EnumButtonEvent.OnClick, OnClickPacksackB, 0, 0);
	}


	private void OnClickSkillB(ButtonScript obj, object args, int param1, int param2)
	{

	}
	private void OnClickPacksackB(ButtonScript obj, object args, int param1, int param2)
	{
		
	}


	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

}
