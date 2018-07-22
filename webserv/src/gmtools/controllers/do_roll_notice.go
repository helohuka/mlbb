package controllers

import (
	"encoding/json"
	"strconv"
	_ "strings"
	"time"

	"github.com/astaxie/beego"
)

type NoticeCmd struct {
	Type     string `json:"Type"`
	SendType string `json:"NoticeSendType"`
	Content  string `json:"Content"`
	TheTime  int64  `json:"TheTime"`
	ItvTime  int64  `json:"ItvTime"`
}

type DoRollNotice struct {
	beego.Controller
}

func (c *DoRollNotice) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	var cmd NoticeCmd
	cmd.Type = "GMT_Notice"
	cmd.SendType = c.GetString("types")
	cmd.Content = c.GetString("content")
	if len(cmd.Content) == 0 {
		c.Redirect("/error?param=没有公告内容", 302)
		return
	}
	time0 := c.GetString("param_name0")

	var sendType = cmd.SendType
	var content = cmd.Content
	var timestr = time0

	if cmd.SendType != "NST_Immediately" {
		if cmd.SendType == "NST_Timming" {
			opentime, _ := time.ParseInLocation("2006-1-02 15:04", time0, time.Local)
			//beego.Debug(time0, opentime)
			if opentime.Unix() < time.Now().Unix() {
				c.Redirect("/error?param=定时发送时间已经过去(请检查时间格式 2006-01-02 15:04:05", 302)
				return
			}
			cmd.TheTime = opentime.Unix()
		} else {

			closetime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("param_name4"), time.Local)

			timestr := c.Input().Get("param_name3")
			itvtime, _ := strconv.Atoi(timestr)

			if closetime.Unix() < time.Now().Unix() {
				c.Ctx.WriteString("/failed?param=循环发送结束时间已经过去(请检查时间格式 2006-01-02 15:04:05,30")
				return
			}
			if itvtime <= 0 {
				c.Ctx.WriteString("/error?param=循环发送间隔时间错误(请检查时间格式 2006-01-02 15:04:05,30")
				return
			}
			cmd.TheTime = closetime.Unix()
			cmd.ItvTime = int64(itvtime)
		}
	}
	time := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	jparam, _ := json.Marshal(cmd) //把数据转成json数据
	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)

	RollNoticeInsert(sendType, content, timestr, time)
	c.Ctx.WriteString(string(jresults))
}
