using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptySpacer : AbstractSpacer
{
    [SerializeField]
    private LayoutElement m_layoutElement;

    public override void Initialise(AbstractSpacerAttribute a_attribute)
    {
        if (a_attribute is EmptySpacerAttribute emptySpacerAttribute)
        {
            m_layoutElement.minHeight = emptySpacerAttribute.m_verticalSize;
        }
        else
            throw new Exception("Non EmptySpacerAttribute given to EmptySpacer initialiser.");
    }

    public override void ReleaseFromFieldData()
    {
        DrawerManager.Instance.SpacerPool.ReleaseObject<EmptySpacer>(this);
    }
}