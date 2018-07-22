package controllers

import (
	_ "encoding/csv"
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"gmtools/models"
	_ "os"
	_ "strings"
)

type AccountLogItems struct {
	Date   string
	ServId int
	Num    int
	Name   string
}
type AccountLogServcmd struct {
	Content []AccountLogItems
}
type AccountLogServ struct {
	beego.Controller
}
type AccountLogServs struct {
	beego.Controller
}

func (this *AccountLogServ) Post() {
	req := httplib.Get(beego.AppConfig.String("logservhost") + "account/dau")
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := AccountLogServcmd{}
	cmd.Content = []AccountLogItems{}
	jerr := json.Unmarshal([]byte(str), &cmd.Content)

	if jerr != nil {
		beego.Debug(jerr.Error())
	}
	for i := 0; i < len(cmd.Content); i++ {
		cmd.Content[i].Name = models.GetGMTServName(cmd.Content[i].ServId)
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	this.Ctx.WriteString(string(jparam))
}
func (this *AccountLogServs) Post() {
	this.Ctx.Request.ParseForm()
	opentime := this.GetString("open")
	closetime := this.GetString("close")
	url := beego.AppConfig.String("logservhost") + "account/dau?a=" + opentime + "&b=" + closetime
	req := httplib.Get(url)
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}

	cmd := AccountLogServcmd{}
	cmd.Content = []AccountLogItems{}
	jerr := json.Unmarshal([]byte(str), &cmd.Content)

	if jerr != nil {
		beego.Debug(jerr.Error())
	}
	for i := 0; i < len(cmd.Content); i++ {
		cmd.Content[i].Name = models.GetGMTServName(cmd.Content[i].ServId)
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	this.Ctx.WriteString(string(jparam))
}
