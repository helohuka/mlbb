package servs

import (
	"carriers/models"
	"encoding/json"

	"github.com/astaxie/beego"
)

type Inilizer struct {
	beego.Controller
}

func (c *Inilizer) Post() {
	c.Ctx.Request.ParseForm()
	str := c.Input().Get("json")
	data := models.ServData{}
	_ = json.Unmarshal([]byte(str), &data)
	models.UpdateServ(&data)
	c.Ctx.WriteString("OK")
}
