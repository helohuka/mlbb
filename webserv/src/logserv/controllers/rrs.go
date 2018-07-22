package controllers

import (
	"encoding/json"
	"logserv/models"

	"github.com/astaxie/beego"
)

type RRSController struct {
	beego.Controller
}

func (this *RRSController) Get() {
	r := models.CalcRRS()
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}
