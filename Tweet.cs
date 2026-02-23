using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_map
{
    public class Tweet
    {
        private DateTime created;

        private string text;    

        public string StateCode { get; set; }
        public double? SentimentScore { get; set; }

        private double latitude;

        public double longitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public double Longitude
        {
            get { return longitude; }
            set {  longitude = value; }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Tweet" + nameof(value) + " is empty");
                text = value;
            }
        }
            
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public Tweet() { }
        public Tweet(string text, double latitude, double longitude, DateTime created)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Tweet is empty", nameof(text));
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("incorrect latitude", nameof(latitude));
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("incorrect longitude", nameof(longitude));
            Text = text;
            Created = created;
            Latitude = latitude;   
            Longitude = longitude;
        }
    }
}
