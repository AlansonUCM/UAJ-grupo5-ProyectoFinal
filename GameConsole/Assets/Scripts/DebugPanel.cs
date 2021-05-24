using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public static DebugPanel Instance;

    [SerializeField]
    private DebugConsole console;

    private Vector2 scroll;

    public List<DebugInfoBase> infoList;

    public void AddInfo(DebugInfoBase info)
    {
        infoList.Add(info);
    }

    private void Init()
    {
        infoList = new List<DebugInfoBase>();
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

    private void OnGUI()
    {
        if (!console || !console.IsConsoleShowing()) return;

        GUI.Box(new Rect(0, Screen.height - 100, 230, 100), "");

        Rect viweport = new Rect(0, Screen.height - 100, 230 - 30, 20 * infoList.Count);

        scroll = GUI.BeginScrollView(new Rect(0, Screen.height - 100 + 5, 230, 90), scroll, viweport);

        for (int i = 0; i < infoList.Count; i++)
        {
            string label = $"{infoList[i].GetInfoId()}";

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
            else if (infoList[i] as DebugInfo<System.ValueType> != null)
            {
                (infoList[i] as DebugInfo<System.ValueType>).GetInfo(out System.ValueType help);
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
            else if (infoList[i] as DebugInfo<float, float> != null)
            {
                (infoList[i] as DebugInfo<float, float>).GetInfo(out float help, out float help2);
                label = $"{label} : {help} , {help2}";
            }
            else if (infoList[i] as DebugInfo<bool, bool> != null)
            {
                (infoList[i] as DebugInfo<bool, bool>).GetInfo(out bool help, out bool help2);
                label = $"{label} : {help} , {help2}";
            }
            else if (infoList[i] as DebugInfo<System.ValueType,System.ValueType> != null)
            {
                (infoList[i] as DebugInfo<System.ValueType,System.ValueType>).GetInfo(out System.ValueType help, out System.ValueType help2);
                label = $"{label} : {help} , {help2}";
            }

            Rect labelRect = new Rect(5, Screen.height - 100 + 20 * i, 230, 20);
            GUI.Label(labelRect, label);
        }

        GUI.EndScrollView();
    }
}
