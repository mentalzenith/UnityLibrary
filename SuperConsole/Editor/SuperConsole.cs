using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;


namespace SuperConsole
{
    public class SuperConsole : EditorWindow
    {
        public List<LogMessage> messages;

        GUIContent content;
        bool collapse, clearOnPlay, pauseOnError;
        bool showLog, showWarning, showError;
        int logCount, warningCount, errorCount;
        Vector2 stackViewScrollPosition, logViewScrollPosition;
        LogMessage selected;

        double clickTime;
        double doubleClickTime = 0.3;

        float logHeight = 10;
        float count;

        string text;

        void OnEnable()
        {
            messages = new List<LogMessage>();
            Application.logMessageReceived += OnLogMessageReceived;
            EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
            content = new GUIContent();
            titleContent = GetIconContent("icons/UnityEditor.ConsoleWindow.png", "SConsole");
            
            LoadPreference();
        }

        void OnDisable()
        {
            SavePreference();
        }

        void LoadPreference()
        {
            collapse = EditorPrefs.GetBool("SC-collapse");
            clearOnPlay = EditorPrefs.GetBool("SC-clearOnPlay");
            pauseOnError = EditorPrefs.GetBool("SC-pauseOnError");

            showLog = EditorPrefs.GetBool("SC-showLog");
            showWarning = EditorPrefs.GetBool("SC-showWarning");
            showError = EditorPrefs.GetBool("SC-showError");
        }

        void SavePreference()
        {
            EditorPrefs.SetBool("SC-collapse", collapse);
            EditorPrefs.SetBool("SC-clearOnPlay", clearOnPlay);
            EditorPrefs.SetBool("SC-pauseOnError", pauseOnError);

            EditorPrefs.SetBool("SC-showLog", showLog);
            EditorPrefs.SetBool("SC-showWarning", showWarning);
            EditorPrefs.SetBool("SC-showError", showError);
        }

