using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRim : MonoBehaviour
{
    [SerializeField]
    float m_highlightTime;

    float m_timeHighlighted;
    RectTransform m_transform;
    Animation m_animation;

    private void Awake()
    {
        m_transform = GetComponent<RectTransform>();
        m_animation = GetComponent<Animation>();
    }

    public void HighlightObject(Transform a_parent)
    {
        m_timeHighlighted = 0;
        transform.SetParent(a_parent, false);
        m_animation.Stop();
        m_animation.Play();
        m_transform.anchorMin = Vector2.zero;
        m_transform.anchorMax = Vector2.one;
        m_transform.offsetMin = Vector2.zero;
        m_transform.offsetMax = Vector2.zero;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        m_timeHighlighted += Time.deltaTime;
        if (m_timeHighlighted >= m_highlightTime)
        {
            gameObject.SetActive(false);
            transform.SetParent(DrawerManager.UnusedDrawerParent, false);
        }
    }
}
