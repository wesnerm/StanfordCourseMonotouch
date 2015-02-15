using System;

namespace WM
{
    public class PlayingCardDeck : Deck
    {
        public PlayingCardDeck()
        {
            foreach(var suit in PlayingCard.validSuits)
            {
                for(var rank=1; rank <= PlayingCard.maxRank; rank++)
                {
                    var card = new PlayingCard();
                    card.rank = rank;
                    card.suit = suit;
                    addCard(card, true);
                }
            }
        }
    }
}

