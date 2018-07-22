package controllers

import (
	"encoding/json"
	"strconv"

	"time"

	"github.com/astaxie/beego"
)

type MailItem struct {
	ItemId    int `json:"ItemId"`
	ItemStack int `json:"ItemStack"`
}

type MailCmd struct {
	Type           string     `json:"Type"`
	SendType       string     `json:"InsertMailType"`
	Sender         string     `json:"Sender"`
	Title          string     `json:"Title"`
	Content        string     `json:"Content"`
	Timmer         int64      `json:"Timmer"`
	LowLevel       int        `json:"LowLevel"`
	HighLevel      int        `json:"HighLevel"`
	LowRegistTime  int64      `json:"LowRegistTime"`
	HighRegistTime int64      `json:"HighRegistTime"`
	Recvers        []int64    `json:"Recvers"`
	Items          []MailItem `json:"Items"`
}

type DoInsertMail struct {
	beego.Controller
}

func (c *DoInsertMail) Get() {
	c.Ctx.WriteString("<html><h1>Donot care get</h1></html>")
}

func (c *DoInsertMail) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))
	if len(servs) == 0 {
		c.Redirect("没有选择服务器", 302)
		return
	}
	var cmd MailCmd
	cmd.Type = "GMT_InsertMail"
	cmd.SendType = c.GetString("types")
	cmd.Sender = c.GetString("sender")
	cmd.Title = c.GetString("title")
	cmd.Content = c.GetString("content")

	cmd.LowLevel, _ = strconv.Atoi(c.GetString("low_level"))
	cmd.HighLevel, _ = strconv.Atoi(c.GetString("high_level"))

	morningtime, _ := time.ParseInLocation("2006-1-02 15:04", c.GetString("low_time1"), time.Local)
	nighttime, _ := time.ParseInLocation("2006-1-02 15:04", c.GetString("high_time1"), time.Local)

	cmd.LowRegistTime = morningtime.Unix()
	cmd.HighRegistTime = nighttime.Unix()

	var LowLevel = cmd.LowLevel
	var HighLevel = cmd.HighLevel
	var SendType = cmd.SendType
	var Title = cmd.Title
	var Sender = cmd.Sender
	var Content = cmd.Content

	if (cmd.LowLevel != 0) && (cmd.HighLevel != 0) && (cmd.LowLevel > cmd.HighLevel) {
		c.Ctx.WriteString("最小等级大于最大等级")
		return
	}

	if (cmd.LowRegistTime != 0) && (cmd.HighRegistTime != 0) && (cmd.LowRegistTime > cmd.HighRegistTime) {
		c.Ctx.WriteString("最小时间大于最大时间")
		return
	}

	if cmd.Sender == "" {
		c.Ctx.WriteString("/error?param=请填写发件人")
		return
	}

	if cmd.Title == "" {
		c.Ctx.WriteString("请填写标题")
		return
	}

	if cmd.Content == "" {
		c.Ctx.WriteString("请填写内容")
		return
	}

	recers := "[" + c.GetString("recvers") + "]"
	if cmd.SendType == "IGMT_PlayerId" {
		if len(recers) != 0 {
			jierr := json.Unmarshal([]byte(recers), &cmd.Recvers)
			if jierr != nil {
				c.Ctx.WriteString("收件人格式错误")
				return
			}
		}
		if cmd.SendType == "IGMT_AllOnline" || cmd.SendType == "IGMT_AllRegist" {
			recers = " "
			jierr := json.Unmarshal([]byte(recers), &cmd.Recvers)
			if jierr != nil {
				c.Ctx.WriteString("收件人格式错误")
				return
			}
		}
	}

	var stritemids string
	var stritemsks string

	for i := 1; i <= 5; i++ {
		stritemid := c.GetString("item_" + strconv.FormatInt(int64(i), 10)) //GetItemIdByName(stritemname)
		stritemsk := c.GetString("stack_" + strconv.FormatInt(int64(i), 10))

		if len(stritemid) == 0 || len(stritemsk) == 0 {
			continue
		}
		itemid, errid := strconv.Atoi(stritemid)
		itemsk, errsk := strconv.Atoi(stritemsk)
		if errid != nil {
			c.Ctx.WriteString(errid.Error())
			return
		}
		if errsk != nil {
			c.Ctx.WriteString(errsk.Error())
			return
		}
		if itemid != 0 && itemsk != 0 {
			if cmd.Items == nil {
				cmd.Items = []MailItem{}
			}
			mi := MailItem{}
			mi.ItemId = itemid
			mi.ItemStack = itemsk
			cmd.Items = append(cmd.Items, mi)
		}
		stritemids += stritemid + ","
		stritemsks += stritemsk + ","

	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	time := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")

	results := PostGMTServs(servs, jparam)

	jresults, _ := json.Marshal(results)

	InsertMailInsert(Title, Sender, Content, recers, stritemids, stritemsks, LowLevel, HighLevel, c.GetString("low_time1"), c.GetString("high_time1"), SendType, time)

	c.Ctx.WriteString(string(jresults))
}
