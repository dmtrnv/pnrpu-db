using System.Text.RegularExpressions;

namespace DbLab5.Misc
{
    public static class StringExtensions
    {
        public static string GetDbName(this string connectionString)
        {
            var dbNameProperty = Regex.Match(connectionString, @"[dD]atabase=.*; ").Value;
            
            return dbNameProperty.Substring(9, dbNameProperty.Length - 11);
        }
    }
}