using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
//		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
//		{
//            return base.FinishedLaunching(app, options);
//
//			_window = new UIWindow(UIScreen.MainScreen.Bounds);
//
//			_viewController = new CardGameViewController();
//			_window.RootViewController = _viewController;
//
//			_window.MakeKeyAndVisible();
//
//			return true;
//		}

        // class-level declarations
        
        public override UIWindow Window {
            get;
            set;
        }
        
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation (UIApplication application)
        {
        }
        
        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground (UIApplication application)
        {
        }
        
        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground (UIApplication application)
        {
        }
        
        // This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate (UIApplication application)
        {
        }
	}
}

