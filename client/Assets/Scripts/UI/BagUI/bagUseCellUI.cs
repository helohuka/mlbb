using UnityEngine;
using System.Collections;

public class bagUseCellUI : MonoBehaviour
{

	public UILabel nameLab;
	public UILabel typeLab;
	public UILabel hpLab;
	public UILabel fpLab;

	public UILabel bagUseHpLab;
	public UILabel bagUseMagicLab;
	public int instId;
	void Start ()
	{
		bagUseHpLab.text = LanguageManager.instance.GetValue("bagUseHpLab");
		bagUseMagicLab.text = LanguageManager.instance.GetValue("bagUseMagicLab");;
	}
	
}

