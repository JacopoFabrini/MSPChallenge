using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class DrawerColumnHandle : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    LayoutElement m_targetLayout;
    [SerializeField]
    bool m_useMax;
    [SerializeField]
    float m_max;

    float m_dragStartPos;
    float m_dragStartWidth;

    public void OnDrag(PointerEventData eventData)
    {
        //float deltaX = eventData.position.x - transform.position.x;
        //m_targetLayout.preferredWidth += deltaX;
        if(m_useMax)
            m_targetLayout.preferredWidth = Math.Min(m_max, eventData.position.x - m_dragStartPos + m_dragStartWidth);
        else
            m_targetLayout.preferredWidth = eventData.position.x - m_dragStartPos + m_dragStartWidth;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_dragStartPos = transform.position.x;
        m_dragStartWidth = m_targetLayout.preferredWidth;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DrawerManager.Instance.DrawerOptions.SetCursorState(CursorState.Scale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DrawerManager.Instance.DrawerOptions.SetCursorState(CursorState.Default);
    }

}

