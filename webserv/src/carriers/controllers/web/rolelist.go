package web

import (
	"carriers/models"
	"encoding/json"
	"strconv"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type (
	_GMTRoleListCommand struct {
		Type     string
		UserName string
	}

	_GMTRoleListResult struct {
		ErrorNo int    `json:"error"`
		Desc    string `json:"desc"`
	}

	Role struct {
		UserId    string `json:"id"`
		RoleId    int    `json:"roleId"`
		RoleName  string `json:"userName"`
		RoleLevel int    `json:"level"`
		Vip       int    `json:"vip"`
	}
	ReqRoles struct {
		StatusCode int    `json:"statusCode"`
		RoleList   []Role `json:"roleList"`
	}

	GetRoleList struct {
		beego.Controller
	}
)

func (c *GetRoleList) makeReqString(req *ReqRoles) string {
	r, _ := json.Marshal(req)
	return string(r)
}

func (c *GetRoleList) Get() {
	username := c.Input().Get("username")
	serverId, _ := strconv.ParseInt(c.Input().Get("server_id"), 10, 32)

	req := ReqRoles{}

	if len(username) == 0 {
		req.StatusCode = 301
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	sv := models.GetServById(int(serverId))

	if nil == sv {
		req.StatusCode = 302
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	cmd, _ := json.Marshal(_GMTRoleListCommand{"GMT_QueryRoleList", username})
	gmtreq := httplib.Post(sv.GetGMTHost())
	gmtreq.Body([]byte(cmd))

	gmtres, gmterr := gmtreq.String()

	if gmterr != nil {
		beego.Debug(gmterr.Error())
		req.StatusCode = 300
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	beego.Debug(string(gmtres))

	gmtresult := _GMTRoleListResult{}

	err := json.Unmarshal([]byte(gmtres), &gmtresult)

	if err != nil {
		beego.Debug(err.Error())
		req.StatusCode = 301
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	req.RoleList = []Role{}

	err = json.Unmarshal([]byte(gmtresult.Desc), &req.RoleList)

	if err != nil {
		beego.Debug(err.Error())
		req.StatusCode = 301
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}
	req.StatusCode = 200
	reqs, err := json.Marshal(req)
	if err != nil {
		beego.Debug(err.Error())
		req.StatusCode = 301
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	if len(req.RoleList) == 0 {
		req.StatusCode = 301
		c.Ctx.WriteString(c.makeReqString(&req))
		return
	}

	c.Ctx.WriteString(string(reqs))
}
