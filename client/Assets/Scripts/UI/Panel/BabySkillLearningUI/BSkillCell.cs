using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSkillCell : MonoBehaviour {

	public UITexture Icon;
	public UISprite IconKuang;
	public UITexture IconSuo;
	public UILabel nameLabel;
	public UILabel LevelLabel;
	public UILabel TishiLabel;
	public UISprite spp;
	public UISprite sppp;
	private SkillData _sData;
	private List<string> _icons = new List<string>();
	public SkillData SData
	{
		set
		{
			if(value != null)
			{
				_sData = value;
				nameLabel.text =_sData._Name;
                nameLabel.gameObject.SetActive(true);
				//LevelLabel.text = _sData._Level.ToString();
                Icon.gameObject.SetActive(true);
				//Icon.spriteName = _sData.resIconName;
				HeadIconLoader.Instance.LoadIcon (_sData._ResIconName, Icon);
				if(!_icons.Contains(_sData._ResIconName))
				{
					_icons.Add(_sData._ResIconName);
				}
				//TishiLabel.text ="学习技能";
                //TishiLabel.gameObject.SetActive(false);
			}
		}
		get
		{
			return _sData;
		}
	}

	public void SetUIDisableb(bool isd)
	{
		IconKuang.gameObject.SetActive(isd);
		Icon.gameObject.SetActive(isd);
		IconSuo.gameObject.SetActive(isd);
		nameLabel.gameObject.SetActive(isd);
		//LevelLabel.gameObject.SetActive(isd);
		TishiLabel.gameObject.SetActive(isd);
		spp.gameObject.SetActive (isd);
		TishiLabel.gameObject.SetActive(!isd);
	}

	void Start () {
	
	}
	

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}
