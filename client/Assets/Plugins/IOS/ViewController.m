//
//  ViewController.m
//  M185SDK_Demo
//
//  Created by Sans on 2018/7/26.
//  Copyright © 2018年 Sans. All rights reserved.
//

#import "ViewController.h"
#import "SDKHandler.h"


#define kScreen_width   [UIScreen mainScreen].bounds.size.width
#define kScreen_height  [UIScreen mainScreen].bounds.size.height
@interface ViewController () <UITextViewDelegate>


@property (strong, nonatomic) UITextView *outPutView;


@end



@implementation ViewController

- (BOOL)textViewShouldBeginEditing:(UITextView *)textView {
    return NO;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor whiteColor];
    [self.view addSubview:[self createButtonWithFrame:CGRectMake(kScreen_width * 0.1, kScreen_height * 0.1, kScreen_width * 0.8, 44) Title:@"登录" Selector:@selector(login:)]];
    [self.view addSubview:[self createButtonWithFrame:CGRectMake(kScreen_width * 0.1, kScreen_height * 0.1 + 50, kScreen_width * 0.8, 44) Title:@"登出" Selector:@selector(logout:)]];
    [self.view addSubview:[self createButtonWithFrame:CGRectMake(kScreen_width * 0.1, kScreen_height * 0.1 + 100, kScreen_width * 0.8, 44) Title:@"发起支付" Selector:@selector(pay:)]];
    [self.view addSubview:[self createButtonWithFrame:CGRectMake(kScreen_width * 0.1, kScreen_height * 0.1 + 150, kScreen_width * 0.8, 44) Title:@"上报数据" Selector:@selector(submitData:)]];
    
    self.outPutView = [[UITextView alloc] initWithFrame:CGRectMake(kScreen_width * 0.1, kScreen_height * 0.1 + 200, kScreen_width * 0.8, kScreen_height * 0.8 - 200)];
//    self.outPutView.userInteractionEnabled = NO;
    self.outPutView.layer.cornerRadius = 8;
    self.outPutView.layer.masksToBounds = YES;
    self.outPutView.layer.borderWidth = 2;
    self.outPutView.layer.borderColor = [UIColor grayColor].CGColor;
    self.outPutView.delegate = self;
    [self.view addSubview:self.outPutView];
}

- (UIButton *)createButtonWithFrame:(CGRect)frame Title:(NSString *)title Selector:(SEL)selector {
    UIButton *button = [UIButton buttonWithType:(UIButtonTypeCustom)];
    button.frame = frame;
    [button setTitle:title forState:(UIControlStateNormal)];
    [button addTarget:self action:selector forControlEvents:(UIControlEventTouchUpInside)];
    [button setBackgroundColor:[UIColor orangeColor]];
    button.layer.cornerRadius = 8;
    button.layer.masksToBounds = YES;
    return button;
}

/** 登录 */
- (void)login:(id)sender {
    [self addLog:@"登录"];
    [SDKHandler login];
}
/** 登出 */
- (void)logout:(id)sender {
    [self addLog:@"登出"];
    [SDKHandler logOut];
}
/** 发起支付 */
- (void)pay:(id)sender {
    M185PayConfig *config = [M185PayConfig new];
    config.serverID = @"1";
    config.serverName = @"搞什么";
    config.productID = @"1";
    config.productName = @"飞龙在天";
    config.productDesc = @"这样总行了吧";
    config.roleName = @"风云天下";
    config.roleID = @"2";
    config.roleLevel = @"99";
    config.amount = @"1";
    config.extension = @"extension";
    [SDKHandler pay:config];
    
    [self addLog:[NSString stringWithFormat:@"发起支付\n serverid = %@\n serverName = %@\n productID = %@\n productName = %@\n productDesc = %@\n roleName = %@\n roleID = %@\n roleLevel = %@\n amount = %@\n extension = %@",config.serverID,config.serverName,config.productID,config.productName,config.productDesc,config.roleName,config.roleID,config.roleLevel,config.amount,config.extension]];
}
/** 上报数据 */
- (void)submitData:(id)sender {
    M185SubmitData *data = [M185SubmitData new];
    data.type = M185SubmitTypeUpgradeLevel;
    data.serverID = @"66";
    data.serverName = @"火影忍者";
    data.roleName = @"血饮狂刀";
    data.roleID = @"77";
    data.roleLevel = @"99";
    data.moneyNumber = @"82376";
    data.vipLevel = @"20";
    [SDKHandler submitData:data];
    
    
    [self addLog:[NSString stringWithFormat:@"上报数据\n type = %lu\n serverID = %@\n serverName = %@\n roleName = %@\n roleID = %@\n roleLevel = %@\n moneyNumber = %@\n vipLevel = %@",(unsigned long)data.type,data.serverID,data.serverName,data.roleName,data.roleID,data.roleLevel,data.moneyNumber,data.vipLevel]];
}

- (void)addLog:(NSString *)log {
    self.outPutView.text = [NSString stringWithFormat:@"%@\n-------------------------------\n%@: %@",self.outPutView.text,[NSDate date],log];
    if (self.outPutView.contentSize.height > self.outPutView.frame.size.height) {
        CGPoint offset = CGPointMake(0, self.outPutView.contentSize.height - self.outPutView.frame.size.height);
        [self.outPutView setContentOffset:offset animated:YES];
    }
}

@end
