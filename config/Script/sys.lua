
Debug = true

function sys_openmushroom()
	Sys.openmushroom(45);
	local str = "[FF0000]".."蘑菇活动开启，大批蘑菇在村庄中出现，采蘑菇可以获得大量金币！".."[-]"
	Sys.notice(str)
end
function sys_closemushroom()
	Sys.closemushroom();
end
function sys_refreshmushroom()
	Sys.refreshmushroom(45);
end
function sys_openxiji()
	Sys.openxiji(45);
end
function sys_refreshxiji()
	Sys.refreshxiji(45);
end
function sys_closexiji()
	Sys.closexiji();
end

--答题
function sys_openexam()
	Sys.openExam();
	local str = "[FF0000]".."答题活动已经开启，参加可以获得大量经验和金币！".."[-]"
	Sys.notice(str)
end

function sys_tishiopenexam()
	local str = "[FF0000]".."3分钟后答题活动开启，参加可以获得大量经验和金币！".."[-]"
	Sys.notice(str)
end

function sys_closeexam()
	Sys.closeExam();
	local str = "[FF0000]".."答题活动已经结束，请明天再来参加！".."[-]"
	Sys.notice(str)
end

function sys_reset_activity()
    Sys.reset_activity();
	--Sys.vipitem("系统","VIP每日礼包","恭喜您获得今日VIP礼包！");
end

function sys_pass_zero_hour()
	Sys.pass_zero_hour();
end

function sys_sendPVRrewardbyday()
	Sys.sendPVRrewardbyday("系统","竞技场每日奖励","恭喜您获得本日竞技场奖励，竞技场排名越靠前，奖励越丰厚");
end

function sys_sendPVRrewardbysenson()
	--Sys.sendPVRrewardbysenson("系统","竞技场每赛季奖励","恭喜您获得本赛季竞技场奖励，竞技场段位越高，奖励越丰厚");
end

function sys_sendPVPrewardbyday()
	--Sys.sendPVPrewardbyday("系统","竞技场每日奖励","恭喜您获得本日竞技场奖励，竞技场排名越靠前，奖励越丰厚");
end

function sys_sendPVPrewardbysenson()
	--Sys.sendPVPrewardbysenson("系统","竞技场每赛季奖励","恭喜您获得本赛季竞技场奖励，竞技场段位越高，奖励越丰厚");
end

function sys_sendPVRrewardbytime(RECEIVER,ARG0,ARG1)
	Sys.sendPVRrewardbytimes("系统","武斗场奖励","每次进行武斗场会获得金币奖励，请注意查收！",ARG0);
	if Sys.isfirstwin_pvr(ARG0) and ARG1 ~= 0 then
		Sys.firstwinreward_pvr("系统","武斗场首胜奖励","恭喜您获得今天武斗场首胜，请查收奖励！",ARG0,300034)
	end
end

--//script sys_openGuildBattle();
function sys_openGuildBattle()
	Sys.open_guild_battle();
	local str = "[FF0000]".."家族战即将开启，请大家通过家族驻地中的家族助理-丽丽进入战场".."[-]"
	Sys.prepare_guild_battle_timeout()
	Sys.notice(str)
end

function sys_openGuildBattle5min()
	local str = "[FF0000]".."家族战还有5分钟就要开始了，请大家通过家族驻地中的家族助理-丽丽进入战场".."[-]"
	Sys.prepare_guild_battle_timeout()
	Sys.notice(str)
end

--//script sys_startGuildBattle();
function sys_startGuildBattle()
	Sys.start_guild_battle();
	local str = "[FF0000]".."家族战正式开启！获胜条件：胜利场数大于对方或击败对方所有守护兽。为了家族的荣誉！".."[-]"
	Sys.notice(str)
end

--//script sys_stopGuildBattle();
function sys_stopGuildBattle()
	Sys.stop_guild_battle();
	local str = "[FF0000]".."本次家族战已经结束，10分钟后关闭战斗场景".."[-]"
	Sys.notice(str)
end

