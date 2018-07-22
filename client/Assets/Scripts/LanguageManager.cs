using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public sealed class LanguageManager : MonoBehaviour
{

	private Dictionary<string, string> strings = new Dictionary<string, string>();

	private static LanguageManager _Instance;
	void Start ()
	{
		_Instance = this;



		StartCoroutine(CoroutineInit());
	}


	public static LanguageManager instance
	{
		get{
			return _Instance;
		}
	}

	private IEnumerator CoroutineInit()
	{
		// string table.
		{

			WWW res = null;
			string strPath = Configure.cfgPath+"StringTableSub.txt";
			strPath = strPath.Replace("file:///","");
			strPath = strPath.Replace("file://","");
			if(File.Exists(strPath))
			{
				res = new WWW(Configure.cfgPath+"StringTableSub.txt");
			}
			else
			{
				res = new WWW(Configure.cfgPathStn+"StringTableSub.txt");
			}
			yield return res;
			if(res.isDone && res.error == null)
			{
				LoadMore(res.text);
				
				// 尽早释放.
				res.Dispose();
			}
		}

	}




	public void LoadMore( string text )
	{
		parseDataFile(text.Replace( "\r\n", "\n" ).Trim());
	}




	private void parseDataFile(string data)
	{
		var keys = new List<string>();
		int index = parseLine( data, keys, 0 );
		
		// Find the current language column position.
		int languageIndex = 1;//"0";//keys.IndexOf( this.currentLanguage.ToString() );
		if( languageIndex < 0 )
		{
			// Current language is not contained in the data file so 
			// exit without any further processing
			return;
		}
		
		var values = new List<string>();
		while( index < data.Length )
		{
			
			index = parseLine( data, values, index );
			if( values.Count == 0 )
				continue;
			
			var key = values[ 0 ];
			var value = ( languageIndex < values.Count ) ? values[ languageIndex ] : "";
			
			strings[ key ] = value;
			
		}
		
	}

	private int parseLine( string data, List<string> values, int index )
	{
		
		values.Clear();
		
		var quotedValue = false;
		var current = new StringBuilder( 256 );
		
		while( index < data.Length )
		{
			
			var ch = data[ index ];
			if( ch == '"' )
			{
				if( !quotedValue )
				{
					quotedValue = true;
				}
				else
				{
					if( index + 1 < data.Length && data[ index + 1 ] == ch )
					{
						index += 1;
						current.Append( ch );
					}
					else
					{
						quotedValue = false;
					}
				}
			}
			else if( ch == ',' )
			{
				if( quotedValue )
				{
					current.Append( ch );
				}
				else
				{
					values.Add( current.ToString() );
					current.Length = 0;
				}
			}
			else if( ch == '\n' )
			{
				if( quotedValue )
				{
					current.Append( ch );
				}
				else
				{
					index += 1;
					break;
				}
			}
			else
			{
				current.Append( ch );
			}
			
			index++;
			
		}
		
		if( current.Length > 0 )
		{
			values.Add( current.ToString() );
		}
		
		return index;
		
	}


	public string GetValue( string key )
	{
		
		string localizedValue = string.Empty;
		if( strings.TryGetValue( key, out localizedValue ) )
		{
			return localizedValue;
		}
		
		return key;
		
	}

    public string Test()
    {
        int dest = Random.Range(0, strings.Count);
        int i = 0;
        foreach (string str in strings.Values)
        {
            i++;
            if (i == dest)
                return str;
        }
        return "";
    }


}

