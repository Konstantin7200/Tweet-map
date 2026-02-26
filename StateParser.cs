using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace program
{
    static class StateParser
    {
        public static List<StateBoundary> LoadFromFile(string filePath)
        {
                string jsonContent = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;

                var result = new List<StateBoundary>();

                foreach (JsonProperty stateProperty in root.EnumerateObject())
                {
                    string stateCode = stateProperty.Name;

                    if (stateProperty.Name == "PR") continue; //Skip Puerto Rico

                    JsonElement stateData = stateProperty.Value;

                    List<List<PointF>> polygons = ExtractPolygonsUniversal(stateData);

                    var stateBoundary = new StateBoundary(stateCode, polygons);
                    result.Add(stateBoundary);
                }
                return result;
        }

        private static List<List<PointF>> ExtractPolygonsUniversal(JsonElement element)
        {
            var allPolygonsOfSate = new List<List<PointF>>();
            var currentPolygon = new List<PointF>();

            // Use a stack for depth-first traversal
            var stack = new Stack<JsonElement>();
            stack.Push(element);

            while (stack.Count > 0)
            {
                JsonElement current = stack.Pop();

                if (current.ValueKind == JsonValueKind.Array)
                {
                    if (IsCoordinatePair(current))
                    {
                        // Extract coordinates and add to current State
                        float lon = (float)current[0].GetDouble();
                        float lat = (float)current[1].GetDouble();
                        currentPolygon.Add(new PointF(lon, lat));
                    }
                    else
                    {
                        // This is a nested array - push all children
                        // Push in reverse to maintain original order
                        var children = current.EnumerateArray().Reverse().ToList();
                        foreach (var child in children)
                        {
                            stack.Push(child);
                        }

                        // If we have points in the current Coordinates, this might be the end of a State
                        if (currentPolygon.Count > 0)
                        {
                            // Check if this State is likely a complete polygon (has at least 3 points)
                            if (currentPolygon.Count >= 3)
                            {
                                allPolygonsOfSate.Add(new List<PointF>(currentPolygon));
                            }
                            currentPolygon.Clear();
                        }
                    }
                }
            }

            if (currentPolygon.Count >= 3)
            {
                allPolygonsOfSate.Add(currentPolygon);
            }

            return allPolygonsOfSate;
        }

        private static bool IsCoordinatePair(JsonElement element)
        {
            return element.ValueKind == JsonValueKind.Array &&
                   element.GetArrayLength() == 2 &&
                   element[0].ValueKind == JsonValueKind.Number &&
                   element[1].ValueKind == JsonValueKind.Number;
        }
    }
}