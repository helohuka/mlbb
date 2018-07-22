using UnityEngine;
using System.Collections.Generic;

public class BabyCellUI : MonoBehaviour
{
    public UISprite cellPane;
    public UITexture BabyIcon;
    public UITexture RaceIcon;
	public UISprite numsp;
	public UISprite iconBack;
    private BabyData _babyData;
    private Baby inst_;
    private bool _showTips = false;
    private List<string> _icons = new List<string>();
    private List<string> _raceIcons = new List<string>();

    public UISprite mask;
    private bool _enable = true;

    void Start()
    {

    }

    void Update()
    {

    }

    public BabyData data
    {
        get
        {
            return _babyData;
        }
        set
        {
            _babyData = value;
            if (_babyData != null)
            {
                EntityAssetsData ead = EntityAssetsData.GetData((int)_babyData._AssetsID);
                //cellPane.spriteName = _babyData.RaceIcon_;
                HeadIconLoader.Instance.LoadIcon(ead.assetsIocn_, BabyIcon);
                HeadIconLoader.Instance.LoadIcon(_babyData._RaceIcon, RaceIcon);
				if (!_raceIcons.Contains(_babyData._RaceIcon))
                {
					_raceIcons.Add(_babyData._RaceIcon);
                }
                if (!_icons.Contains(ead.assetsIocn_))
                {
                    _icons.Add(ead.assetsIocn_);
                }
                BabyIcon.depth = cellPane.depth + 1;
                RaceIcon.depth = cellPane.depth + 2;
            }

            if (_showTips)
            {
                RegesitTips();
            }
        }
    }

    public Baby inst
    {
        get
        {
            return inst_;
        }
        set
        {
            inst_ = value;
            data = BabyData.GetData((int)value.GetIprop(PropertyType.PT_TableId));
			iconBack.spriteName = BabyData.GetPetQuality(data._PetQuality);

            int Magic = data._BIG_Magic - inst_.gear_[(int)BabyInitGear.BIG_Magic];
            int Stama = data._BIG_Stama - inst_.gear_[(int)BabyInitGear.BIG_Stama];
            int Speed = data._BIG_Speed - inst_.gear_[(int)BabyInitGear.BIG_Speed];
            int Power = data._BIG_Power - inst_.gear_[(int)BabyInitGear.BIG_Power];
            int Strength = data._BIG_Strength - inst_.gear_[(int)BabyInitGear.BIG_Strength];
			int num = Magic+Stama+Speed+Power+Strength;
			numsp.spriteName = BabyData.GetBabyLeveSp(num);
            numsp.depth = cellPane.depth + 3;
            mask.depth = numsp.depth + 1;
            mask.enabled = false;
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
                if (_babyData != null)
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

    public bool enable
    {
        set
        {
            _enable = value;
            mask.enabled = !value;
            gameObject.GetComponent<BoxCollider>().enabled = value;
            _showTips = value;
        }
        get
        {
            return _enable;
        }
    }

    public bool collideEnable
    {
        set
        {
            gameObject.GetComponent<BoxCollider>().enabled = value;
        }
    }

    private void OnMouseDown(ButtonScript obj, object args, int param1, int param2)
    {
        if(inst_ != null)
        {
            if (TipsBabyUI.instance != null)
                TipsBabyUI.instance.setData(inst_);
        }
        else
        {
            if (TipsBabyUI.instance != null)
                TipsBabyUI.instance.setData(_babyData);
        }
    }


    private void OnMouseUp(ButtonScript obj, object args, int param1, int param2)
    {
        if (TipsBabyUI.instance != null)
        {
            TipsBabyUI.instance.HideTips();
        }

    }

    void OnDestroy()
    {
        for (int n = 0; n < _icons.Count; n++)
        {
            HeadIconLoader.Instance.Delete(_icons[n]);
        }
        for (int i = 0; i < _raceIcons.Count; i++)
        {
            HeadIconLoader.Instance.Delete(_raceIcons[i]);
        }
        if (TipsBabyUI._instance != null)
            TipsBabyUI.instance.HideTips();
    }
}

