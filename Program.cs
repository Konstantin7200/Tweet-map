using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    class Program
    {
        static public void Main()
        {
            StateParser stateParser = new();
            List<StateBoundary> states = stateParser.LoadFromFile("states.json");
            List<Tweet> tweets = TweetParser.ParseTweetsFromJson("cali_tweets2014.txt");
            Console.WriteLine(states.Count);
            Dictionary<string,float> dict = CSVParser.CreateDictionary("sentiments.csv");
            TweetAnalyzer tweetAnalyzer = new TweetAnalyzer(states, dict);
            var zalupa = tweetAnalyzer.AnalyzeTweets(tweets);
            Console.WriteLine(zalupa.Count);
            MapDrawer.drawMap(zalupa, tweets);
        }
    }
}
