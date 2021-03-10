using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RangeConstraintRefInstance<T> : Constraint where T : IComparable
{
    Variable<T> m_min;
    Variable<T> m_max;
    RangeConstraintRef<T> m_rangeConstraint;
    int m_index;
    FieldData m_fieldData;
    bool m_initialised;

    public RangeConstraintRefInstance(RangeConstraintRef<T> a_rangeConstraintRef, FieldData a_fieldData, int a_constraintIndex)
    {
        m_min = a_rangeConstraintRef.Min.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_max = a_rangeConstraintRef.Max.GetInstance(a_fieldData, ReferenceChanged, ReferenceInvalidated);
        m_constaintType = a_rangeConstraintRef.ConstaintType;
        m_rangeConstraint = a_rangeConstraintRef;
        m_fieldData = a_fieldData;
        m_index = a_constraintIndex;
        m_initialised = true;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (!m_min.Valid || !m_max.Valid)
            return true;

        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;

        if (m_rangeConstraint.Inverse)
        {
            int minResult = comparable.CompareTo(m_min.GetValue());
            int maxResult = comparable.CompareTo(m_max.GetValue());
            return (m_rangeConstraint.InclusiveMin ? minResult >= 0 : minResult > 0)
                && (m_rangeConstraint.InclusiveMax ? maxResult <= 0 : maxResult < 0);
        }

        //Check min
        int result = comparable.CompareTo(m_min.GetValue());
        if (m_rangeConstraint.InclusiveMin ? result < 0 : result <= 0)
            return true;

        //Check max
        result = comparable.CompareTo(m_max.GetValue());
        return m_rangeConstraint.InclusiveMax ? result > 0 : result >= 0;
    }

    public override string GetViolationText()
    {
        if (!m_min.Valid)
            return "Invalid reference value: " + m_rangeConstraint.MinRefName;
        if (!m_max.Valid)
            return "Invalid reference value: " + m_rangeConstraint.MaxRefName;

        if (m_rangeConstraint.Inverse)
            return string.Format("Value inside of range: [{0} ~ {1}] ([{2} ~ {3}])", m_rangeConstraint.MinRefName, m_rangeConstraint.MaxRefName, m_min.GetValue(), m_max.GetValue());
        return string.Format("Value outside of range: [{0} ~ {1}] ([{2} ~ {3}])", m_rangeConstraint.MinRefName, m_rangeConstraint.MaxRefName, m_min.GetValue(), m_max.GetValue());
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
        m_min.RemoveAllReferences();
        m_min = null;
        m_max.RemoveAllReferences();
        m_max = null;
        m_fieldData = null;
        m_rangeConstraint = null;
    }
}

