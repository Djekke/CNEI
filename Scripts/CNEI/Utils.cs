namespace CryoFall.CNEI.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AtomicTorch.CBND.GameApi.Extensions;

    public static class Utils
    {
        /// <summary>
        /// Split path to individual property names.
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>yield propertyName</returns>
        public static IEnumerable<string> GetPropertyNames(string path)
        {
            var propertyName = new StringBuilder();
            bool isDict = false;
            foreach (char c in path)
            {
                if (c == '[')
                    isDict = true;
                if (c == ']')
                    isDict = false;
                if (c == '.' && !isDict)
                {
                    yield return propertyName.ToString();
                    propertyName.Clear();
                }
                else
                {
                    propertyName.Append(c);
                }
            }
            yield return propertyName.ToString();
        }

        /// <summary>
        /// Check if given baseType has property described in path.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPropertyExist(Type baseType, string path)
        {
            var type = baseType;
            foreach (var prop in GetPropertyNames(path))
            {
                if (prop.Contains("["))
                {
                    string dictionary = prop.Substring(0, prop.IndexOf("["));
                    string key = prop.Substring(prop.IndexOf("[") + 1, prop.IndexOf("]") - prop.IndexOf("[") - 1);

                    var dictInfo = type.ScriptingGetProperty(dictionary);
                    if (dictInfo == null)
                    {
                        return false;
                    }
                    type = dictInfo.PropertyType.GetGenericArguments()[1];
                }
                else
                {
                    var propInfo = type.ScriptingGetProperty(prop);
                    if (propInfo == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Get from baseObject value of the property that described in path.
        /// </summary>
        /// <param name="baseObject">base object</param>
        /// <param name="path">Property name with path.</param>
        /// <returns>requested value</returns>
        public static object GetPropertyValueFromPath(object baseObject, string path)
        {
            object valueObject = baseObject;
            foreach (var prop in GetPropertyNames(path))
            {
                if (prop.Contains("["))
                {
                    string dictionary = prop.Substring(0, prop.IndexOf("["));
                    string key = prop.Substring(prop.IndexOf("[") + 1, prop.IndexOf("]") - prop.IndexOf("[") - 1);

                    var dictInfo = valueObject.GetType().ScriptingGetProperty(dictionary);
                    if (dictInfo != null)
                    {
                        var dict = dictInfo.GetValue(valueObject, null);
                        valueObject = dict.GetType().ScriptingGetProperty("Item").GetValue(dict, new object[] { key });
                    }
                }
                else
                {
                    var propInfo = valueObject.GetType().ScriptingGetProperty(prop);
                    if (propInfo != null)
                    {
                        valueObject = propInfo.GetValue(valueObject, null);
                    }
                }
            }
            return valueObject;
        }
    }
}
