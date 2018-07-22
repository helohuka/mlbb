using System.Text;
public static class StringTool
{
    const string NameFlag = "#name#";
    
    public static void RepName(ref string src, string name)
    {
        if (src.IndexOf(src) != -1)
        {
           src = src.Replace(NameFlag, name);
        }
    }

    public static void RepColor(ref string src, int start, int end, string color)
    { /// [FFFFFF]xxxxxx[-]

       color = "[" + color + "]";
       end += color.Length;
       src = src.Insert(start,color);
       src = src.Insert(end, "[-]");
    }

    public static string MakeNGUIStringInfoFmt(string info, string text)
    {
        return "[info=" + info + "][u]" + text + "[/u][/info]";
    }

    public static string MakeNGUIStringQuestNPC(string chuansun)
    {
        if (string.IsNullOrEmpty(chuansun))
            return "";
        else
        {
            int npcid = int.Parse(chuansun.Split(new string[1]{";"},System.StringSplitOptions.RemoveEmptyEntries)[1]);
            NpcData nd = NpcData.GetData(npcid);
            return string.Format(LanguageManager.instance.GetValue("gotoWhoAcceptQuest"), chuansun, nd.Name);
        }
            
    }

    public static void RepColor(ref string src, string color)
    {
        RepColor(ref src, 0, src.Length, color);
    }

    public static void UTF8String(ref string str)
    {
        byte[] cstr = Encoding.Unicode.GetBytes(str);

        int i=0,k=0;

        while (k < cstr.Length)
        {
            if (cstr[k] == 0)
            {
                ++k;
                continue;
            }
            else
            {
                cstr[i] = cstr[k];
                ++k;
                ++i;
            }
        }

        str = Encoding.UTF8.GetString(cstr,0,i);
    }


}