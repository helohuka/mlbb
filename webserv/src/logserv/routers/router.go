package routers

import (
	"logserv/controllers"

	"github.com/astaxie/beego"
)

func init() {
	beego.Router("/channel/dnu", &controllers.DNUController3{})

	beego.Router("/account/dau", &controllers.DAUController2{})
	beego.Router("/account/dnu", &controllers.DNUController2{})

	beego.Router("/player/dau", &controllers.DAUController{})
	beego.Router("/player/dnu", &controllers.DNUController{})
	beego.Router("/player/rrs", &controllers.RRSController{})
	beego.Router("/insert", &controllers.InsertController{})

	beego.Router("/log", &controllers.ClientLogger{})
}
