package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"gmtools/models"
)

type PlayerRrsItems struct {
	PFID, Name, AccountName, PFName, RoleId string
	ServerId, Payment, RoleLevel            int
}
type PlayerRrscmd struct {
	Content []PlayerRrsItems
}
type PlayerRrs struct {
	beego.Controller
}

func (this *PlayerRrs) Post() {

	//beego.Debug(beego.AppConfig.String("logservhost") + ":10998/player/rrs")
	req := httplib.Get(beego.AppConfig.String("logservhost") + "player/rrs")
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	cmd := PlayerRrscmd{}
	cmd.Content = []PlayerRrsItems{}
	jerr := json.Unmarshal([]byte(str), &cmd.Content)

	if jerr != nil {
		beego.Debug(jerr.Error())
	}
	for i := 0; i < len(cmd.Content); i++ {
		cmd.Content[i].Name = models.GetGMTServName(cmd.Content[i].ServerId)
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	this.Ctx.WriteString(string(jparam))
}
