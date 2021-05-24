using UnityEngine;
using UnityEditor;
using System;
using System.Numerics;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


[CustomEditor(typeof(GetDebugCommand), true), Serializable]
public class GetCommandEditor :Editor
{
    private int amountOfParameters;
    private List<string> names1, methodsName1;
    private int selectedScript1,selectedMethod;
    private string type;
    private MonoBehaviour[] scripts;
    private string id;
    private string format;
    private string description;
    private int elected;
    // Start is called before the first frame update
    void OnEnable()
    {
        id = serializedObject.FindProperty("id").stringValue;
        format = serializedObject.FindProperty("format").stringValue;
        description = serializedObject.FindProperty("description").stringValue;


        scripts = ((GetDebugCommand)target).gameObject.GetComponents<MonoBehaviour>();
        amountOfParameters = serializedObject.FindProperty("amountOfParameters").intValue;

        
        type = serializedObject.FindProperty("type1").stringValue;
        if (type == typeof(int).ToString())
        {
            elected = 0;
        }
        else if (type == typeof(float).ToString())
        {
            elected = 1;
        }
        else if(type == typeof(bool).ToString())
        {
            elected = 2;
        }
        names1 = new List<string>(scripts.Length + 1);
        names1.Add("None");

        for (int i = 1; i < scripts.Length + 1; i++)
        {
            if (target != scripts[i - 1])
            {
                names1.Add(scripts[i - 1].GetType().Name);
            }
        }

        if (serializedObject.FindProperty("script").objectReferenceValue != null)
        {
            selectedScript1 = names1.IndexOf(((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().Name);
            if (selectedScript1 == -1)
                selectedScript1 = 0;
        }
        else
        {
            selectedScript1 = 0;
        }

        if (serializedObject.FindProperty("methodName").stringValue != "")
        {
            methodsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().
                GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(m => m.ReturnType == typeof(void)).
                        Select(m => m.Name + "()").ToList();

            selectedMethod = methodsName1.IndexOf(serializedObject.FindProperty("methodName").stringValue + "()");
        }

    }

    // Update is called once per frame
    public override VisualElement CreateInspectorGUI()
    {
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        id = EditorGUILayout.TextField("Id", id);
        serializedObject.FindProperty("id").stringValue = id;

        format = EditorGUILayout.TextField("Format", format);
        serializedObject.FindProperty("format").stringValue = format;

        description = EditorGUILayout.TextField("Description", description);
        serializedObject.FindProperty("description").stringValue = description;


        EditorGUI.BeginChangeCheck();


        amountOfParameters = EditorGUILayout.IntPopup("Amount of parameters ", amountOfParameters, new string[3] { "0","1", "2" }, new int[3] { 0,1, 2 });
        serializedObject.FindProperty("amountOfParameters").intValue = amountOfParameters;
        
        if (amountOfParameters > 0)
        {
            elected = EditorGUILayout.IntPopup("Type of parameters ", elected, new string[3] { "int", "float", "bool" }, new int[3] { 0, 1, 2 });

            if(elected==0)
            {
                type = typeof(int).ToString();
            }
            else if(elected==1)
            {
                type = typeof(float).ToString();
            }
            else
            {
                type = typeof(bool).ToString();
            }
            serializedObject.FindProperty("type1").stringValue = type;

        }

        EditorGUILayout.Space();

        selectedScript1 = EditorGUILayout.Popup("Script", selectedScript1, names1.ToArray());
        if (selectedScript1 == 0)
        {
            return;
        }



        var script = scripts[selectedScript1 - 1];
        serializedObject.FindProperty("script").objectReferenceValue = script;


        if (EditorGUI.EndChangeCheck())
        {
            selectedMethod = 0;


            // serializedObject.FindProperty("counter").boolValue = false;

            methodsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().
                GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(m => m.ReturnType == typeof(void)).
                        Select(m => m.Name + "()").ToList();

            if (methodsName1.Count > 0)
                ((GetDebugCommand)target).methodName = methodsName1[0];

            
        }

       
        EditorGUI.BeginChangeCheck();

        selectedMethod = EditorGUILayout.Popup("Method", selectedMethod, methodsName1.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            if (methodsName1.Count > 0)
                serializedObject.FindProperty("methodName").stringValue = methodsName1[selectedMethod].Substring(0, methodsName1[selectedMethod].Length - 2);
        }
        

        serializedObject.ApplyModifiedProperties();
    }
}


