using UnityEngine;
using System.Collections;

public class NoticePanel : MonoBehaviour {

    public UISprite bg_;

    public UILabel content_;

    float widOffset_;

    string crtMsg_;

    bool isBoardcasting_;

	// Use this for initialization
	void Start () {
        NoticeManager.Instance.OnNoticeReceived += HasNoticeReceived;
        widOffset_ = bg_.drawingDimensions.z;
        bg_.gameObject.SetActive(false);
        content_.gameObject.SetActive(false);
        ShowNotice(NoticeManager.Instance.LastedNotice);
		gameObject.transform.localPosition = new Vector3 (0,0,-2000);
	}

    void ShowNotice(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            return;

        isBoardcasting_ = true;
        crtMsg_ = msg;
        content_.text = crtMsg_;
        content_.transform.localPosition = new Vector2(widOffset_, 0f);
        content_.gameObject.SetActive(true);
        bg_.gameObject.SetActive(true);
        float finalX = -1 * (content_.drawingDimensions.z + widOffset_);
        iTween.MoveTo(content_.gameObject, iTween.Hash("position", new Vector3(finalX, 0f, 0f), "islocal", true, "time", (Mathf.Abs(widOffset_) + Mathf.Abs(finalX)) / 100f, "easetype", iTween.EaseType.linear, "oncomplete", "OnComplete", "oncompletetarget", gameObject));
    }

    void HasNoticeReceived(string msg)
    {
        if (isBoardcasting_) return;
        OnComplete();
    }

    void OnComplete()
    {
        isBoardcasting_ = false;
        if (NoticeManager.Instance.HasNotice)
        {
            ShowNotice(NoticeManager.Instance.LastedNotice);
        }
        else
        {
            bg_.gameObject.SetActive(false);
            content_.gameObject.SetActive(false);
        }
    }

}
