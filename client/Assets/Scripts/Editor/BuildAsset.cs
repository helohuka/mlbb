using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

public class BuildAsset
{

#if UNITY_EDITOR
    const BuildTarget target_ = BuildTarget.StandaloneWindows;
#elif UNITY_IOS || UNITY_IPHONE
    const BuildTarget target_ = BuildTarget.iPhone;
#elif UNITY_ANDROID
    const BuildTarget target_ = BuildTarget.Android;
#else
    const BuildTarget target_ = BuildTarget.StandaloneWindows;
#endif

    static string configFolder_ = Application.dataPath + "/../../config/Tables";

    const string resourceFolder_ = "Assets/ResData/";

    const string playerPrefabFolder_ = resourceFolder_ + "Role/Prefabs/";
    const string uiPrefabFolder_ = resourceFolder_ + "UI/UIPrefab/";
    const string weaponPrefabFolder_ = resourceFolder_ + "Weapon/";
    const string effectPrefabFolder_ = resourceFolder_ + "Effect/Prefab/";
    const string soundPrefabFolder_ = resourceFolder_ + "Sound/";

    const string musicPrefabFolder_ = resourceFolder_ + "Music/";
    const string iconPrefabFolder_ = resourceFolder_ + "Icon/";


    static string playerBundleOutputFolder_  = Configure.StreamAssetsFolder + "/Player/";
    static string uiBundleOutputFolder_ = Configure.StreamAssetsFolder + "/UI/";
    static string weaponBundleOutputFolder_ = Configure.StreamAssetsFolder + "/Weapon/";
    static string effectBundleOutputFolder_ = Configure.StreamAssetsFolder + "/Effect/";
    static string soundBundleOutputFolder_ = Configure.StreamAssetsFolder + "/Sound/";
    static string musicBundleOutputFolder_ = Configure.StreamAssetsFolder + "/Music/";
    static string iconBundleOutputFolder_ = Configure.StreamAssetsFolder + "/Icon/";
  
    static string playerDependConfigFileName_ = Configure.TableFolder + "PlayerDependence.json";
    static string uiDependConfigFileName_ = Configure.TableFolder + "UIDependence.json";
    static string effectDependConfigFileName_ = Configure.TableFolder + "EffectDependence.json";

    static Dictionary<string, List<string>> playerRefDic_ = new Dictionary<string, List<string>>();
    static Dictionary<string, List<string>> uiRefDic_ = new Dictionary<string, List<string>>();
    static Dictionary<string, List<string>> effectRefDic_ = new Dictionary<string, List<string>>();

