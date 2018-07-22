package pay

import (
	"carriers/models"
	"crypto/md5"
	"encoding/hex"
	"encoding/json"
	"sort"
	"strconv"
	"strings"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type Private struct {
	beego.Controller
}

func (c *Private) Get() {
	beego.Error("PayNotify use get method!!! Host:", c.Ctx.Request.Host)
	c.Ctx.WriteString("{}")
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
	passstr := md5str + beego.AppConfig.String("privatecode")

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
