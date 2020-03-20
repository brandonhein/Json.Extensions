using System.Text.RegularExpressions;

namespace Json.Extensions
{
    internal static class StringExtensions
    {
        internal static string ReplaceArrayLocator(this string value)
        {
            var arrayIndex = value.Between("[", "]");

            return value.Replace(string.Concat("[", arrayIndex, "]"), "");
        }

        internal static bool IsArrayPathItem(this string pathName)
        {
            var arrayIndex = pathName.Between("[", "]");
            return !string.IsNullOrEmpty(arrayIndex) && Regex.IsMatch(arrayIndex, @"^\d");
        }

        internal static string Between(this string strSource, string strStart, string strEnd)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return string.Empty;
            }

            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
