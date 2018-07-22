package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"strconv"
	"time"
)

type SimulatorCmd struct {
	Type     string
	PlayerId int
	ShopId   int
	Payment  int
	OrderId  string
}

type DoSimulator struct {
	beego.Controller
}

func (c *DoSimulator) Post() {
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	strplayerid := c.Input().Get("PlayerId")
	PlayerId, errid := strconv.Atoi(strplayerid)
	if errid != nil {
		c.Ctx.WriteString(errid.Error())
		return
	}
	strshopid := c.Input().Get("ShopId")
	ShopId, errshop := strconv.Atoi(strshopid)
	if errshop != nil {
		c.Ctx.WriteString(errshop.Error())
		return
	}
	strpayment := c.Input().Get("Payment")
	Payment, errpayment := strconv.Atoi(strpayment)
	if errpayment != nil {
		c.Ctx.WriteString(errpayment.Error())
		return
	}
	orderid := c.Input().Get("Orderid")
	cmd := SimulatorCmd{}
	cmd.Type = "GNT_MakeOrder"
	cmd.PlayerId = PlayerId
	cmd.ShopId = ShopId
	cmd.Payment = Payment
	cmd.OrderId = orderid

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)

	RecordInsert(PlayerId, ShopId, Payment, orderid, time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	c.Ctx.WriteString(string(jresults))
}
