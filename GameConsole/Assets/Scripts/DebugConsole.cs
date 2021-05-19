using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance;

    [SerializeField]
    private DebugPanel panel;

    private bool showConsole;
    private bool showHelp;
    private bool showInfo;

    private string input;

    private Vector2 scroll;

    private List<string> commandIdList;
    private List<string> commandFoundList;
    private int commandSelected;

    public List<DebugCommandBase> commandList;

    public void AddCommand(DebugCommandBase command)
    {
        commandList.Add(command);
        commandIdList.Add(command.GetCommandId());
    }

    public bool IsConsoleShowing()
    {
        return showConsole;
    }

    private void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    private void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            if (input.Contains(commandList[i].GetCommandId()))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<float> != null)
                {
                    (commandList[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<bool> != null)
                {
                    (commandList[i] as DebugCommand<bool>).Invoke(bool.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<int, int> != null)
                {
                    (commandList[i] as DebugCommand<int, int>).Invoke(int.Parse(properties[1]), int.Parse(properties[2]));
                }
                else if (commandList[i] as DebugCommand<float, float> != null)
                {
                    (commandList[i] as DebugCommand<float, float>).Invoke(float.Parse(properties[1]), float.Parse(properties[2]));
                }
            }
        }
    }

    private void ScrollBox(float y, bool help)
    {
        Rect viweport = new Rect(0, 0, Screen.width - 30, 20 * ((help) ? commandList.Count : panel.infoList.Count));

        scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 90), scroll, viweport);

        for (int i = 0; i < ((help) ? commandList.Count : panel.infoList.Count); i++)
        {
            string label = (help) ? $"{commandList[i].GetCommandFormat()} - {commandList[i].GetCommandDescription()}" : $"{panel.infoList[i].GetInfoDescription()}";

            Rect labelRect = new Rect(5, 20 * i, viweport.width - 100, 20);
            GUI.Label(labelRect, label);
        }
    }

    private void Init()
    {
        DebugCommand HELP = new DebugCommand("help", "Show a list of all available commands", "help", () =>
        {
            showHelp = true;
            showInfo = false;
            scroll = Vector2.zero;
        });

        DebugCommand INFO = new DebugCommand("info", "Show the description of all the info on the panel", "info", () =>
        {
            showHelp = false;
            showInfo = true;
            scroll = Vector2.zero;
        });

        commandList = new List<DebugCommandBase>
        {
            HELP,
            INFO
        };

        commandIdList = new List<string>();
        for (int i = 0; i < commandList.Count; i++)
        {
            commandIdList.Add(commandList[i].GetCommandId());
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            OnToggleDebug();
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
            OnReturn();

        float y = 0;

        if (showHelp || showInfo)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            ScrollBox(y, showHelp);

            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0);

        int length = 0;
        string oldString = input;
        if (oldString != null) length = oldString.Length;
        input = GUI.TextField(new Rect(10, y + 5, Screen.width - 20, 20), input);
        y += 30;

        if (!string.IsNullOrEmpty(input))
        {
            if (input.Length != length)
            {
                commandSelected = 0;
                commandFoundList = commandIdList.FindAll(w => w.StartsWith(input));
            }

            if (commandFoundList != null && commandFoundList.Count > 0)
            {
                GUI.Box(new Rect(0, y, Screen.width, commandFoundList.Count * 20), "");

                for (int i = 0; i < commandFoundList.Count; i++)
                {
                    string label = commandFoundList[i];
                    Rect labelRect = new Rect(5, y + (20 * i), Screen.width - 20, 20);
                    GUI.Label(labelRect, label);
                }

                GUI.Box(new Rect(0, y + 20 * commandSelected, Screen.width, 20), "");

                if (e.keyCode == KeyCode.Tab)
                    input = commandFoundList[commandSelected];

                if (e.type == EventType.Used && e.keyCode == KeyCode.DownArrow)
                    commandSelected = (commandSelected + 1) < commandFoundList.Count ? commandSelected + 1 : commandFoundList.Count - 1;
                else if (e.type == EventType.Used && e.keyCode == KeyCode.UpArrow)
                    commandSelected = (commandSelected - 1) > 0 ? commandSelected - 1 : 0;

                y += commandFoundList.Count * 20;
            }
        }
    }
}
