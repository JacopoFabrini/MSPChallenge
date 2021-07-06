using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public abstract class FieldDrawer<T, U> : AbstractFieldDrawer 
    where T : AbstractFieldDrawerAttribute
    where U : FieldData
{
    protected T m_attribute;
    protected U m_fieldData;
    public override FieldData FieldData => m_fieldData;

    public override string InfoText { get => m_attribute.InfoText; }

    public override void SetToFieldData(FieldData a_fieldData)
    {
        if (a_fieldData.GetType() != typeof(U))
            Debug.LogError($"casting {typeof(U)} to the same type.");
        try
        {
            m_fieldData = (U)a_fieldData;
        }
        catch (Exception e)
        {
            Debug.LogError($"{a_fieldData.GetType()} cannot be cast to {typeof(U)}. Error: {e.Message}");
        }
        Initialise();
        if (m_fieldData.Parent != null)
        {
            m_supportElements = m_fieldData.Parent.GetDrawerSupportElements(transform);
            foreach (ADrawerSupportElement element in m_supportElements)
                element.SetToField(m_fieldData, this);
        }

        SetDrawerAttribute(m_fieldData.DrawerAttribute);
    }

    protected override void SetDrawerAttribute(AbstractFieldDrawerAttribute a_attribute)
    {
        m_attribute = (T)a_attribute;
        
        //Set name & name properties
        if (m_nameText != null)
        {
            if (m_attribute.Name != null)
            {
                m_nameText.gameObject.SetActive(true);
                m_nameText.text = m_attribute.Name;
                LayoutElement nameLayout = m_nameText.GetComponent<LayoutElement>();
                nameLayout.preferredWidth = m_attribute.NameWidth;
            }
            else
                m_nameText.gameObject.SetActive(false);
        }
    }

    public override List<ContextMenuButton> GetContextMenuButtons()
    {
        List<ContextMenuButton> buttons = new List<ContextMenuButton>(2) { new ContextMenuButton("Info", ContextMenuSprite.Info, ShowInfoBox) };

        if (m_attribute.Nullable && m_fieldData.IsNullable)
        {
            if (!m_fieldData.IsNull)
                buttons.Add(new ContextMenuButton("Set to null", ContextMenuSprite.Null, SetFieldDataToNull));
            else
                buttons.Add(new ContextMenuButton("Instantiate", ContextMenuSprite.Instantiate, InstantiateFieldData));
        }

        if (!m_fieldData.IsNull)
            buttons.Add(new ContextMenuButton("Copy", ContextMenuSprite.Copy, CopyToClipBoard));

        if(m_fieldData.CanPasteClipboard(GUIUtility.systemCopyBuffer))
            buttons.Add(new ContextMenuButton("Paste", ContextMenuSprite.Paste, PasteFromClipBoard));

        return buttons;
    }

    protected void PasteFromClipBoard()
    {
        m_fieldData.PasteClipboard(GUIUtility.systemCopyBuffer);
    }

    protected void CopyToClipBoard()
    {
        GUIUtility.systemCopyBuffer = m_fieldData.GetClipboardCopy();
    }

    public override void ReleaseFromFieldData()
    {
        m_fieldData = null;
        if (m_supportElements != null)
        {
            foreach (ADrawerSupportElement element in m_supportElements)
                element.ReleaseField();
            m_supportElements = null;
        }

        m_attribute = null;
        ReleaseObject();
    }

    protected override void SetFieldDataToNull()
    {
        m_fieldData.SetNull(true);
    }

    protected override void InstantiateFieldData()
    {
        m_fieldData.SetNull(false);
    }
}

