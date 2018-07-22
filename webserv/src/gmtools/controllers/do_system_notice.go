package controllers

import (
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"time"
)

type DoSystemNotice struct {
	beego.Controller
}

func (c *DoSystemNotice) Post() {

	content := c.GetString("content")
	title := c.GetString("title")
	color := c.GetString("color")[1:]
	channel := c.GetString("sev_Channels")

	now := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	SystemNoticeInsert("system_notice", title, content, color, now)

	if len(color) == 0 {
		color = "00FF00"
	}

	if len(content) != 0 {
		content = "[" + color + "]" + content + "[-]"
		content = title + ";" + content
		channel = channel
	} else {
		content = ""
	}

	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/notice/system")
	req.Header("content-type", "application/x-www-form-urlencoded")

	req.Param("content", content)
	req.Param("channel", channel)
	str, err := req.String()
	if err == nil {
		beego.Info(content)
		beego.Info(channel)
		c.Ctx.WriteString(str)

	} else {
		c.Ctx.WriteString(err.Error())
	}
}
