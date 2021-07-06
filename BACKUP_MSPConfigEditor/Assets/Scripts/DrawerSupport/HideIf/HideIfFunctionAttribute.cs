using System;

public class HideIfFunctionAttribute : AHideIfAttribute
{
    private string m_functionName;

    public HideIfFunctionAttribute(string a_path, string a_functionName) : base(a_path)
    {
        m_functionName = a_functionName;
    }

    public override AHideIfInstance GetInstance(FieldData a_fieldData)
    {
        return new HideIfFunctionInstance(a_fieldData, m_path, m_functionName);
    }

}