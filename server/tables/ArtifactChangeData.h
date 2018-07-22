#ifndef  __ARTIFACT_CHANGE_H__
#define __ARTIFACT_CHANGE_H__

#include "config.h"

class ArtifactChangeTable
{
public:


	struct ArtifactChangeData
	{
		S32 job_;
		S32 diamonds_;
		JobType professionType_;
	};

public:
	static bool load(char const *fn);
	static bool check();
	static ArtifactChangeData const * getArtifactById(S32 id);

public:
	static std::map< S32 , ArtifactChangeData*>  data_;
};

#endif