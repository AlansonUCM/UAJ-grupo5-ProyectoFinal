using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;
    private bool showInfo;

    private string input;

    private Vector2 scroll;

    private Vector2 scroll2;

    private int selected;
  

    public static DebugCommand HELP;

    public List<DebugCommandBase> commandList;
    List<string> words;
    List<string> found;


    [SerializeField]
    private DebugInfoPanel Info;

    public bool getShowConsole() { return showConsole; }

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
        words = new List<string>();
        DebugCommand INFO;
        INFO = new DebugCommand("info", "Show the description of all the info on the panel", "info", () =>
        {
            showInfo = true;
            showHelp = false;
            scroll2 = Vector2.zero;
        });
        HELP = new DebugCommand("help", "Show a list of all available commands", "help", () =>
        {
            showHelp = true;
            showInfo = false;
            scroll = Vector2.zero;
        });

        commandList = new List<DebugCommandBase>
        {
            HELP,
            INFO,
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
        for (int i = 0; i < commandList.Count; i++)
        {
            words.Add(commandList[i].GetCommandFormat());
        }

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
        if(showInfo)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viweport = new Rect(0, 0, Screen.width - 30, 20 * Info.infoList.Count);

            scroll2 = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 90), scroll2, viweport);

            for (int i = 0; i < Info.infoList.Count; i++)
            {
                string label = $"{Info.infoList[i].GetCommandFormat()} - {Info.infoList[i].GetCommandDescription()}";
                Rect labelRect = new Rect(5, 20 * i, viweport.width - 100, 20);
                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();
            y += 100;
        }
        
       

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        string oldString = input;
        int length = 0;
        input = GUI.TextField(new Rect(10, y + 5, Screen.width - 20, 20), input);
        y += 30;
        if(oldString!=null)
        {
            length = oldString.Length;
        }



        if (!string.IsNullOrEmpty(input) && input.Length != length)
        {
            selected = 0;
            found = words.FindAll(w => w.StartsWith(input));
        }


        if(found!=null&& found.Count>0 && !string.IsNullOrEmpty(input))
        {
            GUI.Box(new Rect(0, y, Screen.width, found.Count*20), "");
            for (int i = 0; i < found.Count; i++)
            {
                string label = found[i];
                Rect labelRect = new Rect(5, y+ (20 * i), Screen.width - 20, 20);
                GUI.Label(labelRect, label);
            }
         
            GUI.Box(new Rect(0, y+20*selected, Screen.width, 20), "");

            if (e.keyCode == KeyCode.Tab)
                input = found[selected];
            if (e.type == EventType.Used && e.keyCode == KeyCode.DownArrow)
                selected = (selected + 1) < found.Count ? selected + 1 : found.Count - 1;

            else if (e.type==EventType.Used && e.keyCode == KeyCode.UpArrow)
                selected = (selected - 1) > 0 ? selected - 1 : 0;
             
            y += found.Count * 20;
            
        }

    }
}
