--addAchuevementValue  是增加
--setAchuevementValue	是赋值
function Achievement_Money(RECEIVER,ARG0)
	if ARG0 == 0 then
		return;
	end

	local value = math.abs(ARG0);
	
	if ARG0 < 0 then
		Player.addAchuevementValue(RECEIVER,AT_SpendMoney,value);
	else
		Player.addAchuevementValue(RECEIVER,AT_EarnConis,value);
	end
	--[[身上拥有的钱
	local money = Player.get_property(RECEIVER,PT_Money)
	if money >= 100000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,100000); 
	end
	if money >= 1000000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,1000000); 
	end
	if money >= 1500000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,1500000); 
	end
	if money >= 3000000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,3000000); 
	end
	if money >= 5000000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,5000000); 
	end
	if money >= 10000000 then
		Player.setAchuevementValue(RECEIVER,AT_OwnConis,10000000); 
	end]]--
end

function Achievement_Diamond(RECEIVER,ARG0)
	if ARG0 == 0 then
		return;
	end

	local value = math.abs(ARG0);
	
	if ARG0 < 0 then
		Player.addAchuevementValue(RECEIVER,AT_SpendDiamond,value);
	else
		Player.addAchuevementValue(RECEIVER,AT_Recharge,value);
	end
end

function Achievement_PlayerLevelUp(RECEIVER,ARG0)
	Player.setAchuevementValue(RECEIVER,AT_RoleLevel,ARG0);
end

function Achievement_BabyLevelUp(RECEIVER,ARG0,ARG1,ARG2)
	Player.setAchuevementValue(RECEIVER,AT_PetLevel,ARG1);
end

function Achievement_ChangeProp(RECEIVER)
	local attackValue = Player.get_property(RECEIVER,PT_Attack);			--攻、防、敏
	local defenseValue = Player.get_property(RECEIVER,PT_Defense);
	local agileValue = Player.get_property(RECEIVER,PT_Agile);
	
	Player.setAchuevementValue(RECEIVER,AT_AttackLevel,attackValue);
	Player.setAchuevementValue(RECEIVER,AT_DefenseLevel,defenseValue);
	Player.setAchuevementValue(RECEIVER,AT_AgileLevel,agileValue);
end

function Achievement_WearEquip(RECEIVER,ARG0)
	if ARG0 >= 1347 and ARG0 <= 1350 then
		Player.setAchuevementValue(RECEIVER,AT_WearCrystal,1);
	end
	
	local ornament0 = Player.get_EquipItem(RECEIVER,ES_Ornament_0);
	local ornament1 = Player.get_EquipItem(RECEIVER,ES_Ornament_1);
	
	if ornament0 ~= 0 and ornament1 ~= 0 then
		Player.setAchuevementValue(RECEIVER,AT_WearAccessories,1);
	end
end

function Achievement_BattleChangeProp(RECEIVER,ARG0)
	local value = math.abs(ARG0);
	if ARG0 < 0 then
		Player.addAchuevementValue(RECEIVER,AT_TotalDamage,value);
	else
		Player.addAchuevementValue(RECEIVER,AT_TotalTreatment,value);
	end
end

function Achievement_LearnSkill(RECEIVER,ARG0)
	Player.addAchuevementValue(RECEIVER,AT_HasSkillNum,1);
end

function Achievement_BabyLearnSkill(RECEIVER,ARG0)
	Player.setAchuevementValue(RECEIVER,AT_BabySkill,ARG0);
end

function Achievement_CatchBaby(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_CatchPet,1);
end
--伙伴数量
function Achievement_PlayerRecruitEmp(RECEIVER,ARG0,ARG1,ARG2)
	Player.addAchuevementValue(RECEIVER,AT_PartnerCard,1);
end
--伙伴进阶
function Achievement_PlayerEmpGrade(RECEIVER,ARG0)
	if ARG0 == QC_Green then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradeGreen,1);
	elseif ARG0 == QC_Blue then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradeBlue,1);
	elseif ARG0 == QC_Purple then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradePurple,1);
	elseif ARG0 == QC_Golden then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradeGold,1);
	elseif ARG0 == QC_Orange then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradeOrangle,1);
	elseif ARG0 == QC_Pink then
		Player.setAchuevementValue(RECEIVER,AT_PartnersUpgradePink,1);
	end
