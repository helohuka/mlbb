using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroEquipUI : MonoBehaviour
{
	public List<UISprite> equipList = new List<UISprite> ();
	public UISprite equipCell;
	//
	private COM_Item[] equips = new COM_Item[8];



	void Start ()
	{
		equips = GamePlayer.Instance.Equips;


	}
	


}

