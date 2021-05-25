using UnityEngine;
using UnityEditor;
using System;
using System.Numerics;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


[CustomEditor(typeof(GetDebugInfo), true), Serializable]
public class GetInfoEditor : Editor
{
    private int amountOfGetter;
    private List<string> names1, fieldsName1, methodsName1;
    private List<string> names2, fieldsName2, methodsName2;
    private int selectedScript1,selectedScript2, fieldOrMethod, selectedField, selectedMethod , fieldOrMethod2, selectedField2, selectedMethod2;
    private MonoBehaviour[] scripts;
    private string id;

    private string description;

    // Start is called before the first frame update
    void OnEnable()
    {
        id = serializedObject.FindProperty("id").stringValue;
        description = serializedObject.FindProperty("description").stringValue;


        scripts = ((GetDebugInfo)target).gameObject.GetComponents<MonoBehaviour>();
        amountOfGetter = serializedObject.FindProperty("amountOfGetter").intValue;
        
        names1 = new List<string>(scripts.Length + 1);
        names1.Add("None");

        names2 = new List<string>(scripts.Length + 1);
        names2.Add("None");

        for (int i = 1; i < scripts.Length + 1; i++)
        {
            if (target != scripts[i - 1])
            {
                names1.Add(scripts[i - 1].GetType().Name);
                names2.Add(scripts[i - 1].GetType().Name);
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
        if (serializedObject.FindProperty("script2").objectReferenceValue != null)
        { 
            selectedScript2 = names2.IndexOf(((MonoBehaviour)serializedObject.FindProperty("script2").objectReferenceValue).GetType().Name);

            if (selectedScript2 == -1)
                selectedScript2 = 0;
        }
        else
        {
            selectedScript2 = 0;
        }

        if (serializedObject.FindProperty("fieldName").stringValue != "")
        {
            fieldsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().GetFields().Where(m => m.FieldType != typeof(void) && m.FieldType.IsValueType).Select(f => f.Name).ToList();

            fieldOrMethod = 0;
            selectedField = fieldsName1.IndexOf(serializedObject.FindProperty("fieldName").stringValue);
        }
        else if (serializedObject.FindProperty("methodName").stringValue != "")
        {
            methodsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().
                GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(m => m.ReturnType != typeof(void) && m.ReturnType.IsValueType).
                        Select(m => m.Name + "()").ToList();
            
            fieldOrMethod = 1;
            selectedMethod = methodsName1.IndexOf(serializedObject.FindProperty("methodName").stringValue + "()");
        }

        
        if (serializedObject.FindProperty("fieldName2").stringValue != "")
        {
            fieldsName2 = ((MonoBehaviour)serializedObject.FindProperty("script2").objectReferenceValue).GetType().GetFields().Where(m => m.FieldType != typeof(void) && m.FieldType.IsValueType).Select(f => f.Name).ToList();

            fieldOrMethod2 = 0;
            selectedField2 = fieldsName2.IndexOf(serializedObject.FindProperty("fieldName2").stringValue);
        }
        else if (serializedObject.FindProperty("methodName2").stringValue != "")
        {
            methodsName2 = ((MonoBehaviour)serializedObject.FindProperty("script2").objectReferenceValue).GetType().
                GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(m => m.ReturnType != typeof(void) && m.ReturnType.IsValueType).
                        Select(m => m.Name + "()").ToList();

            fieldOrMethod2 = 1;
            selectedMethod2 = methodsName2.IndexOf(serializedObject.FindProperty("methodName2").stringValue + "()");
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

        description = EditorGUILayout.TextField("Description", description);
        serializedObject.FindProperty("description").stringValue = description;


        EditorGUI.BeginChangeCheck();


        amountOfGetter = EditorGUILayout.IntPopup("Amount of getter ", amountOfGetter, new string[2] { "1", "2" }, new int[2] { 1, 2 });
        serializedObject.FindProperty("amountOfGetter").intValue = amountOfGetter;

        EditorGUILayout.Space();

        selectedScript1 = EditorGUILayout.Popup("Script", selectedScript1, names1.ToArray());
        if (selectedScript1 == 0)
        {
            return;
        }

       

        var script = scripts[selectedScript1 - 1];
        serializedObject.FindProperty("script").objectReferenceValue = script;

        fieldOrMethod = EditorGUILayout.Popup("Field or Method", fieldOrMethod, new string[2] { "Field", "Method" });

        if (EditorGUI.EndChangeCheck())
        {
            selectedMethod = 0;
            selectedField = 0;
            if (fieldOrMethod == 0)
            {
                fieldsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().GetFields().Where(m => m.FieldType!=typeof(void)&& m.FieldType.IsValueType).Select(f => f.Name).ToList();

                ((GetDebugInfo)target).methodName = null;
                if (fieldsName1.Count > 0)
                    ((GetDebugInfo)target).fieldName = fieldsName1[selectedField];

                selectedMethod = 0;
            }
            else
            {

                methodsName1 = ((MonoBehaviour)serializedObject.FindProperty("script").objectReferenceValue).GetType().
                    GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                        Where(m => m.ReturnType != typeof(void) && m.ReturnType.IsValueType).
                            Select(m => m.Name + "()").ToList();

                ((GetDebugInfo)target).fieldName = null;
                if (methodsName1.Count > 0)
                    ((GetDebugInfo)target).methodName = methodsName1[0];

                selectedField = 0;
            }
        }

        if (fieldOrMethod == 0)
        {
            selectedField = EditorGUILayout.Popup("Field", selectedField, fieldsName1.ToArray());
            if (fieldsName1.Count > 0)
                serializedObject.FindProperty("fieldName").stringValue = fieldsName1[selectedField].Substring(0, fieldsName1[selectedField].Length );

        }
        else
        {
            EditorGUI.BeginChangeCheck();

            selectedMethod = EditorGUILayout.Popup("Method", selectedMethod, methodsName1.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                ((GetDebugInfo)target).fieldName = null;
                if (methodsName1.Count > 0)
                    serializedObject.FindProperty("methodName").stringValue = methodsName1[selectedMethod].Substring(0, methodsName1[selectedMethod].Length-2);
            }
        }

        EditorGUILayout.Space();

        if (amountOfGetter == 2)
        {
            EditorGUI.BeginChangeCheck();
            selectedScript2 = EditorGUILayout.Popup("Script", selectedScript2, names2.ToArray());
            if (selectedScript2 == 0)
            {
                return;
            }

            script = scripts[selectedScript2 - 1];
            serializedObject.FindProperty("script2").objectReferenceValue = script;


            fieldOrMethod2 = EditorGUILayout.Popup("Field or Method", fieldOrMethod2, new string[2] { "Field", "Method" });

            if (EditorGUI.EndChangeCheck())
            {
                selectedMethod2 = 0;
                selectedField2 = 0;
                if (fieldOrMethod2 == 0)
                {
                    fieldsName2 = ((MonoBehaviour)serializedObject.FindProperty("script2").objectReferenceValue).GetType().GetFields().Where(m => m.FieldType != typeof(void) && m.FieldType.IsValueType).Select(f => f.Name).ToList();

                    ((GetDebugInfo)target).methodName2 = null;
                    if (fieldsName2.Count > 0)
                        ((GetDebugInfo)target).fieldName2 = fieldsName2[selectedField2];

                }
                else
                {
                    methodsName2 = ((MonoBehaviour)serializedObject.FindProperty("script2").objectReferenceValue).GetType().
                        GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                            Where(m => m.ReturnType != typeof(void) &&  m.ReturnType.IsValueType).
                                Select(m => m.Name + "()").ToList();

                    ((GetDebugInfo)target).fieldName2 = null;
                    if (methodsName2.Count > 0)
                        ((GetDebugInfo)target).methodName2 = methodsName2[0];

                }
            }

            if (fieldOrMethod2 == 0)
            {
                selectedField2 = EditorGUILayout.Popup("Field", selectedField2, fieldsName2.ToArray());
                if (fieldsName2.Count > 0)
                    serializedObject.FindProperty("fieldName2").stringValue = fieldsName2[selectedField2].Substring(0, fieldsName2[selectedField2].Length);

            }
            else
            {
               
                EditorGUI.BeginChangeCheck();

                selectedMethod2 = EditorGUILayout.Popup("Method", selectedMethod2, methodsName2.ToArray());

                if (EditorGUI.EndChangeCheck())
                {
                    ((GetDebugInfo)target).fieldName2 = null;
                    if (methodsName2.Count > 0)
                        serializedObject.FindProperty("methodName2").stringValue = methodsName2[selectedMethod2].Substring(0, methodsName2[selectedMethod2].Length - 2);
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}

