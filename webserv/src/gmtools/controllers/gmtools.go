package controllers

import (
	"encoding/json"

	"gmtools/models"
	"net/url"
	"strconv"
	"strings"
	_ "time"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

func MakeServId(name string, rephead string) int {
	if strings.Contains(name, rephead) {
		strservid := strings.Replace(name, rephead, "", -1)
		servid, _ := strconv.Atoi(strservid)
		return servid
	}
	return 0
}

func MakeServIds(vs url.Values, rephead string) []int {
	servs := []int{}
	for k, _ := range vs {
		servid := MakeServId(k, rephead)
		if servid != 0 {
			servs = append(servs, servid)
		}
	}
	return servs
}

func ToServIds(s []string) []int {

	result := []int{}
	for i := 0; i < len(s); i++ {
		servid, _ := strconv.Atoi(s[i])
		result = append(result, servid)
	}
	return result
}

func ToPrice(s []string) []int {

	result := []int{}
	for i := 0; i < len(s); i++ {
		price, _ := strconv.Atoi(s[i])
		result = append(result, price)
	}
	return result
}

type DoResult struct {
	ErrorNo   int    `json:"error"`
	ErrorDesc string `json:"desc"`
}

func PostGMTServ(serv int, jparam []byte) DoResult {
	//cotimeout, _ := time.ParseDuration("5s")
	//rwtimeout, _ := time.ParseDuration("10s")
	doresult := DoResult{0, ""}

	host := models.GetGMTServHost(serv)
	if host != "" {
		req := httplib.Post(host)
		req.Body(jparam)

		//req.SetTimeout(cotimeout, rwtimeout)
		str, err := req.String()

		if err != nil {
			doresult.ErrorNo = -1
			doresult.ErrorDesc = "逻辑服务器连接错误(" + host + ")"
		} else {

			jerr := json.Unmarshal([]byte(str), &doresult)
			if jerr != nil {
				beego.Debug(jerr.Error())
				doresult.ErrorNo = -1
				doresult.ErrorDesc = "逻辑服务器返回值错误(" + host + "){" + str + "}"
			}
		}

	} else {
		doresult.ErrorNo = -1
		doresult.ErrorDesc = "逻辑服务器地址错误(" + strconv.Itoa(serv) + ")"
	}

	return doresult
}

func PostGMTServs(servs []int, jparam []byte) []DoResult {

	doresults := []DoResult{}

	for i := 0; i < len(servs); i++ {
		doresult := PostGMTServ(servs[i], jparam)

		doresults = append(doresults, doresult)
	}
	return doresults

}

type SelectOption struct {
	Id         string
	IsSelected bool
	Value      string
}

type Home struct {
	beego.Controller
}

func (c *Home) Get() {
	c.TplName = "index.tpl"
}

var items []*SelectOption

func (c *Home) Post() {

	username := c.GetString("Username")
	if len(username) == 0 {
		c.Ctx.WriteString("Username error !")
		return
	}
	password := c.GetString("Password")
	if len(password) == 0 {
		c.Ctx.WriteString("Password error !")
		return
	}

	if password != beego.AppConfig.String(username) {
		c.Ctx.WriteString("Username & password cannt match !")
		return
	}

	c.TplName = "index.html"

	req := httplib.Post(beego.AppConfig.String("carriershost") + "gmtools/fetch")
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}

	models.UpdateServsByJson(str)

	c.Data["Servs"] = models.GetServerMap()

	c.Data["RollNoticeSendTypes"] = []*SelectOption{
		&SelectOption{"NST_Immediately", true, "立即发送"},
		&SelectOption{"NST_Timming", false, "定时发送"},
		&SelectOption{"NST_Loop", false, "循环发送"},
	}

	c.Data["SendMailTypes"] = []*SelectOption{
		&SelectOption{"IGMT_PlayerId", true, "角色ID"},
		&SelectOption{"IGMT_AllOnline", false, "当前在线"},
		&SelectOption{"IGMT_AllRegist", false, "所有注册"},
	}

	c.Data["CdkeySelectCondition"] = []*SelectOption{
		&SelectOption{"CSC_Channel", true, "渠道"},
		&SelectOption{"CSC_Reward", false, "奖励"},
		&SelectOption{"CSC_ActivationTime", false, "激活时间"},
	}

	csvf, csvferr := models.LoadCsvf("ItemData.csv")
	if csvferr != nil {
		beego.Debug("Load errr")
	}
	items = []*SelectOption{}
	for i := 0; i < csvf.GetRecordLength(); i++ {

		sop := SelectOption{}

		sop.Id, _ = csvf.GetString(i, "ID")
		sop.Value, _ = csvf.GetString(i, "Name")
		sop.IsSelected = (i == 0)

		items = append(items, &sop)
	}
	c.Data["Items"] = items
}

func GetItemIdByName(name string) string {
	for i := 0; i < len(items); i++ {
		if items[i].Value == name {
			return items[i].Id
		}
	}
	return ""
}
