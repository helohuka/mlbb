package subserver

import (
	"carriers/models"
	"crypto/md5"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"sort"
	"strconv"
	"strings"
	"time"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

const (
	channel    = 500026
	privatekay = "4DE193DAECECAF998499E9CE8D0D490F"
)

type (
	AnyOauth struct {
		beego.Controller
	}

	Oauth struct {
		beego.Controller
	}

	Private struct {
		beego.Controller
	}

	Query struct {
		beego.Controller
	}

	System struct {
		beego.Controller
	}

	Server struct {
		beego.Controller
	}
)

func (c *AnyOauth) Post() {
	//beego.Info("AnyLogin app host :", c.Ctx.Request.URL.Host)
	//beego.Debug("AnyLoginOauth Post body length :", len(c.Ctx.Input.RequestBody))
	beego.Debug("AnyLoginOauth Post body content :", string(c.Ctx.Input.RequestBody))

	req := httplib.Post(beego.AppConfig.String("anyoauthurl"))
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Body(c.Ctx.Input.RequestBody)

	str, err := req.String()

	if err != nil {
		// error
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}
	beego.Debug("LoginOauth Req oauth server result : ", str)

	var oauthObj models.OauthObject
	json.Unmarshal([]byte(str), &oauthObj)

	b, _ := beego.AppConfig.Bool("Debug")
	if b {
		c.Ctx.WriteString(str)
		return
	}

	beego.Debug("Login Status:", oauthObj.Status)
	if oauthObj.Status == "ok" {
		channel := oauthObj.Common.Channel
		//servs := models.GetServsByChannel(channel)
		servs := models.FindServerByChannel(channel)
		if len(servs) == 0 {
			beego.Debug("Channel =======> ", channel, " can not find servs")
			if b {
				c.Ctx.WriteString(str)
				return
			}
			c.Ctx.WriteString("{\"status\":\"fail\"}")
			return
		}
		for _, id := range servs {
			host := models.GetServOauthHostById(id)
			gamereq := httplib.Post(host)
			gamereq.Body([]byte(str))
			_, gameerr := gamereq.String()
			if gameerr != nil {
				beego.Debug(gameerr.Error(), channel, "<>", id)
			}
		}
		c.Ctx.WriteString(str)
		return

	} else if oauthObj.Status == "fail" {
		c.Ctx.WriteString(str)
		return
	} else {
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}

	c.Ctx.WriteString(str)

}

func (c *Oauth) Post() {
	beego.Debug("Client=>", string(c.Ctx.Input.RequestBody))

	req := httplib.Post(beego.AppConfig.String("oauthurl"))
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Body(c.Ctx.Input.RequestBody).Debug(true)

	str, err := req.String()

	if err != nil {
		// error
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}
	b, _ := beego.AppConfig.Bool("Debug")
	var oauthObj models.OauthObject
	err = json.Unmarshal([]byte(str), &oauthObj)
	if err != nil {
		beego.Debug(err.Error())
	}
	beego.Debug("Login object :", oauthObj)

	if oauthObj.Status == "ok" {
		channel := oauthObj.Common.Channel
		//servs := models.GetServsByChannel(channel)
		servs := models.FindServerByChannel(channel)
		if len(servs) == 0 {
			beego.Debug("Channel =======> ", channel, " can not find servs")
			if b {
				c.Ctx.WriteString(str)
				return
			}
			c.Ctx.WriteString("{\"status\":\"fail\"}")
			return
		}
		for _, id := range servs {
			host := models.GetServOauthHostById(id)
			//beego.Debug("Connect game server " + host)
			gamereq := httplib.Post(host)
			gamereq.Body([]byte(str))
			_, gameerr := gamereq.String()
			if gameerr != nil {
				beego.Debug(gameerr.Error(), channel, "<>", id)
			}
		}
		c.Ctx.WriteString(str)
		return

	} else if oauthObj.Status == "fail" {
		c.Ctx.WriteString(str)
		return
	} else {
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}

	c.Ctx.WriteString(str)
}

func (c *Private) Post() {

	var params map[string][]string = c.Ctx.Request.Form
	var sign = strings.Join(params["sign"], "")

	delete(params, "sign")

	var names = make([]string, len(params))

	beego.Info(params)
	i := 0
	for key, _ := range params {
		names[i] = key
		i++
	}

	sort.Strings(names)
	var values = make([]string, len(params))
	beego.Debug(names)
	for j := 0; j < i; j++ {
		values[j] = strings.Join(params[names[j]], "")
	}

	var value = strings.Join(values, "")
	beego.Debug(value)
	var md5value = md5.Sum([]byte(value))

	md5str := hex.EncodeToString(md5value[:])
	beego.Debug(md5str)
	passstr := md5str + privatekay

	beego.Debug(passstr)

	passmd5str := md5.Sum([]byte(passstr))

	pass := hex.EncodeToString(passmd5str[:])
	// or

	beego.Debug("MD5[", pass, "]Sign[", sign, "]")

	if pass == sign {
		beego.Debug("pass == sign")
		servId, _ := strconv.Atoi(c.Input().Get("server_id"))

		if models.CheckSandbox(servId) {
			params["server_id"][0] = "999" ///沙箱操作
		}

		b, _ := json.Marshal(params)

		models.InsertOrderLog(string(b))

		host := models.GetServPayHostById(servId)
		gamereq := httplib.Post(host)
		gamereq.Body(b)

		_, gameerr := gamereq.String()
		if gameerr != nil {
			beego.Error(gameerr.Error())
			c.Ctx.WriteString("FAIL")
			return
		}
		beego.Debug(string(b))
		c.Ctx.WriteString("OK")
	} else {
		c.Ctx.WriteString("FAIL")
	}

}

func (c *Query) Post() {
	//beego.Debug(string(c.Ctx.Input.RequestBody))
	err := c.Ctx.Request.ParseForm()
	if err != nil {
		beego.Debug(err)
	}
	username := c.Input().Get("username")
	version := c.Input().Get("version")

	s := models.GetServsJson4(username, version, fmt.Sprintf("%d", channel))

	c.Ctx.WriteString(s)

	beego.Info("500026 ", username, version, c.Ctx.Request.RemoteAddr, time.Now().Format("2006-01-02 15:04:05"), s)
}

func (c *System) Post() {
	notice := models.GetSystemNotice(fmt.Sprintf("%d", channel))
	c.Ctx.WriteString(notice)
}

func (c *Server) Post() {
	servid, _ := strconv.Atoi(c.GetString("servid"))

	serv := models.GetServById(servid)
	if serv != nil {
		c.Ctx.WriteString(serv.Notice)
	} else {
		c.Ctx.WriteString("服务器不在线")
	}
}
