using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tweet_map
{
    public class StateSentiment // for kostyan
    {
        public string StateCode { get; }
        public double? AverageSentiment { get; set; } //change (float -> double)
        public List<List<List<double>>> Polygons { get; }

        public StateSentiment(string stateCode, List<List<PointF>> polygons)
        {
            StateCode = stateCode;
            Polygons = new List<List<List<double>>>();
            AverageSentiment = null;
        }
    }
}
