package models

import (
	"bytes"
	"encoding/json"
	"fmt"
	"time"

	"github.com/astaxie/beego"
)

type (
	COM_GiftItem struct {
		ItemId  int
		ItemNum int
	}

	GiftGroup struct {
		Name      string
		Diamond   int
		GiftItems []COM_GiftItem
	}

	CDKeyInfo struct {
		CDKey      string
		PlayerName string
		UsedTime   int
	}

	GiftInfo struct {
		GiftGroup
		CDKeyInfos []CDKeyInfo
	}
)

var giftset = map[string]*GiftGroup{}
var keyset = map[string]*GiftGroup{}

func parseItemArrayBytes(b []byte) []COM_GiftItem {

	buffer := bytes.NewBuffer(b)

	b1, _ := buffer.ReadByte()
	n := (int(b1) & 0XC0) >> 6
	len := int(b1) & 0X3F
	for i := 0; i < n; i++ {
		b2, _ := buffer.ReadByte()
		len = (len << 8) | int(b2)
	}

	if len == 0 {
		return nil
	}

	//beego.Info(len)
	items := make([]COM_GiftItem, len)
	for i := 0; i < len; i++ {
		buffer.ReadByte()
		item := COM_GiftItem{}
		{
			a, _ := buffer.ReadByte()
			b, _ := buffer.ReadByte()
			c, _ := buffer.ReadByte()
			d, _ := buffer.ReadByte()
			item.ItemId = int(a) | int(b)<<8 | int(c)<<16 | int(d)<<24
		}
		{
			a, _ := buffer.ReadByte()
			b, _ := buffer.ReadByte()
			c, _ := buffer.ReadByte()
			d, _ := buffer.ReadByte()
			item.ItemNum = int(a) | int(b)<<8 | int(c)<<16 | int(d)<<24
		}
		items[i] = item
	}

	return items
}

func GiftInit() {
	go giftInit()
}

func giftInit() {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM `GiftInfo` WHERE `playerName`=''")

	if err != nil {
		beego.Error(err.Error())
		return
	}
	sortId := 0
	cdkey := ""
	giftName := ""
	playerName := ""
	useTime := 0
	diamond := 0
	itemBytes := []byte{}

	for records.Next() {
		err := records.Scan(&sortId, &cdkey, &giftName, &playerName, &useTime, &itemBytes, &diamond)
		if err != nil {
			beego.Error(err.Error())
			continue
		}

		if giftset[giftName] == nil {
			gift := GiftGroup{}
			gift.Name = giftName
			gift.Diamond = diamond

			if len(itemBytes) != 0 {
				if itemBytes[0] != '[' {
					gift.GiftItems = parseItemArrayBytes(itemBytes)
				} else {
					gift.GiftItems = []COM_GiftItem{}
					jerr := json.Unmarshal(itemBytes, &gift.GiftItems)
					if jerr != nil {

						beego.Error(jerr, string(itemBytes))
					}
				}
			}
			giftset[giftName] = &gift
		}

		if playerName != "" || useTime != 0 {
			continue
		}

		if keyset[cdkey] != nil {
			beego.Error("CDKEY already defined ", cdkey)
			continue
		}
		keyset[cdkey] = giftset[giftName]
	}

	beego.Info("Gift infotmation ok ")
}

func InsertCDKEY(cdkeys []string, name, jsonitems string) {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	stmt, err := db.Prepare("INSERT INTO `GiftInfo`(`cdkey`,`giftName`,`playerName`,`useTime`,`rewardItem`, `diamond`)VALUES(?,?,'',0,?,0)")

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer stmt.Close()

	for _, v := range cdkeys {
		_, err := stmt.Exec(v, name, jsonitems)

		if err != nil {
			beego.Error(err.Error())
			return
		}
	}
}

func GenCDKey(tag string, count int, strItems string) {
	if count < 0 {
		return
	}

	items := []COM_GiftItem{}
	err := json.Unmarshal([]byte(strItems), &items)
	if err != nil {
		beego.Error(err.Error())
		return
	}

	stamp0 := 0
	for _, v := range tag {
		stamp0 += int(v)
	}

	stamp1 := 0
	for _, v := range items {
		stamp1 += v.ItemId + v.ItemNum
	}
	gift := GiftGroup{}
	gift.Name = tag
	gift.GiftItems = items
	giftset[tag] = &gift

	cdkeys := []string{}
	for i := 0; i < count; i++ {

		cdkey := fmt.Sprintf("%X%X%X", stamp0, stamp1, i)
		if keyset[cdkey] != nil {
			beego.Error("CDKEY already defined 0 ", cdkey)
			continue
		}

		keyset[cdkey] = giftset[tag]

		cdkeys = append(cdkeys, cdkey)

		if i%5000 == 0 {
			go InsertCDKEY(cdkeys, tag, strItems)
			cdkeys = []string{}
		}

	}

	if len(cdkeys) != 0 {
		go InsertCDKEY(cdkeys, tag, strItems)
	}
}

func DelCDKEY(tag string) {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	_, err = db.Exec("DELETE FROM `GiftInfo` WHERE `giftName`= ? ", tag)

	if err != nil {
		beego.Error(err.Error())
	}
}

func QueryCDKEY(tag string) string {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return ""
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM `GiftInfo` WHERE giftName = ?", tag)

	if err != nil {
		beego.Error(err.Error())
		return ""
	}
	sortId := 0
	cdkey := ""
	giftName := ""
	playerName := ""
	useTime := 0
	diamond := 0
	itemBytes := []byte{}

	var info *GiftInfo

	for records.Next() {
		err := records.Scan(&sortId, &cdkey, &giftName, &playerName, &useTime, &itemBytes, &diamond)
		if err != nil {
			beego.Error(err.Error())
			continue
		}

		if info == nil {
			info = &GiftInfo{}
			info.Name = giftName

			if len(itemBytes) != 0 {
				if itemBytes[0] != '[' {
					info.GiftItems = parseItemArrayBytes(itemBytes)
				} else {
					info.GiftItems = []COM_GiftItem{}
					jerr := json.Unmarshal(itemBytes, &info.GiftItems)
					if jerr != nil {
						beego.Error(jerr, string(itemBytes))
					}
				}
			}
		}

		info.CDKeyInfos = append(info.CDKeyInfos, CDKeyInfo{cdkey, playerName, useTime})
	}

	b, err := json.Marshal(info)

	return string(b)
}

func ReqCDKEY(cdkey string, playerName string, names []string) ([]COM_GiftItem, int, string, string) {
	if keyset[cdkey] == nil {
		beego.Warning("Req undefined cdkey ", cdkey)
		if cdkey[0] == '-' {
			return nil, 0, "", "EN_Max"
		} else {
			return nil, 0, "", "EN_IdgenNull"
		}
	}

	r := keyset[cdkey].GiftItems
	n := keyset[cdkey].Name
	d := keyset[cdkey].Diamond

	for _, v := range names {
		if v == n {
			beego.Info("Can not req same gift ")
			return nil, 0, "", "EN_Gifthas"
		}
	}

	keyset[cdkey] = nil

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return nil, 0, "", "EN_Idgenhas"
	}
	defer db.Close()

	_, err = db.Exec("UPDATE GiftInfo SET playerName = ?, useTime = ? WHERE cdkey = ?", playerName, time.Now().Unix(), cdkey)

	return r, d, n, "EN_None"
}
