using System;
using System.Drawing;
using WM;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
    public partial class SupercardViewController : UIViewController
    {
		Deck deck = new PlayingCardDeck();

        public SupercardViewController(IntPtr handle) : base (handle)
        {
        }
		
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }
		
		#region View lifecycle
		
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
		
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
		
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
		
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
		
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
		
		#endregion
		
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }

        PlayingCardView _playingCardView;


        [Outlet]
        PlayingCardView playingCardView 
        { 
            get { return _playingCardView; }
            set { 
                _playingCardView = value; 
				drawRandomPlayingCard();
                
                var rec = new UIPinchGestureRecognizer(_playingCardView, new MonoTouch.ObjCRuntime.Selector("pinch:"));
                _playingCardView.AddGestureRecognizer(rec);
            }
        }

		void drawRandomPlayingCard()
		{
			var card = deck.drawRandomCard() as PlayingCard;
			if (card!=null)
			{
				playingCardView.rank = card.rank;
				playingCardView.suit = card.suit;
			}
		}

        partial void swipe (MonoTouch.UIKit.UISwipeGestureRecognizer sender)
        {
            UIView.Transition(playingCardView, .5f, UIViewAnimationOptions.TransitionFlipFromLeft,
                              ()=>{

				if (!playingCardView.faceUp) drawRandomPlayingCard();
                playingCardView.faceUp = !playingCardView.faceUp;
            }, null);
        }
    }
}

