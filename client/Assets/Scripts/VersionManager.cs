
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityHTTP;

public class VersionManager
{
	public static string VERSION_BK_FILE = "Version.txt";  //资源服务器版本号文件
	public static string VERSION_BK_MD5 = "VersionMD5.txt";  //资源服务器资源MD5文件
    public static string VERSION_BK_STREAM = "StreamingPath.txt";

	private static VersionManager _instance = null;
	public bool _isCheck;
	private bool _isDowning;
	private bool _isCopyDowning;

	bool isStartMainVersion;
	bool isMd5Down;
	int num= 0;
	bool isDowning;
	Request file;
	bool isDownPath;
	bool isStartDownMd5; 
	Request copyPathW;
	Request copyPathFile;
	int copyNum= 0;
	bool isCopyDowning;
	public bool _isCopyStreaming;
	bool isCopyStreaming;
	string[] pathStrArr;
	public bool isVersionChange;
	

	public RequestEventHandler<int> startDownFileEvent;
	public RequestEventHandler<int> downFileEvent;
	public RequestEventHandler<int> finishDownFileEvent;
	public RequestEventHandler<int> CopyEvent;
	public RequestEventHandler<int> startCopyEvent;
	public RequestEventHandler<string> versionNumEvent;
	public RequestEventHandler<string> versionDataNumEvent;
	public static VersionManager Instance
	{
		get{
			if(_instance == null)
			{
				_instance = new VersionManager();
			}
			return _instance;
		}
	}


	public VersionManager()
	{
		_instance = this;
		Start();
	}


	public void Start()
	{

	}

	
	List<string> dicVersionNumInfo = new List<string>(); 
	List<string> dicCopyInfo = new List<string>();
	Request MD5 ;
	Request MainVersion ;



	// 比较资源MD5 .
	public void CheckVersion()
	{
        //if (File.Exists(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5))
        //{

        //    Dictionary<string, string> OldMD5Info = GetMd5List(File.ReadAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5));
        //    Dictionary<string,string>  MD5Info = GetMd5List(MD5.response.Text);

        //    foreach (KeyValuePair<string, string> newPair in MD5Info)
        //    {
        //        if (OldMD5Info.ContainsKey(newPair.Key) == false)
        //        {
        //            dicVersionNumInfo.Add(newPair.Key);
        //        }
        //        else if (newPair.Value.ToString() != OldMD5Info[newPair.Key].ToString())
        //        {
        //            dicVersionNumInfo.Add(newPair.Key );
        //        }
				
        //    }

        //}
        //else
        //{
        //    //OneCheckVersion();
        //    Dictionary<string,string>  MD5Info1 = GetMd5List(MD5.response.Text);
			
        //    foreach (KeyValuePair<string, string> newPair in MD5Info1)
        //    {
        //        dicVersionNumInfo.Add(newPair.Key );
        //    }
        //}

        //if(dicVersionNumInfo.Count > 0)
        //{
        //    _isDowning = true;
        //}
        //else
        //{
        //    File.WriteAllText(CommonDefines.persistentDataPath + "/CopyVersion.txt", MainVersion.response.Text);
        //    if(finishDownFileEvent != null)
        //    {
        //        finishDownFileEvent(dicVersionNumInfo.Count);
        //    }
        //    if(versionNumEvent != null)
        //    {
        //        string[] gamePathStrArr = MainVersion.response.Text.Split(';');
        //        versionNumEvent(gamePathStrArr[0]);
        //    }
        //}
	}


	public void  CheckStreamingCopy()
	{
        //copyFileToPre(Configure.assetsPathstreaming,ref dicCopyInfo);
        //if(dicCopyInfo.Count  > 0)
        //{
        //    _isCopyDowning = true;
        //}
        //else
        //{
        //    isStartDownMd5 = true;
        //}
	}


