using UnityEngine;
using System.Collections;

public class ProfessionCellUI : MonoBehaviour
{
	public UISprite back;
	public UILabel openlevel;
	public UISprite professionIcon;
	public UILabel professionLab;
	public int jobId;  //professionDataID      

	void Start ()
	{
		Profession prof = Profession.GetData (jobId);
		if (prof == null)
			return;
		if(prof.openLV_ > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
		{
			openlevel.gameObject.SetActive(true);
			openlevel.text =  prof.openLV_.ToString() + LanguageManager.instance.GetValue("levelOpen");
		}
		else
		{
			openlevel.gameObject.SetActive(false);
		}
	}

	void Update ()
	{

	}
}

