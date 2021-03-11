using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

class StringFieldDrawer : InputFieldDrawer<StringFieldDrawerAttribute, FieldData>
{
    protected override void InputFieldCallback(string a_newValue)
    {
        m_fieldData?.SetValue(a_newValue);
    }

    protected override string GetDisplayText(object a_value)
    {
        return (string)a_value;
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<StringFieldDrawer>(this);
    }
}