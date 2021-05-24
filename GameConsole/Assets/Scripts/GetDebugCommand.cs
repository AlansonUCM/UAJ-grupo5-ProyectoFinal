using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GetDebugCommand : MonoBehaviour
{

    public string id;
    public string format;
    public string description;

    [HideInInspector]
    public int amountOfParameters;
    [HideInInspector]
    public MonoBehaviour script;
    [HideInInspector]
    public string  methodName, type1;





    // Start is called before the first frame update
    void Start()
    {

        if (!string.IsNullOrEmpty(methodName))
        {
            if (amountOfParameters == 0)
            {
                DebugCommand SPAWN_CUBE = new DebugCommand(id, description, format, () =>
                {
                    script.GetType().GetMethod(methodName)?.Invoke(script,null);
                });
                DebugConsole.Instance.AddCommand(SPAWN_CUBE);
            }
            if (amountOfParameters == 1)
            {
                Type type = Type.GetType (type1);
                if (type == typeof(float))
                {
                    DebugCommand<float> SPAWN_CUBE = new DebugCommand<float>(id, description, format, (x) =>
                    {
                        script.GetType().GetMethod(methodName)?.Invoke(script, new object[1] { x });
                    });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
                else if(type== typeof(int))
                {
                    DebugCommand<int> SPAWN_CUBE = new DebugCommand<int>(id, description, format, (x) =>
                    {
                        script.GetType().GetMethod(methodName)?.Invoke(script, new object[1] { x });
                    });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
                else if(type==typeof(bool))
                {
                    DebugCommand<bool> SPAWN_CUBE = new DebugCommand<bool>(id, description, format, (x) =>
                    {
                        script.GetType().GetMethod(methodName)?.Invoke(script, new object[1] { x });
                    });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
               
            }
            else
            {
                Type type = Type.GetType(type1);
                if (type == typeof(float))
                {
                    DebugCommand<float, float> SPAWN_CUBE = new DebugCommand<float, float>(id, description, format, (x, y) =>
                     {
                         script.GetType().GetMethod(methodName)?.Invoke(script, new object[2] { x, y });
                     });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
                else if (type == typeof(int))
                {
                    DebugCommand<int, int> SPAWN_CUBE = new DebugCommand<int, int>(id, description, format, (x, y) =>
                    {
                        script.GetType().GetMethod(methodName)?.Invoke(script, new object[2] { x, y });
                    });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
                else if (type == typeof(bool))
                {
                    DebugCommand<bool, bool> SPAWN_CUBE = new DebugCommand<bool, bool>(id, description, format, (x, y) =>
                    {
                        script.GetType().GetMethod(methodName)?.Invoke(script, new object[2] { x, y });
                    });
                    DebugConsole.Instance.AddCommand(SPAWN_CUBE);
                }
            }
        }
    }
}

  