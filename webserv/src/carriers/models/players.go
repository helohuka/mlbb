package models

type (
	Player struct {
		AccountName string
		PlayerName  string
		ServerName  string
		PlayerId    int
		PlayerLevel int
		ServerId    int
	}

	AccountController struct {
		players          []*Player
		account_name_map map[string][]*Player
		player_name_map  map[string][]*Player
	}
)

func (this *AccountController) Init() {

}

func (this *AccountController) AddPlayer(accountName, playerName, serverName string, playerId, playerLevel, serverId int) {

}

func (this *AccountController) DelPlayer() {

}
