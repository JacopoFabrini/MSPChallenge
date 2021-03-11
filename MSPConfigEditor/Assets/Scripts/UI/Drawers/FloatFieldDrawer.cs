using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

class FloatFieldDrawer : InputFieldDrawer<FloatFieldDrawerAttribute, FieldData>
{
    protected override void InputFieldCallback(string a_newValue)
    {
        if (String.IsNullOrEmpty(a_newValue))
        {
            m_inputField.text = "0";
            m_fieldData?.SetValue(0);
        }
        else
        {
            try
            {
                m_fieldData?.SetValue(float.Parse(a_newValue), false);
            }
            catch (Exception e)
            {
                Debug.Log("Exception occured when parsing float input: " + e);
                m_fieldData?.SetValue(0);
            }
        }
    }

    protected override string GetDisplayText(object a_value)
    {
#if UNITY_EDITOR
        try
        {
            return ((float) a_value).ToString(DrawerManager.CultureInfo);
        }
        catch (Exception e)
        {
            Debug.LogError("Exception occurred when casting value to drawer type in object: " + m_nameText.text + ". Error: " + e);
        }
#endif
        return ((float)a_value).ToString(DrawerManager.CultureInfo);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<FloatFieldDrawer>(this);
    }
}