using UnityEngine;
using System.Collections.Generic;

public class QaData {

    public int id;

    public int type;

    public string question;

    public string answer;

    public static Dictionary<int, QaData> metaData;

    public class TypePkg
    {
        public string icon_;
        public string name_;
    }

    public TypePkg typePkg_;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, QaData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("QaData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        QaData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new QaData();
            data.id = parser.GetInt(i, "ID");
            data.type = parser.GetInt(i, "type");
            data.question = parser.GetString(i, "question");
            data.answer = parser.GetString(i, "answer");
            data.typePkg_ = new TypePkg();
            data.typePkg_.icon_ = parser.GetString(i, "icon");
            data.typePkg_.name_ = parser.GetString(i, "name");

            if (metaData.ContainsKey(data.id))
            {
                ClientLog.Instance.LogError("QaData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static QaData[] GetDataByType(int type)
    {
        List<QaData> datas = new List<QaData>();
        foreach (QaData data in metaData.Values)
        {
            if (data.type == type)
                datas.Add(data);
        }
        return datas.ToArray();
    }

    public static TypePkg[] GetAllType()
    {
        List<TypePkg> datas = new List<TypePkg>();
        List<int> types = new List<int>();
        foreach (QaData data in metaData.Values)
        {
            if (!types.Contains(data.type))
            {
                datas.Add(data.typePkg_);
                types.Add(data.type);
            }
        }
        return datas.ToArray();
    }
}
