using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPlayerCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel rankLab;
	public UIButton challengeBtn;
	public UILabel jobLab;
	public Transform Mpos; 
	public UILabel arenaCellChallengBtnLab;
	public UILabel arenaCellRankLab;
	public UITexture icon;
	public UISprite jobImg;
	public UISprite jobBack;

	private COM_EndlessStair _player;
	private GameObject babyObj;
	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (challengeBtn.gameObject, EnumButtonEvent.OnClick, OnChallengeBtn, 0, 0);
		arenaCellChallengBtnLab.text = LanguageManager.instance.GetValue("arenaCellChallengBtnLab");
		arenaCellRankLab.text = LanguageManager.instance.GetValue("arenaCellRankLab");
	}
	
	public COM_EndlessStair Player
	{
		set
		{
			if(value != null)
			{
				_player = value;
				nameLab.text = _player.name_;
				rankLab.text = _player.rank_.ToString();
				levelLab.text = _player.level_.ToString();
				jobImg.spriteName = ((JobType)_player.job_).ToString();

				HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(_player.assetId_).assetsIocn_,icon);

				if(!_icons.Contains(EntityAssetsData.GetData(_player.assetId_).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(_player.assetId_).assetsIocn_);
				}
			}
		}
		get
		{
			return _player;
		}
	}


	private void OnChallengeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(ArenaSystem.Instance.ChallengeNum <= 0)
		{
			//ErrorTipsUI.ShowMe( LanguageManager.instance.GetValue("ChallengeNum"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("ChallengeNum"));
			return;
		}
		if(ArenaSystem.Instance.RemainCDTime > 0)
		{
			//ErrorTipsUI.ShowMe( LanguageManager.instance.GetValue("RemainCDTime"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("RemainCDTime"));
			return; 
		}
		COM_SimplePlayerInst[] team = TeamSystem.GetTeamMembers ();
		if(team != null && team.Length > 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("zhuduibuxing"));
			return; 
		}

		NetConnection.Instance.requestChallenge (_player.name_);
		ArenaUI.HideMe ();
	}


	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		ro.transform.parent = Mpos;
		ro.transform.localScale = new Vector3(300,300,1f);
        ro.transform.localPosition = Vector3.forward * -1000f;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
        EffectLevel el = ro.AddComponent<EffectLevel>();
        el.target = ro.transform.parent.parent.GetComponent<UISprite>();
		if(babyObj != null)
		{
			Destroy (babyObj);
			babyObj = null;
		}
		babyObj = ro;
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}

