package routers

import (
	"github.com/astaxie/beego"
	"statistics/controllers"
)

func init() {
	beego.Router("/statistics", &controllers.Statistics{})
}
