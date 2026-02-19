using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace program
{
    class MapDrawer
    {
        static public void Main()
        {
            List<PointF> list = new List<PointF>();
            list.Add(new PointF(100, 100));
            list.Add(new PointF(900, 100));
            list.Add(new PointF(900, 900));
            list.Add(new PointF(100, 900));
            list.Add(new PointF(100,100));

            List<List<PointF>> superList = new List<List<PointF>>();
            superList.Add(list);
            List<List<List<PointF>>> superpuperList = new List<List<List<PointF>>>();
            superpuperList.Add(superList);
            drawMap(superpuperList);
        }
        static public void drawMap(List<List<List<PointF>>> states)
        {
            foreach (List<List<PointF>> state in states)
            {
                Color color = getColor(Random.Shared.Next(-1,1));
                foreach(List<PointF> polygon in state)
                {
                    drawPolygon(polygon, color);
                }
            }
        }
        static public void drawPolygon(List<PointF> points, Color color)
        {
            using (Bitmap bitmap = new Bitmap(1000, 1200))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    using (Brush brush = new SolidBrush(color))
                    {
                        graphics.FillPolygon(brush, points.ToArray());
                    }
                }
                bitmap.Save("resultingMap.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        static private Color getColor(float? value)
        {
            int defaultColor = 128;
            int colorMultiplier = 200;
            if (value == null)
                return Color.FromArgb(255, 192, 192, 192);
            int red = Math.Max(Math.Min((int)(defaultColor + value * colorMultiplier * 2), 255),0);
            int green = Math.Max(Math.Min((int)(defaultColor + Math.Abs((float)value) * colorMultiplier), 255),0);
            int blue = Math.Max(Math.Min((int)(defaultColor - value * colorMultiplier * 1.5), 255),0);
            return Color.FromArgb(255,red,green,blue);
        }
    }
}