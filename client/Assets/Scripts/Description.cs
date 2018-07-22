using UnityEngine;
using System.Collections.Generic;

public class Description : MonoBehaviour {

    GameObject backGround_;

    GameObject labelObj_;

    float originFrom_, destTo_;

    float timer_;

    string contents_ = "泰瑞尔新历870年，一架蒸汽飞艇正在航行着，有一个年轻的旅行者就在飞艇之上，这个旅行者名叫#name#，为传说中的冒险者罗伯特的孩子，#name#的目的地，就是安静祥和的小国聂拉维尔王城，聂拉维尔的国王克里斯因为某个原因，召唤了#name#，#name#准备带着自己的仲魔，进行一场未知的大冒险……";

    GameObject skipBtn_;

	// Use this for initialization
	void Start () {
        CreateSkipBtn();
        //CreateBackGround();
        //UILabel lbl = backGround_.GetComponentInChildren<UILabel>();
        //labelObj_ = lbl.gameObject;
        //string playerName = GamePlayer.Instance.InstName;
        //StringTool.RepColor(ref playerName, "00FFFF");
        //StringTool.RepName(ref contents_, playerName);
        //lbl.text = contents_;
        //originFrom_ = labelObj_.transform.localPosition.x + lbl.width / 2f;
        //destTo_ = labelObj_.transform.localPosition.x - lbl.width;
	}

    void CreateSkipBtn()
    {
        skipBtn_ = (GameObject)Instantiate(Resources.Load<GameObject>("SkipBtn"));
        Vector2 size = skipBtn_.GetComponent<BoxCollider>().size;
        skipBtn_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        skipBtn_.transform.localScale = Vector3.one;
        skipBtn_.transform.localPosition = new Vector2(ApplicationEntry.Instance.UIWidth / 2f - size.x, ApplicationEntry.Instance.UIHeight / 2f - size.y);
    }

    void CreateBackGround()
    {
        backGround_ = (GameObject)Instantiate(Resources.Load<GameObject>("background"));
        Texture2D tex = new Texture2D((int)ApplicationEntry.Instance.UIWidth, (int)(ApplicationEntry.Instance.UIHeight / 5f));
        for (int i = 0; i < tex.width; ++i )
        {
            for (int j = 0; j < tex.height; ++j )
            {
                tex.SetPixel(i, j, Color.black);
            }
        }
        tex.Apply();

        UITexture texture = backGround_.GetComponentInChildren<UITexture>();
        texture.mainTexture = tex;
        texture.MakePixelPerfect();
        backGround_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        backGround_.transform.localScale = Vector3.one;
        UITexture img = backGround_.GetComponentInChildren<UITexture>();
        img.transform.localPosition = new Vector2(0f, ApplicationEntry.Instance.UIHeight / -2f + tex.height);
    }


    void OnDestroy()
    {
        Destroy(skipBtn_);
    }
}
