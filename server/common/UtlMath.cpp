#include "UtlMath.h"
#include "ace/OS_NS_sys_time.h"

float		UtlMath::s_sinTab[NUM_ANGLES];
void UtlMath::init()
{
	
	srand(int(ACE_OS::gettimeofday().sec()));

	// Init sin tab.
	for( int i = 0; i < NUM_ANGLES; i++ )
		s_sinTab[i] =float( sin( (float)i * 2.f * QPI / (float)NUM_ANGLES ));
}


void UtlMath::updateSrand(){
	srand(int(ACE_OS::gettimeofday().sec()));
}
