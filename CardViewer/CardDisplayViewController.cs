// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
	public partial class CardDisplayViewController : UIViewController
	{
		public CardDisplayViewController (IntPtr handle) : base (handle)
		{
		}

        public int rank;
        public string suit;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            playingCardView.rank = rank;
            playingCardView.suit = suit;
            playingCardView.faceUp = true;
        }


	}
}
