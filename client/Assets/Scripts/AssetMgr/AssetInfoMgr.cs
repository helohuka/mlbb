using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetInfo
{
	public string				szName;
	public AssetBundle			Assetbundle;
	public int					iRef;
	
	public AssetInfo()
	{
		szName			= null;
		Assetbundle		= null;
		iRef			= 0;
	}
	
	public AssetInfo(string strName, AssetBundle Bundle, int iRefCount)
	{
		szName			= strName;
		Assetbundle		= Bundle;
		iRef			= iRefCount;
	}
}

public class AssetInfoMgr 
{
	#region Singletion
	
	static readonly AssetInfoMgr s_Instance = new AssetInfoMgr();
	
	static public AssetInfoMgr Instance
	{
		get
		{
			if (s_Instance == null)
			{
				ClientLog.Instance.LogErrorEx("Singletion is null");
			}
			return s_Instance;
		}
	}
	
	#endregion
	
	#region val
	// 
	static private Dictionary<string, AssetInfo> m_dicAssetInfo;
	
	#endregion
	// 
	private AssetInfoMgr()
	{
		m_dicAssetInfo = new Dictionary<string, AssetInfo>();
	}
	
	#region Interface
	// 
	public AssetInfo FindAssetInfo(string strAssetName)
	{
		if (strAssetName == null || strAssetName.Length <= 0 || m_dicAssetInfo == null)
		{
			//ClientLog.Instance.LogErrorEx("AssetMgrBase GetAssetInfo error");
			return null;
		}
		
		if (m_dicAssetInfo.ContainsKey(strAssetName) == true)
		{
			AssetInfo Info = m_dicAssetInfo[strAssetName];
			if( null == Info.Assetbundle || null == Info.Assetbundle.mainAsset )
			{
				m_dicAssetInfo.Remove(strAssetName);
				Info = null;
			}
			return Info;
		}
		
		return null;
	}

    public bool ForceClearUnusedbundle(string strAssetName)
    {
        if (strAssetName == null || strAssetName.Length <= 0 || m_dicAssetInfo == null)
        {
            //ClientLog.Instance.LogErrorEx("AssetMgrBase GetAssetInfo error");
            return false;
        }

        if (m_dicAssetInfo.ContainsKey(strAssetName))
        {
            if (m_dicAssetInfo[strAssetName].Assetbundle != null)
                ClientLog.Instance.LogWarning("Assetbundle in mgr is reallllllly not nulll!");
            m_dicAssetInfo.Remove(strAssetName);
            return true;
        }

        return false;
    }
	
	// 
	public AssetInfo FindAssetInfo(AssetBundle Asset)
	{
		if (Asset == null || Asset.mainAsset == null || m_dicAssetInfo == null)
		{
			return null;
		}

        if (m_dicAssetInfo.ContainsKey(Asset.mainAsset.name))
        {
            return m_dicAssetInfo[Asset.mainAsset.name];
        }
        
		return null;
	}
	
	
	 
	public void AddAssetInfo(string strAssetName, AssetBundle AssetData, int iRefCount)
	{
		if (m_dicAssetInfo == null)
		{
			ClientLog.Instance.LogErrorEx("m_dicAssetInfo is null");
			return ;
		}
		
		if (strAssetName == null || strAssetName.Length < 1)
		{
			ClientLog.Instance.LogErrorEx("AssetMgrBase AddAssetInfo error");
			return ;
		}
		
		AssetInfo ExistsInfo = FindAssetInfo(strAssetName);
		if( null != ExistsInfo )
		{
			ExistsInfo.Assetbundle	=	AssetData;
			ExistsInfo.iRef += iRefCount;
		}
		else
		{
			AssetInfo	Info	=	new AssetInfo( strAssetName, AssetData, iRefCount );
			m_dicAssetInfo.Add(strAssetName, Info);
		}		
	}
	
	//
	public	AssetInfo	AddRefCount( string strAssetName )
	{
		AssetInfo Info = FindAssetInfo(strAssetName);
		if( null != Info )
		{
			if( null == Info.Assetbundle )
			{
				m_dicAssetInfo.Remove( strAssetName );
				return null;
			}
			
			Info.iRef++;
			return Info;
		}
		return null;
	}
	
	// 
	public bool DecRefCount(string strAssetName, bool bUnloadAllObject)
	{
		AssetInfo Info = FindAssetInfo(strAssetName);
		if (Info == null)
		{
			return true;
		}
		
		Info.iRef--;
		return true;
	}
	
	public bool DecRefCount(AssetBundle Asset, bool bUnloadAllObject)
	{
		AssetInfo Info = FindAssetInfo(Asset);
		
		if (Info == null)
		{
			return true;
		}
		
		Info.iRef--;

        if (bUnloadAllObject)
            Info.iRef = 0;
		return true;
	}

    public void Update()
    {
        if (m_dicAssetInfo == null)
            return;

        foreach (AssetInfo child in m_dicAssetInfo.Values)
        {
            if (child == null)
            {
                continue;
            }

            if (child.iRef <= 0)
            {
                if(child.Assetbundle != null)
                    child.Assetbundle.Unload(false);
                child.Assetbundle = null;
                m_dicAssetInfo.Remove(child.szName);
                break;
            }
        }
    }
	
	public	void	ReleaseAllAsset()
	{
		foreach (AssetInfo child in m_dicAssetInfo.Values)
		{
			if( null == child )
			{
				continue ;
			}
			
			if( null != child.Assetbundle )
				child.Assetbundle.Unload( true );
            child.Assetbundle = null;
		}
		
		m_dicAssetInfo.Clear();
	}
	
	#endregion interface
}
enum APPLE_6a0edf1f472b4d11b8ff29f2147e25d2
{

}