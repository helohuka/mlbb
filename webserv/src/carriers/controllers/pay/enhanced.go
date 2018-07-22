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

type Enhanced struct {
	beego.Controller
}

func (c *Enhanced) Get() {
	beego.Error("PayNotify use get method!!! Host:", c.Ctx.Request.Host)
	c.Ctx.WriteString("{}")
}

func (c *Enhanced) Post() {

	var params map[string][]string = c.Ctx.Request.Form

	var enhancedSign = strings.Join(params["enhanced_sign"], "")

	delete(params, "sign")
	delete(params, "enhanced_sign")

	var names = make([]string, len(params))

	beego.Debug(params)

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
	passstr := md5str + beego.AppConfig.String("enhancedcode")

	beego.Debug(passstr)

	passmd5str := md5.Sum([]byte(passstr))

	pass := hex.EncodeToString(passmd5str[:])
	// or

	beego.Debug("MD5[", pass, "]Sign[", enhancedSign, "]")

	if pass == enhancedSign {
		b, _ := json.Marshal(params)
		servId, _ := strconv.Atoi(c.Input().Get("server_id"))
		host := models.GetServPayHostById(servId)
		gamereq := httplib.Post(host)
		gamereq.Body(b)

		_, gameerr := gamereq.String()
		if gameerr != nil {
			beego.Debug("Connect game server fail")
			c.Ctx.WriteString("FAIL")
			return
		}
		beego.Debug(b)
		c.Ctx.WriteString("OK")
	} else {
		c.Ctx.WriteString("FAIL")
	}

}
