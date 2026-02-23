using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Text.RegularExpressions;
using TwitterProj.Models;

namespace Tweet_map
{
    public class TweetAnalyzer
    {
        private List<StateBoundary> statesBoundaries;
        private Dictionary<string, float> sentiments;

        public TweetAnalyzer(List<StateBoundary> statesBoundaries, Dictionary<string, float> sentiments)
        {
            this.statesBoundaries = statesBoundaries;
            this.sentiments = sentiments;
        }

        public List<StateSentiment> AnalyzeTweets(List<Tweet> tweets)
        {
            foreach (Tweet tweet in tweets) { 
                string currentText=tweet.Text.ToLower();
                string[] currentWords= Regex.Split(currentText, @"[^\p{L}]+").Where(w => !string.IsNullOrEmpty(w)).ToArray();

                List<float> currentSentiments = new List<float>();
                int currentIndex = 0;
                while (currentIndex < currentWords.Length) 
                { 
                    bool foundSentiment=false;
                    for (int length = Math.Min(5, currentWords.Length - currentIndex); length >= 1; length--) 
                    {
                        string phrase=string.Join(" ", currentWords, currentIndex, length);
                        if (sentiments.ContainsKey(phrase))
                        { 
                            foundSentiment = true;
                            currentSentiments.Add(sentiments[phrase]);
                            currentIndex += length;
                            break;
                        }
                    }
                    if(!foundSentiment) currentIndex++;
                }
                if (currentSentiments.Count > 0)
                {
                    tweet.SentimentScore = currentSentiments.Average();
                }
            }
            var validTweets = tweets.Where(tweet => !string.IsNullOrEmpty(tweet.StateCode) && tweet.SentimentScore.HasValue).ToList();
            var groupTweets = validTweets.GroupBy(tweet => tweet.StateCode);
            var result= new List<StateSentiment>();
            foreach (var group in groupTweets) 
            {
                string currentState=group.Key;
                var tweetsInState=group.ToList();
                var boundary=statesBoundaries.FirstOrDefault(bound=>bound.StateCode == currentState);
                if (boundary == null) continue;
                double averageValue = tweetsInState.Average(tweet => tweet.SentimentScore.Value); // change type of averageValue (float -> double) Kirill
                var stateSentiment = new StateSentiment(currentState, boundary.Polygons)
                {
                    AverageSentiment = averageValue
                };

                result.Add(stateSentiment);
            }
            return result;
        }
    }
}
