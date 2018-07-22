package controllers

import (
	"encoding/csv"
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"os"
	_ "strconv"
)

type CDKeyReward struct {
	CDKey      string
	PlayerName string
	UsedTime   int
}
type SelectReward struct {
	ItemId  int
	ItemNum int
}
type DoCdkeySelectCmd struct {
	Name       string
	GiftItems  []SelectReward
	CDKeyInfos []CDKeyReward
}
type DoCdkeySelect struct {
	beego.Controller
}

func (this *DoCdkeySelect) Post() {
	this.Ctx.Request.ParseForm()

	tag := this.GetString("cdkey_select")
	req := httplib.Get(beego.AppConfig.String("carriershost") + "servs/querycdkey?tag=" + tag)
	str, err := req.String()

	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}

	cmd := DoCdkeySelectCmd{}
	jerr := json.Unmarshal([]byte(str), &cmd)

	if jerr != nil {
		beego.Debug(jerr.Error())
	}
	var arr = make([]string, len(cmd.CDKeyInfos))
	for i := 0; i < len(cmd.CDKeyInfos); i++ {
		arr[i] = cmd.CDKeyInfos[i].CDKey
	}
	//导出excel
	f, err := os.Create("static/gift/" + tag + ".csv")
	if err != nil {
		panic(err)
	}
	defer f.Close()

	f.WriteString("\xEF\xBB\xBF") // 写入UTF-8 BOM
	w := csv.NewWriter(f)
	for j := 0; j < len(arr); j++ {
		w.Write([]string{arr[j]})
	}
	w.Flush()
	this.Ctx.WriteString(str)

}
