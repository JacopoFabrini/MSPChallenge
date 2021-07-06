using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldScrollFixer : MonoBehaviour, IScrollHandler//, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    private ScrollRect m_scrollRect;
    //private TMP_InputField m_input;
    //private bool m_isDragging;
    //private bool m_preventScrollRectDrag;

    private void Start()
    {
        m_scrollRect = GetComponentInParent<ScrollRect>();
        //m_input = GetComponent<TMP_InputField>();
        //m_input.DeactivateInputField();
        //m_input.onDeselect.AddListener(_ => m_preventScrollRectDrag = false);
    }

    //public void OnBeginDrag(PointerEventData data)
    //{
    //    if (m_preventScrollRectDrag)
    //        return;

    //    m_scrollRect.OnBeginDrag(data);
    //    m_isDragging = true;

    //    m_input.DeactivateInputField();
    //}

    //public void OnDrag(PointerEventData data)
    //{
    //    m_scrollRect.OnDrag(data);
    //}

    //public void OnEndDrag(PointerEventData data)
    //{
    //    m_scrollRect.OnEndDrag(data);
    //    m_isDragging = false;
    //}

    public void OnScroll(PointerEventData data)
    {
        m_scrollRect.OnScroll(data);
    }

    //public void OnPointerUp(PointerEventData data)
    //{
    //    if (!m_isDragging && !data.dragging)
    //    {
    //        m_input.ActivateInputField();
    //        m_preventScrollRectDrag = true;
    //    }
    //}
}