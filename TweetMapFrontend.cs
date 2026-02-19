using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.Json;
namespace program
{
    public class StateBoundary
    {
        public string StateCode { get; set; }
        public List<List<PointF>> Polygons { get; set; }
        public StateBoundary(string stateCode, List<List<PointF>> polygons)
        {
            StateCode = stateCode;
            Polygons = polygons;
        }
    }
    class MapDrawer
    {
        static public void drawMap(List<StateBoundary> states)
        {
            const int HEIGHT = 2500, WIDTH = 3500;
            using (Bitmap bitmap = new Bitmap(WIDTH, HEIGHT))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    foreach (StateBoundary state in states)
                    {
                        Color color = getColor(Random.Shared.Next(-100, 100) * 1.0f / 100);
                        foreach (List<PointF> polygon in state.Polygons)
                        {
                            correctPolygon(polygon, HEIGHT, WIDTH);
                            drawPolygon(polygon, color,graphics);
                        }
                     
                    }
                    foreach (StateBoundary state in states)
                    {
                        drawStatesName(state.Polygons[0], state.StateCode, graphics);
                    }
                }
                bitmap.Save("resultingMap.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        static public void correctPolygon(List<PointF> polygon,int HEIGHT,int WIDTH)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                polygon[i] = new PointF((polygon[i].X + 180) * 30, HEIGHT - (polygon[i].Y) * 30);
            }
        }
        static public void drawStatesName(List<PointF> points,string name,Graphics graphics)
        {
            using (Font font = new Font("serif", 16, FontStyle.Bold))
            {
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    if(name!="FL")
                    {
                        graphics.DrawString(name, font, brush, new PointF(((points.Max(p => p.X) + points.Min(p => p.X))) / 2, (points.Max(p => p.Y) + points.Min(p => p.Y)) / 2));
                        return;
                    }
                    float x = 0, y = 0;
                    foreach (PointF point in points)
                    {
                        x += point.X;
                        y += point.Y;
                    }
                    x = x / points.Count;
                    y = y / points.Count;
                    graphics.DrawString(name, font, brush, new PointF(x, y));
                }
            }
        }
        static public void drawPolygon(List<PointF> points, Color color,Graphics graphics)
        {
            using (Brush brush = new SolidBrush(color))
            {
                graphics.FillPolygon(brush, points.ToArray());
            }
            using (Pen pen = new Pen(Color.Black, 2))
            {
                graphics.DrawPolygon(pen, points.ToArray());
            }
        }
        static private Color getColor(float? value)
        {
            int defaultColor = 128;
            int colorMultiplier = 128;
            if (value == null)
                return Color.FromArgb(255, 192, 192, 192);
            int red = Math.Max(Math.Min((int)(defaultColor + value * colorMultiplier * 2), 255),0);
            int green = Math.Max(Math.Min((int)(defaultColor + Math.Abs((float)value) * colorMultiplier), 255),0);
            int blue = Math.Max(Math.Min((int)(defaultColor - value * colorMultiplier * 1.5), 255),0);
            Console.WriteLine(red+"|"+ green+"|"+ blue);
            return Color.FromArgb(255,red,green,blue);
        }
    }
}