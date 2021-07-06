using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ContainedConstraintInstance<T> : Constraint 
    where T : IEquatable<T>
{
    Variable<IList<T>> m_listVariable;
    ContainedConstraint<T> m_constraintRef;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public ContainedConstraintInstance(ContainedConstraint<T> a_constraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_constraintRef = a_constraintRef;
        m_constaintType = a_constraintRef.ConstaintType;
        m_fieldData = a_fieldData;
        m_listVariable = a_constraintRef.ListVariable.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_listVariable.Valid || m_listVariable.GetValue() == null)
            return !m_constraintRef.Inverse;
        IEquatable<T> equatable = (IEquatable<T>)a_value;
        if (equatable == null)
        {
            if (m_constraintRef.Inverse)
            {
                foreach (T element in m_listVariable.GetValue())
                    if (element == null)
                        return true;
                return false;
            }
            else
            {
                foreach (T element in m_listVariable.GetValue())
                    if (element == null)
                        return false;
                return true;
            }
        }
        else if (m_constraintRef.Inverse)
        {
            foreach (T element in m_listVariable.GetValue())
                if (equatable.Equals(element))
                    return true;
            return false;
        }
        else
        {
            foreach (T element in m_listVariable.GetValue())
                if (equatable.Equals(element))
                    return false;
            return true;
        }
    }

    public override string GetViolationText()
    {
        if (!m_listVariable.Valid)
            return "Invalid reference value: " + m_constraintRef.ReferenceName;

        if (m_constraintRef.Inverse)
            return "Value contained in: " + m_constraintRef.ReferenceName;
        return "Value not contained in: " + m_constraintRef.ReferenceName;
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
        m_listVariable.RemoveAllReferences();
        m_listVariable = null;
        m_fieldData = null;
        m_constraintRef = null;
    }
}

