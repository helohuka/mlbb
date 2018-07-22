local gameLib = require "GameLib.cs"

-- step0
gameLib.RegistEvent(SGE_EnterMainScene, "EnterMainScene");
gameLib.RegistEvent(SGE_WorldMapOpen, "WorldMapOpen");
gameLib.RegistEvent(SGE_EnterScene, "EnterScene");
gameLib.RegistEvent(SGE_NpcDialogBegin, "NpcDialogBegin");
gameLib.RegistEvent(SGE_NpcRenwuUIOpen, "NpcRenwuUIOpen");
gameLib.RegistEvent(SGE_NpcRenwuPreAccept, "NpcRenwuPreAccept");
gameLib.RegistEvent(SGE_EnterNPCBattle, "EnterNPCBattle");



-- step1
gameLib.RegistEvent(SGE_BeforeEnterBattle, "BeforeEnterBattle");
gameLib.RegistEvent(SGE_EnterBattle, "EnterBattle");
gameLib.RegistEvent(SGE_BattleTurn, "BattleTurn");
gameLib.RegistEvent(SGE_ClickBattleSkill, "SelectSkill");
gameLib.RegistEvent(SGE_SelectSkillLevel, "SelectSkillLevel");
gameLib.RegistEvent(SGE_SelectTarget, "SelectTarget");
gameLib.RegistEvent(SGE_BabySelectSkill, "BabySelectSkill");
gameLib.RegistEvent(SGE_ClickBattleAuto, "ClickBattleAuto");
gameLib.RegistEvent(SGE_SelectTargetOk, "SelectTargetOk");



-- step2
gameLib.RegistEvent(SGE_BattleOverRewardOpen, "BattleOverRewardOpen");
gameLib.RegistEvent(SGE_BattleOverRewardClose, "BattleOverRewardClose");

-- step3
gameLib.RegistEvent(SGE_MainPanelOpen, "MainPanelOpen");
gameLib.RegistEvent(SGE_ClickMainBag, "ClickMainBag");
gameLib.RegistEvent(SGE_BagTipOpen, "BagTipOpen");
gameLib.RegistEvent(SGE_UseItem, "UseItem");
gameLib.RegistEvent(SGE_EquipItem, "EquipItem");
gameLib.RegistEvent(SGE_GainItem, "GainItem");
gameLib.RegistEvent(SGE_NpcRenwuAccept, "NpcRenwuAccept");
gameLib.RegistEvent(SGE_NpcRenwuSubmit, "NpcRenwuSubmit");
gameLib.RegistEvent(SGE_BagItemDoubleClick, "BagItemDoubleClick");



-- step7
gameLib.RegistEvent(SGE_PlayerLevelUp, "PlayerLevelUp");
gameLib.RegistEvent(SGE_PlayerUIOpen, "PlayerUIOpen");
gameLib.RegistEvent(SGE_PlayerUIPropertySwitch, "PlayerUIPropertySwitch");
gameLib.RegistEvent(SGE_PlayerUIAddPoint, "PlayerUIAddPoint");
gameLib.RegistEvent(SGE_PlayerUIPropertyConfirmClick, "PlayerUIPropertyConfirmClick");

-- step9
gameLib.RegistEvent(SGE_BabyLevelUp, "BabyLevelUp");
gameLib.RegistEvent(SGE_BabyUIOpen, "BabyUIOpen");
gameLib.RegistEvent(SGE_BabyUIPropertySwitch, "BabyUIPropertySwitch");
gameLib.RegistEvent(SGE_BabyUIAddPoint, "BabyUIAddPoint");
gameLib.RegistEvent(SGE_BabyUIPropertyConfirmClick, "BabyUIPropertyConfirmClick");
gameLib.RegistEvent(SGE_BabyUIClose, "BabyUIClose");

-- step12
gameLib.RegistEvent(SGE_ClickMainPartner, "ClickMainPartner");
gameLib.RegistEvent(SGE_PartnerShowUI, "PartnerShowUI");
gameLib.RegistEvent(SGE_PartnerHideUI, "PartnerHideUI");
gameLib.RegistEvent(SGE_PartnerPositionTabShow, "PartnerPositionTabShow");
gameLib.RegistEvent(SGE_PartnerListTabShow, "PartnerListTabShow");
gameLib.RegistEvent(SGE_PartnerForBattle, "PartnerForBattle");
gameLib.RegistEvent(SGE_ClickMiniQuest, "ClickMiniQuest");
gameLib.RegistEvent(SGE_MainLearningUI, "MainLearningUI");
--gameLib.RegistEvent(SGE_MainLearningClickTab, "MainLearningClickTab");
gameLib.RegistEvent(SGE_MainLearningOneSkillClick, "MainLearningOneSkillClick");
gameLib.RegistEvent(SGE_MainLearningSkillOk, "MainLearningSkillOk");
gameLib.RegistEvent(SGE_MessageBoxOpen, "MessageBoxOpen");


-- step16 宠物技能
gameLib.RegistEvent(SGE_BabyLearningSkillUI, "BabyLearningSkillUI");
gameLib.RegistEvent(SGE_ClickBabyLearningSkill, "ClickBabyLearningSkill");
gameLib.RegistEvent(SGE_BabyLearningSkill_BabyListUI, "BabyLearningSkill_BabyListUI");
gameLib.RegistEvent(SGE_BabyLearningSkill_BabySkillUI, "BabyLearningSkill_BabySkillUI");
gameLib.RegistEvent(SGE_BabyLearningSkillOk, "BabyLearningSkillOk");


