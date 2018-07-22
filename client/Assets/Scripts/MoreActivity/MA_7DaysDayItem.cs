using UnityEngine;
using System.Collections;

public class MA_7DaysDayItem : MonoBehaviour {

    public UILabel days_;
    public GameObject selected_;
	public UITexture icon;
	public UISprite haveImg;
	public UISprite blackImg;
    public void SetData(int day, int trueDay)
    {
        string val;
        switch(day)
        {
            case 1:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigOne"));
                break;
            case 2:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigTwo"));
                break;
            case 3:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigThree"));
                break;
            case 4:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigFour"));
                break;
            case 5:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigFive"));
                break;
            case 6:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigSix"));
                break;
            case 7:
                val = string.Format(LanguageManager.instance.GetValue("HowDay"), LanguageManager.instance.GetValue("BigSeven"));
                break;
            default:
                val = LanguageManager.instance.GetValue("Wait4Open");
				
                break;
        }
        days_.text = val;
		HeadIconLoader.Instance.LoadIcon ("day" + trueDay, icon);
		if(day ==0)
		{
			blackImg.gameObject.SetActive(true);
			haveImg.gameObject.SetActive (true);
			haveImg.spriteName = "weikaiqi";
		}
		else
		{
			blackImg.gameObject.SetActive(false);
			haveImg.gameObject.SetActive (false);
		}
    }

    public void Select(bool select)
    {
        selected_.SetActive(select);
    }

	public void IsGet(bool b)
	{
		if(b)
		{
			blackImg.gameObject.SetActive(false);
			haveImg.gameObject.SetActive (b);
			haveImg.spriteName = "yilingqu";
		}
	}
}
