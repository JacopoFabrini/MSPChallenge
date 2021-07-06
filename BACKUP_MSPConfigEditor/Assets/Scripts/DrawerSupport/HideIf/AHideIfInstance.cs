using System;

public abstract class AHideIfInstance
{
    private PathReference m_path;
    protected FieldData m_fieldData;
    private bool m_hidden = false;
    public bool Hidden => m_hidden;

    public AHideIfInstance(FieldData a_fieldData, string a_path)
    {
        m_fieldData = a_fieldData;
        m_path = new PathReference(a_path, a_fieldData, PathValueChanged, PathInvalidated, PathValueChanged);
    }

    void PathValueChanged(object a_target, DrawerEventData a_eventData)
    {
        if (Evaluate(a_target) != m_hidden)
        {
            m_hidden = !m_hidden;
            m_fieldData.SetHidden(m_hidden);
        }
    }

    protected void PathInvalidated()
    {
        if (m_hidden)
        {
            m_hidden = false;
            m_fieldData.SetHidden(false);
        }
    }

    protected abstract bool Evaluate(object a_target);

    public void RemoveAllReferences()
    {
        m_path.RemoveAllReferences();
    }
}

