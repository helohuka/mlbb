using UnityEngine;
using System.Collections;

public class RewardCell : MonoBehaviour {

	public UISprite RewardIcon;
	public UILabel RewardLabel;
	public UITexture icont;

	private DropData _dData;
	
	public DropData DData
	{
		set
		{
			if(value != null)
			{
				_dData = value;


			}
		}
		get
		{
			return _dData;
		}
	}
	void Start () {
	
	}
	

}
