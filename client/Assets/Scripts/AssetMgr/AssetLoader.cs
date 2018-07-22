using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetLoader
{
	//Assets Types
	public enum EAssetType : int
	{
		ASSET_PLAYER ,
		ASSET_UI ,
		ASSET_EFFECT ,
		ASSET_STAGE ,
		ASSET_ICON , 
		ASSET_SOUND , 
		ASSET_MUSIC ,
		ASSET_MAZE ,
		ASSET_WEAPON ,
		ASSET_TYPE_MAX,
	}

	class AssetLoadParam
	{
		public	AssetLoadCallback				CallBack;
		public  ParamData               		Param;
		//params object[] 					paramData; 	
	}
	public delegate void AssetLoadCallback(AssetBundle Asset, ParamData Param);
	class AssetLoadInfo
	{
		public EAssetType		AssetType ;
		public string			szAssetName;
		public WWW				AssetBytes;
		public AssetLoadParam	LoadParam;
	}

	static List<AssetLoadInfo>		m_AssetLoaderLst;
	static bool						m_bInitAssetLoad = false;
	static string[]					m_AssetTypeName;

	static void Init()
	{
		if(m_bInitAssetLoad ) return ;
		m_bInitAssetLoad = true;
		m_AssetLoaderLst = new List<AssetLoadInfo>();

		m_AssetTypeName = new string[(int)EAssetType.ASSET_TYPE_MAX];
		m_AssetTypeName[(int)EAssetType.ASSET_EFFECT] = "Effect";
		m_AssetTypeName[(int)EAssetType.ASSET_UI] = "UI";
		m_AssetTypeName[(int)EAssetType.ASSET_ICON] = "Icon";
		m_AssetTypeName[(int)EAssetType.ASSET_MUSIC] = "Music";
		m_AssetTypeName[(int)EAssetType.ASSET_PLAYER] = "Player";
		m_AssetTypeName[(int)EAssetType.ASSET_SOUND] = "Sound";
		m_AssetTypeName[(int)EAssetType.ASSET_STAGE] = "Scene";
		m_AssetTypeName [(int)EAssetType.ASSET_MAZE] = "Maze";
		m_AssetTypeName [(int)EAssetType.ASSET_WEAPON] = "Weapon";
	}

	void Start()
	{
		Init();
	}
	static public void LoadAssetBundle( string szAssetName , EAssetType	Type , AssetLoadCallback	Callback ,  ParamData paramData, string replacePath = "")//params object[] paramData )
	{
		Init();
		bool	bExists = HandleExists( szAssetName );
		AssetLoadInfo info = new AssetLoadInfo();
		info.AssetType = Type;
		if( null != paramData )
			paramData.szAssetName = szAssetName;
		info.szAssetName = szAssetName;

		string assetPath1 = GetAssetPath( Type , szAssetName, replacePath);
		assetPath1 = assetPath1.Replace ("file:///", "");
		assetPath1 = assetPath1.Replace ("file://", "");
		if(!File.Exists(assetPath1))
		{
			if (Type == EAssetType.ASSET_STAGE)
			{
				Callback(null,null);
				return;
			}
			replacePath =  Configure.assetsPathstreaming;
		}
		else
		{
			/*if(File.Exists(Application.persistentDataPath+ "/CopyVersion.txt"))
			{
				string nowVer = File.ReadAllText(Application.persistentDataPath + "/CopyVersion.txt");
				string ver = GameManager.Instance.GetVersionNum();
				string[] nowSreArr = nowVer.Split('.');
				string[] sreArr = ver.Split('.');
				int nowNum = 0;
				int num =0;

				nowNum = int.Parse(nowSreArr[0])*100 + int.Parse(nowSreArr[1])*10 + int.Parse(nowSreArr[2]);
				num = int.Parse(sreArr[0])*100 + int.Parse(sreArr[1])*10 + int.Parse(sreArr[2]);

				if(num > nowNum) 
				{
					replacePath =  Configure.assetsPathstreaming;
				}
			}
			*/
		}

		if( bExists )
		{
			info.AssetBytes = null;
		}
		else
		{
			string assetPath = GetAssetPath( Type , szAssetName, replacePath);

            if (Type == EAssetType.ASSET_STAGE)
                info.AssetBytes = WWW.LoadFromCacheOrDownload(assetPath, 0);
            else
			    info.AssetBytes = new WWW( assetPath );
		}
		info.LoadParam = new AssetLoadParam();
		info.LoadParam.CallBack = Callback;
		info.LoadParam.Param = paramData;

		m_AssetLoaderLst.Add( info );
	}
	
	static public void Update()
	{
		if( null == m_AssetLoaderLst || m_AssetLoaderLst.Count <= 0 )
		{
			return ;
		}

		for( int iCount = 0; iCount < m_AssetLoaderLst.Count; ++ iCount )
		{
			bool	bDeal = false;
			AssetLoadInfo	loadInfo = m_AssetLoaderLst[iCount];
			AssetInfo	info = AssetInfoMgr.Instance.FindAssetInfo( loadInfo.szAssetName ); 
			if( null != info )
			{
				if( null != loadInfo.LoadParam.CallBack )
				{
					loadInfo.LoadParam.CallBack( info.Assetbundle , loadInfo.LoadParam.Param );
					bDeal = true;
				}
			}
			else
			{
				if( null != loadInfo.AssetBytes )
				{
					if( null == loadInfo.AssetBytes.error && loadInfo.AssetBytes.isDone )
					{
						bDeal = true;
						if( null != loadInfo.AssetBytes.assetBundle )
						{
                            AssetInfoMgr.Instance.AddAssetInfo(loadInfo.szAssetName, loadInfo.AssetBytes.assetBundle, 1);
							if( null != loadInfo.LoadParam.CallBack )
							{
								loadInfo.LoadParam.CallBack( loadInfo.AssetBytes.assetBundle , loadInfo.LoadParam.Param );
							}
						}
					}
                    else if (loadInfo.AssetBytes.error != null)
                    {
                        bDeal = true;
                        if (loadInfo.szAssetName.Contains("_dep"))
                        {
                            if (null != loadInfo.LoadParam.CallBack)
                            {
                                loadInfo.LoadParam.CallBack(null, loadInfo.LoadParam.Param);
                            }
                        }
                        else
                        {
                            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("asset") + loadInfo.szAssetName + LanguageManager.instance.GetValue("cannotFind"), () =>
                            {
                                Application.Quit();
                            }, true);
                        }
                        ClientLog.Instance.LogError(loadInfo.AssetBytes.url + loadInfo.AssetBytes.error.ToString());
                    }
				}
			}

			if( bDeal )
			{
                if (loadInfo.AssetBytes != null)
                    loadInfo.AssetBytes.Dispose();
				loadInfo.AssetBytes = null;
				m_AssetLoaderLst.Remove( loadInfo );
                iCount--;
			}
		}
	}

	static string GetAssetPath( EAssetType	Type , string szAssetName, string repPath= "")
	{
        string PathURL = string.IsNullOrEmpty(repPath)? Configure.assetsPath: repPath;
		return PathURL += GetAssetName( Type ) + "/" + szAssetName + (Type == EAssetType.ASSET_STAGE? ".unity3d": ".bytes");
	}

	static string GetAssetName( EAssetType	Type )
	{
		return m_AssetTypeName[(int)Type];
	}

	static bool HandleExists( string szAssetName )
	{
		AssetInfo info = AssetInfoMgr.Instance.AddRefCount( szAssetName );
		if( null != info )
		{
			return true;
		}
		return false;
	}

    static public void ShowWhoIsDownLoading()
    {
        for (int iCount = 0; iCount < m_AssetLoaderLst.Count; ++iCount)
        {
            ClientLog.Instance.Log(m_AssetLoaderLst[iCount].AssetBytes.url + "     name" + m_AssetLoaderLst[iCount].szAssetName);
        }
    }
}





enum APPLE_db57f116b1184d1187d9206211b8d7c4
{

}