using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugInfoBase
{
    private string infoId;
    private string infoDescription;
    private string infoFormat;

    public DebugInfoBase(string id, string description, string format)
    {
        infoId = id;
        infoDescription = description;
        infoFormat = format;
    }

    public string GetCommandId() { return infoId; }
    public string GetCommandDescription() { return infoDescription; }
    public string GetCommandFormat() { return infoFormat; }
}
public class DebugInfo<T> : DebugInfoBase
{
    private Func<T> getter;

    public DebugInfo(string id, string description, string format, Func<T> getter) : base(id, description, format)
    {
        this.getter = getter;
    }

    public void GetInfo(out T value )
    {
       value = getter.Invoke();
    }
}


public class DebugInfo<T,N>: DebugInfoBase where T : N
{
    private Func<T> getter1;
    private Func<N> getter2;

    public DebugInfo(string id, string description, string format,Func<T> getter1,Func<N> getter2) : base(id, description, format)
    {
        this.getter1 = getter1;
        this.getter2 = getter2;
    }

    public void GetInfo(out T value1, out N value2)
    {
        value1 = getter1.Invoke();
        value2 = getter2.Invoke();
    }
}