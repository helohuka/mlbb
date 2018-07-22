package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"gmtools/models"
	_ "gmtools/models"
	_ "strconv"
)

type DataContent struct {
	Date   string
	ServId int
	Num    int
	Name   string
}
type DateAccountLogcmd struct {
	Content []DataContent
}
type DateAccountLog struct {
	beego.Controller
}
type DateAccountLogServs struct {
	beego.Controller
}

func (this *DateAccountLog) Post() {
	req := httplib.Get(beego.AppConfig.String("logservhost") + "account/dnu")
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := DateAccountLogcmd{}
	cmd.Content = []DataContent{}
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
func (this *DateAccountLogServs) Post() {
	this.Ctx.Request.ParseForm()
	opentime := this.GetString("open")
	closetime := this.GetString("close")
	url := beego.AppConfig.String("logservhost") + "account/dnu?a=" + opentime + "&b=" + closetime
	req := httplib.Get(url)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := DateAccountLogcmd{}
	cmd.Content = []DataContent{}
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
