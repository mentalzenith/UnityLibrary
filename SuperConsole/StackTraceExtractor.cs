using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        public static string GetFirstFileName(string path)
        {
            string filter = @"(\w+.cs)";

            Regex regex = new Regex(filter);
            Match match = regex.Match(path);
            if (match.Success)
            {
                return match.Value;
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

        public static List<StackTraceItem> ExtractStackTraceItems(string stackTrace)
        {
            var list = new List<StackTraceItem>();
            string filter = @"(.*)\ \(at (.+):(\d+)";

            Regex regex = new Regex(filter);
            var matches = regex.Matches(stackTrace);
            foreach (Match match in matches)
            {
                var item = new StackTraceItem();
                item.methodName = match.Groups[1].Value;
                item.path = match.Groups[2].Value;
                item.lineNumber = int.Parse(match.Groups[3].Value);
                item.fileName = GetFirstFileName(item.path);
                list.Add(item);
            }
            return list;
        }
    }

    public class StackTraceItem
    {
        public string methodName;
        public string fileName;
        public string path;
        public int lineNumber;
    }
}