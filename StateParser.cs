using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace StateParserProject
{
    struct StateParser
    {
        private string FilePath { get; set; }

        public StateParser(string filePath)
        {
            FilePath = filePath;
        }

        public List<StateBoundary> LoadFromFile(string filePath)
        {
                string jsonContent = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;

                var result = new List<StateBoundary>();

                foreach (JsonProperty stateProperty in root.EnumerateObject())
                {
                    string stateCode = stateProperty.Name;
                    JsonElement stateData = stateProperty.Value;

                    List<List<PointF>> polygons = ExtractPolygonsUniversal(stateData);

                    var stateBoundary = new StateBoundary(stateCode, polygons);
                    result.Add(stateBoundary);
                }
                return result;
        }

        private List<List<PointF>> ExtractPolygonsUniversal(JsonElement element)
        {
            var polygons = new List<List<PointF>>();
            var currentRing = new List<PointF>();

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
                        // Extract coordinates and add to current ring
                        float lon = (float)current[0].GetDouble();
                        float lat = (float)current[1].GetDouble();
                        currentRing.Add(new PointF(lon, lat));
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

                        // If we have points in the current ring, this might be the end of a ring
                        if (currentRing.Count > 0)
                        {
                            // Check if this ring is likely a complete polygon (has at least 3 points)
                            if (currentRing.Count >= 3)
                            {
                                polygons.Add(new List<PointF>(currentRing));
                            }
                            currentRing.Clear();
                        }
                    }
                }
            }

            // Add any remaining ring
            if (currentRing.Count >= 3)
            {
                polygons.Add(currentRing);
            }

            return polygons;
        }

        private void FindAllRings(JsonElement element, List<PointF> currentRing, List<List<PointF>> allRings)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                if (IsCoordinatePair(element))
                {
                    // Found a coordinate - add to current ring
                    float lon = (float)element[0].GetDouble();
                    float lat = (float)element[1].GetDouble();
                    currentRing.Add(new PointF(lon, lat));
                }
                else
                {
                    // Start a new potential ring
                    var newRing = new List<PointF>();
                    bool foundCoordinates = false;

                    foreach (JsonElement child in element.EnumerateArray())
                    {
                        if (child.ValueKind == JsonValueKind.Array)
                        {
                            if (IsCoordinatePair(child))
                            {
                                foundCoordinates = true;
                                float lon = (float)child[0].GetDouble();
                                float lat = (float)child[1].GetDouble();
                                newRing.Add(new PointF(lon, lat));
                            }
                            else
                            {
                                // Recursively process deeper arrays
                                FindAllRings(child, currentRing, allRings);
                            }
                        }
                    }

                    if (foundCoordinates && newRing.Count > 0)
                    {
                        allRings.Add(newRing);
                    }
                }
            }
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