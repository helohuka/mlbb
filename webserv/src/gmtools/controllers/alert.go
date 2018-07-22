package controllers

import (
	"encoding/json"

	"github.com/astaxie/beego"
)

type Alert struct {
	beego.Controller
}

func (c *Alert) Get() {
	c.Ctx.Request.ParseForm()
	c.TplName = "alert.html"
	results := []DoResult{}
	json.Unmarshal([]byte(c.Input().Get("param")), &results)

	c.Ctx.WriteString(string(c.Input().Get("param")))
}
