using UnityEngine;
using System.Collections;

public class HookKeeper : MonoBehaviour {

    public GameObject hook_;
    public GameObject crtMark_;
    public GameObject mask_;
    public GameObject mendMark_;
    public UISprite icon_;

    void Start()
    {
       
    }

    //签过
    public void ToSigned()
    {
        mask_.SetActive(true);
        hook_.SetActive(true);
        crtMark_.SetActive(false);
        mendMark_.SetActive(false);
    }

    //正常未签
    public void ToUnsignNormal()
    {
        mask_.SetActive(false);
        hook_.SetActive(false);
        mendMark_.SetActive(false);
    }

    //正常补签
    public void ToUnsignMend()
    {
        mask_.SetActive(false);
        hook_.SetActive(false);
        mendMark_.SetActive(true);
    }

    public void IsCurrent()
    {
        crtMark_.SetActive(true);
    }

    public void SetDepth(int depth)
    {
        hook_.GetComponent<UISprite>().depth = depth;
    }
}