-- step19 打造
gameLib.RegistEvent(SGE_MainMakeUIOpen, "MainMakeUIOpen");
gameLib.RegistEvent(SGE_MainMakeSubDetail, "MainMakeSubDetail");
gameLib.RegistEvent(SGE_MainMakeItemOk, "MainMakeItemOk");
gameLib.RegistEvent(SGE_MainMakeSub, "MainMakeSub");
gameLib.RegistEvent(SGE_MainMakeGemUI, "MainMakeGemUI");
gameLib.RegistEvent(SGE_MainMakeGemOk, "MainMakeGemOk");
gameLib.RegistEvent(SGE_MainMakeGemUIClose, "MainMakeGemUIClose");


-- step19 组队
gameLib.RegistEvent(SGE_TeamUIOpen, "TeamUIOpen");
gameLib.RegistEvent(SGE_TeamUISelectMapOpen, "TeamUISelectMapOpen");
gameLib.RegistEvent(SGE_MainTeamUI, "MainTeamUI");

-- step27 神器
gameLib.RegistEvent(SGE_MainMagicUIOpen, "MainMagicUIOpen");
gameLib.RegistEvent(SGE_MainMagicFirstClick, "MainMagicFirstClick");
gameLib.RegistEvent(SGE_MainMagicLevelUp, "MainMagicLevelUp");
gameLib.RegistEvent(SGE_MainMagicTipClose, "MainMagicTipClose");


--28 伙伴装备
gameLib.RegistEvent(SGE_PartnerDetailUIOpen, "PartnerDetailUIOpen");
gameLib.RegistEvent(SGE_PartnerDetailEquipClick, "PartnerDetailEquipClick");
gameLib.RegistEvent(SGE_PartnerDetailEquipSucc, "PartnerDetailEquipSucc");


--29、30伙伴技能
gameLib.RegistEvent(SGE_PartnerDetailBaseOpen, "PartnerDetailBaseOpen");
gameLib.RegistEvent(SGE_PartnerDetailBaseSkillOpen, "PartnerDetailBaseSkillOpen");
gameLib.RegistEvent(SGE_ParnterDetailBaseSkillLvUpSucc, "ParnterDetailBaseSkillLvUpSucc");

--33竞技场
gameLib.RegistEvent(SGE_JJCEntryUI, "JJCEntryUI");
gameLib.RegistEvent(SGE_OfflineJJCUI, "OfflineJJCUI");


--成就35
gameLib.RegistEvent(SGE_AchievementUIOpen, "AchievementUIOpen");
gameLib.RegistEvent(SGE_AchievementReceived, "AchievementReceived");


--小地图36 37
gameLib.RegistEvent(SGE_MiniMapOpen, "MiniMapOpen");
gameLib.RegistEvent(SGE_WorldMapToWorld, "WorldMapToWorld");
gameLib.RegistEvent(SGE_MainTaskFlushOk, "MainTaskFlushOk");


--好友38
gameLib.RegistEvent(SGE_ClickMainFriend, "ClickMainFriend");
gameLib.RegistEvent(SGE_ClickAddFriendBtn, "ClickAddFriendBtn");


--家族39
gameLib.RegistEvent(SGE_ClickMainFamily, "ClickMainFamily");


--符文45
gameLib.RegistEvent(SGE_BagFuwenOpen, "BagFuwenOpen");
gameLib.RegistEvent(SGE_BagFuwenCombieUI, "BagFuwenCombieUI");
gameLib.RegistEvent(SGE_BagFuwenCombieSuccess, "BagFuwenCombieSuccess");
gameLib.RegistEvent(SGE_FuwenUIClose, "FuwenUIClose");
gameLib.RegistEvent(SGE_BagFuwenClickTipsInsertBtn, "BagFuwenClickTipsInsertBtn");


--自动战斗
gameLib.RegistEvent(SGE_PlayerAutoOrder, "PlayerAutoOrder");
gameLib.RegistEvent(SGE_OpenAutoPanel, "OpenAutoPanel");


--提升
gameLib.RegistEvent(SGE_ClickRaiseUpBtn, "ClickRaiseUpBtn");



--剧情相关
gameLib.RegistEvent(SGE_ExitSense, "ExitSense");

--[[function WorldMapOpen(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(5) == false then
		gameLib.CreateGuide(GAT_WorldMapER,0,0.5,GPRT_None,"我应该先根据克里斯王的线索，前往伊利斯村。",0.7,true)
		gameLib.FinishGuide(5);
	end
end]]--

function EnterScene(GAMEEVENT,ARG0)
	if ARG0 == 1000 then
		local nofinish = not gameLib.IsQuestFinished(90000)
		if  nofinish and gameLib.GuideIsFinish(41) == false then
			gameLib.PlaySense(0);
			gameLib.FinishGuide(41);
		end
	end
	if gameLib.GuideIsFinish(20) == false and ARG0 == 2 then
		gameLib.CreateGuideInScene(1,5,200,GPRT_None,"这个小姑娘怎么啦？让我过去看看吧。",0.7,true);
		--gameLib.BeginTalk(3018);
		--gameLib.PlaySense(2);
	end
	if ARG0 == 1000 then
		local questfinish = gameLib.IsQuestFinished(90001)
		if questfinish == true and gameLib.GuideIsFinish(42) == false then
			gameLib.ShowGainBaby(1);
			gameLib.FinishGuide(41);
		end
	end
end

function NpcDialogBegin(GAMEEVENT, talkId)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(20) == false and talkId == 5000 then
		gameLib.CreateGuide(GAT_DialogUI,0,0.32,GPRT_None,"不要多说话了，快点吧。");
		gameLib.FinishGuide(15);
	end
	if gameLib.GuideIsFinish(20) == false and talkId == 3013 then
		gameLib.CreateGuide(GAT_DialogUI,0,0.32,GPRT_None,"不要多说话了，快点吧。");
	end
end

function NpcRenwuUIOpen(GAMEEVENT,ARG0)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(20) == false then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_FirstQuest,0,0.1,GPRT_None,"选择任务",0.7,true);
	end
	--[[
	if gameLib.GuideIsFinish(20) == false and ARG0 == 90000 then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_FirstQuest,0,0.1,GPRT_None,"接受任务吧。",0.7,true);
	end
	if gameLib.GuideIsFinish(20) == false and ARG0 == 90001 then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_FirstQuest,0,0.1,GPRT_None,"接受任务吧。",0.7,true);
	end]]--
