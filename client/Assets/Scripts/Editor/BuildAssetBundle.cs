using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

public class BuildAssetBundle : MonoBehaviour
{

    //得到工程中所有场景名称
    static string[] SCENES = FindEnabledEditorScenes();

    const string playerAssetsPath_ = "Assets/ResData/Role/Prefabs/";
    const string uiAssetsPath_ = "Assets/ResData/UI/UIPrefab/";
    const string weaponAssetsPath_ = "Assets/ResData/Weapon/";
    const string effectAssetsPath_ = "Assets/ResData/Effect/Prefab/";
    const string soundAssetsPath_ = "Assets/ResData/Sound/";
	const string musicAssetsPath_ = "Assets/ResData/Music/";
    const string iconAssetsPath_ = "Assets/ResData/Icon/";

    const string playerBundleOutputPath_ = "Assets/StreamingAssets/{0}/Player/";
    const string uiBundleOutputPath_ = "Assets/StreamingAssets/{0}/UI/";
    const string weaponBundleOutputPath_ = "Assets/StreamingAssets/{0}/Weapon/";
    const string effectBundleOutputPath_ = "Assets/StreamingAssets/{0}/Effect/";
    const string soundBundleOutputPath_ = "Assets/StreamingAssets/{0}/Sound/";
	const string musicBundleOutputPath_ = "Assets/StreamingAssets/{0}/Music/";
    const string iconBundleOutputPath_ = "Assets/StreamingAssets/{0}/Icon/";

    const string platformIOS_ = "AssetBundleIOS";
    const string platformPC_ = "AssetBundlePC";
    const string platformAndroid_ = "AssetBundleAndroid";
    const string platformWP8_ = "AssetBundleWP8";

    static string[] ExcludeUI = new string[] { "" };

#if UNITY_IOS
    /// <summary>
    /// IOS资源包
    /// </summary>

    [MenuItem("Custom Editor/Build/IOSPackage/BrandNew")]
    static void BuildIOSPackage()
    {
        BuildIOSAssetbundleEffect();
        BuildIOSAssetbundleUI();
        BuildIOSAssetbundlePlayer();
        BuildIOSAssetbundleWeapon();
        BuildIOSAssetbundleSound();
		BuildIOSAssetbundleMusic ();
        AssetDatabase.Refresh();
        BuildIOSProject();
    }

