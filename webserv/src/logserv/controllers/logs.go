package controllers

import (
	"encoding/json"
	"logserv/models"

	"github.com/astaxie/beego"
)

type (
	InsertController struct {
		beego.Controller
	}
)

func (c *InsertController) Post() {
	what := c.Input().Get("type")

	switch what {
	case "account":
		c.AccountLog()
		break
	case "login":
		c.LoginLog()
		break
	case "role":
		c.RoleLog()
		break
	case "order":
		c.OrderLog()
		break
	case "query":
		c.QueryLog()
	default:
		beego.Error("Can not find log type ", what)
		break
	}

	c.Ctx.WriteString("")
}

func (c *InsertController) AccountLog() {
	value := c.Input().Get("value")
	beego.Debug(value)
	record := models.AccountRecord{}
	err := json.Unmarshal([]byte(value), &record)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	go models.InsertAccountRecord(&record)
}

func (c *InsertController) LoginLog() {
	value := c.Input().Get("value")
	beego.Debug(value)
	record := models.LoginRecord{}
	err := json.Unmarshal([]byte(value), &record)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	// if record.ServerId < 2000 {
	// 	beego.Debug("Is test server ")
	// 	return
	// }
	go models.InsertLoginRecord(&record)
}

func (c *InsertController) RoleLog() {
	value := c.Input().Get("value")
	//beego.Debug(value)
	records := []models.RoleRecord{}
	err := json.Unmarshal([]byte(value), &records)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	if len(records) == 0 {
		return
	}
	// if records[0].ServerId < 2000 {
	// 	beego.Debug("Is test server ")
	// 	return
	// }

	go models.InsertRoleRecords(&records)

}

func (c *InsertController) OrderLog() {
	value := c.Input().Get("value")
	beego.Debug(value)
	record := models.OrderRecord{}
	err := json.Unmarshal([]byte(value), &record)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	// if record.ServerId < 2000 {
	// 	beego.Debug("Is test server ")
	// 	return
	// }
	go models.InsertOrderRecord(&record)
}

func (c *InsertController) QueryLog() {
	value := c.Input().Get("value")
	beego.Debug(value)
	record := models.QueryRecord{}
	err := json.Unmarshal([]byte(value), &record)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	models.InsertQueryRecord(&record)
}
