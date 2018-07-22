package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"gmtools/models"
	_ "gmtools/models"
	_ "strconv"
)

type DataItems struct {
	Date   string
	ServId int
	Num    int
	Name   string
}
type DateQueryLogcmd struct {
	Content []DataItems
}
type DateQueryLog struct {
	beego.Controller
}
type DateQueryLogServs struct {
	beego.Controller
}

func (this *DateQueryLog) Post() {
	req := httplib.Get(beego.AppConfig.String("logservhost") + "player/dnu")
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := DateQueryLogcmd{}
	cmd.Content = []DataItems{}
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
func (this *DateQueryLogServs) Post() {
	this.Ctx.Request.ParseForm()
	opentime := this.GetString("open")
	closetime := this.GetString("close")
	url := beego.AppConfig.String("logservhost") + "player/dnu?a=" + opentime + "&b=" + closetime
	req := httplib.Get(url)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := DateQueryLogcmd{}
	cmd.Content = []DataItems{}
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
