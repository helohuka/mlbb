using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// 支持动态字体的Label组件
/// </summary>
public class SymbolLabel : MonoBehaviour
{
    /// <summary>
    /// 表情转移字符定义
    /// </summary>
    private List<string> m_Symbols = new List<string> { "{00}", "{01}", "{02}", "{03}", "{04}", "{05}"
		, "{06}", "{07}", "{08}", "{09}", "{10}","{11}"};
    
    private string m_Text;
    private string m_realText;

    public UIFont uifont;
    public Font font;
    private int fontSize = 27;
	private int symbolSize = 27;
    public int spacingY = 0;
    public int width = 270;
    private int depth = 3;
    public int maxLine = 0;
	public ChatUILabel.Overflow overflowMethod = ChatUILabel.Overflow.ResizeFreely;

	private ChatUILabel m_TextLabel;
	private ChatUILabel m_SymbolLabel;

    private MatchCollection m_matchs;
    private MatchCollection m_spaceMatchs;
    private List<Match> m_realMatchs;

    private int m_DrawStart = 0;
	 
    void Awake()
    {
        m_realMatchs = new List<Match>();

       
    }
	void Start()
	{

	}

    public int height
    {
        get
        {
            return m_TextLabel.height;
        }
    }

    public UILabel labelText
    {
        get
        {
            return m_TextLabel;
        }
    }

    public UILabel labelSymbol
    {
        get
        {
            return m_SymbolLabel;
        }
    }
	public void InitUIlabel()
	{
		m_TextLabel = NGUITools.AddChild<ChatUILabel>(gameObject);
		
		m_TextLabel.name = "textLabel";
		m_TextLabel.trueTypeFont = font;
		m_TextLabel.spacingY = spacingY;
		m_TextLabel.fontSize = fontSize;
		m_TextLabel.overflowMethod = ChatUILabel.Overflow.ResizeFreely;
		m_TextLabel.alignment = NGUIText.Alignment.Left;
		m_TextLabel.pivot = UIWidget.Pivot.TopLeft;
		m_TextLabel.width = width;
		m_TextLabel.depth = depth;
		m_TextLabel.transform.localPosition = Vector3.zero;
		
		if (overflowMethod == ChatUILabel.Overflow.ClampContent)
		{
			m_TextLabel.height = fontSize;
			m_TextLabel.maxLineCount = maxLine;
		}
		
		m_SymbolLabel = NGUITools.AddChild<ChatUILabel>(gameObject);
		m_SymbolLabel.name = "symbolLabel";
		m_SymbolLabel.bitmapFont = uifont;
		m_SymbolLabel.fontSize = symbolSize;
		m_SymbolLabel.height = symbolSize;
		m_SymbolLabel.overflowMethod = ChatUILabel.Overflow.ShrinkContent;
		m_SymbolLabel.alignment = NGUIText.Alignment.Left;
		m_SymbolLabel.pivot = UIWidget.Pivot.TopLeft;
		m_SymbolLabel.depth = depth + 1;
		m_SymbolLabel.transform.localPosition =  Vector3.zero;
		m_SymbolLabel.SetSymbolOffset(SymbolOffset);
	}
	public void clearObj()
	{
		foreach(Transform tr in gameObject.transform)
		{
			Destroy(tr.gameObject);
		}
	}
    public string text 
    {
        get { return m_Text; }

        set
        {
			clearObj();
			InitUIlabel();
            if(string.IsNullOrEmpty(value))
            {
                m_Text = "";
                m_TextLabel.text = null;
                m_SymbolLabel.text = null;
                m_realMatchs.Clear();
                return;
            }
            
            m_realMatchs.Clear();

            m_Text = value;
			//if(m_TextLabel == null)return;
            string mProcessedText = m_TextLabel.processedText;

            if (overflowMethod == ChatUILabel.Overflow.ResizeHeight) mProcessedText = m_Text;
            else NGUIText.WrapText(m_Text, out mProcessedText);

            StringBuilder sString = new StringBuilder();
            string t = value;
            const string pattern = "\\{\\w\\w\\}";

            m_realText = NGUIText.StripSymbols(mProcessedText);
            m_matchs = Regex.Matches(m_realText, pattern);

            const string pat = " ";
            m_spaceMatchs = Regex.Matches(mProcessedText, pat);

            if (m_matchs.Count > 0)
            {
                Match item;
                for (int i = 0; i < m_matchs.Count; i++)
                {
                    item = m_matchs[i];

                    if (m_Symbols.IndexOf(item.Value) > -1)
                    {
                        m_realMatchs.Add(item);
                        sString.Append(item.Value);
                    }
                }
            }

            m_TextLabel.text = t;
            m_SymbolLabel.text = sString.ToString();

            m_SymbolLabel.width = m_TextLabel.width;
            m_SymbolLabel.height = m_TextLabel.height;

            m_SymbolLabel.MarkAsChanged();
        }
    }

