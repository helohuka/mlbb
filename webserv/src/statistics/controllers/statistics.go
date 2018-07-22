package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	_ "github.com/go-sql-driver/mysql"
	"statistics/models"
)

type Statistics struct {
	beego.Controller
}

func (c *Statistics) Post() {
	var params map[string][]string = c.Ctx.Request.Form

	b, _ := json.Marshal(params)

	end := string(b)
	models.Statistics(end)

	c.Ctx.WriteString("")
}
