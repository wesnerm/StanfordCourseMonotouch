// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("CardGameViewController")]
	partial class CardGameViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _scoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel flipsLabel { get; set; }

		[Action ("flipCard:")]
		partial void flipCard (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_scoreLabel != null) {
				_scoreLabel.Dispose ();
				_scoreLabel = null;
			}

			if (flipsLabel != null) {
				flipsLabel.Dispose ();
				flipsLabel = null;
			}
		}
	}
}
