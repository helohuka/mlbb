using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPvpPlayerCellUI : MonoBehaviour
{
	public UILabel namelab;
	public UILabel levelLab;
	public UILabel professionLab;
	public UITexture icon; 
	public UISprite icomBack;
	public UISprite nobody;
	public UISprite infoImg;
	public UISprite professionImg;

	private bool isRoll;
	private List<string> _icons = new List<string>();
	private string[] iconNames ={"boyIcon01_icon","boyIcon02_icon","boyIcon03_icon","boyIcon02_icon","girl02_icon","girl03_icon","girl03_icon","boy01_icon"};
	void Start () 
	{

	}


	public void StartRoll()
	{
		icon.gameObject.SetActive (true);
		StartCoroutine (DelayAction (0.2f));
	}

	public void StopRoll()
	{
		//StopCoroutine ("DelayAction");
		StopAllCoroutines ();
		icon.gameObject.SetActive (false);
	}

	IEnumerator DelayAction(float dTime)
	{
		while(true)
		{
			yield return new WaitForSeconds(dTime);
			int num = Random.Range(0,iconNames.Length)%iconNames.Length;
			HeadIconLoader.Instance.LoadIcon(iconNames[num],icon);

			if(!_icons.Contains(iconNames[num]))
			{
				_icons.Add(iconNames[num]);
			}
		}
	}


	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
	
	
	
}


