using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChatItem : MonoBehaviour 
{
    //背景的高度
    public int cellHeight = 25;

    public int MaxLineWidth = 540;

    //内容颜色
    [HideInInspector]
    public string infoColor;

    //名字颜色
    [HideInInspector]
    public string nameColor;

    //发送消息名称
    [HideInInspector]
    public string nameText;

    public GameObject FacePrefab;

    public List<string> Prefabs;

    public GameObject Label;

    public Font font;

    public enum InfoType
    {
        Text,
        Face,
    }

    public class LabelType
    {
        public string info;
        public InfoType type;
        
        public LabelType(string text,InfoType tp)
        {
            info = text;
            type = tp;
        }
    }

    private int positionX = 10;
    private int positionY = -6;
    
    //聊天内容显示label
    //private UILabel label;

    //背景
    private UISprite back;

    //名字label的宽度
    private int labelNameWidth;
    //声音字节数组
    //private byte[] m_audioBytes;

    //private List<string> sybloms;

    //label缓存区
    private List<UILabel> labelCaches;

    //label当前使用的数据
    private List<UILabel> labelCur;

    //表情缓存区
    private List<GameObject> prefabCeches;

    //当前使用的数据
    private List<GameObject> prefabCur;

    private static List<LabelType> list;
    
    private string m_text;

    void Awake()
    {
        labelCur = new List<UILabel>();
        prefabCur = new List<GameObject>();
        labelCaches = new List<UILabel>();
        prefabCeches = new List<GameObject>();
        list = new List<LabelType>();

        back = transform.FindChild("back").GetComponent<UISprite>();
    }

    /// <summary>
    /// 文本内容
    /// </summary>
    public string text
    {
        get 
        {
            return m_text; 
        }
        set 
        {
            m_text = value;

            Reset();

            list.Add(new LabelType(nameColor+nameText+" : [-]", InfoType.Text));

            ParseSymbol(value);

            ShowSymbol(list);

            NGUITools.UpdateWidgetCollider(back.gameObject);
        }
    }

    
    private static void ParseSymbol(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        int startIndex = 0;
        int endIndex = 0;
        string startString;
        string endString = value;

        string pattern = "\\{\\d\\d*\\}";
        MatchCollection matchs = Regex.Matches(value, pattern);
        string str;

        if (matchs.Count > 0)
        {
            foreach (Match item in matchs)
            {
                str = item.Value;
                startIndex = endString.IndexOf(str);
                endIndex = startIndex + str.Length;

                if (startIndex > -1)
                {
                    startString = endString.Substring(0, startIndex);

                    if (!string.IsNullOrEmpty(startString))
                        list.Add(new LabelType(startString, InfoType.Text));

                    if (!string.IsNullOrEmpty(str))
                        list.Add(new LabelType(str, InfoType.Face));

                    endString = endString.Substring(endIndex);
                }
            }

            if (!string.IsNullOrEmpty(endString))
                list.Add(new LabelType(endString, InfoType.Text));
        }
        else
        {
            list.Add(new LabelType(endString, InfoType.Text));
        }
    }

    private void ShowSymbol(List<LabelType> list)
    {
        foreach (LabelType item in list)
        {
            switch (item.type)
            {
                case InfoType.Text :
                    CreateTextLabel(item.info);
                    break;
                case  InfoType.Face :
                    CreateFace(item.info);
                    break;
            }
        }
    }

    private void CreateTextLabel(string value)
    {

        UILabel label;
        if (labelCaches.Count > 0)
        {
            label = labelCaches[0];
            labelCaches.Remove(label);
            label.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = GameObject.Instantiate(Label) as GameObject;
            go.transform.parent = transform;
            label = go.GetComponent<UILabel>();
            go.transform.localScale = Vector3.one;
        }

        string sbstr = "";
        string text = "";
        
        NGUIText.fontSize = label.fontSize;
        NGUIText.finalSize = label.fontSize;
        NGUIText.dynamicFont = label.trueTypeFont;
        NGUIText.rectWidth = MaxLineWidth - positionX;
        NGUIText.maxLines = 10000;
        NGUIText.rectHeight = 10000;
        
        NGUIText.WrapText(value, out sbstr);

        int index = sbstr.IndexOf("\n");

        if (index > -1)
        {
            text = sbstr.Substring(0, index);
        }
        else
        {
            text = sbstr;
        }

        label.text = infoColor + text + "[-]";

        label.gameObject.transform.localPosition = new Vector3(positionX, positionY, 0);
        
        positionX += label.width;

        labelCur.Add(label);

        sbstr = sbstr.Remove(0, text.Length);

        if (labelNameWidth == 0)
            labelNameWidth = label.width;

        if (sbstr.Length > 0)
        {
            positionX = 12 + labelNameWidth;
            positionY -= cellHeight;
            back.height += cellHeight;

            sbstr = sbstr.Replace("\n", "");
            CreateTextLabel(sbstr);
        }
    }

    private void CreateFace(string value)
    {
        int index = Prefabs.IndexOf(value);
        if (index > -1)
        {
            GameObject face;
            UIWidget widget;

            if (prefabCeches.Count > 0)
            {
                face = prefabCeches[0];
                prefabCeches.Remove(face);
                face.SetActive(true);
            }
            else
            {
                face = GameObject.Instantiate(FacePrefab) as GameObject;
                face.transform.parent = gameObject.transform;
                face.transform.localScale = FacePrefab.transform.localScale;
            }
           
            UISprite sprite = face.GetComponent<UISprite>();
            sprite.spriteName = value;
            widget = face.GetComponent<UIWidget>();
            widget.pivot = UIWidget.Pivot.TopLeft;


            if (MaxLineWidth < (positionX + widget.width))
            {
                positionX = 12 + labelNameWidth;
                positionY -= cellHeight;
                back.height += cellHeight;
            }

            face.transform.localPosition = new Vector3(positionX, positionY, 0);

            positionX += widget.width;

            prefabCur.Add(face);
        }
        else
        {
            CreateTextLabel(value);
        }
    }

    private void Reset()
    {
        positionX = 10;
        positionY = -6;
        labelNameWidth = 0;
        back.height = cellHeight;
        list.Clear();

        while (labelCur.Count > 0)
        {
            UILabel lab = labelCur[0];

            labelCur.Remove(lab);
            labelCaches.Add(lab);
            lab.gameObject.SetActive(false);
        }

        while (prefabCur.Count > 0)
        {
            GameObject go = prefabCur[0];

            prefabCur.Remove(go);
            prefabCeches.Add(go);
            go.SetActive(false);
        }
    }
}
