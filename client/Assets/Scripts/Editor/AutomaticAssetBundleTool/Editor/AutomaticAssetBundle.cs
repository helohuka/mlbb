using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

public class AutomaticAssetBundle : EditorWindow {

	private TextAsset pathFile = null;
	private TextAsset md5File = null;

    private string L18NPathFile = "";
    private string PlatformPathFile = "";
	private string localPath = "";
	
	private bool android = false;
	private bool ios = true;
	private bool PC = false;

	private bool cns = false;
	private bool cnt = true;
	private bool en = false;

	static private Dictionary<string, string[]> pathFiles = null;
	static private List<string> pathList = null;
	static private Dictionary<string, string> md5Files = null;
	static private Dictionary<string, string> md5Dic = null;

	[MenuItem("Tools/AtomicAssetBundle/Show Window")]
	static private void Entrance()
	{
		pathFiles = new Dictionary<string, string[]> ();
		pathList = new List<string> ();
		md5Files = new Dictionary<string, string> ();
		md5Dic = new Dictionary<string, string> ();
		EditorWindow.GetWindow<AutomaticAssetBundle>("AutomaticAssetBundle");
	}

	void OnGUI()
	{
		GUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("路径文件");
		pathFile = EditorGUILayout.ObjectField (pathFile, typeof(TextAsset), false) as TextAsset;

		EditorGUILayout.Separator ();

		EditorGUILayout.LabelField ("md5文件");
		md5File = EditorGUILayout.ObjectField (md5File, typeof(TextAsset), false) as TextAsset;

		EditorGUILayout.Separator ();

		if (pathFile != null) 
		{
            //EditorGUILayout.LabelField ("选择语言");
            //cns = EditorGUILayout.BeginToggleGroup ("简体中文", !cnt && !en);
            //EditorGUILayout.EndToggleGroup ();
			
            //cnt = EditorGUILayout.BeginToggleGroup ("繁体中文", !cns && !en);
            //EditorGUILayout.EndToggleGroup ();
			
            //en = EditorGUILayout.BeginToggleGroup ("英文", !cnt && !cns);
            //EditorGUILayout.EndToggleGroup ();

            //EditorGUILayout.Separator ();

            //string selectedLanguage = cns ? "Chinese_Simplified" : cnt ? "Chinese_Traditional" : en ? "English" : "";

            //if(localPath != selectedLanguage)
            //{
            //    if(GUILayout.Button("转换路径"))
            //    {
            //        L18NPath();
            //    }
            //}
            //else
            //{
				if(GUILayout.Button("Pack"))
				{
					bool bOk = EditorUtility.DisplayDialog("提示", "请确认md5文件和平台等参数已正确选择之后\n按确定开始打包", "确定", "取消");
					if(bOk)
					{
						if(md5File != null)
							parseMd5File();
                        if (PlatformPath())
                            parsePathFile();
                        else
                        {
                            EditorUtility.DisplayDialog("警告", "路径转换失败，请检查路径文件", "确定");
                            return;
                        }
						string md5path = EditorUtility.SaveFilePanel ("选择新MD5文件生成路径", string.Format("{0}/Res/md5file4assetbundle", Application.dataPath), "AAB_md5", "txt");
						if(md5path != "")
							WriteFile(md5path);
						AssetDatabase.Refresh();
					}
				}
            //}

			EditorGUILayout.LabelField ("选择平台");
			ios = EditorGUILayout.BeginToggleGroup ("IOS", !android && !PC);
			EditorGUILayout.EndToggleGroup ();
			
			android = EditorGUILayout.BeginToggleGroup ("ANDROID", !ios && !PC);
			EditorGUILayout.EndToggleGroup ();
			
			PC = EditorGUILayout.BeginToggleGroup ("PC", !ios && !android);
			EditorGUILayout.EndToggleGroup ();
		}
		GUILayout.EndVertical ();
	}

	bool L18NPath()
	{
		localPath = cns ? "Chinese_Simplified" : cnt ? "Chinese_Traditional" : en ? "English" : "";
		L18NPathFile = pathFile.text.Replace ("I18N", localPath);
		return true;
	}

    bool PlatformPath()
    {
        localPath = ios ? "AssetBundleIOS" : (android ? "AssetBundleAndroid" : "AssetBundlePC");
        PlatformPathFile = pathFile.text.Replace("#Platform#", localPath);
        return true;
    }

	void parsePathFile()
	{
        string[] tip = PlatformPathFile.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		string[] inOut;
		string fullName = "";
		for(int i=0; i < tip.Length; ++i)
		{
			if(tip[i].IndexOf("--") != -1)
				continue;
			tip[i] = tip[i].Replace("-in ", "").Replace("-out ", "");
			inOut = tip[i].Split(' ');
			fullName = convert2Name(inOut[0]);
			if(!pathFiles.ContainsKey(fullName))
				pathFiles.Add(fullName, inOut);
			else
				pathFiles[fullName] = inOut;
			if(LaunchFiles(inOut[0]) == false)
			{
				ClientLog.Instance.Log(inOut[0] + " has error!");
				continue;
			}
		}
	}

