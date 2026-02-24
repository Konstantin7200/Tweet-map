using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TwitterProj.Models;

namespace TwitterProj.Services
{
    public static class TweetAnalyzer
    {
        public static List<StateSentiment> AnalyzeTweets(List<Tweet> tweets, List<StateBoundary> stateBoundaries, Dictionary<string, float> sentiments)
        {
            AnalyzeTweetSentiments(tweets, sentiments);
            return GroupByState(tweets, stateBoundaries);
        }

        private static void GroupTweets(List<Tweet> tweets, List<StateBoundary> stateBoundaries)
        {
            foreach (StateBoundary stateBoundary in stateBoundaries)
            {
                foreach (List<PointF> polygon in stateBoundary.Polygons)
                {
                    foreach (Tweet tweet in tweets)
                    {
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddPolygon(polygon.ToArray());
                            if (path.IsVisible(new PointF((float)tweet.Longitude, (float)tweet.Latitude)))
                            {
                                tweet.StateCode = stateBoundary.StateCode;
                            }
                        }

                    }
                }
            }
        }
        private static void AnalyzeTweetSentiments(List<Tweet> tweets, Dictionary<string, float> sentiments)
        {
            foreach (Tweet tweet in tweets)
            {
                string currentText = tweet.Text.ToLower();
                string[] currentWords = Regex.Split(currentText, @"[^\p{L}]+").Where(w => !string.IsNullOrEmpty(w)).ToArray();

                List<float> currentSentiments = new List<float>();
                int currentIndex = 0;
                while (currentIndex < currentWords.Length)
                {
                    bool foundSentiment = false;
                    for (int length = Math.Min(5, currentWords.Length - currentIndex); length >= 1; length--)
                    {
                        string phrase = string.Join(" ", currentWords, currentIndex, length);
                        if (sentiments.ContainsKey(phrase))
                        {
                            foundSentiment = true;
                            currentSentiments.Add(sentiments[phrase]);
                            currentIndex += length;
                            break;
                        }
                    }
                    if (!foundSentiment) currentIndex++;
                }
                if (currentSentiments.Count > 0)
                {
                    tweet.SentimentScore = currentSentiments.Average();
                }
            }
        }

        private static List<StateSentiment> GroupByState(List<Tweet> tweets, List<StateBoundary> statesBoundaries)
        {
            GroupTweets(tweets, statesBoundaries);
            var validTweets = tweets.Where(tweet => !string.IsNullOrEmpty(tweet.StateCode) && tweet.SentimentScore.HasValue).ToList();
            var groupTweets = validTweets.GroupBy(tweet => tweet.StateCode);
            var result = new List<StateSentiment>();
            foreach (var group in groupTweets)
            {
                string currentState = group.Key;
                var tweetsInState = group.ToList();
                var boundary = statesBoundaries.FirstOrDefault(bound => bound.StateCode == currentState);
                if (boundary == null) continue;
                float averageValue = tweetsInState.Average(tweet => tweet.SentimentScore.Value);
                var stateSentiment = new StateSentiment(currentState, boundary.Polygons)
                {
                    AverageSentiment = averageValue
                };

                result.Add(stateSentiment);
            }
            foreach (StateBoundary stateBoundary in statesBoundaries)
            {
                bool flag = false;
                foreach (StateSentiment stateSentiment in result)
                {
                    if (stateSentiment.StateCode == stateBoundary.StateCode)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    result.Add(new StateSentiment(stateBoundary.StateCode, stateBoundary.Polygons));
                }
                result.Add(stateSentiment);

            }
            return result;
        }
    }
}