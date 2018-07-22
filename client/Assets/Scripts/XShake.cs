using UnityEngine;
using System.Collections;

public class XShake : MonoBehaviour 
{

    //延迟启动
    float delay_ = 0f;

    //频率 动一次多少秒
    float frequency_ = 0.05f;

    float totalTimer_ = 0f;
    float totalTime_ = 0.6f;

    float crtTimer_ = 0f;

    bool run_ = false;

    public bool back_ = false;

    bool forward_ = false;

    bool frontBackFlag_ = false;

    public Vector3 originPos_ = Vector3.zero;

    public int battlePos;

    public delegate void BeattackActionHandler();
    public BeattackActionHandler OnBeattackActionFinish;

    public delegate void MoveBackActionHandler();
    public MoveBackActionHandler OnMoveBackActionFinish;

	// Use this for initialization
	void Start () {
        //originPos_ = Battle.Instance.GetStagePointByIndex(battlePos).position;
        StartCoroutine(StartShake());
	}

    public void AddShakeTime(float time)
    {
        totalTime_ += time;
    }

    IEnumerator StartShake()
    {
        yield return new WaitForSeconds(delay_);
        run_ = true;
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
        if (run_)
        {
            crtTimer_ += Time.deltaTime;
            totalTimer_ += Time.deltaTime;
            if (totalTimer_ > totalTime_)
            {
                run_ = false;
                crtTimer_ = 0f;
                totalTimer_ = 0f;
                transform.localPosition = originPos_;
                back_ = true;
            }
            if (crtTimer_ < frequency_)
            {
                return;
            }
            if (frontBackFlag_)
                transform.localPosition = originPos_ + transform.forward * 0.03f;
            else
                transform.localPosition = originPos_ - transform.forward * 0.03f;
            frontBackFlag_ = !frontBackFlag_;
            crtTimer_ = 0f;
        }
        if (back_)
        {
            crtTimer_ += Time.deltaTime * 5f;
            transform.localPosition = Vector3.Lerp(originPos_, originPos_ - transform.forward * 0.5f , crtTimer_);
            if (crtTimer_ > 1f)
            {
                crtTimer_ = 0f;
                back_ = false;
                forward_ = true;
            }
        }
        if (forward_)
        {
			crtTimer_ += Time.deltaTime * 5f;
            transform.localPosition = Vector3.Lerp(transform.position , originPos_, crtTimer_);
            if (crtTimer_ > 1f)
            {
                crtTimer_ = 0f;
                forward_ = false;
                if (OnBeattackActionFinish != null)
                {
                    OnBeattackActionFinish();
                    OnBeattackActionFinish = null;
                }
            }
            if (OnMoveBackActionFinish != null)
            {
                OnMoveBackActionFinish();
                OnMoveBackActionFinish = null;
            }
        }
	}

    public void OnlyBack()
    {
        totalTime_ = 0f;
    }

    void OnDestroy()
    {
        OnBeattackActionFinish = null;
        OnMoveBackActionFinish = null;
        run_ = false;
        back_ = false;
        forward_ = false;
        StopCoroutine(StartShake());
    }
}
