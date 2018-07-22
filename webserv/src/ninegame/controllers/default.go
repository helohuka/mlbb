package controllers

import (
	"github.com/astaxie/beego"
	"encoding/json"
	"ninegame/models"
)

type UCGift struct {
	beego.Controller
}

func (c *UCGift) Post() {
	beego.Debug(string(c.Ctx.Input.RequestBody))

	gift := models.UCGift{}

	err := json.Unmarshal(c.Ctx.Input.RequestBody,&gift)

	if err != nil{
		beego.Error(err)
		c.Ctx.WriteString("OK")
		return
	}

	beego.Debug(gift)

	models.InsertGift(gift)

	c.Ctx.WriteString("OK")

}
