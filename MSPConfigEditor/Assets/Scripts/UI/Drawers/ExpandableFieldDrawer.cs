using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using TMPro;

public abstract class ExpandableFieldDrawer<T, U> : ReferenceFieldDrawer<T, U> 
    where T : ReferenceFieldDrawerAttribute
    where U : FieldData
{
    [SerializeField]
    protected RectTransform m_childParent;
    [SerializeField]
    protected CustomButton m_expandButton;
    [SerializeField]
    protected RectTransform m_expandImage;
    [SerializeField]
    protected TextMeshProUGUI m_nullIndicator;
    [SerializeField]
    GameObject m_bottomBar;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_expandButton.onClick.AddListener(ToggleExpand);
        m_expandButton.m_onRightClick.AddListener(OnRightClicked);
    }

    void ToggleExpand()
    {
        SetExpanded(!m_childParent.gameObject.activeSelf);
    }

    public virtual void SetExpanded(bool a_expanded)
    {
        m_childParent.gameObject.SetActive(a_expanded);
        m_expandImage.transform.rotation = Quaternion.Euler(0, 0, a_expanded ? 180f : 0);
        m_bottomBar.SetActive(a_expanded);
    }

    protected void SetExpandable(bool a_expandable)
    {
        m_expandButton.interactable = a_expandable;
        m_expandImage.gameObject.SetActive(a_expandable);
    }

    public override Transform GetChildContainer()
    {
        return m_childParent;
    }
}

