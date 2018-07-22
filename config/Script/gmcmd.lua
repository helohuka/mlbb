Sys.log("gmcmd.lua");

Sys.add_gm_cmd("sub_prop",1,"sub_prop");		--添加宝宝
Sys.add_gm_cmd("reset_prop",0,"reset_prop");		--添加宝宝
Sys.add_gm_cmd("add_baby",1,"add_baby");		--添加宝宝
Sys.add_gm_cmd("del_baby",1,"del_baby");		--删除宝宝
Sys.add_gm_cmd("add_Employee",1,"add_Employee");		--添加伙伴
Sys.add_gm_cmd("add_ExpByBattleBaby",1,"add_ExpByBattleBaby"); --给出战宝宝加经验
Sys.add_gm_cmd("learn_skill",1,"learn_skill");		--学习技能
Sys.add_gm_cmd("add_item",2,"add_item");		--添加物品
Sys.add_gm_cmd("del_item",2,"del_item");		--删除物品
Sys.add_gm_cmd("add_Exp",1,"add_Exp");			--添加经验 
Sys.add_gm_cmd("add_skillExp",1,"add_skillExp");		--添加技能经验
Sys.add_gm_cmd("accept_quest",1,"accept_quest");	--接任务
Sys.add_gm_cmd("submit_quest",1,"submit_quest");	--递交任务
Sys.add_gm_cmd("add_diamond",1,"add_diamond");		--加钻石
Sys.add_gm_cmd("add_money",1,"add_money");			--加钱
Sys.add_gm_cmd("add_rmb",1,"add_rmb");				--加魔力币
Sys.add_gm_cmd("add_Reputation",1,"add_Reputation");  --加声望
Sys.add_gm_cmd("LevelUp",1,"LevelUp");				--角色升级
Sys.add_gm_cmd("setlevel",1,"setlevel");			--设置角色等级
Sys.add_gm_cmd("setJob",1,"setJob");				--设置角色职业
Sys.add_gm_cmd("BabyLevelUp",1,"BabyLevelUp");		--宠物升级
Sys.add_gm_cmd("set_HundredTier",1,"set_HundredTier");	--百人道场跳层
Sys.add_gm_cmd("complete_Achievement",1,"complete_Achievement");	--完成某成就
Sys.add_gm_cmd("complete_AllAchievement",1,"complete_AllAchievement");  --完成所有成就
Sys.add_gm_cmd("reset_counter",1,"reset_counter");			--重置所有挑战次数
Sys.add_gm_cmd("enterScene",1,"enterScene");		--进入某场景
Sys.add_gm_cmd("openall",0,"openall");				--开启所有功能
Sys.add_gm_cmd("set_glamour",1,"set_glamour");	--设置魅力值
Sys.add_gm_cmd("openmushroom",1,"openmushroom");
Sys.add_gm_cmd("closemushroom",0,"closemushroom");
Sys.add_gm_cmd("openpet",1,"openpet");
Sys.add_gm_cmd("closepet",1,"closepet");
Sys.add_gm_cmd("openxiji",1,"openxiji");
Sys.add_gm_cmd("closexiji",1,"closexiji");
Sys.add_gm_cmd("openwarrior",1,"openwarrior");			--勇者选拔
Sys.add_gm_cmd("closewarrior",1,"closewarrior");
Sys.add_gm_cmd("add_pvpjjcgrade",1,"add_pvpjjcgrade");	--加 战绩
Sys.add_gm_cmd("send_mail",4,"send_mail");		--发邮件
Sys.add_gm_cmd("set_prof",2,"set_prof"); --设置职业
Sys.add_gm_cmd("skip_guide",1,"skip_guide");	--跳过新手引导
Sys.add_gm_cmd("god",0,"god");	--可以变的很牛B
Sys.add_gm_cmd("tranfor_scene",2,"tranfor_scene");	--可以变的很牛B

