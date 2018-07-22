using UnityEngine;
using System.Collections;

public class ActorRotate : MonoBehaviour {

    public Transform modelRoot_;

    GameObject target_;

    BoxCollider dealTouch_;

	// Use this for initialization
	void Start () {
        dealTouch_ = gameObject.GetComponent<BoxCollider>();
        if (dealTouch_ == null)
            dealTouch_ = gameObject.AddComponent<BoxCollider>();
        dealTouch_.center = new Vector2(transform.position.x * -1f, transform.position.y * -1f);

        if (modelRoot_.childCount > 0)
        {
            target_ = modelRoot_.GetChild(0).gameObject;
        }

        UISprite bgs = gameObject.GetComponent<UISprite>();
        if (bgs != null)
        {
            dealTouch_.size = new Vector2(bgs.drawingDimensions.z * 2f, bgs.drawingDimensions.w * 2f);
        }
        else
        {
            UITexture bgt = gameObject.GetComponent<UITexture>();
            if (bgt != null)
            {
                dealTouch_.size = new Vector2(bgt.drawingDimensions.z * 2f, bgt.drawingDimensions.w * 2f);
            }
        }
	}

    float originX_;
    float currentX_;

    float time_;

    bool beginRot_ = false;

    void OnPress(bool isPress)
    {
        if (isPress)
        {
            if (target_ == null)
            {
                target_ = modelRoot_.GetChild(0).gameObject;
            }
            originX_ = Input.mousePosition.x;
        }
        else
        {
            currentX_ = 0f;
            originX_ = 0f;
            time_ = 0f;
        }
        beginRot_ = isPress;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (beginRot_)
        {
            currentX_ = Input.mousePosition.x;
            if (Mathf.Approximately(currentX_, originX_))
                return;

            if (target_ != null)
            {
                float rotV = currentX_ > originX_ ? -1f : 1f;
                target_.transform.Rotate(Vector3.up, Mathf.Rad2Deg * 0.05f * Mathf.PI * rotV);
            }
        }
	}
}
