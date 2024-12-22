//
//  TTPUnityBilling.m
//  Unity-iPhone
//
//  Created by Tabtale on 12/12/2018.
//

#import <Foundation/Foundation.h>
#import "TTPUnityServiceManager.h"
#import <TT_Plugins_Core/TTPIbilling.h>


@interface TTPUnityBilling : NSObject

+ (NSDictionary *) ttpBillingPostDataFromJsonStr: (const char*) json;

@end

@implementation TTPUnityBilling

+ (NSDictionary *) ttpBillingPostDataFromJsonStr: (const char*) json {
    // if json == null or json == "" subscriptionStarted event should not be sent
    if (json == nullptr) return nil;
    if([@(json) isEqualToString: @""]) return nil;
    NSData *data = [[[NSString alloc] initWithUTF8String:json] dataUsingEncoding:NSUnicodeStringEncoding];
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:nil];
    return dict;
}

extern "C" {
    
    void ttpValidateReceiptAndReport(const float price, const char * currency, const char * productId, const char* subscriptionStartedData)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if (billing != nil) {
            NSString *sPrice =[[NSString alloc] initWithUTF8String: price != -1 ? [[NSString stringWithFormat:@"%f", price] UTF8String] : ""];
            NSString *sCurrency =[[NSString alloc] initWithUTF8String:currency != NULL ? currency : ""];
            NSString *sProductId =[[NSString alloc] initWithUTF8String:productId != NULL ? productId : ""];

            NSDictionary *params = [TTPUnityBilling ttpBillingPostDataFromJsonStr:subscriptionStartedData];
            
            [billing validateReceiptAndReport:price
                                     currency:sCurrency
                                    productID:sProductId
                    subscriptionStartedParams:params
                            completionHandler:^(BOOL verified, ValidationFailureReason validationResult, NSString *errorMessage)
             {
                NSString *str = [NSString stringWithFormat:@"{ \"price\" : \"%@\", \"currency\" : \"%@\", \"productId\" : \"%@\", \"valid\" : %s, \"failureReason\" : %li, \"error\" : \"%@\"  }", sPrice, sCurrency, sProductId, verified ? "true" : "false", validationResult, errorMessage];
                id<TTPIunityMessenger> unityMessenger = [serviceManager get:@protocol(TTPIunityMessenger)];
                if (unityMessenger != nil) {
                    [unityMessenger unitySendMessage:"OnValidateResponse" message:[str UTF8String]];
                }
            }];
        }
    }
    
    void ttpNoAdsPurchased(bool purchased)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil)
        {
            [billing setNoAdsItemPurchased:purchased];
        }
    }

    bool ttpWasUserDetectedInChina()
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil)
        {
            return [billing wasUserDetectedInChina];
        }
        return false;
    }

    extern void ttpReportPurchaseToConversion(const char * message)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil && message != NULL)
        {
            NSString *json = [[NSString alloc] initWithUTF8String:message];
            NSData *data = [json dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
            if(dic != nil){
                NSString *currency = [dic objectForKey:@"currency"];
                NSString *productId = [dic objectForKey:@"productId"];
                NSNumber *price = [dic objectForKey:@"price"];
                NSNumber *consumable = [dic objectForKey:@"consumable"];
                if(productId != nil && price != nil){
                    [billing reportPurchaseToConversion:currency price:[price floatValue] productId:productId consumable:[consumable boolValue]];
                }
            }
        }
    }
    
}

@end
