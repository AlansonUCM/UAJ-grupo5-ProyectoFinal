using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField]
    private DebugConsole console;

    private Vector2 scroll;

    public List<DebugInfoBase> infoList;

    private void Awake()
    {
        DebugInfo<int> HELP = new DebugInfo<int>("Prueba", "Prueba con un entero", "Prueba", () =>
        {
            return 1;
        });

        DebugInfo<int, int> HELP2 = new DebugInfo<int, int>("PruebaDoble", "Prueba con dos enteros", "PruebaDoble", () =>
        {
            return 2;
        }, () =>
        {
            return 27;
        });

        DebugInfo<bool> HELP3 = new DebugInfo<bool>("Boolean", "Prueba con bool", "Boolean", () =>
        {
            return true;
        });

        infoList = new List<DebugInfoBase>
        {
            HELP,
            HELP2,
            HELP3,
            HELP,
            HELP2,
            HELP3,
            HELP,
            HELP2,
            HELP3
        };
    }

    private void OnGUI()
    {
        if (!console || !console.IsConsoleShowing()) return;

        GUI.Box(new Rect(0, Screen.height - 100, 230, 100), "");

        Rect viweport = new Rect(0, Screen.height - 100, 230 - 30, 20 * infoList.Count);

        scroll = GUI.BeginScrollView(new Rect(0, Screen.height - 100 + 5, 230, 90), scroll, viweport);

        for (int i = 0; i < infoList.Count; i++)
        {
            string label = $"{infoList[i].GetInfoFormat()}";

            if (infoList[i] as DebugInfo<int> != null)
            {
                (infoList[i] as DebugInfo<int>).GetInfo(out int help);
                label = $"{label} : {help}";
            }
            else if (infoList[i] as DebugInfo<float> != null)
            {
                (infoList[i] as DebugInfo<float>).GetInfo(out float help);
                label = $"{label} : {help}";
            }
            else if (infoList[i] as DebugInfo<bool> != null)
            {
                (infoList[i] as DebugInfo<bool>).GetInfo(out bool help);
                label = $"{label} : {help}";
            }
            else if (infoList[i] as DebugInfo<int, int> != null)
            {
                (infoList[i] as DebugInfo<int, int>).GetInfo(out int help, out int help2);
                label = $"{label} : {help} , {help2}";
            }

            Rect labelRect = new Rect(5, Screen.height - 100 + 20 * i, 230 - 100, 20);
            GUI.Label(labelRect, label);
        }

        GUI.EndScrollView();
    }
}
