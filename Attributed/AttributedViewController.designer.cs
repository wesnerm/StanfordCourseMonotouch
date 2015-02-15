// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("AttributedViewController")]
	partial class AttributedViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView _label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIStepper _selectedWordStepper { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _selectedWordLabel { get; set; }

		[Action ("updateSelectedWord:")]
		partial void updateSelectedWord (MonoTouch.Foundation.NSObject sender);

		[Action ("underline")]
		partial void underline ();

		[Action ("ununderline")]
		partial void ununderline ();

		[Action ("outline")]
		partial void outline ();

		[Action ("unoutline")]
		partial void unoutline ();

		[Action ("changeColor:")]
		partial void changeColor (MonoTouch.UIKit.UIButton button);

		[Action ("changeFont:")]
		partial void changeFont (MonoTouch.UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_label != null) {
				_label.Dispose ();
				_label = null;
			}

			if (_selectedWordStepper != null) {
				_selectedWordStepper.Dispose ();
				_selectedWordStepper = null;
			}

			if (_selectedWordLabel != null) {
				_selectedWordLabel.Dispose ();
				_selectedWordLabel = null;
			}
		}
	}
}
