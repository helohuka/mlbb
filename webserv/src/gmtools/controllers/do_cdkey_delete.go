package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type DoCdkeyDelete struct {
	beego.Controller
}

func (this *DoCdkeyDelete) Post() {
	this.Ctx.Request.ParseForm()

	tag := this.GetString("cdkey_select")
	//req := httplib.Get("http://10.10.10.254:18080/servs/delcdkey?tag=" + tag)
	req := httplib.Get(beego.AppConfig.String("carriershost") + "servs/delcdkey?tag=" + tag)
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	this.Ctx.WriteString(str)

}
