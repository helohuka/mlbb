#ifndef __SCENE_TABLE_H__
#define __SCENE_TABLE_H__

struct ZoneInfo
{
	U32		zoneId_;
	U32		prob_;						//开始战斗概率
	U32		rollMax_;					//怪物最大数量
	U32		rollMin_;					//怪物最小数量
	float   zoneCenterX_;
	float   zoneCenterZ_;
	float	zoneRadius_;
	std::vector<std::string>	monsterClasses_;		//怪物类型
};

struct EntryInfo
{
public:
	U32 entryId_;
	U32 toSceneId_;
	U32 toEntryId_;
	bool isBornPos_;
	float entryCenterX_;
	float entryCenterZ_;
	float entryRadius_;
};

struct FuncPInfo{
	S32 id_;
	FunctionalPointType fpt_;
	float x,y,z;
	float rx,ry,rz,rw;
};

struct SceneData{
	S32			sceneId_;
	SceneType   sceneType_;
	std::string sceneName_;
	std::string			navFileName_; ///导航网格文件
	std::vector<ZoneInfo*>		zones_;				//区域怪物
	std::vector<EntryInfo*>		entrys_;
	std::vector<FuncPInfo*> funcpinfo_;
	
	COM_FPosition maxPoint_;
	COM_FPosition minPoint_;

	ZoneInfo* getZone(S32 zoneId);
};

class SceneTable
{
public:
	static bool load(char const* fn);
	static bool parse(char const *fn, U32 id, SceneType type,std::string navfile);
	static bool check();
	static SceneData* getSceneById(U32 sceneId)
	{
		SceneMap::iterator itr = scenes_.find(sceneId);
		if(itr!=scenes_.end())
			return itr->second;
		return NULL;
	}
	static SceneData* getHomeScene();
	static SceneData* getGuildHomeScene();
	static SceneData* getGuildBattleScene();
	static SceneData* getNewScene();
	
	typedef std::map< S32 , SceneData* > SceneMap;

	static SceneMap	scenes_;
};

#endif