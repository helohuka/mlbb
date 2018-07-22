using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(XAnchor)), CanEditMultipleObjects]
public class XAnchorEditor : Editor {

	public SerializedProperty authorTypeProp_;

	Transform target_;

	int enumVal_;

    UIRoot root_;

	float width_, height_;

    float UIWidth
    {
        get
        {
            float width = 0f;
            if (root_ != null)
            {
                float s = (float)root_.GetComponent<UIRoot>().activeHeight / Screen.height;
                width = Screen.width * s;
            }
            else
            {
                width = Screen.width;
            }
            return width;
        }
    }

    float UIHeight
    {
        get
        {
            float height = 0f;
            if (root_ != null)
            {
                height = (float)root_.GetComponent<UIRoot>().activeHeight;
            }
            else
            {
                height = Screen.height;
            }
            return height;
        }
    }

	void OnEnable()
	{
        root_ = GameObject.FindObjectOfType<UIRoot>();
		authorTypeProp_ = serializedObject.FindProperty ("authorType_");
		target_ = ((MonoBehaviour)target).transform;
        width_ = UIWidth;
		height_ = UIHeight;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		EditorGUILayout.PropertyField(authorTypeProp_);
		if(enumVal_ != authorTypeProp_.enumValueIndex)
		{
			enumVal_ = authorTypeProp_.enumValueIndex;
			AuthUI();
		}
		serializedObject.ApplyModifiedProperties ();
	}

	public void AuthUI()
	{
		switch((XAnchor.AnchorType)enumVal_)
		{
		case XAnchor.AnchorType.AT_TOPLEFT: target_.localPosition = new Vector2(width_ / -2, height_ / 2); break;
		case XAnchor.AnchorType.AT_TOP: target_.localPosition = new Vector2(0, height_ / 2); break;
		case XAnchor.AnchorType.AT_TOPRIGHT: target_.localPosition = new Vector2(width_ / 2, height_ / 2); break;
		case XAnchor.AnchorType.AT_LEFT: target_.localPosition = new Vector2(width_ / -2, 0); break;
		case XAnchor.AnchorType.AT_CENTER: target_.localPosition = Vector2.zero; break;
		case XAnchor.AnchorType.AT_RIGHT: target_.localPosition = new Vector2(width_ / 2, 0); break;
		case XAnchor.AnchorType.AT_BOTTOMLEFT: target_.localPosition = new Vector2(width_ / -2, height_ / -2); break;
		case XAnchor.AnchorType.AT_BOTTOM: target_.localPosition = new Vector2(0, height_ / -2); break;
		case XAnchor.AnchorType.AT_BOTTOMRIGHT: target_.localPosition = new Vector2(width_ / 2, height_ / -2); break;
		}
	}
}
