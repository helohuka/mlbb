package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"time"
	"fmt"
	"strconv"
	"crypto/aes"
	"crypto/cipher"
	"encoding/base64"
	"ninegame/models"

	"crypto/md5"
	"encoding/hex"

	"strings"
)


const (
	kAPIKey        = "c10d65a6021b7db68fb9c7a7138da7b2"
	kCaller        = "xysk"
	kCBCEncryptKey = "1chuK86H2689v9zt"
)

type (
	Status struct{
		Code int `json:"code"`
		Message string `json:"msg"`
	}

	GetServerList struct {
		beego.Controller
	}

	GetPlayerInfomation struct {
		beego.Controller
	}

	SendNineGameGift struct{
		beego.Controller
	}

	GetGiftCount struct {
		beego.Controller
	}
)

func CBCEncrypt(key, plaintText []byte) []byte {
	block, err := aes.NewCipher(key) //选择加密算法
	if err != nil {
		return nil
	}
	plaintText = PKCS7Padding(plaintText, block.BlockSize())

	blockModel := cipher.NewCBCEncrypter(block, key)

	cipherText := make([]byte, len(plaintText))

	blockModel.CryptBlocks(cipherText, plaintText)
	return cipherText
}

func PKCS7Padding(cipherText []byte, blockSize int) []byte {
	padding := blockSize - len(cipherText)%blockSize
	padtext := make([]byte, padding)
	return append(cipherText, padtext...)
}
func CBCDecrypt(key, cipherText []byte) []byte {
	keyBytes := []byte(key)
	block, err := aes.NewCipher(keyBytes) //选择加密算法
	if err != nil {
		return nil
	}
	blockModel := cipher.NewCBCDecrypter(block, keyBytes)
	plaintText := make([]byte, len(cipherText) * 4)
	blockModel.CryptBlocks(plaintText, cipherText)
	beego.Debug(len(cipherText))
	beego.Debug(block.BlockSize())
	plaintText = PKCS7UnPadding(plaintText, block.BlockSize())
	beego.Debug(len(strings.Trim(string(plaintText)," ")))
	return plaintText
}

func PKCS7UnPadding(plantText []byte, blockSize int) []byte {
	length := len(plantText)
	unpadding := int(plantText[length-1])
	return plantText[:(length - unpadding)]
}

func Encrypt(key, text string) string {
	key_bytes := make([]byte, 16)
	copy(key_bytes, []byte(key))
	encrypted := CBCEncrypt(key_bytes, []byte(text))
	return base64.StdEncoding.EncodeToString(encrypted)
}

func Decrypt(key, text string) string {
	key_bytes := make([]byte, 16)
	copy(key_bytes, []byte(key))
	beego.Debug(text)
	text_bytes, err := base64.StdEncoding.DecodeString(text)
	if err != nil {
		return ""
	}

	decrypted := CBCDecrypt(key_bytes, text_bytes)
	return string(decrypted)
}

func CalcSign(str string)string{
	str = kCaller + str + kAPIKey
	bStr := md5.Sum([]byte(str))
	beego.Debug("CALC FROM SIGN ", str, " ", bStr)
	md5str := hex.EncodeToString(bStr[:])
	beego.Debug("CALC FROM SIGN HEX", md5str)
	return  md5str
}

func UnpackRequestParams(body []byte) (Status,map[string]interface{}){
	status := Status{2000000,"OK"}

	params := map[string]interface{}{}
	err := json.Unmarshal(body, &params)
	if err != nil{
		status.Code = 5000020
		status.Message = "业务参数错误"
		return status,nil
	}

	if params["client"].(map[string]interface{})["caller"].(string) != kCaller {
		status.Code = 5000010
		status.Message = "Caller错误"

		return status,nil
	}

	sign := CalcSign(fmt.Sprintf("params=%s",params["data"].(map[string]interface{})["params"].(string)))

	if sign != params["sign"].(string){
		status.Code = 5000011
		status.Message = "签名错误"

		return status,nil
	}

	dataStr := Decrypt(kCBCEncryptKey, params["data"].(map[string]interface{})["params"].(string))
	dataStr = strings.Trim(dataStr," \r\n\t\x00 ")
	data := map[string]interface{}{}

	err = json.Unmarshal([]byte(dataStr), &data)
	if err != nil{
		status.Code = 5000020
		status.Message = "业务参数无效"
		beego.Error(dataStr)
		beego.Error(err)
		return status,nil
	}

	return  status, data
}

