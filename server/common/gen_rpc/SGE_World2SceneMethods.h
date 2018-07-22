virtual bool initDynamicNpcs(NpcType type, S32 count);
virtual bool refreshDynamicNpcs(NpcType type, S32 count);
virtual bool finiDynamicNpcs(NpcType type);
virtual bool addDynamicNpcs(S32 sceneId, std::vector< S32 >& npcs);
virtual bool delDynamicNpc(S32 npcId);
virtual bool delDynamicNpc2(S32 sceneId, S32 npcId);
virtual bool openSceneCopy(S32 instId);
virtual bool closeSceneCopy(S32 instId);
