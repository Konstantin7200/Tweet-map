using System;
using System.Globalization;
using System.IO;
using System.Text;
namespace program
{
    public static class TweetParser
    {

        public static List<Tweet> ParseTweetsFromJson(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("Error.Can't find file");
                return new List<Tweet>();
            }
            else
            {
                Console.WriteLine("File was seccesfully find");
            }

            StreamReader sr = new(FilePath);

            List<Tweet> tweets = new List<Tweet>();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                tweets.Add(ParseLine(line));
            }
            sr.Close();
            return tweets;

        }


        public static Tweet? ParseLine(string line)
        {
            try
            {
                // Skip null lines
                if (string.IsNullOrWhiteSpace(line))
                    return null;

                Tweet tweet = new Tweet();

                int startBracket = line.IndexOf('[');
                int endBracket = line.IndexOf(']');

                string coords = line.Substring(startBracket + 1, endBracket - startBracket - 1);
                string[] parts = coords.Split(',');

                tweet.Latitude = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                tweet.Longitude = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);

                string remaining = line.Substring(endBracket + 1).Trim();
                string[] fields = remaining.Split('\t');

                if (fields.Length < 3)
                {
                    Console.WriteLine($"Пропущена строка (недостаточно полей): {line}");
                    return null;
                }

                string text = fields[2];

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine($"Пустой текст твита");
                    return null;
                }
                tweet.Text = text;

                return tweet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга строки: {line}");
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}