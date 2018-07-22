package controllers

import (
	"github.com/astaxie/beego"

)

type (
	//角色
	ClientLogger struct {
		beego.Controller
	}

)

func (this *ClientLogger) Post() {
	beego.Info(this.GetString("log"))
	this.Ctx.WriteString("")
}

