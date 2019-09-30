using System;
using Foundation;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Plugin.XSnack;
using Sharpnado.Presentation.Forms.iOS;
using TailwindTraders.Mobile.Features.Scanning;
using TailwindTraders.Mobile.Features.Scanning.Photo;
using TailwindTraders.Mobile.IOS.Features.Scanning;
using TailwindTraders.Mobile.IOS.Features.Scanning.Photo;
using TouchTracking.iOS;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(nameof(TailwindTraders))]

namespace TailwindTraders.Mobile.IOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            InitRenderersAndServices();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            Forms.SetFlags(new[] { "CollectionView_Experimental" });
            Forms.Init();

            RegisterPlatformServices();

            Distribute.DontCheckForUpdatesInDebug();
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            LoadApplication(new App());

            CustomizeAppearance();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override bool OpenUrl(
            UIApplication application,
            NSUrl url,
            string sourceApplication,
            NSObject annotation)
        {
            Distribute.OpenUrl(url);

            return true;
        }

        private void CustomizeAppearance()
        {
            var accentColor = (Color)App.Current.Resources["AccentColor"];
            UITextField.Appearance.TintColor = accentColor.ToUIColor();
        }

        private void InitRenderersAndServices()
        {
            CameraPreviewRenderer.Initialize();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            SharpnadoInitializer.Initialize();
            TouchRecognizer.Initialize();
        }

        private void RegisterPlatformServices()
        {
            DependencyService.Register<IXSnack, XSnackImplementation>();
            DependencyService.Register<IPlatformService, PlatformService>();
        }

        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    Push.RegisteredForRemoteNotifications(deviceToken);
        //}

        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        //{
        //    Push.FailedToRegisterForRemoteNotifications(error);
        //}

        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    var result = Push.DidReceiveRemoteNotification(userInfo);
        //    if (result)
        //    {
        //        completionHandler(UIBackgroundFetchResult.NewData);
        //    }
        //    else
        //    {
        //        completionHandler(UIBackgroundFetchResult.NoData);
        //    }
        //}        
    }
}
