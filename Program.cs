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
            List<StateBoundary> states = StateParser.LoadFromFile("C:\\Users\\User\\Downloads\\states.json");
            List<Tweet> tweets = TweetParser.ParseTweetsFromJson("C:\\Users\\User\\source\\repos\\TwetProject\\cali_tweets2014.txt");
            Console.WriteLine(states.Count);
            Dictionary<string,float> dict = CSVParser.CreateDictionary("C:\\Users\\User\\source\\repos\\TwetProject\\sentiments.csv");
            var AnalysisOfStateSentiment = TweetAnalyzer.AnalyzeTweets(tweets,states,dict);
            Console.WriteLine(AnalysisOfStateSentiment.Count);
            MapDrawer.drawMap(AnalysisOfStateSentiment, tweets);
        }
    }
}
