
#include "ComScriptHeader.h"

GAME_SCRIPT_API( Sys, reload_item_table);
GAME_SCRIPT_API( Sys, reload_drop_table);
GAME_SCRIPT_API( Sys, reload_ai_table);
GAME_SCRIPT_API( Sys, reload_quest_table);

GAME_SCRIPT_API( Sys, add_gm_cmd );

GAME_SCRIPT_API( Sys, init_dynamic_npc);
GAME_SCRIPT_API( Sys, refresh_dynamic_npc);
GAME_SCRIPT_API( Sys, close_dynamic_npc);

GAME_SCRIPT_API( Sys, openmushroom);
GAME_SCRIPT_API( Sys, refreshmushroom);
GAME_SCRIPT_API( Sys, closemushroom);
GAME_SCRIPT_API( Sys, openxiji);
GAME_SCRIPT_API( Sys, refreshxiji);
GAME_SCRIPT_API( Sys, closexiji);
GAME_SCRIPT_API( Sys, refreshTongjiTimes);
GAME_SCRIPT_API( Sys, openalonepk);
GAME_SCRIPT_API( Sys, refreshalonepk);
GAME_SCRIPT_API( Sys, closealonepk);
GAME_SCRIPT_API( Sys, openteampk);
GAME_SCRIPT_API( Sys, refreshteampk);
GAME_SCRIPT_API( Sys, closeteampk);
GAME_SCRIPT_API( Sys, openExam);
GAME_SCRIPT_API( Sys, closeExam);
GAME_SCRIPT_API( Sys, openWarrior);
GAME_SCRIPT_API( Sys, closeWarrior);
GAME_SCRIPT_API( Sys, openpet);
GAME_SCRIPT_API( Sys, closepet);
GAME_SCRIPT_API( Sys, send_mail);
GAME_SCRIPT_API( Sys, send_mail_all);
GAME_SCRIPT_API( Sys, send_mail_drop);
GAME_SCRIPT_API( Sys, send_mail_all_drop);
GAME_SCRIPT_API( Sys, gm_mail);
GAME_SCRIPT_API( Sys, reset_activity);
GAME_SCRIPT_API( Sys, notice);
GAME_SCRIPT_API( Sys, vipitem);
GAME_SCRIPT_API( Sys, pass_zero_hour);
GAME_SCRIPT_API( Sys, isfirstwin_pvr);
GAME_SCRIPT_API( Sys, firstwinreward_pvr);
GAME_SCRIPT_API( Sys, sendPVRrewardbytimes);
GAME_SCRIPT_API( Sys, sendPVRrewardbyday);
GAME_SCRIPT_API( Sys, sendPVRrewardbysenson);
GAME_SCRIPT_API( Sys, sendPVPrewardbyday);
GAME_SCRIPT_API( Sys, sendPVPrewardbysenson);
GAME_SCRIPT_API( Sys, get_world_level);
GAME_SCRIPT_API( Sys, open_guild_battle);
GAME_SCRIPT_API( Sys, start_guild_battle);
GAME_SCRIPT_API( Sys, stop_guild_battle);
GAME_SCRIPT_API( Sys, close_guild_battle);
GAME_SCRIPT_API( Sys, join_guild_battle_scene);
GAME_SCRIPT_API( Sys, talked_guild_progenitus);
GAME_SCRIPT_API( Sys, prepare_guild_battle_timeout);
GAME_SCRIPT_API( Sys, get_guild_name);
GAME_SCRIPT_API( Sys, send_guild_mail);
GAME_SCRIPT_API( Sys, del_guild_npc);
GAME_SCRIPT_API( Sys, open_guild_demon_invaded); 
GAME_SCRIPT_API( Sys, close_guild_demon_invaded);
GAME_SCRIPT_API( Sys, open_guild_leader_invaded);
GAME_SCRIPT_API( Sys, close_guild_leader_invaded);
GAME_SCRIPT_API( Sys, add_npc);
GAME_SCRIPT_API( Sys, del_npc);
GAME_SCRIPT_API( Sys, transfor_scene);

