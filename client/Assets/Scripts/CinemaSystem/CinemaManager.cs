using UnityEngine;
using System.Collections.Generic;

public class CinemaManager : MonoBehaviour {

    public Texture overlayTexture;

    public float duration = 1f;

    private float progress = 0f;

    //场景的剧情
    public Animator[] sense_;

    //是否有场景剧情正在播放
    bool isPlaying_ = false;

    //当前播放的剧情索引
    int senseIdx_;

    int senseProgress_ = 0;

    GameObject cinemaRoot_;

    int cinemaDepth = 3000;

    List<GameObject> hiddenUI_;

    enum FadeType
    {
        FT_None,
        FT_FadeIn,
        FT_FadeOut,
    }

    FadeType fadeType_ = FadeType.FT_None;

    void Awake()
    {
        hiddenUI_ = new List<GameObject>();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeType_ == FadeType.FT_FadeIn)
        {
            progress += Time.deltaTime;
            if (progress >= 1f)
            {
                if (isPlaying_)
                    EnterSenseMode();
                else
                    QuitSenseMode();
                fadeType_ = FadeType.FT_FadeOut;
                progress = 1f;
            }
        }
        else if (fadeType_ == FadeType.FT_FadeOut)
        {
            progress -= Time.deltaTime;
            if (progress <= 0f)
            {
                if (isPlaying_ == false)
                    ProcEventForGuide();
                progress = 0f;
                fadeType_ = FadeType.FT_None;
				ApplicationEntry.Instance.switchSceneMask_.SetActive(false);
            }
        }
        else
            progress = 0f;
	}

    void OnGUI()
    {
//        if(GUILayout.Button("QuitSense"))
//        {
//            QuitSense();
//        }
        if (fadeType_ == FadeType.FT_None)
            return;

		GUI.depth = 0;
		Color c = GUI.color;
		GUI.color = new Color(1, 1, 1, progress);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlayTexture);
		GUI.color = c;
    }

    //相当于进入剧情
    public void PlaySense(int idx)
    {
        if (idx < 0 || idx > sense_.Length - 1)
            return;

        cinemaRoot_ = new GameObject("CinemaMask");
        cinemaRoot_.layer = LayerMask.NameToLayer("UI");
        cinemaRoot_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        cinemaRoot_.AddComponent<CinemaMaskHandler>();
        UIPanel panel = cinemaRoot_.AddComponent<UIPanel>();
        panel.depth = cinemaDepth;
        panel.sortingOrder = cinemaDepth;
        BoxCollider bc = cinemaRoot_.AddComponent<BoxCollider>();
        Vector3 size = new Vector3(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight, 0);
        bc.size = size;

        fadeType_ = FadeType.FT_FadeIn;
        senseIdx_ = idx;
        isPlaying_ = true;

		ApplicationEntry.Instance.switchSceneMask_.SetActive(true);

		GameManager.Instance._IsSenseMode = true;
    }

    void EnterSenseMode()
    {
        if (sense_ == null || sense_.Length <= senseIdx_ || sense_.Length == 0)
            return;

        if (sense_[senseIdx_].gameObject == null)
            return;

        SenseBase sb = sense_[senseIdx_].gameObject.GetComponent<SenseBase>();
        if (sb == null)
            return;

        sb.SetUpActor();

        //隐藏所有角色
        Prebattle.Instance.HideAllPeople();

        //隐藏除剧情外所有UI
        HideAllActiveUI();

        sense_[senseIdx_].SetTrigger("Play");
    }

    public void NextSense()
    {
        if (sense_ == null || sense_[senseIdx_] == null)
            return;

        sense_[senseIdx_].SetTrigger("Next" + senseProgress_);
        senseProgress_ ++;
    }

    void HideAllActiveUI()
    {
        GameObject activeObj = null;
        Camera ui = null;
        //隐藏除剧情外所有UI
        for (int i = 0; i < ApplicationEntry.Instance.uiRoot.transform.childCount; ++i)
        {
            activeObj = ApplicationEntry.Instance.uiRoot.transform.GetChild(i).gameObject;
            if (activeObj.activeSelf)
            {
                ui = activeObj.GetComponent<Camera>();
                if (ui == null && !activeObj.name.Equals("CinemaMask"))
                {
                    hiddenUI_.Add(activeObj);
                    activeObj.SetActive(false);
                }
            }
        }
    }

    void ShowAllHiddenUI()
    {
        //显示所有被隐藏的UI
        for (int i = 0; i < hiddenUI_.Count; ++i)
        {
            if(hiddenUI_[i] != null)
                hiddenUI_[i].SetActive(true);
        }
        if (senseIdx_ == 0)
            Prebattle.Instance.ShowAllPeople();
    }

    //退出剧情
    public void QuitSense()
    {
        if (isPlaying_ == false)
            return;

        isPlaying_ = false;

        fadeType_ = FadeType.FT_FadeIn;

		ApplicationEntry.Instance.switchSceneMask_.SetActive(true);
    }

    void QuitSenseMode()
    {
		GameManager.Instance._IsSenseMode = false;

        CinemaUI.HideMe();
        
        if(sense_[senseIdx_] != null)
            Destroy(sense_[senseIdx_].gameObject);

        if (cinemaRoot_ != null)
            Destroy(cinemaRoot_);

        ShowAllHiddenUI();

        Prebattle.Instance.ShowAllPeople();

        NpcHeadChat.Instance.Clear();
    }

    void ProcEventForGuide()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ExitSense, senseIdx_);
        senseIdx_ = -1;
    }

    public void ActorTalk(SenseActorType sat, string talkKey, int index, float time)
    {
        GameObject.FindObjectOfType<SenseBase>().Talk(sat, talkKey, index, time);
    }

    public void Clear()
    {
        senseIdx_ = -1;
        senseProgress_ = 0;
        fadeType_ = FadeType.FT_None;
        isPlaying_ = false;
		GameManager.Instance._IsSenseMode = false;

        if (cinemaRoot_ != null)
            Destroy(cinemaRoot_);

        NpcHeadChat.Instance.Clear();
        SenseBase.SenseActorObjDic_.Clear();
    }

    void OnDestroy()
    {
        Clear();
    }
}
