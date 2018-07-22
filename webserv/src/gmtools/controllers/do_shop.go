package controllers

import (
	"encoding/json"
	"strconv"
	_ "strings"
	"time"

	"github.com/astaxie/beego"
)

type IntegralContent struct {
	ID     uint32
	Times  uint32
	ItemId uint32
	Cost   uint32
}
type DoIntegralCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Contents  []IntegralContent
}

type DoIntegral struct {
	beego.Controller
}

func (c *DoIntegral) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))
	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := DoIntegralCmd{}
	cmd.Type = "GMT_IntegralShop"

	opentime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("low_time1"), time.Local)
	cmd.OpenTime = uint64(opentime.Unix())

	closetime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("close_time1"), time.Local)
	cmd.CloseTime = uint64(closetime.Unix())

	cmd.Contents = []IntegralContent{}

	for i := 0; i < 6; i++ {

		rwd := IntegralContent{}

		LimitNum, errNum := strconv.Atoi(c.GetString("id" + strconv.FormatInt(int64(i), 10)))
		if errNum != nil {
			c.Ctx.WriteString(errNum.Error())
			return
		}
		rwd.ID = uint32(LimitNum)

		ItemId, errId := strconv.Atoi(c.GetString("itemid" + strconv.FormatInt(int64(i), 10)))
		if errId != nil {
			c.Ctx.WriteString(errId.Error())
			return
		}
		rwd.ItemId = uint32(ItemId)

		times, errStack := strconv.Atoi(c.GetString("times" + strconv.FormatInt(int64(i), 10)))
		if errStack != nil {
			c.Ctx.WriteString(errStack.Error())
			return
		}
		rwd.Times = uint32(times)
		cost, errPro := strconv.Atoi(c.GetString("cost" + strconv.FormatInt(int64(i), 10)))
		if errPro != nil {
			c.Ctx.WriteString(errPro.Error())
			return
		}
		rwd.Cost = uint32(cost)

		cmd.Contents = append(cmd.Contents, rwd)
	}

	if opentime.Unix() > closetime.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)

	//ShopInsert(c.Input().Get("low_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	LoginActivityInsert("积分商店", c.Input().Get("low_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	c.Redirect("/alert?param="+string(jresults), 302)

}
