using UnityEngine;
using System.Collections;

public class LoadingProgressbar : UIBase 
{
	public UISlider slider;
	public GameObject LoadingProgress;
    public UILabel Tips_;

    float changeTipsTime_ = 3f;
    float timer_ = 0f;

    void Start()
    {
        SetTips();
    }

    void SetTips()
    {
        Tips_.text = LoadingTipsData.RandomTips();
    }

    void Update()
    {
        timer_ += Time.deltaTime;
        if (timer_ > changeTipsTime_)
        {
            timer_ = 0f;
            SetTips();
        }
    }

	public void UseThisType()
	{
		gameObject.SetActive (true);

		// regist callback
		LoadingScene.Instance.UpdateCallBack = UpdateProgressbar;
	}

	void UpdateProgressbar(float _nowprocess)  
	{
		if (LoadingProgress == null)
		{
			LoadingProgress = GameObject.FindGameObjectWithTag ("Loading");
		}
		
		if (LoadingProgress != null)
		{
			slider = LoadingProgress.GetComponentInChildren<UISlider> ();    
		}

		if(slider != null)
			slider.value = _nowprocess / 100.0f;
	}

	public override void Destroyobj ()
	{
		Destroy (gameObject);
	}
}