GAME_SCRIPT_API( Sys, open_festival);
GAME_SCRIPT_API( Sys, close_festival);
GAME_SCRIPT_API( Sys, open_card);
GAME_SCRIPT_API( Sys, close_card);
GAME_SCRIPT_API( Sys, open_rechargeTotal);
GAME_SCRIPT_API( Sys, close_rechargeTotal);
GAME_SCRIPT_API( Sys, open_rechargeSingle);
GAME_SCRIPT_API( Sys, close_rechargeSingle);
GAME_SCRIPT_API( Sys, open_discountStore);
GAME_SCRIPT_API( Sys, close_discountStore);
GAME_SCRIPT_API( Sys, open_hotShop);
GAME_SCRIPT_API( Sys, close_hotShop);
GAME_SCRIPT_API( Sys, open_employeeActivityTotal);
GAME_SCRIPT_API( Sys, close_employeeActivityTotal);
GAME_SCRIPT_API( Sys, allonlineplayeraddmoney);
GAME_SCRIPT_API( Sys, updaterolerogtable);
GAME_SCRIPT_API( Sys, open_mingiftbag);
GAME_SCRIPT_API( Sys, close_mingiftbag);
GAME_SCRIPT_API( Sys, open_zhuanpan);
GAME_SCRIPT_API( Sys, close_zhuanpan);
GAME_SCRIPT_API( Sys, open_integralshop);
GAME_SCRIPT_API( Sys, close_integralshop);
GAME_SCRIPT_API( Sys, add_activation_counter_all);
GAME_SCRIPT_API( Sys, kickPlayer);
GAME_SCRIPT_API( Sys, sys_refreshEmployeeQuest);
///========================================================================
///@group 游戏相关命令
///@{
GAME_SCRIPT_API( Entity, getWeapon); //获得武器
GAME_SCRIPT_API( Entity, get_skills);
GAME_SCRIPT_API( Entity, check_state);
GAME_SCRIPT_API( Entity, getType);
GAME_SCRIPT_API( Entity, remove_state);
GAME_SCRIPT_API( Entity, insert_state);
GAME_SCRIPT_API( Entity, set_state_value);
GAME_SCRIPT_API( Entity, get_state_value);
GAME_SCRIPT_API( Player, getPlayerInstId);
GAME_SCRIPT_API( Player, getBabyProp);
GAME_SCRIPT_API( Player, get_BattleBaby);
GAME_SCRIPT_API( Player, get_BattleBabyProp);
GAME_SCRIPT_API( Player, change_property );
GAME_SCRIPT_API( Player, autoprop);
GAME_SCRIPT_API( Player, get_property);
GAME_SCRIPT_API( Player, Lottery_Item);			//激活抽奖
GAME_SCRIPT_API( Player, canuseaddskillexpitem);
GAME_SCRIPT_API( Player, add_SkillExp);
GAME_SCRIPT_API( Player, gm_add_skillExp);
GAME_SCRIPT_API( Player, sub_property);
GAME_SCRIPT_API( Player, reset_property);
GAME_SCRIPT_API( Player, reset_property_1);
GAME_SCRIPT_API( Player, set_level);
GAME_SCRIPT_API( Player, set_BabyLevel);
GAME_SCRIPT_API( Player, add_baby );
GAME_SCRIPT_API( Player, del_baby);
GAME_SCRIPT_API( Player, hasbaby);
GAME_SCRIPT_API( Player, get_hasbaby_race);	//某种族的baby有多少个
GAME_SCRIPT_API( Player, get_baby_size);
GAME_SCRIPT_API( Player, add_Employee);
GAME_SCRIPT_API( Player, learn_skill);
GAME_SCRIPT_API( Player, forget_skill);
GAME_SCRIPT_API( Player, add_item);
GAME_SCRIPT_API( Player, del_item);
GAME_SCRIPT_API( Player, add_money);
GAME_SCRIPT_API( Player, add_diamond);
GAME_SCRIPT_API( Player, add_rmb);
GAME_SCRIPT_API( Player, add_Exp);
GAME_SCRIPT_API( Player, jump_quest);
GAME_SCRIPT_API( Player, is_current_quest);
GAME_SCRIPT_API( Player, is_complate_quest);
GAME_SCRIPT_API( Player, accept_quest);
GAME_SCRIPT_API( Player, submit_quest);
GAME_SCRIPT_API( Player, GMaccept_quest);
GAME_SCRIPT_API( Player, add_Reputation);
GAME_SCRIPT_API( Player, set_opensubsystem);
GAME_SCRIPT_API( Player, get_opensubsystem);
GAME_SCRIPT_API( Player, add_questcounter);
GAME_SCRIPT_API( Player, reduce_questcounter);
GAME_SCRIPT_API( Player, add_ExpByBattleBaby);
GAME_SCRIPT_API( Player, set_HundredTier);
GAME_SCRIPT_API( Player, complete_Achievement);
GAME_SCRIPT_API( Player, complete_AllAchievement);
GAME_SCRIPT_API( Player, reset_counter); //重置计数
GAME_SCRIPT_API( Player, enter_Scene);
GAME_SCRIPT_API( Player, set_guide);
GAME_SCRIPT_API( Player, set_guide_all);
GAME_SCRIPT_API( Player, get_guide);
GAME_SCRIPT_API( Player, set_glamour);
GAME_SCRIPT_API( Player, add_pvpjjcgrade);
GAME_SCRIPT_API( Player, joinbattle);
GAME_SCRIPT_API( Player, joinbattle2);
GAME_SCRIPT_API( Player, joinbattlez);
GAME_SCRIPT_API( Player, delnpc);
GAME_SCRIPT_API( Player, addprivnpc);
GAME_SCRIPT_API( Player, delprivnpc);
GAME_SCRIPT_API( Player, getnpctype);
GAME_SCRIPT_API( Player, isteamleader);
GAME_SCRIPT_API( Player, checkteamLevel);		//检查队伍中是否有不够等级的
GAME_SCRIPT_API( Player, send_mail);
GAME_SCRIPT_API( Player, openvip);
GAME_SCRIPT_API( Player, getviptime);
GAME_SCRIPT_API( Player, get_bag_free_slot);
GAME_SCRIPT_API( Player, get_EquipItem);				//获取角色身上某槽位装备itemId
GAME_SCRIPT_API( Player, add_activation_counter);

