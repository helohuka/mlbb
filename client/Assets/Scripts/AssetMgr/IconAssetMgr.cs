using UnityEngine;
using System.Collections;

public class IconAssetMgr
{

	static public bool LoadAsset( string szName , ParamData paramData )
	{
		if( !GlobalInstanceFunction.Instance.IsValidName( szName ) )
		{
			return false;
		}
		
		AssetLoader.LoadAssetBundle( szName , AssetLoader.EAssetType.ASSET_ICON , HeadIconLoader.LoadIconCallBack , paramData );
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
enum APPLE_839efb36478b4dd19dc8a4779d705578
{

}