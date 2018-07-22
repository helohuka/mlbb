package controllers

import (
	"encoding/json"

	"github.com/astaxie/beego"
)

type SkillInfo struct {
	Id    uint
	Level uint
	Exp   uint
}

type ItemInfo struct {
	Id    uint
	Stack uint
}

type BabyInfo struct {
	Id          uint
	Name        string
	Level       uint
	Exp         uint
	StrongLevel uint
	Gears       []uint
	Skills      []SkillInfo
}

type EmplyeeInfo struct {
	Id         uint
	Name       string
	Level      uint
	Exp        uint
	Color      uint
	Skills     []SkillInfo
	Equipments []ItemInfo
}

type PlayerInfo struct {
	AccId         string
	Id            uint
	Name          string
	Level         uint
	Exp           uint
	Guide         uint
	Guild         uint
	Prof          uint
	ProfLevel     uint
	Gold          uint
	Money         uint
	MagicCurrency uint
	ComplateQuest []uint
	CurrentQuest  []uint
	Skills        []SkillInfo
	BagItems      []ItemInfo
	Equipments    []ItemInfo
	Babies        []BabyInfo
	Emplyees      []EmplyeeInfo
	ItemStore     []ItemInfo
}

type QueryPlayerResult struct {
	beego.Controller
}

func (c *QueryPlayerResult) Get() {
	c.Ctx.Request.ParseForm()
	c.TplName = "query_player_result.html"
	info := PlayerInfo{}
	json.Unmarshal(([]byte)(c.Input().Get("param")), &info)
	c.Data["Param"] = info
}
