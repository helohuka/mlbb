using UnityEngine;
using System.Collections;
using System;

public class HelpLevelItem : MonoBehaviour {

    public UISprite icon_;
    public UILabel level_;
    public UILabel desc_;
    public GameObject lock_;
    public UIButton check_;

    string type_, arg_;

    public void SetData(string name,int lv, string desc, string check)
    {
        icon_.spriteName = name;
        desc_.text = desc;
        level_.text = string.Format("[b]{0}{1}[-]", lv.ToString(), LanguageManager.instance.GetValue("Level"));
        string[] arg;
        if (!string.IsNullOrEmpty(check))
        {
            arg = check.Split(new char[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
            if (arg.Length != 2)
            {
                check_.gameObject.SetActive(false);
                return;
            }

            type_ = arg[0];
            arg_ = arg[1];

            UIManager.SetButtonEventHandler(check_.gameObject, EnumButtonEvent.OnClick, OnCheck, 0, 0);
        }
        else
            check_.gameObject.SetActive(false);
    }

    private void OnCheck(ButtonScript obj, object args, int param1, int param2)
    {
        if (string.IsNullOrEmpty(type_))
            return;

        if (type_.Equals("ui"))
        {
            GlobalInstanceFunction.Instance.NpcOpenUI((UIASSETS_ID)Enum.Parse(typeof(UIASSETS_ID), arg_));
        }
        else if (type_.Equals("npc"))
        {
            GameManager.Instance.ParseNavMeshInfo(arg_);
        }
    }

    public void UpdateLock(bool islock)
    {
        lock_.SetActive(islock);
        if (!string.IsNullOrEmpty(type_))
            check_.gameObject.SetActive(!islock);
        else
            check_.gameObject.SetActive(false);
    }
}
