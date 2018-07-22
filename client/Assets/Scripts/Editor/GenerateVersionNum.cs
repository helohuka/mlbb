using System;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
//using System.Diagnostics;

public class GenerateVersionNum : MonoBehaviour
{
	[MenuItem("Tools/Md5Num")]
	public static void GenerateVersion()
	{
		string path = Application.streamingAssetsPath + "/AssetBundleAndroid/";
		Dictionary<string, string>dicNewMD5Info = new Dictionary<string, string>();

		MakeFileMD5(path, ref dicNewMD5Info);

		string savePath = path + "Version";
		if(Directory.Exists(savePath) == false)
		{
			Directory.CreateDirectory(savePath);
		}

		IDictionary dicVersionNumInfo = new Dictionary<string,string>();

		WriteInfo(savePath + "/VersionMD5.txt", dicNewMD5Info);

	}

	private static MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
	private  string PRE_PATH = "";

	static void MakeFileMD5(string path, ref Dictionary<string, string>DicFileMD5)
	{
		List<string> fileEntries;

		DirectoryInfo di = new DirectoryInfo (path);
		FileInfo[] fi = di.GetFiles ();
		for(int i=0; i < fi.Length; ++i)
		{
			if(fi[i].Extension.Equals(".meta") ||fi[i].Extension.Equals(".svn"))
				continue;

			FileStream file = new FileStream(fi[i].FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] hash = md5Generator.ComputeHash(file);
			string strMD5 = System.BitConverter.ToString(hash);
			file.Close();

			string key = fi[i].FullName;//.Replace(Application.dataPath + "/", "");//:AssetBundle/Mac/Attributes/Attributes
			
			key = key.Replace("\\","/");
			key = key.Replace(Application.streamingAssetsPath, "");
			//key = key.Replace("AssetBundleIOS", "AssetBundleAndroid");

			ClientLog.Instance.Log(key);
			if(key == "") 
			{
				continue;
			}
			if (DicFileMD5.ContainsKey(key) == false)
			{
				DicFileMD5.Add(key, strMD5);
			}
			else
				ClientLog.Instance.LogWarning("<Two File has the same name> name = " + fi[i].FullName);
		}


		DirectoryInfo[] infos = di.GetDirectories ();
		for(int i =0; i<infos.Length;i++)
		{
			MakeFileMD5( infos[i].FullName, ref DicFileMD5);
		}
	}