    static void initPlayerRefAssets()
    {
        string jsonStr = File.ReadAllText(playerDependConfigFileName_);
        playerRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);
    }

    static List<string> getPlayerRefAssets(string playerName)
    {
        if (!playerRefDic_.ContainsKey(playerName))
            return null;
        return playerRefDic_[playerName];   
    }

    static void initUIRefAtlas()
    {
        string jsonStr = File.ReadAllText(uiDependConfigFileName_);
        uiRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);
    }
    static List<string> getUIRefAtlas(string uiName)
    {
        if (!uiRefDic_.ContainsKey(uiName))
            return new List<string>();
        return uiRefDic_[uiName];
    }
    static string findUIAtlas(string fileName)
    {
        string[] fileGuids = AssetDatabase.FindAssets(fileName + " t:GameObject", new string[] { "Assets/ResData/UI/UIAtlass" });
        if (fileGuids == null || fileGuids.Length == 0)
        {
            UnityEngine.Debug.LogError("Can not found atlas by " + fileName + " in project: ");
            return "";
        }
        string finalPath = "";
        if (fileGuids.Length > 1)
        {
            UnityEngine.Debug.LogError("More atlas by " + fileName + " in project: ");
            for (int i = 0; i < fileGuids.Length; ++i)
            {
                UnityEngine.Debug.LogError(AssetDatabase.GUIDToAssetPath(fileGuids[i]));
                if (!AssetDatabase.GUIDToAssetPath(fileGuids[i]).Contains(fileName + ".prefab"))
                    continue;
                finalPath = AssetDatabase.GUIDToAssetPath(fileGuids[i]);
                UnityEngine.Debug.LogError("Use this Above.");
            }
        }
        else
        {
            finalPath = AssetDatabase.GUIDToAssetPath(fileGuids[0]);
        }

        return finalPath;
    }

    static void initEffectRefAssets()
    {
        string jsonStr = File.ReadAllText(effectDependConfigFileName_);
        effectRefDic_ = LitJson.JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonStr);
    }

    static List<string> getRefEffAssets(string effectName)
    {
        if (!effectRefDic_.ContainsKey(effectName))
            return null;
        return effectRefDic_[effectName];
    }

    static string assetGuid2Path(string assetGuid)
    {
        return AssetDatabase.GUIDToAssetPath(assetGuid);
    }

    static void mkdir(string dir)
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
    }
    static List<Object> filter(string dir)
    {
        List<Object> res = new List<Object>();

        string[] files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; ++i)
        {
            if (files[i].Contains(".meta"))
                continue;
            string name = files[i].Substring(files[i].IndexOf("Assets"));
            Object file = UnityEditor.AssetDatabase.LoadAssetAtPath(name, typeof(Object));
            if (file != null)
                res.Add(file);
        }

        return res;
    }
    static void directoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                directoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    [MenuItem("BuildToolset/CopyConfig")]
    public static void BuildConfig()
    {
        directoryCopy(configFolder_, Configure.TableFolder, true);


    }

    [MenuItem("BuildToolset/MakePlayers")]
    public static void BuildPlayers()
    {

        mkdir(playerBundleOutputFolder_);
        Caching.CleanCache();
        //dep角色shader
        {
            BuildPipeline.PushAssetDependencies();
            string[] assetfiles = new string[] { "Assets/Scripts/Shader/PlayerShader.shader", "Assets/ResData/Role/yingzi/yingzi01.mat" };
            Object[] objfiles = new Object[assetfiles.Length];
            for (int i = 0; i < objfiles.Length; ++i)
            {
                objfiles[i] = AssetDatabase.LoadMainAssetAtPath(assetfiles[i]);
                BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(playerBundleOutputFolder_ + "{0}.bytes", "PlayerShader"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
            }
        }

        {

            initPlayerRefAssets();
            List<Object> files = filter(playerPrefabFolder_);
            BuildPipeline.PushAssetDependencies();
            for (int i = 0; i < files.Count; ++i)
            {
                List<string> pr = getPlayerRefAssets(files[i].name);
                if (pr != null)
                {
                    BuildPipeline.PushAssetDependencies();

                    //List<Object> refAsssets = new List<Object>();
                    for (int j = 0; j < pr.Count; ++j)
                    {
                        string assetfile = assetGuid2Path(pr[j]);
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                        BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(playerBundleOutputFolder_ + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
                        //refAsssets.Add(mainAsset);
                    }
                }
                //if (refAsssets.Count > 0)
                //    BuildPipeline.BuildAssetBundle(null, refAsssets.ToArray(), string.Format(pathName + "{0}{1}.bytes", files[i].name, "_dep"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, tar);

                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(playerBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
            }
            BuildPipeline.PopAssetDependencies();
            //if (refAsssets.Count > 0)
            BuildPipeline.PopAssetDependencies();
        }
        AssetDatabase.Refresh();
        
    }
    [MenuItem("BuildToolset/MakeUI")]
    static void BuildUI()
    {
        mkdir(uiBundleOutputFolder_);
        Caching.CleanCache();
        { 
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
            BuildPipeline.BuildAssetBundle(null, objfiles, string.Format(uiBundleOutputFolder_ + "{0}.bytes", "commonAssets"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
        }

        {
            initUIRefAtlas();
            List<Object> files = filter(uiPrefabFolder_);
            for (int i = 0; i < files.Count; ++i)
            {
                
                    List<string> refatlasLst = getUIRefAtlas(files[i].name);
                    if (refatlasLst.Count > 0)
                        BuildPipeline.PushAssetDependencies();
                    for (int j = 0; j < refatlasLst.Count; ++j)
                    {
                        string assetfile = findUIAtlas(refatlasLst[j]);
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                        BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(uiBundleOutputFolder_ + "{0}.bytes", refatlasLst[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
                    }
                    BuildPipeline.PushAssetDependencies();
                    BuildPipeline.BuildAssetBundle(files[i], null, string.Format(uiBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
                    BuildPipeline.PopAssetDependencies();
                    if (refatlasLst.Count > 0)
                        BuildPipeline.PopAssetDependencies();
            }

        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/MakeEffect")]
    public static void BuildEffect()
    {
        Caching.CleanCache();
        {
            initEffectRefAssets();
            List<Object> files = filter(effectPrefabFolder_);
            for (int i = 0; i < files.Count; ++i)
            {
                List<string> pr = getRefEffAssets(files[i].name);
                if (pr != null)
                {
                    BuildPipeline.PushAssetDependencies();

                    //List<Object> refAsssets = new List<Object>();
                    for (int j = 0; j < pr.Count; ++j)
                    {
                        string assetfile = assetGuid2Path(pr[j]);
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetfile);
                        if (mainAsset == null)
                        {
                            UnityEngine.Debug.Log(assetfile + " is can not find.");
                            continue;
                        }
                        BuildPipeline.BuildAssetBundle(mainAsset, null, string.Format(effectBundleOutputFolder_ + "{0}.bytes", pr[j]), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
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
                BuildPipeline.BuildAssetBundle(files[i], null, string.Format(effectBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
                BuildPipeline.PopAssetDependencies();
                //if (refAsssets.Count > 0)
                BuildPipeline.PopAssetDependencies();
            }
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/Weapon")]
    public static void BuildWeapon()
    {
        Caching.CleanCache();
        List<Object> files = filter(weaponPrefabFolder_);
        for (int i = 0; i < files.Count; ++i)
        {
            BuildPipeline.BuildAssetBundle(files[i], null, string.Format(weaponBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/MakeSound")]
    public static void BuildSound()
    {
        Caching.CleanCache();
        List<Object> files = filter(soundPrefabFolder_);
        for (int i = 0; i < files.Count; ++i)
        {
            BuildPipeline.BuildAssetBundle(files[i], null, string.Format(soundPrefabFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/MakeMusic")]
    public static void BuildMusic()
    {
        Caching.CleanCache();
        List<Object> files = filter(musicPrefabFolder_);
        for (int i = 0; i < files.Count; ++i)
        {
            BuildPipeline.BuildAssetBundle(files[i], null, string.Format(musicBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/MakeIcon")]
    public static void BuildIcon()
    {
        Caching.CleanCache();
        List<Object> files = filter(iconPrefabFolder_);
        for (int i = 0; i < files.Count; ++i)
        {
            BuildPipeline.BuildAssetBundle(files[i], null, string.Format(iconBundleOutputFolder_ + "{0}.bytes", files[i].name), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle, target_);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("BuildToolset/MakeAll")]
    public static void BuildAll()
    {
        BuildPlayers();
        BuildUI();
        BuildEffect();
        BuildWeapon();
        BuildSound();
        BuildMusic();
        BuildIcon();
    }
    public static void CopyTable()
    {

    }
}
