using System.Collections.Generic;

public class CommonDefines
{
	private const EVersionDefines VERSION_BUILD=EVersionDefines.VERSION_DEVELOPMENT;
	//
	public	static	bool	RELEASE_VERSION_USE_DEV_RES	= false;
	
	private	enum EVersionDefines 
	{
		VERSION_DEVELOPMENT,		// Development
		VERSION_RELEASE,
		VERSION_MAX
	}
	public	static	bool IsDevVersion()
	{
		return VERSION_BUILD == EVersionDefines.VERSION_DEVELOPMENT;
	}

    static public string persistentDataPath
    {
        get
        {
#if UNITY_IOS || UNITY_IPHONE
            return UnityEngine.Application.persistentDataPath + "/NotBackup";
#else
            return UnityEngine.Application.persistentDataPath;
#endif
        }
    }
}


public class Pair<T0, T1>
{
    public Pair(T0 t0, T1 t1)
    {
        first = t0;
        second = t1;
    }

    public T0 first;
    public T1 second;
}

public class PlayerReference
{
    public List<string> animator;
    public List<string> mesh;
    public List<string> material;
}