Sys.add_gm_cmd("openvip",1,"openvip");	--跳过新手引导
Sys.add_gm_cmd("notice",2,"notice");	--可以变的很牛B
Sys.add_gm_cmd("addtitle",2,"addtitle");	--添加称号

Sys.add_gm_cmd("openscene",1,"openscene");		--添加宝宝

Sys.add_gm_cmd("openexam",1,"openexam");	--答题
Sys.add_gm_cmd("closeexam",1,"closeexam");

Sys.add_gm_cmd("opengb",1,"openscene1");		--帮派战
Sys.add_gm_cmd("startgb",1,"openscene2");
Sys.add_gm_cmd("stopgb",1,"openscene3");
Sys.add_gm_cmd("closegb",1,"openscene4");
Sys.add_gm_cmd("opendemon",1,"opendemon");		--魔族入侵
Sys.add_gm_cmd("closedemon",1,"closedemon");
Sys.add_gm_cmd("opengl",1,"opengl");			--首领入侵
Sys.add_gm_cmd("closegl",1,"closegl");

Sys.add_gm_cmd("openfe",1,"openfestival");		--开启运营活动
Sys.add_gm_cmd("closefe",1,"closefestival");
Sys.add_gm_cmd("opencard",1,"opencard");		
Sys.add_gm_cmd("closecard",1,"closecard");
Sys.add_gm_cmd("openrct",1,"openrechargeTotal");		
Sys.add_gm_cmd("closerct",1,"closerechargeTotal");
Sys.add_gm_cmd("openrcs",1,"openrechargeSingle");		
Sys.add_gm_cmd("closercs",1,"closerechargeSingle");
Sys.add_gm_cmd("opendis",1,"opendiscountStore");		
Sys.add_gm_cmd("closedis",1,"closediscountStore");
Sys.add_gm_cmd("openhotshop",1,"openhotshop");		
Sys.add_gm_cmd("closehotshop",1,"closehotshop");
Sys.add_gm_cmd("openemp",1,"openemployeeActivityTotal");		
Sys.add_gm_cmd("closeemp",1,"closeemployeeActivityTotal");
Sys.add_gm_cmd("openmb",1,"openmingiftbag");
Sys.add_gm_cmd("closemb",1,"closemingiftbag");

Sys.add_gm_cmd("testrole",1,"updateRoleLog");


Sys.add_gm_cmd("openalonepk",1,"openalonepk");
Sys.add_gm_cmd("refreshalonepk",1,"refreshalonepk");
Sys.add_gm_cmd("closealonepk",1,"closealonepk");
Sys.add_gm_cmd("openteampk",1,"openteampk");
Sys.add_gm_cmd("refreshteampk",1,"refreshteampk");
Sys.add_gm_cmd("closeteampk",1,"closeteampk");
Sys.add_gm_cmd("joinbattle",2,"joinbattle");
Sys.add_gm_cmd("joinbattle2",2,"joinbattle2");
Sys.add_gm_cmd("joinbattlez",2,"joinbattlez");


Sys.add_gm_cmd("addnpc",2,"addnpc");		--首领入侵
Sys.add_gm_cmd("delnpc",2,"delnpc");		--首领入侵

Sys.add_gm_cmd("add_pronpc",1,"add_pronpc");		--首领入侵
Sys.add_gm_cmd("del_pronpc",1,"del_pronpc");		--首领入侵
Sys.add_gm_cmd("add_star",1,"add_star");		--首领入侵
Sys.add_gm_cmd("del_star",1,"del_star");		--首领入侵


function addnpc(p,sceneId,npcId)
	Sys.add_npc(sceneId,npcId);
end
function delnpc(p,sceneId,npcId)
	Sys.del_npc(sceneId,npcId);
end

--活动
function add_pronpc(p)
	Sys.add_npc(999,49);
end

function del_pronpc(p)
	Sys.del_npc(999,49);
end

function add_star(p)
	sys_add_star()
end

function del_star(p)
	sys_del_star()
end


