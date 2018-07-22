package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	_ "strings"
)

type DoIsSever struct {
	beego.Controller
}

func (c *DoIsSever) Post() {
	c.Ctx.Request.ParseForm()

	isnew_sever := c.Input().Get("sever_id")
	isnew := c.Input().Get("isnew")
	//beego.Debug(isnew_sever)
	//beego.Debug(isnew)
	req := httplib.Post(beego.AppConfig.String("carriershost") + "servs/newserv")
	req.Param("servid", isnew_sever)
	req.Param("isnew", isnew)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	c.Redirect("/alert?param="+string(str), 302)

}