--//script sys_closeGuildBattle();
function sys_closeGuildBattle()
	Sys.close_guild_battle();
	local str = "[FF0000]".."本次家族战已经结束，奖励由邮件发放，请注意查收".."[-]"
	Sys.notice(str)
end


--//script sys_open_single_pk()
function sys_open_single_pk()
	Sys.openalonepk(45)
	local str = "[FF0000]".."野外宝箱活动开启，从罗德里镇的墓地中进入森林野外战场参加！".."[-]"
	Sys.notice(str)
end
function sys_refresh_single_pk()
	Sys.refreshalonepk(45)
end
function sys_close_single_pk()
	Sys.closealonepk()
end
--//script sys_open_team_pk()
function sys_open_team_pk()
	Sys.openteampk(45)
	
end
--//script sys_refresh_team_pk()
function sys_refresh_team_pk()
	Sys.refreshteampk(45)
end
function sys_close_team_pk()
	Sys.closeteampk()
end


function sys_openWarrior()
	Sys.openWarrior()
	local str = "[FF0000]".."勇者选拔战活动已经开启，请大家前往参加活动".."[-]"
end
function sys_closeWarrior()
	Sys.closeWarrior()
	local str = "[FF0000]".."勇者选拔战活动已经结束，请大家下次再接再厉".."[-]"
end
--//drop 发送邮件
function sys_send_mail_by_drop(sender,recver,title,content,drop)
	Sys.send_mail_drop(sender,recver,title,content,drop);
end
function sys_send_mail_by_drop_all(sender,title,content,drop)
	Sys.send_mail_all_drop(sender,title,content,drop);
end
--// 普通发送邮件 items 格式 "1001,10;1002,22;1003,33" 
function sys_send_mail(sender,recver,title,content,money,diamond,str_items)
	Sys.send_mail(sender,recver,title,content,money,diamond,str_items);
end
function sys_send_mail_all(sender,title,content,money,diamond,str_items)
	Sys.send_mail(sender,title,content,money,diamond,str_items);
end

--背包满了交任务
function sys_send_mail_quest(RECEIVER,ARG0)
	local name = Player.getplayerName(RECEIVER)
	sys_send_mail_by_drop("系统",name,"找回道具","由于您的背包已满，给您补发任务道具奖励",ARG0)
end


--//魔族入侵
--//script sys_open_guild_demon_invaded();
function sys_open_guild_demon_invaded()
    Sys.log("111111111111111111111111111111")
	Sys.add_npc(1100,70001);
	Sys.add_npc(1100,70002);
	Sys.add_npc(1100,70003);
	Sys.add_npc(1100,70004);
	Sys.add_npc(1100,70005);
	Sys.add_npc(1100,70006);
	Sys.add_npc(1100,70007);
	Sys.add_npc(1100,70008);
	Sys.add_npc(1100,70009);
	Sys.add_npc(1100,70010);
	Sys.add_npc(1100,70011);
	Sys.add_npc(1100,70012);
	Sys.add_npc(1100,70013);
	Sys.add_npc(1100,70014);
	Sys.add_npc(1100,70015);
	Sys.open_guild_demon_invaded();
end
--//script sys_close_guild_demon_invaded();
function sys_close_guild_demon_invaded()
	Sys.del_npc(1100,70001);
	Sys.del_npc(1100,70002);
	Sys.del_npc(1100,70003);
	Sys.del_npc(1100,70004);
	Sys.del_npc(1100,70005);
	Sys.del_npc(1100,70006);
	Sys.del_npc(1100,70007);
	Sys.del_npc(1100,70008);
	Sys.del_npc(1100,70009);
	Sys.del_npc(1100,70010);
	Sys.del_npc(1100,70011);
	Sys.del_npc(1100,70012);
	Sys.del_npc(1100,70013);
	Sys.del_npc(1100,70014);
	Sys.del_npc(1100,70015);
	Sys.close_guild_demon_invaded()
end
--//首领入侵
--//script sys_open_guild_leader_invaded();
function sys_open_guild_leader_invaded()
	Sys.add_npc(1100,70017);
	Sys.add_npc(1100,70018);
	Sys.add_npc(1100,70019);
	Sys.add_npc(1100,70020);
	Sys.open_guild_leader_invaded();
