package controllers

import (
	"encoding/json"
	"strconv"
	_ "strings"
	"time"

	"github.com/astaxie/beego"
)

type TurntableContent struct {
	ZhuanpanID  uint32
	Probability uint32
	DailyOutput uint32
	ItemId      uint32
	ItemStack   uint32
}
type DoTurntableCmd struct {
	Type      string
	OpenTime  uint64
	CloseTime uint64
	Contents  []TurntableContent
}

type DoTurntable struct {
	beego.Controller
}

func (c *DoTurntable) Post() {
	c.Ctx.Request.ParseForm()
	servs := ToServIds(c.GetStrings("servs"))
	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	cmd := DoTurntableCmd{}
	cmd.Type = "GMT_Zhuanpan"

	stropentime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("low_time1"), time.Local)
	cmd.OpenTime = uint64(stropentime.Unix())

	strclosetime, _ := time.ParseInLocation("2006-1-02 15:04", c.Input().Get("close_time1"), time.Local)
	cmd.CloseTime = uint64(strclosetime.Unix())

	cmd.Contents = []TurntableContent{}

	for i := 0; i < 12; i++ {

		rwd := TurntableContent{}

		LimitNum, errNum := strconv.Atoi(c.GetString("id" + strconv.FormatInt(int64(i), 10)))
		if errNum != nil {
			c.Ctx.WriteString(errNum.Error())
			return
		}
		rwd.ZhuanpanID = uint32(LimitNum)

		ItemId, errId := strconv.Atoi(c.GetString("itemid" + strconv.FormatInt(int64(i), 10)))
		if errId != nil {
			c.Ctx.WriteString(errId.Error())
			return
		}
		rwd.ItemId = uint32(ItemId)

		ItemStack, errStack := strconv.Atoi(c.GetString("itemstack" + strconv.FormatInt(int64(i), 10)))
		if errStack != nil {
			c.Ctx.WriteString(errStack.Error())
			return
		}
		rwd.ItemStack = uint32(ItemStack)
		Probability, errPro := strconv.Atoi(c.GetString("probability" + strconv.FormatInt(int64(i), 10)))
		if errPro != nil {
			c.Ctx.WriteString(errPro.Error())
			return
		}
		rwd.Probability = uint32(Probability)
		DailyOutput, errDaily := strconv.Atoi(c.GetString("dailyoutput" + strconv.FormatInt(int64(i), 10)))
		if errDaily != nil {
			c.Ctx.WriteString(errDaily.Error())
			return
		}
		rwd.DailyOutput = uint32(DailyOutput)

		cmd.Contents = append(cmd.Contents, rwd)
	}

	if stropentime.Unix() > strclosetime.Unix() {
		c.Redirect("/error?param=活动关闭时间不能小于开始时间", 302)
		return
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)

	//TurntableInsert(c.Input().Get("low_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	LoginActivityInsert("抽奖活动", c.Input().Get("low_time1"), c.Input().Get("close_time1"), time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05"))
	c.Redirect("/alert?param="+string(jresults), 302)

}
