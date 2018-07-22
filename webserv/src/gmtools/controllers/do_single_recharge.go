package controllers

import (
	"encoding/json"
	"strconv"
	//	"strings"
	"time"

	"github.com/astaxie/beego"
)

type SingleReward struct {
	ItemId    uint32
	ItemStack uint32
	ItemMore  uint32
}
type SingleContent struct {
	LimitNum uint32
	Rewards  []SingleReward
}
type SingleRechargeCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Content   []SingleContent
}

type DoSingleRecharge struct {
	beego.Controller
}

func (c *DoSingleRecharge) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := SingleRechargeCmd{}
	cmd.Type = "GMT_ChargeEvery"
	stropentime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("open_time1"), time.Local)
	cmd.OpenTime = uint64(stropentime.Unix())

	strclosetime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("close_time1"), time.Local)
	cmd.CloseTime = uint64(strclosetime.Unix())

	cmd.Content = []SingleContent{}
	var strlimitNum string
	for i := 0; i < 6; i++ {
		content := SingleContent{}
		strlimitNum = c.GetString("ckb" + strconv.FormatInt(int64(i), 10))
		limitnum, errnum := strconv.Atoi(strlimitNum)
		if errnum != nil {
			c.Ctx.WriteString(errnum.Error())
			return
		}
		content.LimitNum = uint32(limitnum)
		strlimitNum += strlimitNum + ","

		var stritemId string
		var stritemStack string
		var stritemMore string
		for j := 1; j < 4; j++ {

			stritemId = c.GetString("ckab" + strconv.FormatInt(int64(i*3+j), 10))
			stritemStack = c.GetString("ckeb" + strconv.FormatInt(int64(i*3+j), 10))
			stritemMore = c.GetString("ckcb")

			itemid, errid := strconv.Atoi(stritemId)
			itemsk, errsk := strconv.Atoi(stritemStack)
			itemMore, errmk := strconv.Atoi(stritemMore)

			if errid != nil {
				c.Ctx.WriteString(errid.Error())
				return
			}
			if errsk != nil {
				c.Ctx.WriteString(errsk.Error())
				return
			}
			if errmk != nil {
				c.Ctx.WriteString(errmk.Error())
				return
			}
			if limitnum != 0 && itemid != 0 && itemsk != 0 && itemMore != 0 {
				if content.Rewards == nil {
					content.Rewards = []SingleReward{}
				}
				rwd := SingleReward{}

				rwd.ItemId = uint32(itemid)
				rwd.ItemStack = uint32(itemsk)
				rwd.ItemMore = uint32(itemMore)

				content.Rewards = append(content.Rewards, rwd)
			}

			stritemId += stritemId + ","
			stritemStack += stritemStack + ","
			stritemMore += stritemMore

		}
		cmd.Content = append(cmd.Content, content)
	}
	if stropentime.Unix() > strclosetime.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	now := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	//SingleRechargeInsert(c.Input().Get("open_time1"), c.Input().Get("close_time1"), now)
	LoginActivityInsert("单笔充值", c.Input().Get("open_time1"), c.Input().Get("close_time1"), now)
	c.Redirect("/alert?param="+string(jresults), 302)

}
