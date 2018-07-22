package notice

import (
	"carriers/models"
	"strconv"

	"github.com/astaxie/beego"
)

type Server struct {
	beego.Controller
}

func (c *Server) Post() {
	servid, _ := strconv.Atoi(c.Input().Get("servid"))
	if servid < 2000 {
		c.Ctx.WriteString("")
		return
	}
	serv := models.GetServById(servid)
	if serv != nil {
		c.Ctx.WriteString(serv.Notice)
	} else {
		c.Ctx.WriteString("服务器不在线")
	}
}