end
--离线竞技场
function Achievement_PlayerJJCWin(RECEIVER,ARG0,ARG1)
	if ARG1 ~= 0 then
		Player.addAchuevementValue(RECEIVER,AT_ArenaWin,1);
	end
end
--在线竞技场
function Achievement_PlayerPVP(RECEIVER,ARG0)
		Activity.update(RECEIVER,ACT_Warrior,1);
end


--好友
function Achievement_PlayerFriend(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_Friend,1);
end

--击杀
function Achievement_PlayerKill(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	if ARG3 ==  ET_Player then
		Player.addAchuevementValue(RECEIVER,AT_KillPlayer,1);
	elseif ARG3 ==  ET_Monster then
		Player.addAchuevementValue(RECEIVER,AT_KillMonster,1);
	end
end

--采集
function Achievement_PlayerGather(RECEIVER,ARG0,ARG1)
	Player.addAchuevementValue(RECEIVER,AT_CollectMaterial,ARG1);
end

--打造装备
function Achievement_MakeEquip(RECEIVER,ARG0,ARG1)
	Player.addAchuevementValue(RECEIVER,AT_MakeEquipment,1);
	if ARG1 == 1 then
		Player.addAchuevementValue(RECEIVER,AT_GoodMake,1);
	end
end
--登陆
function Achievement_Sign(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_Sign,1);
end

--日常
function Achievement_richang(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_EverydayActivities,1);
end

--通缉
function Achievement_tongji(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_Wanted,1);
end

--宠物强化
function Achievement_Babyintensify(RECEIVER,ARG0)
	Player.setAchuevementValue(RECEIVER,AT_PetIntensive,ARG0);
end

--仓库
function Achievement_Bag(RECEIVER,ARG0,ARG1)
	if ARG0 == ST_Item then
		Player.setAchuevementValue(RECEIVER,AT_Bag,ARG1/20-1);
	elseif ARG0 == ST_Baby then
		Player.setAchuevementValue(RECEIVER,AT_PetBag,ARG1/6-1);
	end
end

--宠物
function Achievement_AddBaby(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = Player.get_hasbaby_race(RECEIVER,ARG4)
	if ARG4 == RT_Human then
		Player.setAchuevementValue(RECEIVER,AT_PetHuman,num);
	elseif ARG4 == RT_Insect then
		Player.setAchuevementValue(RECEIVER,AT_PetInsect,num);
	elseif ARG4 == RT_Plant then
		Player.setAchuevementValue(RECEIVER,AT_PetPlant,num);
	elseif ARG4 == RT_Extra then
		Player.setAchuevementValue(RECEIVER,AT_PetExtra,num);
	elseif ARG4 == RT_Dragon then
		Player.setAchuevementValue(RECEIVER,AT_PetDragon,num);
	elseif ARG4 == RT_Animal then
		Player.setAchuevementValue(RECEIVER,AT_PetAnimal,num);
	elseif ARG4 == RT_Fly then
		Player.setAchuevementValue(RECEIVER,AT_PetFly,num);
	elseif ARG4 == RT_Undead then
		Player.setAchuevementValue(RECEIVER,AT_PetUndead,num);
	elseif ARG4 == RT_Metal then
		Player.setAchuevementValue(RECEIVER,AT_PetMetal,num);
	end
end

--家族
function Achievement_Guild(RECEIVER)
	Player.setAchuevementValue(RECEIVER,AT_Home,1);
end

--成就
function Achievement_achive(RECEIVER)
	Player.addAchuevementValue(RECEIVER,AT_Reward50,1);
end

--神器
function Achievement_shenqishengji(RECEIVER,ARG0)
	Player.setAchuevementValue(RECEIVER,AT_MagicEquip,ARG0);
end

--符文
function Achievement_fuwendengji(RECEIVER,ARG0)
	if ARG0 >= 20 then
		Player.setAchuevementValue(RECEIVER,AT_RunesLevel,ARG0);
	end
end