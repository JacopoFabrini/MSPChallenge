using System;
using System.Collections.Generic;
using System.Reflection;


public class NewLineFieldData : ReferenceFieldData
{
    protected Dictionary<string, FieldData> m_children;
    bool m_expanded;
    bool m_ignoreExpandCallback;
    
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

    protected override void CreateChildren()
    {
        if (GetDataObject() == null)
            m_children = new Dictionary<string, FieldData>();
        else
            m_children = DrawerManager.Instance.CreateChildFieldData(GetDataObject(), this, m_depth + 1, m_postCreateInitialised);
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
        DrawerManager.Instance.SelectAtDepth(this, m_depth);
    }

    public override void SetExpanded(bool a_expanded)
    {
        if (a_expanded == m_expanded || m_ignoreExpandCallback)
            return;

        m_ignoreExpandCallback = true;
        if (a_expanded)
            DrawerManager.Instance.SelectAtDepth(this, m_depth);
        else
            DrawerManager.Instance.DeselectAtDepth(m_depth, false);
        m_ignoreExpandCallback = false;

        m_expanded = a_expanded;
        if (m_expanded)
        {
            if (m_children != null)
                foreach (var kvp in m_children)
                    kvp.Value.CreateDrawer();

            DrawerManager.Instance.ShowDrawerColumn(m_depth + 1);
            DrawerManager.Instance.SetDrawerColumnName(m_depth + 1, Name);
        }
        else
        {
            if (m_children != null)
                foreach (var kvp in m_children)
                    kvp.Value.ReleaseDrawer();

            //Recursively deselect any children
            DrawerManager.Instance.DeselectAtDepth(m_depth + 1, true);
        }
        if(m_drawer != null)
            ((NewLineFieldDrawer)m_drawer).SetExpanded(a_expanded);
    }
}