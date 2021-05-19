using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfoPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DebugController controller;
    public static DebugInfo<int> HELP;
    private Vector2 scroll;

    public List<DebugInfoBase> infoList;
    private void Awake()
    {
        DebugInfo<int,int> HELP2;
        HELP = new DebugInfo<int>("Prueba", "Prueba con un entero", "Prueba", () =>
        {
            return 1;
        });
        HELP2 = new DebugInfo<int,int>("PruebaDoble", "Prueba con dos enteros", "PruebaDoble", () =>{return 2;},() => { return 27;});

        infoList = new List<DebugInfoBase>
        {
            HELP,
            HELP2,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP,
            HELP2,
            HELP,
            HELP,
            HELP,
            HELP
        };
    }
    // Update is called once per frame
    private void OnGUI()
    {
        if (!controller || !controller.getShowConsole()) return;


        GUI.Box(new Rect(0, Screen.height-100, 230, 100), "");

        Rect viweport = new Rect(0, Screen.height - 100, 230- 30, 20 * infoList.Count);

        scroll = GUI.BeginScrollView(new Rect(0, Screen.height - 100 + 5, 230, 90), scroll, viweport);

        for (int i = 0; i < infoList.Count; i++)
        {
            string label = $"{infoList[i].GetCommandFormat()}";

            if (infoList[i] as DebugInfo<int> != null)
            {
                int help;
                (infoList[i] as DebugInfo<int>).GetInfo(out help);
                label = $"{label} : {help.ToString()}";
            }

            else if (infoList[i] as DebugInfo<int,int> != null)
            {
                int help, help2;
                (infoList[i] as DebugInfo<int,int>).GetInfo(out help,out help2);
                label = $"{label} : {help.ToString()} , {help2.ToString()}";
            }


            Rect labelRect = new Rect(5, Screen.height - 100+ 20 * i,230 - 100, 20);
            GUI.Label(labelRect, label);
        }

        GUI.EndScrollView();
       

    }
}
