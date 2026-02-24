using System;
using System.Collections.Generic;
using System.Drawing;

namespace TwitterProj.Models
{
    public class StateBoundary
    {
        //штат - массив полигонов, полигон — массив точек. Точка — point.
        public string StateCode { get; }
        public List<List<PointF>> Polygons { get;}//штат.полигон.точки
        public StateBoundary(string stateCode, List<List<PointF>> polygons)
        {
            StateCode = stateCode;
            Polygons = polygons;
        }
    }
}