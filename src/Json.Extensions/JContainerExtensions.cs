using Newtonsoft.Json.Linq;
using System.Linq;

namespace Json.Extensions
{
    public static class JObjectExtensions
    {
        public static JContainer Path(this JContainer container, string path)
        {
            var pathItems = path.Split('.');
            var item = container;
            foreach (var pathItem in pathItems)
            {
                item = item.Bind(pathItem);
            }

            return item;
        }

        private static JContainer Bind(this JContainer container, string pathItem)
        {
            if (pathItem.IsArrayPathItem())
            {
                return (JArray)container.AddArray(pathItem.ReplaceArrayLocator());
            }
            else
            {
                return (JObject)container.AddProperty(pathItem);
            }
        }

        public static JContainer AddProperty(this JContainer container, string propertyName)
        {
            var token = container.SelectToken(propertyName);
            if (token == null)
            {
                if (container.Type == JTokenType.Object)
                {
                    var obj = (JObject)container;
                    obj.Add(propertyName, new JObject());
                    return (JContainer)obj.SelectToken(propertyName);
                }
                else if (container.Type == JTokenType.Array)
                {
                    var array = (JArray)container;
                    var prop = new JObject();
                    prop.Add(propertyName, new JObject());

                    if (array.Children().Any())
                    {
                        var child = array.Children().FirstOrDefault();
                        if (child == null)
                        {
                            array.Add(prop);
                            return prop;
                        }

                        if (child.Type == JTokenType.Object)
                        {
                            var childObject = (JObject)child;
                            childObject.Add(propertyName, new JObject());
                            return (JContainer)childObject.SelectToken(propertyName);
                        }
                    }
                    else
                    {
                        array.Add(prop);
                        return prop;
                    }
                }
            }

            return (JContainer)token;
        }

        public static JContainer AddArray(this JContainer container, string arrayName)
        {
            var token = container.SelectToken(arrayName);
            if (token == null)
            {
                if (container.Type == JTokenType.Object)
                {
                    var obj = (JObject)container;
                    obj.Add(arrayName, new JArray());
                    return (JContainer)obj.SelectToken(arrayName);
                }
                else if (container.Type == JTokenType.Array)
                {
                    var array = (JArray)container;
                    var prop = new JObject();
                    var arrayToAdd = new JArray();
                    //arrayToAdd.Add(new JObject());
                    prop.Add(arrayName, arrayToAdd);

                    if (array.Children().Any())
                    {
                        var child = array.Children().FirstOrDefault();
                        if (child == null)
                        {
                            array.Add(prop);
                            return prop;
                        }

                        if (child.Type == JTokenType.Object)
                        {
                            var childObject = (JObject)child;
                            var doubleChild = childObject.SelectToken(arrayName);
                            if (doubleChild != null)
                            {
                                return (JContainer)doubleChild;
                            }
                            else
                            {
                                childObject.Add(arrayName, arrayToAdd);
                                return (JContainer)childObject.SelectToken(arrayName);
                            }
                        }
                    }
                    else
                    {
                        array.Add(prop);
                        return prop;
                    }
                }
            }

            return (JContainer)token;
        }
    }
}
