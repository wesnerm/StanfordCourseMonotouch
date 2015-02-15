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
		public  MonoTouch.UIKit.UILabel _scoreLabel { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel flipsLabel { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UICollectionView cardCollectionView { get; set; }

		[Outlet]
		public WM.PlayingCardView playingCardView { get; set; }

		[Action ("flipCard:")]
		partial void FlipCard (MonoTouch.Foundation.NSObject sender);

		[Action ("Deal")]
		partial void Deal ();
		
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

			if (cardCollectionView != null) {
				cardCollectionView.Dispose ();
				cardCollectionView = null;
			}

			if (playingCardView != null) {
				playingCardView.Dispose ();
				playingCardView = null;
			}
		}
	}
}
