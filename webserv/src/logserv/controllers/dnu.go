package controllers

import (
	"encoding/json"
	"logserv/models"
	"time"

	"github.com/astaxie/beego"
)

type (
	DNUController struct {
		beego.Controller
	}
	DNUController2 struct {
		beego.Controller
	}
	DNUController3 struct {
		beego.Controller
	}
)

func (this *DNUController) Get() {
	b, _ := time.Parse("2006-01-02", this.GetString("b", time.Now().Add(24*time.Hour).Format("2006-01-02")))
	a, _ := time.Parse("2006-01-02", this.GetString("a", time.Now().Format("2006-01-02")))

	beego.Debug(a, b)

	r := models.CalcDNU(a, b)
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}

func (this *DNUController2) Get() {
	b, _ := time.Parse("2006-01-02", this.GetString("b", time.Now().Add(24*time.Hour).Format("2006-01-02")))
	a, _ := time.Parse("2006-01-02", this.GetString("a", time.Now().Format("2006-01-02")))

	r := models.CalcDNU2(a, b)
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}

func (this *DNUController3) Get() {
	b, _ := time.Parse("2006-01-02", this.GetString("b", time.Now().Add(24*time.Hour).Format("2006-01-02")))
	a, _ := time.Parse("2006-01-02", this.GetString("a", time.Now().Format("2006-01-02")))

	r := models.CalcCDNU(a, b)
	if r == nil {
		this.Ctx.WriteString("[]")
	}
	j, _ := json.Marshal(r)

	this.Ctx.WriteString(string(j))
}
