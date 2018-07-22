package controllers

import (
	"encoding/json"
	"strconv"
	//	"strings"
	"time"

	"github.com/astaxie/beego"
)

type ExtractReward struct {
	ItemId    uint32
	ItemStack uint32
}
type ExtractContent struct {
	LimitNum int32
	Rewards  []ExtractReward
}
type ExtractPartnerCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Content   []ExtractContent
}

type DoExtractPartner struct {
	beego.Controller
}

func (c *DoExtractPartner) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := ExtractPartnerCmd{}
	cmd.Type = "GMT_ExtractEmployee"

	opentimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("opentime1"), time.Local)
	cmd.OpenTime = uint64(opentimestr.Unix())

	closetimestr, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("closetime1"), time.Local)
	cmd.CloseTime = uint64(closetimestr.Unix())

	cmd.Content = []ExtractContent{}

	var strlimitNum string
	for i := 0; i < 10; i++ {
		content := ExtractContent{}

		strlimitNum = c.GetString("ckb" + strconv.FormatInt(int64(i), 10))

		limitnum, errnum := strconv.Atoi(strlimitNum)

		if errnum != nil {
			c.Ctx.WriteString(errnum.Error())
			return
		}
		content.LimitNum = int32(limitnum)

		strlimitNum += strlimitNum + ","

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
			if itemid != 0 && itemsk != 0 {
				if content.Rewards == nil {
					content.Rewards = []ExtractReward{}
				}
				rwd := ExtractReward{}

				rwd.ItemId = uint32(itemid)
				rwd.ItemStack = uint32(itemsk)

				content.Rewards = append(content.Rewards, rwd)

				stritemId += stritemId + ","
				stritemStack += stritemStack + ","
			}
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
	now := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")
	//ExtractPartnerInsert(c.Input().Get("opentime1"), c.Input().Get("closetime1"), time)
	LoginActivityInsert("顶级招募", c.Input().Get("opentime1"), c.Input().Get("closetime1"), now)
	c.Redirect("/alert?param="+string(jresults), 302)
}
