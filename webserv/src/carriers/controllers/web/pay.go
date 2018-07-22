package web

import (
	"carriers/models"
	"crypto/md5"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"sort"
	"strconv"
	"strings"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type (
	_GMTPayCommand struct {
		Type     string
		PlayerId int
		ShopId   int
		Payment  float32
		OrderId  string
	}
	_GMTPayResult struct {
		ErrorNo int    `json:"error"`
		Desc    string `json:"desc"`
	}
	Pay struct {
		beego.Controller
	}
)

func (c *Pay) checkSign() bool {
	var postval map[string][]string = c.Ctx.Request.Form
	//beego.Debug(postval)

	names := []string{}
	for k, _ := range postval {
		if c.Input().Get(k) != "" {
			names = append(names, k)
		}
	}
	sort.Strings(names)

	params := ""
	mapping := map[string]string{}
	for _, k := range names {
		if k == "sign" {
			continue
		}
		params += fmt.Sprintf("%s=%s&", k, c.Input().Get(k))
		mapping[k] = c.Input().Get(k)
	}
	logs, _ := json.Marshal(mapping)

	beego.Info(string(logs))

	params += fmt.Sprintf("key=%s", beego.AppConfig.String("tanyupaykey"))

	md5byes := md5.Sum([]byte(params))

	md5str := hex.EncodeToString(md5byes[:])

	sign := c.Input().Get("sign")

	md5str = strings.ToUpper(md5str)

	beego.Debug(md5str, "=", sign)

	return md5str == sign
}

func (c *Pay) Post() {

	if !c.checkSign() {
		beego.Debug("Tanyu pay sign fail")
		c.Ctx.WriteString("fail")
		return
	}

	serverId, _ := strconv.ParseInt(c.Input().Get("server_id"), 10, 32)
	roleId, _ := strconv.ParseInt(c.Input().Get("role_id"), 10, 32)
	shopId, _ := strconv.ParseInt(c.Input().Get("product_id"), 10, 32)
	payment, _ := strconv.ParseFloat(c.Input().Get("product_id"), 32)
	orderId := c.Input().Get("transaction_id")

	sv := models.GetServById(int(serverId))

	if nil == sv {
		beego.Error("Tanyu pay can not find server ", serverId)
		c.Ctx.WriteString("fail")
		return
	}

	cmd, _ := json.Marshal(_GMTPayCommand{"GNT_MakeOrder", int(roleId), int(shopId), float32(payment), orderId})
	gmtreq := httplib.Post(sv.GetGMTHost())
	gmtreq.Body([]byte(cmd))

	gmtres, gmterr := gmtreq.String()

	if gmterr != nil {
		beego.Debug(gmterr.Error())
		c.Ctx.WriteString("fail")
		return
	}

	gmtresult := _GMTPayResult{}

	err := json.Unmarshal([]byte(gmtres), &gmtresult)

	if err != nil {
		beego.Debug(err.Error())
		c.Ctx.WriteString("fail")
		return
	}

	if gmtresult.ErrorNo != 0 {
		beego.Debug(orderId, " logic server error ", gmtresult.Desc)
		c.Ctx.WriteString("fail")
		return
	}

	c.Ctx.WriteString("success")
}
