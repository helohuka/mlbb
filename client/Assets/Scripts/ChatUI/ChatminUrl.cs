using UnityEngine;
using System.Collections;

public class ChatminUrl : MonoBehaviour {

	public GameObject tipsObj;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick ()
	{
		
		UILabel lbl = GetComponent<UILabel>();
		if (lbl != null)
		{
			string url = lbl.GetUrlAtPosition(UICamera.lastHit.point);
			
			if (!string.IsNullOrEmpty(url))
			{				
				string [] typs = url.Split(',');
				if(int.Parse(typs[0])==4)
				{
					if(int.Parse(typs[1]) == GamePlayer.Instance.InstId)
					{
						return;
					}
					tipsObj.SetActive(true);
					HaoyouShezhi hs = tipsObj.GetComponent<HaoyouShezhi>();
					hs.insetId = uint.Parse(typs[1]);
					hs.name = typs[2];
				}

			}
		}
	}
}
