using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDK185 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        
        SDKInterface.Instance.Init();

        ClientLog.Instance.LogError("INIT  ______");
        SDKInterface.Instance.OnLoginSuc = delegate (LoginResult result)
        {
            OnLoginSuc(result);
        };

        SDKInterface.Instance.OnLogout = delegate ()
        {
            OnLogout();
        };

        SDKInterface.Instance.OnPaySuc = delegate (int payCode)
        {
            OnPaySuc(payCode);
        };

        DontDestroyOnLoad(this);
        // SDKInterface.Instance.Login();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))//返回键
        {
            SDKInterface.Instance.SDKExit();
        }
    }


    void OnLoginSuc(LoginResult result)
    {
        ClientLog.Instance.LogError("void OnLoginSuc(LoginResult result)");
        if (!result.isSuc)
        {
            ClientLog.Instance.LogError("if (!result.isSuc)");
            return;
        }

    
        if (result.isSwitchAccount)
        {
            ClientLog.Instance.LogError("if (result.isSwitchAccount)");
            //TODO:这里退回到登录界面，并且SDK是已经登录成功状态，不用再弹出SDK登录框
            //TODO:登录认证成功，获取服务器列表，进入游戏服...
        }
        else
        {
            ClientLog.Instance.LogError("Login success .." + result.userID + "  " + result.token);

            UILoginPanel.userName_ = result.userID;
            CommonEvent.ExcuteUserExternal(0);

            //TODO:登录认证成功，获取服务器列表，进入游戏服...
        }

        SubmitGameData(ExtraGameData.TYPE_ENTER_GAME);
    }

    void OnLogout()
    {
       
    }

    void OnPaySuc(int payCode)
    {
        if (payCode == 1)
        {
            UnityEngine.Debug.LogError("SuperSYSDK pay success...");
            //TODO:请求游戏服刷新元宝
        }
        else
        {
            UnityEngine.Debug.LogError("SuperSYSDK pay failed...");
        }
    }

    void OnLogoutClick()
    {
        SDKInterface.Instance.Logout();
    }

    void OnPayClick()
    {
        PayParams data = new PayParams();
        data.productId = "1";
        data.productName = "元宝";
        data.productDesc = "购买100元宝";
        data.price = 1; //单位 元
        data.buyNum = 1; 
        data.coinNum = 100;
        data.serverId = 10;
        data.serverName = "测试";
        data.roleId = "1";
        data.roleName = "测试角色名";
        data.roleLevel = 1;
        data.vip = "vip1";
        data.extension = "unity";

        SDKInterface.Instance.Pay(data);
    }

    void SubmitGameData(int type)
    {
        ExtraGameData data = new  ExtraGameData();
        data.dataType = type;
        data.roleID = "role_100";
        data.roleName = "测试角色名";
        data.roleLevel = 10;
        data.serverID = 10;
        data.serverName = "10服";
        data.moneyNum = 100;
        data.roleCreateTime = 1523237901;
        data.roleLevelUpTime = 0;

       // SDKInterface.Instance.SubmitGameData(data);
    }

    static PayParams pp = new PayParams();
    public static void Pay(int id, int stack = 1)
    {

        ShopData sd = ShopData.GetData(id);

        if (sd == null)
        {
            ClientLog.Instance.LogError("Can not find shop id " + id);
            return;
        }

        pp.buyNum = stack;
        pp.coinNum = (int)GamePlayer.Instance.GetProperty(PropertyType.PT_Diamond);
        pp.price = sd._Price;
        pp.productId = sd._Id.ToString();
        pp.productName = sd._Name;
        pp.productDesc = sd._Name;
        pp.roleId = GamePlayer.Instance.InstId.ToString();
        pp.roleLevel = (int)GamePlayer.Instance.GetProperty(PropertyType.PT_Level);
        pp.roleName = GamePlayer.Instance.InstName;
        pp.serverId = GameManager.ServId_;
        pp.serverName = GameManager.ServName_;

        SDKInterface.Instance.Pay(pp);
    }

    public static void Pay(int shopid, int price,  string shopdesc)
    {

        pp.buyNum = 1;
        pp.coinNum = (int)GamePlayer.Instance.GetProperty(PropertyType.PT_Diamond);
        pp.price = price;
        pp.productId = shopid.ToString();
        pp.productName = shopdesc;
        pp.productDesc = shopdesc;
        pp.roleId = GamePlayer.Instance.InstId.ToString();
        pp.roleLevel = (int)GamePlayer.Instance.GetProperty(PropertyType.PT_Level);
        pp.roleName = GamePlayer.Instance.InstName;
        pp.serverId = GameManager.ServId_;
        pp.serverName = GameManager.ServName_;

        SDKInterface.Instance.Pay(pp);
    }
}
