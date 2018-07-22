using UnityEngine;
using System.Collections;

public class DailyQustCell : MonoBehaviour {

	public UILabel descLabel;
	public UILabel finishLabel;
	public UILabel rewardLabel;
	public UISprite stateSp;
	public int actcount;
	private ActivityData  _adata;
	
	public ActivityData Adata
	{
		set
		{
			if(value != null)
			{
				_adata = value;
				descLabel.text = _adata._Desc;
				rewardLabel.text = "+"+_adata._Reward.ToString();

			}
		}
		get
		{
			return _adata;
		}
	}

	public void RefreshFinishProgress(int count)
	{
		actcount = count;
		finishLabel.text = "("+count + "/"+Adata._Target+")";
		if(count>=Adata._Target)
		{
			stateSp.gameObject.SetActive(true);
		}else
		{
			stateSp.gameObject.SetActive(false);
		}
	}

	void Start () {
		finishLabel.text = "("+actcount + "/"+_adata._Target+")";
	}

}
