using System;
using System.Linq;
using System.Collections.Generic;

namespace WM
{
    public class PlayingCard : Card
    {
        public PlayingCard()
        {
        }

        public string suit = "?";
        public int rank;

        public static string[] validSuits = new [] { "♥", "♦", "♠", "♣" };
        public static int maxRank = 13;


        public override string contents
        {
            get {return rank + suit; }
        }

        public static string[] rankStrings = new [] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };


        public override int match(System.Collections.Generic.IEnumerable<Card> otherCards)
        {
            int score = 0;

            if (otherCards.Count() == 1)
            {
                var otherCard = (PlayingCard) otherCards.First();
                if (otherCard.suit == suit)
                    score = 1;
                else if (otherCard.rank == rank)
                    score = 4;
            }

            return score;

        }
    }
}

