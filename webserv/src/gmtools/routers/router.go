package routers

import (
	"gmtools/controllers"

	"github.com/astaxie/beego"
)

func init() {
	beego.Router("/", &controllers.Home{})
	beego.Router("/history", &controllers.History{})
	beego.Router("/data_DoIsSever", &controllers.DoIsSever{})
	beego.Router("/do_player_information", &controllers.DoPlayerInformation{})
	beego.Router("/do_grade_distribution", &controllers.DoGradeDistribution{})
	beego.Router("/do_user_retention", &controllers.DoUserRetention{})

	beego.Router("/alert", &controllers.Alert{})
	beego.Router("/error", &controllers.Error{})

	beego.Router("/query_area_servers.php", &controllers.Home2{})
	beego.Router("/change_server_notice.php", &controllers.DoServerNotice{})
	beego.Router("/change_system_notice.php", &controllers.DoSystemNotice{})
	beego.Router("/chang_roll_notice.php", &controllers.DoRollNotice{})
	beego.Router("/insert_mail.php", &controllers.DoInsertMail{})
	beego.Router("/contro_player.php", &controllers.DoContorlPlayer{})

	beego.Router("/login_activity.php", &controllers.DoLoginActivity{})
	beego.Router("/accumulate_recharge.php", &controllers.DoAccmulateRecharge{})
	beego.Router("/discount_store.php", &controllers.DoDiscountStore{})
	beego.Router("/single_recharge.php", &controllers.DoSingleRecharge{})
	beego.Router("/hotspot_partner.php", &controllers.DoHotspotPartner{})
	beego.Router("/extract_partner.php", &controllers.DoExtractPartner{})
	beego.Router("/money.php", &controllers.DoInsertMoney{})

	beego.Router("/query_history_chat.php", &controllers.DoQueryHistoryChat{})
	beego.Router("/query_now_chat.php", &controllers.DoQueryNowChat{})
	beego.Router("/query_player.php", &controllers.DoQueryPlayer{})
	beego.Router("/query_player_result.php", &controllers.QueryPlayerResult{})
	beego.Router("/query_chat_result.php", &controllers.QueryChatResult{})

	beego.Router("/history.php", &controllers.History{})
	beego.Router("/cdkey_generate.php", &controllers.DoCdkeyGenerate{})
	beego.Router("/cdkey_select.php", &controllers.DoCdkeySelect{})
	beego.Router("/cdkey_delete.php", &controllers.DoCdkeyDelete{})

	beego.Router("/order_inquiry.php", &controllers.DoOrderInquiry{})
	beego.Router("/pay_statistics.php", &controllers.DoPayStatistics{})

	beego.Router("/do_login.php", &controllers.DoLogin{})
	beego.Router("/do_regist.php", &controllers.Regist{})
	beego.Router("/do_admin.php", &controllers.DoAdmin{})
	beego.Router("/do_add.php", &controllers.DoAdd{})

	beego.Router("/do_simulator.php", &controllers.DoSimulator{})
	beego.Router("/do_execute.php", &controllers.DoExecute{})

	beego.Router("/do_password.php", &controllers.DoPassword{})
	beego.Router("/do_sever_execute.php", &controllers.DoSever{})

	beego.Router("/do_channel.php", &controllers.DoChannel{})
	beego.Router("/do_add_sever.php", &controllers.DoAddSever{})
	beego.Router("/do_up_sever.php", &controllers.DoUpSever{})
	beego.Router("/do_backups_sever.php", &controllers.DoBackupsSever{})
	beego.Router("/do_turntable.php", &controllers.DoTurntable{})
	beego.Router("/do_integral.php", &controllers.DoIntegral{})
	beego.Router("/do_channelsmapping.php", &controllers.DoChannelsMapping{})
	beego.Router("/queryNineMoney", &controllers.DoNineMoney{})

	beego.Router("/player/dau", &controllers.QueryLogServ{})
	beego.Router("/player/dnu", &controllers.DateQueryLog{})
	beego.Router("/account/dau", &controllers.AccountLogServ{})
	beego.Router("/account/dnu", &controllers.DateAccountLog{})

	beego.Router("/player/dau_date", &controllers.QueryLogServs{})
	beego.Router("/player/dnu_date", &controllers.DateQueryLogServs{})
	beego.Router("/account/dau_date", &controllers.AccountLogServs{})
	beego.Router("/account/dnu_date", &controllers.DateAccountLogServs{})
	beego.Router("/player/rrs", &controllers.PlayerRrs{})
	beego.Router("/channel_versions", &controllers.Do_Channel_Versions{})
	beego.Router("/versions_channel", &controllers.Do_Version_sChannel{})
	beego.Router("/add_versions_channel", &controllers.Add_Version_sChannel{})

	beego.Router("/queryRMB", &controllers.QueryRMB{})
	beego.Router("/queryDia", &controllers.QueryDia{})
	beego.Router("/queryMoney", &controllers.QueryMoney{})

}