GAME_SCRIPT_API( Player, get_activation_counter);
GAME_SCRIPT_API( Player, send_errorno);
GAME_SCRIPT_API( Player, getCurBagGirdNum);
GAME_SCRIPT_API( Player, openbaggrid);
GAME_SCRIPT_API( Player, openstoragegrid);
GAME_SCRIPT_API( Player, isstorageFull);
GAME_SCRIPT_API( Player, getstoragesize);
GAME_SCRIPT_API( Player, openscene);
GAME_SCRIPT_API( Player, errorhint);	//错误提示
GAME_SCRIPT_API( Player, sendawardByDropId);
GAME_SCRIPT_API( Player, openMagic);
GAME_SCRIPT_API( Player, getskillName);
GAME_SCRIPT_API( Player, getplayerName);
GAME_SCRIPT_API( Player, getbabyName);
GAME_SCRIPT_API( Player, getemployeeName);
GAME_SCRIPT_API( Player, getbabydelat);
GAME_SCRIPT_API( Player, checkOpenGather);
GAME_SCRIPT_API( Player, opengather);
GAME_SCRIPT_API( Player, opencompound);
GAME_SCRIPT_API( Player, checkcompound);
GAME_SCRIPT_API( Player, copyGo);
GAME_SCRIPT_API( Player, setAchuevementValue);
GAME_SCRIPT_API( Player, addAchuevementValue);
GAME_SCRIPT_API( Player, addGuildContribution);
GAME_SCRIPT_API( Player, addGuildMoney);
GAME_SCRIPT_API( Player, startonlinetime);
GAME_SCRIPT_API( Player, stoponlinetime);
GAME_SCRIPT_API( Player, sevenopen);
GAME_SCRIPT_API( Player, sevenclose);
GAME_SCRIPT_API( Player, checkquestitem);
GAME_SCRIPT_API( Player, intensifybaby2target);
GAME_SCRIPT_API( Player, getbabyintensifylevel);
GAME_SCRIPT_API( Player, setPlayerLevel);
GAME_SCRIPT_API( Player, addplayertitle);
GAME_SCRIPT_API( Player, setPlayerJob);
GAME_SCRIPT_API( Player, setPlayerLevel);
GAME_SCRIPT_API( Player, openCystal);
GAME_SCRIPT_API( Player, addcoursegift);
GAME_SCRIPT_API( Player, addIntegral);
GAME_SCRIPT_API( Player, get_minlevel_baby);
GAME_SCRIPT_API( Player, addBabyProp);
GAME_SCRIPT_API( Player, setemployeebattlegroup);
///@}

