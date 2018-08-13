using UnityEngine;
using System.Collections;

public class XyskAndroidAPI {

    public static string getMacAndroid()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.xysk.androidapi.XyskActivity");
        string mac = "";
        mac = jc.CallStatic<string>("getMACAddress", new object[]{"wlan0"});
        if(string.IsNullOrEmpty(mac))
			mac = jc.CallStatic<string>("getMACAddress", new object[]{"eth0"});
        return mac;
#endif
        return "";
    }
    public static float GetButtery()
    {
#if UNITY_ANDROID
        //AndroidJavaClass jc = new AndroidJavaClass("com.xysk.androidapi.XyskActivity");
        //return jc.CallStatic<int>("GetButtery");
        if (Application.platform == RuntimePlatform.Android)
         {
             try
             {
                 using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                 {
                     if (null != unityPlayer)
                     {
                         using (AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                         {
                             if (null != currActivity)
                             {
                                 using (AndroidJavaObject intentFilter = new AndroidJavaObject("android.content.IntentFilter", new object[]{ "android.intent.action.BATTERY_CHANGED" }))
                                 {
                                     using (AndroidJavaObject batteryIntent = currActivity.Call<AndroidJavaObject>("registerReceiver", new object[]{null,intentFilter}))
                                     {
                                         int level = batteryIntent.Call<int>("getIntExtra", new object[]{"level",-1});
                                         int scale = batteryIntent.Call<int>("getIntExtra", new object[]{"scale",-1});
 
                                         // Error checking that probably isn't needed but I added just in case.
                                         if (level == -1 || scale == -1)
                                         {
                                             return -1f;
                                         }
                                         return ((float)level / (float)scale) * 100.0f; 
                                     }
                                 
                                 }
                             }
                         }
                     }
                 }
             } catch (System.Exception ex)
             {
               
             }
         }
#endif
        return -1;
    }

    public static int getWifiStatus()
    {
#if UNITY_ANDROID
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                if (null != unityPlayer)
                {
                    using (AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        if (null != currActivity)
                        {
                            using (AndroidJavaObject context = currActivity.Call<AndroidJavaObject>("getApplicationContext"))
                            {
                                using (AndroidJavaObject wifiMgr = context.Call<AndroidJavaObject>("getSystemService", new object[]{"wifi"}))
                                {
                                    using (AndroidJavaObject connecInfo = wifiMgr.Call<AndroidJavaObject>("getConnectionInfo"))
                                    {
                                        using (AndroidJavaObject rssi = connecInfo.Call<AndroidJavaObject>("getRssi"))
                                        {
											int level = -70;
											level = wifiMgr.CallStatic<int>("calculateSignalLevel", new object[]{rssi, level});
                                            return level; 
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            return -70;
        }
#endif
        return -70;
    }

    public static bool hasMicrophoneAuth()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.xysk.androidapi.XyskActivity");
        return jc.CallStatic<bool>("isHasPermission");
#endif
        return true;
    }

    public static string getPackageVersion()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            if (null != unityPlayer)
            {
                using (AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    if (null != currActivity)
                    {
                        using (AndroidJavaObject pkgMgr = currActivity.Call<AndroidJavaObject>("getPackageManager"))
                        {
                            string pkgName = currActivity.Call<string>("getPackageName");
                            using (AndroidJavaObject pkgInfo = pkgMgr.Call<AndroidJavaObject>("getPackageInfo", new object[] { pkgName, 0 }))
                            {
                                return pkgInfo.Get<string>("versionName");
                            }
                        }
                    }
                }
            }
        }
        return "";
#endif
        return "";
    }
}