	public void copyFileToPre(string path, ref List<string> dic)
	{
        //List<string> fileEntries;
        //DirectoryInfo di = new DirectoryInfo (path);
        //FileInfo[] fi = di.GetFiles ();
        //for(int i=0; i < fi.Length; ++i)
        //{
        //    if(fi[i].Extension.Equals(".meta") || fi[i].Extension.Equals(".svn"))
        //        continue;
			
        //    string key = fi[i].FullName;

        //    if(key == "") 
        //    {
        //        continue;
        //    }
        //    key = key.Replace("\\","/");
        //    key = key.Replace(Application.streamingAssetsPath, "");

        //    dic.Add(key);
        //}
		
        //DirectoryInfo[] infos = di.GetDirectories ();
        //for(int i =0; i<infos.Length;i++)
        //{
        //    copyFileToPre( infos[i].FullName, ref dic);
        //}
	}


	public void CheckMainVersion()
	{
        //if (!File.Exists(CommonDefines.persistentDataPath + "/CopyVersion.txt"))
        //{
        //    isStartDownMd5 = true;
        //    if(versionNumEvent != null)
        //    {
        //        string[] gamePathStrArr = MainVersion.response.Text.Split(';');
        //        versionNumEvent(gamePathStrArr[0]);
        //    }
        //    //CheckStreamingCopy();
        //}
        //else
        //{
        //    string vText = MainVersion.response.Text.Trim ();
        //    String[] gamePathStrArr = vText.Split(';');
        //    String[] serverStrArr = gamePathStrArr[0].Split('.');

        //    String[] strArr = File.ReadAllText(CommonDefines.persistentDataPath + "/CopyVersion.txt").Split(';');
        //    String[] strArr1 = strArr[0].Split('.');

        //    //String ss1 = serverStrArr[0];
        ////	String ss2 = strArr1[0];
        ////	Debug.Log(ss1);
        ////	Debug.Log(ss2);
        ////	int aa1 = int.Parse(ss2);
        ////	int aa2 = int.Parse(ss1);
        ////	int aa = Convert.ToInt32(ss1);//int.Parse(ss1);



        //    /*if((int.Parse(ss1.ToString())) != (int.Parse(ss2.ToString())) )
        //    {
        //        int a= 0;
        //    }
        //    if(!serverStrArr[0].Equals(strArr1[0]))
        //    {
        //        isVersionChange = true;
        //        Application.OpenURL(gamePathStrArr[1]); // 大版本号变 重新下载

        //        return;
        //    }
        //    else if(!serverStrArr[1].Equals(strArr1[1]))
        //    {
        //        isVersionChange = true;
        //        isStartDownMd5 = true;
        //    }
        //    else if(!serverStrArr[2].Equals(strArr1[2]))
        //    {
        //        isVersionChange = true;
        //        isStartDownMd5 = true;
        //    }
        //    else
        //    {0
        //        isVersionChange = false;
        //        finishDownFileEvent(1);
        //    }
        //    */
        //    isVersionChange = true;
        //    isStartDownMd5 = true;
        //}
	}


