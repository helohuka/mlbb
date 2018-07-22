package servs

import (
	"carriers/models"
	"encoding/json"
	"strconv"

	"github.com/astaxie/beego"
)

type (
	CDKeyReqInfo struct {
		PlayerName string
		CDKey      string
		GiftNames  []string
	}

	CDKeyResult struct {
		PlayerName string
		GiftName   string
		Diamond    int
		ErrorDesc  string
		Items      []models.COM_GiftItem
	}

	CDKey struct {
		beego.Controller
	}

	GenCDKEY struct {
		beego.Controller
	}

	RMCDKEY struct {
		beego.Controller
	}

	QueryCDKEY struct {
		beego.Controller
	}
)

func (c *CDKey) Post() {
	c.Ctx.Request.ParseForm()
	str := c.Input().Get("json")
	beego.Debug(str)
	info := CDKeyReqInfo{}
	err := json.Unmarshal([]byte(str), &info)
	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString("")
		return
	}

	items, diamond, name, errdesc := models.ReqCDKEY(info.CDKey, info.PlayerName, info.GiftNames)

	result := CDKeyResult{info.PlayerName, name, diamond, errdesc, items}

	b, err := json.Marshal(result)
	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString("")
		return
	}

	c.Ctx.WriteString(string(b))
}

func (g *GenCDKEY) Get() {
	g.Post()
}

func (g *GenCDKEY) Post() {
	tag := g.Input().Get("tag")
	cou, _ := strconv.ParseInt(g.Input().Get("count"), 10, 32)
	str := g.Input().Get("items")

	models.GenCDKey(tag, int(cou), str)

	g.Ctx.WriteString("OK")
}

func (g *RMCDKEY) Get() {
	g.Post()
}

func (g *RMCDKEY) Post() {
	tag := g.Input().Get("tag")

	go models.DelCDKEY(tag)

	g.Ctx.WriteString("OK")
}

func (g *QueryCDKEY) Get() {
	g.Post()
}

func (g *QueryCDKEY) Post() {
	tag := g.Input().Get("tag")
	str := models.QueryCDKEY(tag)
	g.Ctx.WriteString(str)
}