///========================================================================
///@group 战斗相关
///@{ 

GAME_SCRIPT_API( Battle, get_BattleType);
GAME_SCRIPT_API( Battle, get_prop);
GAME_SCRIPT_API( Battle, change_prop);
GAME_SCRIPT_API( Battle, changeProp_state);
GAME_SCRIPT_API( Battle, changeProp_fanji);

GAME_SCRIPT_API( Battle, insert_state); 
GAME_SCRIPT_API( Battle, check_state); 
GAME_SCRIPT_API( Battle, cutTime_state);
GAME_SCRIPT_API( Battle, clear_state);
GAME_SCRIPT_API( Battle, remove_state);

GAME_SCRIPT_API( Battle, change_position);
GAME_SCRIPT_API( Battle, is_battle_baby);
GAME_SCRIPT_API( Battle, isBabybyPos);		
GAME_SCRIPT_API( Battle, select_baby);
GAME_SCRIPT_API( Battle, get_Force);
GAME_SCRIPT_API( Battle, run_away);
GAME_SCRIPT_API( Battle, get_runawayNum);
GAME_SCRIPT_API( Battle, set_runawayNum);
GAME_SCRIPT_API( Battle, add_Counter);
GAME_SCRIPT_API( Battle, getLevel_State);
GAME_SCRIPT_API( Battle, change_report_skill);
GAME_SCRIPT_API( Battle, change_order_skill);
GAME_SCRIPT_API( Battle, change_order_target);
GAME_SCRIPT_API( Battle, check_Order);
GAME_SCRIPT_API( Battle, set_huwei);
GAME_SCRIPT_API( Battle, getItemType_BySlot);
GAME_SCRIPT_API( Battle, getSkill_TargetType);
GAME_SCRIPT_API( Battle, get_skill_melee);
GAME_SCRIPT_API( Battle, current_order_skill);
GAME_SCRIPT_API( Battle, zhuachong);
GAME_SCRIPT_API( Battle, getSneakAttack);
GAME_SCRIPT_API( Battle, getRound);
GAME_SCRIPT_API( Battle, getBattleDataID);
GAME_SCRIPT_API( Battle, getMaxMpPos);
GAME_SCRIPT_API( Battle, getMinHpPos);
GAME_SCRIPT_API( Battle, getMaxHpPos);
GAME_SCRIPT_API( Battle, getBossPos);
GAME_SCRIPT_API( Battle, CheckHp);
GAME_SCRIPT_API( Battle, ai_pushOrder );
GAME_SCRIPT_API( Battle, add_Monster);
GAME_SCRIPT_API( Battle, getMonsterTypebyPos);
///@}

GAME_SCRIPT_API( Team, get_teammembers);
GAME_SCRIPT_API( Team, get_level);

GAME_SCRIPT_API( Activity, update);




