// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("CardChooserViewController")]
	partial class CardChooserViewController
	{
		[Outlet]
		MonoTouch.UIKit.UISegmentedControl suitSegmentedControl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rankLabel { get; set; }

		[Action ("changeRank:")]
		partial void changeRank (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (suitSegmentedControl != null) {
				suitSegmentedControl.Dispose ();
				suitSegmentedControl = null;
			}

			if (rankLabel != null) {
				rankLabel.Dispose ();
				rankLabel = null;
			}
		}
	}
}
