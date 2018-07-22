package main

import (
	"database/sql"
	"fmt"

	_"github.com/go-sql-driver/mysql"
	"time"
)

func OpenDB(dsn string) *sql.DB {
	db, err := sql.Open("mysql", dsn)
	if err != nil {
		fmt.Println(err)
		return nil
	}
	return db
}

var max_entity_id = 0
var max_guild_id = 0
var max_mall_id = 0
var same_players map[string]int = map[string]int{}
var same_guilds map[string]int = map[string]int{}
var same_accounts map[string]bool = map[string]bool{}

func CalcSame(from string) {
	db := OpenDB(from)
	defer db.Close()

	records, err := db.Query("SELECT `PlayerName` FROM `Player`")

	if err != nil {
		fmt.Println(err)
		return
	}

	for records.Next() {
		name := ""
		records.Scan(&name)
		same_players[name] = same_players[name] + 1
	}

	records, _ = db.Query("SELECT `GuildName` FROM `Guild`")

	for records.Next() {
		name := ""
		records.Scan(&name)
		same_guilds[name] = same_guilds[name] + 1
	}
	records.Close()
}

func Merge(from, to, tag string) {
	player_id2id := map[int]int{}
	baby_id2id := map[int]int{}
	guild_id2id := map[int]int{}

	t_db := OpenDB(to)
	defer t_db.Close()
	f_db := OpenDB(from)
	defer f_db.Close()
	f_db.SetConnMaxLifetime(time.Second * 60)
	t_db.SetConnMaxLifetime(time.Second * 60)

	records, err := f_db.Query("SELECT * FROM `Account`")
	if err != nil {
		fmt.Println(err)
		return
	}
	acc_guild, acc_seal := 0, 0
	acc_name, acc_pass, acc_info, acc_phone := "", "", "", ""
	for records.Next() {
		err = records.Scan(&acc_guild, &acc_name, &acc_pass, &acc_info, &acc_seal, &acc_phone)
		if err != nil {
			fmt.Println(err)
		}

		if same_accounts[acc_name] {
			continue
		}
		same_accounts[acc_name] = true

		_ ,err = t_db.Exec("INSERT INTO `Account`(`UserName`,`Password`,`AccountInfo`,`Seal`,`PhoneNumber`)VALUES(?,?,?,?,?)", acc_name, acc_pass, acc_info, acc_seal, acc_phone)
		if err != nil {
			fmt.Println(err,acc_name)
		}
	}
	fmt.Println("------------------------------------------ACCOUNT OK------------------------------------------")
	records, err = f_db.Query("SELECT * FROM `Player`")
	if err != nil {
		fmt.Println(err )
		return
	}

	player_id, player_level, player_prof, player_grade, player_seal, player_freeze, player_server_id, player_money, player_diamond, player_magic, player_logouttime , versionNumber :=0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	user_name, player_name, player_bin := "", "", ""
	for records.Next() {
		err = records.Scan(&player_id, &user_name, &player_name, &player_level, &player_prof, &player_grade, &player_money, &player_diamond, &player_magic, &player_logouttime, &player_bin, &player_seal, &player_freeze, &player_server_id,&versionNumber)
		if err != nil {
			fmt.Println( err)
			continue
		}
		if user_name == "" {
			fmt.Println( "user_name == nil")
			continue
		}
		if same_players[player_name] > 1 {
			player_name += tag
		}
		old_player_id := player_id
		if max_entity_id >= player_id {
			player_id = max_entity_id + 1
		}
		max_entity_id = player_id
		_,err = t_db.Exec("INSERT INTO `Player`(`PlayerGuid`,`UserName`,`PlayerName`,`PlayerLevel`,`PlayerProfession`,`PlayerGrade`,`Money`,`Diamond`,`Magic`,`LogoutTime`,`BinData`,`Seal`, `Freeze`,`InDoorId`,`VersionNumber`)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", player_id, user_name, player_name, player_level, player_prof, player_grade, player_money, player_diamond, player_magic, player_logouttime, player_bin, player_seal, player_freeze, player_server_id,versionNumber)
		if err != nil{
			fmt.Println(player_id,player_name,user_name,err)
		}
		player_id2id[old_player_id] = player_id
	}
	fmt.Println("------------------------------------------PLAYER OK------------------------------------------")
	records, err = f_db.Query("SELECT * FROM `Baby`")
	if err != nil {
		fmt.Errorf("%s", err)
		return
	}
	baby_id, baby_guid, baby_grade, baby_level, baby_table, baby_prop := 0, 0, 0, 0, 0, 0
	baby_owner_name, baby_name, baby_bin := "", "", ""

	for records.Next() {
		err := records.Scan(&baby_id, &baby_guid, &baby_grade, &baby_owner_name, &baby_name, &baby_level, &baby_table, &baby_prop, &baby_bin)
		if err != nil {
			fmt.Errorf("%s", err)
			continue
		}
		if baby_owner_name == "" {
			continue
		}
		if same_players[baby_owner_name] > 1 {
			baby_owner_name += tag
		}
		old_baby_id := baby_guid
		if max_entity_id >= baby_guid {
			baby_guid = max_entity_id + 1

		}
		max_entity_id = baby_guid
		_,err = t_db.Exec("INSERT INTO `Baby`(`BabyGuid`,`BabyGrade`,`OwnerName`,`BabyName`,`BabyLevel`,`TableID`,`AddProp`,`BinData`)VALUES(?,?,?,?,?,?,?,?)", baby_guid, baby_grade, baby_owner_name, baby_name, baby_level, baby_table, baby_prop, baby_bin)
		if err != nil{
			fmt.Println(baby_guid,baby_table,baby_owner_name)
		}

		baby_id2id[old_baby_id] = baby_guid
	}
	fmt.Println("------------------------------------------BABY OK------------------------------------------")
	records, err = f_db.Query("SELECT * FROM `Employee`")
	if err != nil {
		fmt.Errorf("%s", err)
		return
	}
	employee_id, employee_guid, employee_grade := 0, 0, 0
	employee_owner_name, employee_name, employee_bin := "", "", ""

	for records.Next() {
		err := records.Scan(&employee_id, &employee_guid, &employee_grade, &employee_owner_name, &employee_name, &employee_bin)
		if err != nil {
			fmt.Errorf("%s", err)
			continue
		}
		if employee_owner_name == "" {
			continue
		}
		if same_players[employee_owner_name] > 1 {
			employee_owner_name += tag
		}
		if max_entity_id >= employee_guid {
			employee_guid = max_entity_id + 1

		}
		max_entity_id = employee_guid
		_, err = t_db.Exec("INSERT INTO `Employee`(`EmployeeGuid`,`EmployeeGrade`,`OwnerName`,`EmployeeName`,`BinData`)VALUES(?,?,?,?,?)", employee_guid, employee_grade, employee_owner_name, employee_name, employee_bin)
		if err != nil{
			fmt.Println(employee_guid,employee_id,employee_owner_name)
		}
	}
	fmt.Println("------------------------------------------EMPLOYEE OK------------------------------------------")
	records, _ = f_db.Query("SELECT * FROM `Mail`")

	mail_recv_name, mail_bin := "", ""
	mail_guild, mail_send_time, mail_item_num := 0, 0, 0

	for records.Next() {
		err := records.Scan(&mail_guild, &mail_recv_name, &mail_send_time, &mail_item_num, &mail_bin)
		if err != nil {
			fmt.Errorf("%s", err)
			continue
		}
		if mail_recv_name == "" {
			continue
		}
		if same_players[mail_recv_name] > 1 {
			mail_recv_name += tag
		}

		_ , err  = t_db.Exec("INSERT INTO `Mail`(`RecvName`,`SendTime`,`ItemNum`,`BinData`)VALUES(?,?,?,?)", mail_recv_name, mail_send_time, mail_item_num, mail_bin)
		if err != nil{
			fmt.Println(mail_recv_name,mail_send_time,mail_item_num)
		}
	}
	fmt.Println("------------------------------------------MAIL OK------------------------------------------")
	records, _ = f_db.Query("SELECT * FROM `Guild`")

	guild_id, guild_level, guild_contri, guild_fundz, guild_credit, guild_master_id, pNum := 0, 0, 0, 0, 0, 0,0
	guild_name, guild_master_name, guild_notice, guild_request, guild_buildings, guild_progenitus, guild_progenitu_positions := "", "", "", "", "", "", ""

	for records.Next() {
		err := records.Scan(&guild_id, &guild_name, &guild_level, &guild_contri, &guild_fundz, &guild_credit, &guild_master_id, &guild_master_name, &guild_notice, &guild_request, &guild_buildings, &guild_progenitus, &guild_progenitu_positions,&pNum)
		if err != nil {
			fmt.Println(err)
			continue
		}

		if same_guilds[guild_name] > 1 {
			guild_name = guild_name + tag
		}

		if same_players[guild_master_name] > 1 {
			guild_master_name = guild_master_name + tag
		}
		guild_master_id = player_id2id[guild_master_id]
		old_guild_id := guild_id
		if max_guild_id >= guild_id {
			guild_id = max_guild_id + 1
		}
		max_guild_id = guild_id

		_, err = t_db.Exec("INSERT INTO `Guild`(`GuildId`,`GuildName`,`GuildLevel`,`Contribution`,`Fundz`,`Credit`,`Master`,`MasterName`,`Notice`,`RequestList`,`Buildings`,`Progenitus`,`ProgenitusPos`,`PresentNum`)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", guild_id, guild_name, guild_level, guild_contri, guild_fundz, guild_credit, guild_master_id, guild_master_name, guild_notice, guild_request, guild_buildings, guild_progenitus, guild_progenitu_positions,pNum)
		if err != nil {
			fmt.Println(guild_id,guild_name)
		}
		guild_id2id[old_guild_id] = guild_id
	}
	fmt.Println("------------------------------------------GUILD OK------------------------------------------")
	records, _ = f_db.Query("SELECT * FROM `GuildMember`")

	member_guild_id, member_role_id, member_job, member_contri, member_level, member_join_time, member_prof, member_prof_level, member_offline := 0, 0, 0, 0, 0, 0, 0, 0, 0
	member_role_name := ""

	for records.Next() {
		err := records.Scan(&member_guild_id, &member_role_id, &member_job, &member_contri, &member_level, &member_join_time, &member_prof, &member_prof_level, &member_offline, &member_role_name)
		if err != nil {
			fmt.Println(err)
			continue
		}
		member_guild_id = guild_id2id[member_guild_id]
		member_role_id = player_id2id[member_role_id]
		if same_players[member_role_name] > 1 {
			member_role_name = member_role_name + tag
		}

		if member_role_id == 0{
			fmt.Println(member_guild_id,member_role_id,member_role_name)
			continue
		}

		_, err = t_db.Exec("INSERT INTO `GuildMember`(`GuildId`,`RoleId`,`Job`,`Contribution`,`Rolelevel`,`JoinTime`,`Proftype`,`Proflevel`,`OfflineTime`,`RoleName`)VALUES(?,?,?,?,?,?,?,?,?,?)", member_guild_id, member_role_id, member_job, member_contri, member_level, member_join_time, member_prof, member_prof_level, member_offline, member_role_name)
		if err != nil {
			fmt.Println(member_guild_id,member_role_id,member_role_name)
		}
	}
	fmt.Println("------------------------------------------GUILDMEMBER OK------------------------------------------")
	records, _ = f_db.Query("SELECT * FROM `MallTable`")

	mall_key, mall_guild, mall_player_id, mall_sell_price, mall_item_sub_type, mall_race_type, mall_item_id, mall_baby_id := 0, 0, 0, 0, 0, 0, 0, 0
	mall_title, mall_sell_item := "", ""
	for records.Next() {
		err := records.Scan(&mall_key, &mall_guild, &mall_player_id, &mall_sell_price, &mall_title, &mall_item_sub_type, &mall_race_type, &mall_item_id, &mall_baby_id, &mall_sell_item)
		if err != nil {
			fmt.Println(err)
			continue
		}
		if max_mall_id >= mall_guild {
			mall_guild = max_mall_id + 1
		}
		max_mall_id = mall_guild
		mall_player_id = player_id2id[mall_player_id]
		mall_baby_id = baby_id2id[mall_baby_id]
		if mall_player_id == 0{
			fmt.Println(mall_player_id,mall_guild,mall_title)
			continue
		}
		_, err = t_db.Exec("INSERT INTO `MallTable`(`Guid`,`PlayerId`,`SellPrice`,`Title`,`ItemSubType`,`RaceType`,`ItemId`,`BabyId`,`SellItem`)VALUES(?,?,?,?,?,?,?,?,?)", mall_guild, mall_player_id, mall_sell_price, mall_title, mall_item_sub_type, mall_race_type, mall_item_id, mall_baby_id, mall_sell_item)
		if err != nil {
			fmt.Println(mall_player_id,mall_guild,mall_title)
		}

	}
	fmt.Println("------------------------------------------MALLTABLE OK------------------------------------------")
}

func main() {

	conf, _ := NewConfigerFile("merge.conf")

	froms := conf.GetStrings("froms")
	tags := conf.GetStrings("tags")
	to := conf.GetString("to")
	fmt.Println(froms)
	for _, from := range froms {
		CalcSame(from)
	}

	for i, from := range froms {
		fmt.Println("Begin Merge ", from)
		Merge(from, to, tags[i])
	}

}
