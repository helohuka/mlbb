using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicChangeCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel jobLab;
	public UISprite jobIcon;
	public UITexture icon;
	public UISprite selectImg;
	public ArtifactChangeData _artifactData;
	private List<string> _icons = new List<string>();


	void Start ()
	{


	}
	

	public ArtifactChangeData ArtifactData
	{
		set
		{
			if(value != null)
			{
				_artifactData = value;
				HeadIconLoader.Instance.LoadIcon(_artifactData._Icon,icon);
				if(!_icons.Contains(_artifactData._Icon))
				{
					_icons.Add(_artifactData._Icon);
				}

				jobLab.text = LanguageManager.instance.GetValue(_artifactData._JobType.ToString()); 
				jobIcon.spriteName = _artifactData._JobType.ToString();
			}
		}
		get
		{
			return _artifactData;
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

