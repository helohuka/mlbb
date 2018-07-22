using UnityEngine;
using System.Collections;

public class ResourceCleanup : MonoBehaviour 
{
	private	float	CLEAN_RESOURCE_TIME		=	10f;
	private	bool	mNeedCleanup			=	false;
	private	float	mCleanupTime			=	0f;
	
	bool	NeedCleanup
	{
		get{ return mNeedCleanup; }
		set{ mNeedCleanup = value; }
	}
	
	float	CleanupTime
	{
		get { return mCleanupTime; }
		set { mCleanupTime = value; }
	}
	
	// Use this for initialization
	void Start () 
	{
		// Clean up unused resource at start up
		Resources.UnloadUnusedAssets();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( NeedCleanup )
		{
			CleanupTime	-=	Time.deltaTime;
			if( CleanupTime < 0f )
			{
				NeedCleanup = false;
				System.GC.Collect();
				Resources.UnloadUnusedAssets();
			}
		}
	}
	
	//
	public	void	RequireUnloadUnusedAssets()
	{
		if( !NeedCleanup )
		{
			NeedCleanup	=	true;
			CleanupTime	=	CLEAN_RESOURCE_TIME;
		}		
	}
}
