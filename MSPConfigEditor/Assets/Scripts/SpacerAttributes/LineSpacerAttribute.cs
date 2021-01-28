using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class LineSpacerAttribute : AbstractSpacerAttribute
{
    //public enum LineType { Solid, Dashed, Dotted }

    public float m_lineThickness;
    public RectOffset m_margin = new RectOffset(0,0,0,0);
    //public LineType m_lineType;

    public RectOffset Margin
    {
        get { return m_margin; }
        set { m_margin = value; }
    }

    public LineSpacerAttribute(float a_lineThickness/*, LineType a_lineType = LineType.Solid*/)
    {
        m_lineThickness = a_lineThickness;
        //m_lineType = a_lineType;
    }

    public override Type SpacerType => typeof(LineSpacer);
}
