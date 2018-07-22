package tables

import (
	"archive/zip"
	"carriers/models"
	"fmt"
	"os"
	"time"

	"github.com/astaxie/beego"
)

type (
	GetOrder struct {
		beego.Controller
	}
)

func (c *GetOrder) Get() {
	conn, err := models.CarriersDB()
	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString(err.Error())
		return
	}
	defer conn.Close()
	records, err := conn.Query("SELECT RoleId,PFID,PFName,ServerId,AccountName,Sum(Payment),Max(RoleLevel) FROM OrderLog GROUP BY RoleId")

	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString(err.Error())
		return
	}
	filename := fmt.Sprintf("static/%s_%d.zip", beego.AppConfig.String("gamename"), time.Now().Unix())

	ziparchive, _ := os.Create(filename)

	zipbuffer := zip.NewWriter(ziparchive)

	zipwriter, _ := zipbuffer.Create("order.csv")

	zipwriter.Write([]byte("ServerId,AccountName,RoleId,PFID,PFName,Sum(Payment),Max(RoleLevel)\n"))

	var roleId, pfId, pfName, accountName string
	var serverId, payment, roleLevel int

	for records.Next() {
		err := records.Scan(&roleId, &pfId, &pfName, &serverId, &accountName, &payment, &roleLevel)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		zipwriter.Write([]byte(fmt.Sprintf("%d,%s,%s,%s,%s,%d,%d\n", serverId, accountName, roleId, pfId, pfName, payment, roleLevel)))
	}

	zipbuffer.Close()

	ziparchive.Close()

	c.Redirect(fmt.Sprintf("/%s", filename), 302)
}
