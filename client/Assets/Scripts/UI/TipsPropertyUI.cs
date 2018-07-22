using UnityEngine;
using System.Collections;

public class TipsPropertyUI : UIBase {

    public UILabel desc_;

    static string sDesc;
    static Vector2 position;

    static float w;
    static float h;

    void Start()
    {
        desc_.text = sDesc;
        transform.position = position;
    }

    public static void ShowMe(string desc, Vector2 pos)
    {
        sDesc = desc;

        w = 1800f / ApplicationEntry.Instance.UIWidth;
        h = 320f / ApplicationEntry.Instance.UIHeight;

        float plusX = (pos.x > 0) ? w * -1 : 0;
        float plusY = (pos.y > 0) ? h * -1 : h;

        position = new Vector2(pos.x + plusX, pos.y + plusY);
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_PorpertyTips, false);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_PorpertyTips);
    }

    public override void Destroyobj()
    {
        GameObject.Destroy(gameObject);
    }
}
