﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public class DoubleFieldDrawer : InputFieldDrawer<DoubleFieldDrawerAttribute, FieldData>
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
                m_fieldData?.SetValue(Double.Parse(a_newValue), false);
            }
            catch (Exception e)
            {
                Debug.Log("Exception occured when parsing int input: " + e);
                m_fieldData?.SetValue(0);
            }
        }
    }

    protected override string GetDisplayText(object a_value)
    {
        return ((Double)a_value).ToString(DrawerManager.CultureInfo);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<DoubleFieldDrawer>(this);
    }
}