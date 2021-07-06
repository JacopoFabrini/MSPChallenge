using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSpacer : AbstractSpacer
{
    [SerializeField]
    private TextMeshProUGUI m_titleText;

    public override void Initialise(AbstractSpacerAttribute a_attribute)
    {
        if (a_attribute is TitleSpacerAttribute titleAttribute)
        {
            m_titleText.text = titleAttribute.m_text;
            m_titleText.alignment = titleAttribute.m_alignment;
            m_titleText.margin = titleAttribute.m_margin;
            m_titleText.fontSize = titleAttribute.m_fontSize;
        }
        else
            throw new Exception("Non TitleSpacerAttribute given to TitleSpacer initialiser.");
    }

    public override void ReleaseFromFieldData()
    {
        DrawerManager.Instance.SpacerPool.ReleaseObject<TitleSpacer>(this);
    }
}

