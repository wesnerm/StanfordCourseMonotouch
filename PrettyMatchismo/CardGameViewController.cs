
using System;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
	public partial class CardGameViewController : UIViewController
	{
        #region Variables
        int _flipCount;
        CardMatchingGame _game;
        GameResult _gameResult ;

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

		public int StartingCardCount()
		{
			return 20;
		}

		public GameResult GameResult
		{
			get
			{
				if (_gameResult == null)
					_gameResult = new GameResult();
				return _gameResult;
			}
			set { _gameResult = value; }

		}

		public CardMatchingGame Game
        {
            get 
            {
				//if (_game == null)
				//	_game = new CardMatchingGame(_cardButtons.Length, new PlayingCardDeck());
				if (_game == null)
					_game = new CardMatchingGame(StartingCardCount(), CreateDeck());
				return _game;
            }
			set { _game = value; }
        }

		public Deck CreateDeck()
		{
			return new PlayingCardDeck();
		}

		partial void FlipCard(NSObject sender)
        {
            var gesture = (UITapGestureRecognizer) sender;
			var tapLocation = gesture.LocationInView(cardCollectionView);
			var indexPath = cardCollectionView.IndexPathForItemAtPoint(tapLocation);
			if (indexPath == null)
				return;
	        int index = indexPath.Item;
			Game.flipCardAtIndex(index);
            FlipCount++;
            UpdateUI();
            GameResult.Score = Game.score;
        }

        public void UpdateUI()
        {
            
			//for(int i=0; i<_cardButtons.Length; i++)
			//{
			//	UIButton cardButton = _cardButtons[i];
			//	Card card = game.cardAtIndex(i);
			//	cardButton.SetTitle(card.contents, UIControlState.Selected);
			//	cardButton.SetTitle(card.contents, UIControlState.Selected|UIControlState.Disabled);
			//	cardButton.Selected = card.faceUp;
			//	cardButton.Enabled = !card.isUnplayable;
			//	cardButton.Alpha = card.isUnplayable ? 0.3f : 1.0f;
			//}

            foreach (UICollectionViewCell cell in cardCollectionView.VisibleCells)
            {
                var indexPath = cardCollectionView.IndexPathForCell(cell);
                var card = Game.cardAtIndex(indexPath.Item);
                UpdateCell(cell, card);
            }

            if (_scoreLabel != null)
                _scoreLabel.Text = "Score: " + Game.score;
        }
        
		[Export("numberOfSectionsInCollectionView:")]
		public int NumberOfSectionsInCollectionView(UICollectionView collectionView)
		{
			return 1;
		}


		[Export("collectionView:numberOfItemsInSection:")]
		public int NumberOfItemsInSection(UICollectionView collectionView, int section)
		{
			return StartingCardCount();
		}


		[Export("collectionView:cellForItemAtIndexPath:")]
		public UICollectionViewCell CellForItemAtIndexPath(UICollectionView collectionView, NSIndexPath path)
		{
			var cell = (UICollectionViewCell)collectionView.DequeueReusableCell((NSString)"PlayingCard", path);
			var card = Game.cardAtIndex(path.Item);
			UpdateCell(cell, card);
			return cell;
		}

		void UpdateCell(UICollectionViewCell cell, Card card)
		{
			var c = (PlayingCardCollectionViewCell)cell;
			var cd = (PlayingCard)card;
			var view = c.PlayingCardView;
			view.rank = cd.rank;
			view.suit = cd.suit;
			view.faceUp = cd.faceUp;
			view.Alpha = cd.isUnplayable ? 0.3f : 1f;
		}

        partial void Deal()
        {
            Game = null;
            GameResult = null;
            FlipCount = 0;
            UpdateUI();
        }

		public class CollectionDataSource :UICollectionViewDataSource
		{
			readonly CardGameViewController _controller;

			public CollectionDataSource(CardGameViewController controller)
			{
				_controller = controller;
			}

			public override int GetItemsCount(UICollectionView collectionView, int section)
			{
				return _controller.NumberOfItemsInSection(collectionView, section);
			}

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				return _controller.CellForItemAtIndexPath(collectionView, indexPath);
			}

			public override int NumberOfSections(UICollectionView collectionView)
			{
				return _controller.NumberOfSectionsInCollectionView(collectionView);
			}
		}


	}
}