end

function NpcRenwuPreAccept(GAMEEVENT, talkId)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(20) == false and talkId == 1 then
		gameLib.CreateGuide(GAT_DialogUI,0,0.32,GPRT_None,"要继续，快点吧。");
	end
	if gameLib.GuideIsFinish(16) == false and talkId == 5002 then
		gameLib.CreateGuide(GAT_DialogUI,0,0.32,GPRT_None,"还说废话！");
	end
end

function EnterNPCBattle(GAMEEVENT,ARG0)
	gameLib.ClearGuide();
	gameLib.FinishGuide(20);
	gameLib.FinishGuide(24);
end

--[[   step 1  --]]
function BeforeEnterBattle(GAMEEVENT)
	if gameLib.GetBattleID() == 8 and gameLib.GuideIsFinish(11) == false then
		gameLib.SetAutoBattle(false);
	end
end


function EnterBattle(GAMEEVENT)
	if gameLib.GetBattleID() == 8 and gameLib.GuideIsFinish(11) == false then
		gameLib.SetAutoBattle(false);
		gameLib.CreateGuide(GAT_BattleCatch,-0,0.2,GPRT_None,"捕捉这个怪物吧！",0.7,true);
	end
	if gameLib.GuideIsFinish(1) == false and gameLib.GetBattleID() == 1 then
		gameLib.CreateGuide(GAT_BattleAttack,0,0.2,GPRT_None,"现在就是使用攻击的时候了。",0.7,true);
	end
	if gameLib.GetBattleID() == 2 then
		gameLib.FinishGuide(48);
	end
end

function BattleTurn(GAMEEVENT, ARG0)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(1) == false then
		if( ARG0 == 2) and gameLib.GetBattleID() == 1 then
			gameLib.CreateGuide(GAT_BattleSkill,0,0.2,GPRT_None,"战斗不能用蛮力，技能招式还是非常重要的。",0.7,true);
		elseif(ARG0 == 3) and gameLib.GetBattleID() == 1 then
			gameLib.CreateGuide(GAT_BattleAuto,0,0.23,GPRT_None,"如何能轻松一点呢？\n自动吧",0.7,true);
		end
	end
end

