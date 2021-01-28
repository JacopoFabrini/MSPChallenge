using System;
using System.Collections;
using System.Collections.Generic;

class ContainedDictConstraintInstance<T> : Constraint
    where T : IEquatable<T>
{
    Variable<IDictionary> m_dictVariable;
    ContainedDictConstraint<T> m_constraintRef;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public ContainedDictConstraintInstance(string a_dictVariablePath, ContainedDictConstraint<T> a_constraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_constraintRef = a_constraintRef;
        m_constaintType = a_constraintRef.ConstaintType;
        m_fieldData = a_fieldData;
        m_dictVariable = new RefVarInstance<IDictionary>(a_dictVariablePath, a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_dictVariable.Valid || m_dictVariable.GetValue() == null)
            return !m_constraintRef.Inverse;
        IEquatable<T> equatable = (IEquatable<T>)a_value;
        if (equatable == null)
        {
            foreach (DictionaryEntry kvp in m_dictVariable.GetValue())
                if (m_constraintRef.MatchKeys ? kvp.Key == null : kvp.Value == null)
                    return m_constraintRef.Inverse;
            return !m_constraintRef.Inverse;
        }

        foreach (DictionaryEntry kvp in m_dictVariable.GetValue())
            if (m_constraintRef.MatchKeys ? equatable.Equals(kvp.Key) : equatable.Equals(kvp.Value))
                return m_constraintRef.Inverse;
        return !m_constraintRef.Inverse;
    }

    public override string GetViolationText()
    {
        if (!m_dictVariable.Valid)
            return "Invalid reference value: " + m_constraintRef.ReferenceName;
        if (m_constraintRef.MatchKeys)
        {
            if (m_constraintRef.Inverse)
                return "Value contained in keys of: " + m_constraintRef.ReferenceName;
            return "Value not contained in keys of: " + m_constraintRef.ReferenceName;
        }
        else
        {
            if (m_constraintRef.Inverse)
                return "Value contained in values of: " + m_constraintRef.ReferenceName;
            return "Value not contained in values of: " + m_constraintRef.ReferenceName;
        }
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
        m_dictVariable.RemoveAllReferences();
        m_dictVariable = null;
        m_fieldData = null;
        m_constraintRef = null;
    }
}

