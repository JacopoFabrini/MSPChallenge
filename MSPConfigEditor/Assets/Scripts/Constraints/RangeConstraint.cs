using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RangeConstraint<T> : Constraint, IConstraintDefinition where T : IComparable
{
    protected T m_min;
    protected T m_max;
    protected bool m_inverse;
    protected bool m_inclusiveMin;
    protected bool m_inclusiveMax;

    public RangeConstraint(T a_min, T a_max, EConstraintType a_type, bool a_inverse = false, bool a_inclusiveMin = true, bool a_inclusiveMax = true)
    {
        m_min = a_min;
        m_max = a_max;
        m_inclusiveMin = a_inverse;
        m_inclusiveMin = a_inclusiveMin;
        m_inclusiveMax = a_inclusiveMax;
        m_constaintType = a_type;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;

        if (m_inverse)
        {
            int minResult = comparable.CompareTo(m_min);
            int maxResult = comparable.CompareTo(m_max);
            return (m_inclusiveMin ? minResult >= 0 : minResult > 0)
                && (m_inclusiveMax ? maxResult <= 0 : maxResult < 0);
        }

        //Check min
        int result = comparable.CompareTo(m_min);
        if (m_inclusiveMin ? result < 0 : result <= 0)
            return true;

        //Check max
        result = comparable.CompareTo(m_max);
        return m_inclusiveMax ? result > 0 : result >= 0;
    }

    public override string GetViolationText()
    {
        if (m_inverse)
            return string.Format("Value inside of range: [{0} ~ {1}]", m_min, m_max);
        return string.Format("Value outside of range: [{0} ~ {1}]", m_min, m_max);
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return this;
    }

    public override void RemoveAllReferences()
    { }
}
