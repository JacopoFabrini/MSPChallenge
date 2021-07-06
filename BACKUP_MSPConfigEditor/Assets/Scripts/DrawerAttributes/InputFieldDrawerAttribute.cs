using System;

public abstract class InputFieldDrawerAttribute : AbstractFieldDrawerAttribute
{
    public InputFieldDrawerAttribute(string a_name) : base(a_name)
    { }

    public override Type FieldDataType => typeof(FieldData);
}