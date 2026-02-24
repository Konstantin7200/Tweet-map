using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text.Json;
namespace program
{
    class MapDrawer
        {
            private const int HEIGHT = 2500, WIDTH = 3500,DOT_RADIUS = 15;
            static public void drawMap(List<StateSentiment> states,List<Tweet> tweets)
            {
                using (Bitmap bitmap = new Bitmap(WIDTH, HEIGHT))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.White);
                        foreach (StateSentiment state in states)
                        {
                            Color color = getColor((float?)state.AverageSentiment);
                            foreach (List<PointF> polygon in state.Polygons)
                            {
                                drawPolygon(correctPolygon(polygon), color, graphics);
                            }

                        }
                        drawDots(tweets,graphics);
                        foreach (StateSentiment state in states)
                        {
                            drawStatesName(state.Polygons, state.StateCode, graphics);
                        }
                    }
                    bitmap.Save("resultingMap.png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            static private void drawDots(List<Tweet> tweets,Graphics graphics)
            {
                foreach(Tweet tweet in tweets)
                {
                    PointF point = correctPoint(new PointF((float)tweet.Longitude,(float)tweet.Latitude));
                    Color color = getColor((float?)tweet.SentimentScore);
                    using (Brush brush = new SolidBrush(color))
                    {
                        graphics.FillEllipse(brush, point.X, point.Y, DOT_RADIUS, DOT_RADIUS);
                    }
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        graphics.DrawEllipse(pen,point.X,point.Y,DOT_RADIUS,DOT_RADIUS);
                    }
                }
            }
            static private List<PointF> correctPolygon(List<PointF> polygon)
            {
                List<PointF> correctedPolygon = new();
                for (int i = 0; i < polygon.Count; i++)
                {
                    correctedPolygon.Add(correctPoint(polygon[i]));
                }
                return correctedPolygon;
            }
            static private PointF correctPoint(PointF point)
            {
                return new PointF((point.X + 180) * 30, HEIGHT - (point.Y) * 30);
            }
            static private float calculateArea(List<PointF> polygon)
            {
                return (polygon.Max(p => p.X) - polygon.Min(p => p.X)) * (polygon.Max(p => p.Y) - polygon.Min(p => p.Y));
            }
            static private void drawStatesName(List<List<PointF>> polygons, string name, Graphics graphics)
            {
                List<PointF> points = correctPolygon(polygons[0]);
                float maxArea = calculateArea(points);
                for (int i=1;i<polygons.Count;i++)
                {
                    float newArea = calculateArea(polygons[i]);
                    if(newArea>maxArea)
                    {
                        maxArea = newArea;
                        points = correctPolygon(polygons[i]);
                    }
                }
                using (Font font = new Font("serif", 16, FontStyle.Bold))
                {
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        //if (name != "FL")
                        {
                            graphics.DrawString(name, font, brush, new PointF(((points.Max(p => p.X) + points.Min(p => p.X))) / 2, (points.Max(p => p.Y) + points.Min(p => p.Y)) / 2));
                            return;
                        }
                        /*float x = 0, y = 0;
                        foreach (PointF point in points)
                        {
                            x += point.X;
                            y += point.Y;
                        }
                        x = x / points.Count;
                        y = y / points.Count;
                        graphics.DrawString(name, font, brush, new PointF(x, y));*/
                    }
                }
            }
            static private void drawPolygon(List<PointF> points, Color color, Graphics graphics)
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
                int colorMultiplier = 512;
                if (value == null)
                    return Color.FromArgb(255, 192, 192, 192);
                int red = Math.Max(Math.Min((int)(defaultColor + value * colorMultiplier * 2), 255), 0);
                int green = Math.Max(Math.Min((int)(defaultColor + Math.Abs((float)value) * colorMultiplier), 255), 0);
                int blue = Math.Max(Math.Min((int)(defaultColor - value * colorMultiplier * 1.5), 255), 0);
                return Color.FromArgb(255, red, green, blue);
            }
        }
    }