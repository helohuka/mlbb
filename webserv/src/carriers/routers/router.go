package routers

import (
	"carriers/controllers/gmtools"
	"carriers/controllers/login"
	"carriers/controllers/notice"
	"carriers/controllers/pay"
	"carriers/controllers/sdk"
	"carriers/controllers/servs"
	"carriers/controllers/web"
	"carriers/tables"

	"github.com/astaxie/beego"

	"carriers/controllers/2"
	"carriers/controllers/cl"
)

func init() {

	beego.Router("/servs/cdn", &servs.CDN{})
	beego.Router("/servs/query", &servs.Query{})
	beego.Router("/servs/query2", &servs.Query2{})
	beego.Router("/servs/inilize", &servs.Inilizer{})
	beego.Router("/servs/update", &servs.Update{})
	beego.Router("/servs/create", &servs.Create{})
	beego.Router("/servs/newserv", &servs.ChangeNewServer{})
	beego.Router("/servs/updateserver", &servs.UpdateServer{})
	beego.Router("/servs/backup", &servs.BackupServer{})
	beego.Router("/servs/restart", &servs.RestartServer{})
	beego.Router("/servs/cdkey", &servs.CDKey{})
	beego.Router("/servs/gencdkey", &servs.GenCDKEY{})
	beego.Router("/servs/delcdkey", &servs.RMCDKEY{})
	beego.Router("/servs/querycdkey", &servs.QueryCDKEY{})

	beego.Router("/servs/logs", &servs.InsertLog{})

	beego.Router("/notice/system", &notice.System{})
	beego.Router("/notice/server", &notice.Server{})

	beego.Router("/login/anyoauth", &login.AnyOauth{})
	beego.Router("/login/oauth", &login.Oauth{})
	beego.Router("/pay/notify/private", &pay.Private{})
	beego.Router("/pay/notify/nhanced", &pay.Enhanced{})

	beego.Router("/gmtools/fetch", &gmtools.Fetch{})
	beego.Router("/gmtools/channel", &gmtools.Channel{})
	beego.Router("/gmtools/notice/system", &gmtools.SystemNotice{})
	beego.Router("/gmtools/notice/server", &gmtools.ServerNotice{})
	beego.Router("/sdk/intert", &sdk.UpdateAnyId{})

	beego.Router("/web/tanyu/servers", &web.GetServerList{})
	beego.Router("/web/tanyu/rolelist", &web.GetRoleList{})
	beego.Router("/web/tanyu/pay", &web.Pay{})

	beego.Router("/tables/order", &tables.GetOrder{})

	beego.Router("/login/anyoauth/2", &subserver.AnyOauth{})
	beego.Router("/login/oauth/2", &subserver.Oauth{})
	beego.Router("/notice/system/2", &subserver.System{})
	beego.Router("/notice/server/2", &subserver.Server{})
	beego.Router("/servs/query/2", &subserver.Query{})
	beego.Router("/pay/notify/private/2", &subserver.Private{})

	beego.Router("/cl/servers", &cl.Channels{})
	beego.Router("/cl/login", &cl.Login{})
	beego.Router("/cl/notifypay", &cl.Pay{})
	beego.Router("/cl/notifypay2", &cl.Pay2{})
}
