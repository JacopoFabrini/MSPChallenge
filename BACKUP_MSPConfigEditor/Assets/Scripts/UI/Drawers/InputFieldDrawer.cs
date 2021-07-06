using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public abstract class InputFieldDrawer<T, U> : FieldDrawer<T, U> 
    where T : InputFieldDrawerAttribute
    where U : FieldData
{
    [SerializeField]
    protected CustomInputField m_inputField;
    [SerializeField]
    private TMPro.TMP_InputField.ContentType m_contentType;

    protected bool m_ignoreFieldChange;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_inputField.onEndEdit.AddListener(s => { if (!m_ignoreFieldChange) InputFieldCallback(s); }) ;
        m_inputField.m_onRightClick.AddListener(OnRightClicked);
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        m_ignoreFieldChange = true;
        if (a_value == null)
        {
            m_inputField.contentType = TMPro.TMP_InputField.ContentType.Standard;
            m_inputField.interactable = false;
            m_inputField.text = "NULL";
        }
        else
        {
            m_inputField.contentType = m_contentType;
            m_inputField.interactable = true;
            m_inputField.text = GetDisplayText(a_value);
        }
        m_ignoreFieldChange = false;
    }

    protected virtual string GetDisplayText(object a_value)
    {
        return a_value.ToString();
    }

    protected abstract void InputFieldCallback(string a_newValue);
}