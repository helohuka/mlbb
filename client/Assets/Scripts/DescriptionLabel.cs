using UnityEngine;
using System.Collections;

public class DescriptionLabel : MonoBehaviour {

    UILabel label_;

    float speed_ = 5f;

    float timer_;

	// Use this for initialization
	void Start () {
        label_ = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {

        if (label_ != null)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer_ += Time.deltaTime);
            label_.color = new Color(label_.color.r, label_.color.g, label_.color.b, alpha);
            if (timer_ > 1f)
                label_ = null;
        }
	}
}
