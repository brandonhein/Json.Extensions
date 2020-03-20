using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Json.Extensions
{
    public static class JsonSimplifier
    {
        public static JToken Simplify(IDictionary<string, object> values)
        {
            var json = JsonShell.GenerateFromPaths(values.Select(x => x.Key));
            return Simplify(json, values);
        }

        public static JToken Simplify(JContainer jsonShell, IDictionary<string, object> values)
        {
            foreach (var item in values)
            {
                jsonShell.SetValueByPath(item.Key, item.Value);
            }

            return jsonShell;
        }

        public static JToken ToJson(this IDictionary<string, object> values)
        {
            return Simplify(values);
        }

        public static void SetValueByPath(this JContainer obj, string path, object value)
        {
            var items = new string[] { ".", "[", "]" };
            var segments = path.Split(items, StringSplitOptions.RemoveEmptyEntries);
            var lastSegment = segments.LastOrDefault();

            var stringQuery = "$";
            foreach (var segment in segments.Where(x => x != lastSegment))
            {
                stringQuery = string.Concat(stringQuery, Regex.IsMatch(segment, @"^\d") ? $"[{segment}]" : $"['{segment}']");
            }

            var token = obj.SelectToken(stringQuery);
            if (token != null)
            {
                if (token.Type == JTokenType.Array)
                {
                    ((JArray)token).Add(new JValue(value));
                }
                else
                {
                    token[lastSegment] = new JValue(value);
                }
            }
            else
            {
                var segementToRemove = segments.Reverse().Skip(1).FirstOrDefault();
                stringQuery = stringQuery.Replace($"[{segementToRemove}]", "");

                token = obj.SelectToken(stringQuery);
                if (token != null && token.Type == JTokenType.Array)
                {
                    var array = (JArray)token;
                    var child = array.Children().FirstOrDefault();
                    if (child.Type == JTokenType.Object)
                    {
                        var blankChildToAdd = new JObject();
                        var childObj = (JObject)child;
                        var props = childObj.Properties();
                        foreach (var prop in props)
                        {
                            blankChildToAdd.Add(prop.Name, new JObject());
                        }

                        array.Add(blankChildToAdd);
                    }

                    SetValueByPath(obj, path, value);
                }
                else
                {
                    SpecialArraySetting(obj, path, value);
                }
            }
        }

        private static void SpecialArraySetting(this JContainer obj, string path, object value)
        {
            var items = new string[] { ".", "[", "]" };
            var segments = path.Split(items, StringSplitOptions.RemoveEmptyEntries);
            var lastSegment = segments.LastOrDefault();

            var stringQuery = "$";

            var querySegements = segments.Count() > 1 ? segments.Take(segments.Count() - 1).ToArray() : segments;
            foreach (var segment in querySegements)
            {
                stringQuery = string.Concat(stringQuery, Regex.IsMatch(segment, @"^\d") ? $"[{segment}]" : $"['{segment}']");
            }

            var token = obj.SelectToken(stringQuery);
            if (token != null)
            {
                if (token.Type == JTokenType.Array)
                {
                    ((JArray)token).Add(new JValue(value));
                }
                else
                {
                    token[lastSegment] = new JValue(value);
                }
            }
            else
            {
                var segementToRemove = segments.Reverse().Skip(1).FirstOrDefault();
                stringQuery = stringQuery.ReplaceTheLast($"[{segementToRemove}]", "");

                token = obj.SelectToken(stringQuery);
                if (token != null && token.Type == JTokenType.Array)
                {
                    var array = (JArray)token;
                    var child = array.Children().FirstOrDefault();
                    if (child.Type == JTokenType.Object)
                    {
                        var blankChildToAdd = new JObject();
                        var childObj = (JObject)child;
                        var props = childObj.Properties();
                        foreach (var prop in props)
                        {
                            blankChildToAdd.Add(prop.Name, new JObject());
                        }

                        array.Add(blankChildToAdd);
                    }

                    SetValueByPath(obj, path, value);
                }
            }
        }
    }
}
