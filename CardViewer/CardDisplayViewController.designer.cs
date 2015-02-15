// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace WM
{
	[Register ("CardDisplayViewController")]
	partial class CardDisplayViewController
	{
		[Outlet]
		WM.PlayingCardView playingCardView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (playingCardView != null) {
				playingCardView.Dispose ();
				playingCardView = null;
			}
		}
	}
}
