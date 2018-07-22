package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"gmtools/models"
	"strconv"
	"time"
)

type SayRecord struct {
	SortId     int
	Timestamp  string
	PlayerGuid int
	PlayerName string
	ChannelId  int
	Content    string
}

var SayRecords []SayRecord

type DoQueryHistoryChat struct {
	beego.Controller
}

func (c *DoQueryHistoryChat) Post() {
	SayRecords = []SayRecord{}
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	db, err := models.GetServLogMysql(serv)

	if err != nil {
		c.Redirect("/error?param="+err.Error(), 302) //跳转
		return
	}
	defer db.Close()

	opentime, _ := time.Parse("2006-1-02 15:04", c.Input().Get("chat_low_time1")) //指定的换成有效的时间
	closetime, _ := time.Parse("2006-1-02 15:04", c.Input().Get("chat_high_time1"))

	if opentime.Unix() >= closetime.Unix() {
		c.Redirect("/error?param=起始时间大于结束时间", 302)
		return
	}

	records, err := db.Query("SELECT * FROM PlayerSay WHERE Tiemstamp > ? AND Tiemstamp < ?", opentime, closetime)

	if err != nil {
		c.Redirect("/error?param="+err.Error(), 302)
		return
	}

	for records.Next() {
		sr := SayRecord{}
		err := records.Scan(&sr.SortId, &sr.PlayerGuid, &sr.PlayerName, &sr.ChannelId, &sr.Content, &sr.Timestamp)
		if err != nil {
			c.Redirect("/error?param="+err.Error(), 302)
			return
		}

		SayRecords = append(SayRecords, sr)
	}

	jparam, _ := json.Marshal(SayRecords)

	c.Ctx.WriteString(string(jparam))

}

type DoQueryNowChat struct {
	beego.Controller
}

func (c *DoQueryNowChat) Post() {
	SayRecords = []SayRecord{}
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	db, err := models.GetServLogMysql(serv)

	if err != nil {
		c.Redirect("/error?param="+err.Error(), 302)
		return
	}
	defer db.Close()

	limit, err := strconv.Atoi(c.Input().Get("query_limit"))
	if err != nil {
		c.Redirect("/error?param="+err.Error(), 302)
		return
	}
	if limit > 100 {
		limit = 100
	}

	records, err := db.Query("SELECT * FROM PlayerSay ORDER BY SortId DESC LIMIT " + strconv.FormatInt(int64(limit), 10))
	if err != nil {
		c.Redirect("/error?param="+err.Error(), 302)
		return
	}

	for records.Next() {
		sr := SayRecord{}
		err := records.Scan(&sr.SortId, &sr.PlayerGuid, &sr.PlayerName, &sr.ChannelId, &sr.Content, &sr.Timestamp)
		if err != nil {
			c.Redirect("/error?param="+err.Error(), 302)
			return
		}

		SayRecords = append(SayRecords, sr)
	}

	jparam, _ := json.Marshal(SayRecords)

	c.Ctx.WriteString(string(jparam))

}
