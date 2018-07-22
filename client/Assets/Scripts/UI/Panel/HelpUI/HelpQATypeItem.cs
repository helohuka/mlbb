using UnityEngine;
using System.Collections;

public class HelpQATypeItem : MonoBehaviour {

    public UISprite icon_;
    public UILabel name_;

    public void SetData(string icon, string name)
    {
        icon_.spriteName = icon;
        name_.text = name;
    }
}
