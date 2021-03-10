using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


class Blocker : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent m_onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        m_onClick?.Invoke();
    }
}

