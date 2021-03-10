using System;

class EqualsConstraintRefInstance<T> : Constraint where T : IEquatable<T>
{
    Variable<T> m_equalsVariable;
    EqualsConstraintRef<T> m_constraintRef;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public EqualsConstraintRefInstance(EqualsConstraintRef<T> a_constraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_constraintRef = a_constraintRef;
        m_constaintType = a_constraintRef.ConstaintType;
        m_fieldData = a_fieldData;
        m_equalsVariable = a_constraintRef.EqualsVariable.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_equalsVariable.Valid)
            return true;

        IEquatable<T> equatable = (IEquatable<T>)a_value;
        if (equatable == null)
            return true;
        bool result = equatable.Equals(m_equalsVariable.GetValue());
        return m_constraintRef.Inverse ? !result : result;
    }

    public override string GetViolationText()
    {
        if (!m_equalsVariable.Valid)
            return "Invalid reference value: " + m_constraintRef.ReferenceName;

        if (m_constraintRef.Inverse)
            return string.Format("Value equal to: {0} ({1})", m_constraintRef.ReferenceName, m_equalsVariable.GetValue());
        return string.Format("Value not equal to: {0} ({1})", m_constraintRef.ReferenceName, m_equalsVariable.GetValue());
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
        m_equalsVariable.RemoveAllReferences();
        m_equalsVariable = null;
        m_fieldData = null;
        m_constraintRef = null;
    }
}