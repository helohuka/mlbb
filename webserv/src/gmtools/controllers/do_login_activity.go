package controllers

import (
	"encoding/json"
	"strconv"
	_ "strings"
	"time"

	"github.com/astaxie/beego"
)

type ActivityReward struct {
	ItemId    uint32
	ItemStack uint32
}
type ActivityContent struct {
	LimitNum uint32
	Rewards  []ActivityReward
}
type LoginActivityCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Content   []ActivityContent
}

type DoLoginActivity struct {
	beego.Controller
}

func (c *DoLoginActivity) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))
	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := LoginActivityCmd{}
	cmd.Type = "GMT_LoginActivity"

	opentimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("low_time1"), time.Local)
	cmd.OpenTime = uint64(opentimestr.Unix())

	closetimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("close_time1"), time.Local)
	cmd.CloseTime = uint64(closetimestr.Unix())

	cmd.Content = []ActivityContent{}

	var strLimit string
	for i := 0; i < 7; i++ {
		content := ActivityContent{}
		strLimit = c.GetString("ckb" + strconv.FormatInt(int64(i), 10))
		itemLimit, errLimit := strconv.Atoi(strLimit)
		if errLimit != nil {
			c.Ctx.WriteString(errLimit.Error())
			return
		}
		content.LimitNum = uint32(itemLimit)
		strLimit += strLimit + ","
		var stritemId string
		var stritemStack string
		for j := 1; j < 4; j++ {

			stritemId = c.GetString("ckab" + strconv.FormatInt(int64(i*3+j), 10))
			stritemStack = c.GetString("ckeb" + strconv.FormatInt(int64(i*3+j), 10))

			itemid, errid := strconv.Atoi(stritemId)
			itemsk, errsk := strconv.Atoi(stritemStack)

			if errid != nil {
				c.Ctx.WriteString(errid.Error())
				return
			}
			if errsk != nil {
				c.Ctx.WriteString(errsk.Error())
				return
			}
			if itemLimit != 0 && itemid != 0 && itemsk != 0 {
				if content.Rewards == nil {
					content.Rewards = []ActivityReward{}
				}
				rwd := ActivityReward{}

				rwd.ItemId = uint32(itemid)
				rwd.ItemStack = uint32(itemsk)

				content.Rewards = append(content.Rewards, rwd)
			}

			stritemId += stritemId + ","
			stritemStack += stritemStack + ","
		}
		cmd.Content = append(cmd.Content, content)
	}

	if opentimestr.Unix() > closetimestr.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)

	LoginActivityInsert("登录活动", c.Input().Get("low_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	c.Redirect("/alert?param="+string(jresults), 302)

}
