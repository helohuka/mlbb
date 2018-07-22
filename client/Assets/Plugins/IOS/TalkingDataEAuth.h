//
//  TalkingDataEAuth.h
//  TalkingDataSDK
//
//  Created by Robin on 7/13/16.
//  Copyright © 2016 TendCloud. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, TDEAuthType) {
    TDEAuthTypeApplyCode = 0,   // 申请短信认证码
    TDEAuthTypeChecker,         // 检查账号是否已认证
    TDEAuthTypePhoneMatch,      // 检查账号与手机号是否匹配
    TDEAuthTypeBind,            // 账号认证绑定
    TDEAuthTypeUnBind           // 账号认证解绑定
};

@protocol TalkingDataEAuthDelegate <NSObject>

// Delegate回调是在您的主线程中
- (void)onRequestSuccess:(TDEAuthType)type;
- (void)onRequestFailed:(TDEAuthType)type errorCode:(NSInteger)errorCode errorMessage:(NSString *)errorMessage;

@end


@interface TalkingDataEAuth : NSObject

/**
 * 实名认证初始化
 *
 * @param AppId
 *            TalkingData 分配的App Id
 * @param secretId
 *            TalkingData 分配的 secretId
 */
+ (void)initEAuth:(NSString *)appId secretId:(NSString *)secretId;



/**
 *	@method	setLogEnabled
 *  统计日志开关（可选）
 *	@param 	enable 	默认是开启状态
 */
+ (void)setLogEnabled:(BOOL)enable;
 

/**
 * 申请短信认证码
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)applyAuthCode:(NSString *)countryCode
               mobile:(NSString *)mobile
          accountName:(NSString *)acctName
             delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 检查账号是否已认证
 *
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)isVerifyAccount:(NSString *)acctName
               delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 检查手机号和账号是否匹配
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            申请验证码的手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            申请认证码异步回调接口
 */
+ (void)isMobileMatchAccount:(NSString *)acctName
            countryCode:(NSString *)countryCode
                 mobile:(NSString *)mobile
               delegate:(id<TalkingDataEAuthDelegate>)delegate;


/**
 * 进行实名认证绑定
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            验证手机号
 * @param authCode
 *            短信认证码
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            绑定请求的异步回调接口
 */
+ (void)bindEAuth:(NSString *)countryCode
           mobile:(NSString *)mobile
         authCode:(NSString *)authCode
      accountName:(NSString *)acctName
         delegate:(id<TalkingDataEAuthDelegate>)delegate;

/**
 * 解除实名认证绑定
 *
 * @param countryCode
 *            国家码 如：中国，86
 * @param mobile
 *            验证手机号
 * @param acctName
 *            用户登录时所使用的用户名
 * @param delegate
 *            解绑请求的异步回调接口
 */
+ (void)unbindEAuth:(NSString *)countryCode
             mobile:(NSString *)mobile
        accountName:(NSString *)acctName
           delegate:(id<TalkingDataEAuthDelegate>)delegate;

@end
