package tools

import (


"github.com/astaxie/beego"
)

type (
	SDKInfo struct {
		AnyId   string //自定义ID
		PfId    string //渠道ID
		PfName  string //渠道名
		Desc    string //描述
		AddTime string //添加时间
	}

	SDKInfoCache struct {
		Infos []*SDKInfo
		AnyId map[string]*SDKInfo
	}
)

var sdkCache = SDKInfoCache{}

func (c *SDKInfoCache) AddSDK(sdk *SDKInfo) {
	if c.AnyId[sdk.AnyId] != nil {
		beego.Error("Can not insert same anyid")
		return
	}
	c.Infos = append(c.Infos, sdk)
	c.AnyId[sdk.AnyId] = sdk
}

func (c *SDKInfoCache) Init() {
	c.Infos = []*SDKInfo{}
	c.AnyId = map[string]*SDKInfo{}

	db, err := OpenDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM SDKInfo")

	if err != nil {
		beego.Error(err.Error())
		return
	}

	for records.Next() {
		sid := 0
		info := SDKInfo{}
		err := records.Scan(&sid, &info.AnyId, &info.PfId, &info.PfName, &info.Desc, &info.AddTime)
		if err != nil {
			beego.Error(err)
			continue
		}
		//beego.Debug(info.AnyId)
		c.AddSDK(&info)
	}

	beego.Info("Init SDK information ok")
}

func CareSDKInfo(pfid string, pfname string) (string, string) {
	if sdkCache.AnyId[pfid] == nil {
		//beego.Error("Can not anyid to sdkid", pfid)
		return pfid, pfname
	}
	if sdkCache.AnyId[pfid].PfName == "" {
		return sdkCache.AnyId[pfid].PfId, pfname
	}
	return sdkCache.AnyId[pfid].PfId, sdkCache.AnyId[pfid].PfName
}

func InitSDKInfo(){
	sdkCache.Init()
}