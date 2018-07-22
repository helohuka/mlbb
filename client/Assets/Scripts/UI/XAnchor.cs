using UnityEngine;
using System.Collections;

public class XAnchor : MonoBehaviour {

	public enum AnchorType
	{
		AT_TOP,
		AT_LEFT,
		AT_RIGHT,
		AT_TOPLEFT,
		AT_TOPRIGHT,
		AT_CENTER,
		AT_BOTTOM,
		AT_BOTTOMLEFT,
		AT_BOTTOMRIGHT,
	}

	public AnchorType authorType_ = AnchorType.AT_CENTER;

    UIRoot root_;
	float width_, height_;
	// Use this for initialization
	void Start () {
        
        width_ = ApplicationEntry.Instance.UIWidth;
        height_ = ApplicationEntry.Instance.UIHeight;

		AuthUI ();
	}

	void AuthUI()
	{
		switch(authorType_)
		{
		case AnchorType.AT_TOPLEFT: transform.localPosition = new Vector2(width_ / -2, height_ / 2); break;
		case AnchorType.AT_TOP: transform.localPosition = new Vector2(0, height_ / 2); break;
		case AnchorType.AT_TOPRIGHT: transform.localPosition = new Vector2(width_ / 2, height_ / 2); break;
		case AnchorType.AT_LEFT: transform.localPosition = new Vector2(width_ / -2, 0); break;
		case AnchorType.AT_CENTER: transform.localPosition = Vector2.zero; break;
		case AnchorType.AT_RIGHT: transform.localPosition = new Vector2(width_ / 2, 0); break;
		case AnchorType.AT_BOTTOMLEFT: transform.localPosition = new Vector2(width_ / -2, height_ / -2); break;
		case AnchorType.AT_BOTTOM: transform.localPosition = new Vector2(0, height_ / -2); break;
		case AnchorType.AT_BOTTOMRIGHT: transform.localPosition = new Vector2(width_ / 2, height_ / -2); break;
		}
	}
	

}
