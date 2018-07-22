package routers

import (
	"ninegame/controllers"
	"github.com/astaxie/beego"
)

func init() {
	beego.Router("/insert", &controllers.UCGift{})
	beego.Router("/uc/query.php", &controllers.GetPlayerInfomation{})
	beego.Router("/uc/servs.php", &controllers.GetServerList{})
	beego.Router("/uc/post.php",&controllers.SendNineGameGift{})
	beego.Router("/uc/check.php",&controllers.GetGiftCount{})
}
