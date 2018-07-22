using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpProfessionCellUI : MonoBehaviour
{
	public List<UISprite> porfessIcons = new List<UISprite>();
	public List<UILabel> porfessLabs = new List<UILabel>();
	public List<GameObject> porfessCell = new List<GameObject>();
	public int _porfess;

	void Start ()
	{
	
	}

	public int Porfession
	{
		set
		{
			_porfess = value;
			for(int i =0;i<porfessLabs.Count;i++)
			{
				Profession  pData = Profession.get((JobType)_porfess,i+1);
				porfessLabs[i].text = pData.jobName_;
				porfessIcons[i].spriteName = ((JobType)_porfess).ToString();
				porfessCell[i].name =  _porfess.ToString();
			}

		}
		get
		{
			return _porfess; 
		}
	}


}

