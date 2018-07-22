using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections;

public class EditorTools : MonoBehaviour {

	[MenuItem("Tools/GenRpc")]
	public static void Gen()
	{
		ProcessStartInfo startInfo = new ProcessStartInfo ();
		startInfo.WorkingDirectory = Application.dataPath + "/../../../Tools/";
		startInfo.FileName = Application.dataPath + "/../../../Tools/genArpc.bat";
		Process.Start (startInfo);
	}

	[MenuItem("Tools/GenRpc")]
	public static void testBuildLightMap()
	{
		LightmapEditorSettings.maxAtlasHeight = 512;
		LightmapEditorSettings.maxAtlasWidth = 512;
		Lightmapping.Clear();
		Lightmapping.Bake();
	}
}
