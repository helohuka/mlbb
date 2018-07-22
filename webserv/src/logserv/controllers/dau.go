package controllers

import (
	"encoding/json"
	"logserv/models"
	"time"

	"github.com/astaxie/beego"
)

type (
	//角色
	DAUController struct {
		beego.Controller
	}
	//账户
	DAUController2 struct {
		beego.Controller
	}
)

func (this *DAUController) Get() {
	b, _ := time.Parse("2006-01-02", this.GetString("b", time.Now().Add(24*time.Hour).Format("2006-01-02")))
	a, _ := time.Parse("2006-01-02", this.GetString("a", time.Now().Format("2006-01-02")))

	r := models.CalcDAU(a, b)
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}

func (this *DAUController2) Get() {
	b, _ := time.Parse("2006-01-02", this.GetString("b", time.Now().Add(24*time.Hour).Format("2006-01-02")))
	a, _ := time.Parse("2006-01-02", this.GetString("a", time.Now().Format("2006-01-02")))

	r := models.CalcDAU2(a, b)
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}
