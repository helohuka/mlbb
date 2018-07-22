package models

import (
	"database/sql"
	"encoding/json"
	_ "errors"

	_ "strconv"

	"github.com/astaxie/beego"
)

func Mysql() (*sql.DB, error) {
	dsn := beego.AppConfig.String("dbuser") + ":" + beego.AppConfig.String("dbpass") + "@tcp(" + beego.AppConfig.String("dbhost") + ":" + beego.AppConfig.String("dbport") + ")/" + beego.AppConfig.String("dbname")
	beego.Debug(dsn)
	return sql.Open("mysql", dsn)
}

type OrderInner struct {
	OrderId   int    `json:"orderid"`
	Game      string `json:"game"`
	PfId      int    `json:"pfid"`
	PfName    string `json:"pfname"`
	RoleId    int    `json:"roleid"`
	RoleLv    int    `json:"rolelv"`
	AccountId int    `json:"accountid"`
	Payment   int    `json:"payment"`
	PayTime   string `json:"paytime"`
}
type OrderNotice struct {
	AreaMap map[string][]OrderInner
	IdMap   map[int]*OrderInner
	Orders  []*OrderInner
}
type Order struct {
	OrderInner
}

var sd OrderNotice

func UpdateOrders(m map[string][]OrderInner) {
	sd.AreaMap = m
	sd.Orders = []*OrderInner{}
	sd.IdMap = map[int]*OrderInner{}

	for _, ss := range m {
		for i := 0; i < len(ss); i++ {
			sd.Orders = append(sd.Orders, &ss[i])

			if sd.IdMap[ss[i].OrderId] != nil {
				beego.Debug("Id same error!!!", ss[i].OrderId)
			}

			sd.IdMap[ss[i].OrderId] = &ss[i]
		}
	}
}
func UpdateOrdersByJson(jsonstr string) {

	m := map[string][]OrderInner{}
	json.Unmarshal([]byte(jsonstr), &m)
	UpdateOrders(m)
}
func GetOrdersById(id int) *OrderInner {
	return sd.IdMap[id]
}

func GetOrderMap() *map[string][]OrderInner {
	return &sd.AreaMap
}

type OrdersCache struct {
	Orders  []*Order
	OrderId map[int]*Order
}

func (sc *OrdersCache) Init() {
	sc.Orders = []*Order{}
	sc.OrderId = map[int]*Order{}

	db, err := Mysql()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM Orders")

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	for records.Next() {
		ser := Order{}
		err := records.Scan(&ser.OrderId, &ser.Game, &ser.PfId, &ser.PfName, &ser.RoleId, &ser.RoleLv, &ser.AccountId, &ser.Payment, &ser.PayTime)
		if err != nil {
			beego.Error("DB fail", err)
			return
		}

		sc.Orders = append(sc.Orders, &ser)

	}

}
