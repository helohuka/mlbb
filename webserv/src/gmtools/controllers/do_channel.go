package controllers

import (
	_ "database/sql"
	//"encoding/json"
	_ "errors"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type DoChannelCmd struct {
	Type string
}

type DoChannel struct {
	beego.Controller
}

func (c *DoChannel) Post() {
	c.Ctx.Request.ParseForm()

	severid := c.GetString("id")
	severname := c.GetString("name")
	versions := c.GetString("version")
	CDN := c.GetString("CDN")
	channels := c.GetString("qd")
	sandbox := c.GetString("sandbox")
	req := httplib.Post(beego.AppConfig.String("carriershost") + "servs/update")
	req.Param("servid", severid)
	req.Param("channels", channels)
	req.Param("version", versions)
	req.Param("resVersion", CDN)
	req.Param("sandbox", sandbox)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	beego.Debug(severname)
	c.Ctx.WriteString(string(str))

}
