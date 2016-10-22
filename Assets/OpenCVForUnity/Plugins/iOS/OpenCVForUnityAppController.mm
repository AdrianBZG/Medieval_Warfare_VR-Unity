#import <UIKit/UIKit.h>
#import "UnityAppController.h"

extern "C" void OpenCVForUnitySetGraphicsDevice(void* device, int deviceType, int eventType);
extern "C" void OpenCVForUnityRenderEvent(int marker);

@interface OpenCVForUnityAppController : UnityAppController
{
}
- (void)shouldAttachRenderDelegate;
@end

@implementation OpenCVForUnityAppController

- (void)shouldAttachRenderDelegate;
{
	UnityRegisterRenderingPlugin(&OpenCVForUnitySetGraphicsDevice, &OpenCVForUnityRenderEvent);
}
@end


IMPL_APP_CONTROLLER_SUBCLASS(OpenCVForUnityAppController)

