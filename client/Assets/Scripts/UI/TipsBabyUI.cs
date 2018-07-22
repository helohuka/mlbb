using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsBabyUI : MonoBehaviour
{
    public UIPanel pane;
    public UISprite icon;
    public UILabel nameLab;
    public UILabel levelLab;
    public GameObject[] skills_;
    private UIPanel _tooltip;


    public static bool Loading = false;
    public static TipsBabyUI _instance;
    public static TipsBabyUI instance
    {
        get
        {
            if (_instance == null && Loading == false)
            {
                Loading = true;

                string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TipsBabyUI, AssetLoader.EAssetType.ASSET_UI);

                AssetLoader.LoadAssetBundle(uiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
                {
                    if (null == Assets || null == Assets.mainAsset)
                    {
						Loading = false;
                        return;
                    }

                    GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset) as GameObject;
                    TipsBabyUI t = (TipsBabyUI)go.GetComponent<TipsBabyUI>();
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

    public void setData(object value)
    {
        if (value is BabyData)
            data = (BabyData)value;
        else
            inst = (Baby)value;
        ShowTips();
    }

    private BabyData data
    {
        set
        {
            UIManager.Instance.AddBabyCellUI(icon, value);
            nameLab.text = value._Name;
            levelLab.text = "1";
            
        }
    }

    private Baby inst
    {
        set
        {
            data = BabyData.GetData((int)value.GetIprop(PropertyType.PT_TableId));
            levelLab.text = value.GetIprop(PropertyType.PT_Level).ToString();
            SkillData skill = null;
            UITexture tex = null;
            GameObject texGo = null;
            for (int i = 0; i < value.SkillInsts.Count; ++i)
            {
                skill = SkillData.GetData((int)value.SkillInsts[i].skillID_, (int)value.SkillInsts[i].skillLevel_);
                if (skill._Name.Equals(LanguageManager.instance.GetValue("PT_counterpunch")) || skill._Name.Equals(LanguageManager.instance.GetValue("PT_Dodge")))
                    continue;

                tex = skills_[i].GetComponentInChildren<UITexture>();
                if (tex == null)
                {
                    texGo = new GameObject();
                    texGo.layer = LayerMask.NameToLayer("UI");
                    tex = texGo.AddComponent<UITexture>();
                    tex.transform.parent = skills_[i].transform;
                    texGo.transform.localPosition = Vector3.zero;
                    texGo.transform.localScale = Vector3.one;
                    tex.depth = skills_[i].GetComponent<UISprite>().depth + 1;
                }
                HeadIconLoader.Instance.LoadIcon(skill._ResIconName, tex);
            }
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
        _tooltip.gameObject.transform.parent = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>().transform;
        _tooltip.gameObject.transform.localScale = Vector3.one;

        // 开始要关闭。.
        HideTips();
    }

}

