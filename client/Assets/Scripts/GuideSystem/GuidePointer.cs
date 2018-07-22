using UnityEngine;
using System.Collections;

public class GuidePointer : MonoBehaviour {


	public UILabel descLab; 
	public UITexture arrowsImg;
    public GameObject effect_;

    bool begin_;

    bool isLocalPos_;

    GameObject aimObj_;

    Vector2 originPos_;

    Vector2 forward_;

    float offsetX_, offsetY_;

    float wave_ = 60f;

    float speed_ = 2f;

	

	public void Begin(GameObject aimObj, float offsetX, float offsetY, GuidePointerRotateType rotateType,string str)
    {
        isLocalPos_ = false;
        offsetX_ = offsetX;
        offsetY_ = offsetY;
        aimObj_ = aimObj;
        transform.position = new Vector2(aimObj.transform.position.x + offsetX, aimObj.transform.position.y + offsetY);
		originPos_ = arrowsImg.transform.position;
        Set(rotateType, str);
    }

    public void BeginInScene(GameObject aimObj, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str)
    {
        isLocalPos_ = true;
        wave_ = 60f;
        offsetX_ = offsetX;
        offsetY_ = offsetY;
        aimObj_ = aimObj;
        Vector3 uiPos = GlobalInstanceFunction.WorldToUI(aimObj_.transform.position);
        transform.localPosition = new Vector2(uiPos.x + offsetX, uiPos.y + offsetY);
		originPos_ = arrowsImg.transform.localPosition;
        Set(rotateType, str);
    }

    void Set(GuidePointerRotateType rotateType, string str)
    {
        UIPanel panel = arrowsImg.gameObject.AddComponent<UIPanel>();
        panel.sortingOrder = GuideCreator.GuideDepth + 2;
        panel.depth = GuideCreator.GuideDepth + 2;
        panel = descLab.gameObject.AddComponent<UIPanel>();
        panel.sortingOrder = GuideCreator.GuideDepth + 3;
        panel.depth = GuideCreator.GuideDepth + 3;
        GuideManager.Instance.creator.OnClearGuide -= DestroySelf;
        GuideManager.Instance.creator.OnClearGuide += DestroySelf;

        float angle = 0f;
        switch (rotateType)
        {
            case GuidePointerRotateType.GPRT_None:
                forward_ = (transform.up).normalized;
				descLab.transform.localPosition = descLab.transform.localPosition = new Vector3(descLab.transform.localPosition.x,120f,0f);;
                break;
            case GuidePointerRotateType.GPRT_R45:
                angle = 45f;
                forward_ = (transform.up + transform.right * -1f).normalized;
                break;
            case GuidePointerRotateType.GPRT_R90:
                angle = 90f;
                forward_ = (transform.right * -1f).normalized;
                break;
            case GuidePointerRotateType.GPRT_R135:
                angle = 135f;
                forward_ = (transform.up * -1f + transform.right * -1f).normalized;
                break;
            case GuidePointerRotateType.GPRT_R180:
                angle = 180f;
				descLab.transform.localPosition = new Vector3(descLab.transform.localPosition.x,-120f,0f);
                forward_ = (transform.up * -1f).normalized;
                break;
            case GuidePointerRotateType.GPRT_R225:
                angle = 225f;
                forward_ = (transform.up * -1f + transform.right).normalized;
                break;
            case GuidePointerRotateType.GPRT_R270:
                angle = 270f;
                forward_ = (transform.right).normalized;
                break;
            case GuidePointerRotateType.GPRT_R315:
                angle = 315f;
                forward_ = (transform.up + transform.right).normalized;
                break;
            default:
                break;
        }
        if (!string.IsNullOrEmpty(str))
        {
            descLab.gameObject.SetActive(true);
            descLab.text = string.Format("[b]{0}[-]", str);
        }
        else
        {
            descLab.gameObject.SetActive(false);
        }
        //transform.Rotate(Vector3.forward, angle);
        arrowsImg.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        begin_ = true;
    }

    void Update()
    {
       if (!begin_)
           return;

       if (aimObj_ == null)
           return;

        float len = Mathf.PingPong(Time.time * speed_, 1f);
        len *= wave_;
        Vector2 dest = new Vector2(originPos_.x + forward_.x * len, originPos_.y + forward_.y * len);
        if (!isLocalPos_)
        {
            transform.position = new Vector2(aimObj_.transform.position.x + offsetX_, aimObj_.transform.position.y + offsetY_);
            effect_.transform.position = aimObj_.transform.position;
        }
        else
        {
            Vector3 uiPos = GlobalInstanceFunction.WorldToUI(aimObj_.transform.position);
            transform.localPosition = new Vector2(uiPos.x + offsetX_, uiPos.y + offsetY_);
        }
        arrowsImg.transform.localPosition = dest;
    }

    void DestroySelf()
    {
        GuideManager.Instance.creator.OnClearGuide -= DestroySelf;
        GameObject.Destroy(gameObject);
    }

    void OnDestroy()
    {
        GuideManager.Instance.creator.OnClearGuide -= DestroySelf;
    }
}
