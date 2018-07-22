package controllers

import (
	"encoding/json"
	"strconv"

	"time"

	"github.com/astaxie/beego"
)

type COM_GiftItem struct {
	ItemId    uint32
	ItemStack uint32
}
type ADGiftBagCmd struct {
	Rewards   []COM_GiftItem
	Price     uint32
	OldPrice  uint32
	OpenTime  uint64
	CloseTime uint64
	Type      string
}

type DoInsertMoney struct {
	beego.Controller
}

func (c *DoInsertMoney) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("没有选择服务器", 302)
		return
	}
	cmd := ADGiftBagCmd{}
	cmd.Type = "GMT_MinGiftBag"

	var price string
	var oldprice string
	price = c.GetString("price")
	oldprice = c.GetString("oldprice")
	num, errmk := strconv.Atoi(price)
	nums, errnk := strconv.Atoi(oldprice)
	if errmk != nil {
		c.Ctx.WriteString(errmk.Error())
		return
	}
	if errnk != nil {
		c.Ctx.WriteString(errnk.Error())
		return
	}
	if num != 0 {
		cmd.Price = uint32(num)
	}
	if nums != 0 {
		cmd.OldPrice = uint32(nums)
	}

	opentimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.GetString("sinceStamp"), time.Local)
	closetimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.GetString("endStamp"), time.Local)
	cmd.OpenTime = uint64(opentimestr.Unix())
	cmd.CloseTime = uint64(closetimestr.Unix())
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
			if cmd.Rewards == nil {
				cmd.Rewards = []COM_GiftItem{}
			}
			mi := COM_GiftItem{}
			mi.ItemId = uint32(itemid)
			mi.ItemStack = uint32(itemsk)
			cmd.Rewards = append(cmd.Rewards, mi)
		}
		stritemids += stritemid + ","
		stritemsks += stritemsk + ","

	}
	if opentimestr.Unix() > closetimestr.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	now := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	//MoneyRechargeInsert(c.Input().Get("sinceStamp"), c.Input().Get("endStamp"), time)
	LoginActivityInsert("小额礼包", c.Input().Get("sinceStamp"), c.Input().Get("endStamp"), now)
	c.Redirect("/alert?param="+string(jresults), 302)
}
