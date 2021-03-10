using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ListElementIndexer : ADrawerSupportElement, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    CustomButton m_removeButton;
    [SerializeField]
    TextMeshProUGUI m_indexLabel;
    [SerializeField]
    GameObject[] m_reorderableIndicators;

    GameObject m_movePreviewObject;
    bool m_reorderable;
    bool m_usePreview = false;
    AbstractFieldDrawer m_fieldDrawer;
    FieldData m_fieldData;

    public CustomButton RemoveButton { get => m_removeButton; }

    public override void SetToField(FieldData a_fieldData, AbstractFieldDrawer a_drawer)
    {
        m_fieldDrawer = a_drawer;
        m_fieldData = a_fieldData;
        m_fieldData.m_indexChangeCallback.AddListener(SetIndexText);
        SetIndexText(m_fieldData.Index);
        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(0,0);
        rect.offsetMin = new Vector2(-22f,0);
    }

    public override void ReleaseField()
    {
        m_fieldData.m_indexChangeCallback.RemoveListener(SetIndexText);
        m_fieldDrawer = null;
        m_fieldData = null;
        DrawerManager.Instance.SupportElementPool.ReleaseObject<ListElementIndexer>(this);
    }

    public override void Initialise(AbstractFieldDrawerAttribute a_attribute)
    {
        IListElementIndexerAttribute listAttribute = (IListElementIndexerAttribute) a_attribute;

        m_removeButton.gameObject.SetActive(listAttribute.Removable);
        m_reorderable = listAttribute.Reorderable;
        foreach (GameObject go in m_reorderableIndicators)
            go.SetActive(listAttribute.Reorderable);
    }

    void SetIndexText(int a_index)
    {
        m_indexLabel.text = a_index.ToString();
    }

    void Awake()
    {
        m_removeButton.onClick.AddListener(RemoveButtonPressed);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_reorderable)
            return;
        if(m_usePreview)
            m_movePreviewObject = Instantiate(DrawerManager.Instance.DrawerOptions.ListMovePreviewPrefab, m_fieldDrawer.transform.parent);
        m_fieldDrawer.LockRimColour(Color.white);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!m_reorderable)
            return;
        if (m_usePreview)
        {
            if(m_movePreviewObject != null)
                m_movePreviewObject.transform.SetSiblingIndex(MousePositionToIndex(eventData.position));
        }
        else
        {
            int newIndex = MousePositionToIndex(eventData.position, true);
            if (m_fieldData.Index != newIndex)
                m_fieldData.Parent.ChangeChildIndex(m_fieldData.Index, newIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_reorderable)
            return;
        if (m_usePreview)
        {
            int newIndex = MousePositionToIndex(eventData.position, true);
            if (m_fieldData.Index != newIndex)
                m_fieldData.Parent.ChangeChildIndex(m_fieldData.Index, newIndex);
            Destroy(m_movePreviewObject.gameObject);
            m_movePreviewObject = null;
        }
        m_fieldDrawer.UnlockRimColour();
    }

    int MousePositionToIndex(Vector2 a_mousePosition, bool a_compensateOwnIndex = false)
    {
        if (m_usePreview)
        {
            float localMouseY = a_mousePosition.y - m_fieldDrawer.transform.parent.position.y;
            for (int i = 0; i < m_fieldDrawer.transform.parent.childCount - 1; i++)
            {
                if (localMouseY >= m_fieldDrawer.transform.parent.GetChild(i).localPosition.y)
                {
                    if (i > m_movePreviewObject.transform.GetSiblingIndex())
                        i--;
                    if (a_compensateOwnIndex && i > m_fieldData.Index)
                        i--;
                    return i;
                }
            }
            if (a_compensateOwnIndex)
                return m_fieldDrawer.transform.parent.childCount - 3;
            return m_fieldDrawer.transform.parent.childCount - 2;
        }
        else
        {
            float localMouseY = a_mousePosition.y - m_fieldDrawer.transform.parent.position.y;
            for (int i = 0; i < m_fieldDrawer.transform.parent.childCount - 1; i++)
            {
                if (localMouseY >= m_fieldDrawer.transform.parent.GetChild(i).localPosition.y)
                {
                    return i;
                }
            }
            if (a_compensateOwnIndex)
                return m_fieldDrawer.transform.parent.childCount - 2;
            return m_fieldDrawer.transform.parent.childCount - 1;
        }
    }

    void RemoveButtonPressed()
    {
        m_fieldData.Parent.DestroyChild(m_fieldData.Index);
    }
}

