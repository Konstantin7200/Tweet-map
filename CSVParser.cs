using System.Globalization;

namespace program
{
    class CSVParser
    {
        static public Dictionary<string,float> CreateDictionary(string path)
        {
            string file = File.ReadAllText(path);
            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            List<String> data=file.Split(',','\n').ToList();
            for(int i=0;i<data.Count-1;i+=2)
            {
                dictionary.Add(data[i], float.Parse(data[i + 1],CultureInfo.InvariantCulture));
            }
            return dictionary;
        }
    }
}