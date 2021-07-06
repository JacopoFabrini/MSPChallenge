using System;
using System.Collections.Generic;


public abstract class Variable<T>
{
    public delegate void VariableChangeCallback(T a_newValue);
    public abstract T GetValue();
    public abstract Variable<T> GetInstance(FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback);
    public abstract bool Valid { get; }
    public abstract void RemoveAllReferences();
}

