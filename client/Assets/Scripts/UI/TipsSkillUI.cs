using UnityEngine;
using System.Collections;

public class TipsSkillUI : MonoBehaviour {

    public UILabel name_;

    public UILabel desc_;

    public UILabel lv_;

    public UITexture icon_;

    public UIButton closeBtn_;

    private UIPanel _tooltip;

    string iconRes_;

    public static bool Loading = false;
    public static TipsSkillUI _instance;
    public static TipsSkillUI instance
    {
        get
        {
            if (_instance == null && Loading == false)
            {
                Loading = true;

                string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TipsSkillUI, AssetLoader.EAssetType.ASSET_UI);

                AssetLoader.LoadAssetBundle(uiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
                {
                    if (null == Assets || null == Assets.mainAsset)
                    {
						Loading = false;
                        return;
                    }

                    GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset) as GameObject;
                    TipsSkillUI t = (TipsSkillUI)go.GetComponent<TipsSkillUI>();
                    t.AttachToGameObject(go);
                    _instance = t;
					Loading = false;
                }
                , null);

            }//
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void setData(object value, Transform aimpos)
    {
        //_tooltip.gameObject.transform.position = new Vector3(aimpos.position.x + (_tooltip.width / 2f) * ApplicationEntry.Instance.uiRoot.transform.localScale.x, aimpos.position.y - (_tooltip.height / 2f) * ApplicationEntry.Instance.uiRoot.transform.localScale.x, aimpos.position.z);
        data = (SkillData)value;
        ShowTips();
    }

    private SkillData data
    {
        set
        {
            name_.text = value._Name;
            desc_.supportEncoding = true;
            desc_.text = value._Desc;
            lv_.text = value._Level.ToString();
            HeadIconLoader.Instance.LoadIcon(value._ResIconName, icon_);
			iconRes_ = value._ResIconName;
        }

    }

    public void ShowTips()
    {
        if (_tooltip != null)
        {
            _tooltip.gameObject.SetActive(true);
			UIManager.Instance.AdjustUIDepth(_tooltip.transform, true, -1000f);
        }
    }


    public void HideTips()
    {
        if (_tooltip != null)
        {
            _tooltip.gameObject.SetActive(false);
            HeadIconLoader.Instance.Delete(iconRes_);
        }
    }

    void OnEnable()
    {
        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
    }

    void OnDisable()
    {
        UIManager.RemoveButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick);
    }

    private void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        HideTips();
    }


    private void setPosition(Vector2 position)
    {
        // The tooltip should appear above the mouse
        var cursorOffset = new Vector2(0, _tooltip.GetComponent<UISprite>().height + 50);

        // Convert position from "screen coordinates" to "gui coordinates"


        _tooltip.transform.position = position;

    }

    public void AttachToGameObject(GameObject goSelf)
    {
        _tooltip = (UIPanel)goSelf.GetComponent<UIPanel>();
        _tooltip.gameObject.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        _tooltip.gameObject.transform.localScale = Vector3.one;
        _tooltip.gameObject.transform.localPosition = new Vector3(0f, 0f, -450f);
        HideTips();
    }
}
