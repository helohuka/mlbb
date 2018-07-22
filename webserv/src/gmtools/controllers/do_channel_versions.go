package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	_ "gmtools/models"
	_ "strconv"
	"strings"
)

type Add_Version_sChannel struct {
	beego.Controller
}
type Do_Version_sChannel struct {
	beego.Controller
}
type Do_Channel_Versions struct {
	beego.Controller
}

func (this *Do_Channel_Versions) Post() {
	//req := httplib.Get(beego.AppConfig.String("logservhost") + ":80/gmtools/channel")
	req := httplib.Get(beego.AppConfig.String("carriershost") + "gmtools/channel")
	//req := httplib.Get("http://10.10.10.188:18081/gmtools/channel")

	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	//beego.Debug(string(str))

	this.Ctx.WriteString(string(str))
}
func (this *Do_Version_sChannel) Post() {
	this.Ctx.Request.ParseForm()
	channel := this.GetString("channel")
	version := this.GetString("version")
	serverids := this.GetStrings("serverids")
	del := this.GetString("delete")
	//beego.Debug(channel,version,serverids)
	//beego.Debug(del)
	//req := httplib.Post(beego.AppConfig.String("logservhost") + ":80/gmtools/channel")
	//req := httplib.Post("http://10.10.10.188:18081/gmtools/channel")
	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/channel")
	req.Param("channel", channel)
	req.Param("version", version)
	req.Param("delete", del)
	s := strings.Join(serverids, ",")
	beego.Debug(s)
	req.Param("serverids", s)

	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	//beego.Debug(string(str))
	this.Ctx.WriteString(string(str))
}
func (this *Add_Version_sChannel) Post() {
	this.Ctx.Request.ParseForm()
	channel := this.GetString("channel")
	version := this.GetString("version")

	//beego.Debug(channel,version)
	//req := httplib.Post(beego.AppConfig.String("logservhost") + ":80/gmtools/channel")
	//req := httplib.Post("http://10.10.10.188:18081/gmtools/channel")
	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/channel")
	req.Param("channel", channel)
	req.Param("version", version)
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	//beego.Debug(string(str))
	this.Ctx.WriteString(string(str))
}
