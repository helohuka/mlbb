using UnityEngine;
using System.Collections;

public class MaskClick : MonoBehaviour {

    public delegate void ClickMaskHandler();
    public static ClickMaskHandler OnClickMask;

	void OnClick()
    {
        if (OnClickMask != null)
            OnClickMask();
    }
}