	void parseMd5File()
	{
		string[] tip = md5File.text.Split (new char[]{'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		string[] pair;
		for(int i=0; i < tip.Length; ++i)
		{
			pair = tip[i].Split(',');
			if(!md5Files.ContainsKey(pair[0]))
				md5Files.Add(pair[0], pair[1]);
			else
				md5Files[pair[0]] = pair[1];
		}
	}

	bool LaunchFiles(string folder)
	{
		folder = convertPath (folder);
		folder = cutAfterAssetFolder (folder);
		ListFile(new DirectoryInfo(string.Format("{0}/{1}", Application.dataPath, folder)));
		string fullName = "";
		string md5Str = "";
		bool needPack = false;
		for(int i=0; i < pathList.Count; ++i)
		{
			fullName = convert2Name(cut2AssetFolder(pathList[i]));
			md5Str = md5(pathList[i]);
			if(!md5Dic.ContainsKey(fullName))
				md5Dic.Add(fullName, md5Str);
			else
				md5Dic[fullName] = md5Str;
			if(checkMd5(fullName, md5Str) == false)
			{
				needPack = true;
			}
		}
		if (needPack)
		{
			//pack
			string assetFullPath = cut2AssetFolder(string.Format("{0}/{1}", Application.dataPath, folder));
			Pack(assetFullPath, pathFiles[convert2Name(assetFullPath)][1]);
		}
		pathList.Clear();
		return true;
	}

	void ListFile(FileSystemInfo info){
		if(!info.Exists){
			if(info.Name.Contains(".") && info.Extension != ".meta")
			{
				pathList.Add(info.FullName);
			}
			return;
		}
		DirectoryInfo dir = info as DirectoryInfo;
		if(dir == null){
			return;
		}
		FileSystemInfo[] files = dir.GetFileSystemInfos();
		for(int i=0; i < files.Length; ++i){
			FileInfo file = files[i] as FileInfo;
			if(file != null){
				if(file.Extension != ".meta")
					pathList.Add(file.FullName);
			}
			else
				ListFile(files[i]);
		}
	}

	void Pack(string path, string pathName)
	{
		Caching.CleanCache();
		List<Object> files = new List<Object>();
		if (path.Contains ("."))
		{
			string filePath = cut2AssetFolder (path);
            files.Add(AssetDatabase.LoadAssetAtPath(filePath, typeof(Object)));
		}
		else
		{
			string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
			for(int i=0; i < filePaths.Length; ++i)
			{
				string filePath = cut2AssetFolder(filePaths[i]);
				files.Add(AssetDatabase.LoadAssetAtPath(filePath, typeof(Object)));
			}
		}

		if (!pathName.Contains ("."))
		{
			string tName = path.Substring(path.LastIndexOf("/"));
			if(tName == "/")
				tName = path.Substring(path.Remove(path.LastIndexOf("/")).LastIndexOf("/")).Replace("/", "");
			if(tName.Contains("."))
				tName = tName.Remove(tName.IndexOf("."));
			pathName = string.Format ("{0}/{1}{2}", pathName, tName, ".bytes");
		}
        for (int i = 0; i < files.Count; ++i )
        {
            BuildTarget tar = ios ? BuildTarget.iPhone : (android ? BuildTarget.Android : BuildTarget.StandaloneWindows);
            BuildPipeline.BuildAssetBundle(files[i], null, pathName, BuildAssetBundleOptions.CollectDependencies, tar);
        }
	}

	string md5(string path){
		byte[] data = File.ReadAllBytes(path);
		StringBuilder sb = new StringBuilder();
		foreach(byte b in data){
			sb.Append(b.ToString());
		}
		
		char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
		MD5 md5 = MD5.Create();
		byte[] input = Encoding.UTF8.GetBytes(sb.ToString());
		byte[] hash = md5.ComputeHash(input);
		sb = new StringBuilder();
		for(int i=0; i < hash.Length; ++i){
			int n = hash[i];
			if(n < 0)
				n = 256 + n;
			int d1 = n / 16;
			int d2 = n % 16;
			sb.Append(hexDigits[d1].ToString() + hexDigits[d2].ToString());
		}
		return sb.ToString();
	}

	void WriteFile(string outPath){
		if(File.Exists(outPath))
			File.Delete(outPath);
		string content = "";
		foreach(string key in md5Dic.Keys){
			content += key + "," + md5Dic[key] + "\r\n";
		}
		File.WriteAllText(outPath, content);
	}

	bool checkMd5(string keyName, string md5Str)
	{
		if (md5File == null)
			return false;
		if (!md5Files.ContainsKey (keyName))
			return false;
		return md5Files [keyName].Equals (md5Str);
	}

	string convertPath(string src)
	{
		return src.Replace ("\\", "/");
	}

	string cutAfterAssetFolder(string src)
	{
		return src.Substring (src.IndexOf("Assets/") + "Assets/".Length);
	}

	string cut2AssetFolder(string src)
	{
		return src.Substring (src.IndexOf("Assets"));
	}

	string convert2Name(string src)
	{
		return src.Replace ("/", "~").Replace("\\", "~");
	}
}
