using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSpacerAttribute : AbstractSpacerAttribute
{
    public string m_text;
    public int m_fontSize;
    public TextAlignmentOptions m_alignment;
    public Vector4 m_margin = Vector4.zero;

    public Vector4 Margin
    {
        get { return m_margin; }
        set { m_margin = value; }
    }

    public TitleSpacerAttribute(string a_text, int a_fontSize, TextAlignmentOptions a_alignment = TextAlignmentOptions.Center)
    {
        m_text = a_text;
        m_fontSize = a_fontSize;
        m_alignment = a_alignment;
    }

    public override Type SpacerType => typeof(TitleSpacer);
}

