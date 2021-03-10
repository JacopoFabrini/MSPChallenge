using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class DictionaryKeyValueSizeHandle : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    DictionaryFieldDrawer m_dictionaryDrawer;

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rect = transform.parent.GetComponent<RectTransform>();
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        if (eventData.position.x < worldCorners[0].x)
        {
            //clamp left (t = 0)
            m_dictionaryDrawer.SetKeyToValueSizeRatio(0);
            transform.position = new Vector3(worldCorners[0].x, transform.position.y, transform.position.z);
        }
        else if (eventData.position.x > worldCorners[3].x)
        {
            //clamp right (t = 1)
            m_dictionaryDrawer.SetKeyToValueSizeRatio(1f); 
            transform.position = new Vector3(worldCorners[3].x, transform.position.y, transform.position.z);
        }
        else
        {
            float t = (eventData.position.x - worldCorners[0].x) / (worldCorners[3].x - worldCorners[0].x);
            m_dictionaryDrawer.SetKeyToValueSizeRatio(t); 
            transform.position = new Vector3(eventData.position.x, transform.position.y, transform.position.z);
        }
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

