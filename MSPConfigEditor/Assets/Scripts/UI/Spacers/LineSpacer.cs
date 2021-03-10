using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSpacer : AbstractSpacer
{
    [SerializeField]
    private Image m_lineImage;
    [SerializeField]
    private VerticalLayoutGroup m_layoutgroup;
    [SerializeField]
    private LayoutElement m_imageLayoutElement;
    //[SerializeField]
    //private Material m_dashedMaterial;
    //[SerializeField]
    //private Material m_dottedMaterial;

    public override void Initialise(AbstractSpacerAttribute a_attribute)
    {
        if (a_attribute is LineSpacerAttribute lineSpacerAttribute)
        {
            m_imageLayoutElement.minHeight = lineSpacerAttribute.m_lineThickness;
            m_layoutgroup.padding = lineSpacerAttribute.Margin;
            //if(lineSpacerAttribute.m_lineType == LineSpacerAttribute.LineType.Dashed)
            //    m_lineImage.material = m_dashedMaterial;
            //else if(lineSpacerAttribute.m_lineType == LineSpacerAttribute.LineType.Dotted)
            //m_lineImage.material = m_dottedMaterial;
        }
        else
            throw new Exception("Non LineSpacerAttribute given to LineSpacer initialiser.");
    }

    public override void ReleaseFromFieldData()
    {
        DrawerManager.Instance.SpacerPool.ReleaseObject<LineSpacer>(this);
    }
}