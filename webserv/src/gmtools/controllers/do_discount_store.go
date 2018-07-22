package controllers

import (
	"encoding/json"
	"strconv"
	//"strings"
	"github.com/astaxie/beego"
	"time"
)

type DiscountReward struct {
	ItemId uint32

	Discount float64
	Price    uint32
	LimitNum uint32
}
type DiscountStoreCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Rewards   []DiscountReward
}

type DoDiscountStore struct {
	beego.Controller
}

func (c *DoDiscountStore) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := DiscountStoreCmd{}

	cmd.Type = "GMT_DiscountStore"

	opentime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("open_time1"), time.Local)

	cmd.OpenTime = uint64(opentime.Unix())

	closetime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("close_time1"), time.Local)

	cmd.CloseTime = uint64(closetime.Unix())

	cmd.Rewards = []DiscountReward{}

	for i := 0; i < 11; i++ {

		rwd := DiscountReward{}
		strnum := c.GetString("ckcb" + strconv.FormatInt(int64(i), 10))

		LimitNum, errNum := strconv.Atoi(strnum)
		if errNum != nil {
			c.Ctx.WriteString(errNum.Error())
			return
		}
		rwd.LimitNum = uint32(LimitNum)
		stritemid := c.GetString("ckb" + strconv.FormatInt(int64(i), 10))
		ItemId, errId := strconv.Atoi(stritemid)
		if errId != nil {
			c.Ctx.WriteString(errId.Error())
			return
		}
		rwd.ItemId = uint32(ItemId)
		strprice := c.GetString("ckab" + strconv.FormatInt(int64(i), 10))
		Price, errPrice := strconv.Atoi(strprice)
		if errPrice != nil {
			c.Ctx.WriteString(errPrice.Error())
			return
		}
		rwd.Price = uint32(Price)
		strdiscount := c.GetString("ckeb" + strconv.FormatInt(int64(i), 10))
		Discount, _ := strconv.ParseFloat(strdiscount, 64)

		rwd.Discount = Discount

		cmd.Rewards = append(cmd.Rewards, rwd)
	}
	if opentime.Unix() > closetime.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}
	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	results := PostGMTServs(servs, jparam)

	jresults, _ := json.Marshal(results)
	//DiscountStoreInsert(c.Input().Get("open_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	LoginActivityInsert("打折商店", c.Input().Get("open_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	c.Redirect("/alert?param="+string(jresults), 302)
}
