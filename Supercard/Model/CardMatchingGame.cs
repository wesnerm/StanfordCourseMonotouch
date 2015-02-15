using System;
using System.Collections.Generic;


namespace WM
{
    public class CardMatchingGame
    {
	    readonly List<Card> _cards = new List<Card>();

        public CardMatchingGame(int cardCount, Deck usingDeck)
        {

            for (int i = 0; i<cardCount; i++)
            {
                Card card = usingDeck.drawRandomCard();
                if (card != null)
                    _cards.Add(card);
            }
        }

        const int MatchBonus = 4;
        const int MismatchPenalty = 2;
        const int FlipCost = 1;

        public void FlipCardAtIndex(int index)
        {
            Card card = CardAtIndex(index);
            if (card != null)
            {
                if (!card.isUnplayable)
                {
                    if (!card.faceUp)
                    {
                        foreach(var otherCard in _cards)
                        {
                            if (otherCard.faceUp && !otherCard.isUnplayable)
                            {
                                int matchScore = card.match(new [] {otherCard});
                                if (matchScore > 0)
                                {
                                    otherCard.isUnplayable = true;
                                    card.isUnplayable = true;
                                    Score += matchScore * MatchBonus;
                                }
                                else
                                {
                                    otherCard.faceUp = false;
                                    Score -= MismatchPenalty;
                                }
                                break;
                            }
                        }

                        Score -= FlipCost;
                    }

                    card.faceUp = !card.faceUp;
                }
            }

        }

        public Card CardAtIndex(int index)
        {
            if (index < _cards.Count)
                return _cards[index];
            return null;

        }

        public int Score;
    }
}

