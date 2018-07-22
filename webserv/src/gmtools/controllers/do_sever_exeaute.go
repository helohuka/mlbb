package controllers

import (
	_ "database/sql"
	//"encoding/json"
	_ "errors"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	_ "gmtools/models"
	_ "strconv"
)

type DoSever struct {
	beego.Controller
}

func (c *DoSever) Post() {
	c.Ctx.Request.ParseForm()

	//SSHUsername := c.GetString("SSHUsername")
	SSHPassword := c.GetString("SSHPassword")
	severid := c.GetString("sever_id")
	beego.Debug(severid)
	req := httplib.Post(beego.AppConfig.String("carriershost") + "servs/restart")
	//req.Param("username", SSHUsername)
	req.Param("password", SSHPassword)
	req.Param("servid", severid)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	c.Redirect("/alert?param="+string(str), 302)
}
