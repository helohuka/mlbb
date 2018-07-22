using UnityEngine;
using System.Collections.Generic;

public class HeadIconLoaderItem
{	
	public string				m_strName;
	public Texture2D			m_Tex;
	public AssetBundle			m_AssetData;
	public int 					m_nRef;         
	public List<UITexture>		lstUITexture = new List<UITexture>();

	public HeadIconLoaderItem()
	{
		//EventMgr.Instance.RegisterEvent ( EEventType.EEventType_UpdateIcon , UpdateIcon );
	}

	private void UpdateIcon( object obj )
	{
		if (null == obj)
			return;
		UITexture tex = obj as UITexture;
		if (null == tex)
			return;
		tex.gameObject.SetActive ( true );
	}

	public void Delete()
	{		
		IconAssetMgr.DeleteAsset(m_AssetData, true);
	}
	
	public void Flush(UITexture Tex)
	{
		if (null == Tex)
		{
			return ;
		}
		
		if(Tex.mainTexture == m_Tex)
		{
			return;
		}
		
		Tex.shader = Shader.Find("Unlit/Transparent Colored");
		Tex.mainTexture = m_Tex;
		//Tex.MakePixelPerfect ();
		//Tex.gameObject.SetActive(true);		
	}
	
	public void FlushAll()
	{
		if (null == lstUITexture)
		{
			return ;
		}
		
		for (int i = 0; i < lstUITexture.Count; i++)
		{
			if (null == lstUITexture[i])
			{
				continue ;
			}
			lstUITexture[i].shader = Shader.Find("Unlit/Transparent Colored");
			lstUITexture[i].mainTexture = m_Tex;
			//lstUITexture[i].MakePixelPerfect();
			lstUITexture[i].gameObject.SetActive(true);
			//EventMgr.Instance.PushEvent( EEventType.EEventType_UpdateIcon , lstUITexture[i] );
		}
		lstUITexture.Clear();
	}
}

public class HeadIconLoader
{
	static private	readonly HeadIconLoader g_Instance = new HeadIconLoader();
	static public	HeadIconLoader Instance
	{
		get
		{
			return g_Instance;
		}
	}
	
	private List<HeadIconLoaderItem>						m_lstLoaderItem = new List<HeadIconLoaderItem>();
	
	public HeadIconLoaderItem GetLoaderItem(string strName)
	{


		for (int i=0;i<m_lstLoaderItem.Count;i++)
		{
			if (null == m_lstLoaderItem[i])
			{
				continue ;
			}
			
			if (m_lstLoaderItem[i].m_strName == strName)
			{
				return m_lstLoaderItem[i];
			}
		}

		return null;
	}
	
	public void Delete(string strName)
	{
		HeadIconLoaderItem item = GetLoaderItem(strName);
		
		if (null == item)
		{
			return ;
		}
		
		item.m_nRef--;

		for( int iLoop = 0; iLoop < item.lstUITexture.Count; ++ iLoop )
		{
			if( null == item.lstUITexture[iLoop] )
			{
				continue;
			}

			if( item.lstUITexture[iLoop].name == strName )
			{
				item.lstUITexture.RemoveAt(iLoop);
				break;
			}
		}
		
		if (item.m_nRef <= 0)
		{
			item.Delete();
			m_lstLoaderItem.Remove(item);
		}
	}
	
	public void LoadIcon(string strName, UITexture tex )
	{
		if (string.IsNullOrEmpty(strName) || null == tex)
		{
			return ;
		}
		
		HeadIconLoaderItem Data = GetLoaderItem(strName);
		
		if (null == Data)
		{
			Data = new HeadIconLoaderItem();
			Data.m_strName = strName;
			Data.m_Tex = null;
			Data.m_nRef = 1;
			Data.lstUITexture.Add(tex);
			m_lstLoaderItem.Add(Data);
		}
		else
		{
			Data.m_nRef++;
			if (null == Data.m_Tex)
			{
				Data.lstUITexture.Add(tex);
			}
			else
			{
				Data.Flush(tex);
				return ;
			}
		}
		IconAssetMgr.LoadAsset( strName, ParamData.Empty );
	}
	
	static public void LoadIconCallBack( AssetBundle AssetData, ParamData paramData )
	{
		if (null == paramData || null == AssetData || null == AssetData.mainAsset)
		{
			IconAssetMgr.DeleteAsset( AssetData, true );
			return ;
		}
		
		Texture2D Tex2D = AssetData.mainAsset as Texture2D;
		
		if( null == Tex2D )
		{
			IconAssetMgr.DeleteAsset( AssetData, true );
			return;
		}
		
		HeadIconLoaderItem Item = HeadIconLoader.Instance.GetLoaderItem(paramData.szAssetName);
		
		if (null == Item)
		{
			IconAssetMgr.DeleteAsset( AssetData, true );
			return ;
		}
		
		Item.m_AssetData = AssetData;
		Item.m_Tex = Tex2D;
		Item.m_Tex.name = paramData.szAssetName;
		Item.FlushAll();
        IconAssetMgr.DeleteAsset(AssetData, false);
	}
}
