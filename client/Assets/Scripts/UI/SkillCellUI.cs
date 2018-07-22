using UnityEngine;
using System.Collections.Generic;

public class SkillCellUI : MonoBehaviour
{
	public UILabel SkillName;
	public UILabel SkillLevel;
	public UITexture SkillIcon;
	public UISprite back;
	public UISprite buleBack0;
	public UISprite buleBack1;
	public COM_Skill skillInst;
    public UISprite cellPane;

    private SkillData _skillData;
    private uint _skillId;
    private bool _showTips = false;
    private List<string> _icons = new List<string>();
    void Start()
    {

    }

    void Update()
    {

    }


    //public COM_State stateInst
    //{
    //    set
    //    {
    //        _stateInst = value;
    //        _stateId = value.stateId_;
    //        data = StateData.GetData((int)_stateId);
    //    }
    //}

    public float scale
    {
        set
        {
            cellPane.SetDimensions((int)(cellPane.width * value), (int)(cellPane.height * value));
            SkillIcon.SetDimensions((int)(SkillIcon.width * value), (int)(SkillIcon.height * value));
        }
    }

    public SkillData data
    {
        get
        {
            return _skillData;
        }
        set
        {
            _skillData = value;
            if (_skillData != null)
            {
                //cellPane.spriteName = _babyData.RaceIcon_;
                HeadIconLoader.Instance.LoadIcon(_skillData._ResIconName, SkillIcon);
				if (!_icons.Contains(_skillData._ResIconName))
                {
					_icons.Add(_skillData._ResIconName);
                }
                SkillIcon.depth = cellPane.depth + 1;
            }

            if (_showTips)
            {
                RegesitTips();
            }
        }
    }

    private void RegesitTips()
    {
        UIManager.SetButtonEventHandler(cellPane.gameObject, EnumButtonEvent.TouchDown, OnMouseDown, 0, 0);
        UIManager.SetButtonEventHandler(cellPane.gameObject, EnumButtonEvent.TouchUp, OnMouseUp, 0, 0);
    }

    private void UnRegesitTips()
    {
        UIManager.RemoveButtonEventHandler(cellPane.gameObject, EnumButtonEvent.TouchDown);
        UIManager.RemoveButtonEventHandler(cellPane.gameObject, EnumButtonEvent.TouchUp);
    }


    public bool showTips
    {
        set
        {
            if (_showTips != value)
            {
                if (skillInst != null || _skillData != null)
                {
                    if (_showTips)
                    {
                        UnRegesitTips();
                    }
                    else
                    {
                        RegesitTips();
                    }
                }
                _showTips = value;
            }

        }
        get
        {
            return _showTips;
        }
    }



    private void OnMouseDown(ButtonScript obj, object args, int param1, int param2)
    {
        if (TipsSkillUI.instance != null)
        {
            TipsSkillUI.instance.setData(_skillData, transform);
        }

    }


    private void OnMouseUp(ButtonScript obj, object args, int param1, int param2)
    {

        //if (TipsSkillUI.instance != null)
        //{
        //    TipsSkillUI.instance.HideTips();
        //}

    }


    void OnDestroy()
    {
        for (int n = 0; n < _icons.Count; n++)
        {
            HeadIconLoader.Instance.Delete(_icons[n]);
        }

        if (TipsSkillUI._instance != null)
        {
            TipsSkillUI.instance.HideTips();
        }
    }
}

