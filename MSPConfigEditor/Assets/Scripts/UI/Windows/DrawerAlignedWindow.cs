using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerAlignedWindow : MonoBehaviour
{
    [SerializeField]
    protected RectTransform m_menuTransform;
    [SerializeField]
    protected RectTransform m_arrowTransformLeft;
    [SerializeField]
    protected RectTransform m_arrowTransformRight;
    [SerializeField]
    protected RectTransform m_contentTransform;

    protected bool m_initialised;
    protected virtual void Initialise()
    {
        m_initialised = true;
    }

    public virtual void SetToDrawer(AbstractFieldDrawer a_drawer)
    {
        if (!m_initialised)
            Initialise();
        if (a_drawer == null)
            return;

        gameObject.SetActive(true);        
    }

    protected void PositionWindowImmediate(Vector3 a_point)
    {
        if (a_point.y >= (float)(Screen.height / 2))
        {
            if (a_point.x >= (float)(Screen.width / 2))
            {
                m_menuTransform.position = a_point + new Vector3(0, m_arrowTransformRight.sizeDelta.y);

                //Align top right menu to top left of drawer
                m_arrowTransformLeft.gameObject.SetActive(false);
                m_arrowTransformRight.gameObject.SetActive(true);
                m_arrowTransformRight.localPosition = new Vector3(-m_arrowTransformRight.sizeDelta.y, 0, m_arrowTransformRight.localPosition.z);
                SetPivotAndAnchor(new Vector2(1f, 1f));
                m_contentTransform.anchoredPosition = new Vector2(-m_arrowTransformRight.sizeDelta.y, 0);
            }
            else
            {
                m_menuTransform.position = a_point + new Vector3(0, m_arrowTransformRight.sizeDelta.y);

                //Align top left menu to top right of drawer
                m_arrowTransformLeft.gameObject.SetActive(true);
                m_arrowTransformRight.gameObject.SetActive(false);
                m_arrowTransformLeft.localPosition = new Vector3(m_arrowTransformLeft.localPosition.x, 0, m_arrowTransformLeft.localPosition.z);
                SetPivotAndAnchor(new Vector2(0, 1f));
                m_contentTransform.anchoredPosition = new Vector2(m_arrowTransformLeft.sizeDelta.y, 0);
            }
        }
        else
        {
            if (a_point.x >= (float)(Screen.width / 2))
            {
                m_menuTransform.position = a_point - new Vector3(0, m_arrowTransformRight.sizeDelta.y);
                
                //Align bottom right menu to bottom left of drawer
                m_arrowTransformLeft.gameObject.SetActive(false);
                m_arrowTransformRight.gameObject.SetActive(true);
                m_arrowTransformRight.localPosition = new Vector3(-m_arrowTransformRight.sizeDelta.y, m_arrowTransformRight.sizeDelta.x, m_arrowTransformRight.localPosition.z);
                SetPivotAndAnchor(new Vector2(1f, 0));
                m_contentTransform.anchoredPosition = new Vector2(-m_arrowTransformRight.sizeDelta.y, 0);
            }
            else
            {
                m_menuTransform.position = a_point - new Vector3(0, m_arrowTransformRight.sizeDelta.y);
                
                //Align bottom left menu to bottom right of drawer
                m_arrowTransformLeft.gameObject.SetActive(true);
                m_arrowTransformRight.gameObject.SetActive(false);
                m_arrowTransformLeft.localPosition = new Vector3(m_arrowTransformLeft.localPosition.x, m_arrowTransformLeft.sizeDelta.x, m_arrowTransformLeft.localPosition.z);
                SetPivotAndAnchor(new Vector2(0, 0));
                m_contentTransform.anchoredPosition = new Vector2(m_arrowTransformLeft.sizeDelta.y, 0);
            }
        }
    }

    protected void PositionWindowImmediate(AbstractFieldDrawer m_drawer)
    {
        //Position window
        Vector3[] cornerArray = new Vector3[4];
        m_drawer.GetComponent<RectTransform>().GetWorldCorners(cornerArray);
        if (cornerArray[2].y >= (float)(Screen.height / 2))
        {
            if (cornerArray[2].x >= (float)(Screen.width / 2))
            {
                //Align top right menu to top left of drawer
                m_arrowTransformLeft.gameObject.SetActive(false);
                m_arrowTransformRight.gameObject.SetActive(true);
                AlignToPoint(cornerArray[1]);
                m_arrowTransformRight.localPosition = new Vector3(-m_arrowTransformRight.sizeDelta.y, 0, m_arrowTransformRight.localPosition.z);
                SetPivotAndAnchor(new Vector2(1f, 1f));
                m_contentTransform.anchoredPosition = new Vector2(-m_arrowTransformRight.sizeDelta.y, 0);
            }
            else
            {
                //Align top left menu to top right of drawer
                m_arrowTransformLeft.gameObject.SetActive(true);
                m_arrowTransformRight.gameObject.SetActive(false);
                AlignToPoint(cornerArray[2]);
                m_arrowTransformLeft.localPosition = new Vector3(m_arrowTransformLeft.localPosition.x, 0, m_arrowTransformLeft.localPosition.z);
                SetPivotAndAnchor(new Vector2(0, 1f));
                m_contentTransform.anchoredPosition = new Vector2(m_arrowTransformLeft.sizeDelta.y, 0);
            }
        }
        else
        {
            if (cornerArray[2].x >= (float)(Screen.width / 2))
            {
                //Align bottom right menu to bottom left of drawer
                m_arrowTransformLeft.gameObject.SetActive(false);
                m_arrowTransformRight.gameObject.SetActive(true);
                AlignToPoint(cornerArray[0]);
                m_arrowTransformRight.localPosition = new Vector3(-m_arrowTransformRight.sizeDelta.y, m_arrowTransformRight.sizeDelta.x, m_arrowTransformRight.localPosition.z);
                SetPivotAndAnchor(new Vector2(1f, 0));
                m_contentTransform.anchoredPosition = new Vector2(-m_arrowTransformRight.sizeDelta.y, 0);
            }
            else
            {
                //Align bottom left menu to bottom right of drawer
                m_arrowTransformLeft.gameObject.SetActive(true);
                m_arrowTransformRight.gameObject.SetActive(false);
                AlignToPoint(cornerArray[3]);
                m_arrowTransformLeft.localPosition = new Vector3(m_arrowTransformLeft.localPosition.x, m_arrowTransformLeft.sizeDelta.x, m_arrowTransformLeft.localPosition.z);
                SetPivotAndAnchor(new Vector2(0, 0));
                m_contentTransform.anchoredPosition = new Vector2(m_arrowTransformLeft.sizeDelta.y, 0);
            }
        }
    }

    void SetPivotAndAnchor(Vector2 a_value)
    {
        m_contentTransform.pivot = a_value;
        m_contentTransform.anchorMin = a_value;
        m_contentTransform.anchorMax = a_value;      
    }

    public void AlignToPoint(Vector3 a_point)
    {
        m_menuTransform.position = a_point;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
