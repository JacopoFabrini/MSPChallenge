using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using Object = System.Object;

class InlineFieldDrawer : ExpandableFieldDrawer<InlineFieldDrawerAttribute, InlineFieldData>
{
    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        SetExpanded(false);

        if (a_value == null)
        {
            m_nullIndicator.gameObject.SetActive(true);
            SetExpandable(false);
        }
        else
        {
            m_nullIndicator.gameObject.SetActive(false);
            SetExpandable(true);
        }
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<InlineFieldDrawer>(this);
    }
}