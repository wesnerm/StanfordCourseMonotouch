
using System;
using System.Collections.Generic;

namespace WM
{
    public class Deck
    {
        List<Card> cards = new List<Card>();

        public Deck()
        {
        }

        public void addCard(Card card, bool atTop)
        {
            if (atTop)
                cards.Insert(0, card);
            else
                cards.Add(card);
        }

        static Random random = new Random();

        public Card drawRandomCard()
        {
            if (cards.Count==0)
                return null;

            return cards[random.Next(0, cards.Count)];
        }

    }
}

