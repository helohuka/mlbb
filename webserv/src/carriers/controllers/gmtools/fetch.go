package gmtools

import (
	"carriers/models"

	"github.com/astaxie/beego"
)

type Fetch struct {
	beego.Controller
}

func (c *Fetch) Get() {
	c.Ctx.WriteString(models.GetAreaServsJson())
}

func (c *Fetch) Post() {
	c.Ctx.WriteString(models.GetAreaServsJson())
}
