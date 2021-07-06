using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using ColourPalette;
using System;
using System.Reflection;
using TMPro;

public enum DrawerEventType { ValueChanged, Destroyed, Initialised, IndexChanged, Added }

public abstract class AbstractFieldDrawer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected TextMeshProUGUI m_nameText;
    [SerializeField] protected RectTransform m_header;
    [SerializeField] protected HorizontalLayoutGroup m_headerLayout;
    [SerializeField] protected Image m_rimImage;
    
    protected bool m_rimColourLocked;
    protected bool m_rimOnBeforeLock;
    protected Color m_originalRimColour;
    protected List<ADrawerSupportElement> m_supportElements;
    public abstract string InfoText { get; }
    protected bool m_initialised;

    protected virtual void Initialise()
    {
        if (m_initialised)
            return;
        m_initialised = true;
    }

    public void LockRimColour(Color a_color)
    {
        m_rimImage.color = a_color;
        m_rimImage.gameObject.SetActive(true);
    }

    public void UnlockRimColour()
    {
        if (m_rimOnBeforeLock)
        {
            m_rimImage.color = m_originalRimColour;
        }
        else
        {
            m_rimImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData a_eventData)
    {
        if (a_eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClicked();
        }
    }

    public void OnRightClicked()
    {
        DrawerManager.Instance.OpenContextMenu(this);
    }

    protected virtual void ShowInfoBox()
    {
        DrawerManager.Instance.DrawerInfoWindow.SetToDrawer(this);
    }

    public void SetConstraintRim(EConstraintType a_worstViolation)
    {
        if (a_worstViolation == EConstraintType.None)
        {
            m_rimOnBeforeLock = false;
            if (!m_rimColourLocked)
                m_rimImage.gameObject.SetActive(false);
        }
        else
        {
            m_rimOnBeforeLock = true;
            if (a_worstViolation == EConstraintType.FatalError)
                m_originalRimColour = DrawerManager.Instance.DrawerOptions.FatalErrorColour.GetColour();
            else if (a_worstViolation == EConstraintType.Error)
                m_originalRimColour = DrawerManager.Instance.DrawerOptions.ErrorColour.GetColour();
            else
                m_originalRimColour = DrawerManager.Instance.DrawerOptions.WarningColour.GetColour();
            if (!m_rimColourLocked)
            {
                m_rimImage.gameObject.SetActive(true);
                m_rimImage.color = m_originalRimColour;
            }
        }
    }

    public virtual Transform GetChildContainer()
    {
        throw new NotImplementedException();
    }

    public abstract void SetToFieldData(FieldData a_fieldData);
    public abstract void ReleaseFromFieldData();
    protected abstract void SetFieldDataToNull();
    protected abstract void InstantiateFieldData();
    public virtual void UpdateDisplayedValue(object a_value) { }
    protected abstract void SetDrawerAttribute(AbstractFieldDrawerAttribute a_attribute);
    protected abstract void ReleaseObject();
    public abstract FieldData FieldData { get; }
    public virtual void ChildIndicesUpdated() { }
    public abstract List<ContextMenuButton> GetContextMenuButtons();
}

