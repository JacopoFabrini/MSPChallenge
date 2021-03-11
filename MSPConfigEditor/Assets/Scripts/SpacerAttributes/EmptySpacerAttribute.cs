using System;
using System.Collections.Generic;
using System.Linq;

public class EmptySpacerAttribute : AbstractSpacerAttribute
{
    public float m_verticalSize;

    public EmptySpacerAttribute(float a_verticalSize)
    {
        m_verticalSize = a_verticalSize;
    }

    public override Type SpacerType => typeof(EmptySpacer);
}
