using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MaxConstraintRefInstance<T> : Constraint where T : IComparable
{
    Variable<T> m_maxVariable;
    MaxConstraintRef<T> m_maxConstraintRef;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public MaxConstraintRefInstance(MaxConstraintRef<T> a_maxConstraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_maxConstraintRef = a_maxConstraintRef;
        m_constaintType = a_maxConstraintRef.ConstaintType;
        m_fieldData = a_fieldData;
        m_maxVariable = a_maxConstraintRef.MaxVariable.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_maxVariable.Valid)
            return true;

        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;
        int result = comparable.CompareTo(m_maxVariable.GetValue());
        return m_maxConstraintRef.AcceptEqual ? result > 0 : result >= 0;
    }

    public override string GetViolationText()
    {
        if (!m_maxVariable.Valid)
            return "Invalid reference value: " + m_maxConstraintRef.ReferenceName;

        if (m_maxConstraintRef.AcceptEqual)
            return string.Format("Value higher than: {0} ({1})", m_maxConstraintRef.ReferenceName, m_maxVariable.GetValue());
        return string.Format("Value higher than or equal to: {0} ({1})", m_maxConstraintRef.ReferenceName, m_maxVariable.GetValue());
    }

    void ReferenceChanged(object a_value, DrawerEventData a_eventData)
    {
        if (!m_initialised)
            return;
        m_fieldData.ConstraintUpdated(ViolatesConstraint(m_fieldData.GetDataObject()), m_index);
    }

    void ReferenceInvalidated()
    {
        if (!m_initialised)
            return;
        m_fieldData.ConstraintUpdated(true, m_index);
    }

    public override void RemoveAllReferences()
    {
        m_maxVariable.RemoveAllReferences();
        m_maxVariable = null;
        m_fieldData = null;
        m_maxConstraintRef = null;
    }
}
