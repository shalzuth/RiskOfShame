using System.Collections.Generic;
using UnityEngine;
namespace RiskOfShame
{
    public class Logger : MonoBehaviour
    {
        struct Log
        {
            public string Message;
            public string StackTrace;
            public LogType Type;
        }

        public KeyCode toggleKey = KeyCode.BackQuote;

        List<Log> logs = new List<Log>();
        Vector2 scrollPosition;
        bool show = true;
        bool collapse;

        static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        const int margin = 50;

        Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
        Rect titleBarRect = new Rect(0, 0, 10000, 20);
        GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                show = !show;
            if (show && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log(">> " + Command);
                Command = "";

            }
        }

        void OnGUI()
        {
            if (!show)
                return;
            windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
        }
        string Command = "";
        void ConsoleWindow(int windowID)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                if (collapse)
                {
                    var messageSameAsPrevious = i > 0 && log.Message == logs[i - 1].Message;
                    if (messageSameAsPrevious)
                        continue;
                }
                GUI.contentColor = logTypeColors[log.Type];
                if (log.Type == LogType.Exception)
                    GUILayout.Label(log.StackTrace.Replace("\n", "") + " : " + log.Message);
                else
                    GUILayout.Label(log.Message);
            }
            GUILayout.EndScrollView();
            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();
            Command = GUILayout.TextField(Command);
            if (GUILayout.Button(clearLabel))
                logs.Clear();
            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUI.DragWindow(titleBarRect);
        }
        void HandleLog(string message, string stackTrace, LogType type)
        {
            logs.Add(new Log()
            {
                Message = message,
                StackTrace = stackTrace,
                Type = type,
            });
        }
    }
}