end
--//script sys_close_guild_leader_invaded();
function sys_close_guild_leader_invaded()
	Sys.del_npc(1100,70017);
	Sys.del_npc(1100,70018);
	Sys.del_npc(1100,70019);
	Sys.del_npc(1100,70020);
	Sys.close_guild_leader_invaded()
end

--宠物神殿
function sys_open_pet()
	Sys.openpet()
end

function sys_close_pet()
	Sys.closepet()
end

--红包
function sys_open_hongbao()
	Sys.allonlineplayeraddmoney(1000)
	local str = "[FF0000]".."天降红包活动开始，在线玩家每5分钟将获得金币红包奖励，请注意查收。".."[-]"
	Sys.notice(str)
	Sys.add_activation_counter_all(ACT_Money,1);
end

function sys_refresh_hongbao()
	Sys.allonlineplayeraddmoney(1000)
	Sys.add_activation_counter_all(ACT_Money,1);
end

function sys_close_hongbao()
	local str = "[FF0000]".."天降红包活动已经结束，每周一，周四晚上7点-8点开启天降红包活动。".."[-]"
	Sys.notice(str)
end

--//检测加入队伍
--//RECV 队长HANDLE
--//ARG0 加入者HANDLE
--//ARG1 队伍ID
function check_team_addmember(RECEIVER,ARG0,ARG1)
	return true
end




--刷新伙伴任务
function sys_refreshWhiteQuest()
	Sys.sys_refreshEmployeeQuest(EQC_White)
end
function sys_refreshBlueQuest()
	Sys.sys_refreshEmployeeQuest(EQC_Blue)
end
function sys_refreshPurpleQuest()
	Sys.sys_refreshEmployeeQuest(EQC_Purple)
end


--刷新活动npc
function sys_add_pronpc()
	local str = "[FF0000]".."职业大师活动开启，请去酒馆找全职高手，接受挑战吧！".."[-]"
	Sys.notice(str)
	Sys.add_npc(999,49);
end

function sys_del_pronpc()
	local str = "[FF0000]".."职业大师活动结束了！".."[-]"
	Sys.notice(str)
	Sys.del_npc(999,49);
end

--杀星
function sys_add_star()
	local str = "[FF0000]".."消灭精怪活动开启，请去村庄里寻找精怪的踪迹，并消灭他们吧！".."[-]"
	Sys.notice(str)
	local table2 = {67000,67001,67002,67003,67004,67005,67006,67007,67008,67009}
	local table20 = {67020,67021,67022,67023,67024}
	local table3 = {67010,67011,67025}
	local table30 = {67012,67013,67014,67015,67016,67018,67019,67026,67027,67028,67029}
	local index1 = math.ceil(math.random(1,10));
	local index2 = math.ceil(math.random(1,5));
	local index3 = math.ceil(math.random(1,3));
	local index4 = math.ceil(math.random(1,11));
	Sys.add_npc(2,table2[index1]);
	Sys.add_npc(20,table20[index2]);
	Sys.add_npc(3,table3[index3]);
	Sys.add_npc(30,table30[index4]);
end

function sys_del_star()
	local str = "[FF0000]".."经过大家的努力，精怪已经被打跑。村庄恢复了安宁！".."[-]"
	Sys.notice(str)
	local table2 = {67000,67001,67002,67003,67004,67005,67006,67007,67008,67009}
	local table20 = {67020,67021,67022,67023,67024}
	local table3 = {67010,67011,67025}
	local table30 = {67012,67013,67014,67015,67016,67018,67019,67026,67027,67028,67029}
	for a=1,table.getn(table2),1
		do
			Sys.del_npc(2,table2[a]);
		end
	for b=1,table.getn(table20),1
		do
			Sys.del_npc(20,table20[b]);
		end
	for c=1,table.getn(table3),1
		do
			Sys.del_npc(3,table3[c]);
		end
	for d=1,table.getn(table30),1
		do
			Sys.del_npc(30,table30[d]);
		end
end