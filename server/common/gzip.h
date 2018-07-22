#ifndef __GZIP_H__
#define __GZIP_H__
#ifdef _WIN32
	#include "zlib/zlib.h"
#else
	#include "zlib.h"
#endif 
/* Compress gzip data */
/* data 原数据 ndata 原数据长度 zdata 压缩后数据 nzdata 压缩后长度 */
int gzcompress(Bytef *data, uLong ndata, Bytef *zdata, uLong *nzdata);

/* Uncompress gzip data */
/* zdata 数据 nzdata 原数据长度 data 解压后数据 ndata 解压后长度 */
int gzdecompress(Byte *zdata, uLong nzdata, Byte *data, uLong *ndata);

int isgzcompress(Byte id1, Byte id2);


#endif // __GZIP_H__