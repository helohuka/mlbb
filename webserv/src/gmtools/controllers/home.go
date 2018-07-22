package controllers

import (
	"encoding/json"
)

import (
	"gmtools/models"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type Home2 struct {
	beego.Controller
}

func (this *Home2) Get() {
	this.Ctx.WriteString("Get Function ")
}
func (this *Home2) Post() {

	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/fetch")
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}

	models.UpdateServsByJson(str)

	areas := models.GetServerMap()

	jparam, _ := json.Marshal(areas)

	this.Ctx.WriteString(string(jparam))
}
