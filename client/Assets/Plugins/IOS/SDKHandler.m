//
//  SDKHandler.m
//  M185SDK_Demo
//
//  Created by Sans on 2018/7/23.
//  Copyright © 2018年 Sans. All rights reserved.
//

#import "SDKHandler.h"
#import "ViewController.h"

static SDKHandler *_instance = nil;
@implementation SDKHandler

+ (SDKHandler *)sharedHandeler {
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        if (!_instance) {
            _instance = [[SDKHandler alloc] init];
        }
    });
    return _instance;
}

/** 登录 */
+ (void)login {
    [M185SDKManager login];
}

/** 退出登录 */
+ (void)logOut {
    [M185SDKManager logOut];
}

/** 显示用户中心 */
+ (void)showUserCenter {
    [M185SDKManager showUserCenter];
}

/** 上报数据 */
+ (void)submitData:(M185SubmitData *)data {
    [M185SDKManager submitDataWith:data];
}

/** 发起支付 */
+ (void)pay:(M185PayConfig *)config {
    [M185SDKManager payStartWithConfig:config];
}


#pragma mark - call back

- (void)M185SDKCustomCallBackWithInformation:(id _Nullable)info {
    /** 注:
     *  如没有特殊需求, 不会调用这个方法回调.
     */
    NSLog(@"自定义回调事件 == %@",info);
}

- (void)M185SDKInitCallBackWithSuccess:(BOOL)success Information:(NSDictionary * _Nonnull)dict {
    if (success) {
        NSLog(@"demo 初始成功 == %@",dict);
    } else {
        NSLog(@"demo 初始失败 == %@",dict);
    }
    [self addLog:[NSString stringWithFormat:@"初始化回调 %@ -> %@ ",success ? @"成功" : @"失败",[dict description]]];
}

- (void)M185SDKLogOutCallBackWithSuccess:(BOOL)success Information:(NSDictionary * _Nullable)dict {
    NSLog(@"demo 退出登录 == %@",dict);
    [self addLog:[NSString stringWithFormat:@"登出回调 -> %@",[dict description]]];
}

- (void)M185SDKLoginResultWithCode:(M185LoginResultCode)code Information:(NSDictionary * _Nonnull)dict {
    if (code == CODE_LOGIN_SUCCESS) {
        NSString *extension = [NSString stringWithFormat:@"%@",dict[@"extension"]];
        NSString *token = [NSString stringWithFormat:@"%@",dict[@"token"]];
        NSString *userID = [NSString stringWithFormat:@"%@",dict[@"userID"]];
        NSLog(@"demo 登录成功 == %@",dict);
    } else {
        NSLog(@"demo 登录失败  == %@",dict);
    }
    [self addLog:[NSString stringWithFormat:@"登录回调 -> %@",[dict description]]];
}

- (void)M185SDKPayResultWithStatus:(M185PayResultCode)code Information:(NSDictionary *)dict {
    if (code == CODE_PAY_SUCCESS) {
        NSLog(@"demo 支付成功 == %@",dict);
    } else {
        NSLog(@"demo 支付失败 == %@",dict);
    }
    [self addLog:[NSString stringWithFormat:@"支付回调 %@ -> %@",code == CODE_PAY_SUCCESS ? @"成功" : @"失败" ,[dict description]]];
}

- (void)M185SDKSwitchAccountCallBackWith:(BOOL)success Information:(NSDictionary * _Nullable)dict {
    if (success) {
        NSString *extension = [NSString stringWithFormat:@"%@",dict[@"extension"]];
        NSString *token = [NSString stringWithFormat:@"%@",dict[@"token"]];
        NSString *userID = [NSString stringWithFormat:@"%@",dict[@"userID"]];
        NSLog(@"demo 切换账号 == %@",dict);
    } else {
        NSLog(@"demo 切换账号  == %@",dict);
    }
    [self addLog:[NSString stringWithFormat:@"切换账号回调 -> %@",[dict description]]];
}

static ViewController *VC;
- (void)addLog:(NSString *)log {
    if (VC) {
        [VC addLog:log];
        return;
    } else {
        VC = (ViewController *)[UIApplication sharedApplication].keyWindow.rootViewController;
    }
    
    if ([VC isKindOfClass:[ViewController class]]) {
        [VC addLog:log];
    }
}

@end


extern "C" {
	
	
	
}




