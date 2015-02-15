
using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace WM
{
	public partial class CardGameViewController : UIViewController
	{
        #region Variables
        int _flipCount;
        CardMatchingGame _game;

        UIButton[] _cardButtons;

        [Export("cardButtons")]
        UIButton[] cardButtons 
        { 
            get { return _cardButtons; }
            set { 
                _cardButtons = value; 
                updateUI();
            }
        }


        #endregion

		public CardGameViewController (IntPtr handle) : base (handle)
		{
		}

        public int FlipCount
        {
            get { return _flipCount; }
            set {
                _flipCount = value;
                flipsLabel.Text = "Flips: " + _flipCount;
            }
        }

        public CardMatchingGame game
        {
            get 
            {
                if (_game == null)
                    _game = new CardMatchingGame(_cardButtons.Length, new PlayingCardDeck());
                return _game;
            }

        }


        partial void flipCard(NSObject sender)
        {
            var button = (UIButton) sender;
            game.flipCardAtIndex(Array.IndexOf(cardButtons, sender));
            FlipCount++;
            updateUI();
        }

        public void updateUI()
        {
            
            for(int i=0; i<_cardButtons.Length; i++)
            {
                UIButton cardButton = _cardButtons[i];
                Card card = game.cardAtIndex(i);
                cardButton.SetTitle(card.contents, UIControlState.Selected);
                cardButton.SetTitle(card.contents, UIControlState.Selected|UIControlState.Disabled);
                cardButton.Selected = card.faceUp;
                cardButton.Enabled = !card.isUnplayable;
                cardButton.Alpha = card.isUnplayable ? 0.3f : 1.0f;
            }

            if (_scoreLabel != null)
                _scoreLabel.Text = "Score: " + game.score;
        }
        

    
    }
}

