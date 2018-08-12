using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SuperSYSDK Unity 统一调用单例接口
/// </summary>
public abstract class SDKInterface
{

    public delegate void LoginSucHandler(LoginResult data);
    public delegate void LogoutHandler();
    public delegate void PayHandler(int payCode);

    private static SDKInterface _instance;
    public LoginSucHandler OnLoginSuc;
    public LogoutHandler OnLogout;
    public PayHandler OnPaySuc;

    public static SDKInterface Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR || UNITY_STANDLONE
                _instance = new SDKInterfaceDefault();
#elif UNITY_ANDROID
                _instance = new SDKInterfaceAndroid();
#elif UNITY_IOS
                _instance = new SDKInterfaceIOS();
#endif
            }

            return _instance;
        }
    }

    //初始化
    public virtual void Init()
    {
        SuperSYSDKCallback.InitCallback();
    }

    //登录
    public abstract void Login();

    //登出
    public abstract bool Logout();

    //上传游戏数据
    public abstract void SubmitGameData(ExtraGameData data);

    //退出游戏
    public abstract bool SDKExit();

    //调用SDK支付界面
    public abstract void Pay(PayParams data);

}

/// <summary>
/// 支付接口需要的参数
/// </summary>
public class PayParams
{
    //购买数量,一般都为1
    public int buyNum { get; set; }

    //当前玩家身上剩余的虚拟币数量
    public int coinNum { get; set; }

    //价格，单位为元
    public int price { get; set; }

    //游戏中商品ID
    public string productId { get; set; }

    //游戏中商品名称，比如元宝，钻石...
    public string productName { get; set; }

    //游戏中商品描述
    public string productDesc { get; set; }

    //当前角色ID
    public string roleId { get; set; }

    //当前角色名称
    public string roleName { get; set; }

    //当前角色等级
    public int roleLevel { get; set; }

    //当前角色所在的服务器ID
    public int serverId { get; set; }

    //当前角色所在的服务器名称
    public string serverName { get; set; }

    //当前角色的vip等级
    public string vip { get; set; }

    //扩展数据， 支付成功回调通知游戏服务器的时候，会原封不动返回这个值
    public string extension { get; set; }
}


/// <summary>
/// 数据上报接口需要的参数
/// </summary>
public class ExtraGameData
{

    public const int TYPE_SELECT_SERVER = 1;        //选择服务器
    public const int TYPE_CREATE_ROLE = 2;          //创建角色
    public const int TYPE_ENTER_GAME = 3;           //进入游戏
    public const int TYPE_LEVEL_UP = 4;				//等级提升
    public const int TYPE_EXIT_GAME = 5;			//退出游戏

    //调用时机，设置为上面定义的类型，在各个对应的地方调用submitGameData方法
    public int dataType { get; set; }

    //角色ID
    public string roleID { get; set; }

    //角色名称
    public string roleName { get; set; }

    //角色等级
    public int roleLevel { get; set; }

    //服务器ID
    public int serverID { get; set; }

    //服务器名称
    public string serverName { get; set; }

    //当前角色生成拥有的虚拟币数量
    public int moneyNum { get; set; }

    //角色创建时间，从1970年到现在的时间，单位秒
    public int roleCreateTime { get; set; }

    //角色等级变化时间，从1970年到现在的时间，单位秒
    public int roleLevelUpTime { get; set; }
	
	//vip
    public int vip { get; set; }
}

/// <summary>
/// 登录结果
/// </summary>
public class LoginResult
{
    //是否认证成功
    public bool isSuc { get; set; }

    //当前是否为SDK界面中切换帐号的回调
    public bool isSwitchAccount { get; set; }

    //全局唯一userID
    public string userID { get; set; }

    //登录认证的凭据
    public string token { get; set; }

}
