package servs

import (
	"carriers/models"
	_ "encoding/json"
	"github.com/astaxie/beego"
)

type Query struct {
	beego.Controller
}

func (c *Query) Post() {
	//beego.Debug(string(c.Ctx.Input.RequestBody))
	err := c.Ctx.Request.ParseForm()
	if err != nil {
		beego.Debug(err)
	}
	username := c.Input().Get("username")
	version := c.Input().Get("version")
	channel := c.Input().Get("channel")

	beego.Info("Client query serverlist ", username, "|", version, "|", channel)

	s := models.GetServsJson4(username, version, channel)

	c.Ctx.WriteString(s)

	//record := QueryRecord{username, channel, version, s, time.Now().Format("2006-01-02 15:04:05"), c.Ctx.Request.RemoteAddr}

	//go InsertQueryRecord(&record)
}
