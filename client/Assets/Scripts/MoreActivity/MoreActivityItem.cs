using UnityEngine;
using System.Collections;

public class MoreActivityItem : MonoBehaviour {

    public UISprite bgSp_;
    public UILabel nameLbl_;
    public UIButton btn_;

    string beginNormalState;

    void Start()
    {
        btn_ = gameObject.GetComponent<UIButton>();
        if (!string.IsNullOrEmpty(beginNormalState))
        {
            if (btn_ != null)
                btn_.normalSprite = beginNormalState;
        }
    }

    public void SetData(string name)
    {
        nameLbl_.text = name;
    }

    public void SetSelected(bool select)
    {
        bgSp_.spriteName = select ? "jn_jinlanliang" : "jn_jinlan";
        if(btn_ != null)
            btn_.normalSprite = select ? "jn_jinlanliang" : "jn_jinlan";
        else
            beginNormalState = select ? "jn_jinlanliang" : "jn_jinlan";
    }
}
