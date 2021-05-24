using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDebugInfo : MonoBehaviour
{
    
    public string id;
    
    public string description;

    [HideInInspector]
    public int amountOfGetter;
    [HideInInspector]
    public MonoBehaviour script;
    [HideInInspector]
    public MonoBehaviour script2;
    [HideInInspector]
    public string fieldName, methodName;
    [HideInInspector]
    public string fieldName2, methodName2;

    private System.ValueType value1, value2;

   


    // Start is called before the first frame update
    void Start()
    {
       
        if ((!string.IsNullOrEmpty(fieldName) || !string.IsNullOrEmpty(methodName)))
        {
            if (amountOfGetter == 1)
            {
                DebugInfo<System.ValueType> INFO = new DebugInfo<System.ValueType>(id, description, () =>
                {
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        value1 = (System.ValueType)script.GetType().GetField(fieldName)?.GetValue(script);

                    }
                    else if (!string.IsNullOrEmpty(methodName))
                    {

                        value1 = (System.ValueType)script.GetType().GetMethod(methodName)?.Invoke(script, null);
                    }
                    return value1;
                });
                DebugPanel.Instance.AddInfo(INFO);
            }
            else
            {
                if ((!string.IsNullOrEmpty(fieldName2) || !string.IsNullOrEmpty(methodName2)))
                {
                    DebugInfo<System.ValueType, System.ValueType> INFO = new DebugInfo<System.ValueType, System.ValueType>(id, description, () =>
                    {
                        if (!string.IsNullOrEmpty(fieldName))
                        {
                            value1 = (System.ValueType)script.GetType().GetField(fieldName)?.GetValue(script);

                        }
                        else if (!string.IsNullOrEmpty(methodName))
                        {

                            value1 = (System.ValueType)script.GetType().GetMethod(methodName)?.Invoke(script, null);
                        }
                        return value1;
                    }, () =>
                    {
                        if (!string.IsNullOrEmpty(fieldName2))
                        {
                            value2 = (System.ValueType)script.GetType().GetField(fieldName2)?.GetValue(script);

                        }
                        else if (!string.IsNullOrEmpty(methodName2))
                        {
                            value2 = (System.ValueType)script.GetType().GetMethod(methodName2)?.Invoke(script, null);
                        }
                        return value2;
                    });

                    DebugPanel.Instance.AddInfo(INFO);
                }
            }
        }
    }

    
}
