package main

import (
	"database/sql"
	"fmt"
	_ "github.com/go-sql-driver/mysql"
	"os"
	"time"
)

var (
	debugLog *os.File
)

func OpenDB(dsn string) *sql.DB {
	db, err := sql.Open("mysql", dsn)

	if err != nil {
		fmt.Println(err)
		return nil
	}
	return db
}

func cleanGO(from string) {
	db := OpenDB(from)
	if db == nil {
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM `Player`")
	defer records.Close()


	if err != nil {
		fmt.Println(err)
		return
	}

	debugLog, err = os.Create( fmt.Sprintf("%d.log",int(time.Now().Unix())))
	if err != nil {
		fmt.Println(err)
	}

	var instIds []int
	var instNames []string

	player_id, player_level, player_prof, player_grade, player_seal, player_freeze, player_server_id, player_money, player_diamond, player_magic, player_logouttime , versionNumber :=0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	user_name, player_name, player_bin := "", "", ""
	for records.Next() {
		err = records.Scan(&player_id, &user_name, &player_name, &player_level, &player_prof, &player_grade, &player_money, &player_diamond, &player_magic, &player_logouttime, &player_bin, &player_seal, &player_freeze, &player_server_id,&versionNumber)
		if err != nil {
			fmt.Println( err)
			continue
		}

		if IsGuildMember(db,player_name) {
			continue
		}

		tm := time.Unix(int64(player_logouttime), 0)
		cd := time.Now().Sub(tm)
		days := cd.Hours() / 24

		if days > 14 && player_level < 5 {
			//DELETE
			instNames = append(instNames,player_name)
			instIds = append(instIds,player_id)
			debugLog.WriteString(fmt.Sprintf("DELETE PLAYER NAME[%s]  LogoutTime[%d] Level[%d] Magic[%d]\n", player_name, int(player_logouttime), player_level, player_magic))
			continue
		}

		if player_magic < 1 && days > 30 && player_level < 30 {
			//DELETE
			instNames = append(instNames,player_name)
			instIds = append(instIds,player_id)
			debugLog.WriteString(fmt.Sprintf("DELETE PLAYER NAME[%s]  LogoutTime[%d] Level[%d] Magic[%d]\n", player_name, int(player_logouttime), player_level, player_magic))
			continue
		}

	}

	stmtfunc0 := func(params []string, sql string ) <- chan int64{
		rchan := make(chan int64)

		go func(){

			stmt, err := db.Prepare(sql)

			CheckErr(err)

			var allowed int64 = 0

			for _, i := range params{
				res , err := stmt.Exec(i)
				time.Sleep(2)
				CheckErr(err)
				allowed,_ = res.RowsAffected()
			}
			stmt.Close()
			rchan <- allowed
			close(rchan)
		}()

		return  rchan
	}

	stmtfunc1 := func(params []int, sql string ) <- chan int64{
		rchan := make(chan int64)

		go func(){

			stmt, err := db.Prepare(sql)

			CheckErr(err)

			var allowed int64 = 0

			for _, i := range params{
				res , err := stmt.Exec(i)
				time.Sleep(2)
				CheckErr(err)
				allowed,_ = res.RowsAffected()
			}
			stmt.Close()
			rchan <- allowed
			close(rchan)
		}()

		return  rchan
	}

	//干掉相关表中符合条件的角色

	<- stmtfunc0(instNames,"DELETE FROM `Player` WHERE `PlayerName`= ? ")
	<- stmtfunc0(instNames,"DELETE FROM `Baby` WHERE `OwnerName`= ? ")
	<- stmtfunc0(instNames,"DELETE FROM `Employee` WHERE `OwnerName`= ? ")
	<- stmtfunc0(instNames,"DELETE FROM `EndlessStair` WHERE `PlayerName`= ?  ")
	<- stmtfunc1(instIds,"DELETE FROM `EmployeeQuestTable` WHERE `PlayerId`= ?  ")
	<- stmtfunc1(instIds,"DELETE FROM `MallTable` WHERE `PlayerId`= ?   ")
	<- stmtfunc1(instIds,"DELETE FROM `MallSelledTable` WHERE `PlayerId`= ?  ")

	//for i := 0; i < len(instNames); i++ {
	//	CheckErr(err1)
	//	res,err1 := stmt.Exec(instNames[i] )
	//	CheckErr(err1)
	//	affect, err1 := res.RowsAffected()
	//	CheckErr(err1)
	//	fmt.Println(instNames[i],affect)
	//
	//	db.Exec("DELETE FROM `Baby` WHERE `OwnerName`= ? ", instNames[i])
	//	db.Exec("DELETE FROM `Employee` WHERE `OwnerName`= ? ", instNames[i])
	//	db.Exec("DELETE FROM `Mail` WHERE `RecvName`= ? ", instNames[i])
	//	db.Exec("DELETE FROM `EndlessStair` WHERE `PlayerName`= ? ", instNames[i])
	//
	//	fmt.Printf("DELETE TABLE PLAYER BABY EMPLOYEE MAIL ENDLESSSTAIR GUILDMEMBER BY %s ===NUM[%d] ===INDEX[%d]\n", instNames[i], len(instNames), i)
	//
	//}


	debugLog.Sync()
	debugLog.Close()

	fmt.Println("CLEAN END !!!! ")
}

func CheckErr(err error) {
	if err != nil {
		panic(err)
	}
}

func IsGuildMember(db *sql.DB,name string) bool {
	guild,guilderr := db.Query("SELECT * FROM `GuildMember` WHERE `RoleName`= ? ",name)
	defer guild.Close()
	if guilderr != nil {
		fmt.Println( guilderr)
		return true
	}

	if guild.Next() {
		return true
	}
	return false
}

func main() {

	conf, _ := NewConfigerFile("clean.conf")

	froms := conf.GetStrings("froms")

	for _, from := range froms {
		fmt.Println("Begin clean ", from)
		cleanGO(from)
	}
}
