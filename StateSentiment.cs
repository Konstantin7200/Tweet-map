using System;
using System.Collections.Generic;
using System.Drawing;

namespace program
{
    public class StateSentiment // for kostyan
    {
        public string StateCode { get; }
        public double? AverageSentiment { get; set; } //change (float -> double)
        public List<List<PointF>> Polygons { get; }

        public StateSentiment(string stateCode, List<List<PointF>> polygons)
        {
            StateCode = stateCode;
            Polygons = polygons;
            AverageSentiment = null;
        }
    }
}