	private static void WriteInfo(string outpath, IDictionary filedic)
	{
		if(File.Exists(outpath))
		{
			File.Delete(outpath);
		}
		
		FileStream fs = new FileStream(outpath,FileMode.CreateNew);
		StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII);
		int count = 0;
		foreach(string key in filedic.Keys)
		{
			if(count > 0)
			{
				sw.Write(",");
			}
			if(filedic[key].GetType() == typeof(string))
			{
				sw.Write(key + ";" + filedic[key]);
			}
			else
			{
				sw.Write(key + ";" + filedic[key]);
			}
			
			count++;
		}
		sw.Close();
		fs.Close();
	}

	[MenuItem("Tools/streamingAssetsPath")]
	public static void GenerateStreamingPath()
	{
		string path = Application.streamingAssetsPath + "/AssetBundleAndroid/";
		Dictionary<string, string>dicPath = new Dictionary<string, string>();
		
		MakeCopyPath(path, ref dicPath);
		
		string savePath = path + "StreamingPath";
		if(Directory.Exists(savePath) == false)
		{
			Directory.CreateDirectory(savePath);
		}
		
		IDictionary dicVersionNumInfo = new Dictionary<string,string>();
		
		WriteStreamingPathInfo(savePath + "/StreamingPath.txt", dicPath);
		
	}




	static void MakeCopyPath(string path, ref Dictionary<string, string>DicFileMD5)
	{
		List<string> fileEntries;
		
		DirectoryInfo di = new DirectoryInfo (path);
		FileInfo[] fi = di.GetFiles ();
		for(int i=0; i < fi.Length; ++i)
		{
			if(fi[i].Extension.Equals(".meta") )
				continue;
			
			FileStream file = new FileStream(fi[i].FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] hash = md5Generator.ComputeHash(file);
			string strMD5 = System.BitConverter.ToString(hash);
			file.Close();
			//				PRE_PATH = Assets/AssetBundle/Mac/

			string key = fi[i].FullName;//.Replace(Application.dataPath + "/", "");//:AssetBundle/Mac/Attributes/Attributes

			key = key.Replace("\\","/");
			key = key.Replace(Application.streamingAssetsPath, "");

			ClientLog.Instance.Log(key);
			if(key == "") 
			{
				continue;
			}
			if (DicFileMD5.ContainsKey(key) == false)
			{
				DicFileMD5.Add(key, strMD5);
			}
			else
				ClientLog.Instance.LogWarning("<Two File has the same name> name = " + fi[i].FullName);
		}
		
		
		DirectoryInfo[] infos = di.GetDirectories ();
		for(int i =0; i<infos.Length;i++)
		{
			MakeCopyPath( infos[i].FullName, ref DicFileMD5);
		}
	}
	
	private static void WriteStreamingPathInfo(string outpath, IDictionary filedic)
	{
		if(File.Exists(outpath))
		{
			File.Delete(outpath);
		}
		
		FileStream fs = new FileStream(outpath,FileMode.CreateNew);
		StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII);
		int count = 0;
		foreach(string key in filedic.Keys)
		{

			if(filedic[key].GetType() == typeof(string))
			{
				sw.Write(key + ";" );
			}
			else
			{
				sw.Write(key + ";");
			}
			
			count++;
		}
		sw.Close();
		fs.Close();
	}

	[MenuItem("Tools/SevenZipFile")]
	public static  void CheckVersion()
	{
		List<string> dicVersionNumInfo = new List<string>();
		if(File.Exists(Application.persistentDataPath + "/VersionMD5.txt"))
		{
			
			Dictionary<string,string>  OldMD5Info = GetMd5List(File.ReadAllText(Application.persistentDataPath + "/VersionMD5.txt"));
			Dictionary<string,string>  MD5Info = GetMd5List(File.ReadAllText(Configure.assetsPathstreaming+"Version/VersionMD5.txt"));
			
			foreach (KeyValuePair<string, string> newPair in MD5Info)
			{
				if (OldMD5Info.ContainsKey(newPair.Key) == false)
				{
					dicVersionNumInfo.Add(newPair.Key);
				}
				else if (newPair.Value.ToString() != OldMD5Info[newPair.Key].ToString())
				{
					dicVersionNumInfo.Add(newPair.Key );
				}
			}
			
		}
		else
		{

			Dictionary<string,string>  MD5Info1 = GetMd5List(File.ReadAllText(Application.dataPath + "/VersionMD5.txt"));
			
			foreach (KeyValuePair<string, string> newPair in MD5Info1)
			{
				dicVersionNumInfo.Add(newPair.Key );
			}
		}

		foreach(string s in dicVersionNumInfo)
		{
			string[] strArr =  s.Split('/');
			string spath = s.Replace(strArr[strArr.Length -1],"");
			if(Directory.Exists(Application.dataPath+ "/7z/"+spath) == false)
			{
				Directory.CreateDirectory((Application.dataPath+ "/7z/"+spath));
			}

			string source = (Application.streamingAssetsPath +s).Replace("\n","").Replace("\t","").Replace(@"/",@"\\");
			string targrt = (Application.dataPath+ "/7z"+s).Replace("\n","").Replace("\t","").Replace(@"/",@"\\");
			File.Copy(source,targrt,true);
		}

	}

	private  static Dictionary<string,string> GetMd5List(string text)
	{		
		Dictionary<string, string> keyV = new Dictionary<string, string> ();
		if(text.Length == 0)
			return keyV;
		string[] strArr = text.Split(',');
		for(int i= 0;i< strArr.Length;i++)
		{
			string[] sArr = strArr[i].Split(';');
			
			keyV.Add(sArr[0],sArr[1]);
		}
		
		return keyV;
	}
	
}

