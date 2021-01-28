using System;
using System.Reflection;
using UnityEngine;

public class HideIfFunctionInstance : AHideIfInstance
{
    private MethodInfo m_function;

    public HideIfFunctionInstance(FieldData a_fieldData, string a_path, string a_functionName) : base(a_fieldData, a_path)
    {
        m_function = a_fieldData.Parent.ObjectType.GetMethod(a_functionName);
        if(m_function == null)
            Debug.LogError($"No function with name [{a_functionName}] found in {a_fieldData.Parent.ObjectType}.");
    }

    protected override bool Evaluate(object a_target)
    {
        return (bool)m_function.Invoke(m_fieldData.Parent.GetDataObject(), new object[]{ a_target} );
    }
}

