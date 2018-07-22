package gmtools

import (
	"carriers/models"
	"encoding/json"
	"github.com/astaxie/beego"
)

type Channel struct {
	beego.Controller
}

type CHReslut struct {
	Error int    `json:"error"`
	Desc  string `json:"desc"`
}

func (this *Channel) Get() {
	//models.FindChannel()
	str := models.FindChannel()
	this.Ctx.WriteString(str)
}

func (c *Channel) Post() {

	result := [1]CHReslut{}
	functype := c.GetString("delete")

	if functype == "delete" {
		err := models.DeleteChannel(c.GetString("channel"), c.GetString("version"))
		if err != nil {
			result[0].Error = 1
			result[0].Desc = "Delete channel fail"
		} else {
			result[0].Error = 0
			result[0].Desc = "Delete channel succ"
		}
	} else {
		err := models.InsertChannel(c.GetString("channel"), c.GetString("version"), c.GetString("serverids"))
		if err != nil {
			result[0].Error = 1
			result[0].Desc = "Change channel fail"
		} else {
			result[0].Error = 0
			result[0].Desc = "Change channel succ"
		}
	}

	strresult, _ := json.Marshal(result)
	c.Ctx.WriteString((string)(strresult))
}
