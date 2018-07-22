#import <AdSupport/AdSupport.h>
#import <AVFoundation/AVFoundation.h>
#import "TalkingDataEAuth.h"
#include <sys/socket.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <err.h>
#include <sys/xattr.h>

extern "C" {
    
	char* _MakeStringCopy( const char* string)  
    {  
        if (NULL == string) {  
            return NULL;  
        }  
        char* res = (char*)malloc(strlen(string)+1);  
        strcpy(res, string);  
        return res;  
    }
	
    const char* _getIDFA() {
		char* str = _MakeStringCopy([[[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString] UTF8String]);
        return str;
    }
	
	float _getButtery()  
	{  
		[[UIDevice currentDevice] setBatteryMonitoringEnabled:YES];  
		return [[UIDevice currentDevice] batteryLevel];  
	}
	
	const char* _getVersion()
	{
       NSDictionary *infoDictionary = [[NSBundle mainBundle] infoDictionary];
	   NSString *app_build = [infoDictionary objectForKey:@"CFBundleVersion"];  
		return _MakeStringCopy([app_build UTF8String]);
	}
	
	bool _hasMicrophoneAuth()
	{
		__block BOOL bCanRecord = YES;
		[[AVAudioSession sharedInstance] requestRecordPermission:^(BOOL granted) { bCanRecord = granted; }];
		return bCanRecord;
	}
	
	void _initEAuth( const char* appid, const char* secretid)
	{
		NSString *AppID = [[NSString alloc] initWithUTF8String:appid];
		NSString *SecretID = [[NSString alloc] initWithUTF8String:secretid];
		[TalkingDataEAuth initEAuth:AppID secretId:SecretID];
	}
	
	void _requestCode(const char* mobile, const char* userid)
	{
		NSString *MobileNum = [[NSString alloc] initWithUTF8String:mobile];
		NSString *UserID = [[NSString alloc] initWithUTF8String:userid];
		[TalkingDataEAuth applyAuthCode:@"86" mobile:MobileNum accountName:UserID delegate:nil];
	}
	
	void _auth(const char* mobile, const char* userid, const char* code)
	{
		NSString *MobileNum = [[NSString alloc] initWithUTF8String:mobile];
		NSString *CodeNum = [[NSString alloc] initWithUTF8String:code];
		NSString *UserID = [[NSString alloc] initWithUTF8String:userid];
		[TalkingDataEAuth bindEAuth:@"86"  mobile:MobileNum authCode:CodeNum accountName:UserID delegate:nil];
	}
    
    bool AddSkipBackupAttributeToItemAtURL(NSURL* URL)
    {
        assert([[NSFileManager defaultManager] fileExistsAtPath: [URL path]]);
        
        NSError *error = nil;
        BOOL success = [URL setResourceValue: [NSNumber numberWithBool: YES]
                                      forKey: NSURLIsExcludedFromBackupKey error: &error];
        if(!success){
            NSLog(@"Error excluding %@ from backup %@", [URL lastPathComponent], error);
        }
        else
            NSLog(@"Success for set excluding from backup");
        return success;
    }
	
//	void AddSkipBackupAttributeToItemAtURL(NSURL* URL)
//	{
//		const char* filePath = [[URL path] fileSystemRepresentation];
//		const char* attrName = "com.apple.MobileBackup";
//		u_int8_t attrValue = 1;
//		setxattr(filePath, attrName, &attrValue, sizeof(attrValue), 0, 0);
//	}
	
	void _setNotBackup()
	{
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
		NSString *docDir = [paths objectAtIndex:0];
		NSString* temp = [docDir stringByAppendingFormat:@"/NotBackup"];
		[[NSFileManager defaultManager] createDirectoryAtPath:temp withIntermediateDirectories:YES attributes:nil error:nil];
		NSURL *dbURLPath = [NSURL URLWithString:temp];
		AddSkipBackupAttributeToItemAtURL(dbURLPath);
	}
	
	const char* _getIPv6(const char *mHost,const char *mPort)
	{
		if( nil == mHost )
			return NULL;
		const char *newChar = "No";
		const char *cause = NULL;
		struct addrinfo* res0;
		struct addrinfo hints;
		struct addrinfo* res;
		int n, s;
		
		memset(&hints, 0, sizeof(hints));
		
		hints.ai_flags = AI_DEFAULT;
		hints.ai_family = PF_UNSPEC;
		hints.ai_socktype = SOCK_STREAM;
		
		if((n=getaddrinfo(mHost, "http", &hints, &res0))!=0)
		{
			printf("getaddrinfo error: %s\n",gai_strerror(n));
			return NULL;
		}
		
		struct sockaddr_in6* addr6;
		struct sockaddr_in* addr;
		NSString * NewStr = NULL;
		char ipbuf[32];
		s = -1;
		for(res = res0; res; res = res->ai_next)
		{
			if (res->ai_family == AF_INET6)
			{
				addr6 =( struct sockaddr_in6*)res->ai_addr;
				newChar = inet_ntop(AF_INET6, &addr6->sin6_addr, ipbuf, sizeof(ipbuf));
				NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar 
	encoding:NSASCIIStringEncoding];
				NSString * TempB = [NSString stringWithUTF8String:"&&ipv6"];
				
				NewStr = [TempA stringByAppendingString: TempB];
				printf("%s\n", newChar);
			}
			else
			{
				addr =( struct sockaddr_in*)res->ai_addr;
				newChar = inet_ntop(AF_INET, &addr->sin_addr, ipbuf, sizeof(ipbuf));
				NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar 
	encoding:NSASCIIStringEncoding];
				NSString * TempB = [NSString stringWithUTF8String:"&&ipv4"];
				
				NewStr = [TempA stringByAppendingString: TempB];			
				printf("%s\n", newChar);
			}
			break;
		}
		
		
		freeaddrinfo(res0);
		
		printf("getaddrinfo OK");
		
		NSString * mIPaddr = NewStr;
        	const char* ipaddr = [mIPaddr UTF8String];
		return _MakeStringCopy(ipaddr);
	}
}
