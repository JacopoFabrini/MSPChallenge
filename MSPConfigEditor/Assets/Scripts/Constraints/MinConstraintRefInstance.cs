using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MinConstraintRefInstance<T> : Constraint where T : IComparable
{
    Variable<T> m_minVariable;
    MinConstraintRef<T> m_minConstraintRef;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public MinConstraintRefInstance(MinConstraintRef<T> a_minConstraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_minConstraintRef = a_minConstraintRef;
        m_constaintType = a_minConstraintRef.ConstaintType;
        m_fieldData = a_fieldData;
        m_minVariable = a_minConstraintRef.MinVariable.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_minVariable.Valid)
            return true;

        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;
        int result = comparable.CompareTo(m_minVariable.GetValue());
        return m_minConstraintRef.AcceptEqual ? result < 0 : result <= 0;
    }
    
    public override string GetViolationText()
    {
        if (!m_minVariable.Valid)
            return "Invalid reference value: " + m_minConstraintRef.ReferenceName;

        if (m_minConstraintRef.AcceptEqual)
            return string.Format("Value lower than: {0} ({1})", m_minConstraintRef.ReferenceName, m_minVariable.GetValue());
        return string.Format("Value lower than or equal to: {0} ({1})", m_minConstraintRef.ReferenceName, m_minVariable.GetValue());
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
        m_minVariable.RemoveAllReferences();
        m_minVariable = null;
        m_fieldData = null;
        m_minConstraintRef = null;
    }
}
