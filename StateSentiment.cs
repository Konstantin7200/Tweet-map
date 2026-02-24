using System;
using System.Collections.Generic;
using System.Drawing;

namespace TwitterProj
{
    public class StateSentiment // for kostyan
    {
        public string StateCode { get; }
        public float? AverageSentiment { get; set; }
        public List<List<List<double>>> Polygons { get; }

        public StateSentiment(string stateCode, List<List<PointF>> polygons)
        {
            StateCode = stateCode;
            Polygons = new List<List<List<double>>>();
            AverageSentiment = null;
        }
    }
}