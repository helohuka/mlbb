using UnityEngine;
using System.Collections;

public class MoreSignInCellUI : MonoBehaviour
{

	public UISprite icon;
	public UISprite icon1;
	public UISprite icon2;
	public UILabel descLab;
	public UIButton sgignInBtn;
	public UISprite haveImg;
	public UILabel BtnLab;

	void Start ()
	{
		BtnLab.text = LanguageManager.instance.GetValue ("Success_SuccessLQ");
	}


}

