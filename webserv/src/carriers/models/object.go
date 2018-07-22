package models

type OauthCommon struct {
	Channel  string `json:"channel"`
	UserSdk  string `json:"user_sdk"`
	Uid      string `json:"uid"`
	ServerId string `json:"server_id"`
	PluginId string `json:"plugin_id"`
}

type OauthObject struct {
	Status string                 `json:"status"`
	Data   map[string]interface{} `json:"data"`
	Common OauthCommon            `json:"common"`
	Ext    string                 `json:"ext"`
	SN     string                 `json:"sn"`
}

func NewOauthObject() *OauthObject {
	return &OauthObject{}
}
