using UnityEngine;
using System.Collections;

public class ArenaRankCellUI : MonoBehaviour
{
	public UISprite pane;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel rankLab;
	public UILabel proLab;
	public UISprite rankImg;

	private COM_EndlessStair _player;

	void Start ()
	{

	}

	public COM_EndlessStair Player
	{
		set
		{
			if(value != null)
			{
				_player = value;
				nameLab.text  = _player.name_;
				levelLab.text =  _player.level_.ToString();
				rankLab.text  =_player.rank_.ToString();
				proLab.text = LanguageManager.instance.GetValue(_player.job_.ToString());
				if(_player.rank_%2 > 0)
				{
					pane.spriteName = "paiminglan";
				}
				else
				{
					pane.spriteName = "paimingv";
				}
				if(_player.rank_ == 1 )
				{
					rankLab.text  ="";
					rankImg.gameObject.SetActive(true);
					rankImg.spriteName = "diyi";
				}
				else if(_player.rank_ == 2 )
				{
					rankLab.text  ="";
					rankImg.gameObject.SetActive(true);
					rankImg.spriteName = "dier";
				}
				else if(_player.rank_ == 3 )
				{
					rankLab.text  ="";
					rankImg.gameObject.SetActive(true);
					rankImg.spriteName = "disan";
				}

			}
		}
		get
		{
			return _player;
		}
	}


}

