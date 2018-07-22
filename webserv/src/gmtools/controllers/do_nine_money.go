package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"strconv"
	_ "time"
)

type Item struct {
	ItemId    uint32
	ItemStack uint32
}
type DoNineMoneyCmd struct {
	KaId        string
	ReqType     uint32
	ReqValue    uint32
	MailSender  string
	MailTitle   string
	MailContent string
	Items       []Item
}

type DoNineMoney struct {
	beego.Controller
}

func (c *DoNineMoney) Post() {
	c.Ctx.Request.ParseForm()
	Sender := c.GetString("sender")
	Title := c.GetString("title")
	num, nerr := strconv.Atoi(c.GetString("num"))
	if nerr != nil {
		c.Ctx.WriteString(nerr.Error())
		return
	}
	kaid := c.GetString("kaid")
	limit, tperr := strconv.Atoi(c.GetString("limit"))
	if tperr != nil {
		c.Ctx.WriteString(tperr.Error())
		return
	}
	content := c.GetString("content")

	cmd := DoNineMoneyCmd{}
	//cmd.Type = "GMT_MinGiftBag"
	cmd.KaId = kaid
	cmd.MailSender = Sender
	cmd.MailTitle = Title
	cmd.MailContent = content
	cmd.ReqType = uint32(limit)
	cmd.ReqValue = uint32(num)
	var stritemids string
	var stritemsks string
	for i := 1; i < 5; i++ {
		stritemid := c.GetString("itemId_" + strconv.FormatInt(int64(i), 10)) //GetItemIdByName(stritemname)
		stritemsk := c.GetString("itemNum_" + strconv.FormatInt(int64(i), 10))

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
				cmd.Items = []Item{}
			}
			mi := Item{}
			mi.ItemId = uint32(itemid)
			mi.ItemStack = uint32(itemsk)
			cmd.Items = append(cmd.Items, mi)
		}
		stritemids += stritemid + ","
		stritemsks += stritemsk + ","

	}

	url := beego.AppConfig.String("jiuyouservhost") + "insert"

	req := httplib.Post(url)

	req.JSONBody(cmd)

	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		c.Ctx.WriteString("")
		return
	}

	c.Redirect("/alert?param="+string(str), 302)
}
