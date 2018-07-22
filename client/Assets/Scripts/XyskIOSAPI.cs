using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class XyskIOSAPI {

    [DllImport("__Internal")]
    private static extern string _getIDFA();

    public static string GetIDFA()
    {
        return _getIDFA();
    }

    [DllImport("__Internal")]
    private static extern float _getButtery();

    public static float GetButtery()
    {
        return _getButtery();
    }

    [DllImport("__Internal")]
    private static extern string _getVersion();

    public static string GetVersion()
    {
        return _getVersion();
    }

    [DllImport("__Internal")]
    private static extern bool _hasMicrophoneAuth();

    public static bool HasMicrophoneAuth()
    {
        return _hasMicrophoneAuth();
    }

    [DllImport("__Internal")]
    private static extern bool _setNotBackup();

    public static bool SetNotBackup()
    {
        return _setNotBackup();
    }

    [DllImport("__Internal")]
    private static extern string _getIPv6(string mHost, string mPort);

    public static string GetIPv6(string mHost, string mPort)
    {
        return _getIPv6(mHost, mPort);
    }

    [DllImport("__Internal")]
    private static extern void _initEAuth(string appid, string secretid);

    public static void InitEAuth(string appid, string secretid)
    {
        _initEAuth(appid, secretid);
    }

    [DllImport("__Internal")]
    private static extern void _requestCode(string mobile, string userid);

    public static void RequestCode(string mobile, string userid)
    {
        _requestCode(mobile, userid);
    }

    [DllImport("__Internal")]
    private static extern void _auth(string mobile, string userid, string code);

    public static void Auth(string mobile, string userid, string code)
    {
        _auth(mobile, userid, code);
    }
}
