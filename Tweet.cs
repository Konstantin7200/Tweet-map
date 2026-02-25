using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace program
{
    public class Tweet
    {
        private readonly float latitude;
        private readonly float longitude;
        private readonly DateTime tweetTime;
        private readonly string text;
        public string StateCode { get; set; }
        public float? SentimentScore { get; set; }
        public PointF Location
        {
            get { return new PointF(longitude, latitude); }
        }

        public float Latitude => latitude;
        public float Longitude=> longitude;

        public DateTime TweetTime => tweetTime;

        public string Text => text;

        public Tweet(float latitude, float longitude, DateTime tweetTime, string text)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("incorrect latitude", nameof(latitude));
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("incorrect longitude", nameof(longitude));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Tweet is empty", nameof(text));

            this.latitude = latitude;
            this.longitude = longitude;
            this.tweetTime = tweetTime;
            this.text = text;

            SentimentScore = null;
            StateCode = null;
        }
    }
}