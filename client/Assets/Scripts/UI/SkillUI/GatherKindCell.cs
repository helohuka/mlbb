using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatherKindCell :  MonoBehaviour{

    public UILabel _Title;
    public UILabel _Desc;

    public UISprite _Selected;
    public UITexture _Show;
    
    public UITexture _Icon0;
    public UITexture _Icon1;
    public UILabel _Desc0;
    public UILabel _Desc1;
    public UISprite _Mask;
    public UILabel _NeedLevel;
	public UISprite titleImg;
	public UILabel needItem;
	public UISprite mineBack;

    public GatherKindSelectHandler _SelectHandler;
	private List<string> _icons = new List<string>();
    int _GatherId;
	public GatherData _gatherData;


	void Start ()
	{
		GatherSystem.instance.UpdateGatheEvent += new RequestEventHandler<COM_Gather> (OnUpdateGatheEvent);
	}

    public void OnClick()
    {
        if (_Mask.Visible == true)
            return;
        if (_SelectHandler != null)
            _SelectHandler(this.gameObject, _GatherId);
    }

    public GatherData Data
    {
        set
        {
			if(value == null)
				return;
			_gatherData = value;
            HeadIconLoader.Instance.LoadIcon(value._Icon, _Show);
            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(value._Show0).icon_, _Icon0);
            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(value._Show1).icon_, _Icon1);

			if(!_icons.Contains(value._Icon))
			{
				_icons.Add(value._Icon);
			}
			if(!_icons.Contains(ItemData.GetData(value._Show0).icon_))
			{
				_icons.Add(ItemData.GetData(value._Show0).icon_);
			}
			if(!_icons.Contains(ItemData.GetData(value._Show1).icon_))
			{
				_icons.Add(ItemData.GetData(value._Show1).icon_);
			}
			if(_gatherData._Type == (int)MineType.MT_BuLiao)
			{
				mineBack.spriteName = "zhibu";
			}
			else if(_gatherData._Type == (int)MineType.MT_JinShu)
			{
				mineBack.spriteName = "caikuang";
			}
			else 
			{
				mineBack.spriteName = "famu";
			}

            _Selected.Hide();

            _Title.text = value._Title;
            _Desc0.text = ItemData.GetData(value._Show0).name_;
            _Desc1.text = ItemData.GetData(value._Show1).name_;

            _GatherId = value._Id;

            if (value._Level > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
            {
                _Mask.Show();
                _NeedLevel.Show();
				_NeedLevel.text =  LanguageManager.instance.GetValue("caijidengji").Replace("{n}",value._Level.ToString());
            }
            else
            {
                _Mask.Hide();
                _NeedLevel.Hide();
                
            }
			if(value._Level < 40)
			{
				titleImg.spriteName  ="putongtiao"; 
			}

			if(value._Level >= 40)
			{
				COM_Gather  gatherD = GatherSystem.instance.GetOpenGather(value._Id);
				if(gatherD == null)
				{
					_Mask.Show();
					needItem.gameObject.gameObject.SetActive(true);
					needItem.text = LanguageManager.instance.GetValue("gatherNeedBook").Replace("{n}",value._Title.ToString());
				}
				else
				{
					if(gatherD.flag_ != GatherStateType.GST_Advanced)
					{
						titleImg.spriteName  ="putongtiao"; 
					}
					else
					{
						titleImg.spriteName  ="gaojitiao"; 
					}
					needItem.gameObject.gameObject.SetActive(false);
					_Mask.Hide();
					//_Desc.text = LanguageManager.instance.GetValue("gatherGetItemLab");
					if (value._Level > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
					{
						_Mask.Show();
					}
				}
			
			}
        }
		get
		{
			return _gatherData ;
		}
    }

	void OnUpdateGatheEvent(COM_Gather g)
	{
		if(g.gatherId_ == Data._Id)
		{
			//Data = Data;
		}
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}