        void OnPlayModeStateChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                //enter playmode
                if (clearOnPlay)
                    Clear();
            }
        }

        void OnGUI()
        {
            //Header
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
                Clear();

            GUILayout.Space(5);

            collapse = GUILayout.Toggle(collapse, "Collapse", EditorStyles.toolbarButton);
            clearOnPlay = GUILayout.Toggle(clearOnPlay, "Clear on Play", EditorStyles.toolbarButton);
            pauseOnError = GUILayout.Toggle(pauseOnError, "Error Pause", EditorStyles.toolbarButton);

            GUILayout.Label(text, EditorStyles.toolbarButton);

            GUILayout.FlexibleSpace();

            showLog = GUILayout.Toggle(showLog, GetIconContent("icons/console.infoicon.sml.png", logCount.ToString()), EditorStyles.toolbarButton);
            showWarning = GUILayout.Toggle(showWarning, GetIconContent("icons/console.warnicon.sml.png", warningCount.ToString()), EditorStyles.toolbarButton);
            showError = GUILayout.Toggle(showError, GetIconContent("icons/console.erroricon.sml.png", errorCount.ToString()), EditorStyles.toolbarButton);
            GUILayout.EndHorizontal();

            //content
            EditorGUILayout.BeginHorizontal();
            DrawStackView();
            DrawLogView();
            EditorGUILayout.EndHorizontal();
        }

        void DrawStackView()
        {
            stackViewScrollPosition = EditorGUILayout.BeginScrollView(stackViewScrollPosition);

            EditorGUILayout.EndScrollView();
        }

        private Vector2 scrollPos = Vector2.zero;
        float currentScrollViewHeight;
        bool resize = false;
        Rect cursorChangeRect;

        void ResizeScrollView()
        {
            GUI.DrawTexture(cursorChangeRect, EditorGUIUtility.whiteTexture);
            EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.mouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
            {
                resize = true;
            }
            if (resize)
            {
                currentScrollViewHeight = Event.current.mousePosition.y;
                cursorChangeRect.Set(cursorChangeRect.x, currentScrollViewHeight, cursorChangeRect.width, cursorChangeRect.height);
            }
            if (Event.current.type == EventType.MouseUp)
                resize = false;        
        }

        void DrawLogView()
        {
            logViewScrollPosition = EditorGUILayout.BeginScrollView(logViewScrollPosition);

            foreach (var message in messages)
                DrawLog(message);

            EditorGUILayout.EndScrollView();
        }

        void DrawLog(LogMessage message)
        {
            if (Filtered(message.logType))
                return;

            EditorGUILayout.BeginHorizontal();
            DrawLogColorLabel(message);
            DrawLogMessage(message);

            EditorGUILayout.EndHorizontal();

        }

        void DrawLogColorLabel(LogMessage message)
        {
            switch (message.logType)
            {
                case LogType.Log:
                    GUI.backgroundColor = Color.white;
                    break;
                case LogType.Warning:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case LogType.Error:
                    GUI.backgroundColor = Color.red;
                    break;
                case LogType.Exception:
                    GUI.backgroundColor = Color.red;
                    break;
            }
            GUILayout.Label("", EditorStyles.helpBox, GUILayout.Width(10), GUILayout.Height(logHeight));
        }

        void DrawLogMessage(LogMessage message)
        {
            //GetIconContent(message.logType).text = message.message;
            content.text = message.message;

            var style = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).textField);
            style.alignment = TextAnchor.MiddleLeft;
            style.richText = true;


            if (message == selected)
            {
                GUI.backgroundColor = new Color32(62, 125, 231, 255);
                style.normal.textColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = Color.white;
            }

            if (GUILayout.Button(content, style, GUILayout.ExpandWidth(true)))
            {
                if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                    OnDoubleClick();
                else
                    selected = message;
                clickTime = EditorApplication.timeSinceStartup;
            }
        }

        void OnDoubleClick()
        {
            int lineNumber = 0;
            string fileName = StackTraceExtractor.GetFirstPath(selected.stackTrace,out lineNumber);
            text = fileName+" "+lineNumber;
            EditorTools.OpenScript(fileName, lineNumber);
        }

        bool Filtered(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    if (showLog)
                        return false;
                    break;
                case LogType.Warning:
                    if (showWarning)
                        return false;
                    break;
                case LogType.Error:
                    if (showError)
                        return false;
                    break;
                case LogType.Exception:
                    if (showError)
                        return false;
                    break;
            }
            return true;
        }

        void Clear()
        {
            messages.Clear();
            logCount = 0;
            warningCount = 0;
            errorCount = 0;
        }

        GUIContent GetIconContent(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    return GetIconContent("icons/console.infoicon.png", "");
                case LogType.Warning:
                    return GetIconContent("icons/console.warnicon.png", "");
                case LogType.Error:
                    return GetIconContent("icons/console.erroricon.png", "");
            }
            return content;
        }

        GUIContent GetIconContent(string path, string text)
        {
            Texture tex = EditorGUIUtility.Load(path) as Texture2D;
            
            var content = new GUIContent();
            content.text = text;
            content.image = tex;
            return content;
        }

        void OnLogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            count++;

            var newMessage = new LogMessage();
            newMessage.message = condition;
            newMessage.stackTrace = stackTrace;
            newMessage.logType = logType;
            messages.Add(newMessage);
            EditorUtility.SetDirty(GetWindow(typeof(SuperConsole)));

            //update message count
            switch (logType)
            {
                case LogType.Log:
                    logCount++;
                    break;
                case LogType.Warning:
                    warningCount++;
                    break;
                case LogType.Error:
                    errorCount++;
                    break;
                case LogType.Exception:
                    errorCount++;
                    break;
                case LogType.Assert:
                    errorCount++;
                    break;
            }
        }

        public class LogMessage
        {
            public string message;
            public string stackTrace;
            public LogType logType;
        }

        [MenuItem("Window/SuperConsole")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SuperConsole));
        }
    }
}