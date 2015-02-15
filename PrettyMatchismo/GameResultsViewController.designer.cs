// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("GameResultsViewController")]
	partial class GameResultsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView display { get; set; }

		[Action ("sortByDate")]
		partial void sortByDate ();

		[Action ("sortByScore")]
		partial void sortByScore ();

		[Action ("sortByDuration")]
		partial void sortByDuration ();
		
		void ReleaseDesignerOutlets ()
		{
			if (display != null) {
				display.Dispose ();
				display = null;
			}
		}
	}
}