function updateRoleLog(p)
	Sys.updaterolerogtable();
end

function openvip(p,l,t)
	Player.openvip(p,l,t);
end
function openscene(p,s)
	Player.openscene(p,s);
end

function openscene1(p,s)
	sys_openGuildBattle();
end

function openscene2(p,s)
	sys_startGuildBattle();
end

function openscene3(p,s)
	sys_stopGuildBattle();
end

function openscene4(p,s)
	sys_closeGuildBattle();
end

function opendemon(p)
	sys_open_guild_demon_invaded();
end

function closedemon(p)
	sys_close_guild_demon_invaded();
end

function opengl(p)
	sys_open_guild_leader_invaded();
end

function closegl(p)
	sys_close_guild_leader_invaded();
end

function notice(p,c)
	Sys.notice(c);
end


function skip_guide(p)
	Player.set_guide_all(p,-1);
end

function set_prof(p,prof,pflv)
	local playerid = Player.getPlayerInstId(p);
	Player.change_property(p,playerid,PT_Profession,prof);
	Player.change_property(p,playerid,PT_ProfessionLevel,pflv);
end

function send_mail(p,mailtype,drop)   --mailtype  1:在线JJC天奖励,2:在线JJC季奖励,3:全服发邮件
	Sys.log("mailtype ===>"..mailtype.."drop====>"..drop);
	Sys.gm_mail(mailtype,drop);
end

function openmushroom(p)
	Sys.openmushroom(60);
end
function closemushroom(p)
	Sys.closemushroom();
end
function openxiji(p)
	Sys.openxiji(60);
end
function closexiji(p)
	Sys.closexiji();
end

function openexam(p)
	Sys.openExam();
end

function closeexam(p)
	Sys.closeExam();
end

function openwarrior(p)
	Sys.openWarrior();
end

function closewarrior(p)
	Sys.closeWarrior();
end

function openfestival(p, daytime)
	Sys.open_festival(daytime);
end

function closefestival(p)
	Sys.close_festival();
end

function opencard(p,daytime)
	Sys.open_card(daytime);
end

function closecard(p)
	Sys.close_card();
end

function openrechargeTotal(p,daytime)
	Sys.open_rechargeTotal(daytime);
end

function closerechargeTotal(p)
	Sys.close_rechargeTotal();
end

function openrechargeSingle(p,daytime)
	Sys.open_rechargeSingle(daytime);
end

function closerechargeSingle(p)
	Sys.close_rechargeSingle();
end

function opendiscountStore(p,daytime)
	Sys.open_discountStore(daytime);
end

function closediscountStore(p)
	Sys.close_discountStore();
end

function openhotshop(p,daytime)
	Sys.open_hotShop(daytime);
end

function closehotshop(p)
	Sys.close_hotShop();
end

function openemployeeActivityTotal(p,daytime)
	Sys.open_employeeActivityTotal(daytime);
end

function closeemployeeActivityTotal(p)
	Sys.close_employeeActivityTotal();
end

function openmingiftbag(p)
	Sys.open_mingiftbag();
end

function closemingiftbag(p)
	Sys.close_mingiftbag();
end

--/////////////////////////////////
function openall(p)
	Player.openMagic(p);
	Player.set_opensubsystem(p,OSSF_Skill);
	Player.set_opensubsystem(p,OSSF_Baby);
	Player.set_opensubsystem(p,OSSF_Friend);
	Player.set_opensubsystem(p,OSSF_EmployeeGet);
	Player.set_opensubsystem(p,OSSF_EmployeeList);
	Player.set_opensubsystem(p,OSSF_EmployeePosition);
	Player.set_opensubsystem(p,OSSF_EmployeeEquip);
	Player.set_opensubsystem(p,OSSF_Bar);
	Player.set_opensubsystem(p,OSSF_Castle);
	Player.set_opensubsystem(p,OSSF_JJC);
	Player.set_opensubsystem(p,OSSF_Make);
	Player.set_opensubsystem(p,OSSF_MagicItem);
	Player.set_opensubsystem(p,OSSF_Activity);
	Player.set_opensubsystem(p,OSSF_Achieve);
	Player.set_opensubsystem(p,OSSF_Team);
