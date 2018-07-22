using UnityEngine;
using System.Collections;

static public class WishingTreeSystem {
	public delegate void ShowWish(COM_Wish Wish);
	public static event ShowWish ShowWishOk;
	public static COM_Wish _Wish;
	public static void ShareWish(COM_Wish Wish)
	{
		_Wish = Wish;
		if(ShowWishOk != null)
		{
			ShowWishOk(Wish);
		}
	}
	public static void WishOk()
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue ("WishOk"));
	}
}
