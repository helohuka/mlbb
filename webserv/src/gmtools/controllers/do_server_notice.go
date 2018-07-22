package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"time"
)

type DoServerNotice struct {
	beego.Controller
}

func (c *DoServerNotice) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}
	content := c.GetString("content")
	title := c.GetString("title")
	color := c.GetString("color")[1:]
	now := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	SystemNoticeInsert("sever_notice", title, content, color, now)
	if len(color) == 0 {
		color = "00FF00"
	}

	if len(content) != 0 {
		content = "[" + color + "]" + content + "[-]"
		content = title + ";" + content
	}
	strservs, _ := json.Marshal(servs)

	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/notice/server")
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Param("servs", (string)(strservs))
	req.Param("content", content)

	str, err := req.String()

	//ServiceNoticeInsert(title, content, color, time)

	if err == nil {
		beego.Info(content)
		c.Redirect("/alert?param="+str, 302)

	} else {
		c.Redirect("/error?param="+err.Error(), 302)

	}

}