    /// <summary>
    /// 修改顶点坐标 适配表情位置
    /// 1 — 2
    /// |  / |
    /// 0 — 3
    /// </summary>
    private void SymbolOffset()
    {
        BetterList<Vector3> textVerts = m_TextLabel.geometry.verts;
        BetterList<Vector3> symbolVerts = m_SymbolLabel.geometry.verts;
        Vector3 spacing = new Vector3(0,0);

        if (textVerts.size > 0 && symbolVerts.size > 0)
        {
            Match item;
            float tw, sw, x = 0;
            int end, start;

            for (int i = 0; i < m_realMatchs.Count; i++)
            {
                item = m_realMatchs[i];

                //获取表情转移字符顶点开始、结束索引
                start = GetIndex(item.Index) * 4;
                end = start + (item.Length - 1) * 4 + 3;

                //表情都顶点索引
                int p = i * 4;  

                //表情宽度
                sw = Mathf.Abs(symbolVerts.buffer[p].x - symbolVerts.buffer[p + 3].x);

                //如果不换行，计算文本表情转移符都宽带 否则换行不需要计算 添加1个单位距离 跟在后面
                if (textVerts.buffer[start].y == textVerts.buffer[end].y)
                {
                    //文本表情转义符宽度
                    tw = Mathf.Abs(textVerts.buffer[start].x - textVerts.buffer[end].x);

                    //计算居中坐标
                    x = (tw - sw) / 2;
					//x = 1;
                }
                else x = 1;              

                //居中显示表情
                spacing.x = x;

                //计算偏移
                Vector2 po = m_TextLabel.pivotOffset;
                float fx = Mathf.Lerp(0f, -NGUIText.rectWidth, po.x);
                float fy = Mathf.Lerp(NGUIText.rectHeight, 0f, po.y) + Mathf.Lerp((m_TextLabel.printedSize.y - NGUIText.rectHeight), 0f, po.y);
                fx = Mathf.Round(fx);
                fy = Mathf.Round(fy);
		
                //计算出位移向量   
                Vector3 v = textVerts.buffer[start] - symbolVerts.buffer[p];   

                //第一个顶点
                symbolVerts.buffer[p] = textVerts.buffer[start] + spacing;
                symbolVerts.buffer[p].x -= fx;
                symbolVerts.buffer[p++].y -= fy;

                //第二个顶点
                symbolVerts.buffer[p] = symbolVerts.buffer[p] + v + spacing;
                symbolVerts.buffer[p].x -= fx;
                symbolVerts.buffer[p++].y -= fy;

                //第三个顶点
                symbolVerts.buffer[p] = symbolVerts.buffer[p] + v + spacing;
                symbolVerts.buffer[p].x -= fx;
                symbolVerts.buffer[p++].y -= fy;

                //第四个顶点
                symbolVerts.buffer[p] = symbolVerts.buffer[p] + v + spacing;
                symbolVerts.buffer[p].x -= fx;
                symbolVerts.buffer[p++].y -= fy;

                for (int j = 0; j < item.Length; j++)
                {
                    //本来是希望将顶点坐标抹除、但是由于会出现坐标不对都情况、所以放弃了该方法，将顶点都颜色清除掉。
                    //textVerts.buffer[start++] = Vector3.zero;
                    //textVerts.buffer[start++] = Vector3.zero;
                    //textVerts.buffer[start++] = Vector3.zero;
                    //textVerts.buffer[start++] = Vector3.zero;

                    if (m_TextLabel.geometry.cols.size >= (start + 4))
                    {
                        m_TextLabel.geometry.cols[start++] = Color.clear;
                        m_TextLabel.geometry.cols[start++] = Color.clear;
                        m_TextLabel.geometry.cols[start++] = Color.clear;
                        m_TextLabel.geometry.cols[start++] = Color.clear;
                    }
                }
            }
        }
    }

	public float GetCharWidth(char ch)
	{
		NGUIText.dynamicFont = font;
		return NGUIText.GetGlyphWidth (ch, 0);
	}

    /// <summary>
    /// 获取表情转移字符'{'顶点索引，并且需要排除空格符的部分，因为空格符UILabel是不会生成顶点的 所以需要减去空格符都数量，才能正确获得表情索引
    /// </summary>
    /// <returns></returns>
    private int GetIndex(int itemIndex)
    {
        Match item;

        int count = 0;
        for (int i = 0; i < m_spaceMatchs.Count; i++)
        {
            item = m_spaceMatchs[i];
            if (item.Index < itemIndex)
            {
                count++;
            }
        }

        return itemIndex - count;
    }

}