	public void Update()
	{
        //if(_isCheck )
        //{
        //    if(!isStartMainVersion)
        //    {
        //        try
        //        {
        //            MainVersion  = new Request("get", GlobalValue.cdnservhost + VERSION_BK_FILE);
        //            MainVersion.Send();
        //        }
        //        catch(Exception e)
        //        {
        //            Debug.Log("Download Exception!!! " + e.ToString());
        //            //if(finishDownFileEvent != null)
        //            ////{
        //                //finishDownFileEvent(1);
        //            //}
        //        }
        //        isStartMainVersion = true;
        //    }
        //    if( MainVersion.isDone)
        //    {
        //        _isCheck =false;
        //        CheckMainVersion();
        //    }

        //}

		
        //if(isStartDownMd5)
        //{
        //    if(!isMd5Down)
        //    {
        //        MD5 = new Request("get", GlobalValue.cdnservhost + VERSION_BK_MD5);
        //        MD5.Send();
        //        isMd5Down = true;
        //    }
        //    if(MD5.isDone)
        //    {
        //        isStartDownMd5 =false;
        //        CheckVersion();
        //    }
        //}
			
        //if(_isDowning)
        //{
        //    if(startDownFileEvent != null)
        //    {
        //        startDownFileEvent(dicVersionNumInfo.Count);
        //    }

        //    if(!isDowning)
        //    {
        //        dicVersionNumInfo[num] = dicVersionNumInfo[num].Replace("\n","").Replace("\t","").Replace("{","").Replace("\"","");
        //        file = new Request("get", GlobalValue.cdnservhost + dicVersionNumInfo[num]);//"http://10.10.10.254/down/Version/VersionMD5.txt");
        //        file.Send();


        //        isDowning = true;
        //        if(downFileEvent !=null)
        //            downFileEvent(num +1);
        //    }
        //    if(isDowning)
        //    {
        //        float bytNum = file.response.bytes.Length/file.bufferSize/1024;
        //        float bytDownNum = file.response.bytes.Length/1024f;
        //        if(versionDataNumEvent != null)
        //            versionDataNumEvent(num +"/"+dicVersionNumInfo.Count+"  ("+bytDownNum.ToString("#0.0") + "KB/" +bytNum.ToString("#0.0")+"KB)");
        //    }
        //        // file.progress = 0;//0~0.9;

        //    if(file.isDone)
        //    {

        //        string[] strArr = dicVersionNumInfo[num].Split('/');
        //        string spath = dicVersionNumInfo[num].Replace(strArr[strArr.Length -1],"");


        //        if (Directory.Exists(CommonDefines.persistentDataPath + spath) == false)
        //        {
        //            Directory.CreateDirectory(CommonDefines.persistentDataPath + spath);

        //        }
        //        File.WriteAllBytes(CommonDefines.persistentDataPath + dicVersionNumInfo[num], file.response.bytes);


        //        Dictionary<string,string>  MD5Info = GetMd5List(MD5.response.Text);
        //        if (File.Exists(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5))
        //        {
        //            Dictionary<string, string> OldMD5Info = GetMd5List(File.ReadAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5));

        //            string oldMD5InfoString = File.ReadAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5);
        //            if (OldMD5Info.ContainsKey(spath) == false)
        //            {
        //                File.AppendAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5, "," + dicVersionNumInfo[num] + ";" + MD5Info[dicVersionNumInfo[num]]);
        //            }
        //            else
        //            {
        //                oldMD5InfoString.Replace(dicVersionNumInfo[num]+";"+OldMD5Info[dicVersionNumInfo[num]] , dicVersionNumInfo[num]+";"+MD5Info[dicVersionNumInfo[num]]);
        //                File.WriteAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5, oldMD5InfoString);

        //            }
        //        }
        //        else
        //        {
        //            File.AppendAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5, dicVersionNumInfo[num] + ";" + MD5Info[dicVersionNumInfo[num]]);
        //        }

        //        num ++;
        //        downFileEvent(num+1);
        //        //file = null;
        //        isDowning = false;
        //    }

        //    if(dicVersionNumInfo.Count > 0 && num == dicVersionNumInfo.Count)
        //    {
        //        if(finishDownFileEvent != null)
        //        {
        //            _isDowning = false;
        //            finishDownFileEvent(dicVersionNumInfo.Count);
        //            File.WriteAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_MD5, MD5.response.Text);
        //            File.WriteAllText(CommonDefines.persistentDataPath + "/CopyVersion.txt", MainVersion.response.Text);
        //            string[] strArr = MainVersion.response.Text.Split(';');
        //            PlayerPrefs.SetString ("Version",strArr[0]);
        //        }
				
        //    }
		
        //}

        //if(_isCopyStreaming )
        //{

        //    if (File.Exists(CommonDefines.persistentDataPath + "/" + VERSION_BK_STREAM))
        //    {
        //        _isCopyStreaming = false;
				
        //        if(CopyEvent != null)
        //            CopyEvent(copyNum +1);
        //        return;
        //    }
        //    if(!isCopyStreaming)
        //    {
        //        try
        //        {
        //            copyPathW = new Request("get", GlobalValue.cdnservhost + VERSION_BK_STREAM);
        //            copyPathW.Send();
        //        }
        //        catch(Exception e)
        //        {
        //            if(finishDownFileEvent != null)
        //            {
        //                finishDownFileEvent(1);
        //            }
        //        }
        //        isCopyStreaming = true;
        //    }
        //    if(isCopyStreaming)
        //    {
        //        //float bytNum = file.bytesDownloaded/file.progress/1024;
        //        //float bytDownNum = file.bytesDownloaded/1024f;
        //        //if(versionDataNumEvent != null)
        //        //	versionDataNumEvent(num +"/"+dicVersionNumInfo.Count+"  ("+bytDownNum.ToString("#0.0") + "KB/" +bytNum.ToString("#0.0")+"KB)");
        //    }
        //    if( copyPathW.isDone)
        //    {
        //        _isCopyStreaming =false;
        //        pathStrArr =  copyPathW.response.Text.Split(';');
        //        _isCopyDowning = true;
        //    }
			
        //}
        ////copy to persistent
        //if(_isCopyDowning)
        //{
        //    if(startCopyEvent != null)
        //    {
        //        startCopyEvent(pathStrArr.Length);
        //    }

        //    if(!isCopyDowning)
        //    {
        //        pathStrArr[copyNum] = pathStrArr[copyNum].Replace("\n","").Replace("\t","").Replace("{","").Replace("\"","");
        //        string str1=  pathStrArr[copyNum].Replace("/StreamingAssets","");
        //        copyPathFile  = new Request("get", Application.streamingAssetsPath + str1);
        //        copyPathFile.Send();
        //        isCopyDowning = true;
        //        if(downFileEvent !=null)
        //            downFileEvent(copyNum +1);
        //    }
        //    if(copyPathFile.isDone)
        //    {
        //        string[] strArr =  pathStrArr[copyNum].Split('/');
        //        string spath = pathStrArr[copyNum].Replace(strArr[strArr.Length -1],"");

        //        if (Directory.Exists(CommonDefines.persistentDataPath + spath) == false)
        //        {
        //            Directory.CreateDirectory(CommonDefines.persistentDataPath + spath);
        //        }

        //        File.WriteAllBytes(CommonDefines.persistentDataPath + pathStrArr[copyNum], copyPathFile.response.bytes);
        //        copyNum ++;
        //        if(downFileEvent !=null)
        //            downFileEvent(copyNum +1);
        //        isCopyDowning = false;

        //    }
        //    if(pathStrArr.Length > 0 && copyNum == pathStrArr.Length-1)
        //    {
        //        _isCopyDowning = false;

        //        if(CopyEvent != null)
        //            CopyEvent(copyNum +1);
        //        File.WriteAllText(CommonDefines.persistentDataPath + "/" + VERSION_BK_STREAM, copyPathW.response.Text);
        //    }
        //}
	}

	
	private  Dictionary<string,string> GetMd5List(string text)
	{		
        //text = text.Trim ();
		Dictionary<string, string> keyV = new Dictionary<string, string> ();
        //if(text.Length == 0)
        //    return keyV;
        //string[] strArr = text.Split(',');

        //for(int i= 0;i< strArr.Length;i++)
        //{
        //    string[] sArr = strArr[i].Split(';');

        //    if(keyV.ContainsKey(sArr[0]))
        //    {
        //        keyV[sArr[0]] = sArr[1];
        //        continue;
        //    }
        //    keyV.Add(sArr[0],sArr[1]);
        //}

		return keyV;
	}





}