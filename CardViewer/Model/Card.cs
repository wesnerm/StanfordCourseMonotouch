using System;
using System.Collections.Generic;

namespace WM
{
    public class Card
    {
        public Card()
        {
        }

        public virtual string contents
        {
            get { return null; }
        }

        public bool faceUp;
        public bool isUnplayable;

        public virtual int match(IEnumerable<Card> otherCards)
        {
            int score = 0;
            foreach(var card in otherCards)
            {
                if (card.contents==contents)
                    score = 1;
            }
            return score;
        }


    }
}

