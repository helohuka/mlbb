using UnityEngine;
using System.Collections;

public class NpcItemUi : MonoBehaviour {

	public UISlider progressBar;
	public delegate void progressBarDelegate(int npcId);
	public static progressBarDelegate OnProgressBarDown;
	public bool isStop;
    public int npcId_;
	float crtTime_;
	float totalTime_;

	// Update is called once per frame
	void Update () {
		if(isStop)
		{
			crtTime_ += Time.deltaTime;
			progressBar.value = Mathf.Lerp (0f, 1f, crtTime_ / totalTime_);
			if(crtTime_ / totalTime_ >= 1f)
			{
				Close();
			}
		}

	}

    public void Show(int npcId)
    {
		isStop = true;
        npcId_ = npcId;
        progressBar.value = 0;
        totalTime_ = 3f;
        crtTime_ = 0f;
        gameObject.SetActive(true);
    }
	void OnDisable()
	{
		if(gameObject.transform.parent.gameObject.activeSelf)
		{
			OnProgressBarDown = null;
		}

	}
	public void Close()
    {
        if (OnProgressBarDown != null)
        {
            OnProgressBarDown(npcId_);
            OnProgressBarDown = null;
        }
        gameObject.SetActive(false);
        npcId_ = 0;
    }
}
