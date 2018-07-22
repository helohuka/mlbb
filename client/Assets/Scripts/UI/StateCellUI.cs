using UnityEngine;
using System.Collections.Generic;

public class StateCellUI : MonoBehaviour
{
    public UISprite cellPane;
    public UITexture StateIcon;

    private StateData _stateData;
    private COM_State _stateInst;
    private uint _stateId;
    private bool _showTips = false;
    private List<string> _icons = new List<string>();
    void Start()
    {

    }

    void Update()
    {

    }


    public COM_State stateInst
    {
        set
        {
            _stateInst = value;
            _stateId = value.stateId_;
            data = StateData.GetData((int)_stateId);
        }
    }

    public StateData data
    {
        get
        {
            return _stateData;
        }
        set
        {
            _stateData = value;
            if (_stateData != null)
            {
                //cellPane.spriteName = _babyData.RaceIcon_;
                HeadIconLoader.Instance.LoadIcon(_stateData._Icon, StateIcon);
				if (!_icons.Contains(_stateData._Icon))
                {
					_icons.Add(_stateData._Icon);
                }
                StateIcon.depth = cellPane.depth + 1;
            }

            //if (_showTips)
            //{
                RegesitTips();
            //}
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
                if (_stateInst != null)
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
        if (TipsStateUI.instance != null)
        {
            TipsStateUI.instance.setData(_stateInst, transform);
        }

    }


    private void OnMouseUp(ButtonScript obj, object args, int param1, int param2)
    {

        if (TipsStateUI.instance != null)
        {
            TipsStateUI.instance.HideTips();
        }

    }


    void OnDestroy()
    {
        for (int n = 0; n < _icons.Count; n++)
        {
            HeadIconLoader.Instance.Delete(_icons[n]);
        }

        if (TipsStateUI._instance != null)
        {
            TipsStateUI.instance.HideTips();
        }
    }

}

