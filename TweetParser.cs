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

                int startBracket = line.IndexOf('[');
                int endBracket = line.IndexOf(']');

                string coords = line.Substring(startBracket + 1, endBracket - startBracket - 1);
                string[] parts = coords.Split(',');

                string remaining = line.Substring(endBracket + 1).Trim();
                string[] fields = remaining.Split('\t');

                if (fields.Length < 3)
                {
                    Console.WriteLine($"Пропущена строка (недостаточно полей): {line}");
                    return null;
                }

                string dateString = fields[1].Trim();
                DateTime tweetDate;

                // Try to parse Data "2014-02-16 03:11:36"
                if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out tweetDate))
                {
                    // Standart Data 
                    if (!DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out tweetDate))
                    {
                        Console.WriteLine($"Не удалось распарсить дату: {dateString}");
                        return null;
                    }
                }

                string text = fields[2];

                string cleanedText = CleanTweetText(text);

                if (string.IsNullOrWhiteSpace(cleanedText))
                {
                    Console.WriteLine($"Пустой текст твита");
                    return null;
                }

                return new Tweet(float.Parse(parts[0].Trim(), CultureInfo.InvariantCulture), float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture), tweetDate, cleanedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга строки: {line}");
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
        private static string CleanTweetText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string[] words = text.Split(' ');
            List<string> filteredWords = new List<string>();

            foreach (string word in words)
            {
                if (word.StartsWith("@"))
                    continue;

                if (word.StartsWith("http://") || word.StartsWith("https://") || word.StartsWith("www."))
                    continue;

                if (word.Contains("://") || word.Contains("t.co/"))
                    continue;

                filteredWords.Add(word);
            }

            return string.Join(" ", filteredWords).Trim();
        }
    }
}