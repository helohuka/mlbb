package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
)

type QueryChatResult struct {
	beego.Controller
}

func (c *QueryChatResult) Get() {
	c.Ctx.Request.ParseForm()

	jparam, _ := json.Marshal(SayRecords)

	c.Ctx.WriteString(string(jparam))
}
