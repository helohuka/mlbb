package cl

import (
	"carriers/models"

	"fmt"
	"github.com/astaxie/beego"
)

type Channels struct {
	beego.Controller
}

func (c *Channels) Post() {

	err := c.Ctx.Request.ParseForm()
	if err != nil {
		beego.Debug(err)
	}
	username := c.GetString("username")
	version := c.GetString("version")
	//channel := c.GetString("channel")

	//beego.Info("Client query serverlist ", username, "|", version, "|", channel)

	s := models.GetServsJson4(username, version, fmt.Sprintf("%d", channel))

	c.Ctx.WriteString(s)

}
