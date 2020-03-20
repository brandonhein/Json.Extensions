using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Json.Extensions
{
    public static class JsonParser
    {
        public static IEnumerable<string> Paths(string json)
        {
            var result = PathsAndValues(json);
            return result.Select(x => x.Key);
        }

        public static IDictionary<string, object> PathsAndValues(string json, string root = null)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var parser = ObjectOrArray(json);
                if (parser is JObject)
                {
                    var jObject = (JObject)parser;
                    foreach (var property in jObject.Properties())
                    {
                        if (property.Value.Type == JTokenType.Object)
                        {
                            var subProperties = PathsAndValues(property.Value.ToString(), property.Name);
                            foreach (var subProperty in subProperties)
                            {
                                result.Add($"{property.Name}.{subProperty.Key}", subProperty.Value);
                            }
                        }
                        else if (property.Value.Type == JTokenType.Array)
                        {
                            var children = property.Children().Children();
                            var count = 0;
                            foreach (var child in children)
                            {
                                var subProperties = PathsAndValues(child.ToString(), $"{property.Name}[{count}]");
                                foreach (var subProperty in subProperties)
                                {
                                    if (!subProperty.Key.StartsWith($"{property.Name}[{count}]"))
                                    {
                                        result.Add($"{property.Name}[{count}].{subProperty.Key}", subProperty.Value);
                                    }
                                    else
                                    {
                                        result.Add($"{subProperty.Key}", subProperty.Value);
                                    }
                                }
                                count++;
                            }
                        }
                        else
                        {
                            result.Add(property.Name, property.Value);
                        }
                    }
                }
                else if (parser is JArray)
                {
                    var jArray = (JArray)parser;
                    var children = jArray.Children();
                    var count = 0;
                    foreach (var child in children)
                    {
                        var subProperties = PathsAndValues(child.ToString(), $"[{count}]");
                        foreach (var subProperty in subProperties)
                        {
                            if (!subProperty.Key.StartsWith($"[{count}]"))
                            {
                                result.Add($"[{count}].{subProperty.Key}", subProperty.Value);
                            }
                            else
                            {
                                result.Add($"{subProperty.Key}", subProperty.Value);
                            }
                        }
                        count++;
                    }
                }
            }
            catch
            {
                result.Add($"{root}", ParseToValue(json));
            }

            return result;
        }

        private static object ParseToValue(string json)
        {
            if (int.TryParse(json, out var value))
            {
                return value;
            }

            if (decimal.TryParse(json, out var value2))
            {
                return value2;
            }

            if (bool.TryParse(json, out var value3))
            {
                return value3;
            }

            if (DateTime.TryParse(json, out var value4))
            {
                return value4;
            }

            return json;
        }

        private static object ObjectOrArray(string json)
        {
            try
            {
                return JArray.Parse(json);
            }
            catch
            { }

            try
            {
                return JObject.Parse(json);
            }
            catch
            { }

            throw new Exception("JsonParse couldn't figure out if json is an array or object");
        }
    }
}