function SelectSkill(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(1) == false and gameLib.GetBattleID() == 1 then
		gameLib.CreateGuide(GAT_FirstSkill,-0.1,0.2,GPRT_None,"我能感受到这个技能招式的强大。",0.7,true);
	end
end

function SelectSkillLevel(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(1) == false and gameLib.GetBattleID() == 1 then
		gameLib.CreateGuide(GAT_FirstLevelSkill,0,0.12,GPRT_None,"只有最初等级1级吗？",0.7,true);
	end
end

function SelectTarget(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(1) == false and gameLib.GetBattleID() == 1 then
		gameLib.CreateGuideInBattle(BP_Up2,0,200,GPRT_None,"选取目标吧",0.7,true);
	end
	if gameLib.GetBattleID() == 8 and gameLib.GuideIsFinish(11) == false then
		gameLib.CreateGuideInBattle(BP_Up2,0,200,GPRT_None,"选取目标吧",0.7,true);
	end
end

function BabySelectSkill(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(22) == false and gameLib.GuideIsFinish(1) == false and gameLib.GetBattleID() == 1 then
		gameLib.CreateGuide(GAT_FirstSkill,0,0.2,GPRT_None,"让宠物也进行攻击吧。",0.7,true);
		gameLib.FinishGuide(22);
	end
end

function ClickBattleAuto()
	if gameLib.GuideIsFinish(1) == false then
		gameLib.ClearGuide();
		gameLib.FinishGuide(1);
	end
end

function SelectTargetOk(GAMEEVENT)
	gameLib.ClearGuide();
end


--[[   step 2  --]]
function BattleOverRewardOpen(GAMEEVENT)
	gameLib.CreateGuide(GAT_MainRaise,0,-0.2,GPRT_R180,"去提升一下自己的战斗能力吧！",0.7,true);
	--if gameLib.GuideIsFinish(2) == false then
		--gameLib.CreateGuide(GAT_BattleRewardClose,0,0.13,GPRT_None,"现在能体验胜利的快感了。");
	--end
end

function BattleOverRewardClose(GAMEEVENT)
	if gameLib.GuideIsFinish(2) == false then
		gameLib.ClearGuide();
		gameLib.FinishGuide(2);
	end
	if gameLib.GuideIsFinish(12) == false then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_QuestMiniFirst,0,-0.2,GPRT_R180,"点击这个去做任务",0.7,true);
		gameLib.FinishGuide(12);
	end
end
function ClickMiniQuest(GAMEEVENT)
	gameLib.ClearGuide();
end

--[[   step 3  --]]
function MainPanelOpen(GAMEEVENT)

end

function ClickMainBag(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(2) == true and gameLib.GuideIsFinish(3) == false and gameLib.GuideIsFinish(6) == true then
		gameLib.CreateGuideWithItemID(5033,0,0.3,GPRT_None,"这个礼包模样的东西究竟是什么呢？",0.7,true);
	end
	--符文
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false then
		gameLib.CreateGuide(GAT_MainBagFuwenTab,0,-0.2,GPRT_R180,"进入符文页面",0.7,true,45);
	end
end

function BagTipOpen(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(2) == true and gameLib.GuideIsFinish(3) == false and gameLib.GuideIsFinish(6) == true then
		gameLib.CreateGuide(GAT_MainBagTipUseItem,0,0.33,GPRT_None,"使用一次吧。",0.7,true,3);
	elseif gameLib.GuideIsFinish(1) == true and gameLib.GuideIsFinish(2) == true and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.CreateGuide(GAT_MainbagTipEquip,0,0.14,GPRT_None,"穿上这个试试吧。",0.7,true,4);
	end
	--符文
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false and gameLib.GuideIsFinish(47) == false then
		gameLib.CreateGuide(GAT_MainBagFuwenTipsCombieBtn,0,0.14,GPRT_None,"合成高级的看看",0.7,true,45);
	end
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false and gameLib.GuideIsFinish(47) == true then
		gameLib.CreateGuide(GAT_MainBagFuwenTipsInsertBtn,0,0.14,GPRT_None,"镶嵌上去",0.7,true,45);
	end
end

function UseItem(GAMEEVENT,ARG0)
	gameLib.ClearGuide();
	if ARG0 == 5033 then
		gameLib.FinishGuide(3);
	end
end

function GainItem(GAMEEVENT,ARG0)
	if ARG0 == 1023 and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.ClearGuide();
		gameLib.CreateGuideWithItemID(1023,0,0.3,GPRT_None,"装备竟然如此多，让我一个个的都穿上吧。",0.7,true,4);
	elseif ARG0 == 1069 and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.ClearGuide();
		gameLib.CreateGuideWithItemID(1069,0,0.3,GPRT_None,"装备竟然如此多，让我一个个的都穿上吧。",0.7,true,4);
	elseif ARG0 == 1092 and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.ClearGuide();
		gameLib.CreateGuideWithItemID(1092,0,0.3,GPRT_None,"装备竟然如此多，让我一个个的都穿上吧。",0.7,true,4);
	end
end

function NpcRenwuAccept(GAMEEVENT,ARG0)
	if ARG0 == 2 and gameLib.GuideIsFinish(4) == false then
		--gameLib.FinishGuide(2);
		--gameLib.DisplayBottomBtns();
		--gameLib.CreateGuide(GAT_MainBag,0,0.2,GPRT_None,"打开背包，让我们看看获得了什么好东西吧。",0.7,true);
		--gameLib.FinishGuide(6);
	end
	if ARG0 == 7 then
		--gameLib.PlaySense(3);
	end

	if ARG0 == 14 and gameLib.GuideIsFinish(14) == false then
		gameLib.DisplayBottomBtns();
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_MainPartner,0,0.19,GPRT_None,"可以招募伙伴了",0.7,true);
	end
	if ARG0 == 8 and gameLib.GuideIsFinish(16) == false then
		gameLib.ClearGuide();
		--gameLib.CreateGuideInScene(3,25,240,GPRT_None,"这个人长得好奇怪",0.7,true);
		gameLib.FinishGuide(18);
	end
	if ARG0 == 17 and gameLib.GuideIsFinish(28) == false then
		gameLib.DisplayBottomBtns();
		gameLib.FinishGuide(25);
		gameLib.CreateGuide(GAT_MainPartner,0,0.2,GPRT_None,"去提升一下伙伴吧！",0.7,true);
	end
	if ARG0 == 12 and gameLib.GuideIsFinish(19) == false then
		gameLib.DisplayBottomBtns();
		gameLib.CreateGuide(GAT_MainMake,0,0.2,GPRT_None,"10级了，该换一把新武器了！",0.7,true);
	end
	if ARG0 == 19 and gameLib.GuideIsFinish(30) == false then
		gameLib.DisplayBottomBtns();
		gameLib.FinishGuide(29);
		gameLib.CreateGuide(GAT_MainPartner,0,0.2,GPRT_None,"去给伙伴技能升级吧！",0.7,true);
	end
	if ARG0 == 39 and gameLib.GuideIsFinish(26) == false then
		gameLib.DisplayBottomBtns();
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_MainTeamBtn,-0.05,0.13,GPRT_None,"这个怪物有点凶，去找个帮手吧",0.7,true);
	end
	if gameLib.GuideIsFinish(21) == false then
		if ARG0 == 10000 or ARG0 == 10001 or ARG0 == 10002 or ARG0 == 10003 then
			gameLib.CreateGuide(GAT_QuestMiniSecond,0,-0.4,GPRT_R180,"先去就职吧",0.7,true);
		end
	end
	if ARG0 == 90000 then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_QuestMiniFirst,0,-0.2,GPRT_R180,"点击这个去做任务",0.7,true);
	end
	if ARG0 == 90001 then
		gameLib.ClearGuide();
		gameLib.CreateGuide(GAT_QuestMiniFirst,0,-0.2,GPRT_R180,"点击这个去做任务",0.7,true);
	end
	if ARG0 == 90007 then
		--gameLib.PlaySense(1);
	end
	if ARG0 == 90008 then
		gameLib.CreateGuide(GAT_MiniMap,0,-0.2,GPRT_R180,"打开地图",0.7,true);
		gameLib.FinishGuide(37);
	end
end

function MainTeamUI(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(26);
end


function NpcRenwuSubmit(GAMEEVENT,ARG0)
	if ARG0 == 1 then
		gameLib.FinishGuide(1);
		gameLib.FinishGuide(7);
		gameLib.FinishGuide(10);
		gameLib.FinishGuide(24);
	end
	if ARG0 == 90001 then
		gameLib.HideRenwuList()
		gameLib.FinishGuide(24);
		gameLib.PlaySense(1);
	end
	if ARG0 == 90008 then
		gameLib.CreateGuideInScene(9562,25,200,GPRT_None,"学技能！",0.7,true);
	end
	if ARG0 == 10000 or ARG0 == 10001 or ARG0 == 10002 or ARG0 == 10003 then
		gameLib.FinishGuide(21);
	end
	if ARG0 == 26 then
		gameLib.FinishGuide(33);
	end
end


function EquipItem(GAMEEVENT)
	if gameLib.GuideIsFinish(1) == true and gameLib.GuideIsFinish(2) == true and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.ClearGuide();
		gameLib.FinishGuide(4);
	end
end

function BagItemDoubleClick(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(1) == true and gameLib.GuideIsFinish(2) == true and gameLib.GuideIsFinish(3) == true and gameLib.GuideIsFinish(4) == false then
		gameLib.ClearGuide();
		gameLib.FinishGuide(4);
	end
	if ARG0 == 5033 then
		gameLib.ClearGuide();
		gameLib.FinishGuide(3);
	end
end


--家族39
function MainPanelOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(39) == false and gameLib.PlayerLevel() >= 20 then
		gameLib.DisplayBottomBtns();
		gameLib.CreateGuide(GAT_MainFamily,0,0.16,GPRT_None,"家族开启了，可以去寻找志同道合的小伙伴了",0.7,true);
	end
end


--[[   step 7，8  --]]
function PlayerLevelUp(GAMEEVENT,ARG0)
	--gameLib.FinishGuide(8);
	if gameLib.GuideIsFinish(7) == false and ARG0 == 21 then
		gameLib.CreateGuide(GAT_MainPlayerInfo,0.08,-0.3,GPRT_R180,"这样的感觉......这就是传说中的升级吗？",0.7,true);
		gameLib.FinishGuide(8);
	end
	if ARG0 == 2 and  gameLib.GuideIsFinish(35) == false then
		--点击成就按钮，打开成就界面，然后领取第一个
		--gameLib.CreateGuide(GAT_MainAchievement,0,-0.18,GPRT_R180,"获得了一个成就");
	end
	if gameLib.GuideIsFinish(38) == false and ARG0 == 15 then
		gameLib.CreateGuide(GAT_MainFriend,0,0.2,GPRT_None,"可以结交新的朋友了");
	end
	if ARG0 == 3 and gameLib.GuideIsFinish(4) == false then
		--gameLib.FinishGuide(2);
		--gameLib.DisplayBottomBtns();
		--gameLib.CreateGuide(GAT_MainBag,0,0.2,GPRT_None,"打开背包，让我们看看获得了什么好东西吧。",0.7,true);
		--gameLib.FinishGuide(6);
	end
	--符文
	if ARG0 == 30 and gameLib.GuideIsFinish(45) == false then
		gameLib.DisplayBottomBtns();
		gameLib.CreateGuide(GAT_MainBag,0,0.2,GPRT_None,"获得了一些符文，去看看他们是干什么用的吧。",0.7,true);
		gameLib.FinishGuide(46);
	end
end

function PlayerUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(7) == false and gameLib.GuideIsFinish(8) == true then
		gameLib.CreateGuide(GAT_MainPlayerInfoPropertyBtn,0,0.2,GPRT_None,"升级后会获得属性点，让我们去看看究竟是怎样的吧。");
	end
end

function PlayerUIPropertySwitch(GAMEEVENT)
	if gameLib.GuideIsFinish(7) == false and gameLib.GuideIsFinish(8) == true then
		gameLib.CreateGuide(GAT_MainPlayerInfoPropertyContainer,0,0.65,GPRT_None,"现在有职业了，合理的分配你的初始技能点吧！");
	end
end

function PlayerUIAddPoint(GAMEEVENT)
	if gameLib.GuideIsFinish(7) == false and gameLib.GuideIsFinish(8) == true then
		gameLib.CreateGuide(GAT_MainPlayerInfoPropertyConfirm,0,0.2,GPRT_None,"属性点配置完毕就点确定吧。");
	end
end

function PlayerUIPropertyConfirmClick(GAMEEVENT)
	if gameLib.GuideIsFinish(7) == false and gameLib.GuideIsFinish(8) == true then
		gameLib.CreateGuide(GAT_MainPlayerInfoClose,0.3,0,GPRT_R270,"关掉这个吧。");
		gameLib.FinishGuide(7);
	end
end

--[[   step 9,10   --]]
function BabyLevelUp(GAMEEVENT)
	gameLib.CloseAllSubUI();
	gameLib.FinishGuide(9);
	if gameLib.GuideIsFinish(10) == false and gameLib.GuideIsFinish(7) == true then
		gameLib.DisplayBottomBtns();
		gameLib.CreateGuide(GAT_MainBaby,0,0.6,GPRT_R180,"宠物成功升级。",0.7,true);
	end
end

function BabyUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(10) == false and gameLib.GuideIsFinish(9) == true and gameLib.GuideIsFinish(7) == true then
		gameLib.CreateGuide(GAT_MainBabyPropertyBtn,0.01,0.2,GPRT_None,"查看一下现在的属性变化吧。");
	end
end

function BabyUIPropertySwitch(GAMEEVENT)
	if gameLib.GuideIsFinish(10) == false and gameLib.GuideIsFinish(9) == true and gameLib.GuideIsFinish(7) == true then
		gameLib.CreateGuide(GAT_MainBabyPropertyContainer,0.6,0.1,GPRT_R270,"宠物确实变强了，并且还得到了属性点。");
	end
end

function BabyUIAddPoint(GAMEEVENT)
	if gameLib.GuideIsFinish(10) == false and gameLib.GuideIsFinish(9) == true and gameLib.GuideIsFinish(7) == true then
		gameLib.CreateGuide(GAT_MainBabyPropertyConfirm,0,0.18,GPRT_None,"属性点配置完毕就点确定吧。");
	end
end

function BabyUIPropertyConfirmClick(GAMEEVENT)
	if gameLib.GuideIsFinish(10) == false and gameLib.GuideIsFinish(9) == true and gameLib.GuideIsFinish(7) == true then
		gameLib.CreateGuide(GAT_MainBabyClose,0.3,0,GPRT_R270,"关掉这个吧。");
		gameLib.FinishGuide(10);
	end
end

function BabyUIClose(GAMEEVENT)
	gameLib.ClearGuide();
end


--[[   step 14   --]]
function ClickMainPartner(GAMEEVENT)
	if gameLib.GuideIsFinish(14) == false and gameLib.GuideIsFinish(43) == false then
		gameLib.CreateGuide(GAT_FreeLootPartner,0,0.1,GPRT_None,"可以免费抽一次",0.7,true);
		gameLib.FinishGuide(43);
	end
end

function PartnerShowUI(GAMEEVENT)
	if gameLib.GuideIsFinish(14) == false then
		gameLib.CreateGuide(GAT_PartnerShowCancel,0,0.18,GPRT_None,"确定！",0.7,true);
	end
end

function PartnerHideUI(GAMEEVENT)
	if gameLib.GuideIsFinish(14) == false then
		gameLib.CreateGuide(GAT_PartnerPositionTab,0,0.18,GPRT_None,"上阵！",0.7,true);
	end
end

function PartnerPositionTabShow(GAMEEVENT)
	if gameLib.GuideIsFinish(14) == false then
		gameLib.CreateGuide(GAT_FirstPartner_PosUI,0,0.3,GPRT_None,"点击这个伙伴就能让他出战了！",0.7,true);
	end
end
function PartnerForBattle(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(14) == false then
		gameLib.FinishGuide(14);
	end
end


--[[   step 15  学技能 --]]
function EnterMainScene(GAMEEVENT)
--[[
	if gameLib.GuideIsFinish(5) == false then
		gameLib.CreateGuideInMainScene(GAT_MainCrystal,0,150,GPRT_None,"看来这里就是聂拉维尔王城出口，就从这里进入战斗区域吧。")
	end
	if gameLib.GuideIsFinish(15) == false and gameLib.GuideIsFinish(17) == true  then
		gameLib.CreateGuideInMainScene(GAT_MainJiubaHouse,0,130,GPRT_None,"技能学院~");
	end
	if gameLib.GuideIsFinish(34) == false and gameLib.GuideIsFinish(33) == true  then
		gameLib.CreateGuideInMainScene(GAT_MainJJC,0,250,GPRT_None,"好宏伟的竞技场呀！");
	end
	]]--
end

function MainLearningUI(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(15) == false then
		if ARG0 == JT_Axe then
			gameLib.CreateGuideWithSkillID(1061,0,0.2,GPRT_None,"学这个吧！",0.7,true,15);
		elseif ARG0 == JT_Archer then
			gameLib.CreateGuideWithSkillID(1041,0,0.2,GPRT_None,"学这个吧！",0.7,true,15);
		elseif ARG0 == JT_Mage then
			gameLib.CreateGuideWithSkillID(2001,0,0.2,GPRT_None,"学这个吧！",0.7,true,15);
		elseif ARG0 == JT_Sage then
			gameLib.CreateGuideWithSkillID(2361,0,0.2,GPRT_None,"学这个吧！",0.7,true,15);
		else
			return
		end
	end
end

function MainLearningClickTab(GAMEEVENT,ARG0)
	gameLib.ClearGuide();
end

function MainLearningOneSkillClick(GAMEEVENT)                                        --玩家学习第一个技能的时候，进行的锁屏指引
	if gameLib.GuideIsFinish(15) == false then
		gameLib.CreateGuide(GAT_LearnSkillBtn,0,0.14,GPRT_None,"学这个吧！",0.7,true);
	end
end

function MessageBoxOpen(GAMEEVENT)
	--[[--主角学技能
	if gameLib.GuideIsFinish(17) == true and gameLib.GuideIsFinish(15) == false then
		gameLib.CreateGuide(GAT_MessageBoxOkBtn,0,0.14,GPRT_None,"确定");
	end
	--宠物学技能
	if gameLib.GuideIsFinish(18) == true and gameLib.GuideIsFinish(16) == false then
		--gameLib.CreateGuide(GAT_MessageBoxOkBtn,0,0.14,GPRT_None,"确定");
		gameLib.ClearGuide();
		gameLib.FinishGuide(16);
	end
	--神器
	if gameLib.GuideIsFinish(27) == false and gameLib.GuideIsFinish(31) == true then
		--gameLib.CreateGuide(GAT_MessageBoxOkBtn,0,0.14,GPRT_None,"确定");
		gameLib.ClearGuide();
		gameLib.FinishGuide(27);
	end
	--打造
	if gameLib.GuideIsFinish(19) == false and gameLib.GuideIsFinish(32) == true then
		--gameLib.CreateGuide(GAT_MessageBoxOkBtn,0,0.14,GPRT_None,"确定");
		gameLib.ClearGuide();
		gameLib.FinishGuide(19);
	end
	--回城
	if gameLib.GuideIsFinish(34) == false and gameLib.GuideIsFinish(33) == true then
		gameLib.CreateGuide(GAT_MessageBoxOkBtn,0,0.14,GPRT_None,"确定");
	end]]--
end


function MainLearningSkillOk(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(15) == false then
		gameLib.FinishGuide(15);
	end
end


--[[   step 15  宠物学技能 --]]
function BabyLearningSkillUI(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(16) == false then
		gameLib.CreateGuide(GAT_FirstLearningBabySkill,0,0.2,GPRT_None,"选择一个技能",0.7,true);
	end
end

function ClickBabyLearningSkill(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(16) == false and gameLib.GuideIsFinish(44) == false then
		gameLib.CreateGuide(GAT_BabySkillLearningBtn,0,0.14,GPRT_None,"确定学习！",0.7,true);
		gameLib.FinishGuide(44);
	end
end

function BabyLearningSkill_BabyListUI(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(16) == false then
		gameLib.CreateGuide(GAT_FirstLearningBabySkill_BabyList,0,0.3,GPRT_None,"选择一个宠物",0.7,true);
	end
end

function BabyLearningSkill_BabySkillUI(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(16) == false then
		gameLib.CreateGuide(GAT_ThirdLearningBabySkill_SkillSlot,0.33,0,GPRT_None,"放在这个空白的位置吧！",0.7,true);
		gameLib.FinishGuide(16);
	end
end

function BabyLearningSkillOk(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(16) == false then
		gameLib.FinishGuide(16);
	end
end




--打造
function MainMakeUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(19) == false then
		gameLib.FinishGuide(32);
		gameLib.CreateGuide(GAT_MainMakeLevel10,0.25,-0.25,GPRT_None,"打造一把10级的武器吧！",0.7,true,19);
	end
end

function MainMakeSub(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(19) == false then
		if ARG0 == JT_Axe then
			gameLib.CreateGuide(GAT_MainMakeSubFirst,0,0.14,GPRT_None,"打造一把斧子",0.7,true,19);
		elseif ARG0 == JT_Mage then
			gameLib.CreateGuide(GAT_MainMakeSubFirst,0,0.14,GPRT_None,"打造一把杖",0.7,true,19);
		elseif ARG0 == JT_Sage then
			gameLib.CreateGuide(GAT_MainMakeSubFirst,0,0.14,GPRT_None,"打造一把杖",0.7,true,19);
		elseif ARG0 == JT_Archer then
			gameLib.CreateGuide(GAT_MainMakeSubFirst,0,0.14,GPRT_None,"打造一把弓",0.7,true,19);
		else
			gameLib.ClearGuide();
			gameLib.FinishGuide(19);
		end
	end
end

function MainMakeGemUI(GAMEEVENT)
	if gameLib.GuideIsFinish(19) == false then
		gameLib.CreateGuide(GAT_MainMakeGemFirst,0,0.14,GPRT_None,"选择这颗宝石！",0.7,true,19);
	end
end

function MainMakeGemOk(GAMEEVENT)
	if gameLib.GuideIsFinish(19) == false then
		gameLib.CreateGuide(GAT_MainMakeCompoundBtn,0,0.14,GPRT_None,"打造",0.7,true,19);
	end
end


function MainMakeSubDetail(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(19) == false then
		if ARG0 == 1 then
			gameLib.CreateGuide(GAT_MainMakeGemBtn,0,0.14,GPRT_None,"打造时加入一颗宝石会增强装备属性！",0.7,true,19);
		else
			gameLib.FinishGuide(19);
			gameLib.ClearGuide();
		end
	end
end

function MainMakeItemOk(GAMEEVENT)
	gameLib.FinishGuide(19);
	gameLib.ClearGuide();
end

--神器
function MainMagicTipClose(GAMEEVENT)
	if gameLib.GuideIsFinish(27) == false then
		gameLib.DisplayBottomBtns();
		gameLib.CreateGuide(GAT_MainMagic,0,0.2,GPRT_None,"神器好像很厉害！");
	end
end

function MainMagicUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(27) == false then
		gameLib.CreateGuide(GAT_MainMagicLevelFirst,0,0.14,GPRT_None,"用装备来给神器升级",0.7,true,27);
	end
end
function MainMagicFirstClick(GAMEEVENT)
	if gameLib.GuideIsFinish(27) == false then
		gameLib.FinishGuide(31);
		gameLib.CreateGuide(GAT_MainMagicLevelBtn,0,0.14,GPRT_None,"升级吧！",0.7,true,27);
	end
end
function MainMagicLevelUp(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(27);
end

function PartnerListTabShow(GAMEEVENT)
	if gameLib.GuideIsFinish(28) == false and gameLib.GuideIsFinish(25) == true then
		gameLib.CreateGuide(GAT_FirstPartner_PosUI,0,0.28,GPRT_None,"选择伙伴！");
	end
	if gameLib.GuideIsFinish(30) == false and gameLib.GuideIsFinish(29) == true then
		gameLib.CreateGuide(GAT_FirstPartner_PosUI,0,0.28,GPRT_None,"选择伙伴！");
	end
end

--伙伴装备
function PartnerDetailUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(28) == false and gameLib.GuideIsFinish(25) == true then
		gameLib.CreateGuide(GAT_PartnerDetailBodySlot,0,0.18,GPRT_None,"选择一个装备位置",0.7,true,28);
	end
	if gameLib.GuideIsFinish(30) == false and gameLib.GuideIsFinish(29) == true then
		gameLib.CreateGuide(GAT_PartnerDetailBaseFirstSkill,0,0.18,GPRT_None,"选择一个技能");
	end
end

function PartnerDetailEquipClick(GAMEEVENT,ARG0)
	if ARG0 == 1 then
		if gameLib.GuideIsFinish(28) == false and gameLib.GuideIsFinish(25) == true then
			gameLib.CreateGuide(GAT_PartnerDetailEquipBtn,0,0.18,GPRT_None,"装备！");
		end
	else
		gameLib.ClearGuide();
		gameLib.FinishGuide(28);
	end
end

function PartnerDetailEquipSucc(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(28);
end

--伙伴技能
function PartnerDetailBaseOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(30) == false and gameLib.GuideIsFinish(29) == true then
		--gameLib.CreateGuide(GAT_PartnerDetailBaseFirstSkill,0,0.18,GPRT_None,"选择一个技能");
	end
end

function PartnerDetailBaseSkillOpen(GAMEEVENT,ARG0)
	if ARG0 == 0 then
		gameLib.ClearGuide();
		gameLib.FinishGuide(30);
		return
	end
	if gameLib.GuideIsFinish(30) == false and gameLib.GuideIsFinish(29) == true then
		gameLib.CreateGuide(GAT_PartnerDetailBaseSkillLvUpBtn,0,0.18,GPRT_None,"升级！");
	end
end

function ParnterDetailBaseSkillLvUpSucc(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(30);
end

--竞技场
function JJCEntryUI(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(34) == false and gameLib.GuideIsFinish(33) == true then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.CreateGuide(GAT_OfflineJJC,0,0.18,GPRT_None,"迫不及待的要进去试试自己的实力！");
		else
			gameLib.FinishGuide(34);
		end
	end
end

function OfflineJJCUI(GAMEEVENT)
	gameLib.ClearGuide();
	if gameLib.GuideIsFinish(34) == false and gameLib.GuideIsFinish(33) == true then
		gameLib.CreateGuide(GAT_OfflineJJC4,0,0.18,GPRT_None,"先选一个实力弱点的吧！",0.7,true);
		gameLib.FinishGuide(34);
	end
end



--剧情相关
function ExitSense(GAMEEVENT, SenseId)
	if SenseId == 0 and gameLib.GuideIsFinish(40) == false then
		gameLib.CreateGuideInScene(5015,0,150,GPRT_None,"点击他可以接取任务！",0.7,true);
		gameLib.FinishGuide(40);
	elseif SenseId == 1 then
		gameLib.ShowGainBaby(1);
	elseif SenseId == 2 then
		--gameLib.CreateGuideInScene(1,5,200,GPRT_None,"这个小姑娘怎么啦？让我过去看看吧。",0.7,true);
	end
end

function AchievementUIOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(35) == false then
		--gameLib.CreateGuide(GAT_FirstAchievement,0,0.18,GPRT_None,"领取奖励吧",0.7,true);
	end
end

function AchievementReceived(GAMEEVENT)
	gameLib.FinishGuide(35);
end

--小地图36
function MainTaskFlushOk(GAMEEVENT)
	if gameLib.CurrentSceneID() == 1 and gameLib.GuideIsFinish(36) == false  and gameLib.GuideIsFinish(37) == true then
		gameLib.CreateGuide(GAT_QuestMiniFirst,0,-0.2,GPRT_R180,"去找技能教官吧！",0.7,true);
		gameLib.FinishGuide(36);
	end
end

function WorldMapOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(36) == false  and gameLib.GuideIsFinish(37) == true then
		gameLib.CreateGuide(GAT_WorldMapWorldBtn,0,0.18,GPRT_None,"切换到世界地图");
	end
end


function WorldMapToWorld(GAMEEVENT)
	if gameLib.GuideIsFinish(36) == false and gameLib.GuideIsFinish(37) == true then
		gameLib.CreateGuide(GAT_WorldMapFL,0,0.18,GPRT_None,"回主城看看");
	end
end

--好友38
function ClickMainFriend(GAMEEVENT)
	if gameLib.GuideIsFinish(38) == false then
		gameLib.CreateGuide(GAT_FriendAddBtn,0,0.18,GPRT_None,"去添加一个好友吧");
	end
end
function ClickAddFriendBtn(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(38);
end

function ClickMainFamily(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(39);
end
--提升
function ClickRaiseUpBtn(GAMEEVENT)
	gameLib.ClearGuide();
end


--符文45
function BagFuwenOpen(GAMEEVENT)
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false then
		gameLib.CreateGuide(GAT_MainBagFuwenFirstItem,0,-0.2,GPRT_R180,"选择这个符文",0.7,true,45);
	end
end

function BagFuwenCombieUI(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false then
		if ARG0 == 1 then
			gameLib.CreateGuide(GAT_MainFuwenUICombieBtn,0,-0.2,GPRT_R180,"3个合成1个，可以合成了",0.7,true,45);
		else
			gameLib.CreateGuide(GAT_MainFuwenCloseBtn,0,-0.2,GPRT_R180,"3个合成1个，数量不够！先关掉吧",0.7,true,45);
			gameLib.FinishGuide(47);
		end
	end
end

function BagFuwenCombieSuccess(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false then
		gameLib.CreateGuide(GAT_MainFuwenCloseBtn,0,-0.2,GPRT_R180,"合成完毕，等级高了一级",0.7,true,45);
	end
	gameLib.FinishGuide(47);
end

function FuwenUIClose(GAMEEVENT,ARG0)
	if gameLib.GuideIsFinish(46) == true and gameLib.GuideIsFinish(45) == false then
		gameLib.CreateGuide(GAT_MainBagFuwenFirstItem,0,-0.2,GPRT_R180,"装备上这个试试吧",0.7,true,45);
	end
end

function BagFuwenClickTipsInsertBtn(GAMEEVENT)
	gameLib.ClearGuide();
	gameLib.FinishGuide(45);
end

function PlayerAutoOrder(GAMEEVENT,ARG0)
	if ARG0 == 2 and gameLib.GuideIsFinish(48) == true and gameLib.GuideIsFinish(50) == false then
		gameLib.CreateGuide(GAT_PlayerAuto,0,0.2,GPRT_None,"更换一个技能");
		gameLib.FinishGuide(50);
	end
end

function OpenAutoPanel(GAMEEVENT)
	if gameLib.GuideIsFinish(49) == false and gameLib.GuideIsFinish(50) == true then
		gameLib.CreateGuide(GAT_FirstAutoSkill,0.5,0.2,GPRT_None,"选择这个技能");
	end
	gameLib.FinishGuide(49);
end
