using System;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace WM
{
    public class GameResult
    {
        public DateTime Start;
        public DateTime End;
        private int _score;
		
        public GameResult()
        {
            Start = DateTime.Now;
            End = Start;
        }

        private const string AllResultsKey = "GameResult_All";
        private const string StartKey = "StartDate";
        private const string EndKey = "EndDate";
        private const string ScoreKey = "Score";

        public static int CompareScoreToGameResult(GameResult self, GameResult otherResult)
        {
			return self.Score.CompareTo(otherResult.Score);
        }

        public static int CompareEndDateToGameResult(GameResult self, GameResult otherResult)
        {
            return self.End.CompareTo(otherResult.End);
        }

        public static int CompareDurationToGameResult(GameResult self, GameResult otherResult)
        {
            return self.Duration.CompareTo(otherResult.Duration);
        }

	    public static List<GameResult> AllGameResults
	    {
		    get
		    {
			    var list = new List<GameResult>();






                var find = NSUserDefaults.StandardUserDefaults
                    .DictionaryForKey(AllResultsKey);

                if (find != null)
        		    foreach (var plist in find.Values)
        		    {
        			    var result = FromPropertyList(plist);
        			    if (result != null)
        				    list.Add(result);
        		    }

			    return list;
		    }
	    }

	    public static GameResult FromPropertyList(NSObject plist)
        {
            var result = new GameResult();
            var nsdict = plist as NSDictionary;
            if (nsdict != null)
            {
                result.Start = (NSDate)nsdict[StartKey];
                result.End = (NSDate)nsdict[EndKey];
                result.Score = ((NSNumber)nsdict[ScoreKey]).IntValue;
            }

            return result;
        }

        public void Synchronize()
        {
			var gameResults = NSUserDefaults.StandardUserDefaults.DictionaryForKey(AllResultsKey);
	        gameResults = gameResults != null ? (NSDictionary) gameResults.MutableCopy() : new NSMutableDictionary();
	        gameResults[(NSString)Start.ToString()] = AsPropertyList();
            NSUserDefaults.StandardUserDefaults[AllResultsKey] =gameResults;
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public NSObject AsPropertyList()
        {
            return new NSDictionary(
                StartKey, (NSDate)Start,
                EndKey, (NSDate)End,
                ScoreKey, Score
                );
        }

        public TimeSpan Duration
        {
            get { return End-Start; }
        }

        public int Score
        {
            get { return _score; }
            set {
                _score = value;
                End = DateTime.Now;
                Synchronize();
            }
        }



    }
}

