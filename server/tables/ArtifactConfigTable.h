#ifndef __ARTIFACT_CONFIG_H__
#define __ARTIFACT_CONFIG_H__

#include "config.h"

class ArtifactConfigTable
{
public:
	struct   ArtifactConfigItem
	{
		S32	itemId_;
		S32 itemNum_;
	};

	struct ArtifactConfigData
	{
		S32 level_;
		S32 diamonds_;
		S32	tupoLevel_;
		S32	grow_;
		std::vector<ArtifactConfigItem> items_;
		JobType professionType_;
	};

public:
	static bool load(char const *fn);
	static bool check();
	static ArtifactConfigData const * getArtifactById(S32 id,JobType job);

public:
	static std::map< S32 , std::vector<ArtifactConfigData*> >  data_;
};

#endif