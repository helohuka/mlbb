//
//  SDKHandler.h
//  M185SDK_Demo
//
//  Created by Sans on 2018/7/23.
//  Copyright © 2018年 Sans. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <M185SDK/M185SDK.h>
#import <SY_185SDK/SY_185SDK.h>

@interface SDKHandler : NSObject <M185CallBackDelegate>

+ (SDKHandler *)sharedHandeler;

+ (void)login;
+ (void)logOut;

+ (void)showUserCenter;


+ (void)submitData:(M185SubmitData *)data;
+ (void)pay:(M185PayConfig *)config;


@end
