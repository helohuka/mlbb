package controllers

import (
	_ "encoding/json"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type ChannelMappingCmd struct {
	Type string
}

type DoChannelsMapping struct {
	beego.Controller
}

func (c *DoChannelsMapping) Post() {
	c.Ctx.Request.ParseForm()
	anyId := c.Input().Get("anyId")
	pfId := c.Input().Get("pfId")
	pfName := c.Input().Get("pfName")
	desc := c.Input().Get("desc")
	req := httplib.Post(beego.AppConfig.String("carriershost") + "sdk/intert")
	req.Param("anyId", anyId)
	req.Param("pfId", pfId)
	req.Param("pfName", pfName)
	req.Param("desc", desc)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	c.Ctx.WriteString(string(str))

}
