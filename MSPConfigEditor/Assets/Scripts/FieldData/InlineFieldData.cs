using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class InlineFieldData : ReferenceFieldData
{
    protected Dictionary<string, FieldData> m_children;
    
    public override void PostCreateInitialise()
    {
        if (m_postCreateInitialised)
            return;
        base.PostCreateInitialise();
        if (m_children != null)
            foreach (var kvp in m_children)
                kvp.Value.PostCreateInitialise();
    }

    public override IEnumerable<FieldData> GetAllChildren()
    {
        return m_children?.Values;
    }

    public override FieldData GetChild(string a_index)
    {
        if (m_children == null)
            return null;
        FieldData result = null;
        m_children.TryGetValue(a_index, out result);
        return result;
    }

    public override void CreateDrawer(Transform a_drawerParent = null)
    {
        base.CreateDrawer(a_drawerParent);
        if (m_children != null)
            foreach (var kvp in m_children)
                kvp.Value.CreateDrawer(m_drawer.GetChildContainer());
    }

    public override void ReleaseDrawer()
    {
        base.ReleaseDrawer();
        if (m_children != null)
            foreach (var kvp in m_children)
                kvp.Value.ReleaseDrawer();
    }

    protected override void CreateChildren()
    {
        if (GetDataObject() == null)
            m_children = new Dictionary<string, FieldData>();
        else
            m_children = DrawerManager.Instance.CreateChildFieldData(GetDataObject(), this, m_depth, m_postCreateInitialised);
        if (m_children != null && m_drawer != null)
            foreach (var kvp in m_children)
                kvp.Value.CreateDrawer(m_drawer.GetChildContainer());
    }

    protected override void ClearChildren()
    {
        if (m_children != null)
            foreach (var kvp in m_children)
                kvp.Value.DestroyFieldData();
        m_children = null;
    }

    public override void Select()
    {
        base.Select();
        SetExpanded(true);
    }

    public override void SetExpanded(bool a_expanded)
    {
        if(m_drawer != null)
            ((InlineFieldDrawer)m_drawer).SetExpanded(a_expanded);
    }
}

