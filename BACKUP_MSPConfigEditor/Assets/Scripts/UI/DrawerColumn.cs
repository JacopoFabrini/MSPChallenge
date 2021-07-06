using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

class DrawerColumn : MonoBehaviour
{
    [SerializeField]
    Transform m_container;
    [SerializeField]
    ScrollRect m_scrollRect;
    [SerializeField]
    TextMeshProUGUI[] m_nameTexts;
    [SerializeField]
    LayoutElement m_columnLayout;
    [SerializeField]
    GameObject m_childViewport;
    [SerializeField]
    GameObject m_expandedHeader;
    [SerializeField]
    GameObject m_collapsedHeader;
    [SerializeField]
    float m_collapsedWidth;

    NewLineFieldData m_selectedFieldData;
    bool m_expanded;
    float m_oldWidth = 320f;

    void Start()
    {
        m_expanded = true;
    }

    public void Deselect(bool a_hideColumn)
    {
        if (a_hideColumn)
            gameObject.SetActive(false);

        if (m_selectedFieldData == null)
            return;

        m_selectedFieldData.SetExpanded(false);
        m_selectedFieldData = null;
    }

    public void Select(NewLineFieldData a_drawer)
    {
        if (m_selectedFieldData == a_drawer)
            return;
        Deselect(false);
        m_selectedFieldData = a_drawer;
        m_selectedFieldData.SetExpanded(true);
    }

    public void ScollToViewDrawer(AbstractFieldDrawer a_drawer)
    {
        StartCoroutine("ScrollToDrawerDelayed", a_drawer);
    }

    IEnumerator ScrollToDrawerDelayed(AbstractFieldDrawer a_drawer)
    {
        yield return new WaitForEndOfFrame();
        float contentSize = m_scrollRect.content.sizeDelta.y;
        float viewSize = m_scrollRect.viewport.rect.height;
        if (viewSize >= contentSize)
            m_scrollRect.verticalScrollbar.value = 1f;  
        else
        {
            float targetPos = a_drawer.GetComponent<RectTransform>().position.y;
            float offsetPos = m_scrollRect.content.position.y - targetPos;
            float halfViewSize = viewSize * 0.5f;
            if (offsetPos < halfViewSize)
                m_scrollRect.verticalScrollbar.value = 1f;
            else if (offsetPos > contentSize - halfViewSize)
                m_scrollRect.verticalScrollbar.value = 0;
            else
            {
                m_scrollRect.verticalScrollbar.value = 1f - (offsetPos - halfViewSize) / (contentSize - viewSize);
            }
        }
    }

    public void SetExpanded(bool a_value)
    {
        if (m_expanded == a_value)
            return;
        m_expanded = a_value;

        if (a_value)
        {
            m_columnLayout.preferredWidth = m_oldWidth;
            m_columnLayout.minWidth = 200f;
        }
        else
        {
            m_oldWidth = m_columnLayout.preferredWidth;
            m_columnLayout.preferredWidth = m_collapsedWidth;
            m_columnLayout.minWidth = 0;
        }
        m_expandedHeader.SetActive(m_expanded);
        m_childViewport.SetActive(m_expanded);
        m_collapsedHeader.SetActive(!m_expanded);
    }

    public Transform Container { get => m_container; }
    public string Name {
        set
        {
            foreach(TextMeshProUGUI text in m_nameTexts)
                text.text = value;
        }
        get { return m_nameTexts[0].text; }
    }
}

