using UnityEngine;
using System.Collections;

public class MusicAssetMgr
{
	static public bool LoadAsset( string szName , ParamData paramData )
	{
		if( !GlobalInstanceFunction.Instance.IsValidName( szName ) )
		{
			return false;
		}
		
		AssetLoader.LoadAssetBundle( szName , AssetLoader.EAssetType.ASSET_MUSIC , SoundTools.PlayMusicCallBack , paramData );
		return true;
	}
	
	static public bool DeleteAsset( string szName , bool bUnLoadAllObject )
	{
		return AssetInfoMgr.Instance.DecRefCount( szName , bUnLoadAllObject );
	}
	
	static public bool DeleteAsset( AssetBundle Asset , bool UnLoadAllObject )
	{
		return AssetInfoMgr.Instance.DecRefCount( Asset , UnLoadAllObject );
	}
}

enum APPLE_d8c6698405cb42d5a3dd3749f309c325
{

}