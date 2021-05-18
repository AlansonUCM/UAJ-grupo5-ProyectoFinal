using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;

    private string input;

    private Vector2 scroll;

    public static DebugCommand HELP;

    public List<DebugCommandBase> commandList;

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
            }
        }
    }

    private void Awake()
    {
        HELP = new DebugCommand("help", "Show a list of all available commands", "help", () =>
        {
            showHelp = true;
        });

        commandList = new List<DebugCommandBase>
        {
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP
        };
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

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viweport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 90), scroll, viweport);

            for (int i = 0; i < commandList.Count; i++)
            {
                string label = $"{commandList[i].GetCommandFormat()} - {commandList[i].GetCommandDescription()}";
                Rect labelRect = new Rect(5, 20 * i, viweport.width - 100, 20);
                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        input = GUI.TextField(new Rect(10, y + 5, Screen.width - 20, 20), input);
    }
}
