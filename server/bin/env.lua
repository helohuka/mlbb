
Sys.log("running environment")

--//ADDR & PORT CONFIGURE
Env.setString(V_WorldListenDB,		"127.0.0.1:20002");
Env.setString(V_WorldListenLogin,	"127.0.0.1:20003");
Env.setString(V_WorldListenMall,	"127.0.0.1:20004");
Env.setString(V_WorldListenGateway,	"127.0.0.1:20005");
Env.setString(V_WorldListenScene,	"127.0.0.1:20006");
Env.setString(V_WorldListenLogser,	"127.0.0.1:20007");
Env.setString(V_WorldListenGMT,		"127.0.0.1:20010");

--//机器人
Env.setString(V_GatewayHost,"127.0.0.1:20001"); --//机器人配置 需要和V_GatewayListenClientMultiIndoor 其中一个端口一样

Env.setString(V_GatewayListenClientMultiIndoor,"1,21000"); --//网关监听客户端  

Env.setString(V_MysqlHost,		"10.10.10.254:3306");
Env.setString(V_MysqlUser,		"xysk");
Env.setString(V_MysqlPassword,	"123456");
Env.setString(V_DatabaseName,	"8000_game");
Env.setString(V_logersName,		"8000_log");
--//

Env.setInt(V_DebugLog,1);
Env.setInt(V_GM, 1);
--//是否使用any sdk
Env.setInt(V_UsedAnySDK,0);
Env.setString(V_GMTListenWebServer,	"0.0.0.0:20082");
Env.setInt(V_PayListenPort,	21081);

Env.setString(V_GMTListenWebServer,	"0.0.0.0:20082");


Env.setString(V_CenterServerHost, "http://127.0.0.1");		--//分服服务器地址
Env.setString(V_LogServerHost, "http://10.19.182.167:10998"); --//日志服务器地址 (这个日志就是中央数据库的伙伴库)

Env.setString(V_TableFolder, "../../config/Tables/");	--//手机号那个平台这个不用动
Env.setString(V_ScriptFolder, "../../config/Script/");

