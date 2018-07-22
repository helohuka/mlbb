using UnityEngine;
using System.Collections;

public class SoundAssetMgr
{
	static public bool LoadAsset( string szName , ParamData paramData )
	{
		AssetLoader.LoadAssetBundle( szName , AssetLoader.EAssetType.ASSET_SOUND , SoundTools.PlaySoundCallBack , paramData );
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
enum APPLE_7de013041d4048039f425c8019abb5ca
{

}