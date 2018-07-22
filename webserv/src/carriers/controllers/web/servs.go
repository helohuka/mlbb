package web

import (
	"carriers/models"

	"encoding/json"

	"github.com/astaxie/beego"
)

type (

	ServerInfo struct {
		Id int `json:"id"`
		ServId int `json:"serverID"`
		ServName string `json:"serverName"`
	}

	ServerList struct {
		StatusCode int               `json:"state"`
		ServerList []ServerInfo `json:"data"`
	}

	GetServerList struct {
		beego.Controller
	}
)

func (c *GetServerList) Get() {
	servs := models.GetServers()

	sl := ServerList{1, []ServerInfo{}}

	for _, sv := range servs {

		si := ServerInfo{}
		si.Id = sv.Area
		si.ServId = sv.Id
		si.ServName = sv.Name


		sl.ServerList = append(sl.ServerList,si)
	}

	ret, err := json.Marshal(sl)

	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString("{\"statusCode\":300}")
	}

	c.Ctx.WriteString(string(ret))
}
