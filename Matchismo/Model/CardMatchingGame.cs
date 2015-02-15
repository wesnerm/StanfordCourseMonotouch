using System;
using System.Collections.Generic;


namespace WM
{
    public class CardMatchingGame
    {
        List<Card> cards = new List<Card>();

        public CardMatchingGame(int cardCount, Deck usingDeck)
        {

            for (int i = 0; i<cardCount; i++)
            {
                Card card = usingDeck.drawRandomCard();
                if (card != null)
                    cards.Add(card);
            }
        }

        const int MATCH_BONUS = 4;
        const int MISMATCH_PENALTY = 2;
        const int FLIP_COST = 1;

        public void flipCardAtIndex(int index)
        {
            Card card = cardAtIndex(index);
            if (card != null)
            {
                if (!card.isUnplayable)
                {
                    if (!card.faceUp)
                    {
                        foreach(var otherCard in cards)
                        {
                            if (otherCard.faceUp && !otherCard.isUnplayable)
                            {
                                int matchScore = card.match(new [] {otherCard});
                                if (matchScore > 0)
                                {
                                    otherCard.isUnplayable = true;
                                    card.isUnplayable = true;
                                    score += matchScore * MATCH_BONUS;
                                }
                                else
                                {
                                    otherCard.faceUp = false;
                                    score -= MISMATCH_PENALTY;
                                }
                                break;
                            }
                        }

                        score -= FLIP_COST;
                    }

                    card.faceUp = !card.faceUp;
                }
            }

        }

        public Card cardAtIndex(int index)
        {
            if (index < cards.Count)
                return cards[index];
            return null;

        }

        public int score;
    }
}

