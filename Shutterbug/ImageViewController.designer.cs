// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("ImageViewController")]
	partial class ImageViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem titleBarButtonItem { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIActivityIndicatorView spinner { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (titleBarButtonItem != null) {
				titleBarButtonItem.Dispose ();
				titleBarButtonItem = null;
			}

			if (spinner != null) {
				spinner.Dispose ();
				spinner = null;
			}
		}
	}
}