    [MenuItem("Custom Editor/Build/IOSPackage/BuildOnly")]
    static void BuildIOSProject()
    {
        string applicationPath = Application.dataPath.Replace("/Assets", "");
        string target_dir = Application.dataPath + "/../TargetIOS";
        string target_name = "xysk";
        GenericBuild(SCENES, target_dir + "/" + target_name, BuildTarget.iPhone, BuildOptions.None);
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/Effect")]
    static void BuildIOSAssetbundleEffect()
    {
        Pack(effectAssetsPath_, string.Format(effectBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/SingleEffect")]
    static void BuildIOSAssetbundleEffectSingle()
    {
        PackSimpleEffect(string.Format(playerBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/UI")]
    static void BuildIOSAssetbundleUI()
    {
        Pack(uiAssetsPath_, string.Format(uiBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/SingleUI")]
    static void BuildIOSAssetbundleUISingle()
    {
        PackSimpleUI(string.Format(uiBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/Player")]
    static void BuildIOSAssetbundlePlayer()
    {
        Pack(playerAssetsPath_, string.Format(playerBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/SinglePlayer")]
    static void BuildIOSAssetbundlePlayerSingle()
    {
        PackSimplePlayer(string.Format(playerBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/Weapon")]
    static void BuildIOSAssetbundleWeapon()
    {
        Pack(weaponAssetsPath_, string.Format(weaponBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/Sound")]
    static void BuildIOSAssetbundleSound()
    {
        Pack(soundAssetsPath_, string.Format(soundBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

	[MenuItem("Custom Editor/Build/IOSAssetbundle/Music")]
	static void BuildIOSAssetbundleMusic()
	{
		Pack(musicAssetsPath_, string.Format(musicBundleOutputPath_, platformIOS_));
		AssetDatabase.Refresh();
	}

    [MenuItem("Custom Editor/Build/IOSAssetbundle/Icon")]
    static void BuildIOSAssetbundleIcon()
    {
        Pack(iconAssetsPath_, string.Format(iconBundleOutputPath_, platformIOS_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/IOSAssetbundle/All")]
    static void BuildIOSAssetbundleAll()
    {
        BuildIOSAssetbundleEffect();
        BuildIOSAssetbundleUI();
        BuildIOSAssetbundlePlayer();
        BuildIOSAssetbundleWeapon();
        BuildIOSAssetbundleSound();
		BuildIOSAssetbundleMusic ();
        BuildIOSAssetbundleIcon();
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Create AssetBunldes IOS")]  
	static void CreateAssetBunldesIOS ()  
	{  
		
		IOS ();
		
		AssetDatabase.Refresh ();     
		
	} 

#endif

#if UNITY_STANDALONE
    /// <summary>
    /// PC 资源包
    /// </summary>

    [MenuItem("Custom Editor/Build/PCPackage/BrandNew")]
    static void BuildPCPackage()
    {
        BuildPCAssetbundleEffect();
        BuildPCAssetbundleUI();
        BuildPCAssetbundlePlayer();
        BuildPCAssetbundleWeapon();
        BuildPCAssetbundleSound();
        CopyTablePC();
        AssetDatabase.Refresh();
        BuildPC();
    }

    [MenuItem("Custom Editor/Build/PCPackage/BuildOnly")]
    static void BuildPC()
    {
        string applicationPath = Application.dataPath.Replace("/Assets", "");
        string target_dir = Application.dataPath + "/../TargetPC";
        string target_name = "xysk.exe";
        GenericBuild(SCENES, target_dir + "/" + target_name, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/Effect")]
    static void BuildPCAssetbundleEffect()
    {
        Pack(effectAssetsPath_, string.Format(effectBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/SingleEffect")]
    static void BuildPCAssetbundleEffectSingle()
    {
        PackSimpleEffect(string.Format(playerBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/UI")]
    static void BuildPCAssetbundleUI()
    {
        Pack(uiAssetsPath_, string.Format(uiBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/SingleUI")]
    static void BuildPCAssetbundleUISingle()
    {
        PackSimpleUI(string.Format(uiBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/Player")]
    static void BuildPCAssetbundlePlayer()
    {
        Pack(playerAssetsPath_, string.Format(playerBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/SinglePlayer")]
    static void BuildPCAssetbundlePlayerSingle()
    {
        PackSimplePlayer(string.Format(playerBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/Weapon")]
    static void BuildPCAssetbundleWeapon()
    {
        Pack(weaponAssetsPath_, string.Format(weaponBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/Sound")]
    static void BuildPCAssetbundleSound()
    {
        Pack(soundAssetsPath_, string.Format(soundBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/Icon")]
    static void BuildPCAssetbundleIcon()
    {
        Pack(iconAssetsPath_, string.Format(iconBundleOutputPath_, platformPC_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/CopyTable/PC")]
    static void CopyTablePC()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WorkingDirectory = Application.dataPath + "/../../Config/";
        startInfo.FileName = Application.dataPath + "/../../Config/syncTable.bat";
        startInfo.Arguments = "AssetBundlePC";
        Process.Start(startInfo);
    }

    [MenuItem("Custom Editor/Build/PCAssetbundle/All")]
    static void BuildPCAssetbundleAll()
    {
        BuildPCAssetbundleEffect();
        BuildPCAssetbundleUI();
        BuildPCAssetbundlePlayer();
        BuildPCAssetbundleWeapon();
        BuildPCAssetbundleSound();
        BuildPCAssetbundleIcon();
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Create AssetBunldes PC")]
    static void CreateAssetBunldesWeb()
    {

        PC();
        AssetDatabase.Refresh();

    }

    [MenuItem("Tools/BuildScene/PC")]
    public static void BuildScenePC()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < SelectedAsset.Length; ++i)
        {
            string[] levels = new string[] { Application.dataPath + "/Scene/" + SelectedAsset[i].name + ".unity" };
            string path = Application.streamingAssetsPath + "/" + platformPC_ + "/Scene/" + SelectedAsset[i].name + ".unity3d";
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, BuildTarget.StandaloneWindows);
        }
        ClientLog.Instance.Log("BuildScenePC Success.");
    }

#endif

#if UNITY_ANDROID

    /// <summary>
    /// Android资源包
    /// </summary>

    [MenuItem("Custom Editor/Build/AndroidPackage/BrandNew")]
    static void BuildAndroidPackage()
    {
        BuildAndroidAssetbundleEffect();
        BuildAndroidAssetbundleUI();
        BuildAndroidAssetbundlePlayer();
        BuildAndroidAssetbundleWeapon();
        BuildAndroidAssetbundleSound();
		BuildAndroidAssetbundleMusic ();
        CopyTableAndroid();
        AssetDatabase.Refresh();
        BuildAndroidProject();
    }

    [MenuItem("Custom Editor/Build/AndroidPackage/BuildOnly")]
    static void BuildAndroidProject()
    {
        string applicationPath = Application.dataPath.Replace("/Assets", "");
        string target_dir = Application.dataPath + "/../TargetAndroid";
        string target_name = "xysk";
        GenericBuild(SCENES, target_dir + "/" + target_name, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/Effect")]
    static void BuildAndroidAssetbundleEffect()
    {
        Pack(effectAssetsPath_, string.Format(effectBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/SingleEffect")]
    static void BuildAndroidAssetbundleEffectSingle()
    {
        PackSimpleEffect(string.Format(playerBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/UI")]
    static void BuildAndroidAssetbundleUI()
    {
        Pack(uiAssetsPath_, string.Format(uiBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/SingleUI")]
    static void BuildAndroidAssetbundleUISingle()
    {
        PackSimpleUI(string.Format(uiBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/Player")]
    static void BuildAndroidAssetbundlePlayer()
    {
        Pack(playerAssetsPath_, string.Format(playerBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/SinglePlayer")]
    static void BuildAndroidAssetbundlePlayerSingle()
    {
        PackSimplePlayer(string.Format(playerBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/Weapon")]
    static void BuildAndroidAssetbundleWeapon()
    {
        Pack(weaponAssetsPath_, string.Format(weaponBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/Sound")]
    static void BuildAndroidAssetbundleSound()
    {
        Pack(soundAssetsPath_, string.Format(soundBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }
	[MenuItem("Custom Editor/Build/AndroidAssetbundle/Music")]
	static void BuildAndroidAssetbundleMusic()
	{
		Pack(musicAssetsPath_, string.Format(musicBundleOutputPath_, platformAndroid_));
		AssetDatabase.Refresh();
	}

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/Icon")]
    static void BuildAndroidAssetbundleIcon()
    {
        Pack(iconAssetsPath_, string.Format(iconBundleOutputPath_, platformAndroid_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/CopyTable/Android")]
    static void CopyTableAndroid()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WorkingDirectory = Application.dataPath + "/../../Config/";
        startInfo.FileName = Application.dataPath + "/../../Config/syncTable.bat";
        startInfo.Arguments = "AssetBundleAndroid";
        Process.Start(startInfo);
    }

    [MenuItem("Custom Editor/Build/AndroidAssetbundle/All")]
    static void BuildAndroidAssetbundleAll()
    {
        BuildAndroidAssetbundleEffect();
        BuildAndroidAssetbundleUI();
        BuildAndroidAssetbundlePlayer();
        BuildAndroidAssetbundleWeapon();
        BuildAndroidAssetbundleSound();
		BuildAndroidAssetbundleMusic ();
        BuildAndroidAssetbundleIcon();
        AssetDatabase.Refresh();
    }

    	//打包单个  
	[MenuItem("Custom Editor/Create AssetBunldes Android")]  
	static void CreateAssetBunldesAndroid ()  
	{  

		Android ();

		AssetDatabase.Refresh ();     
		
	}  
    
    [MenuItem("Tools/BuildScene/Android")]
    public static void BuildSceneAndroid()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < SelectedAsset.Length; ++i)
        {
            string[] levels = new string[] { Application.dataPath + "/Scene/" + SelectedAsset[i].name + ".unity" };
            string path = Application.streamingAssetsPath + "/" + platformAndroid_ + "/Scene/" + SelectedAsset[i].name + ".unity3d";
            BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, BuildTarget.Android);
        }
        ClientLog.Instance.Log("BuildSceneAndroid Success.");
    }

#endif

#if UNITY_WINPHONE

    /// <summary>
    /// WP8资源包
    /// </summary>

    [MenuItem("Custom Editor/Build/WP8Package/BrandNew")]
    static void BuildWP8Package()
    {
        BuildWP8AssetbundleEffect();
        BuildWP8AssetbundleUI();
        BuildWP8AssetbundlePlayer();
        BuildWP8AssetbundleWeapon();
        BuildWP8AssetbundleSound();
        BuildWP8AssetbundleMusic();
        AssetDatabase.Refresh();
        BuildWP8Project();
    }

    [MenuItem("Custom Editor/Build/WP8Package/BuildOnly")]
    static void BuildWP8Project()
    {
        string applicationPath = Application.dataPath.Replace("/Assets", "");
        string target_dir = Application.dataPath + "/../TargetWP";
        string target_name = "xysk";
        GenericBuild(SCENES, target_dir + "/" + target_name, BuildTarget.WP8Player, BuildOptions.None);
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Effect")]
    static void BuildWP8AssetbundleEffect()
    {
        Pack(effectAssetsPath_, string.Format(effectBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/UI")]
    static void BuildWP8AssetbundleUI()
    {
        Pack(uiAssetsPath_, string.Format(uiBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Player")]
    static void BuildWP8AssetbundlePlayer()
    {
        Pack(playerAssetsPath_, string.Format(playerBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Weapon")]
    static void BuildWP8AssetbundleWeapon()
    {
        Pack(weaponAssetsPath_, string.Format(weaponBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Sound")]
    static void BuildWP8AssetbundleSound()
    {
        Pack(soundAssetsPath_, string.Format(soundBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Music")]
    static void BuildWP8AssetbundleMusic()
    {
        Pack(musicAssetsPath_, string.Format(musicBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/Icon")]
    static void BuildWP8AssetbundleIcon()
    {
        Pack(iconAssetsPath_, string.Format(iconBundleOutputPath_, platformWP8_));
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Build/WP8Assetbundle/All")]
    static void BuildWP8AssetbundleAll()
    {
        BuildWP8AssetbundleEffect();
        BuildWP8AssetbundleUI();
        BuildWP8AssetbundlePlayer();
        BuildWP8AssetbundleWeapon();
        BuildWP8AssetbundleSound();
        BuildWP8AssetbundleMusic();
        BuildWP8AssetbundleIcon();
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Create AssetBunldes WP8")]
    static void CreateAssetBunldesWP8()
    {

        WP8();
        AssetDatabase.Refresh();

    } 

#endif

    //===============================================================================//

	static void Android()
	{
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{  
			string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".bytes";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android))
            {  
				UnityEngine.Debug.Log(obj.name +"资源打包成功");  
			}   
			else   
			{
                UnityEngine.Debug.Log(obj.name + "资源打包失败");  
			}  
		}  
	}

	static void IOS()
	{
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{  
			string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".bytes";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.iPhone))
            {
                UnityEngine.Debug.Log(obj.name + "资源打包成功");  
			}   
			else   
			{
                UnityEngine.Debug.Log(obj.name + "资源打包失败");  
			}  
		}  
	}

	static void PC()
	{
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{  
			string targetPath = Application.dataPath + "/StreamingAssets/AssetBundlePC/" + obj.name + ".bytes";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows))
            {
                UnityEngine.Debug.Log(obj.name + "资源打包成功");  
			}   
			else   
			{  
				UnityEngine.Debug.Log(obj.name +"资源打包失败");  
			}  
		}  
	}

    static void WP8()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in SelectedAsset)
        {
            string targetPath = Application.dataPath + "/StreamingAssets/AssetBundleWP8/" + obj.name + ".bytes";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.WP8Player))
            {
                UnityEngine.Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                UnityEngine.Debug.Log(obj.name + "资源打包失败");
            }
        }  
    }

    static void Pack(string path, string pathName)
    {
        if (Directory.Exists(pathName))
            Directory.Delete(pathName, true);
        Directory.CreateDirectory(pathName);
        Caching.CleanCache();
        List<Object> files = new List<Object>();
        string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        for (int i = 0; i < filePaths.Length; ++i)
        {
            if (filePaths[i].Contains(".meta"))
                continue;
            string filePath = cut2AssetFolder(filePaths[i]);
            Object file = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            if(file != null)
                files.Add(file);
        }

        BuildTarget tar = BuildTarget.StandaloneWindows;
        if (pathName.Contains(platformIOS_)) tar = BuildTarget.iPhone;
        else if (pathName.Contains(platformAndroid_)) tar = BuildTarget.Android;

        if (path.Equals(uiAssetsPath_))
        {
            string file = EditorUtility.OpenFilePanel("请选择UI依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
            string jsonStr = System.IO.File.ReadAllText(file);
            atlasRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);

            //dep字体(所有ui)
            BuildPipeline.PushAssetDependencies();
            string[] assetfiles = new string[] { "Assets/ResData/Font/qingyuan.ttf", 
                                                    "Assets/ResData/Font/minijianshaoer.ttf",
                                                    "Assets/Scripts/NGUI/Scripts/UI/UIAtlas.cs", 
                                                    "Assets/Scripts/NGUI/Scripts/UI/UIFont.cs",
                                                    "Assets/Scripts/NGUI/Resources/Shaders/Unlit - Transparent Colored.shader"};
            Object[] objfiles = new Object[assetfiles.Length];
            for(int i=0; i < objfiles.Length; ++i)
            {
                objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
            }
            BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "commonAssets"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
        }

        if (path.Equals(playerAssetsPath_))
        {
            string file = EditorUtility.OpenFilePanel("请选择角色依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
            string jsonStr = System.IO.File.ReadAllText(file);
            playerRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);
            
            //dep角色shader
            BuildPipeline.PushAssetDependencies();
            string[] assetfiles = new string[] { "Assets/Scripts/Shader/PlayerShader.shader", "Assets/ResData/Role/yingzi/yingzi01.mat"};
            Object[] objfiles = new Object[assetfiles.Length];
            for (int i = 0; i < objfiles.Length; ++i)
            {
                objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
            }
            BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "PlayerShader"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
        }

        if (path.Equals(effectAssetsPath_))
        {
            string file = EditorUtility.OpenFilePanel("请选择特效依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
            string jsonStr = System.IO.File.ReadAllText(file);
            effectRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);

            //dep特效shader
            //BuildPipeline.PushAssetDependencies();
            //string[] assetfiles = new string[] { "Assets/ResData/GameScene/CreateRole/Shaders/Mobile Particles Additive Culled.shader" };
            //Object[] objfiles = new Object[assetfiles.Length];
            //for (int i = 0; i < objfiles.Length; ++i)
            //{
            //    objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
            //}
            //BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "EffectShader"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
        }

        for (int i = 0; i < files.Count; ++i)
        {
            if(path.Equals(uiAssetsPath_))
            {
                List<string> refatlasLst = GetRefAtlas(files[i].name);
                if (refatlasLst.Count > 0)
                    BuildPipeline.PushAssetDependencies();
                for (int j = 0; j < refatlasLst.Count; ++j)
                {
                    string assetfile = FindAtlas(refatlasLst[j]);
                    Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                    BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(pathName + "{0}.bytes", refatlasLst[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                }
                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(pathName + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                BuildPipeline.PopAssetDependencies();
                if (refatlasLst.Count > 0)
                    BuildPipeline.PopAssetDependencies();
            }
            else if (path.Equals(playerAssetsPath_))
            {
                List<string> pr = GetRefAssets(files[i].name);
                if (pr != null)
                {
                    BuildPipeline.PushAssetDependencies();

                    //List<Object> refAsssets = new List<Object>();
                    for (int j = 0; j < pr.Count; ++j)
                    {
                        string assetfile = FindAsset(pr[j]);
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                        BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(pathName + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                        //refAsssets.Add(mainAsset);
                    }
                }
                //if (refAsssets.Count > 0)
                //    BuildPipeline.BuildAssetBundle(null, refAsssets.ToArray(), string.Format(pathName + "{0}{1}.bytes", files[i].name, "_dep"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(pathName + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                BuildPipeline.PopAssetDependencies();
                //if (refAsssets.Count > 0)
                    BuildPipeline.PopAssetDependencies();
            }
            else if (path.Equals(effectAssetsPath_))
            {
                List<string> pr = GetRefEffAssets(files[i].name);
                if (pr != null)
                {
                    BuildPipeline.PushAssetDependencies();

                    //List<Object> refAsssets = new List<Object>();
                    for (int j = 0; j < pr.Count; ++j)
                    {
                        string assetfile = FindAsset(pr[j]);
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                        if (mainAsset == null)
                        {
                            UnityEngine.Debug.Log(assetfile + " is can not find.");
                            continue;
                        }
                        BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(pathName + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                        //refAsssets.Add(mainAsset);
                    }
                }
                else
                {
                    UnityEngine.Debug.Log(files[i].name + " is can not find file effect.");
                }
                //if (refAsssets.Count > 0)
                //    BuildPipeline.BuildAssetBundle(null, refAsssets.ToArray(), string.Format(pathName + "{0}{1}.bytes", files[i].name, "_dep"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(pathName + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                BuildPipeline.PopAssetDependencies();
                //if (refAsssets.Count > 0)
                BuildPipeline.PopAssetDependencies();
            }
            else
            {
                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(pathName + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            }
        }
        if (path.Equals(uiAssetsPath_) || path.Equals(playerAssetsPath_))
            BuildPipeline.PopAssetDependencies();
    }

    // 在菜单来创建 选项 ， 点击该选项执行搜索代码      
    [MenuItem("Tools/Check Unity Font")]
    static void Check()
    {
        string[] tmpFilePathArray = Directory.GetFiles(Application.dataPath + "/ResData/UI/", "*.*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont", 0f);

        for (int i = 0; i < tmpFilePathArray.Length; i++)
        {
            EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont", (i * 1.0f) / tmpFilePathArray.Length);

            string tmpFilePath = tmpFilePathArray[i];

            if (tmpFilePath.EndsWith(".prefab"))
            {
                StreamReader tmpStreamReader = new StreamReader(tmpFilePath);
                string tmpContent = tmpStreamReader.ReadToEnd();
                if (tmpContent.Contains("mFont: {fileID: 0}"))
                {
                    UnityEngine.Debug.LogError(tmpFilePath);
                }
            }

            if (tmpFilePath.EndsWith(".prefab"))
            {
                StreamReader tmpStreamReader = new StreamReader(tmpFilePath);
                string tmpContent = tmpStreamReader.ReadToEnd();
                if (tmpContent.Contains("guid: 0000000000000000d000000000000000"))
                {
                    UnityEngine.Debug.LogError(tmpFilePath);
                }
            }
        }

        EditorUtility.ClearProgressBar();
    }  

    static void PackSimpleUI(string pathName)
    {
        Caching.CleanCache();
        BuildTarget tar = BuildTarget.StandaloneWindows;
        if (pathName.Contains(platformIOS_)) tar = BuildTarget.iPhone;
        else if (pathName.Contains(platformAndroid_)) tar = BuildTarget.Android;

        string file = EditorUtility.OpenFilePanel("请选择UI依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
        string jsonStr = System.IO.File.ReadAllText(file);
        atlasRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);

        //dep字体(所有ui)
        BuildPipeline.PushAssetDependencies();
        string[] assetfiles = new string[] { "Assets/ResData/Font/qingyuan.ttf", 
                                                "Assets/ResData/Font/minijianshaoer.ttf",
                                                "Assets/Scripts/NGUI/Scripts/UI/UIAtlas.cs", 
                                                "Assets/Scripts/NGUI/Scripts/UI/UIFont.cs",
                                                "Assets/Scripts/NGUI/Resources/Shaders/Unlit - Transparent Colored.shader"};
        Object[] objfiles = new Object[assetfiles.Length];
        for (int i = 0; i < objfiles.Length; ++i)
        {
            objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
        }
        BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "commonAssets"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);

        Object[] ui = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        if (ui != null && ui.Length == 1)
        {
            List<string> refatlasLst = GetRefAtlas(ui[0].name);
            if (refatlasLst.Count > 0)
                BuildPipeline.PushAssetDependencies();
            string bundleFile = "";
            for (int j = 0; j < refatlasLst.Count; ++j)
            {
                bundleFile = string.Format(pathName + "{0}.bytes", refatlasLst[j]);
                if (File.Exists(bundleFile))
                    File.Delete(bundleFile);
                string assetfile = FindAtlas(refatlasLst[j]);
                Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                BuildPipeline.BuildAssetBundle(mainAsset, null, bundleFile, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            }

            BuildPipeline.PushAssetDependencies();
            bundleFile = string.Format(pathName + "{0}.bytes", ui[0].name);
            if (File.Exists(bundleFile))
                File.Delete(bundleFile);
            BuildPipeline.BuildAssetBundle(ui[0], null, bundleFile, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            BuildPipeline.PopAssetDependencies();
            if (refatlasLst.Count > 0)
                BuildPipeline.PopAssetDependencies();
            BuildPipeline.PopAssetDependencies();
        }
    }

    static void PackSimplePlayer(string pathName)
    {
        Caching.CleanCache();
        BuildTarget tar = BuildTarget.StandaloneWindows;
        if (pathName.Contains(platformIOS_)) tar = BuildTarget.iPhone;
        else if (pathName.Contains(platformAndroid_)) tar = BuildTarget.Android;

        string file = EditorUtility.OpenFilePanel("请选择角色依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
        string jsonStr = System.IO.File.ReadAllText(file);
        playerRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);

        //dep角色shader
        BuildPipeline.PushAssetDependencies();
        string[] assetfiles = new string[] { "Assets/Scripts/Shader/PlayerShader.shader", "Assets/ResData/Role/yingzi/yingzi01.mat" };
        Object[] objfiles = new Object[assetfiles.Length];
        for (int i = 0; i < objfiles.Length; ++i)
        {
            objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
        }
        BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "PlayerShader"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);

        Object[] player = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        if (player != null && player.Length == 1)
        {
            List<string> pr = GetRefAssets(player[0].name);
            if (pr != null)
                BuildPipeline.PushAssetDependencies();

            //List<Object> refAsssets = new List<Object>();
            ClientLog.Instance.Log(player[0].name + " Player dependence on: ");
            for (int j = 0; j < pr.Count; ++j)
            {
                ClientLog.Instance.Log(pr[j]);
                string assetfile = FindAsset(pr[j]);
                Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(pathName + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                //refAsssets.Add(mainAsset);
            }
            //string bundleFile = "";
            //if (refAsssets.Count > 0)
            //{
            //    bundleFile = string.Format(pathName + "{0}{1}.bytes", player[0].name, "_dep");
            //    if (File.Exists(bundleFile))
            //        File.Delete(bundleFile);
            //    BuildPipeline.BuildAssetBundle(null, refAsssets.ToArray(), bundleFile, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            //}
            BuildPipeline.PushAssetDependencies();
            //bundleFile = string.Format(pathName + "{0}.bytes", player[0].name);
            //if (File.Exists(bundleFile))
                //File.Delete(bundleFile);
            BuildPipeline.BuildAssetBundle(player[0], null, string.Format(pathName + "{0}.bytes", player[0].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            BuildPipeline.PopAssetDependencies();
            BuildPipeline.PopAssetDependencies();
            BuildPipeline.PopAssetDependencies();
        }
    }

    static void PackSimpleEffect(string pathName)
    {
        Caching.CleanCache();
        BuildTarget tar = BuildTarget.StandaloneWindows;
        if (pathName.Contains(platformIOS_)) tar = BuildTarget.iPhone;
        else if (pathName.Contains(platformAndroid_)) tar = BuildTarget.Android;

        string file = EditorUtility.OpenFilePanel("请选择特效依赖数据文件", Application.dataPath + "/../../Config/Tables/", "json");
        string jsonStr = System.IO.File.ReadAllText(file);
        effectRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);

        //dep特效shader
        //BuildPipeline.PushAssetDependencies();
        //string[] assetfiles = new string[] { "Assets/ResData/GameScene/CreateRole/Shaders/Mobile Particles Additive Culled.shader" };
        //Object[] objfiles = new Object[assetfiles.Length];
        //for (int i = 0; i < objfiles.Length; ++i)
        //{
        //    objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
        //}
        //BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(pathName + "{0}.bytes", "EffectShader"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);

        Object[] effect = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        if (effect != null && effect.Length == 1)
        {
            List<string> pr = GetRefEffAssets(effect[0].name);
            if (pr != null)
                BuildPipeline.PushAssetDependencies();

            //List<Object> refAsssets = new List<Object>();
            ClientLog.Instance.Log(effect[0].name + " Effect dependence on: ");
            for (int j = 0; j < pr.Count; ++j)
            {
                ClientLog.Instance.Log(pr[j]);
                string assetfile = FindAsset(pr[j]);
                Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(pathName + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
                //refAsssets.Add(mainAsset);
            }
            //string bundleFile = "";
            //if (refAsssets.Count > 0)
            //{
            //    bundleFile = string.Format(pathName + "{0}{1}.bytes", player[0].name, "_dep");
            //    if (File.Exists(bundleFile))
            //        File.Delete(bundleFile);
            //    BuildPipeline.BuildAssetBundle(null, refAsssets.ToArray(), bundleFile, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            //}
            BuildPipeline.PushAssetDependencies();
            //bundleFile = string.Format(pathName + "{0}.bytes", player[0].name);
            //if (File.Exists(bundleFile))
            //File.Delete(bundleFile);
            BuildPipeline.BuildAssetBundle(effect[0], null, string.Format(pathName + "{0}.bytes", effect[0].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);
            BuildPipeline.PopAssetDependencies();
            BuildPipeline.PopAssetDependencies();
            //BuildPipeline.PopAssetDependencies();
        }
    }

    static string FindAsset(string assetGuid)
    {
        return AssetDatabase.GUIDToAssetPath(assetGuid);
    }

    static string FindAtlas(string fileName)
    {
        string[] fileGuids = AssetDatabase.FindAssets(fileName + " t:GameObject", new string[] { "Assets/ResData/UI/UIAtlass" });
        if (fileGuids == null || fileGuids.Length == 0)
        {
            ClientLog.Instance.LogError("Can not found atlas by " + fileName + " in project: ");
            return "";
        }
        string finalPath = "";
        if (fileGuids.Length > 1)
        {
            ClientLog.Instance.LogError("More atlas by " + fileName + " in project: ");
            for (int i = 0; i < fileGuids.Length; ++i)
            {
                ClientLog.Instance.LogError(AssetDatabase.GUIDToAssetPath(fileGuids[i]));
                if(!AssetDatabase.GUIDToAssetPath(fileGuids[i]).Contains(fileName + ".prefab"))
                    continue;
                finalPath = AssetDatabase.GUIDToAssetPath(fileGuids[i]);
                ClientLog.Instance.LogError("Use this Above.");
            }
        }
        else
        {
            finalPath = AssetDatabase.GUIDToAssetPath(fileGuids[0]);
        }

        return finalPath;
    }

    static List<string> GetRefAtlas(string uiName)
    {
        if (!atlasRefDic_.ContainsKey(uiName))
            return new List<string>();
        return atlasRefDic_[uiName];
    }

    static List<string> GetRefAssets(string playerName)
    {
        if (!playerRefDic_.ContainsKey(playerName))
            return null;
        return playerRefDic_[playerName];
    }

    static List<string> GetRefEffAssets(string effectName)
    {
        if (!effectRefDic_.ContainsKey(effectName))
            return null;
        return effectRefDic_[effectName];
    }

    static string cut2AssetFolder(string src)
    {
        return src.Substring(src.IndexOf("Assets"));
    }

    static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

        if (res.Length > 0)
        {
            UnityEngine.Debug.LogError("BuildPlayer failure: " + res);
        }
    }

    [MenuItem("Tools/CreateUIDependence")]
    public static void CreateUIDependenceFile()
    {
        string path = uiAssetsPath_;
        string pathName = string.Format(uiBundleOutputPath_, platformAndroid_);
        string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        for (int i = 0; i < filePaths.Length; ++i)
        {
            if (filePaths[i].Contains(".meta"))
                continue;

            string filePath = cut2AssetFolder(filePaths[i]);
            if (Contain(filePath))
                continue;

            Object file = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            crtUI_ = file.name;
            ListAtlas(((GameObject)file).transform);
        }
        string jsonStr = LitJson.JsonMapper.ToJson(atlasRefDic_);
        System.IO.File.WriteAllText(Application.dataPath + "/../../Config/Tables/UIDependence.json", jsonStr);
        AssetDatabase.Refresh();
        ClientLog.Instance.Log("Success");
    }

    static bool Contain(string fileName)
    {
        for (int i = 0; i < ExcludeUI.Length; ++i)
        {
            if (ExcludeUI[i].Equals(fileName))
                return true;
        }
        return false;
    }

    static Dictionary<string, List<string>> atlasRefDic_ = new Dictionary<string,List<string>>();
    static string crtUI_;
    static void ListAtlas(Transform trans)
    {
        UISprite sprite = trans.GetComponent<UISprite>();
        if (sprite != null && sprite.atlas != null)
        {
            if (!atlasRefDic_.ContainsKey(crtUI_))
                atlasRefDic_.Add(crtUI_, new List<string>());
            if (!atlasRefDic_[crtUI_].Contains(sprite.atlas.name))
                atlasRefDic_[crtUI_].Add(sprite.atlas.name);
        }
        
        for (int i = 0; i < trans.childCount; ++i)
        {
            ListAtlas(trans.GetChild(i));
        }
    }

    [MenuItem("Tools/CreatePlayerDependence")]
    public static void CreatePlayerDependenceFile()
    {
        playerRefDic_.Clear();
        string path = playerAssetsPath_;
        string pathName = string.Format(playerBundleOutputPath_, platformAndroid_);
        string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        for (int i = 0; i < filePaths.Length; ++i)
        {
            if (filePaths[i].Contains(".meta"))
                continue;

            string filePath = cut2AssetFolder(filePaths[i]);
            Object file = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            crtPlayer_ = file.name;
            ListPlayerRef(((GameObject)file).transform);
        }
        string jsonStr = LitJson.JsonMapper.ToJson(playerRefDic_);
        System.IO.File.WriteAllText(Application.dataPath + "/../../Config/Tables/PlayerDependence.json", jsonStr);
        AssetDatabase.Refresh();
        ClientLog.Instance.Log("Success");
    }

    static Dictionary<string, List<string>> playerRefDic_ = new Dictionary<string, List<string>>();
    static string crtPlayer_;
    static void ListPlayerRef(Transform trans)
    {
        if (!playerRefDic_.ContainsKey(crtPlayer_))
        {
            //PlayerReference pr = new PlayerReference();
            //pr.animator = new List<string>();
            //pr.mesh = new List<string>();
            //pr.material = new List<string>();
            playerRefDic_.Add(crtPlayer_, new List<string>());
        }

        Animator anim = trans.GetComponent<Animator>();
        string path = "";
        string guid = "";
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            path = AssetDatabase.GetAssetPath(anim.runtimeAnimatorController);
            guid = AssetDatabase.AssetPathToGUID(path);
            if (!playerRefDic_[crtPlayer_].Contains(guid))
                playerRefDic_[crtPlayer_].Add(guid);
        }

        SkinnedMeshRenderer smr = trans.GetComponent<SkinnedMeshRenderer>();
        if (smr != null)
        {
            path = AssetDatabase.GetAssetPath(smr.sharedMaterial);
            guid = AssetDatabase.AssetPathToGUID(path);
            if (!playerRefDic_[crtPlayer_].Contains(guid))
                playerRefDic_[crtPlayer_].Add(guid);

            path = AssetDatabase.GetAssetPath(smr.sharedMaterial.mainTexture);
            guid = AssetDatabase.AssetPathToGUID(path);
            if (!playerRefDic_[crtPlayer_].Contains(guid))
                playerRefDic_[crtPlayer_].Add(guid);

            path = AssetDatabase.GetAssetPath(smr.sharedMesh);
            guid = AssetDatabase.AssetPathToGUID(path);
            if (!playerRefDic_[crtPlayer_].Contains(guid))
                playerRefDic_[crtPlayer_].Add(guid);
        }
        for (int i = 0; i < trans.childCount; ++i)
        {
            ListPlayerRef(trans.GetChild(i));
        }
    }

    [MenuItem("Tools/CreateEffectDependence")]
    public static void CreateEffectDependenceFile()
    {
        effectRefDic_.Clear();
        string path = effectAssetsPath_;
        string pathName = string.Format(effectBundleOutputPath_, platformAndroid_);
        string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        for (int i = 0; i < filePaths.Length; ++i)
        {
            if (filePaths[i].Contains(".meta"))
                continue;

            string filePath = cut2AssetFolder(filePaths[i]);
            Object file = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            crtEffect_ = file.name;
            ListEffectRef(((GameObject)file).transform);
        }
        string jsonStr = LitJson.JsonMapper.ToJson(effectRefDic_);
        System.IO.File.WriteAllText(Application.dataPath + "/../../Config/Tables/EffectDependence.json", jsonStr);
        AssetDatabase.Refresh();
        ClientLog.Instance.Log("Success");
    }

    static Dictionary<string, List<string>> effectRefDic_ = new Dictionary<string, List<string>>();
    static string crtEffect_;
    static void ListEffectRef(Transform trans)
    {
        if (!effectRefDic_.ContainsKey(crtEffect_))
        {
            //PlayerReference pr = new PlayerReference();
            //pr.animator = new List<string>();
            //pr.mesh = new List<string>();
            //pr.material = new List<string>();
            effectRefDic_.Add(crtEffect_, new List<string>());
        }

        string path = "";
        string guid = "";
        Animation anima = trans.GetComponent<Animation>();
        if (anima != null)
        {
            foreach (AnimationState state in anima)
            {
                path = AssetDatabase.GetAssetPath(state.clip);
                guid = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid))
                {
                    if (!effectRefDic_[crtEffect_].Contains(guid))
                        effectRefDic_[crtEffect_].Add(guid);
                }
            }
        }

        MeshRenderer mr = trans.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            if (mr.sharedMaterial != null && mr.sharedMaterial.shader != null && mr.sharedMaterial.mainTexture != null)
            {
                //bool mpackMatOnly = false;
                //path = AssetDatabase.GetAssetPath(mr.sharedMaterial.shader);
                //if (!path.Equals("Resources/unity_builtin_extra"))
                //{
                //    guid = AssetDatabase.AssetPathToGUID(path);
                //    if (!string.IsNullOrEmpty(guid))
                //    {
                //        if (!effectRefDic_[crtEffect_].Contains(guid))
                //            effectRefDic_[crtEffect_].Add(guid);
                //    }
                //}
                //else
                //    mpackMatOnly = true;

                //path = AssetDatabase.GetAssetPath(mr.sharedMaterial);
                //if (!path.Equals("Resources/unity_builtin_extra"))
                //{
                //    guid = AssetDatabase.AssetPathToGUID(path);
                //    if (!string.IsNullOrEmpty(guid))
                //    {
                //        if (!effectRefDic_[crtEffect_].Contains(guid))
                //            effectRefDic_[crtEffect_].Add(guid);
                //    }
                //}
                //else
                //    mpackMatOnly = true;

                //if (!mpackMatOnly)
                //{
                    path = AssetDatabase.GetAssetPath(mr.sharedMaterial.mainTexture);
                    if (!path.Equals("Resources/unity_builtin_extra"))
                    {
                        guid = AssetDatabase.AssetPathToGUID(path);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            if (!effectRefDic_[crtEffect_].Contains(guid))
                                effectRefDic_[crtEffect_].Add(guid);
                        }
                    }
                //}
            }
        }

        ParticleSystem ps = trans.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            //bool packMatOnly = false;
            //if (ps.renderer != null && ps.renderer.sharedMaterial != null && ps.renderer.sharedMaterial.shader != null)
            //{
            //    path = AssetDatabase.GetAssetPath(ps.renderer.sharedMaterial.shader);
            //    if (!path.Equals("Resources/unity_builtin_extra"))
            //    {
            //        guid = AssetDatabase.AssetPathToGUID(path);
            //        if (!string.IsNullOrEmpty(guid))
            //        {
            //            if (!effectRefDic_[crtEffect_].Contains(guid))
            //                effectRefDic_[crtEffect_].Add(guid);
            //        }
            //    }
            //    else
            //        packMatOnly = true;
            //}

            //if (ps.renderer != null && ps.renderer.sharedMaterial != null)
            //{
            //    path = AssetDatabase.GetAssetPath(ps.renderer.sharedMaterial);
            //    if (!path.Equals("Resources/unity_builtin_extra"))
            //    {
            //        guid = AssetDatabase.AssetPathToGUID(path);
            //        if (!string.IsNullOrEmpty(guid))
            //        {
            //            if (!effectRefDic_[crtEffect_].Contains(guid))
            //                effectRefDic_[crtEffect_].Add(guid);
            //        }
            //    }
            //    else
            //        packMatOnly = true;
            //}

            //if (!packMatOnly)
            //{
                if (ps.renderer != null && ps.renderer.sharedMaterial != null && ps.renderer.sharedMaterial.mainTexture != null)
                {
                    path = AssetDatabase.GetAssetPath(ps.renderer.sharedMaterial.mainTexture);
                    if (!path.Equals("Resources/unity_builtin_extra"))
                    {
                        guid = AssetDatabase.AssetPathToGUID(path);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            if (!effectRefDic_[crtEffect_].Contains(guid))
                                effectRefDic_[crtEffect_].Add(guid);
                        }
                    }
                }
            //}
        }
        for (int i = 0; i < trans.childCount; ++i)
        {
            ListEffectRef(trans.GetChild(i));
        }
    }

    [MenuItem("Tools/DestroyEmptyAnimator")]
    public static void FindEmptyAnimator()
    {
        Animator[] animators = GameObject.FindObjectsOfType<Animator>();
        for (int i = 0; i < animators.Length; ++i)
        {
            if (animators[i].runtimeAnimatorController == null)
            {
                GameObject.DestroyImmediate(animators[i]);
            }
        }
        ClientLog.Instance.Log("Destroy " + animators.Length + " Animators.");
    }

    [MenuItem("Tools/ChangeAllFBX2RWEnableFalse&MeshCompression")]
    public static void MeshCompression()
    {
        Object[] fbxs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < fbxs.Length; ++i)
        {
            if (fbxs[i] is GameObject)
            {
                string path = AssetDatabase.GetAssetPath(fbxs[i]);
                ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
                importer.isReadable = false;
                importer.meshCompression = ModelImporterMeshCompression.High;
                AssetDatabase.ImportAsset(path);
            }
        }
    }

    [MenuItem("Tools/BakeLightMapping/Bake512")]
    static void BakeLightMapping512()
    {
        LightmapEditorSettings.maxAtlasHeight = 512;
        LightmapEditorSettings.maxAtlasWidth = 512;
        Lightmapping.Clear();
        Lightmapping.Bake();
    }

    [MenuItem("Tools/BakeLightMapping/Bake256")]
    static void BakeLightMapping256()
    {
        LightmapEditorSettings.maxAtlasHeight = 256;
        LightmapEditorSettings.maxAtlasWidth = 256;
        Lightmapping.Clear();
        Lightmapping.Bake();
    }

    [MenuItem("Tools/BakeLightMapping/Bake128")]
    static void BakeLightMapping128()
    {
        LightmapEditorSettings.maxAtlasHeight = 128;
        LightmapEditorSettings.maxAtlasWidth = 128;
        Lightmapping.Clear();
        Lightmapping.Bake();
    }

    [MenuItem("Tools/ClearCollider")]
    static void ClearMeshCollider()
    {
        MeshCollider[] allMeshCollider = GameObject.FindObjectsOfType<MeshCollider>();
        for (int i = 0; i < allMeshCollider.Length; ++i)
        {
            if (allMeshCollider[i].gameObject.layer == 1 << LayerMask.NameToLayer("Ground"))
                continue;

            DestroyImmediate(allMeshCollider[i]);
        }
        ClientLog.Instance.Log("Clear " + allMeshCollider.Length + " MeshColliders.");

        BoxCollider[] allBoxCollider = GameObject.FindObjectsOfType<BoxCollider>();
        for (int i = 0; i < allBoxCollider.Length; ++i)
        {
            if (allBoxCollider[i].gameObject.layer == 1 << LayerMask.NameToLayer("Ground"))
                continue;

            DestroyImmediate(allBoxCollider[i]);
        }
        ClientLog.Instance.Log("Clear " + allBoxCollider.Length + " allBoxCollider.");
    }


    static int totalPanel_;
    static int totalSp_;
    [MenuItem("Tools/AdjustUIDepth")]
    static void AdjustUIDepth()
    {
        Object[] panels = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets);
        for (int i = 0; i < panels.Length; ++i)
        {
            Excute(((GameObject)panels[i]).transform, 0, 0);
            ClientLog.Instance.Log(panels[i].name + " Adjusted.");
        }
    }

    static void Excute(Transform t, int panelDepth, int widDepth)
    {
        UIPanel panel = t.GetComponent<UIPanel>();
        if (panel != null)
        {
            panel.depth = panelDepth;
            panelDepth ++;
        }

        UIWidget wid = t.GetComponent<UIWidget>();
        if (wid != null)
        {
            wid.depth = widDepth;
            widDepth ++;
        }
        
        for(int i=0; i < t.childCount; ++i)
        {
            Excute(t.GetChild(i), panelDepth, widDepth);
        }
    }

    [MenuItem("Tools/OpenObjectOpti")]
    public static void OpenObjectOpti()
    {
        Object[] fbxs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < fbxs.Length; ++i)
        {
            if (fbxs[i] is GameObject)
            {
                string path = AssetDatabase.GetAssetPath(fbxs[i]);
                ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
                if (importer == null)
                    continue;
                importer.optimizeGameObjects = true;
                AssetDatabase.ImportAsset(path);
            }
        }
        UnityEngine.Debug.Log("Success.");
    }

    [MenuItem("Tools/ChangeShader")]
    public static void ChangeShader()
    {
        Object[] fbxs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < fbxs.Length; ++i)
        {
            if (fbxs[i] is Material)
            {
                if(fbxs[i].name.Equals("yingzi"))
                {
                    ((Material)fbxs[i]).shader = Shader.Find("Unlit/Transparent");
                }
                else
                {
                    ((Material)fbxs[i]).shader = Shader.Find("Unlit/Texture");
                }
            }
        }
        UnityEngine.Debug.Log("Success.");
    }
}
