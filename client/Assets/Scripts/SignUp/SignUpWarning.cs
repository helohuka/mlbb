using UnityEngine;
using System.Collections;

public class SignUpWarning : MonoBehaviour {

    public UILabel CostLabel_;

    public UIButton ConfirmBtn_;
    public UIButton CancelBtn_;

    public delegate void ConfirmHandler();
    public ConfirmHandler OnConfirm;

    void Start()
    {
        UIManager.SetButtonEventHandler(ConfirmBtn_.gameObject, EnumButtonEvent.OnClick, OnClickConfirm, 0, 0);
        UIManager.SetButtonEventHandler(CancelBtn_.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);
    }

    void OnClickConfirm(ButtonScript obj, object args, int param1, int param2)
    {
        if (OnConfirm != null)
            OnConfirm();
        gameObject.SetActive(false);
    }

    void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
    {
        gameObject.SetActive(false);
    }

	public void Show(int cost, ConfirmHandler callback)
    {
        CostLabel_.text = cost.ToString();
        OnConfirm = callback;
        gameObject.SetActive(true);
    }
}