func PackReturnParams(status Status, data map[string]interface{})string{
	params := map[string]interface{}{}
	params["id"] = time.Now().Unix()
	params["state"] = status

	dataStr, err := json.Marshal(data)
	if err != nil{
		beego.Error(err)
	}

	cbcDataStr := Encrypt(kCBCEncryptKey,string(dataStr))

	params["sign"] = CalcSign(fmt.Sprintf("prams=%s",cbcDataStr))
	params["data"] = cbcDataStr

	paramsStr, err := json.Marshal(params)
	if err != nil{
		beego.Error(err)
	}
	return string(paramsStr)
}


func (c *GetServerList) Post(){
	beego.Debug(string(c.Ctx.Input.RequestBody))

	status , data := UnpackRequestParams(c.Ctx.Input.RequestBody)

	beego.Debug(status,"|", data)
	first := int(data["count"].(float64) * (data["page"].(float64) -1 ))
	last :=  int(data["count"].(float64) * data["page"].(float64))

	pageServers := models.Servers

	if first < 0 {
		first = 0
	}
	if last > len(models.Servers){
		last = len(models.Servers)
	}

	pageServers = pageServers[first:last]

	object := map[string]interface{}{}
	object["recordCount"] = len(models.Servers)
	object["list"] = []map[string]string{}
	for _, s := range pageServers{
		serverObject := map[string]string{}
		serverObject["servId"] = strconv.Itoa(s.ServerId)
		serverObject["serverName"] = s.ServerName
		object["list"] = append(object["list"].([]map[string]string),serverObject)
	}

	beego.Debug(status,"|", object)

	result := PackReturnParams(status,object)

	beego.Debug(result)

	c.Ctx.WriteString(result)
}

func (c *GetPlayerInfomation) Post() {
	beego.Debug(string(c.Ctx.Input.RequestBody))
	status , data := UnpackRequestParams(c.Ctx.Input.RequestBody)

	beego.Debug(status,"|", data)

	players := []map[string]string{}
	for _, s := range models.Servers{

		pls := s.Accounts[fmt.Sprintf("6=%s",data["accountId"])]
		for _ , p :=range pls {
			v := map[string]string{}
			v["serverId"] = strconv.Itoa(s.ServerId)
			v["serverName"] = s.ServerName
			v["roleId"] = strconv.Itoa(p.PlayerId)
			v["roleName"] = p.PlayerName
			v["roleLevel"] = strconv.Itoa(p.PlayerLevel)

			players = append(players,v)
		}
	}

	beego.Debug(players)

	object := map[string]interface{}{}
	object["roleInfos"] = players
	object["accountId"] = data["accountId"]


	result := PackReturnParams(status,object)

	beego.Debug(result)

	c.Ctx.WriteString(result)
}

func (c* SendNineGameGift) Post() {
	beego.Debug(string(c.Ctx.Input.RequestBody))
	status , data := UnpackRequestParams(c.Ctx.Input.RequestBody)

	beego.Debug(status,"|", data)

	serverId,_ := strconv.Atoi(data["serverId"].(string));
	playerId,_ := strconv.Atoi(data["roleId"].(string));

	object := map[string]interface{}{}

	server := models.GetServerById(serverId)

	if server == nil{
		object["result"] = "false"
		status.Code = 5000035
		status.Message = "区服信息错误"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}

	player := server.GetPlayer(fmt.Sprintf("6=%s",data["accountId"]),playerId)

	if player == nil{
		object["result"] = "false"
		status.Code = 5000034
		status.Message = "角色信息错误"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}

	gift := models.GetGift(data["kaId"].(string))
	if gift == nil{
		object["result"] = "false"
		status.Code = 5000032
		status.Message = "礼包Id错误"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}



	if models.IsGotGift(data["kaId"].(string),fmt.Sprintf("6=%s",data["accountId"]),serverId,playerId){
		object["result"] = "false"
		status.Code = 5000036
		status.Message = "已经领取过了"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}

	if !server.SendGift(fmt.Sprintf("6=%s",data["accountId"]),data["kaId"].(string),playerId){
		object["result"] = "false"
		status.Code = 5000000
		status.Message = "服务器内部错误"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}

	object["result"] = "true"


	result := PackReturnParams(status,object)

	beego.Debug(result)

	c.Ctx.WriteString(result)
}

func (c* GetGiftCount) Post() {
	beego.Debug(string(c.Ctx.Input.RequestBody))
	status , data := UnpackRequestParams(c.Ctx.Input.RequestBody)

	beego.Debug(status,"|", data)

	object := map[string]interface{}{}

	gift := models.GetGift(data["kaId"].(string))
	if gift == nil{
		object["result"] = "false"
		status.Code = 5000032
		status.Message = "礼包Id错误"
		result := PackReturnParams(status,object)
		beego.Debug(result)
		c.Ctx.WriteString(result)
		return
	}

	object["result"] = "true"

	result := PackReturnParams(status,object)

	beego.Debug(result)

	c.Ctx.WriteString(result)
}