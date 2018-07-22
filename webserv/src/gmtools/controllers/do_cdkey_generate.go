package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"strconv"
)

type ItemReward struct {
	ItemId  int
	ItemNum int
}
type DoCdkeyGenerateCmd struct {
	Items []ItemReward
}
type DoCdkeyGenerate struct {
	beego.Controller
}

func (this *DoCdkeyGenerate) Post() {
	this.Ctx.Request.ParseForm()

	count := this.GetString("cdkey_generate_number")

	tag := this.GetString("cdkey_generate_tag")

	cmd := DoCdkeyGenerateCmd{}

	cmd.Items = []ItemReward{}
	for i := 1; i <= 5; i++ {
		ItemId, _ := strconv.Atoi(this.GetString("item_" + strconv.FormatInt(int64(i), 10)))
		ItemNum, _ := strconv.Atoi(this.GetString("stack_" + strconv.FormatInt(int64(i), 10)))

		if cmd.Items == nil {
			cmd.Items = []ItemReward{}
		}
		rwd := ItemReward{}

		rwd.ItemId = ItemId
		rwd.ItemNum = ItemNum

		cmd.Items = append(cmd.Items, rwd)

	}

	jparam, _ := json.Marshal(cmd.Items)
	//url := "http://10.10.10.254:18080/servs/gencdkey?tag=" + tag + "&count=" + count + "&items=" + string(jparam)
	url := beego.AppConfig.String("carriershost") + "servs/gencdkey?tag=" + tag + "&count=" + count + "&items=" + string(jparam)
	beego.Debug(url)
	req := httplib.Get(url)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
		this.Ctx.WriteString("")
		return
	}
	this.Ctx.WriteString(str)

}