end;

function sub_prop(p,pt)
	Player.sub_property(p,pt);
end

function reset_prop(p)
	Player.reset_property(p);
end

function add_baby(p,id)
	Player.add_baby(p,id);
end

function del_baby(p,id)
	Player.del_baby(p,id);
end

function add_Employee(p,id)
	Player.add_Employee(p,id);
end

function learn_skill(p,id)
	Player.learn_skill(p,id);
end

function add_item(p,id,num)
	Player.add_item(p,id,num);
end

function del_item(p,id,num)
	Player.del_item(p,id,num);
end

function add_Exp(p,value)
	Player.add_Exp(p,value);
end

function accept_quest(p, value)
	Player.GMaccept_quest(p,value);
end

function submit_quest(p, value)
	Player.submit_quest(p,value);
end

function add_skillExp(p,skillId,value)
	Player.gm_add_skillExp(p,skillId,value);
end

function add_diamond(p,value)
	Player.add_diamond(p,value);
end

function add_money(p,value)
	Player.add_money(p,value);
end

function add_rmb(p,value)
	Player.add_rmb(p,value);
end

function add_ExpByBattleBaby(p,value)
	Player.add_ExpByBattleBaby(p,value);
end

function add_Reputation(p,value)
	Player.add_Reputation(p,value);
end

function LevelUp(p,value)
	Player.set_level(p,value);
end

function BabyLevelUp(p,value)
	Player.set_BabyLevel(p,value);
end

function set_HundredTier(p,value)
	Player.set_HundredTier(p,value);
end

function complete_Achievement(p,value)
	Player.complete_Achievement(p,value);
end

function complete_AllAchievement(p)
	Player.complete_AllAchievement(p);
end

function reset_counter(p)
	Player.reset_counter(p);
end

function enterScene(p,value)
	Player.enter_Scene(p,value);
end

function set_glamour(p,value)
	Player.set_glamour(p,value);
end

function add_pvpjjcgrade(p,value)
	Player.add_pvpjjcgrade(p,value);
end

function god(p)
	skip_guide(p);
	openall(p);
	add_item(p,1755,1);
	add_item(p,5037,3);
	add_item(p,5038,3);
	LevelUp(p,59);
	BabyLevelUp(p,59);
	add_money(p,900000);
	add_diamond(p,900000);
	Player.openscene(p,2);
	Player.openscene(p,3);
	Player.openscene(p,4);
	Player.openscene(p,5);
	Player.openscene(p,20);
	Player.openscene(p,30);
	Player.openscene(p,40);
	Player.openscene(p,402);
	Player.openscene(p,810);
end

function tranfor_scene(p,value)
	Sys.transfor_scene(p,value)
end

function openpet(p)
	Sys.openpet();
end

function closepet(p)
	Sys.closepet()
end

function setlevel(p, value)
	Player.setPlayerLevel(p,value);
end

function addtitle(p, value)
	Player.addplayertitle(p,value);
end

function setJob(p,value)
	Player.setPlayerJob(p,value);
end

function openalonepk(p,value)
	Sys.openalonepk(value);
end

function refreshalonepk(p,value)
	Sys.refreshalonepk(value);
end

function closealonepk(p)
	Sys.closealonepk();
end

function openteampk(p,value)
	Sys.openteampk(value);
end

function refreshteampk(p,value)
	Sys.refreshteampk(value);
end

function closeteampk(p)
	Sys.closeteampk();
end

function joinbattle(p,bid)
	Player.joinbattle(p,bid)
end 

function joinbattle2(p,bid)
	Player.joinbattle2(p,bid)
end 

function joinbattlez(p,zid)
	Player.joinbattlez(p,zid)
end 