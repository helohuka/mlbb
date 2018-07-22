using UnityEngine;
using System.Collections.Generic;

public class PopText {

    public enum WarningType
    {
        WT_None,
        WT_Tip,
        WT_Warning,
    }

    string[] popColor = new string[] { "FFFFFF", "8FDB84", "FDA787"};

    Object popTextObj_;

    Queue<Pkg> msgQue_;

    static PopText inst_;
    public static PopText Instance
    {
        get
        {
            if (inst_ == null)
            {
                inst_ = new PopText();
            }
            return inst_;
        }
    }

    float timeGap_ = 0.8f;
    bool showing_ = false;

    class Pkg
    {
        public string content;
        public WarningType type;
        public bool rightNow;
		public string strColor;
    }

    public void Init()
    {
        msgQue_ = new Queue<Pkg>();
        if (popTextObj_ == null)
        {
            UIAssetMgr.LoadUI("PopText", (AssetBundle bundle, ParamData data) =>
            {
                popTextObj_ = bundle.mainAsset;
            }, null);
        }
    }

    public void ExcuteCache()
    {
        if (msgQue_.Count > 0)
        {
            Pkg p = msgQue_.Dequeue();
            PopText.Instance.Show(p.content, p.type, p.rightNow,p.strColor);
        }
    }

    public void Show(string content, WarningType type = WarningType.WT_None, bool rightNow = false,string StrColor = "")
    {
        if (showing_ || (GamePlayer.Instance.isInBattle && !rightNow))
        {
            Pkg pkg = new Pkg();
            pkg.content = content;
            pkg.type = type;
            pkg.rightNow = rightNow;
			pkg.strColor = StrColor;
            msgQue_.Enqueue(pkg);
            return;
        }
        showing_ = true;
        GameObject clone = (GameObject)GameObject.Instantiate(popTextObj_);
        clone.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        clone.transform.localScale = Vector3.one;
		UILabel uLab = clone.GetComponentInChildren<UILabel> ();
	
		if(!string.IsNullOrEmpty(StrColor))
		{
			//uLab.color = Color.white;
			uLab.text =  string.Format("[b][{0}]{1}[-][-]", StrColor, content);;
		}
		else
		{
			uLab.text = string.Format("[b][{0}]{1}[-][-]", popColor[(int)type], content);
		}
        float lifeTime = clone.GetComponent<UITweener>().duration;
        Destroy des = clone.AddComponent<Destroy>();
        des.lifetime = lifeTime;
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            showing_ = false;
            if (msgQue_.Count > 0)
            {
                Pkg p = msgQue_.Dequeue();
                PopText.Instance.Show(p.content, p.type, p.rightNow,p.strColor);
            }
        }, timeGap_);
    }

    public void Show(Texture tex)
    {
        GameObject clone = (GameObject)GameObject.Instantiate(popTextObj_);
        clone.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        clone.transform.localScale = Vector3.one;
        UITexture texture = clone.GetComponentInChildren<UITexture>();
        texture.mainTexture = tex;
        texture.MakePixelPerfect();
        float lifeTime = clone.GetComponent<UITweener>().duration;
        Destroy des = clone.AddComponent<Destroy>();
        des.lifetime = lifeTime;
    }

	public void Clear()
	{
		showing_ = false;
		msgQue_.Clear ();
	}
}
