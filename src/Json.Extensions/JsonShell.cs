using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Json.Extensions
{
    public static class JsonShell
    {
        public static JContainer GenerateFromPaths(IEnumerable<string> paths)
        {
            JContainer obj;

            var firstPath = paths.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstPath) && firstPath.StartsWith("["))
            {
                obj = JArray.Parse("[]");
            }
            else
            {
                obj = JObject.Parse("{}");
            }

            foreach (var path in paths)
            {
                try
                {
                    obj.Path(path);
                }
                catch
                { }
            }

            return obj;
        }
    }
}
