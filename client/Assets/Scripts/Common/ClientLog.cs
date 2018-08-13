using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ClientLog
{
	static readonly ClientLog g_Instance = new ClientLog();
	static public ClientLog Instance
	{
		get{ return g_Instance; }
	}
	
	// If you need in different versions output, add a jugement..
	private bool CanOutputError()
	{
		return ( CommonDefines.IsDevVersion() );	
	}
	// If you need in different versions output, add a jugement..
	private bool CanOutputWarning()
	{
		return ( CommonDefines.IsDevVersion() );	
	}
	private bool CanOutputLog()
	{
		return ( CommonDefines.IsDevVersion() );	
	}
	
	private int	m_nCurrLogCount = 0;
	//
	public void LogError( object message )
	{
		if( null == message  || !CanOutputError())
		{
			return;
		}
		Debug.LogError( message );
	}
	//
	public void LogError( object message, Object context )
	{
		if( null == message || null == context || !CanOutputError())
		{
			return;
		}
		Debug.LogError( message, context );
	}
	//
	public void LogErrorEx( object message )
	{
		if( !CanOutputError() )
		{
			return;
		}
		
		if( null == message )
		{
			return;
		}
		Debug.LogError( message );
	}
	//
	public void LogErrorEx( object message, Object context )
	{
		if( !CanOutputError() )
		{
			return;
		}
		
		if( null == message || null == context )
		{
			return;
		}
		Debug.LogError( message, context );
	}
	//
	public void LogWarning( object message )
	{
		if (null == message || !CanOutputWarning())
		{
			return;
		}
		Debug.LogWarning( message );
	}
	//
	public void LogWarning( object message, Object context )
	{
		if( null == message || null == context || !CanOutputWarning())
		{
			return;
		}
		Debug.LogWarning( message, context );
	}
	//
	public void LogWarningEx( object message )
	{
		if( !CanOutputWarning() )
		{
			return;
		}
		
		if( null == message )
		{
			return;
		}
		Debug.LogWarning( message );
	}
	//
	public void LogWarningEx( object message, Object context )
	{
		if( !CanOutputWarning() )
		{
			return;
		}
		
		if( null == message || null == context )
		{
			return;
		}
		Debug.LogWarning( message, context );
	}
	//
	public void Log( object message )
	{
		if( null == message || !CanOutputLog())
		{
			return;
		}
		Debug.Log( message );
	}
	//
	public void Log( object message, Object context )
	{
		if( null == message || null == context || !CanOutputLog())
		{
			return;
		}
		Debug.Log( message, context );
	}
	//
	public void LogEx( object message )
	{
		if( !CanOutputLog() )
		{
			return;
		}
		if( null == message )
		{
			return;
		}
		Debug.Log( message );
	}
	//
	public void LogEx( object message, Object context )
	{
		if( !CanOutputLog() )
		{
			return;
		}
		if( null == message || null == context )
		{
			return;
		}
		Debug.Log( message, context );
	}
}
