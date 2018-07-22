using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsStateUI : MonoBehaviour
{
    public UILabel descLab;
    private UIPanel _tooltip;

    public static bool Loading = false;
    public static TipsStateUI _instance;
    public static TipsStateUI instance
    {
        get
        {
            if (_instance == null && Loading == false)
            {
                Loading = true;

                string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TipsStateUI, AssetLoader.EAssetType.ASSET_UI);

                AssetLoader.LoadAssetBundle(uiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
                {
                    if (null == Assets || null == Assets.mainAsset)
                    {
						Loading = false;
                        return;
                    }

                    GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset) as GameObject;
                    TipsStateUI t = (TipsStateUI)go.GetComponent<TipsStateUI>();
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
        _tooltip.gameObject.transform.position = new Vector3(aimpos.position.x + (_tooltip.width / 2f) * ApplicationEntry.Instance.uiRoot.transform.localScale.x, aimpos.position.y - (_tooltip.height / 2f) * ApplicationEntry.Instance.uiRoot.transform.localScale.x, aimpos.position.z);
        data = (COM_State)value;
        ShowTips();
    }

    private COM_State data
    {
        set
        {
            descLab.supportEncoding = true;
            descLab.text = string.Format(StateData.GetData((int)value.stateId_)._Name, value.value0_);
        }

    }

    public void ShowTips()
    {
        if (_tooltip != null)
        {
            _tooltip.gameObject.SetActive(true);
			UIManager.Instance.AdjustUIDepth(_tooltip.transform, true, -500f);
        }
    }


    public void HideTips()
    {
        if (_tooltip != null)
        {
            _tooltip.gameObject.SetActive(false);
			UIManager.Instance.AdjustUIDepth(_tooltip.transform);
        }
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

        // 开始要关闭。.
        HideTips();
    }

}

