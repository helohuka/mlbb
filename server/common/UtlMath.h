#ifndef __UTL_Math_H_ // Inc Guard
#define __UTL_Math_H_

#include <math.h>
#include <float.h>
#include "config.h"


// Constants.
#define QPI 				(3.1415926535897932)
#define QINV_PI				(0.31830988618)
#define QHALF_PI			(1.57079632679)

#define SMALL_NUMBER		(1.e-8)
#define KINDA_SMALL_NUMBER	(1.e-4)

// Magic numbers for numerical precision.
#define QDELTA				(0.00001f)

#define QMAXBYTE		0XFF
#define QMAXWORD		0XFFFFU
#define QMAXDWORD		0XFFFFFFFFU
#define QMAXSBYTE		0X7F
#define QMAXSWORD		0X7FFF
#define QMAXINT			0X7FFFFFFF




/**
 * @class QMath
 *
 * @brief Basic math functions.
 */
class UtlMath
{
public:
	/// 得到指数
	static inline double exp( double value )			{ return ::exp( value ); }

	/// 自然对数
	static inline double log( double value )			{ return ::log(value); }

	/// 以10为底的对数
	static inline double log10( double value )		{ return ::log10(value); }

	/// float remainder of a/b
	static inline double fmod( double a, double b )	{ return ::fmod(a,b); }

	///
	static inline double sin( double value )			{ return ::sin(value); }

	static inline double asin( double value )			{ return ::asin(value); }

	static inline double cos( double value )			{ return ::cos(value); }

	static inline double acos( double value )			{ return ::acos(value); }

	static inline double tan( double value )			{ return ::tan(value); }

	static inline double atan( double value )			{ return ::atan(value); }

	static inline double atan2( double y, double x )	{ return ::atan2(y,x); }
	
	template<class T>
	static inline T abs(T v){ return v<0 ? -v:v; }
	static inline bool feq(float a,float b){ return ::fabs(a-b) < FLT_EPSILON;}
	/// 平方根
	static inline double sqrt( double value )			{ return ::sqrt(value); }

	/// 幂
	static inline double pow( double a, double b )	{ return ::pow(a,b); }

	//static bool isnan( double value )			{ return (::_isnan(value)!=0); }

	static inline void srand( int seed )				{ ::srand(seed); }

	//[0,RAND_MAX]
	static inline int rand()						{ return ::rand(); }
	//[0,n)
	static inline int randN(int n)					{ return ::rand()%n;}
	//[n,m]
	static inline int randNM(int n,int m)			{ return (n+::rand()%(m-n+1));}
	//[0,1] 浮点数
	static inline float frand()					{ return ::rand() / (float)RAND_MAX; }
	//(n,m]
	static inline float frandNM(float n , float m) { return n + (float(randNM(1 ,RAND_MAX)) / float(RAND_MAX)) * (float)(m-n); }
	//c in [n,m]
	static void  randCinNM(int c,int n,int m,std::set<int>& res)
	{
		while(res.size()<size_t(c))
		{
			int r=randNM(n,m);
			res.insert(r);
		}
	}
	
	///  T容器类型 必须是stl库内容器 
	template<typename T>
	static bool randSubtract( const std::vector<T>& total, const std::vector<T>& sub, T& result)
	{
		if (total.size() <= sub.size())
			return false;

		std::vector<T> ret(total);	
		for (size_t i=0;i<sub.size();i++)
		{
			for (size_t j=0; j<ret.size();j++)
			{
				if(sub[i]==ret[j])
				{
					ret.erase(ret.begin()+j);
				}
			}
		}
		if(ret.empty())
			return false;			
		int r=randN(ret.size());
		result=ret[r];
		return true;
	}
	
	template<class T>
	static void subtract(std::vector<T> const& A, std::vector<T> const& B, std::vector<T>& R){
		R = A;
		for(size_t i=0; i<B.size(); ++i){
			for(size_t j=0; j<R.size(); ++j){
				if(R[i] == B[i]){
					R.erase(R.begin() + j);
					break;
				}
			}
		}
	}

	//权重随机
	static int randWeight(std::vector<std::pair<int ,int> > pool)
	{
		
		if(pool.empty())
			return -1;


		std::random_shuffle(pool.begin(),pool.end());

		int	sum = 0; 
		std::vector<std::pair<int ,int> >::iterator	iter = pool.begin();
		for(;iter != pool.end(); ++iter)
		{
			sum += iter->second;
		}
	
		int randNum = rand() % sum;
		iter = pool.begin();
		for(;iter != pool.end(); ++iter)
		{
			if(randNum < iter->second)
				break;
			randNum -= iter->second;
		}
		int ret = iter->first;
		return ret;
	}
	/// 四舍五入
	static inline S32 round( float value )			{ return (S32)::floor(value+0.5);}

	static inline S32 floor( float value )			{ return (S32)::floor(value); }

	static inline S64 floor_l( float value )			{ return (S64)::floor(value); }

	static inline S32 ceil( float value )				{ return (S32)::ceil(value); }

	static inline float fractional( float value )		{ return value - ::floor(value); }
	
	/// Find the closest power of 2 that is >= N.
	static inline S64 nextPowOf2( S64 N )
	{
		if (N<=0L		) return 0L;
		if (N<=1L		) return 1L;
		if (N<=2L		) return 2L;
		if (N<=4L		) return 4L;
		if (N<=8L		) return 8L;
		if (N<=16L	    ) return 16L;
		if (N<=32L	    ) return 32L;
		if (N<=64L 	    ) return 64L;
		if (N<=128L     ) return 128L;
		if (N<=256L     ) return 256L;
		if (N<=512L     ) return 512L;
		if (N<=1024L    ) return 1024L;
		if (N<=2048L    ) return 2048L;
		if (N<=4096L    ) return 4096L;
		if (N<=8192L    ) return 8192L;
		if (N<=16384L   ) return 16384L;
		if (N<=32768L   ) return 32768L;
		if (N<=65536L   ) return 65536L;
		else			  return 0;
	}

	static inline bool isPowOf2( S64 N )
	{
		return !(N & (N - 1));
	}

	
	enum {ANGLE_SHIFT 	= 2};		// Bits to right-shift to get lookup value.
	enum {ANGLE_BITS	= 14};		// Number of valid bits in angles.
	enum {NUM_ANGLES 	= 16384}; 	// Number of angles that are in lookup table.
	enum {ANGLE_MASK    = (((1<<ANGLE_BITS)-1)<<(16-ANGLE_BITS))};
	static float		s_sinTab[NUM_ANGLES];
	/**
	 * 查表计算角度的sin和cos
	 * @param i 一个整数代表的角度。系统使用NUM_ANGLES个项来映射2PI,然后将一个word
	 * 映射到NUM_ANGLES。i的有效范围为0~65535，代表0~2PI
	 */
	static inline float sinTab( int i )
	{
		return s_sinTab[((i>>ANGLE_SHIFT)&(NUM_ANGLES-1))];
	}

	static inline float cosTab( int i )
	{
		return s_sinTab[(((i+16384)>>ANGLE_SHIFT)&(NUM_ANGLES-1))];
	}

	static inline int reduceAngle( int a )
	{
		return a & ANGLE_MASK;
	}
	/// 初始化数学库
	
	
	static void init();

	static void updateSrand();

};

#endif//__QMath_H_
