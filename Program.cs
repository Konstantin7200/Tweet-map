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
            List<StateBoundary> states = StateParser.LoadFromFile("states.json");
            List<Tweet> tweets = TweetParser.ParseTweetsFromJson("cali_tweets2014.txt");
            Console.WriteLine(states.Count);
            Dictionary<string,float> dict = CSVParser.CreateDictionary("sentiments.csv");
            var AnalysisOfStateSentiment = TweetAnalyzer.AnalyzeTweets(tweets,states,dict);
            Console.WriteLine(AnalysisOfStateSentiment.Count);
            MapDrawer.drawMap(AnalysisOfStateSentiment, tweets);
        }
    }
}
/*C:\\Users\\User\\Downloads\\
 * C:\\Users\\User\\source\\repos\\TwetProject\\
 * C:\\Users\\User\\source\\repos\\TwetProject\\*/