using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugInfo : SingletonMonoManager<DebugInfo>
{
    public static int candidatesCount;
    public static int connectedObjectCount;

    Dictionary<string,Info> infos;
    Queue<Log> logs;

    void Awake()
    {
        infos = new Dictionary<string, Info>();
        logs = new Queue<Log>();
        Application.logMessageReceived += OnLogReceived;
    }

    public static void SetText(string name, object content)
    {
        var info = new Info();
        info.content = content;
        info.color = Color.white;
        Instance.infos[name] = info;
    }

    public static void RemoveText(string name)
    {
        if (Instance.infos.ContainsKey(name))
            Instance.infos.Remove(name);
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 30;

        foreach (var pair in infos)
            DrawLabel(pair.Key + pair.Value.content.ToString());

        DrawLabel("");
        DrawLog();       
    }

    void DrawLabel(string s)
    {
        DrawLabel(s, Color.white);
    }

    void DrawLabel(string s, Color color)
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.normal.textColor = color;
        GUILayout.Label(s, style);
    }

    void DrawLog()
    {
        if (logs.Count > 30)
            logs.Dequeue();

        string text = "";

        foreach (var log in logs)
        {
            text += log.message + "\n";
            if (log.logType == LogType.Exception)
                text += @"<color=red>" + log.stackTrace + @"</color>";
        }

        DrawLabel(text);
    }

    void OnLogReceived(string message, string stackTrace, LogType logType)
    {
        var log = new Log();
        log.message = message;
        log.stackTrace = stackTrace;
        log.logType = logType;
        logs.Enqueue(log);
    }

    class Info
    {
        public object content;
        public Color color;
    }

    class Log
    {
        public string message;
        public string stackTrace;
        public LogType logType;
    }
}
