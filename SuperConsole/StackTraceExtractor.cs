using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace SuperConsole
{
    public static class StackTraceExtractor
    {
        public static string GetFirstClass(string stackTrace)
        {
            stackTrace = RemoveFirstLine(stackTrace);

            Regex regex = new Regex(@"/(^\w.+)\:/");
            Match match = regex.Match(stackTrace);
            if (match.Success)
                return match.Value;
            else
                return null;
        }

        public static string GetFirstFileName(string stackTrace, out int lineNumber)
        {
            string filter = @"(\w+).cs:(\d+)";

            Regex regex = new Regex(filter);
            Match match = regex.Match(stackTrace);
            lineNumber = 0;
            if (match.Success)
            {
                lineNumber = int.Parse(match.Groups[2].Value);
                return match.Groups[1].Value;
            }
            else
                return null;
        }

        public static string GetFirstPath(string stackTrace, out int lineNumber)
        {
            string filter = @"\(at (.+):(\d+)";

            Regex regex = new Regex(filter);
            Match match = regex.Match(stackTrace);
            lineNumber = 0;
            if (match.Success)
            {
                lineNumber = int.Parse(match.Groups[2].Value);
                return match.Groups[1].Value;
            }
            else
                return null;
        }

        public static string RemoveFirstLine(string s)
        {
            int index = s.IndexOf("\n") + 1;
            return s.Substring(index);
        }
    }
}