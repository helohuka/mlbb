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

type QueryLogItems struct {
	Date   string
	ServId int
	Num    int
	Name   string
}
type QueryLogServcmd struct {
	Content []QueryLogItems
}
type QueryLogServ struct {
	beego.Controller
}
type QueryLogServs struct {
	beego.Controller
}

func (this *QueryLogServ) Post() {
	req := httplib.Get(beego.AppConfig.String("logservhost") + "player/dau")
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := QueryLogServcmd{}
	cmd.Content = []QueryLogItems{}
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
func (this *QueryLogServs) Post() {
	this.Ctx.Request.ParseForm()
	opentime := this.GetString("open")
	closetime := this.GetString("close")
	url := beego.AppConfig.String("logservhost") + "player/dau?a=" + opentime + "&b=" + closetime
	req := httplib.Get(url)
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}

	cmd := QueryLogServcmd{}
	cmd.Content = []QueryLogItems{}
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
